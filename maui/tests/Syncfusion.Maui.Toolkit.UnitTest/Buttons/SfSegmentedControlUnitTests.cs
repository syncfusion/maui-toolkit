using System.Collections.ObjectModel;
using System.Reflection;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.SegmentedControl;
using Color = Microsoft.Maui.Graphics.Color;
using Size = Microsoft.Maui.Graphics.Size;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfSegmentedControlUnitTests : BaseUnitTest
	{
		#region Constructors
		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var segmentedControl = new SfSegmentedControl();
			Assert.True(segmentedControl.AutoScrollToSelectedSegment);
			Assert.Equal(20, segmentedControl.CornerRadius);
			Assert.Equal(Brush.Transparent, segmentedControl.DisabledSegmentBackground);
			Assert.Equal(Color.FromArgb("#1C1B1F61"), segmentedControl.DisabledSegmentTextColor);
			Assert.Null(segmentedControl.ItemsSource);
			Assert.Equal(Brush.Transparent, segmentedControl.SegmentBackground);
			Assert.Equal(0, segmentedControl.SegmentCornerRadius);
			Assert.Equal(38d, segmentedControl.SegmentHeight);
			Assert.Null(segmentedControl.SegmentTemplate);
			Assert.Equal(100d, segmentedControl.SegmentWidth);
			Assert.Null(segmentedControl.SelectedIndex);
			var color = new SolidColorBrush(Color.FromArgb("#79747E"));
			Assert.Equal(color.Color, ((SolidColorBrush)segmentedControl.Stroke).Color);
			Assert.True(segmentedControl.ShowSeparator);
			Assert.Equal(1, segmentedControl.StrokeThickness);
			Assert.Equal(-1, segmentedControl.VisibleSegmentsCount);
			Assert.Equal(SegmentSelectionMode.Single, segmentedControl.SelectionMode);
		}
		#endregion

		#region Public properties
		[Fact]
		public void ItemsSource_SetValue_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var itemsSource = new[] { "Item1", "Item2", "Item3" };
			segmentedControl.ItemsSource = itemsSource;
			Assert.Equal(itemsSource, segmentedControl.ItemsSource);
		}

		[Fact]
		public void ItemsSource_AddItem_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var itemsSource = new ObservableCollection<string> { "Item1", "Item2" };
			segmentedControl.ItemsSource = itemsSource;
			itemsSource.Add("Item3");
			Assert.Equal(3, ((ObservableCollection<string>)segmentedControl.ItemsSource).Count);
			Assert.Equal("Item3", ((ObservableCollection<string>)segmentedControl.ItemsSource)[2]);
		}

		[Fact]
		public void ItemsSource_RemoveItem_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
			segmentedControl.ItemsSource = itemsSource;
			itemsSource.Remove("Item2");
			Assert.Equal(2, ((ObservableCollection<string>)segmentedControl.ItemsSource).Count);
			Assert.Equal("Item1", ((ObservableCollection<string>)segmentedControl.ItemsSource)[0]);
			Assert.Equal("Item3", ((ObservableCollection<string>)segmentedControl.ItemsSource)[1]);
		}

		[Fact]
		public void ItemsSource_ReplaceItem_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
			segmentedControl.ItemsSource = itemsSource;
			itemsSource[1] = "NewItem2";
			Assert.Equal(3, ((ObservableCollection<string>)segmentedControl.ItemsSource).Count);
			Assert.Equal("Item1", ((ObservableCollection<string>)segmentedControl.ItemsSource)[0]);
			Assert.Equal("NewItem2", ((ObservableCollection<string>)segmentedControl.ItemsSource)[1]);
			Assert.Equal("Item3", ((ObservableCollection<string>)segmentedControl.ItemsSource)[2]);
		}

		[Fact]
		public void ItemsSource_ClearItems_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
			segmentedControl.ItemsSource = itemsSource;
			itemsSource.Clear();
			Assert.Empty(((ObservableCollection<string>)segmentedControl.ItemsSource));
		}

		[Fact]
		public void ItemsSource_ResetItems_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var itemsSource = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
			segmentedControl.ItemsSource = itemsSource;
			itemsSource = ["NewItem1", "NewItem2"];
			segmentedControl.ItemsSource = itemsSource;
			Assert.Equal(2, ((ObservableCollection<string>)segmentedControl.ItemsSource).Count);
			Assert.Equal("NewItem1", ((ObservableCollection<string>)segmentedControl.ItemsSource)[0]);
			Assert.Equal("NewItem2", ((ObservableCollection<string>)segmentedControl.ItemsSource)[1]);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(-1)]
		public void SelectedIndex_SetValue_ReturnsExpectedValue(int selectedIndex)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SelectedIndex = selectedIndex
			};
			Assert.Equal(selectedIndex, segmentedControl.SelectedIndex);
		}

		[Fact]
		public void SelectedIndex_SetValue_ThrowsNullReferenceException_WhenSegmentedControlIsNull()
		{
			SfSegmentedControl? segmentedControl = null;
			Assert.Throws<NullReferenceException>(() => segmentedControl!.SelectedIndex = 0);
		}

		[Theory]
		[InlineData(-50.0)]
		[InlineData(0.0)]
		[InlineData(200.0)]
		public void SegmentHeight_SetValue_ReturnsExpectedValue(double segmentHeight)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentHeight = segmentHeight
			};
			Assert.Equal(segmentHeight, segmentedControl.SegmentHeight);
		}

		[Fact]
		public void SegmentHeight_SetValue_ThrowsNullReferenceException_WhenSegmentedControlIsNull()
		{
			SfSegmentedControl? segmentedControl = null;
			Assert.Throws<NullReferenceException>(() => segmentedControl!.SegmentHeight = 50.0);
		}

		[Theory]
		[InlineData(-100.0)]
		[InlineData(0.0)]
		[InlineData(250.0)]
		public void SegmentWidth_SetValue_ReturnsExpectedValue(double segmentWidth)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentWidth = segmentWidth
			};
			Assert.Equal(segmentWidth, segmentedControl.SegmentWidth);
		}

		[Fact]
		public void SegmentWidth_SetValue_ThrowsNullReferenceException_WhenSegmentedControlIsNull()
		{
			SfSegmentedControl? segmentedControl = null;
			Assert.Throws<NullReferenceException>(() => segmentedControl!.SegmentWidth = 50.0);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-10)]
		[InlineData(100)]
		public void VisibleSegmentsCount_SetValue_ReturnsExpectedValue(int visibleSegmentsCount)
		{
			var segmentedControl = new SfSegmentedControl
			{
				VisibleSegmentsCount = visibleSegmentsCount
			};
			Assert.Equal(visibleSegmentsCount, segmentedControl.VisibleSegmentsCount);
		}

		[Fact]
		public void VisibleSegmentsCount_SetValue_ThrowsNullReferenceException_WhenSegmentedControlIsNull()
		{
			SfSegmentedControl? segmentedControl = null;
			Assert.Throws<NullReferenceException>(() => segmentedControl!.VisibleSegmentsCount = 10);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void AutoScrollToSelectedSegment_SetValue_ReturnsExpectedValue(bool autoScroll)
		{
			var segmentedControl = new SfSegmentedControl
			{
				AutoScrollToSelectedSegment = autoScroll
			};
			Assert.Equal(autoScroll, segmentedControl.AutoScrollToSelectedSegment);
		}

		[Fact]
		public void AutoScrollToSelectedSegment_SetValue_ThrowsNullReferenceException_WhenSegmentedControlIsNull()
		{
			SfSegmentedControl? segmentedControl = null;
			Assert.Throws<NullReferenceException>(() => segmentedControl!.AutoScrollToSelectedSegment = true);
		}

		[Theory]
		[InlineData("#FF0500")]
		public void DisabledSegmentTextColor_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var segmentedControl = new SfSegmentedControl();
			Color disabledSegmentTextColor = Color.FromArgb(colorHex);
			segmentedControl.DisabledSegmentTextColor = disabledSegmentTextColor;
			Assert.Equal(disabledSegmentTextColor, segmentedControl.DisabledSegmentTextColor);
		}

		[Theory]
		[InlineData("#FF5733")]
		public void SegmentBackground_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var segmentedControl = new SfSegmentedControl();
			Brush segmentBackground = new SolidColorBrush(Color.FromArgb(colorHex));
			segmentedControl.SegmentBackground = segmentBackground;
			Assert.Equal(segmentBackground, segmentedControl.SegmentBackground);
		}

		[Theory]
		[InlineData("#FF0000")]
		public void SegmentBackground_WithZeroStrokeThickness_ReturnsExpectedValue(string colorHex)
		{
			var segmentedControl = new SfSegmentedControl();
			Brush segmentBackground = new SolidColorBrush(Color.FromArgb(colorHex));
			segmentedControl.SegmentBackground = segmentBackground;
			segmentedControl.StrokeThickness = 0;
			Assert.Equal(segmentBackground, segmentedControl.SegmentBackground);
			Assert.Equal(0, segmentedControl.StrokeThickness);
		}

		[Theory]
		[InlineData("#FF0000")]
		public void DisabledSegmentBackground_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var segmentedControl = new SfSegmentedControl();
			Brush disabledSegmentBackground = new SolidColorBrush(Color.FromArgb(colorHex));
			segmentedControl.DisabledSegmentBackground = disabledSegmentBackground;
			Assert.Equal(disabledSegmentBackground, segmentedControl.DisabledSegmentBackground);
		}

		[Theory]
		[InlineData("#FF0000")]
		public void Stroke_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var segmentedControl = new SfSegmentedControl();
			Brush stroke = new SolidColorBrush(Color.FromArgb(colorHex));
			segmentedControl.Stroke = stroke;
			Assert.Equal(stroke, segmentedControl.Stroke);
		}

		[Theory]
		[InlineData(0.0)]
		[InlineData(-1.5)]
		[InlineData(10)]
		public void StrokeThickness_SetValue_ReturnsExpectedValue(double thickness)
		{
			var segmentedControl = new SfSegmentedControl
			{
				StrokeThickness = thickness
			};
			Assert.Equal(thickness, segmentedControl.StrokeThickness);
		}

		[Theory]
		[InlineData(10, 5, 10, 5)]
		public void CornerRadius_SetValue_ReturnsExpectedValue(double topLeft, double topRight, double bottomRight, double bottomLeft)
		{
			var segmentedControl = new SfSegmentedControl();
			var cornerRadius = new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
			segmentedControl.CornerRadius = cornerRadius;
			Assert.Equal(cornerRadius, segmentedControl.CornerRadius);
		}

		[Theory]
		[InlineData(10, 5, 10, 5)]
		public void SegmentCornerRadius_SetValue_ReturnsExpectedValue(double topLeft, double topRight, double bottomRight, double bottomLeft)
		{
			var segmentedControl = new SfSegmentedControl();
			var cornerRadius = new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
			segmentedControl.SegmentCornerRadius = cornerRadius;
			Assert.Equal(cornerRadius, segmentedControl.SegmentCornerRadius);
		}

		[Fact]
		public void SegmentTemplate_SetValue_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var dataTemplate = new DataTemplate(() =>
			{
				return new Label { Text = "Segment" };
			});
			segmentedControl.SegmentTemplate = dataTemplate;
			Assert.Equal(dataTemplate, segmentedControl.SegmentTemplate);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowSeparator_SetValue_ReturnsExpectedValue(bool showSeparator)
		{
			var segmentedControl = new SfSegmentedControl
			{
				ShowSeparator = showSeparator
			};
			Assert.Equal(showSeparator, segmentedControl.ShowSeparator);
		}

		[Fact]
		public void TextStyle_SetValue_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var textStyle = new SegmentTextStyle()
			{
				FontSize = 18,
				TextColor = Colors.Blue
			};
			segmentedControl.TextStyle = textStyle;
			Assert.Equal(textStyle, segmentedControl.TextStyle);
			Assert.Equal(18, segmentedControl.TextStyle.FontSize);
			Assert.Equal(Colors.Blue, segmentedControl.TextStyle.TextColor);
		}

		[Fact]
		public void SelectionIndicatorSettings_SetValue_ReturnsExpectedValue()
		{
			var segmentedControl = new SfSegmentedControl();
			var selectionIndicatorSettings = new SelectionIndicatorSettings
			{
				Stroke = Colors.Red,
				SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill,
				StrokeThickness = 2,
			};
			segmentedControl.SelectionIndicatorSettings = selectionIndicatorSettings;
			Assert.Equal(selectionIndicatorSettings, segmentedControl.SelectionIndicatorSettings);
			Assert.Equal(2, segmentedControl.SelectionIndicatorSettings.StrokeThickness);
			Assert.Equal(SelectionIndicatorPlacement.Fill, segmentedControl.SelectionIndicatorSettings.SelectionIndicatorPlacement);
			Assert.Equal(Colors.Red, segmentedControl.SelectionIndicatorSettings.Stroke);
		}
		[Theory]
		[InlineData(-100.0)]
		[InlineData(0.0)]
		[InlineData(250.0)]
		public void SegmentItemWidth_SetValues_ReturnsExpectedValue(double segmentWidth)
		{
			var segmentedControl = new SfSegmentItem
			{
				Width = segmentWidth
			};
			Assert.Equal(segmentWidth, segmentedControl.Width);
		}

		[Theory]
		[InlineData("#FF5733")]
		public void SegmentItemTextColor_SetValues_ReturnsExpectedValue(string colorHex)
		{
			var segmentedControl = new SfSegmentItem();
			Color segmentBackground = Color.FromArgb(colorHex);
			segmentedControl.SelectedSegmentTextColor = segmentBackground;
			Assert.Equal(segmentBackground, segmentedControl.SelectedSegmentTextColor);
		}

		[Theory]
		[InlineData("#FF5733")]
		public void SegmentItemSelectionBackground_SetValue_ReturnsExpectedValue(string colorHex)
		{
			var segmentedControl = new SfSegmentItem();
			Color segmentBackground = Color.FromArgb(colorHex);
			segmentedControl.SelectedSegmentBackground = segmentBackground;
			Assert.Equal(segmentBackground, segmentedControl.SelectedSegmentBackground);
		}

		[Theory]
		[InlineData(-100.0)]
		[InlineData(0.0)]
		[InlineData(40)]
		public void SegmnetItemImageSize_SetValue_ReturnsExpectedValue(double segmentImageSize)
		{
			var segmentedControl = new SfSegmentItem
			{
				ImageSize = segmentImageSize
			};
			Assert.Equal(segmentImageSize, segmentedControl.ImageSize);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void SegmentItemIsEnabled_SetValue_ReturnsExpectedValue(bool IsEnabled)
		{
			var segmentedControl = new SfSegmentItem
			{
				IsEnabled = IsEnabled
			};
			Assert.Equal(IsEnabled, segmentedControl.IsEnabled);
		}

		[Theory]
		[InlineData("true")]
		[InlineData("false")]
		public void SegmentItemText_SetValue_ReturnsExpectedValue(string Text)
		{
			var segmentedControl = new SfSegmentItem
			{
				Text = Text
			};
			Assert.Equal(Text, segmentedControl.Text);
		}

		[Theory]
		[InlineData(10, 20)]
		public void SegmentHeightAndSegmentWidth_SetValue_ReturnsExpectedValue(int segmentHeight, int segmentWidth)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentHeight = segmentHeight,
				SegmentWidth = segmentWidth
			};
			Assert.Equal(segmentHeight, segmentedControl.SegmentHeight);
			Assert.Equal(segmentWidth, segmentedControl.SegmentWidth);
		}

		[Theory]
		[InlineData(200, 200)]
		public void SegmentHeightAndSegmentWidth_SetValue_ReturnsExpectedValue_WhenValuesAreEqual(int segmentHeight, int segmentWidth)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentHeight = segmentHeight,
				SegmentWidth = segmentWidth
			};
			Assert.Equal(segmentHeight, segmentedControl.SegmentHeight);
			Assert.Equal(segmentWidth, segmentedControl.SegmentWidth);
		}

		[Theory]
		[InlineData(10, 20, 30, 40)]
		public void SegmentHeightAndSegmentWidth_SetValue_ReturnsExpectedValue_WhenValuesAreDifferent(int segmentHeight1, int segmentWidth1, int segmentHeight2, int segmentWidth2)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentHeight = segmentHeight1,
				SegmentWidth = segmentWidth1
			};
			segmentedControl.SegmentHeight = segmentHeight2;
			segmentedControl.SegmentWidth = segmentWidth2;
			Assert.Equal(segmentHeight2, segmentedControl.SegmentHeight);
			Assert.Equal(segmentWidth2, segmentedControl.SegmentWidth);
		}

		[Theory]
		[InlineData(30, 40)]
		public void SegmentHeightAndSegmentWidth_GetValue_ReturnsExpectedValue(int segmentHeight, int segmentWidth)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentHeight = segmentHeight,
				SegmentWidth = segmentWidth
			};
			var height = segmentedControl.SegmentHeight;
			var width = segmentedControl.SegmentWidth;
			Assert.Equal(segmentHeight, height);
			Assert.Equal(segmentWidth, width);
		}

		[Theory]
		[InlineData(10, 20)]
		[InlineData(20, 30)]
		[InlineData(30, 40)]
		public void SegmentCornerRadiusAndCornerRadius_SetValue_ReturnsExpectedValue(int segmentCornerRadius, int cornerRadius)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentCornerRadius = segmentCornerRadius,
				CornerRadius = cornerRadius
			};
			Assert.Equal(segmentCornerRadius, segmentedControl.SegmentCornerRadius);
			Assert.Equal(cornerRadius, segmentedControl.CornerRadius);
		}

		[Theory]
		[InlineData(30, 40)]
		public void SegmentCornerRadiusAndCornerRadius_GetValue_ReturnsExpectedValue(int segmentCornerRadius, int cornerRadius)
		{
			var segmentedControl = new SfSegmentedControl
			{
				SegmentCornerRadius = segmentCornerRadius,
				CornerRadius = cornerRadius
			};
			var segmentCornerRadiusValue = segmentedControl.SegmentCornerRadius;
			var cornerRadiusValue = segmentedControl.CornerRadius;
			Assert.Equal(segmentCornerRadius, segmentCornerRadiusValue);
			Assert.Equal(cornerRadius, cornerRadiusValue);
		}

		[Theory]
		[InlineData("#0000FF", "#FFFF00")]
		public void DisabledSegmentBackgroundAndTextColor_SetValue_ReturnsExpectedValue(string disabledSegmentBackgroundHex, string disabledSegmentTextColorHex)
		{
			var segmentedControl = new SfSegmentedControl();
			var disabledSegmentBackground = Color.FromArgb(disabledSegmentBackgroundHex);
			var disabledSegmentTextColor = Color.FromArgb(disabledSegmentTextColorHex);
			segmentedControl.DisabledSegmentBackground = disabledSegmentBackground;
			segmentedControl.DisabledSegmentTextColor = disabledSegmentTextColor;
			Assert.Equal(disabledSegmentBackground, segmentedControl.DisabledSegmentBackground);
			Assert.Equal(disabledSegmentTextColor, segmentedControl.DisabledSegmentTextColor);
		}

		[Fact]
		public void SelectionIndicatorSettings_SetValue_ReturnsExpectedValueCombination()
		{
			var segmentedControl = new SfSegmentedControl();
			var selectionIndicatorSettings = new SelectionIndicatorSettings
			{
				Stroke = Colors.Red,
				TextColor = Colors.Blue,
				Background = Colors.Maroon,
				SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill,
				StrokeThickness = 2
			};
			segmentedControl.SelectionIndicatorSettings = selectionIndicatorSettings;
			Assert.Equal(selectionIndicatorSettings, segmentedControl.SelectionIndicatorSettings);
			Assert.Equal(Colors.Maroon, segmentedControl.SelectionIndicatorSettings.Background);
			Assert.Equal(Colors.Blue, segmentedControl.SelectionIndicatorSettings.TextColor);
			Assert.Equal(2, segmentedControl.SelectionIndicatorSettings.StrokeThickness);
			Assert.Equal(SelectionIndicatorPlacement.Fill, segmentedControl.SelectionIndicatorSettings.SelectionIndicatorPlacement);
			Assert.Equal(Colors.Red, segmentedControl.SelectionIndicatorSettings.Stroke);
		}

		[Theory]
		[InlineData("#FF0000", 2)]
		public void StrokeAndStrokeThickness_SetValue_ReturnsExpectedValue(string colorHex, double thickenss)
		{
			var segmentedControl = new SfSegmentedControl();
			Brush stroke = new SolidColorBrush(Color.FromArgb(colorHex));
			var strokeThickness = thickenss;
			segmentedControl.StrokeThickness = strokeThickness;
			segmentedControl.Stroke = stroke;
			Assert.Equal(stroke, segmentedControl.Stroke);
			Assert.Equal(strokeThickness, segmentedControl.StrokeThickness);
		}

		[Theory]
		[InlineData(0, true)]
		[InlineData(1, false)]
		[InlineData(2, true)]
		[InlineData(3, false)]
		[InlineData(4, true)]
		public void SelectedIndexAndShowSeparatorTest(int selectedIndex, bool showSeparator)
		{
			var control = new SfSegmentedControl
			{
				SelectedIndex = selectedIndex,
				ShowSeparator = showSeparator
			};
			Assert.Equal(selectedIndex, control.SelectedIndex);
			Assert.Equal(showSeparator, control.ShowSeparator);
		}

		[Theory]
		[InlineData(new[] { "Item1", "Item2", "Item3" }, 2)]
		[InlineData(new[] { "Item1", "Item2", "Item3", "Item4", "Item5" }, 3)]
		[InlineData(new[] { "Item1", "Item2", "Item3", "Item4", "Item5", "Item6" }, 4)]
		public void ItemsSourceAndVisibleSegmentCountTest(string[] itemsSource, int visibleSegmentCount)
		{

			var control = new SfSegmentedControl
			{
				ItemsSource = itemsSource,
				VisibleSegmentsCount = visibleSegmentCount
			};
			Assert.Equal(itemsSource, control.ItemsSource);
			Assert.Equal(visibleSegmentCount, control.VisibleSegmentsCount);
		}
		#endregion

		#region Internal Methods
		[Fact]
		public void GetLocalizedString_ReturnsLocalizedString_WhenResourceManagerIsNotNull()
		{
			var text = "Key";
			var result = SfSegmentedResources.GetLocalizedString(text);
			Assert.Equal("", result);
		}

		[Fact]
		public void GetItemWidth_ReturnsSegmentItemWidth_WhenWidthIsSet()
		{
			var segmentInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { Width = 100 };
			var resultWidth = SegmentItemViewHelper.GetItemWidth(segmentInfo, segmentItem);
			Assert.Equal(101, resultWidth);
		}

		[Fact]
		public void GetItemWidth_ReturnsSegmentWidth_WhenWidthIsNotSet()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var resultWidth = SegmentItemViewHelper.GetItemWidth(itemInfo, segmentItem);
			Assert.Equal(101, resultWidth);
		}
		[Fact]
		public void GetItemWidth_ReturnsWidthPlusStrokeThickness_WhenWidthIsSetAndStrokeThicknessIsSet()
		{
			var segmentControl = new SfSegmentedControl { StrokeThickness = 5 };
			var segmentItem = new SfSegmentItem { Width = 100 };
			var resultWidth = SegmentItemViewHelper.GetItemWidth(segmentControl, segmentItem);
			Assert.Equal(105, resultWidth);
		}
		[Fact]
		public void GetItemWidth_ReturnsSegmentWidthPlusStrokeThickness_WhenWidthIsNotSetAndStrokeThicknessIsSet()
		{
			var segmentControl = new SfSegmentedControl { StrokeThickness = 5 };
			var segmentItem = new SfSegmentItem();
			var resultWidth = SegmentItemViewHelper.GetItemWidth(segmentControl, segmentItem);
			Assert.Equal(105, resultWidth);
		}

		[Fact]
		public void GetItemWidth_ReturnsNegativeValue_WhenWidthIsSetToNegative()
		{
			var segmentInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { Width = -50 };
			var resultWidth = SegmentItemViewHelper.GetItemWidth(segmentInfo, segmentItem);
			Assert.True(resultWidth < 0);
		}

		[Fact]
		public void GetItemWidth_ReturnsZero_WhenSegmentInfoIsNull()
		{
			SfSegmentedControl? segmentInfo = null;
			var segmentItem = new SfSegmentItem { Width = 100 };
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			var resultWidth = SegmentItemViewHelper.GetItemWidth(segmentInfo, segmentItem);
#pragma warning restore CS8604 // Re-enable the warning
			Assert.Equal(100, resultWidth);
		}

		[Fact]
		public void GetItemWidth_ReturnsZero_WhenSegmentItemIsNull()
		{
			var segmentInfo = new SfSegmentedControl();
			SfSegmentItem? segmentItem = null;

#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentItemViewHelper.GetItemWidth(segmentInfo, segmentItem));
#pragma warning restore CS8604 // Re-enable the warning

		}

		[Fact]
		public void GetItemWidth_ReturnsInvalidValue_WhenStrokeThicknessIsNegative()
		{
			var segmentInfo = new SfSegmentedControl { StrokeThickness = -5 };
			var segmentItem = new SfSegmentItem { Width = 100 };
			var resultWidth = SegmentItemViewHelper.GetItemWidth(segmentInfo, segmentItem);
			Assert.Equal(95, resultWidth);
		}

		[Fact]
		public void TrimText_ReturnsOriginalText_WhenFontSizeOrWidthIsNegative()
		{
			var text = "Hello World";
			var width = -10;
			var textStyle = new SegmentTextStyle { FontSize = 12 };
			var resultText = SegmentItemViewHelper.TrimText(text, width, textStyle);
			Assert.Equal(text, resultText);
		}

		[Fact]
		public void GetTextSize_ReturnsDefaultSize_WhenTextIsEmpty()
		{
			var text = string.Empty;
			var textStyle = new SegmentTextStyle { FontSize = 12 };
			var resultSize = SegmentItemViewHelper.GetTextSize(text, textStyle);
			Assert.Equal(default(Size), resultSize);
		}

		[Fact]
		public void CreateTemplateView_ReturnsView_WhenTemplateIsNotNull()
		{
			var template = new DataTemplate(() => new Label());
			var info = new object();
			var isRTL = false;
			var resultView = SegmentItemViewHelper.CreateTemplateView(template, info, isRTL);
			Assert.NotNull(resultView);
			Assert.IsAssignableFrom<View>(resultView);
		}

		[Fact]
		public void CreateTemplateView_ReturnsView_WhenTemplateHasViewCell()
		{
			var template = new DataTemplate(() =>new Label());
			var info = new object();
			var isRTL = false;
			var resultView = SegmentItemViewHelper.CreateTemplateView(template, info, isRTL);
			Assert.NotNull(resultView);
			Assert.IsAssignableFrom<View>(resultView);
		}

		[Fact]
		public void CreateTemplateView_SetsBindingContext_WhenBindingContextIsNull()
		{
			var template = new DataTemplate(() => new Label());
			var info = new object();
			var isRTL = false;
			var resultView = SegmentItemViewHelper.CreateTemplateView(template, info, isRTL);
			Assert.NotNull(resultView);
			Assert.Equal(info, resultView.BindingContext);
		}

		[Theory]
		[InlineData(true, FlowDirection.RightToLeft)]
		[InlineData(false, FlowDirection.LeftToRight)]
		public void CreateTemplateView_SetsFlowDirection_BasedOnIsRTL(bool isRTL, FlowDirection expectedFlowDirection)
		{
			var template = new DataTemplate(() => new Label());
			var info = new object();
			var resultView = SegmentItemViewHelper.CreateTemplateView(template, info, isRTL);
			Assert.NotNull(resultView);
			Assert.Equal(expectedFlowDirection, resultView.FlowDirection);
		}

		[Fact]
		public void CreateTemplateView_ThrowsInvalidOperationException_WhenTemplateReturnsUnsupportedView()
		{
			var template = new DataTemplate(() => new ContentPage());
			var info = new object();
			var isRTL = false;
			Assert.Throws<InvalidCastException>(() => SegmentItemViewHelper.CreateTemplateView(template, info, isRTL));
		}
		[Theory]
		[InlineData(1.5, 3f)]
		[InlineData(0, 0f)]
		public void GetStrokeThickness_ReturnsExpectedStrokeThickness(double strokeThickness, float expectedStrokeThickness)
		{
			var resultStrokeThickness = SegmentViewHelper.GetStrokeThickness(strokeThickness);
			Assert.Equal(expectedStrokeThickness, resultStrokeThickness);
		}
		[Fact]
		public void BrushToColorConverter_ReturnsColor_WhenBrushIsSolidColorBrush()
		{
			var brush = new SolidColorBrush(Colors.Red);
			var resultColor = SegmentViewHelper.BrushToColorConverter(brush);
			Assert.Equal(Colors.Red, resultColor);
		}

		[Fact]
		public void BrushToColorConverter_ReturnsTransparent_WhenBrushIsNotSolidColorBrush()
		{
			var unsupportedBrush = new LinearGradientBrush();
			var resultColor = SegmentViewHelper.BrushToColorConverter(unsupportedBrush);
			Assert.Equal(Colors.White, resultColor);
		}


		[Fact]
		public void BrushToColorConverter_ReturnsTransparent_WhenBrushIsNull()
		{
			Brush? brush = null;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			var resultColor = SegmentViewHelper.BrushToColorConverter(brush);
#pragma warning restore CS8604 // Re-enable the warning
			Assert.Equal(Colors.Transparent, resultColor);
		}

		[Fact]
		public void GetItemHeight_ThrowsArgumentException_WhenSegmentControlIsNull()
		{
			SfSegmentedControl? segmentControl = null;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentItemViewHelper.GetItemHeight(segmentControl));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Theory]
		[InlineData(40, 2, 44)]
		[InlineData(0, 2, 4)]
		[InlineData(0, 0, 0)]
		[InlineData(20, -2, 16)]
		[InlineData(float.NaN, 2, float.NaN)]
		[InlineData(20, float.NaN, float.NaN)]
		public void GetItemHeight_ReturnsExpectedHeight(float segmentHeight, float strokeThickness, float expectedHeight)
		{
			var segmentControl = new SfSegmentedControl
			{
				SegmentHeight = segmentHeight,
				StrokeThickness = strokeThickness
			};
			var resultHeight = SegmentItemViewHelper.GetItemHeight(segmentControl);
			Assert.Equal(expectedHeight, resultHeight);
		}

		[Theory]
		[InlineData(-1, 3)]
		[InlineData(0, 0)]
		[InlineData(2, 2)]
		[InlineData(4, 3)]
		public void GetVisibleSegmentsCount_ReturnsExpectedCount(int visibleSegmentsCount, int expectedCount)
		{
			var itemInfo = new SfSegmentedControl
			{
				VisibleSegmentsCount = visibleSegmentsCount,
				_items =
			[
				new SfSegmentItem {},
				new SfSegmentItem {},
				new SfSegmentItem {}
			]
			};
			var resultCount = SegmentViewHelper.GetVisibleSegmentsCount(itemInfo);
			Assert.Equal(expectedCount, resultCount);
		}
		[Fact]
		public void GetVisibleSegmentsCount_ThrowsArgumentException_WhenItemInfoIsNull()
		{
			SfSegmentedControl? itemInfo = null;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentViewHelper.GetVisibleSegmentsCount(itemInfo));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}
		[Theory]
		[InlineData(2, 0)]
		[InlineData(-1, 0)]
		[InlineData(0, 0)]
		public void GetVisibleSegmentsCount_ReturnsZero_WhenItemsAreEmpty(int visibleSegmentsCount, int expectedCount)
		{
			var itemInfo = new SfSegmentedControl
			{
				VisibleSegmentsCount = visibleSegmentsCount,
				_items = []
			};
			var resultCount = SegmentViewHelper.GetVisibleSegmentsCount(itemInfo);
			Assert.Equal(expectedCount, resultCount);
		}
		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public void GetVisibleSegmentsCount_ReturnsZero_WhenItemsAreNull(int visibleSegmentsCount)
		{
			var itemInfo = new SfSegmentedControl
			{
				VisibleSegmentsCount = visibleSegmentsCount,
				_items = null
			};
			var resultCount = SegmentViewHelper.GetVisibleSegmentsCount(itemInfo);
			Assert.Equal(0, resultCount);
		}

		[Theory]
		[InlineData(100, 100, 100, 100, 20.0, LayoutAlignment.Start, 3f)]
		[InlineData(100, 100, 100, 100, 20.0, LayoutAlignment.Fill, 140f)]
		[InlineData(100, 100, 100, 10, 20.0, LayoutAlignment.Fill, 95f)]
		[InlineData(100, 100, 100, 100, 10.0, LayoutAlignment.Fill, 145f)]
		[InlineData(100, 100, 100, 100, -20.0, LayoutAlignment.Start, 3f)]
		[InlineData(0, 0, 0, 0, 20.0, LayoutAlignment.Start, 3f)]
		public void GetVerticalYPosition_ReturnsExpectedPosition(double x, double y, double width, double height, double segmentHeight, LayoutAlignment alignment, float expectedResult)
		{
			var dirtyRect = new Rect(x, y, width, height);
			var resultYPosition = SegmentViewHelper.GetVerticalYPosition(dirtyRect, segmentHeight, alignment);
			Assert.Equal(expectedResult, resultYPosition);
		}

		[Theory]
		[InlineData(100, 100, 100, 100, 20.0, LayoutAlignment.Start, 3f)]
		[InlineData(100, 100, 100, 100, 20.0, LayoutAlignment.Fill, 140f)]
		[InlineData(10, 10, 10, 100, 20.0, LayoutAlignment.Fill, 5f)]
		[InlineData(100, 10, 100, 100, 10.0, LayoutAlignment.Fill, 145f)]
		public void GetHorizontalXPosition_ReturnsExpectedPosition(double x, double y, double width, double height, double segmentWidth, LayoutAlignment alignment, float expectedResult)
		{
			var dirtyRect = new Rect(x, y, width, height);
			var resultXPosition = SegmentViewHelper.GetHorizontalXPosition(dirtyRect, segmentWidth, alignment);
			Assert.Equal(expectedResult, resultXPosition);
		}

		[Theory]
		[InlineData(100.0, 50.0, 200.0, LayoutAlignment.Fill, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Fill, 194.0)]
		[InlineData(100.0, 50.0, 100.0, LayoutAlignment.Start, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Start, 52.0)]
		public void GetActualSegmentWidth_ReturnsExpectedWidth(double widthRequest, double minWidth, double maxWidth, LayoutAlignment alignment, double expectedResult)
		{
			var segmentInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }],
				VisibleSegmentsCount = 1,
				StrokeThickness = 1
			};
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);
			Assert.Equal(expectedResult, resultWidth);
		}

		[Theory]
		[InlineData(100.0, 50.0, 200.0, LayoutAlignment.Fill, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Fill, 194.0)]
		[InlineData(100.0, 50.0, 100.0, LayoutAlignment.Start, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Start, 50.0)]

		public void GetActualSegmentWidth_ReturnsExpectedWidth_WithStrokethicknessZero(double widthRequest, double minWidth, double maxWidth, LayoutAlignment alignment, double expectedResult)
		{
			var segmentInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }, new SfSegmentItem { }],
				VisibleSegmentsCount = 2,
				StrokeThickness = 0
			};
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);
			Assert.Equal(expectedResult, resultWidth);
		}

		[Theory]
		[InlineData(100.0, 50.0, 200.0, LayoutAlignment.Fill, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Fill, 194.0)]
		[InlineData(100.0, 50.0, 100.0, LayoutAlignment.Start, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Start, 50.0)]

		public void GetActualSegmentWidth_DifferentCount_ReturnsExpectedWidth_WithStrokethicknessZero(double widthRequest, double minWidth, double maxWidth, LayoutAlignment alignment, double expectedResult)
		{
			var segmentInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }, new SfSegmentItem { }],
				VisibleSegmentsCount = 1,
				StrokeThickness = 0
			};
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);
			Assert.Equal(expectedResult, resultWidth);
		}

		[Theory]
		[InlineData(100.0, 50.0, 200.0, LayoutAlignment.Fill, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Fill, 194.0)]
		[InlineData(100.0, 50.0, 100.0, LayoutAlignment.Start, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Start, 194.0)]

		public void GetActualSegmentWidth_NegativeCount_ReturnsExpectedWidth_WithStrokethicknessZero(double widthRequest, double minWidth, double maxWidth, LayoutAlignment alignment, double expectedResult)
		{
			var segmentInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }, new SfSegmentItem { }],
				VisibleSegmentsCount = -1,
				StrokeThickness = 0
			};
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);
			Assert.Equal(expectedResult, resultWidth);
		}

		[Theory]
		[InlineData(100.0, 50.0, 200.0, LayoutAlignment.Fill, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Fill, 194.0)]
		[InlineData(100.0, 50.0, 100.0, LayoutAlignment.Start, 94.0)]
		[InlineData(-100.0, 50.0, 200.0, LayoutAlignment.Start, 194.0)]

		public void GetActualSegmentWidth_NegativeCount_ReturnsExpectedWidth_WithStrokethickness(double widthRequest, double minWidth, double maxWidth, LayoutAlignment alignment, double expectedResult)
		{
			var segmentInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }, new SfSegmentItem { }],
				VisibleSegmentsCount = -1,
				StrokeThickness = 1
			};
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);
			Assert.Equal(expectedResult, resultWidth);
		}

		[Theory]
		[InlineData(LayoutAlignment.Fill)]
		[InlineData(LayoutAlignment.Start)]
		public void GetActualSegmentWidth_ReturnsTotalWidth_WhenVisibleSegmentsCountIsGreaterThanZero(LayoutAlignment alignment)
		{
			var segmentInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }],
				VisibleSegmentsCount = 2,
				StrokeThickness = 1
			};

			var widthRequest = 100.0;
			var minWidth = 50.0;
			var maxWidth = 200.0;
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);

			Assert.Equal(94, resultWidth);
		}

		[Fact]
		public void GetActualSegmentWidth_ReturnsMinWidth_WhenMaxWidthIsLessThanMinWidth()
		{
			var segmentInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }],
				VisibleSegmentsCount = 1,
				StrokeThickness = 1
			};
			var widthRequest = 100.0;
			var minWidth = 100.0;
			var maxWidth = 50.0;
			var alignment = LayoutAlignment.Fill;
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);
			Assert.Equal(94, resultWidth);
		}

		[Fact]
		public void GetActualSegmentWidth_ReturnsCorrectWidth_WhenStrokeThicknessIsNegative()
		{
			var segmentInfo = new SfSegmentedControl { _items = [new SfSegmentItem { }], VisibleSegmentsCount = 1, StrokeThickness = -1 };
			var widthRequest = 100.0;
			var minWidth = 50.0;
			var maxWidth = 200.0;
			var alignment = LayoutAlignment.Fill;
			var resultWidth = SegmentViewHelper.GetActualSegmentWidth(segmentInfo, widthRequest, minWidth, maxWidth, alignment);
			Assert.Equal(94, resultWidth);
		}
		[Fact]
		public void GetActualSegmentHeight_ReturnsHeightRequest_WhenHeightRequestIsNonNegativeAndItemsArePresent()
		{
			var itemInfo = new SfSegmentedControl { _items = [new SfSegmentItem { }] };
			var heightRequest = 100.0;
			var minHeight = 50.0;
			var maxHeight = 200.0;
			var resultHeight = SegmentViewHelper.GetActualSegmentHeight(itemInfo, heightRequest, minHeight, maxHeight);
			Assert.Equal(heightRequest - 6, resultHeight);
		}

		[Fact]
		public void GetActualSegmentHeight_ReturnsMinHeight_WhenHeightRequestIsNegativeAndItemsArePresent()
		{
			var itemInfo = new SfSegmentedControl { _items = [new SfSegmentItem { }] };
			var heightRequest = -100.0;
			var minHeight = 40.0;
			var maxHeight = 200.0;
			var resultHeight = SegmentViewHelper.GetActualSegmentHeight(itemInfo, heightRequest, minHeight, maxHeight);
			Assert.Equal(minHeight, resultHeight);
		}

		[Fact]
		public void GetActualSegmentHeight_ReturnsTotalHeight_WhenHeightRequestIsNegativeAndTotalHeightIsGreaterThanMaxHeight()
		{
			var itemInfo = new SfSegmentedControl { _items = [new SfSegmentItem { }], SegmentHeight = 200 };
			var heightRequest = -100.0;
			var minHeight = 50.0;
			var maxHeight = 100.0;
			var totalHeight = 202.0;
			var resultHeight = SegmentViewHelper.GetActualSegmentHeight(itemInfo, heightRequest, minHeight, maxHeight);
			Assert.Equal(totalHeight, resultHeight);
		}

		[Fact]
		public void GetActualSegmentHeight_ReturnsTotalHeight_WhenHeightRequestIsNegativeAndTotalHeightIsLessThanMaxHeight()
		{
			var itemInfo = new SfSegmentedControl { _items = [new SfSegmentItem { }] };
			var heightRequest = -100.0;
			var minHeight = 50.0;
			var maxHeight = 200.0;
			var resultHeight = SegmentViewHelper.GetActualSegmentHeight(itemInfo, heightRequest, minHeight, maxHeight);
			Assert.Equal(SegmentItemViewHelper.GetItemHeight(itemInfo), resultHeight);
		}

		[Theory]
		[InlineData(-100.0, 50.0, 200.0, 50.0)]
		[InlineData(100.0, 50.0, 200.0, 100.0)]
		public void GetActualSegmentHeight_ReturnsExpectedHeight_WhenItemsAreAbsent(double heightRequest, double minHeight, double maxHeight, double expectedResult)
		{
			var itemInfo = new SfSegmentedControl { _items = null };
			var resultHeight = SegmentViewHelper.GetActualSegmentHeight(itemInfo, heightRequest, minHeight, maxHeight);
			Assert.Equal(expectedResult, resultHeight);
		}
		[Fact]
		public void GetActualSegmentHeight_ThrowsArgumentNullException_WhenItemInfoIsNull()
		{
			SfSegmentedControl? itemInfo = null;
			var heightRequest = 100.0;
			var minHeight = 50.0;
			var maxHeight = 200.0;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentViewHelper.GetActualSegmentHeight(itemInfo, heightRequest, minHeight, maxHeight));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Theory]
		[InlineData(double.PositiveInfinity, 50.0, 200.0, double.PositiveInfinity)]
		[InlineData(100.0, -50.0, 200.0, 94)]
		[InlineData(-100.0, -50.0, -200.0, 40)]
		public void GetActualSegmentHeight_ReturnsExpectedHeight(double heightRequest, double minHeight, double maxHeight, double expectedResult)
		{
			var itemInfo = new SfSegmentedControl
			{
				_items = [new SfSegmentItem { }]
			};
			var resultHeight = SegmentViewHelper.GetActualSegmentHeight(itemInfo, heightRequest, minHeight, maxHeight);
			Assert.Equal(expectedResult, resultHeight);
		}

		[Fact]
		public void GetSegmentTextStyle_ReturnsSegmentItemTextStyle_WhenSegmentItemTextStyleIsNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { TextStyle = new SegmentTextStyle { FontSize = 16 } };
			var resultTextStyle = SegmentViewHelper.GetSegmentTextStyle(itemInfo, segmentItem);
			Assert.Equal(segmentItem.TextStyle, resultTextStyle);
		}

		[Fact]
		public void GetSegmentTextStyle_ReturnsItemInfoTextStyle_WhenSegmentItemTextStyleIsNull()
		{
			var itemInfo = new SfSegmentedControl { TextStyle = new SegmentTextStyle { FontSize = 16 } };
			var segmentItem = new SfSegmentItem();
			var resultTextStyle = SegmentViewHelper.GetSegmentTextStyle(itemInfo, segmentItem);
			Assert.Equal(itemInfo.TextStyle, resultTextStyle);
		}

		[Fact]
		public void GetSegmentTextStyle_ReturnsDefaultTextStyle_WhenSegmentItemTextStyleAndItemInfoTextStyleAreNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var resultTextStyle = SegmentViewHelper.GetSegmentTextStyle(itemInfo, segmentItem);
			Assert.NotNull(resultTextStyle);
			Assert.Equal(16, resultTextStyle.FontSize);
		}

		[Fact]
		public void GetSegmentTextStyle_ReturnsDefaultFontSize_WhenSegmentItemTextStyleAndItemInfoTextStyleHaveFontSizeMinusOne()
		{
			var itemInfo = new SfSegmentedControl { TextStyle = new SegmentTextStyle { FontSize = -1 } };
			var segmentItem = new SfSegmentItem { TextStyle = new SegmentTextStyle { FontSize = -1 } };
			var resultTextStyle = SegmentViewHelper.GetSegmentTextStyle(itemInfo, segmentItem);
			Assert.NotNull(resultTextStyle);
			Assert.Equal(14, resultTextStyle.FontSize);
		}

		[Fact]
		public void GetItemEnabled_ReturnsSegmentItemEnabledStatus_WhenSegmentItemIsEnabledIsSet()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { IsEnabled = true };
			var resultEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, segmentItem);
			Assert.True(resultEnabled);
		}

		[Fact]
		public void GetItemEnabled_ReturnsItemInfoEnabledStatus_WhenSegmentItemIsEnabledIsNotSet()
		{
			var itemInfo = new SfSegmentedControl { IsEnabled = true };
			var segmentItem = new SfSegmentItem();
			var resultEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, segmentItem);
			Assert.True(resultEnabled);
		}

		[Fact]
		public void GetItemEnabled_ReturnsFalse_WhenBothSegmentItemAndItemInfoEnabledStatusAreFalse()
		{
			var itemInfo = new SfSegmentedControl { IsEnabled = false };
			var segmentItem = new SfSegmentItem { IsEnabled = false };
			var resultEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, segmentItem);
			Assert.False(resultEnabled);
		}

		[Fact]
		public void GetSegmentBackground_ReturnsSegmentItemBackground_WhenSegmentItemBackgroundIsNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { Background = Brush.Red };
			var resultBackground = SegmentViewHelper.GetSegmentBackground(itemInfo, segmentItem);
			Assert.Equal(segmentItem.Background, resultBackground);
		}

		[Fact]
		public void GetItemEnabled_ReturnsFalse_WhenItemInfoIsNull()
		{
			SfSegmentedControl? itemInfo = null;
			var segmentItem = new SfSegmentItem();
			var resultEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, segmentItem);
			Assert.False(resultEnabled);
		}

		[Fact]
		public void GetItemEnabled_ReturnsFalse_WhenSegmentItemIsNull()
		{
			var itemInfo = new SfSegmentedControl();
			SfSegmentItem? segmentItem = null;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentViewHelper.GetItemEnabled(itemInfo, segmentItem));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Fact]
		public void GetSegmentBackground_ReturnsNull_WhenSegmentItemBackgroundIsNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { };
			var resultBackground = (SolidColorBrush)SegmentViewHelper.GetSegmentBackground(itemInfo, segmentItem);
			Assert.Equal((SolidColorBrush)Colors.Transparent, resultBackground);
		}

		[Fact]
		public void GetSegmentBackground_ReturnsItemInfoSegmentBackground_WhenSegmentItemBackgroundIsNull()
		{
			var itemInfo = new SfSegmentedControl { SegmentBackground = Brush.Green };
			var segmentItem = new SfSegmentItem();
			var resultBackground = SegmentViewHelper.GetSegmentBackground(itemInfo, segmentItem);
			Assert.Equal(itemInfo.SegmentBackground, resultBackground);
		}

		[Fact]
		public void GetSegmentBackground_ReturnsItemInfoSegmentBackground_WhenStrokeThicknessIsZero()
		{
			var itemInfo = new SfSegmentedControl { SegmentBackground = Brush.Red, StrokeThickness = 0 };
			var segmentItem = new SfSegmentItem();
			var resultBackground = SegmentViewHelper.GetSegmentBackground(itemInfo, segmentItem);
			Assert.Equal(itemInfo.SegmentBackground, resultBackground);
			Assert.Equal(0, itemInfo.StrokeThickness);
		}

		[Fact]
		public void GetSegmentBackground_ReturnsTransparent_WhenBothSegmentItemAndItemInfoBackgroundsAreNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var resultBackground = SegmentViewHelper.GetSegmentBackground(itemInfo, segmentItem);
			Assert.Equal(Brush.Transparent, resultBackground);
		}

		[Fact]
		public void GetSelectedSegmentBackground_ReturnsSegmentItemSelectedSegmentBackground_WhenSegmentItemSelectedSegmentBackgroundIsNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { SelectedSegmentBackground = Brush.Red };
			var resultBackground = SegmentViewHelper.GetSelectedSegmentBackground(itemInfo, segmentItem);
			Assert.Equal(segmentItem.SelectedSegmentBackground, resultBackground);
		}

		[Fact]
		public void GetSelectedSegmentBackground_ReturnsItemInfoSelectionIndicatorSettingsBackground_WhenSegmentItemSelectedSegmentBackgroundIsNull()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { Background = Brush.Green } };
			var segmentItem = new SfSegmentItem();
			var resultBackground = SegmentViewHelper.GetSelectedSegmentBackground(itemInfo, segmentItem);
			Assert.Equal(itemInfo.SelectionIndicatorSettings.Background, resultBackground);
		}

		[Fact]
		public void GetSelectedSegmentBackground_ThrowsArgumentException_WhenSegmentItemIsNull()
		{
			var itemInfo = new SfSegmentedControl();
			SfSegmentItem? segmentItem = null;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentViewHelper.GetSelectedSegmentBackground(itemInfo, segmentItem));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Fact]
		public void GetSelectedSegmentStroke_ReturnsSegmentItemSelectedSegmentBackground_WhenSegmentItemSelectedSegmentStrokeIsNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { SelectedSegmentBackground = Brush.Red };
			var resultStroke = SegmentViewHelper.GetSelectedSegmentStroke(itemInfo, segmentItem);
			Assert.Equal(segmentItem.SelectedSegmentBackground, resultStroke);
		}

		[Fact]
		public void GetSelectedSegmentStroke_ReturnsItemInfoSelectionIndicatorSettingsStroke_WhenSegmentItemSelectedSegmentStrokeIsNull()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { Stroke = Colors.Green } };
			var segmentItem = new SfSegmentItem();
			var resultStroke = SegmentViewHelper.GetSelectedSegmentStroke(itemInfo, segmentItem);
			Assert.Equal(new SolidColorBrush(itemInfo.SelectionIndicatorSettings.Stroke), resultStroke);
		}

		[Fact]
		public void GetSelectedSegmentStroke_ReturnsSelectedSegmentBackground_WhenSegmentItemSelectedSegmentBackgroundIsNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { SelectedSegmentBackground = Brush.Yellow };
			var resultStroke = SegmentViewHelper.GetSelectedSegmentStroke(itemInfo, segmentItem);
			Assert.Equal(segmentItem.SelectedSegmentBackground, resultStroke);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsSegmentItemSelectedSegmentTextColor_WhenSelectionIndicatorIsFilled()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill } };
			var segmentItem = new SfSegmentItem { SelectedSegmentTextColor = Colors.Red };
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			Assert.Equal(segmentItem.SelectedSegmentTextColor, resultColor);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsItemInfoSelectionIndicatorSettingsTextColor_WhenSelectionIndicatorIsFilledAndSegmentItemSelectedSegmentTextColorIsNull()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill, TextColor = Colors.Green } };
			var segmentItem = new SfSegmentItem();
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			Assert.Equal(itemInfo.SelectionIndicatorSettings.TextColor, resultColor);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsWhite_WhenSelectionIndicatorIsFilledAndBothSegmentItemAndItemInfoTextColorsAreNull()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill } };
			var segmentItem = new SfSegmentItem();
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			Assert.Equal(Colors.White, resultColor);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsColorFromSelectedSegmentbackground_WhenSelectionIndicatorIsNotFilled()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Border } };
			var segmentItem = new SfSegmentItem { SelectedSegmentTextColor = Colors.Yellow };
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			var expected = Color.FromRgba(255, 255, 0, 255);
			Assert.Equal(expected, resultColor);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsColorFromItemInfoSelectionIndicatorSettingsStroke_WhenSelectionIndicatorIsNotFilledAndSegmentItemSelectedSegmentStrokeIsNull()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Border, Stroke = Colors.Purple } };
			var segmentItem = new SfSegmentItem();
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			Assert.Equal(SegmentViewHelper.BrushToColorConverter(new SolidColorBrush(itemInfo.SelectionIndicatorSettings.Stroke)), resultColor);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ThrowsArgumentException_WhenSegmentItemIsNull()
		{
			var itemInfo = new SfSegmentedControl();
			SfSegmentItem? segmentItem = null;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsTransparent_WhenSegmentItemSelectedSegmentTextColorIsTransparent()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill } };
			var segmentItem = new SfSegmentItem { SelectedSegmentTextColor = Colors.Transparent };
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			Assert.True(resultColor == Colors.Transparent);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsTransparent_WhenItemInfoSelectionIndicatorSettingsTextColorIsTransparent()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill, TextColor = Colors.Transparent } };
			var segmentItem = new SfSegmentItem();
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			Assert.True(resultColor == Colors.Transparent);
		}

		[Fact]
		public void GetSelectedSegmentForeground_ReturnsTransparent_WhenBothSegmentItemAndItemInfoTextColorsAreTransparent()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Fill, TextColor = Colors.Transparent } };
			var segmentItem = new SfSegmentItem { SelectedSegmentTextColor = Colors.Transparent };
			var resultColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, segmentItem);
			Assert.Equal(Colors.Transparent, resultColor);
		}

		[Fact]
		public void GetHoveredSegmentBackground_ReturnsItemInfoHoveredBackground_WhenItemInfoHoveredBackgroundIsNotNull()
		{
			var itemInfo = new SfSegmentedControl { HoveredBackground = Brush.Red };
			var resultBackground = SegmentViewHelper.GetHoveredSegmentBackground(itemInfo);
			Assert.Equal(itemInfo.HoveredBackground, resultBackground);
		}
		[Fact]
		public void GetHoveredSegmentBackground_ReturnsDefaultHoveredBackground_WhenItemInfoHoveredBackgroundIsNull()
		{
			var itemInfo = new SfSegmentedControl();
			var resultBackground = SegmentViewHelper.GetHoveredSegmentBackground(itemInfo);
			var expectedBrush = new SolidColorBrush(Color.FromArgb("#1C1B1F14"));
			Assert.Equal(expectedBrush.Color, ((SolidColorBrush)resultBackground).Color);
		}

		[Fact]
		public void GetSelectedSegmentHoveredBackground_ReturnsHoveredBackgroundWithAlpha_WhenSelectedSegmentBackgroundIsNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { SelectedSegmentBackground = Brush.Red };
			var resultBackground = SegmentViewHelper.GetSelectedSegmentHoveredBackground(itemInfo, segmentItem);
			Assert.NotNull(resultBackground);
			Assert.IsType<SolidColorBrush>(resultBackground);
			var solidColorBrush = (SolidColorBrush)resultBackground;
			Assert.Equal(Color.FromRgba(1, 0, 0, 0.8), solidColorBrush.Color);
		}

		[Fact]
		public void GetSelectedSegmentHoveredBackground_ReturnsHoveredBackgroundWithAlpha_WhenSelectedSegmentBackgroundIsNull()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { Background = Colors.Green } };
			var segmentItem = new SfSegmentItem();
			var resultBackground = SegmentViewHelper.GetSelectedSegmentHoveredBackground(itemInfo, segmentItem);
			Assert.NotNull(resultBackground);
			Assert.IsType<SolidColorBrush>(resultBackground);
			var solidColorBrush = (SolidColorBrush)resultBackground;
			Assert.Equal(Color.FromRgba(0, 0.5019608, 0, 0.8), solidColorBrush.Color);
		}

		[Fact]
		public void GetSelectedSegmentHoveredBackground_ReturnsHoveredBackgroundWithAlpha_WhenSelectedSegmentBackgroundIsBlue()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var resultBackground = SegmentViewHelper.GetSelectedSegmentHoveredBackground(itemInfo, segmentItem);
			Assert.NotNull(resultBackground);
			Assert.IsType<SolidColorBrush>(resultBackground);
			var solidColorBrush = (SolidColorBrush)resultBackground;
			Assert.Equal(Color.FromRgba(0.40392157, 0.3137255, 0.6431373, 0.8), solidColorBrush.Color);
		}

		[Fact]
		public void GetSelectedSegmentHoveredStroke_ReturnsHoveredStrokeWithAlpha_WhenSelectedSegmentStrokeIsNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { SelectedSegmentBackground = Colors.Red };
			var resultStroke = SegmentViewHelper.GetSelectedSegmentHoveredStroke(itemInfo, segmentItem);
			Assert.NotNull(resultStroke);
			Assert.IsType<SolidColorBrush>(resultStroke);
			var solidColorBrush = (SolidColorBrush)resultStroke;
			var expectedColor = Colors.Red.WithAlpha(0.8f);
			Assert.Equal(expectedColor, solidColorBrush.Color);
		}

		[Fact]
		public void GetSelectedSegmentHoveredStroke_ReturnsHoveredStrokeWithAlpha_WhenSelectedSegmentStrokeIsNull()
		{
			var itemInfo = new SfSegmentedControl { SelectionIndicatorSettings = new SelectionIndicatorSettings { Stroke = Colors.Green } };
			var segmentItem = new SfSegmentItem();
			var resultStroke = SegmentViewHelper.GetSelectedSegmentHoveredStroke(itemInfo, segmentItem);
			Assert.NotNull(resultStroke);
			Assert.IsType<SolidColorBrush>(resultStroke);
			var solidColorBrush = (SolidColorBrush)resultStroke;
			var expectedColor = Colors.Green.WithAlpha(0.8f);
			Assert.Equal(expectedColor, solidColorBrush.Color);
		}

		[Fact]
		public void GetSelectedSegmentHoveredStroke_ReturnsHoveredStrokeWithAlpha_WhenSelectedSegmentStrokeIsBlue()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var resultStroke = SegmentViewHelper.GetSelectedSegmentHoveredStroke(itemInfo, segmentItem);
			Assert.NotNull(resultStroke);
			Assert.IsType<SolidColorBrush>(resultStroke);
			var solidColorBrush = (SolidColorBrush)resultStroke;
			var expectedColor = Color.FromRgba(0.40392157, 0.3137255, 0.6431373, 0.8);
			Assert.Equal(expectedColor, solidColorBrush.Color);
		}

		[Fact]
		public void GetClonedSegmentTextStyle_ReturnsClonedTextStyle_WhenItemInfoAndSegmentItemAreNotNull()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var textStyle = new SegmentTextStyle { TextColor = Color.FromRgba(0.10980392, 0.105882354, 0.12156863, 1), FontSize = 16, FontAttributes = FontAttributes.None };
			var resultTextStyle = SegmentViewHelper.GetClonedSegmentTextStyle(itemInfo, segmentItem);
			Assert.NotNull(resultTextStyle);
			Assert.Equal(textStyle.TextColor, resultTextStyle.TextColor);
			Assert.Equal(textStyle.FontSize, resultTextStyle.FontSize);
			Assert.Equal(textStyle.FontAttributes, resultTextStyle.FontAttributes);
		}

		[Fact]
		public void GetClonedSegmentTextStyle_ReturnsClonedTextStyle_WhenItemInfoIsNull()
		{
			var segmentItem = new SfSegmentItem();
			var textStyle = new SegmentTextStyle { TextColor = Color.FromRgba(0.10980392, 0.105882354, 0.12156863, 1), FontSize = 16 };
			var resultTextStyle = SegmentViewHelper.GetClonedSegmentTextStyle(null, segmentItem);
			Assert.NotNull(resultTextStyle);
			Assert.Equal(textStyle.TextColor, resultTextStyle.TextColor);
			Assert.Equal(textStyle.FontSize, resultTextStyle.FontSize);
		}

		[Fact]
		public void GetClonedSegmentTextStyle_ReturnsClonedTextStyle_WhenSegmentItemIsNull()
		{
			var itemInfo = new SfSegmentedControl();
			SfSegmentItem? segmentItem = null;
			var textStyle = new SegmentTextStyle { TextColor = Color.FromRgba(0.10980392, 0.105882354, 0.12156863, 1), FontSize = 16 };
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<NullReferenceException>(() => SegmentViewHelper.GetClonedSegmentTextStyle(itemInfo, segmentItem));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsSelected_ShouldReflectAssignedValue(bool isSelected)
		{
			var item = new SfSegmentItem() { IsSelected = isSelected };
			Assert.Equal(isSelected, item.IsSelected);
		}

		[Fact]
		public void UpdateSelection_ShouldSetIsSelectedTrue()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { Text = "Item 1" };
			var segmentItemView = new SegmentItemView(itemInfo, segmentItem);
			Assert.False(segmentItem.IsSelected);
			segmentItemView.UpdateSelection();
			Assert.True(segmentItem.IsSelected);
		}

		[Fact]
		public void ClearSelection_ShouldSetIsSelectedFalse()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem { Text = "Item 1", IsSelected = true };
			var segmentItemView = new SegmentItemView(itemInfo, segmentItem);
			Assert.True(segmentItem.IsSelected);
			segmentItemView.ClearSelection();
			Assert.False(segmentItem.IsSelected);
		}
		#endregion

		#region Private Methods
		[Fact]
		public void GetOutlinedBorderRect_ShouldReturnCorrectRect()
		{
			var segmentView = new SfSegmentedControl();
			var dirtyRect = new RectF(100, 100, 100, 100);
			SetPrivateField(segmentView, "_segmentWidth", 50f);
			SetPrivateField(segmentView, "_segmentHeight", 40f);
			var result = InvokePrivateMethod(segmentView, "GetOutlinedBorderRect", dirtyRect);
			Assert.NotNull(result);
			var rectResult = (RectF)result!;
			Assert.Equal(125, rectResult.Left);
			Assert.Equal(130, rectResult.Top);
			Assert.Equal(50, rectResult.Width);
			Assert.Equal(40, rectResult.Height);
		}

		[Fact]
		public void GetTextRect_ShouldReturnCorrectRect_WhenGivenValidInput_DefaultMode()
		{
			_ = new SfSegmentedResources();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(itemInfo, segmentItem);
			var dirtyRect = new RectF(60, 60, 100, 50);
			var imageRect = new RectF(50, 50, 30, 50);
			double padding = 5.0;
			var result = InvokePrivateMethod(segment, "GetTextRect", dirtyRect, imageRect, padding);
			Assert.NotNull(result);
			var textRect = (Rect)result!;
			var expectedX = imageRect.Right + padding;
			var expectedWidth = dirtyRect.Width - (imageRect.Right + padding);
			Assert.Equal(expectedX, textRect.X);
			Assert.Equal(dirtyRect.Top, textRect.Y);
			Assert.Equal(expectedWidth, textRect.Width);
			Assert.Equal(dirtyRect.Height, textRect.Height);
		}

		[Fact]
		public void GetSegmentLayoutRect_ReturnsCorrectRect_Windows()
		{
			var segmentView = new SfSegmentedControl { StrokeThickness = 2f };
			var dirtyRect = new RectF(10, 10, 10, 10);
			SetPrivateField(segmentView, "_segmentWidth", 10);
			SetPrivateField(segmentView, "_segmentHeight", 10);
			var result = InvokePrivateMethod(segmentView, "GetSegmentLayoutRect", dirtyRect);
			Assert.NotNull(result);
			var rectResult = (RectF)result!;
			Assert.Equal(10, rectResult.Left);
			Assert.Equal(12, rectResult.Top);
			Assert.Equal(10, rectResult.Width);
			Assert.Equal(6, rectResult.Height);
		}

		[Fact]
		public void GetKeyboardLayoutRect_ReturnsCorrectRect()
		{
			var segmentView = new SfSegmentedControl();
			var dirtyRect = new RectF(100, 100, 100, 100);
			var result = InvokePrivateMethod(segmentView, "GetKeyboardLayoutRect", dirtyRect);
			Assert.NotNull(result);
			var rectResult = (RectF)result!;
			Assert.Equal(147, rectResult.Left);
			Assert.Equal(147, rectResult.Top);
			Assert.Equal(6, rectResult.Width);
			Assert.Equal(6, rectResult.Height);
		}

		[Fact]
		public void GetTextRect_ReturnsCorrectRect()
		{
			_ = new SfSegmentItem();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(itemInfo, segmentItem);
			var dirtyRect = new RectF(100, 100, 100, 100);
			var imageRect = new RectF(50, 50, 20, 20);
			double padding = 5;
			var result = InvokePrivateMethod(segment, "GetTextRect", dirtyRect, imageRect, padding);
			Assert.NotNull(result);
			var rectResult = (Rect)result!;
			float left = imageRect.Right + (float)padding;
			float top = dirtyRect.Top;
			float width = dirtyRect.Width - (imageRect.Right + (float)padding);
			float height = dirtyRect.Height;
			Assert.Equal(left, rectResult.Left);
			Assert.Equal(top, rectResult.Top);
			Assert.Equal(width, rectResult.Width);
			Assert.Equal(height, rectResult.Height);
		}

		[Fact]
		public void GetRightBorderPosition_ShouldReturnCorrectPosition()
		{
			_ = new SfSegmentedControl();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selection = new SelectionView(segmentItem, itemInfo);
			var dirtyRect = new RectF(100, 100, 100, 100);
			var strokeThickness = 5.0f;
			var rightBorderPosition = InvokePrivateMethod(selection, "GetRightBorderPosition", dirtyRect, strokeThickness);
			Assert.NotNull(rightBorderPosition);
			var actualPosition = (float)rightBorderPosition!;
			var expectedPosition = dirtyRect.Width - strokeThickness;
			Assert.Equal(expectedPosition, actualPosition);
		}

		[Fact]
		public void GetTopBorderPosition_ShouldReturnCorrectPosition()
		{
			_ = new SfSegmentedControl();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selection = new SelectionView(segmentItem, itemInfo);
			var dirtyRect = new RectF(100, 100, 100, 100);
			var strokeThickness = 5.0f;
			var result = InvokePrivateMethod(selection, "GetTopBorderPosition", dirtyRect, strokeThickness);
			Assert.NotNull(result);
			var topBorderPosition = (float)result;
			var expectedPosition = dirtyRect.Top + strokeThickness / 2;
			Assert.Equal(expectedPosition, topBorderPosition);
		}

		[Fact]
		public void GetBottomBorderPosition_ShouldReturnCorrectPosition()
		{
			_ = new SfSegmentedControl();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selection = new SelectionView(segmentItem, itemInfo);
			var dirtyRect = new RectF(100, 100, 100, 100);
			var strokeThickness = 5.0f;
			var result = InvokePrivateMethod(selection, "GetBottomBorderPosition", dirtyRect, strokeThickness);
			Assert.NotNull(result);
			var bottomBorderPosition = (float)result;
			var expectedPosition = dirtyRect.Height - strokeThickness;
			Assert.Equal(expectedPosition, bottomBorderPosition);
		}

		[Fact]
		public void GetBottomBorderPosition_ReturnsNaN_WhenDirtyRectIsNull()
		{
			_ = new SfSegmentedControl();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selection = new SelectionView(segmentItem, itemInfo);
			var strokeThickness = 5.0f;
			var result = InvokePrivateMethod(selection, "GetBottomBorderPosition", null, strokeThickness);
			Assert.NotNull(result);
			var bottomBorderPosition = (float)result;
			Assert.False(float.IsNaN(bottomBorderPosition));
		}

		[Fact]
		public void GetBottomBorderPosition_ReturnsNaN_WhenStrokeThicknessIsNegative()
		{
			_ = new SfSegmentedControl();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selection = new SelectionView(segmentItem, itemInfo);
			var dirtyRect = new RectF(100, 100, 100, 100);
			var strokeThickness = -5.0f;
			var result = InvokePrivateMethod(selection, "GetBottomBorderPosition", dirtyRect, strokeThickness);
			Assert.NotNull(result);
			var bottomBorderPosition = (float)result;
			Assert.False(float.IsNaN(bottomBorderPosition));
		}

		[Fact]
		public void GetLeftBorderPosition_ShouldReturnCorrectPosition()
		{
			_ = new SfSegmentedControl();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selection = new SelectionView(segmentItem, itemInfo);
			var dirtyRect = new RectF(100, 100, 100, 100);
			var strokeThickness = 5.0f;
			var result = InvokePrivateMethod(selection, "GetLeftBorderPosition", dirtyRect, strokeThickness);
			Assert.NotNull(result);
			var leftBorderPosition = (float)result;
			var expectedPosition = dirtyRect.Left + strokeThickness / 2;
			Assert.Equal(expectedPosition, leftBorderPosition);
		}


		[Theory]
		[InlineData("Disabled", "Disabled")]
		[InlineData("Selected", "Selected")]
		[InlineData("Other", "")]
		public void GetDefaultString_ReturnsCorrectDefaultString(string text, string expectedResult)
		{
			var resources = new SfSegmentedResources();
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();

			_ = new SegmentItemView(itemInfo, segmentItem);
			var result = (string)InvokeStaticPrivateMethod(resources, "GetDefaultString", text);
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void GetDrawRect_ReturnsDefaultRect_WhenSegmentItemIsNull()
		{
			var dirtyRect = new Rect(10, 100, 100, 100);
			var segmentControl = new SfSegmentedControl();
			var keyNavigation = new KeyNavigationView(segmentControl);
			SetPrivateField(keyNavigation, "_segmentItem", null);
			var resultRect = InvokePrivateMethod(keyNavigation, "GetDrawRect", dirtyRect);
			Assert.Equal(default(RectF), resultRect);
		}
		[Theory]
		[InlineData(10, 10, 100, 100, 10, 10, 20, 20, 5.0, 35, 10, 65, 100)]
		[InlineData(10, 10, 200, 10, 110, 10, 20, 20, 5.0, 135, 10, 65, 10)]
		[InlineData(10, 10, 100, 100, 10, 10, 20, 20, 0.0, 30, 10, 70, 100)]
		public void GetTextRect_ReturnsCorrectRect_WhenConditionsAreMet(
		 float dirtyX, float dirtyY, float dirtyWidth, float dirtyHeight,
		 float imageX, float imageY, float imageWidth, float imageHeight,
		 double padding,
		 float expectedX, float expectedY, float expectedWidth, float expectedHeight)
		{
			var segmentControl = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segmentView = new SegmentItemView(segmentControl, segmentItem);

			var dirtyRect = new RectF(dirtyX, dirtyY, dirtyWidth, dirtyHeight);
			var imageRect = new RectF(imageX, imageY, imageWidth, imageHeight);

			var resultRect = InvokePrivateMethod(segmentView, "GetTextRect", dirtyRect, imageRect, padding);

			Assert.Equal(new Rect(expectedX, expectedY, expectedWidth, expectedHeight), resultRect);
		}

		[Fact]
		public void GetDrawRect_ReturnsDefaultRect_WhenBoundsIsNull()
		{
			var segmentControl = new SfSegmentedControl();
			var keyNavigation = new KeyNavigationView(segmentControl);
			var segmentItem = new SfSegmentItem();
			SetPrivateField(keyNavigation, "_bounds", null);
			var dirtyRect = new RectF(10, 10, 100, 100);
			Assert.Throws<ArgumentException>(() => InvokePrivateMethod(keyNavigation, "GetDrawRect", dirtyRect));
		}

		[Fact]
		public void GetDrawRect_ReturnsDefaultRect_WhenItemInfoIsNull()
		{
			var segmentItem = new SfSegmentedControl();
			var keyNavigation = new KeyNavigationView(segmentItem);
			SetPrivateField(keyNavigation, "_itemInfo", null);
			var dirtyRect = new RectF(10, 10, 100, 100);
			Assert.Throws<ArgumentException>(() => InvokePrivateMethod(keyNavigation, "GetDrawRect", dirtyRect));
		}

		[Fact]
		public void GetDrawRect_WhenItemInfoIsNull()
		{
			var segmentItem = new SfSegmentedControl();
			var keyNavigation = new KeyNavigationView(segmentItem);
			SetPrivateField(keyNavigation, "_itemInfo", null);

			Assert.Throws<TargetParameterCountException>(() => InvokePrivateMethod(keyNavigation, "GetDrawRect"));

		}

		[Fact]
		public void GetStrokeValue_ReturnsTransparentBrush_WhenItemInfoIsNull()
		{

			var segmentItem = new SfSegmentItem();
			SfSegmentedControl? segment = null;

#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			var selectionView = new SelectionView(segmentItem, segment);
#pragma warning restore CS8604 // Suppress warning for possible null reference argument
			var resultBrush = InvokePrivateMethod(selectionView, "GetStrokeValue");
			Assert.Equal(Brush.Transparent, resultBrush);
		}

		[Fact]
		public void GetStrokeValue_ReturnsTransparentBrush_WhenSegmentItemIsNull()
		{
			var iteminfo = new SfSegmentedControl();
			SfSegmentItem? segmentItem = null;

#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			var selectionView = new SelectionView(segmentItem, iteminfo);
#pragma warning restore CS8604 // Suppress warning for possible null reference argument
			var resultBrush = InvokePrivateMethod(selectionView, "GetStrokeValue");
			Assert.Equal(Brush.Transparent, resultBrush);
		}

		[Fact]
		public void GetStrokeValue_ReturnsTransparentBrush_WhenSegmentItemIsItemInfoNull()
		{
			SfSegmentedControl? segment = null;
			SfSegmentItem? segmentItem = null;

#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			var selectionView = new SelectionView(segmentItem, segment);
#pragma warning restore CS8604 // Suppress warning for possible null reference argument
			var resultBrush = InvokePrivateMethod(selectionView, "GetStrokeValue");
			Assert.Equal(Brush.Transparent, resultBrush);
		}

		[Fact]
		public void GetStrokeValue_ReturnsSelectedSegmentStroke_WhenIsSelectedIsTrueAndIsMouseOverIsFalse()
		{
			var iteminfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selectionView = new SelectionView(segmentItem, iteminfo);
			SetPrivateField(selectionView, "_isSelected", true);
			SetPrivateField(selectionView, "_isMouseOver", false);
			var resultBrush = InvokePrivateMethod(selectionView, "GetStrokeValue");
			var expected = SegmentViewHelper.GetSelectedSegmentStroke(iteminfo, segmentItem);
			Assert.Equal(expected, resultBrush);
		}

		[Fact]
		public void GetStrokeValue_ReturnsSelectedSegmentHoveredStroke_WhenIsSelectedIsTrueAndIsMouseOverIsTrue()
		{
			var iteminfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selectionView = new SelectionView(segmentItem, iteminfo);
			SetPrivateField(selectionView, "_isSelected", true);
			SetPrivateField(selectionView, "_isMouseOver", true);
			var resultBrush = InvokePrivateMethod(selectionView, "GetStrokeValue") as SolidColorBrush;
			var expectedBrush = SegmentViewHelper.GetSelectedSegmentHoveredStroke(iteminfo, segmentItem) as SolidColorBrush;
			Assert.NotNull(resultBrush);
			Assert.NotNull(expectedBrush);
			Assert.Equal(expectedBrush!.Color, resultBrush!.Color);
		}

		[Fact]
		public void GetStrokeValue_ReturnsHoveredSegmentBackground_WhenIsSelectedIsFalseAndIsMouseOverIsTrue()
		{
			var iteminfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selectionView = new SelectionView(segmentItem, iteminfo);
			SetPrivateField(selectionView, "_isSelected", false);
			SetPrivateField(selectionView, "_isMouseOver", true);
			var resultBrush = InvokePrivateMethod(selectionView, "GetStrokeValue") as SolidColorBrush;
			var expectedBrush = SegmentViewHelper.GetHoveredSegmentBackground(iteminfo) as SolidColorBrush;
			Assert.NotNull(resultBrush);
			Assert.NotNull(expectedBrush);
			Assert.Equal(expectedBrush!.Color, resultBrush!.Color);
		}


		[Fact]
		public void GetStrokeValue_ReturnsTransparentBrush_WhenIsSelectedIsFalseAndIsMouseOverIsFalse()
		{
			var iteminfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var selectionView = new SelectionView(segmentItem, iteminfo);
			SetPrivateField(selectionView, "_isSelected", false);
			SetPrivateField(selectionView, "_isMouseOver", false);
			var resultBrush = InvokePrivateMethod(selectionView, "GetStrokeValue");
			Assert.Equal(Brush.Transparent, resultBrush);
		}

		[Fact]
		public void GetSegmentCornerRadius_ReturnsDefaultCornerRadius_WhenItemInfoEmpty()
		{
			var itemInfo = new SfSegmentedControl();
			var layout = new SegmentLayout(itemInfo);
			var resultCornerRadius = InvokePrivateMethod(layout, "GetSegmentCornerRadius", 0);
			Assert.Equal(default(CornerRadius), resultCornerRadius);
		}

		[Theory]
		[InlineData(null, 0)]
		[InlineData(null, -1)]
		public void GetSegmentCornerRadius_ReturnsDefaultCornerRadius_WhenItemsAreNullOrIndexIsLessThanZero(ObservableCollection<SfSegmentItem>? items, int index)
		{
			var itemInfo = new SfSegmentedControl { _items = items };
			var layout = new SegmentLayout(itemInfo);

			var resultCornerRadius = InvokePrivateMethod(layout, "GetSegmentCornerRadius", index);
			Assert.Equal(default(CornerRadius), resultCornerRadius);
		}

		[Fact]
		public void GetSegmentCornerRadius_ReturnsCornerRadius_WhenItemsCountIsOne()
		{
			var itemInfo = new SfSegmentedControl { _items = [new SfSegmentItem { }], CornerRadius = new CornerRadius(10, 20, 30, 40) };
			var layout = new SegmentLayout(itemInfo);
			var resultCornerRadius = InvokePrivateMethod(layout, "GetSegmentCornerRadius", 0);
			Assert.Equal(new CornerRadius(10, 20, 30, 40), resultCornerRadius);
		}

		[Theory]
		[InlineData(0, FlowDirection.LeftToRight, 10, 0, 30, 0)]
		[InlineData(1, FlowDirection.LeftToRight, 0, 20, 0, 40)]
		[InlineData(0, FlowDirection.RightToLeft, 0, 20, 0, 40)]
		public void GetSegmentCornerRadius_ReturnsExpectedCornerRadius(int index, FlowDirection flowDirection, double topLeft, double topRight, double bottomLeft, double bottomRight)
		{
			var itemInfo = new SfSegmentedControl
			{
				_items =
		[
			new SfSegmentItem { },
			new SfSegmentItem { }
		],
				CornerRadius = new CornerRadius(10, 20, 30, 40),
				FlowDirection = flowDirection
			};

			var layout = new SegmentLayout(itemInfo);
			var resultCornerRadius = InvokePrivateMethod(layout, "GetSegmentCornerRadius", index);

			Assert.Equal(new CornerRadius(topLeft, topRight, bottomLeft, bottomRight), resultCornerRadius);
		}


		[Fact]
		public void GetImageRect_ReturnsCorrectImageRect_WhenTextIsEmpty()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(itemInfo, segmentItem);
			segmentItem.ImageSize = 20;
			var dirtyRect = new RectF(100, 100, 100, 100);
			var result = InvokePrivateMethod(segment, "GetImageRect", dirtyRect);
			Assert.NotNull(result);
			var resultRect = (RectF)result!;
			Assert.Equal(100, resultRect.X);
			Assert.Equal(40, resultRect.Y);
			Assert.Equal(20, resultRect.Width);
			Assert.Equal(20, resultRect.Height);
		}

		[Fact]
		public void GetImageRect_ReturnsCorrectImageRect_WhenImageXPosExceedsLeftBoundary()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(itemInfo, segmentItem);
			segmentItem.ImageSize = 20;
			var dirtyRect = new RectF(100, 100, 50, 100);
			var result = InvokePrivateMethod(segment, "GetImageRect", dirtyRect);
			Assert.NotNull(result);
			var resultRect = (RectF)result!;
			Assert.Equal(100, resultRect.X);
			Assert.Equal(40, resultRect.Y);
			Assert.Equal(20, resultRect.Width);
			Assert.Equal(20, resultRect.Height);
		}

		[Fact]
		public void GetImageRect_ReturnsCorrectImageRect_WhenImageXPosExceedsRightBoundary()
		{
			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(itemInfo, segmentItem);
			segmentItem.ImageSize = 20;
			var dirtyRect = new RectF(100, 100, 30, 100);
			var result = InvokePrivateMethod(segment, "GetImageRect", dirtyRect);
			Assert.NotNull(result);
			var resultRect = (RectF)result!;
			Assert.Equal(100, resultRect.X);
			Assert.Equal(40, resultRect.Y);
			Assert.Equal(20, resultRect.Width);
			Assert.Equal(20, resultRect.Height);
		}

		[Fact]
		public void GetBackgroundValue_ReturnsTransparent_WhenItemInfoOrSegmentItemIsNull()
		{
			var segmentControl = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(segmentControl, segmentItem);
			var resultBrush = InvokePrivateMethod(segment, "GetBackgroundValue");
			Assert.NotNull(resultBrush);
			var result = (Brush)resultBrush!;
			Assert.Equal(Brush.Transparent, result);
		}

		[Fact]
		public void GetBackgroundValue_ReturnsSelectedSegmentBackground_WhenItemSelectedAndNotMouseOver()
		{
			var segmentControl = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(segmentControl, segmentItem)
			{
				_isSelected = true
			};
			var result = InvokePrivateMethod(segment, "GetBackgroundValue");
			Assert.NotNull(result);
			var resultBrush = (Brush)result!;
			Assert.Equal(SegmentViewHelper.GetSelectedSegmentBackground(segmentControl, segmentItem), resultBrush);
		}

		[Fact]
		public void GetBackgroundValue_ReturnsSelectedSegmentHoveredBackground_WhenItemSelectedAndMouseOver()
		{
			var segmentControl = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(segmentControl, segmentItem)
			{
				_isSelected = true
			};
			SetPrivateField(segment, "_isMouseOver", true);
			var expectedBrush = new SolidColorBrush(Color.FromRgba(0.40392157, 0.3137255, 0.6431373, 0.8));
			var resultBrush = InvokePrivateMethod(segment, "GetBackgroundValue");
			Assert.NotNull(resultBrush);
			Assert.Equal(expectedBrush.Color, ((SolidColorBrush)resultBrush).Color);
		}


		[Fact]
		public void GetBackgroundValue_ReturnsHoveredSegmentBackground_WhenNotItemSelectedAndMouseOver()
		{
			var segmentControl = new SfSegmentedControl { HoveredBackground = Colors.Blue };
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(segmentControl, segmentItem)
			{
				_isSelected = false
			};
			var result = InvokePrivateMethod(segment, "GetBackgroundValue");
			Assert.NotNull(result);
			var resultBrush = (SolidColorBrush)result!;
			var expectedBrush = new SolidColorBrush(Colors.Transparent);
			Assert.Equal(expectedBrush.Color, resultBrush.Color);
		}

		[Fact]
		public void GetBackgroundValue_ReturnsTransparent_WhenNotItemSelectedAndNotMouseOver()
		{

			var itemInfo = new SfSegmentedControl();
			var segmentItem = new SfSegmentItem();
			var segment = new SegmentItemView(itemInfo, segmentItem)
			{
				_isSelected = false
			};
			var resultBrush = InvokePrivateMethod(segment, "GetBackgroundValue");
			Assert.NotNull(resultBrush);
			var result = (Brush)resultBrush!;
			Assert.Equal(Brush.Transparent, result);
		}
		[Fact]
		public void GetBackgroundValue_ReturnsTransparent_WhenSegmentControlIsNull()
		{
			var segmentItem = new SfSegmentItem();
			SfSegmentedControl? control = null;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			var segment = new SegmentItemView(control, segmentItem);
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

			var result = InvokePrivateMethod(segment, "GetBackgroundValue");
			Assert.NotNull(result);
			var resultBrush = (Brush)result!; // Safely cast after null check
			Assert.Equal(Brush.Transparent, resultBrush);
		}

		[Fact]
		public void GetBackgroundValue_ReturnsTransparent_WhenSegmentItemIsNull()
		{
			var segmentControl = new SfSegmentedControl();
			SfSegmentItem? segmentItem = null;

#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			var segment = new SegmentItemView(segmentControl, segmentItem);
#pragma warning restore CS8604 // Suppress warning for possible null reference argument
			var result = InvokePrivateMethod(segment, "GetBackgroundValue");
			Assert.NotNull(result);
			var resultBrush = (Brush)result!;
			Assert.Equal(Brush.Transparent, resultBrush);
		}
		[Theory]
		[InlineData(100, 50, 200, 3, true, 100)]
		[InlineData(21, 50, 200, 3, true, 50)]
		[InlineData(-1, 50, 200, 3, true, 200)]
		[InlineData(-1, 50, 200, -1, true, 50)]
		[InlineData(-1, 50, 200, 3, false, 50)]
		public void GetEffectiveWidth_ReturnsExpectedWidth(double widthRequest, double minWidth, double maxWidth, int visibleSegmentCount, bool isAlignmentFill, double expectedWidth)
		{
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetEffectiveWidth", widthRequest, minWidth, maxWidth, visibleSegmentCount, isAlignmentFill);
			Assert.Equal(expectedWidth, resultWidth);
		}
		[Fact]
		public void GetEffectiveWidth_ReturnsNaN_WhenWidthRequestIsNaN()
		{
			var widthRequest = double.NaN;
			var minWidth = 50;
			var maxWidth = 200;
			var visibleSegmentCount = 3;
			var isAlignmentFill = true;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetEffectiveWidth", widthRequest, minWidth, maxWidth, visibleSegmentCount, isAlignmentFill);
			Assert.False(double.IsNaN(resultWidth));
		}

		[Fact]
		public void GetEffectiveWidth_ReturnsNaN_WhenMinWidthIsNaN()
		{
			var widthRequest = -1;
			var minWidth = double.NaN;
			var maxWidth = 200;
			var visibleSegmentCount = 3;
			var isAlignmentFill = true;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetEffectiveWidth", widthRequest, minWidth, maxWidth, visibleSegmentCount, isAlignmentFill);
			Assert.False(double.IsNaN(resultWidth));
		}

		[Fact]
		public void GetEffectiveWidth_ReturnsNaN_WhenMaxWidthIsNaN()
		{
			var widthRequest = -1;
			var minWidth = 50;
			var maxWidth = double.NaN;
			var visibleSegmentCount = 3;
			var isAlignmentFill = true;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetEffectiveWidth", widthRequest, minWidth, maxWidth, visibleSegmentCount, isAlignmentFill);
			Assert.True(double.IsNaN(resultWidth));
		}

		[Fact]
		public void GetEffectiveWidth_ReturnsInvalidWidth_WhenMinWidthIsGreaterThanMaxWidth()
		{

			var widthRequest = -1;
			var minWidth = 200;
			var maxWidth = 50;
			var visibleSegmentCount = 3;
			var isAlignmentFill = true;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetEffectiveWidth", widthRequest, minWidth, maxWidth, visibleSegmentCount, isAlignmentFill);
			Assert.False(minWidth == resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsValue_WhenItemsAreNotNull()
		{
			var segmentControl = new SfSegmentedControl
			{
				_items =
				[
					new SfSegmentItem {},
					new SfSegmentItem {},
					new SfSegmentItem {},
				]
			};
			var visibleSegmentsCount = 3;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount);
			Assert.Equal(300, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsTotalWidth_WhenItemsHaveWidth()
		{
			var segmentControl = new SfSegmentedControl
			{
				_items =
				[
					new SfSegmentItem {},
					new SfSegmentItem {},
					new SfSegmentItem {}
				]
			};

			_ = new SfSegmentItem { Width = 100 };
			var visibleSegmentsCount = 2;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount);
			Assert.Equal(200, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsTotalWidth_WhenItemsHaveSegmentWidth()
		{
			var segmentControl = new SfSegmentedControl
			{
				_items =
				[
					new SfSegmentItem {},
					new SfSegmentItem {},
					new SfSegmentItem {}
				]
			};
			var segment = new SfSegmentItem { Width = 100 };
			segment.Width = 100;
			var visibleSegmentsCount = 2;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount);
			Assert.Equal(200, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsTotalWidth_WhenItemsHaveWidthAndSegmentWidth()
		{
			var segmentControl = new SfSegmentedControl
			{
				_items =
				[
					new SfSegmentItem {},
					new SfSegmentItem {},
					new SfSegmentItem {}
				]
			};

			_ = new SfSegmentItem { Width = 100 };
			var visibleSegmentsCount = 2;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount);
			Assert.Equal(200, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ThrowsArgumentNullException_WhenSegmentControlIsNull()
		{
			SfSegmentedControl? segmentControl = null;
			var visibleSegmentsCount = 3;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<TargetInvocationException>(() => InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsZero_WhenVisibleSegmentsCountIsNegative()
		{
			var segmentControl = new SfSegmentedControl
			{
				_items =
				[
					new SfSegmentItem {},
					new SfSegmentItem {},
					new SfSegmentItem {}
				]
			};
			var visibleSegmentsCount = -1;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount);
			Assert.Equal(0, resultWidth);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(3, 0)]
		public void GetTotalSegmentWidth_ReturnsZero_WhenItemsAreEmpty(int visibleSegmentsCount, double expectedWidth)
		{
			var segmentControl = new SfSegmentedControl();
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount);
			Assert.Equal(expectedWidth, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsZero_WhenItemsAreNullAndVisibleSegmentsCountIsGreaterThanZero()
		{
			var segmentControl = new SfSegmentedControl { _items = null };
			var visibleSegmentsCount = 3;
			var resultWidth = (double)InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", segmentControl, visibleSegmentsCount);
			Assert.Equal(0, resultWidth);
		}
		[Fact]
		public void GetTotalSegmentWidth_ReturnsTotalWidth_WhenAllSegmentsHaveWidth()
		{
			var itemInfo = new SfSegmentedControl
			{
				_items =
				[
					new SfSegmentItem { Width = 10},
					new SfSegmentItem { Width=20 },
					new SfSegmentItem {Width =30}
				]
			};
			var visibleSegmentCount = 3;
			var resultWidth = InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", itemInfo, visibleSegmentCount);
			Assert.Equal(60.0, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsTotalWidth_WhenSomeSegmentsHaveNaNWidth()
		{
			var itemInfo = new SfSegmentedControl
			{
				SegmentWidth = 20,
				_items =
				[
					new SfSegmentItem { Width = 10},
					new SfSegmentItem {},
					new SfSegmentItem {Width =30}
				]
			};

			var visibleSegmentCount = 3;
			var resultWidth = InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", itemInfo, visibleSegmentCount);
			Assert.Equal(60.0, resultWidth);
		}
		[Fact]
		public void GetTotalSegmentWidth_ThrowsArgumentException_WhenItemInfoIsNull()
		{
			SfSegmentedControl? itemInfo = null;
			var visibleSegmentCount = 3;
#pragma warning disable CS8604 // Suppress warning for possible null reference argument
			Assert.Throws<TargetInvocationException>(() => InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", itemInfo, visibleSegmentCount));
#pragma warning restore CS8604 // Suppress warning for possible null reference argument

		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnZero_WhenVisibleSegmentCountIsNegative()
		{
			var itemInfo = new SfSegmentedControl();
			var visibleSegmentCount = -1;
			var resultWidth = InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", itemInfo, visibleSegmentCount);
			Assert.Equal(0.0, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsZero_WhenItemsAreNull()
		{
			var itemInfo = new SfSegmentedControl { _items = null };
			var visibleSegmentCount = 3;
			var resultWidth = InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", itemInfo, visibleSegmentCount);
			Assert.Equal(0.0, resultWidth);
		}

		[Fact]
		public void GetTotalSegmentWidth_ReturnsZero_WhenVisibleSegmentsCountIsGreaterThanItemsCount()
		{
			var itemInfo = new SfSegmentedControl
			{
				_items =
				[
					new SfSegmentItem { Width = 10},
					new SfSegmentItem { Width = 20},
					new SfSegmentItem { Width = 30}
				]
			};

			var visibleSegmentCount = 4;
			Assert.Throws<TargetInvocationException>(() => InvokeStaticPrivateMethodClass(typeof(SegmentViewHelper), "GetTotalSegmentWidth", itemInfo, visibleSegmentCount));

		}

		[Fact]
		public void UpdateSelectedIndex_ShouldReturnsExpectedValue()
		{
			var segmentItem = new SfSegmentItem() { Text = "Gabriella", Background = Colors.Blue };
			var itemsSource = new ObservableCollection<SfSegmentItem>
				{
					 new SfSegmentItem() { Text = "Jackson", Background = Colors.Red },
					 segmentItem,
					 new SfSegmentItem() { Text = "Liam", Background = Colors.Green },
				}
			;
			var itemInfo = new SfSegmentedControl() { _items = itemsSource };

			var segment = new SegmentItemView(itemInfo, null!);
			InvokePrivateMethod(segment, "UpdateSelectedIndex");
			Assert.Null(itemInfo.SelectedIndex);

			var segmentView = new SegmentItemView(itemInfo, segmentItem);
			InvokePrivateMethod(segmentView, "UpdateSelectedIndex");
			Assert.NotNull(itemInfo.SelectedIndex);
			Assert.Equal(segmentItem, itemsSource[itemInfo.SelectedIndex.Value]);
		}

		[Theory]
		[InlineData("Alice")]
		[InlineData("Jackson")]
		[InlineData("Mei")]
		[InlineData("Carlos")]
		[InlineData("Fatima")]

		public void SelectedAndDeSelectedIndex_SetValue_ReturnsExpectedValue(string text)
		{
			var segmentItem = new SfSegmentItem() { Text = text, Background = Colors.Blue };
			var itemsSource = new ObservableCollection<SfSegmentItem>
				{
					 new SfSegmentItem() { Text = "Jackson", Background = Colors.Red },
					 segmentItem,
					 new SfSegmentItem() { Text = "Liam", Background = Colors.Green },
				}
			;
			var itemInfo = new SfSegmentedControl() { _items = itemsSource };

			var segment = new SegmentItemView(itemInfo, segmentItem);
			InvokePrivateMethod(segment, "UpdateSelectedIndex");
			Assert.NotNull(itemInfo.SelectedIndex);
			Assert.Equal(segmentItem, itemsSource[itemInfo.SelectedIndex.Value]);
			SfSegmentedControl segmentControl = itemInfo;
			segmentControl.SelectionMode = SegmentSelectionMode.SingleDeselect;
			InvokePrivateMethod(segment, "UpdateSelectedIndex");
			Assert.Throws<ArgumentOutOfRangeException>(() => itemsSource[itemInfo.SelectedIndex.Value]);
		}
		#endregion
		#region Events
		[Fact]
		public void TappedInvoked_WhenTrigger()
		{
			SfSegmentedControl segmentedControl = new SfSegmentedControl();

			var fired = false;
			segmentedControl.Tapped += (sender, e) => fired = true;
			ISegmentItemInfo segmentItemInfo = segmentedControl;
			segmentItemInfo.TriggerTappedEvent(new SegmentTappedEventArgs());
			Assert.True(fired);
		}

		[Fact]
		public void ItemTapped_SelectsExpectedItem()
		{
			SfSegmentedControl segmentedControl = new SfSegmentedControl();

			var tappedItem = new SfSegmentItem();
			var segmentItem = new SfSegmentItem() { Text = "Jackson", Background = Colors.Red };
			segmentedControl.Tapped += (sender, e) => tappedItem = e.SegmentItem;
			ISegmentItemInfo segmentItemInfo = segmentedControl;
			SegmentTappedEventArgs segmentTapped = new SegmentTappedEventArgs() { SegmentItem = segmentItem };
			segmentItemInfo.TriggerTappedEvent(segmentTapped);
			Assert.Equal(segmentItem, tappedItem);
			Assert.Equal(segmentItem.Text, tappedItem.Text);
		}

		[Fact]
		public void TappedInvoked_WhenTap()
		{
			var fired = false;
			var segmentedControl = new SfSegmentedControl();
			segmentedControl.Tapped += (sender, e) => fired = true;
			var segmentView = new SegmentItemView(segmentedControl, null!);
			((ITapGestureListener)segmentView).OnTap(new TapEventArgs(new Point(30, 30), 1));
			Assert.False(fired);
		}

		[Fact]
		public void TappedItem_EqualsExpectedItem()
		{
			var tappedItem = new SfSegmentItem();
			var segmentItem = new SfSegmentItem() { Text = "Gabriella", Background = Colors.Blue };
			var segmentedControl = new SfSegmentedControl();
			segmentedControl.Tapped += (sender, e) => tappedItem = e.SegmentItem;
			var eventArgs = new TapEventArgs(new Point(30, 30), 1);
			var segmentView = new SegmentItemView(segmentedControl, segmentItem);
			((ITapGestureListener)segmentView).OnTap(eventArgs);
			Assert.Equal(segmentItem, tappedItem);
			Assert.Equal(segmentItem.Text, tappedItem.Text);
		}
		#endregion
	}
}
