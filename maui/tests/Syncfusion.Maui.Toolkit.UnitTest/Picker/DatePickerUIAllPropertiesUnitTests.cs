using Syncfusion.Maui.Toolkit.Picker;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class DatePickerUIAllPropertiesUnitTests : PickerBaseUnitTest
    {
        [Fact]
        public void MAUI_SfDatePicker_CodeBehind()
        {
            SfDatePicker datepicker = new SfDatePicker();
            
            ContentPage contentPage = new ContentPage();
            contentPage.Content = datepicker;

            Assert.Equal(datepicker, contentPage.Content);
        }

        [Fact]
        public void MAUI_SfDatePicker_ContentPage1()
        {
            SfDatePicker datePicker = new SfDatePicker();
            
            ContentPage contentPage = new ContentPage();
            contentPage.Content = datePicker;

            Assert.Equal(datePicker, contentPage.Content);
        }

        [Fact]
        public void MAUI_SfDatePicker_FooterBackgroundColor()
        {
            SfDatePicker datePicker = new SfDatePicker();

            datePicker.FooterView.Background = Colors.Yellow;

            Assert.Equal(Colors.Yellow, datePicker.FooterView.Background);
        }


        [Fact]
        public void MAUI_SfDatePicker_Grid1()
        {
            SfDatePicker datePicker = new SfDatePicker();

            Grid grid = new Grid();
            grid.Children.Add(datePicker);

            Assert.Equal(datePicker, grid.Children[0]);
        }

        [Fact]
        public void MAUI_SfDatePicker_HeaderBackgroundColor()
        {
            SfDatePicker datePicker = new SfDatePicker();

            datePicker.HeaderView.Background = Colors.Yellow;

            Assert.Equal(Colors.Yellow, datePicker.HeaderView.Background);
        }

        [Fact]
        public void MAUI_SfDatePicker_HeaderTextColor()
        {
            SfDatePicker datePicker = new SfDatePicker();

            datePicker.HeaderView.TextStyle.TextColor = Colors.Yellow;

            Assert.Equal(Colors.Yellow, datePicker.HeaderView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfDatePicker_Padding()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.SelectionView.Padding = new Thickness(10, 5, 10, 5);

            Assert.Equal(new Thickness(10, 5, 10, 5), datePicker.SelectionView.Padding);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_001()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignBottom;

            Assert.Equal(PickerRelativePosition.AlignBottom, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_002()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignBottomLeft;

            Assert.Equal(PickerRelativePosition.AlignBottomLeft, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_003()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignBottomRight;

            Assert.Equal(PickerRelativePosition.AlignBottomRight, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_004()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignToLeftOf;

            Assert.Equal(PickerRelativePosition.AlignToLeftOf, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_005()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignTop;

            Assert.Equal(PickerRelativePosition.AlignTop, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_006()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignTopLeft;

            Assert.Equal(PickerRelativePosition.AlignTopLeft, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_007()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignTopRight;

            Assert.Equal(PickerRelativePosition.AlignTopRight, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_RelativeDialogMode_008()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.IsOpen = true;
            datePicker.Mode = PickerMode.RelativeDialog;
            datePicker.RelativePosition = PickerRelativePosition.AlignToRightOf;

            Assert.Equal(PickerRelativePosition.AlignToRightOf, datePicker.RelativePosition);
            Assert.True(datePicker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, datePicker.Mode);
        }

        [Fact]
        public void MAUI_SfDatePicker_ScrollingView()
        {
            SfDatePicker datePicker = new SfDatePicker();

            ScrollView scrollView = new ScrollView();
            scrollView.Content = datePicker;

            Assert.Equal(datePicker, scrollView.Content);
        }

        [Fact]
        public void MAUI_SfDatePicker_SelectedBackgroundColor()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.SelectionView.Background = Colors.Yellow;

            Assert.Equal(Colors.Yellow, datePicker.SelectionView.Background);
        }

        [Fact]
        public void MAUI_SfDatePicker_ShowColumnHeader()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.ColumnHeaderView.Height = 40;
            datePicker.ColumnHeaderView.DayHeaderText = "Day";
            datePicker.ColumnHeaderView.MonthHeaderText = "Month";
            datePicker.ColumnHeaderView.YearHeaderText = "Year";

            Assert.Equal(40, datePicker.ColumnHeaderView.Height);
            Assert.Equal("Day", datePicker.ColumnHeaderView.DayHeaderText);
            Assert.Equal("Month", datePicker.ColumnHeaderView.MonthHeaderText);
            Assert.Equal("Year", datePicker.ColumnHeaderView.YearHeaderText);
        }

        [Fact]
        public void MAUI_SfDatePicker_ShowFooter()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.FooterView.Height = 40;
            datePicker.FooterView.OkButtonText = "OK";
            datePicker.FooterView.CancelButtonText = "Cancel";

            Assert.NotNull(datePicker.FooterView);
        }

        [Fact]
        public void MAUI_SfDatePicker_ShowHeader()
        {
            SfDatePicker datePicker = new SfDatePicker();
            datePicker.HeaderView.Height = 40;
            datePicker.HeaderView.Text = "Date Picker";

            Assert.Equal(40, datePicker.HeaderView.Height);
            Assert.Equal("Date Picker", datePicker.HeaderView.Text);
        }

        [Fact]
        public void MAUI_SfDatePicker_StackLayout1()
        {
            SfDatePicker datePicker = new SfDatePicker();

            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(datePicker);

            Assert.Equal(datePicker, stackLayout.Children[0]);
        }

        [Fact]
        public void MAUI_SfDatePicker_TextColor()
        {
            SfDatePicker datePicker = new SfDatePicker();

            datePicker.TextStyle.TextColor = Colors.Yellow;

            Assert.Equal(Colors.Yellow, datePicker.TextStyle.TextColor);
        }
    }
}
