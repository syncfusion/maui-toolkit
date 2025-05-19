using Syncfusion.Maui.Toolkit.Picker;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class PickerUnitTest : PickerBaseUnitTest
    {
        #region PickerColumn Public Properties

        [Fact]
        public void PickerColumn_Constructor_InitializesDefaultsCorrectly()
        {
            PickerColumn picker = new PickerColumn();

            Assert.Equal(-1d, picker.Width);
            Assert.Null(picker.ItemsSource);
            Assert.Equal(string.Empty, picker.DisplayMemberPath);
            Assert.Equal(0, picker.SelectedIndex);
            Assert.Equal(string.Empty, picker.HeaderText);
            Assert.Null(picker.SelectedItem);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(0)]
        [InlineData(-20)]
        public void PickerColumn_Width_GetAndSet(int expectedValue)
        {
            var picker = new SfPicker();
            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.Width = expectedValue;
            picker.Columns.Add(pickerColumn);
            PickerColumn actualValue = pickerColumn;
            Assert.Equal(expectedValue, actualValue.Width);
            picker.Columns.Clear();
        }

        [Theory]
        [InlineData("Red", "Blue", "Yellow")]
        [InlineData(1, 2, 3)]
        [InlineData(0.1, 0.2, 0.3)]
        [InlineData((float)0.1 ,(float)0.2, (float)0.3)]
        [InlineData("String", 1, 0.2)]
        public void PickerColumn_ItemSource_GetAndSet(object value, object value1, object value2)
        {
            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                value, value1, value2
            };

            var picker = new SfPicker();
            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.ItemsSource = dataSource;
            picker.Columns.Add(pickerColumn);
            PickerColumn actualValue = pickerColumn;
            Assert.Equal(dataSource, actualValue.ItemsSource);
            picker.Columns.Clear();
        }

        [Theory]
        [InlineData("Red", "Blue", "Yellow", 0)]
        [InlineData(1, 2, 3, 1)]
        [InlineData(0.1, 0.2, 0.3, 2)]
        [InlineData((float)0.1, (float)0.2, (float)0.3, 3)]
        [InlineData("String", 1, 0.2, 1)]
        public void PickerColumn_SelectedIndex_GetAndSet(object value, object value1, object value2, int index)
        {
            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                value, value1, value2
            };
            var picker = new SfPicker();
            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.ItemsSource = dataSource;
            pickerColumn.SelectedIndex = index;
            picker.Columns.Add(pickerColumn);
            PickerColumn actualValue = pickerColumn;
            Assert.Equal(index, actualValue.SelectedIndex);
            picker.Columns.Clear();
        }

        [Theory]
        [InlineData("Red", "Blue", "Yellow", 0)]
        [InlineData(1, 2, 3, 2)]
        [InlineData(0.1, 0.2, 0.3, 0.3)]
        [InlineData((float)0.1, (float)0.2, (float)0.3, (float)0.1)]
        [InlineData("String", 1, 0.2, "String")]
        public void PickerColumn_SelectedItem_GetAndSet(object value, object value1, object value2, object item)
        {
            ObservableCollection<object> dataSource = new ObservableCollection<object>()
            {
                value, value1, value2
            };
            var picker = new SfPicker();
            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.ItemsSource = dataSource;
            pickerColumn.SelectedItem = item;
            picker.Columns.Add(pickerColumn);
            PickerColumn actualValue = pickerColumn;
            Assert.Equal(item, actualValue.SelectedItem);
            picker.Columns.Clear();
        }

        [Theory]
        [InlineData("HeaderText")]
        [InlineData("HeaderText1")]
        public void PickerColumn_HeaderText_GetAndSet(string text)
        {
            var picker = new SfPicker();
            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.HeaderText = text;
            picker.Columns.Add(pickerColumn);
            PickerColumn actualValue = pickerColumn;
            Assert.Equal(text, actualValue.HeaderText);
        }

        [Fact]
        public void PickerColumn_TestSetSelectedIndexOnNullRows()
        {
            var picker = new SfPicker();
            PickerColumn pickerColumn = new PickerColumn();
            picker.Columns.Add(pickerColumn);

            picker.Columns[0].ItemsSource = new List<string>();

            Assert.Empty((IEnumerable)picker.Columns[0].ItemsSource);
            Assert.Equal(0, picker.Columns[0].SelectedIndex);

            picker.Columns[0].SelectedIndex = 2;

            Assert.Equal(-1, picker.Columns[0].SelectedIndex);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(42)]
        [InlineData(-1)]
        [InlineData(-42)]
        public void PickerColumn_TestSelectedIndexInRange(int index)
        {
            var picker = new SfPicker();
            var pickerColumn = new PickerColumn
            {
                ItemsSource = new List<string> { "John", "Paul", "George", "Ringo" }
            };
            picker.Columns.Add(pickerColumn);

            pickerColumn.SelectedIndex = index;

            if (index <= -1)
            {
                Assert.Equal(-1, pickerColumn.SelectedIndex);
            }
            else
            {
                Assert.Equal(index, pickerColumn.SelectedIndex);
            }

            picker.Columns.Clear();
        }

        [Fact]
        public void PickerColumn_TestSelectedIndexOutOfRangeUpdatesSelectedItem()
        {
            var picker = new SfPicker();
            var items = new ObservableCollection<string>
            {
                "Monkey",
                "Banana",
                "Lemon"
            };

            var pickerColumn = new PickerColumn
            {
                ItemsSource = items,
                SelectedIndex = 0
            };

            picker.Columns.Add(pickerColumn);

            Assert.Equal("Monkey", pickerColumn.SelectedItem);

            pickerColumn.SelectedIndex = 42;
            Assert.Equal("Monkey", pickerColumn.SelectedItem);

            pickerColumn.SelectedIndex = -42;
            Assert.Null(pickerColumn.SelectedItem);
        }

        #endregion

        #region Picker Public Properties

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfPicker picker = new SfPicker();

            Assert.NotNull(picker.HeaderView);
            Assert.NotNull(picker.ColumnHeaderView);
            Assert.NotNull(picker.Columns);
            Assert.Null(picker.ItemTemplate);
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
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_ColumnDividerColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_ItemHeight_GetAndSet(int expectedValue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_TextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.TextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void Picker_TextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.TextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void Picker_TextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Picker_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_SelectedTextStyle_GetAndSet_PickerTextStyle_TextColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectedTextStyle = new PickerTextStyle() { TextColor = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.TextColor);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.Bold)]
        public void Picker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontAttributes(FontAttributes expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontAttributes = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAttributes);
        }

        [Theory]
        [InlineData("Calibri")]
        [InlineData("Cambria")]
        [InlineData("TimesNewRoman")]
        public void Picker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontFamily(string expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Picker_SelectedTextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.SelectedTextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.SelectedTextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Picker_IsOpen_GetAndSet(bool expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.IsOpen = expectedValue;
            bool actualValue = picker.IsOpen;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(PickerMode.Default)]
        [InlineData(PickerMode.Dialog)]
        [InlineData(PickerMode.RelativeDialog)]
        public void Picker_Mode_GetAndSet(PickerMode mode)
        {
            SfPicker picker = new SfPicker();
            picker.Mode = mode;
            PickerMode actualValue = picker.Mode;
            Assert.Equal(mode, actualValue);
        }

        [Theory]
        [InlineData(PickerTextDisplayMode.Default)]
        [InlineData(PickerTextDisplayMode.Fade)]
        [InlineData(PickerTextDisplayMode.Shrink)]
        [InlineData(PickerTextDisplayMode.FadeAndShrink)]
        public void Picker_TextDisplayMode_GetAndSet(PickerTextDisplayMode mode)
        {
            SfPicker picker = new SfPicker();
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
        public void Picker_RelativePosition_GetAndSet(PickerRelativePosition position)
        {
            SfPicker picker = new SfPicker();
            picker.RelativePosition = position;
            PickerRelativePosition actualValue = picker.RelativePosition;
            Assert.Equal(position, actualValue);
        }

        [Fact]
        public void AcceptCommand_Execute_ReturnsTrue()
        {
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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

        #region Picker Internal Properties

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_PickerBackground_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.PickerBackground = expectedValue;
            Color actualValue = picker.PickerBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_HeaderBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderBackground = expectedValue;
            Brush actualValue = picker.HeaderBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_FooterBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            picker.FooterBackground = expectedValue;
            Brush actualValue = picker.FooterBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_SelectionBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectionBackground = expectedValue;
            Brush actualValue = picker.SelectionBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_SelectionStrokeColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.SelectionStrokeColor = expectedValue;
            Color actualValue = picker.SelectionStrokeColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(30, 30, 30, 30)]
        [InlineData(50, 50, 50, 50)]
        [InlineData(-30, -30, -30, -30)]
        [InlineData(0, 0, 0, 0)]
        [InlineData(30, 0, 0, 0)]
        [InlineData(0, 30, 0, 0)]
        [InlineData(0, 0, 30, 0)]
        [InlineData(0, 0, 0, 30)]
        [InlineData(0, -30, 30, 0)]
        public void Picker_SelectionCornerRadius_GetAndSet(double topLeft, double topRight, double bottomLeft, double bottomRight)
        {
            SfPicker picker = new SfPicker();
            picker.SelectionCornerRadius = new CornerRadius(topLeft, topRight, bottomLeft, bottomRight);
            CornerRadius actualValue = picker.SelectionCornerRadius;
            Assert.Equal(new CornerRadius(topLeft, topRight, bottomLeft, bottomRight), actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_HeaderDividerColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.HeaderDividerColor = expectedValue;
            Color actualValue = picker.HeaderDividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_FooterDividerColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.FooterDividerColor = expectedValue;
            Color actualValue = picker.FooterDividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Picker_HeaderTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_FooterTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_SelectedTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_SelectionTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_NormalTextColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_HeaderFontSize_GetAndSet(double expectedValue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_FooterFontSize_GetAndSet(double expectedValue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_SelectedFontSize_GetAndSet(double expectedValue)
        {
            SfPicker picker = new SfPicker();

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
        public void Picker_NormalFontSize_GetAndSet(double expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.NormalFontSize = expectedValue;
            double actualValue = picker.NormalFontSize;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Header Settings Public Properties

        [Fact]
        public void HeaderSettings_Constructor_InitializesDefaultsCorrectly()
        {
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();
            picker.HeaderView.Height = value;
            double actualValue = picker.HeaderView.Height;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("HeaderText")]
        [InlineData("")]
        public void HeaderSettings_Text_GetAndSet(string value)
        {
            SfPicker picker = new SfPicker();
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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void HeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.HeaderView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.HeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        #endregion

        #region ColumnHeader Settings Public Properties

        [Fact]
        public void ColumnHeaderSettings_Constructor_InitializesDefaultsCorrectly()
        {
            SfPicker picker = new SfPicker();

            Assert.Equal(0d, picker.ColumnHeaderView.Height);
            Assert.NotNull(picker.ColumnHeaderView.TextStyle);
            ////Assert.Equal(new SolidColorBrush(Color.FromArgb("#F7F2FB")), picker.ColumnHeaderView.Background);
            Assert.Equal(Color.FromArgb("#CAC4D0"), picker.ColumnHeaderView.DividerColor);
        }

        [Theory]
        [InlineData(40)]
        [InlineData(10)]
        [InlineData(-40)]
        [InlineData(60)]
        public void ColumnHeaderSettings_Height_GetAndSet(double value)
        {
            SfPicker picker = new SfPicker();
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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

            Color expectedValue = Color.FromRgb(red, green, blue);
            picker.ColumnHeaderView.DividerColor = expectedValue;
            Color actualValue = picker.ColumnHeaderView.DividerColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(35)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(-20)]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontSize(int expectedValue)
        {
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ColumnHeaderSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.ColumnHeaderView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.ColumnHeaderView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        [Fact]
        public void ColumnHeaderSettings_GetAndSet_Null()
        {
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();
            picker.FooterView.Height = value;
            double actualValue = picker.FooterView.Height;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Save")]
        [InlineData("")]
        public void FooterSettings_OkButtonText_GetAndSet(string value)
        {
            SfPicker picker = new SfPicker();
            picker.FooterView.OkButtonText = value;
            string actualValue = picker.FooterView.OkButtonText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData("Exit")]
        [InlineData("")]
        public void FooterSettings_CancelButtonText_GetAndSet(string value)
        {
            SfPicker picker = new SfPicker();
            picker.FooterView.CancelButtonText = value;
            string actualValue = picker.FooterView.CancelButtonText;
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FooterSettings_ShowOkButton_GetAndSet(bool value)
        {
            SfPicker picker = new SfPicker();
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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontFamily = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontFamily);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FooterSettings_TextStyle_GetAndSet_PickerTextStyle_FontAutoScalingEnabled(bool expectedValue)
        {
            SfPicker picker = new SfPicker();

            picker.FooterView.TextStyle = new PickerTextStyle() { FontAutoScalingEnabled = expectedValue };
            PickerTextStyle actualValue = picker.FooterView.TextStyle;

            Assert.Equal(expectedValue, actualValue.FontAutoScalingEnabled);
        }

        #endregion

        #region Picker Selection View Public Properties

        [Fact]
        public void PickerSelectionView_Constructor_InitializesDefaultsCorrectly()
        {
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();
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
            SfPicker picker = new SfPicker();
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
            SfPicker picker = new SfPicker();
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
            SfPicker picker = new SfPicker();
            Thickness exceptedValue = new Thickness(value1, value2, value3, value4);
            picker.SelectionView.Padding = exceptedValue;
            Thickness actualValue = picker.SelectionView.Padding;
            Assert.Equal(exceptedValue, actualValue);
        }

        [Fact]
        public void PickerSelectionView_GetAndSet_Null()
        {
            SfPicker picker = new SfPicker();

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
            SfPicker picker = new SfPicker();
            PickerColumn pickerColumn = new PickerColumn();
            pickerColumn.ItemsSource = new ObservableCollection<string>()
            {
                "Red", "Blue", "Yellow"
            };
            picker.Columns.Add(pickerColumn);
            var fired = false;
            picker.SelectionChanged += (sender, e) => fired = true;
            picker.Columns[0].SelectedIndex = 1;
            Assert.True(fired);
            picker.Columns.Clear();
        }

        #endregion
    }
}
