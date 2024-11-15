using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Platform;
using UIKit;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
    public partial class SfNavigationDrawer
    {
        #region Fields

        /// <summary>
        /// Determines whether the touch should be processed.
        /// </summary>
        internal bool _canProcessTouch = true;

        /// <summary>
        /// Indicates whether the tap gesture has been removed.
        /// </summary>
        bool _isTapGestureRemoved;

        /// <summary>
        /// The pan gesture recognizer used for handling touch events.
        /// </summary>
        UIPanGestureRecognizer? _panGesture;

        /// <summary>
        /// The native view associated with the navigation drawer.
        /// </summary>
        LayoutViewExt? _nativeView;

        #endregion

        #region Private Methods

        /// <summary>
        /// This method used for handle the tap.
        /// </summary>
        /// <param name="e">The TapEventArgs parameter.</param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            if (!_canProcessTouch)
            {
                _canProcessTouch = true;
                return;
            }
        }

        /// <summary>
        /// Determines whether the tap gesture should be handled for the specified view.
        /// </summary>
        /// <param name="view">The view to check.</param>
        void ITapGestureListener.ShouldHandleTap(object view)
        {
            UIKit.UIView? touchView = (view as UIKit.UITouch)?.View;
            _canProcessTouch = true;
            // Provide the touch to the list view(framework) by removing the TapGesture.
            if (touchView is not Syncfusion.Maui.Toolkit.Platform.LayoutViewExt &&
                touchView is not Syncfusion.Maui.Toolkit.Platform.NativePlatformGraphicsView)
            {
                if (view is UIKit.UITouch uiTouch)
                {
                    if (uiTouch.GestureRecognizers != null)
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
        }

        /// <summary>
        /// Initializes the gesture recognizer for handling touch events.
        /// </summary>
        void InitializeGesture()
        {
            if (Handler != null && Handler.PlatformView != null)
            {
                _nativeView = Handler.PlatformView as LayoutViewExt;
                if (_nativeView != null && _nativeView.GestureRecognizers != null && _nativeView.GestureRecognizers.Length > 0)
                {
                    _panGesture = (UIPanGestureRecognizer)_nativeView.GestureRecognizers.FirstOrDefault(x => x is UIPanGestureRecognizer)!;
                    _panGesture.ShouldBegin += _proxy.GestureShouldBegin;
                }
            }
            else
            {
                Dispose();
            }
        }

        /// <summary>
        /// Raises when <see cref="_panGesture"/> begins.
        /// </summary>
        /// <param name="uIGestureRecognizer">Instance of <see cref="_panGesture"/>.</param>
        /// <returns>True, If the gesture should begin, else false.</returns>
        bool GestureShouldBegin(UIGestureRecognizer uIGestureRecognizer)
        {
            if (!_canProcessTouch)
            {
                // This enables drawing on the SignaturePad control.
                // Return false if the CanProcessTouch value is false, preventing the control's gesture from starting.
                return false;
            }

            return true;
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

        /// <summary>
        /// Handles touch input events when a pointer enters the control.
        /// </summary>
        /// <param name="e">Pointer event arguments containing information about the touch event.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            if (!_canProcessTouch) return;

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

                case PointerActions.Cancelled:
                case PointerActions.Entered:
                    break;
            }
        }

        /// <summary>
        /// Method configures all the touch related works.
        /// </summary>
        void ConfigureTouch()
        {
            InitializeGesture();
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Raises on handler changing.
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

	internal class SfNavigationDrawerProxy
	{
		readonly WeakReference<SfNavigationDrawer> _view;
		public SfNavigationDrawerProxy(SfNavigationDrawer navigationDrawer) => _view = new(navigationDrawer);

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

#endif
}