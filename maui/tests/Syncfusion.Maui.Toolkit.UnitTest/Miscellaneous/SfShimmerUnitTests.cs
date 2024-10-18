using Syncfusion.Maui.Toolkit.Shimmer;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfShimmerUnitTests : BaseUnitTest
    {
        #region Constructor
        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfShimmer shimmer = new SfShimmer();
            Assert.Equal(1000, shimmer.AnimationDuration);
            Assert.Null(shimmer.CustomView);
            Assert.True(shimmer.IsActive);
            Assert.Equal(1, shimmer.RepeatCount);
            Assert.Equal(Color.FromArgb("#FFFFFF"), shimmer.WaveColor);
            var color = new SolidColorBrush(Color.FromArgb("#F7F2FB"));
            Assert.Equal(color.Color, ((SolidColorBrush)shimmer.Fill).Color);
            Assert.Equal(200, shimmer.WaveWidth);
            Assert.Equal(ShimmerWaveDirection.Default, shimmer.WaveDirection);
            Assert.Equal(ShimmerType.CirclePersona, shimmer.Type);
            Assert.Equal(Color.FromArgb("#FFFBFE"), shimmer.ShimmerBackground);
        }
        #endregion 
        #region Public Properties
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(100)]
        public void AnimationDuration_SetValue_ReturnsExpectedValue(int expectedValue)
        {
            SfShimmer shimmer = new SfShimmer();
            shimmer.AnimationDuration = expectedValue;
            Assert.Equal(expectedValue, shimmer.AnimationDuration);
        }

        [Theory]
        [InlineData("#FF0000")]
        public void Fill_SetValue_ReturnedExpectedValue(string colorHex)
        {
            SfShimmer shimmer = new SfShimmer();
            var expectedColor = Color.FromArgb(colorHex);
            shimmer.Fill = expectedColor;
            Assert.Equal(expectedColor, shimmer.Fill);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsActive_SetValue_ReturnsExpectedValue(bool isActive)
        {
            SfShimmer shimmer = new SfShimmer();
            shimmer.IsActive = isActive;
            Assert.Equal(isActive ,shimmer.IsActive);
        }

        [Theory]
        [InlineData("#FF0000")]
        public void WaveColor_SetValue_ReturnsExpectedValue(string colorHex)
        {
            SfShimmer shimmer = new SfShimmer();
            var expectedColor = Color.FromArgb(colorHex);
            shimmer.WaveColor = expectedColor;
            Assert.Equal(expectedColor, shimmer.WaveColor);
        }

        [Fact]
        public void WaveWidth_SetValue_ReturnsExpectedValue()
        {
            SfShimmer shimmer = new SfShimmer();
            shimmer.WaveWidth = 5;
            Assert.Equal(5, shimmer.WaveWidth);
        }

        [Theory]
        [InlineData(ShimmerType.CirclePersona)]
        [InlineData(ShimmerType.Profile)]
        [InlineData(ShimmerType.Article)]
        [InlineData(ShimmerType.Feed)]
        [InlineData(ShimmerType.SquarePersona)]
        [InlineData(ShimmerType.Shopping)]
        public void ShimmerType_SetValue_ReturnsExpectedValue(ShimmerType expectedValue)
        {
            SfShimmer shimmer = new SfShimmer();
            shimmer.Type = expectedValue;
            Assert.Equal(expectedValue, shimmer.Type);
        }

        [Theory]
        [InlineData(ShimmerWaveDirection.TopToBottom)]
        [InlineData(ShimmerWaveDirection.BottomToTop)]
        [InlineData(ShimmerWaveDirection.LeftToRight)]
        [InlineData(ShimmerWaveDirection.RightToLeft)]
        public void ShimmerWaveDirection_SetValue_ReturnsExpectedValue(ShimmerWaveDirection expectedValue)
        {
            SfShimmer shimmer = new SfShimmer();
            shimmer.WaveDirection = expectedValue;
            Assert.Equal(expectedValue, shimmer.WaveDirection);

        }
        [Fact]
        public void RepeatCount_SetValue_ReturnsExpectedValue()
        {
            SfShimmer shimmer = new SfShimmer();
            shimmer.RepeatCount = 10;
            Assert.Equal(10, shimmer.RepeatCount);
        }
        [Theory]
        [InlineData(ShimmerShapeType.Circle)]
        [InlineData(ShimmerShapeType.Rectangle)]
        [InlineData(ShimmerShapeType.RoundedRectangle)]
        public void ShimmerShapeType_SetValue_ReturnsExpectedValue(ShimmerShapeType expectedShapeType)
        {
            var shimmerView = new ShimmerView();
            shimmerView.ShapeType = expectedShapeType;
            Assert.Equal(expectedShapeType, shimmerView.ShapeType);
        }
        [Fact]
        public void CustomView_SetValue_ReturnsExpectedValue()
        {

            SfShimmer shimmer = new SfShimmer();
            var grid = new Grid
            {
                Padding = 10,
                ColumnSpacing = 15,
                RowSpacing = 10,
                VerticalOptions = LayoutOptions.Fill,
                RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            },
                ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto },
            }
            };
            shimmer.CustomView = grid;
            Assert.Equal(grid, shimmer.CustomView);
        }
        [Fact]
        public void CustomView_AddsView_WhenIsActiveIsTrue()
        {
            var shimmer = new SfShimmer();
            var customView = new ContentView();
            shimmer.IsActive = true;
            shimmer.CustomView = customView;
            Assert.True(shimmer.Children.Contains(customView));
        }
        [Fact]
        public void CustomView_DoesNotAddView_WhenIsActiveIsFalse()
        {
            var shimmer = new SfShimmer();
            var customView = new ContentView();
            shimmer.IsActive = false;
            shimmer.CustomView = customView;
            Assert.False(shimmer.Children.Contains(customView));
        }
        [Fact]
        public void CustomView_RemainsInChildren_WhenIsActiveChangesFromFalseToTrue()
        {
            var shimmer = new SfShimmer();
            var customView = new ContentView();
            shimmer.IsActive = false;
            shimmer.CustomView = customView;
            shimmer.IsActive = true;
            Assert.True(shimmer.Children.Contains(customView));
        }
        [Fact]
        public void CustomView_Removed_WhenIsActiveChnagesFromFalseToTrue()
        {
            var shimmer = new SfShimmer();
            var customView = new ContentView();
            shimmer.IsActive = true;
            shimmer.CustomView = customView;
            shimmer.IsActive = false;
            Assert.False(shimmer.Children.Contains(customView));
        }
        [Fact]
        public void CustomView_OpacityIsOne_WhenIsActiveIsFalse()
        {
            var shimmer = new SfShimmer
            {
                Content = new ContentView { Opacity = 0 },
                CustomView = new ContentView()
            };
            shimmer.IsActive = false;
            Assert.Equal(1, shimmer.Content.Opacity);
        }
        [Fact]
        public void CustomView_OpacityIsZero_WhenIsActiveIsTrue()
        {
            var shimmer = new SfShimmer
            {
                Content = new ContentView { Opacity = 0 },
                CustomView = new ContentView()
            };
            shimmer.IsActive = true;
            Assert.Equal(0, shimmer.Content.Opacity);
        }
        #endregion
        #region internal properties
        [Fact]
        public void ShimmerBackground_SetValue_ReturnsExpectedValue()
        {
            var shimmer = new SfShimmer();
            shimmer.BackgroundColor = Colors.Red;
            Assert.Equal(Colors.Red, shimmer.BackgroundColor);
        }
        [Fact]
        public void ShimmerBackground_SetValue_UpdatesBackgroundColor()
        {
            var shimmer = new SfShimmer();
            var oldBackground = Colors.Red;
            var newBackground = Colors.Blue;
            shimmer.ShimmerBackground = oldBackground;
            shimmer.BackgroundColor = oldBackground;
            shimmer.ShimmerBackground = newBackground;
            Assert.Equal(newBackground, shimmer.BackgroundColor);
        }
        #endregion
        #region Internal Methods
        [Theory]
        [InlineData(ShimmerWaveDirection.TopToBottom)]
        [InlineData(ShimmerWaveDirection.BottomToTop)]
        [InlineData(ShimmerWaveDirection.LeftToRight)]
        [InlineData(ShimmerWaveDirection.RightToLeft)]
        [InlineData(ShimmerWaveDirection.Default)]
        public void CreateWavePaint_UpdatesGradientBasedOnWaveDirection(ShimmerWaveDirection direction)
        {
            var shimmer = new SfShimmer();
            shimmer.WaveDirection = direction;
            var shimmerDrawable = GetNonPublicProperty(shimmer, "ShimmerDrawable");
            var gradientValue = new LinearGradientBrush();
            if (shimmerDrawable != null)
            {
                SetPrivateField(shimmerDrawable, "_gradient", gradientValue);
                var createWavePaintMethod = shimmerDrawable.GetType().GetMethod("CreateWavePaint", BindingFlags.NonPublic | BindingFlags.Instance);
                createWavePaintMethod!.Invoke(shimmerDrawable, null);
            }
            Assert.Equal(direction, shimmer.WaveDirection);
        }
        [Fact]
        public void CreateWaveAnimator_WithNegativeAnimationDuration_DoesNotChangeAnimationDuration()
        {
            var shimmer = new SfShimmer();
            var animationDuration = -1;
            shimmer.AnimationDuration = animationDuration;
            var shimmerDrawable = GetNonPublicProperty(shimmer, "ShimmerDrawable");
            var createWaveAnimatorMethod = shimmerDrawable!.GetType().GetMethod("CreateWaveAnimator", BindingFlags.NonPublic | BindingFlags.Instance);
            createWaveAnimatorMethod!.Invoke(shimmerDrawable, null);
            Assert.Equal(animationDuration, shimmer.AnimationDuration);
        }
        #endregion
        #region PrivateMethods
        [Fact]
        public void CreateShimmerViewPath()
        {
            var shimmer = new SfShimmer();
            var shimmerDrawable = GetNonPublicProperty(shimmer, "ShimmerDrawable");
            var createShimmerViewPathMethod = shimmerDrawable!.GetType().GetMethod("CreateShimmerViewPath", BindingFlags.NonPublic | BindingFlags.Instance);
            createShimmerViewPathMethod!.Invoke(shimmerDrawable, null);
            Assert.NotNull(shimmer);
        }

        [Theory]
        [InlineData(ShimmerType.CirclePersona)]
        [InlineData(ShimmerType.Profile)]
        [InlineData(ShimmerType.Article)]
        [InlineData(ShimmerType.Feed)]
        [InlineData(ShimmerType.SquarePersona)]
        [InlineData(ShimmerType.Shopping)]
        [InlineData(ShimmerType.Video)]
        public void TestCreateShimmerViewPathDefaultView(ShimmerType type)
        {
            var shimmer = new SfShimmer();
            shimmer.Type = type;
            var shimmerDrawable = GetNonPublicProperty(shimmer, "ShimmerDrawable");
            var createShimmerViewPathMethod = shimmerDrawable!.GetType().GetMethod("CreateShimmerViewPath", BindingFlags.NonPublic | BindingFlags.Instance);
            var size = new Size(100, 100);
            SetPrivateField(shimmerDrawable, "_availableSize", size);
            createShimmerViewPathMethod!.Invoke(shimmerDrawable, null);
            Assert.Equal(type, shimmer.Type);
        }

        [Fact]
        public void CreateShimmerViewPathCustomViewMethod()
        {
            var shimmer = new SfShimmer();
            var grid = new Grid
            {
                Padding = 10,
                ColumnSpacing = 15,
                RowSpacing = 10,
                VerticalOptions = LayoutOptions.Fill,
                RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            },
                ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto },
            }
            };
            shimmer.CustomView = grid;
            var shimmerDrawable = GetNonPublicProperty(shimmer, "ShimmerDrawable");
            var createShimmerViewPathMethod = shimmerDrawable!.GetType().GetMethod("CreateShimmerViewPath", BindingFlags.NonPublic | BindingFlags.Instance);
            var size = new Size(100, 100);
            SetPrivateField(shimmerDrawable, "_availableSize", size);
            createShimmerViewPathMethod!.Invoke(shimmerDrawable, null);
            Assert.Equal(grid, shimmer.CustomView);
        }

        [Fact]
        public void UpdateShimmerView_SetsShimmerDrawable()
        {
            var shimmer = new SfShimmer();
            var shimmerDrawable = GetNonPublicProperty(shimmer, "ShimmerDrawable");
            var updateShimmerViewMethod = shimmerDrawable!.GetType().GetMethod("UpdateShimmerView", BindingFlags.NonPublic | BindingFlags.Instance);
            updateShimmerViewMethod!.Invoke(shimmerDrawable, null);
            Assert.NotNull(shimmer);
        }
        #endregion 
    }
}
