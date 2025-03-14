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
		internal static int GetStatusBarHeight()
		{
			// The overlay extends into the title bar even when ExtendsContentIntoTitleBar is set to false.
#if !NET8_0_OR_GREATER
            if (WindowOverlayHelper.PlatformWindow is Microsoft.UI.Xaml.Window platformWindow)
            {
                if (platformWindow is not null && !platformWindow.ExtendsContentIntoTitleBar)
                {
                    return 0;
                }
            }
#endif

			// TODO: Ensure StatusBarHeight in 4K resolution screen.
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
		/// <param name="ignoreActionBar">Specifies whether the popup should consider action bar or not.</param>
		/// <returns>Returns action bar height.</returns>
		internal static int GetActionBarHeight(bool ignoreActionBar)
		{
			if (ignoreActionBar)
			{
				return 0;
			}

			var mainPage = GetMainPage();
			var mainWindowPage = PopupExtension.GetMainWindowPage();

			if (WindowOverlayHelper._platformWindow is Microsoft.UI.Xaml.Window platformWindow && mainPage is not null && mainPage.Handler is not null && mainPage.Handler.PlatformView is Microsoft.UI.Xaml.UIElement nativeViewMainPage)
			{
				bool hasNavBar = (mainWindowPage is NavigationPage && NavigationPage.GetHasNavigationBar(mainPage)) ||
								 (mainWindowPage is Shell && Shell.GetNavBarIsVisible(mainPage));

				if (hasNavBar)
				{
					var topHeight = nativeViewMainPage.TransformToVisual(null).TransformPoint(new Windows.Foundation.Point(0, 0));
					var titleBarHeight = platformWindow.AppWindow.TitleBar.Height;
					var displayDensity = platformWindow.GetDisplayDensity();
					return (int)(topHeight.Y - (titleBarHeight / displayDensity));
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

				if (popupList is not null)
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
			var isRelativeViewRTL = false;
			if (((relative as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
			{
				isRelativeViewRTL = true;
			}

			var rootView = WindowOverlayHelper._platformRootView;
			if (rootView is null || relative.Handler is null || relative.Handler.MauiContext is null)
			{
				return;
			}
			PlatformView relativeView = relative.ToPlatform(relative.Handler.MauiContext);

			var popupViewWidth = popupView._popup._popupViewWidth;
			var popupViewHeight = popupView._popup._popupViewHeight;

			var heightOfRelativeView = relativeView.ActualSize.Y;
			var widthOfRelativeView = relativeView.ActualSize.X;

			double[] location = new double[2] { 0, 0 };
			GeneralTransform transform = relativeView.TransformToVisual(rootView);
			location[0] = transform.TransformPoint(new PlatformPoint(0, 0)).X;
			location[1] = transform.TransformPoint(new PlatformPoint(0, 0)).Y;

			// Adds absolute points to the location of the relative view.
			location[0] += absoluteX;
			location[1] += absoluteY;

			var screenHeight = GetScreenHeight();
			var screenWidth = GetScreenWidth();

			// Calculates the X-position relative to the specified view.
			CalculateXPosition(popupView, position, ref relativeX, absoluteX, popupViewWidth, location, widthOfRelativeView, screenWidth, isRelativeViewRTL);

			// Calculates the Y-position relative to the specified view.
			CalculateYPosition(popupView, position, ref relativeY, popupViewHeight, location, heightOfRelativeView, screenHeight);
		}

		#endregion

		#region Private Methods

		static void CalculateXPosition(PopupView popupView, PopupRelativePosition position, ref double relativeX, double absoluteX, double popupViewWidth, double[] location, float widthOfRelativeView, float screenWidth, bool isRelativeViewRTL)
		{
			if (position == PopupRelativePosition.AlignToLeftOf || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignBottomLeft)
			{
				if (popupView._popup._isRTL)
				{
					if (isRelativeViewRTL)
					{
						relativeX = Math.Max(location[0] - (2 * absoluteX) - widthOfRelativeView - popupViewWidth, 0);
					}
					else
					{
						relativeX = Math.Max(location[0] - (2 * absoluteX) - popupViewWidth, 0);
					}
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
					if (isRelativeViewRTL)
					{
						relativeX = Math.Max(location[0] - (2 * absoluteX) - popupViewWidth, 0);
					}
					else
					{
						relativeX = Math.Max(location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth, 0);
					}
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
					if (isRelativeViewRTL)
					{
						relativeX = Math.Max(location[0] - (2 * absoluteX) - popupViewWidth, 0);
					}
					else
					{
						relativeX = Math.Max(location[0] - (2 * absoluteX) + widthOfRelativeView - popupViewWidth, 0);
					}
				}
				else
				{
					relativeX = location[0] + popupViewWidth < screenWidth ? location[0] : screenWidth - popupViewWidth;
				}
			}
		}

		static void CalculateYPosition(PopupView popupView, PopupRelativePosition position, ref double relativeY, double popupViewHeight, double[] location, float heightOfRelativeView, float screenHeight)
		{
			if (position == PopupRelativePosition.AlignTop || position == PopupRelativePosition.AlignTopLeft || position == PopupRelativePosition.AlignTopRight)
			{
				relativeY = Math.Max(GetStatusBarHeight() + GetActionBarHeight(popupView._popup.IgnoreActionBar), location[1] - popupViewHeight);
			}
			else if (position == PopupRelativePosition.AlignBottom || position == PopupRelativePosition.AlignBottomLeft || position == PopupRelativePosition.AlignBottomRight)
			{
				relativeY = location[1] + heightOfRelativeView + popupViewHeight < screenHeight ? location[1] + heightOfRelativeView : screenHeight - popupViewHeight;
			}
			else
			{
				relativeY = location[1] + popupViewHeight < screenHeight ? location[1] : screenHeight - popupViewHeight;
			}
		}

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
			if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Light)
			{
				return 5;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraDark)
			{
				return 10;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.ExtraLight)
			{
				return 2.5f;
			}
			else if (popup.PopupStyle.BlurIntensity == PopupBlurIntensity.Custom)
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