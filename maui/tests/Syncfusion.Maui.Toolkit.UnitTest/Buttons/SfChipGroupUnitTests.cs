using Syncfusion.Maui.Toolkit.Chips;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfChipGroupUnitTests : BaseUnitTest
	{
		#region Constructor
		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var chipGroup = new SfChipGroup();
			Assert.Equal(chipGroup.ChipCornerRadius, 8);
			Assert.Equal(18d, chipGroup.ChipImageSize);
			Assert.Equal(new Thickness(2d, 1d, 1d, 1d), chipGroup.ChipPadding);
			Assert.Equal(FontAttributes.None, chipGroup.ChipFontAttributes);
			Assert.Equal(1, chipGroup.ChipStrokeThickness);
			Assert.Equal(Color.FromArgb("#49454F"), chipGroup.ChipTextColor);
			Assert.Equal(14d, chipGroup.ChipTextSize);
			Assert.Equal(SfChipsType.Input, chipGroup.ChipType);
			Assert.Equal(ChoiceMode.Single, chipGroup.ChoiceMode);
			Assert.Equal(Color.FromArgb("#1C1B1F"), chipGroup.CloseButtonColor);
			Assert.Equal(double.NaN, chipGroup.ItemHeight);
			Assert.Equal(Color.FromArgb("#1D192B"), chipGroup.SelectedChipTextColor);
			Assert.Equal(Color.FromArgb("#49454F"), chipGroup.SelectionIndicatorColor);
			Assert.Empty(chipGroup.ChipFontFamily);

			if (chipGroup.ChipLayout != null)
			{
				Assert.Empty(chipGroup.ChipLayout);
			}

			Assert.Empty(chipGroup.DisplayMemberPath);
			Assert.Empty(chipGroup.ImageMemberPath);
			Assert.Null(chipGroup.InputView);
			Assert.Null(chipGroup.Command);
			Assert.Null(chipGroup.ItemsSource);
			Assert.Null(chipGroup.ItemTemplate);
			Assert.Null(chipGroup.SelectedItem);
			Assert.NotNull(chipGroup.ChipBackground);
			Assert.False(chipGroup.ShowIcon);
		}
		#endregion

		#region Properties

		[Theory]
		[InlineData(null)]
		public void Items_SetValue_ReturnsExpectedValue(ChipCollection? expectedItems)
		{
			var chipGroup = new SfChipGroup
			{
				Items = expectedItems
			};
			Assert.Equal(expectedItems, chipGroup.Items);
		}

		[Fact]
		public void ItemsSource_SetValue_ReturnsExpectedValue()
		{
			var chipGroup = new SfChipGroup();
			IList<string> expectedItemsSource = ["Chip1", "Chip2"];
			chipGroup.ItemsSource = (System.Collections.IList)expectedItemsSource;
			Assert.Equal(expectedItemsSource, (IEnumerable<string>?)chipGroup.ItemsSource);
		}

		[Theory]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		[InlineData(FontAttributes.None)]
		public void ChipFontAttributes_SetValue_ReturnsExpectedValue(FontAttributes expectedFontAttributes)
		{
			var chipGroup = new SfChipGroup
			{
				ChipFontAttributes = expectedFontAttributes
			};
			Assert.Equal(expectedFontAttributes, chipGroup.ChipFontAttributes);
		}

		[Theory]
		[InlineData(-20.0)]
		[InlineData(32.0)]
		[InlineData(480.0)]
		public void ChipImageSize_SetValue_ReturnsExpectedValue(double expectedSize)
		{
			var chipGroup = new SfChipGroup
			{
				ChipImageSize = expectedSize
			};
			Assert.Equal(expectedSize, chipGroup.ChipImageSize);
		}

		[Fact]
		public void ChipLayout_SetValue_ReturnsExpectedValue()
		{
			var chipGroup = new SfChipGroup();
			var stackLayout = new StackLayout();
			Assert.NotNull(chipGroup.ChipLayout);
			chipGroup.ChipLayout = stackLayout;
			Assert.Equal(stackLayout, chipGroup.ChipLayout);
		}

		[Theory]
		[InlineData("String Value")]
		[InlineData("")]
		public void DisplayMemberPath_SetValue_ReturnsExpectedValue(string expectedPath)
		{
			var chipGroup = new SfChipGroup
			{
				DisplayMemberPath = expectedPath
			};
			Assert.Equal(expectedPath, chipGroup.DisplayMemberPath);
		}

		[Theory]
		[InlineData("Empty Value")]
		[InlineData("")]
		public void ImageMemberPath_SetValue_ReturnsExpectedValue(string expectedPath)
		{
			var chipGroup = new SfChipGroup
			{
				ImageMemberPath = expectedPath
			};
			Assert.Equal(expectedPath, chipGroup.ImageMemberPath);
		}

		[Theory]
		[InlineData(SfChipsType.Action)]
		[InlineData(SfChipsType.Input)]
		[InlineData(SfChipsType.Choice)]
		[InlineData(SfChipsType.Filter)]
		public void ChipType_SetValue_ReturnsExpectedValue(SfChipsType expectedType)
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = expectedType
			};
			Assert.Equal(expectedType, chipGroup.ChipType);
		}

		[Theory]
		[InlineData(0, 0, 0, 0)]
		[InlineData(5, 5, 5, 5)]
		[InlineData(10, 5, 10, 5)]
		public void ChipCornerRadius_SetValue_ReturnsExpectedValue(double topLeft, double topRight, double bottomRight, double bottomLeft)
		{
			var chipGroup = new SfChipGroup();
			var expectedRadius = new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
			chipGroup.ChipCornerRadius = expectedRadius;
			Assert.Equal(expectedRadius, chipGroup.ChipCornerRadius);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("Item1")]
		[InlineData(42)]
		public void SelectedItem_SetValue_ReturnsExpectedValue(object? expectedItem)
		{
			var chipGroup = new SfChipGroup
			{
				SelectedItem = expectedItem
			};
			Assert.Equal(expectedItem, chipGroup.SelectedItem);
		}

		[Theory]
		[InlineData("Test View 1")]
		[InlineData("Test View 2")]
		public void InputView_SetValue_ReturnsExpectedValue(string labelText)
		{
			var chipGroup = new SfChipGroup();
			var expectedView = new Label { Text = labelText };
			Assert.Null(chipGroup.InputView);
			chipGroup.InputView = expectedView;
			Assert.Equal(expectedView, chipGroup.InputView);
		}

		[Fact]
		public void Command_SetValue_ReturnsExpectedValue()
		{
			var chipGroup = new SfChipGroup();
			var expectedCommand = new Command(() => { });
			Assert.Null((Command)chipGroup.Command);
			chipGroup.Command = expectedCommand;
			Assert.Equal(expectedCommand, chipGroup.Command);
		}

		[Theory]
		[InlineData(255, 255, 255)]
		public void ChipStroke_SetValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var chipGroup = new SfChipGroup();
			var expectedBrush = new SolidColorBrush(Color.FromRgb(r, g, b));
			Assert.NotNull(chipGroup.ChipStroke);
			chipGroup.ChipStroke = expectedBrush;
			Assert.Equal(expectedBrush, chipGroup.ChipStroke);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(5)]
		public void ChipStrokeThickness_SetValue_ReturnsExpectedValue(double expectedThickness)
		{
			var chipGroup = new SfChipGroup
			{
				ChipStrokeThickness = expectedThickness
			};
			Assert.Equal(expectedThickness, chipGroup.ChipStrokeThickness);
		}

		[Theory]
		[InlineData(255, 255, 255)]
		public void ChipTextColor_SetValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var chipGroup = new SfChipGroup();
			var expectedColor = Color.FromRgb(r, g, b);
			Assert.NotNull(chipGroup.ChipTextColor);
			chipGroup.ChipTextColor = expectedColor;
			Assert.Equal(expectedColor, chipGroup.ChipTextColor);
		}

		[Theory]
		[InlineData(255, 255, 255)]
		public void SelectedChipTextColor_SetValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var chipGroup = new SfChipGroup();
			var expectedColor = Color.FromRgb(r, g, b);
			chipGroup.SelectedChipTextColor = expectedColor;
			Assert.Equal(expectedColor, chipGroup.SelectedChipTextColor);
		}

		[Theory]
		[InlineData(255, 255, 255)]
		public void SelectedChipBackground_SetValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var chipGroup = new SfChipGroup();
			var expectedBrush = new SolidColorBrush(Color.FromRgb(r, g, b));
			chipGroup.SelectedChipBackground = expectedBrush;
			Assert.Equal(expectedBrush, chipGroup.SelectedChipBackground);
		}

		[Theory]
		[InlineData("#FF00FF")]
		public void SelectedChipBackground_SetValueUsingConverter_ReturnsExpectedValue(string colorHex)
		{
			var chipGroup = new SfChipGroup();
			var converter = new BrushTypeConverter();
			var expectedBrush = converter.ConvertFromString(colorHex) as SolidColorBrush;
			if (expectedBrush == null)
			{
				Assert.Fail($"Failed to convert hex color '{colorHex}' to a SolidColorBrush.");
			}
			chipGroup.SelectedChipBackground = expectedBrush;
			Assert.Equal(expectedBrush, chipGroup.SelectedChipBackground);
		}

		[Theory]
		[InlineData(-12.0)]
		[InlineData(14.5)]
		[InlineData(106.0)]
		public void ChipTextSize_SetValue_ReturnsExpectedValue(double expectedSize)
		{
			var chipGroup = new SfChipGroup
			{
				ChipTextSize = expectedSize
			};
			Assert.Equal(expectedSize, chipGroup.ChipTextSize);
		}

		[Theory]
		[InlineData("Arial")]
		[InlineData("Times New Roman")]
		[InlineData("Courier New")]
		public void ChipFontFamily_SetValue_ReturnsExpectedValue(string expectedFontFamily)
		{
			var chipGroup = new SfChipGroup
			{
				ChipFontFamily = expectedFontFamily
			};
			Assert.Equal(expectedFontFamily, chipGroup.ChipFontFamily);
		}

		[Theory]
		[InlineData("#FFFF0000")]
		public void ChipBackground_SetSolidColorBrush_ReturnsExpectedBrush(string colorHex)
		{
			var chipGroup = new SfChipGroup();
			var expectedBrush = new SolidColorBrush(Color.FromArgb(colorHex));
			chipGroup.ChipBackground = expectedBrush;
			Assert.Equal(expectedBrush, chipGroup.ChipBackground);
		}

		[Theory]
		[InlineData("#FF0000")]
		public void ChipBackground_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var chipGroup = new SfChipGroup();
			var converter = new BrushTypeConverter();
			var expectedBrush = converter.ConvertFromString(colorHex) as SolidColorBrush;
			if (expectedBrush == null)
			{
				Assert.Fail($"Failed to convert hex color '{colorHex}' to a SolidColorBrush.");
			}
			chipGroup.ChipBackground = expectedBrush;
			Assert.Equal(expectedBrush, chipGroup.ChipBackground);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		public void SelectionIndicatorColor_SetValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var chipGroup = new SfChipGroup();
			var expectedColor = Color.FromRgb(r, g, b);
			chipGroup.SelectionIndicatorColor = expectedColor;
			Assert.Equal(expectedColor, chipGroup.SelectionIndicatorColor);
		}

		[Theory]
		[InlineData(255, 255, 0)]
		public void CloseButtonColor_SetValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var chipGroup = new SfChipGroup();
			var expectedColor = Color.FromRgb(r, g, b);
			chipGroup.CloseButtonColor = expectedColor;
			Assert.Equal(expectedColor, chipGroup.CloseButtonColor);
		}

		[Theory]
		[InlineData(10, 10, 10, 10)]
		[InlineData(5, 10, 15, 20)]
		public void ChipPadding_SetValue_ReturnsExpectedValue(double left, double top, double right, double bottom)
		{
			var chipGroup = new SfChipGroup();
			var expectedPadding = new Thickness(left, top, right, bottom);
			chipGroup.ChipPadding = expectedPadding;
			Assert.Equal(expectedPadding, chipGroup.ChipPadding);
		}

		[Theory]
		[InlineData(-50)]
		[InlineData(100)]
		[InlineData(double.NaN)]
		public void ItemHeight_SetValue_ReturnsExpectedValue(double height)
		{
			var chipGroup = new SfChipGroup
			{
				ItemHeight = height
			};
			Assert.Equal(height, chipGroup.ItemHeight);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowIcon_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var chipGroup = new SfChipGroup
			{
				ShowIcon = expectedValue
			};
			Assert.Equal(expectedValue, chipGroup.ShowIcon);
		}

		[Theory]
		[InlineData(ChoiceMode.Single)]
		[InlineData(ChoiceMode.SingleOrNone)]
		public void ChoiceMode_SetValue_ReturnsExpectedValue(ChoiceMode expectedMode)
		{
			var chipGroup = new SfChipGroup
			{
				ChoiceMode = expectedMode
			};
			Assert.Equal(expectedMode, chipGroup.ChoiceMode);
		}

		[Fact]
		public void SfChipGroup_ShouldHaveCorrectChipBackground()
		{
			var chipGroup = new SfChipGroup
			{
				ChipBackground = Colors.Violet
			};
			var actualBackground = chipGroup.ChipBackground;
			Assert.Equal(Colors.Violet, actualBackground);
		}

		[Fact]
		public void SfChipGroup_ShouldContainCorrectNumberOfChips()
		{
			var chipGroup = new SfChipGroup();

			Assert.NotNull(chipGroup.Items);
			chipGroup.Items.Add(new SfChip { Text = "Extra Small" });
			chipGroup.Items.Add(new SfChip { Text = "Small" });
			chipGroup.Items.Add(new SfChip { Text = "Medium" });
			chipGroup.Items.Add(new SfChip { Text = "Large" });
			chipGroup.Items.Add(new SfChip { Text = "Extra Large" });
			var actualChipCount = chipGroup.Items?.Count;
			Assert.Equal(5, actualChipCount);
		}

		[Fact]
		public void SfChipGroup_ShouldContainChipsWithCorrectText()
		{
			var chipGroup = new SfChipGroup();
			Assert.NotNull(chipGroup.Items);
			chipGroup.Items.Add(new SfChip { Text = "Extra Small" });
			chipGroup.Items.Add(new SfChip { Text = "Small" });
			chipGroup.Items.Add(new SfChip { Text = "Medium" });
			chipGroup.Items.Add(new SfChip { Text = "Large" });
			chipGroup.Items.Add(new SfChip { Text = "Extra Large" });
			Assert.Equal("Extra Small", chipGroup.Items?[0].Text);
			Assert.Equal("Small", chipGroup.Items?[1].Text);
			Assert.Equal("Medium", chipGroup.Items?[2].Text);
			Assert.Equal("Large", chipGroup.Items?[3].Text);
			Assert.Equal("Extra Large", chipGroup.Items?[4].Text);
		}
		#endregion

		#region Bindable Properties

		[Fact]
		public void Choice_NormalState_ShouldApplyBlackTextColorAndWhiteBackground()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice,
				DisplayMemberPath = "Name",
				ItemsSource = new ObservableCollection<string> { "John Doe" }
			};
			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Black });
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.White });
			var selectedState = new VisualState { Name = "Selected" };
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Green });
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.Violet });
			var commonStateGroup = new VisualStateGroup();
			commonStateGroup.States.Add(normalState);
			commonStateGroup.States.Add(selectedState);
			var visualStateGroupList = new VisualStateGroupList
			{
				commonStateGroup
			};
			VisualStateManager.SetVisualStateGroups(chipGroup, visualStateGroupList);
			VisualStateManager.GoToState(chipGroup, "Normal");
			Assert.Equal(Colors.Black, chipGroup.ChipTextColor);
			Assert.Equal(Colors.White, chipGroup.ChipBackground);
		}

		[Fact]
		public void Choice_SelectedState_ShouldApplyGreenTextColorAndVioletBackground()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice,
				DisplayMemberPath = "Name",
				ItemsSource = new ObservableCollection<string> { "John Doe" }
			};
			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Black });
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.White });
			var selectedState = new VisualState { Name = "Selected" };
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Green });
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.Violet });
			var commonStateGroup = new VisualStateGroup();
			commonStateGroup.States.Add(normalState);
			commonStateGroup.States.Add(selectedState);
			var visualStateGroupList = new VisualStateGroupList
			{
				commonStateGroup
			};
			VisualStateManager.SetVisualStateGroups(chipGroup, visualStateGroupList);
			VisualStateManager.GoToState(chipGroup, "Selected");
			Assert.Equal(Colors.Green, chipGroup.ChipTextColor);
			Assert.Equal(Colors.Violet, chipGroup.ChipBackground);
		}

		[Fact]
		public void Filter_NormalState_ShouldApplyBlackTextColorAndWhiteBackground()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter,
				DisplayMemberPath = "Name",
				ItemsSource = new ObservableCollection<string> { "John Doe" },
				SelectionIndicatorColor = Colors.White
			};
			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Black });
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.White });
			var selectedState = new VisualState { Name = "Selected" };
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Green });
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.Violet });
			var commonStateGroup = new VisualStateGroup();
			commonStateGroup.States.Add(normalState);
			commonStateGroup.States.Add(selectedState);
			var visualStateGroupList = new VisualStateGroupList
			{
				commonStateGroup
			};
			VisualStateManager.SetVisualStateGroups(chipGroup, visualStateGroupList);
			VisualStateManager.GoToState(chipGroup, "Normal");
			Assert.Equal(Colors.Black, chipGroup.ChipTextColor);
			Assert.Equal(Colors.White, chipGroup.ChipBackground);
		}

		[Fact]
		public void Filter_SelectedState_ShouldApplyGreenTextColorAndVioletBackground()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter,
				DisplayMemberPath = "Name",
				ItemsSource = new ObservableCollection<string> { "John Doe" },
				SelectionIndicatorColor = Colors.White
			};
			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Black });
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.White });
			var selectedState = new VisualState { Name = "Selected" };
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipTextColorProperty, Value = Colors.Green });
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.Violet });
			var commonStateGroup = new VisualStateGroup();
			commonStateGroup.States.Add(normalState);
			commonStateGroup.States.Add(selectedState);
			var visualStateGroupList = new VisualStateGroupList
			{
				commonStateGroup
			};
			VisualStateManager.SetVisualStateGroups(chipGroup, visualStateGroupList);
			VisualStateManager.GoToState(chipGroup, "Selected");
			Assert.Equal(Colors.Green, chipGroup.ChipTextColor);
			Assert.Equal(Colors.Violet, chipGroup.ChipBackground);
		}

		[Fact]
		public void NormalState_ShouldApplyWhiteBackground()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice,
				DisplayMemberPath = "Name",
				ItemsSource = new ObservableCollection<string> { "John Doe" }
			};
			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.White });
			var selectedState = new VisualState { Name = "Selected" };
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.Violet });
			var commonStateGroup = new VisualStateGroup();
			commonStateGroup.States.Add(normalState);
			commonStateGroup.States.Add(selectedState);
			var visualStateGroupList = new VisualStateGroupList
			{
				commonStateGroup
			};
			VisualStateManager.SetVisualStateGroups(chipGroup, visualStateGroupList);
			VisualStateManager.GoToState(chipGroup, "Normal");
			Assert.Equal(Colors.White, chipGroup.ChipBackground);
		}

		[Fact]
		public void SelectedState_ShouldApplyVioletBackground()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice,
				DisplayMemberPath = "Name",
				ItemsSource = new ObservableCollection<string> { "John Doe" }
			};
			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.White });
			var selectedState = new VisualState { Name = "Selected" };
			selectedState.Setters.Add(new Setter { Property = SfChipGroup.ChipBackgroundProperty, Value = Colors.Violet });
			var commonStateGroup = new VisualStateGroup();
			commonStateGroup.States.Add(normalState);
			commonStateGroup.States.Add(selectedState);
			var visualStateGroupList = new VisualStateGroupList
			{
				commonStateGroup
			};
			VisualStateManager.SetVisualStateGroups(chipGroup, visualStateGroupList);
			VisualStateManager.GoToState(chipGroup, "Selected");
			Assert.Equal(Colors.Violet, chipGroup.ChipBackground);
		}
		#endregion

		#region Public Methods

		[Fact]
		public void GetChips_ShouldReturnEmptyCollection_WhenNoChipsAreAdded()
		{
			var chipGroup = new SfChipGroup();
			var chips = chipGroup.GetChips();
			Assert.NotNull(chips);
			Assert.Empty(chips);
		}

		[Fact]
		public void GetChips_ShouldReturnChips_WhenItemsSourceIsSet()
		{
			var chipGroup = new SfChipGroup();
			var items = new ObservableCollection<string> { "Chip 1", "Chip 2" };
			chipGroup.ItemsSource = items;
			var chips = chipGroup.GetChips();
			Assert.NotNull(chips);
			Assert.Equal(2, chips.Count);
			Assert.Equal("Chip 1", chips[0].Text);
			Assert.Equal("Chip 2", chips[1].Text);
		}

		[Fact]
		public void GetChips_ShouldHandleNullItemsInItemsSource()
		{
			var chipGroup = new SfChipGroup();
			var items = new ObservableCollection<string?> { "Chip 1", null };
			chipGroup.ItemsSource = items;
			var chips = chipGroup.GetChips();
			Assert.NotNull(chips);
			Assert.Equal(2, chips.Count);
			Assert.Equal("Chip 1", chips[0].Text);
			Assert.Equal(string.Empty, chips[1].Text);
		}

		[Fact]
		public void GetChips_ShouldReturnChips_WhenItemsSourceIsSet_WithoutDisplayMemberPath()
		{
			var chipGroup = new SfChipGroup();
			var items = new ObservableCollection<string> { "Chip 1", "Chip 2" };
			chipGroup.ItemsSource = items;
			var chips = chipGroup.GetChips();
			Assert.NotNull(chips);
			Assert.Equal(2, chips.Count);
			Assert.Equal("Chip 1", chips[0].Text);
			Assert.Equal("Chip 2", chips[1].Text);
		}

		[Fact]
		public void GetChips_ShouldReturnEmptyCollection_WhenItemsSourceIsEmpty()
		{
			var chipGroup = new SfChipGroup();
			var items = new ObservableCollection<string>();
			chipGroup.ItemsSource = items;
			var chips = chipGroup.GetChips();
			Assert.NotNull(chips);
			Assert.Empty(chips);
		}
		[Fact]
		public void RemoveOldItem_ShouldRemoveChip_WhenPropertyNameIsItemsProperty()
		{
			var chipGroup = new SfChipGroup();
			var items = new ObservableCollection<string> { "Chip 1", "Chip 2", "Old Chip" };
			chipGroup.ItemsSource = items;
			InvokePrivateMethod(chipGroup, "RemoveOldItem", nameof(SfChipGroup.ItemsSource), "Old Chip");
			var chipsAfterRemoval = chipGroup.GetChips();
			Assert.DoesNotContain(chipsAfterRemoval, c => c.Text == "Old Chip");
		}

		[Fact]
		public void RemoveOldItem_ShouldDoNothing_WhenChipIsNull()
		{
			var chipGroup = new SfChipGroup();
			object? invalidOldItem = null;
			InvokePrivateMethod(chipGroup, "RemoveOldItem", SfChipGroup.ItemsProperty.PropertyName, invalidOldItem);
			Assert.Empty(chipGroup.GetChips());
		}

		[Fact]
		public void RemoveOldItem_ShouldDoNothing_WhenPropertyNameDoesNotMatchAndGetChipReturnsNull()
		{
			var chipGroup = new SfChipGroup();
			var oldItem = new object();
			InvokePrivateMethod(chipGroup, "RemoveOldItem", "SomeOtherProperty", oldItem);
			Assert.Empty(chipGroup.GetChips());
		}
		#endregion

		#region Private Methods

		[Fact]
		public void GetPropertyValue_ShouldReturnCorrectValue()
		{
			var collection = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
			var methodInfo = typeof(SfChipGroup).GetMethod("GetPropertyValue", BindingFlags.NonPublic | BindingFlags.Static);
			var result = methodInfo?.Invoke(null, ["Count", collection]);
			Assert.Equal(3, result);
		}

		[Fact]
		public void GetPropertyValue_ShouldReturnValue_WhenValidProperty()
		{
			var item = new { Name = "Test", Age = 25 };
			var methodInfo = typeof(SfChipGroup).GetMethod("GetPropertyValue", BindingFlags.NonPublic | BindingFlags.Static);
			var result = methodInfo?.Invoke(null, ["Name", item]);
			Assert.NotNull(result);
			Assert.Equal("Test", result);
		}

		[Fact]
		public void GetPropertyValue_ShouldReturnEmptyString_WhenInvalidProperty()
		{
			var item = new { Name = "Test", Age = 25 };
			var methodInfo = typeof(SfChipGroup).GetMethod("GetPropertyValue", BindingFlags.NonPublic | BindingFlags.Static);
			var result = methodInfo?.Invoke(null, ["InvalidProp", item]);
			Assert.NotNull(result);
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void GetPropertyValue_ShouldThrowException_WhenItemIsNull()
		{
			object? item = null;
			var methodInfo = typeof(SfChipGroup).GetMethod("GetPropertyValue", BindingFlags.NonPublic | BindingFlags.Static);
			Assert.NotNull(methodInfo);
			var parameters = new object?[] { "Name", item };
			var exception = Assert.Throws<TargetInvocationException>(() => methodInfo?.Invoke(null, parameters));
			Assert.IsType<NullReferenceException>(exception.InnerException);
		}

		[Fact]
		public void GetPropertyValue_ShouldReturnEmptyString_WhenPropertyNameIsEmpty()
		{
			var item = new { Name = "Test", Age = 25 };
			var methodInfo = typeof(SfChipGroup).GetMethod("GetPropertyValue", BindingFlags.NonPublic | BindingFlags.Static);
			var result = methodInfo?.Invoke(null, [string.Empty, item]);
			Assert.NotNull(result);
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void GetPropertyValue_ShouldReturnEmptyString_WhenPropertyValueIsNull()
		{
			var item = new { Name = "Test", Age = 25, MiddleName = (string?)null };
			var methodInfo = typeof(SfChipGroup).GetMethod("GetPropertyValue", BindingFlags.NonPublic | BindingFlags.Static);
			var result = methodInfo?.Invoke(null, ["MiddleName", item]);
			Assert.NotNull(result);
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void GetPropertyValue_ShouldReturnEmptyString_WhenPropertyNameIsNull()
		{
			var item = new { Name = "Test", Age = 25 };
			var methodInfo = typeof(SfChipGroup).GetMethod("GetPropertyValue", BindingFlags.NonPublic | BindingFlags.Static);
			var result = methodInfo?.Invoke(null, [string.Empty, item]);
			Assert.NotNull(result);
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void DeselectChip_ShouldHideSelectionIndicator()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip();
			SetNonPublicProperty(chip, "IsSelected", true);
			chip.ShowSelectionIndicator = true;
			InvokePrivateMethod(chipGroup, "DeselectChip", chip);
			Assert.False(chip.ShowSelectionIndicator);
		}

		[Fact]
		public void DeselectChip_ShouldCallApplyChipColor()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip();
			SetNonPublicProperty(chip, "IsSelected", true);

			_ = chip.ShowSelectionIndicator;
			InvokePrivateMethod(chipGroup, "DeselectChip", chip);
			Assert.False(chip.ShowSelectionIndicator);
			Assert.False(chip.IsSelected);
		}

		[Theory]
		[InlineData(null)]
		[InlineData(0)]
		public void SelectOrUnselectMultipleChips_ShouldNotThrow_WhenListIsNullOrEmpty(int? count)
		{
			var chipGroup = new SfChipGroup();
			List<object>? chips = count == null ? null : [];
			var exception = Record.Exception(() => InvokePrivateMethod(chipGroup, "SelectOrUnselectMultipleChips", chips));
			Assert.Null(exception);
		}

		[Fact]
		public void SelectOrUnselectMultipleChips_WithEmptyList_DoesNothing()
		{
			var chipGroup = new SfChipGroup();
			var emptyList = new ObservableCollection<object>();
			InvokePrivateMethod(chipGroup, "SelectOrUnselectMultipleChips", emptyList);
			Assert.Empty(emptyList);
		}

		[Fact]
		public void ConfigureActionChip_SetsShowCloseButtonToFalse()
		{
			var chip = new SfChip
			{
				ShowCloseButton = true
			};
			var chipGroup = new SfChipGroup();
			InvokePrivateMethod(chipGroup, "ConfigureActionChip", chip);
			Assert.False(chip.ShowCloseButton);
		}

		[Fact]
		public void ConfigureActionChip_SetsShowSelectionIndicatorToFalse()
		{
			var chip = new SfChip
			{
				ShowSelectionIndicator = true
			};
			var chipGroup = new SfChipGroup();
			InvokePrivateMethod(chipGroup, "ConfigureActionChip", chip);
			Assert.False(chip.ShowSelectionIndicator);
		}

		[Theory]
		[InlineData(true, true, false, false)]
		[InlineData(false, false, false, false)]
		public void ConfigureActionChip_WithInitialValues_SetsPropertiesCorrectly(bool initialShowCloseButton, bool initialShowSelectionIndicator, bool expectedShowCloseButton, bool expectedShowSelectionIndicator)
		{
			var chip = new SfChip
			{
				ShowCloseButton = initialShowCloseButton,
				ShowSelectionIndicator = initialShowSelectionIndicator
			};
			var chipGroup = new SfChipGroup();
			InvokePrivateMethod(chipGroup, "ConfigureActionChip", chip);
			Assert.Equal(expectedShowCloseButton, chip.ShowCloseButton);
			Assert.Equal(expectedShowSelectionIndicator, chip.ShowSelectionIndicator);
		}

		[Theory]
		[InlineData(true, true, false, false)]
		[InlineData(false, false, false, false)]
		public void ConfigureActionChip_MultipleCalls_DoesNotCauseSideEffects(bool initialShowCloseButton, bool initialShowSelectionIndicator, bool expectedShowCloseButton, bool expectedShowSelectionIndicator)
		{
			var chip = new SfChip
			{
				ShowCloseButton = initialShowCloseButton,
				ShowSelectionIndicator = initialShowSelectionIndicator
			};
			var chipGroup = new SfChipGroup();
			for (int i = 0; i < 10; i++)
			{
				InvokePrivateMethod(chipGroup, "ConfigureActionChip", chip);
			}
			Assert.Equal(expectedShowCloseButton, chip.ShowCloseButton);
			Assert.Equal(expectedShowSelectionIndicator, chip.ShowSelectionIndicator);

			InvokePrivateMethod(chipGroup, "ConfigureActionChip", chip);
			Assert.Equal(expectedShowCloseButton, chip.ShowCloseButton);
			Assert.Equal(expectedShowSelectionIndicator, chip.ShowSelectionIndicator);
		}

		[Fact]
		public void SelectOrUnselectMultipleChips_ShouldUnselectChips_WhenListContainsSelectedChips()
		{
			var chipGroup = new SfChipGroup();
			var chip1 = new SfChip();
			var chip2 = new SfChip();
			chipGroup.ItemsSource = new ObservableCollection<SfChip> { chip1, chip2 };
			SetNonPublicProperty(chip1, "IsSelected", false);
			SetNonPublicProperty(chip2, "IsSelected", false);
			var chipsToUnselect = new List<object> { chip1, chip2 };
			InvokePrivateMethod(chipGroup, "SelectOrUnselectMultipleChips", chipsToUnselect);
			Assert.False(chip1.IsSelected);
			Assert.False(chip2.IsSelected);
		}

		[Fact]
		public void UnselectChips_WithEmptyList_ShouldNotThrowException()
		{
			var chipGroup = new SfChipGroup();
			IList<object> emptyList = [];
			var exception = Record.Exception(() => InvokePrivateMethod(chipGroup, "UnselectChips", emptyList));
			Assert.Null(exception);
		}

		[Fact]
		public void UnselectChips_WithNullValues_ShouldNotThrowException()
		{
			var chipGroup = new SfChipGroup();
			IList<object?> listWithNulls = [null, null, null];
			var exception = Record.Exception(() => InvokePrivateMethod(chipGroup, "UnselectChips", listWithNulls));
			Assert.Null(exception);
		}

		[Fact]
		public void UnselectChips_ShouldUnselectFilterChips_WhenChipTypeIsFilter()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter
			};
			var chip1 = new SfChip();
			var chip2 = new SfChip();
			chipGroup.ItemsSource = new ObservableCollection<SfChip> { chip1, chip2 };
			SetNonPublicProperty(chip1, "IsSelected", true);
			SetNonPublicProperty(chip2, "IsSelected", true);
			InvokePrivateMethod(chipGroup, "UnselectChips", new List<object> { chip1, chip2 });
			Assert.False(chip1.IsSelected);
			Assert.False(chip2.IsSelected);
		}

		[Fact]
		public void UnselectChips_ShouldUnselectOnlySelectedFilterChips()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter
			};
			var chip1 = new SfChip() { IsSelected = true };
			var chip2 = new SfChip() { IsSelected = true };
			var chip3 = new SfChip() { IsSelected = true };
			InvokePrivateMethod(chipGroup, "UnselectChips", new List<object> { chip1, chip2, chip3 });
			Assert.False(chip1.IsSelected);
			Assert.False(chip3.IsSelected);
			Assert.False(chip2.IsSelected);
		}

		[Fact]
		public void UnselectFilterChip_ShouldDoNothing_WhenChipTypeIsNotFilter()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice
			};
			var chip = new SfChip();
			InvokePrivateMethod(chipGroup, "UnselectFilterChip", chip);
			Assert.False(chip.IsSelected);
		}

		[Fact]
		public void UnselectFilterChip_ShouldUnselectChipAndRemoveFromSelectedItems_WhenSelectedItemIsNull()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter
			};
			var chip = new SfChip();
			SetNonPublicProperty(chip, "IsSelected", true);
			SetNonPublicProperty(chipGroup, "SelectedItem", null);
			InvokePrivateMethod(chipGroup, "UnselectFilterChip", chip);
			Assert.False(chip.IsSelected);
			Assert.False(chip.ShowSelectionIndicator);
			Assert.Null(chipGroup.SelectedItem);
		}

		[Fact]
		public void UnselectFilterChip_ShouldUnselectChipButKeepInSelectedItems_WhenItemTemplateIsNotNull()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter
			};
			var chip = new SfChip();
			SetNonPublicProperty(chip, "IsSelected", true);
			SetNonPublicProperty(chipGroup, "SelectedItem", chip);
			SetNonPublicProperty(chipGroup, "ItemTemplate", new DataTemplate());
			InvokePrivateMethod(chipGroup, "UnselectFilterChip", chip);
			Assert.False(chip.IsSelected);
			Assert.False(chip.ShowSelectionIndicator);
			Assert.Same(chip, chipGroup.SelectedItem);
		}

		[Fact]
		public void HandleClicked_ShouldDoNothing_WhenChipIsNull()
		{
			var chipGroup = new SfChipGroup();
			SfChip? chip = null;
			InvokePrivateMethod(chipGroup, "HandleClicked", chip);
		}

		[Fact]
		public void HandleClicked_ShouldExecuteCommand_WhenChipIsActionAndEnabled()
		{
			bool commandExecuted = false;
			var chipGroup = new SfChipGroup
			{
				Command = new Command((parameter) => commandExecuted = true),
				ChipType = SfChipsType.Action
			};
			var chip = new SfChip { IsEnabled = true };
			InvokePrivateMethod(chipGroup, "HandleClicked", chip);
			Assert.True(commandExecuted, "The command should be executed when the chip is action and enabled.");
		}

		[Fact]
		public void HandleClicked_ShouldNotExecuteCommand_WhenChipIsActionButNoCommand()
		{
			var chipGroup = new SfChipGroup { ChipType = SfChipsType.Action };
			var chip = new SfChip { IsEnabled = true };
			InvokePrivateMethod(chipGroup, "HandleClicked", chip);
		}

		[Fact]
		public void HandleClicked_ShouldInvokeChipClickedEvent_WhenChipIsClicked()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip();
			bool eventInvoked = false;
			chipGroup.ChipClicked += (sender, e) => eventInvoked = true;
			InvokePrivateMethod(chipGroup, "HandleClicked", chip);
			Assert.True(eventInvoked, "The ChipClicked event should be invoked when the chip is clicked.");
		}

		[Theory]
		[InlineData(50, 50)]
		[InlineData(double.NaN, -1)]
		public void SetChipHeight_Should_SetOrNotSetHeightRequest_BasedOnItemHeight(double itemHeight, double expectedHeightRequest)
		{
			var chipGroup = new SfChipGroup { ItemHeight = itemHeight };
			var chip = new SfChip { IsCreatedInternally = true };
			InvokePrivateMethod(chipGroup, "SetChipHeight", [chip]);
			Assert.Equal(expectedHeightRequest, chip.HeightRequest);
		}

		[Fact]
		public void SetChipHeight_ShouldNotSetHeightRequest_WhenChipIsNotCreatedInternally()
		{
			var chipGroup = new SfChipGroup { ItemHeight = 50 };
			var chip = new SfChip { IsCreatedInternally = false };
			InvokePrivateMethod(chipGroup, "SetChipHeight", chip);
			Assert.Equal(-1, chip.HeightRequest);
		}

		[Fact]
		public void RemoveChipFromSelectedItems_ShouldNotRemove_WhenSelectedItemIsNull()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip();
			chipGroup.SelectedItem = null;
			InvokePrivateMethod(chipGroup, "RemoveChipFromSelectedItems", chip);
			Assert.Null(chipGroup.SelectedItem);
		}

		[Fact]
		public void RemoveChipFromSelectedItems_ShouldNotRemove_WhenSelectedItemIsEmptyList()
		{

			var chipGroup = new SfChipGroup();
			var chip = new SfChip();
			var emptyList = new List<object>();
			chipGroup.SelectedItem = emptyList;
			InvokePrivateMethod(chipGroup, "RemoveChipFromSelectedItems", chip);
			Assert.Empty(emptyList);
		}

		[Fact]
		public void RemoveChipFromSelectedItems_ShouldNotRemove_WhenChipDataIsNotInList()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip();

			_ = new SfChip();
			var selectedList = new List<object> { "SomeOtherData" };
			chipGroup.SelectedItem = selectedList;
			InvokePrivateMethod(chipGroup, "RemoveChipFromSelectedItems", chip);
			Assert.Single(selectedList);
		}

		[Fact]
		public void RemoveChipFromSelectedItems_ShouldRemove_WhenChipDataIsInList()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip();
			var chipDataMock = new object();
			chip.IsCreatedInternally = true;
			SetNonPublicProperty(chip, "DataContext", chipDataMock);
			var selectedList = new List<object> { chipDataMock };
			chipGroup.SelectedItem = selectedList;

			_ = GetNonPublicProperty(chip, "DataContext");
			InvokePrivateMethod(chipGroup, "RemoveChipFromSelectedItems", chip);
			Assert.Empty(selectedList);
		}

		[Fact]
		public void UnselectChoiceChip_ShouldDoNothing_WhenPreviouslySelectedChipIsNull()
		{
			var chipGroup = new SfChipGroup();
			SetPrivateField(chipGroup, "_previouslySelectedChip", null);
			InvokePrivateMethod(chipGroup, "UnselectChoiceChip");
			Assert.Null(GetPrivateField(chipGroup, "_previouslySelectedChip"));
		}

		[Fact]
		public void UnselectChoiceChip_ShouldDoNothing_WhenChipLayoutIsNull()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip { IsSelected = true };
			SetPrivateField(chipGroup, "_previouslySelectedChip", chip);
			SetNonPublicProperty(chipGroup, "ChipLayout", null);
			InvokePrivateMethod(chipGroup, "UnselectChoiceChip");
			Assert.True(chip.IsSelected);
			Assert.Equal(chipGroup.ChipStrokeThickness, chip.StrokeThickness);
		}

		[Fact]
		public void UnselectChoiceChip_ShouldSetIsSelectedToFalse_WhenChipExistsInChipLayout()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip { IsSelected = true };
			SetPrivateField(chipGroup, "_previouslySelectedChip", chip);
			var chipLayout = new StackLayout();
			chipLayout.Children.Add(chip);
			SetNonPublicProperty(chipGroup, "ChipLayout", chipLayout);
			InvokePrivateMethod(chipGroup, "UnselectChoiceChip");
			Assert.False(chip.IsSelected);
			Assert.Equal(chipGroup.ChipStrokeThickness, chip.StrokeThickness);
		}

		[Fact]
		public void UnselectChoiceChip_ShouldNotChangeProperties_WhenChipNotFoundInChipLayout()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip { IsSelected = true };
			SetPrivateField(chipGroup, "_previouslySelectedChip", chip);
			var chipLayout = new StackLayout();
			SetNonPublicProperty(chipGroup, "ChipLayout", chipLayout);
			InvokePrivateMethod(chipGroup, "UnselectChoiceChip");
			Assert.True(chip.IsSelected);
			Assert.Equal(chipGroup.ChipStrokeThickness, chip.StrokeThickness);
		}

		[Fact]
		public void UnselectFilterChip_ShouldSetChipAsSelected_WhenSelectedItemIsNotNullAndItemTemplateIsDefault()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip();
			SetNonPublicProperty(chip, "IsSelected", true);
			chipGroup.ChipType = SfChipsType.Filter;
			chipGroup.SelectedItem = new object();
			InvokePrivateMethod(chipGroup, "UnselectFilterChip", chip);
			Assert.True(chip.ShowSelectionIndicator);
		}

		[Fact]
		public void UnselectFilterChip_ShouldNotChangeChipSelection_WhenItemTemplateIsNotNull()
		{
			var chipGroup = new SfChipGroup();
			var chip = new SfChip { IsSelected = true };
			chipGroup.ChipType = SfChipsType.Filter;
			chipGroup.SelectedItem = new object();
			chipGroup.ItemTemplate = new DataTemplate();
			InvokePrivateMethod(chipGroup, "UnselectFilterChip", chip);
			Assert.False(chip.ShowSelectionIndicator);
			Assert.False(chip.IsSelected);
		}

		[Fact]
		public void UnselectpreviousChip_ShouldUnselectAllFilterChips_WhenChipsTypeIsFilter()
		{
			var chipGroup = new SfChipGroup();
			var chip1 = new SfChip();
			var chip2 = new SfChip();
			SetNonPublicProperty(chip1, "IsSelected", true);
			SetNonPublicProperty(chip2, "IsSelected", true);
			SetPrivateField(chipGroup, "_chipGroupChildren", new List<SfChip> { chip1, chip2 });
			InvokePrivateMethod(chipGroup, "UnselectpreviousChip", SfChipsType.Filter);
			Assert.False(chip1.ShowSelectionIndicator);
			Assert.False(chip2.ShowSelectionIndicator);
		}

		[Fact]
		public void UnselectpreviousChip_ShouldDoNothing_WhenChipGroupChildrenIsNull()
		{
			var chipGroup = new SfChipGroup();
			SetPrivateField(chipGroup, "_chipGroupChildren", null);
			var initialSelectedItem = chipGroup.SelectedItem;
			var initialChipStrokeThickness = chipGroup.ChipStrokeThickness;
			InvokePrivateMethod(chipGroup, "UnselectpreviousChip", SfChipsType.Filter);
			Assert.Equal(initialSelectedItem, chipGroup.SelectedItem);
			Assert.Equal(initialChipStrokeThickness, chipGroup.ChipStrokeThickness);
		}

		[Fact]
		public void HandleInputView_ShouldAddChild_WhenInputViewDoesNotExist()
		{
			var chipGroup = new SfChipGroup();
			var chipLayout = new StackLayout();
			SetNonPublicProperty(chipGroup, "ChipLayout", chipLayout);
			var child = new SfChip();
			InvokePrivateMethod(chipGroup, "HandleInputView", child);
			Assert.Contains(child, chipLayout.Children);
			Assert.Equal(1, chipLayout?.Children.Count);
		}
		#endregion

	}
}
