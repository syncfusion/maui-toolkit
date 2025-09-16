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

		Canvas? _shadowCanvas;
		SpriteVisual? _shadowVisual;
		Rectangle? _shadowHost;
		DropShadow? _dropShadow;

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
		/// Used to wire root view loaded events.
		/// </summary>
		internal void WirePlatformViewLoaded()
		{
			if (WindowOverlayHelper._platformRootView is not null && WindowOverlayHelper._platformRootView is FrameworkElement frameworkElement)
			{
				if (!frameworkElement.IsLoaded)
				{
					frameworkElement.Loaded += FrameworkElement_Loaded;
				}
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
				if (_dropShadow is not null)
				{
					// The Popup shadow is updated with _popupViewWidth and _popupViewHeight.
					_dropShadow.Mask = await popupNativeView.GetAlphaMaskAsync((int)_popupViewWidth, (int)_popupViewHeight);
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
				if (_shadowCanvas is not null)
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
						_shadowCanvas = new Canvas();
						containerView.Children.Insert(0, _shadowCanvas);
						var compositor = ElementCompositionPreview.GetElementVisual(_shadowCanvas).Compositor;
						_shadowHost = new Rectangle() { Fill = new SolidColorBrush(Colors.Transparent).ToBrush(), Width = _popupViewWidth, Height = _popupViewHeight };
						Canvas.SetLeft(_shadowHost, 0);
						Canvas.SetTop(_shadowHost, 0);
						_shadowCanvas.Children.Insert(0, _shadowHost);
						_dropShadow = compositor.CreateDropShadow();
						_dropShadow.BlurRadius = 20f;
						_dropShadow.Color = Color.FromRgba(0, 0, 0, 0.3).ToWindowsColor();

						// The Popup shadow is updated with _popupViewWidth and _popupViewHeight.
						_dropShadow.Mask = await popupNativeView.GetAlphaMaskAsync((int)_popupViewWidth, (int)_popupViewHeight);
						_dropShadow.Offset = new Vector3(0, 0, 0);
						_shadowVisual = compositor.CreateSpriteVisual();
						_shadowVisual.Shadow = _dropShadow;
						_shadowVisual.Size = new Vector2((float)_popupViewWidth, (float)_popupViewHeight);
						ElementCompositionPreview.SetElementChildVisual(_shadowHost, _shadowVisual);
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
		/// Occurs when platformview got loaded.
		/// </summary>
		/// <param name="sender">The platform root view.</param>
		/// <param name="e">The event arguments.</param>
		private void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
		{
			InitializeOverlay();
			CheckAndOpenDeferredPopup();
			if (WindowOverlayHelper._platformRootView is not null && WindowOverlayHelper._platformRootView is FrameworkElement frameworkElement)
			{
				frameworkElement.Loaded -= FrameworkElement_Loaded;
			}
		}

		/// <summary>
		/// Method disposes shadow related fields.
		/// </summary>
		void DisposeShadow()
		{
			if (_shadowCanvas is not null)
			{
				_shadowCanvas.Children.Clear();
				_shadowCanvas = null;
			}

			if (_shadowHost is not null)
			{
				ElementCompositionPreview.SetElementChildVisual(_shadowHost, null);
				_shadowHost = null;
			}

			if (_shadowVisual is not null)
			{
				_shadowVisual.Dispose();
				_shadowVisual = null;
			}

			if (_dropShadow is not null)
			{
				_dropShadow.Mask?.Dispose();
				_dropShadow.Dispose();
				_dropShadow = null;
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
					ResetPopupWidthHeight();

					SyncPopupDimensionFields();

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
			if (_shadowVisual is not null && _shadowHost is not null)
			{
				if (_shadowVisual.Size != new Vector2((float)_popupViewWidth, (float)_popupViewHeight))
				{
					_shadowVisual.Size = new Vector2((float)_popupViewWidth, (float)_popupViewHeight);

					_shadowHost.Width = _popupViewWidth;
					_shadowHost.Height = _popupViewHeight;

					// The PopupView shadow was not updated properly, So updating the native PopupView mask for shadow.
					UpdateNativePopupViewShadowMask();
				}
			}
		}

		#endregion
	}
}
