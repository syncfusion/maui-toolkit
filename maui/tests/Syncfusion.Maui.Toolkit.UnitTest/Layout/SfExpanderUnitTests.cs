using System.Reflection;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Expander;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfExpanderUnitTests : BaseUnitTest
	{
		#region Properties

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var expander = new SfExpander();
			Assert.NotNull(expander);
			Assert.False(expander.IsExpanded);
			Assert.Null(expander.Content);
			Assert.Null(expander.Header);
			Assert.Equal(ExpanderIconPosition.End, expander.HeaderIconPosition);
			Assert.Equal(ExpanderAnimationEasing.Linear, expander.AnimationEasing);
			Assert.Equal(1d, expander.HeaderStrokeThickness);
			Assert.Equal(300d, expander.AnimationDuration);
			Assert.Equal(Color.FromArgb("#49454F"), expander.HeaderIconColor);
			Assert.Equal(Color.FromArgb("#1C1B1F"), expander.HoverIconColor);
			Assert.Equal(Color.FromArgb("#49454F"), expander.PressedIconColor);
			Assert.Equal(Color.FromArgb("#49454F"), expander.FocusedIconColor);
			Assert.Equal(Color.FromArgb("#CAC4D0"), expander.HeaderStroke);
			Assert.Equal(Color.FromArgb("#000000"), expander.FocusBorderColor);
		}

		[Theory]
		[InlineData(double.MinValue)]
		[InlineData(double.MaxValue)]
		[InlineData((double)0)]
		[InlineData(1000)]
		public void AnimationDuration_SetValue_ReturnsExpected(double expectedValue)
		{
			var expander = new SfExpander
			{
				AnimationDuration = expectedValue
			};

			Assert.Equal(expectedValue, expander.AnimationDuration);
		}

		[Fact]
		public void AnimationDurationProperty_Binding_UpdatesValue()
		{
			var expander = new SfExpander();
			var bindingContext = new { AnimationDuration = 500d };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.AnimationDurationProperty, new Binding("AnimationDuration"));
			Assert.Equal(500d, expander.AnimationDuration);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsExpanded_SetValue_ReturnsExpected(bool expectedValue)
		{
			var expander = new SfExpander
			{
				IsExpanded = expectedValue
			};

			Assert.Equal(expectedValue, expander.IsExpanded);
		}

		[Theory]
		[InlineData(ExpanderIconPosition.Start)]
		[InlineData(ExpanderIconPosition.End)]
		[InlineData(ExpanderIconPosition.None)]
		public void HeaderIconPosition_SetValue_ReturnsExpected(ExpanderIconPosition expectedValue)
		{
			var expander = new SfExpander
			{
				HeaderIconPosition = expectedValue
			};

			Assert.Equal(expectedValue, expander.HeaderIconPosition);
		}

		[Theory]
		[InlineData(ExpanderAnimationEasing.SinIn)]
		[InlineData(ExpanderAnimationEasing.SinInOut)]
		[InlineData(ExpanderAnimationEasing.SinOut)]
		[InlineData(ExpanderAnimationEasing.Linear)]
		[InlineData(ExpanderAnimationEasing.None)]
		public void AnimationEasing_SetValue_ReturnsExpected(ExpanderAnimationEasing expectedValue)
		{
			var expander = new SfExpander
			{
				AnimationEasing = expectedValue
			};

			Assert.Equal(expectedValue, expander.AnimationEasing);
		}

		[Theory]
		[InlineData("Red")]
		[InlineData("Green")]
		public void HeaderBackground_ShouldSetAndGetSystemBrush(string colorName)
		{
			var expander = new SfExpander();
			var expectedBrush = new BrushTypeConverter().ConvertFromString(colorName) as Brush;
			if (expectedBrush == null)
			{
				Assert.Fail($"Failed to convert color name '{colorName}' to a Brush.");
			}

			expander.HeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HeaderBackground);
		}

		[Theory]
		[InlineData(typeof(SolidColorBrush))]
		[InlineData(typeof(LinearGradientBrush))]
		public void HeaderBackgroundPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Brush;
			var bindingContext = new { HeaderBackgroundBrush = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.HeaderBackgroundProperty, new Binding("HeaderBackgroundBrush"));
			Assert.Equal(brush, expander.HeaderBackground);
		}
		
		[Theory]
		[InlineData(typeof(SolidColorBrush))]
		[InlineData(typeof(LinearGradientBrush))]
		public void HoverHeaderBackgroundPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Brush;
			var bindingContext = new { HoverHeaderBackgroundBrush = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.HoverHeaderBackgroundProperty, new Binding("HoverHeaderBackgroundBrush"));
			Assert.Equal(brush, expander.HoverHeaderBackground);
		}

		[Theory]
		[InlineData(typeof(SolidColorBrush))]
		[InlineData(typeof(LinearGradientBrush))]
		public void HeaderRippleBackgroundPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Brush;
			var bindingContext = new { HeaderRippleBackground = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.HeaderRippleBackgroundProperty, new Binding("HeaderRippleBackground"));
			Assert.Equal(brush, expander.HeaderRippleBackground);
		}

		[Theory]
		[InlineData(typeof(SolidColorBrush))]
		[InlineData(typeof(LinearGradientBrush))]
		public void FocusedHeaderBackgroundPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Brush;
			var bindingContext = new { FocusedHeaderBackground = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.FocusedHeaderBackgroundProperty, new Binding("FocusedHeaderBackground"));
			Assert.Equal(brush, expander.FocusedHeaderBackground);
		}

		[Theory]
		[InlineData(typeof(Color))]
		public void HeaderStrokeWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Color;
			var bindingContext = new { HeaderStroke = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.HeaderStrokeProperty, new Binding("HeaderStroke"));
			Assert.Equal(brush, expander.HeaderStroke);
		}

		[Theory]
		[InlineData(typeof(Color))]
		public void FocusBorderColorWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Color;
			var bindingContext = new { FocusBorderColor = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.FocusBorderColorProperty, new Binding("FocusBorderColor"));
			Assert.Equal(brush, expander.FocusBorderColor);
		}

		[Theory]
		[InlineData(typeof(Color))]
		public void HeaderIconColorPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Color;
			var bindingContext = new { HeaderIconColor = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.HeaderIconColorProperty, new Binding(nameof(bindingContext.HeaderIconColor)));
			Assert.Equal(brush, expander.HeaderIconColor);
		}

		[Theory]
		[InlineData(typeof(Color))]
		public void HoverIconColorPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Color;
			var bindingContext = new { HeaderIconColor = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.HoverIconColorProperty, new Binding(nameof(bindingContext.HeaderIconColor)));
			Assert.Equal(brush, expander.HoverIconColor);
		}

		[Theory]
		[InlineData(typeof(Color))]
		public void PressedIconColorPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var expander = new SfExpander();
			var brush = Activator.CreateInstance(brushType) as Color;
			var bindingContext = new { PressedIconColor = brush };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.PressedIconColorProperty, new Binding(nameof(bindingContext.PressedIconColor)));
			Assert.Equal(brush, expander.PressedIconColor);
		}

		[Fact]
		public void HeaderBackground_ShouldSetAndGetSolidColorBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new SolidColorBrush
			{
				Color = Color.FromArgb("#00000000")
			};

			expander.HeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HeaderBackground);
		}

		[Theory]
		[InlineData(255, 228, 196)]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		public void HeaderBackground_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.HeaderBackground = expectedColor;
			Assert.Equal(expectedColor, expander.HeaderBackground);
		}

		[Fact]
		public void HeaderBackground_ShouldSetAndGetLinearGradientBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new LinearGradientBrush
			{
				GradientStops =
				[
					new GradientStop(Colors.Yellow, (float)0.0),
					new GradientStop(Colors.Red, (float)1.0)
				]
			};

			expander.HeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HeaderBackground);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsExpandedPropertyWithBinding_SetAndGet_ReturnsExpectedValue(bool isExpandedValue)
		{
			var expander = new SfExpander();
			var bindingContext = new { IsExpandedValue = isExpandedValue };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.IsExpandedProperty, new Binding("IsExpandedValue"));
			Assert.Equal(isExpandedValue, expander.IsExpanded);
		}

		[Fact]
		public void HeaderIconColor_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander
			{
				HeaderIconColor = Colors.Blue
			};

			Assert.Equal(Colors.Blue, expander.HeaderIconColor);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 0, 0)]
		public void HeaderIconColor_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.HeaderIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.HeaderIconColor);
		}

		[Theory]
		[InlineData(0xFFFF0000)]
		[InlineData(0xFF00FF00)]
		public void HeaderIconColor_SetAndGetVariousColors(uint expectedColorValue)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb((byte)(expectedColorValue >> 24), (byte)(expectedColorValue >> 16), (byte)(expectedColorValue >> 8));
			expander.HeaderIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.HeaderIconColor);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("Test Label")]
		public void Content_SetValue_ReturnsExpected(string? labelText)
		{
			var expander = new SfExpander();
			Label? expectedContent = labelText == null ? null : new Label { Text = labelText };
			expander.Content = expectedContent;
			Assert.Equal(expectedContent, expander.Content);
		}

		[Theory]
		[InlineData(typeof(ContentView))]
		[InlineData(typeof(Label))]
		public void ContentPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type contentType)
		{
			var expander = new SfExpander();
			var contentView = Activator.CreateInstance(contentType) as View;
			var bindingContext = new { ContentView = contentView };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.ContentProperty, new Binding("ContentView"));
			Assert.Equal(contentView, expander.Content);
		}

		[Fact]
		public void Header_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander();
			var contentHeader = new Label { Text = "Test Label" };
			expander.Header = contentHeader;
			Assert.Equal(contentHeader, expander.Header);
		}

		[Fact]
		public void HeaderAsNull_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander()
			{
				Header = null!
			};

			Assert.Null(expander.Header);
		}

		[Theory]
		[InlineData(typeof(ContentView))]
		[InlineData(typeof(Label))]
		public void HeaderPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type headerType)
		{
			var expander = new SfExpander();
			var headerView = Activator.CreateInstance(headerType) as View;
			var bindingContext = new { HeaderView = headerView };
			expander.BindingContext = bindingContext;
			expander.SetBinding(SfExpander.HeaderProperty, new Binding("HeaderView"));
			Assert.Equal(headerView, expander.Header);
		}

		[Theory]
		[InlineData(double.MinValue)]
		[InlineData(double.MaxValue)]
		[InlineData((double)0)]
		[InlineData(50)]
		public void ContentHeightOnAnimation_SetValue_ReturnsExpected(double value)
		{
			var expander = new SfExpander()
			{
				ContentHeightOnAnimation=value
			};

			Assert.Equal(value, expander.ContentHeightOnAnimation);
		}

		[Fact]
		public void SemanticDescription_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander();
			var expectedDescription = "Test Description";
			expander.SemanticDescription = expectedDescription;
			Assert.Equal(expectedDescription, expander.SemanticDescription);
		}

		[Fact]
		public void HeaderView_SetValue_ReturnsExpected()
		{
			var header = new ExpanderHeader();
			var expander = new SfExpander()
			{
				HeaderView = header
			};

			Assert.Equal(header, expander.HeaderView);
		}
		
		[Fact]
		public void HeaderViewAsNull_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander()
			{
				HeaderView = null
			};

			Assert.Null(expander.HeaderView);
		}

		[Fact]
		public void ContentView_SetValue_ReturnsExpected()
		{
			var content = new ExpanderContent();
			var expander = new SfExpander()
			{
				ContentView=content
			};

			Assert.Equal(content, expander.ContentView);
		}
		
		[Fact]
		public void ContentViewAsNull_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander()
			{
				ContentView=null
			};

			Assert.Null(expander.ContentView);
		}

		[Theory]
		[InlineData("Red")]
		[InlineData("Green")]
		public void HoverHeaderBackground_ShouldSetAndGetSystemBrush(string colorName)
		{
			var expander = new SfExpander();
			var expectedBrush = new BrushTypeConverter().ConvertFromString(colorName) as Brush;
			if (expectedBrush == null)
			{
				Assert.Fail($"Failed to convert color name '{colorName}' to a Brush.");
			}

			expander.HoverHeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HoverHeaderBackground);
		}

		[Fact]
		public void HoverHeaderBackground_ShouldSetAndGetSolidColorBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new SolidColorBrush
			{
				Color = Color.FromArgb("#141C1B1F")
			};

			expander.HoverHeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HoverHeaderBackground);
		}

		[Theory]
		[InlineData(255, 228, 196)]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		public void HoverHeaderBackground_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.HoverHeaderBackground = expectedColor;
			Assert.Equal(expectedColor, expander.HoverHeaderBackground);
		}

		[Fact]
		public void HoverHeaderBackground_ShouldSetAndGetLinearGradientBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new LinearGradientBrush
			{
				GradientStops =
				[
					new GradientStop(Colors.Yellow, (float)0.0),
					new GradientStop(Colors.Red, (float)1.0)
				]
			};

			expander.HoverHeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HoverHeaderBackground);
		}

		[Theory]
		[InlineData("Red")]
		[InlineData("Green")]
		public void HeaderRippleBackground_ShouldSetAndGetSystemBrush(string colorName)
		{
			var expander = new SfExpander();
			var expectedBrush = new BrushTypeConverter().ConvertFromString(colorName) as Brush;
			if (expectedBrush == null)
			{
				Assert.Fail($"Failed to convert color name '{colorName}' to a Brush.");
			}

			expander.HeaderRippleBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HeaderRippleBackground);
		}

		[Fact]
		public void HeaderRippleBackground_ShouldSetAndGetSolidColorBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new SolidColorBrush
			{
				Color = Color.FromArgb("#1C1B1E")
			};

			expander.HeaderRippleBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HeaderRippleBackground);
		}

		[Theory]
		[InlineData(255, 228, 196)]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		public void HeaderRippleBackground_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.HeaderRippleBackground = expectedColor;
			Assert.Equal(expectedColor, expander.HeaderRippleBackground);
		}

		[Fact]
		public void HeaderRippleBackground_ShouldSetAndGetLinearGradientBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new LinearGradientBrush
			{
				GradientStops =
				[
					new GradientStop(Colors.Yellow, (float)0.0),
					new GradientStop(Colors.Red, (float)1.0)
				]
			};

			expander.HeaderRippleBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.HeaderRippleBackground);
		}

		[Fact]
		public void HoverIconColor_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander
			{
				HoverIconColor = Colors.Blue
			};

			Assert.Equal(Colors.Blue, expander.HoverIconColor);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 0, 0)]
		public void HoverIconColor_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.HoverIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.HoverIconColor);
		}

		[Theory]
		[InlineData(0xFFFF0000)]
		[InlineData(0xFF00FF00)]
		public void HoverIconColor_SetAndGetVariousColors(uint expectedColorValue)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb((byte)(expectedColorValue >> 24), (byte)(expectedColorValue >> 16), (byte)(expectedColorValue >> 8));
			expander.HoverIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.HoverIconColor);
		}

		[Fact]
		public void PressedIconColor_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander
			{
				PressedIconColor = Colors.Blue
			};

			Assert.Equal(Colors.Blue, expander.PressedIconColor);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 0, 0)]
		public void PressedIconColor_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.PressedIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.PressedIconColor);
		}

		[Theory]
		[InlineData(0xFFFF0000)]
		[InlineData(0xFF00FF00)]
		public void PressedIconColor_SetAndGetVariousColors(uint expectedColorValue)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb((byte)(expectedColorValue >> 24), (byte)(expectedColorValue >> 16), (byte)(expectedColorValue >> 8));
			expander.PressedIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.PressedIconColor);
		}

		[Fact]
		public void HeaderStroke_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander
			{
				HeaderStroke = Colors.Blue
			};

			Assert.Equal(Colors.Blue, expander.HeaderStroke);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 0, 0)]
		public void HeaderStroke_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.HeaderStroke = expectedColor;
			Assert.Equal(expectedColor, expander.HeaderStroke);
		}

		[Theory]
		[InlineData(0xFFFF0000)]
		[InlineData(0xFF00FF00)]
		public void HeaderStroke_SetAndGetVariousColors(uint expectedColorValue)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb((byte)(expectedColorValue >> 24), (byte)(expectedColorValue >> 16), (byte)(expectedColorValue >> 8));
			expander.HeaderStroke = expectedColor;
			Assert.Equal(expectedColor, expander.HeaderStroke);
		}

		[Theory]
		[InlineData("Red")]
		[InlineData("Green")]
		public void FocusedHeaderBackground_ShouldSetAndGetSystemBrush(string colorName)
		{
			var expander = new SfExpander();
			var expectedBrush = new BrushTypeConverter().ConvertFromString(colorName) as Brush;
			if (expectedBrush == null)
			{
				Assert.Fail($"Failed to convert color name '{colorName}' to a Brush.");
			}

			expander.FocusedHeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.FocusedHeaderBackground);
		}

		[Theory]
		[InlineData(255, 228, 196)]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		public void FocusedHeaderBackground_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.FocusedHeaderBackground = expectedColor;
			Assert.Equal(expectedColor, expander.FocusedHeaderBackground);
		}

		[Fact]
		public void FocusedHeaderBackground_ShouldSetAndGetLinearGradientBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new LinearGradientBrush
			{
				GradientStops =
				[
					new GradientStop(Colors.Yellow, (float)0.0),
					new GradientStop(Colors.Red, (float)1.0)
				]
			};

			expander.FocusedHeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.FocusedHeaderBackground);
		}

		[Fact]
		public void FocusedHeaderBackground_ShouldSetAndGetSolidColorBrush()
		{
			var expander = new SfExpander();
			var expectedBrush = new SolidColorBrush
			{
				Color= Color.FromArgb("#1F1C1B1F")
			};

			expander.FocusedHeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, expander.FocusedHeaderBackground);
		}

		[Fact]
		public void FocusedIconColor_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander()
			{
				FocusedIconColor = Colors.Blue
			};

			Assert.Equal(Colors.Blue, expander.FocusedIconColor);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 0, 0)]
		public void FocusedIconColor_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.FocusedIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.FocusedIconColor);
		}

		[Theory]
		[InlineData(0xFFFF0000)]
		[InlineData(0xFF00FF00)]
		public void FocusedIconColor_SetAndGetVariousColors(uint expectedColorValue)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb((byte)(expectedColorValue >> 24), (byte)(expectedColorValue >> 16), (byte)(expectedColorValue >> 8));
			expander.FocusedIconColor = expectedColor;
			Assert.Equal(expectedColor, expander.FocusedIconColor);
		}

		[Theory]
		[InlineData(double.MinValue)]
		[InlineData(double.MaxValue)]
		[InlineData((double)0)]
		[InlineData(3d)]
		public void HeaderStrokeThickness_SetValue_ReturnsExpected(double expectedValue)
		{
			var expander = new SfExpander()
			{
				HeaderStrokeThickness = expectedValue
			};

			Assert.Equal(expectedValue, expander.HeaderStrokeThickness);
		}

		[Fact]
		public void FocusBorderColor_SetValue_ReturnsExpected()
		{
			var expander = new SfExpander()
			{
				FocusBorderColor = Colors.Red
			};

			Assert.Equal(Colors.Red, expander.FocusBorderColor);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 0, 0)]
		public void FocusBorderColor_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb(r, g, b);
			expander.FocusBorderColor = expectedColor;
			Assert.Equal(expectedColor, expander.FocusBorderColor);
		}

		[Theory]
		[InlineData(0xFFFF0000)]
		[InlineData(0xFF00FF00)]
		public void FocusBorderColor_SetAndGetVariousColors(uint expectedColorValue)
		{
			var expander = new SfExpander();
			var expectedColor = Color.FromRgb((byte)(expectedColorValue >> 24), (byte)(expectedColorValue >> 16), (byte)(expectedColorValue >> 8));
			expander.FocusBorderColor = expectedColor;
			Assert.Equal(expectedColor, expander.FocusBorderColor);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsSelected_SetValue_ReturnsExpected(bool expectedValue)
		{
			var expander = new SfExpander()
			{
				IsSelected = expectedValue
			};

			Assert.Equal(expectedValue, expander.IsSelected);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsMouseHover_SetValue_ReturnsExpected(bool expectedValue)
		{
			var expander = new SfExpander()
			{
				HeaderView =
				[
				  new Label()
				]
			};
			
			if (expander.HeaderView != null)
			{
				expander.HeaderView.IsMouseHover = expectedValue;
				Assert.Equal(expectedValue, expander.HeaderView.IsMouseHover);
			}
		}

		[Fact]
		public void AnimationDuration_SetValue_Runtime_ShouldUpdateProperty()
		{
			var expander = new SfExpander { AnimationDuration = 200 };
			Assert.Equal(200, expander.AnimationDuration);
			expander.AnimationDuration = 500;
			Assert.Equal(500, expander.AnimationDuration);
		}

		[Fact]
		public void AnimationEasing_SetValue_Runtime_ShouldUpdateProperty()
		{
			var expander = new SfExpander { AnimationEasing = ExpanderAnimationEasing.SinIn };
			Assert.Equal(ExpanderAnimationEasing.SinIn, expander.AnimationEasing);
			expander.AnimationEasing = ExpanderAnimationEasing.Linear;
			Assert.Equal(ExpanderAnimationEasing.Linear, expander.AnimationEasing);
		}

		[Fact]
		public void HeaderIconPosition_SetValue_Runtime_ShouldUpdateProperty()
		{
			var expander = new SfExpander { HeaderIconPosition = ExpanderIconPosition.Start };
			Assert.Equal(ExpanderIconPosition.Start, expander.HeaderIconPosition);
			expander.HeaderIconPosition = ExpanderIconPosition.End;
			Assert.Equal(ExpanderIconPosition.End, expander.HeaderIconPosition);
		}

		[Fact]
		public void HeaderBackground_SetValue_Runtime_ShouldUpdateProperty()
		{
			var expander = new SfExpander { HeaderBackground = Colors.Green };
			Assert.Equal(Colors.Green, expander.HeaderBackground);
			expander.HeaderBackground = Colors.Blue;
			Assert.Equal(Colors.Blue, expander.HeaderBackground);
		}

		[Fact]
		public void HeaderIconColor_SetValue_Runtime_ShouldUpdateProperty()
		{
			var expander = new SfExpander { HeaderIconColor = Colors.Green };
			Assert.Equal(Colors.Green, expander.HeaderIconColor);
			expander.HeaderIconColor = Colors.Blue;
			Assert.Equal(Colors.Blue, expander.HeaderIconColor);
		}

		[Fact]
		public void IsExpanded_SetValue_Runtime_ShouldUpdateProperty()
		{
			var expander = new SfExpander { IsExpanded = true };
			Assert.True(expander.IsExpanded);
			expander.IsExpanded = false;
			Assert.False(expander.IsExpanded);
		}

		[Fact]
		public void ExpanderStyle_WhenApplied_SetsCorrectProperties()
		{
			// Arrange
			var animationDuration = 100;
			ExpanderAnimationEasing easing = new ExpanderAnimationEasing();
			easing = ExpanderAnimationEasing.SinInOut;
			ExpanderIconPosition expanderIconPosition = new ExpanderIconPosition();
			expanderIconPosition = ExpanderIconPosition.Start;
			Color color = new Color();
			color = Colors.Aquamarine;
			Color color2 = new Color();
			color2 = Colors.GreenYellow;
			var style = new Style(typeof(Syncfusion.Maui.Toolkit.Expander.SfExpander));
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Expander.SfExpander.AnimationDurationProperty,
				Value = animationDuration
			});
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Expander.SfExpander.HeaderIconPositionProperty,
				Value = expanderIconPosition
			});
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Expander.SfExpander.AnimationEasingProperty,
				Value = easing
			});
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Expander.SfExpander.HeaderBackgroundProperty,
				Value = color
			});
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Expander.SfExpander.IsExpandedProperty,
				Value = true
			});
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Expander.SfExpander.HeaderIconColorProperty,
				Value = color2
			});
			var resources = new ResourceDictionary();
			resources.Add("ExpanderStyle", style);
			Application.Current = new Application();
			Application.Current.Resources = resources;
			var expander = new Syncfusion.Maui.Toolkit.Expander.SfExpander();
			// Act
			expander.Style = (Style)Application.Current.Resources["ExpanderStyle"];
			// Assert
			Assert.Equal(animationDuration, expander.AnimationDuration);
			Assert.Equal(expanderIconPosition, expander.HeaderIconPosition);
			Assert.Equal(easing, expander.AnimationEasing);
			Assert.Equal(color, expander.HeaderBackground);
			Assert.Equal(color2, expander.HeaderIconColor);
			Assert.True(expander.IsExpanded);
		}

		[Fact]
		public void SfExpander_ShouldBeAddedToStackLayout()
		{
			// Arrange
			var stackLayout = new StackLayout();
			var label = new Label { Text = "Expander Content", };
			var expander = new SfExpander
			{
				Header = new Label { Text = "Tap to Expand" },
				Content = label,
				AutomationId = "myExpander"
			};

			// Act
			stackLayout.Children.Add(expander);

			// Assert
			Assert.Contains(expander, stackLayout.Children);
		}

		[Fact]
		public void SfExpander_ShouldBeAddedToAbsoluteLayout()
		{
			// Arrange
			var absoluteLayout = new AbsoluteLayout();
			var label = new Label { Text = "Expander Content", };
			var expander = new SfExpander
			{
				Header = new Label { Text = "Tap to Expand" },
				Content = label,
				AutomationId = "myExpander"
			};
			// Act
			absoluteLayout.Children.Add(expander);
			// Assert
			Assert.Contains(expander, absoluteLayout.Children);
		}

		[Fact]
		public void SfExpander_ShouldBeAddedToHorizontalStackLayout()
		{
			// Arrange
			var stackLayout = new HorizontalStackLayout();
			var label = new Label { Text = "Expander Content", };
			var expander = new SfExpander
			{
				Header = new Label { Text = "Tap to Expand" },
				Content = label,
				AutomationId = "myExpander"
			};
			// Act
			stackLayout.Children.Add(expander);
			// Assert
			Assert.Contains(expander, stackLayout.Children);
		}

		[Fact]
		public void SfExpander_ShouldBeAddedToFlexLayout()
		{
			// Arrange
			var flexLayout = new FlexLayout();
			var label = new Label { Text = "Expander Content", };
			var expander = new SfExpander
			{
				Header = new Label { Text = "Tap to Expand" },
				Content = label,
				AutomationId = "myExpander"
			};
			// Act
			flexLayout.Children.Add(expander);
			// Assert
			Assert.Contains(expander, flexLayout.Children);
		}

		[Fact]
		public void SfExpander_ShouldBeAddedToGrid()
		{
			// Arrange
			var grid = new Grid();
			var label = new Label { Text = "Expander Content", };
			var expander = new SfExpander
			{
				Header = new Label { Text = "Tap to Expand" },
				Content = label,
				AutomationId = "myExpander"
			};
			// Act
			grid.Children.Add(expander);
			// Assert
			Assert.Contains(expander, grid.Children);
		}

		#endregion

		#region Methods

		[Fact]
		public void UpdateIconView_ShouldDoNothing_WhenExpanderIsNull()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = null,
			};

			expanderHeader.UpdateIconView();
			Assert.Null(expanderHeader.IconView);
		}

		[Fact]
		public void UpdateIconView_ShouldDoNothing_WhenExpanderHeaderIconPositionIsNone()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = new SfExpander()
				{
					HeaderIconPosition = ExpanderIconPosition.None,
				},
				IconView = null
			};

			expanderHeader.UpdateIconView();
			Assert.Null(expanderHeader.IconView);
		}

		[Fact]
		public void UpdateIconView_ShouldUpdateHeaderIconView_WhenExpanderIsNotNull()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = new SfExpander
				{
					HeaderIconPosition = ExpanderIconPosition.Start,
					Header = new Label { Text = "Header" }
				},
			};

			expanderHeader.UpdateIconView();
			Assert.NotNull(expanderHeader.IconView);
		}

		[Fact]
		public void UpdateIconColor_DoesNothing_WhenIconViewIsNull()
		{
			var expander = new ExpanderHeader
			{
				IconView = null
			};

			expander.UpdateIconColor(Color.FromArgb("#FF0000"));
			Assert.Null(expander.IconView);
		}

		[Fact]
		public void UpdateIconColor_ShouldUpdateIconColor_WhenIconViewIsNotNull()
		{
			var label = new Label { TextColor = Colors.Red };
			var expander = new SfExpander()
			{
				Content = label
			};

			var iconView = new ExpandCollapseButton(expander);
			var expanderHeader = new ExpanderHeader
			{
				IconView = iconView,
				Expander = expander
			};

			expanderHeader.UpdateIconColor(Colors.Red);
			Assert.Equal(Colors.Red, label.TextColor);
		}

		[Fact]
		public void UpdateIconViewDirection_ShouldDoNothing_WhenExpanderIconViewandExpanderIsNull()
		{
			var expanderHeader = new ExpanderHeader()
			{
				IconView = null,
				Expander = null
			};

			expanderHeader.UpdateIconViewDirection(true);
			Assert.Null(expanderHeader.IconView);
		}

		[Fact]
		public void RemoveChildrenInView_ShouldRemoveChildView_WhenChildViewIsNotNull()
		{
			var expander = new SfExpander();
			var expanderHeader = new ExpanderHeader();
			expander.Children.Add(expanderHeader);
			Assert.Contains(expanderHeader, expander.Children);
			expander.RemoveChildrenInView(expanderHeader);
			Assert.DoesNotContain(expanderHeader, expander.Children);
		}

		[Fact]
		public void RemoveChildAtIndex_ShouldRemoveChildAtSpecifiedIndex_WhenChildrenExist()
		{
			var expander = new SfExpander();
			var expanderHeader = new ExpanderHeader();
			var expanderHeader2 = new ExpanderHeader();
			var expanderHeader3 = new ExpanderHeader();
			expander.Children.Add(expanderHeader);
			expander.Children.Add(expanderHeader2);
			expander.Children.Add(expanderHeader3);
			expander.RemoveChildAtIndex(1);
			Assert.Contains(expanderHeader, expander.Children);
			Assert.Contains(expanderHeader3, expander.Children);
		}

		[Fact]
		public void RemoveChildrens_ShouldClearChildrenCollection_WhenChildrenExist()
		{
			var expander = new SfExpander();
			var expanderHeader = new ExpanderHeader();
			var expanderHeader2 = new ExpanderHeader();
			var expanderHeader3 = new ExpanderHeader();
			expander.Children.Add(expanderHeader);
			expander.Children.Add(expanderHeader2);
			expander.Children.Add(expanderHeader3);
			Assert.Contains(expanderHeader, expander.Children);
			Assert.Contains(expanderHeader2, expander.Children);
			Assert.Contains(expanderHeader3, expander.Children);
			expander.RemoveChildrens();
			Assert.Empty(expander.Children);
		}

		[Fact]
		public void SetItemVisibility_ShouldSetIsVisible_WhenVisibilityChangesTotrue()
		{
			var expander = new SfExpander()
			{ 
				IsVisible=false
			};

			expander.SetItemVisibility(true);
			Assert.True(expander.IsVisible);
		}

		[Fact]
		public void CalculateHeaderAutoHeight_Returns_WhenHeaderViewIsNull()
		{
			var expander = new SfExpander
			{
				HeaderView = null
			};

			InvokePrivateMethod(expander, "CalculateHeaderAutoHeight", 100.0);
			Assert.Null(expander.HeaderView);
		}

		[Fact]
		public void CalculateContentAutoHeight_SetsMeasuredSizeToZero_WhenContentIsNull()
		{
			var expander = new SfExpander()
			{
				IsExpanded = true,
			};

			SetPrivateField(expander, "_contentMeasuredSize", new Size(1, 1));
			InvokePrivateMethod(expander, "CalculateContentAutoHeight", 100.0);
			var contentMeasuredSize = GetPrivateField<SfExpander>(expander, "_contentMeasuredSize");
			Assert.Equal(new Size(0, 0), contentMeasuredSize);
		}

		[Fact]
		public void CalculateContentAutoHeight_SetsMeasuredSizeToZero_WhenContentIsNotNull()
		{
			var expander = new SfExpander()
			{
				IsExpanded = true,
				Content = new Label()
			};

			SetPrivateField(expander, "_contentMeasuredSize", new Size(1, 1));
			InvokePrivateMethod(expander, "CalculateContentAutoHeight", 100.0);
			var contentMeasuredSize = GetPrivateField<SfExpander>(expander, "_contentMeasuredSize");
			Assert.Equal(new Size(0, 0), contentMeasuredSize);
		}

		[Fact]
		public void UpdateContentViewLayoutAndVisibility_UpdatesContent_WhenContentIsNotNull()
		{
			var expander = new SfExpander
			{
				IsViewLoaded = true,
				Content = new Label(),
				ContentView =
				[
					new Label()
				]
			};

			InvokePrivateMethod(expander, "UpdateContentViewLayoutAndVisibility");
			Assert.True(expander.IsViewLoaded);
		}

		[Fact]
		public void UpdateIconViewDirection_RotateExpanderIcon_WhenIsExpandedIsFalse()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = new SfExpander()
				{
					IsExpanded = false,
					HeaderIconPosition = ExpanderIconPosition.Start,
					Header = new Label { Text = "Header" }
				},	
			};

			expanderHeader.IconView=new ExpandCollapseButton(expanderHeader.Expander);
			expanderHeader.UpdateIconViewDirection(expanderHeader.Expander.IsExpanded);
			Assert.NotNull(expanderHeader);
			Assert.False(expanderHeader.Expander.IsExpanded);
		}

		[Fact]
		public void UpdateVisualState_IsCollapsedState_WhenIsExpandedIsFalse()
		{
			var expander = new SfExpander()
			{
				IsExpanded = false,
			};

			var methodInfo = typeof(SfExpander).GetMethod("UpdateVisualState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			methodInfo?.Invoke(null, [expander]);
			Assert.False(expander.IsExpanded);
		}

		[Fact]
		public void UpdateVisualState_IsExpandedState_WhenIsExpandedIsTrue()
		{
			var expander = new SfExpander()
			{
				IsExpanded = true,
			};

			var methodInfo = typeof(SfExpander).GetMethod("UpdateVisualState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			methodInfo?.Invoke(null, [expander]);
			Assert.True(expander.IsExpanded);
		}

		[Fact]
		public void UpdateChildViews_ClearHeaderView_UpdateItsChildren()
		{
			var expanderHeader = new ExpanderHeader();
			var headerViewGrid = new Grid
			{
				ColumnDefinitions = { new ColumnDefinition(), new ColumnDefinition() },
				Children = { new Label { Text = "Header Label" } }
			};

			var headerViewGridField = typeof(ExpanderHeader).GetField("_headerViewGrid", BindingFlags.NonPublic | BindingFlags.Instance);
			headerViewGridField?.SetValue(expanderHeader, headerViewGrid);
			expanderHeader.Children.Add(headerViewGrid);
			expanderHeader.UpdateChildViews();
			Assert.Empty(headerViewGrid.ColumnDefinitions);
			Assert.Empty(headerViewGrid.Children);
			Assert.Contains(headerViewGrid, expanderHeader.Children);
		}

		[Fact]
		public void SetUp_ShouldInitializeExpanderHeaderProperties()
		{
			var expanderHeader = new ExpanderHeader();
			var setUpMethod = typeof(ExpanderHeader).GetMethod("SetUp", BindingFlags.NonPublic | BindingFlags.Instance);
			setUpMethod?.Invoke(expanderHeader, null);
			Assert.Equal(Colors.Transparent, expanderHeader.BackgroundColor);
			Assert.Equal(DrawingOrder.BelowContent, expanderHeader.DrawingOrder);
		}

		[Fact]
		public void SetUp_ShouldInitializeExpanderProperties()
		{
			var expander = new SfExpander();
			var setUpMethod = typeof(SfExpander).GetMethod("SetUp", BindingFlags.NonPublic | BindingFlags.Instance);
			setUpMethod?.Invoke(expander, null);
			Assert.Equal(Colors.Transparent, expander.BackgroundColor);
			Assert.NotNull(expander.HeaderView);
			Assert.NotNull(expander.ContentView);
			Assert.NotNull(expander._effectsView);
			bool isAnimationInProgress = (bool?)expander.GetType().GetField("_isAnimationInProgress", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(expander) ?? false;
			Assert.False(isAnimationInProgress);
		}

		[Fact]
		public void InitializeExpanderViewContent_AddsChildViewsAndCallsMethods()
		{
			var expander = new SfExpander();
			var expanderHeader = new ExpanderHeader();
			var contentView = new ExpanderContent();
			var content = new Label();
			var effectsView = new SfEffectsView();
			expander.HeaderView = expanderHeader;
			expander.ContentView = contentView;
			expander.Content = content;
			expander._effectsView = effectsView;
			var initializeMethod = typeof(SfExpander).GetMethod("InitializeExpanderViewContent", BindingFlags.NonPublic | BindingFlags.Instance);
			initializeMethod?.Invoke(expander, null);
			Assert.Contains(effectsView, expander.Children);
			Assert.Contains(expanderHeader, expander.Children);
			Assert.Contains(contentView, expander.Children);
			Assert.Contains(content, contentView.Children);
		}

		[Fact]
		public void CalculateContentAutoHeight_SetsMeasuredSize_WhenIsExpandedAndContentNotNull()
		{
			var expander = new SfExpander();
			var label = new Label();
			expander.Content = label;
			expander.IsExpanded = true;
			double testWidth = 100.0;
			var method = typeof(SfExpander).GetMethod("CalculateContentAutoHeight", BindingFlags.NonPublic | BindingFlags.Instance);
			method?.Invoke(expander, [testWidth]);
			var contentMeasuredSizeField = typeof(SfExpander).GetField("_contentMeasuredSize", BindingFlags.NonPublic | BindingFlags.Instance);
			var contentMeasuredSize = contentMeasuredSizeField?.GetValue(expander) as Size? ?? default;
			contentMeasuredSizeField?.GetValue(expander);
			Assert.Equal(new Size(0, 0), contentMeasuredSize);
			Assert.Equal(0, contentMeasuredSize.Width);
		}
		
		[Fact]
		public void CalculateHeaderAutoHeight_ShouldSetMeasuredSize_WhenExpanderIconPositionIsNone()
		{
			var expander = new SfExpander
			{
				WidthRequest = 200,
				HeaderIconPosition = ExpanderIconPosition.None,
				Header = new Label { Text = "Header Content" }
			};

			if (expander.HeaderView != null)
			{
				expander.HeaderView.IconView = new ExpandCollapseButton(expander);
			}

			var method = typeof(SfExpander).GetMethod("CalculateHeaderAutoHeight", BindingFlags.NonPublic | BindingFlags.Instance);
			method?.Invoke(expander, [300]);
			var headerMeasuredSizeField = typeof(SfExpander).GetField("_headerMeasuredSize", BindingFlags.NonPublic | BindingFlags.Instance);
			var headerMeasuredSize = headerMeasuredSizeField?.GetValue(expander) as Size? ?? default;
			Assert.Equal(new Size(0, 0), headerMeasuredSize);
			Assert.Equal(0, headerMeasuredSize.Width);
		}


		[Fact]
		public void CalculateHeaderAutoHeight_WhenExpanderIconPositionIsNotNone()
		{
			var expander = new SfExpander
			{
				WidthRequest = 200,
				HeaderIconPosition = ExpanderIconPosition.Start,
				Header = new Label { Text = "Header Content" }
			};

			if (expander.HeaderView != null)
			{
				expander.HeaderView.IconView = new ExpandCollapseButton(expander);
			}

			var method = typeof(SfExpander).GetMethod("CalculateHeaderAutoHeight", BindingFlags.NonPublic | BindingFlags.Instance);
			method?.Invoke(expander, [300]);
			var headerMeasuredSizeField = typeof(SfExpander).GetField("_headerMeasuredSize", BindingFlags.NonPublic | BindingFlags.Instance);
			var headerMeasuredSize = headerMeasuredSizeField?.GetValue(expander) as Size? ?? default;
			Assert.Equal(new Size(0, 0), headerMeasuredSize);
			Assert.True(headerMeasuredSize.Width <= 0);
		}

		[Fact]
		public void OnExpanderLoaded_LoadsExpander_WhenIsViewLoadedIsFalse()
		{
			var expander = new SfExpander()
			{
				IsViewLoaded=false,
				Content=new Label()
			};

			var methodInfo = typeof(SfExpander).GetMethod("OnExpanderLoaded", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [new object(), EventArgs.Empty]);
			var isViewLoadedField = typeof(SfExpander).GetProperty("IsViewLoaded", BindingFlags.NonPublic | BindingFlags.Instance);
			var isViewLoaded = isViewLoadedField?.GetValue(expander) as bool?;
			Assert.True(isViewLoaded);
		}

		[Fact]
		public void ProgressAnimation_UpdatesContentHeightOnAnimation()
		{
			var expander = new SfExpander();
			var methodInfo = typeof(SfExpander).GetMethod("ProgressAnimation",BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [100]);
			Assert.Equal(100, expander.ContentHeightOnAnimation);
		}

		[Fact]
		public void SetRTL_SetsIsRTLToFalse_WhenFlowDirectionIsLeftToRight()
		{
			var expander = new SfExpander();
			var methodInfo = typeof(SfExpander).GetMethod("SetRTL", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, null);
			var isRtlField = typeof(SfExpander).GetField("_isRTL", BindingFlags.NonPublic | BindingFlags.Instance);
			var isRtl = isRtlField?.GetValue(expander) as bool?;
			Assert.False(isRtl);
		}

		[Fact]
		public void CreateIndicatorView_Returns_WhenExpanderIsNull()
		{
			var expander = new ExpanderHeader
			{
				Expander = null,
			};

			var methodInfo = typeof(ExpanderHeader).GetMethod("CreateIndicatorView", BindingFlags.NonPublic | BindingFlags.Instance);
			var result = methodInfo?.Invoke(expander, null);
			Assert.Null(result);
		}

		[Fact]
		public void CreateIndicatorView_ShouldUpdateIconView()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander =
				[
					new Label()
				]
			};

			var methodInfo = typeof(ExpanderHeader).GetMethod("CreateIndicatorView", BindingFlags.NonPublic | BindingFlags.Instance);
			var result = methodInfo?.Invoke(expanderHeader, null);
			Assert.NotNull(result);
			Assert.NotNull(expanderHeader.IconView);
		}

		[Fact]
		public void AddChildViews_ShouldAddTwoColumnDefinitions_WhenHeaderIconPositionIsStart()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = new SfExpander
				{
					HeaderIconPosition = ExpanderIconPosition.Start,
					Header = new Label { Text = "Header" }
				},
			};

			var headerViewGridField = typeof(ExpanderHeader).GetField("_headerViewGrid", BindingFlags.NonPublic | BindingFlags.Instance);
			var headerViewGrid = new Grid();
			headerViewGridField?.SetValue(expanderHeader, headerViewGrid);
			var methodInfo = typeof(ExpanderHeader).GetMethod("AddChildViews", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expanderHeader, null);
			Assert.Equal(2, headerViewGrid.ColumnDefinitions.Count);
			Assert.Equal(new GridLength(40), headerViewGrid.ColumnDefinitions[0].Width);
			Assert.Equal(GridLength.Star, headerViewGrid.ColumnDefinitions[1].Width);
		}

		[Fact]
		public void AddChildViews_ShouldAddCorrectColumnDefinitions_WhenIconViewIsNotNull()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = new SfExpander
				{
					HeaderIconPosition = ExpanderIconPosition.Start,
					Header = new Label { Text = "Header" }
				},
				IconView = new ExpandCollapseButton([])
			};

			var headerViewGridField = typeof(ExpanderHeader).GetField("_headerViewGrid", BindingFlags.NonPublic | BindingFlags.Instance);
			var headerViewGrid = new Grid();
			headerViewGridField?.SetValue(expanderHeader, headerViewGrid);
			var methodInfo = typeof(ExpanderHeader).GetMethod("AddChildViews", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expanderHeader, null);
			Assert.Equal(2, headerViewGrid.ColumnDefinitions.Count);
			Assert.Equal(new GridLength(40), headerViewGrid.ColumnDefinitions[0].Width);
			Assert.Equal(GridLength.Star, headerViewGrid.ColumnDefinitions[1].Width);
		}

		[Fact]
		public void AddChildViews_ShouldAddHeaderAndIconView_WhenIconPositionIsEnd()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = new SfExpander
				{
					HeaderIconPosition = ExpanderIconPosition.End,
					Header = new Label { Text = "Header" }
				},
				IconView = new ExpandCollapseButton([])
			};

			var headerViewGridField = typeof(ExpanderHeader).GetField("_headerViewGrid", BindingFlags.NonPublic | BindingFlags.Instance);
			var headerViewGrid = new Grid();
			headerViewGridField?.SetValue(expanderHeader, headerViewGrid);
			var methodInfo = typeof(ExpanderHeader).GetMethod("AddChildViews", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expanderHeader, null);
			Assert.Contains(expanderHeader.Expander.Header, headerViewGrid.Children);
			Assert.Contains(expanderHeader.IconView,headerViewGrid.Children);
		}

		[Fact]
		public void AddChildViews_ShouldAddOnlyHeader_WhenIconPositionIsNone()
		{
			var expanderHeader = new ExpanderHeader
			{
				Expander = new SfExpander
				{
					HeaderIconPosition = ExpanderIconPosition.None,
					Header = new Label { Text = "Header" }
				},
				IconView = new ExpandCollapseButton([])
			};

			var headerViewGridField = typeof(ExpanderHeader).GetField("_headerViewGrid", BindingFlags.NonPublic | BindingFlags.Instance);
			var headerViewGrid = new Grid();
			headerViewGridField?.SetValue(expanderHeader, headerViewGrid);
			var methodInfo = typeof(ExpanderHeader).GetMethod("AddChildViews", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expanderHeader, null);
			var headerColumnIndex = Grid.GetColumn(expanderHeader.Expander.Header);
			Assert.Equal(0, headerColumnIndex);
		}

		[Fact]
		public void OnContentPropertyChanged_ShouldUpdateContent_WhenContentIsNotNull()
		{
			var expander = new SfExpander
			{
				IsExpanded = true,
			};

			var oldContent = new Label() { Text = "Unit test" };
			var newContent = new Label() { Text = "Label" };
			expander.Content = oldContent;
			var methodInfo = typeof(SfExpander).GetMethod("OnContentPropertyChanged", BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null, [expander, oldContent, newContent]);
			Assert.True(newContent.IsVisible);
		}

		[Fact]
		public void OnContentPropertyChanged_ShouldUpdateContent_WhenContentIsNull()
		{
			var expander = new SfExpander
			{
				IsExpanded = true,
				Content=new Label()
			};

			expander.Content = null;
			expander.IsViewLoaded = true;
			Assert.Null(expander.Content);
		}

		[Fact]
		public void OnHeaderPropertyChanged_ShouldUpdateHeader_WhenHeaderIsNotNull()
		{
			var expander = new SfExpander
			{
				Content = new Label(),
				Header = new Label(),
			};

			var oldValue = new Button();
			var newValue = new Label();
			var methodInfo = typeof(SfExpander).GetMethod("OnHeaderPropertyChanged", BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null, [expander, oldValue, newValue]);
			Assert.NotNull(expander.Header);
		}

		[Fact]
		public void OnHeaderIconPositionPropertyChanged_ShouldUpdateHeaderIconPosition()
		{
			var expander = new SfExpander() 
			{ 
				IsViewLoaded = true, 
				HeaderIconPosition=ExpanderIconPosition.Start
			};

			var oldValue = ExpanderIconPosition.End;
			var newValue = ExpanderIconPosition.Start;
			var methodInfo = typeof(SfExpander).GetMethod("OnHeaderIconPositionPropertyChanged", BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null, [expander, oldValue, newValue]);
			Assert.Equal(newValue, expander.HeaderIconPosition);
		}

		[Fact]
		public void OnIsExpandedPropertyChanged_UpdatesContentVisibility_WhenExpanded()
		{
			var expander = new SfExpander
			{
				Content = new Label { IsVisible = false },
				IsExpanded = false
			};

			var methodInfo = typeof(SfExpander).GetMethod("OnIsExpandedPropertyChanged",BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null,[expander, false, true]);
			Assert.True(expander.Content.IsVisible);
		}

		[Fact]
		public void OnHeaderBackgroundPropertyChanged_ShouldUpdateHeaderBackground_WhenIsViewLoadedAsTrueAndHeaderViewIsNull()
		{
			var expander = new SfExpander
			{
				IsViewLoaded = true,
				HeaderView =
				[
				  new Label()
				],
				HeaderBackground=Colors.Red
			};

			var oldValue = Colors.Blue;
			var newValue = Colors.Red;
			var methodInfo = typeof(SfExpander).GetMethod("OnHeaderBackgroundPropertyChanged", BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null, [expander, oldValue, newValue]);
			Assert.Equal(newValue,expander.HeaderBackground);
		}

		[Fact]
		public void OnHeaderBackgroundPropertyChanged_Returns_WhenIsViewLoadedasFalseAndHeaderViewIsNull()
		{
			var expander = new SfExpander
			{
				IsViewLoaded = false,
			};

			var methodInfo = typeof(SfExpander).GetMethod("OnHeaderBackgroundPropertyChanged", BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null, [expander, new(), new()]);
			Assert.False(expander.IsViewLoaded);
		}
		
		[Fact]
		public void OnHeaderIconColorPropertyChanged_ShouldUpdateHeaderIconColor_WhenIsViewLoadedasFalseAndHeaderViewIsNull()
		{
			var expander = new SfExpander
			{
				IsViewLoaded = false,
				HeaderIconColor=Colors.Red
			};

			var oldValue = Colors.Blue;
			var newValue = Colors.Red;
			var methodInfo = typeof(SfExpander).GetMethod("OnHeaderIconColorPropertyChanged", BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null, [expander, oldValue, newValue]);
			Assert.Equal(expander.HeaderIconColor,newValue);
		}

		[Fact]
		public void OnEffectsViewAnimationCompleted_ShouldSet_IsRippleAnimationProgressToFalse()
		{
			var expander = new SfExpander();
			var methodInfo = typeof(SfExpander).GetMethod("OnEffectsViewAnimationCompleted", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [expander, EventArgs.Empty]);
			Assert.False(expander._isRippleAnimationProgress);
		}

		[Fact]
		public void OnContentChanged_ShouldUpdateContent_WhenIsViewLoadedIsFalseAndContentViewIsNull()
		{
			var expander = new SfExpander()
			{
				Content = new Button()
			};

			var oldValue = new ExpanderHeader();
			var newValue = new ExpanderHeader();
			var methodInfo = typeof(SfExpander).GetMethod("OnContentChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [oldValue, newValue]);
			Assert.False(expander.IsViewLoaded);
		}

		[Fact]
		public void OnContentChanged_ShouldUpdateContent_WhenIsViewLoadedIsTrue()
		{
			var expander = new SfExpander()
			{
				IsViewLoaded = true,
				ContentView=
				[
					new Label()
				],
				Content=new Button()
			};

			var oldValue = new ExpanderHeader();
			var newValue = new ExpanderHeader();
			var methodInfo = typeof(SfExpander).GetMethod("OnContentChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [oldValue, newValue]);
			Assert.True(expander.IsViewLoaded);
		}

		[Fact]
		public void OnHeaderChanged_Returns_WhenIsViewLoadedIsFalseAndHeaderViewIsNull()
		{
			var expander = new SfExpander()
			{
				Header= new Label()
			};

			var oldValue = new Button();
			var newValue = new Label();
			var methodInfo = typeof(SfExpander).GetMethod("OnHeaderChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [oldValue, newValue]);
			Assert.False(expander.IsViewLoaded);
		}

		[Fact]
		public void OnHeaderIconPositionChanged_ShouldUpdateHeaderIconPosition_WhenIsViewLoadedIsTrueAndHeaderViewIsNotNull()
		{
			var expander = new SfExpander()
			{
				IsViewLoaded = true,
				HeaderIconPosition = ExpanderIconPosition.End,
				HeaderView= [new Label()]
			};

			var oldValue = ExpanderIconPosition.Start;
			var newValue = ExpanderIconPosition.End;
			var methodInfo = typeof(SfExpander).GetMethod("OnHeaderIconPositionChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [oldValue, newValue]);
			Assert.Equal(newValue, expander.HeaderIconPosition);
		}

		[Fact]
		public void OnIconColorChanged_ShouldUpdateIconColor_WhenIsViewLoadedIsTrue()
		{
			var expander = new SfExpander
			{
				IsViewLoaded = true,
				HeaderIconPosition =
				ExpanderIconPosition.Start,
				HeaderView =
				[
					new Label()
				]
			};

			expander.HeaderView.IconView = new ExpandCollapseButton(expander);
			var methodInfo = typeof(SfExpander).GetMethod("OnIconColorChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, [Colors.Green, Colors.Red]);
			Assert.NotNull(expander.HeaderView.IconView);
		}

		[Fact]
		public void RaiseExpandingEvent_ShouldSetIsExpandedToFalse_WhenAnimationIsInProgress()
		{
			var expander = new SfExpander
			{
				IsExpanded = false,
			};

			SetPrivateField(expander, "_isAnimationInProgress", true);
			expander.RaiseExpandingEvent();
			Assert.False(expander.IsExpanded);
		}

		[Fact]
		public void RaiseExpandingEvent_ShouldSetIsExpandedToTrue_WhenExpandingEventIsNotCancelled()
		{
			var expander = new SfExpander
			{
				IsExpanded = false,
			};

			expander.Expanding += (sender, args) =>
			{
				args.Cancel = false;
			};

			expander.RaiseExpandingEvent();
			Assert.True(expander.IsExpanded);
		}

		[Fact]
		public void RaiseExpandedEvent_ShouldTriggerEvent_WhenItemIsExpanded()
		{
			var expander = new SfExpander();
			bool eventTriggered = false;
			expander.Expanded += (sender, args) =>
			{
				eventTriggered = true;
			};

			expander.RaiseExpandedEvent();
			Assert.True(eventTriggered);
		}

		[Fact]
		public void RaiseCollapsingEvent_ShouldNotSetIsExpandedToTrue_WhenAnimationIsInProgress()
		{
			var expander = new SfExpander
			{
				IsExpanded = false,
			};

			SetPrivateField(expander, "_isAnimationInProgress", true);
			expander.RaiseCollapsingEvent();
			Assert.False(expander.IsExpanded);
		}

		[Fact]
		public void RaiseCollapsingEvent_ShouldSetIsExpandedToTrue_WhenExpandingEventIsNotCancelled()
		{
			var expander = new SfExpander
			{
				IsExpanded = false,
			};

			expander.Collapsing += (sender, args) =>
			{
				args.Cancel = false;
			};

			expander.RaiseCollapsingEvent();
			Assert.False(expander.IsExpanded);
		}

		[Fact]
		public void RaiseCollapsedEvent_ShouldTriggerEvent_WhenItemIsCollapsed()
		{
			var expander = new SfExpander();
			bool eventTriggered = false;
			expander.Collapsed += (sender, args) =>
			{
				eventTriggered = true;
			};

			expander.RaiseCollapsedEvent();
			Assert.True(eventTriggered);
		}

		[Fact]
		public void AnimationCompleted_CallsExpandedEvent_WhenExpandedIsTrue()
		{
			var expander = new SfExpander
			{
				IsExpanded = true,
				Content = new Label()
			};

			var methodInfo = typeof(SfExpander).GetMethod("AnimationCompleted", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, null);
			Assert.True(expander.IsExpanded);
		}

		[Fact]
		public void AnimationCompleted_CallsCollapsedEvent_WhenExpandedIsFalseAndContentIsNotNull()
		{
			var expander = new SfExpander
			{
				IsExpanded = false,
				Content = new Label()
			};

			var methodInfo = typeof(SfExpander).GetMethod("AnimationCompleted", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(expander, null);
			Assert.False(expander.Content.IsVisible);
		}

		[Fact]
		public void OnPropertyChanged_ShouldUpdateProperty_WhenIsEnabledChanges()
		{
			var expander = new SfExpander
			{
				ContentView =
				[
					new Label()
				],
				HeaderView =
				[
					new Label()
				],
				IsEnabled = true,
				FlowDirection = FlowDirection.LeftToRight,
			};

			expander.IsEnabled = false;
			expander.FlowDirection = FlowDirection.RightToLeft;
			Assert.Equal(FlowDirection.RightToLeft, expander.FlowDirection);
			Assert.False(expander.IsEnabled);
		}

		#endregion

	}
}
