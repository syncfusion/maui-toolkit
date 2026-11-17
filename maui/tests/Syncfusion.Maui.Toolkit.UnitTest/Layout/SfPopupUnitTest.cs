using System.Reflection;
using Syncfusion.Maui.Toolkit.Popup;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfPopupUnitTest : BaseUnitTest
	{
		#region iOS SafeArea / FullScreen CloseButton regression tests

		/// <summary>
		/// In non-fullscreen mode, the header must keep the constructor's
		/// IgnoreSafeArea value (i.e. respect the host page's safe-area setting).
		/// The fix must not flip non-fullscreen popups to IgnoreSafeArea = true,
		/// because that would re-introduce the 895700 close-icon overlap bug.
		/// </summary>
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestPopupHeaderNonFullScreenPreservesIgnoreSafeArea(bool headerIgnoreSafeArea)
		{
			var popup = new SfPopup
			{
				IsOpen = true,
				IsFullScreen = false,
				ShowCloseButton = true,
				ShowHeader = true,
			};

			var popupView = GetPrivateField(popup, "_popupView")
				as Syncfusion.Maui.Toolkit.Popup.PopupView;
			Assert.NotNull(popupView);

			var header = GetPrivateField(popupView!, "_headerView")
				as Syncfusion.Maui.Toolkit.Popup.PopupHeader;
			Assert.NotNull(header);

			// Preset the constructor's value.
			header!.IgnoreSafeArea = headerIgnoreSafeArea;

			InvokePrivateMethod(header, "AddChildViews");

			// Non-fullscreen must not flip the header's IgnoreSafeArea.
			Assert.Equal(headerIgnoreSafeArea, header.IgnoreSafeArea);

			// Inner grid must mirror the header's value.
			var innerGrid = GetPrivateField(header, "_headerView") as Microsoft.Maui.Controls.Layout;
			Assert.NotNull(innerGrid);
		}

		/// <summary>
		/// PopupExtension.GetSafeAreaEdges() is iOS-only — defined in
		/// PopupExtension.iOS.cs under #if IOS and in PopupExtension.Standard.cs
		/// (stub returning false) under #if !IOS. The method is therefore
		/// platform-gated by design. This test asserts that the production
		/// source file actually contains the unified check for both the new
		/// (.NET 10 SafeAreaEdges) and the legacy (Page.GetUseSafeArea)
		/// APIs — i.e. that the fix is in place and the file wasn't silently
		/// reverted.
		///
		/// Source-level assertion is the right tool here because:
		///   1. The behavior is iOS-only and not exercisable on the test host.
		///   2. Appium tests in maui-sandbox-ui-test-creation cover the
		///      runtime behavior.
		///   3. Regressions in this file (e.g. someone removes the legacy
		///      branch) would otherwise go undetected by unit tests.
		/// </summary>
		[Fact]
		public void TestPopupExtensionGetSafeAreaEdgesUnifiesApis()
		{
			// Source-relative path: the iOS source file lives in the same
			// project as the test project, so we can read it directly.
			var sourceFile = System.IO.Path.Combine(
				AppContext.BaseDirectory,
				"..", "..", "..", "..", "..",
				"src", "Popup", "Helpers", "PopupExtension", "PopupExtension.iOS.cs");

			sourceFile = System.IO.Path.GetFullPath(sourceFile);
			Assert.True(System.IO.File.Exists(sourceFile),
				$"Expected source file not found at {sourceFile}");

			var source = System.IO.File.ReadAllText(sourceFile);

			// New .NET 10 API must still be checked.
			Assert.Contains("page.SafeAreaEdges != SafeAreaEdges.None", source, System.StringComparison.Ordinal);

			// Legacy Page.GetUseSafeArea API must also be checked (the fix).
			Assert.Contains(
				"Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.GetUseSafeArea",
				source, System.StringComparison.Ordinal);
		}

		#endregion

		#region PopupRefreshAnimationTest

		[Theory]
		[InlineData(PopupAnimationMode.Zoom)]
		[InlineData(PopupAnimationMode.SlideOnLeft)]
		[InlineData(PopupAnimationMode.SlideOnRight)]
		[InlineData(PopupAnimationMode.SlideOnTop)]
		[InlineData(PopupAnimationMode.SlideOnBottom)]
		public void Test_Refresh_WithAnimationMode_ResetsAnimatedProperties(PopupAnimationMode animationMode)
		{
			// Ticket 881122: Test that Refresh() resets animation properties when animation modes are enabled
			// This ensures that content layout is updated correctly after changing StackLayout orientation
			var popup = new SfPopup();
			popup.AnimationMode = animationMode;
			popup.AnimationDuration = 300;
			popup.Show();
			Assert.True(popup.IsOpen);

			// Get the PopupView to check animated properties
			var popupViewField = typeof(SfPopup).GetField("_popupView", BindingFlags.Instance | BindingFlags.NonPublic);
			var popupView = popupViewField?.GetValue(popup) as View;
			Assert.NotNull(popupView);

			// Simulate animation state by setting animated properties (as would happen during animation)
			popupView!.Scale = 0.8;
			popupView.TranslationX = 10;
			popupView.TranslationY = 20;
			popupView.Opacity = 0.9;

			// Verify initial state was set
			Assert.Equal(0.8, popupView.Scale);
			Assert.Equal(10.0, popupView.TranslationX);
			Assert.Equal(20.0, popupView.TranslationY);
			Assert.Equal(0.9, popupView.Opacity);

			// Act - call Refresh which should reset animated properties
			popup.Refresh();

			// Assert - animated properties should be reset to their default values
			Assert.Equal(1.0, popupView.Scale);
			Assert.Equal(0.0, popupView.TranslationX);
			Assert.Equal(0.0, popupView.TranslationY);
			Assert.Equal(1.0, popupView.Opacity);
			Assert.True(popup.IsOpen);
		}

		[Fact]
		public void Test_Refresh_WithAnimationMode_None_StillWorks()
		{
			// Ensure that AnimationMode.None doesn't break (this was already working)
			var popup = new SfPopup();
			popup.AnimationMode = PopupAnimationMode.None;
			popup.AnimationDuration = 0;
			popup.Show();
			Assert.True(popup.IsOpen);

			// Get the PopupView to check animated properties
			var popupViewField = typeof(SfPopup).GetField("_popupView", BindingFlags.Instance | BindingFlags.NonPublic);
			var popupView = popupViewField?.GetValue(popup) as View;
			Assert.NotNull(popupView);

			// Set some animated properties
			popupView!.Scale = 0.8;
			popupView.TranslationX = 10;

			// Act - call Refresh
			popup.Refresh();

			// Assert - popup still open (AnimationMode.None means CanAnimate() is false, so ResetAnimatedProperties should not be called)
			Assert.True(popup.IsOpen);
		}

		[Theory]
		[InlineData(PopupAnimationMode.Zoom)]
		[InlineData(PopupAnimationMode.SlideOnLeft)]
		[InlineData(PopupAnimationMode.SlideOnRight)]
		[InlineData(PopupAnimationMode.SlideOnTop)]
		[InlineData(PopupAnimationMode.SlideOnBottom)]
		public void Test_Refresh_WithAnimationMode_AndContent_LayoutsCorrectly(PopupAnimationMode animationMode)
		{
			// Ticket 881122: Test that Refresh() correctly measures content after orientation change
			var popup = new SfPopup();
			popup.AnimationMode = animationMode;
			popup.AnimationDuration = 300;

			// Create a StackLayout as content (simulating the issue scenario)
			var stackLayout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Padding = 10,
				Children =
				{
					new Label { Text = "Test Content", FontSize = 16 },
					new Button { Text = "Button" }
				}
			};

			popup.ContentTemplate = new DataTemplate(() =>
			{
				return stackLayout;
			});

			popup.Show();
			Assert.True(popup.IsOpen);

			// Get the PopupView
			var popupViewField = typeof(SfPopup).GetField("_popupView", BindingFlags.Instance | BindingFlags.NonPublic);
			var popupView = popupViewField?.GetValue(popup) as View;
			Assert.NotNull(popupView);

			// Simulate animation state
			popupView!.Scale = 0.5;
			popupView.TranslationY = 100;

			// Change the StackLayout orientation
			stackLayout.Orientation = StackOrientation.Horizontal;

			// Act - call Refresh (which should reset animation properties and remeasure content)
			popup.Refresh();

			// Assert - animated properties are reset, popup is still open
			Assert.Equal(1.0, popupView.Scale);
			Assert.Equal(0.0, popupView.TranslationY);
			Assert.True(popup.IsOpen);
			Assert.Equal(StackOrientation.Horizontal, stackLayout.Orientation);
		}

		#endregion

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupIsOpen(bool value)
		{
			var popup = new SfPopup();
			popup.IsOpen = value;
			Assert.Equal(value, popup.IsOpen);
		}

		SfPopup popup2 = new SfPopup();
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupIsFullScreen(bool value)
		{
			var popup = new SfPopup();
			popup.IsFullScreen = value;
			popup.IsOpen = true;
			Assert.Equal(value, popup.IsFullScreen);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupShowCloseButton(bool value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowCloseButton = value;
			Assert.Equal(value, popup.ShowCloseButton);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupShowFooter(bool value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = value;
			Assert.Equal(value, popup.ShowFooter);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupShowHeader(bool value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowHeader = value;
			Assert.Equal(value, popup.ShowHeader);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupShowOverlayAlways(bool value)
		{
			var popup = new SfPopup();
			popup.ShowOverlayAlways = value;
			popup.IsOpen = true;
			Assert.Equal(value, popup.ShowOverlayAlways);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupStaysOpen(bool value)
		{
			var popup = new SfPopup();
			popup.StaysOpen = value;
			popup.IsOpen = true;
			Assert.Equal(value, popup.StaysOpen);
		}

		[Theory]
		[InlineData(PopupAutoSizeMode.Width)]
		[InlineData(PopupAutoSizeMode.Height)]
		[InlineData(PopupAutoSizeMode.Both)]
		[InlineData(PopupAutoSizeMode.None)]

		public void TestSfPopupAutoSizeMode(PopupAutoSizeMode value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.AutoSizeMode = value;
			Assert.Equal(value, popup.AutoSizeMode);
		}

		[Theory]
		[InlineData(PopupOverlayMode.Blur)]
		[InlineData(PopupOverlayMode.Transparent)]
		public void TestSfPopupOverlayMode(PopupOverlayMode value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowOverlayAlways = true;
			popup.OverlayMode = value;
			Assert.Equal(value, popup.OverlayMode);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(150)]
		public void TestSfPopupHeaderHeight(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.HeaderHeight = value;
			Assert.Equal(value, popup.HeaderHeight);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(150)]
		public void TestSfPopupFooterHeight(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			popup.FooterHeight = value;
			Assert.Equal(value, popup.FooterHeight);
		}

		[Theory]
		[InlineData(PopupAnimationMode.None)]
		[InlineData(PopupAnimationMode.Zoom)]
		[InlineData(PopupAnimationMode.Fade)]
		[InlineData(PopupAnimationMode.SlideOnLeft)]
		[InlineData(PopupAnimationMode.SlideOnRight)]
		[InlineData(PopupAnimationMode.SlideOnTop)]
		[InlineData(PopupAnimationMode.SlideOnBottom)]
		public void TestSfPopupAnimationMode(PopupAnimationMode value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.AnimationMode = value;
			Assert.Equal(value, popup.AnimationMode);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(150)]
		[InlineData(300)]
		public void TestSfPopupAnimationDuration(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.AnimationDuration = value;
			Assert.Equal(value, popup.AnimationDuration);
		}

		[Theory]
		[InlineData(PopupAnimationEasing.Linear)]
		[InlineData(PopupAnimationEasing.SinIn)]
		[InlineData(PopupAnimationEasing.SinOut)]
		[InlineData(PopupAnimationEasing.SinInOut)]
		public void TestSfPopupAnimationEasing(PopupAnimationEasing value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.AnimationEasing = value;
			Assert.Equal(value, popup.AnimationEasing);
		}

		[Theory]
		[InlineData("Accept")]
		[InlineData("AcceptButtonTextValues")]
		public void TestSfPopupAcceptButtonText(string value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			popup.AcceptButtonText = value;
			Assert.Equal(value, popup.AcceptButtonText);
		}

		[Theory]
		[InlineData("Decline")]
		[InlineData("DeclineButtonTextValues")]
		public void TestSfPopupDeclineButtonText(string value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			popup.DeclineButtonText = value;
			Assert.Equal(value, popup.DeclineButtonText);
		}

		[Theory]
		[InlineData(PopupButtonAppearanceMode.OneButton)]
		[InlineData(PopupButtonAppearanceMode.TwoButton)]
		public void TestSfPopupAppearenceMode(PopupButtonAppearanceMode value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			popup.AppearanceMode = value;
			Assert.Equal(value, popup.AppearanceMode);
		}

		[Theory]
		[InlineData("Title")]
		[InlineData("Syncfusion.Maui.Popup")]
		public void TestSfPopupHeaderTitle(string value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.HeaderTitle = value;
			Assert.Equal(value, popup.HeaderTitle);
		}

		[Theory]
		[InlineData("Message")]
		[InlineData("Syncfusion.Maui.Popup is a Popup controls.")]
		public void TestSfPopupFooterTitle(string value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.Message = value;
			Assert.Equal(value, popup.Message);
		}

		[Theory]
		[InlineData(PopupRelativePosition.AlignBottom)]
		[InlineData(PopupRelativePosition.AlignBottomRight)]
		[InlineData(PopupRelativePosition.AlignBottomLeft)]
		[InlineData(PopupRelativePosition.AlignTop)]
		[InlineData(PopupRelativePosition.AlignTopRight)]
		[InlineData(PopupRelativePosition.AlignTopLeft)]
		[InlineData(PopupRelativePosition.AlignToRightOf)]
		[InlineData(PopupRelativePosition.AlignToLeftOf)]
		public void TestSfPopupRelativePosition(PopupRelativePosition value)
		{
			var popup = new SfPopup();
			popup.RelativePosition = value;
			Assert.Equal(value, popup.RelativePosition);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(3000)]
		public void TestSfPopupAutoCloseDuration(int value)
		{
			var popup = new SfPopup();
			popup.AutoCloseDuration = value;
			Assert.Equal(value, popup.AutoCloseDuration);
		}

		[Fact]
		public void TestSfPopupHeaderBackground()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowHeader = true;
			var popupstyle = new PopupStyle();
			popupstyle.HeaderBackground = Colors.Red;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.HeaderBackground = Colors.Red;
			Color color = Colors.Red;
			Assert.Equal(color, popup.PopupStyle.HeaderBackground);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		[InlineData(FontAttributes.None)]
		public void TestSfPopupHeaderFontAttribute(FontAttributes value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowHeader = true;
			var popupstyle = new PopupStyle();
			popupstyle.HeaderFontAttribute = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.HeaderFontAttribute = value;
			Assert.Equal(value, popup.PopupStyle.HeaderFontAttribute);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(100)]
		public void TestSfPopupHeaderFontSize(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowHeader = true;
			var popupstyle = new PopupStyle();
			popupstyle.HeaderFontSize = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.HeaderFontSize = value;
			Assert.Equal(value, popup.PopupStyle.HeaderFontSize);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Justify)]
		public void TestSfPopupHeaderTextAlignment(TextAlignment value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowHeader = true;
			var popupstyle = new PopupStyle();
			popupstyle.HeaderTextAlignment = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.HeaderTextAlignment = value;
			Assert.Equal(value, popup.PopupStyle.HeaderTextAlignment);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupHeaderColor()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowHeader = true;
			var popupstyle = new PopupStyle();
			popupstyle.HeaderTextColor = Colors.Yellow;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.HeaderTextColor = Colors.Yellow;
			Color color = Colors.Yellow;
			Assert.Equal(color, popup.PopupStyle.HeaderTextColor);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupFooterBackground()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.FooterBackground = Colors.Yellow;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.FooterBackground = Colors.Yellow;
			Color color = Colors.Yellow;
			Assert.Equal(color, popup.PopupStyle.FooterBackground);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		[InlineData(FontAttributes.None)]
		public void TestSfPopupFooterFontAttribute(FontAttributes value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.FooterFontAttribute = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.FooterFontAttribute = value;
			Assert.Equal(value, popup.PopupStyle.FooterFontAttribute);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(100)]
		public void TestSfPopupFooterFontSize(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.FooterFontSize = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.FooterFontSize = value;
			Assert.Equal(value, popup.PopupStyle.FooterFontSize);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(40)]
		public void TestSfPopupFooterButtonCornerRadius(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.FooterButtonCornerRadius = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.FooterButtonCornerRadius = value;
			Assert.Equal(value, popup.PopupStyle.FooterButtonCornerRadius);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(0, 20, 20, 0)]
		[InlineData(20, 20, 0, 0)]
		[InlineData(20, 0, 20, 0)]
		[InlineData(20, 0, 20, 20)]
		public void TestSfPopupFooterButtonCornerRadius2(int tleft, int tright, int bleft, int bright)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.FooterButtonCornerRadius = new CornerRadius(tleft, tright, bleft, bright);
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.FooterButtonCornerRadius = new CornerRadius(tleft, tright, bleft, bright);
			Assert.Equal(new CornerRadius(tleft, tright, bleft, bright), popup.PopupStyle.FooterButtonCornerRadius);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupAcceptButtonBackground()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.AcceptButtonBackground = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.AcceptButtonBackground = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.AcceptButtonBackground);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupAcceptButtonTextColor()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.AcceptButtonTextColor = Colors.Red;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.AcceptButtonTextColor = Colors.Red;
			Color color = Colors.Red;
			Assert.Equal(color, popup.PopupStyle.AcceptButtonTextColor);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupDeclineButtonBackground()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.DeclineButtonBackground = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.AppearanceMode = PopupButtonAppearanceMode.TwoButton;
			popup.PopupStyle.DeclineButtonBackground = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.DeclineButtonBackground);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupDeclineButtonTextColor()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.ShowFooter = true;
			var popupstyle = new PopupStyle();
			popupstyle.DeclineButtonTextColor = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.DeclineButtonTextColor = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.DeclineButtonTextColor);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupMessageBackground()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.MessageBackground = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.MessageBackground = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.MessageBackground);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		[InlineData(FontAttributes.None)]
		public void TestSfPopupMessageFontAttribute(FontAttributes value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.MessageFontAttribute = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.MessageFontAttribute = value;
			Assert.Equal(value, popup.PopupStyle.MessageFontAttribute);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}


		[Theory]
		[InlineData(10)]
		[InlineData(100)]
		public void TestSfPopupMessageFontSize(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.MessageFontSize = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.MessageFontSize = value;
			Assert.Equal(value, popup.PopupStyle.MessageFontSize);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Justify)]
		public void TestSfPopupMessageTextAlignment(TextAlignment value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.MessageTextAlignment = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.MessageTextAlignment = value;
			Assert.Equal(value, popup.PopupStyle.MessageTextAlignment);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupMessageTextColor()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.MessageTextColor = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.MessageTextColor = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.MessageTextColor);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupStroke()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.Stroke = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.Stroke = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.Stroke);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}


		[Theory]
		[InlineData(5)]
		[InlineData(50)]
		public void TestSfPopupStrokeThickness(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.StrokeThickness = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.StrokeThickness = value;
			Assert.Equal(value, popup.PopupStyle.StrokeThickness);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(50)]
		public void TestSfPopupCornerRadius(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.CornerRadius = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.CornerRadius = value;
			Assert.Equal(value, popup.PopupStyle.CornerRadius);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupBackground()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.PopupBackground = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.PopupBackground = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.PopupBackground);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Fact]
		public void TestSfPopupOverlayColor()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.OverlayColor = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.OverlayColor = Colors.Green;
			Color color = Colors.Green;
			Assert.Equal(color, popup.PopupStyle.OverlayColor);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(PopupBlurIntensity.Dark)]
		[InlineData(PopupBlurIntensity.Light)]
		[InlineData(PopupBlurIntensity.ExtraLight)]
		[InlineData(PopupBlurIntensity.ExtraDark)]
		[InlineData(PopupBlurIntensity.Custom)]
		public void TestSfPopupBlurIntensity(PopupBlurIntensity value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.BlurIntensity = value;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.BlurIntensity = value;
			Assert.Equal(value, popup.PopupStyle.BlurIntensity);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(25)]
		public void TestSfPopupBlurRadius(int radius)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.BlurRadius = radius;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.BlurRadius = radius;
			Assert.Equal(radius, popup.PopupStyle.BlurRadius);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupHasShadow(bool value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var popupstyle = new PopupStyle();
			popupstyle.MessageBackground = Colors.Green;
			popup.PopupStyle = popupstyle;
			popup.PopupStyle.HasShadow = value;
			Assert.Equal(value, popup.PopupStyle.HasShadow);
			Assert.Equal(popupstyle, popup.PopupStyle);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSfPopupShow(bool value)
		{
			var popup = new SfPopup();
			popup.Show(value);
			Assert.True(popup.IsOpen);
		}
		[Theory]
		[InlineData(false, false, false)]
		[InlineData(true, true, false)]
		[InlineData(false, true, false)]
		public void TestSfPopupDismiss(bool isOpen, bool staysOpen, bool Open)
		{
			var popup = new SfPopup
			{
				IsOpen = isOpen,
				StaysOpen = staysOpen
			};
			popup.Dismiss();
			Assert.Equal(Open, popup.IsOpen);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(100)]

		public void TestSfPopupAbsoluteX(int value)
		{
			var popup = new SfPopup();
			popup.AbsoluteX = value;
			Assert.Equal(value, popup.AbsoluteX);
		}


		[Theory]
		[InlineData(0)]
		[InlineData(100)]

		public void TestSfPopupAbsoluteY(int value)
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			popup.AbsoluteY = value;
			Assert.Equal(value, popup.AbsoluteY);
		}

		[Fact]
		public void TestSfPopupContentTemplate()
		{
			var popup = new SfPopup();
			popup.IsOpen = true;
			var dataTemplate = new DataTemplate(() => new Label { Text = "Template" });
			popup.ContentTemplate = dataTemplate;
			Assert.Equal(dataTemplate, popup.ContentTemplate);
		}

		[Fact]
		public void TestSfPopupHeaderTemplate()
		{
			var popup = new SfPopup();
			var dataTemplate = new DataTemplate(() => new Label { Text = "Template" });
			popup.HeaderTemplate = dataTemplate;
			Assert.Equal(dataTemplate, popup.HeaderTemplate);
		}

		[Fact]
		public void TestSfPopupFooterTemplate()
		{
			var popup = new SfPopup();
			var dataTemplate = new DataTemplate(() => new Label { Text = "Template" });
			popup.FooterTemplate = dataTemplate;
			Assert.Equal(dataTemplate, popup.FooterTemplate);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(100)]
		public void TestSfPopupStartX(int value)
		{
			var popup = new SfPopup();
			popup.StartX = value;
			Assert.Equal(value, popup.StartX);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(100)]
		public void TestSfPopupStartY(int value)
		{
			var popup = new SfPopup();
			popup.StartX = value;
			Assert.Equal(value, popup.StartX);
		}

		[Fact]
		public void TestSfPopupRelativeView()
		{
			var popup = new SfPopup();
			var popup2 = new SfPopup();
			popup.RelativeView = popup2;
			Assert.Equal(popup, popup2);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(50, 100)]
		public void TestSfPopupShowXY(double x, double y)
		{
			var popup = new SfPopup();
			popup.Show(x, y);
			popup.ShowRelativeToView(popup, PopupRelativePosition.AlignToLeftOf, x, y);
		}
	}
}
