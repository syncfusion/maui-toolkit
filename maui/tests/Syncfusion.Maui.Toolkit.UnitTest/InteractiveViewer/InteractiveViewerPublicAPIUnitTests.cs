using Syncfusion.Maui.Toolkit.InteractiveViewer;
using Syncfusion.Maui.Toolkit;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class InteractiveViewerPublicAPIUnitTests : BaseUnitTest
    {
        #region Constructor and Public API Tests

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            Assert.Equal(100, interactiveViewer.MinimumWidthRequest);
            Assert.Equal(100, interactiveViewer.MinimumHeightRequest);
            Assert.True(interactiveViewer.IsZoomEnabled);
            Assert.True(interactiveViewer.IsPanEnabled);
            Assert.Equal(PanAxis.Both, interactiveViewer.PanAxis);
            Assert.Equal(1d, interactiveViewer.ZoomFactor);
            Assert.Equal(1d, interactiveViewer.MinimumZoomFactor);
            Assert.Equal(10d, interactiveViewer.MaximumZoomFactor);
            Assert.Null(interactiveViewer.Content);
            Assert.Empty(interactiveViewer.Children);
        }

        [Fact]
        public void Content_GetAndSet_View()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            Label expectedValue = new Label { Text = "Content" };

            interactiveViewer.Content = expectedValue;
            View? actualValue = interactiveViewer.Content;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsZoomEnabled_GetAndSet_Boolean(bool expectedValue)
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            interactiveViewer.IsZoomEnabled = expectedValue;
            bool actualValue = interactiveViewer.IsZoomEnabled;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsPanEnabled_GetAndSet_Boolean(bool expectedValue)
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            interactiveViewer.IsPanEnabled = expectedValue;
            bool actualValue = interactiveViewer.IsPanEnabled;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(PanAxis.Both)]
        [InlineData(PanAxis.Horizontal)]
        [InlineData(PanAxis.Vertical)]
        public void PanAxis_GetAndSet_Enum(PanAxis expectedValue)
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            interactiveViewer.PanAxis = expectedValue;
            PanAxis actualValue = interactiveViewer.PanAxis;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0.5)]
        [InlineData(1d)]
        [InlineData(2.5)]
        [InlineData(10d)]
        [InlineData(25d)]
        public void ZoomFactor_GetAndSet_Double(double expectedValue)
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            interactiveViewer.ZoomFactor = expectedValue;
            double actualValue = interactiveViewer.ZoomFactor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0.5)]
        [InlineData(1d)]
        [InlineData(2.5)]
        [InlineData(10d)]
        [InlineData(25d)]
        public void MinimumZoomFactor_GetAndSet_Double(double expectedValue)
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            interactiveViewer.MinimumZoomFactor = expectedValue;
            double actualValue = interactiveViewer.MinimumZoomFactor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(1d)]
        [InlineData(5d)]
        [InlineData(10d)]
        [InlineData(25d)]
        [InlineData(50d)]
        public void MaximumZoomFactor_GetAndSet_Double(double expectedValue)
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();

            interactiveViewer.MaximumZoomFactor = expectedValue;
            double actualValue = interactiveViewer.MaximumZoomFactor;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Methods

        [Fact]
        public void Rotate_WithContent_RotatesBy90Degrees()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            Label content = new Label { Rotation = 0 };
            interactiveViewer.Content = content;

            // Force lazy initialization so Rotate() has an interactive layout to forward to.
            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            interactiveViewer.Rotate();

            Assert.Equal(90d, content.Rotation);
        }

        [Fact]
        public void Reset_WithContent_DoesNotThrow()
        {
            SfInteractiveViewer interactiveViewer = new SfInteractiveViewer();
            interactiveViewer.Content = new Label { Rotation = 90 };

            InvokePrivateMethod(interactiveViewer, "InitializeInteractiveViewer");

            var exception = Record.Exception(() => interactiveViewer.Reset());

            Assert.Null(exception);
        }

        #endregion
    }
}