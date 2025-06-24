using Microsoft.Maui.Controls.PlatformConfiguration;
using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.OtpInput;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfOtpInputUnitTests : BaseUnitTest
	{

		#region Constructor

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var otpInput = new SfOtpInput();
			Assert.Null(otpInput.Placeholder);
			Assert.True(otpInput.IsEnabled);
			Assert.False(otpInput.AutoFocus);
			Assert.Null(otpInput.Value);
			Assert.Equal(4, otpInput.Length);
			Assert.Equal(string.Empty, otpInput.Separator);
			Assert.Equal(OtpInputStyle.Outlined, otpInput.StylingMode);
			Assert.Equal(OtpInputType.Number, otpInput.Type);
			Assert.Equal('●', otpInput.MaskCharacter);
			Assert.Equal(OtpInputState.Default, otpInput.InputState);
			if (otpInput.SeparatorColor is Color color)
			{
				var SeparatorColor = color;
				Assert.Equal(Color.FromArgb("#CAC4D0"), SeparatorColor);
			}

		}

		#endregion

		#region OTPEntry

		#region Internal Property
		[Theory]
		[InlineData("1234")]
		[InlineData("abcd")]
		[InlineData("ABCD")]
		[InlineData(",#$&")]
		public void Text(string text)
		{
			OTPEntry otpEntry = new OTPEntry();
			otpEntry.Text = text;
			Assert.Equal(otpEntry.Text, text);
		}
		#endregion

		#region Private Method

		[Theory]
		[InlineData(false,true, "#611c1b1f")]
		[InlineData(true, true, "#1C1B1F")]
		public void GetVisualState_Outlined(bool enabled,bool hovered, string stroke)
		{
			OTPEntry otpEntry = new OTPEntry();
			SetPrivateField(otpEntry, "_sfOtpInput", new SfOtpInput());
			SetPrivateField(otpEntry, "_isEnabled", enabled);
			SetPrivateField(otpEntry, "_isHovered", hovered);
			SetPrivateField(otpEntry, "_styleMode", OtpInputStyle.Outlined);
			InvokePrivateMethod(otpEntry, "GetVisualState");
			Color? resultStroke = (Color?)GetPrivateField(otpEntry, "_stroke");
			Color? resultBackground = (Color?)GetPrivateField(otpEntry, "_background");
			Color expectedStroke = Color.FromArgb(stroke);
			Assert.Equal(expectedStroke, resultStroke);
			Assert.Equal(Colors.Transparent, resultBackground);
		}

		[Theory]
		[InlineData(false, true, "#611c1b1f")]
		[InlineData(true, true, "#1C1B1F")]
		public void GetVisualState_Underlined(bool enabled, bool hovered, string stroke)
		{
			OTPEntry otpEntry = new OTPEntry();
			SetPrivateField(otpEntry, "_sfOtpInput", new SfOtpInput());
			SetPrivateField(otpEntry, "_isEnabled", enabled);
			SetPrivateField(otpEntry, "_isHovered", hovered);
			SetPrivateField(otpEntry, "_styleMode", OtpInputStyle.Underlined);
			InvokePrivateMethod(otpEntry, "GetVisualState");
			Color? resultStroke = (Color?)GetPrivateField(otpEntry, "_stroke");
			Color? resultBackground = (Color?)GetPrivateField(otpEntry, "_background");
			Color expectedStroke = Color.FromArgb(stroke);
			Assert.Equal(expectedStroke, resultStroke);
			Assert.Equal(Colors.Transparent, resultBackground);
		}

		[Theory]
		[InlineData(false, true, "#611c1b1f", "0a1c1b1f")]
		[InlineData(true, true, "#1C1B1F", "#D7D0DD")]
		public void GetVisualState_Filled(bool enabled, bool hovered, string stroke, string background)
		{
			OTPEntry otpEntry = new OTPEntry();
			SetPrivateField(otpEntry, "_sfOtpInput", new SfOtpInput());
			SetPrivateField(otpEntry, "_isEnabled", enabled);
			SetPrivateField(otpEntry, "_isHovered", hovered);
			SetPrivateField(otpEntry, "_styleMode", OtpInputStyle.Filled);
			InvokePrivateMethod(otpEntry, "GetVisualState");
			Color? resultStroke = (Color?)GetPrivateField(otpEntry, "_stroke");
			Color? resultBackground = (Color?)GetPrivateField(otpEntry, "_background");
			Color expectedStroke = Color.FromArgb(stroke);
			Color expectedBackground = Color.FromArgb(background);
			Assert.Equal(expectedStroke, resultStroke);
			Assert.Equal(expectedBackground, resultBackground);
		}

		#endregion

		#region Internal Properties

		[Theory]
		[InlineData(OtpInputStyle.Underlined, 10, true, OtpInputState.Default, "#FF0000", "#008000", "#1C1B1F", "#611c1b1f")]
		[InlineData(OtpInputStyle.Underlined, 12, false, OtpInputState.Success, "#0000FF", "#008000", "#1C1B1F", "#611c1b1f")]
		[InlineData(OtpInputStyle.Filled, 9, true, OtpInputState.Default, "#0000FF", "#FFA500", "#1C1B1F", "#611c1b1f")]
		[InlineData(OtpInputStyle.Filled, 5, false, OtpInputState.Warning, "#FFA500", "#FF0000", "#1C1B1F", "#611c1b1f")]
		[InlineData(OtpInputStyle.Outlined, 15, true, OtpInputState.Default, "#FF0000", "#008000", "#1C1B1F", "#611c1b1f")]
		[InlineData(OtpInputStyle.Outlined, 13, false, OtpInputState.Error, "#008000", "#FF0000", "#1C1B1F", "#611c1b1f")]
		public void UpdateParameters(OtpInputStyle otpInputStyle, double cornerRadius, bool isEnabled, OtpInputState otpInputState, string stroke, string background, string textColor, string disabledTextColor)
		{
			OTPEntry otpEntry = new OTPEntry();
			otpEntry.UpdateParameters(otpInputStyle, cornerRadius, new PointF(10, 10), new PointF(30, 30), new SfOtpInput(),isEnabled, otpInputState, Color.FromArgb(stroke), Color.FromArgb(background), Color.FromArgb(textColor), Color.FromArgb(disabledTextColor));
			OtpInputStyle? resultInputStyle = (OtpInputStyle?)GetPrivateField(otpEntry, "_styleMode");
			double? resultCornerRadius = (double?)GetPrivateField(otpEntry, "_cornerRadius");
			bool? resultIsEnabled = (bool?)GetPrivateField(otpEntry, "_isEnabled");
			OtpInputState? resultInputState = (OtpInputState?)GetPrivateField(otpEntry, "_inputState");
			Color? resultStroke = (Color?)GetPrivateField(otpEntry, "_stroke");
			Color? resultBackground = (Color?)GetPrivateField(otpEntry, "_background");
			Color? resultTextColor = (Color?)GetPrivateField(otpEntry, "_textColor");
			Color? resultBackgrounDisabledTextColor = (Color?)GetPrivateField(otpEntry, "_disabledTextColor");
			Assert.Equal(otpInputStyle, resultInputStyle);
			Assert.Equal(cornerRadius, resultCornerRadius);
			Assert.Equal(isEnabled, resultIsEnabled);
			Assert.Equal(otpInputState, resultInputState);
		}

		#endregion

		#region Public Method
		[Fact]
		public void OnTouch()
		{
			OTPEntry otpEntry = new OTPEntry();
			var touchEventArgs = new Internals.PointerEventArgs(1, PointerActions.Entered, new Point(30, 30));
			otpEntry.OnTouch(touchEventArgs);
			bool? result = (bool?) GetPrivateField(otpEntry, "_isHovered");
			Assert.True(result);
		}

		#endregion

		#endregion

		#region Public Properties

		[Theory]
		[InlineData("1234")]
		public void Value(string expectedValue)
		{
			var otpInput = new SfOtpInput();
			otpInput.Value = expectedValue;
			Assert.Equal(expectedValue, otpInput.Value);
		}

		[Theory]
		[InlineData("X")]
		[InlineData("XYZZ")]
		[InlineData("@")]
		public void Placeholder(string expectedValue)
		{
			var otpInput = new SfOtpInput();
			otpInput.Placeholder = expectedValue;
			Assert.Equal(expectedValue, otpInput.Placeholder);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		public void Length(double value)
		{
			var otpInput = new SfOtpInput();
			otpInput.Length = value;
			if (value <= 0)
			{
				Assert.Equal(4, otpInput.Length);
			}
			else
			{
				Assert.Equal(value, otpInput.Length);
			}
		}

		[Theory]
		[InlineData("X")]
		[InlineData("/")]
		[InlineData("@")]
		[InlineData("6")]
		public void Separator(string expectedValue)
		{
			var otpInput = new SfOtpInput();
			otpInput.Separator = expectedValue;
			Assert.Equal(expectedValue, otpInput.Separator);
		}

		[Theory]
		[InlineData(OtpInputStyle.Outlined)]
		[InlineData(OtpInputStyle.Underlined)]
		[InlineData(OtpInputStyle.Filled)]
		public void StylingMode(OtpInputStyle mode)
		{
			var otpInput = new SfOtpInput();
			otpInput.StylingMode = mode;
			Assert.Equal(mode, otpInput.StylingMode);
		}

		[Theory]
		[InlineData(OtpInputType.Number)]
		[InlineData(OtpInputType.Text)]
		[InlineData(OtpInputType.Password)]
		public void InputType(OtpInputType inputType)
		{
			var otpInput = new SfOtpInput();
			otpInput.Type = inputType;
			Assert.Equal(inputType, otpInput.Type);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsEnabled(bool expectedValue)
		{
			var otpInput = new SfOtpInput();
			otpInput.IsEnabled = expectedValue;
			var actual = otpInput.IsEnabled;
			Assert.Equal(expectedValue, actual);
		}

		[Theory]
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void AutoFocus(bool input, bool expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.AutoFocus = input;
			var actual = otpInput.AutoFocus;
			Assert.Equal(expected, actual);
		}


		[Theory]
		[InlineData(OtpInputState.Success)]
		[InlineData(OtpInputState.Warning)]
		[InlineData(OtpInputState.Error)]
		[InlineData(OtpInputState.Default)]
		public void InputState(OtpInputState otpInputState)
		{
			var otpInput = new SfOtpInput();
			otpInput.InputState = otpInputState;
			Assert.Equal(otpInputState, otpInput.InputState);
		}

		[Theory]
		[InlineData('*')]
		[InlineData('#')]
		[InlineData('X')]
		public void MaskCharacter(char expectedValue)
		{
			var otpInput = new SfOtpInput();
			otpInput.MaskCharacter = expectedValue;
			Assert.Equal(expectedValue, otpInput.MaskCharacter);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void Stroke(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.Stroke = input;
			var actual = otpInput.Stroke;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void InputBackground(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.InputBackground = input;
			var actual = otpInput.InputBackground;
			Assert.Equal(expected, actual);
		}



		[Theory]
		[InlineData(40)]
		[InlineData(60)]
		[InlineData(100)]
		public void BoxWidth_UpdatesEntryDimensions(double newWidth)
		{
			var otpInput = new SfOtpInput();
			otpInput.BoxWidth = newWidth;
			var _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			if (_otpEntries != null)
			{
				foreach (var entry in _otpEntries)
				{
					Assert.Equal(newWidth, entry.MinimumWidthRequest);
					Assert.Equal(newWidth, entry.WidthRequest);
				}
			}
		}



		[Theory]
		[InlineData(40)]
		[InlineData(55)]
		[InlineData(80)]
		public void BoxHeight_UpdatesEntryDimensions(double newHeight)
		{
			var otpInput = new SfOtpInput();
			otpInput.BoxHeight = newHeight;
			var _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			if (_otpEntries != null)
			{
				foreach (var entry in _otpEntries)
				{
					Assert.Equal(newHeight, entry.MinimumHeightRequest);
					Assert.Equal(newHeight, entry.HeightRequest);
				}
			}
		}

		#endregion

		#region Internal Properties

		public static IEnumerable<object[]> ColorData =>
		new List<object[]>
		{
			new object[] { Color.FromArgb("#CAC4D0"), Color.FromArgb("#CAC4D0") },
			new object[] { Colors.Red,  Colors.Red },
			new object[] { Colors.Blue, Colors.Blue }
		};

		[Theory]
		[MemberData(nameof(ColorData))]
		public void SeparatorColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.SeparatorColor = input;
			var actual = otpInput.SeparatorColor;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void HoveredStrokeColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.HoveredStroke = input;
			var actual = otpInput.HoveredStroke;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void FocusedStrokeColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.FocusedStroke = input;
			var actual = otpInput.FocusedStroke;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void DisabledStrokeColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.DisabledStroke = input;
			var actual = otpInput.DisabledStroke;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void SuccessStrokeColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.SuccessStroke = input;
			var actual = otpInput.SuccessStroke;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void WarningStrokeColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.WarningStroke = input;
			var actual = otpInput.WarningStroke;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void ErrorStrokeColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.ErrorStroke = input;
			var actual = otpInput.ErrorStroke;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void FilledDisableBackgroundColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.FilledDisableBackground = input;
			var actual = otpInput.FilledDisableBackground;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[MemberData(nameof(ColorData))]
		public void FilledHoverBackgroundColor(Color input, Color expected)
		{
			var otpInput = new SfOtpInput();
			otpInput.FilledHoverBackground = input;
			var actual = otpInput.FilledHoverBackground;
			Assert.Equal(expected, actual);
		}

		#endregion

		#region Events

		[Fact]
		public void RaiseValueChangedEvent_ShouldTrigger_ValueChangedEvent()
		{
			var otpinput = new SfOtpInput();
			string? oldValue = "1234";
			string? newValue = "5678";
			bool eventTriggered = false;
			otpinput.ValueChanged += (sender, e) =>
			{
				eventTriggered = true;
			};

			InvokeStaticPrivateMethod(otpinput, "RaiseValueChangedEvent", new object[] { otpinput, oldValue, newValue });
			Assert.True(eventTriggered);
		}

		#endregion

		#region Public Methods
		public SfOtpInput OTPEntryHelper()
		{
			var otpInput = new SfOtpInput();
			var otpEntries = new OTPEntry[4];
			for (int i = 0; i < 4; i++)
			{
				otpEntries[i] = new OTPEntry();
			}

			SetPrivateField(otpInput, "_otpEntries", otpEntries);

			return otpInput;
		}

		[Theory]
		[InlineData("Left",1,0)]
		[InlineData("Left",0,0)]
		[InlineData("Right", 0, 1)]
		[InlineData("Right",3,3)]
		public void HandleKeyPress_LeftRightKey(string key,int focusedIndex,int expectedValue)
		{
			var otpInput= OTPEntryHelper();
			SetPrivateField(otpInput, "_focusedIndex", focusedIndex);
			InvokePrivateMethod(otpInput, "HandleKeyPress", key);
			int? resultFocusedIndex = (int?)GetPrivateField(otpInput, "_focusedIndex");
			Assert.Equal(expectedValue, resultFocusedIndex);
		}

		[Theory]
		[InlineData("Tab",0,1,false)]
		[InlineData("Tab",2,1,true)]
		public void HandleKeyPress_TabKey(string key,  int focusedIndex, int expectedValue, bool isShiftOn)
		{
			var otpInput = OTPEntryHelper();
			SetPrivateField(otpInput, "_focusedIndex", focusedIndex);
			SetPrivateField(otpInput, "_isShiftOn", isShiftOn);
			InvokePrivateMethod(otpInput, "HandleKeyPress", key);
			int? resultFocusedIndex = (int?)GetPrivateField(otpInput, "_focusedIndex");
			Assert.Equal(expectedValue, resultFocusedIndex);
		}

		[Theory]
		[InlineData("Back", "1","")]
		[InlineData("Back", "","")]
		public void HandleKeyPress_BackKey(string key, string value,string expectedValue)
		{
			var otpInput = OTPEntryHelper();
			otpInput.Value = value;
			InvokePrivateMethod(otpInput, "HandleKeyPress", key);
			OTPEntry[]? otpEntry = (OTPEntry[]?) GetPrivateField(otpInput, "_otpEntries");
			if(otpEntry != null)
			{
				Assert.Equal(expectedValue, otpEntry[0].Text);
			}
		}

		[Theory]
		[InlineData("1", "1")]
		[InlineData("A", "")]
		[InlineData("@", "")]
		public void HandleKeyPress(string value, string expected)
		{
			var otpInput = OTPEntryHelper();
			otpInput.Value = value;
			InvokePrivateMethod(otpInput, "HandleKeyPress", value);
			OTPEntry[]? otpEntry = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			if (otpEntry != null)
			{
				Assert.Equal(expected,otpEntry[0].Text);
			}
		}

		#endregion

		#region Private Methods

		[Fact]
		public void InitializeFields()
		{
			var otpInput = new SfOtpInput();
			InvokePrivateMethod(otpInput, "InitializeFields");
			OTPEntry[]? otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			SfLabel[]? otpLabel = (SfLabel[]?)GetPrivateField(otpInput, "_separators");
			RectF[]? entryBounds = (RectF[]?)GetPrivateField(otpInput, "_entryBounds");
			var length = otpInput.Length;
			Assert.Equal(4, (int)length);
			Assert.Equal(4, otpEntries?.Length);
			Assert.Equal(3, otpLabel?.Length);
			Assert.Equal(4, entryBounds?.Length);
			Assert.NotNull(otpEntries);
			Assert.NotNull(entryBounds);
			Assert.NotNull(otpLabel);
			Assert.NotNull(otpInput.Children);
		}

		[Fact]
		public void InitializeSeparator()
		{
			var otpInput = new SfOtpInput();
			var label = InvokePrivateMethod(otpInput, "InitializeSeparator");
			Assert.NotNull(label);
			Assert.True(label is  SfLabel);
		}

		[Fact]
		public void InitializeEntry()
		{
			var otpInput = new SfOtpInput();
			OTPEntry? otpEntry = (OTPEntry?) InvokePrivateMethod(otpInput, "InitializeEntry");
			Assert.NotNull(otpEntry);
			Assert.Equal(40, otpEntry.MinimumWidthRequest);
			Assert.Equal(40, otpEntry.MinimumHeightRequest);
			Assert.Equal(16, otpEntry.FontSize);
			Assert.Equal(Keyboard.Numeric,otpEntry.Keyboard);
			Assert.Equal(otpInput,otpEntry.BindingContext);

			Color placeholderColorBinding =(Color) otpEntry.GetValue(OTPEntry.PlaceholderColorProperty);
			Assert.NotNull(placeholderColorBinding);
			Assert.Equal(placeholderColorBinding, otpInput.PlaceholderColor);

			Color textColorBinding = (Color)otpEntry.GetValue(OTPEntry.TextColorProperty);
			Assert.NotNull(textColorBinding);
			Assert.Equal(textColorBinding, otpInput.TextColor);
		}

		[Fact]
		public void FocusAsync()
		{
			var otpInput = new SfOtpInput();
			OTPEntry[]? otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			SetPrivateField(otpInput, "_otpEntries", otpEntries);
			var sender = otpEntries?[2];
			var focusEventArgs = new FocusEventArgs(otpInput,true);
			InvokePrivateMethod(otpInput, "FocusAsync", sender, focusEventArgs);
			var focusedIndex = (int?)GetPrivateField(otpInput, "_focusedIndex");
			Assert.Equal(2, focusedIndex);
		}


		[Fact]
		public void FocusOutAsync()
		{
			var otpInput = new SfOtpInput();
			otpInput.Unfocus();
			var focusEvent = new FocusEventArgs(otpInput, false);
			InvokePrivateMethod(otpInput, "FocusOutAsync", otpInput, focusEvent);
			Assert.False(focusEvent.IsFocused);
		}

		[Theory]
		[InlineData(0, true, 0)]
		[InlineData(1, false, 1)]
		public void FocusEntry(int index, bool setCursorToStart, int expectedCursorPosition)
		{
			var otpInput = new SfOtpInput();
			OTPEntry[]? otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			SetPrivateField(otpInput, "_otpEntries", otpEntries);
			SetPrivateField(otpInput, "_focusedIndex", 0);
			otpEntries?[0].Focus();
			InvokePrivateMethod(otpInput, "FocusEntry", index, setCursorToStart);
			int? resultFocusedIndex = (int?)GetPrivateField(otpInput, "_focusedIndex");
			Assert.Equal(index, resultFocusedIndex);
			int? cursorPosition = otpEntries?[index].CursorPosition;
			Assert.Equal(expectedCursorPosition, cursorPosition);
		}

		[Theory]
		[InlineData(3, true, 3)]
		[InlineData(0, false, 0)]
		[InlineData(3, false, 3)]
		public void HandleFocus(int index, bool hasText, int expectedFocusIndex)
		{
			var otpInput = new SfOtpInput ();
			OTPEntry[]? otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			SetPrivateField(otpInput,"_otpEntries", otpEntries);
			var focusedIndex = GetPrivateField(otpInput, "_focusedIndex");
			SetPrivateField(otpInput, "_focusedIndex", index);
			InvokePrivateMethod(otpInput,"HandleFocus", index, hasText);
			focusedIndex = GetPrivateField(otpInput, "_focusedIndex");
			Assert.Equal(expectedFocusIndex, focusedIndex);
		}

		[Theory]
		[InlineData(0, 1)]
		[InlineData(1, 1)]
		[InlineData(2,1)]
		[InlineData(3, 1)]
		public void GetStrokeThickness(int index, float expectedStrokeThickness)
		{
			var otpInput = new SfOtpInput();
			OTPEntry[]? otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			SetPrivateField(otpInput,"_otpEntries", otpEntries);
			float? strokeThickness =(float?) InvokePrivateMethod(otpInput, "GetStrokeThickness", index);
			Assert.Equal(expectedStrokeThickness, strokeThickness);
		}


		[Theory]
		[InlineData(0, 30f, 40f, 5f, 3f, false, 300f)]
		[InlineData(1, 35f, 45f, 10f, 5f, false, 300f)]
		public void UpdateDrawingParameters(int index, float entryWidth, float entryHeight, float spacing, float extraSpacing, bool isRTL, float controlWidth)
		{
			var otpInput = new SfOtpInput();
			SetPrivateField(otpInput, "_entryWidth", entryWidth);
			SetPrivateField(otpInput, "_entryHeight", entryHeight);
			SetPrivateField(otpInput, "_spacing", spacing);
			SetPrivateField(otpInput, "_extraSpacing", extraSpacing);
			otpInput.WidthRequest = controlWidth;

			var flowDirection = isRTL ? EffectiveFlowDirection.RightToLeft : 0;
			SetPrivateField(otpInput, "_effectiveFlowDirection", flowDirection);
			InvokePrivateMethod(otpInput, "UpdateDrawingParameters", index);

			var startPointObj = GetPrivateField(otpInput, "_startPoint");
			var startPoint = startPointObj != null ? (PointF)startPointObj : new PointF(0, 0);

			var endPointObj = GetPrivateField(otpInput, "_endPoint");
			var endPoint = endPointObj != null ? (PointF)endPointObj : new PointF(0, 0);

			var outlineRectFObj = GetPrivateField(otpInput, "_outlineRectF");
			var outlineRectF = outlineRectFObj != null ? (RectF)outlineRectFObj : new RectF(0, 0, 10, 10);

			// Expected calculations
			float baseXPos = index * (entryWidth + spacing) + extraSpacing + 0;
			float xPos = isRTL ? (controlWidth - entryWidth - baseXPos) : baseXPos;
			float yPadding = 0;

			// Assert start point
			Assert.Equal(xPos, startPoint.X);
			Assert.Equal(entryHeight + extraSpacing + yPadding, startPoint.Y);

			// Assert end point
			Assert.Equal(xPos + entryWidth, endPoint.X);
			Assert.Equal(entryHeight + extraSpacing + yPadding, endPoint.Y);

			// Assert outline rectangle
			Assert.Equal(xPos, outlineRectF.X);
			Assert.Equal(extraSpacing + yPadding, outlineRectF.Y);
			Assert.Equal(entryWidth, outlineRectF.Width);
			Assert.Equal(entryHeight, outlineRectF.Height);
		}


		[Fact]
		public void UpdateValue()
		{
			var otpInput = new SfOtpInput();
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");

			string invalidInput = "12A4";
			InvokePrivateMethod(otpInput, "UpdateValue", otpInput, invalidInput);
			Assert.Equal("1", _otpEntries?[0].Text);
			Assert.Equal("2", _otpEntries?[1].Text);
			Assert.Equal("", _otpEntries?[2].Text);
			Assert.Equal("4", _otpEntries?[3].Text);

			otpInput.Type = OtpInputType.Password;
			string passwordInput = "1234";
			InvokePrivateMethod(otpInput, "UpdateValue", otpInput, passwordInput);
			if (_otpEntries != null)
			{
				foreach (var entry in _otpEntries)
				{
					Assert.Equal(otpInput.MaskCharacter.ToString(), entry.Text);
				}
			}
		}

		[Theory]
		[InlineData(4,5)]
		[InlineData(4,3)]
		public void UpdateEntriesLength(int initialLength, int newLength)
		{
			var otpInput = new SfOtpInput();
			var layout = otpInput.Children[0] as AbsoluteLayout;
			InvokePrivateMethod(otpInput, "UpdateEntriesLength", initialLength, newLength);
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			SetPrivateField(otpInput, "_otpEntries", _otpEntries);
			Assert.Equal(newLength, _otpEntries?.Length);
		}

		[Theory]
		[InlineData(4,3)]
		[InlineData(4,0)]
		public void RemoveEntry(int initialLength,int newLength)
		{
			var otpInput = new SfOtpInput();
			var layout = otpInput.Children[0] as AbsoluteLayout;
			if(layout == null)
			{
				return;
			}
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");

			InvokePrivateMethod(otpInput, "RemoveEntry", initialLength, newLength, layout);
			if (_otpEntries != null)
			{
				foreach (var entry in _otpEntries.Take(newLength))
				{
					Assert.Contains(entry, layout.Children);
				}

				foreach (var entry in _otpEntries.Skip(newLength))
				{
					Assert.DoesNotContain(entry, layout.Children);
				}
			}

		}

		[Theory]
		[InlineData(4,5)]
		[InlineData(4,6)]
		public void AddEntry(int initialLength,int newLength)
		{
			var otpInput = new SfOtpInput();
			var layout = otpInput.Children.FirstOrDefault() as AbsoluteLayout;
			if (layout == null)
			{
				return;
			}
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			SetPrivateField(otpInput, "_otpEntries", _otpEntries);
			InvokePrivateMethod(otpInput, "AddEntry", initialLength, newLength, layout);
			OTPEntry[]? updatedOtpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			if (updatedOtpEntries != null)
			{
				foreach (var entry in updatedOtpEntries)
				{
					Assert.Contains(entry, layout.Children);
				}
			}
			int expectedSeparatorCount = newLength - 1;
			int actualSeparatorCount = layout.Children.OfType<SfLabel>().Count();
			Assert.Equal(expectedSeparatorCount, actualSeparatorCount);
			int expectedTotalChildren = newLength + expectedSeparatorCount;
			Assert.Equal(expectedTotalChildren, layout.Children.Count);
		}


		[Theory]
		[InlineData(0, 50f, 30f, 5f, 5f)]
		[InlineData(0, 60f, 40f, 10f, 10f)]
		public void SetInputFieldPosition(int index, float entryWidth, float entryHeight, float spacing, float extraSpacing)
		{

			var otpInput = new SfOtpInput();

			SetPrivateField(otpInput, "_spacing", spacing);
			SetPrivateField(otpInput, "_extraSpacing", extraSpacing);
			SetPrivateField(otpInput, "_entryBounds", new RectF[index]);
			SetPrivateField(otpInput, "_entryWidth", entryWidth);
			SetPrivateField(otpInput, "_entryHeight", entryHeight);

			var otpEntry = new OTPEntry();

			InvokePrivateMethod(otpInput, "SetInputFieldPosition", index, otpEntry);

			RectF[]? entryBounds = (RectF[]?)GetPrivateField(otpInput, "_entryBounds");
			Assert.Equal(index + 1, entryBounds?.Length);

			var layoutBounds = AbsoluteLayout.GetLayoutBounds(otpEntry);

			float expectedX =  (entryWidth + spacing) * index + extraSpacing;
			float expectedY =  extraSpacing;

			Assert.Equal(expectedX, layoutBounds.X);
			Assert.Equal(expectedY, layoutBounds.Y);
			Assert.Equal(entryWidth, layoutBounds.Width);
			Assert.Equal(entryHeight, layoutBounds.Height);
		}

		[Theory]
		[InlineData(0, 50f, 30f, 5f, 5f)]
		[InlineData(1, 60f, 40f, 10f, 10f)]
		public void SetSeparatorPosition(int index, float entryWidth, float entryHeight, float spacing, float separatorWidth)
		{
			var otpInput = new SfOtpInput();
			SetPrivateField(otpInput, "_spacing", spacing);
			SetPrivateField(otpInput, "_separators", new SfLabel[index]);
			SetPrivateField(otpInput, "_entryWidth", entryWidth);
			SetPrivateField(otpInput, "_entryHeight", entryHeight);
			SetPrivateField(otpInput, "_separatorWidth", separatorWidth);
			SetPrivateField(otpInput, "_separatorHeight", entryHeight);

			var separatorLabel = new SfLabel();

			InvokePrivateMethod(otpInput, "SetSeparatorPosition", index, separatorLabel);
			SfLabel[]? separators = (SfLabel[]?)GetPrivateField(otpInput, "_separators");
			if (separators is not null)
			{
				Assert.Contains(separatorLabel, separators);
			}
			var layoutBounds = AbsoluteLayout.GetLayoutBounds(separatorLabel);

			float expectedEntryX = (entryWidth + spacing) * index;
			float expectedSeparatorX = expectedEntryX + entryWidth + spacing / 2;

			Assert.Equal(expectedSeparatorX, layoutBounds.X);
			Assert.Equal(0, layoutBounds.Y);
			Assert.Equal(separatorWidth, layoutBounds.Width);
			Assert.Equal(entryHeight, layoutBounds.Height);
		}

		[Theory]
		[InlineData("X", "XXXX")]
		[InlineData("ABCD", "ABCD")]
		[InlineData("AB", "AB\0\0")]
		[InlineData("", "")]
		public void GetPlaceHolder(string placeholder, string expectedResult)
		{
			var otpInput = new SfOtpInput();
			otpInput.Placeholder = placeholder;
			string? result = (string?)InvokePrivateMethod(otpInput, "GetPlaceHolder");
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void UpdatePlaceholderText()
		{
			var otpInput = new SfOtpInput();
			otpInput.Placeholder = "1";
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			InvokePrivateMethod(otpInput, "UpdatePlaceholderText");
			if (_otpEntries != null)
			{
				foreach (var entry in _otpEntries)
				{
					Assert.Equal("1", entry.Placeholder);
				}
			}
		}

		[Theory]
		[InlineData("1","1")]
		[InlineData("2","2")]
		public void UpdateEntryValue(string acutalinput,string expectedoutput)
		{
			var otpInput = new SfOtpInput();
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			InvokePrivateMethod(otpInput, "UpdateEntryValue", acutalinput);
			Assert.Equal(expectedoutput, _otpEntries?[0].Text);
		}

		[Theory]
		[InlineData(OtpInputType.Number,"1234","1234")]
		[InlineData(OtpInputType.Text,"abcd","abcd")]
		[InlineData(OtpInputType.Password,"a", "●")]
		public void UpdateTypeProperty(OtpInputType type,string value,string expectedvalue)
		{
			var otpInput = new SfOtpInput();
			otpInput.Type = type;
			otpInput.Value = value;
			InvokePrivateMethod(otpInput, "UpdateTypeProperty");
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			if (otpInput.Type is not OtpInputType.Password)
			{
				Assert.Equal(expectedvalue, otpInput.Value);
			}
			else
			{
				if (_otpEntries != null)
				{
					foreach (var entry in _otpEntries)
					{
						if (entry.Text is not "")
						{
							Assert.Equal(expectedvalue, entry.Text);
						}
					}
				}
			}
		}

		[Theory]
		[InlineData('*')]
		[InlineData('&')]
		public void UpdateMaskCharacter(char maskedcharacter)
		{

			var otpInput = new SfOtpInput();
			otpInput.Type = OtpInputType.Password;
			otpInput.Value = "1234";
			otpInput.MaskCharacter = maskedcharacter;
			InvokePrivateMethod(otpInput,"UpdateMaskCharacter");
			OTPEntry[]? _otpEntries = (OTPEntry[]?)GetPrivateField(otpInput, "_otpEntries");
			if (_otpEntries != null)
			{
				foreach (var entry in _otpEntries)
				{
					Assert.Equal(maskedcharacter.ToString(), entry.Text);
				}
			}
		}

		#endregion
	}
}