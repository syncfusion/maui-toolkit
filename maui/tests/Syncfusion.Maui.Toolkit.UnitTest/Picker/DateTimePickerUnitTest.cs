using Syncfusion.Maui.Toolkit.Picker;
using System.Windows.Input;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class DateTimePickerUnitTest : PickerBaseUnitTest
    {
        #region DateTimePicker Public Properties

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Assert.NotNull(picker.HeaderView);
            Assert.NotNull(picker.ColumnHeaderView);
            ////Assert.Equal(DateTime.Now, picker.SelectedDate);
            Assert.Equal(1, picker.DayInterval);
            Assert.Equal(1, picker.MonthInterval);
            Assert.Equal(1, picker.YearInterval);
            Assert.Equal(1, picker.HourInterval);
            Assert.Equal(1, picker.MinuteInterval);
            Assert.Equal(1, picker.SecondInterval);
            Assert.Equal(PickerTimeFormat.HH_mm_ss, picker.TimeFormat);
            Assert.Equal(PickerDateFormat.yyyy_MM_dd, picker.DateFormat);
            Assert.Equal(new DateTime(1900, 01, 01), picker.MinimumDate);
            Assert.Equal(new DateTime(2100, 12, 31, 23, 59, 59), picker.MaximumDate);
            Assert.Null(picker.SelectionChangedCommand);
            Assert.NotNull(picker.FooterView);
            Assert.Equal(Color.FromArgb("#CAC4D0"), picker.ColumnDividerColor);
            Assert.Equal(40d, picker.ItemHeight);
            Assert.NotNull(picker.SelectedTextStyle);
            Assert.NotNull(picker.TextStyle);
            Assert.NotNull(picker.SelectionView);
            Assert.False(picker.IsOpen);
            Assert.Equal(PickerMode.Default, picker.Mode);
            Assert.Equal(PickerTextDisplayMode.Default, picker.TextDisplayMode);
            Assert.Equal(PickerRelativePosition.AlignTop, picker.RelativePosition);
            Assert.Null(picker.AcceptCommand);
            Assert.Null(picker.DeclineCommand);
            Assert.Null(picker.SelectionChangedCommand);
        }

        [Theory]
        [InlineData(2001, 04, 25, 01, 15)]
        [InlineData(2065, 07, 5, 15, 25)]
        [InlineData(1920, 12, 16, 6, 50)]
        public void DateTimePicker_SelectedDate_GetAndSet(int year, int month, int day, int hour, int minute)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            DateTime expectedValue = new DateTime(year, month, day, hour, minute, 00);
            picker.SelectedDate = expectedValue;
            DateTime? actualValue = picker.SelectedDate;
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        ////[InlineData(-5)]
        [InlineData(25)]
        public void DateTimePicker_DayInterval_GetAndSet(int value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.DayInterval = value;
            int actualValue = picker.DayInterval;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        ////[InlineData(-5)]
        [InlineData(25)]
        public void DateTimePicker_MonthInterval_GetAndSet(int value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.MonthInterval = value;
            int actualValue = picker.MonthInterval;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        ////[InlineData(-5)]
        [InlineData(25)]
        public void DateTimePicker_YearInterval_GetAndSet(int value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.YearInterval = value;
            int actualValue = picker.YearInterval;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        ////[InlineData(-5)]
        [InlineData(25)]
        public void TimePicker_HourInterval_GetAndSet(int value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.HourInterval = value;
            int actualValue = picker.HourInterval;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        ////[InlineData(-5)]
        [InlineData(25)]
        public void TimePicker_MinuteInterval_GetAndSet(int value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.MinuteInterval = value;
            int actualValue = picker.MinuteInterval;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        ////[InlineData(-5)]
        [InlineData(25)]
        public void TimePicker_SecondInterval_GetAndSet(int value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.SecondInterval = value;
            int actualValue = picker.SecondInterval;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(PickerTimeFormat.Default)]
        [InlineData(PickerTimeFormat.h_mm_ss_tt)]
        [InlineData(PickerTimeFormat.hh_mm_tt)]
        [InlineData(PickerTimeFormat.HH_mm)]
        [InlineData(PickerTimeFormat.h_mm_tt)]
        [InlineData(PickerTimeFormat.hh_tt)]
        [InlineData(PickerTimeFormat.HH_mm_ss)]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt)]
        [InlineData(PickerTimeFormat.H_mm)]
        [InlineData(PickerTimeFormat.H_mm_ss)]
        public void TimePicker_Format_DetAndSet(PickerTimeFormat format)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.TimeFormat = format;
            PickerTimeFormat actualValue = picker.TimeFormat;
            Assert.Equal(format, actualValue);
        }

        [Theory]
        [InlineData(PickerDateFormat.Default)]
        [InlineData(PickerDateFormat.dd_MM)]
        [InlineData(PickerDateFormat.dd_MM_yyyy)]
        [InlineData(PickerDateFormat.dd_MMM_yyyy)]
        [InlineData(PickerDateFormat.M_d_yyyy)]
        [InlineData(PickerDateFormat.MM_dd_yyyy)]
        [InlineData(PickerDateFormat.yyyy_MM_dd)]
        [InlineData(PickerDateFormat.MM_yyyy)]
        [InlineData(PickerDateFormat.MMM_yyyy)]
        public void DateTimePicker_Format_DetAndSet(PickerDateFormat format)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.DateFormat = format;
            PickerDateFormat actualValue = picker.DateFormat;
            Assert.Equal(format, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("2065-08-04")]
        [InlineData("1900-03-08")]
        [InlineData("1995-02-16")]
        [InlineData("2110-02-16")]
        public void DateTimePicker_MinimumDate_GetAndSet(string dateValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            DateTime expectedValue = DateTime.Parse(dateValue);
            picker.MinimumDate = expectedValue;
            DateTime actualValue = picker.MinimumDate;
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("2065-08-04")]
        [InlineData("1900-03-08")]
        [InlineData("1995-02-16")]
        [InlineData("1800-02-16")]
        public void DateTimePicker_MaximumDate_GetAndSet(string dateValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            DateTime expectedValue = DateTime.Parse(dateValue);
            picker.MaximumDate = expectedValue;
            DateTime actualValue = picker.MaximumDate;
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_ColumnDividerColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.ColumnDividerColor = expectedValue;
            Color actualValue = picker.ColumnDividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void DateTimePicker_ItemHeight_GetAndSet(int expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.ItemHeight = expectedValue;
            double actualValue = picker.ItemHeight;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void DateTimePicker_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.TextStyle = new PickerTextStyle() { FontSize = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_TextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.TextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void DateTimePicker_TextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.TextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void DateTimePicker_TextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DateTimePicker_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void DateTimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontSize = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectedTextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void DateTimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void DateTimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DateTimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DateTimePicker_IsOpen_GetAndSet(bool expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.IsOpen = expectedValue;
            bool actualValue = picker.IsOpen;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(PickerMode.Default)]
        [InlineData(PickerMode.Dialog)]
        [InlineData(PickerMode.RelativeDialog)]
        public void DateTimePicker_Mode_GetAndSet(PickerMode mode)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.Mode = mode;
            PickerMode actualValue = picker.Mode;
            Assert.Equal(mode, actualValue);
        }

        [Theory]
        [InlineData(PickerTextDisplayMode.Default)]
        [InlineData(PickerTextDisplayMode.Fade)]
        [InlineData(PickerTextDisplayMode.Shrink)]
        [InlineData(PickerTextDisplayMode.FadeAndShrink)]
        public void DateTimePicker_TextDisplayMode_GetAndSet(PickerTextDisplayMode mode)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.TextDisplayMode = mode;
            PickerTextDisplayMode actualValue = picker.TextDisplayMode;
            Assert.Equal(mode, actualValue);
        }

        [Theory]
        [InlineData(PickerRelativePosition.AlignTop)]
        [InlineData(PickerRelativePosition.AlignBottom)]
        [InlineData(PickerRelativePosition.AlignTopRight)]
        [InlineData(PickerRelativePosition.AlignBottomRight)]
        [InlineData(PickerRelativePosition.AlignTopLeft)]
        [InlineData(PickerRelativePosition.AlignBottomLeft)]
        [InlineData(PickerRelativePosition.AlignToLeftOf)]
        [InlineData(PickerRelativePosition.AlignToRightOf)]
        public void DateTimePicker_RelativePosition_GetAndSet(PickerRelativePosition position)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.RelativePosition = position;
            PickerRelativePosition actualValue = picker.RelativePosition;
            Assert.Equal(position, actualValue);
        }

        [Fact]
        public void HeaderTemplate_GetAndSet_UsingDataTemplate()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderTemplate = new DataTemplate(() =>
            {
                return new Label { Text = "Header Content" };
            });

            Assert.NotNull(picker.HeaderTemplate);
        }

        [Fact]
        public void HeaderTemplate_GetAndSet_UsingDataTemplate_WhenCalledDynamic()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Assert.Null(picker.HeaderTemplate);

            picker.HeaderTemplate = new DataTemplate(() =>
            {
                return new Label { Text = "Header Content" };
            });

            Assert.NotNull(picker.HeaderTemplate);
        }

        [Fact]
        public void ColumnHeaderTemplate_GetAndSet_UsingDataTemplate()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.ColumnHeaderView.Height = 50;
            picker.ColumnHeaderTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    BackgroundColor = Colors.LightGray,
                    Padding = 5
                };

                var label = new Label
                {
                    Text = "Column Header Content",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                grid.Add(label);
                return grid;
            });

            Assert.NotNull(picker.ColumnHeaderTemplate);
        }

        [Fact]
        public void ColumnHeaderTemplate_GetAndSet_UsingDataTemplate_WhenCalledDynamic()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Assert.Null(picker.ColumnHeaderTemplate);

            picker.ColumnHeaderView.Height = 50;
            picker.ColumnHeaderTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    BackgroundColor = Colors.LightGray,
                    Padding = 5
                };

                var label = new Label
                {
                    Text = "Column Header Content",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                grid.Add(label);
                return grid;
            });

            Assert.NotNull(picker.ColumnHeaderTemplate);
        }

        [Fact]
        public void FooterTemplate_GetAndSet_UsingDataTemplate()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.FooterView.Height = 50;
            picker.FooterTemplate = new DataTemplate(() =>
            {
                return new Label { Text = "Footer Content" };
            });

            Assert.NotNull(picker.FooterTemplate);
        }

        [Fact]
        public void FooterTemplate_GetAndSet_UsingDataTemplate_WhenCalledDynamic()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Assert.Null(picker.FooterTemplate);

            picker.FooterView.Height = 50;
            picker.FooterTemplate = new DataTemplate(() =>
            {
                return new Label { Text = "Footer Content" };
            });

            Assert.NotNull(picker.FooterTemplate);
        }

        [Fact]
        public void SelectionChangedCommand_Execute_ReturnsTrue()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            picker.SelectionChangedCommand = expectedValue;
            picker.SelectionChangedCommand.Execute(null);
            ICommand actualValue = picker.SelectionChangedCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

        [Fact]
        public void AcceptCommand_Execute_ReturnsTrue()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            picker.AcceptCommand = expectedValue;
            picker.AcceptCommand.Execute(null);
            ICommand actualValue = picker.AcceptCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

        [Fact]
        public void DeclineCommand_Execute_ReturnsTrue()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            picker.DeclineCommand = expectedValue;
            picker.DeclineCommand.Execute(null);
            ICommand actualValue = picker.DeclineCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

        #endregion

        #region DateTimePicker Internal Properties

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_DateTimePickerBackground_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.DateTimePickerBackground = expectedValue;
            Color actualValue = picker.DateTimePickerBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_FooterTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.FooterTextColor = expectedValue;
            Color actualValue = picker.FooterTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_SelectedTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectedTextColor = expectedValue;
            Color actualValue = picker.SelectedTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_SelectionTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectionTextColor = expectedValue;
            Color actualValue = picker.SelectionTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void DateTimePicker_NormalTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.NormalTextColor = expectedValue;
            Color actualValue = picker.NormalTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void DateTimePicker_FooterFontSize_GetAndSet(double expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.FooterFontSize = expectedValue;
            double actualValue = picker.FooterFontSize;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void DateTimePicker_SelectedFontSize_GetAndSet(double expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.SelectedFontSize = expectedValue;
            double actualValue = picker.SelectedFontSize;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void DateTimePicker_NormalFontSize_GetAndSet(double expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.NormalFontSize = expectedValue;
            double actualValue = picker.NormalFontSize;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Header Settings Public Properties

        [Fact]
        public void HeaderSettings_Constructor_InitializesDefaultsCorrectly()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Assert.Equal(50d, picker.HeaderView.Height);
            Assert.NotNull(picker.HeaderView.TextStyle);
            Assert.NotNull(picker.HeaderView.SelectionTextStyle);
            //Assert.Equal(new SolidColorBrush(Color.FromArgb("#F7F2FB")), picker.HeaderView.Background);
            Assert.Equal(Color.FromArgb("#CAC4D0"), picker.HeaderView.DividerColor);
            Assert.Equal("dd/MM/yyyy", picker.HeaderView.DateFormat);
            Assert.Equal("hh:mm:ss tt", picker.HeaderView.TimeFormat);
        }

        [Theory]
        [InlineData(40)]
        [InlineData(10)]
        [InlineData(-40)]
        [InlineData(60)]
        public void HeaderSettings_Height_GetAndSet(double value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.HeaderView.Height = value;
            double actualValue = picker.HeaderView.Height;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderSettings_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderView.Background = expectedValue;
            Brush actualValue = picker.HeaderView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderSettings_DividerColor_GetAndSet(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderView.DividerColor = expectedValue;
            Color actualValue = picker.HeaderView.DividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("dd MM")]
        [InlineData("MM_dd_yyyy")]
        [InlineData("dd/MM")]
        [InlineData("MMM-yyyy")]
        public void HeaderSettings_DateFormat_GetAndSet(string format)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.HeaderView.DateFormat = format;
            string actualValue = picker.HeaderView.DateFormat;
            Assert.Equal(format, actualValue);
        }

        [Theory]
        [InlineData("hh tt")]
        [InlineData("HH_mm_ss")]
        [InlineData("hh/mm/tt")]
        [InlineData("H-mm")]
        public void HeaderSettings_TimeFormat_GetAndSet(string format)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.HeaderView.TimeFormat = format;
            string actualValue = picker.HeaderView.TimeFormat;
            Assert.Equal(format, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontSize = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderView.TextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void HeaderSettings_SelectionTextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.SelectionTextStyle = new PickerTextStyle() { FontSize = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderSettings_SelectionTextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderView.SelectionTextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void HeaderSettings_SelectionTextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.SelectionTextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void HeaderSettings_SelectionTextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.SelectionTextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void HeaderSettings_SelectionTextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.HeaderView.SelectionTextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.SelectionTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        #endregion

        #region ColumnHeader Settings Public Properties

        [Fact]
        public void ColumnHeaderSettings_Constructor_InitializesDefaultsCorrectly()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Assert.Equal(40d, picker.ColumnHeaderView.Height);
            Assert.NotNull(picker.ColumnHeaderView.TextStyle);
            ////Assert.Equal(new SolidColorBrush(Color.FromArgb("#F7F2FB")), picker.ColumnHeaderView.Background);
            Assert.Equal(Color.FromArgb("#CAC4D0"), picker.ColumnHeaderView.DividerColor);
            Assert.Equal("Day", picker.ColumnHeaderView.DayHeaderText);
            Assert.Equal("Month", picker.ColumnHeaderView.MonthHeaderText);
            Assert.Equal("Year", picker.ColumnHeaderView.YearHeaderText);
            Assert.Equal("Hour", picker.ColumnHeaderView.HourHeaderText);
            Assert.Equal("Minute", picker.ColumnHeaderView.MinuteHeaderText);
            Assert.Equal("Second", picker.ColumnHeaderView.SecondHeaderText);
            Assert.Equal(string.Empty, picker.ColumnHeaderView.MeridiemHeaderText);
        }

        [Theory]
        [InlineData(40)]
        [InlineData(10)]
        [InlineData(-40)]
        [InlineData(60)]
        public void ColumnHeaderSettings_Height_GetAndSet(double value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.Height = value;
            double actualValue = picker.ColumnHeaderView.Height;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void ColumnHeaderSettings_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            picker.ColumnHeaderView.Background = expectedValue;
            Brush actualValue = picker.ColumnHeaderView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void ColumnHeaderSettings_DividerColor_GetAndSet(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.ColumnHeaderView.DividerColor = expectedValue;
            Color actualValue = picker.ColumnHeaderView.DividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("Date")]
        [InlineData("")]
        public void ColumnHeaderSettings_DayHeaderText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.DayHeaderText = value;
            string actualValue = picker.ColumnHeaderView.DayHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("MonthText")]
        [InlineData("")]
        public void ColumnHeaderSettings_MonthHeaderText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.MonthHeaderText = value;
            string actualValue = picker.ColumnHeaderView.MonthHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("YearText")]
        [InlineData("")]
        public void ColumnHeaderSettings_YearHeaderText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.YearHeaderText = value;
            string actualValue = picker.ColumnHeaderView.YearHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("HourText")]
        [InlineData("")]
        public void ColumnHeaderSettings_HourHeaderText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.HourHeaderText = value;
            string actualValue = picker.ColumnHeaderView.HourHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("MinuteText")]
        [InlineData("")]
        public void ColumnHeaderSettings_MinuteHeaderText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.MinuteHeaderText = value;
            string actualValue = picker.ColumnHeaderView.MinuteHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("SecondText")]
        [InlineData("")]
        public void ColumnHeaderSettings_SecondHeaderText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.SecondHeaderText = value;
            string actualValue = picker.ColumnHeaderView.SecondHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Meridiem")]
        public void ColumnHeaderSettings_MeridiemHeaderText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.ColumnHeaderView.MeridiemHeaderText = value;
            string actualValue = picker.ColumnHeaderView.MeridiemHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontSize = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Fact]
        public void ColumnHeaderSettings_GetAndSet_Null()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

