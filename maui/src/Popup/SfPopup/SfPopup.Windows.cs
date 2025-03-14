using System.Numerics;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Shapes;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Platform;
using DropShadow = Microsoft.UI.Composition.DropShadow;
using SpriteVisual = Microsoft.UI.Composition.SpriteVisual;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// <see cref="SfPopup"/> allows the user to display an alert message with the customizable buttons or load any desired view inside the pop-up.
	/// </summary>
	public partial class SfPopup
	{
		#region Fields

		Canvas? shadowCanvas;
		SpriteVisual? shadowVisual;
		Rectangle? shadowHost;
		DropShadow? dropShadow;

		#endregion

		#region Internal Properties

		/// <summary>
		/// This field stores the UIElement blurred by this popup.
		/// </summary>
		internal UIElement? BlurElement;

		#endregion

		#region Internal Methods
		/// <summary>
		/// Used to wire the events.
		/// </summary>
		internal void WirePlatformSpecificEvents()
		{
			if (WindowOverlayHelper._platformWindow is not null)
			{
				WindowOverlayHelper._platformWindow.SizeChanged += OnPlatformWindowSizeChanged;
			}

			if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.PlatformView is not null &&
					_popupView.Handler.PlatformView is LayoutPanelExt popupNativeView && popupNativeView is not null)
			{
				popupNativeView.SizeChanged += OnNativePopupViewSizeChanged;
			}
		}

		/// <summary>
		/// Used to Unwire the events.
		/// </summary>
		internal void UnWirePlatformSpecificEvents()
		{
			if (WindowOverlayHelper._platformWindow is not null)
			{
				WindowOverlayHelper._platformWindow.SizeChanged -= OnPlatformWindowSizeChanged;
			}

			if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.PlatformView is not null &&
					_popupView.Handler.PlatformView is LayoutPanelExt popupNativeView && popupNativeView is not null)
			{
				popupNativeView.SizeChanged -= OnNativePopupViewSizeChanged;
			}
		}

		/// <summary>
		/// Method updates the mask for the shadow.
		/// </summary>
		internal async void UpdateNativePopupViewShadowMask()
		{
			if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.PlatformView is not null &&
					_popupView.Handler.PlatformView is LayoutPanelExt popupNativeView && popupNativeView is not null)
			{
				if (dropShadow is not null)
				{
					// The Popup shadow is updated with _popupViewWidth and _popupViewHeight.
					dropShadow.Mask = await popupNativeView.GetAlphaMaskAsync((int)_popupViewWidth, (int)_popupViewHeight);
				}
			}
		}

		/// <summary>
		/// Applies shadow to the native popup view.
		/// </summary>
		internal async void ApplyNativePopupViewShadow()
		{
			if (PopupStyle.HasShadow)
			{
				if (shadowCanvas is not null)
				{
					return;
				}

				if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.PlatformView is not null)
				{
					if (!_popupView.Handler.HasContainer)
					{
						_popupView.Handler.HasContainer = true;
						UpdateRTL();
					}

					if (_popupView.Handler.ContainerView is WrapperView containerView && containerView is not null && _popupView.Handler.PlatformView is LayoutPanelExt popupNativeView && popupNativeView is not null)
					{
						shadowCanvas = new Canvas();
						containerView.Children.Insert(0, shadowCanvas);
						var compositor = ElementCompositionPreview.GetElementVisual(shadowCanvas).Compositor;
						shadowHost = new Rectangle() { Fill = new SolidColorBrush(Colors.Transparent).ToBrush(), Width = _popupViewWidth, Height = _popupViewHeight };
						Canvas.SetLeft(shadowHost, 0);
						Canvas.SetTop(shadowHost, 0);
						shadowCanvas.Children.Insert(0, shadowHost);
						dropShadow = compositor.CreateDropShadow();
						dropShadow.BlurRadius = 20f;
						dropShadow.Color = Color.FromRgba(0, 0, 0, 0.3).ToWindowsColor();

						// The Popup shadow is updated with _popupViewWidth and _popupViewHeight.
						dropShadow.Mask = await popupNativeView.GetAlphaMaskAsync((int)_popupViewWidth, (int)_popupViewHeight);
						dropShadow.Offset = new Vector3(0, 0, 0);
						shadowVisual = compositor.CreateSpriteVisual();
						shadowVisual.Shadow = dropShadow;
						shadowVisual.Size = new Vector2((float)_popupViewWidth, (float)_popupViewHeight);
						ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);
					}
				}
			}
			else
			{
				DisposeShadow();
			}
		}

		/// <summary>
		/// Set FlowDirection for container view of popup view.
		/// </summary>
		internal void UpdateRTL()
		{
			if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.ContainerView is not null && _popupView.Handler.ContainerView is WrapperView wrapperView)
			{
				wrapperView.FlowDirection = _isRTL ? Microsoft.UI.Xaml.FlowDirection.RightToLeft : Microsoft.UI.Xaml.FlowDirection.LeftToRight;
			}
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Method disposes shadow related fields.
		/// </summary>
		void DisposeShadow()
		{
			if (shadowCanvas is not null)
			{
				shadowCanvas.Children.Clear();
				shadowCanvas = null;
			}

			if (shadowHost is not null)
			{
				ElementCompositionPreview.SetElementChildVisual(shadowHost, null);
				shadowHost = null;
			}

			if (shadowVisual is not null)
			{
				shadowVisual.Dispose();
				shadowVisual = null;
			}

			if (dropShadow is not null)
			{
				dropShadow.Mask?.Dispose();
				dropShadow.Dispose();
				dropShadow = null;
			}
		}

		/// <summary>
		/// Occurs when app window size changes.
		/// </summary>
		/// <param name="sender">The platform root view.</param>
		/// <param name="e">The event arguments.</param>
		async void OnPlatformWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
		{
			if (_popupView is not null && _popupOverlay is not null)
			{
				// Added delay here, so the native relative view height will be updated in the CalculateRelativePoint method.
				await Task.Delay(50).ConfigureAwait(true);

				if (IsOpen)
				{
					_popupOverlay.UpdateOverlaySize();

					// Abort the PopupView animation when the window size changes.
					AbortPopupViewAnimation();
					ResetAnimatedProperties();
					ReAssignPopupViewWidthAndHeight();
					ResetPopupWidthHeight();

					// Need to update the effect visual size, when the window is resized or the orientation changes.
					if (IsOpen && ShowOverlayAlways && OverlayMode is PopupOverlayMode.Blur)
					{
						PopupExtension.UpdateEffectVisualSize(this);
					}

					_popupView.InvalidateForceLayout();

				}
			}
		}

		/// <summary>
		/// Occurs when PopupView size changes.
		/// </summary>
		/// <param name="sender">The instance of native PopupView.</param>
		/// <param name="e">The size changed event arguments.</param>
		void OnNativePopupViewSizeChanged(object? sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
		{
			if (shadowVisual is not null && shadowHost is not null)
			{
				if (shadowVisual.Size != new Vector2((float)_popupViewWidth, (float)_popupViewHeight))
				{
					shadowVisual.Size = new Vector2((float)_popupViewWidth, (float)_popupViewHeight);

					shadowHost.Width = _popupViewWidth;
					shadowHost.Height = _popupViewHeight;

					// The PopupView shadow was not updated properly, So updating the native PopupView mask for shadow.
					UpdateNativePopupViewShadowMask();
				}
			}
		}

		#endregion
	}
}
