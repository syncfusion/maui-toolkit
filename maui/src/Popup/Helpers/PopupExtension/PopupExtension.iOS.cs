using CoreGraphics;
using CoreImage;
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
		internal static double GetStatusBarHeight()
		{
			return 0;
		}

		/// <summary>
		/// Gets action bar height.
		/// </summary>
		/// <returns>Returns action bar height.</returns>
		internal static int GetActionBarHeight()
		{
			var platformWindow = WindowOverlayHelper._window?.ToPlatform() as UIWindow;

			// To calculate the navigationBar height with the page Y position.
			if (GetMainPage() is Page page && page.Handler is not null && page.Handler.PlatformView is UIView pageNativeView && platformWindow is not null)
			{
				CGPoint pagePosition;
				if (GetMainWindowPage() is Shell shellpage && shellpage.Handler is not null && shellpage.Handler.PlatformView is UIView shellNativeView)
				{
					pagePosition = pageNativeView.ConvertPointToView(new CGPoint(0, 0), shellNativeView);
				}
				else
				{
					pagePosition = pageNativeView.ConvertPointToView(new CGPoint(0, 0), platformWindow);
				}

				return Math.Max(0, (int)pagePosition.Y - GetSafeAreaHeight("Top"));
			}

			PlatformView? navigationBar = GetNavigationBar();
			return navigationBar is not null ? (int)navigationBar.Frame.Height : 0;
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
					return platformWindow is not null ? (int)platformWindow.SafeAreaInsets.Top : 0;
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
			if (isopen && popup is not null && view.Handler is not null && view.Handler.PlatformView is not null && view.Handler.PlatformView is UIView platformOverlayView)
			{
				ClearBlurViews(popup);
				if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.Custom)
				{
					ApplyCustomBlur(popup, platformOverlayView);
				}
				else
				{
					popup._blurView = new UIVisualEffectView(UIBlurEffect.FromStyle(GetUIBlurStyle(popup)))
					{
						Frame = new System.Drawing.RectangleF(0, 0, GetScreenWidth(), GetScreenHeight()),

						// Set the parent size for the child when the orientation changes.
						AutoresizingMask = UIViewAutoresizing.All,
					};

					if (WindowOverlayHelper._platformRootView is not null)
					{
						platformOverlayView.InsertSubview(popup._blurView, 0);
					}
				}
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

			if (popup._customBlurView != null)
			{
				popup._customBlurView.RemoveFromSuperview();
				popup._customBlurView.Dispose();
				popup._customBlurView = null;
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
		/// Determines the bounds of a relative view in screen coordinates.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		/// <param name="relativeView">The view for which to calculate the screen bounds.</param>
		/// <returns>Returns a <see cref="Rect"/> that represents the bounds of the view in screen coordinates.</returns>
		internal static Rect GetRelativeViewBounds(this SfPopup popup, MauiView relativeView)
		{
			if (relativeView.Handler != null && relativeView.Handler.PlatformView != null && relativeView.Handler.PlatformView is PlatformView nativeRelativeView
				&& WindowOverlayHelper._platformRootView != null)
			{
				PlatformView rootView = WindowOverlayHelper._platformRootView;
				nfloat[] location = new nfloat[2];
				CGPoint relativeViewOrigin = nativeRelativeView.ConvertPointToView(new CGPoint(0, 0), rootView);
				return new Rect(relativeViewOrigin.X, relativeViewOrigin.Y, nativeRelativeView.Frame.Width, nativeRelativeView.Frame.Height);
			}
			else
			{
				return Rect.Zero;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Applies a custom blur to the popup overlay.
		/// </summary>
		/// <param name="popup">The popup instance.</param>
		/// <param name="platformOverlayView">The overlay view to apply the blur on.</param>
		static void ApplyCustomBlur(SfPopup popup, UIView platformOverlayView)
		{
			var keyWindow = platformOverlayView.Superview;
			if (keyWindow is not null && keyWindow.Subviews.Count() > 1 && keyWindow.Layer is not null)
			{
				var renderer = new UIGraphicsImageRenderer(keyWindow.Bounds.Size);
				UIImage? screenshot = renderer.CreateImage(context =>
				{
					if (keyWindow.Subviews.Count() > 2)
					{
						platformOverlayView.Hidden = true;
						keyWindow.Layer.RenderInContext(context.CGContext);
						platformOverlayView.Hidden = false;
					}
					else
					{
						keyWindow.Subviews[0].Layer.RenderInContext(context.CGContext);
					}
				});

				if (screenshot is not null)
				{
					var inputImage = new CIImage(screenshot);
					var clampFilter = new CIAffineClamp
					{
						InputImage = inputImage,
						Transform = CGAffineTransform.MakeIdentity(),
					};

					var blurFilter = new CIGaussianBlur
					{
						InputImage = clampFilter.OutputImage,
						Radius = Math.Max(popup.PopupStyle.BlurRadius, 0),
					};

					var context = CIContext.Create();
					if (blurFilter.OutputImage is not null)
					{
						var cgImage = context.CreateCGImage(blurFilter.OutputImage, inputImage.Extent);
						var blurredImage = UIImage.FromImage(cgImage!);
						var blurImageView = new UIImageView(blurredImage)
						{
							Frame = new CGRect(0, 0, GetScreenWidth(), GetScreenHeight()),
							ContentMode = UIViewContentMode.ScaleToFill,
							AutoresizingMask = UIViewAutoresizing.All,
						};

						popup._customBlurView = blurImageView;
						platformOverlayView.InsertSubview(blurImageView, 0);
					}
				}
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