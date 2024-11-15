using Syncfusion.Maui.Toolkit.Chips;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfChipsUnitTests : BaseUnitTest
	{
		#region Fields
		private readonly BrushTypeConverter _converter = new BrushTypeConverter();
		#endregion

		#region Constructor
		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var chips = new SfChip();
			Assert.Equal(Color.FromArgb("#1C1B1F"), chips.CloseButtonColor);
			Assert.Equal(Color.FromArgb("#49454F"), chips.TextColor);
			Assert.Equal(TextAlignment.Center, chips.VerticalTextAlignment);
			Assert.Equal(Color.FromArgb("#49454F"), chips.SelectionIndicatorColor);
			Assert.Equal(Colors.Transparent, chips.Background);
			Assert.Equal(8, chips.CornerRadius);
			Assert.Equal(FontAttributes.None, chips.FontAttributes);
			Assert.Equal(14d, chips.FontSize);
			Assert.Equal(TextAlignment.Center, chips.HorizontalTextAlignment);
			Assert.Equal(Alignment.Start, chips.ImageAlignment);
			Assert.Equal(18, chips.ImageSize);
			Assert.Equal(1f, chips.StrokeThickness);
			Assert.Equal(0, chips.Padding);
			Assert.True(chips.EnableRippleEffect);
			Assert.False(chips.FontAutoScalingEnabled);
			Assert.False(chips.ShowCloseButton);
			Assert.False(chips.ShowSelectionIndicator);
			Assert.False(chips.ShowIcon);
			Assert.Null(chips.BackgroundImageSource);
			Assert.Null(chips.Command);
			Assert.Null(chips.CommandParameter);
			Assert.Null(chips.FontFamily);
			Assert.Null(chips.ImageSource);
			Assert.Empty(chips.CloseButtonPath);
			Assert.Empty(chips.Text);
		}
		#endregion

		#region Properties

		[Theory]
		[InlineData("#ef2e05")]
		[InlineData("#05ef6c")]
		[InlineData("#053def")]
		public void CloseButtonColor_SetValue_ReturnsExpectedValue(string colorName)
		{
			var chips = new SfChip();
			var expectedColor = Color.FromArgb(colorName);
			chips.CloseButtonColor = expectedColor;
			var actualColor = chips.CloseButtonColor;
			Assert.Equal(expectedColor, actualColor);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowCloseButton_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var chip = new SfChip();
			chip.ShowCloseButton = expectedValue;
			var actualValue = chip.ShowCloseButton;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowSelectionIndicator_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var chip = new SfChip();
			chip.ShowSelectionIndicator = expectedValue;
			var actualValue = chip.ShowSelectionIndicator;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("#ef2e05")]
		[InlineData("#05ef6c")]
		[InlineData("#053def")]
		public void SelectionIndicator_SetValue_ReturnsExpectedValue(string colorName)
		{
			var chip = new SfChip();
			var expectedValue = Color.FromArgb(colorName);
			chip.SelectionIndicatorColor = expectedValue;
			var actualValue = chip.SelectionIndicatorColor;
			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void SelectionIndicatorColorValue_Get_ReturnsExpectedColor()
		{
			var sfChip = new SfChip();
			var expectedColor = Colors.Blue;
			var selectionIndicatorColorValueField = typeof(SfChip).GetField("_selectionIndicatorColorValue", BindingFlags.NonPublic | BindingFlags.Instance);
			if (selectionIndicatorColorValueField == null)
				throw new InvalidOperationException("Field 'selectionIndicatorColorValue' not found.");

			selectionIndicatorColorValueField.SetValue(sfChip, expectedColor);
			var actualColor = (Color?)GetNonPublicProperty(sfChip, "SelectionIndicatorColorValue");
			Assert.Equal(expectedColor, actualColor);
		}

		[Theory]
		[InlineData("M10 10 H 90 V 90 H 10 Z")]
		[InlineData("M20 20 H 80 V 80 H 20 Z")]
		[InlineData("")]
		public void CloseButtonPath_SetValue_ReturnsExpectedValue(string expectedValue)
		{
			var chip = new SfChip();
			chip.CloseButtonPath = expectedValue;
			var actualValue = chip.CloseButtonPath;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(0, 0, 0, 0)]
		[InlineData(5, 5, 5, 5)]
		[InlineData(10, 15, 20, 25)]
		public void CornerRadius_SetValue_ReturnsExpectedValue(double topLeft, double topRight, double bottomLeft, double bottomRight)
		{
			var chip = new SfChip();
			var expectedCornerRadius = new CornerRadius(topLeft, topRight, bottomLeft, bottomRight);
			chip.CornerRadius = expectedCornerRadius;
			var actualCornerRadius = chip.CornerRadius;
			Assert.Equal(expectedCornerRadius, actualCornerRadius);
		}

		[Theory]
		[InlineData(0.0)]
		[InlineData(1.5)]
		[InlineData(5)]
		[InlineData(10.0)]
		public void StrokeThickness_SetValue_ReturnsExpectedValue(double expectedThickness)
		{
			var chip = new SfChip();
			chip.StrokeThickness = expectedThickness;
			var actualThickness = chip.StrokeThickness;
			Assert.Equal(expectedThickness, actualThickness);
		}

		[Theory]
		[InlineData("#FF0000")]
		[InlineData("#00FF00")]
		[InlineData("#0000FF")]
		public void Stroke_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var chip = new SfChip();
			var expectedBrush = GetSolidColorBrush(colorHex);
			chip.Stroke = expectedBrush;
			var actualBrush = chip.Stroke;
			Assert.Equal(expectedBrush.Color, actualBrush);
		}

		[Theory]
		[InlineData("#FFFFFF")]
		[InlineData("#FF5733")]
		[InlineData("#C70039")]
		public void Background_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var chip = new SfChip();
			var expectedBrush = GetSolidColorBrush(colorHex);
			chip.Background = expectedBrush;
			var actualBrush = chip.Background;
			Assert.Equal(expectedBrush.Color, actualBrush);
		}

		[Theory]
		[InlineData("sample")]
		[InlineData("sAmPlE")]
		[InlineData("SAMPLE")]
		public void Text_SetValue_ReturnsExpectedValue(string expectedText)
		{
			var chip = new SfChip();
			chip.Text = expectedText;
			var actualText = chip.Text;
			Assert.Equal(expectedText, actualText);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		public void TextColor_SetValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var chip = new SfChip();
			var expectedColor = Color.FromRgb(r, g, b);
			chip.TextColor = expectedColor;
			var actualColor = chip.TextColor;
			Assert.Equal(expectedColor, actualColor);
		}

		[Theory]
		[InlineData(12.0)]
		[InlineData(16.5)]
		[InlineData(24.0)]
		public void FontSize_SetValue_ReturnsExpectedValue(double expectedFontSize)
		{
			var chip = new SfChip();
			chip.FontSize = expectedFontSize;
			var actualFontSize = chip.FontSize;
			Assert.Equal(expectedFontSize, actualFontSize);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void FontAutoScalingEnabled_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var chip = new SfChip();
			chip.FontAutoScalingEnabled = expectedValue;
			var actualValue = chip.FontAutoScalingEnabled;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		public void HorizontalTextAlignment_SetValue_ReturnsExpectedValue(TextAlignment expectedValue)
		{
			var chip = new SfChip();
			chip.HorizontalTextAlignment = expectedValue;
			var actualValue = chip.HorizontalTextAlignment;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		public void VerticalTextAlignment_SetValue_ReturnsExpectedValue(TextAlignment expectedValue)
		{
			var chip = new SfChip();
			chip.VerticalTextAlignment = expectedValue;
			var actualValue = chip.VerticalTextAlignment;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("SampleImage1.png")]
		[InlineData("SampleImage2.png")]
		public void ImageSource_SetValue_ReturnsExpectedValue(string imagePath)
		{
			var chip = new SfChip();
			var expectedImageSource = ImageSource.FromFile(imagePath);
			chip.ImageSource = expectedImageSource;
			var actualImageSource = chip.ImageSource;
			Assert.Equal(expectedImageSource, actualImageSource);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowIcon_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var chip = new SfChip();
			chip.ShowIcon = expectedValue;
			var actualValue = chip.ShowIcon;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(10.0)]
		[InlineData(20.5)]
		[InlineData(0.0)]
		public void ImageSize_SetValue_ReturnsExpectedValue(double expectedValue)
		{
			var chip = new SfChip();
			chip.ImageSize = expectedValue;
			var actualValue = chip.ImageSize;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(Alignment.Top)]
		[InlineData(Alignment.Bottom)]
		[InlineData(Alignment.Left)]
		[InlineData(Alignment.Right)]
		public void ImageAlignment_SetValue_ReturnsExpectedValue(Alignment expectedValue)
		{
			var chip = new SfChip();
			chip.ImageAlignment = expectedValue;
			var actualValue = chip.ImageAlignment;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("SampleImage1.png")]
		[InlineData("SampleImage2.png")]
		public void BackgroundImageSource_SetValue_ReturnsExpectedValue(string imagePath)
		{
			var chip = new SfChip();
			var expectedValue = ImageSource.FromFile(imagePath);
			chip.BackgroundImageSource = expectedValue;
			var actualValue = chip.BackgroundImageSource;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("TestParameter1")]
		[InlineData(123)]
		[InlineData("")]
		public void CommandParameter_SetValue_ReturnsExpectedValue(object expectedParameter)
		{
			var chip = new SfChip();
			chip.CommandParameter = expectedParameter;
			var actualParameter = chip.CommandParameter;
			Assert.Equal(expectedParameter, actualParameter);
		}

		[Theory]
		[InlineData(10, 10, 10, 10)]
		[InlineData(5, 10, 5, 10)]
		[InlineData(0, 0, 0, 0)]
		public void Padding_SetValue_ReturnsExpectedValue(double left, double top, double right, double bottom)
		{
			var chip = new SfChip();
			var expectedPadding = new Thickness(left, top, right, bottom);
			chip.Padding = expectedPadding;
			var actualPadding = chip.Padding;
			Assert.Equal(expectedPadding, actualPadding);
		}

		[Theory]
		[InlineData("Arial")]
		[InlineData("Times New Roman")]
		[InlineData("Courier New")]
		[InlineData("")]
		public void FontFamily_SetValue_ReturnsExpectedValue(string expectedFontFamily)
		{
			var chip = new SfChip();
			chip.FontFamily = expectedFontFamily;
			var actualFontFamily = chip.FontFamily;
			Assert.Equal(expectedFontFamily, actualFontFamily);
		}

		[Theory]
		[InlineData(FontAttributes.None)]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		public void FontAttributes_SetValue_ReturnsExpectedValue(FontAttributes expectedAttributes)
		{
			var chip = new SfChip();
			chip.FontAttributes = expectedAttributes;
			var actualAttributes = chip.FontAttributes;
			Assert.Equal(expectedAttributes, actualAttributes);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableRippleEffect_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var chip = new SfChip();
			chip.EnableRippleEffect = expectedValue;
			var actualValue = chip.EnableRippleEffect;
			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void CloseButtonPath_ShouldReturnDefaultValue_WhenNotSet()
		{
			var sfChip = new SfChip();
			var defaultValue = sfChip.CloseButtonPath;
			Assert.Equal(string.Empty, defaultValue);
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
		#endregion

		#region Internal Properties
		[Fact]
		public void ChipCloseButtonColor_Get_ReturnsExpectedColor()
		{
			var sfChip = new SfChip();
			var expectedColor = Colors.Red;
			var chipCloseButtonColorField = typeof(SfChip).GetField("_chipCloseButtonColor", BindingFlags.NonPublic | BindingFlags.Instance);
			if (chipCloseButtonColorField == null)
				throw new InvalidOperationException("Field 'chipcloseButtonColor' not found.");

			chipCloseButtonColorField.SetValue(sfChip, expectedColor);
			var actualColor = (Color?)GetNonPublicProperty(sfChip, "ChipCloseButtonColor");
			Assert.Equal(expectedColor, actualColor);
		}

		[Fact]
		public void IsKeyDown_GetterSetter_WorksCorrectly()
		{
			var chip = new SfChip();
			chip.IsKeyDown = true;
			Assert.True(chip.IsKeyDown);
			chip.IsKeyDown = false;
			Assert.False(chip.IsKeyDown);
		}

		[Fact]
		public void IsCloseButtonClicked_Should_SetAndGetCorrectly()
		{
			var instance = new SfChip();
			Assert.NotNull(instance);
			SetPrivateField(instance, "IsCloseButtonClicked", true);
			bool? isCloseButtonClicked = (bool?)GetPrivateField(instance, "IsCloseButtonClicked");
			Assert.True(isCloseButtonClicked);
			SetPrivateField(instance, "IsCloseButtonClicked", false);
			isCloseButtonClicked = (bool?)GetPrivateField(instance, "IsCloseButtonClicked");
			Assert.False(isCloseButtonClicked);
		}
		#endregion

		#region Private Method
		private SolidColorBrush GetSolidColorBrush(string colorString)
		{
			var brush = _converter.ConvertFromString(colorString) as SolidColorBrush;
			if (brush == null)
			{
				throw new InvalidOperationException($"Failed to convert color: {colorString}");
			}
			return brush;
		}

		[Fact]
		public void CalculateHeight_ShouldReturnHeight_WhenHeightConstraintIsPositiveInfinity()
		{
			var chip = new SfChip();
			SetPrivateField(chip, "textHeightPadding", 5);
			SetNonPublicProperty(chip, "IsCreatedInternally", false);
			chip.ImageSize = 10;
			chip.ShowIcon = true;
			chip.Padding = new Thickness(5, 5, 5, 5);
			var result = InvokePrivateMethod(chip, "CalculateHeight", double.PositiveInfinity);
			Assert.Equal(34d, result);
		}

		[Fact]
		public void CalculateHeight_ShouldReturnHeight_WhenHeightConstraintIsZero()
		{
			var chip = new SfChip();
			SetPrivateField(chip, "textHeightPadding", 5);
			SetNonPublicProperty(chip, "IsCreatedInternally", false);
			chip.ImageSize = 10;
			chip.ShowIcon = true;
			chip.Padding = new Thickness(5, 5, 5, 5);
			var result = InvokePrivateMethod(chip, "CalculateHeight", 0d);
			Assert.Equal(0d, result);
		}

		[Fact]
		public void CalculateHeight_ShouldReturnHeight_WhenIsCreatedInternallyIsTrue()
		{
			var chip = new SfChip();
			SetPrivateField(chip, "textHeightPadding", 5);
			SetNonPublicProperty(chip, "IsCreatedInternally", true);
			chip.ImageSize = 10;
			chip.ShowIcon = true;
			chip.Padding = new Thickness(5, 5, 5, 5);
			var result = InvokePrivateMethod(chip, "CalculateHeight", double.PositiveInfinity);
			Assert.Equal(34d, result);
		}

		[Fact]
		public void SetChipHeight_ShouldSetHeightRequest_WhenItemHeightIsValid()
		{
			var chipGroup = new SfChipGroup { ItemHeight = 50 };
			var chip = new SfChip { IsCreatedInternally = true };
			InvokePrivateMethod(chipGroup, "SetChipHeight", chip);
			Assert.Equal(50, chip.HeightRequest);
		}

		[Theory]
		[InlineData("#FFFBFE", "#E8DEF8")]
		[InlineData("rgba(255, 251, 254, 1)", "#E8DEF8")]
		[InlineData("rgba(232, 223, 248, 1)", "#E8DEF8")]
		[InlineData("rgba(0, 0, 0, 0)", "#E8DEF8")]
		public void HandleEnabledState_Should_SetBaseBackgroundColor_ToDefault2_When_BackgroundIsNullOrDefault1(string default1Color, string expectedColor)
		{
			var instance = new SfChip();
			var default1Brush = _converter.ConvertFromString(default1Color) as SolidColorBrush;
			var expectedBrush = _converter.ConvertFromString(expectedColor) as SolidColorBrush;
			if (default1Brush == null || expectedBrush == null)
			{
				throw new InvalidOperationException($"Failed to convert colors: {default1Color} or {expectedColor}");
			}

			instance.Background = null;
			instance.ShowSelectionIndicator = true;
			InvokePrivateMethod(instance, "HandleEnabledState");
			Assert.NotNull(instance.BaseBackgroundColor);
			Assert.Equal(expectedBrush.Color, ((SolidColorBrush)instance.BaseBackgroundColor).Color);
		}

		[Theory]
		[InlineData("Red")]
		[InlineData("Green")]
		[InlineData("Blue")]
		[InlineData("rgba(255, 0, 0, 1)")]
		[InlineData("rgba(0, 255, 0, 1)")]
		[InlineData("rgba(0, 0, 255, 1)")]
		[InlineData("rgba(128, 128, 128, 1)")]
		public void HandleEnabledState_Should_SetBaseStrokeColor_When_StrokeIsNotNull(string strokeColorName)
		{
			var instance = new SfChip();
			var strokeBrush = _converter.ConvertFromString(strokeColorName) as SolidColorBrush;
			if (strokeBrush == null)
			{
				throw new InvalidOperationException($"Failed to convert '{strokeColorName}' to SolidColorBrush.");
			}

			instance.Stroke = strokeBrush;
			InvokePrivateMethod(instance, "HandleEnabledState");
			Assert.Equal(strokeBrush.Color, instance.BaseStrokeColor);
		}

		[Fact]
		public void HandleEnabledState_Should_SetSelectionIndicatorColorValue_And_ChipCloseButtonColor()
		{
			var instance = new SfChip();
			Color expectedSelectionIndicatorColor = Colors.Blue;
			Color expectedCloseButtonColor = Colors.Green;
			instance.SelectionIndicatorColor = expectedSelectionIndicatorColor;
			instance.CloseButtonColor = expectedCloseButtonColor;
			InvokePrivateMethod(instance, "HandleEnabledState");
			Assert.Equal(expectedSelectionIndicatorColor, instance.SelectionIndicatorColorValue);
			Assert.Equal(expectedCloseButtonColor, instance.ChipCloseButtonColor);
		}

		[Fact]
		public void HandleDisabledState_Should_KeepExistingColors_When_VisualStateGroupsPresent()
		{
			var instance = new SfChip
			{
				Background = Color.FromRgba(255, 255, 255, 255),
				SelectionIndicatorColor = Color.FromRgba(0, 255, 0, 255),
				CloseButtonColor = Color.FromRgba(255, 0, 0, 255),
				Stroke = new SolidColorBrush(Color.FromRgba(100, 100, 100, 255)),
				TextColor = Color.FromRgba(0, 0, 0, 255)
			};

			var visualStateGroupList = new VisualStateGroupList();
			VisualStateManager.SetVisualStateGroups(instance, visualStateGroupList);
			InvokePrivateMethod(instance, "HandleDisabledState");

			Assert.Equal(instance.Background, instance.BaseBackgroundColor);
			Assert.Equal(instance.SelectionIndicatorColor, instance.SelectionIndicatorColorValue);
			Assert.Equal(instance.CloseButtonColor, instance.ChipCloseButtonColor);
			Assert.Equal(((SolidColorBrush)instance.Stroke).Color, instance.BaseStrokeColor);
			Assert.Equal(instance.TextColor, instance.BaseTextColor);
		}

		[Fact]
		public void HandleDisabledState_HasVisualStateGroupsOrTheme_SetsDefaultColors()
		{
			var sfChip = new SfChip();
			if (Application.Current == null)
			{
				new Application();
			}

			if (Application.Current != null)
			{
				Application.Current.Resources["SfChipTheme"] = new object();
			}

			var backgroundBrush = _converter.ConvertFromString("#FFFBFE") as SolidColorBrush;
			var selectionIndicatorBrush = _converter.ConvertFromString("rgba(0, 255, 0, 1)") as SolidColorBrush;
			var closeButtonBrush = _converter.ConvertFromString("#FF0000") as SolidColorBrush;
			var strokeBrush = _converter.ConvertFromString("rgba(100, 100, 100, 1)") as SolidColorBrush;
			var textColor = Color.FromRgba(0, 0, 0, 255);

			if (backgroundBrush == null || selectionIndicatorBrush == null || closeButtonBrush == null || strokeBrush == null)
			{
				throw new InvalidOperationException("Failed to convert colors for default states.");
			}

			sfChip.Background = backgroundBrush.Color;
			sfChip.SelectionIndicatorColor = selectionIndicatorBrush.Color;
			sfChip.CloseButtonColor = closeButtonBrush.Color;
			sfChip.Stroke = strokeBrush;
			InvokePrivateMethod(sfChip, "HandleDisabledState");

			Assert.Equal(sfChip.Background, sfChip.BaseBackgroundColor);
			Assert.Equal(sfChip.SelectionIndicatorColor, sfChip.SelectionIndicatorColorValue);
			Assert.Equal(sfChip.CloseButtonColor, sfChip.ChipCloseButtonColor);
			Assert.Equal(((SolidColorBrush)sfChip.Stroke).Color, sfChip.BaseStrokeColor);
			Assert.Equal(sfChip.TextColor, sfChip.BaseTextColor);
		}


		[Fact]
		public void ChangeVisualState_Should_CallHandleEnabledState_When_IsEnabled()
		{
			var sfChip = new SfChip();
			sfChip.IsEnabled = true;
			InvokePrivateMethod(sfChip, "ChangeVisualState");
			Assert.True(sfChip.IsVisible);
		}

		[Fact]
		public void ChangeVisualState_Should_CallHandleDisabledState_When_IsNotEnabled()
		{
			var sfChip = new SfChip();
			sfChip.IsEnabled = false;
			InvokePrivateMethod(sfChip, "ChangeVisualState");
			Assert.True(sfChip.IsVisible);
		}

		[Fact]
		public void ApplyChipColor_UpdatesChipCorrectly()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice,
				ChipTextColor = Colors.Black,
				SelectedChipTextColor = Colors.White,
				ChipBackground = GetSolidColorBrush("#808080"),
				SelectedChipBackground = GetSolidColorBrush("#008000")
			};

			var chip = new SfChip();
			var isSelectedProperty = typeof(SfChip).GetProperty("IsSelected", BindingFlags.Public | BindingFlags.Instance);
			isSelectedProperty?.SetValue(chip, true);
			var methodInfo = typeof(SfChipGroup).GetMethod("ApplyChipColor", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(chipGroup, new object[] { chip });

			Assert.NotNull(chip.Background);
			Assert.Equal(Colors.White, chip.TextColor);
			Assert.Equal(Colors.Green, ((SolidColorBrush)chip.Background).Color);
		}

		[Fact]
		public void ApplyChipColor_ChoiceChip_Selected_ShouldApplySelectedColors()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice,
				ChipBackground = GetSolidColorBrush("#FFFFFF"),
				SelectedChipBackground = GetSolidColorBrush("#FF0000"),
				SelectedChipTextColor = Colors.Black
			};

			var chip = new SfChip();
			var isSelectedProperty = chip.GetType().GetProperty("IsSelected", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			isSelectedProperty?.SetValue(chip, true);
			InvokePrivateMethod(chipGroup, "ApplyChipColor", chip);

			Assert.NotNull(chip.Background);
			Assert.Equal(Colors.Red, ((SolidColorBrush)chip.Background).Color);
			Assert.Equal(Colors.Black, chip.TextColor);
		}

		[Fact]
		public void ApplyChipColor_FilterChip_Selected_ShouldApplySelectedColors()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter,
				SelectedChipBackground = GetSolidColorBrush("#FF0000")
			};

			var chip = new SfChip();
			var isSelectedProperty = typeof(SfChip).GetProperty("IsSelected", BindingFlags.Public | BindingFlags.Instance);
			isSelectedProperty?.SetValue(chip, true);
			InvokePrivateMethod(chipGroup, "ApplyChipColor", chip);

			Assert.NotNull(chip.Background);
			Assert.Equal(Colors.Red, ((SolidColorBrush)chip.Background).Color);
			Assert.Equal(chipGroup.SelectedChipTextColor, chip.TextColor);
		}

		[Fact]
		public void ApplyChipColor_ChoiceChip_NotSelected_ShouldApplyDefaultColors()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Choice,
				ChipBackground = GetSolidColorBrush("#FFFFFF"),
				ChipTextColor = Colors.Black
			};

			var chip = new SfChip();
			var isSelectedProperty = typeof(SfChip).GetProperty("IsSelected", BindingFlags.Public | BindingFlags.Instance);
			isSelectedProperty?.SetValue(chip, false);
			InvokePrivateMethod(chipGroup, "ApplyChipColor", chip);

			Assert.NotNull(chip.Background);
			Assert.Equal(Colors.White, ((SolidColorBrush)chip.Background).Color);
			Assert.Equal(Colors.Black, chip.TextColor);
		}

		[Fact]
		public void ApplyChipColor_FilterChip_NotSelected_ShouldApplyDefaultColors()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Filter,
				ChipBackground = GetSolidColorBrush("#808080"),
				ChipTextColor = Colors.Black
			};

			var chip = new SfChip();
			var isSelectedProperty = typeof(SfChip).GetProperty("IsSelected", BindingFlags.Public | BindingFlags.Instance);
			isSelectedProperty?.SetValue(chip, false);
			InvokePrivateMethod(chipGroup, "ApplyChipColor", chip);

			Assert.NotNull(chip.Background);
			Assert.Equal(Colors.Gray, ((SolidColorBrush)chip.Background).Color);
			Assert.Equal(Colors.Black, chip.TextColor);
		}

		[Fact]
		public void ApplyChipColor_OtherChipType_ShouldApplyDefaultProperties()
		{
			var chipGroup = new SfChipGroup
			{
				ChipType = SfChipsType.Input,
				ChipBackground = GetSolidColorBrush("#0000FF"),
				ChipTextColor = Colors.Red
			};

			var chip = new SfChip();
			var isSelectedProperty = typeof(SfChip).GetProperty("IsSelected", BindingFlags.Public | BindingFlags.Instance);
			isSelectedProperty?.SetValue(chip, false);
			InvokePrivateMethod(chipGroup, "ApplyChipColor", chip);

			Assert.NotNull(chip.Background);
			Assert.Equal(Colors.Blue, ((SolidColorBrush)chip.Background).Color);
			Assert.Equal(Colors.Red, chip.TextColor);
			Assert.Equal(chipGroup.CloseButtonColor, chip.CloseButtonColor);
		}

		#endregion

	}
}