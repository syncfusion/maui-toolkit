using Syncfusion.Maui.Toolkit.BottomSheet;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfBottomSheetUnitTests : BaseUnitTest
    {
        Syncfusion.Maui.Toolkit.BottomSheet.SfBottomSheet _bottomSheet;

        public SfBottomSheetUnitTests()
        {
            _bottomSheet = new Syncfusion.Maui.Toolkit.BottomSheet.SfBottomSheet();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsModal(bool input, bool expected)
        {
            _bottomSheet.IsModal = input;

            var actual = _bottomSheet.IsModal;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ShowGrabber(bool input, bool expected)
        {
            _bottomSheet.ShowGrabber = input;

            var actual = _bottomSheet.ShowGrabber;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void EnableSwiping(bool input, bool expected)
        {
            _bottomSheet.EnableSwiping = input;

            var actual = _bottomSheet.EnableSwiping;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0.5, 0.5)]
        [InlineData(0, 0.1)]
        [InlineData(-0.3, 0.1)]
        [InlineData(1.0, 0.9)]
        [InlineData(1.5, 0.9)]
        public void HalfExpandedRatio(double input, double expected)
        {
            _bottomSheet.HalfExpandedRatio = input;

            var actual = _bottomSheet.HalfExpandedRatio;

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> ContentPaddingData =>
        new List<object[]>
        {
            new object[] { new Thickness(5), new Thickness(5) },
            new object[] { new Thickness(0), new Thickness(0) },
            new object[] { new Thickness(-5), new Thickness(-5) },
            new object[] { new Thickness(10, 5, 10, 5), new Thickness(10, 5, 10, 5) },
            new object[] { new Thickness(15, 0, 5, 10), new Thickness(15, 0, 5, 10) }
        };

        [Theory]
        [MemberData(nameof(ContentPaddingData))]
        public void ContentPadding(Thickness input, Thickness expected)
        {
            _bottomSheet.ContentPadding = input;

            var actual = _bottomSheet.ContentPadding;

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> MarginData =>
        new List<object[]>
        {
            new object[] { new Thickness(5), new Thickness(5) },
            new object[] { new Thickness(0), new Thickness(0) },
            new object[] { new Thickness(-5), new Thickness(-5) },
            new object[] { new Thickness(10, 5, 10, 5), new Thickness(10, 5, 10, 5) },
            new object[] { new Thickness(15, 0, 5, 10), new Thickness(15, 0, 5, 10) }
        };

        [Theory]
        [MemberData(nameof(MarginData))]
        public void Margin(Thickness input, Thickness expected)
        {
            _bottomSheet.Margin = input;

            var actual = _bottomSheet.Margin;

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> CornerRadiusData =>
        new List<object[]>
        {
            new object[] { new CornerRadius(5), new CornerRadius(5) },
            new object[] { new CornerRadius(0), new CornerRadius(0) },
            new object[] { new CornerRadius(10, 5, 10, 5), new CornerRadius(10, 5, 10, 5) },
            new object[] { new CornerRadius(15, 0, 5, 10), new CornerRadius(15, 0, 5, 10) },
        };

        [Theory]
        [MemberData(nameof(CornerRadiusData))]
        public void CornerRadius(CornerRadius input, CornerRadius expected)
        {
            _bottomSheet.CornerRadius = input;

            var actual = _bottomSheet.CornerRadius;

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> BackgroundBrushData =>
        new List<object[]>
        {
            new object[] { new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Red) },
            new object[] { new SolidColorBrush(Colors.Blue), new SolidColorBrush(Colors.Blue) },
            new object[] { new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.White) },
            new object[] { new SolidColorBrush(Colors.Transparent), new SolidColorBrush(Colors.Transparent) }
        };

        [Theory]
        [MemberData(nameof(BackgroundBrushData))]
        public void BackgroundBrush(Brush input, Brush expected)
        {
            _bottomSheet.Background = input;

            var actual = _bottomSheet.Background;

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GrabberBackgroundData =>
        new List<object[]>
        {
            new object[] { Colors.Red, Colors.Red },
            new object[] { Colors.Blue, Colors.Blue },
            new object[] { Colors.Black, Colors.Black },
            new object[] { Colors.Transparent, Colors.Transparent }
        };

        [Theory]
        [MemberData(nameof(GrabberBackgroundData))]
        public void GrabberBackground(Color input, Color expected)
        {
            _bottomSheet.GrabberBackground = Colors.Gray;
            _bottomSheet.GrabberBackground = input;

            var actual = _bottomSheet.GrabberBackground;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(BottomSheetState.FullExpanded, BottomSheetState.FullExpanded)]
        [InlineData(BottomSheetState.HalfExpanded, BottomSheetState.HalfExpanded)]
        [InlineData(BottomSheetState.Hidden, BottomSheetState.Hidden)]
        public void SetBottomSheetState(BottomSheetState input, BottomSheetState expected)
        {
            _bottomSheet.State = input;

            var actual = _bottomSheet.State;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(BottomSheetState.Hidden, BottomSheetState.HalfExpanded)]
        [InlineData(BottomSheetState.Hidden, BottomSheetState.FullExpanded)]
        [InlineData(BottomSheetState.HalfExpanded, BottomSheetState.FullExpanded)]
        [InlineData(BottomSheetState.FullExpanded, BottomSheetState.HalfExpanded)]
        [InlineData(BottomSheetState.HalfExpanded, BottomSheetState.Hidden)]
        [InlineData(BottomSheetState.FullExpanded, BottomSheetState.Hidden)]
        public void TransitionBottomSheetState(BottomSheetState initialState, BottomSheetState targetState)
        {
            _bottomSheet.State = initialState;

            _bottomSheet.State = targetState;
            var actual = _bottomSheet.State;

            Assert.Equal(targetState, actual);
        }

        [Theory]
        [InlineData(BottomSheetAllowedState.FullExpanded, BottomSheetAllowedState.FullExpanded)]
        [InlineData(BottomSheetAllowedState.HalfExpanded, BottomSheetAllowedState.HalfExpanded)]
        [InlineData(BottomSheetAllowedState.All, BottomSheetAllowedState.All)]
        public void SetBottomSheetAllowedState(BottomSheetAllowedState input, BottomSheetAllowedState expected)
        {
            _bottomSheet.AllowedState = input;

            var actual = _bottomSheet.AllowedState;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(BottomSheetState.FullExpanded, BottomSheetState.FullExpanded, BottomSheetAllowedState.All)]
        [InlineData(BottomSheetState.HalfExpanded, BottomSheetState.HalfExpanded, BottomSheetAllowedState.All)]
        [InlineData(BottomSheetState.Hidden, BottomSheetState.Hidden, BottomSheetAllowedState.All)]
        public void BottomSheetAllowedState_All(BottomSheetState input, BottomSheetState expected, BottomSheetAllowedState allowedState)
        {
            _bottomSheet.AllowedState = allowedState;
            _bottomSheet.State = input;

            var actual = _bottomSheet.State;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(BottomSheetState.FullExpanded, BottomSheetState.FullExpanded, BottomSheetAllowedState.FullExpanded)]
        [InlineData(BottomSheetState.HalfExpanded, BottomSheetState.Hidden, BottomSheetAllowedState.FullExpanded)]
        [InlineData(BottomSheetState.Hidden, BottomSheetState.Hidden, BottomSheetAllowedState.FullExpanded)]
        public void BottomSheetAllowedState_FullExpanded(BottomSheetState input, BottomSheetState expected, BottomSheetAllowedState allowedState)
        {
            _bottomSheet.AllowedState = allowedState;
            _bottomSheet.State = input;

            var actual = _bottomSheet.State;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(BottomSheetState.FullExpanded, BottomSheetState.Hidden, BottomSheetAllowedState.HalfExpanded)]
        [InlineData(BottomSheetState.HalfExpanded, BottomSheetState.HalfExpanded, BottomSheetAllowedState.HalfExpanded)] // expected is provided collapsed since the sheet is closed
        [InlineData(BottomSheetState.Hidden, BottomSheetState.Hidden, BottomSheetAllowedState.HalfExpanded)]
        public void BottomSheetAllowedState_HalfExpanded(BottomSheetState input, BottomSheetState expected, BottomSheetAllowedState allowedState)
        {
            _bottomSheet.AllowedState = allowedState;
            _bottomSheet.State = input;

            var actual = _bottomSheet.State;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(BottomSheetState.FullExpanded, BottomSheetState.FullExpanded, BottomSheetAllowedState.All)]
        [InlineData(BottomSheetState.HalfExpanded, BottomSheetState.HalfExpanded, BottomSheetAllowedState.All)]
        public void BottomSheetAllowedState_WhenOpen(BottomSheetState input, BottomSheetState expected, BottomSheetAllowedState allowedState)
        {
            var temp = new Syncfusion.Maui.Toolkit.BottomSheet.SfBottomSheet();
            temp.IsVisible = true;
            temp.HeightRequest = 500;
            temp.State = input;
            temp.Show();
            temp.AllowedState = allowedState;

            var actual = temp.State;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BottomSheetContent_SetAndGetContent_ShouldMatchAssignedView()
        {
            var bottomSheet = new Syncfusion.Maui.Toolkit.BottomSheet.SfBottomSheet();
            var labelContent = new Label { Text = "Test Label" };

            bottomSheet.BottomSheetContent = labelContent;

            Assert.Equal(labelContent, bottomSheet.BottomSheetContent);
        }

        [Fact]
        public void BottomSheetContent_ChangeContent_ShouldUpdateToNewView()
        {
            var bottomSheet = new Syncfusion.Maui.Toolkit.BottomSheet.SfBottomSheet();
            var initialContent = new Label { Text = "Initial Content" };
            var newContent = new Button { Text = "New Content" };

            bottomSheet.BottomSheetContent = initialContent;
            Assert.Equal(initialContent, bottomSheet.BottomSheetContent);
            bottomSheet.BottomSheetContent = newContent;

            Assert.Equal(newContent, bottomSheet.BottomSheetContent);
        }

		public static IEnumerable<object[]> GrabberCornerRadiusData =>
		new List<object[]>
		{
			new object[] { new CornerRadius(5), new CornerRadius(5) },
			new object[] { new CornerRadius(0), new CornerRadius(0) },
			new object[] { new CornerRadius(10, 5, 10, 5), new CornerRadius(10, 5, 10, 5) },
			new object[] { new CornerRadius(15, 0, 5, 10), new CornerRadius(15, 0, 5, 10) },
		};

		[Theory]
		[MemberData(nameof(GrabberCornerRadiusData))]
		public void GrabberCornerRadius(CornerRadius input, CornerRadius expected)
		{
			_bottomSheet.GrabberCornerRadius = input;

			var actual = _bottomSheet.GrabberCornerRadius;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(8, 8)]
		[InlineData(35, 35)]
		[InlineData(0, 0)]
		public void GrabberHeight(double input, double expected)
		{
			_bottomSheet.GrabberHeight = input;

			var actual = _bottomSheet.GrabberHeight;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(40, 40)]
		[InlineData(85, 85)]
		[InlineData(0, 0)]
		public void GrabberWidth(double input, double expected)
		{
			_bottomSheet.GrabberWidth = input;

			var actual = _bottomSheet.GrabberWidth;

			Assert.Equal(expected, actual);
		}
	}
}