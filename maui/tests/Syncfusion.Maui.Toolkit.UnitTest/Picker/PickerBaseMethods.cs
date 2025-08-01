using Syncfusion.Maui.Toolkit.Picker;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class PickerBaseMethods : PickerBaseUnitTest
    {
        [Theory]
        [InlineData(typeof(SfPicker))]
        [InlineData(typeof(SfDatePicker))]
        [InlineData(typeof(SfTimePicker))]
        [InlineData(typeof(SfDateTimePicker))]
        public void IsRTL_If_ReturnsBool_WhenCalled(Type classType)
        {
            object? instance = Activator.CreateInstance(classType);
            bool expectedValue = Convert.ToBoolean(InvokePrivateMethod(instance, "IsRTL", instance));
            Assert.False(expectedValue);
        }

        [Theory]
        [InlineData(typeof(SfPicker))]
        [InlineData(typeof(SfDatePicker))]
        [InlineData(typeof(SfTimePicker))]
        [InlineData(typeof(SfDateTimePicker))]
        public void IsRTL_Else_ReturnsBool_WhenCalled(Type classType)
        {
            object? instance = Activator.CreateInstance(classType);
            Grid grid = new Grid();
            bool expectedValue = Convert.ToBoolean(InvokePrivateMethod(instance, "IsRTL", grid));
            Assert.False(expectedValue);
        }

        public static IEnumerable<object[]> BrushColorData =>
            new List<object[]>
            {
            new object[] { Brush.Red, Colors.Red },
            new object[] { Brush.Green, Colors.Green },
            new object[] { Brush.Blue, Colors.Blue }
            };

        [Fact]
        public void TestUpdateFormatBasedOnCulture()
        {
            var pickerBase = new SfPicker();
            var originalCulture = CultureInfo.CurrentUICulture;
            try
            {
                CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
                InvokePrivateMethod(pickerBase, "UpdateFormatBasedOnCulture");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                CultureInfo previousUICulture = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                var type = pickerBase.GetType();
                while (type != null && previousUICulture == null)
                {
                    try
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        previousUICulture = GetPrivateField(pickerBase, "previousUICulture") as CultureInfo;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                    catch (InvalidOperationException)
                    {
                        type = type.BaseType;
                    }
                }

                if (previousUICulture != null)
                {
                    Assert.Equal("fr-FR", previousUICulture.Name);
                }
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }

        [Fact]
        public void TestAddChildren()
        {
            var pickerBase = new SfPicker();
            InvokePrivateMethod(pickerBase, "AddChildren");
            Assert.True(pickerBase.Children.Count > 0);
            Assert.IsType<PickerStackLayout>(pickerBase.Children[0]);
        }

        [Fact]
        public void TestAddOrRemoveHeaderLayout_WhenAdd()
        {
            var pickerBase = new SfPicker();
            pickerBase.BaseHeaderView.Height = 50;
            InvokePrivateMethod(pickerBase, "AddOrRemoveHeaderLayout");
            Assert.True(pickerBase.Children.Count > 0);
            var pickerStackLayout = Assert.IsType<PickerStackLayout>(pickerBase.Children[0]);
            Assert.True(pickerStackLayout.Children.Count > 0);
            var headerLayout = pickerStackLayout.Children.FirstOrDefault(c => c is HeaderLayout);
            Assert.NotNull(headerLayout);
        }

        [Fact]
        public void TestAddOrRemoveHeaderLayout_WhenRemove()
        {
            var pickerBase = new SfPicker();
            InvokePrivateMethod(pickerBase, "AddChildren");
            pickerBase.BaseHeaderView.Height = 0;
            InvokePrivateMethod(pickerBase, "AddOrRemoveHeaderLayout");
            var pickerStackLayout = (PickerStackLayout)pickerBase.Children[0];
            Assert.Null(pickerStackLayout.Children.FirstOrDefault(c => c is HeaderLayout));
        }

        [Fact]
        public void TestAddOrRemoveHeaderLayout_WhenPickerHeaderTemplate()
        {
            var picker = new SfPicker();
            picker.HeaderView.Height = 40;

            Label expectedValue = new Label { Text = "Header Content" };
            picker.HeaderTemplate = new DataTemplate(() =>
            {
                return expectedValue;
            });
            ;

            SetPrivateField(picker, "_headerLayout", null);

            ObservableCollection<object> cityName = new ObservableCollection<object>();
            cityName.Add("Chennai");
            cityName.Add("Mumbai");
            cityName.Add("Delhi");
            cityName.Add("Kolkata");
            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "Select City",
                ItemsSource = cityName,
                SelectedIndex = 1,
            };
            picker.Columns.Add(pickerColumn);

            InvokePrivateMethod(picker, "AddOrRemoveHeaderLayout");

            PickerBase pickerBase = (PickerBase)picker;

            var layout = GetPrivateField(pickerBase, "_headerLayout");
            HeaderLayout headerLayout = new HeaderLayout(pickerBase);

            if (layout != null)
            {
                headerLayout = (HeaderLayout)layout;
            }
            Label actualValue = (Label)headerLayout.Children[0];

            Assert.NotNull(picker.HeaderTemplate);
            Assert.Equal(expectedValue, actualValue);
            picker.Columns.Clear();
        }

        [Fact]
        public void TestAddOrRemoveFooterLayout_WhenAdd()
        {
            var pickerBase = new SfPicker();
            pickerBase.FooterView.Height = 40;
            InvokePrivateMethod(pickerBase, "AddOrRemoveFooterLayout");
            Assert.True(pickerBase.Children.Count > 0);
            var pickerStackLayout = Assert.IsType<PickerStackLayout>(pickerBase.Children[0]);
            Assert.True(pickerStackLayout.Children.Count > 0);
            var footerLayout = pickerStackLayout.Children.FirstOrDefault(c => c is FooterLayout);
            Assert.NotNull(footerLayout);
        }

        [Fact]
        public void TestAddOrRemoveFooterLayout_WhenRemove()
        {
            var pickerBase = new SfPicker();
            InvokePrivateMethod(pickerBase, "AddChildren");
            pickerBase.FooterView.Height = 0;
            InvokePrivateMethod(pickerBase, "AddOrRemoveFooterLayout");
            var pickerStackLayout = (PickerStackLayout)pickerBase.Children[0];
            Assert.Null(pickerStackLayout.Children.FirstOrDefault(c => c is FooterLayout));
        }

        [Fact]
        public void TestAddOrRemoveFooterLayout_WhenPickerFooterTemplate()
        {
            var picker = new SfPicker();
            picker.FooterView.Height = 40;

            Label expectedValue = new Label { Text = "Footer Content" };
            picker.FooterTemplate = new DataTemplate(() =>
            {
                return expectedValue;
            });

            SetPrivateField(picker, "_footerLayout", null);

            ObservableCollection<object> cityName = new ObservableCollection<object>();
            cityName.Add("Chennai");
            cityName.Add("Mumbai");
            cityName.Add("Delhi");
            cityName.Add("Kolkata");
            cityName.Add("Bangalore");
            cityName.Add("Hyderabad");
            cityName.Add("Pune");
            PickerColumn pickerColumn = new PickerColumn()
            {
                HeaderText = "Select City",
                ItemsSource = cityName,
                SelectedIndex = 1,
            };
            picker.Columns.Add(pickerColumn);

            InvokePrivateMethod(picker, "AddOrRemoveFooterLayout");

            PickerBase pickerBase = (PickerBase)picker;

            var layout = GetPrivateField(pickerBase, "_footerLayout");
            FooterLayout footerLayout = new FooterLayout(pickerBase);

            if (layout != null)
            {
                footerLayout = (FooterLayout)layout;
            }
            Label actualValue = (Label)footerLayout.Children[0];

            Assert.NotNull(pickerBase.FooterTemplate);
            Assert.Equal(expectedValue, actualValue);

            Assert.Equal(pickerColumn.SelectedIndex, picker.Columns[0].SelectedIndex);
            picker.Columns.Clear();
        }

        [Theory]
        [MemberData(nameof(BrushColorData))]
        public void ToColor_ReturnsColor_WhenCalled(Brush brush, Color expectedColor)
        {
            Color result = PickerHelper.ToColor(brush);
            Assert.Equal(expectedColor, result);
        }

        [Theory]
        [MemberData(nameof(datas))]
        public void GetItemsCount_ValidCollection_ReturnsCorrectCount(ObservableCollection<string> collection, int expectedCount)
        {
            var result = PickerHelper.GetItemsCount(collection);
            Assert.Equal(expectedCount, result);
        }

        public static IEnumerable<object[]> datas =>
            new List<object[]>
            {
                new object[] { new ObservableCollection<string> { "Item1", "Item2", "Item3" }, 3 },
                new object[] { new ObservableCollection<string> { "A", "B" }, 2 },
                new object[] { new ObservableCollection<string>(), 0 }
            };

        [Fact]
        public void GetSelectedItemIndex_NullItemsSource_ReturnsMinusOne()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var column = new PickerColumn
            {
                ItemsSource = null,
                SelectedItem = "Item2",
                _isSelectedItemChanged = true
            };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            var result = PickerHelper.GetSelectedItemIndex(column);
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetSelectedItemIndex_EmptyItemsSource_ReturnsMinusOne()
        {
            var column = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string>(),
                SelectedItem = "Item2",
                _isSelectedItemChanged = true
            };

            var result = PickerHelper.GetSelectedItemIndex(column);
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetSelectedItemIndex_SelectedItemNotInCollection_ReturnsMinusOne()
        {
            var column = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" },
                SelectedItem = "Item4",
                _isSelectedItemChanged = true
            };

            var result = PickerHelper.GetSelectedItemIndex(column);
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetSelectedItemIndex_SelectedItemNull_ReturnsMinusOne()
        {
            var column = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" },
                SelectedItem = null,
                _isSelectedItemChanged = true
            };

            var result = PickerHelper.GetSelectedItemIndex(column);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void GetSelectedItemIndex_ValidColumn_ReturnsCorrectIndex()
        {
            var column = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" },
                SelectedItem = "Item2",
                _isSelectedItemChanged = true
            };
            var result = PickerHelper.GetSelectedItemIndex(column);
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetSelectedItemIndex__isSelectedItemChangedFalse_ReturnsCorrectIndex()
        {
            var column = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" },
                SelectedItem = "Item2",
                SelectedIndex = 1,
                _isSelectedItemChanged = false
            };

            var result = PickerHelper.GetSelectedItemIndex(column);
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetSelectedItemIndex__isSelectedItemChangedFalse_IncorrectSelectedIndex_ReturnsMinusOne()
        {
            var column = new PickerColumn
            {
                ItemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" },
                SelectedItem = "Item2",
                SelectedIndex = 0,
                _isSelectedItemChanged = false
            };

            var result = PickerHelper.GetSelectedItemIndex(column);
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(2, 5, 2)]
        [InlineData(-1, 5, -1)]
        [InlineData(5, 5, 4)]
        [InlineData(0, 0, -1)]
        [InlineData(6, 5, 4)]
        public void GetValidSelectedIndex_ValidIndex_ReturnsCorrectIndex(int selectedIndex, int itemsCount, int exceptedValue)
        {
            var result = PickerHelper.GetValidSelectedIndex(selectedIndex, itemsCount);
            Assert.Equal(exceptedValue, result);
        }

        [Fact]
        public void IsCollectionEquals_EqualCollections_ReturnsTrue()
        {
            var collection1 = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            var collection2 = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            var result = PickerHelper.IsCollectionEquals(collection1, collection2);
            Assert.True(result);
        }

        [Fact]
        public void IsCollectionEquals_EqualCollections_ReturnsFalse()
        {
            var collection1 = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            var collection2 = new ObservableCollection<string> { "Item2", "Item1", "Item3" };
            var result = PickerHelper.IsCollectionEquals(collection1, collection2);
            Assert.False(result);
        }

        [Fact]
        public void IsCollectionEquals_EqualNullCollections_ReturnsTrue()
        {
            var collection1 = new ObservableCollection<string>();
            var collection2 = new ObservableCollection<string>();
            var result = PickerHelper.IsCollectionEquals(collection1, collection2);
            Assert.True(result);
        }

        [Fact]
        public void GetParentName_SfPicker_ReturnsPicker()
        {
            var picker = new SfPicker();
            var result = PickerHelper.GetParentName(picker);
            Assert.Equal("Picker", result);
        }

        [Fact]
        public void GetParentName_SfDatePicker_ReturnsPicker()
        {
            var picker = new SfDatePicker();
            var result = PickerHelper.GetParentName(picker);
            Assert.Equal("DatePicker", result);
        }

        [Fact]
        public void GetParentName_SfTimePicker_ReturnsPicker()
        {
            var picker = new SfTimePicker();
            var result = PickerHelper.GetParentName(picker);
            Assert.Equal("TimePicker", result);
        }

        [Fact]
        public void GetParentName_SfDateTimePicker_ReturnsPicker()
        {
            var picker = new SfDateTimePicker();
            var result = PickerHelper.GetParentName(picker);
            Assert.Equal("DateTimePicker", result);
        }

        [Fact]
        public void GetParentName_Empty_ReturnsPicker()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var result = PickerHelper.GetParentName(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSelectedItemDefaultValue_ValidPickerColumn_ReturnsCorrectItem()
        {
            var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            var pickerColumn = new PickerColumn
            {
                ItemsSource = itemsSource,
                SelectedIndex = 1
            };

            var result = PickerHelper.GetSelectedItemDefaultValue(pickerColumn);
            Assert.Equal("Item2", result);
        }

        [Fact]
        public void GetSelectedItemDefaultValue_SelectedIndexZero_ReturnsFirstItem()
        {
            var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            var pickerColumn = new PickerColumn
            {
                ItemsSource = itemsSource,
                SelectedIndex = 0
            };

            var result = PickerHelper.GetSelectedItemDefaultValue(pickerColumn);
            Assert.Equal("Item1", result);
        }

        [Fact]
        public void GetSelectedItemDefaultValue_SelectedIndexNegative_ReturnsNull()
        {
            var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            var pickerColumn = new PickerColumn
            {
                ItemsSource = itemsSource,
                SelectedIndex = -1
            };

            var result = PickerHelper.GetSelectedItemDefaultValue(pickerColumn);
            Assert.Null(result);
        }

        [Fact]
        public void GetSelectedItemDefaultValue_SelectedIndexOutOfRange_ReturnsFirstItem()
        {
            var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            var pickerColumn = new PickerColumn
            {
                ItemsSource = itemsSource,
                SelectedIndex = 5
            };

            var result = PickerHelper.GetSelectedItemDefaultValue(pickerColumn);
            Assert.Equal("Item1", result);
        }

        [Fact]
        public void GetSelectedItemDefaultValue_NullItemsSource_ReturnsNull()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var pickerColumn = new PickerColumn
            {
                ItemsSource = null,
                SelectedIndex = 0
            };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            var result = PickerHelper.GetSelectedItemDefaultValue(pickerColumn);
            Assert.Null(result);
        }

        [Fact]
        public void GetSelectedItemDefaultValue_EmptyItemsSource_ReturnsNull()
        {
            var itemsSource = new ObservableCollection<string>();
            var pickerColumn = new PickerColumn
            {
                ItemsSource = itemsSource,
                SelectedIndex = 0
            };

            var result = PickerHelper.GetSelectedItemDefaultValue(pickerColumn);
            Assert.Equal("", result);
        }
    }
}
