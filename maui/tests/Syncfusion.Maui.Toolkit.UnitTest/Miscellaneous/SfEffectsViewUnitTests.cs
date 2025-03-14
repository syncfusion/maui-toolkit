using Syncfusion.Maui.Toolkit.EffectsView;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfEffectsViewUnitTests : BaseUnitTest
	{
		#region Contructor
		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			SfEffectsView effectsView = [];

			Assert.NotNull(effectsView);
			Assert.Equal(0, effectsView.Angle);
			Assert.Equal(180d, effectsView.RippleAnimationDuration);
			Assert.Equal(150d, effectsView.ScaleAnimationDuration);
			Assert.Equal(200d, effectsView.RotationAnimationDuration);
			Assert.Equal(0.25d, effectsView.InitialRippleFactor);
			Assert.Equal(1d, effectsView.ScaleFactor);
			var color = new SolidColorBrush(Color.FromArgb("#1C1B1F"));
			Assert.Equal(color.Color, ((SolidColorBrush)effectsView.HighlightBackground).Color);
			Assert.Equal(color.Color, ((SolidColorBrush)effectsView.RippleBackground).Color);
			Assert.Equal(color.Color, ((SolidColorBrush)effectsView.SelectionBackground).Color);
			Assert.False(effectsView.FadeOutRipple);
			Assert.Equal(AutoResetEffects.None, effectsView.AutoResetEffects);
			Assert.Equal(SfEffects.Ripple, effectsView.TouchDownEffects);
			Assert.Equal(SfEffects.None, effectsView.TouchUpEffects);
			Assert.Equal(SfEffects.None, effectsView.LongPressEffects);
			Assert.Null(effectsView.LongPressedCommand);
			Assert.Null(effectsView.LongPressedCommandParameter);
			Assert.False(effectsView.IsSelected);
			Assert.False(effectsView.ShouldIgnoreTouches);
			Assert.Null(effectsView.TouchDownCommand);
			Assert.Null(effectsView.TouchUpCommand);
			Assert.Null(effectsView.TouchDownCommandParameter);
			Assert.Null(effectsView.TouchUpCommandParameter);
		}

		#endregion
		#region Public Properties
		[Theory]
		[InlineData(120)]
		[InlineData(-180)]
		[InlineData(0)]
		public void Angle_SetValue_ReturnsExpectedValue(int angle)
		{
			SfEffectsView effectsView = [];
			effectsView.Angle = angle;

			Assert.Equal(angle, effectsView.Angle);
		}

		[Theory]
		[InlineData(AutoResetEffects.Highlight)]
		[InlineData(AutoResetEffects.Scale)]
		[InlineData(AutoResetEffects.Ripple)]
		[InlineData(AutoResetEffects.None)]
		public void AutoResetEffects_SetValue_ReturnsExpectedValue(AutoResetEffects autoResetEffects)
		{
			SfEffectsView effectsView = [];
			effectsView.AutoResetEffects = autoResetEffects;

			Assert.Equal(autoResetEffects, effectsView.AutoResetEffects);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void FadeOutRipple_SetValue_ReturnsExpectedValue(bool fadeOutRipple)
		{
			SfEffectsView effectsView = [];
			effectsView.FadeOutRipple = fadeOutRipple;

			Assert.Equal(fadeOutRipple, effectsView.FadeOutRipple);
		}

		[Theory]
		[InlineData("#FF0000")]

		public void HighlightBackground_SetValue_ReturnsExpectedValue(string colorName)
		{
			SfEffectsView effectsView = [];
			var color = Color.FromArgb(colorName);
			var highlightBackground = new SolidColorBrush(color);
			effectsView.HighlightBackground = highlightBackground;

			Assert.Same(highlightBackground, effectsView.HighlightBackground);
		}

		[Theory]
		[InlineData(0.5)]
		[InlineData(0)]
		[InlineData(-0.75)]
		public void InitialRippleFactor_SetValue_ReturnsExpectedValue(double initialRippleFactor)
		{
			SfEffectsView effecstView = [];
			effecstView.InitialRippleFactor = initialRippleFactor;

			Assert.Equal(initialRippleFactor, effecstView.InitialRippleFactor);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsSelected_SetValue_ReturnsExpectedValue(bool isSelected)
		{
			SfEffectsView effectsView = [];
			effectsView.IsSelected = isSelected;

			Assert.Equal(isSelected, effectsView.IsSelected);
		}

		[Theory]
		[InlineData(1000)]
		[InlineData(0)]
		[InlineData(-50)]
		public void RippleAnimationDuration_SetValue_ReturnsExpectedValue(double rippleAnimationDuration)
		{
			SfEffectsView effecstView = [];
			effecstView.RippleAnimationDuration = rippleAnimationDuration;

			Assert.Equal(rippleAnimationDuration, effecstView.RippleAnimationDuration);
		}

		[Theory]
		[InlineData("#0000FF")]
		public void RippleBackground_SetValue_ReturnsExpectedValue(string colorName)
		{
			SfEffectsView effectsView = [];
			var color = Color.FromArgb(colorName);
			var rippleBackground = new SolidColorBrush(color);
			effectsView.RippleBackground = rippleBackground;

			Assert.Same(rippleBackground, effectsView.RippleBackground);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-100)]
		[InlineData(1200)]
		public void RotationAnimationDuration_SetValue_ReturnsExpectedValue(double rotationAnimationDuration)
		{
			SfEffectsView effecstView = [];
			effecstView.RotationAnimationDuration = rotationAnimationDuration;

			Assert.Equal(rotationAnimationDuration, effecstView.RotationAnimationDuration);
		}

		[Theory]
		[InlineData(1000)]
		[InlineData(0)]
		[InlineData(-2)]
		public void ScaleAnimationDuration_SetValue_ReturnsExpectedValue(double scaleAnimationDuration)
		{
			SfEffectsView effecstView = [];
			effecstView.ScaleAnimationDuration = scaleAnimationDuration;

			Assert.Equal(scaleAnimationDuration, effecstView.ScaleAnimationDuration);
		}

		[Theory]
		[InlineData(0.5)]
		[InlineData(0)]
		[InlineData(-1.5)]
		public void ScaleFactor_SetValue_ReturnsExpectedValue(double scaleFactor)
		{
			SfEffectsView effecstView = [];
			effecstView.ScaleFactor = scaleFactor;

			Assert.Equal(scaleFactor, effecstView.ScaleFactor);
		}

		[Theory]
		[InlineData("#FF0000")]

		public void SelectionBackground_SetValue_ReturnsExpectedValue(string colorName)
		{
			SfEffectsView effectsView = [];
			var color = Color.FromArgb(colorName);
			var selectionBackground = new SolidColorBrush(color);
			effectsView.SelectionBackground = selectionBackground;

			Assert.Same(selectionBackground, effectsView.SelectionBackground);
		}

		[Theory]
		[InlineData(SfEffects.Selection)]
		[InlineData(SfEffects.Rotation)]
		[InlineData(SfEffects.Scale)]
		[InlineData(SfEffects.Highlight)]
		[InlineData(SfEffects.Ripple)]
		[InlineData(SfEffects.None)]
		public void LongPressEffects_SetValue_ReturnsExpectedValue(SfEffects longPressEffects)
		{
			var effectsView = new SfEffectsView
			{
				LongPressEffects = longPressEffects
			};

			Assert.Equal(longPressEffects, effectsView.LongPressEffects);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShouldIgnoreTouches_SetValue_ReturnsExpectedValue(bool shouldIgnoreTouches)
		{
			SfEffectsView effectsView = [];
			effectsView.ShouldIgnoreTouches = shouldIgnoreTouches;

			Assert.Equal(shouldIgnoreTouches, effectsView.ShouldIgnoreTouches);
		}

		[Theory]
		[InlineData(SfEffects.Selection)]
		[InlineData(SfEffects.Rotation)]
		[InlineData(SfEffects.Scale)]
		[InlineData(SfEffects.Highlight)]
		[InlineData(SfEffects.Ripple)]
		[InlineData(SfEffects.None)]
		public void TouchDownEffects_SetValue_ReturnsExpectedValue(SfEffects touchDownEffects)
		{
			var effectsView = new SfEffectsView
			{
				TouchDownEffects = touchDownEffects
			};

			Assert.Equal(touchDownEffects, effectsView.TouchDownEffects);
		}

		[Theory]
		[InlineData(SfEffects.Selection)]
		[InlineData(SfEffects.Rotation)]
		[InlineData(SfEffects.Scale)]
		[InlineData(SfEffects.Highlight)]
		[InlineData(SfEffects.Ripple)]
		[InlineData(SfEffects.None)]
		public void TouchUpEffects_SetValue_ReturnsExpectedValue(SfEffects touchUpEffects)
		{
			var effectsView = new SfEffectsView
			{
				TouchUpEffects = touchUpEffects
			};

			Assert.Equal(touchUpEffects, effectsView.TouchUpEffects);
		}

		[Fact]
		public void TouchUpCommandParameter_SetValue_ReturnsExpectedValue()
		{
			var effectsView = new SfEffectsView();
			var touchUpCommandParameter = new object();
			effectsView.TouchUpCommandParameter = touchUpCommandParameter;

			Assert.Equal(touchUpCommandParameter, effectsView.TouchUpCommandParameter);
		}

		[Fact]
		public void LongPressedCommandParameter_SetValue_ReturnsExpectedValue()
		{
			var effectsView = new SfEffectsView();
			var longPressedCommandParameter = new object();
			effectsView.LongPressedCommandParameter = longPressedCommandParameter;

			Assert.Equal(longPressedCommandParameter, effectsView.LongPressedCommandParameter);
		}

		[Fact]
		public void TouchDownCommandParameter_SetValue_ReturnsExpectedValue()
		{
			var effectsView = new SfEffectsView();
			var touchDownCommandParameter = new object();
			effectsView.TouchDownCommandParameter = touchDownCommandParameter;

			Assert.Equal(touchDownCommandParameter, effectsView.TouchDownCommandParameter);
		}

		#endregion
		#region Commands

		[Fact]
		public void LongPressedCommand_SetBinding_ExecutesCommandAndReturnsExpectedValue()
		{
			var effectsView = new SfEffectsView();
			bool commandExecuted = false;
			var mockCommand = new Command(() =>
			{
				commandExecuted = true;
			});
			effectsView.BindingContext = new
			{
				LongPressedCommand = mockCommand
			};
			effectsView.SetBinding(SfEffectsView.LongPressedCommandProperty, new Binding("LongPressedCommand"));
			effectsView.LongPressedCommand?.Execute(null);

			Assert.True(commandExecuted);
			Assert.Same(mockCommand, effectsView.LongPressedCommand);
		}

		[Fact]
		public void TouchUpCommand_SetBinding_ExecutesCommandAndReturnsExpectedValue()
		{
			var effectsView = new SfEffectsView();
			bool commandExecuted = false;
			var mockCommand = new Command(() =>
			{
				commandExecuted = true;
			});
			effectsView.BindingContext = new
			{
				TouchUpCommand = mockCommand
			};
			effectsView.SetBinding(SfEffectsView.TouchUpCommandProperty, new Binding("TouchUpCommand"));
			effectsView.TouchUpCommand?.Execute(null);

			Assert.True(commandExecuted);
			Assert.Same(mockCommand, effectsView.TouchUpCommand);
		}

		[Fact]
		public void TouchDownCommand_SetBinding_ExecutesCommandAndReturnsExpectedValue()
		{
			var effectsView = new SfEffectsView();
			bool commandExecuted = false;
			var mockCommand = new Command(() =>
			{
				commandExecuted = true;
			});
			effectsView.BindingContext = new
			{
				TouchDownCommand = mockCommand
			};
			effectsView.SetBinding(SfEffectsView.TouchDownCommandProperty, new Binding("TouchDownCommand"));
			effectsView.TouchDownCommand?.Execute(null);

			Assert.True(commandExecuted);
			Assert.Same(mockCommand, effectsView.TouchDownCommand);
		}
		#endregion
		#region Internal Properties

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsSelection_Setvalue_ReturnsExpectedValue(bool isSelection)
		{
			var effectsView = new SfEffectsView
			{
				IsSelection = isSelection
			};

			Assert.Equal(isSelection, effectsView.IsSelection);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void LongPressHandled_Setvalue_ReturnsExpectedValue(bool longPressHandled)
		{
			var effectsView = new SfEffectsView
			{
				LongPressHandled = longPressHandled
			};

			Assert.Equal(longPressHandled, effectsView.LongPressHandled);
		}

		#endregion
	}
}