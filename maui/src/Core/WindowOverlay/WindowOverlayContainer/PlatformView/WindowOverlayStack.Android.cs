using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Activity;
using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal class WindowOverlayStack : FrameLayout
    {
		// Back press callback registered with the host activity's OnBackPressedDispatcher.
		private OnBackPressedCallback? backPressedCallback;

		// To check whether the popup has Overlay Mode blur.
		internal bool HasBlurMode { get; set; }

		// Represents the cross platform overlay view used for event delegation.
		internal WindowOverlayContainer? OverlayContanier { get; set; }

		public WindowOverlayStack(Context context)
            : base(context)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs)
            : base(context, attrs)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected WindowOverlayStack(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
		}

		/// <summary>
		/// Handles key events dispatched to the current view.
		/// </summary>
		/// <param name="e">The key event for this action, or null.</param>
		/// <returns>True if handled by the view hierarchy; otherwise, false.</returns>
		public override bool DispatchKeyEvent(KeyEvent? e)
		{
			// Forwards the Back key press to the OverlayContanier for handling overlay actions.
			if (e != null && e.KeyCode == Keycode.Back && OverlayContanier != null)
			{
				OverlayContanier.ProcessBackButtonPressed();
			}

			return base.DispatchKeyEvent(e);
		}

		/// <summary>
		/// Handles touch events dispatched to the current view.
		/// </summary>
		/// <param name="e">The touch event for this action, or null.</param>
		/// <returns>True if handled by the view hierarchy; otherwise, false.</returns>
		public override bool DispatchTouchEvent(MotionEvent? e)
		{
			if (e != null && OverlayContanier != null && OverlayContanier.canHandleTouch)
			{
				OverlayContanier.ProcessTouchInteraction(e.RawX / WindowOverlayHelper._density, e.RawY / WindowOverlayHelper._density);
			}

			return base.DispatchTouchEvent(e);
		}

		/// <summary>
		/// Called when this view is attached to a window. Registers an <see cref="AndroidX.Activity.OnBackPressedCallback"/>
		/// with the host <see cref="AndroidX.Activity.ComponentActivity"/>'s <see cref="AndroidX.Activity.OnBackPressedDispatcher"/>
		/// so the overlay can handle system Back presses without requiring view focus.
		/// </summary>
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (this.OverlayContanier != null && this.OverlayContanier.canHandleTouch)
			{
				var window = WindowOverlayHelper._window;
				if (window != null && window.Handler != null && window.Handler is WindowHandler windowHandler
						&& windowHandler.PlatformView is ComponentActivity activity && activity != null && this.backPressedCallback == null)
				{
					this.backPressedCallback = new BackPressedHandler(this.OverlayContanier);
					activity.OnBackPressedDispatcher.AddCallback(this.backPressedCallback);
				}
			}
		}

		/// <summary>
		/// Called when this view is detached from the window. Unregisters any previously
		/// registered back-press callback to avoid leaks and restore normal back handling.
		/// </summary>
		protected override void OnDetachedFromWindow()
		{
			if (this.backPressedCallback != null)
			{
				this.backPressedCallback.Remove();
				this.backPressedCallback = null;
			}

			base.OnDetachedFromWindow();
		}
	}

	/// <summary>
	/// Handles Android back-button callbacks for the overlay stack.
	/// </summary>
	/// <remarks>
	/// This callback is registered with the host Activity's
	/// <see cref="AndroidX.Activity.ComponentActivity.OnBackPressedDispatcher"/> and
	/// forwards back-press events to the cross-platform
	/// <see cref="WindowOverlayContainer"/> so overlays can handle them.
	/// </remarks>
	internal class BackPressedHandler : OnBackPressedCallback
	{
		/// <summary>
		/// Reference to the cross-platform overlay container that will receive
		/// delegated back-button events. May be <c>null</c> if no container is available.
		/// </summary>
		private readonly WindowOverlayContainer? container;

		/// <summary>
		/// Initializes a new instance of the <see cref="BackPressedHandler"/> class.
		/// </summary>
		/// <param name="container">The overlay container to which back presses are forwarded. May be <c>null</c>.</param>
		internal BackPressedHandler(WindowOverlayContainer? container)
			: base(true)
		{
			this.container = container;
		}

		/// <summary>
		/// Called when the system back button is pressed. Forwards the event to
		/// the <see cref="WindowOverlayContainer"/> if it is available.
		/// </summary>
		public override void HandleOnBackPressed()
		{
			if (this.container != null)
			{
				this.container.ProcessBackButtonPressed();
			}
		}
	}

}
