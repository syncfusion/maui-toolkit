using Syncfusion.Maui.Toolkit.Carousel;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfCarouselUnitTests : BaseUnitTest
    {

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfCarousel carousel = new SfCarousel();
            Assert.Equal(300, carousel.ItemHeight);
            Assert.Equal(200, carousel.ItemWidth);
            Assert.Equal(18f, carousel.Offset);
            Assert.Equal(40, carousel.SelectedItemOffset);
            Assert.Equal(0.7f, carousel.ScaleOffset);
            Assert.Equal(600, carousel.Duration);
            Assert.Equal(12, carousel.ItemSpacing);
            Assert.Null(carousel.ItemsSource);
            Assert.Null(carousel.ItemTemplate);
            Assert.Null(carousel.LoadMoreView);
            Assert.Equal(3, carousel.LoadMoreItemsCount);
            Assert.Equal(45, carousel.RotationAngle);
            Assert.Equal(0, carousel.SelectedIndex);
            Assert.True(carousel.EnableInteraction);
            Assert.False(carousel.AllowLoadMore);
            Assert.False(carousel.EnableVirtualization);
            Assert.Equal(ViewMode.Default, carousel.ViewMode);
            Assert.Equal(SwipeMovementMode.MultipleItems, carousel.SwipeMovementMode);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AllowLoadMore_SetValue_ReturnsExpectedValue(bool expectedValue)
        {
            SfCarousel carousel = new SfCarousel();
            carousel.AllowLoadMore = expectedValue;
            Assert.Equal(expectedValue, carousel.AllowLoadMore);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(0)]
        [InlineData(-100)]
        public void Duration_SetValue_ReturnsExpectedValue(int expectedValue)
        {
            var carousel = new SfCarousel();
            carousel.Duration = expectedValue;
            Assert.Equal(expectedValue, carousel.Duration);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableInteraction_SetValue_ReturnsExpectedValue(bool expectedValue)
        {
            SfCarousel carousel = new SfCarousel();
            carousel.EnableInteraction = expectedValue;
            Assert.Equal(expectedValue, carousel.EnableInteraction);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableVirtualization_SetValue_ReturnsExpectedValue(bool expectedValue)
        {
            SfCarousel carousel = new SfCarousel();
            carousel.EnableVirtualization = expectedValue;
            Assert.Equal(expectedValue, carousel.EnableVirtualization);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(0)]
        [InlineData(-50)]
        public void ItemHeight_SetValue_ReturnsExpectedValue(int expectedValue)
        {
            var carousel = new SfCarousel();
            carousel.ItemHeight = expectedValue;
            Assert.Equal(expectedValue, carousel.ItemHeight);
        }

        [Fact]
        public void LoadMoreView_SetValue_ReturnsExpectedValue()
        {
            var carousel = new SfCarousel();
            var customView = new ContentView
            {
                Content = new Label { Text = "Load More Custom" }
            };

            carousel.LoadMoreView = customView;
            Assert.Equal(customView, carousel.LoadMoreView);
        }

        [Fact]
        public void LoadMoreItemsCount_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            carousel.LoadMoreItemsCount = 1;
            Assert.Equal(1, carousel.LoadMoreItemsCount);
        }

        [Fact]
        public void TestItemSpacing_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            carousel.ItemSpacing = 20;
            Assert.Equal(20, carousel.ItemSpacing);
        }
       

        [Fact]
        public void ItemTemplate_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            var template = new DataTemplate(() =>
            {
                var label = new Label();
                label.SetBinding(Label.TextProperty, "ImageName");
                return label;
            });
            carousel.ItemTemplate = template;
            Assert.Equal(template, carousel.ItemTemplate);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-50)]
        public void ItemWidth_SetValue_ReturnsExpectedValue(int expectedValue)
        {
            var carousel = new SfCarousel();
            carousel.ItemWidth = expectedValue;
            Assert.Equal(expectedValue, carousel.ItemWidth);
        }


        [Fact]
        public void Offset_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            carousel.Offset = 5;
            Assert.Equal(5, carousel.Offset);
        }

        [Fact]
        public void RotationAngle_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            carousel.RotationAngle = 45;
            Assert.Equal(45, carousel.RotationAngle);
        }

        [Fact]
        public void ScaleOffSet_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            carousel.ScaleOffset = 0.76f;
            Assert.Equal(0.76f, carousel.ScaleOffset);
        }

        [Fact]
        public void SelectedIndex_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            carousel.SelectedIndex = 3;
            Assert.Equal(3, carousel.SelectedIndex);
        }

        [Fact]
        public void SelectedItemOffset_SetValue_ReturnsExpectedValue()
        {
            SfCarousel carousel = new SfCarousel();
            carousel.SelectedItemOffset = 5;
            Assert.Equal(5, carousel.SelectedItemOffset);
        }

        [Theory]
        [InlineData(SwipeMovementMode.SingleItem)]
        [InlineData(SwipeMovementMode.MultipleItems)]
        public void SwipeMovementMode_SetValue_ReturnsExpectedValue(SwipeMovementMode expectedValue)
        {
            SfCarousel carousel = new SfCarousel();
            carousel.SwipeMovementMode = expectedValue;
            Assert.Equal(expectedValue, carousel.SwipeMovementMode);
        }

        [Theory]
        [InlineData(ViewMode.Default)]
        [InlineData(ViewMode.Linear)]
        public void ViewMode_SetValue_ReturnsExpectedValue(ViewMode expectedValue)
        {
            SfCarousel carousel = new SfCarousel();
            carousel.ViewMode = expectedValue;
            Assert.Equal(expectedValue, carousel.ViewMode);
        }
    }
}
 








        
    

    

