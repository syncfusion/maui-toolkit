using Android.Views;
using Android.Views.InputMethods;
using AndroidX.Core.View;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using Activity = Android.App.Activity;
using PlatformRect = Android.Graphics.Rect;
using PlatformView = Android.Views.View;
using View = Android.Views.View;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// <see cref="SfPopup"/> allows the user to display an alert message with the customizable buttons or load any desired view inside the pop-up.
	/// </summary>
	public partial class SfPopup
	{
		#region Fields

		/// <summary>
		/// List to store the views blurred by this popup.
		/// </summary>
		internal PlatformView? _blurTarget;

		/// <summary>
		/// Backing field to store the decorView content.
		/// </summary>
		internal View? _decorViewContent = null;

		/// <summary>
		/// Backing field to store the keyboard height when device orientation changes.
		/// </summary>
		int _previousKeyboardHeight = 0;

		bool _hasShrunk = false;

		/// <summary>
		/// Backing field to store the decorView content height before decorView layout was changed.
		/// </summary>
		int _oldDecorViewHeight = -1;

		/// <summary>
		/// Backing field to store the decorView content width before decorView layout was changed.
		/// </summary>
		int _oldDecorViewWidth = -1;

		/// <summary>
		/// Backing field to store the decorView content width before decorView layout was changed.
		/// </summary>
		int _oldRootViewHeight = -1;

		#endregion

		#region Internal Methods

		/// <summary>
		/// Used to wire the events.
		/// </summary>
		internal void WirePlatformSpecificEvents()
		{
			if (PopupExtension.GetMainPage() is Page mainPage && mainPage is not null && mainPage.Handler is not null && mainPage.Handler.MauiContext is not null)
			{
				PlatformView nativeView = mainPage.ToPlatform(mainPage.Handler.MauiContext);
				if (nativeView is not null && nativeView.ViewTreeObserver is not null)
				{
					nativeView.ViewTreeObserver.GlobalLayout += OnViewTreeObserverGlobalLayout;
				}
			}
		}

		/// <summary>
		/// Used to unwire the events.
		/// </summary>
		internal void UnWirePlatformSpecificEvents()
		{
			if (PopupExtension.GetMainPage() is Page mainPage && mainPage is not null && mainPage.Handler is not null && mainPage.Handler.MauiContext is not null)
			{
				PlatformView nativeView = mainPage.ToPlatform(mainPage.Handler.MauiContext);
				if (nativeView is not null && nativeView.ViewTreeObserver is not null)
				{
					nativeView.ViewTreeObserver.GlobalLayout -= OnViewTreeObserverGlobalLayout;
				}
			}
		}

		/// <summary>
		/// Reposition and Resize the PopupView based on Keyboard.
		/// </summary>
		internal void PopupPositionBasedOnKeyboard()
		{
			if (_popupView is not null)
			{
				_popupView.HandlerChanged += OnPopupViewHandlerChanged;
			}
		}

		/// <summary>
		/// Applies shadow to the native popup view.
		/// </summary>
		internal void ApplyNativePopupViewShadow()
		{
			if (_popupView is not null && _popupView.Handler is not null && PopupStyle is not null && _popupView.Handler.PlatformView is not null)
			{
				View platformView = (View)_popupView.Handler.PlatformView;
				MauiDrawable mauiDrawable = new MauiDrawable(platformView.Context);
				var popupColor = PopupStyle.GetPopupBackground();
				mauiDrawable.SetBackground(popupColor);
				var cornerRadii = _isRTL ? new CornerRadius(PopupStyle.CornerRadius.TopRight, PopupStyle.CornerRadius.TopLeft, PopupStyle.CornerRadius.BottomRight, PopupStyle.CornerRadius.BottomLeft) : PopupStyle.CornerRadius;
				var radii = new float[]
				{
					(float)(cornerRadii.TopLeft * DeviceDisplay.MainDisplayInfo.Density),
					(float)(cornerRadii.TopLeft * DeviceDisplay.MainDisplayInfo.Density),
					(float)(cornerRadii.TopRight * DeviceDisplay.MainDisplayInfo.Density),
					(float)(cornerRadii.TopRight * DeviceDisplay.MainDisplayInfo.Density),
					(float)(cornerRadii.BottomRight * DeviceDisplay.MainDisplayInfo.Density),
					(float)(cornerRadii.BottomRight * DeviceDisplay.MainDisplayInfo.Density),
					(float)(cornerRadii.BottomLeft * DeviceDisplay.MainDisplayInfo.Density),
					(float)(cornerRadii.BottomLeft * DeviceDisplay.MainDisplayInfo.Density),
				};

				if (OperatingSystem.IsAndroidVersionAtLeast(33))
				{
					mauiDrawable.SetCornerRadii(radii);
				}
				else
				{
					// The corner radius is set using the average of the calculated radii. If avgRadius is non-zero, it's applied to mauiDrawable.SetCornerRadius, and RadiusValue is set to avgRadius / DeviceDisplay.MainDisplayInfo.Density. If avgRadius is zero, RadiusValue is set to zero, and the corner radius is not applied.
					var avgRadius = radii.Average();
					if (avgRadius != 0)
					{
						mauiDrawable.SetCornerRadius(avgRadius);
						_radiusValue = avgRadius / DeviceDisplay.MainDisplayInfo.Density;
					}
					else
					{
						_radiusValue = 0;
					}
				}

				platformView.SetBackground(mauiDrawable);
				if (PopupStyle.HasShadow)
				{
					platformView.SetElevation((float)(6 * DeviceDisplay.MainDisplayInfo.Density));
				}
				else
				{
					platformView.SetElevation(0f);
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles, when popup view handler has changed.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/>.</param>
		void OnPopupViewHandlerChanged(object? sender, EventArgs e)
		{
			// When adding OverlayStack to the window manager, update oldDecorViewHeight and oldDecorViewWidth during the initial load. Otherwise, the overlay's width and height won't update correctly the first time the device orientation changes.
			if (WindowOverlayHelper._decorViewContent is not null)
			{
				_decorViewContent = WindowOverlayHelper._decorViewContent;
			}

			if (_decorViewContent is not null)
			{
				_oldDecorViewHeight = _decorViewContent.Height;
				_oldDecorViewWidth = _decorViewContent.Width;
			}

			if (WindowOverlayHelper._platformRootView is not null)
			{
				_oldRootViewHeight = WindowOverlayHelper._platformRootView.Height;
			}

		}

		/// <summary>
		/// Handles, when global layout of popupview has changed.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/>.</param>
		void OnViewTreeObserverGlobalLayout(object? sender, EventArgs e)
		{
			if (WindowOverlayHelper._decorViewContent is not null)
			{
				_decorViewContent = WindowOverlayHelper._decorViewContent;
			}

			if (_decorViewContent is null || WindowOverlayHelper._decorViewFrame is null || (WindowOverlayHelper._window is IWindow currentWindow && currentWindow.Content is not null && currentWindow.Content.Handler is null))
			{
				return;
			}

			var platformRootView = WindowOverlayHelper._platformRootView;
			var decorViewFrameBottom = WindowOverlayHelper._decorViewFrame.Bottom;

			if (platformRootView is not null)
			{
				bool viewHeightChanged = PopupExtension.IsResizeMode() ? _oldRootViewHeight != platformRootView.Height : _oldDecorViewWidth != -1 && (_oldDecorViewHeight != _decorViewContent.Height || _oldDecorViewWidth != _decorViewContent.Width);
				if (viewHeightChanged)
				{
					// Abort the _popupView animation when the size changes. Resetting TranslationX and TranslationY to 0 in ResetAnimatedProperties won't cause issues, as _popupView will be repositioned in ResetPopupWidthHeight.
					AbortPopupViewAnimation();
					ResetAnimatedProperties();
					if (IsOpen && _popupOverlay is not null && _popupView is not null)
					{
						// limitation : When change orientation from landscape to landscape, x position of window manger params will not update.
						if (PopupExtension.CheckWindowFlagsHasLayoutNoLimits())
						{
							var windowManagerLayoutParams = _popupOverlay.GetWindowManagerLayoutParams();
							SetXPositionForWindowManagerLayoutParams(windowManagerLayoutParams);
						}

						// Updates the size of WindowManagerLayoutParams when the device orientation changes.
						_popupOverlay.UpdateWindowManagerLayoutParamsSize();
						ResetPopupWidthHeight();

						// Reset values during device orientation change with the keyboard open, as the popup view's height and position are re-evaluated after the keyboard closes.
						_popupYPositionBeforeKeyboardInView = _popupYPosition;
						_popupViewHeightBeforeKeyboardInView = _popupViewHeight;
						_popupView.InvalidateForceLayout();
					}
				}
			}

			if (_decorViewContent is not null && _decorViewContent.Context is not null)
			{
				var inputService = _decorViewContent.Context.GetSystemService(Activity.InputMethodService);
				PlatformRect? visibleBounds = new PlatformRect();
				_decorViewContent.GetWindowVisibleDisplayFrame(visibleBounds);
				int keyboardHeight = 0;
				if (inputService is InputMethodManager inputMethodManager && inputMethodManager is not null)
				{
					if (IsOpen)
					{
						// The keyboard size is inaccurate when the system navigation mode is set to 'Buttons' in Android settings.
						keyboardHeight = PopupExtension.GetKeyboardHeight();
						if (keyboardHeight > 0 && inputMethodManager.IsAcceptingText && (!_hasShrunk || (_decorViewContent.Height == _oldDecorViewHeight && _previousKeyboardHeight != keyboardHeight)))
						{
							// If the popup is positioned with the wrong keyboard height, reset its position and size to accommodate the correct keyboard height.
							if (_hasShrunk && _previousKeyboardHeight != keyboardHeight)
							{
								UnshrinkPoupViewOnKeyboardCollapse();
							}

							_hasShrunk = true;

							// Adjust the popup's position as the keyboard appears.
							PositionPoupViewBasedOnKeyboard((visibleBounds.Bottom - WindowOverlayHelper._decorViewFrame.Top) / WindowOverlayHelper._density);
						}
						else if ((!inputMethodManager.IsAcceptingText && keyboardHeight <= 0 && _hasShrunk) || (keyboardHeight <= 0 && _hasShrunk))
						{
							// Restore the popup to its original Y position and height as the keyboard collapses and goes out of view.
							UnshrinkPoupViewOnKeyboardCollapse();
							_hasShrunk = false;
						}
					}
					else if (!inputMethodManager.IsAcceptingText && _hasShrunk)
					{
						// Hide the keyboard before unfocusing the edit element when the popup is closed.
						inputMethodManager.HideSoftInputFromWindow(_decorViewContent.WindowToken, HideSoftInputFlags.None);
						UnshrinkPoupViewOnKeyboardCollapse();
						_hasShrunk = false;
					}
				}

				_oldDecorViewHeight = _decorViewContent.Height;
				_oldDecorViewWidth = _decorViewContent.Width;
				if (platformRootView is not null)
				{
					_oldRootViewHeight = platformRootView.Height;
				}

				_previousKeyboardHeight = keyboardHeight;
				inputService = null;
				visibleBounds = null;
			}
		}

		/// <summary>
		/// Set LayoutNoLimits flag for windowManager params when Window Flags has LayoutNoLimits.
		/// </summary>
		void SetWindowFlagsForLayoutNoLimits()
		{
			if (PopupExtension.CheckWindowFlagsHasLayoutNoLimits() && _popupOverlay is not null)
			{
				var windowManagerLayoutParams = _popupOverlay.GetWindowManagerLayoutParams();
				windowManagerLayoutParams.Flags = (PopupExtension.CheckWindowFlagsHasFullScreen() && !PopupExtension.CheckNavigationbarIsVisible()) ? WindowManagerFlags.Fullscreen : WindowManagerFlags.LayoutNoLimits | WindowManagerFlags.LayoutInScreen;

				// To Layout overlay at top left corner.
				windowManagerLayoutParams.Gravity = GravityFlags.Start | GravityFlags.Top;
				SetXPositionForWindowManagerLayoutParams(windowManagerLayoutParams);
			}
		}

		/// <summary>
		/// Hides the navigation bar when a popup is opened.
		/// </summary>
		void HideNavigationBar()
		{
			if (OperatingSystem.IsAndroidVersionAtLeast(30) && PopupExtension.CheckWindowFlagsHasFullScreen() && !PopupExtension.CheckNavigationbarIsVisible())
			{
				if (_popupOverlayContainer is not null && _popupOverlayContainer.Handler is not null && _popupOverlayContainer.Handler.PlatformView is WindowOverlayStack overlayStack && overlayStack is not null && overlayStack.WindowInsetsController is not null)
				{
					overlayStack.WindowInsetsController.Hide(WindowInsetsCompat.Type.NavigationBars());
				}
			}
		}

		/// <summary>
		/// Set X position windowManager params when Window Flags has LayoutNoLimits.
		/// </summary>
		void SetXPositionForWindowManagerLayoutParams(WindowManagerLayoutParams windowManagerLayoutParams)
		{
			// In Landscape mode, when the Window Flags include LayoutNoLimits, the windowManager X position is not set correctly, causing it to overlap with the dark system bar.
			if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape && WindowOverlayHelper._decorViewContent is not null)
			{
				int[] decorViewContentCoordinates = new int[2] { 0, 0 };
				WindowOverlayHelper._decorViewContent.GetLocationOnScreen(decorViewContentCoordinates);
				var systemBarHeight = decorViewContentCoordinates[0];
				var leftInset = PopupExtension.GetWindowInsets("Left");

				if (systemBarHeight != 0)
				{
					windowManagerLayoutParams.X = (int)systemBarHeight;
				}
				else if (leftInset != 0)
				{
					windowManagerLayoutParams.X = -(int)leftInset;
				}
			}
			else
			{
				windowManagerLayoutParams.X = 0;
			}
		}

		#endregion
	}
}
