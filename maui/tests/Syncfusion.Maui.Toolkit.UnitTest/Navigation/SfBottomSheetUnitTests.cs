using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.BottomSheet;
using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;
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

		#region Constructor

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			Assert.True(_bottomSheet.IsModal);
			Assert.True(_bottomSheet.ShowGrabber);
			Assert.True(_bottomSheet.EnableSwiping);
			Assert.False(_bottomSheet.IsOpen);
			Assert.Null(_bottomSheet.Content);
			Assert.Null(_bottomSheet.BottomSheetContent);
			Assert.Equal(0.5d, _bottomSheet.HalfExpandedRatio);
			Assert.Equal(1d, _bottomSheet.FullExpandedRatio);
			Assert.Equal(100d, _bottomSheet.CollapsedHeight);
			Assert.Equal(BottomSheetContentWidthMode.Full, _bottomSheet.ContentWidthMode);
			Assert.Equal(300d, _bottomSheet.BottomSheetContentWidth);
			Assert.Equal(new Thickness(5), _bottomSheet.ContentPadding);
			Assert.Equal(new CornerRadius(0), _bottomSheet.CornerRadius);
			Assert.Equal(BottomSheetState.Hidden, _bottomSheet.State);
			Assert.Equal(BottomSheetAllowedState.All, _bottomSheet.AllowedState);
			Assert.Equal(4d, _bottomSheet.GrabberHeight);
			Assert.Equal(32d, _bottomSheet.GrabberWidth);
			Assert.Equal(12d, _bottomSheet.GrabberCornerRadius);
			if (_bottomSheet.GrabberBackground is SolidColorBrush grabberBrush)
			{
				var grabberColor = grabberBrush.Color;
				Assert.Equal(Color.FromArgb("#CAC4D0"), grabberColor);
			}

			if (_bottomSheet.Background is SolidColorBrush backgroundBrush)
			{
				var backgroundColor = backgroundBrush.Color;
				Assert.Equal(Color.FromArgb("#F7F2FB"), backgroundColor);
			}
		}

		#endregion

		#region Pubilc Properties

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
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void IsOpen(bool input, bool expected)
		{
			_bottomSheet.IsOpen = input;

			var actual = _bottomSheet.IsOpen;

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

		[Theory]
		[InlineData(0.5, 0.5)]
		[InlineData(0, 0.1)]
		[InlineData(-0.3, 0.1)]
		[InlineData(1.0, 1.0)]
		[InlineData(1.5, 1.0)]
		public void FullExpandedRatio(double input, double expected)
		{
			_bottomSheet.FullExpandedRatio = input;

			var actual = _bottomSheet.FullExpandedRatio;

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

		[Fact]
		public void Content_SetAndGetContent_ShouldMatchAssignedView()
		{
			var bottomSheet = new Syncfusion.Maui.Toolkit.BottomSheet.SfBottomSheet();
			var labelContent = new Label { Text = "Test Label" };

			bottomSheet.Content = labelContent;

			Assert.Equal(labelContent, bottomSheet.Content);
		}

		[Fact]
		public void Content_ChangeContent_ShouldUpdateToNewView()
		{
			var bottomSheet = new Syncfusion.Maui.Toolkit.BottomSheet.SfBottomSheet();
			var initialContent = new Label { Text = "Initial Content" };
			var newContent = new Button { Text = "New Content" };

			bottomSheet.Content = initialContent;
			Assert.Equal(initialContent, bottomSheet.Content);
			bottomSheet.Content = newContent;

			Assert.Equal(newContent, bottomSheet.Content);
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

		[Theory]
		[InlineData(BottomSheetContentWidthMode.Full,  BottomSheetContentWidthMode.Full)]
		[InlineData(BottomSheetContentWidthMode.Custom, BottomSheetContentWidthMode.Custom)]
		public void ContentWidthMode(BottomSheetContentWidthMode input, BottomSheetContentWidthMode expected)
		{
			_bottomSheet.ContentWidthMode = input;

			var actual = _bottomSheet.ContentWidthMode;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(500, 500 , BottomSheetContentWidthMode.Full)]
		[InlineData(200, 200, BottomSheetContentWidthMode.Custom)]
		public void BottomSheetContentWidth(double input, double expected, BottomSheetContentWidthMode mode)
		{
			_bottomSheet.BottomSheetContentWidth = input;

			_bottomSheet.ContentWidthMode = mode;
			var actual = _bottomSheet.BottomSheetContentWidth;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(300, 300)]
		[InlineData(80, 80)]
		public void CollapsedHeight(double input, double expected)
		{
			_bottomSheet.CollapsedHeight = input;

			var actual = _bottomSheet.CollapsedHeight;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(60, 60)]
		[InlineData(5, 5)]
		[InlineData(120, 120)]
		public void GrabberAreaHeight(double input, double expected)
		{
			_bottomSheet.GrabberAreaHeight = input;

			var actual = _bottomSheet.GrabberAreaHeight;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void CollapseOnOverlayTap(bool input, bool expected)
		{
			_bottomSheet.CollapseOnOverlayTap = input;

			var actual = _bottomSheet.CollapseOnOverlayTap;

			Assert.Equal(expected, actual);
		}

		#endregion

		#region Internal Properties

		public static IEnumerable<object[]> OverlayBackgroundColorData =>
		new List<object[]>
		{
			new object[] { Color.FromArgb("#80000000"), Color.FromArgb("#80000000") },
			new object[] { Color.FromArgb("#CAC4D0"), Color.FromArgb("#CAC4D0") }
		};

		[Theory]
		[MemberData(nameof(OverlayBackgroundColorData))]
		public void OverlayBackgroundColor(Color input, Color expected)
		{
			_bottomSheet.OverlayBackgroundColor = input;

			var actual = _bottomSheet.OverlayBackgroundColor;

			Assert.Equal(expected, actual);
		}

		#endregion

		#region Internal Methods

		[Theory]
		[InlineData(PointerActions.Pressed, 100)]
		[InlineData(PointerActions.Moved, 200)]
		[InlineData(PointerActions.Released, 300)]
		public void OnHandleTouch_ShouldHandleValidActions(PointerActions action, double touchY)
		{
			_bottomSheet.EnableSwiping = true;
			SetPrivateField(_bottomSheet, "_isSheetOpen", true);
			var point = new Point(0, touchY);

			_bottomSheet.OnHandleTouch(action, point);
			_bottomSheet.TranslationY = touchY;

			switch (action)
			{
				case PointerActions.Pressed:
					Assert.Equal(touchY, GetPrivateField(_bottomSheet, "_initialTouchY"));
					Assert.True(Convert.ToBoolean(GetPrivateField(_bottomSheet, "_isPointerPressed")));
					break;
				case PointerActions.Moved:
					Assert.True(_bottomSheet.TranslationY > 0);
					break;
				case PointerActions.Released:
					Assert.False(Convert.ToBoolean(GetPrivateField(_bottomSheet, "_isPointerPressed")));
					break;
			}
		}

		[Fact]
		public void OnHandleTouch_ShouldNotHandleWhenSwipingDisabled()
		{
			_bottomSheet.EnableSwiping = false;

			_bottomSheet.OnHandleTouch(PointerActions.Pressed, new Point(0, 100));

			Assert.False(Convert.ToBoolean(GetPrivateField(_bottomSheet, "_isPointerPressed")));
		}

		#endregion

		#region Private Methods

		[Fact]
		public void InitializeLayout()
		{
			InvokePrivateMethod(_bottomSheet, "InitializeLayout");
			var bottomSheet = GetPrivateField(_bottomSheet, "_bottomSheet");
			var overlayGrid = GetPrivateField(_bottomSheet, "_overlayGrid");
			Assert.True(_bottomSheet.Children?.Contains(bottomSheet));
			Assert.True(_bottomSheet.Children?.Contains(overlayGrid));
		}

		[Fact]
		public void AddChild()
		{
			var verticalStackLayout = new VerticalStackLayout()
			{
				HorizontalOptions = LayoutOptions.Center,
			};

			var label = new Label()
			{
				Text = "Bottom Sheet"
			};

			verticalStackLayout.Children.Add(label);
			InvokePrivateMethod(_bottomSheet, "AddChild", [verticalStackLayout]);
			Assert.True(_bottomSheet.Children.Contains(verticalStackLayout));
		}

		[Fact]
		public void InitializeOverlayGrid()
		{
			InvokePrivateMethod(_bottomSheet, "InitializeOverlayGrid");
			Grid? overlayGrid = (Grid?)GetPrivateField(_bottomSheet, "_overlayGrid");
			Assert.Equal(overlayGrid?.BackgroundColor, Color.FromArgb("#80000000"));
			Assert.Equal(overlayGrid?.Opacity, 0.5);
			Assert.Equal(overlayGrid?.IsVisible, false);
		}

		[Fact]
		public void InitializeGrabber()
		{
			InvokePrivateMethod(_bottomSheet, "InitializeGrabber");
			Border? grabber = (Border?)GetPrivateField(_bottomSheet, "_grabber");
			RoundRectangle? grabberStrokeShape = (RoundRectangle?)GetPrivateField(_bottomSheet, "_grabberStrokeShape");
			Assert.Equal(grabber?.StrokeShape, grabberStrokeShape);
		}

		[Fact]
		public void InitializeContentBorder()
		{
			InvokePrivateMethod(_bottomSheet, "InitializeContentBorder");
			Border? contentBorder = (Border?)GetPrivateField(_bottomSheet, "_contentBorder");
			Assert.NotNull(contentBorder);
		}

		[Theory]
		[InlineData(0.5, 0.5)]
		[InlineData(0, 0.1)]
		[InlineData(-0.5, 0.1)]
		public void UpdateHalfExpandedRatioProperty(double value, double expectedValue)
		{
			InvokePrivateMethod(_bottomSheet, "UpdateHalfExpandedRatioProperty", [value]);
			double result = _bottomSheet.HalfExpandedRatio;
			Assert.Equal(expectedValue, result);
		}

		[Theory]
		[InlineData(0.8, 0.8)]
		[InlineData(0, 0.1)]
		[InlineData(-0.5, 0.1)]
		public void UpdateFullExpandedRatioProperty(double value, double expectedValue)
		{
			InvokePrivateMethod(_bottomSheet, "UpdateFullExpandedRatioProperty", [value]);
			double result = _bottomSheet.FullExpandedRatio;
			Assert.Equal(expectedValue, result);
		}

		[Fact]
		public void SetBottomSheetContent()
		{
			var verticalStackLayout = new VerticalStackLayout()
			{
				HorizontalOptions = LayoutOptions.Center,
			};

			var label = new Label()
			{
				Text = "Bottom Sheet"
			};

			verticalStackLayout.Children.Add(label);
			InvokePrivateMethod(_bottomSheet, "SetBottomSheetContent", [verticalStackLayout]);
			SfGrid? grid = (SfGrid?)GetPrivateField(_bottomSheet, "_bottomSheetContent");
			SfBorder? content = (SfBorder?)GetPrivateField(_bottomSheet, "_contentBorder");
			Assert.NotNull(content);
			Assert.True(grid?.Children.Contains(content));
		}

		[Fact]
		public void UpdateState_HalfExpanded()
		{
			SetPrivateField(_bottomSheet, "_isSheetOpen", true);
			_bottomSheet.State = BottomSheetState.HalfExpanded;
			var isHalfExpanded = GetPrivateField(_bottomSheet, "_isHalfExpanded");
			Assert.Equal(BottomSheetState.HalfExpanded, _bottomSheet.State);
			Assert.True((bool?)isHalfExpanded);
		}

		[Fact]
		public void UpdateState_FullExpanded()
		{
			SetPrivateField(_bottomSheet, "_isSheetOpen", true);
			_bottomSheet.State = BottomSheetState.Hidden;
			_bottomSheet.AllowedState = BottomSheetAllowedState.FullExpanded;
			var isHalfExpanded = GetPrivateField(_bottomSheet, "_isHalfExpanded");
			Assert.Equal(BottomSheetState.FullExpanded, _bottomSheet.State);
			Assert.False((bool?)isHalfExpanded);
		}

		[Fact]
		public void UpdateState()
		{
			_bottomSheet.State = BottomSheetState.Hidden;
			SetPrivateField(_bottomSheet, "_isSheetOpen", false);
			_bottomSheet.AllowedState = BottomSheetAllowedState.HalfExpanded;
			var isHalfState = GetPrivateField(_bottomSheet, "_isHalfExpanded");
			Assert.Equal(BottomSheetState.Hidden, _bottomSheet.State);
		}

		[Fact]
		public void UpdateBottomSheetBackground()
		{
			var grid = new SfGrid();
			Brush brush = new SolidColorBrush(Colors.Red);
			SetPrivateField(_bottomSheet, "_bottomSheetContent", grid);
			InvokePrivateMethod(_bottomSheet, "UpdateBottomSheetBackground", [brush]);
			View? content = (View?)GetPrivateField(_bottomSheet, "_bottomSheetContent");
			Assert.Equal(brush, content?.Background);
		}

		[Fact]
		public void UpdateGrabberBackground()
		{
			SfBorder border = new SfBorder();
			Brush brush = new SolidColorBrush(Colors.Red);
			SetPrivateField(_bottomSheet, "_grabber", border);
			InvokePrivateMethod(_bottomSheet, "UpdateGrabberBackground", [brush]);
			SfBorder? grabber = (SfBorder?)GetPrivateField(_bottomSheet, "_grabber");
			Assert.Equal((brush), grabber?.Background);
		}

		[Theory]
		[MemberData(nameof(CornerRadiusData))]
		public void UpdateCornerRadius(CornerRadius value, CornerRadius expected)
		{
			BottomSheetBorder bottomSheetBorder = new BottomSheetBorder(_bottomSheet);
			RoundRectangle roundRectangle = new RoundRectangle();
			bottomSheetBorder.StrokeShape = roundRectangle;
			SetPrivateField(_bottomSheet, "_bottomSheetStrokeShape", roundRectangle);
			SetPrivateField(_bottomSheet, "_bottomSheet", bottomSheetBorder);
			InvokePrivateMethod(_bottomSheet, "UpdateCornerRadius", [value]);
			BottomSheetBorder? resultBottomSheetBorder = (BottomSheetBorder?)GetPrivateField(_bottomSheet, "_bottomSheet");
			RoundRectangle? resultRoundRectangle = (RoundRectangle?)GetPrivateField(_bottomSheet, "_bottomSheetStrokeShape");
			Assert.Equal(expected, resultRoundRectangle?.CornerRadius);
			Assert.Equal(resultRoundRectangle, resultBottomSheetBorder?.StrokeShape);
		}

		[Theory]
		[MemberData(nameof(ContentPaddingData))]
		public void UpdatePadding(Thickness value, Thickness expected)
		{
			BottomSheetBorder border = new BottomSheetBorder(_bottomSheet);
			SetPrivateField(_bottomSheet, "_bottomSheet", border);
			InvokePrivateMethod(_bottomSheet, "UpdatePadding", [value]);
			BottomSheetBorder? resultBorder = (BottomSheetBorder?)GetPrivateField(_bottomSheet, "_bottomSheet");
			Assert.Equal(expected, resultBorder?.Padding);
		}

		[Theory]
		[InlineData(30, 30)]
		[InlineData(0, 4)]
		[InlineData(-28, 4)]
		public void UpdateGrabberHeightProperty(double input, double expected)
		{
			InvokePrivateMethod(_bottomSheet, "UpdateGrabberHeightProperty", input);
			SfBorder? grabber = (SfBorder?)GetPrivateField(_bottomSheet, "_grabber");
			var actual = grabber?.HeightRequest;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(48, 48)]
		[InlineData(0, 32)]
		[InlineData(-48, 32)]
		public void UpdateGrabberWidthProperty(double input, double expected)
		{
			InvokePrivateMethod(_bottomSheet, "UpdateGrabberWidthProperty", input);
			SfBorder? grabber = (SfBorder?)GetPrivateField(_bottomSheet, "_grabber");
			var actual = grabber?.WidthRequest;
			Assert.Equal(expected, actual);
		}

		public static IEnumerable<object[]> CornerRadiusGrabber =>
		new List<object[]>
		{
			new object[] { new CornerRadius(5), new CornerRadius(5) },
			new object[] { new CornerRadius(0), new CornerRadius(0) },
			new object[] { new CornerRadius(10, 5, 10, 5), new CornerRadius(10, 5, 10, 5) },
			new object[] { new CornerRadius(15, 0, 5, 10), new CornerRadius(15, 0, 5, 10) },
		};

		[Theory]
		[MemberData(nameof(CornerRadiusGrabber))]
		public void UpdateGrabberCornerRadius(CornerRadius input, CornerRadius expected)
		{
			InvokePrivateMethod(_bottomSheet, "UpdateGrabberCornerRadius", input);
			RoundRectangle? strokeShape = (RoundRectangle?)GetPrivateField(_bottomSheet, "_grabberStrokeShape");
			SfBorder? grabber = (SfBorder?)GetPrivateField(_bottomSheet, "_grabber");
			Assert.Equal(expected, strokeShape?.CornerRadius);
			Assert.Equal(grabber?.StrokeShape, strokeShape);
		}

		[Theory]
		[InlineData(220, BottomSheetAllowedState.FullExpanded, BottomSheetState.Collapsed)]
		public void HandleFullExpandedState(double swipeDistance, BottomSheetAllowedState input, BottomSheetState expected)
		{
			double swipeThreshold = 100;
			double doubleSwipeThreshold = 2 * swipeThreshold;
			var currentState = BottomSheetState.FullExpanded;
			_bottomSheet.State = currentState;
			_bottomSheet.AllowedState = input;
			InvokePrivateMethod(_bottomSheet, "HandleFullExpandedState", swipeDistance, swipeThreshold, doubleSwipeThreshold);
			var actual = _bottomSheet.State;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(-120, BottomSheetAllowedState.All, BottomSheetState.FullExpanded)]
		[InlineData(140, BottomSheetAllowedState.HalfExpanded, BottomSheetState.Collapsed)]
		public void HandleHalfExpandedState(double swipeDistance, BottomSheetAllowedState input, BottomSheetState expected)
		{
			double swipeThreshold = 100;
			var currentState = BottomSheetState.HalfExpanded;
			_bottomSheet.State = currentState;
			_bottomSheet.AllowedState = input;
			InvokePrivateMethod(_bottomSheet, "HandleHalfExpandedState", swipeDistance, swipeThreshold);
			var actual = _bottomSheet.State;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(-120, BottomSheetAllowedState.HalfExpanded, BottomSheetState.HalfExpanded)]
		[InlineData(-160, BottomSheetAllowedState.FullExpanded, BottomSheetState.FullExpanded)]
		[InlineData(-80, BottomSheetAllowedState.All, BottomSheetState.Collapsed)]
		public void HandleCollapsedState(double swipeDistance, BottomSheetAllowedState input, BottomSheetState expected)
		{
			double swipeThreshold = 100;
			var currentState = BottomSheetState.Collapsed;
			_bottomSheet.State = currentState;
			_bottomSheet.AllowedState = input;
			InvokePrivateMethod(_bottomSheet, "HandleCollapsedState", swipeDistance, swipeThreshold);
			var actual = _bottomSheet.State;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(BottomSheetState.Hidden, BottomSheetState.Collapsed)]
		[InlineData(BottomSheetState.Hidden, BottomSheetState.HalfExpanded)]
		[InlineData(BottomSheetState.Hidden, BottomSheetState.FullExpanded)]
		[InlineData(BottomSheetState.Collapsed, BottomSheetState.HalfExpanded)]
		[InlineData(BottomSheetState.Collapsed, BottomSheetState.FullExpanded)]
		[InlineData(BottomSheetState.HalfExpanded, BottomSheetState.Collapsed)]
		[InlineData(BottomSheetState.HalfExpanded, BottomSheetState.FullExpanded)]
		[InlineData(BottomSheetState.FullExpanded, BottomSheetState.HalfExpanded)]
		[InlineData(BottomSheetState.FullExpanded, BottomSheetState.Collapsed)]
		public void UpdateStateChanged(BottomSheetState oldState, BottomSheetState newState)
		{
			_bottomSheet.State = newState;
			SetPrivateField(_bottomSheet, "_isSheetOpen", true);
			InvokePrivateMethod(_bottomSheet, "UpdateStateChanged", oldState, newState);
			StateChangedEventArgs? eventArgs = (StateChangedEventArgs?)GetPrivateField(_bottomSheet, "_stateChangedEventArgs");
			Assert.NotEqual(eventArgs?.OldState, eventArgs?.NewState);
		}

		[Theory]
		[InlineData(true, true)]
		[InlineData(false, false)]
		public void SetupBottomSheetForShow(bool input, bool expected)
		{
			_bottomSheet.IsModal = input;
			InvokePrivateMethod(_bottomSheet, "SetupBottomSheetForShow");
			SfGrid? overlay = (SfGrid?)GetPrivateField(_bottomSheet, "_overlayGrid");
			SfBorder? bottomsheet = (SfBorder?)GetPrivateField(_bottomSheet, "_bottomSheet");
			Assert.True(bottomsheet?.IsVisible);
			Assert.Equal(0, overlay?.Opacity);
			Assert.Equal(expected, overlay?.IsVisible);
		}

		[Fact]
		public void GetCollapsedPosition()
		{
			InvokePrivateMethod(_bottomSheet, "GetCollapsedPosition");
			SfGrid? overlay = (SfGrid?)GetPrivateField(_bottomSheet, "_overlayGrid");
			Assert.False(overlay?.IsVisible);
		}

		[Fact]
		public void HandleTouchPressed()
		{
			double initialTouchY = 500;
			SetPrivateField(_bottomSheet, "_initialTouchY", initialTouchY);
			SetPrivateField(_bottomSheet, "_isPointerPressed", true);

			InvokePrivateMethod(_bottomSheet, "HandleTouchReleased", initialTouchY);

			Assert.False(Convert.ToBoolean(GetPrivateField(_bottomSheet, "_isPointerPressed")));
		}

		[Fact]
		public void HandleTouchReleased()
		{
			double initialTouchY = 500;
			SetPrivateField(_bottomSheet, "_initialTouchY", initialTouchY);
			SetPrivateField(_bottomSheet, "_isPointerPressed", true);

			InvokePrivateMethod(_bottomSheet,"HandleTouchReleased", initialTouchY);

			Assert.Equal(initialTouchY, GetPrivateField(_bottomSheet, "_endTouchY"));
			Assert.False(Convert.ToBoolean(GetPrivateField(_bottomSheet, "_isPointerPressed")));
		}

		#endregion

		#region Events

		public static IEnumerable<object[]> StateData =>
		new List<object[]>
		{
			new object[] { BottomSheetState.Hidden, BottomSheetState.Collapsed },
			new object[] { BottomSheetState.Hidden, BottomSheetState.HalfExpanded },
			new object[] { BottomSheetState.Hidden, BottomSheetState.FullExpanded },
			new object[] { BottomSheetState.Collapsed, BottomSheetState.HalfExpanded },
			new object[] { BottomSheetState.Collapsed, BottomSheetState.FullExpanded },
			new object[] { BottomSheetState.HalfExpanded, BottomSheetState.Collapsed },
			new object[] { BottomSheetState.HalfExpanded, BottomSheetState.FullExpanded },
			new object[] { BottomSheetState.FullExpanded, BottomSheetState.Collapsed },
			new object[] { BottomSheetState.FullExpanded, BottomSheetState.HalfExpanded }
		};

		[Fact]
		public void StateChanged()
		{
			var fired = false;
			_bottomSheet.StateChanged += (sender, e) => fired = true;
			var methodInfo = typeof(SfBottomSheet).GetMethod("UpdateStateChanged", BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (var data in StateData)
			{
				var oldState = (BottomSheetState)data[0];
				var newState = (BottomSheetState)data[1];

				methodInfo?.Invoke(_bottomSheet, new object[] { oldState, newState });
			}
			Assert.True(fired);
		}

		#endregion

	}
}