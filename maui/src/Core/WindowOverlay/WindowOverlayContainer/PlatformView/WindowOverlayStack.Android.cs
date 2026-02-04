using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal class WindowOverlayStack : FrameLayout
    {
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
	}

}
