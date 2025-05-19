using Syncfusion.Maui.Toolkit.Picker;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class TimePickerUIAllPropertiesUnitTests : PickerBaseUnitTest
    {
        [Fact]
        public void MAUI_SfTimePicker_Code_Behind1()
        {
            SfTimePicker timepicker = new SfTimePicker();
            DateTime today = DateTime.Now;
            timepicker.SelectedTime = new TimeSpan(today.Hour, today.Minute, today.Second); 

            ContentPage contentPage = new ContentPage();
            contentPage.Content = timepicker;

            Assert.Equal(timepicker, contentPage.Content);
        }

        [Fact]
        public void MAUI_SfTimePicker_ColumnDividerColor()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.ColumnDividerColor = Colors.Yellow;  

            Assert.Equal(Colors.Yellow, timepicker.ColumnDividerColor);
        }

        [Fact]
        public void MAUI_SfTimePicker_ColumnHeader_DividerColor()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.ColumnHeaderView.DividerColor = Colors.Yellow;

            Assert.Equal(Colors.Yellow, timepicker.ColumnHeaderView.DividerColor);
        }

        [Fact]
        public void MAUI_SfTimePicker_ContentPage1()
        {
            SfTimePicker timepicker = new SfTimePicker();

            ContentPage contentPage = new ContentPage();
            contentPage.Content = timepicker;
            Assert.Equal(timepicker, contentPage.Content);
        }

        [Fact]
        public void MAUI_SfTimePicker_DisableColumnHeader()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.ColumnHeaderView.Height = 0;

            Assert.Equal(0, timepicker.ColumnHeaderView.Height);
        }

        [Fact]
        public void MAUI_SfTimePicker_DisableHeader()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.HeaderView.Height = 0;

            Assert.Equal(0, timepicker.HeaderView.Height);
        }


        [Fact]
        public void MAUI_SfTimePicker_Grid1()
        {
            SfTimePicker timepicker = new SfTimePicker();

            Grid grid = new Grid();
            grid.Children.Add(timepicker);

            Assert.Equal(timepicker, grid.Children[0]);
        }

        [Theory]
        [InlineData(PickerRelativePosition.AlignBottom)]
        [InlineData(PickerRelativePosition.AlignBottomLeft)]
        [InlineData(PickerRelativePosition.AlignBottomRight)]
        [InlineData(PickerRelativePosition.AlignToLeftOf)]
        [InlineData(PickerRelativePosition.AlignTop)]
        [InlineData(PickerRelativePosition.AlignTopLeft)]
        [InlineData(PickerRelativePosition.AlignTopRight)]
        [InlineData(PickerRelativePosition.AlignToRightOf)]
        public void MAUI_SfTimePicker_RelativeDialogMode_001(PickerRelativePosition expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();

            timepicker.IsOpen = true;
            timepicker.Mode = PickerMode.RelativeDialog;
            timepicker.RelativePosition = expectedValue;

            PickerRelativePosition actualValue = timepicker.RelativePosition;

            Assert.True(timepicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, timepicker.Mode);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfTimePicker_ScrollingView()
        {
            SfTimePicker timepicker = new SfTimePicker();

            ScrollView scrollView = new ScrollView();
            scrollView.Content = timepicker;
            Assert.Equal(timepicker, scrollView.Content);
        }

        [Fact]
        public void MAUI_SfTimePicker_ShowColumnHeader()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.ColumnHeaderView.Height = 0;
            timepicker.ColumnHeaderView.HourHeaderText = "Hour";
            timepicker.ColumnHeaderView.MinuteHeaderText = "Minute";
            timepicker.ColumnHeaderView.SecondHeaderText = "Second";
            timepicker.ColumnHeaderView.MeridiemHeaderText = "Meridiem";

            Assert.Equal(0, timepicker.ColumnHeaderView.Height);
            Assert.Equal("Hour", timepicker.ColumnHeaderView.HourHeaderText);
            Assert.Equal("Minute", timepicker.ColumnHeaderView.MinuteHeaderText);
            Assert.Equal("Second", timepicker.ColumnHeaderView.SecondHeaderText);
            Assert.Equal("Meridiem", timepicker.ColumnHeaderView.MeridiemHeaderText);
        }

        [Fact]
        public void MAUI_SfTimePicker_ShowFooter()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.FooterView.Height = 0;
           
            Assert.Equal(0, timepicker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfTimePicker_ShowHeader()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.HeaderView.Height = 0;
            timepicker.HeaderView.Text = "Time Picker";

            Assert.Equal(0, timepicker.HeaderView.Height);
            Assert.Equal("Time Picker", timepicker.HeaderView.Text);
        }

        [Fact]
        public void MAUI_SfTimePicker_StackLayout1()
        {
            SfTimePicker timepicker = new SfTimePicker();

            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(timepicker);

            Assert.Equal(timepicker, stackLayout.Children[0]);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_1()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
        }

        [Theory]
        [InlineData(PickerTimeFormat.Default)]
        [InlineData(PickerTimeFormat.HH_mm)]
        [InlineData(PickerTimeFormat.HH_mm_ss)]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt)]
        [InlineData(PickerTimeFormat.hh_mm_tt)]
        [InlineData(PickerTimeFormat.hh_tt)]
        [InlineData(PickerTimeFormat.H_mm)]
        [InlineData(PickerTimeFormat.H_mm_ss)]
        [InlineData(PickerTimeFormat.h_mm_ss_tt)]
        [InlineData(PickerTimeFormat.h_mm_tt)]
        public void MAUI_SfTimePicker_MinMaxSupport_10(PickerTimeFormat expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.Format = expectedValue;
            PickerTimeFormat acutalValue = timepicker.Format;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(expectedValue, acutalValue);
        }

        [Theory]
        [InlineData(PickerTextDisplayMode.Default)]
        [InlineData(PickerTextDisplayMode.FadeAndShrink)]
        [InlineData(PickerTextDisplayMode.Shrink)]
        [InlineData(PickerTextDisplayMode.Fade)]
        public void MAUI_SfTimePicker_MinMaxSupport_11(PickerTextDisplayMode expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.TextDisplayMode = expectedValue;
            PickerTextDisplayMode acutalValue = timepicker.TextDisplayMode;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(expectedValue, acutalValue);
        }

        [Theory]
        [InlineData(PickerTextDisplayMode.Default)]
        [InlineData(PickerTextDisplayMode.FadeAndShrink)]
        [InlineData(PickerTextDisplayMode.Shrink)]
        [InlineData(PickerTextDisplayMode.Fade)]
        public void MAUI_SfTimePicker_MinMaxSupport_12(PickerTextDisplayMode expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.TextDisplayMode = expectedValue;
            PickerTextDisplayMode acutalValue = timepicker.TextDisplayMode;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(expectedValue, acutalValue);
        }

        [Theory]
        [InlineData(PickerTextDisplayMode.Default)]
        [InlineData(PickerTextDisplayMode.FadeAndShrink)]
        [InlineData(PickerTextDisplayMode.Shrink)]
        [InlineData(PickerTextDisplayMode.Fade)]
        public void MAUI_SfTimePicker_MinMaxSupport_13(PickerTextDisplayMode expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.TextDisplayMode = expectedValue;
            PickerTextDisplayMode acutalValue = timepicker.TextDisplayMode;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(expectedValue, acutalValue);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_14()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.HourInterval = 2;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(2, timepicker.HourInterval);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_15()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.MinuteInterval = 2;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(2, timepicker.MinuteInterval);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_16()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.SecondInterval = 2;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(2, timepicker.SecondInterval);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_17()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.Mode = PickerMode.Dialog;
            timepicker.IsOpen = true;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.True(timepicker.IsOpen);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_18()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.Mode = PickerMode.Dialog;
            timepicker.IsOpen = true;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.True(timepicker.IsOpen);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_19()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.Mode = PickerMode.Dialog;
            timepicker.IsOpen = true;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.True(timepicker.IsOpen);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_2()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
        }

        [Theory]
        [InlineData(PickerTimeFormat.Default)]
        [InlineData(PickerTimeFormat.HH_mm)]
        [InlineData(PickerTimeFormat.HH_mm_ss)]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt)]
        [InlineData(PickerTimeFormat.hh_mm_tt)]
        [InlineData(PickerTimeFormat.hh_tt)]
        [InlineData(PickerTimeFormat.H_mm)]
        [InlineData(PickerTimeFormat.H_mm_ss)]
        [InlineData(PickerTimeFormat.h_mm_ss_tt)]
        [InlineData(PickerTimeFormat.h_mm_tt)]
        public void MAUI_SfTimePicker_MinMaxSupport_20(PickerTimeFormat expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.Mode = PickerMode.Dialog;
            timepicker.IsOpen = true;
            timepicker.Format = expectedValue;
            PickerTimeFormat acutalValue = timepicker.Format;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(expectedValue, acutalValue);
        }

        [Theory]
        [InlineData(PickerTimeFormat.Default)]
        [InlineData(PickerTimeFormat.HH_mm)]
        [InlineData(PickerTimeFormat.HH_mm_ss)]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt)]
        [InlineData(PickerTimeFormat.hh_mm_tt)]
        [InlineData(PickerTimeFormat.hh_tt)]
        [InlineData(PickerTimeFormat.H_mm)]
        [InlineData(PickerTimeFormat.H_mm_ss)]
        [InlineData(PickerTimeFormat.h_mm_ss_tt)]
        [InlineData(PickerTimeFormat.h_mm_tt)]
        public void MAUI_SfTimePicker_MinMaxSupport_21(PickerTimeFormat expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.Mode = PickerMode.Dialog;
            timepicker.IsOpen = true;
            timepicker.Format = expectedValue;
            PickerTimeFormat acutalValue = timepicker.Format;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(expectedValue, acutalValue);
        }

        [Theory]
        [InlineData(PickerTimeFormat.Default)]
        [InlineData(PickerTimeFormat.HH_mm)]
        [InlineData(PickerTimeFormat.HH_mm_ss)]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt)]
        [InlineData(PickerTimeFormat.hh_mm_tt)]
        [InlineData(PickerTimeFormat.hh_tt)]
        [InlineData(PickerTimeFormat.H_mm)]
        [InlineData(PickerTimeFormat.H_mm_ss)]
        [InlineData(PickerTimeFormat.h_mm_ss_tt)]
        [InlineData(PickerTimeFormat.h_mm_tt)]
        public void MAUI_SfTimePicker_MinMaxSupport_22(PickerTimeFormat expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.Mode = PickerMode.Dialog;
            timepicker.IsOpen = true;
            timepicker.Format = expectedValue;
            PickerTimeFormat acutalValue = timepicker.Format;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(expectedValue, acutalValue);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_23()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(18, 50, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.IsOpen = true;
            timepicker.Mode = PickerMode.Dialog;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MinimumTime);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_24()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(2, 30, 0);
            timepicker.MaximumTime = new TimeSpan(9, 30, 0);
            timepicker.IsOpen = true;
            timepicker.Mode = PickerMode.Dialog;
            timepicker.Format = PickerTimeFormat.h_mm_ss_tt;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(2, 30, 0), timepicker.MinimumTime);
            Assert.Equal(PickerTimeFormat.h_mm_ss_tt, timepicker.Format);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_25()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(13, 50, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.IsOpen = true;
            timepicker.Mode = PickerMode.Dialog;
            timepicker.Format = PickerTimeFormat.h_mm_ss_tt;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(13, 50, 0), timepicker.MinimumTime);
            Assert.Equal(PickerTimeFormat.h_mm_ss_tt, timepicker.Format);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_26()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(18, 50, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.IsOpen = true;
            timepicker.Mode = PickerMode.Dialog;
             timepicker.HourInterval = 2;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MinimumTime);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.Equal(2, timepicker.HourInterval);   
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_27()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(18, 50, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.IsOpen = true;
            timepicker.Mode = PickerMode.Dialog;
            timepicker.MinuteInterval = 2;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MinimumTime);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.Equal(2, timepicker.MinuteInterval);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_28()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(18, 50, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.IsOpen = true;
            timepicker.Mode = PickerMode.Dialog;
            timepicker.SecondInterval = 2;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MinimumTime);
            Assert.True(timepicker.IsOpen);
            Assert.Equal(PickerMode.Dialog, timepicker.Mode);
            Assert.Equal(2, timepicker.SecondInterval);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_3()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(18, 50, 0);
            timepicker.MaximumTime = new TimeSpan(09, 20, 0);

            Assert.Equal(new TimeSpan(09, 20, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MinimumTime);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_4()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(18, 50, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MinimumTime);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_5()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(2, 30, 0);
            timepicker.MaximumTime = new TimeSpan(9, 30, 0);
            timepicker.Format = PickerTimeFormat.h_mm_ss_tt;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(2, 30, 0), timepicker.MinimumTime);
            Assert.Equal(PickerTimeFormat.h_mm_ss_tt, timepicker.Format);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_6()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(18, 30, 0);
            timepicker.MaximumTime = new TimeSpan(20, 30, 0);
            timepicker.Format = PickerTimeFormat.h_mm_ss_tt;

            Assert.Equal(new TimeSpan(20, 30, 0), timepicker.MaximumTime);
            Assert.Equal(new TimeSpan(18, 30, 0), timepicker.MinimumTime);
            Assert.Equal(PickerTimeFormat.h_mm_ss_tt, timepicker.Format);
        }

        [Fact]
        public void MAUI_SfTimePicker_MinMaxSupport_7()
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
        }


        [Theory]
        [InlineData(PickerTimeFormat.Default)]
        [InlineData(PickerTimeFormat.HH_mm)]
        [InlineData(PickerTimeFormat.HH_mm_ss)]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt)]
        [InlineData(PickerTimeFormat.hh_mm_tt)]
        [InlineData(PickerTimeFormat.hh_tt)]
        [InlineData(PickerTimeFormat.H_mm)]
        [InlineData(PickerTimeFormat.H_mm_ss)]
        [InlineData(PickerTimeFormat.h_mm_ss_tt)]
        [InlineData(PickerTimeFormat.h_mm_tt)]
        public void MAUI_SfTimePicker_MinMaxSupport_8(PickerTimeFormat expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MinimumTime = new TimeSpan(9, 30, 0);
            timepicker.Format = expectedValue;
            PickerTimeFormat acutalValue = timepicker.Format;

            Assert.Equal(new TimeSpan(9, 30, 0), timepicker.MinimumTime);
            Assert.Equal(expectedValue, acutalValue);
        }


        [Theory]
        [InlineData(PickerTimeFormat.Default)]
        [InlineData(PickerTimeFormat.HH_mm)]
        [InlineData(PickerTimeFormat.HH_mm_ss)]
        [InlineData(PickerTimeFormat.hh_mm_ss_tt)]
        [InlineData(PickerTimeFormat.hh_mm_tt)]
        [InlineData(PickerTimeFormat.hh_tt)]
        [InlineData(PickerTimeFormat.H_mm)]
        [InlineData(PickerTimeFormat.H_mm_ss)]
        [InlineData(PickerTimeFormat.h_mm_ss_tt)]
        [InlineData(PickerTimeFormat.h_mm_tt)]
        public void MAUI_SfTimePicker_MinMaxSupport_9(PickerTimeFormat expectedValue)
        {
            SfTimePicker timepicker = new SfTimePicker();
            timepicker.MaximumTime = new TimeSpan(18, 50, 0);
            timepicker.Format = expectedValue;
            PickerTimeFormat acutalValue = timepicker.Format;

            Assert.Equal(new TimeSpan(18, 50, 0), timepicker.MaximumTime);
            Assert.Equal(expectedValue, acutalValue);
        }
    }
}
