using Android.Graphics;
using Android.Views;
using AndroidX.Core.View;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformView = Android.Views.View;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Represents the <see cref="PopupExtension"/> class for Android platform.
	/// </summary>
	internal static partial class PopupExtension
	{
		#region Internal methods

		/// <summary>
		/// Gets the status bar height.
		/// </summary>
		/// <returns>Returns the status bar height.</returns>
		internal static int GetStatusBarHeight()
		{
			if (PopupExtension.CheckWindowFlagsHasLayoutNoLimits() && !PopupExtension.CheckWindowFlagsHasFullScreen())
			{
				int[] activityCoordinates = new int[2] { 0, 0 };

				// Popup not positioned properly when show the popup relative to view in shell page.
				var mainPage = GetMainPage();
				if (mainPage is not null && mainPage.Handler is not null)
				{
					var nativeView = mainPage.Handler.PlatformView as ViewGroup;

					// When show is called from OnAppearing method, the GetLocationOnScreen not returned property Y coordinate.
					if (nativeView is null || (nativeView is not null && nativeView.Height == 0))
					{
						return 0;
					}

					nativeView?.GetLocationInWindow(activityCoordinates);

					if (GetAttributes() is WindowManagerLayoutParams attributes && (attributes.Flags & WindowManagerFlags.Fullscreen) == WindowManagerFlags.Fullscreen)
					{
						return (int)Math.Round(activityCoordinates[1] / WindowOverlayHelper._density);
					}
				}

				return (int)(activityCoordinates[1] / WindowOverlayHelper._density);
			}

			// To-do : Overlay not applied above status bar properly in android physical device.
			return 0;
		}

		/// <summary>
		/// Gets action bar height.
		/// </summary>
		/// <returns>Returns the Y coordinates of the page.</returns>
		internal static double GetActionBarHeight(bool ignoreActionBar)
		{
			int[] activityCoordinates = new int[2] { 0, 0 };

			// The popup is not positioned correctly when displayed relative to the view within a Shell page.
			var mainPage = GetMainPage();
			if (mainPage is not null && mainPage.Handler is not null)
			{
				var nativeView = mainPage.Handler.PlatformView as ViewGroup;

				// When Show is called from the OnAppearing method, the GetLocationOnScreen method does not return the correct Y coordinate.
				if (nativeView is null || (nativeView is not null && nativeView.Height == 0))
				{
					return 0;
				}

				nativeView?.GetLocationInWindow(activityCoordinates);
			}

			if (ignoreActionBar)
			{
				return 0;
			}

			if (GetAttributes() is WindowManagerLayoutParams attributes && (attributes.Flags & WindowManagerFlags.Fullscreen) == WindowManagerFlags.Fullscreen)
			{
				return (int)Math.Round(activityCoordinates[1] / WindowOverlayHelper._density);
			}

			if (PopupExtension.CheckWindowFlagsHasLayoutNoLimits())
			{
				return 0;
			}

			return Math.Round((activityCoordinates[1] - (WindowOverlayHelper._decorViewFrame?.Top ?? 0f)) / WindowOverlayHelper._density);
		}

		/// <summary>
		/// Gets the X coordinate of the view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Returns the X coordinate of the view.</returns>
		internal static double GetX(this MauiView view)
		{
			if (view.Handler is not null)
			{
				IMauiContext? context = view.Handler.MauiContext;
				if (context is not null)
				{
					PlatformView nativeView = view.ToPlatform(context);
					if (nativeView is not null)
					{
						var xPos = nativeView.GetX();
						return Math.Round(xPos / WindowOverlayHelper._density);
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Gets the Y coordinate of the view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Returns the Y coordinate of the view.</returns>
		internal static double GetY(this MauiView view)
		{
			if (view.Handler is not null)
			{
				IMauiContext? context = view.Handler.MauiContext;
				if (context is not null)
				{
					PlatformView nativeView = view.ToPlatform(context);
					if (nativeView is not null)
					{
						var yPos = nativeView.GetY();
						return Math.Round(yPos / WindowOverlayHelper._density);
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Gets the width of the device.
		/// </summary>
		/// <returns>The width of the device in pixels.</returns>
		internal static int GetScreenWidth()
		{
			var platformRootView = WindowOverlayHelper._platformRootView;

			// The PlatformRootView will be null when Show(0, 0) is called from the constructor of MainPage, NavigationPage, TabbedPage, or FlyoutPage.
			if (platformRootView is null || (platformRootView is not null && platformRootView.Width == 0))
			{
				var widthPixel = 0;
				if (Android.Content.Res.Resources.System is not null && Android.Content.Res.Resources.System.DisplayMetrics is not null)
				{
					widthPixel = (int)(Android.Content.Res.Resources.System.DisplayMetrics.WidthPixels / WindowOverlayHelper._density);
				}

				return widthPixel;
			}

			if (platformRootView is not null)
			{
				if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
				{
					if (platformRootView.Width < platformRootView.Height)
					{
						return (int)Math.Round(platformRootView.Height / WindowOverlayHelper._density);
					}
					else
					{
						return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
					}
				}
				else
				{
					if (platformRootView.Width < platformRootView.Height)
					{
						return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
					}
					else
					{
						return (int)Math.Round(platformRootView.Height / WindowOverlayHelper._density);
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Gets the height of the device.
		/// </summary>
		/// <returns>The height of the device in pixels.</returns>
		internal static int GetScreenHeight()
		{
			var platformRootView = WindowOverlayHelper._platformRootView;

			// The PlatformRootView will be null when Show(0, 0) is called from the constructor of MainPage, NavigationPage, TabbedPage, or FlyoutPage.
			if (platformRootView is null || (platformRootView is not null && platformRootView.Height == 0))
			{
				var heightPixel = 0;
				if (Android.Content.Res.Resources.System is not null && Android.Content.Res.Resources.System.DisplayMetrics is not null)
				{
					heightPixel = (int)(Android.Content.Res.Resources.System.DisplayMetrics.HeightPixels / WindowOverlayHelper._density);
				}

				return heightPixel;
			}

			// When Window Flags has LayoutNoLimits need to return height without bottom navigation bar to calculate full screen.
			if (CheckWindowFlagsHasLayoutNoLimits() && WindowOverlayHelper._decorViewContent is not null)
			{
				return (int)((WindowOverlayHelper._decorViewContent.Height - GetWindowInsets("Bottom")) / WindowOverlayHelper._density);
			}

			if (platformRootView is not null)
			{
				double platformRootViewHeight = platformRootView.Height;
				if (WindowOverlayHelper._decorViewContent is not null && WindowOverlayHelper._decorViewFrame is not null && IsResizeMode())
				{
					var keyboardHeight = GetKeyboardHeight();
					platformRootViewHeight = platformRootView.Height + (keyboardHeight > 0 ? keyboardHeight : 0);
				}

				if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
				{
					if (platformRootView.Width < platformRootViewHeight)
					{
						return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
					}
					else
					{
						return (int)Math.Round(platformRootViewHeight / WindowOverlayHelper._density);
					}
				}
				else
				{
					if (platformRootView.Width < platformRootViewHeight)
					{
						return (int)Math.Round(platformRootViewHeight / WindowOverlayHelper._density);
					}
					else
					{
						return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Calculates the height of bottom navigation bar.
		/// </summary>
		/// <returns>returns the height of bottom navigation bar.</returns>
		internal static int GetBottomNavigationBarHeight()
		{
			int navigationBarHeight = 0;
			var context = Android.App.Application.Context;
			if (context is not null && context.Resources is not null)
			{
				var resources = context.Resources;
				var resourceId = context.Resources.GetIdentifier("navigation_bar_height", "dimen", "android");
				if (resourceId > 0)
				{
					navigationBarHeight = resources.GetDimensionPixelSize(resourceId);
				}
			}

			return navigationBarHeight;
		}

		/// <summary>
		/// Checks whether window flags has LayoutNoLimits.
		/// </summary>
		/// <returns>returns whether window flags has LayoutNoLimits or not.</returns>
		internal static bool CheckWindowFlagsHasLayoutNoLimits()
		{
			var platformWindow = WindowOverlayHelper.GetPlatformWindow();
			if (platformWindow is not null && platformWindow.Attributes is not null)
			{
				var windowFlags = platformWindow.Attributes.Flags;
				return windowFlags.HasFlag(WindowManagerFlags.LayoutNoLimits);
			}

			return false;
		}

		/// <summary>
		/// Checks whether window flags has FullScreen.
		/// </summary>
		/// <returns>returns whether window flags has FullScreen or not.</returns>
		internal static bool CheckWindowFlagsHasFullScreen()
		{
			var platformWindow = WindowOverlayHelper.GetPlatformWindow();
			if (platformWindow is not null && platformWindow.Attributes is not null)
			{
				var windowFlags = platformWindow.Attributes.Flags;
				return windowFlags.HasFlag(WindowManagerFlags.Fullscreen);
			}

			return false;
		}

		/// <summary>
		/// Checks whether navigation bar is visible.
		/// </summary>
		/// <returns>returns whether whether navigation bar is visible or not.</returns>
		internal static bool CheckNavigationbarIsVisible()
		{
			if (WindowOverlayHelper._decorViewContent is not null)
			{
				var windowInsets = ViewCompat.GetRootWindowInsets(WindowOverlayHelper._decorViewContent);
				if (windowInsets is not null)
				{
					return windowInsets.IsVisible(WindowInsetsCompat.Type.NavigationBars());
				}
			}

			return false;
		}

		/// <summary>
		/// Get window insets value.
		/// </summary>
		/// <param name="position">size of window insets.</param>
		/// <returns>returns window insets value.</returns>
		internal static int GetWindowInsets(string position)
		{
			if (WindowOverlayHelper._decorViewContent is not null)
			{
				var windowInsets = ViewCompat.GetRootWindowInsets(WindowOverlayHelper._decorViewContent);
				if (windowInsets is not null)
				{
					var insets = windowInsets.GetInsets(WindowInsetsCompat.Type.SystemBars());
					switch (position)
					{
						case "Bottom":
							return insets.Bottom;
						case "Top":
							return insets.Top;
						case "Left":
							return insets.Left;
						case "Right":
							return insets.Right;
						default:
							return 0;
					}
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
		internal static void Blur(MauiView view, SfPopup popup, bool isopen)
		{
			if (OperatingSystem.IsAndroidVersionAtLeast(31) && Shader.TileMode.Clamp is not null && popup.GetBlurRadius() > 0)
			{
				if (IPlatformApplication.Current is not null && IPlatformApplication.Current.Application is Microsoft.Maui.Controls.Application application &&
					application.Windows is not null && application.Windows.Count > 0)
				{
					Microsoft.Maui.Controls.Window window = application.Windows[0];
					bool hasModalPage = window is not null && window.Page is not null && window.Page.Navigation is not null
						&& window.Page.Navigation.ModalStack is not null && window.Navigation.ModalStack.Count > 0;

					// Applies blur effect to the top page in modal stack when the modal page is displayed.
					if (hasModalPage)
					{
						Page? mainPage = PopupExtension.GetMainPage();
						if (mainPage is not null && mainPage.Handler is not null && mainPage.Handler.PlatformView is PlatformView pageView)
						{
							popup._blurTarget = pageView;
							popup._blurTarget.SetRenderEffect(RenderEffect.CreateBlurEffect(popup.GetBlurRadius(), popup.GetBlurRadius(), Shader.TileMode.Clamp));
							return;
						}
					}
				}

				ViewGroup? platformRootview = WindowOverlayHelper._platformRootView;
				if (platformRootview is not null && platformRootview.ChildCount > 0 && platformRootview.GetChildAt(0) is PlatformView blurTarget)
				{
					// Applies blur effect to target view.
					popup._blurTarget = blurTarget;
					popup._blurTarget.SetRenderEffect(RenderEffect.CreateBlurEffect(popup.GetBlurRadius(), popup.GetBlurRadius(), Shader.TileMode.Clamp));
				}
			}
		}

		/// <summary>
		/// Used to clear the blur effect for popup.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		internal static void ClearBlurViews(SfPopup popup)
		{
			if (OperatingSystem.IsAndroidVersionAtLeast(31))
			{
				// Clears the blur effect for all views listed in BlurredViews.
				if (popup._blurTarget is not null)
				{
					popup._blurTarget.SetRenderEffect(null);
					popup._blurTarget = null;
				}
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
			var decorViewFrame = WindowOverlayHelper._decorViewFrame;
			if (relative is null || relative.Handler is null || relative.Handler.MauiContext is null)
			{
				return;
			}

			PlatformView relativeView = relative.ToPlatform(relative.Handler.MauiContext);

			var popupViewWidth = popupView._popup._popupViewWidth * WindowOverlayHelper._density;
			var popupViewHeight = popupView._popup._popupViewHeight * WindowOverlayHelper._density;

			var heightOfRelativeView = relativeView.Height;
			var widthOfRelativeView = relativeView.Width;

			int[] location = new int[2];
			relativeView.GetLocationInWindow(location);
			var top = decorViewFrame is not null && decorViewFrame.Top > 0 ? decorViewFrame.Top : 0;
			var left = decorViewFrame is not null && decorViewFrame.Left > 0 ? decorViewFrame.Left : 0;

			// Adds the absolute points to the location of the relative view.
			location[0] += (int)(absoluteX - left);
			if (GetAttributes() is WindowManagerLayoutParams attributes && (attributes.Flags & WindowManagerFlags.Fullscreen) is WindowManagerFlags.Fullscreen)
			{
				location[1] += (int)absoluteY;
			}
			else
			{
				location[1] += (int)(absoluteY - (decorViewFrame?.Top ?? 0));
			}

			var screenHeight = GetScreenHeight() * WindowOverlayHelper._density;
			var screenWidth = GetScreenWidth() * WindowOverlayHelper._density;
			var statusBarHeight = GetStatusBarHeight() * WindowOverlayHelper._density;
			var actionBarHeight = GetActionBarHeight(popupView._popup.IgnoreActionBar) * WindowOverlayHelper._density;

			// Calculates the X-position relative to the specified view.
			CalculateXPosition(popupView, position, ref relativeX, absoluteX, popupViewWidth, location, widthOfRelativeView, screenWidth);

			// Calculates the Y-position relative to the specified view.
			CalculateYPosition(popupView, position, ref relativeY, absoluteY, popupViewHeight, location, heightOfRelativeView, screenHeight, statusBarHeight, actionBarHeight);

			relativeX = relativeX / WindowOverlayHelper._density;
			relativeY = relativeY / WindowOverlayHelper._density;
		}

		/// <summary>
		/// Gets the radius to determine blur intensity.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		/// <returns>Return radius value based on the defined blur intensity value.</returns>
		internal static float GetBlurRadius(this SfPopup popup)
		{
			if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Light)
			{
				return 11;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraDark)
			{
				return 21;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraLight)
			{
				return 4;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Custom)
			{
				return popup.PopupStyle.BlurRadius;
			}
			else
			{
				return 16;
			}
		}

		/// <summary>
		/// Gets the SoftInputMode of the application.
		/// </summary>
		/// <returns>returns whether the softinputMode is resize or not.</returns>
		internal static bool IsResizeMode()
		{
			if (GetAttributes() is WindowManagerLayoutParams attributes && attributes.SoftInputMode == SoftInput.AdjustResize)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Calculates the height of the on-screen keyboard.
		/// </summary>
		/// <returns>The height of the keyboard in pixels. Returns 0 if the view references are null.</returns>
		internal static int GetKeyboardHeight()
		{
			if (WindowOverlayHelper._decorViewContent is null || WindowOverlayHelper._decorViewFrame is null)
			{
				return 0;
			}

			var keyboardHeight = WindowOverlayHelper._decorViewContent.Height -
								 WindowOverlayHelper._decorViewFrame.Bottom -
								 GetBottomNavigationBarHeight();

			if (IsResizeMode() &&
				GetAttributes() is WindowManagerLayoutParams attributes &&
				(attributes.Flags & WindowManagerFlags.Fullscreen) is WindowManagerFlags.Fullscreen)
			{
				keyboardHeight += WindowOverlayHelper._decorViewFrame.Top;
			}

			return keyboardHeight;
		}

		/// <summary>
		/// Gets the attributes of the current activity's window.
		/// </summary>
		/// <returns> returns WindowManagerLayoutParams if available; otherwise,returns null.</returns>
		/// <summary>
		/// Gets the attributes of the current activity's window.
		/// </summary>
		/// <returns> returns WindowManagerLayoutParams if available; otherwise,returns null.</returns>
		internal static WindowManagerLayoutParams? GetAttributes()
		{
			var platformRootView = WindowOverlayHelper._platformRootView;
			if (platformRootView is not null &&
				platformRootView.Context is not null &&
				platformRootView.Context.GetActivity() is Android.App.Activity activity &&
				activity.Window is not null && activity.Window.Attributes is WindowManagerLayoutParams attributes)
			{
				return attributes;
			}

			return null;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Calculates the X position for the Popup.
		/// </summary>
		/// <param name="popupView"></param>
		/// <param name="position"></param>
		/// <param name="relativeX"></param>
		/// <param name="absoluteX"></param>
		/// <param name="popupViewWidth"></param>
		/// <param name="location"></param>
		/// <param name="widthOfRelativeView"></param>
		/// <param name="screenWidth"></param>
		static void CalculateXPosition(PopupView popupView, PopupRelativePosition position, ref double relativeX, double absoluteX, double popupViewWidth, int[] location, int widthOfRelativeView, float screenWidth)
		{
			if (position == PopupRelativePosition.AlignToLeftOf || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignBottomLeft)
			{
				if (popupView._popup._isRTL)
				{
					relativeX = Math.Max(location[0] - (2 * absoluteX) - popupViewWidth, 0);
				}
				else
				{
					relativeX = location[0] - popupViewWidth > 0 ? location[0] - popupViewWidth : 0;
				}
			}
			else if (position == PopupRelativePosition.AlignToRightOf || position == PopupRelativePosition.AlignTopRight || position == PopupRelativePosition.AlignBottomRight)
			{
				if (popupView._popup._isRTL)
				{
					relativeX = Math.Max(location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth, 0);

					// In the RTL case, if the button's width request exceeds the screen size, the popup is not displayed correctly within the view.
					relativeX = popupView._popup.ValidatePopupPosition(relativeX, popupViewWidth, screenWidth);
				}
				else
				{
					relativeX = location[0] + widthOfRelativeView + popupViewWidth < screenWidth ? location[0] + widthOfRelativeView : screenWidth - popupViewWidth;
				}
			}
			else
			{
				if (popupView._popup._isRTL)
				{
					relativeX = Math.Max(location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth, 0);
					relativeX = popupView._popup.ValidatePopupPosition(relativeX, popupViewWidth, screenWidth);
				}
				else
				{
					relativeX = location[0] + popupViewWidth < screenWidth ? location[0] : screenWidth - popupViewWidth;
					relativeX = popupView._popup.ValidatePopupPosition(relativeX, popupViewWidth, screenWidth);
				}
			}
		}

		/// <summary>
		/// Calculates the Y position for the popup.
		/// </summary>
		/// <param name="popupView"></param>
		/// <param name="position"></param>
		/// <param name="relativeY"></param>
		/// <param name="absoluteY"></param>
		/// <param name="popupViewHeight"></param>
		/// <param name="location"></param>
		/// <param name="heightOfRelativeView"></param>
		/// <param name="screenHeight"></param>
		/// <param name="statusBarHeight"></param>
		/// <param name="actionBarHeight"></param>
		static void CalculateYPosition(PopupView popupView, PopupRelativePosition position, ref double relativeY, double absoluteY, double popupViewHeight, int[] location, int heightOfRelativeView, float screenHeight, float statusBarHeight, double actionBarHeight)
		{
			if (position == PopupRelativePosition.AlignTop || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignTopRight)
			{
				relativeY = Math.Max(statusBarHeight + actionBarHeight, location[1] - popupViewHeight);
			}
			else if (position == PopupRelativePosition.AlignBottom || position == PopupRelativePosition.AlignBottomLeft || position == PopupRelativePosition.AlignBottomRight)
			{
				relativeY = location[1] + heightOfRelativeView + popupViewHeight < screenHeight ? location[1] + heightOfRelativeView : screenHeight - popupViewHeight;
			}
			else
			{
				// When the button's height exceeds the screen size, the popup is not displayed correctly within the view.
				relativeY = Math.Max(statusBarHeight + actionBarHeight, location[1] + popupViewHeight < screenHeight ? location[1] : screenHeight - popupViewHeight);
			}
		}

		/// <summary>
		/// Gets the Width of the screen based on the Orientation.
		/// </summary>
		/// <returns></returns>
		static int GetWidth(ViewGroup platformRootView)
		{
			if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
			{
				if (platformRootView.Width < platformRootView.Height)
				{
					return (int)Math.Round(platformRootView.Height / WindowOverlayHelper._density);
				}
				else
				{
					return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
				}
			}
			else
			{
				if (platformRootView.Width < platformRootView.Height)
				{
					return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
				}
				else
				{
					return (int)Math.Round(platformRootView.Height / WindowOverlayHelper._density);
				}
			}
		}

		/// <summary>
		/// Gets the height of the screen based on the Orientation.
		/// </summary>
		/// <returns></returns>
		static int GetHeight(ViewGroup platformRootView)
		{
			if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
			{
				if (platformRootView.Width < platformRootView.Height)
				{
					return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
				}
				else
				{
					return (int)Math.Round(platformRootView.Height / WindowOverlayHelper._density);
				}
			}
			else
			{
				if (platformRootView.Width < platformRootView.Height)
				{
					// We require the screen height excluding the status bar. therefore, the top value is considered.
					return (int)Math.Round(platformRootView.Height / WindowOverlayHelper._density);
				}
				else
				{
					return (int)Math.Round(platformRootView.Width / WindowOverlayHelper._density);
				}
			}
		}

		#endregion
	}
}
