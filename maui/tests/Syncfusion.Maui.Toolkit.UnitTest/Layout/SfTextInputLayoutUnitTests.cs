using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using Syncfusion.Maui.Toolkit.NumericEntry;
using System.Reflection;
using System.Linq;
namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfTextInputLayoutUnitTests : BaseUnitTest
	{
		#region Constructor Tests
		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var inputLayout = new SfTextInputLayout();

			Assert.Equal(ContainerType.Filled, inputLayout.ContainerType);
			Assert.True(inputLayout.ShowHint);
			Assert.True(inputLayout.ShowHelperText);
			Assert.True(inputLayout.EnableFloating);
			Assert.True(inputLayout.EnableHintAnimation);
			Assert.True(inputLayout.IsEnabled);
			Assert.False(inputLayout.HasError);
			Assert.True(inputLayout.ShowLeadingView);
			Assert.True(inputLayout.ShowTrailingView);
			Assert.False(inputLayout.IsHintAlwaysFloated);
			Assert.False(inputLayout.HasError);
			Assert.False(inputLayout.HasError);
			Assert.False(inputLayout.HasError);
			Assert.False(inputLayout.HasError);
			Assert.False(inputLayout.HasError);
			Assert.NotNull(inputLayout.HintLabelStyle);
			Assert.NotNull(inputLayout.HelperLabelStyle);
			Assert.NotNull(inputLayout.ErrorLabelStyle);
			Assert.Null(inputLayout.LeadingView);
			Assert.Null(inputLayout.TrailingView);
			Assert.Equal(ViewPosition.Inside, inputLayout.LeadingViewPosition);
			Assert.Equal(ViewPosition.Inside, inputLayout.TrailingViewPosition);
			Assert.Equal(3.5d, inputLayout.OutlineCornerRadius);
			Assert.Equal(2d, inputLayout.FocusedStrokeThickness);
			Assert.Equal(1d, inputLayout.UnfocusedStrokeThickness);
			Assert.Equal(int.MaxValue, inputLayout.CharMaxLength);
			Assert.Equal(-1, inputLayout.InputViewPadding.Left);
			Assert.Equal(-1, inputLayout.InputViewPadding.Top);
			Assert.Equal(-1, inputLayout.InputViewPadding.Right);
			Assert.Equal(-1, inputLayout.InputViewPadding.Bottom);
			Assert.False(inputLayout.EnablePasswordVisibilityToggle);
			Assert.Null(inputLayout.Content);
			Assert.Equal(12, inputLayout.HelperLabelStyle.FontSize);
			Assert.Equal(12, inputLayout.ErrorLabelStyle.FontSize);
			Assert.Equal(16, inputLayout.HintLabelStyle.FontSize);
			Assert.Null(inputLayout.ClearButtonPath);
			Assert.Equal(Color.FromArgb("#49454F"), inputLayout.HelperTextColor);
			Assert.Equal(Color.FromArgb("#49454F"), inputLayout.HintTextColor);
			Assert.False(inputLayout.IsHintFloated);
			Assert.NotNull(inputLayout.CounterLabelStyle);
			Assert.Equal(Color.FromArgb("#49454F"), inputLayout.ClearButtonColor);
			Assert.False(inputLayout.ShowCharCount);
			Assert.Equal(string.Empty, inputLayout.Text);
		}
		#endregion

		#region Property Tests

		[Theory]
		[InlineData(ContainerType.Filled)]
		[InlineData(ContainerType.Outlined)]
		[InlineData(ContainerType.None)]
		public void ContainerType_SetValue_ReturnsExpectedValue(ContainerType containerType)
		{
			var inputLayout = new SfTextInputLayout
			{
				ContainerType = containerType
			};

			Assert.Equal(containerType, inputLayout.ContainerType);
		}

		[Fact]
		public void Hint_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout
			{
				Hint = "Test Hint"
			};

			Assert.Equal("Test Hint", inputLayout.Hint);
		}

		[Fact]
		public void HelperText_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout
			{
				HelperText = "Test Helper"
			};

			Assert.Equal("Test Helper", inputLayout.HelperText);
		}

		[Fact]
		public void ErrorText_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout
			{
				ErrorText = "Test Error"
			};

			Assert.Equal("Test Error", inputLayout.ErrorText);
		}

		[Fact]
		public void HasError_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout
			{
				HasError = true
			};

			Assert.True(inputLayout.HasError);
		}

		[Fact]
		public void Stroke_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout();
			var color = Colors.Red;
			inputLayout.Stroke = color;

			Assert.Equal(color, inputLayout.Stroke);
		}

		[Fact]
		public void ContainerBackground_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout();
			var brush = new SolidColorBrush(Colors.Blue);
			inputLayout.ContainerBackground = brush;

			Assert.Equal(brush, inputLayout.ContainerBackground);
		}

		[Fact]
		public void OutlineCornerRadius_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout
			{
				OutlineCornerRadius = 10
			};

			Assert.Equal(10, inputLayout.OutlineCornerRadius);
		}

		[Fact]
		public void CharMaxLength_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout
			{
				CharMaxLength = 50
			};

			Assert.Equal(50, inputLayout.CharMaxLength);
		}

		[Fact]
		public void LeadingView_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout();
			var leadingView = new Label();
			inputLayout.LeadingView = leadingView;

			Assert.Equal(leadingView, inputLayout.LeadingView);
		}

		[Fact]
		public void TrailingView_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout();
			var trailingView = new Label();
			inputLayout.TrailingView = trailingView;

			Assert.Equal(trailingView, inputLayout.TrailingView);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowLeadingView_SetValue_ReturnsExpectedValue(bool showLeadingView)
		{
			var inputLayout = new SfTextInputLayout
			{
				ShowLeadingView = showLeadingView
			};
			var leadingView = new Label();
			inputLayout.LeadingView = leadingView;

			Assert.Equal(showLeadingView, inputLayout.LeadingView.IsVisible);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowTrailingView_SetValue_ReturnsExpectedValue(bool showTrailingView)
		{
			var inputLayout = new SfTextInputLayout
			{
				ShowTrailingView = showTrailingView
			};
			var trailingView = new Label();
			inputLayout.TrailingView = trailingView;

			Assert.Equal(showTrailingView, inputLayout.TrailingView.IsVisible);
		}

		[Theory]
		[InlineData(ViewPosition.Inside)]
		[InlineData(ViewPosition.Outside)]
		public void LeadingViewPosition_SetValue_ReturnsExpectedValue(ViewPosition position)
		{
			var inputLayout = new SfTextInputLayout
			{
				LeadingViewPosition = position
			};

			Assert.Equal(position, inputLayout.LeadingViewPosition);
		}

		[Theory]
		[InlineData(ViewPosition.Inside)]
		[InlineData(ViewPosition.Outside)]
		public void TrailingViewPosition_SetValue_ReturnsExpectedValue(ViewPosition position)
		{
			var inputLayout = new SfTextInputLayout
			{
				TrailingViewPosition = position
			};

			Assert.Equal(position, inputLayout.TrailingViewPosition);
		}

		[Fact]
		public void InputViewPadding_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout();
			var padding = new Thickness(5, 10, 5, 10);
			inputLayout.InputViewPadding = padding;

			Assert.Equal(padding, inputLayout.InputViewPadding);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnablePasswordVisibilityToggle_SetValue_ReturnsExpectedValue(bool enableToggle)
		{
			var inputLayout = new SfTextInputLayout
			{
				EnablePasswordVisibilityToggle = enableToggle
			};

			Assert.Equal(enableToggle, inputLayout.EnablePasswordVisibilityToggle);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowHint_SetValue_ReturnsExpectedValue(bool showHint)
		{
			var inputLayout = new SfTextInputLayout
			{
				ShowHint = showHint
			};

			Assert.Equal(showHint, inputLayout.ShowHint);
		}

		[Fact]
		public void FlowDirection_SetValue_ReturnsExpectedValue()
		{
			var inputLayout = new SfTextInputLayout
			{
				FlowDirection = FlowDirection.RightToLeft
			};

			Assert.Equal(FlowDirection.RightToLeft, inputLayout.FlowDirection);
		}
		#endregion

		#region BehaviorTests

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnablePasswordToogle_SetsEntryControlIsPasswordProperty(bool enablePasswordToggle)
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry();
			inputLayout.Content = entry;

			inputLayout.EnablePasswordVisibilityToggle = enablePasswordToggle;

			Assert.Equal(enablePasswordToggle, entry.IsPassword);
		}

		[Fact]
		public void ContentBackground_ShouldIgnoreSetValues_AlwaysReturnsTransparentColor()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry() { BackgroundColor = Colors.Red };
			inputLayout.Content = entry;

			Assert.Equal(Colors.Transparent, entry.BackgroundColor);
		}

		[Fact]
		public void IsHintFloated_RetrunsTrue_WhenContentTextIsNotNullOrEmpty()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry() { Text = "HintText" };
			inputLayout.Content = entry;

			Assert.True(inputLayout.IsHintFloated);
		}

		[Fact]
		public void IsHintFloated_RetrunsFalse_WhenContentTextIsEmpty()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry() { Text = string.Empty };
			inputLayout.Content = entry;

			Assert.False(inputLayout.IsHintFloated);
		}

		[Fact]
		public void ControlText_ShouldUpdate_BasedOnEntryText()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry() { Text = "Hello" };
			inputLayout.Content = entry;

			Assert.Equal("Hello", inputLayout.Text);
		}

		[Fact]
		public void ControlText_ShouldUpdate_BasedOnEditorText()
		{
			var inputLayout = new SfTextInputLayout();
			var editor = new Editor() { Text = "Hello" };
			inputLayout.Content = editor;

			Assert.Equal("Hello", inputLayout.Text);
		}

		[Fact]
		public void IsHintFloated_RetrunsTrue_WhenPickerSelectedItemIsNotNull()
		{
			var inputLayout = new SfTextInputLayout();
			var picker = new Microsoft.Maui.Controls.Picker { SelectedItem = "SelectedItem" };
			inputLayout.Content = picker;

			Assert.True(inputLayout.IsHintFloated);
		}

		[Fact]
		public void IsHintAlwaysFloated_ReturnsTrue_WhenContentIsDatePicker()
		{
			var inputLayout = new SfTextInputLayout();
			var datePicker = new DatePicker();
			inputLayout.Content = datePicker;

			Assert.True(inputLayout.IsHintAlwaysFloated);
		}

		[Fact]
		public void IsHintAlwaysFloated_ReturnsTrue_WhenContentIsTimePicker()
		{
			var inputLayout = new SfTextInputLayout();
			var timePicker = new TimePicker();
			inputLayout.Content = timePicker;

			Assert.True(inputLayout.IsHintAlwaysFloated);
		}

		[Theory]
		[InlineData("Hello", 1)]
		[InlineData("", 0)]
		public void Opacity_IsSetCorrectly_ForInputViewBasedOnIsHintFloated(string text, double expectedOpacity)
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry() { Text = text };
			inputLayout.Content = entry;

			Assert.Equal(expectedOpacity, entry.Opacity);
		}

		[Fact]
		public void Opacity_ChangesCorrectly_ForInputViewBasedOnIsHintFloated()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry();
			inputLayout.Content = entry;

			entry.Text = "Hello";

			Assert.Equal(1, entry.Opacity);
		}



		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Control_IsDisabled_WhenIsEnabledIsFalse(bool isEnabled)
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry();
			inputLayout.Content = entry;

			inputLayout.IsEnabled = isEnabled;

			Assert.Equal(isEnabled, entry.IsEnabled);
		}

		[Fact]
		public void OnPickerSelectedIndexChanged_SelectedIndexGreaterThanOrEqualToZero_ShouldSetIsHintFloatedToTrue()
		{
			var inputLayout = new SfTextInputLayout();
			var picker = new Microsoft.Maui.Controls.Picker
			{
				ItemsSource = new List<string> { "Item1", "Item2", "Items3" }
			};
			inputLayout.Content = picker;

			picker.SelectedIndex = 0;

			Assert.True(inputLayout.IsHintFloated);

			picker.SelectedIndex = -1;

			Assert.False(inputLayout.IsHintFloated);
		}

		[Fact]
		public void OnPickerSelectedIndexChanged_TextProperty_ShouldUpdateBasedOnIndex()
		{
			var inputLayout = new SfTextInputLayout();
			var picker = new Microsoft.Maui.Controls.Picker
			{
				ItemsSource = new List<string> { "Item1", "Item2", "Items3" }
			};
			inputLayout.Content = picker;

			picker.SelectedIndex = 1;

			Assert.Equal("Item2", inputLayout.Text);

			picker.SelectedIndex = -1;

			Assert.Equal(string.Empty, inputLayout.Text);
		}

		#endregion


		#region EventTests


		[Fact]
		public void TestPasswordVisibilityToggleEvent()
		{
			var inputLayout = new SfTextInputLayout()
			{
				EnablePasswordVisibilityToggle = true,
				Content = new Entry()
			};

			bool eventRaised = false;
			inputLayout.PasswordVisibilityToggled += (sender, args) =>
			{
				eventRaised = true;
			};

			var toggleMethod = typeof(SfTextInputLayout).GetMethod("ToggleIcon", BindingFlags.NonPublic | BindingFlags.Instance);
			toggleMethod?.Invoke(inputLayout, null);

			Assert.True(eventRaised);
		}

		#endregion

		#region ContentTests
		[Fact]
		public void Content_SettingEntry_ShouldUpdateProperty()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry();
			inputLayout.Content = entry;

			Assert.Equal(entry, inputLayout.Content);
		}

		[Fact]
		public void Content_SettingEditor_ShouldUpdateProperty()
		{
			var inputLayout = new SfTextInputLayout();
			var editor = new Editor();
			inputLayout.Content = editor;

			Assert.Equal(editor, inputLayout.Content);
		}

		[Fact]
		public void Content_SettingPicker_ShouldUpdateProperty()
		{
			var inputLayout = new SfTextInputLayout();
			var picker = new Microsoft.Maui.Controls.Picker();
			inputLayout.Content = picker;

			Assert.Equal(picker, inputLayout.Content);
		}

		[Fact]
		public void Content_SettingDatePicker_ShouldUpdateProperty()
		{
			var inputLayout = new SfTextInputLayout();
			var datePicker = new DatePicker();
			inputLayout.Content = datePicker;

			Assert.Equal(datePicker, inputLayout.Content);
		}

		[Fact]
		public void Content_SettingTimePicker_ShouldUpdateProperty()
		{
			var inputLayout = new SfTextInputLayout();
			var timePicker = new TimePicker();
			inputLayout.Content = timePicker;

			Assert.Equal(timePicker, inputLayout.Content);
		}

		#endregion

		#region Internal/Private Method or Property Tests

		[Fact]
		public void TestHintLabelFontSize()
		{
			var inputLayout = new SfTextInputLayout()
			{
				Hint = "Enter text here",
				HintLabelStyle = new LabelStyle { FontSize = 16 },
			};
			Assert.Equal(16, inputLayout.HintLabelStyle.FontSize);
		}

		[Fact]
		public void TestHelperLabelFontSize()
		{
			var inputLayout = new SfTextInputLayout()
			{
				HelperText = "Enter text here",
				HelperLabelStyle = new LabelStyle { FontSize = 16 },
			};
			Assert.Equal(16, inputLayout.HelperLabelStyle.FontSize);
		}

		[Fact]
		public void TestErrorTextLabelFontSize()
		{
			var inputLayout = new SfTextInputLayout()
			{
				ErrorText = "Enter text here",
				ErrorLabelStyle = new LabelStyle { FontSize = 16 },
			};
			Assert.Equal(16, inputLayout.ErrorLabelStyle.FontSize);
		}

		[Fact]
		public void TestCurrentActiveColor()
		{
			var inputLayout = new SfTextInputLayout();
			Assert.Equal(Color.FromRgba("#79747E"), inputLayout.CurrentActiveColor);
		}

		[Fact]
		public void TestClearButtonPath()
		{
			var inputLayout = new SfTextInputLayout();
			Microsoft.Maui.Controls.Shapes.Path path = new Microsoft.Maui.Controls.Shapes.Path();
			inputLayout.ClearButtonPath = path;
			Assert.Equal(path, inputLayout.ClearButtonPath);
		}

		[Fact]
		public void TestOutlinedContainerBackground()
		{
			var inputLayout = new SfTextInputLayout();
			var redBrush = new SolidColorBrush(Colors.Red);
			SetPrivateField(inputLayout, "_outlinedContainerBackground", redBrush);
			var expectedBrushObj = GetPrivateField(inputLayout, "_outlinedContainerBackground");
			Assert.NotNull(expectedBrushObj);

			var expectedBrush = expectedBrushObj as SolidColorBrush;
			Assert.NotNull(expectedBrush);
			Assert.Equal(Colors.Red, expectedBrush.Color);
		}

		[Fact]
		public void TestHelperTextColor()
		{
			var inputLayout = new SfTextInputLayout
			{
				HelperTextColor = Colors.Green
			};
			Assert.Equal(Colors.Green, inputLayout.HelperTextColor);
		}

		[Fact]
		public void TestHintTextColor()
		{
			var inputLayout = new SfTextInputLayout
			{
				HintTextColor = Colors.Yellow
			};
			Assert.Equal(Colors.Yellow, inputLayout.HintTextColor);
		}

		[Fact]
		public void TestIsHintFloated()
		{
			var inputLayout = new SfTextInputLayout
			{
				IsHintFloated = true
			};
			Assert.True(inputLayout.IsHintFloated);
		}

		[Fact]
		public void TestCounterLabelStyle()
		{
			var inputLayout = new SfTextInputLayout();
			var fontSize = 12;
			inputLayout.CounterLabelStyle = new LabelStyle() { FontSize = fontSize };
			Assert.Equal(fontSize, inputLayout.CounterLabelStyle.FontSize);
		}

		[Fact]
		public void TestClearButtonColor()
		{
			var inputLayout = new SfTextInputLayout
			{
				ClearButtonColor = Colors.Red
			};
			Assert.Equal(Colors.Red, inputLayout.ClearButtonColor);
		}

		[Fact]
		public void TestShowCharCount()
		{
			var inputLayout = new SfTextInputLayout
			{
				ShowCharCount = true
			};
			Assert.True(inputLayout.ShowCharCount);
		}

		[Fact]
		public void TestIsLayoutFocused()
		{
			var inputLayout = new SfTextInputLayout();
			bool IsLayoutFocusedProperty = (bool)GetNonPublicProperty(inputLayout, "IsLayoutFocused")!;
			Assert.False(IsLayoutFocusedProperty);
		}

		[Fact]
		public void TestOnPickerSelectedIndexChanged_SelectedIndex()
		{
			var inputLayout = new SfTextInputLayout();
			var picker = new Microsoft.Maui.Controls.Picker();
			picker.Items.Add("Apple");
			picker.Items.Add("Orange");
			picker.Items.Add("Strawberry");
			picker.SelectedIndex = 0;
			inputLayout.Content = picker;
			var button = new Button();
			var btn_LessthanZero = new Button();

			button.Clicked += (sender, e) =>
			{
				picker.SelectedIndex = 1;
			};

			btn_LessthanZero.Clicked += (sender, e) =>
			{
				picker.SelectedIndex = -1;
			};


			button.SendClicked();
			btn_LessthanZero.SendClicked();
			Assert.Equal(picker, inputLayout.Content);
		}

		[Fact]
		public void TestUpdateIconRectFMethod()
		{
			var inputLayout = new SfTextInputLayout
			{
				EnablePasswordVisibilityToggle = true,
				Content = new Entry(),
				LeadingView = new Entry(),
				LeadingViewPosition = ViewPosition.Outside,
				TrailingView = new Entry(),
				TrailingViewPosition = ViewPosition.Outside
			};
			InvokePrivateMethod(inputLayout, "UpdateIconRectF");
			var outRectExpected = new RectF() { X = 21, Y = 2, Width = -43, Height = -24 };
			var resultOutRect = GetPrivateField(inputLayout, "_outlineRectF");
			Assert.Equal(resultOutRect, outRectExpected);
			var backgroundRectExpected = new RectF() { X = 19, Y = 0, Width = -39, Height = -22 };
			var resultBackgroundRect = GetPrivateField(inputLayout, "_backgroundRectF");
			Assert.Equal(resultBackgroundRect, backgroundRectExpected);
			var _passwordToggleIconRectF = new RectF() { X = -83, Y = -27, Width = 32, Height = 32 };
			var resultPassToggleRect = GetPrivateField(inputLayout, "_passwordToggleIconRectF");
			Assert.Equal(resultPassToggleRect, _passwordToggleIconRectF);
		}

		[Fact]
		public void TestAddDefaultVSMMethod()
		{
			var inputLayout = new SfTextInputLayout();
			InvokePrivateMethod(inputLayout, "AddDefaultVSM");
			bool result = inputLayout.HasVisualStateGroups();
			Assert.True(result);
		}

		[Fact]
		public void TestUpdateCounterLabelTextMethod()
		{
			var inputLayout = new SfTextInputLayout() { ShowCharCount = true, Text = "Input" };
			InvokePrivateMethod(inputLayout, "UpdateCounterLabelText");
			var expected = "5";
			var result = GetPrivateField(inputLayout, "_counterText");
			Assert.Equal(expected, result);
		}

		[Fact]
		public void TestUpdateHintPositionMethod_IsAnimating()
		{
			var inputLayout = new SfTextInputLayout
			{
				IsHintAlwaysFloated = true,
				LeadingView = new Entry(),
				LeadingViewPosition = ViewPosition.Outside,
				TrailingView = new Entry(),
				TrailingViewPosition = ViewPosition.Outside
			};
			bool isAnimate = true;
			SetPrivateField(inputLayout, "_isAnimating", isAnimate);
			InvokePrivateMethod(inputLayout, "UpdateHintPosition");
			var result = GetPrivateField(inputLayout, "_isAnimating");
			if (result != null)
			{
				Assert.Equal(isAnimate, (bool)result);
			}
		}

		[Theory]
		[InlineData(ContainerType.Filled)]
		[InlineData(ContainerType.None)]
		[InlineData(ContainerType.Outlined)]
		public void TestUpdateHintPositionMethod_IsHintAlwaysFloatedTrue(ContainerType containerType)
		{
			var inputLayout = new SfTextInputLayout
			{
				IsHintAlwaysFloated = true,
				ContainerType = containerType,
				LeadingView = new Entry(),
				LeadingViewPosition = ViewPosition.Outside,
				TrailingView = new Entry(),
				TrailingViewPosition = ViewPosition.Outside
			};
			InvokePrivateMethod(inputLayout, "UpdateHintPosition");
			var result = GetPrivateField(inputLayout, "_hintRect");
			Assert.NotNull(result);
		}

		[Theory]
		[InlineData(ContainerType.Filled)]
		[InlineData(ContainerType.None)]
		[InlineData(ContainerType.Outlined)]
		public void TestUpdateHintPositionMethod_IsHintAlwaysFloatedFalse(ContainerType containerType)
		{
			var inputLayout = new SfTextInputLayout
			{
				IsHintAlwaysFloated = false,
				ContainerType = containerType,
				LeadingView = new Entry(),
				LeadingViewPosition = ViewPosition.Outside,
				TrailingView = new Entry(),
				TrailingViewPosition = ViewPosition.Outside
			};
			InvokePrivateMethod(inputLayout, "UpdateHintPosition");
			var result = GetPrivateField(inputLayout, "_hintRect");
			Assert.NotNull(result);
		}

		[Fact]
		public void TestUpdateHelperTextPositionMethod()
		{
			var inputLayout = new SfTextInputLayout
			{
				LeadingView = new Entry(),
				LeadingViewPosition = ViewPosition.Outside,
				TrailingView = new Entry(),
				TrailingViewPosition = ViewPosition.Outside
			};
			var expected = new RectF() { X = 60, Y = -18, Width = -96, Height = 16 };
			InvokePrivateMethod(inputLayout, "UpdateHelperTextPosition");
			var result = GetPrivateField(inputLayout, "_helperTextRect");
			Assert.Equal(expected, result);
		}

		[Fact]
		public void TestOnInputViewTextChangedMethodWithText()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry();
			inputLayout.Content = entry;
			var newText = "T";
			var oldText = entry.Text;
			var textChangedEventArgs = new TextChangedEventArgs(oldText, newText);
			InvokePrivateMethod(inputLayout, "OnTextInputViewTextChanged", entry, textChangedEventArgs);
			Assert.Equal(newText, inputLayout.Text);
		}

		[Fact]
		public void TestOnInputViewTextChangedMethodWithoutText()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry();
			inputLayout.Content = entry;
			var newText = string.Empty;
			var oldText = entry.Text;
			var textChangedEventArgs = new TextChangedEventArgs(oldText, newText);
			InvokePrivateMethod(inputLayout, "OnTextInputViewTextChanged", entry, textChangedEventArgs);
			Assert.Equal(newText, inputLayout.Text);
		}

		[Fact]
		public void TestGetHintLineCountMethod()
		{
			var inputLayout = new SfTextInputLayout();
			int width = 300;
			int expected = 1;
			var result = InvokePrivateMethod(inputLayout, "GetHintLineCount", width);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void TestGetAssistiveTextLineCountMethod()
		{
			var inputLayout = new SfTextInputLayout();
			int width = 300;
			int expected = 1;
			var result = InvokePrivateMethod(inputLayout, "GetAssistiveTextLineCount", width);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void TestUpdateErrorTextPositionMethod()
		{
			var inputLayout = new SfTextInputLayout();
			InvokePrivateMethod(inputLayout, "UpdateErrorTextPosition");
			RectF expectedRect = new RectF() { X = 16, Y = -18, Width = -33, Height = 16 };
			var result = GetPrivateField(inputLayout, "_errorTextRect");
			Assert.Equal(expectedRect, result);
		}

		[Fact]
		public void TestUpdateErrorTextColorMethod()
		{
			var inputLayout = new SfTextInputLayout();
			InvokePrivateMethod(inputLayout, "UpdateErrorTextColor");
			var expected = Color.FromRgba(0, 0, 0, 0.87);
			if (inputLayout.ErrorLabelStyle != null)
			{
				Assert.Equal(expected, inputLayout.ErrorLabelStyle.TextColor);
			}
		}

		[Fact]
		public void TestUpdateCounterTextColorMethod()
		{
			var inputLayout = new SfTextInputLayout();
			InvokePrivateMethod(inputLayout, "UpdateCounterTextColor");
			var expected = Color.FromRgba(0, 0, 0, 0.87);
			Assert.Equal(expected, inputLayout.CounterLabelStyle.TextColor);
		}

		[Fact]
		public void TestUpdateBaseLinePointsMethod()
		{
			var inputLayout = new SfTextInputLayout
			{
				ContainerType = ContainerType.Filled
			};
			InvokePrivateMethod(inputLayout, "UpdateBaseLinePoints");
			var startResult = GetPrivateField(inputLayout, "_startPoint");
			var endResult = GetPrivateField(inputLayout, "_endPoint");
			PointF startPoint = new PointF() { X = 0, Y = -22 };
			PointF endPoint = new PointF() { X = -1, Y = -22 };
			Assert.Equal(startResult, startPoint);
			Assert.Equal(endResult, endPoint);
		}
		#endregion

		#region Automation Cases


		[Theory]
		[InlineData(ContainerType.Filled)]
		[InlineData(ContainerType.Outlined)]
		[InlineData(ContainerType.None)]
		public void Content_SettingEditor(ContainerType container)
		{
			var inputLayout = new SfTextInputLayout();
			var editor = new Editor();
			editor.Text = "Enter your name";
			editor.FontSize = 13;
			inputLayout.ContainerType = container;
			inputLayout.Content = editor;

			Assert.Equal(editor, inputLayout.Content);
			Assert.Equal("Enter your name", editor.Text);
			Assert.Equal(13, editor.FontSize);
			Assert.Equal(container, inputLayout.ContainerType);
		}

		[Fact]
		public void Content_SettingEntry()
		{
			var inputLayout = new SfTextInputLayout();
			var entry = new Entry();
			entry.Text = "Enter your name";
			entry.FontSize = 13;
			inputLayout.Content = entry;

			Assert.Equal(entry, inputLayout.Content);
			Assert.Equal("Enter your name", entry.Text);
			Assert.Equal(13, entry.FontSize);
		}

		[Fact]
		public void ErrorText_HasError()
		{
			var inputLayout = new SfTextInputLayout
			{
				ErrorText = "Test Error"
			};
			inputLayout.HasError = true;

			Assert.Equal("Test Error", inputLayout.ErrorText);
			Assert.True(inputLayout.HasError);
		}

		[Theory]
		[InlineData(ViewPosition.Inside)]
		[InlineData(ViewPosition.Outside)]
		public void LeadingViewPosition_ShowLeadingView(ViewPosition position)
		{
			var inputLayout = new SfTextInputLayout();
			inputLayout.ShowLeadingView = true;
			inputLayout.LeadingViewPosition = position;
			var leadingView = new Label();
			inputLayout.LeadingView = leadingView;


			Assert.Equal(position, inputLayout.LeadingViewPosition);
			Assert.True(inputLayout.LeadingView.IsVisible);
		}

		[Theory]
		[InlineData(ViewPosition.Inside)]
		[InlineData(ViewPosition.Outside)]
		public void TrailingViewPosition_ShowTrailingView(ViewPosition position)
		{
			var inputLayout = new SfTextInputLayout();
			inputLayout.TrailingViewPosition = position;
			inputLayout.ShowTrailingView = true;
			var trailingView = new Label();
			inputLayout.TrailingView = trailingView;

			Assert.Equal(position, inputLayout.TrailingViewPosition);
			Assert.True(inputLayout.TrailingView.IsVisible);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnablePasswordVisibilityToggle(bool enableToggle)
		{
			var inputLayout = new SfTextInputLayout
			{
				EnablePasswordVisibilityToggle = enableToggle
			};
			bool? isPasswordVisible = (bool?)GetPrivateField(inputLayout, "_isPasswordTextVisible");

			Assert.Equal(enableToggle, inputLayout.EnablePasswordVisibilityToggle);
			Assert.False(isPasswordVisible);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowHelperText_SetValue_ReturnsExpectedValue(bool showHelperText)
		{
			var inputLayout = new SfTextInputLayout
			{
				ShowHelperText = showHelperText
			};

			Assert.Equal(showHelperText, inputLayout.ShowHelperText);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ReserveSpaceForAssistiveLabels_SetValue(bool expectedValue)
		{
			var inputLayout = new SfTextInputLayout
			{
				ReserveSpaceForAssistiveLabels = expectedValue
			};

			Assert.Equal(expectedValue, inputLayout.ReserveSpaceForAssistiveLabels);
		}

		[Fact]
		public void TestStackLayout()
		{
			var entry = new Entry();
			var textInputLayout1 = new SfTextInputLayout { Content = entry };
			var textInputLayout2 = new SfTextInputLayout { Content = entry };
			var stackLayout = new StackLayout { Orientation = StackOrientation.Vertical };
			stackLayout.Children.Add(textInputLayout1);
			stackLayout.Children.Add(textInputLayout2);

			Assert.Contains(textInputLayout1, stackLayout.Children);
			Assert.Contains(textInputLayout2, stackLayout.Children);
		}

		[Fact]
		public void TestHorizontalLayout()
		{
			var entry = new Entry();
			var textInputLayout = new SfTextInputLayout { Content = entry };
			var stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			stackLayout.Children.Add(textInputLayout);

			Assert.Equal(StackOrientation.Horizontal, stackLayout.Orientation);
			Assert.Contains(textInputLayout, stackLayout.Children);
		}

		[Fact]
		public void TestVerticalLayout()
		{
			var entry = new Entry();
			var textInputLayout = new SfTextInputLayout { Content = entry };
			var stackLayout = new StackLayout { Orientation = StackOrientation.Vertical };
			stackLayout.Children.Add(textInputLayout);

			Assert.Equal(StackOrientation.Vertical, stackLayout.Orientation);
			Assert.Contains(textInputLayout, stackLayout.Children);
		}

		[Fact]
		public void TestGrid()
		{
			var entry = new Entry();
			var textInputLayout = new SfTextInputLayout { Content = entry };

			var grid = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition { Width = GridLength.Auto }
				}
			};

			grid.Children.Add(textInputLayout);
			Grid.SetRow(textInputLayout, 0);
			Grid.SetColumn(textInputLayout, 0);
			Assert.Contains(textInputLayout, grid.Children);
			Assert.Single(grid.RowDefinitions);
			Assert.Single(grid.ColumnDefinitions);
		}

		[Fact]
		public void TestBorder()
		{
			var entry = new Entry();
			var textInputLayout = new SfTextInputLayout { Content = entry };
			var border = new Border
			{
				Content = textInputLayout,
				Stroke = Colors.Green,
				StrokeThickness = 2,
				BackgroundColor = Colors.White
			};

			Assert.Equal(textInputLayout, border.Content);

			Assert.Equal(Colors.Green, border.Stroke);
			Assert.Equal(2, border.StrokeThickness);
			Assert.Equal(Colors.White, border.BackgroundColor);
		}

		[Theory]
		[InlineData(ContainerType.Filled)]
		[InlineData(ContainerType.Outlined)]
		[InlineData(ContainerType.None)]
		public void Content_SfNumericEntry(ContainerType container)
		{
			var inputLayout = new SfTextInputLayout();
			var numericEntry = new SfNumericEntry();
			numericEntry.Value = 123;
			numericEntry.FontSize = 13;
			inputLayout.ContainerType = container;
			inputLayout.Content = numericEntry;

			Assert.Equal(numericEntry, inputLayout.Content);
			Assert.Equal(13, numericEntry.FontSize);
			Assert.Equal(container, inputLayout.ContainerType);
		}

		[Fact]
		public void Content_SfNumericEntry_AllowNull()
		{
			var inputLayout = new SfTextInputLayout();
			var numericEntry = new SfNumericEntry();
			numericEntry.Value = 123;
			numericEntry.AllowNull = true;
			inputLayout.Content = numericEntry;

			Assert.Equal(numericEntry, inputLayout.Content);
			Assert.True(numericEntry.AllowNull);
		}

		[Theory]
		[InlineData(ContainerType.Filled)]
		[InlineData(ContainerType.Outlined)]
		[InlineData(ContainerType.None)]
		public void Content_Picker(ContainerType container)
		{
			var inputLayout = new SfTextInputLayout();
			var picker = new Microsoft.Maui.Controls.Picker();
			picker.Items.Add("Item 1");
			picker.Items.Add("Item 2");
			picker.Items.Add("Item 3");
			picker.SelectedIndex = 1;
			inputLayout.ContainerType = container;
			inputLayout.Content = picker;

			Assert.Equal(picker, inputLayout.Content);
			Assert.Equal(container, inputLayout.ContainerType);
		}

		[Theory]
		[InlineData(ContainerType.Filled)]
		[InlineData(ContainerType.Outlined)]
		[InlineData(ContainerType.None)]
		public void Content_TimePicker(ContainerType container)
		{
			var inputLayout = new SfTextInputLayout();
			var timePicker = new TimePicker
			{
				Time = new TimeSpan(10, 30, 0)
			};

			inputLayout.ContainerType = container;
			inputLayout.Content = timePicker;

			Assert.Equal(timePicker, inputLayout.Content);
			Assert.Equal(container, inputLayout.ContainerType);
			Assert.Equal(new TimeSpan(10, 30, 0), timePicker.Time);
		}


		#endregion

		#region Accessibility Tests

		[Fact]
		public void GetSemanticsNodes_HelperTextVisible_CreatesHelperTextSemanticNode()
		{
			// Arrange
			var inputLayout = new SfTextInputLayout();
			inputLayout.HelperText = "Test helper text";
			inputLayout.ShowHelperText = true;
			inputLayout.HasError = false;
			
			// Set up size to ensure layout has occurred
			inputLayout.Width = 300;
			inputLayout.Height = 100;
			
			// Use reflection to set the helper text rect as it would be set during layout
			var helperTextRectField = typeof(SfTextInputLayout).GetField("_helperTextRect", BindingFlags.NonPublic | BindingFlags.Instance);
			helperTextRectField?.SetValue(inputLayout, new RectF(10, 70, 200, 20));

			// Act
			var semanticsNodes = inputLayout.GetSemanticsNodes(300, 100);

			// Assert
			Assert.NotNull(semanticsNodes);
			var helperTextNode = semanticsNodes.FirstOrDefault(n => n.Text.Contains("Helper text: Test helper text"));
			Assert.NotNull(helperTextNode);
			Assert.Equal(1000, helperTextNode.Id);
			Assert.Equal(10, helperTextNode.Bounds.X);
			Assert.Equal(70, helperTextNode.Bounds.Y);
			Assert.Equal(200, helperTextNode.Bounds.Width);
			Assert.Equal(20, helperTextNode.Bounds.Height);
		}

		[Fact]
		public void GetSemanticsNodes_ErrorTextVisible_CreatesErrorTextSemanticNode()
		{
			// Arrange
			var inputLayout = new SfTextInputLayout();
			inputLayout.ErrorText = "Test error text";
			inputLayout.HasError = true;
			
			// Set up size to ensure layout has occurred
			inputLayout.Width = 300;
			inputLayout.Height = 100;
			
			// Use reflection to set the error text rect as it would be set during layout
			var errorTextRectField = typeof(SfTextInputLayout).GetField("_errorTextRect", BindingFlags.NonPublic | BindingFlags.Instance);
			errorTextRectField?.SetValue(inputLayout, new RectF(10, 70, 200, 20));

			// Act
			var semanticsNodes = inputLayout.GetSemanticsNodes(300, 100);

			// Assert
			Assert.NotNull(semanticsNodes);
			var errorTextNode = semanticsNodes.FirstOrDefault(n => n.Text.Contains("Error text: Test error text"));
			Assert.NotNull(errorTextNode);
			Assert.Equal(1001, errorTextNode.Id);
			Assert.Equal(10, errorTextNode.Bounds.X);
			Assert.Equal(70, errorTextNode.Bounds.Y);
			Assert.Equal(200, errorTextNode.Bounds.Width);
			Assert.Equal(20, errorTextNode.Bounds.Height);
		}

		[Fact]
		public void GetSemanticsNodes_ErrorTextVisibleOverridesHelperText_OnlyCreatesErrorTextSemanticNode()
		{
			// Arrange
			var inputLayout = new SfTextInputLayout();
			inputLayout.HelperText = "Test helper text";
			inputLayout.ErrorText = "Test error text";
			inputLayout.ShowHelperText = true;
			inputLayout.HasError = true; // Error text should override helper text
			
			// Set up size to ensure layout has occurred
			inputLayout.Width = 300;
			inputLayout.Height = 100;
			
			// Use reflection to set both text rects
			var helperTextRectField = typeof(SfTextInputLayout).GetField("_helperTextRect", BindingFlags.NonPublic | BindingFlags.Instance);
			var errorTextRectField = typeof(SfTextInputLayout).GetField("_errorTextRect", BindingFlags.NonPublic | BindingFlags.Instance);
			helperTextRectField?.SetValue(inputLayout, new RectF(10, 70, 200, 20));
			errorTextRectField?.SetValue(inputLayout, new RectF(10, 70, 200, 20));

			// Act
			var semanticsNodes = inputLayout.GetSemanticsNodes(300, 100);

			// Assert
			Assert.NotNull(semanticsNodes);
			var errorTextNode = semanticsNodes.FirstOrDefault(n => n.Text.Contains("Error text: Test error text"));
			var helperTextNode = semanticsNodes.FirstOrDefault(n => n.Text.Contains("Helper text: Test helper text"));
			
			Assert.NotNull(errorTextNode);
			Assert.Null(helperTextNode); // Helper text should not be present when error text is shown
		}

		[Fact]
		public void GetSemanticsNodes_NoAssistiveText_DoesNotCreateAssistiveTextSemanticNodes()
		{
			// Arrange
			var inputLayout = new SfTextInputLayout();
			inputLayout.HelperText = "";
			inputLayout.ErrorText = "";
			inputLayout.ShowHelperText = true;
			inputLayout.HasError = false;
			
			// Set up size to ensure layout has occurred
			inputLayout.Width = 300;
			inputLayout.Height = 100;

			// Act
			var semanticsNodes = inputLayout.GetSemanticsNodes(300, 100);

			// Assert
			Assert.NotNull(semanticsNodes);
			var assistiveTextNode = semanticsNodes.FirstOrDefault(n => n.Text.Contains("Helper text:") || n.Text.Contains("Error text:"));
			Assert.Null(assistiveTextNode);
		}

		#endregion
	}
}

