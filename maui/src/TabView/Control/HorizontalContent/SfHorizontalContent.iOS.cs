using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using UIKit;
using Syncfusion.Maui.Toolkit.Platform;
using Syncfusion.Maui.Toolkit.TextInputLayout;

namespace Syncfusion.Maui.Toolkit.TabView
{
	internal partial class SfHorizontalContent
	{
		#region Fields

		internal bool _canProcessTouch = true;
		bool _isTapGestureRemoved;
		UIPanGestureRecognizer? _panGesture;
		LayoutViewExt? _nativeView;

		#endregion

		#region Interface Methods

		/// <summary>
		/// This method is used to handle the tap gesture.
		///  If `canProcessTouch` is `false`, it sets it to `true`.
		/// </summary>
		/// <param name="e">Tap event arguments</param>
		void ITapGestureListener.OnTap(TapEventArgs e)
		{
			if (!_canProcessTouch)
			{
				_canProcessTouch = true;
				return;
			}
		}

		/// <summary>
		/// Handles the gesture based on which child view is tapped.
		/// </summary>
		/// <param name="view">view.</param>
		void ITapGestureListener.ShouldHandleTap(object view)
		{
#if IOS || MACCATALYST
			UIKit.UIView? touchView = (view as UIKit.UITouch)?.View;
			_canProcessTouch = true;
			if (IsSpecialView(touchView))
			{
				_canProcessTouch = false;
			}
			else if (touchView is Syncfusion.Maui.Toolkit.Platform.LayoutViewExt layoutViewExt)
			{
				if (layoutViewExt.Drawable is SfTextInputLayout || layoutViewExt.Drawable is Editor )
				{
					// for SfTextInputLayout || for Editor inside LayoutViewExt
					this._canProcessTouch = false;
				}
            }
			else if (touchView is Microsoft.Maui.Platform.MauiImageView)
			{
				HandleMauiImageViewOnTap(view);
			}
			else
			{
				// Provide the touch to the list view(framework) by removing the TapGesture.
				if (touchView is not Syncfusion.Maui.Toolkit.Platform.LayoutViewExt &&
					touchView is not Syncfusion.Maui.Toolkit.Platform.NativePlatformGraphicsView)
				{
					if (touchView is MauiTextField || touchView is MauiTextView || touchView is UIKit.UITextField)
					{
						this._canProcessTouch = false;
					}
					else if (view is UIKit.UITouch uiTouch)
                {
				// For SfTextInputLayout or similar views that require precise touch interactions.
                    var touchLocation = uiTouch.LocationInView(uiTouch.View?.Superview);
					var textInputView = FindSfTextInputLayout(uiTouch.View?.Superview);
                    this._canProcessTouch = true;
                    if (textInputView != null)
                    {
                        if (uiTouch.GestureRecognizers != null)
                        {
                            foreach (var gesture in uiTouch.GestureRecognizers)
                            {
                                if (gesture is UILongPressGestureRecognizer)
                                {
                                    this.RemoveGestureListener(this);
                                    this._isTapGestureRemoved = true;
                                }
                            }
                        }
                    }
					else
                    {
                        if (uiTouch.GestureRecognizers != null)
						{
							foreach (var gesture in uiTouch.GestureRecognizers)
							{
								if (gesture is UILongPressGestureRecognizer)
								{
									this._canProcessTouch = false;
								}
							}
						}
                    }
				}
			  }
                
			}
#endif
		}

		/// <summary>
		/// Finds and returns the native <see cref="UIView"/> that represents a <see cref="SfTextInputLayout"/>
		/// </summary>
		/// <param name="root">The root <see cref="UIView"/> to start the search from.</param>
		/// <returns>
		/// The matching <see cref="UIView"/> that wraps a <see cref="SfTextInputLayout"/> if found; otherwise, <c>null</c>.
		/// </returns>
		private UIView? FindSfTextInputLayout(UIView? root)
		{
			if (root == null)
			{
				return null;
			}
			else
			{
				return root;
			}
		}

