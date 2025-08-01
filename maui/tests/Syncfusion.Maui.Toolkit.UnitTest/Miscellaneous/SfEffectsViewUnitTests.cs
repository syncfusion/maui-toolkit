using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Internals;

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
		[InlineData(1.0)]
		[InlineData(0.0)]
		[InlineData(-2.0)]
		public void TestScale(double scaleValue)
		{
			var effectsView = new SfEffectsView();
			effectsView.Scale = scaleValue;
			Assert.Equal(scaleValue, effectsView.Scale);
		}

		[Theory]
		[InlineData(1.0)]
		[InlineData(0.0)]
		[InlineData(-1.0)]
		public void TestOpacity(double opacityValue)
		{
			var effectsView = new SfEffectsView();
			effectsView.Opacity = opacityValue;
			if (opacityValue < 0)
			{
				Assert.Equal(0.0, effectsView.Opacity);
			}
			else
			{
				Assert.Equal(opacityValue, effectsView.Opacity);
			}
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestIsEnabled(bool isEnabled)
		{
			var effectsView = new SfEffectsView();
			effectsView.IsEnabled = isEnabled;
			Assert.Equal(isEnabled, effectsView.IsEnabled);
		}

		[Theory]
		[InlineData(200.0)]
		[InlineData(0.0)]
		[InlineData(-300.0)]
		public void TestTranslationX(double translation)
		{
			var effectsView = new SfEffectsView();
			effectsView.TranslationX = translation;
			Assert.Equal(translation, effectsView.TranslationX);
		}

		[Theory]
		[InlineData(200.0)]
		[InlineData(0.0)]
		[InlineData(-300.0)]
		public void TestTranslationY(double translation)
		{
			var effectsView = new SfEffectsView();
			effectsView.TranslationX = translation;
			Assert.Equal(0.0, effectsView.TranslationY);
		}

		[Theory]
		[InlineData("Red")]
		[InlineData("Green")]
		public void TestEffectColor(string colorName)
		{
			var effectsView = new SfEffectsView();
			var expected = colorName switch
			{
				"Red" => Colors.Red,
				"Green" => Colors.Green,
				_ => throw new ArgumentOutOfRangeException()
			};
			effectsView.BackgroundColor = expected;
			Assert.Equal(expected, effectsView.BackgroundColor);
		}

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
		public void TestState_Initialization()
		{
			var effectsView = new SfEffectsView();
			Assert.Equal(SfEffects.None, effectsView.TouchUpEffects);
			Assert.Equal(AutoResetEffects.None, effectsView.AutoResetEffects);
			Assert.Equal(1.0, effectsView.Scale);
		}

		[Fact]
		public void TestUpdateSelectionBackground()
		{
			var view = new SfEffectsView();
			view.SelectionBackground = new SolidColorBrush(Colors.Red);
			var brush = new SolidColorBrush(Colors.Red);
			view.IsSelected = true;
			InvokePrivateMethod(view, "UpdateSelectionBackground", brush);
			Assert.Equal(brush.Color, view.SelectionBackground);
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

		[Fact]
		public void TestLongPressHandled_ShouldSetAndGetProperly()
		{
			var effectsView = new SfEffectsView();
			effectsView.LongPressHandled = true;
			Assert.True(effectsView.LongPressHandled);
		}

		[Fact]
		public void TestInvokeTouchUpEventAndCommand()
		{
			var view = new SfEffectsView();
			bool eventTriggered = false;
			view.TouchUp += (sender, args) => eventTriggered = true;
			view.InvokeTouchUpEventAndCommand();
			Assert.True(eventTriggered);
		}

		[Fact]
		public void TestInvokeTouchDownEventAndCommand()
		{
			var view = new SfEffectsView();
			bool eventTriggered = false;
			view.TouchDown += (sender, args) => eventTriggered = true;
			view.InvokeTouchDownEventAndCommand();
			Assert.True(eventTriggered);
		}

		[Fact]
		public void TestInvokeLongPressEventAndCommand()
		{
			var view = new SfEffectsView();
			bool eventTriggered = false;
			view.LongPressed += (sender, args) => eventTriggered = true;
			view.InvokeLongPressedEventAndCommand();
			Assert.True(eventTriggered);
		}

		[Fact]
		public void TestInvokeTouchUpEventAndCommand_ShouldExecuteCommand()
		{
			var view = new SfEffectsView();
			var commandExecuted = false;
			view.TouchUpCommand = new Command(() => commandExecuted = true);
			view.InvokeTouchUpEventAndCommand();
			Assert.True(commandExecuted);
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

		#region Events

		[Fact]
		public void TestAnimationCompleted_Event()
		{
			var sfEffectsView = new SfEffectsView();
			bool eventTriggered = false;
			sfEffectsView.AnimationCompleted += (sender, args) => eventTriggered = true;
			sfEffectsView.RaiseAnimationCompletedEvent(EventArgs.Empty);
			Assert.True(eventTriggered, "AnimationCompleted event was not triggered correctly.");
		}

		[Fact]
		public void TestTouchEventsSequence()
		{
			var effectsView = new SfEffectsView();
			bool touchDownFired = false, touchUpFired = false;

			effectsView.TouchDown += (s, e) => touchDownFired = true;
			effectsView.TouchUp += (s, e) => touchUpFired = true;

			effectsView.InvokeTouchDownEventAndCommand();
			effectsView.InvokeTouchUpEventAndCommand();

			Assert.True(touchDownFired && touchUpFired, "Both touch events should fire in sequence.");
		}

		#endregion

		#region Test Layouts

		[Fact]
		public void Test_InStackLayout()
		{
			var stackLayout = new StackLayout();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in StackLayout" } };
			stackLayout.Children.Add(effectsView);
			Assert.Contains(effectsView, stackLayout.Children);
			Assert.IsType<StackLayout>(effectsView.Parent);
		}

		[Fact]
		public void Test_InGrid()
		{
			var grid = new Grid();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in Grid" } };
			grid.Children.Add(effectsView);
			Assert.Contains(effectsView, grid.Children);
			Assert.IsType<Grid>(effectsView.Parent);
		}

		[Fact]
		public void Test_InAbsoluteLayout_()
		{
			var absoluteLayout = new AbsoluteLayout();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in AbsoluteLayout" } };
			AbsoluteLayout.SetLayoutBounds(effectsView, new Rect(0, 0, 100, 50));
			AbsoluteLayout.SetLayoutFlags(effectsView, AbsoluteLayoutFlags.None);
			absoluteLayout.Children.Add(effectsView);
			Assert.Contains(effectsView, absoluteLayout.Children);
			Assert.IsType<AbsoluteLayout>(effectsView.Parent);
		}

		[Fact]
		public void Test_InFlexLayout()
		{
			var flexLayout = new FlexLayout();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in FlexLayout" } };
			flexLayout.Children.Add(effectsView);
			Assert.Contains(effectsView, flexLayout.Children);
			Assert.IsType<FlexLayout>(effectsView.Parent);
		}

		[Fact]
		public void Test_InHorizontalStackLayout()
		{
			var horizontalStackLayout = new HorizontalStackLayout();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in HorizontalStackLayout" } };
			horizontalStackLayout.Children.Add(effectsView);
			Assert.Contains(effectsView, horizontalStackLayout.Children);
			Assert.IsType<HorizontalStackLayout>(effectsView.Parent);
		}

		[Fact]
		public void Test_InVerticalStackLayout()
		{
			var verticalStackLayout = new VerticalStackLayout();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in VerticalStackLayout" } };
			verticalStackLayout.Children.Add(effectsView);
			Assert.Contains(effectsView, verticalStackLayout.Children);
			Assert.IsType<VerticalStackLayout>(effectsView.Parent);
		}

		[Fact]
		public void Test_InScrollView()
		{
			var scrollView = new ScrollView();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in ScrollView" } };
			scrollView.Content = effectsView;
			Assert.Equal(effectsView, scrollView.Content);
			Assert.IsType<ScrollView>(effectsView.Parent);
		}

		[Fact]
		public void Test_InContentView()
		{
			var contentView = new ContentView();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in ContentView" } };
			contentView.Content = effectsView;
			Assert.Equal(effectsView, contentView.Content);
			Assert.IsType<ContentView>(effectsView.Parent);
		}

		[Fact]
		public void Test_InCustomLayout()
		{
			var customLayout = new AbsoluteLayout();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in Custom Layout" } };
			AbsoluteLayout.SetLayoutBounds(effectsView, new Rect(10, 20, 150, 50));
			AbsoluteLayout.SetLayoutFlags(effectsView, AbsoluteLayoutFlags.PositionProportional);
			customLayout.Children.Add(effectsView);
			Assert.Contains(effectsView, customLayout.Children);
			Assert.IsType<AbsoluteLayout>(effectsView.Parent);
		}

		[Fact]
		public void TestMultipleVisualStateGroups()
		{
			var effectsView = new SfEffectsView();
			var group1 = new VisualStateGroup { Name = "Group1", States = { new VisualState { Name = "State1" } } };
			var group2 = new VisualStateGroup { Name = "Group2", States = { new VisualState { Name = "State2" } } };
			VisualStateManager.SetVisualStateGroups(effectsView, new VisualStateGroupList { group1, group2 });
			Assert.Equal(2, VisualStateManager.GetVisualStateGroups(effectsView).Count);
		}

		[Fact]
		public void TestAddVSMStates()
		{
			var effectsView = new SfEffectsView();
			var group = new VisualStateGroup
			{
				Name = "CommonStates",
				States =
		{
			new VisualState { Name = "Normal" },
			new VisualState { Name = "Disabled" }
		}
			};

			VisualStateManager.SetVisualStateGroups(effectsView, new VisualStateGroupList { group });
			var visualStateGroups = VisualStateManager.GetVisualStateGroups(effectsView);
			Assert.Single(visualStateGroups);
			Assert.Equal(2, visualStateGroups[0].States.Count);
		}

		[Fact]
		public void TestIsEnabledChange()
		{
			var effectsView = new SfEffectsView();
			var group = new VisualStateGroup
			{
				Name = "CommonStates",
				States =
		{
			new VisualState { Name = "Enabled" },
			new VisualState { Name = "Disabled" }
		}
			};

			VisualStateManager.SetVisualStateGroups(effectsView, new VisualStateGroupList { group });
			effectsView.IsEnabled = false;
			VisualStateManager.GoToState(effectsView, "Disabled");
			var currentState = VisualStateManager.GetVisualStateGroups(effectsView)[0].CurrentState;
			Assert.Equal("Disabled", currentState.Name);
		}

		[Fact]
		public void TestRTLDirection()
		{
			var effectsView = new SfEffectsView { FlowDirection = FlowDirection.RightToLeft };
			Assert.Equal(FlowDirection.RightToLeft, effectsView.FlowDirection);
		}

		[Fact]
		public void Test_DefaultVSMStates()
		{
			var effectsView = new SfEffectsView();
			var visualStateGroups = VisualStateManager.GetVisualStateGroups(effectsView);
			Assert.Empty(visualStateGroups);
		}

		[Fact]
		public void Test_InBorder()
		{
			var border = new Border();
			var effectsView = new SfEffectsView { Content = new Label { Text = "Test in Frame" } };
			border.Content = effectsView;
			Assert.Equal(effectsView, border.Content);
			Assert.IsType<Border>(effectsView.Parent);
		}

		[Fact]
		public void TestRTLContent()
		{
			var label = new Label { Text = "Test Content", FlowDirection = FlowDirection.RightToLeft };
			var effectsView = new SfEffectsView { Content = label };
			Assert.Equal(FlowDirection.RightToLeft, ((Label)effectsView.Content).FlowDirection);
		}

		#endregion

		#region Methods

		[Fact]
		public void TestRapid_Reset_Calls()
		{
			var effectsView = new SfEffectsView();

			for (int i = 0; i < 1000; i++)
			{
				effectsView.Reset();
			}

			Assert.True(true);
		}

		[Theory]
		[InlineData(PointerActions.Released)]
		[InlineData(PointerActions.Moved)]
		[InlineData(PointerActions.Exited)]
		[InlineData(PointerActions.Cancelled)]
		public void Test_EffectsView_OnTouch(PointerActions action)
		{
			var effectsView = new SfEffectsView();
			effectsView.AutoResetEffects = AutoResetEffects.None;
			var eventArgs = new Syncfusion.Maui.Toolkit.Internals.PointerEventArgs(1, action, new Point(30, 30));
			var exception = Record.Exception(() => ((ITouchListener)effectsView).OnTouch(eventArgs));
			Assert.Null(exception);
		}

		[Fact]
		public void Test_ArrangeContent()
		{
			var effectsView = new SfEffectsView();
			InvokePrivateMethod(effectsView, "AddResetEffects", AutoResetEffects.None, new Point(30, 30));
			Assert.Equal(AutoResetEffects.None, effectsView.AutoResetEffects);
		}

		[Fact]
		public void Test_ApplyEffects()
		{
			var effectsView = new SfEffectsView
			{
				IsSelected = true,
				Content = new Label { Text = "Test", WidthRequest = 200, HeightRequest = 200 }
			};
			var result = InvokePrivateMethod(effectsView, "ArrangeContent", new Rect(10, 10, 100, 100));
			Assert.NotNull(result);
			var contentSize = (Size)result;
			Assert.Equal(100, contentSize.Width);
		}

		[Fact]
		public void Test_OnAnimationUpdate()
		{
			var effectsView = new SfEffectsView
			{
				Content = new Label { Text = "Test", WidthRequest = 200, HeightRequest = 200 }
			};
			InvokePrivateMethod(effectsView, "OnAnimationUpdate", 30);
			InvokePrivateMethod(effectsView, "OnScaleAnimationUpdate", 50);
			Assert.Equal(30, effectsView.Content.Rotation);
			Assert.Equal(50, effectsView.Content.Scale);
		}

		[Fact]
		public void Test_UpdateToGradient()
		{
			var effectsView = new SfEffectsView();
			var rippleEffectLayer = GetPrivateField(effectsView, "_rippleEffectLayer") as RippleEffectLayer;
			var radialGradientBrush = (RadialGradientBrush?)InvokePrivateMethod(rippleEffectLayer, "UpdateToGradient", new SolidColorBrush(Color.FromArgb("#1C1B1F")));
			if(radialGradientBrush!=null)
			{
				Assert.Equal(0.5, radialGradientBrush.Center.X);
			}
		}

		[Fact]
		public void Test_OnFadeAnimationUpdate()
		{
			var effectsView = new SfEffectsView();
			var rippleEffectLayer = GetPrivateField(effectsView, "_rippleEffectLayer") as RippleEffectLayer;
			InvokePrivateMethod(rippleEffectLayer, "OnFadeAnimationUpdate", 30);
			var alphaValue = GetPrivateField(rippleEffectLayer, "_alphaValue");
			Assert.Equal((float)30, alphaValue);
		}

		[Fact]
		public void Test_OnRippleAnimationUpdate()
		{
			var effectsView = new SfEffectsView();
			var rippleEffectLayer = GetPrivateField(effectsView, "_rippleEffectLayer") as RippleEffectLayer;
			InvokePrivateMethod(rippleEffectLayer, "OnRippleAnimationUpdate", 20);
			var rippleDiameter = GetPrivateField(rippleEffectLayer, "_rippleDiameter");
			Assert.Equal((float)20, rippleDiameter);
		}

		#endregion
	}
}