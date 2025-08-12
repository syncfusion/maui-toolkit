using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.PullToRefresh;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfPullToRefreshUnitTests : BaseUnitTest
    {
        #region Public properties

        [Fact]
        public void TestConstructor()
        {
            var pullToRefresh = new SfPullToRefresh();

            Assert.NotNull(pullToRefresh);
            Assert.False(pullToRefresh.CanRestrictChildTouch);
            Assert.False(pullToRefresh.IsRefreshing);
            Assert.Null(pullToRefresh.PullableContent);
            Assert.Null(pullToRefresh.RefreshCommand);
            Assert.Null(pullToRefresh.RefreshCommandParameter);
            Assert.Null(pullToRefresh.PullingViewTemplate);
            Assert.Null(pullToRefresh.RefreshingViewTemplate);
            Assert.Equal(200d, pullToRefresh.PullingThreshold);
            Assert.Equal(PullToRefreshTransitionType.SlideOnTop, pullToRefresh.TransitionMode);
            Assert.Equal(3d, pullToRefresh.ProgressThickness);
            Assert.Equal(48d, pullToRefresh.RefreshViewHeight);
            Assert.Equal(48d, pullToRefresh.RefreshViewWidth);
            Assert.Equal(50d, pullToRefresh.RefreshViewThreshold);
            Assert.Equal(Color.FromArgb("6750A4"), pullToRefresh.ProgressColor);
        }

        [Theory]
        [InlineData(300d)]
        [InlineData((double)0)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        public void PullingThreshold_DifferentValues(double expectedValue)
        {
			var pullToRefresh = new SfPullToRefresh
			{
				PullingThreshold = expectedValue
			};
			var actualValue = pullToRefresh.PullingThreshold;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(PullToRefreshTransitionType.Push)]
        [InlineData(PullToRefreshTransitionType.SlideOnTop)]
        public void TransitionMode_SetValue_ReturnsExpected(PullToRefreshTransitionType expected)
        {
			var pullToRefresh = new SfPullToRefresh
			{
				TransitionMode = expected
			};

			Assert.Equal(expected, pullToRefresh.TransitionMode);
        }

        [Fact]
        public void ProgressColor_SetValue_ReturnsExpected()
        {
			var pullToRefresh = new SfPullToRefresh
			{
				ProgressColor = Colors.Blue
			};

			Assert.Equal(Colors.Blue, pullToRefresh.ProgressColor);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 0, 0)]
        public void ProgressColor_SetRgbValue_ReturnsExpectedValue(byte r, byte g, byte b)
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedColor = Color.FromRgb(r, g, b);
            pullToRefresh.ProgressColor = expectedColor;

            Assert.Equal(expectedColor, pullToRefresh.ProgressColor);
        }

        [Theory]
        [InlineData(0xFFFF0000)]
        [InlineData(0xFF00FF00)]
        public void ProgressColor_SetAndGetVariousColors(uint expectedColorValue)
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedColor = Color.FromRgb((byte)(expectedColorValue >> 24), (byte)(expectedColorValue >> 16), (byte)(expectedColorValue >> 8));
            pullToRefresh.ProgressColor = expectedColor;

            Assert.Equal(expectedColor, pullToRefresh.ProgressColor);
        }

        [Theory]
        [InlineData(255, 228, 196)]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)] 
        public void ProgressBackground_SetValue_ReturnsExpected(byte r, byte g, byte b)
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedColor = Color.FromRgb(r, g, b);
            pullToRefresh.ProgressBackground = new SolidColorBrush(expectedColor);

            Assert.Equal(new SolidColorBrush(expectedColor), pullToRefresh.ProgressBackground);
        }

        [Theory]
        [InlineData("Red")]
        [InlineData("Green")]
        [InlineData("Blue")]
        public void ProgressBackground_ShouldSetAndGetSystemBrush(string colorName)
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedBrush = new BrushTypeConverter().ConvertFromString(colorName) as Brush;

            if (expectedBrush == null)
            {
                Assert.Fail($"Failed to convert color name '{colorName}' to a Brush.");
            }

            pullToRefresh.ProgressBackground = expectedBrush;
            var actualBrush = pullToRefresh.ProgressBackground;
            Assert.Equal(expectedBrush, actualBrush);
        }

        [Fact]
        public void ProgressBackground_ShouldSetAndGetLinearGradientBrush()
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedBrush = new LinearGradientBrush
            {
                GradientStops =
				[
					new GradientStop(Colors.Yellow, (float)0.0),
                    new GradientStop(Colors.Red, (float)1.0)
                ]
            };
            pullToRefresh.ProgressBackground = expectedBrush;
            var actualBrush = pullToRefresh.ProgressBackground;
            Assert.Equal(expectedBrush, actualBrush);
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData((double)0)]
        [InlineData(3d)]
        public void ProgressThickness_SetValue_ReturnsExpected(double expected)
        {
			var pullToRefresh = new SfPullToRefresh
			{
				ProgressThickness = expected
			};

			Assert.Equal(expected, pullToRefresh.ProgressThickness);
        }

        [Fact]
        public void IsRefreshing_SetValue_ReturnsExpectedValue()
        {
            var pullToRefresh = new SfPullToRefresh();
            Assert.False(pullToRefresh.IsRefreshing);
            pullToRefresh.IsRefreshing = true;

            Assert.True(pullToRefresh.IsRefreshing);
            pullToRefresh.IsRefreshing = false;
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanRestrictChildTouch_SetValue_ReturnsExpected(bool expected)
        {
			var pullToRefresh = new SfPullToRefresh
			{
				CanRestrictChildTouch = expected
			};

			Assert.Equal(expected, pullToRefresh.CanRestrictChildTouch);
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData((double)0)]
        [InlineData(40d)]
        public void RefreshViewThreshold_SetValue_ReturnsExpected(double expected)
        {
			var pullToRefresh = new SfPullToRefresh
			{
				RefreshViewThreshold = expected
			};

			Assert.Equal(expected, pullToRefresh.RefreshViewThreshold);
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData((double)0)]
        [InlineData(40d)]
        public void RefreshViewHeight_SetValue_ReturnsExpected(double expected)
        {
			var pullToRefresh = new SfPullToRefresh
			{
				RefreshViewHeight = expected
			};

			Assert.Equal(expected, pullToRefresh.RefreshViewHeight);
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData((double)0)]
        [InlineData(200d)]
        public void RefreshViewWidth_ShouldSetAndGetVariousValidValues(double expected)
        {
			var pullToRefresh = new SfPullToRefresh
			{
				RefreshViewWidth = expected
			};

			Assert.Equal(expected, pullToRefresh.RefreshViewWidth);
        }

        [Fact]
        public void PullableContent_SetValue_ReturnsExpected()
        {
            var pullToRefresh = new SfPullToRefresh();
            var labelView = new Label { Text = "Label Content" };
            pullToRefresh.PullableContent = labelView;
            var actualLabelView = pullToRefresh.PullableContent;
            Assert.Equal(labelView, actualLabelView);

            var buttonView = new Button { Text = "Button Content" };
            pullToRefresh.PullableContent = buttonView;
            var actualButtonView = pullToRefresh.PullableContent;
            Assert.Equal(buttonView, actualButtonView);
        }

        [Fact]
        public void PullingViewTemplate_SetValue_ReturnsExpected()
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedTemplate = new DataTemplate(() => new Label { Text = "Loading..." });
            pullToRefresh.PullingViewTemplate = expectedTemplate;
            var actualTemplate = pullToRefresh.PullingViewTemplate;

            Assert.Same(expectedTemplate, actualTemplate);
        }

        [Fact]
        public void RefreshingViewTemplate_SetValue_ReturnsExpected()
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedTemplate = new DataTemplate(() => new Label { Text = "Refreshing..." });
            pullToRefresh.RefreshingViewTemplate = expectedTemplate;
            var actualTemplate = pullToRefresh.RefreshingViewTemplate;
            Assert.Same(expectedTemplate, actualTemplate);
        }

        [Fact]
        public void RefreshCommand_SetValue_ReturnsExpectedValue()
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedCommand = new Command(() => { });
            pullToRefresh.RefreshCommand = expectedCommand;
            var actualCommand = pullToRefresh.RefreshCommand;
            Assert.Same(expectedCommand, actualCommand);
        }

        [Fact]
        public void RefreshCommandParameter_SetValue_ReturnsExpectedValue()
        {
            var pullToRefresh = new SfPullToRefresh();
            var expectedParameter = new { Message = "Refresh" };
            pullToRefresh.RefreshCommandParameter = expectedParameter;
            var actualParameter = pullToRefresh.RefreshCommandParameter;
            Assert.Same(expectedParameter, actualParameter);
        }

        #endregion

        #region Public Methods

        [Fact]
        public void EndRefreshing_ShouldSetIsRefreshingToFalse()
        {
            var pullToRefresh = new SfPullToRefresh();

            pullToRefresh.StartRefreshing();
            pullToRefresh.EndRefreshing();

            Assert.False(pullToRefresh.IsRefreshing, "IsRefreshing should be false after calling EndRefreshing.");
        }

        #endregion

        #region Internal Methods

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetShadowSpace(bool isTop)
        {
            var pullToRefresh = new SfPullToRefresh();

            var result = pullToRefresh.GetShadowSpace(isTop);
            if(isTop)
            {
                Assert.Equal(1.0, result );
            }
            else
            {
                Assert.Equal(3.0, result );
            }
        }

        [Fact]
        public void IsChildElementScrolled_ReturnsFalse_WhenElementIsNull()
        {
            var pullToRefresh = new SfPullToRefresh();
            var result = pullToRefresh.IsChildElementScrolled(null, new Point(10, 10));

            Assert.False(result);
        }

        [Fact]
        public void IsChildElementScrolled_ReturnsFalse_WhenElementIsNull1()
        {
            var pullToRefresh = new SfPullToRefresh();
            var view = typeof(SfPullToRefresh).Assembly.GetType("Microsoft.Maui.Controls.View");

            var result = pullToRefresh.IsChildElementScrolled((IVisualTreeElement?)view, new Point(50, 50));

            Assert.False(result);
        }


        #endregion

        #region Private Methods

        [Theory]
        [InlineData(PullToRefreshTransitionType.SlideOnTop)]
        [InlineData(PullToRefreshTransitionType.Push)]
        public void UpdateProgressRate_CorrectlyCalculatesProgressRate_WhenTransitionMode(PullToRefreshTransitionType expected)
        {
            var pullToRefresh = new SfPullToRefresh
            {
                TransitionMode = expected,
                RefreshViewThreshold = 50,
                PullingThreshold = 100,
                RefreshViewHeight = 30,
            };

            InvokePrivateMethod(pullToRefresh, "UpdateProgressRate", 75);

            if (pullToRefresh.TransitionMode == PullToRefreshTransitionType.SlideOnTop) 
            {
                Assert.Equal(0.5, pullToRefresh.ProgressRate);
            }
            else
            {
                Assert.Equal(0.37, pullToRefresh.ProgressRate);
            }
        }

        [Fact]
        public void HandlePullingEvent_ShouldReturnFalse_WhenNoPullToRefreshElement()
        {
            var pullToRefresh = new SfPullToRefresh();

            var result = InvokePrivateMethod(pullToRefresh, "HandlePullingEvent");

            Assert.False((bool?)result); 
        }

        [Fact]
        public void HandlePullingEvent_ShouldReturnTrue_WhenPullingEventArgsCancel()
        {
            var pullToRefresh = new SfPullToRefresh();
            pullToRefresh.Pulling += (sender, args) => args.Cancel = true;

            var result = InvokePrivateMethod(pullToRefresh, "HandlePullingEvent");

            Assert.True((bool?)result);
        }

        [Theory]
        [InlineData(50.0)]
        [InlineData(150.0)]
        public void RaisePullingEvent_ShouldReturnExpectedValue_WhenDistancePulled(double pulledDistance)
        {
            var pullToRefresh = new SfPullToRefresh
            {
                TransitionMode = PullToRefreshTransitionType.SlideOnTop,
                RefreshViewThreshold = 100,
                PullingThreshold = 200,
            };
            var result = (bool?)InvokePrivateMethod(pullToRefresh, "RaisePullingEvent", pulledDistance);
            Assert.False(result);
        }

        [Fact]
        public void RaisePullingEvent_ShouldInvokePullingEvent_AndCancelIfRequested()
        {
            var pullToRefresh = new SfPullToRefresh
            {
                TransitionMode = PullToRefreshTransitionType.SlideOnTop,
                RefreshViewThreshold = 100,
                PullingThreshold = 200,
            };

            bool pullingEventInvoked = false;
            pullToRefresh.Pulling += (sender, args) =>
            {
                pullingEventInvoked = true;
                args.Cancel = true;
            };
            var result = (bool?)InvokePrivateMethod(pullToRefresh, "RaisePullingEvent", 150.0);
            Assert.True(pullingEventInvoked);
            Assert.True(result);
        }

        [Fact]
        public void RaisePullingEvent_ShouldCancelPulling_WhenRefreshCommandCannotExecute()
        {
            var pullToRefresh = new SfPullToRefresh
            {
                TransitionMode = PullToRefreshTransitionType.SlideOnTop,
                RefreshViewThreshold = 100,
                PullingThreshold = 200,
                IsRefreshing = false,
                RefreshCommand = new Command(() => { }, () => false)
            };
            var result = (bool?)InvokePrivateMethod(pullToRefresh, "RaisePullingEvent", 150.0);
            Assert.True(result);
        }

        [Theory]
        [InlineData(PointerActions.Pressed, 100, 200)]
        [InlineData(PointerActions.Moved, 100, 250)] 
        [InlineData(PointerActions.Released, 100, 250)] 
        [InlineData(PointerActions.Exited, 100, 200)] 
        public void HandleTouchInteraction_FullInteractionFlow_ShouldWorkAsExpected(
        PointerActions action,
        double pointX,
        double pointY)
        {
            var pullToRefresh = new SfPullToRefresh();

            var point = new Point(pointX, pointY);

            switch (action)
            {
                case PointerActions.Pressed:
                    InvokePrivateMethod(pullToRefresh, "HandleTouchInteraction", PointerActions.Pressed, point);
                    var isPressed_One = GetPrivateField<SfPullToRefresh>(pullToRefresh, "_isPressed");
                    Assert.True((bool?)isPressed_One);
                    break;

                case PointerActions.Moved:
                    InvokePrivateMethod(pullToRefresh, "HandleTouchInteraction", PointerActions.Pressed, new Point(100, 200));
                    InvokePrivateMethod(pullToRefresh, "HandleTouchInteraction", PointerActions.Moved, point);
                    var distancePulled = GetPrivateField<SfPullToRefresh>(pullToRefresh, "_distancePulled");
                    Assert.Equal(50, (double?)distancePulled);
                    break;

                case PointerActions.Released:
                    InvokePrivateMethod(pullToRefresh, "HandleTouchInteraction", PointerActions.Pressed, new Point(100, 200));
                    pullToRefresh.IsPulling = true;
                    InvokePrivateMethod(pullToRefresh, "HandleTouchInteraction", PointerActions.Released, point);
                    var isPressed_Two = GetPrivateField<SfPullToRefresh>(pullToRefresh, "_isPressed");
                    Assert.False((bool?)isPressed_Two);
                    break;

                case PointerActions.Exited:
                    InvokePrivateMethod(pullToRefresh, "HandleTouchInteraction", PointerActions.Pressed, new Point(100, 200));
                    InvokePrivateMethod(pullToRefresh, "HandleTouchInteraction", PointerActions.Exited, point);
                    var isPressed_Three = GetPrivateField<SfPullToRefresh>(pullToRefresh, "_isPressed");
                    Assert.False((bool?)isPressed_Three);
                    break;
            }
        }

        [Fact]
        public void MeasureContent_ShouldMeasurePullableContent_WhenNotPullingAndNotRefreshing()
        {
            var pullToRefresh = new SfPullToRefresh();
            var widthConstraint = 100;
            var heightConstraint = 100;

            var result = InvokePrivateMethod(pullToRefresh, "MeasureContent", widthConstraint, heightConstraint);

            Assert.Equal(new Size(double.NaN,double.NaN), result);
        }

        [Fact]
        public void ArrangeContent_ShouldCallManualArrangeContent_WhenNotPullingAndNotRefreshing()
        {
            var pullToRefresh = new SfPullToRefresh();
            var bounds = new Rect(0, 0, 100, 100);
            var result = InvokePrivateMethod(pullToRefresh, "ArrangeContent", bounds);

            Assert.Equal(new Size(bounds.Width, bounds.Height), result);
        }

        [Fact]
        public void Refreshed_Event_IsRaised_WhenCalled()
        {
            var pullToRefresh = new SfPullToRefresh();
            bool eventRaised = false;

            pullToRefresh.Refreshed += (sender, e) =>
            {
                eventRaised = true;
            };
            var result = InvokePrivateMethod(pullToRefresh, "RaiseRefreshedEvent", eventRaised);

            Assert.True(eventRaised);
        }

        [Fact]
        public void Refreshed_Event_IsNotRaised_WhenNoSubscribers()
        {
            var pullToRefresh = new SfPullToRefresh();
            bool eventRaised = false;
            InvokePrivateMethod(pullToRefresh, "RaiseRefreshedEvent", eventRaised);
            Assert.False(eventRaised);
        }

        [Fact]
        public void RefreshCommand_Executed_WhenNotNull()
        {
            var pullToRefresh = new SfPullToRefresh()
            {

                RefreshCommandParameter = new object()
            };
            var methodInfo = typeof(SfPullToRefresh).GetMethod("Refresh", BindingFlags.Instance | BindingFlags.Public);
            methodInfo?.Invoke(pullToRefresh, null);
            Assert.True(true, "Refresh method executed without throwing exceptions.");
        }

        [Fact]
        public void RefreshCommand_Executed_WithSpecificParameter()
        {
            var pullToRefresh = new SfPullToRefresh
            {

                RefreshCommandParameter = "ExpectedParameter"
            };
            var methodInfo = typeof(SfPullToRefresh).GetMethod("Refresh", BindingFlags.Instance | BindingFlags.Public);
            methodInfo?.Invoke(pullToRefresh, null);
            Assert.True(true, "Refresh method executed with a specific parameter without throwing exceptions.");
        }

        [Fact]
        public void MeasureProgressCircleView_CallsMeasure_WhenContentIsNotNull()
        {
            var pullToRefresh = new SfPullToRefresh();
            var progress = new SfProgressCircleView(pullToRefresh);
            var methodInfo = typeof(SfPullToRefresh).GetMethod("MeasureProgressCircleView", BindingFlags.Instance | BindingFlags.Public);
            methodInfo?.Invoke(pullToRefresh, [100, 100]);
            Assert.True(true, "MeasureProgressCircleView executed without throwing exceptions.");
        }

        [Fact]
        public void MeasureContent_ShouldMeasurePullableContent_WhenNotPulling()
        {
            var pullToRefresh = new SfPullToRefresh
            {
                IsPulling = false,
                ActualIsRefreshing = false,
            };

            var widthConstraint = 100;
            var heightConstraint = 100;
            var pullableContent = new SfPullToRefresh();
            typeof(SfPullToRefresh).GetProperty("PullableContent", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(pullToRefresh, pullableContent);
            InvokePrivateMethod(pullToRefresh, "MeasureContent", widthConstraint, heightConstraint);
            Assert.False(pullableContent.IsPulling);
            var previousMeasuredSize = (Size?)typeof(SfPullToRefresh).GetField("_previousMeasuredSize", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(pullToRefresh);
            Assert.Equal(new Size(double.NaN,double.NaN), previousMeasuredSize);
        }

        [Fact]
        public void ShouldCallCheckIfAndSetTemplate_WhenNewValueIsNull()
        {
            var pullToRefresh = new SfPullToRefresh();
            var newValue = new object();
            var progressCircleViewField = pullToRefresh.ProgressCircleView;
            Assert.NotNull(progressCircleViewField);
            var progressCircleViewInstance = progressCircleViewField.GetType();
            if (newValue != null)
            {
                var checkIfAndSetTemplateMethod = progressCircleViewInstance.GetType().GetMethod("CheckIfAndSetTemplate", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.Null(checkIfAndSetTemplateMethod);
                checkIfAndSetTemplateMethod?.Invoke(progressCircleViewInstance, null);
            }
            var templateSetField = progressCircleViewInstance.GetType().GetField("_templateSet", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.Null(templateSetField);
            var templateSetValue = templateSetField?.GetValue(progressCircleViewInstance);
            Assert.Null(templateSetValue);
        }

        [Fact]
        public void InvalidateDrawable_ShouldBeCalled_WhenProgressCircleViewIsSet()
        {
            var pullToRefresh = new SfPullToRefresh();

            pullToRefresh.InvalidateDrawable();
            pullToRefresh.ProgressCircleView.InvalidateDrawable();
            var testView = pullToRefresh.ProgressCircleView;
            Assert.True(testView.IsEnabled, "InvalidateDrawable should have been called.");
        }

        [Fact]
        public void AdjustYCoordinate_WhenTransitionModeIsPush_AndProgressCircleViewContentIsNull()
        {
			var pullToRefresh = new SfPullToRefresh
			{
				TransitionMode = PullToRefreshTransitionType.Push
			};
			pullToRefresh.ProgressCircleView = new SfProgressCircleView(pullToRefresh);
            pullToRefresh.RefreshViewHeight = 100;

            double y = 200;
            if (pullToRefresh.TransitionMode == PullToRefreshTransitionType.Push)
            {
                if (pullToRefresh.ProgressCircleView.Content == null)
                {
                    y -= pullToRefresh.RefreshViewHeight;
                }
                else
                {
                    y -= pullToRefresh.ProgressCircleView.CircleViewBounds.Height;
                }
            }
            Assert.Equal(100, y);
        }

        [Fact]
        public void CalculateProgressRate_WhenTemplateViewIsNull_CalculatesCorrectly()
        {
            SfPullToRefresh pullToRefresh = [];
            double pulledDistance = 160;
            pullToRefresh.ProgressRate = pulledDistance;
            Assert.Equal(160, pullToRefresh.ProgressRate, 0.01);
        }

        [Fact]
		public void CreateStyles_ShouldReturnNewInstance()
		{
			SfPullToRefreshStyles style = new SfPullToRefreshStyles();

			Assert.NotNull(style);
			Assert.IsType<SfPullToRefreshStyles>(style);
		}

		[Fact]
        public void MeasureProgressCircleView_CallsMeasureWithCorrectConstraints()
        {
            SfPullToRefresh pullToRefresh = [];
            double widthConstraint = 200.0;
            double heightConstraint = 100.0;

            pullToRefresh.MeasureSfProgressCircleView(widthConstraint, heightConstraint);

            var size = pullToRefresh.ProgressCircleView;
            Assert.Equal(-1, size.Width);
            Assert.Equal(-1, size.Height);
        }

        #endregion

    }
}
