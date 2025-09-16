using Android.Graphics;
using Android.Views;
using AndroidX.Core.View;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformView = Android.Views.View;
using View = Android.Views.View;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Represents the <see cref="PopupExtension"/> class for Android platform.
	/// </summary>
	internal static partial class PopupExtension
	{
		/// <summary>
		/// True if the WindowFlagHasNoLimits flag is set.
		/// </summary>
		internal static bool WindowFlagHasNoLimits;

		/// <summary>
		/// True if the WindowFlagHasFullScreen flag is set.
		/// </summary>
		internal static bool WindowFlagHasFullScreen;

		#region Internal methods

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
		/// <returns>Returns the Y coordinates of the page.</returns>
		internal static int GetActionBarHeight()
		{
			// Popup not positioned properly when show the popup relative to view in shell page.
			var mainPage = GetMainPage();
			if (mainPage is not null)
			{
				return GetLocationInApp(mainPage);
			}

			return 0;
		}

		/// <summary>
		/// Gets the X coordinate of the view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Returns the X coordinate of the view.</returns>
		internal static int GetX(this MauiView view)
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
						return (int)Math.Round(xPos / WindowOverlayHelper._density);
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
		internal static int GetY(this MauiView view)
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
						return (int)Math.Round(yPos / WindowOverlayHelper._density);
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Returns the Y location of the view relative to the app's rectangle.
		/// </summary>
		/// <param name="virtualView">Instance of maui view.</param>
		/// <returns>The Y location of the view relative to the app's rectangle.</returns>
		internal static int GetLocationInApp(VisualElement virtualView)
		{
			PlatformView? decorView = WindowOverlayHelper._decorViewContent;
			if (virtualView is not null && virtualView.Handler is not null && virtualView.Handler.PlatformView is View nativeView)
			{
				int[] viewCoordinates = new int[2] { 0, 0 };
				nativeView.GetLocationInWindow(viewCoordinates);

				// In Android 30, decorview location returning without status bar height.
				if ((int)Android.OS.Build.VERSION.SdkInt is not 30 && (WindowFlagHasFullScreen || WindowFlagHasNoLimits))
				{
					if (decorView is not null)
					{
						int[] decorCoordinates = new int[2] { 0, 0 };
						decorView.GetLocationInWindow(decorCoordinates);
						return (int)Math.Round((viewCoordinates[1] - decorCoordinates[1]) / WindowOverlayHelper._density);
					}
				}
				else
				{
					return (int)Math.Round((viewCoordinates[1] - GetWindowInsets("Top")) / WindowOverlayHelper._density);
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

			// Maui:826232 Referred xamarin popupLayout to take widthPixels.
			// PlatfromRootView will be null, when calling the show(0,0) from MainPage, NavigationPage, Tabbed and flyoutPage Constructor.
			if ((platformRootView is null || (platformRootView is not null && platformRootView.Width is 0)) && Android.Content.Res.Resources.System is not null && Android.Content.Res.Resources.System.DisplayMetrics is not null)
			{
				var widthPixel = (int)(Android.Content.Res.Resources.System.DisplayMetrics.WidthPixels / WindowOverlayHelper._density);
				return widthPixel;
			}

			return (int)Math.Round(platformRootView!.Width / WindowOverlayHelper._density);
		}

		/// <summary>
		/// Gets the height of the device.
		/// </summary>
		/// <returns>The height of the device in pixels.</returns>
		internal static int GetScreenHeight()
		{
			var platformRootView = WindowOverlayHelper._platformRootView;
			double platformRootViewHeight = 0;

			// Maui:826232 Referred xamarin popupLayout to take withPixels.
			// PlatfromRootView will be null, when calling the show(0,0) from MainPage, NavigationPage, Tabbed and flyoutPage Constructor.
			if ((platformRootView is null || (platformRootView is not null && platformRootView.Height is 0)) && Android.Content.Res.Resources.System is not null && Android.Content.Res.Resources.System.DisplayMetrics is not null)
			{
				var heightPixel = (int)(Android.Content.Res.Resources.System.DisplayMetrics.HeightPixels / WindowOverlayHelper._density);
				return heightPixel;
			}
			else if (platformRootView is not null)
			{
				if (IsResizeMode() && !WindowFlagHasNoLimits)
				{
					platformRootViewHeight = platformRootView.Height + (GetKeyboardHeight() * WindowOverlayHelper._density);
				}
				else
				{
					platformRootViewHeight = platformRootView.Height;
				}

				return (int)Math.Round(platformRootViewHeight / WindowOverlayHelper._density);
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
		/// <param name="popup">The SfPopup instance to apply flags to.</param>
		internal static void SetFlags(SfPopup popup)
		{
			var platformWindow = WindowOverlayHelper.GetPlatformWindow();
			if (platformWindow is not null && platformWindow.Attributes is not null)
			{
				var windowFlags = platformWindow.Attributes.Flags;

				// For android 30 LayoutNoLimits has no effect on main window, so should not process LayoutNoLimits flags for it.
				WindowFlagHasNoLimits = (int)Android.OS.Build.VERSION.SdkInt is not 30 && windowFlags.HasFlag(WindowManagerFlags.LayoutNoLimits);
				WindowFlagHasFullScreen = windowFlags.HasFlag(WindowManagerFlags.Fullscreen);
			}
			else
			{
				WindowFlagHasNoLimits = false;
				WindowFlagHasFullScreen = false;
			}
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
						case "Keyboard":
							return windowInsets.GetInsets(WindowInsetsCompat.Type.Ime()).Bottom;
						case "LeftGesture":
							return windowInsets.GetInsets(WindowInsetsCompat.Type.SystemGestures()).Left;
						case "RightGesture":
							return windowInsets.GetInsets(WindowInsetsCompat.Type.SystemGestures()).Right;
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
			ClearBlurViews(popup);

			// SetRenderEffect will not affect the background; it only applies the blur to the view on which the renderer is called. i.e it will take effect within its canvas, blurring the descendants.
			if (OperatingSystem.IsAndroidVersionAtLeast(31) && Shader.TileMode.Clamp is not null && popup.GetBlurRadius() > 0 && SfWindowOverlay._stackList is not null)
			{
				popup._blurredViews = new System.Collections.Generic.List<PlatformView>();
				bool hasModalPage = false;
				if (IPlatformApplication.Current is not null && IPlatformApplication.Current.Application is Microsoft.Maui.Controls.Application application &&
					application.Windows is not null && application.Windows.Count > 0)
				{
					Microsoft.Maui.Controls.Window window = application.Windows[0];
					hasModalPage = window is not null && window.Page is not null && window.Page.Navigation is not null
						&& window.Page.Navigation.ModalStack is not null && window.Navigation.ModalStack.Count > 0;
				}

				// In the case of multiple popups, if none of the popups in the view list have a blur effect, we need to apply the blur to the main view.
				if (!SfWindowOverlay._stackList.Any(view => view.HasBlurMode))
				{
					if (hasModalPage)
					{
						Microsoft.Maui.Controls.Page? mainPage = PopupExtension.GetMainPage();
						if (mainPage is not null && mainPage.Handler is not null && mainPage.Handler.PlatformView is PlatformView pageView)
						{
							popup._blurredViews.Add(pageView);
							pageView.SetRenderEffect(RenderEffect.CreateBlurEffect(popup.GetBlurRadius(), popup.GetBlurRadius(), Shader.TileMode.Clamp));
						}
					}
					else
					{
						ViewGroup? platformRootview = WindowOverlayHelper._platformRootView;
						if (platformRootview is not null && platformRootview.GetChildAt(0) is PlatformView blurTarget)
						{
							// Applies blur effect to target view.
							popup._blurredViews.Add(blurTarget);
							blurTarget.SetRenderEffect(RenderEffect.CreateBlurEffect(popup.GetBlurRadius(), popup.GetBlurRadius(), Shader.TileMode.Clamp));
						}
					}
				}

				// Applying Blur for nested popup.
				foreach (WindowOverlayStack windowOverlay in SfWindowOverlay._stackList.SkipLast(1).Reverse())
				{
					if (windowOverlay is not null)
					{
						windowOverlay.SetRenderEffect(RenderEffect.CreateBlurEffect(popup.GetBlurRadius(), popup.GetBlurRadius(), Shader.TileMode.Clamp));
						popup._blurredViews.Add(windowOverlay);
						if (windowOverlay.HasBlurMode)
						{
							break; // If the HasBlurMode is enabled, the blurring of previous views will be handled.
						}
					}
					else
					{
						continue;
					}
				}

				if (popup._popupOverlay is not null && popup._popupOverlay._overlayStack is not null)
				{
					popup._popupOverlay._overlayStack.HasBlurMode = true;
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
				if (popup._blurredViews is not null)
				{
					foreach (var blurredView in popup._blurredViews)
					{
						blurredView.SetRenderEffect(null);
					}

					if (popup._popupOverlay is not null && popup._popupOverlay._overlayStack is not null)
					{
						popup._popupOverlay._overlayStack.HasBlurMode = false;
					}

					popup._blurredViews.Clear();
					popup._blurredViews = null;
				}
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
			if (relativeView.Handler is not null && relativeView.Handler.PlatformView is not null && relativeView.Handler.PlatformView is PlatformView nativeRelativeView)
			{
				int[] relativeViewOrigin = new int[2];
				nativeRelativeView.GetLocationInWindow(relativeViewOrigin);
				relativeViewOrigin[1] = PopupExtension.GetLocationInApp(relativeView);
				return new Rect(relativeViewOrigin[0] / WindowOverlayHelper._density, relativeViewOrigin[1], nativeRelativeView.Width / WindowOverlayHelper._density, nativeRelativeView.Height / WindowOverlayHelper._density);
			}
			else
			{
				return Rect.Zero;
			}
		}

		/// <summary>
		/// Gets the SoftInputMode of the application.
		/// </summary>
		/// <returns>returns whether the softinputMode is resize or not.</returns>
		internal static bool IsResizeMode()
		{
			if (GetAttributes() is WindowManagerLayoutParams attributes && attributes.SoftInputMode is SoftInput.AdjustResize)
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
			// When gesture navigation bar is used its seen in the view when the keyboard is open in landscape mode so the naviagation bar height should be reduced from keyboard height in landscape mode.
			// When left and right swipe insets are greater than 0 means the gesture navigation bar is used.
			bool isGestureNavigation = (GetWindowInsets("LeftGesture") > 0 && GetWindowInsets("RightGesture") > 0) ? true : false;
			var keyboardHeight = (GetWindowInsets("Keyboard") > 0) ? ((WindowFlagHasNoLimits || (DeviceDisplay.MainDisplayInfo.Orientation is DisplayOrientation.Landscape && !isGestureNavigation)) ? GetWindowInsets("Keyboard") : (GetWindowInsets("Keyboard") - GetBottomNavigationBarHeight())) : 0;
			var actualKeyboardHeight = (double)keyboardHeight / WindowOverlayHelper._density;
			return (int)actualKeyboardHeight;
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
		/// Gets the radius to determine blur intensity.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		/// <returns>Return radius value based on the defined blur intensity value.</returns>
		static float GetBlurRadius(this SfPopup popup)
		{
			if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.Light)
			{
				return 11;
			}
			else if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.ExtraDark)
			{
				return 21;
			}
			else if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.ExtraLight)
			{
				return 4;
			}
			else if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.Custom)
			{
				return popup.PopupStyle.BlurRadius;
			}
			else
			{
				return 16;
			}
		}

		#endregion
	}
}
