using CoreGraphics;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using UIKit;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformView = UIKit.UIView;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Represents the <see cref="PopupExtension"/> class for iOS platform.
	/// </summary>
	internal static partial class PopupExtension
	{
		#region Internal Methods

		/// <summary>
		/// Gets the width of the device.
		/// </summary>
		/// <returns>The width of the device.</returns>
		internal static int GetScreenWidth()
		{
			var rootView = WindowOverlayHelper._platformRootView;
			return rootView is not null ? (int)rootView.Frame.Width : 0;
		}

		/// <summary>
		/// Gets the height of the device.
		/// </summary>
		/// <returns>The height of the device.</returns>
		internal static int GetScreenHeight()
		{
			var rootView = WindowOverlayHelper._platformRootView;
			return rootView is not null ? (int)rootView.Frame.Height : 0;
		}

		/// <summary>
		/// Gets the X coordinate of the view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Returns the X coordinate of the view.</returns>
		internal static int GetX(this MauiView view)
		{
			IMauiContext? context = view.Handler?.MauiContext;
			if (context is not null)
			{
				PlatformView nativeView = view.ToPlatform(context);
				if (nativeView is not null)
				{
					return (int)nativeView.Frame.X;
				}
			}

			return 0;
		}

		/// <summary>
		/// Gets the Y coordinate of the view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Returns the Y coordinate of the view.</returns>
		internal static int GetY(this MauiView view)
		{
			IMauiContext? context = view.Handler?.MauiContext;
			if (context is not null)
			{
				PlatformView nativeView = view.ToPlatform(context);
				if (nativeView is not null)
				{
					return (int)nativeView.Frame.Y;
				}
			}

			return 0;
		}

		/// <summary>
		/// Gets the status bar height.
		/// </summary>
		/// <returns>Returns the status bar height.</returns>
		internal static int GetStatusBarHeight()
		{
			var platformWindow = WindowOverlayHelper._window?.ToPlatform() as UIWindow;

			// The status bar height is incorrect when using the StatusBarFrame height on iOS; therefore, we used SafeAreaInsets instead.
			return platformWindow is not null ? (int)platformWindow.SafeAreaInsets.Top : 0;
		}

		/// <summary>
		/// Gets action bar height.
		/// </summary>
		/// <param name="ignoreActionBar">Specifies whether the popup should consider action bar or not.</param>
		/// <returns>Returns action bar height.</returns>
		internal static int GetActionBarHeight(bool ignoreActionBar)
		{
			// The action bar height is calculated using the top value of the safe area.
			return 0;
		}

		/// <summary>
		/// This method will returns the SafeAreaHeight.
		/// </summary>
		/// <param name="position">Position of the safe area.</param>
		/// <returns>Returns the SafeAreaHeight.</returns>
		internal static int GetSafeAreaHeight(string position)
		{
			// The popup is drawn within the safe area when shown relative to a view. Safe area calculations apply only to the iOS X simulator on iOS.
			if (Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.GetUseSafeArea(GetMainPage()))
			{
				var platformWindow = WindowOverlayHelper._window?.ToPlatform() as UIWindow;
				var statusBarOrientation = platformWindow?.WindowScene?.InterfaceOrientation;

				// Since StatusBarHeight is already calculated, the top safe area does not need to be computed.
				if (position == "Top")
				{
					// To calculate the navigationBar height with the page Y position.
					if (GetMainWindowPage() is Shell shellpage && shellpage.Handler is not null && shellpage.Handler.PlatformView is UIView shellNativeView &&
						GetMainPage() is Page page && page.Handler is not null && page.Handler.PlatformView is UIView pageNativeView && platformWindow is not null)
					{
						var pageYposition = pageNativeView.ConvertPointToView(new CGPoint(0, 0), shellNativeView);
						return (int)pageYposition.Y - (int)platformWindow.SafeAreaInsets.Top;
					}

					PlatformView? navigationBar = GetNavigationBar();
					return navigationBar is not null ? (int)navigationBar.Frame.Height : 0;
				}
				else if ((statusBarOrientation == UIInterfaceOrientation.Portrait || statusBarOrientation == UIInterfaceOrientation.PortraitUpsideDown || statusBarOrientation == UIInterfaceOrientation.LandscapeLeft || statusBarOrientation == UIInterfaceOrientation.LandscapeRight || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceUp || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceDown) && position == "Bottom")
				{
					return platformWindow?.SafeAreaInsets.Bottom > 0 ? (int)platformWindow.SafeAreaInsets.Bottom : 0;
				}
				else if ((UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceUp || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceDown) && position == "Right" && GetScreenWidth() > GetScreenHeight())
				{
					return platformWindow?.SafeAreaInsets.Right > 0 ? (int)platformWindow.SafeAreaInsets.Right : 0;
				}
				else if ((UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceUp || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.FaceDown) && (position == "Left") && GetScreenWidth() > GetScreenHeight())
				{
					return platformWindow?.SafeAreaInsets.Left > 0 ? (int)platformWindow.SafeAreaInsets.Left : 0;
				}
				else if (statusBarOrientation == UIInterfaceOrientation.LandscapeLeft)
				{
					return platformWindow?.SafeAreaInsets.Left > 0 ? (int)platformWindow.SafeAreaInsets.Left : 0;
				}
				else if (statusBarOrientation == UIInterfaceOrientation.LandscapeRight)
				{
					return platformWindow?.SafeAreaInsets.Right > 0 ? (int)platformWindow.SafeAreaInsets.Right : 0;
				}
			}

			return 0;
		}

		/// <summary>
		/// Used to applied the blur effect for popup.
		/// </summary>
		/// <param name="view">The instance of the MauiView.</param>
		/// <param name="popup">The instance of the SfPopup.</param>
		/// <param name="isopen">Specifies whether the popup is open or not.</param>
		internal static void Blur(this MauiView view, SfPopup popup, bool isopen)
		{
			if (isopen && popup is not null)
			{
				popup._blurView = new UIVisualEffectView(UIBlurEffect.FromStyle(GetUIBlurStyle(popup)))
				{
					Frame = new System.Drawing.RectangleF(0, 0, (float)GetScreenWidth(), (float)GetScreenHeight()),

					// Set the parent size for the child when the orientation changes.
					AutoresizingMask = UIViewAutoresizing.All,
				};

				CheckSubView(popup);
			}
		}

		/// <summary>
		/// Used to clear the blur effect for popup.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		internal static void ClearBlurViews(SfPopup popup)
		{
			if (popup._blurView is not null)
			{
				popup._blurView.RemoveFromSuperview();
				popup._blurView.Dispose();
				popup._blurView = null;
			}
		}

		/// <summary>
		/// Updates blurios frame when resize the window.
		/// </summary>
		/// <param name="popup"> The instance of the SfPopup.</param>
		internal static void UpdateBluriosFrame(SfPopup popup)
		{
			if (popup._blurView is not null)
			{
				popup._blurView.Frame = new System.Drawing.RectangleF(0, 0, (float)GetScreenWidth(), (float)GetScreenHeight());
			}
		}

		/// <summary>
		/// Calculates the X and Y point of the popup, relative to the given view.
		/// </summary>
		/// <param name="popupView">popup view to display in the view.</param>
		/// <param name="relative">Positions the popup view relatively to the relative view.</param>
		/// <param name="position">The relative position from the view.</param>
		/// <param name="absoluteX">Absolute X Point where the popup should be positioned from the relative view.</param>
		/// <param name="absoluteY">Absolute Y-Point where the popup should be positioned from the relative view.</param>
		/// <param name="relativeX">References the X position of popup relative to view.</param>
		/// <param name="relativeY">References the Y position of popup relative to view.</param>
		internal static void CalculateRelativePoint(PopupView popupView, MauiView relative, PopupRelativePosition position, double absoluteX, double absoluteY, ref double relativeX, ref double relativeY)
		{
			var rootView = WindowOverlayHelper._platformRootView;
			if (relative is null || relative.Handler is null || relative.Handler.MauiContext is null)
			{
				return;
			}

			PlatformView relativeView = relative.ToPlatform(relative.Handler.MauiContext);

			var popupViewWidth = popupView._popup._popupViewWidth;
			var popupViewHeight = popupView._popup._popupViewHeight;

			var heightOfRelativeView = relativeView.Frame.Height;
			var widthOfRelativeView = relativeView.Frame.Width;

			nfloat[] location = new nfloat[2];
			CGPoint relativeViewOrigin = relativeView.ConvertPointToView(new CGPoint(0, 0), rootView);

			location[0] = relativeViewOrigin.X;
			location[1] = relativeViewOrigin.Y;

			// Adds absolute points to the location of the relative view.
			location[0] += (float)absoluteX;
			location[1] += (float)absoluteY;

			var screenHeight = GetScreenHeight();
			var screenWidth = GetScreenWidth();
			var safeAreaHeightAtLeft = GetSafeAreaHeight("Left");
			var safeAreaHeightAtRight = GetSafeAreaHeight("Right");
			var safeAreaHeightAtTop = GetSafeAreaHeight("Top");
			var safeAreaHeightAtBottom = GetSafeAreaHeight("Bottom");

			// Calculates the X-position relative to the specified view.
			CalculateXPosition(popupView, position, ref relativeX, absoluteX, popupViewWidth, location, widthOfRelativeView, screenWidth, safeAreaHeightAtLeft, safeAreaHeightAtRight);

			// Calculates the Y-position relative to the specified view.
			CalculateYPosition(popupView, position, ref relativeY, popupViewHeight, location, heightOfRelativeView, screenHeight, safeAreaHeightAtTop, safeAreaHeightAtBottom);
		}

		#endregion

		#region Private Methods

		static void CheckSubView(SfPopup popup)
		{
			if (popup is not null && popup._popupOverlayContainer is not null)
			{
				var subview = popup._popupOverlayContainer.ToPlatform();

				if (subview is not null && popup._blurView is not null)
				{
					// A black background appears because the blur view is added multiple times. To prevent this, a check is added to return early if the first subview is a UIVisualEffectView.
					if (subview.Subviews.Count() > 0 && subview.Subviews[0] is UIVisualEffectView uiVisualEffectView && uiVisualEffectView is not null && uiVisualEffectView.Effect is not null && uiVisualEffectView.Effect.Equals(popup._blurView.Effect))
					{
						return;
					}

					if (WindowOverlayHelper._platformRootView is not null)
					{
						subview.InsertSubview(popup._blurView, 0);
					}
				}
			}
		}

		/// <summary>
		/// Calculates the X position of the Popup.
		/// </summary>
		/// <param name="popupView"></param>
		/// <param name="position"></param>
		/// <param name="relativeX"></param>
		/// <param name="absoluteX"></param>
		/// <param name="popupViewWidth"></param>
		/// <param name="location"></param>
		/// <param name="widthOfRelativeView"></param>
		/// <param name="screenWidth"></param>
		/// <param name="safeAreaHeightAtLeft"></param>
		/// <param name="safeAreaHeightAtRight"></param>
		static void CalculateXPosition(PopupView popupView, PopupRelativePosition position, ref double relativeX, double absoluteX, double popupViewWidth, nfloat[] location, nfloat widthOfRelativeView, float screenWidth, int safeAreaHeightAtLeft, int safeAreaHeightAtRight)
		{
			if (position == PopupRelativePosition.AlignToLeftOf || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignBottomLeft)
			{
				if (popupView._popup._isRTL)
				{
					relativeX = location[0] - (2 * absoluteX) - popupViewWidth - safeAreaHeightAtLeft > 0 ? location[0] - (2 * absoluteX) - popupViewWidth : safeAreaHeightAtLeft;
				}
				else
				{
					relativeX = location[0] - popupViewWidth - safeAreaHeightAtLeft > 0 ? location[0] - popupViewWidth : safeAreaHeightAtLeft;
				}
			}
			else if (position == PopupRelativePosition.AlignToRightOf || position == PopupRelativePosition.AlignTopRight || position == PopupRelativePosition.AlignBottomRight)
			{
				if (popupView._popup._isRTL)
				{
					relativeX = location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth - safeAreaHeightAtLeft > 0 ? location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth : safeAreaHeightAtLeft;
				}
				else
				{
					relativeX = location[0] + widthOfRelativeView + popupViewWidth < screenWidth - safeAreaHeightAtRight ? location[0] + widthOfRelativeView : screenWidth - safeAreaHeightAtRight - popupViewWidth;
				}
			}
			else
			{
				if (popupView._popup._isRTL)
				{
					relativeX = location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth - safeAreaHeightAtLeft > 0 ? location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth : safeAreaHeightAtLeft;
				}
				else
				{
					relativeX = location[0] + popupViewWidth < screenWidth - safeAreaHeightAtRight ? location[0] : screenWidth - safeAreaHeightAtRight - popupViewWidth;
				}
			}
		}

		/// <summary>
		/// Calsulates the Y Position of the Popup.
		/// </summary>
		/// <param name="popupView"></param>
		/// <param name="position"></param>
		/// <param name="relativeY"></param>
		/// <param name="popupViewHeight"></param>
		/// <param name="location"></param>
		/// <param name="heightOfRelativeView"></param>
		/// <param name="screenHeight"></param>
		/// <param name="safeAreaHeightAtTop"></param>
		/// <param name="safeAreaHeightAtBottom"></param>
		static void CalculateYPosition(PopupView popupView, PopupRelativePosition position, ref double relativeY, double popupViewHeight, nfloat[] location, nfloat heightOfRelativeView, float screenHeight, int safeAreaHeightAtTop, int safeAreaHeightAtBottom)
		{
			if (position == PopupRelativePosition.AlignTop || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignTopRight)
			{
				relativeY = location[1] - popupViewHeight - (GetStatusBarHeight() + (popupView._popup.IgnoreActionBar ? 0 : safeAreaHeightAtTop)) > 0 ? location[1] - popupViewHeight : (GetStatusBarHeight() + (popupView._popup.IgnoreActionBar ? 0 : safeAreaHeightAtTop));
			}
			else if (position == PopupRelativePosition.AlignBottom || position == PopupRelativePosition.AlignBottomLeft || position == PopupRelativePosition.AlignBottomRight)
			{
				relativeY = location[1] + heightOfRelativeView + popupViewHeight < screenHeight - safeAreaHeightAtBottom ? location[1] + heightOfRelativeView : screenHeight - safeAreaHeightAtBottom - popupViewHeight;
			}
			else
			{
				relativeY = location[1] + popupViewHeight < screenHeight - safeAreaHeightAtBottom ? location[1] : screenHeight - safeAreaHeightAtBottom - popupViewHeight;
			}
		}

		/// <summary>
		/// Checks for navigation bar.
		/// </summary>
		/// <returns>Returns true if navigation bar present. Else returns false.</returns>
		static PlatformView? CheckNavigationBar()
		{
			Page? page = GetMainPage(true);
			if (page is NavigationPage navigationPage)
			{
				page = navigationPage;
			}
			else if (page is TabbedPage tabbedPage && tabbedPage.CurrentPage is not null &&
			  tabbedPage.CurrentPage is NavigationPage tabNavPage)
			{
				page = tabNavPage;
			}
			else if (page is FlyoutPage flyoutPage && flyoutPage.Detail is not null &&
				flyoutPage.Detail is NavigationPage flyNavPage)
			{
				page = flyNavPage;
			}

			if (page is NavigationPage)
			{
				var window = (PlatformView?)page.Handler?.PlatformView;
				if (window is not null && (window.Subviews.Any(x => !string.IsNullOrEmpty(x.Class.Name) && x.Class.Name.Equals("UINavigationTransitionView", StringComparison.OrdinalIgnoreCase)) || window.Subviews.Any(x => !string.IsNullOrEmpty(x.Class.Name) && x.Class.Name.Equals("Microsoft_Maui_Controls_Handlers_Compatibility_NavigationRenderer_MauiControlsNavigationBar", StringComparison.OrdinalIgnoreCase))))
				{
					return window;
				}

				page = null;
				return window;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the navigation bar.
		/// </summary>
		/// <returns>Returns the navigation bar.</returns>
		static PlatformView? GetNavigationBar()
		{
			PlatformView? uiLayoutContainerView = CheckNavigationBar();

			if (uiLayoutContainerView is not null)
			{
				return uiLayoutContainerView?.Subviews?.FirstOrDefault(x => !string.IsNullOrEmpty(x.Class.Name) && x.Class.Name.Equals("Microsoft_Maui_Controls_Handlers_Compatibility_NavigationRenderer_MauiControlsNavigationBar", StringComparison.OrdinalIgnoreCase));
			}
			else
			{
				return uiLayoutContainerView;
			}
		}

		/// <summary>
		/// Gets the UI blur effect style to determine the blur intensity.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		/// <returns>Return UI blur style value based on the defined blur intensity value.</returns>
		static UIBlurEffectStyle GetUIBlurStyle(this SfPopup popup)
		{
			if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Light)
			{
				return UIBlurEffectStyle.Light;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraLight)
			{
				return UIBlurEffectStyle.ExtraLight;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Custom)
			{
				return UIBlurEffectStyle.Light;
			}
			else
			{
				return UIBlurEffectStyle.Dark;
			}
		}

		#endregion
	}
}