using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.NumericEntry;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using System.Globalization;
using Syncfusion.Maui.Toolkit.EntryView;
using Syncfusion.Maui.Toolkit.Internals;
using System.Linq;


namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfNumericEntryUnitTests : BaseUnitTest
	{

		#region Constructor

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var numericEntry = new SfNumericEntry();
			Assert.Empty(numericEntry.Placeholder);
			Assert.Null(numericEntry.PlaceholderColor);
			Assert.Equal(Colors.Black, numericEntry.ClearButtonColor);
			Assert.Equal(FontAttributes.None, numericEntry.FontAttributes);
			Assert.Null(numericEntry.FontFamily);
			Assert.Equal(14d, numericEntry.FontSize);
			Assert.False(numericEntry.FontAutoScalingEnabled);
			Assert.Equal(Colors.Black, numericEntry.TextColor);
			Assert.Equal(0, numericEntry.CursorPosition);
			Assert.Equal(0, numericEntry.SelectionLength);
			Assert.Equal(ValueChangeMode.OnLostFocus, numericEntry.ValueChangeMode);
			Assert.Null(numericEntry.ClearButtonPath);
			Assert.Null(numericEntry.ReturnCommand);
			Assert.Null(numericEntry.ReturnCommandParameter);
			Assert.Null(numericEntry.Value);
			Assert.Equal(double.MinValue, numericEntry.Minimum);
			Assert.Equal(double.MaxValue, numericEntry.Maximum);
			Assert.Equal(TextAlignment.Start, numericEntry.HorizontalTextAlignment);
			Assert.Equal(TextAlignment.Center, numericEntry.VerticalTextAlignment);
			Assert.Equal(ReturnType.Default, numericEntry.ReturnType);
			Assert.Equal(2, numericEntry.MaximumNumberDecimalDigits);
			Assert.Null(numericEntry.CustomFormat);
			Assert.True(numericEntry.AllowNull);
			Assert.True(numericEntry.IsEditable);
			Assert.True(numericEntry.ShowClearButton);
			Assert.True(numericEntry.ShowBorder);
			Assert.Equal(Visibility.Visible, numericEntry.EntryVisibility);
			Assert.Equal(PercentDisplayMode.Compute, numericEntry.PercentDisplayMode);
			Assert.Equal(CultureInfo.CurrentCulture, numericEntry.Culture);
		}

		#endregion

		#region Property

		[Theory]
		[InlineData("Enter a value")]
		[InlineData("12345")]
		[InlineData("Special_Characters!@#")]
		public void Placeholder_ShouldSetAndGetCorrectly(string expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.Placeholder = expectedValue;

			Assert.Equal(expectedValue, numericEntry.Placeholder);
		}

		[Theory]
		[InlineData("#FF0000")] 
		[InlineData("#00FF00")] 
		[InlineData("#0000FF")] 
		public void PlaceholderColor_ShouldSetAndGetCorrectly(string color)
		{
			var expectedColor = Color.FromArgb(color);
			var numericEntry = new SfNumericEntry();
			numericEntry.PlaceholderColor = expectedColor;

			Assert.Equal(expectedColor, numericEntry.PlaceholderColor);
		}

		[Theory]
		[InlineData(12)]
		[InlineData(18.3)]
		[InlineData(0)]
		[InlineData(double.MaxValue)]
		[InlineData(double.MinValue)]
		public void FontSize_ShouldSetAndGetCorrectly(double expectedSize)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.FontSize = expectedSize;

			Assert.Equal(expectedSize, numericEntry.FontSize);
		}


		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		[InlineData(-1)]     
		[InlineData(int.MaxValue)] 
		[InlineData(int.MinValue)]
		public void SelectionLength_ShouldSetAndGetCorrectly(int expectedLength)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.Value = 8787898;
			numericEntry.SelectionLength = expectedLength;

			Assert.Equal(expectedLength, numericEntry.SelectionLength);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(123.45)]
		[InlineData(-987.65)] 
		[InlineData(double.MaxValue)]  
		[InlineData(double.MinValue)] 
		[InlineData(double.NaN)]    
		public void Value_ShouldSetAndGetCorrectly(double expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.Value = expectedValue;

			Assert.Equal(expectedValue, numericEntry.Value);
		}

		[Theory]
		[InlineData(double.MinValue, double.MaxValue)]
		[InlineData(0, 100)]          
		[InlineData(-100, 100)]    
		[InlineData(-500, -100)]        
		[InlineData(0, 0)]      
		[InlineData(1.5, 99.9)]     
		[InlineData(double.MinValue, 0)]   
		[InlineData(0, double.MaxValue)]       
		[InlineData(123.45, 678.90)]
		public void MinimumAndMaximum_ShouldSetAndGetCorrectly(double minimum, double maximum)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.Minimum = minimum;
			numericEntry.Maximum = maximum;

			Assert.Equal(minimum, numericEntry.Minimum);
			Assert.Equal(maximum, numericEntry.Maximum);
		}

		[Theory]
		[InlineData(Visibility.Visible)]
		[InlineData(Visibility.Collapsed)]
		public void EntryVisibility_ShouldSetAndGetCorrectly(Visibility expectedVisibility)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.EntryVisibility = expectedVisibility;

			Assert.Equal(expectedVisibility, numericEntry.EntryVisibility);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsEditable_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.IsEditable = expectedValue;

			Assert.Equal(expectedValue, numericEntry.IsEditable);
		}

		[Theory]
		[InlineData(FontAttributes.None)]                     
		[InlineData(FontAttributes.Bold)]                      
		[InlineData(FontAttributes.Italic)]
		public void FontAttributes_ShouldSetAndGetCorrectly(FontAttributes expectedAttributes)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.FontAttributes = expectedAttributes;

			Assert.Equal(expectedAttributes, numericEntry.FontAttributes);
		}

		[Theory]
		[InlineData("Arial")] 
		[InlineData("Times New Roman")] 
		[InlineData("Courier New")]   
		public void FontFamily_ShouldSetAndGetCorrectly(string expectedFontFamily)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.FontFamily = expectedFontFamily;

			Assert.Equal(expectedFontFamily, numericEntry.FontFamily);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)] 
		public void FontAutoScalingEnabled_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.FontAutoScalingEnabled = expectedValue;

			Assert.Equal(expectedValue, numericEntry.FontAutoScalingEnabled);
		}

		[Theory]
		[InlineData("#FF0000")] 
		[InlineData("#00FF00")]
		[InlineData("#0000FF")]
		public void TextColor_ShouldSetAndGetCorrectly(string color)
		{
			var expectedColor = Color.FromArgb(color);
			var numericEntry = new SfNumericEntry();
			numericEntry.TextColor = expectedColor;

			Assert.Equal(expectedColor, numericEntry.TextColor);
		}

		[Theory]
		[InlineData(ValueChangeMode.OnLostFocus)]
		[InlineData(ValueChangeMode.OnKeyFocus)]
		public void ValueChangeMode_ShouldSetAndGetCorrectly(ValueChangeMode expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.ValueChangeMode = expectedValue;

			Assert.Equal(expectedValue, numericEntry.ValueChangeMode);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("TestCommand")] 
		public void ReturnCommand_ShouldSetAndGetCorrectly(string? commandName) 
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.ReturnCommand = commandName == null ? 
				new Command(() => { Console.WriteLine("Default Command Executed"); }) : 
				new Command(() => { Console.WriteLine($"Command {commandName} Executed"); });

			Assert.NotNull(numericEntry.ReturnCommand);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("SomeParameter")]
		[InlineData(123)]
		public void ReturnCommandParameter_ShouldSetAndGetCorrectly(object? commandParameter)
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.ReturnCommandParameter = commandParameter ?? "DefaultCommandParameter";

			Assert.Equal(commandParameter ?? "DefaultCommandParameter", numericEntry.ReturnCommandParameter);
		}

		[Theory]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		public void HorizontalTextAlignment_ShouldSetAndGetCorrectly(TextAlignment expectedAlignment)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.HorizontalTextAlignment = expectedAlignment;

			Assert.Equal(expectedAlignment, numericEntry.HorizontalTextAlignment);
		}

		[Theory]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		public void VerticalTextAlignment_ShouldSetAndGetCorrectly(TextAlignment expectedAlignment)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.VerticalTextAlignment = expectedAlignment;

			Assert.Equal(expectedAlignment, numericEntry.VerticalTextAlignment);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(int.MaxValue)]
		[InlineData(-1)]
		public void MaximumNumberDecimalDigits_ShouldSetAndGetCorrectly(int expectedDecimalDigits)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.MaximumNumberDecimalDigits = expectedDecimalDigits;

			Assert.Equal(expectedDecimalDigits, numericEntry.MaximumNumberDecimalDigits);
		}

		[Theory]
		[InlineData(null)]           
		[InlineData("")]            
		[InlineData("#.# dollars")]   
		[InlineData("#,0.00##")]     
		[InlineData("#,#.#")]        
		[InlineData("C")]             
		[InlineData("#.#%")]       
		public void CustomFormat_ShouldSetAndGetCorrectly(string? expectedCustomFormat)
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.CustomFormat = expectedCustomFormat ?? string.Empty;

			Assert.Equal(expectedCustomFormat ?? string.Empty, numericEntry.CustomFormat);
		}


		[Theory]
		[InlineData(true)]  
		[InlineData(false)]  
		public void ShowClearButton_ShouldSetAndGetCorrectly(bool expectedShowClearButton)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.ShowClearButton = expectedShowClearButton;

			Assert.Equal(expectedShowClearButton, numericEntry.ShowClearButton);
		}

		[Theory]
		[InlineData(true)] 
		[InlineData(false)] 
		public void ShowBorder_ShouldSetAndGetCorrectly(bool expectedShowBorder)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.ShowBorder = expectedShowBorder;

			Assert.Equal(expectedShowBorder, numericEntry.ShowBorder);
		}

		[Theory]
		[InlineData(true)] 
		[InlineData(false)] 
		public void IsEnabled_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.IsEnabled = expectedValue;

			Assert.Equal(expectedValue, numericEntry.IsEnabled);
		}

		[Theory]
		[InlineData("#FF0000", typeof(SolidColorBrush))] 
		[InlineData("#00FF00", typeof(SolidColorBrush))] 
		[InlineData("#0000FF", typeof(SolidColorBrush))]                       
		public void StrokeProperty_ShouldSetAndGetCorrectly(string? brushValue, Type? expectedType)
		{
			var numericEntry = new SfNumericEntry();

			if (brushValue != null)
			{
				numericEntry.Stroke = new SolidColorBrush(Color.FromArgb(brushValue));
			}

			var stroke = numericEntry.Stroke;
			if (expectedType != null)
			{
				Assert.NotNull(stroke);
				Assert.IsType(expectedType, stroke);
			}
		}

		[Theory]
		[InlineData("#FF0000")] 
		[InlineData("#00FF00")] 
		[InlineData("#0000FF")] 
		public void ClearButtonColorProperty_ShouldSetAndGetCorrectly(string colorValue)
		{
			var numericEntry = new SfNumericEntry();
			Color expectedColor = Color.FromArgb(colorValue);
			numericEntry.ClearButtonColor = expectedColor;

			Assert.Equal(expectedColor, numericEntry.ClearButtonColor);
		}

		[Theory]
		[MemberData(nameof(GetPathTestData))]
		public void ClearButtonPathProperty_ShouldSetAndGetCorrectly(Microsoft.Maui.Controls.Shapes.Path pathData)
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.ClearButtonPath = pathData;

			Assert.Equal(pathData, numericEntry.ClearButtonPath);
		}

		public static IEnumerable<object[]> GetPathTestData()
		{
			yield return new object[]
			{
				new Microsoft.Maui.Controls.Shapes.Path
				{
					Data = new PathGeometry
					{
						Figures = new PathFigureCollection
						{
							new PathFigure
							{
								StartPoint = new Point(0, 0),
								Segments = new PathSegmentCollection
								{
									new LineSegment { Point = new Point(10, 10) }
								}
							}
						}
					}
				}
			};
		}


		[Theory]
		[InlineData(PercentDisplayMode.Value)]
		[InlineData(PercentDisplayMode.Compute)]
		public void PercentDisplayModeProperty_ShouldSetAndGetCorrectly(PercentDisplayMode percentDisplayMode)
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.PercentDisplayMode = percentDisplayMode;

			Assert.Equal(percentDisplayMode, numericEntry.PercentDisplayMode);
		}

		[Fact]
		public void CultureProperty_ShouldSetAndGetCorrectly()
		{
			var numericEntry = new SfNumericEntry();

			var expectedCultureUS = new CultureInfo("en-US");
			numericEntry.Culture = expectedCultureUS;
			Assert.Equal(expectedCultureUS, numericEntry.Culture);

			var expectedCultureFR = new CultureInfo("fr-FR");
			numericEntry.Culture = expectedCultureFR;
			Assert.Equal(expectedCultureFR, numericEntry.Culture);

			var expectedCultureDE = new CultureInfo("de-DE");
			numericEntry.Culture = expectedCultureDE;
			Assert.Equal(expectedCultureDE, numericEntry.Culture);
		}

		[Theory]
		[InlineData(ReturnType.Default)]
		[InlineData(ReturnType.Done)]
		[InlineData(ReturnType.Go)]
		[InlineData(ReturnType.Next)]
		[InlineData(ReturnType.Search)]
		[InlineData(ReturnType.Send)]
		public void ReturnTypeProperty_ShouldSetAndGetCorrectly(ReturnType returnType)
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.ReturnType = returnType;

			Assert.Equal(returnType, numericEntry.ReturnType);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void AllowNullProperty_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.AllowNull = expectedValue;

			Assert.Equal(expectedValue, numericEntry.AllowNull);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsVisibleProperty_ShouldGetAndSetCorrectly(bool expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			numericEntry.IsVisible = expectedValue;

			Assert.Equal(expectedValue, numericEntry.IsVisible);
		}

		[Theory]
		[InlineData(Visibility.Visible)]
		[InlineData(Visibility.Hidden)]
		[InlineData(Visibility.Collapsed)]
		public void EntryVisibilityProperty_ShouldSetAndApplyCorrectly(Visibility expectedValue)
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.EntryVisibility = expectedValue;

			Assert.Equal(expectedValue, numericEntry.EntryVisibility);
		}

		#endregion

		#region Private Method

		[Fact]
		public void ValidateParentIsTextInputLayout_ShouldIdentify_TextInputLayout()
		{
			var numericEntry = new SfNumericEntry();
			var textInputLayout = new SfTextInputLayout{ Content = numericEntry };

			InvokePrivateMethod(numericEntry, "ValidateParentIsTextInputLayout");

			Assert.NotNull(GetPrivateField(numericEntry, "_textInputLayout"));
			Assert.True((bool?)GetNonPublicProperty(numericEntry, "IsTextInputLayout"));
		}


		[Fact]
		public void UpdateEffectsRendererBounds_ShouldReturn_EffectsRenderer()
		{
			var numericEntry = new SfNumericEntry();

			SetPrivateField(numericEntry, "_isClearButtonVisible", true);
			InvokePrivateMethod(numericEntry, "UpdateEffectsRendererBounds");


			Assert.NotNull(GetPrivateField(numericEntry, "_effectsRenderer"));
		}

		[Theory]
		[InlineData("42.5", 0)]
		[InlineData("invalid", 42.5)]
		public void ConvertToDouble_ShouldConvertValue_ToDouble(object newValue, object oldValue)
		{
			var numericEntry = new SfNumericEntry();

			var result = InvokeStaticPrivateMethod(numericEntry, "ConvertToDouble", new object[] { newValue, oldValue });

			Assert.Equal(42.5, result);
		}

		[Theory]
		[InlineData(true, "#000000")] 
		[InlineData(false, "#808080")] 
		public void UpdateVisualState_ShouldSet_CorrectTextColor(bool isEnabled, string expectedColorHex)
		{
			var numericEntry = new SfNumericEntry
			{
				IsEnabled = isEnabled
			};

			AttachVisualStates(numericEntry);

			InvokePrivateMethod(numericEntry, "UpdateVisualState");

			Assert.Equal(expectedColorHex, numericEntry.TextColor.ToHex());
		}

		private void AttachVisualStates(SfNumericEntry numericEntry)
		{
			VisualStateGroupList visualStateGroupList = new();
			var visualStateGroup = new VisualStateGroup
			{
				Name = "CommonStates"
			};

			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter
			{
				Property = SfNumericEntry.TextColorProperty,
				Value = Colors.Black
			});
			visualStateGroup.States.Add(normalState);

			var disabledState = new VisualState { Name = "Disabled" };
			disabledState.Setters.Add(new Setter
			{
				Property = SfNumericEntry.TextColorProperty,
				Value = Colors.Gray
			});
			visualStateGroup.States.Add(disabledState);

			visualStateGroupList.Add(visualStateGroup);
			VisualStateManager.SetVisualStateGroups(numericEntry, visualStateGroupList);
		}

		[Theory]
		[InlineData(PointerActions.Released)]
		[InlineData(PointerActions.Pressed)]
		public void OnTouch_ShouldReturn_TouchPoint(PointerActions pointerActions)
		{
			var numericEntry = new SfNumericEntry();
			Point expectedValue = new Point(10, 10);
			var pointerEventArgs = new Internals.PointerEventArgs(1, pointerActions, expectedValue);
			numericEntry.OnTouch(pointerEventArgs);
			var touchPoint = (Point?)GetPrivateField(numericEntry, "_touchPoint");
			Assert.Equal(expectedValue, touchPoint);
		}

		[Fact]
		public void OnTap_InvokesExpectedBehavior()
		{
			var numericEntry = new SfNumericEntry();

			var expectedPoint = new Point(10, 10);
			var expectedCount = 1;
			var tapEventArgs = new TapEventArgs(expectedPoint, expectedCount);

			numericEntry.OnTap(tapEventArgs);

			var tapCount = (int?)GetPrivateField(tapEventArgs, "_noOfTapCounts");
			var touchPoint = (Point?)GetPrivateField(tapEventArgs, "_tapPoint");
			Assert.Equal(expectedCount, tapCount);
			Assert.Equal(expectedPoint, touchPoint);

		}

		[Fact]
		public void FocusChanged_ShouldTriggerEvent()
		{
			var numericEntry = new SfNumericEntry();

			numericEntry.Focus();
			var focusEvent = new FocusEventArgs(numericEntry, true);
			InvokePrivateMethod(numericEntry, "TextBoxOnGotFocus", numericEntry, focusEvent);

			Assert.True(focusEvent.IsFocused);

			numericEntry.Unfocus();
			focusEvent = new FocusEventArgs(numericEntry, false);
			InvokePrivateMethod(numericEntry, "TextBoxOnLostFocus", numericEntry, focusEvent);

			Assert.False(focusEvent.IsFocused);
		}

		[Theory]
		[InlineData(KeyboardKey.Home, true)]
		[InlineData(KeyboardKey.PageUp, false)]
		[InlineData(KeyboardKey.Left, true)]
		[InlineData(KeyboardKey.Up, false)]
		public void KeyPressed_ShouldTriggerEvent(KeyboardKey key, bool expectedValue)
		{
			var numericEntry = new SfNumericEntry();
			var isNegative = true;
			var caretPosition = 1;

			var isHandle = (bool?)InvokePrivateMethod(numericEntry, "HandleNavigation", key, isNegative, caretPosition);

			Assert.Equal(expectedValue, isHandle);
		}

		[Theory]
		[InlineData(100, 100, 100, 100)]
		[InlineData(double.PositiveInfinity, double.PositiveInfinity, 0, 0)]
		[InlineData(-51, 100, -51, 100)]
		[InlineData(100, -89, 100, -89)]
		public void MeasureContent_ShouldReturn_SizeCorrectly(double width, double height, double expectedWidth, double expectedHeight)
		{
			var numericEntry = new SfNumericEntry();
			var size = (Size?)InvokePrivateMethod(numericEntry, "MeasureContent", width, height);
			Assert.NotNull(size);
			Assert.Equal(expectedHeight, size.Value.Height);
			Assert.Equal(expectedWidth, size.Value.Width);
		}

		#endregion
	}
}