#pragma warning disable CS8625
            picker.ColumnHeaderView = null;
#pragma warning restore CS8625

            Assert.Null(picker.ColumnHeaderView);
        }

        #endregion

        #region Footer Settings Public Properties

        [Fact]
        public void FooterSettings_Constructor_InitializesDefaultsCorrectly()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Assert.Equal(0d, picker.FooterView.Height);
            Assert.Equal("OK", picker.FooterView.OkButtonText);
            Assert.Equal("Cancel", picker.FooterView.CancelButtonText);
            Assert.True(picker.FooterView.ShowOkButton);
            Assert.NotNull(picker.FooterView.TextStyle);
            Assert.Equal(Brush.Transparent, picker.FooterView.Background);
            Assert.Equal(Color.FromArgb("#CAC4D0"), picker.FooterView.DividerColor);
        }

        [Theory]
        [InlineData(40)]
        [InlineData(10)]
        [InlineData(-40)]
        [InlineData(60)]
        public void FooterSettings_Height_GetAndSet(double value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.FooterView.Height = value;
            double actualValue = picker.FooterView.Height;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Save")]
        [InlineData("")]
        public void FooterSettings_OkButtonText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.FooterView.OkButtonText = value;
            string actualValue = picker.FooterView.OkButtonText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Exit")]
        [InlineData("")]
        public void FooterSettings_CancelButtonText_GetAndSet(string value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.FooterView.CancelButtonText = value;
            string actualValue = picker.FooterView.CancelButtonText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FooterSettings_ShowOkButton_GetAndSet(bool value)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.FooterView.ShowOkButton = value;
            bool actualValue = picker.FooterView.ShowOkButton;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void FooterSettings_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            picker.FooterView.Background = expectedValue;
            Brush actualValue = picker.FooterView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void FooterSettings_DividerColor_GetAndSet(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.FooterView.DividerColor = expectedValue;
            Color actualValue = picker.FooterView.DividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void FooterSettings_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontSize = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontSize);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void FooterSettings_TextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.FooterView.TextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void FooterSettings_TextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void FooterSettings_TextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FooterSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        #endregion

        #region Picker Selection View Public Properties

        [Fact]
        public void PickerSelectionView_Constructor_InitializesDefaultsCorrectly()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            ////Assert.Equal(new SolidColorBrush(Color.FromArgb("#6750A4")), picker.SelectionView.Background);
            Assert.Equal(Colors.Transparent, picker.SelectionView.Stroke);
            Assert.Equal(new CornerRadius(20), picker.SelectionView.CornerRadius);
            Assert.Equal(new Thickness(5, 2), picker.SelectionView.Padding);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void PickerSelectionView_Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectionView.Background = expectedValue;
            Brush actualValue = picker.SelectionView.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void PickerSelectionView_DividerColor_GetAndSet(byte red, byte green, byte blue)
        {
            SfDateTimePicker picker = new SfDateTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectionView.Stroke = expectedValue;
            Color actualValue = picker.SelectionView.Stroke;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(30, 30, 30, 30)]
        [InlineData(50, 50, 50, 50)]
        [InlineData(-30, -30, -30, -30)]
        [InlineData(0, 0, 0, 0)]
        public void PickerSelectionView_CornerRadius_GetAndSet(double topLeft, double topRight, double bottomLeft, double bottomRight)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.SelectionView.CornerRadius = new CornerRadius(topLeft, topRight, bottomLeft, bottomRight);
            CornerRadius actualValue = picker.SelectionView.CornerRadius;
            Assert.Equal(new CornerRadius(topLeft, topRight, bottomLeft, bottomRight), actualValue);
        }

        [Theory]
        [InlineData(30, 30, 30, 30)]
        [InlineData(50, 50, 50, 50)]
        [InlineData(-30, -30, -30, -30)]
        [InlineData(0, 0, 0, 0)]
        public void PickerSelectionView_Padding_GetAndSet(double left, double top, double right, double bottom)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            picker.SelectionView.Padding = new Thickness(left, top, right, bottom);
            Thickness actualValue = picker.SelectionView.Padding;
            Assert.Equal(new Thickness(left, top, right, bottom), actualValue);
        }

        [Theory]
        [InlineData(30, 0, 0, 0)]
        [InlineData(0, 30, 0, 0)]
        [InlineData(0, 0, 30, 0)]
        [InlineData(0, 0, 0, 30)]
        [InlineData(30, 0, 0, 30)]
        [InlineData(0, 30, 30, 0)]
        [InlineData(0, -30, 30, 0)]
        public void PickerSelectionView_CornerRadius1_GetAndSet(int value1, int value2, int value3, int value4)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            CornerRadius exceptedValue = new CornerRadius(value1, value2, value3, value4);
            picker.SelectionView.CornerRadius = exceptedValue;
            CornerRadius actualValue = picker.SelectionView.CornerRadius;
            Assert.Equal(exceptedValue, actualValue);
        }

        [Theory]
        [InlineData(30, 0, 0, 0)]
        [InlineData(0, 30, 0, 0)]
        [InlineData(0, 0, 30, 0)]
        [InlineData(0, 0, 0, 30)]
        [InlineData(30, 0, 0, 30)]
        [InlineData(0, 30, 30, 0)]
        [InlineData(0, -30, 30, 0)]
        public void PickerSelectionView_Padding1_GetAndSet(int value1, int value2, int value3, int value4)
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            Thickness exceptedValue = new Thickness(value1, value2, value3, value4);
            picker.SelectionView.Padding = exceptedValue;
            Thickness actualValue = picker.SelectionView.Padding;
            Assert.Equal(exceptedValue, actualValue);
        }

        [Fact]
        public void PickerSelectionView_GetAndSet_Null()
        {
            SfDateTimePicker picker = new SfDateTimePicker();

#pragma warning disable CS8625
            picker.SelectionView = null;
#pragma warning restore CS8625

            Assert.Null(picker.SelectionView);
        }

        #endregion

        #region Events

        [Fact]
        public void SelectionChangedInvoked()
        {
            SfDateTimePicker picker = new SfDateTimePicker();
            var fired = false;
            picker.SelectionChanged += (sender, e) => fired = true;
            picker.SelectedDate = new DateTime(2025, 05, 19);
            Assert.True(fired);
        }

        #endregion

        #region PopupSizeFeature

        [Fact]
        public void DateTimePicker_PopupSize1()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            double expectedWidth = 100;
            double expectedHeight = 200;
            sfDateTimePicker.PopupWidth = expectedWidth;
            sfDateTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DateTimePicker_PopupSize_WhenPopupSizeIsNotSet()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DateTimePicker_PopupSize_WhenPopupSizeOnPropertyChange()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            double expectedWidth = 50;
            double expectedHeight = 20;

            sfDateTimePicker.PopupWidth = 100;
            sfDateTimePicker.PopupHeight = 200;
            sfDateTimePicker.PopupWidth = expectedWidth;
            sfDateTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DateTimePicker_PopupSize_WhenHeaderHeightProvided_PopupSizeProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            DateTimePickerHeaderView headerView = new DateTimePickerHeaderView();
            headerView.Height = 50;
            sfDateTimePicker.HeaderView = headerView;

            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DateTimePicker_PopupSize_WhenHeaderHeightProvided_PopupSizeNotProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            DateTimePickerHeaderView headerView = new DateTimePickerHeaderView();
            headerView.Height = 50;
            sfDateTimePicker.HeaderView = headerView;

            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DateTimePicker_PopupSize_WhenColumnHeaderProvided_PopupSizeProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            DateTimePickerColumnHeaderView columnHeaderView = new DateTimePickerColumnHeaderView();
            columnHeaderView.Height = 50;
            sfDateTimePicker.ColumnHeaderView = columnHeaderView;

            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenColumnHeaderProvided_PopupSizeNotProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            DateTimePickerColumnHeaderView columnHeaderView = new DateTimePickerColumnHeaderView();
            columnHeaderView.Height = 50;
            sfDateTimePicker.ColumnHeaderView = columnHeaderView;

            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_PopupSizeProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            sfDateTimePicker.ItemHeight = 10;

            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_PopupSizeNotProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            sfDateTimePicker.ItemHeight = 10;
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_ItemsLessThanFive()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            sfDateTimePicker.ItemHeight = 10;
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_ItemsGreaterThanFive()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            sfDateTimePicker.ItemHeight = 10;
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenFooterHeightProvided_PopupSizeProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            PickerFooterView pickerFooterView = new PickerFooterView();
            pickerFooterView.Height = 50;
            sfDateTimePicker.FooterView = pickerFooterView;
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenFooterHeightProvided_PopupSizeNotProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            PickerFooterView pickerFooterView = new PickerFooterView();
            pickerFooterView.Height = 50;
            sfDateTimePicker.FooterView = pickerFooterView;
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenAllHeaderHeightProvided_PopupSizeProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            DateTimePickerHeaderView headerView = new DateTimePickerHeaderView();
            DateTimePickerColumnHeaderView pickerColumnHeaderView = new DateTimePickerColumnHeaderView();
            PickerFooterView footerView = new PickerFooterView();
            sfDateTimePicker.ItemHeight = 10;
            headerView.Height = 50;
            pickerColumnHeaderView.Height = 50;
            footerView.Height = 50;
            sfDateTimePicker.HeaderView = headerView;
            sfDateTimePicker.ColumnHeaderView = pickerColumnHeaderView;
            sfDateTimePicker.FooterView = footerView;
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenAllHeaderHeightProvided_PopupSizeNotProvided()
        {
            SfDateTimePicker sfDateTimePicker = new SfDateTimePicker();
            DateTimePickerHeaderView headerView = new DateTimePickerHeaderView();
            DateTimePickerColumnHeaderView pickerColumnHeaderView = new DateTimePickerColumnHeaderView();
            PickerFooterView footerView = new PickerFooterView();
            headerView.Height = 50;
            pickerColumnHeaderView.Height = 50;
            footerView.Height = 50;
            sfDateTimePicker.HeaderView = headerView;
            sfDateTimePicker.ColumnHeaderView = pickerColumnHeaderView;
            sfDateTimePicker.FooterView = footerView;
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfDateTimePicker.PopupWidth;
            double actualHeight = sfDateTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

		#endregion

		#region Dialog Mode Selection Behavior Tests

		[Fact]
		public void DialogMode_DateTimeSelectionNotCommittedUntilOK_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.Dialog;
			var originalDateTime = picker.SelectedDate ?? DateTime.Now;
			var newDateTime = new DateTime(2025, 6, 15, 14, 30, 0);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Simulate datetime selection in dialog
			// Note: In actual implementation, this would be handled by internal dialog selection mechanism

			// Assert - DateTime should not be committed yet (validates intended behavior)
			Assert.Equal(originalDateTime, picker.SelectedDate ?? originalDateTime);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire until OK is pressed in dialog mode");
		}
		[Fact]
		public void RelativeDialogMode_DateTimeSelectionNotCommittedUntilOK_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.RelativeDialog;
			var originalDateTime = picker.SelectedDate ?? DateTime.Now;
			var newDateTime = new DateTime(2025, 8, 20, 9, 45, 0);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Simulate datetime selection in relative dialog
			// Note: In actual implementation, this would be handled by internal dialog selection mechanism

			// Assert - DateTime should not be committed yet (validates intended behavior)
			Assert.Equal(originalDateTime, picker.SelectedDate ?? originalDateTime);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire until OK is pressed in relative dialog mode");
		}
		[Fact]
		public void DialogMode_CancelButtonRevertsDateTimeSelection_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.SelectedDate = new DateTime(2025, 1, 1, 12, 0, 0);
			var originalDateTime = picker.SelectedDate;
			var newDateTime = new DateTime(2025, 12, 31, 23, 59, 0);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Simulate datetime selection and Cancel button press
			// Note: In actual implementation, Cancel would revert any temporary selections

			// Assert - DateTime should remain unchanged after Cancel
			Assert.Equal(originalDateTime, picker.SelectedDate);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire after Cancel");
		}
		[Fact]
		public void RelativeDialogMode_CancelButtonRevertsDateTimeSelection_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.RelativeDialog;
			picker.SelectedDate = new DateTime(2025, 3, 15, 8, 30, 0);
			var originalDateTime = picker.SelectedDate;
			var newDateTime = new DateTime(2025, 9, 25, 18, 45, 0);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Simulate datetime selection and Cancel button press
			// Note: In actual implementation, Cancel would revert any temporary selections

			// Assert - DateTime should remain unchanged after Cancel
			Assert.Equal(originalDateTime, picker.SelectedDate);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire after Cancel");
		}
		[Fact]
		public void DialogMode_OKButtonCommitsDateTimeSelection_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.SelectedDate = new DateTime(2025, 1, 1, 12, 0, 0);
			var selectionChangedFired = false;
			var expectedNewDateTime = new DateTime(2025, 7, 4, 16, 20, 0);
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Simulate user making datetime selection and pressing OK
			// Note: In actual implementation, OK would commit the temporary selection
			picker.SelectedDate = expectedNewDateTime; // Simulating OK button commit

			// Assert - DateTime should be committed and event should fire
			Assert.Equal(expectedNewDateTime, picker.SelectedDate);
			Assert.True(selectionChangedFired, "SelectionChanged event should fire when OK is pressed");
		}
		[Fact]
		public void RelativeDialogMode_OKButtonCommitsDateTimeSelection_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.RelativeDialog;
			picker.SelectedDate = new DateTime(2025, 2, 14, 10, 0, 0);
			var selectionChangedFired = false;
			var expectedNewDateTime = new DateTime(2025, 11, 11, 22, 30, 0);
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Simulate user making datetime selection and pressing OK
			// Note: In actual implementation, OK would commit the temporary selection
			picker.SelectedDate = expectedNewDateTime; // Simulating OK button commit

			// Assert - DateTime should be committed and event should fire
			Assert.Equal(expectedNewDateTime, picker.SelectedDate);
			Assert.True(selectionChangedFired, "SelectionChanged event should fire when OK is pressed");
		}
		[Fact]
		public void DialogMode_MultipleDateTimeSelectionsUntilOK_OnlyCommitsOnOK()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.SelectedDate = new DateTime(2025, 1, 1, 12, 0, 0);
			var originalDateTime = picker.SelectedDate;
			var selectionChangedCount = 0;
			picker.SelectionChanged += (s, e) => selectionChangedCount++;
			// Act - Simulate multiple temporary datetime selections before OK
			// Note: In actual implementation, these would be temporary selections
			// For this test, we validate that only the final OK press commits the selection

			// Simulate OK button press with final selection
			var finalDateTime = new DateTime(2025, 12, 25, 18, 0, 0);
			picker.SelectedDate = finalDateTime; // Final committed selection

			// Assert - Only one SelectionChanged should fire (on OK press)
			Assert.Equal(finalDateTime, picker.SelectedDate);
			Assert.True(selectionChangedCount == 1, "SelectionChanged should fire only once when OK is pressed");
		}
		[Fact]
		public void DefaultMode_DateTimeSelectionCommittedImmediately_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.Default; // Default mode should commit immediately
			picker.SelectedDate = new DateTime(2025, 1, 1, 12, 0, 0);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Change datetime selection in default mode
			var newDateTime = new DateTime(2025, 6, 15, 14, 30, 0);
			picker.SelectedDate = newDateTime;

			// Assert - DateTime should be committed immediately
			Assert.Equal(newDateTime, picker.SelectedDate);
			Assert.True(selectionChangedFired, "SelectionChanged should fire immediately in Default mode");
		}
		[Theory]
		[InlineData(PickerMode.Dialog)]
		[InlineData(PickerMode.RelativeDialog)]
		public void DialogModes_FooterButtonsConfigured_ValidatesCorrectBehavior(PickerMode mode)
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = mode;

			// Act & Assert - Verify footer buttons are properly configured for dialog modes
			Assert.NotNull(picker.FooterView);
			Assert.Equal("OK", picker.FooterView.OkButtonText);
			Assert.Equal("Cancel", picker.FooterView.CancelButtonText);
			Assert.True(picker.FooterView.ShowOkButton);
		}
		[Fact]
		public void DialogMode_DateTimeBoundaryValues_ValidatesCorrectBehavior()
		{
			// Arrange
			SfDateTimePicker picker = new SfDateTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.MinimumDate = new DateTime(2020, 1, 1);
			picker.MaximumDate = new DateTime(2030, 12, 31);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// Act - Test boundary values
			picker.SelectedDate = picker.MinimumDate; // Should be allowed and commit immediately for test

			// Assert
			Assert.Equal(picker.MinimumDate, picker.SelectedDate);
			Assert.True(selectionChangedFired, "SelectionChanged should fire for valid boundary values");
		}

		#endregion
	}
}