		/// <summary>
		/// This method triggers on any touch interaction on <see cref="SfHorizontalContent"/>.
		/// </summary>
		/// <param name="e">The pointer event args.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
		{
			if (!_canProcessTouch)
			{
				return;
			}

			switch (e.Action)
			{
				case PointerActions.Pressed:
					OnHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
					break;

				case PointerActions.Moved:
					OnHandleTouchInteraction(PointerActions.Moved, e.TouchPoint);
					break;

				case PointerActions.Released:
					if (_isTapGestureRemoved)
					{
						this.AddGestureListener(this);
						_isTapGestureRemoved = false;
					}
					OnHandleTouchInteraction(PointerActions.Released, e.TouchPoint);
					break;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Check if the touch view is one of the special views we don't want to process.
		/// </summary>
		/// <param name="touchView">The touched view</param>
		/// <returns>True if it's a special view, otherwise false</returns>
		static bool IsSpecialView(UIKit.UIView? touchView)
		{
			return touchView is Syncfusion.Maui.Toolkit.Platform.PlatformGraphicsViewExt ||
				touchView is Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel ||
				touchView is Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem ||
				touchView is Syncfusion.Maui.Toolkit.Platform.NativePlatformGraphicsView;
		}

		void HandleMauiImageViewOnTap(object view)
		{
			if (view is UIKit.UITouch uiTouch &&
				uiTouch?.GestureRecognizers != null &&
				uiTouch.GestureRecognizers.Length >= 2)
			{
				for (int i = 0; i < uiTouch.GestureRecognizers.Length - 1; i++)
				{
					if (uiTouch.GestureRecognizers[i] is UIPanGestureRecognizer)
					{
						_canProcessTouch = false;
						return;
					}
				}
			}
		}

		void HandleOtherViewsOnTap(UIKit.UIView? touchView, object view)
		{
			// Disable touch processing for specific input views.
			if (touchView is MauiTextField || touchView is MauiTextView || touchView is UIKit.UITextField)
			{
				_canProcessTouch = false;
			}
			else if (view is UIKit.UITouch uiTouch)
			{
				if (uiTouch != null && uiTouch.GestureRecognizers != null)
				{
					foreach (var gesture in uiTouch.GestureRecognizers)
					{
						if (gesture is UILongPressGestureRecognizer)
						{
							this.RemoveGestureListener(this);
							_isTapGestureRemoved = true;
						}
					}
				}
			}
		}

		/// <summary>
		/// This method is used to initialize the gesture.
		/// </summary>
		void InitializeGesture()
		{
			if (Handler != null && Handler.PlatformView != null)
			{
				_nativeView = Handler.PlatformView as LayoutViewExt;
				if (_nativeView != null &&
					_nativeView.GestureRecognizers != null &&
					_nativeView.GestureRecognizers.Length > 0)
				{
					_panGesture = _nativeView.GestureRecognizers.FirstOrDefault(x => x is UIPanGestureRecognizer) as UIPanGestureRecognizer;
					if (_panGesture != null)
					{
						_panGesture.ShouldBegin += _proxy.GestureShouldBegin;
					}
				}
			}
			else
			{
				Dispose();
			}
		}

		/// <summary>
		/// Unwires wired events and disposes used objects.
		/// </summary>
		void Dispose()
		{
			if (_panGesture != null)
			{
				_panGesture.ShouldBegin -= _proxy.GestureShouldBegin;
			}

			_nativeView = null;
			_panGesture = null;
		}

		void ConfigureTouch()
		{
			InitializeGesture();
		}
		#endregion

		#region Override Methods

		/// <summary>
		/// Raises on handler changing event to dispose old resources.
		/// </summary>
		/// <param name="args">Relevant <see cref="HandlerChangingEventArgs"/>.</param>
		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
		{
			if (args.OldHandler != null)
			{
				Dispose();
			}

			base.OnHandlerChanging(args);
		}

		#endregion
	}

#if IOS || MACCATALYST

	internal class SfHorizontalContentProxy(SfHorizontalContent horizontalContent)
	{
		readonly WeakReference<SfHorizontalContent> _view = new(horizontalContent);

		internal bool GestureShouldBegin(UIGestureRecognizer uIGestureRecognizer)
		{
			_view.TryGetTarget(out var view);
			if (view != null)
			{
				bool? isPressed = view?._canProcessTouch;
				if (isPressed == false)
				{
					// Return false if the CanProcessTouch value is false, preventing the control's gesture from starting.
					return false;
				}
			}

			return true;
		}

	}
}

#endif