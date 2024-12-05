using Syncfusion.Maui.Toolkit.Calendar;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Syncfusion.Maui.Toolkit;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class CalendarAllViewsPublicAPIUnitTests : BaseUnitTest
	{

        #region YearView Public Properties

        [Fact]
        public void YearView_Constructor_InitializesDefaultsCorrectly()
        {
            SfCalendar calendar = new SfCalendar();

            // YearView Properties
            Assert.Equal(Brush.Transparent, calendar.YearView.Background);
            Assert.Null(calendar.YearView.CellTemplate);
            Assert.Equal(Brush.Transparent, calendar.YearView.DisabledDatesBackground);
            Assert.NotNull(calendar.YearView.DisabledDatesTextStyle);
            Assert.Equal(Brush.Transparent, calendar.YearView.LeadingDatesBackground);
            Assert.NotNull(calendar.YearView.LeadingDatesTextStyle);
            Assert.Equal(Brush.Transparent, calendar.YearView.TodayBackground);
            Assert.NotNull(calendar.YearView.TodayTextStyle);
            Assert.NotNull(calendar.YearView.RangeTextStyle);
            Assert.NotNull(calendar.YearView.TextStyle);
            Assert.NotNull(calendar.YearView.SelectionTextStyle);
            Assert.Equal("MMM", calendar.YearView.MonthFormat);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_DisabledDatesBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.DisabledDatesBackground = expectedValue;
            Brush actualValue = calendar.YearView.DisabledDatesBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_LeadingDatesBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.LeadingDatesBackground = expectedValue;
            Brush actualValue = calendar.YearView.LeadingDatesBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.Background = expectedValue;
            Brush actualValue = calendar.YearView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_TodayBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.TodayBackground = expectedValue;
            Brush actualValue = calendar.YearView.TodayBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void YearView_CellTemplate_GetAndSet_DataTemplate()
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
            calendar.YearView.CellTemplate = expectedValues;
            DataTemplate actualValue = calendar.YearView.CellTemplate;

            Assert.Equal(expectedValues, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void YearView_DisabledDatesTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.DisabledDatesTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.DisabledDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void YearView_TextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void YearView_LeadingDatesTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.LeadingDatesTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.LeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void YearView_RangeTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.RangeTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void YearView_SelectionTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.SelectionTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void YearView_TodayTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TodayTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_DisabledDatesTextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.DisabledDatesTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.DisabledDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_TextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.TextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_LeadingDatesTextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.LeadingDatesTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.LeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_RangeTextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.RangeTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_SelectionTextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.SelectionTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void YearView_TodayTextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.YearView.TodayTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        public void YearView_DisabledDatesTextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.DisabledDatesTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.DisabledDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        public void YearView_TextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        public void YearView_LeadingDatesTextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.LeadingDatesTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.LeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        public void YearView_RangeTextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.RangeTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        public void YearView_SelectionTextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.SelectionTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        public void YearView_TodayTextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TodayTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void YearView_DisabledDatesTextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.DisabledDatesTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.DisabledDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void YearView_TextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void YearView_LeadingDatesTextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.LeadingDatesTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.LeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void YearView_RangeTextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.RangeTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void YearView_SelectionTextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.SelectionTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void YearView_TodayTextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TodayTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void YearView_DisabledDatesTextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.DisabledDatesTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.DisabledDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void YearView_TextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void YearView_LeadingDatesTextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.LeadingDatesTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.LeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void YearView_RangeTextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.RangeTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void YearView_SelectionTextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.SelectionTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void YearView_TodayTextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.TodayTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.YearView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData("M")]
        [InlineData("MM")]
        [InlineData("MMM")]
        [InlineData("MMMM")]
        public void YearView_TextFormat_GetAndSet_Double(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.YearView.MonthFormat = expectedValue;
            string actualValue = calendar.YearView.MonthFormat;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region HeaderView Public Properies

        [Fact]
        public void HeaderView_Constructor_InitializesDefaultsCorrectly()
        {
            SfCalendar calendar = new SfCalendar();

            // HeaderView Properties
            Assert.Equal(Brush.Transparent, calendar.HeaderView.Background);
            Assert.Empty(calendar.HeaderView.TextFormat);
            Assert.NotNull(calendar.HeaderView.TextStyle);
            Assert.True(calendar.HeaderView.ShowNavigationArrows);
            Assert.Equal(35, calendar.HeaderView.Height);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderView_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.HeaderView.Background = expectedValue;
            Brush actualValue = calendar.HeaderView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("d")]
        [InlineData("dd")]
        [InlineData("ddd")]
        [InlineData("dddd")]
        [InlineData("ddddd")]
        [InlineData("dddddd")]
        public void HeaderView_TextFormat_GetAndSet_String(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.TextFormat = expectedValue;
            string actualValue = calendar.HeaderView.TextFormat;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void HeaderView_TextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.TextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderView_TextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.HeaderView.TextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void HeaderView_TextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.TextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void HeaderView_TextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.TextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void HeaderView_TextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.TextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Fact]
        public void HeaderView_ShowNavigationArrows_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.ShowNavigationArrows = true;
            bool actualValue = calendar.HeaderView.ShowNavigationArrows;

            Assert.True(actualValue);
        }

        [Fact]
        public void HeaderView_ShowNavigationArrows_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.ShowNavigationArrows = false;
            bool actualValue = calendar.HeaderView.ShowNavigationArrows;

            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void HeaderView_Height_GetAndSet_Double(double expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeaderView.Height = expectedValue;
            double actualValue = calendar.HeaderView.Height;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region FooterView Public Properties

        [Fact]
        public void FooterView_Constructor_InitializesDefaultsCorrectly()
        {
            SfCalendar calendar = new SfCalendar();

            // FooterView Properties
            Assert.Equal(Brush.Transparent, calendar.FooterView.Background);
            Assert.Equal(Color.FromArgb("#CAC4D0"), calendar.FooterView.DividerColor);
            Assert.NotNull(calendar.FooterView.TextStyle);
            Assert.False(calendar.FooterView.ShowActionButtons);
            Assert.False(calendar.FooterView.ShowTodayButton);
            Assert.Equal(50, calendar.FooterView.Height);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void FooterView_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.FooterView.Background = expectedValue;
            Brush actualValue = calendar.FooterView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void FooterView_Background_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.FooterView.DividerColor = expectedValue;
            Brush actualValue = calendar.FooterView.DividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void FooterView_TextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.TextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void FooterView_TextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.FooterView.TextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void FooterView_TextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.TextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void FooterView_TextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.TextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FooterView_TextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.TextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Fact]
        public void FooterView_ShowActionButtons_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.ShowActionButtons = true;
            bool actualValue = calendar.FooterView.ShowActionButtons;

            Assert.True(actualValue);
        }

        [Fact]
        public void FooterView_ShowActionButtons_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.ShowActionButtons = false;
            bool actualValue = calendar.FooterView.ShowActionButtons;

            Assert.False(actualValue);
        }

        [Fact]
        public void FooterView_ShowTodayButton_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.ShowTodayButton = true;
            bool actualValue = calendar.FooterView.ShowTodayButton;

            Assert.True(actualValue);
        }

        [Fact]
        public void FooterView_ShowTodayButton_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.ShowTodayButton = false;
            bool actualValue = calendar.FooterView.ShowTodayButton;

            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void FooterView_Height_GetAndSet_Double(double expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FooterView.Height = expectedValue; 
            double actualValue = calendar.FooterView.Height;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region MonthView Public Properties

        [Fact]
        public void MonthView_Constructor_InitializesDefaultsCorrectly()
        {
            SfCalendar calendar = new SfCalendar();

            // MonthView Properties
            Assert.Equal(Brush.Transparent, calendar.MonthView.Background);
            Assert.Null(calendar.MonthView.CellTemplate);
            Assert.Equal(Brush.Transparent, calendar.MonthView.DisabledDatesBackground);
            Assert.NotNull(calendar.MonthView.DisabledDatesTextStyle);
            Assert.Equal(DayOfWeek.Sunday, calendar.MonthView.FirstDayOfWeek);
            Assert.NotNull(calendar.MonthView.HeaderView);
            Assert.Equal(6, calendar.MonthView.NumberOfVisibleWeeks);
            Assert.NotNull(calendar.MonthView.TextStyle);
            Assert.NotNull(calendar.MonthView.TodayTextStyle);
            Assert.NotNull(calendar.MonthView.WeekNumberStyle);
            Assert.Null(calendar.MonthView.WeekendDatesTextStyle);
            Assert.NotNull(calendar.MonthView.RangeTextStyle);
            Assert.NotNull(calendar.MonthView.SelectionTextStyle);
            Assert.NotNull(calendar.MonthView.SpecialDatesTextStyle);
            Assert.NotNull(calendar.MonthView.TrailingLeadingDatesTextStyle);
            Assert.False(calendar.MonthView.ShowWeekNumber);
            Assert.Null(calendar.MonthView.SpecialDayPredicate);
            Assert.Equal(Brush.Transparent, calendar.MonthView.SpecialDatesBackground);
            Assert.Equal(Brush.Transparent, calendar.MonthView.TodayBackground);
            Assert.Equal(Brush.Transparent, calendar.MonthView.TrailingLeadingDatesBackground);
            Assert.Null(calendar.MonthView.WeekendDatesBackground);
            Assert.Empty(calendar.MonthView.WeekendDays);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.Background = expectedValue;
            Brush actualValue = calendar.MonthView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_DisabledDatesBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.DisabledDatesBackground = expectedValue;
            Brush actualValue = calendar.MonthView.DisabledDatesBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MonthView_CellTemplate_GetAndSet_DataTemplate()
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
            calendar.MonthView.CellTemplate = expectedValues;
            DataTemplate actualValue = calendar.MonthView.CellTemplate;

            Assert.Equal(expectedValues, actualValue);
        }

        [Theory]
        [InlineData(DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Friday)]
        [InlineData(DayOfWeek.Saturday)]
        public void MonthView_FirstDayOfWeek_GetAndSet_DayOfWeek(DayOfWeek expectedValue)
        {
            SfCalendar calendar = new SfCalendar();
            
            calendar.MonthView.FirstDayOfWeek = expectedValue;
            DayOfWeek actualValue = calendar.MonthView.FirstDayOfWeek;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_DisabledDatesTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.DisabledDatesTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.DisabledDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_HeaderView_Background__GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.HeaderView.Background = expectedValue;
            Brush actualValue = calendar.MonthView.HeaderView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_HeaderView_TextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.HeaderView.TextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_HeaderView_Height_GetAndSet_Double(double expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.HeaderView.Height = expectedValue ;
            double actualValue = calendar.MonthView.HeaderView.Height;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("d")]
        [InlineData("dd")]
        [InlineData("ddd")]
        [InlineData("dddd")]
        [InlineData("ddddd")]
        [InlineData("dddddd")]
        public void MonthView_HeaderView_TextFormat_GetAndSet_String(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.HeaderView.TextFormat = expectedValue;
            string actualValue = calendar.MonthView.HeaderView.TextFormat;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MonthView_HeaderView_GetAndSet_Initial()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.HeaderView = new CalendarMonthHeaderView() { Parent = calendar };
            double actualValue = calendar.MonthView.HeaderView.Height;
            
            Assert.Equal(30,actualValue);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void MonthView_NumberOfVisibleWeeks_GetAndSet_Int(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.NumberOfVisibleWeeks = expectedValue;
            int actualValue = calendar.MonthView.NumberOfVisibleWeeks;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_TextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_TextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.TextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_TextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_TextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_TextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_TodayTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TodayTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_TodayTextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.TodayTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_TodayTextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TodayTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_TodayTextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TodayTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_TodayTextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TodayTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TodayTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_WeekNumberStyle_Background_GetAndSet_CalendarWeekNumberStyle(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.WeekNumberStyle = new CalendarWeekNumberStyle() { Background = expectedValue };
            CalendarWeekNumberStyle actualValue = calendar.MonthView.WeekNumberStyle;

            Assert.Equal(expectedValue, actualValue.Background);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_WeekNumberStyle_TextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekNumberStyle.TextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekNumberStyle.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }


        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_WeekNumberStyle_TextStyle_GetAndSet_CalendarTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.WeekNumberStyle.TextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekNumberStyle.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_WeekNumberStyle_TextStyle_GetAndSet_CalendarTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekNumberStyle.TextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekNumberStyle.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_WeekNumberStyle_TextStyle_GetAndSet_CalendarTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_WeekNumberStyle_TextStyle_GetAndSet_CalendarTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekNumberStyle.TextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekNumberStyle.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_WeekendDatesTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekendDatesTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekendDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_WeekendDatesTextStyle_GetAndSet_CalendarWeekendDatesTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.WeekendDatesTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekendDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_WeekendDatesTextStyle_GetAndSet_CalendarWeekendDatesTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekendDatesTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekendDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_WeekendDatesTextStyle_GetAndSet_CalendarWeekendDatesTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekendDatesTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekendDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_WeekendDatesTextStyle_GetAndSet_CalendarWeekendDatesTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekendDatesTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.WeekendDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_RangeTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.RangeTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_RangeTextStyle_GetAndSet_CalendarRangeTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.RangeTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_RangeTextStyle_GetAndSet_CalendarRangeTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.RangeTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_RangeTextStyle_GetAndSet_CalendarRangeTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.RangeTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_RangeTextStyle_GetAndSet_CalendarRangeTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.RangeTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.RangeTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_SelectionTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SelectionTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_SelectionTextStyle_GetAndSet_CalendarSelectionTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.SelectionTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_SelectionTextStyle_GetAndSet_CalendarSelectionTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SelectionTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_SelectionTextStyle_GetAndSet_CalendarSelectionTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SelectionTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_SelectionTextStyle_GetAndSet_CalendarSelectionTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SelectionTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_SpecialDatesTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SpecialDatesTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SpecialDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_SpecialDatesTextStyle_GetAndSet_CalendarSpecialDatesTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.SpecialDatesTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SpecialDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_SpecialDatesTextStyle_GetAndSet_CalendarSpecialDatesTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SpecialDatesTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SpecialDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_SpecialDatesTextStyle_GetAndSet_CalendarSpecialDatesTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SpecialDatesTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SpecialDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_SpecialDatesTextStyle_GetAndSet_CalendarSpecialDatesTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SpecialDatesTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.SpecialDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void MonthView_TrailingLeadingDatesTextStyle_GetAndSet_CalendarTextStyle(int expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TrailingLeadingDatesTextStyle = new CalendarTextStyle() { FontSize = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TrailingLeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_TrailingLeadingDatesTextStyle_GetAndSet_CalendarTrailingLeadingDatesTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.TrailingLeadingDatesTextStyle = new CalendarTextStyle() { TextColor = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TrailingLeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void MonthView_TrailingLeadingDatesTextStyle_GetAndSet_CalendarTrailingLeadingDatesTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TrailingLeadingDatesTextStyle = new CalendarTextStyle() { FontAttributes = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TrailingLeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void MonthView_TrailingLeadingDatesTextStyle_GetAndSet_CalendarTrailingLeadingDatesTextStyle_FontFamily(string expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TrailingLeadingDatesTextStyle = new CalendarTextStyle() { FontFamily = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TrailingLeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MonthView_TrailingLeadingDatesTextStyle_GetAndSet_CalendarTrailingLeadingDatesTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.TrailingLeadingDatesTextStyle = new CalendarTextStyle() { FontAutoScalingEnabled = expectedValue };
            CalendarTextStyle actualValue = calendar.MonthView.TrailingLeadingDatesTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_SpecialDatesBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.SpecialDatesBackground = expectedValue;
            Brush actualValue = calendar.MonthView.SpecialDatesBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_TodayBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.TodayBackground = expectedValue;
            Brush actualValue = calendar.MonthView.TodayBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_TrailingLeadingDatesBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.TrailingLeadingDatesBackground = expectedValue;
            Brush actualValue = calendar.MonthView.TrailingLeadingDatesBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void MonthView_WeekendDatesBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.MonthView.WeekendDatesBackground = expectedValue;
            Brush actualValue = calendar.MonthView.WeekendDatesBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MonthView_ShowWeekNumber_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.ShowWeekNumber = true;

            Assert.True(calendar.MonthView.ShowWeekNumber);
        }

        [Fact]
        public void MonthView_ShowWeekNumber_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.ShowWeekNumber = false;
            bool actualValue = calendar.MonthView.ShowWeekNumber;

            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(DayOfWeek.Sunday,DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Monday, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Tuesday, DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Wednesday, DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Thursday, DayOfWeek.Friday)]
        [InlineData(DayOfWeek.Friday, DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Saturday, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Monday, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Tuesday, DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Wednesday, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Thursday, DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Friday, DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Saturday, DayOfWeek.Friday)]
        [InlineData(DayOfWeek.Sunday, DayOfWeek.Saturday)]
        public void MonthView_WeekendDates_GetAndSet_DayOfWeek(DayOfWeek expectedValue1, DayOfWeek expectedValue2)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.WeekendDays.Add(expectedValue1);
            calendar.MonthView.WeekendDays.Add(expectedValue2);
            DayOfWeek actualValue1 = calendar.MonthView.WeekendDays[0];
            DayOfWeek actualValue2 = calendar.MonthView.WeekendDays[1];

            Assert.Contains<DayOfWeek>(expectedValue1, calendar.MonthView.WeekendDays);
            Assert.Contains<DayOfWeek>(expectedValue2, calendar.MonthView.WeekendDays);
            Assert.Equal(expectedValue1, actualValue1);
            Assert.Equal(expectedValue2, actualValue2);
            calendar.MonthView.WeekendDays.Clear();
        }

        [Theory]
        [InlineData(DayOfWeek.Sunday, DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Monday, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Tuesday, DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Wednesday, DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Thursday, DayOfWeek.Friday)]
        [InlineData(DayOfWeek.Friday, DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Saturday, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Monday, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Tuesday, DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Wednesday, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Thursday, DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Friday, DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Saturday, DayOfWeek.Friday)]
        [InlineData(DayOfWeek.Sunday, DayOfWeek.Saturday)]
        public void MonthView_WeekendDates_GetAndSet_List(DayOfWeek Value1, DayOfWeek Value2)
        {
            SfCalendar calendar = new SfCalendar();

            List<DayOfWeek> expectedValue = new List<DayOfWeek>();
            expectedValue.Add(Value1);
            expectedValue.Add(Value2);            
            List<DayOfWeek> actualValue = expectedValue;

            Assert.Equal(expectedValue, actualValue);
            calendar.MonthView.WeekendDays.Clear();
        }

        [Fact]
        public void SpecialDayPredicate_GetAndSet_Func()
        {
            SfCalendar calendar = new SfCalendar();

            Func<DateTime, CalendarIconDetails> expectedValue = (date) =>
            {
                    CalendarIconDetails iconDetails = new CalendarIconDetails();
                    iconDetails.Icon = CalendarIcon.Dot;
                    iconDetails.Fill = Colors.Red;
                    return iconDetails;
            };

            calendar.MonthView.SpecialDayPredicate = expectedValue;
            Func<DateTime, CalendarIconDetails> actualValue = calendar.MonthView.SpecialDayPredicate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(-7)]
        [InlineData(-12)]
        [InlineData(1)]
        [InlineData(15)]
        public void SpecialDayPredicate_Usage_WhenElseCondition(int dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SpecialDayPredicate = (date) =>
            {
                CalendarIconDetails iconDetails = new CalendarIconDetails();

                if (date.Date == DateTime.Now.AddDays(-2).Date || date.Date == DateTime.Now.AddDays(-7).Date || date.Date == DateTime.Now.AddDays(-12).Date || date.Date == DateTime.Now.AddDays(1).Date || date.Date == DateTime.Now.AddDays(15).Date)
                {
                    iconDetails.Icon = CalendarIcon.Dot;
                    iconDetails.Fill = Colors.Red;
                    return iconDetails;
                }

                iconDetails.Icon = CalendarIcon.Dot;
                iconDetails.Fill = Colors.Blue;
                return iconDetails;

            };

            CalendarIconDetails actualValue = calendar.MonthView.SpecialDayPredicate(DateTime.Now.AddDays(dateValue));

            Assert.Equal(Colors.Red, actualValue.Fill);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(40)]
        [InlineData(56)]
        [InlineData(-16)]
        [InlineData(-32)]
        public void SpecialDayPredicate_Usage_WhenIfCondition(int dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SpecialDayPredicate = (date) =>
            {
                CalendarIconDetails iconDetails = new CalendarIconDetails();

                if (date.Date == DateTime.Now.AddDays(-2).Date || date.Date == DateTime.Now.AddDays(-7).Date || date.Date == DateTime.Now.AddDays(-12).Date || date.Date == DateTime.Now.AddDays(1).Date || date.Date == DateTime.Now.AddDays(15).Date)
                {
                    iconDetails.Icon = CalendarIcon.Dot;
                    iconDetails.Fill = Colors.Red;
                    return iconDetails;
                }

                iconDetails.Icon = CalendarIcon.Dot;
                iconDetails.Fill = Colors.Blue;
                return iconDetails;

            };

            CalendarIconDetails actualValue = calendar.MonthView.SpecialDayPredicate(DateTime.Now.AddDays(dateValue));

            Assert.Equal(Colors.Blue, actualValue.Fill);
        }

        [Theory]
        [InlineData(CalendarIcon.Dot)]
        [InlineData(CalendarIcon.Square)]
        [InlineData(CalendarIcon.Triangle)]
        [InlineData(CalendarIcon.Heart)]
        [InlineData(CalendarIcon.Diamond)]
        [InlineData(CalendarIcon.Star)]
        [InlineData(CalendarIcon.Bell)]
        public void SpecialDayPredicate_GetAndSet_CalendarIcon(CalendarIcon expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.MonthView.SpecialDayPredicate = (date) =>
            {
                CalendarIconDetails iconDetails = new CalendarIconDetails();
                iconDetails.Icon = expectedValue;
                iconDetails.Fill = Colors.Red;
                return iconDetails;

            };

            CalendarIconDetails actualValue = calendar.MonthView.SpecialDayPredicate(DateTime.Now);

            Assert.Equal(expectedValue, actualValue.Icon);
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
            calendar.MonthView.SpecialDayPredicate = (date) =>
            {
                CalendarIconDetails iconDetails = new CalendarIconDetails();
                iconDetails.Icon = CalendarIcon.Dot;
                iconDetails.Fill = Colors.Red;
                iconDetails.Date = expectedValue;
                return iconDetails;

            };
            CalendarIconDetails actualValue = calendar.MonthView.SpecialDayPredicate(DateTime.Now);

            Assert.Equal(expectedValue, actualValue.Date);
        }

        #endregion
    }
}
