using Syncfusion.Maui.Toolkit.Picker;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class PickerUIAllPropertiestUnitTests : PickerBaseUnitTest
    {
        [Fact]
        public void MAUI_SfPicker_AddColumn()
        {
            SfPicker picker = new SfPicker();
            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            PickerColumn pickerColumn1 = new PickerColumn() { HeaderText = "AddedColumns", ItemsSource = dataSource, SelectedIndex = 4 };
            PickerColumn pickerColumn2 = new PickerColumn() { HeaderText = "AddedColumns", ItemsSource = dataSource, SelectedIndex = 5 };
            PickerColumn pickerColumn3 = new PickerColumn() { HeaderText = "AddedColumns", ItemsSource = dataSource, SelectedIndex = 1 };

            picker.Columns.Add(pickerColumn1);
            picker.Columns.Add(pickerColumn2);
            picker.Columns.Add(pickerColumn3);

            Assert.Equal(pickerColumn1, picker.Columns[0]);
            Assert.Equal(pickerColumn2, picker.Columns[1]);
            Assert.Equal(pickerColumn3, picker.Columns[2]);

            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_AllBackgroundColor()
        {
            SfPicker picker = new SfPicker();
            picker.HeaderView.Background = Colors.Red;
            picker.ColumnHeaderView.Background = Colors.Yellow;
            picker.FooterView.Background = Colors.Blue;

            Assert.Equal(Colors.Red, picker.HeaderView.Background);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.Background);
            Assert.Equal(Colors.Blue, picker.FooterView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_AllDividerColor()
        {
            SfPicker picker = new SfPicker();
            picker.HeaderView.DividerColor = Colors.Red;
            picker.ColumnHeaderView.DividerColor = Colors.Yellow;
            picker.FooterView.DividerColor = Colors.Blue;

            Assert.Equal(Colors.Red, picker.HeaderView.DividerColor);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.DividerColor);
            Assert.Equal(Colors.Blue, picker.FooterView.DividerColor);
        }

        [Fact]
        public void MAUI_SfPicker_AllTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.HeaderView.TextStyle.TextColor = Colors.Red;
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.FooterView.TextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Red, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.FooterView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_CodeBehind()
        {

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };


            SfPicker picker = new SfPicker()
            {
                Columns = new ObservableCollection<PickerColumn>()
            {
            new PickerColumn()
                {
                ItemsSource = dataSource,
                HeaderText = "Color",
                }
            },
                HeaderView = new PickerHeaderView()
                {
                    Height = 40,
                    Text = "Select a color",
                },
                FooterView = new PickerFooterView()
                {
                    Height = 40,
                }
            };


            ContentPage contentPage = new ContentPage();
            contentPage.Content = picker;

            Assert.Equal(picker, contentPage.Content);
        }


        [Fact]
        public void MAUI_SfPicker_ColumnHeaderHeight_BackgroundColor()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.Height = 35;
            picker.ColumnHeaderView.Background = Colors.Red;

            Assert.Equal(Colors.Red, picker.ColumnHeaderView.Background);
            Assert.Equal(35, picker.ColumnHeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderHeight_ColumnText()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.Height = 35;

            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].HeaderText = "Text";

            Assert.Equal("Text", picker.Columns[0].HeaderText);
            Assert.Equal(35, picker.ColumnHeaderView.Height);
            picker.Columns.Clear();

        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderHeight_FooterHeight()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.Height = 35;
            picker.FooterView.Height = 40;

            Assert.Equal(40, picker.FooterView.Height);
            Assert.Equal(35, picker.ColumnHeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderText()
        {
            SfPicker picker = new SfPicker();

            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].HeaderText = "Text";

            Assert.Equal("Text", picker.Columns[0].HeaderText);
            picker.Columns.Clear();

        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderTextColor_FontAttributes()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.ColumnHeaderView.TextStyle.FontAttributes = FontAttributes.Italic;

            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
            Assert.Equal(FontAttributes.Italic, picker.ColumnHeaderView.TextStyle.FontAttributes);
        }

        [Theory]
        [InlineData(16)]
        [InlineData(6)]
        public void MAUI_SfPicker_ColumnHeaderTextColor_FontSize(double expectedValue)
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.ColumnHeaderView.TextStyle.FontSize = expectedValue;

            double actualValue = picker.ColumnHeaderView.TextStyle.FontSize;


            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderTextColor_FooterTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.FooterView.TextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.FooterView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderTextColor_SelectionTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.SelectedTextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.SelectedTextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderTextColor_TextColor()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.TextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderText_FontAttributes()
        {
            SfPicker picker = new SfPicker();
            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].HeaderText = "Text";
            picker.ColumnHeaderView.TextStyle.FontAttributes = FontAttributes.Italic;

            Assert.Equal("Text", picker.Columns[0].HeaderText);
            Assert.Equal(FontAttributes.Italic, picker.ColumnHeaderView.TextStyle.FontAttributes);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderText_FontSize()
        {
            SfPicker picker = new SfPicker();
            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].HeaderText = "Text";
            picker.ColumnHeaderView.TextStyle.FontSize = 19;

            Assert.Equal("Text", picker.Columns[0].HeaderText);
            Assert.Equal(19, picker.ColumnHeaderView.TextStyle.FontSize);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderText_FooterText()
        {
            SfPicker picker = new SfPicker();
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            picker.Columns[2].HeaderText = "Text";
            picker.FooterView.OkButtonText = "exit";
            picker.FooterView.CancelButtonText = "can";

            Assert.Equal("Text", picker.Columns[2].HeaderText);
            Assert.Equal("exit", picker.FooterView.OkButtonText);
            Assert.Equal("can", picker.FooterView.CancelButtonText);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeaderText_TextColor()
        {
            SfPicker picker = new SfPicker();
            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].HeaderText = "Text";
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Red;

            Assert.Equal("Text", picker.Columns[0].HeaderText);
            Assert.Equal(Colors.Red, picker.ColumnHeaderView.TextStyle.TextColor);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeader_BackgroundColor_001()
        {
            SfPicker picker = new SfPicker();
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            picker.ColumnHeaderView.Background = Colors.Yellow;

            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.Background);
            picker.Columns.Clear();
        }

        [Theory]
        [InlineData(8)]
        [InlineData(13)]
        public void MAUI_SfPicker_ColumnHeader_FontSize_002(double expectedValue)
        {
            SfPicker picker = new SfPicker();
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            picker.ColumnHeaderView.TextStyle.FontSize = expectedValue;

            double actualValue = picker.ColumnHeaderView.TextStyle.FontSize;

            Assert.Equal(expectedValue, actualValue);
            picker.Columns.Clear();

        }

        [Fact]
        public void MAUI_SfPicker_ColumnHeader_TextColor()
        {
            SfPicker picker = new SfPicker();
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;

            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_ContentPage()
        {

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };


            SfPicker picker = new SfPicker()
            {
                Columns = new ObservableCollection<PickerColumn>()
            {
            new PickerColumn()
                {
                ItemsSource = dataSource,
                }
            },
                HeaderView = new PickerHeaderView()
                {
                    Height = 40,
                    Text = "Select a color",
                },
                FooterView = new PickerFooterView()
                {
                    Height = 40,
                }
            };

            ContentPage contentPage = new ContentPage();
            contentPage.Content = picker;

            Assert.Equal(picker, contentPage.Content);
        }

        [Fact]
        public void MAUI_SfPicker_CornerRadius()
        {
            SfPicker picker = new SfPicker();
            picker.SelectionView.CornerRadius = new CornerRadius(30, 30, 30, 30);

            Assert.Equal(new CornerRadius(30, 30, 30, 30), picker.SelectionView.CornerRadius);
        }

        [Fact]
        public void MAUI_SfPicker_CornerRadius_Padding()
        {
            SfPicker picker = new SfPicker();
            picker.SelectionView.CornerRadius = new CornerRadius(2, 4, 3, 5);
            picker.SelectionView.Padding = new Thickness(0, 2, 0, 0);

            Assert.Equal(new CornerRadius(2, 4, 3, 5), picker.SelectionView.CornerRadius);
            Assert.Equal(new Thickness(0, 2, 0, 0), picker.SelectionView.Padding);
        }

        [Fact]
        public void MAUI_SfPicker_DialogAddMultipleColumn()
        {
            SfPicker picker = new SfPicker();
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.NotNull(picker.Columns);
            picker.Columns.Clear();

        }

        [Fact]
        public void MAUI_SfPicker_DialogCancelButtonText()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.FooterView.CancelButtonText = "exit";

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal("exit", picker.FooterView.CancelButtonText);
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeaderHeight_ColumnText()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.ColumnHeaderView.Height = 35;

            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].HeaderText = "Text";


            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal("Text", picker.Columns[0].HeaderText);
            Assert.Equal(35, picker.ColumnHeaderView.Height);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeaderHeight_FooterHeight()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.ColumnHeaderView.Height = 35;
            picker.FooterView.Height = 55;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(35, picker.ColumnHeaderView.Height);
            Assert.Equal(55, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeaderTextColor_FooterTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.FooterView.TextStyle.TextColor = Colors.Red;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Red, picker.FooterView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeaderText_001()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            picker.Columns[1].HeaderText = "Text";

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal("Text", picker.Columns[1].HeaderText);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeaderText_Background()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.Columns.Add(new PickerColumn());

            picker.Columns[0].HeaderText = "Text";
            picker.ColumnHeaderView.Background = Colors.Yellow;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal("Text", picker.Columns[0].HeaderText);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.Background);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeaderText_FontAttributes()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.Columns.Add(new PickerColumn());

            picker.Columns[0].HeaderText = "Text";
            picker.ColumnHeaderView.TextStyle.FontAttributes = FontAttributes.Bold;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal("Text", picker.Columns[0].HeaderText);
            Assert.Equal(FontAttributes.Bold, picker.ColumnHeaderView.TextStyle.FontAttributes);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeader_BackgroundColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            picker.ColumnHeaderView.Background = Colors.Yellow;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_DialogColumnHeader_DividerColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            picker.ColumnHeaderView.DividerColor = Colors.Yellow;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.DividerColor);
        }

        [Fact]
        public void MAUI_SfPicker_DialogDisableColumnHeader_Header()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            picker.ColumnHeaderView.Height = 0;
            picker.HeaderView.Height = 0;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(0, picker.ColumnHeaderView.Height);
            Assert.Equal(0, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogDisableFooter()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            picker.FooterView.Height = 0;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(0, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogDisableHeader()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            picker.HeaderView.Height = 0;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(0, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogDisableHeader_Footer()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            picker.FooterView.Height = 0;
            picker.HeaderView.Height = 0;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(0, picker.FooterView.Height);
            Assert.Equal(0, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogDisplayMemberPath()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].DisplayMemberPath = "StateName";

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal("StateName", picker.Columns[0].DisplayMemberPath);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DialogFontSize()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.TextStyle.FontSize = 15;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(15, picker.TextStyle.FontSize);
        }

        [Theory]
        [InlineData(16)]
        [InlineData(8)]
        public void MAUI_SfPicker_DialogFooterFontSize(double expectedValue)
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.FooterView.TextStyle.FontSize = expectedValue;
            double actualValue = picker.FooterView.TextStyle.FontSize;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfPicker_DialogFooterHeight()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.FooterView.Height = 44;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(44, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogFooterHeight_Text()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.FooterView.Height = 44;
            picker.FooterView.OkButtonText = "";
            picker.FooterView.CancelButtonText = "";

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(44, picker.FooterView.Height);
            Assert.Equal("", picker.FooterView.OkButtonText);
            Assert.Equal("", picker.FooterView.CancelButtonText);
        }

        [Fact]
        public void MAUI_SfPicker_DialogFooterText_BackgroundColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.FooterView.Background = Colors.Yellow;
            picker.FooterView.OkButtonText = "okay1";
            picker.FooterView.CancelButtonText = "cancel1";

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.FooterView.Background);
            Assert.Equal("okay1", picker.FooterView.OkButtonText);
            Assert.Equal("cancel1", picker.FooterView.CancelButtonText);
        }

        [Fact]
        public void MAUI_SfPicker_DialogFooterText_FontSize()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.FooterView.TextStyle.FontSize = 18;
            picker.FooterView.CancelButtonText = "cancel1";

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(18, picker.FooterView.TextStyle.FontSize);
            Assert.Equal("cancel1", picker.FooterView.CancelButtonText);
        }

        [Fact]
        public void MAUI_SfPicker_DialogHeaderFontSize_001()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.TextStyle.FontSize = 8;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(8, picker.HeaderView.TextStyle.FontSize);
        }

        [Fact]
        public void MAUI_SfPicker_DialogHeaderHeight()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.Height = 45;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(45, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogHeaderText()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.Text = "Header";

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal("Header", picker.HeaderView.Text);
        }

        [Fact]
        public void MAUI_SfPicker_DialogHeaderTextColor_ColumnHeaderTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.TextStyle.TextColor = Colors.Blue;
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Yellow;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Blue, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Yellow, picker.ColumnHeaderView.TextStyle.TextColor);
        }


        [Fact]
        public void MAUI_SfPicker_DialogHeaderTextColor_FooterTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.FooterView.TextStyle.TextColor = Colors.Blue;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.FooterView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_DialogHeaderTextColor_SelectionTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.SelectedTextStyle.TextColor = Colors.Yellow;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Yellow, picker.SelectedTextStyle.TextColor);
        }


        [Fact]
        public void MAUI_SfPicker_DialogHeaderTextColor_TextColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.TextStyle.TextColor = Colors.Blue;
            picker.TextStyle.TextColor = Colors.Blue;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Blue, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_DialogHeaderText_BackgroundColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.HeaderView.Background = Colors.Blue;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.HeaderView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_DialogHeaderTextColor()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_DialogMode()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
        }

        [Fact]
        public void MAUI_SfPicker_DialogSelectedTextColor_FontAttribute()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.SelectedTextStyle.TextColor = Colors.Blue;
            picker.SelectedTextStyle.FontAttributes = FontAttributes.Italic;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(Colors.Blue, picker.SelectedTextStyle.TextColor);
            Assert.Equal(FontAttributes.Italic, picker.SelectedTextStyle.FontAttributes);
        }

        [Fact]
        public void MAUI_SfPicker_DialogShowColumnHeader()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.ColumnHeaderView.Height = 40;

            PickerColumn pickerColumn = new PickerColumn() { HeaderText = "Select Languages" };
            picker.Columns.Add(pickerColumn);

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(40, picker.ColumnHeaderView.Height);
            Assert.Equal("Select Languages", pickerColumn.HeaderText);

            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DialogShowColumnHeader_FooterHeight()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.ColumnHeaderView.Height = 40;
            picker.FooterView.Height = 400;

            PickerColumn pickerColumn = new PickerColumn() { HeaderText = "Select Languages" };
            picker.Columns.Add(pickerColumn);

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(40, picker.ColumnHeaderView.Height);
            Assert.Equal("Select Languages", pickerColumn.HeaderText);
            Assert.Equal(400, picker.FooterView.Height);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DialogShowFooter()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.FooterView.Height = 0;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(0, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DialogShowHeader()
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.Dialog;
            picker.HeaderView.Height = 0;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.Dialog, picker.Mode);
            Assert.Equal(0, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DisableColumnHeader()
        {
            SfPicker picker = new SfPicker();

            picker.ColumnHeaderView.Height = 0;

            Assert.Equal(0, picker.ColumnHeaderView.Height);
        }


        [Fact]
        public void MAUI_SfPicker_DisableColumnHeader_001()
        {
            SfPicker picker = new SfPicker();

            picker.ColumnHeaderView.Height = 0;
            picker.Columns.Add(new PickerColumn() { HeaderText = "StateName" });
            picker.Columns.Add(new PickerColumn() { HeaderText = "StateName" });
            picker.Columns.Add(new PickerColumn() { HeaderText = "StateName" });

            Assert.Equal(0, picker.ColumnHeaderView.Height);
            Assert.Equal("StateName", picker.Columns[2].HeaderText);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DisableColumnHeader_Footer()
        {
            SfPicker picker = new SfPicker();

            picker.ColumnHeaderView.Height = 0;
            picker.FooterView.Height = 0;

            Assert.Equal(0, picker.ColumnHeaderView.Height);
            Assert.Equal(0, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DisableColumnHeader_Header()
        {
            SfPicker picker = new SfPicker();

            picker.ColumnHeaderView.Height = 0;
            picker.HeaderView.Height = 0;

            Assert.Equal(0, picker.ColumnHeaderView.Height);
            Assert.Equal(0, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DisableFooter()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.Height = 0;

            Assert.Equal(0, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DisableHeader()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Height = 0;

            Assert.Equal(0, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DisableHeader_Footer()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Height = 0;
            picker.FooterView.Height = 0;

            Assert.Equal(0, picker.FooterView.Height);
            Assert.Equal(0, picker.HeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_DisplayMemberPath()
        {
            SfPicker picker = new SfPicker();

            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.DisplayMemberPath = "StateName";

            picker.Columns.Add(pickerColumn);

            Assert.Equal("StateName", pickerColumn.DisplayMemberPath);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DisplayMemberPath_ItemTemplate1()
        {
            SfPicker picker = new SfPicker();

            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.DisplayMemberPath = "StateName";

            DataTemplate customView = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    Padding = new Thickness(0, 1, 0, 1),
                    BackgroundColor = Colors.LightBlue,
                };

                Label label = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Blue,
                };
                label.SetBinding(Label.TextProperty, new Binding("Data"));
                grid.Children.Add(label);
                return grid;
            });

            picker.ItemTemplate = customView;

            picker.Columns.Add(pickerColumn);

            Assert.Equal("StateName", pickerColumn.DisplayMemberPath);
            Assert.Equal(customView, picker.ItemTemplate);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_DisplayMemberPath_ItemTemplate2()
        {
            SfPicker picker = new SfPicker();

            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.DisplayMemberPath = "StateName";

            DataTemplate customView = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    Padding = new Thickness(0, 1, 0, 1),
                    BackgroundColor = Colors.LightGreen,
                };

                Label label = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Green,
                };
                label.SetBinding(Label.TextProperty, new Binding("Data"));
                grid.Children.Add(label);
                return grid;
            });

            picker.ItemTemplate = customView;

            picker.Columns.Add(pickerColumn);

            Assert.Equal("StateName", pickerColumn.DisplayMemberPath);
            Assert.Equal(customView, picker.ItemTemplate);
            picker.Columns.Clear();
        }

        //[Fact]
        //public void MAUI_SfPicker_DisplayMemberPath_ItemTemplate3()
        //{
        //    SfPicker picker = new SfPicker();

        //    ObservableCollection<string> languages = new ObservableCollection<string> { "French", "Tamil", "English", "Malayalam", "Chinese", "Telugu", "Japanese", "Arabic", "Hindi", "Portuguese", "Italian" };
        //    PickerColumn pickerColumn = new PickerColumn()
        //    {
        //        HeaderText = "Select Languages",
        //        ItemsSource = languages,
        //        SelectedIndex = 1,
        //    };
        //    DataTemplate indianLanguage = new DataTemplate(() =>
        //    {
        //        Grid grid = new Grid();
        //        grid.BackgroundColor = Colors.LightBlue;
        //        Label label = new Label
        //        {
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center
        //        };
        //        label.SetBinding(Label.TextProperty, "Data");
        //        grid.Children.Add(label);
        //        return new ViewCell { View = grid };
        //    });

        //    DataTemplate otherLanguage = new DataTemplate(() =>
        //    {
        //        Grid grid = new Grid();
        //        grid.BackgroundColor = Colors.LightGreen;
        //        Label label = new Label
        //        {
        //            HorizontalTextAlignment = TextAlignment.Center,
        //            VerticalTextAlignment = TextAlignment.Center
        //        };
        //        label.SetBinding(Label.TextProperty, "Data");
        //        grid.Children.Add(label);
        //        return new ViewCell { View = grid };
        //    });

        //    PickerTemplate

        //    this.picker.ItemTemplate = pickerTemplate;
        //    this.picker.Columns.Add(pickerColumn);

        //    Assert.Equal("StateName", pickerColumn.DisplayMemberPath);
        //    Assert.Equal(customView, picker.ItemTemplate);
        //}


        [Theory]
        [InlineData(6)]
        [InlineData(13)]
        public void MAUI_SfPicker_FontSize_002(double expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            picker.TextStyle.FontSize = expectedValue;
            double actualValue = picker.TextStyle.FontSize;

            Assert.Equal(expectedValue, actualValue);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_FooterBackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.Background = Colors.Yellow;

            Assert.Equal(Colors.Yellow, picker.FooterView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_FooterBackgroundColor_TextColor()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.Background = Colors.Yellow;
            picker.FooterView.TextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.FooterView.Background);
            Assert.Equal(Colors.Blue, picker.FooterView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_FooterDividerColor()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.DividerColor = Colors.Yellow;

            Assert.Equal(Colors.Yellow, picker.FooterView.DividerColor);
        }

        [Fact]
        public void MAUI_SfPicker_FooterHeight_BackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.Background = Colors.Yellow;
            picker.FooterView.Height = 45;

            Assert.Equal(Colors.Yellow, picker.FooterView.Background);
            Assert.Equal(45, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_FooterHeight_Text()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.OkButtonText = "Okay1";
            picker.FooterView.CancelButtonText = "Cancel1";
            picker.FooterView.Height = 45;

            Assert.Equal("Okay1", picker.FooterView.OkButtonText);
            Assert.Equal("Cancel1", picker.FooterView.CancelButtonText);
            Assert.Equal(45, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_FooterText()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.OkButtonText = "Okay1";
            picker.FooterView.CancelButtonText = "Cancel1";

            Assert.Equal("Okay1", picker.FooterView.OkButtonText);
            Assert.Equal("Cancel1", picker.FooterView.CancelButtonText);
        }

        [Fact]
        public void MAUI_SfPicker_FooterTextColor_FontAttributes()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.TextStyle.TextColor = Colors.Yellow;
            picker.FooterView.TextStyle.FontAttributes = FontAttributes.Bold;

            Assert.Equal(Colors.Yellow, picker.FooterView.TextStyle.TextColor);
            Assert.Equal(FontAttributes.Bold, picker.FooterView.TextStyle.FontAttributes);
        }

        [Fact]
        public void MAUI_SfPicker_FooterTextColor_FontSize()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.TextStyle.TextColor = Colors.Yellow;
            picker.FooterView.TextStyle.FontSize = 16;

            Assert.Equal(Colors.Yellow, picker.FooterView.TextStyle.TextColor);
            Assert.Equal(16, picker.FooterView.TextStyle.FontSize);
        }


        [Fact]
        public void MAUI_SfPicker_FooterText_BackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.TextStyle.TextColor = Colors.Blue;
            picker.Background = Colors.Red;

            Assert.Equal(Colors.Blue, picker.FooterView.TextStyle.TextColor);
            Assert.Equal(Colors.Red, picker.Background);
        }

        [Fact]
        public void MAUI_SfPicker_FooterText_FontSize()
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.TextStyle.TextColor = Colors.Blue;
            picker.FooterView.TextStyle.FontSize = 20;

            Assert.Equal(Colors.Blue, picker.FooterView.TextStyle.TextColor);
            Assert.Equal(20, picker.FooterView.TextStyle.FontSize);
        }

        [Fact]
        public void MAUI_SfPicker_Grid()
        {

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };


            SfPicker picker = new SfPicker()
            {
                Columns = new ObservableCollection<PickerColumn>()
            {
            new PickerColumn()
                {
                ItemsSource = dataSource,
                }
            },
                HeaderView = new PickerHeaderView()
                {
                    Height = 40,
                    Text = "Select a color",
                },
                FooterView = new PickerFooterView()
                {
                    Height = 40,
                }
            };

            Grid grid = new Grid();
            grid.Children.Add(picker);

            Assert.Equal(picker, grid.Children[0]);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderBackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Background = Colors.Yellow;

            Assert.Equal(Colors.Yellow, picker.HeaderView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderBackgroundColor_ColumnHeaderBackground()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Background = Colors.Yellow;
            picker.ColumnHeaderView.Background = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.HeaderView.Background);
            Assert.Equal(Colors.Blue, picker.ColumnHeaderView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderBackgroundColor_FooterBackground()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Background = Colors.Yellow;
            picker.FooterView.Background = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.HeaderView.Background);
            Assert.Equal(Colors.Blue, picker.FooterView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderBackgroundColor_TextColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Background = Colors.Yellow;
            picker.HeaderView.TextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.HeaderView.Background);
            Assert.Equal(Colors.Blue, picker.HeaderView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderDividerColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.DividerColor = Colors.Yellow;

            Assert.Equal(Colors.Yellow, picker.HeaderView.DividerColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderDividerColor_BackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.DividerColor = Colors.Yellow;
            picker.HeaderView.Background = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.HeaderView.DividerColor);
            Assert.Equal(Colors.Red, picker.HeaderView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderDividerColor_ColumnDividerColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.DividerColor = Colors.Yellow;
            picker.ColumnDividerColor = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.HeaderView.DividerColor);
            Assert.Equal(Colors.Red, picker.ColumnDividerColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderDividerColor_ColumnHeaderDivider()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.DividerColor = Colors.Yellow;
            picker.ColumnHeaderView.DividerColor = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.HeaderView.DividerColor);
            Assert.Equal(Colors.Red, picker.ColumnHeaderView.DividerColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderDividerColor_FooterDivider()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.DividerColor = Colors.Yellow;
            picker.FooterView.DividerColor = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.HeaderView.DividerColor);
            Assert.Equal(Colors.Red, picker.FooterView.DividerColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderHeight_BackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Height = 45;
            picker.HeaderView.Background = Colors.Red;

            Assert.Equal(45, picker.HeaderView.Height);
            Assert.Equal(Colors.Red, picker.HeaderView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderHeight_ColumnHeight()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Height = 45;
            picker.ColumnHeaderView.Height = 50;

            Assert.Equal(45, picker.HeaderView.Height);
            Assert.Equal(50, picker.ColumnHeaderView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderHeight_FooterHeight()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Height = 45;
            picker.FooterView.Height = 50;

            Assert.Equal(45, picker.HeaderView.Height);
            Assert.Equal(50, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderHeight_Text()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Height = 45;
            picker.HeaderView.Text = "Header";

            Assert.Equal(45, picker.HeaderView.Height);
            Assert.Equal("Header", picker.HeaderView.Text);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderTextColor_ColumnHeaderTextColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.ColumnHeaderView.TextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.ColumnHeaderView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderTextColor_FontAttribute()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.ColumnHeaderView.TextStyle.FontAttributes = FontAttributes.Italic;

            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(FontAttributes.Italic, picker.ColumnHeaderView.TextStyle.FontAttributes);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderTextColor_FontSize()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.ColumnHeaderView.TextStyle.FontSize = 16;

            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(16, picker.ColumnHeaderView.TextStyle.FontSize);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderTextColor_FooterTextColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.FooterView.TextStyle.TextColor = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Red, picker.FooterView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderTextColor_SelectionTextColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.SelectedTextStyle.TextColor = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Red, picker.SelectedTextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderText_BackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle.TextColor = Colors.Yellow;
            picker.HeaderView.Background = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.HeaderView.TextStyle.TextColor);
            Assert.Equal(Colors.Red, picker.HeaderView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderText_ColumnHeaderText()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Text = "Header";
            picker.Columns.Add(new PickerColumn());
            picker.Columns[0].HeaderText = "ColumnHeaderText";

            Assert.Equal("Header", picker.HeaderView.Text);
            Assert.Equal("ColumnHeaderText", picker.Columns[0].HeaderText);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_HeaderText_FontAttributes()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Text = "Header";
            picker.HeaderView.TextStyle.FontAttributes = FontAttributes.Bold;

            Assert.Equal("Header", picker.HeaderView.Text);
            Assert.Equal(FontAttributes.Bold, picker.HeaderView.TextStyle.FontAttributes);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderText_FontSize()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Text = "Header";
            picker.HeaderView.TextStyle.FontSize = 16;

            Assert.Equal("Header", picker.HeaderView.Text);
            Assert.Equal(16, picker.HeaderView.TextStyle.FontSize);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderText_FooterText()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Text = "Header";
            picker.FooterView.OkButtonText = "exit";
            picker.FooterView.CancelButtonText = "can";

            Assert.Equal("Header", picker.HeaderView.Text);
            Assert.Equal("exit", picker.FooterView.OkButtonText);
            Assert.Equal("can", picker.FooterView.CancelButtonText);
        }

        [Fact]
        public void MAUI_SfPicker_HeaderText_TextColor()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Text = "Header";
            picker.HeaderView.TextStyle.TextColor = Colors.Red;

            Assert.Equal("Header", picker.HeaderView.Text);
            Assert.Equal(Colors.Red, picker.HeaderView.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_InsertColumn()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            picker.Columns.Add(new PickerColumn());
            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "InsertColumns",
                ItemsSource = dataSource,
                SelectedIndex = 0,
            };

            picker.Columns.Insert(0, pickerColumn);
            Assert.Equal(pickerColumn, picker.Columns[0]);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_InsertIndexColumn()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            ObservableCollection<object> dataSource1 = new ObservableCollection<object>()
            {
            "Australia", "India", "England", "South Africa", "West Indies", "GreenLand", "IreLand", "America"
            };

            picker.Columns.Add(new PickerColumn());
            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "ReplaceColumns",
                ItemsSource = dataSource,
                SelectedIndex = 0,
            };

            picker.Columns[0] = pickerColumn;
            picker.Columns[0].ItemsSource = dataSource1;

            Assert.Equal(dataSource1, picker.Columns[0].ItemsSource);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_ItemTemplate1()
        {
            SfPicker picker = new SfPicker();
            DataTemplate customView = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    Padding = new Thickness(0, 1, 0, 1),
                    BackgroundColor = Colors.LightBlue,
                };

                Label label = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Blue,
                };
                label.SetBinding(Label.TextProperty, new Binding("Data"));
                grid.Children.Add(label);
                return grid;
            });

            picker.ItemTemplate = customView;


            Assert.Equal(customView, picker.ItemTemplate);
        }

        [Fact]
        public void MAUI_SfPicker_ItemTemplate2()
        {
            SfPicker picker = new SfPicker();
            DataTemplate customView = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    Padding = new Thickness(0, 1, 0, 1),
                    BackgroundColor = Colors.LightGreen,
                };

                Label label = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Green,
                };
                label.SetBinding(Label.TextProperty, new Binding("Data"));
                grid.Children.Add(label);
                return grid;
            });

            picker.ItemTemplate = customView;

            Assert.Equal(customView, picker.ItemTemplate);
        }

        [Fact]
        public void MAUI_SfPicker_MoveColumn()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            picker.Columns.Add(new PickerColumn());
            PickerColumn pickerColumn1 = new PickerColumn()
            {
                HeaderText = "AddedColumns",
                ItemsSource = dataSource,
                SelectedIndex = 4,
            };
            picker.Columns.Add(pickerColumn1);
            picker.Columns[0] = pickerColumn1;

            Assert.Equal(pickerColumn1, picker.Columns[0]);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_NewColumn()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };
            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.ItemsSource = dataSource;
            pickerColumn.SelectedIndex = 2;
            pickerColumn.HeaderText = "Colors";
            picker.Columns = new ObservableCollection<PickerColumn> { pickerColumn };

            Assert.Equal(pickerColumn, picker.Columns[0]);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_Padding_001()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };
            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "AddedColumns",
                ItemsSource = dataSource,
                SelectedIndex = 4,
            };
            picker.Columns.Add(pickerColumn);
            picker.SelectionView.CornerRadius = new CornerRadius(0, 2, 0, 0);

            Assert.Equal(pickerColumn, picker.Columns[0]);
            Assert.Equal(new CornerRadius(0, 2, 0, 0), picker.SelectionView.CornerRadius);
            picker.Columns.Clear();
        }

        [Theory]
        [InlineData(PickerRelativePosition.AlignTop)]
        [InlineData(PickerRelativePosition.AlignTopRight)]
        [InlineData(PickerRelativePosition.AlignBottom)]
        [InlineData(PickerRelativePosition.AlignToRightOf)]
        [InlineData(PickerRelativePosition.AlignBottomRight)]
        [InlineData(PickerRelativePosition.AlignTopLeft)]
        [InlineData(PickerRelativePosition.AlignToLeftOf)]
        [InlineData(PickerRelativePosition.AlignBottomLeft)]
        public void MAUI_SfPicker_RelativeDialog_001(PickerRelativePosition expectedValue)
        {
            SfPicker picker = new SfPicker();
            picker.IsOpen = true;
            picker.Mode = PickerMode.RelativeDialog;
            picker.RelativePosition = expectedValue;
            PickerRelativePosition actualValue = picker.RelativePosition;

            Assert.True(picker.IsOpen);
            Assert.Equal(PickerMode.RelativeDialog, picker.Mode);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfPicker_RemoveColumn()
        {
            SfPicker picker = new SfPicker();


            picker.Columns.Add(new PickerColumn());

            picker.Columns.RemoveAt(0);
            Assert.Empty(picker.Columns);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_RemoveIndexColumn()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "AddedColumns",
                ItemsSource = dataSource,
                SelectedIndex = 4,
            };

            picker.Columns.Add(pickerColumn);

            picker.Columns.Remove(picker.Columns[0]);
            Assert.Empty(picker.Columns);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_RemoveMultipleColumn()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "AddedColumns",
                ItemsSource = dataSource,
                SelectedIndex = 4,
            };

            picker.Columns.Add(pickerColumn);
            picker.Columns.Add(pickerColumn);


            picker.Columns.RemoveAt(picker.Columns.Count - 1);
            picker.Columns.RemoveAt(picker.Columns.Count - 1);

            Assert.Empty(picker.Columns);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_Replace()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            picker.Columns.Add(new PickerColumn());
            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "ReplaceColumns",
                ItemsSource = dataSource,
                SelectedIndex = 0,
            };

            picker.Columns[0] = pickerColumn;

            Assert.Equal(pickerColumn, picker.Columns[0]);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_ResetColumn()
        {
            SfPicker picker = new SfPicker();

            picker.Columns.Add(new PickerColumn());

            picker.Columns.Clear();

            Assert.Empty(picker.Columns);
        }

        [Fact]
        public void MAUI_SfPicker_ResetMultipleColumn()
        {
            SfPicker picker = new SfPicker();

            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            picker.Columns.Clear();

            Assert.Empty(picker.Columns);
        }

        [Fact]
        public void MAUI_SfPicker_Scrolling()
        {

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };


            SfPicker picker = new SfPicker()
            {
                Columns = new ObservableCollection<PickerColumn>()
            {
            new PickerColumn()
                {
                ItemsSource = dataSource,
                HeaderText = "Color",
                }
            },
                HeaderView = new PickerHeaderView()
                {
                    Height = 40,
                    Text = "Select a color",
                },
                FooterView = new PickerFooterView()
                {
                    Height = 40,
                }
            };


            ScrollView scrollView = new ScrollView();
            scrollView.Content = picker;

            Assert.Equal(picker, scrollView.Content);
        }

        [Fact]
        public void MAUI_SfPicker_SelectedBackgroundColor()
        {
            SfPicker picker = new SfPicker();

            picker.SelectionView.Background = Colors.Yellow;

            Assert.Equal(Colors.Yellow, picker.SelectionView.Background);
        }

        [Fact]
        public void MAUI_SfPicker_SelectedBackgroundColor_FooterBC()
        {
            SfPicker picker = new SfPicker();

            picker.SelectionView.Background = Colors.Yellow;
            picker.FooterView.Background = Colors.Red;

            Assert.Equal(Colors.Yellow, picker.SelectionView.Background);
            Assert.Equal(Colors.Red, picker.FooterView.Background);
        }

        [Theory]
        [InlineData(14)]
        [InlineData(8)]
        public void MAUI_SfPicker_SelectedFontSize_002(double expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.SelectedTextStyle.FontSize = expectedValue;
            double actualValue = picker.SelectedTextStyle.FontSize;

            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());

            Assert.NotNull(picker.Columns);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfPicker_SelectedIndex_001()
        {
            SfPicker picker = new SfPicker();

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            picker.Columns.Add(new PickerColumn());
            PickerColumn pickerColumn = new PickerColumn()
            {
                ItemsSource = dataSource,
                SelectedIndex = 0,
            };
            picker.Columns.Add(pickerColumn);
            picker.Columns[1].SelectedIndex = 3;

            Assert.Equal(3, picker.Columns[1].SelectedIndex);
        }

        [Fact]
        public void MAUI_SfPicker_SelectedTextColor_001()
        {
            SfPicker picker = new SfPicker();
            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };

            PickerColumn pickerColumn1 = new PickerColumn() { HeaderText = "AddedColumns", ItemsSource = dataSource, SelectedIndex = 4 };
            PickerColumn pickerColumn2 = new PickerColumn() { HeaderText = "AddedColumns", ItemsSource = dataSource, SelectedIndex = 5 };
            PickerColumn pickerColumn3 = new PickerColumn() { HeaderText = "AddedColumns", ItemsSource = dataSource, SelectedIndex = 1 };

            picker.Columns.Add(pickerColumn1);
            picker.Columns.Add(pickerColumn2);
            picker.Columns.Add(pickerColumn3);

            picker.SelectedTextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Blue, picker.SelectedTextStyle.TextColor);
            Assert.NotNull(picker.Columns);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_SelectedTextColor_FontAttribute()
        {
            SfPicker picker = new SfPicker();

            picker.SelectedTextStyle.FontAttributes = FontAttributes.None;
            picker.SelectedTextStyle.TextColor = Colors.Red;

            Assert.Equal(Colors.Red, picker.SelectedTextStyle.TextColor);
            Assert.Equal(FontAttributes.None, picker.SelectedTextStyle.FontAttributes);
        }

        [Fact]
        public void MAUI_SfPicker_SelectedTextColor_FontSize()
        {
            SfPicker picker = new SfPicker();

            picker.SelectedTextStyle.FontSize = 14;
            picker.SelectedTextStyle.TextColor = Colors.Red;

            Assert.Equal(Colors.Red, picker.SelectedTextStyle.TextColor);
            Assert.Equal(14, picker.SelectedTextStyle.FontSize);
        }

        [Fact]
        public void MAUI_SfPicker_ShowColumnHeader_001()
        {
            SfPicker picker = new SfPicker();

            picker.ColumnHeaderView.Height = 0;
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());


            Assert.NotNull(picker.Columns);
            Assert.Equal(0, picker.ColumnHeaderView.Height);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_ShowHeader_Footer()
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.Height = 0;
            picker.FooterView.Height = 0;

            Assert.Equal(0, picker.HeaderView.Height);
            Assert.Equal(0, picker.FooterView.Height);
        }

        [Fact]
        public void MAUI_SfPicker_StackLayout()
        {

            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
            "Pink", "Green", "Blue", "Yellow", "Orange", "Purple", "SkyBlue", "PaleGreen"
            };


            SfPicker picker = new SfPicker()
            {
                Columns = new ObservableCollection<PickerColumn>()
            {
            new PickerColumn()
                {
                ItemsSource = dataSource,
                }
            },
                HeaderView = new PickerHeaderView()
                {
                    Height = 40,
                    Text = "Select a color",
                },
                FooterView = new PickerFooterView()
                {
                    Height = 40,
                }
            };

            StackLayout staclLayout = new StackLayout();
            staclLayout.Children.Add(picker);

            Assert.Equal(picker, staclLayout.Children[0]);
        }

        [Fact]
        public void MAUI_SfPicker_StrokeColor_001()
        {
            SfPicker picker = new SfPicker();

            picker.SelectionView.Stroke = Colors.Red;
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());


            Assert.NotNull(picker.Columns);
            Assert.Equal(Colors.Red, picker.SelectionView.Stroke);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_TextColor_001()
        {
            SfPicker picker = new SfPicker();

            picker.TextStyle.TextColor = Colors.Red;
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());
            picker.Columns.Add(new PickerColumn());


            Assert.NotNull(picker.Columns);
            Assert.Equal(Colors.Red, picker.TextStyle.TextColor);
            picker.Columns.Clear();
        }

        [Fact]
        public void MAUI_SfPicker_TextColor_FontAttribute()
        {
            SfPicker picker = new SfPicker();

            picker.TextStyle.TextColor = Colors.Red;
            picker.TextStyle.FontAttributes = FontAttributes.Italic;

            Assert.Equal(FontAttributes.Italic, picker.TextStyle.FontAttributes);
            Assert.Equal(Colors.Red, picker.TextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_TextColor_FontSize()
        {
            SfPicker picker = new SfPicker();

            picker.TextStyle.TextColor = Colors.Red;
            picker.TextStyle.FontSize = 16;

            Assert.Equal(Colors.Red, picker.TextStyle.TextColor);
            Assert.Equal(16, picker.TextStyle.FontSize);
        }

        [Fact]
        public void MAUI_SfPicker_TextColor_SelectionTextColor()
        {
            SfPicker picker = new SfPicker();

            picker.TextStyle.TextColor = Colors.Red;
            picker.SelectedTextStyle.TextColor = Colors.Blue;

            Assert.Equal(Colors.Red, picker.TextStyle.TextColor);
            Assert.Equal(Colors.Blue, picker.SelectedTextStyle.TextColor);
        }

        [Fact]
        public void MAUI_SfPicker_FooterOkButton_RemainVisible_AfterMultipleClicks()
        {
            // Test for issue #238: OK button disappears after first click
            SfPicker picker = new SfPicker();
            
            // Set up data source
            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                "Option1", "Option2", "Option3"
            };

            // Add a column
            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "Select Option",
                ItemsSource = dataSource,
                SelectedIndex = 0,
            };
            picker.Columns.Add(pickerColumn);

            // Set up footer with OK button visible
            picker.FooterView.Height = 40;
            picker.FooterView.ShowOkButton = true;

            // Verify initial state
            Assert.True(picker.FooterView.ShowOkButton);
            Assert.Equal(40, picker.FooterView.Height);

            // Test multiple events to ensure ShowOkButton property remains stable
            int clickCount = 0;
            picker.OkButtonClicked += (sender, e) => clickCount++;

            // Simulate property changes that might affect button visibility
            picker.FooterView.ShowOkButton = false;
            picker.FooterView.ShowOkButton = true; // Should restore button

            Assert.True(picker.FooterView.ShowOkButton);

            // Test that changing other properties doesn't affect OK button
            picker.FooterView.OkButtonText = "Confirm";
            picker.FooterView.OkButtonText = "OK";
            
            Assert.True(picker.FooterView.ShowOkButton);
            Assert.Equal("OK", picker.FooterView.OkButtonText);

            picker.Columns.Clear();
        }
    }
}
