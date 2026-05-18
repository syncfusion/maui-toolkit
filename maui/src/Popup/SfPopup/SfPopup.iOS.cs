using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// <see cref="SfPopup"/> allows the user to display an alert message with the customizable buttons or load any desired view inside the pop-up.
	/// </summary>
	public partial class SfPopup
	{
		#region Fields
		NSObject? _keyboardShow;
		NSObject? _keyboardHide;
		private UITapGestureRecognizer? rootTapRecognizer;

		/// <summary>
		/// This field stores the blur effect view added by this popup.
		/// </summary>
		internal UIVisualEffectView? _blurView;
		internal UIView? _customBlurView;

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
				else if (windowPage is Shell shellPage && shellPage.CurrentPage != null)
				{
					// Maui:990575-TODO : Shell page size changed event not triggered after .NET 10 update, so wire the size changed event for the current page of the Shell page.
					shellPage.CurrentPage.SizeChanged += OnMainPageSizeChanged;
				}
				else
				{
					windowPage.SizeChanged += OnMainPageSizeChanged;
				}
			}

			WireKeyboardNotification();

			// Enable pass-through touches and outside-tap close when ShowOverlayAlways is false
			this.AddRootTapRecognizer();
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
				else if (windowPage is Shell shellPage && shellPage.CurrentPage != null)
				{
					// Maui:990575-TODO : Shell page size changed event not triggered after .NET 10 update, so unwire the size changed event for the current page of the Shell page.
					shellPage.CurrentPage.SizeChanged -= this.OnMainPageSizeChanged;
				}
				else
				{
					windowPage.SizeChanged -= OnMainPageSizeChanged;
				}
			}

			_keyboardHide?.Dispose();
			_keyboardShow?.Dispose();

			this.RemoveRootTapRecognizer();
		}

		/// <summary>
		/// Reposition and Resize the PopupView based on Keyboard.
		/// </summary>
		internal void WireKeyboardNotification()
		{
			var popupView = _popupView;
			if (popupView is not null)
			{
				// When keyboard comes to the view, can get the notification from ObserveWillShow delegates.
				_keyboardShow = UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
				{
					_keyboardHeight = args.FrameEnd.Height - PopupExtension.GetSafeAreaHeight("Bottom");
					CalculatePopupSizeAndPosition();
					popupView.InvalidateForceLayout();
				});

				// When keyboard hides from the view, can get the notification from ObserveWillHide delegates.
				_keyboardHide = UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
				{
					_keyboardHeight = 0;
					CalculatePopupSizeAndPosition();
					popupView.InvalidateForceLayout();
				});
			}
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
				else
				{
					view.Layer.Mask = null;
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

				SyncPopupDimensionFields();

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
			}
		}

		/// <summary>
		/// Adds a root-level tap recognizer on the key window to detect taps outside the popup
		/// when ShowOverlayAlways is false. Does not cancel touches so underlying views remain interactive.
		/// </summary>
		private void AddRootTapRecognizer()
		{
			if (this.ShowOverlayAlways || !this.IsOpen)
			{
				return;
			}

			// Resolve current key window (multi-window aware)
			UIWindow? keyWindow = PopupExtension.GetActiveWindow();
			var root = (UIView?)(keyWindow ?? WindowOverlayHelper._platformRootView);
			if (root == null)
			{
				return;
			}

			// If an existing recognizer is attached to a different root, remove it first
			if (this.rootTapRecognizer != null)
			{
				var previousView = this.rootTapRecognizer.View;
				if (previousView != null && previousView != root)
				{
					previousView.RemoveGestureRecognizer(this.rootTapRecognizer);
				}

				this.rootTapRecognizer.Dispose();
				this.rootTapRecognizer = null;
			}

			this.rootTapRecognizer = new UITapGestureRecognizer(g =>
			{
				// Only the topmost popup should respond to outside taps.
				if (PopupExtension.TopMostOpenPopup != this)
				{
					return;
				}

				if (!this.IsOpen || this._popupView == null || this.StaysOpen)
				{
					return;
				}

				var currentRoot = (UIView?)(WindowOverlayHelper._platformRootView ?? keyWindow ?? PopupExtension.GetActiveWindow());
				if (currentRoot == null)
				{
					return;
				}

				if (this._popupView.Handler?.PlatformView is UIView popupNative)
				{
					var origin = popupNative.ConvertPointToView(new CGPoint(0, 0), currentRoot);
					var rect = new CGRect(origin.X, origin.Y, popupNative.Bounds.Width, popupNative.Bounds.Height);
					var location = g.LocationInView(currentRoot);
					bool inside = rect.Contains(location);
					if (!inside)
					{
						if (!this.RaisePopupClosingEvent())
						{
							// Close immediately so underlying control can still receive the tap
							this.IsOpen = false;
						}
					}
				}
			})
			{
				CancelsTouchesInView = false,
				DelaysTouchesBegan = false,
				DelaysTouchesEnded = false,
			};

			// Allow recognizing simultaneously so the tap on underlying control still fires
			this.rootTapRecognizer.ShouldRecognizeSimultaneously += (gesture, other) => true;

			// Reject touches that began inside the popup so the root recognizer does not also treat the same tap as an outside-tap and close the popup.
			this.rootTapRecognizer.ShouldReceiveTouch += (recognizer, touch) =>
			{
				try
				{
					var currentRoot = (UIView?)(WindowOverlayHelper._platformRootView ?? keyWindow ?? PopupExtension.GetActiveWindow());
					if (currentRoot == null)
					{
						return true;
					}

					if (this._popupView?.Handler?.PlatformView is UIView popupNative)
					{
						// Convert popup origin to the root coordinate space and test containment.
						var origin = popupNative.ConvertPointToView(new CGPoint(0, 0), currentRoot);
						var rect = new CGRect(origin.X, origin.Y, popupNative.Bounds.Width, popupNative.Bounds.Height);
						var location = touch.LocationInView(currentRoot);
						if (rect.Contains(location))
						{
							// Touch began inside popup - do not let root recognizer handle it.
							return false;
						}
					}
				}
				catch
				{
					// If anything goes wrong, fall back to allowing the recognizer to receive the touch.
				}

				return true;
			};

			root.AddGestureRecognizer(this.rootTapRecognizer);
		}

		/// <summary>
		/// Removes the root-level tap recognizer.
		/// </summary>
		private void RemoveRootTapRecognizer()
		{
			// Resolve current key window to remove recognizer from active root
			UIWindow? keyWindow = PopupExtension.GetActiveWindow();
			var root = (UIView?)(keyWindow ?? WindowOverlayHelper._platformRootView);

			if (root != null && this.rootTapRecognizer != null)
			{
				root.RemoveGestureRecognizer(this.rootTapRecognizer);
				this.rootTapRecognizer.Dispose();
				this.rootTapRecognizer = null;
			}
		}
	}
}