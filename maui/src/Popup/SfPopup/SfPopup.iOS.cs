using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// <see cref="SfPopup"/> allows the user to display an alert message with the customizable buttons or load any desired view inside the pop-up.
	/// </summary>
	public partial class SfPopup
	{
		#region Fields

		/// <summary>
		/// Used to store keyboard height.
		/// </summary>
		double _keyboardHeight = 0;
		NSObject? _keyboardShow;
		NSObject? _keyboardHide;

		/// <summary>
		/// This field stores the blur effect view added by this popup.
		/// </summary>
		internal UIVisualEffectView? _blurView;

		#endregion

		#region Internal Propeties

		/// <summary>
		/// Gets the last page from modal stack.
		/// </summary>
		internal Page? ModalPage
		{
			get
			{
				var windowPage = PopupExtension.GetMainWindowPage();
				if (windowPage is not null && windowPage.Navigation is not null && windowPage.Navigation.ModalStack is not null)
				{
					return windowPage.Navigation.ModalStack.LastOrDefault();
				}

				return null;
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Used to wire the events.
		/// </summary>
		internal void WirePlatformSpecificEvents()
		{
			var windowPage = PopupExtension.GetMainWindowPage();
			if (windowPage is not null)
			{
				if (ModalPage is not null)
				{
					// Wired the size change event for the Modal page when a popup is opened from it.
					ModalPage.SizeChanged += OnMainPageSizeChanged;
				}
				else
				{
					windowPage.SizeChanged += OnMainPageSizeChanged;
				}
			}

			PopupPositionBasedOnKeyboard();
		}

		/// <summary>
		/// Used to unwire the events.
		/// </summary>
		internal void UnWirePlatformSpecificEvents()
		{
			var windowPage = PopupExtension.GetMainWindowPage();
			if (windowPage is not null)
			{
				if (ModalPage is not null)
				{
					// Wired the size change event for the Modal page when a popup is opened from it.
					ModalPage.SizeChanged -= OnMainPageSizeChanged;
				}
				else
				{
					windowPage.SizeChanged -= OnMainPageSizeChanged;
				}
			}

			_keyboardHide?.Dispose();
			_keyboardShow?.Dispose();

			// When closing the popup with the keyboard open, reset the BeforeKeyboardInView and YPositionBeforeKeyboardInView values.
			// This ensures the values are updated, as dismissing the popup with the keyboard open disposes of the keyboardHide object,
			// preventing the event from being invoked and leaving the values unset.
			if (_popupViewHeightBeforeKeyboardInView != 0)
			{
				_popupViewHeightBeforeKeyboardInView = 0;
			}

			if (_popupYPositionBeforeKeyboardInView != -1)
			{
				_popupYPositionBeforeKeyboardInView = -1;
			}
		}

		/// <summary>
		/// Reposition and Resize the _popupView based on Keyboard.
		/// </summary>
		internal void PopupPositionBasedOnKeyboard()
		{
			// When keyboard comes to the view, notifications can be received from the ObserveWillShow delegates.
			_keyboardShow = UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
			{
				if (_popupView is not null)
				{
					_keyboardHeight = args.FrameEnd.Height;
					PositionPoupViewBasedOnKeyboard(args.FrameEnd.Y);
				}
			});

			// When keyboard hides from the view, notifications can be received from the ObserveWillHide delegates.
			_keyboardHide = UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
			{
				if (_popupView is not null)
				{
					_keyboardHeight = 0;
					UnshrinkPoupViewOnKeyboardCollapse();
				}
			});
		}

		/// <summary>
		/// This method updates the _popupView Bounds.
		/// </summary>
		internal void UpdateBounds()
		{
			if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.PlatformView is not null && _popupView.Handler.PlatformView is UIView platformView)
			{
				platformView.Bounds = new CoreGraphics.CGRect(0, 0, _popupViewWidth, _popupViewHeight);
			}
		}

		/// <summary>
		/// This method updates the _popupView Background.
		/// </summary>
		/// <param name="brush">The Popup color to be updated.</param>
		internal void UpdateNativePopupViewBackground(Brush brush)
		{
			if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.PlatformView is not null && _popupView.Handler.PlatformView is UIView platformView)
			{
				platformView.UpdateBackground(brush);
			}
		}

		/// <summary>
		/// Applies clip to the native popup view.
		/// </summary>
		internal void ApplyNativePopupViewClip()
		{
			if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.PlatformView is UIView view && view is not null)
			{
				if (PopupStyle.CornerRadius.TopRight != 0 || PopupStyle.CornerRadius.TopLeft != 0 || PopupStyle.CornerRadius.BottomLeft != 0 || PopupStyle.CornerRadius.BottomRight != 0)
				{
					var maskPath = new UIBezierPath();
					var topLeftRadius = (float)PopupStyle.CornerRadius.TopLeft;
					var topRightRadius = (float)PopupStyle.CornerRadius.TopRight;
					var bottomLeftRadius = (float)PopupStyle.CornerRadius.BottomLeft;
					var bottomRightRadius = (float)PopupStyle.CornerRadius.BottomRight;
					maskPath.MoveTo(new CGPoint(0, topLeftRadius));
					maskPath.AddArc(new CGPoint(topLeftRadius, topLeftRadius), topLeftRadius, (nfloat)Math.PI, (nfloat)Math.PI * 1.5f, true);

					maskPath.AddLineTo(new CGPoint(view.Bounds.Width - topRightRadius, 0));
					maskPath.AddArc(new CGPoint(view.Bounds.Width - topRightRadius, topRightRadius), topRightRadius, (nfloat)Math.PI * 1.5f, 0, true);

					maskPath.AddLineTo(new CGPoint(view.Bounds.Width, view.Bounds.Height - bottomRightRadius));
					maskPath.AddArc(new CGPoint(view.Bounds.Width - bottomRightRadius, view.Bounds.Height - bottomRightRadius), bottomRightRadius, 0, (nfloat)(Math.PI * 0.5), true);

					maskPath.AddLineTo(new CGPoint(bottomLeftRadius, view.Bounds.Height));
					maskPath.AddArc(new CGPoint(bottomLeftRadius, view.Bounds.Height - bottomLeftRadius), bottomLeftRadius, (nfloat)(Math.PI * 0.5), (nfloat)Math.PI, true);

					maskPath.ClosePath();
					var maskLayer = new CAShapeLayer { Frame = view.Bounds, Path = maskPath.CGPath };
					view.Layer.Mask = maskLayer;
				}
			}
		}

		#endregion

		/// <summary>
		/// Occurs when Width or Height properties changes.
		/// </summary>
		/// <param name="sender">The main page of the current application.</param>
		/// <param name="e">The event arguments.</param>
		void OnMainPageSizeChanged(object? sender, EventArgs e)
		{
			if (IsOpen && _popupView is not null)
			{
				// Abort the PopupView animation when orientation or window size is change.
				AbortPopupViewAnimation();
				ResetAnimatedProperties();

				// While show the popup using ShowRelativeToView, Popup is not positioned properly when resize the window in MAC and Split the screen in iOS.
				if (_relativeView is not null)
				{
					Dispatcher.Dispatch(ResetPopupWidthHeight);
				}
				else
				{
					ResetPopupWidthHeight();
				}

				// Update the BlurIOS frame during window resizing or orientation changes.
				if (IsOpen && ShowOverlayAlways && OverlayMode is PopupOverlayMode.Blur)
				{
					PopupExtension.UpdateBluriosFrame(this);
				}

				_popupView.InvalidateForceLayout();

				// When the device orientation changes with the keyboard open,
				// reset the values as the popup view height and position will be re-evaluated after the keyboard closes.
				if (_keyboardHeight > 0)
				{
					_popupViewHeightBeforeKeyboardInView = _popupViewHeight;
					_popupYPositionBeforeKeyboardInView = _popupYPosition;
				}
			}
		}
	}
}