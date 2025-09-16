using System.Numerics;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Maui.Platform;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using Syncfusion.Maui.Toolkit.Internals;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformPoint = Windows.Foundation.Point;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Represents the <see cref="PopupExtension"/> class for Windows platform.
	/// </summary>
	internal static partial class PopupExtension
	{
		#region Internal Methods

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
					return (int)nativeView.ActualOffset.X;
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
					return (int)nativeView.ActualOffset.Y;
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
			// Overlay extends to the title bar even ExtendsContentIntoTitleBar set to false.
			if (WindowOverlayHelper._platformWindow is Microsoft.UI.Xaml.Window platformWindow && platformWindow is not null && platformWindow.AppWindow is not null)
			{
				if (platformWindow.AppWindow.Presenter is not null && platformWindow.AppWindow.Presenter.Kind is Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen)
				{
					return 0;
				}
				else if (platformWindow.AppWindow.TitleBar is not null)
				{
					var titleBarHeight = Math.Max(0, platformWindow.AppWindow.TitleBar.Height);
					var displayDensity = platformWindow.GetDisplayDensity();
					return titleBarHeight / displayDensity;
				}
			}

			// TODO: Ensure StatusBarHeight in 4K resolution screen.
			// The TitleBar is null in Windows 10. In Windows 11, we can get the TitleBar but the height is 0.
			// Hence, calculated the TitleBar height from the Y position of the window from screen.
			return 30;
		}

		/// <summary>
		/// Gets the height of the device.
		/// </summary>
		/// <returns>The height of the device.</returns>
		internal static int GetScreenHeight()
		{
			var rootView = WindowOverlayHelper._platformWindow;
			return rootView is not null ? (int)rootView.Bounds.Height : 0;
		}

		/// <summary>
		/// Gets the width of the device.
		/// </summary>
		/// <returns>The width of the device.</returns>
		internal static int GetScreenWidth()
		{
			var rootView = WindowOverlayHelper._platformWindow;
			return rootView is not null ? (int)rootView.Bounds.Width : 0;
		}

		/// <summary>
		/// Gets action bar height.
		/// </summary>
		/// <returns>Returns action bar height.</returns>
		internal static int GetActionBarHeight()
		{
			var mainPage = GetMainPage();
			var mainWindowPage = PopupExtension.GetMainWindowPage();

			if (WindowOverlayHelper._platformWindow is Microsoft.UI.Xaml.Window platformWindow && mainPage is not null && mainPage.Handler is not null && mainPage.Handler.PlatformView is Microsoft.UI.Xaml.UIElement nativeViewMainPage)
			{
				bool hasNavBar = (mainWindowPage is NavigationPage && NavigationPage.GetHasNavigationBar(mainPage)) ||
								 (mainWindowPage is Shell && Shell.GetNavBarIsVisible(mainPage));

				if (hasNavBar)
				{
					var topHeight = nativeViewMainPage.TransformToVisual(null).TransformPoint(new Windows.Foundation.Point(0, 0));
					return (int)(topHeight.Y - GetStatusBarHeight());
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
			if (isopen)
			{
				if (WindowOverlayHelper._platformRootView is null)
				{
					return;
				}

				var popupList = VisualTreeHelper.GetOpenPopupsForXamlRoot(WindowOverlayHelper._platformRootView.XamlRoot);
				UIElement? blurElement = null;

				if (popupList is not null && popupList.Count > 0)
				{
					// The blur effect is applied to the root view for a single popup and to the previous popup's overlay when multiple popups are open.
					if (popupList.Count == 1)
					{
						blurElement = WindowOverlayHelper._platformRootView;
					}
					else
					{
						// The popup is added to the top of the list, so the previous popup is accessed by getting the child at index 1.
						blurElement = popupList[1].Child;
					}

					if (blurElement is null)
					{
						return;
					}

					BlurEffect(blurElement, popup, popupList);
				}
			}
		}

		/// <summary>
		/// Used to clear the blur effect for popup.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		internal static void ClearBlurViews(SfPopup popup)
		{
			if (popup.BlurElement is not null)
			{
				var effectVisual = ElementCompositionPreview.GetElementChildVisual(popup.BlurElement);
				effectVisual.Dispose();
				effectVisual = null;
				ElementCompositionPreview.SetElementChildVisual(popup.BlurElement, null);
				popup.BlurElement = null;
			}
		}

		/// <summary>
		/// Updates effect visual size when resize the window.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		internal static void UpdateEffectVisualSize(SfPopup popup)
		{
			if (popup.BlurElement is not null)
			{
				var effectsVisual = ElementCompositionPreview.GetElementChildVisual(popup.BlurElement);
				if (effectsVisual is not null)
				{
					effectsVisual.Size = new Vector2(GetScreenWidth(), GetScreenHeight());
					effectsVisual = null;
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
			if (relativeView.Handler is not null && relativeView.Handler.PlatformView is not null && relativeView.Handler.PlatformView is PlatformView nativeRelativeView && WindowOverlayHelper._platformRootView is not null)
			{
				UIElement rootView = WindowOverlayHelper._platformRootView;
				double[] relativeViewOrigin = new double[2] { 0, 0 };
				GeneralTransform transform = nativeRelativeView.TransformToVisual(rootView);
				relativeViewOrigin[0] = transform.TransformPoint(new PlatformPoint(0, 0)).X;
				relativeViewOrigin[1] = transform.TransformPoint(new PlatformPoint(0, 0)).Y;

				return new Rect(relativeViewOrigin[0], relativeViewOrigin[1], nativeRelativeView.ActualSize.X, nativeRelativeView.ActualSize.Y);
			}
			else
			{
				return Rect.Zero;
			}
		}

		#endregion

		#region Private Methods

		static void BlurEffect(UIElement blurElement, SfPopup popup, IReadOnlyList<Microsoft.UI.Xaml.Controls.Primitives.Popup>? popupList)
		{
			Compositor? compositor = ElementCompositionPreview.GetElementVisual(blurElement).Compositor;
			SpriteVisual? effectVisual = compositor?.CreateSpriteVisual();
			GaussianBlurEffect? graphicsEffect = new GaussianBlurEffect
			{
				Name = "Blur",
				BlurAmount = GetBlurRadius(popup),
				BorderMode = EffectBorderMode.Soft,
				Source = new CompositionEffectSourceParameter("Background"),
			};
			CompositionEffectBrush? effectBrush = compositor?.CreateEffectFactory(graphicsEffect).CreateBrush();
			effectBrush?.SetSourceParameter("Background", compositor?.CreateBackdropBrush());
			if (effectVisual is not null)
			{
				effectVisual.Brush = effectBrush;
				effectVisual.Size = new Vector2(GetScreenWidth(), GetScreenHeight());
			}

			ElementCompositionPreview.SetElementChildVisual(blurElement, effectVisual);
			popup.BlurElement = blurElement;
			compositor = null;
			effectBrush = null;
			effectVisual = null;
			graphicsEffect = null;
			popupList = null;
		}

		/// <summary>
		/// Gets the radius to determine blur intensity.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		/// <returns>Return radius value based on the defined blur intensity value.</returns>
		static float GetBlurRadius(this SfPopup popup)
		{
			if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.Light)
			{
				return 5;
			}
			else if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.ExtraDark)
			{
				return 10;
			}
			else if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.ExtraLight)
			{
				return 2.5f;
			}
			else if (popup.PopupStyle.BlurIntensity is PopupBlurIntensity.Custom)
			{
				return popup.PopupStyle.BlurRadius;
			}
			else
			{
				return 7.5f;
			}
		}

		#endregion
	}
}