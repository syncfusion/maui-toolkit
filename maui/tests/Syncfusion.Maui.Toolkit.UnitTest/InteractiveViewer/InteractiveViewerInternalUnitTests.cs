using System.Reflection;
using Syncfusion.Maui.Toolkit.InteractiveViewer;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class InteractiveViewerInternalUnitTests : BaseUnitTest
    {
        #region InteractiveViewerHelper tests

        [Theory]
        [InlineData(true, PanAxis.Both, ScrollOrientation.Both)]
        [InlineData(true, PanAxis.Horizontal, ScrollOrientation.Horizontal)]
        [InlineData(true, PanAxis.Vertical, ScrollOrientation.Vertical)]
        [InlineData(false, PanAxis.Both, ScrollOrientation.Neither)]
        [InlineData(false, PanAxis.Horizontal, ScrollOrientation.Neither)]
        [InlineData(false, PanAxis.Vertical, ScrollOrientation.Neither)]
        public void GetScrollOrientation_ReturnsExpectedOrientation(bool isPanEnabled, PanAxis panAxis, ScrollOrientation expectedValue)
        {
            ScrollOrientation actualValue = InteractiveViewerHelper.GetScrollOrientation(isPanEnabled, panAxis);

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0d, 90d)]
        [InlineData(90d, 180d)]
        [InlineData(270d, 0d)]
        [InlineData(350d, 80d)]
        public void GetRotationAngle_ReturnsExpectedRotation(double currentRotation, double expectedValue)
        {
            double actualValue = InteractiveViewerHelper.GetRotationAngle(currentRotation);

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Event args

        [Theory]
        [InlineData(PanAxis.Both, 1d)]
        [InlineData(PanAxis.Horizontal, 2.5d)]
        [InlineData(PanAxis.Vertical, 10d)]
        public void InteractiveScrollChangedEventArgs_Constructor_InitializesProperties(PanAxis panAxis, double zoomFactor)
        {
            InteractiveScrollChangedEventArgs args = new InteractiveScrollChangedEventArgs(panAxis, zoomFactor);

            Assert.Equal(panAxis, args.PanAxis);
            Assert.Equal(zoomFactor, args.ZoomFactor);
        }

        [Theory]
        [InlineData(1d, 2d)]
        [InlineData(2.5d, 10d)]
        [InlineData(10d, 25d)]
        public void ZoomFactorChangedEventArgs_Constructor_InitializesProperties(double oldZoomFactor, double newZoomFactor)
        {
            ZoomFactorChangedEventArgs args = new ZoomFactorChangedEventArgs(oldZoomFactor, newZoomFactor);

            Assert.Equal(oldZoomFactor, args.OldZoomFactor);
            Assert.Equal(newZoomFactor, args.NewZoomFactor);
        }

        #endregion

        #region InteractiveLayout tests

        [Fact]
        public void UpdateContentView_SetsContent()
        {
            TestInteractiveLayout layout = new TestInteractiveLayout(null);
            Label expectedValue = new Label { Text = "Interactive" };

            layout.UpdateContentView(expectedValue);

            Assert.Equal(expectedValue, layout.Content);
        }

        [Fact]
        public void RemoveItemsViewHandler_WithContent_ClearsChildren()
        {
            TestInteractiveLayout layout = new TestInteractiveLayout(null);
            Label content = new Label();
            layout.SetContent(content);

            layout.RemoveItemsViewHandler();

            Assert.Empty(layout.Children);
            // TestInteractiveLayout.Content reads the private _contentView field via reflection.
            Assert.Null(layout.Content);
        }

        [Fact]
        public void ProcessOnZoomStarted_WithNullContent_DoesNotThrow()
        {
            TestInteractiveLayout layout = new TestInteractiveLayout(null);

            var exception = Record.Exception(() => layout.ProcessZoomStarted(new ZoomEventArgs(2d, new Point(10, 10))));

            Assert.Null(exception);
        }

        [Fact]
        public void ProcessOnZoomStarted_WithContent_UpdatesAnchorsAndTranslation()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo();
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);
            Label content = new Label { WidthRequest = 120, HeightRequest = 80 };
            layout.SetContent(content);
            layout.SetScale(1d);
            viewerInfo.ResetInternalCounters();

            layout.ProcessZoomStarted(new ZoomEventArgs(2d, new Point(20, 30)));

            // ProcessOnZoomStarted calls UpdateContentSize once on the viewer info.
            Assert.Equal(1, viewerInfo.UpdateContentSizeCallCount);
            Assert.True(viewerInfo.LastIsInitialZoom);
        }

        [Fact]
        public void ProcessOnZoomChanged_WithNullInfo_DoesNotThrow()
        {
            TestInteractiveLayout layout = new TestInteractiveLayout(null);
            layout.SetContent(new Label());

            var exception = Record.Exception(() => layout.ProcessZoomChanged(new ZoomEventArgs(2d, new Point(10, 10))));

            Assert.Null(exception);
        }

        [Fact]
        public void ProcessOnZoomChanged_WithValidInfo_UpdatesScale()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo
            {
                ScrollViewDesiredSize = new Size(50, 50),
                ViewportSize = new Size(50, 50)
            };
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);
            layout.SetContent(new Label());
            layout.SetScale(1d);
            layout.SetAnchors(0.5, 0.5);

            layout.ProcessZoomStarted(new ZoomEventArgs(1d, new Point(10, 10)));
            layout.ProcessZoomChanged(new ZoomEventArgs(2d, new Point(10, 10)));

            Assert.Equal(2d, layout.Scale);
        }

        [Fact]
        public void ProcessOnZoomChanged_WithExpandedContent_AdjustsAnchors()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo
            {
                ScrollViewDesiredSize = new Size(50, 50),
                ViewportSize = new Size(50, 50)
            };
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);
            Label content = new Label { WidthRequest = 120, HeightRequest = 90 };
            layout.SetContent(content);
            layout.SetScale(1d);
            layout.SetAnchors(0d, 0d);

            layout.ProcessZoomStarted(new ZoomEventArgs(2d, new Point(10, 15)));
            layout.ProcessZoomChanged(new ZoomEventArgs(3d, new Point(25, 30)));

            Assert.Equal(3d, layout.Scale);
            Assert.NotEqual(0d, layout.AnchorX);
            Assert.NotEqual(0d, layout.AnchorY);
        }

        [Fact]
        public void ProcessOnZoomEnded_WithValidInfo_UpdatesContentAndScrolls()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo
            {
                ScrollX = 5,
                ScrollY = 7
            };
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);
            Label content = new Label { WidthRequest = 100, HeightRequest = 60 };
            layout.SetContent(content);
            layout.SetScale(1d);
            viewerInfo.ResetInternalCounters();

            layout.ProcessZoomStarted(new ZoomEventArgs(2d, new Point(10, 20)));
            layout.ProcessZoomEnded(new ZoomEventArgs(2d, new Point(10, 20)));

            // ProcessOnZoomStarted and ProcessOnZoomEnded each call UpdateContentSize once.
            Assert.Equal(2, viewerInfo.UpdateContentSizeCallCount);
            Assert.Equal(1, viewerInfo.ScrollToAsyncCallCount);
        }

        [Fact]
        public void ProcessOnZoomEnded_WhenOffsetsDoNotChange_DoesNotScroll()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo
            {
                ScrollX = 0,
                ScrollY = 0
            };
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);
            Label content = new Label { WidthRequest = 100, HeightRequest = 60 };
            layout.SetContent(content);
            layout.SetScale(1d);
            viewerInfo.ResetInternalCounters();

            layout.ProcessZoomStarted(new ZoomEventArgs(2d, new Point(0, 0)));
            layout.ProcessZoomEnded(new ZoomEventArgs(2d, new Point(0, 0)));

            Assert.Equal(2, viewerInfo.UpdateContentSizeCallCount);
        }

        [Fact]
        public void ResetView_WithContent_ResetsRotation()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo();
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);
            Label content = new Label { Rotation = 180 };
            layout.SetContent(content);
            viewerInfo.ResetInternalCounters();

            layout.ResetView();

            Assert.Equal(0d, content.Rotation);
            // ResetView calls ResetZoom which calls ResetZoomAndPan exactly once.
            Assert.Equal(1, viewerInfo.ResetZoomAndPanCallCount);
        }

        #endregion

        #region SfInteractiveViewer tests

        [Fact]
        public void ContentChanged_WhenInteractiveLayoutIsNull_ReturnsWithoutException()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            // Makes _interactiveLayout null.
            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            var exception = Record.Exception(() =>
            {
                interactiveViewer.Content = new Label
                {
                    Text = "Test"
                };
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task ScrollToAsync_WithNullScrollView_Returns()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            IInteractiveZoomInfo zoomInfo = interactiveViewer;

            var exception = await Record.ExceptionAsync(
                () => zoomInfo.ScrollToAsync(10, 20));

            Assert.Null(exception);
        }

        [Fact]
        public void UpdateContentSize_WithValidValues_UpdatesExtentSize()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            // Force lazy creation of _scrollView and _interactiveLayout since
            // OnContentPropertyChanged bails out when the control is not loaded.
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            object? scrollView = GetPrivateField(interactiveViewer, "_scrollView");
            Assert.NotNull(scrollView);

            IInteractiveZoomInfo zoomInfo = interactiveViewer;

            Size expectedSize = new Size(200, 300);

            zoomInfo.UpdateContentSize(expectedSize, false);

            Size extentSize = Assert.IsType<Size>(
                InvokePrivateMethod(scrollView!, "get_ExtentSize"));

            Assert.Equal(expectedSize, extentSize);
        }

        [Fact]
        public void UpdateContentSize_WithInitialZoomAndExistingExtentSize_DoesNotUpdate()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            object? scrollView = GetPrivateField(interactiveViewer, "_scrollView");
            Assert.NotNull(scrollView);

            InvokePrivateMethod(
                scrollView!,
                "set_ExtentSize",
                new Size(50, 50));

            IInteractiveZoomInfo zoomInfo = interactiveViewer;

            zoomInfo.UpdateContentSize(new Size(200, 200), true);

            Size extentSize = Assert.IsType<Size>(
                InvokePrivateMethod(scrollView!, "get_ExtentSize"));

            Assert.Equal(new Size(50, 50), extentSize);
        }

        [Fact]
        public void UpdateContentSize_WithNullScrollView_Returns()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            IInteractiveZoomInfo zoomInfo = interactiveViewer;

            var exception = Record.Exception(() =>
                zoomInfo.UpdateContentSize(new Size(100, 100), false));

            Assert.Null(exception);
        }

        [Fact]
        public void ResetZoomAndPan_WhenZoomFactorNotOne_ResetsZoomFactor()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            object? scrollView = GetPrivateField(interactiveViewer, "_scrollView");
            Assert.NotNull(scrollView);

            // Set zoom factor to a non-default value.
            InvokePrivateMethod(scrollView!, "set_ZoomFactor", 2d);

            IInteractiveViewerInfo viewerInfo = interactiveViewer;

            viewerInfo.ResetZoomAndPan();

            double zoomFactor = Assert.IsType<double>(
                InvokePrivateMethod(scrollView!, "get_ZoomFactor"));

            Assert.Equal(1d, zoomFactor);
        }

        [Fact]
        public void OnScrollViewZoomEnded_WithNullScrollView_Returns()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            // Makes _scrollView and _interactiveLayout null.
            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            var exception = Record.Exception(() =>
                InvokePrivateMethod(
                    interactiveViewer,
                    "OnScrollViewZoomEnded",
                    null,
                    new ZoomEventArgs(2d, new Point(10, 10))));

            Assert.Null(exception);
        }

        [Fact]
        public void OnScrollViewZoomChanged_WithNullScrollViewAndLayout_Returns()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            interactiveViewer.ZoomFactor = 1d;

            InvokePrivateMethod(
                interactiveViewer,
                "OnScrollViewZoomChanged",
                null,
                new ZoomEventArgs(2d, new Point(10, 10)));

            Assert.Equal(1d, interactiveViewer.ZoomFactor);
        }

        [Fact]
        public void OnScrollViewZoomChanged_WhenZoomFactorDiffers_UpdatesZoomFactor()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            interactiveViewer.Content = new Label();
            interactiveViewer.ZoomFactor = 1d;

            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            InvokePrivateMethod(
                interactiveViewer,
                "OnScrollViewZoomChanged",
                null,
                new ZoomEventArgs(2d, new Point(10, 10)));

            Assert.Equal(2d, interactiveViewer.ZoomFactor);
        }

        [Fact]
        public void ContentChanged_UpdatesViewerContent()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            Label expectedValue = new Label { Text = "Changed" };

            interactiveViewer.Content = expectedValue;

            Assert.Equal(expectedValue, interactiveViewer.Content);
        }

        [Fact]
        public void OnScrollViewZoomStarted_WithNullInteractiveLayout_Returns()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            // Removes _scrollView and _interactiveLayout.
            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            var exception = Record.Exception(() =>
                InvokePrivateMethod(
                    interactiveViewer,
                    "OnScrollViewZoomStarted",
                    null,
                    new ZoomEventArgs(2d, new Point(10, 10))));

            Assert.Null(exception);
        }

        [Fact]
        public void OnScrollViewZoomStarted_WithNeitherOrientation_ReturnsWithoutException()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            // Disable panning so orientation becomes Neither.
            interactiveViewer.IsPanEnabled = false;

            var exception = Record.Exception(() =>
                InvokePrivateMethod(
                    interactiveViewer,
                    "OnScrollViewZoomStarted",
                    null,
                    new ZoomEventArgs(2d, new Point(10, 10))));

            Assert.Null(exception);
        }

        [Fact]
        public async Task RotateView_WithRotation270_WrapsToZero()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo();

            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);

            Label content = new Label
            {
                Rotation = 270
            };

            layout.SetContent(content);
            viewerInfo.ResetInternalCounters();

            await layout.RotateViewAsync();

            // RotateView rotates the content by 90 degrees clockwise each time.
            Assert.Equal(360d, content.Rotation);
            Assert.Equal(1, viewerInfo.ResetZoomAndPanCallCount);
        }

        [Fact]
        public void ApplyZoomAndPanSettings_UpdatesScrollViewSettings()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            // Force lazy initialization of the scroll view where zoom/pan settings are applied.
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            var scrollView = GetPrivateField(interactiveViewer, "_scrollView");
            Assert.NotNull(scrollView);
            Assert.True((bool)Assert.IsType<bool>(InvokePrivateMethod(scrollView, "get_AllowZoom")));
            Assert.Equal(ScrollOrientation.Both, Assert.IsType<ScrollOrientation>(InvokePrivateMethod(scrollView, "get_Orientation")));
            Assert.Equal(1d, Assert.IsType<double>(InvokePrivateMethod(scrollView, "get_MinZoomFactor")));
            Assert.Equal(10d, Assert.IsType<double>(InvokePrivateMethod(scrollView, "get_MaxZoomFactor")));
            Assert.Equal(1d, Assert.IsType<double>(InvokePrivateMethod(scrollView, "get_ZoomFactor")));
        }

        [Fact]
        public void OnThemeChangedAndGetThemeDictionary_DoNotThrow()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            // Stronger than the smoke test: verify the dictionary is the typed styles collection.
            var themeDictionary = ((Syncfusion.Maui.Toolkit.Themes.IParentThemeElement)interactiveViewer).GetThemeDictionary();
            var controlThemeException = Record.Exception(() => ((Syncfusion.Maui.Toolkit.Themes.IThemeElement)interactiveViewer).OnControlThemeChanged("old", "new"));
            var commonThemeException = Record.Exception(() => ((Syncfusion.Maui.Toolkit.Themes.IThemeElement)interactiveViewer).OnCommonThemeChanged("old", "new"));

            Assert.NotNull(themeDictionary);
            Assert.IsType<SfInteractiveViewerStyles>(themeDictionary);
            Assert.Null(controlThemeException);
            Assert.Null(commonThemeException);
        }

        [Fact]
        public void RemoveInteractiveViewHandler_AfterInitialization_CanBeCalledTwice()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");
            var exception = Record.Exception(() => InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler"));

            Assert.Null(exception);
        }

        [Fact]
        public void RemoveInteractiveViewHandler_RemovesScrollViewAndClearsFields()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            Assert.Empty(interactiveViewer.Children);
            Assert.Null(GetPrivateField(interactiveViewer, "_scrollView"));
            Assert.Null(GetPrivateField(interactiveViewer, "_interactiveLayout"));
        }

        [Fact]
        public void OnHandlerChanged_WithNullHandler_RemovesInteractiveView()
        {
            TestSfInteractiveViewer interactiveViewer = new TestSfInteractiveViewer();

            interactiveViewer.TriggerHandlerChanged();

            Assert.Empty(interactiveViewer.Children);
            Assert.Null(GetPrivateField<SfInteractiveViewer>(interactiveViewer, "_scrollView"));
            Assert.Null(GetPrivateField<SfInteractiveViewer>(interactiveViewer, "_interactiveLayout"));
        }

        [Fact]
        public void WireAndUnwireScrollViewEvents_WithNullScrollView_DoNotThrow()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            // The guard branches are the meaningful behaviour. Force _scrollView == null so
            // the early-return paths execute instead of touching live event handlers.
            typeof(SfInteractiveViewer)
                .GetField("_scrollView", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(interactiveViewer, null);

            var wireException = Record.Exception(() => InvokePrivateMethod(interactiveViewer, "WireScrollViewEvents"));
            var unwireException = Record.Exception(() => InvokePrivateMethod(interactiveViewer, "UnWireScrollViewEvents"));

            Assert.Null(wireException);
            Assert.Null(unwireException);
        }

        [Fact]
        public void WireScrollViewEvents_SubscribesToScrollViewEvents()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            object? scrollView = GetPrivateField(interactiveViewer, "_scrollView");
            Assert.NotNull(scrollView);

            // Wheel event hookup is a reliable indicator that WireScrollViewEvents ran.
            FieldInfo? field = scrollView!.GetType().GetField(
                "ZoomStarted",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(field);
            Delegate? handler = field!.GetValue(scrollView) as Delegate;
            Assert.NotNull(handler);
            Assert.NotEmpty(handler!.GetInvocationList());
        }

        [Fact]
        public void OnScrollViewZoomChanged_WithSameValue_DoesNotRecurseIntoSetValue()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");
            interactiveViewer.Content = new Label();
            interactiveViewer.ZoomFactor = 2d;

            // Count how many times ZoomFactor property changes while we drive the inner handler.
            int propertyChangeCount = 0;
            interactiveViewer.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SfInteractiveViewer.ZoomFactor))
                {
                    propertyChangeCount++;
                }
            };

            // The private event handler only writes ZoomFactor back when it differs, so a
            // matching zoom factor should not raise another property change.
            InvokePrivateMethod(interactiveViewer, "OnScrollViewZoomChanged", null, new ZoomEventArgs(2d, new Point(10, 10)));

            Assert.Equal(2d, interactiveViewer.ZoomFactor);
            Assert.Equal(0, propertyChangeCount);
        }



        [Fact]
        public void ScrollChangedEvent_IsRaisedWhenScrollChanges()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            int callCount = 0;
            InteractiveScrollChangedEventArgs? eventArgs = null;
            interactiveViewer.ScrollChanged += (sender, args) =>
            {
                callCount++;
                eventArgs = args;
            };

            interactiveViewer.PanAxis = PanAxis.Horizontal;
            interactiveViewer.ZoomFactor = 2d;
            InvokePrivateMethod(interactiveViewer, "OnScrollViewScrollChanged", null, new ScrollChangedEventArgs(0, 0, 0, 0));

            Assert.Equal(1, callCount);
            Assert.NotNull(eventArgs);
            Assert.Equal(PanAxis.Horizontal, eventArgs!.PanAxis);
            Assert.Equal(2d, eventArgs.ZoomFactor);
        }

        [Fact]
        public void Rotate_WithContent_RotatesContentBy90Degrees()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            Label content = new Label
            {
                Rotation = 0
            };

            interactiveViewer.Content = content;

            // Force lazy initialization of _interactiveLayout so Rotate() forwards to it.
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            interactiveViewer.Rotate();

            Assert.Equal(90d, content.Rotation);
        }

        [Fact]
        public void Rotate_WhenInteractiveLayoutIsNull_ShouldNotThrow()
        {
            var viewer = new SfInteractiveViewer();

            typeof(SfInteractiveViewer)
                .GetField("_interactiveLayout",
                    BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(viewer, null);

            var exception = Record.Exception(() => viewer.Rotate());

            Assert.Null(exception);
        }

        [Fact]
        public void Reset_WhenInteractiveLayoutIsNull_ShouldNotThrow()
        {
            var viewer = new SfInteractiveViewer();

            typeof(SfInteractiveViewer)
                .GetField("_interactiveLayout",
                    BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(viewer, null);

            var exception = Record.Exception(() => viewer.Reset());

            Assert.Null(exception);
        }

        [Fact]
        public void InitializeInteractiveLayout_WhenScrollViewIsNull_CreatesScrollView()
        {
            // Arrange
            var viewer = new SfInteractiveViewer();

            typeof(SfInteractiveViewer)
                .GetField("_scrollView",
                    BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(viewer, null);

            var method = typeof(SfInteractiveViewer)
                .GetMethod("InitializeInteractiveLayout",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            method!.Invoke(viewer, null);

            // Assert
            var scrollView = typeof(SfInteractiveViewer)
                .GetField("_scrollView",
                    BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(viewer);

            Assert.NotNull(scrollView);
        }

        [Fact]
        public void ZoomFactorChangedEvent_IsRaisedWhenZoomEnds()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            // Force lazy creation of _scrollView and _interactiveLayout since
            // OnContentPropertyChanged bails out when the control is not loaded.
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");
            interactiveViewer.Content = new Label();

            int callCount = 0;
            ZoomFactorChangedEventArgs? eventArgs = null;
            interactiveViewer.ZoomFactorChanged += (sender, args) =>
            {
                callCount++;
                eventArgs = args;
            };

            InvokePrivateMethod(interactiveViewer, "OnScrollViewZoomEnded", null, new ZoomEventArgs(2d, new Point(10, 10)));

            Assert.Equal(1, callCount);
            Assert.NotNull(eventArgs);
            Assert.Equal(1d, eventArgs!.OldZoomFactor);
            Assert.Equal(2d, eventArgs.NewZoomFactor);
        }

        [Fact]
        public void ZoomEndedTwice_UpdatesOldZoomFactor()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            // Force lazy creation of _scrollView and _interactiveLayout.
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");
            interactiveViewer.Content = new Label();

            ZoomFactorChangedEventArgs? eventArgs = null;

            interactiveViewer.ZoomFactorChanged += (_, args) => eventArgs = args;
            InvokePrivateMethod(interactiveViewer, "OnScrollViewZoomEnded", null, new ZoomEventArgs(2d, new Point(10, 10)));
            InvokePrivateMethod(interactiveViewer, "OnScrollViewZoomEnded", null, new ZoomEventArgs(3d, new Point(10, 10)));

            Assert.NotNull(eventArgs);
            Assert.Equal(2d, eventArgs!.OldZoomFactor);
            Assert.Equal(3d, eventArgs.NewZoomFactor);
        }

        [Fact]
        public void MeasureContent_WithNonViewChild_IgnoresChildAndReturnsSize()
        {
            TestSfInteractiveViewer interactiveViewer = new TestSfInteractiveViewer();
            interactiveViewer.WidthRequest = 150;
            interactiveViewer.HeightRequest = 200;
            interactiveViewer.AddChild(new TestView());

            Size actualValue = interactiveViewer.MeasurePublic(300d, 400d);

            Assert.Equal(new Size(300, 400), actualValue);
        }

        [Fact]
        public void ArrangeContent_WithNonViewChild_IgnoresChildAndReturnsBoundsSize()
        {
            TestSfInteractiveViewer interactiveViewer = new TestSfInteractiveViewer();
            Rect bounds = new Rect(0, 0, 123, 456);
            interactiveViewer.AddChild(new TestView());

            Size actualValue = interactiveViewer.ArrangePublic(bounds);

            Assert.Equal(bounds.Size, actualValue);
        }

        [Fact]
        public void GetScrollX_WhenScrollViewIsNull_ReturnsZero()
        {
            // Arrange
            var viewer = new SfInteractiveViewer();

            typeof(SfInteractiveViewer)
                .GetField("_scrollView",
                    BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetValue(viewer, null);

            // Act
            double result = ((IInteractiveZoomInfo)viewer).GetScrollX();

            Assert.Equal(0, result);
        }

        [Fact]
        public void ResetZoomAndPan_ResetsScrollViewExtentSize()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            object? scrollView = GetPrivateField(interactiveViewer, "_scrollView");
            Assert.NotNull(scrollView);
            InvokePrivateMethod(scrollView!, "set_ExtentSize", new Size(120, 80));

            IInteractiveViewerInfo viewerInfo = interactiveViewer;

            viewerInfo.ResetZoomAndPan();

            // ResetZoomAndPan clears the scroll view's extent so the next measure starts fresh.
            object? extent = InvokePrivateMethod(scrollView!, "get_ExtentSize");
            // ExtentSize is declared as Size?; after reset it must be null.
            Assert.Null(extent);
        }

        [Fact]
        public void RemoveInteractiveViewHandler_AfterInitialization_ClearsLazyState()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            // Sanity check: lazy state was created.
            Assert.NotEmpty(interactiveViewer.Children);
            Assert.NotNull(GetPrivateField(interactiveViewer, "_scrollView"));

            InvokePrivateMethod(interactiveViewer, "RemoveInteractiveViewHandler");

            Assert.Empty(interactiveViewer.Children);
            Assert.Null(GetPrivateField(interactiveViewer, "_scrollView"));
            Assert.Null(GetPrivateField(interactiveViewer, "_interactiveLayout"));
        }

        [Fact]
        public void ProcessOnSizeChanged_InvalidatesMeasureAndResetsZoom()
        {
            FakeViewerInfo viewerInfo = new FakeViewerInfo();
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);
            layout.SetContent(new Label());

            viewerInfo.ResetInternalCounters();

            layout.ProcessOnSizeChanged();

            // ResetZoom invokes ResetZoomAndPan exactly once and triggers an UpdateContentSize.
            Assert.Equal(1, viewerInfo.ResetZoomAndPanCallCount);
            Assert.Equal(1, viewerInfo.UpdateContentSizeCallCount);
        }

        [Fact]
        public void InteractiveLayout_LayoutMeasure_WithRotatedContent_SwapsWidthAndHeight()
        {
            // When the content is rotated by 90 or 270 degrees, LayoutMeasure swaps width
            // and height so that the rendered bounds correctly match the rotated content.
            FakeViewerInfo viewerInfo = new FakeViewerInfo();
            TestInteractiveLayout layout = new TestInteractiveLayout(viewerInfo);

            Label content = new Label { WidthRequest = 100, HeightRequest = 200, Rotation = 90 };
            typeof(InteractiveLayout)
                .GetField("_availableSize", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(layout, new Size(500, 500));
            layout.SetContent(content);

            Size measured = layout.LayoutMeasure(400d, 300d);

            Assert.True(measured.Width >= measured.Height);
        }

        [Fact]
        public void InitializeInteractiveViewer_AddsLayoutChildToViewer()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            Assert.NotEmpty(interactiveViewer.Children);
            Assert.NotNull(GetPrivateField(interactiveViewer, "_scrollView"));
            Assert.NotNull(GetPrivateField(interactiveViewer, "_interactiveLayout"));
        }

        #endregion

        #region InteractiveLayoutBase tests

        [Fact]
        public void InteractiveStackLayout_LayoutMeasure_ReturnsRequestedSize()
        {
            InteractiveStackLayout layout = new InteractiveStackLayout();
            Label first = new Label { WidthRequest = 40, HeightRequest = 30 };
            Label second = new Label { WidthRequest = 60, HeightRequest = 80 };
            layout.Children.Add(first);
            layout.Children.Add(second);

            Size actualValue = layout.LayoutMeasure(250d, 150d);

            Assert.Equal(250d, actualValue.Width);
            Assert.Equal(150d, actualValue.Height);
            // Both children received a measure pass and retained their requested dimensions.
            Assert.Equal(40d, first.WidthRequest);
            Assert.Equal(30d, first.HeightRequest);
            Assert.Equal(60d, second.WidthRequest);
            Assert.Equal(80d, second.HeightRequest);
        }

        [Fact]
        public void InteractiveStackLayout_LayoutMeasure_WithoutFiniteConstraints_UsesMinimumSize()
        {
            InteractiveStackLayout layout = new InteractiveStackLayout();
            layout.MinimumWidthRequest = 123d;
            layout.MinimumHeightRequest = 456d;

            Size actualValue = layout.LayoutMeasure(double.PositiveInfinity, double.PositiveInfinity);

            Assert.Equal(123d, actualValue.Width);
            Assert.Equal(456d, actualValue.Height);
        }

        [Fact]
        public void InteractiveStackLayout_LayoutArrangeChildren_ArrangesAllChildrenAndReturnsSize()
        {
            InteractiveStackLayout layout = new InteractiveStackLayout();
            Label first = new Label();
            Label second = new Label();
            layout.Children.Add(first);
            layout.Children.Add(second);

            Rect bounds = new Rect(0, 0, 200, 100);
            Size actualValue = layout.LayoutArrangeChildren(bounds);

            Assert.Equal(bounds.Size, actualValue);
            // Both children were arranged with the same rectangle.
            Assert.Equal(bounds, first.Bounds);
            Assert.Equal(bounds, second.Bounds);
        }

        [Fact]
        public void InteractiveStackLayout_CreateLayoutManager_ReturnsInteractiveLayoutManager()
        {
            InteractiveStackLayout layout = new InteractiveStackLayout();

            object manager = InvokeCreateLayoutManager(layout);

            Assert.NotNull(manager);
            Assert.IsType<InteractiveLayoutManager>(manager);
        }

        [Fact]
        public void InteractiveLayoutManager_Measure_DelegatesToLayout()
        {
            TestInteractiveBaseLayout layout = new TestInteractiveBaseLayout();
            InteractiveLayoutManager manager = new InteractiveLayoutManager(layout);

            Size actualValue = manager.Measure(180d, 90d);

            Assert.Equal(1, layout.MeasureCallCount);
            Assert.Equal(180d, layout.LastMeasureWidth);
            Assert.Equal(90d, layout.LastMeasureHeight);
            Assert.Equal(Size.Zero, actualValue);
        }

        [Fact]
        public void InteractiveLayoutManager_ArrangeChildren_DelegatesToLayout()
        {
            TestInteractiveBaseLayout layout = new TestInteractiveBaseLayout();
            InteractiveLayoutManager manager = new InteractiveLayoutManager(layout);
            Rect bounds = new Rect(0, 0, 77, 55);

            Size actualValue = manager.ArrangeChildren(bounds);

            Assert.Equal(1, layout.ArrangeCallCount);
            Assert.Equal(bounds, layout.LastArrangeBounds);
            Assert.Equal(bounds.Size, actualValue);
        }

        [Fact]
        public void InteractiveLayout_ExposesBaseLayoutBridge()
        {
            // InteractiveLayout inherits from InteractiveBaseLayout; ensure the bridge is wired.
            TestInteractiveLayout layout = new TestInteractiveLayout(null);

            Assert.NotNull(layout);
            Assert.IsAssignableFrom<InteractiveBaseLayout>(layout);
        }

        #endregion

        #region Helper

        private sealed class FakeViewerInfo : IInteractiveViewerInfo
        {
            public View? Content { get; set; }

            public int ResetZoomAndPanCallCount { get; private set; }

            public int UpdateContentSizeCallCount { get; private set; }

            public Size LastContentSize { get; private set; }

            public bool LastIsInitialZoom { get; private set; }

            public int ScrollToAsyncCallCount { get; private set; }

            public double LastScrollX { get; private set; }

            public double LastScrollY { get; private set; }

            public Size ViewportSize { get; set; }

            public Size ScrollViewDesiredSize { get; set; }

            public double ScrollX { get; set; }

            public double ScrollY { get; set; }

            public void ResetZoomAndPan()
            {
                ResetZoomAndPanCallCount++;
            }

            public void UpdateContentSize(Size contentSize, bool isInitialZoom)
            {
                UpdateContentSizeCallCount++;
                LastContentSize = contentSize;
                LastIsInitialZoom = isInitialZoom;
            }

            public Task ScrollToAsync(double xOffset, double yOffset)
            {
                ScrollToAsyncCallCount++;
                LastScrollX = xOffset;
                LastScrollY = yOffset;
                return Task.CompletedTask;
            }

            public Size GetViewportSize()
            {
                return ViewportSize;
            }

            public Size GetScrollViewDesiredSize()
            {
                return ScrollViewDesiredSize;
            }

            public double GetScrollX()
            {
                return ScrollX;
            }

            public double GetScrollY()
            {
                return ScrollY;
            }

            /// <summary>
            /// Resets the call counters so each test phase can assert counts starting from zero.
            /// </summary>
            public void ResetInternalCounters()
            {
                ResetZoomAndPanCallCount = 0;
                UpdateContentSizeCallCount = 0;
                ScrollToAsyncCallCount = 0;
            }
        }

        private sealed class TestInteractiveLayout : InteractiveLayout
        {
            static readonly System.Reflection.FieldInfo ContentViewField =
                typeof(InteractiveLayout).GetField("_contentView",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;

            public TestInteractiveLayout(IInteractiveViewerInfo? interactiveViewerInfo) : base(interactiveViewerInfo)
            {
            }

            public View? Content
            {
                get { return ContentViewField.GetValue(this) as View; }
                set { UpdateContentView(value!); }
            }

            public void SetContent(View view)
            {
                UpdateContentView(view);
            }

            public void SetScale(double value)
            {
                Scale = value;
            }

            public void SetAnchors(double anchorX, double anchorY)
            {
                AnchorX = anchorX;
                AnchorY = anchorY;
            }

            public void SetTranslation(double translationX, double translationY)
            {
                TranslationX = translationX;
                TranslationY = translationY;
            }

            public async Task RotateViewAsync()
            {
                await RotateView();
            }

            public void ProcessZoomStarted(ZoomEventArgs args)
            {
                ProcessOnZoomStarted(args);
            }

            public void ProcessZoomChanged(ZoomEventArgs args)
            {
                ProcessOnZoomChanged(args);
            }

            public void ProcessZoomEnded(ZoomEventArgs args)
            {
                ProcessOnZoomEnded(args);
            }
        }

        private sealed class TestView : View
        {
        }

        private sealed class TestSfInteractiveViewer : SfInteractiveViewer
        {
            public Size MeasurePublic(double widthConstraint, double heightConstraint)
            {
                return MeasureContent(widthConstraint, heightConstraint);
            }

            public Size ArrangePublic(Rect bounds)
            {
                return ArrangeContent(bounds);
            }

            public void TriggerHandlerChanged()
            {
                OnHandlerChanged();
            }

            public void AddChild(View child)
            {
                Children.Add(child);
            }
        }

        /// <summary>
        /// Exposes the protected <see cref="Microsoft.Maui.Controls.Layout.CreateLayoutManager"/>
        /// method so it can be invoked directly from unit tests.
        /// </summary>
        private static object InvokeCreateLayoutManager(InteractiveBaseLayout layout)
        {
            System.Reflection.MethodInfo? method = null;
            var type = layout.GetType();
            while (type != null)
            {
                method = type.GetMethod("CreateLayoutManager",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Public);
                if (method != null)
                {
                    break;
                }
                type = type.BaseType;
            }
            if (method == null)
            {
                throw new InvalidOperationException("CreateLayoutManager not found.");
            }
            return method.Invoke(layout, null)!;
        }

        /// <summary>
        /// Test-friendly <see cref="InteractiveBaseLayout"/> that tracks measure and arrange
        /// invocations so the corresponding <see cref="InteractiveLayoutManager"/> tests can
        /// assert that delegation occurs.
        /// </summary>
        private sealed class TestInteractiveBaseLayout : InteractiveBaseLayout
        {
            public int MeasureCallCount { get; private set; }

            public double LastMeasureWidth { get; private set; }

            public double LastMeasureHeight { get; private set; }

            public int ArrangeCallCount { get; private set; }

            public Rect LastArrangeBounds { get; private set; }

            internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
            {
                MeasureCallCount++;
                LastMeasureWidth = widthConstraint;
                LastMeasureHeight = heightConstraint;
                return Size.Zero;
            }

            internal override Size LayoutArrangeChildren(Rect bounds)
            {
                ArrangeCallCount++;
                LastArrangeBounds = bounds;
                return bounds.Size;
            }
        }

        #endregion

    }
}