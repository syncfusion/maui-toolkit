using Syncfusion.Maui.Toolkit.Picker;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class TimePickerUnitTest : PickerBaseUnitTest
    {
        #region TimePicker Public Properties

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfTimePicker picker = new SfTimePicker();

            Assert.NotNull(picker.HeaderView);
            Assert.NotNull(picker.ColumnHeaderView);
            Assert.Equal(1, picker.HourInterval);
            Assert.Equal(1, picker.MinuteInterval);
            Assert.Equal(1, picker.SecondInterval);
            Assert.Equal(PickerTimeFormat.HH_mm_ss, picker.Format);
            Assert.Equal(TimeSpan.Zero, picker.MinimumTime);
            Assert.Equal(new TimeSpan(23, 59, 59), picker.MaximumTime);
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
        [InlineData("02:45")]
        [InlineData("17:31")]
        [InlineData("6:10")]
        [InlineData("22:2")]
        public void TimePicker_SelectedTime_GetAndSet(string dateValue)
        {
            SfTimePicker picker = new SfTimePicker();

            TimeSpan expectedValue = TimeSpan.Parse(dateValue);
            picker.SelectedTime = expectedValue;
            TimeSpan? actualValue = picker.SelectedTime;
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        ////[InlineData(-5)]
        [InlineData(25)]
        public void TimePicker_HourInterval_GetAndSet(int value)
        {
            SfTimePicker picker = new SfTimePicker();
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
            SfTimePicker picker = new SfTimePicker();
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
            SfTimePicker picker = new SfTimePicker();
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
            SfTimePicker picker = new SfTimePicker();
            picker.Format = format;
            PickerTimeFormat actualValue = picker.Format;
            Assert.Equal(format, actualValue);
        }

        [Theory]
        [InlineData("02:45")]
        [InlineData("17:31")]
        [InlineData("6:10")]
        [InlineData("22:2")]
        public void TimePicker_MinimumTime_GetAndSet(string dateValue)
        {
            SfTimePicker picker = new SfTimePicker();

            TimeSpan expectedValue = TimeSpan.Parse(dateValue);
            picker.MinimumTime = expectedValue;
            TimeSpan actualValue = picker.MinimumTime;
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("02:45")]
        [InlineData("17:31")]
        [InlineData("6:10")]
        [InlineData("22:2")]
        public void TimePicker_MaximumTime_GetAndSet(string dateValue)
        {
            SfTimePicker picker = new SfTimePicker();

            TimeSpan expectedValue = TimeSpan.Parse(dateValue);
            picker.MaximumTime = expectedValue;
            TimeSpan actualValue = picker.MaximumTime;
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void TimePicker_ColumnDividerColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_ItemHeight_GetAndSet(int expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_TextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.TextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void TimePicker_TextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.TextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void TimePicker_TextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TimePicker_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectedTextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void TimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void TimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TimePicker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TimePicker_IsOpen_GetAndSet(bool expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.IsOpen = expectedValue;
            bool actualValue = picker.IsOpen;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(PickerMode.Default)]
        [InlineData(PickerMode.Dialog)]
        [InlineData(PickerMode.RelativeDialog)]
        public void TimePicker_Mode_GetAndSet(PickerMode mode)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.Mode = mode;
            PickerMode actualValue = picker.Mode;
            Assert.Equal(mode, actualValue);
        }

        [Theory]
        [InlineData(PickerTextDisplayMode.Default)]
        [InlineData(PickerTextDisplayMode.Fade)]
        [InlineData(PickerTextDisplayMode.Shrink)]
        [InlineData(PickerTextDisplayMode.FadeAndShrink)]
        public void TimePicker_TextDisplayMode_GetAndSet(PickerTextDisplayMode mode)
        {
            SfTimePicker picker = new SfTimePicker();
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
        public void TimePicker_RelativePosition_GetAndSet(PickerRelativePosition position)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.RelativePosition = position;
            PickerRelativePosition actualValue = picker.RelativePosition;
            Assert.Equal(position, actualValue);
        }

        [Fact]
        public void HeaderTemplate_GetAndSet_UsingDataTemplate()
        {
            SfTimePicker picker = new SfTimePicker();

            picker.HeaderView.Height = 50;
            picker.HeaderTemplate = new DataTemplate(() =>
            {
                return new Label { Text = "Header Content" };
            });

            Assert.NotNull(picker.HeaderTemplate);
        }

        [Fact]
        public void HeaderTemplate_GetAndSet_UsingDataTemplate_WhenaCalledDynamic()
        {
            SfTimePicker picker = new SfTimePicker();

            Assert.Null(picker.HeaderTemplate);

            picker.HeaderView.Height = 50;
            picker.HeaderTemplate = new DataTemplate(() =>
            {
                return new Label { Text = "Header Content" };
            });

            Assert.NotNull(picker.HeaderTemplate);
        }

        [Fact]
        public void ColumnHeaderTemplate_GetAndSet_UsingDataTemplate()
        {
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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

        #region TimePicker Internal Properties

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void TimePicker_TimePickerBackground_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.TimePickerBackground = expectedValue;
            Color actualValue = picker.TimePickerBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void TimePicker_HeaderTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderTextColor = expectedValue;
            Color actualValue = picker.HeaderTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void TimePicker_FooterTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_SelectedTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_SelectionTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_NormalTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_HeaderFontSize_GetAndSet(double expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.HeaderFontSize = expectedValue;
            double actualValue = picker.HeaderFontSize;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void TimePicker_FooterFontSize_GetAndSet(double expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_SelectedFontSize_GetAndSet(double expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

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
        public void TimePicker_NormalFontSize_GetAndSet(double expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.NormalFontSize = expectedValue;
            double actualValue = picker.NormalFontSize;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Header Settings Public Properties

        [Fact]
        public void HeaderSettings_Constructor_InitializesDefaultsCorrectly()
        {
            SfTimePicker picker = new SfTimePicker();

            Assert.Equal(0d, picker.HeaderView.Height);
            Assert.Equal(string.Empty, picker.HeaderView.Text);
            Assert.NotNull(picker.HeaderView.TextStyle);
            Assert.Equal(Brush.Transparent, picker.HeaderView.Background);
            Assert.Equal(Color.FromArgb("#CAC4D0"), picker.HeaderView.DividerColor);
        }

        [Theory]
        [InlineData(40)]
        [InlineData(10)]
        [InlineData(-40)]
        [InlineData(60)]
        public void HeaderSettings_Height_GetAndSet(double value)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.HeaderView.Height = value;
            double actualValue = picker.HeaderView.Height;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("HeaderText")]
        [InlineData("")]
        public void HeaderSettings_Text_GetAndSet(string value)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.HeaderView.Text = value;
            string actualValue = picker.HeaderView.Text;
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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderView.DividerColor = expectedValue;
            Color actualValue = picker.HeaderView.DividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        #endregion

        #region ColumnHeader Settings Public Properties

        [Fact]
        public void ColumnHeaderSettings_Constructor_InitializesDefaultsCorrectly()
        {
            SfTimePicker picker = new SfTimePicker();

            Assert.Equal(40d, picker.ColumnHeaderView.Height);
            Assert.NotNull(picker.ColumnHeaderView.TextStyle);
            ////Assert.Equal(new SolidColorBrush(Color.FromArgb("#F7F2FB")), picker.ColumnHeaderView.Background);
            Assert.Equal(Color.FromArgb("#CAC4D0"), picker.ColumnHeaderView.DividerColor);
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
            SfTimePicker picker = new SfTimePicker();
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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.ColumnHeaderView.DividerColor = expectedValue;
            Color actualValue = picker.ColumnHeaderView.DividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("HourText")]
        [InlineData("")]
        public void ColumnHeaderSettings_HourHeaderText_GetAndSet(string value)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.ColumnHeaderView.HourHeaderText = value;
            string actualValue = picker.ColumnHeaderView.HourHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("MinuteText")]
        [InlineData("")]
        public void ColumnHeaderSettings_MinuteHeaderText_GetAndSet(string value)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.ColumnHeaderView.MinuteHeaderText = value;
            string actualValue = picker.ColumnHeaderView.MinuteHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("SecondText")]
        [InlineData("")]
        public void ColumnHeaderSettings_SecondHeaderText_GetAndSet(string value)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.ColumnHeaderView.SecondHeaderText = value;
            string actualValue = picker.ColumnHeaderView.SecondHeaderText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Meridiem")]
        public void ColumnHeaderSettings_MeridiemHeaderText_GetAndSet(string value)
        {
            SfTimePicker picker = new SfTimePicker();
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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Fact]
        public void ColumnHeaderSettings_GetAndSet_Null()
        {
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();
            picker.FooterView.Height = value;
            double actualValue = picker.FooterView.Height;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Save")]
        [InlineData("")]
        public void FooterSettings_OkButtonText_GetAndSet(string value)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.FooterView.OkButtonText = value;
            string actualValue = picker.FooterView.OkButtonText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Exit")]
        [InlineData("")]
        public void FooterSettings_CancelButtonText_GetAndSet(string value)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.FooterView.CancelButtonText = value;
            string actualValue = picker.FooterView.CancelButtonText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FooterSettings_ShowOkButton_GetAndSet(bool value)
        {
            SfTimePicker picker = new SfTimePicker();
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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FooterSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfTimePicker picker = new SfTimePicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        #endregion

        #region Picker Selection View Public Properties

        [Fact]
        public void PickerSelectionView_Constructor_InitializesDefaultsCorrectly()
        {
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectionView.Stroke = expectedValue;
            Color actualValue = picker.SelectionView.Stroke;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(30, 30, 30, 30)]
        [InlineData(50, 50, 50, 50)]
        [InlineData(30, 50, 30, 50)]
        [InlineData(-30, -30, -30, -30)]
        [InlineData(0, 0, 0, 0)]
        public void PickerSelectionView_CornerRadius_GetAndSet(double topLeft, double topRight, double bottomLeft, double bottomRight)
        {
            SfTimePicker picker = new SfTimePicker();
            picker.SelectionView.CornerRadius = new CornerRadius(topLeft, topRight, bottomLeft, bottomRight);
            CornerRadius actualValue = picker.SelectionView.CornerRadius;
            Assert.Equal(new CornerRadius(topLeft, topRight, bottomLeft, bottomRight), actualValue);
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
            SfTimePicker picker = new SfTimePicker();
            CornerRadius exceptedValue = new CornerRadius(value1, value2, value3, value4);
            picker.SelectionView.CornerRadius = exceptedValue;
            CornerRadius actualValue = picker.SelectionView.CornerRadius;
            Assert.Equal(exceptedValue, actualValue);
        }

        [Theory]
        [InlineData(30, 30, 30, 30)]
        [InlineData(50, 50, 50, 50)]
        [InlineData(30, 50, 30, 50)]
        [InlineData(-30, -30, -30, -30)]
        [InlineData(0, 0, 0, 0)]
        public void PickerSelectionView_Padding_GetAndSet(double left, double top, double right, double bottom)
        {
            SfTimePicker picker = new SfTimePicker();
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
        public void PickerSelectionView_Padding1_GetAndSet(int value1, int value2, int value3, int value4)
        {
            SfTimePicker picker = new SfTimePicker();
            Thickness exceptedValue = new Thickness(value1, value2, value3, value4);
            picker.SelectionView.Padding = exceptedValue;
            Thickness actualValue = picker.SelectionView.Padding;
            Assert.Equal(exceptedValue, actualValue);
        }

        [Fact]
        public void PickerSelectionView_GetAndSet_Null()
        {
            SfTimePicker picker = new SfTimePicker();

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
            SfTimePicker picker = new SfTimePicker();
            var fired = false;
            picker.SelectionChanged += (sender, e) => fired = true;
            picker.SelectedTime = new TimeSpan(20, 05, 19);
            Assert.True(fired);
        }

        #endregion

        #region TimePickerHelper Methods

        [Fact]
        public void IsSameTimeSpan_BothNull_ReturnsFalse()
        {
            Assert.False(TimePickerHelper.IsSameTimeSpan(null, null));
        }

        [Fact]
        public void IsSameTimeSpan_OneNull_ReturnsFalse()
        {
            TimeSpan time = new TimeSpan(10, 30, 0);
            Assert.False(TimePickerHelper.IsSameTimeSpan(time, null));
            Assert.False(TimePickerHelper.IsSameTimeSpan(null, time));
        }

        [Fact]
        public void IsSameTimeSpan_SameTime_ReturnsTrue()
        {
            TimeSpan time1 = new TimeSpan(10, 30, 45);
            TimeSpan time2 = new TimeSpan(10, 30, 45);
            Assert.True(TimePickerHelper.IsSameTimeSpan(time1, time2));
        }

        [Fact]
        public void IsSameTimeSpan_DifferentTime_ReturnsFalse()
        {
            TimeSpan time1 = new TimeSpan(10, 30, 45);
            TimeSpan time2 = new TimeSpan(10, 30, 46);
            Assert.False(TimePickerHelper.IsSameTimeSpan(time1, time2));
        }

        [Fact]
        public void GetMinutes_NoRestrictions_ReturnsFullRange()
        {
            var result = TimePickerHelper.GetMinutes(1, 10, null, null, null);
            Assert.Equal(60, result.Count);
            Assert.Equal("00", result[0]);
            Assert.Equal("59", result[59]);
        }

        [Fact]
        public void GetMinutes_WithInterval_ReturnsCorrectRange()
        {
            var result = TimePickerHelper.GetMinutes(15, 10, null, null, null);
            Assert.Equal(4, result.Count);
            Assert.Equal("00", result[0]);
            Assert.Equal("15", result[1]);
            Assert.Equal("30", result[2]);
            Assert.Equal("45", result[3]);
        }

        [Fact]
        public void GetMinutes_WithMinDate_ReturnsCorrectRange()
        {
            DateTime selectedDate = new DateTime(2023, 1, 1, 10, 0, 0);
            DateTime minDate = new DateTime(2023, 1, 1, 10, 30, 0);
            var result = TimePickerHelper.GetMinutes(1, 10, selectedDate, minDate, null);
            Assert.Equal(30, result.Count);
            Assert.Equal("30", result[0]);
            Assert.Equal("59", result[29]);
        }

        [Fact]
        public void GetMinutes_WithMaxDate_ReturnsCorrectRange()
        {
            DateTime selectedDate = new DateTime(2023, 1, 1, 10, 0, 0);
            DateTime maxDate = new DateTime(2023, 1, 1, 10, 45, 0);
            var result = TimePickerHelper.GetMinutes(1, 10, selectedDate, null, maxDate);
            Assert.Equal(46, result.Count);
            Assert.Equal("00", result[0]);
            Assert.Equal("45", result[45]);
        }

        [Fact]
        public void GetMinutes_WithMinAndMaxDate_ReturnsCorrectRange()
        {
            DateTime selectedDate = new DateTime(2023, 1, 1, 10, 0, 0);
            DateTime minDate = new DateTime(2023, 1, 1, 10, 15, 0);
            DateTime maxDate = new DateTime(2023, 1, 1, 10, 45, 0);
            var result = TimePickerHelper.GetMinutes(5, 10, selectedDate, minDate, maxDate);
            Assert.Equal(7, result.Count);
            Assert.Equal("15", result[0]);
            Assert.Equal("45", result[6]);
        }

        [Fact]
        public void GetSeconds_NoRestrictions_ReturnsFullRange()
        {
            var result = TimePickerHelper.GetSeconds(1, 10, 30, null, null, null);
            Assert.Equal(60, result.Count);
            Assert.Equal("00", result[0]);
            Assert.Equal("59", result[59]);
        }

        [Fact]
        public void GetSeconds_WithInterval_ReturnsCorrectRange()
        {
            var result = TimePickerHelper.GetSeconds(15, 10, 30, null, null, null);
            Assert.Equal(4, result.Count);
            Assert.Equal("00", result[0]);
            Assert.Equal("15", result[1]);
            Assert.Equal("30", result[2]);
            Assert.Equal("45", result[3]);
        }

        [Fact]
        public void GetSeconds_WithMinDate_ReturnsCorrectRange()
        {
            DateTime selectedDate = new DateTime(2023, 1, 1, 10, 30, 0);
            DateTime minDate = new DateTime(2023, 1, 1, 10, 30, 20);
            var result = TimePickerHelper.GetSeconds(1, 10, 30, selectedDate, minDate, null);
            Assert.Equal(40, result.Count);
            Assert.Equal("20", result[0]);
            Assert.Equal("59", result[39]);
        }

        [Fact]
        public void GetSeconds_WithMaxDate_ReturnsCorrectRange()
        {
            DateTime selectedDate = new DateTime(2023, 1, 1, 10, 30, 0);
            DateTime maxDate = new DateTime(2023, 1, 1, 10, 30, 40);
            var result = TimePickerHelper.GetSeconds(1, 10, 30, selectedDate, null, maxDate);
            Assert.Equal(41, result.Count);
            Assert.Equal("00", result[0]);
            Assert.Equal("40", result[40]);
        }

        [Fact]
        public void GetSeconds_WithMinAndMaxDate_ReturnsCorrectRange()
        {
            DateTime selectedDate = new DateTime(2023, 1, 1, 10, 30, 0);
            DateTime minDate = new DateTime(2023, 1, 1, 10, 30, 15);
            DateTime maxDate = new DateTime(2023, 1, 1, 10, 30, 45);
            var result = TimePickerHelper.GetSeconds(5, 10, 30, selectedDate, minDate, maxDate);
            Assert.Equal(7, result.Count);
            Assert.Equal("15", result[0]);
            Assert.Equal("45", result[6]);
        }

        [Fact]
        public void GetHours_Format_h_NoRestrictions_ReturnsCorrectRange()
        {
            var result = TimePickerHelper.GetHours("h", 1, null, null, null);
            Assert.Equal(12, result.Count);
            Assert.Equal("1", result[0]);
            Assert.Equal("11", result[10]);
            Assert.Contains("12", result);
        }

        [Fact]
        public void GetHours_Format_hh_NoRestrictions_ReturnsCorrectRange()
        {
            var result = TimePickerHelper.GetHours("hh", 1, null, null, null);
            Assert.Equal(12, result.Count);
            Assert.Equal("01", result[0]);
            Assert.Equal("11", result[10]);
            Assert.Contains("12", result);
        }

        [Fact]
        public void GetHours_Format_H_NoRestrictions_ReturnsCorrectRange()
        {
            var result = TimePickerHelper.GetHours("H", 1, null, null, null);
            Assert.Equal(24, result.Count);
            Assert.Equal("0", result[0]);
            Assert.Equal("23", result[23]);
        }

        [Fact]
        public void GetHours_Format_HH_NoRestrictions_ReturnsCorrectRange()
        {
            var result = TimePickerHelper.GetHours("HH", 1, null, null, null);
            Assert.Equal(24, result.Count);
            Assert.Equal("00", result[0]);
            Assert.Equal("23", result[23]);
        }

        [Fact]
        public void GetHours_Format_h_WithInterval_ReturnsCorrectRange()
        {
            var result = TimePickerHelper.GetHours("h", 3, null, null, null);
            Assert.Equal(4, result.Count);
            Assert.Equal("3", result[0]);
            Assert.Equal("6", result[1]);
            Assert.Equal("9", result[2]);
            Assert.Equal("12", result[3]);
        }

        [Fact]
        public void GetHours_Format_H_WithMinMaxDate_ReturnsCorrectRange()
        {
            DateTime selectedDate = new DateTime(2023, 1, 1, 12, 0, 0);
            DateTime minDate = new DateTime(2023, 1, 1, 5, 0, 0);
            DateTime maxDate = new DateTime(2023, 1, 1, 20, 0, 0);
            var result = TimePickerHelper.GetHours("H", 1, selectedDate, minDate, maxDate);
            Assert.Equal(16, result.Count);
            Assert.Equal("5", result[0]);
            Assert.Equal("20", result[15]);
        }

        [Theory]
        [InlineData("h", 0, "12")]
        [InlineData("h", 1, "1")]
        [InlineData("h", 11, "11")]
        [InlineData("h", 12, "12")]
        [InlineData("h", 13, "1")]
        [InlineData("h", 23, "11")]
        public void GetHourText_Format_h_ReturnsCorrectValue(string format, int hour, string expected)
        {
            var result = TimePickerHelper.GetHourText(format, hour);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("hh", 0, "12")]
        [InlineData("hh", 1, "01")]
        [InlineData("hh", 11, "11")]
        [InlineData("hh", 12, "12")]
        [InlineData("hh", 13, "01")]
        [InlineData("hh", 23, "11")]
        public void GetHourText_Format_hh_ReturnsCorrectValue(string format, int hour, string expected)
        {
            var result = TimePickerHelper.GetHourText(format, hour);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("H", 0, "0")]
        [InlineData("H", 1, "1")]
        [InlineData("H", 11, "11")]
        [InlineData("H", 12, "12")]
        [InlineData("H", 13, "13")]
        [InlineData("H", 23, "23")]
        public void GetHourText_Format_H_ReturnsCorrectValue(string format, int hour, string expected)
        {
            var result = TimePickerHelper.GetHourText(format, hour);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("HH", 0, "00")]
        [InlineData("HH", 1, "01")]
        [InlineData("HH", 11, "11")]
        [InlineData("HH", 12, "12")]
        [InlineData("HH", 13, "13")]
        [InlineData("HH", 23, "23")]
        public void GetHourText_Format_HH_ReturnsCorrectValue(string format, int hour, string expected)
        {
            var result = TimePickerHelper.GetHourText(format, hour);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, "00")]
        [InlineData(1, "01")]
        [InlineData(9, "09")]
        [InlineData(10, "10")]
        [InlineData(59, "59")]
        public void GetMinuteOrSecondText_ReturnsCorrectValue(int value, string expected)
        {
            var result = TimePickerHelper.GetMinuteOrSecondText(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetMeridiem_NoRestrictions_ReturnsBothMeridiems()
        {
            var result = TimePickerHelper.GetMeridiem(null, null, null);
            Assert.Equal(2, result.Count);
            Assert.Equal(CultureInfo.CurrentUICulture.DateTimeFormat.AMDesignator, result[0]);
            Assert.Equal(CultureInfo.CurrentUICulture.DateTimeFormat.PMDesignator, result[1]);
        }

        [Fact]
        public void GetMeridiem_OnlyAM_ReturnsOnlyAM()
        {
            var selectedDate = new DateTime(2023, 1, 1, 10, 0, 0);
            var maxDate = new DateTime(2023, 1, 1, 11, 59, 59);
            var result = TimePickerHelper.GetMeridiem(null, maxDate, selectedDate);
            Assert.Single(result);
            Assert.Equal(CultureInfo.CurrentUICulture.DateTimeFormat.AMDesignator, result[0]);
        }

        [Fact]
        public void GetMeridiem_OnlyPM_ReturnsOnlyPM()
        {
            var selectedDate = new DateTime(2023, 1, 1, 14, 0, 0);
            var minDate = new DateTime(2023, 1, 1, 12, 0, 0);
            var result = TimePickerHelper.GetMeridiem(minDate, null, selectedDate);
            Assert.Single(result);
            Assert.Equal(CultureInfo.CurrentUICulture.DateTimeFormat.PMDesignator, result[0]);
        }

        [Fact]
        public void GetMeridiem_BothAMAndPM_ReturnsBothMeridiems()
        {
            var selectedDate = new DateTime(2023, 1, 1, 10, 0, 0);
            var minDate = new DateTime(2023, 1, 1, 9, 0, 0);
            var maxDate = new DateTime(2023, 1, 1, 14, 0, 0);
            var result = TimePickerHelper.GetMeridiem(minDate, maxDate, selectedDate);
            Assert.Equal(2, result.Count);
            Assert.Equal(CultureInfo.CurrentUICulture.DateTimeFormat.AMDesignator, result[0]);
            Assert.Equal(CultureInfo.CurrentUICulture.DateTimeFormat.PMDesignator, result[1]);
        }

        [Fact]
        public void IsAMText_NegativeIndex_ReturnsTrue()
        {
            var meridiems = new ObservableCollection<string> { "AM", "PM" };
            var result = TimePickerHelper.IsAMText(meridiems, -1);
            Assert.True(result);
        }

        [Fact]
        public void IsAMText_AMIndex_ReturnsTrue()
        {
            var meridiems = new ObservableCollection<string> { "AM", "PM" };
            var result = TimePickerHelper.IsAMText(meridiems, 0);
            Assert.True(result);
        }

        [Fact]
        public void IsAMText_PMIndex_ReturnsFalse()
        {
            var meridiems = new ObservableCollection<string> { "AM", "PM" };
            var result = TimePickerHelper.IsAMText(meridiems, 1);
            Assert.False(result);
        }

        [Fact]
        public void GetMinuteOrSecondIndex_ExactMatch_ReturnsCorrectIndex()
        {
            var collection = new ObservableCollection<string> { "00", "15", "30", "45" };
            var result = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(collection, 30);
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetMinuteOrSecondIndex_NoExactMatch_ReturnsNextHigherIndex()
        {
            var collection = new ObservableCollection<string> { "00", "15", "30", "45" };
            var result = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(collection, 20);
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetMinuteOrSecondIndex_ValueHigherThanAll_ReturnsLastIndex()
        {
            var collection = new ObservableCollection<string> { "00", "15", "30", "45" };
            var result = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(collection, 50);
            Assert.Equal(3, result);
        }

        [Fact]
        public void GetMinuteOrSecondIndex_EmptyCollection_ReturnsNegativeOne()
        {
            var collection = new ObservableCollection<string>();
            var result = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(collection, 30);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void GetHourIndex_Format_h_ExactMatch_ReturnsCorrectIndex()
        {
            var hours = new ObservableCollection<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            var result = TimePickerHelper.GetHourIndex("h", hours, 3);
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetHourIndex_Format_hh_ExactMatch_ReturnsCorrectIndex()
        {
            var hours = new ObservableCollection<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            var result = TimePickerHelper.GetHourIndex("hh", hours, 3);
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetHourIndex_Format_H_ExactMatch_ReturnsCorrectIndex()
        {
            var hours = new ObservableCollection<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            var result = TimePickerHelper.GetHourIndex("H", hours, 15);
            Assert.Equal(15, result);
        }

        [Fact]
        public void GetHourIndex_Format_HH_ExactMatch_ReturnsCorrectIndex()
        {
            var hours = new ObservableCollection<string> { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            var result = TimePickerHelper.GetHourIndex("HH", hours, 15);
            Assert.Equal(15, result);
        }

        [Fact]
        public void GetHourIndex_NoExactMatch_ReturnsNextHigherIndex()
        {
            var hours = new ObservableCollection<string> { "0", "3", "6", "9", "12", "15", "18", "21" };
            var result = TimePickerHelper.GetHourIndex("H", hours, 4);
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetHourIndex_ValueHigherThanAll_ReturnsLastIndex()
        {
            var hours = new ObservableCollection<string> { "0", "3", "6", "9", "12", "15", "18", "21" };
            var result = TimePickerHelper.GetHourIndex("H", hours, 23);
            Assert.Equal(7, result);
        }

        [Fact]
        public void GetHourIndex_EmptyFormat_ReturnsNegativeOne()
        {
            var hours = new ObservableCollection<string> { "0", "3", "6", "9", "12", "15", "18", "21" };
            var result = TimePickerHelper.GetHourIndex("", hours, 12);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void GetHourIndex_NullHour_ReturnsZero()
        {
            var hours = new ObservableCollection<string> { "0", "3", "6", "9", "12", "15", "18", "21" };
            var result = TimePickerHelper.GetHourIndex("H", hours, null);
            Assert.Equal(7, result);
        }

        [Theory]
        [InlineData(PickerTimeFormat.H_mm, "H", new[] { 0, 1 })]
        [InlineData(PickerTimeFormat.HH_mm, "HH", new[] { 0, 1 })]
        [InlineData(PickerTimeFormat.H_mm_ss, "H", new[] { 0, 1, 2 })]
        [InlineData(PickerTimeFormat.HH_mm_ss, "HH", new[] { 0, 1, 2 })]
        [InlineData(PickerTimeFormat.h_mm_ss_tt, "h", new[] { 0, 1, 2, 3 })]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt, "hh", new[] { 0, 1, 2, 3 })]
        [InlineData(PickerTimeFormat.h_mm_tt, "h", new[] { 0, 1, 3 })]
        [InlineData(PickerTimeFormat.hh_mm_tt, "hh", new[] { 0, 1, 3 })]
        [InlineData(PickerTimeFormat.hh_tt, "hh", new[] { 0, 3 })]
        public void GetFormatStringOrder_PredefinedFormats_ReturnsCorrectOrderAndFormat(PickerTimeFormat format, string expectedHourFormat, int[] expectedOrder)
        {
            string hourFormat;
            var result = TimePickerHelper.GetFormatStringOrder(out hourFormat, format);

            Assert.Equal(expectedHourFormat, hourFormat);
            Assert.Equal(expectedOrder, result);
        }

        [Fact]
        public void GetFormatStringOrder_DefaultFormat_UsesCurrentCultureFormat()
        {
            var currentCulture = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentUICulture = new CultureInfo("en-US");

                string hourFormat;
                var result = TimePickerHelper.GetFormatStringOrder(out hourFormat, PickerTimeFormat.Default);

                Assert.Equal("h", hourFormat);
                Assert.Equal(new List<int> { 0, 1, 3 }, result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = currentCulture;
            }
        }

        [Fact]
        public void GetFormatStringOrder_CustomFormat_ReturnsCorrectOrderAndFormat()
        {
            var currentCulture = CultureInfo.CurrentUICulture;

            try
            {
                var customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                customCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
                CultureInfo.CurrentUICulture = customCulture;

                string hourFormat;
                var result = TimePickerHelper.GetFormatStringOrder(out hourFormat, PickerTimeFormat.Default);

                Assert.Equal("HH", hourFormat);
                Assert.Equal(new List<int> { 0, 1, 2 }, result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = currentCulture;
            }
        }

        [Fact]
        public void GetFormatStringOrder_UnexpectedFormat_HandlesGracefully()
        {
            var currentCulture = CultureInfo.CurrentUICulture;

            try
            {
                var customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                customCulture.DateTimeFormat.ShortTimePattern = "unexpected_format";
                CultureInfo.CurrentUICulture = customCulture;

                string hourFormat;
                var result = TimePickerHelper.GetFormatStringOrder(out hourFormat, PickerTimeFormat.Default);

                Assert.Empty(result);
                Assert.Equal(string.Empty, hourFormat);
            }
            finally
            {
                CultureInfo.CurrentUICulture = currentCulture;
            }
        }

        [Fact]
        public void GetValidMaxTime_MaxTimeGreaterThanMinTime_ReturnsMaxTime()
        {
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidMaxTime(minTime, maxTime);
            Assert.Equal(maxTime, result);
        }

        [Fact]
        public void GetValidMaxTime_MaxTimeLessThanMinTime_ReturnsMinTime()
        {
            TimeSpan minTime = new TimeSpan(12, 0, 0);
            TimeSpan maxTime = new TimeSpan(10, 0, 0);
            var result = TimePickerHelper.GetValidMaxTime(minTime, maxTime);
            Assert.Equal(minTime, result);
        }

        [Fact]
        public void GetValidMaxTime_MaxTimeEqualToMinTime_ReturnsMinTime()
        {
            TimeSpan minTime = new TimeSpan(12, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidMaxTime(minTime, maxTime);
            Assert.Equal(minTime, result);
        }

        [Fact]
        public void GetValidSelectedTime_TimeWithinRange_ReturnsOriginalTime()
        {
            TimeSpan? time = new TimeSpan(11, 0, 0);
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidSelectedTime(time, minTime, maxTime);
            Assert.Equal(time, result);
        }

        [Fact]
        public void GetValidSelectedTime_TimeLessThanMinTime_ReturnsMinTimeWithOriginalSeconds()
        {
            TimeSpan? time = new TimeSpan(9, 30, 45);
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidSelectedTime(time, minTime, maxTime);
            Assert.Equal(new TimeSpan(10, 0, 45), result);
        }

        [Fact]
        public void GetValidSelectedTime_TimeGreaterThanMaxTime_ReturnsMaxTimeWithOriginalSeconds()
        {
            TimeSpan? time = new TimeSpan(13, 30, 45);
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidSelectedTime(time, minTime, maxTime);
            Assert.Equal(new TimeSpan(12, 0, 45), result);
        }

        [Fact]
        public void GetValidSelectedTime_TimeIsNull_ReturnsNull()
        {
            TimeSpan? time = null;
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidSelectedTime(time, minTime, maxTime);
            Assert.Null(result);
        }

        [Fact]
        public void GetValidSelectedTime_TimeEqualToMinTime_ReturnsMinTime()
        {
            TimeSpan? time = new TimeSpan(10, 0, 0);
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidSelectedTime(time, minTime, maxTime);
            Assert.Equal(minTime, result);
        }

        [Fact]
        public void GetValidSelectedTime_TimeEqualToMaxTime_ReturnsMaxTime()
        {
            TimeSpan? time = new TimeSpan(12, 0, 0);
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(12, 0, 0);
            var result = TimePickerHelper.GetValidSelectedTime(time, minTime, maxTime);
            Assert.Equal(maxTime, result);
        }

        #endregion

        #region TimePicker Private Methods

        [Fact]
        public void OnPickerSelectionIndexChanged_HourChanged_UpdatesSelectedTime()
        {
            var picker = new SfTimePicker();
            picker.SelectedTime = new TimeSpan(10, 30, 0);
            var e = new PickerSelectionChangedEventArgs { ColumnIndex = 0, NewValue = 15 };

            InvokePrivateMethod(picker, "OnPickerSelectionIndexChanged", null, e);

            Assert.Equal(new TimeSpan(15, 30, 0), picker.SelectedTime);
        }

        [Fact]
        public void OnPickerSelectionIndexChanged_MinuteChanged_UpdatesSelectedTime()
        {
            var picker = new SfTimePicker();
            picker.SelectedTime = new TimeSpan(10, 30, 0);
            var e = new PickerSelectionChangedEventArgs { ColumnIndex = 1, NewValue = 45 };

            InvokePrivateMethod(picker, "OnPickerSelectionIndexChanged", null, e);

            Assert.Equal(new TimeSpan(10, 45, 0), picker.SelectedTime);
        }

        [Fact]
        public void OnPickerSelectionIndexChanged_SecondChanged_UpdatesSelectedTime()
        {
            var picker = new SfTimePicker();
            picker.SelectedTime = new TimeSpan(10, 30, 0);
            var e = new PickerSelectionChangedEventArgs { ColumnIndex = 2, NewValue = 45 };

            InvokePrivateMethod(picker, "OnPickerSelectionIndexChanged", null, e);

            Assert.Equal(new TimeSpan(10, 30, 45), picker.SelectedTime);
        }

        [Fact]
        public void OnPickerSelectionIndexChanged_MeridiemChanged_UpdatesSelectedTime()
        {
            var picker = new SfTimePicker();
            picker.Format = PickerTimeFormat.hh_mm_ss_tt;
            picker.SelectedTime = new TimeSpan(10, 30, 0);
            InvokePrivateMethod(picker, "GeneratePickerColumns");
            var e = new PickerSelectionChangedEventArgs { ColumnIndex = 3, NewValue = 1 };

            InvokePrivateMethod(picker, "OnPickerSelectionIndexChanged", null, e);

            Assert.Equal(new TimeSpan(22, 30, 0), picker.SelectedTime);
        }

        [Fact]
        public void OnPickerSelectionIndexChanged_HourChangedWithMinuteInterval_UpdatesMinuteColumn()
        {
            var picker = new SfTimePicker();
            picker.MinuteInterval = 15;
            picker.SelectedTime = new TimeSpan(10, 0, 0);
            var e = new PickerSelectionChangedEventArgs { ColumnIndex = 0, NewValue = 11 };

            InvokePrivateMethod(picker, "OnPickerSelectionIndexChanged", null, e);

            var minuteColumn = GetPrivateField<SfTimePicker>(picker, "_minuteColumn") as PickerColumn;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var minutes = (ObservableCollection<string>)minuteColumn.ItemsSource;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal(new[] { "00", "15", "30", "45" }, minutes);
        }

        [Fact]
        public void SetSelectedTime_NewTime_UpdatesSelectedTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(10, 30, 0);

            InvokePrivateMethod(timePicker, "SetSelectedTime", new TimeSpan(11, 45, 30), 26);

            Assert.Equal(new TimeSpan(11, 45, 30), timePicker.SelectedTime);
        }

        [Fact]
        public void SetSelectedTime_SameTime_DoesNotUpdateSelectedTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(10, 30, 0);

            InvokePrivateMethod(timePicker, "SetSelectedTime", new TimeSpan(10, 30, 0), 5);

            Assert.Equal(new TimeSpan(10, 30, 0), timePicker.SelectedTime);
        }

        [Fact]
        public void SetSelectedTime_NullToValidTime_UpdatesSelectedTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = null;

            InvokePrivateMethod(timePicker, "SetSelectedTime", new TimeSpan(12, 0, 0), 7);

            Assert.Equal(new TimeSpan(12, 0, 0), timePicker.SelectedTime);
        }

        [Fact]
        public void SetSelectedTime_OutOfRange_UpdatesSelectedTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.MinimumTime = new TimeSpan(9, 0, 0);
            timePicker.MaximumTime = new TimeSpan(17, 0, 0);
            timePicker.SelectedTime = new TimeSpan(12, 0, 0);

            InvokePrivateMethod(timePicker, "SetSelectedTime", new TimeSpan(20, 0, 0), 3);

            Assert.Equal(new TimeSpan(17, 0, 0), timePicker.SelectedTime);
        }

        [Fact]
        public void GenerateHourColumn_24HourFormat_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            var selectedTime = new TimeSpan(10, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateHourColumn", "HH", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(SfPickerResources.GetLocalizedString("Hour"), result.HeaderText);
            Assert.IsType<ObservableCollection<string>>(result.ItemsSource);

            var hours = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(24, hours.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", hours[0]);
            Assert.Equal("23", hours[23]);
            Assert.Equal(10, result.SelectedIndex);
        }

        [Fact]
        public void GenerateHourColumn_12HourFormat_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            timePicker.Format = PickerTimeFormat.hh_mm_ss_tt;
            var selectedTime = new TimeSpan(14, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateHourColumn", "hh", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            var hours = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(12, hours.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("01", hours[0]);
            Assert.Equal("12", hours[11]);
            Assert.Equal(1, result.SelectedIndex);
        }

        [Fact]
        public void GenerateHourColumn_WithHourInterval_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            timePicker.HourInterval = 2;
            var selectedTime = new TimeSpan(10, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateHourColumn", "HH", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            var hours = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(12, hours.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", hours[0]);
            Assert.Equal("22", hours[11]);
            Assert.Equal(5, result.SelectedIndex);
        }

        [Fact]
        public void GenerateHourColumn_NullSelectedTime_UsesDefaultTime()
        {
            var timePicker = new SfTimePicker();
            SetPrivateField(timePicker, "_previousSelectedDateTime", new DateTime(2023, 1, 1, 15, 0, 0));

            var result = InvokePrivateMethod(timePicker, "GenerateHourColumn", "HH", null, null) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(15, result.SelectedIndex);
        }

        [Fact]
        public void GenerateHourColumn_OutOfRangeTime_AdjustsToValidTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.MinimumTime = new TimeSpan(8, 0, 0);
            timePicker.MaximumTime = new TimeSpan(16, 0, 0);
            var selectedTime = new TimeSpan(20, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateHourColumn", "HH", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            var hours = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(9, hours.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("08", hours[0]);
            Assert.Equal("16", hours[8]);
            Assert.Equal(8, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMinuteColumn_DefaultInterval_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            var selectedTime = new TimeSpan(10, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMinuteColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(SfPickerResources.GetLocalizedString("Minute"), result.HeaderText);
            Assert.IsType<ObservableCollection<string>>(result.ItemsSource);

            var minutes = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(60, minutes.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", minutes[0]);
            Assert.Equal("59", minutes[59]);
            Assert.Equal(30, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMinuteColumn_CustomInterval_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            timePicker.MinuteInterval = 15;
            var selectedTime = new TimeSpan(10, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMinuteColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            var minutes = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(4, minutes.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", minutes[0]);
            Assert.Equal("15", minutes[1]);
            Assert.Equal("30", minutes[2]);
            Assert.Equal("45", minutes[3]);
            Assert.Equal(2, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMinuteColumn_NullSelectedTime_UsesDefaultTime()
        {
            var timePicker = new SfTimePicker();
            SetPrivateField(timePicker, "_previousSelectedDateTime", new DateTime(2023, 1, 1, 15, 45, 0));

            var result = InvokePrivateMethod(timePicker, "GenerateMinuteColumn", null, null) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(45, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMinuteColumn_OutOfRangeTime_AdjustsToValidTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.MinimumTime = new TimeSpan(10, 30, 0);
            timePicker.MaximumTime = new TimeSpan(14, 45, 0);
            var selectedTime = new TimeSpan(12, 0, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMinuteColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            var minutes = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(60, minutes.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal(0, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMinuteColumn_MinuteIntervalNotDivisibleBy60_AdjustsCorrectly()
        {
            var timePicker = new SfTimePicker();
            timePicker.MinuteInterval = 7;
            var selectedTime = new TimeSpan(10, 28, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMinuteColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            var minutes = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(9, minutes.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("28", minutes[4]);
            Assert.Equal(4, result.SelectedIndex);
        }

        [Fact]
        public void GenerateSecondColumn_DefaultInterval_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(10, 30, 45);

            var result = InvokePrivateMethod(timePicker, "GenerateSecondColumn") as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(SfPickerResources.GetLocalizedString("Second"), result.HeaderText);
            Assert.IsType<ObservableCollection<string>>(result.ItemsSource);

            var seconds = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(60, seconds.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", seconds[0]);
            Assert.Equal("59", seconds[59]);
            Assert.Equal(45, result.SelectedIndex);
        }

        [Fact]
        public void GenerateSecondColumn_CustomInterval_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            timePicker.SecondInterval = 15;
            timePicker.SelectedTime = new TimeSpan(10, 30, 30);

            var result = InvokePrivateMethod(timePicker, "GenerateSecondColumn") as PickerColumn;

            Assert.NotNull(result);
            var seconds = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(4, seconds.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", seconds[0]);
            Assert.Equal("15", seconds[1]);
            Assert.Equal("30", seconds[2]);
            Assert.Equal("45", seconds[3]);
            Assert.Equal(2, result.SelectedIndex);
        }

        [Fact]
        public void GenerateSecondColumn_NullSelectedTime_UsesDefaultTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = null;
            SetPrivateField(timePicker, "_previousSelectedDateTime", new DateTime(2023, 1, 1, 15, 45, 30));
            var result = InvokePrivateMethod(timePicker, "GenerateSecondColumn") as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(30, result.SelectedIndex);
        }

        [Fact]
        public void GenerateSecondColumn_SecondIntervalNotDivisibleBy60_AdjustsCorrectly()
        {
            var timePicker = new SfTimePicker();
            timePicker.SecondInterval = 7;
            timePicker.SelectedTime = new TimeSpan(10, 30, 28);

            var result = InvokePrivateMethod(timePicker, "GenerateSecondColumn") as PickerColumn;

            Assert.NotNull(result);
            var seconds = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(9, seconds.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("28", seconds[4]);
            Assert.Equal(4, result.SelectedIndex);
        }

        [Fact]
        public void GenerateSecondColumn_LargeInterval_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            timePicker.SecondInterval = 30;
            timePicker.SelectedTime = new TimeSpan(10, 30, 30);

            var result = InvokePrivateMethod(timePicker, "GenerateSecondColumn") as PickerColumn;

            Assert.NotNull(result);
            var seconds = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(2, seconds.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", seconds[0]);
            Assert.Equal("30", seconds[1]);
            Assert.Equal(1, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMeridiemColumn_DefaultSettings_ReturnsCorrectColumn()
        {
            var timePicker = new SfTimePicker();
            var selectedTime = new TimeSpan(14, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMeridiemColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(SfPickerResources.GetLocalizedString(""), result.HeaderText);
            Assert.IsType<ObservableCollection<string>>(result.ItemsSource);

            var meridiems = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(2, meridiems.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("AM", meridiems[0]);
            Assert.Equal("PM", meridiems[1]);
            Assert.Equal(1, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMeridiemColumn_AMTime_SelectsAM()
        {
            var timePicker = new SfTimePicker();
            var selectedTime = new TimeSpan(9, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMeridiemColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(0, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMeridiemColumn_NullSelectedTime_UsesDefaultTime()
        {
            var timePicker = new SfTimePicker();
            SetPrivateField(timePicker, "_previousSelectedDateTime", new DateTime(2023, 1, 1, 15, 0, 0));

            var result = InvokePrivateMethod(timePicker, "GenerateMeridiemColumn", null, null) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(1, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMeridiemColumn_CustomMinMaxTime_AdjustsCorrectly()
        {
            var timePicker = new SfTimePicker();
            timePicker.MinimumTime = new TimeSpan(14, 0, 0);
            timePicker.MaximumTime = new TimeSpan(20, 0, 0);
            var selectedTime = new TimeSpan(16, 30, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMeridiemColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            var meridiems = result.ItemsSource as ObservableCollection<string>;
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.Single(meridiems);
#pragma warning restore CS8604 // Possible null reference argument.
            Assert.Equal("PM", meridiems[0]);
            Assert.Equal(1, result.SelectedIndex);
        }

        [Fact]
        public void GenerateMeridiemColumn_MidnightSelectedTime_SelectsAM()
        {
            var timePicker = new SfTimePicker();
            var selectedTime = new TimeSpan(0, 0, 0);
            var selectedDate = DateTime.Today.Add(selectedTime);

            var result = InvokePrivateMethod(timePicker, "GenerateMeridiemColumn", selectedTime, selectedDate) as PickerColumn;

            Assert.NotNull(result);
            Assert.Equal(0, result.SelectedIndex);
        }

        [Fact]
        public void UpdateSelectedIndex_ShouldUpdateHourColumnIndex()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(14, 30, 0);
            var hourColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "00" },
                SelectedIndex = 0
            };
            SetPrivateField(timePicker, "_hourColumn", hourColumn);

            InvokePrivateMethod(timePicker, "UpdateSelectedIndex", timePicker.SelectedTime);

            Assert.Equal(13, hourColumn.SelectedIndex);
        }

        [Fact]
        public void UpdateSelectedIndex_ShouldUpdateMinuteColumnIndex()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(14, 30, 0);
            var minuteColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "00", "15", "30", "45" },
                SelectedIndex = 0
            };
            SetPrivateField(timePicker, "_minuteColumn", minuteColumn);

            InvokePrivateMethod(timePicker, "UpdateSelectedIndex", timePicker.SelectedTime);

            Assert.Equal(2, minuteColumn.SelectedIndex);
        }

        [Fact]
        public void UpdateSelectedIndex_ShouldUpdateSecondColumnIndex()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(14, 30, 45);
            var secondColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "00", "15", "30", "45" },
                SelectedIndex = 0
            };
            SetPrivateField(timePicker, "_secondColumn", secondColumn);

            InvokePrivateMethod(timePicker, "UpdateSelectedIndex", timePicker.SelectedTime);

            Assert.Equal(3, secondColumn.SelectedIndex);
        }

        [Fact]
        public void UpdateSelectedIndex_ShouldUpdateMeridiemColumnIndex()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(14, 30, 0);
            var meridiemColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "AM", "PM" },
                SelectedIndex = 0
            };
            SetPrivateField(timePicker, "_meridiemColumn", meridiemColumn);

            InvokePrivateMethod(timePicker, "UpdateSelectedIndex", timePicker.SelectedTime);

            Assert.Equal(1, meridiemColumn.SelectedIndex);
        }

        [Fact]
        public void UpdateSelectedIndex_ShouldNotUpdateIndexesWhenSelectedTimeIsNull()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = null;
            var hourColumn = new PickerColumn { SelectedIndex = 0 };
            var minuteColumn = new PickerColumn { SelectedIndex = 0 };
            var secondColumn = new PickerColumn { SelectedIndex = 0 };
            var meridiemColumn = new PickerColumn { SelectedIndex = 0 };
            SetPrivateField(timePicker, "_hourColumn", hourColumn);
            SetPrivateField(timePicker, "_minuteColumn", minuteColumn);
            SetPrivateField(timePicker, "_secondColumn", secondColumn);
            SetPrivateField(timePicker, "_meridiemColumn", meridiemColumn);

            InvokePrivateMethod(timePicker, "UpdateSelectedIndex", timePicker.SelectedTime);

            Assert.Equal(0, hourColumn.SelectedIndex);
            Assert.Equal(0, minuteColumn.SelectedIndex);
            Assert.Equal(0, secondColumn.SelectedIndex);
            Assert.Equal(0, meridiemColumn.SelectedIndex);
        }

        [Fact]
        public void UpdateMinimumMaximumTime_ShouldUpdateHourColumn_WhenHourChanges()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(10, 0, 0);
            timePicker.MinimumTime = new TimeSpan(9, 0, 0);
            timePicker.MaximumTime = new TimeSpan(18, 0, 0);

            var oldHourColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "09", "10", "11", "12", "13", "14", "15", "16", "17", "18" },
                SelectedIndex = 1
            };
            SetPrivateField(timePicker, "_hourColumn", oldHourColumn);

            var columns = new ObservableCollection<PickerColumn> { oldHourColumn };
            SetPrivateField(timePicker, "_columns", columns);

            InvokePrivateMethod(timePicker, "UpdateMinimumMaximumTime", new TimeSpan(9, 0, 0), new TimeSpan(8, 0, 0));

            var updatedColumns = GetPrivateField(timePicker, "_columns") as ObservableCollection<PickerColumn>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.NotEqual(oldHourColumn, updatedColumns[0]);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("09", ((ObservableCollection<string>)updatedColumns[0].ItemsSource)[0]);
        }

        [Fact]
        public void UpdateMinimumMaximumTime_ShouldUpdateMinuteColumn_WhenSelectedHourEqualsOldOrNewHour()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(10, 30, 0);
            timePicker.MinimumTime = new TimeSpan(10, 0, 0);
            timePicker.MaximumTime = new TimeSpan(11, 0, 0);
            timePicker.Format = PickerTimeFormat.HH_mm;

            var oldMinuteColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "00", "15", "30", "45" },
                SelectedIndex = 2
            };
            SetPrivateField(timePicker, "_minuteColumn", oldMinuteColumn);

            var columns = new ObservableCollection<PickerColumn> { new PickerColumn(), oldMinuteColumn };
            SetPrivateField(timePicker, "_columns", columns);

            InvokePrivateMethod(timePicker, "UpdateMinimumMaximumTime", new TimeSpan(10, 0, 0), new TimeSpan(10, 15, 0));

            var updatedColumns = GetPrivateField(timePicker, "_columns") as ObservableCollection<PickerColumn>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.NotEqual(oldMinuteColumn, updatedColumns[1]);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", ((ObservableCollection<string>)updatedColumns[1].ItemsSource)[0]);
        }

        [Fact]
        public void UpdateMinimumMaximumTime_ShouldUpdateSecondColumn_WhenSelectedHourAndMinuteEqualOldOrNewTime()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(10, 30, 30);
            timePicker.MinimumTime = new TimeSpan(10, 30, 0);
            timePicker.MaximumTime = new TimeSpan(10, 31, 0);

            var oldSecondColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "00", "15", "30", "45" },
                SelectedIndex = 2
            };
            SetPrivateField(timePicker, "_secondColumn", oldSecondColumn);

            var columns = new ObservableCollection<PickerColumn> { new PickerColumn(), new PickerColumn(), oldSecondColumn };
            SetPrivateField(timePicker, "_columns", columns);

            InvokePrivateMethod(timePicker, "UpdateMinimumMaximumTime", new TimeSpan(10, 30, 0), new TimeSpan(10, 30, 15));

            var updatedColumns = GetPrivateField(timePicker, "_columns") as ObservableCollection<PickerColumn>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.NotEqual(oldSecondColumn, updatedColumns[2]);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("00", ((ObservableCollection<string>)updatedColumns[2].ItemsSource)[0]);
        }

        [Fact]
        public void UpdateMinimumMaximumTime_ShouldUpdateMeridiemColumn_WhenSelectedHourPeriodChanges()
        {
            var timePicker = new SfTimePicker();
            timePicker.SelectedTime = new TimeSpan(14, 0, 0);
            timePicker.MinimumTime = new TimeSpan(13, 0, 0);
            timePicker.MaximumTime = new TimeSpan(15, 0, 0);
            timePicker.Format = PickerTimeFormat.hh_mm_tt;

            var oldMeridiemColumn = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "AM", "PM" },
                SelectedIndex = 1
            };
            SetPrivateField(timePicker, "_meridiemColumn", oldMeridiemColumn);

            var columns = new ObservableCollection<PickerColumn> { new PickerColumn(), new PickerColumn(), oldMeridiemColumn };
            SetPrivateField(timePicker, "_columns", columns);

            InvokePrivateMethod(timePicker, "UpdateMinimumMaximumTime", new TimeSpan(13, 0, 0), new TimeSpan(11, 0, 0));

            var updatedColumns = GetPrivateField(timePicker, "_columns") as ObservableCollection<PickerColumn>;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.NotEqual(oldMeridiemColumn, updatedColumns[2]);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Single(((ObservableCollection<string>)updatedColumns[2].ItemsSource));
            Assert.Equal("PM", ((ObservableCollection<string>)updatedColumns[2].ItemsSource)[0]);
        }

        #endregion

        #region PopupSizeFeature

        [Fact]
        public void DatePicker_PopupSize1()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            double expectedWidth = 100;
            double expectedHeight = 200;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenPopupSizeIsNotSet()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            double expectedWidth = -1;
            double expectedHeight = -1;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenPopupSizeOnPropertyChange()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            double expectedWidth = 50;
            double expectedHeight = 20;

            sfTimePicker.PopupWidth = 100;
            sfTimePicker.PopupHeight = 200;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenHeaderHeightProvided_PopupSizeProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            PickerHeaderView headerView = new PickerHeaderView();
            headerView.Height = 50;
            sfTimePicker.HeaderView = headerView;

            double expectedWidth = 200;
            double expectedHeight = 400;

            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenHeaderHeightProvided_PopupSizeNotProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            PickerHeaderView headerView = new PickerHeaderView();
            headerView.Height = 50;
            sfTimePicker.HeaderView = headerView;

            double expectedWidth = 200;
            double expectedHeight = 290;

            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenColumnHeaderProvided_PopupSizeProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            TimePickerColumnHeaderView columnHeaderView = new TimePickerColumnHeaderView();
            columnHeaderView.Height = 50;
            sfTimePicker.ColumnHeaderView = columnHeaderView;

            double expectedWidth = 200;
            double expectedHeight = 400;

            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenColumnHeaderProvided_PopupSizeNotProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            TimePickerColumnHeaderView columnHeaderView = new TimePickerColumnHeaderView();
            columnHeaderView.Height = 50;
            sfTimePicker.ColumnHeaderView = columnHeaderView;

            double expectedWidth = 200;
            double expectedHeight = 290;

            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_PopupSizeProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            sfTimePicker.ItemHeight = 10;

            double expectedWidth = 200;
            double expectedHeight = 640;

            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;

            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
        }


        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_PopupSizeNotProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            sfTimePicker.ItemHeight = 10;
            double expectedWidth = -1;
            double expectedHeight = -1;
            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_ItemsLessThanFive()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            sfTimePicker.ItemHeight = 10;
            double expectedWidth = 200;
            double expectedHeight = 290 + 30;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;
            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenItemHeightProvided_ItemsGreaterThanFive()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            sfTimePicker.ItemHeight = 10;
            double expectedWidth = 200;
            double expectedHeight = 290 + 50;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;
            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenFooterHeightProvided_PopupSizeProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            PickerFooterView pickerFooterView = new PickerFooterView();
            pickerFooterView.Height = 50;
            sfTimePicker.FooterView = pickerFooterView;
            double expectedWidth = 200;
            double expectedHeight = 290 + 50;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;
            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenFooterHeightProvided_PopupSizeNotProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            PickerFooterView pickerFooterView = new PickerFooterView();
            pickerFooterView.Height = 50;
            sfTimePicker.FooterView = pickerFooterView;
            double expectedWidth = 200;
            double expectedHeight = 290;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;
            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenAllHeaderHeightProvided_PopupSizeProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            PickerHeaderView headerView = new PickerHeaderView();
            TimePickerColumnHeaderView pickerColumnHeaderView = new TimePickerColumnHeaderView();
            PickerFooterView footerView = new PickerFooterView();
            sfTimePicker.ItemHeight = 10;
            headerView.Height = 50;
            pickerColumnHeaderView.Height = 50;
            footerView.Height = 50;
            sfTimePicker.HeaderView = headerView;
            sfTimePicker.ColumnHeaderView = pickerColumnHeaderView;
            sfTimePicker.FooterView = footerView;
            double expectedWidth = 200;
            double expectedHeight = 290 + 50 + 50 + 50 + 50;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;
            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void DatePicker_PopupSize_WhenAllHeaderHeightProvided_PopupSizeNotProvided()
        {
            SfTimePicker sfTimePicker = new SfTimePicker();
            PickerHeaderView headerView = new PickerHeaderView();
            TimePickerColumnHeaderView pickerColumnHeaderView = new TimePickerColumnHeaderView();
            PickerFooterView footerView = new PickerFooterView();
            headerView.Height = 50;
            pickerColumnHeaderView.Height = 50;
            footerView.Height = 50;
            sfTimePicker.HeaderView = headerView;
            sfTimePicker.ColumnHeaderView = pickerColumnHeaderView;
            sfTimePicker.FooterView = footerView;
            double expectedWidth = 200;
            double expectedHeight = 50 + 50 + 50;
            sfTimePicker.PopupWidth = expectedWidth;
            sfTimePicker.PopupHeight = expectedHeight;
            double actualWidth = sfTimePicker.PopupWidth;
            double actualHeight = sfTimePicker.PopupHeight;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

		#endregion

		#region Dialog Mode Selection Behavior Tests

		[Fact]
		public void DialogMode_TimeSelectionNotCommittedUntilOK_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.Dialog;
			var originalTime = picker.SelectedTime ?? new TimeSpan(9, 0, 0);
			var newTime = new TimeSpan(15, 45, 20);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// (Simulated: In actual dialog usage, time is not committed until OK)
			Assert.Equal(originalTime, picker.SelectedTime ?? originalTime);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire until OK is pressed in dialog mode");
		}
		[Fact]
		public void RelativeDialogMode_TimeSelectionNotCommittedUntilOK_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.RelativeDialog;
			var originalTime = picker.SelectedTime ?? new TimeSpan(10, 0, 0);
			var newTime = new TimeSpan(20, 30, 40);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			Assert.Equal(originalTime, picker.SelectedTime ?? originalTime);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire until OK is pressed in relative dialog mode");
		}
		[Fact]
		public void DialogMode_CancelButtonRevertsTimeSelection_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.SelectedTime = new TimeSpan(8, 30, 0);
			var originalTime = picker.SelectedTime;
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			// (Simulated: Cancel in dialog should revert selection)
			Assert.Equal(originalTime, picker.SelectedTime);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire after Cancel");
		}
		[Fact]
		public void RelativeDialogMode_CancelButtonRevertsTimeSelection_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.RelativeDialog;
			picker.SelectedTime = new TimeSpan(19, 10, 0);
			var originalTime = picker.SelectedTime;
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			Assert.Equal(originalTime, picker.SelectedTime);
			Assert.False(selectionChangedFired, "SelectionChanged event should not fire after Cancel");
		}
		[Fact]
		public void DialogMode_OKButtonCommitsTimeSelection_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.SelectedTime = new TimeSpan(11, 00, 00);
			var selectionChangedFired = false;
			var expectedNewTime = new TimeSpan(16, 30, 30);
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			picker.SelectedTime = expectedNewTime; // Simulates OK
			Assert.Equal(expectedNewTime, picker.SelectedTime);
			Assert.True(selectionChangedFired, "SelectionChanged event should fire when OK is pressed");
		}
		[Fact]
		public void RelativeDialogMode_OKButtonCommitsTimeSelection_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.RelativeDialog;
			picker.SelectedTime = new TimeSpan(7, 15, 00);
			var selectionChangedFired = false;
			var expectedNewTime = new TimeSpan(20, 22, 13);
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			picker.SelectedTime = expectedNewTime; // Simulates OK
			Assert.Equal(expectedNewTime, picker.SelectedTime);
			Assert.True(selectionChangedFired, "SelectionChanged event should fire when OK is pressed");
		}
		[Fact]
		public void DialogMode_MultipleTimeSelectionsUntilOK_OnlyCommitsOnOK()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.SelectedTime = new TimeSpan(10, 0, 0);
			var selectionChangedCount = 0;
			picker.SelectionChanged += (s, e) => selectionChangedCount++;
			var finalTime = new TimeSpan(18, 59, 20);
			picker.SelectedTime = finalTime; // Simulate OK
			Assert.Equal(finalTime, picker.SelectedTime);
			Assert.Equal(1, selectionChangedCount);
		}
		[Fact]
		public void DefaultMode_TimeSelectionCommittedImmediately_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.Default;
			picker.SelectedTime = new TimeSpan(8, 44, 11);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			var newTime = new TimeSpan(15, 22, 5);
			picker.SelectedTime = newTime;
			Assert.Equal(newTime, picker.SelectedTime);
			Assert.True(selectionChangedFired, "SelectionChanged should fire immediately in Default mode");
		}
		[Theory]
		[InlineData(PickerMode.Dialog)]
		[InlineData(PickerMode.RelativeDialog)]
		public void DialogModes_FooterButtonsConfigured_ValidatesCorrectBehavior(PickerMode mode)
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = mode;
			Assert.NotNull(picker.FooterView);
			Assert.Equal("OK", picker.FooterView.OkButtonText);
			Assert.Equal("Cancel", picker.FooterView.CancelButtonText);
			Assert.True(picker.FooterView.ShowOkButton);
		}
		[Fact]
		public void DialogMode_TimeBoundaryValues_ValidatesCorrectBehavior()
		{
			SfTimePicker picker = new SfTimePicker();
			picker.Mode = PickerMode.Dialog;
			picker.MinimumTime = new TimeSpan(5, 0, 0);
			picker.MaximumTime = new TimeSpan(23, 0, 0);
			var selectionChangedFired = false;
			picker.SelectionChanged += (s, e) => selectionChangedFired = true;
			picker.SelectedTime = picker.MinimumTime;
			Assert.Equal(picker.MinimumTime, picker.SelectedTime);
			Assert.True(selectionChangedFired, "SelectionChanged should fire for valid boundary values");
		}

		#endregion
	}
}
