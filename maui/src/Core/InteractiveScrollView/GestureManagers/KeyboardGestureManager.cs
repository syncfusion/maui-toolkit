namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Provides keyboard-based scrolling support for the <see cref="SfInteractiveScrollView"/> and translates keyboard navigation keys into scrolling actions.
    /// </summary>
    internal class KeyboardGestureManager : IKeyboardListener
    {
        #region Fields

        /// <summary>
        /// Holds the associated interactive scroll view.
        /// </summary>
        SfInteractiveScrollView _scrollView;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardGestureManager"/> class.
        /// </summary>
        /// <param name="scrollView">The interactive scroll view that receives keyboard-based scrolling support.</param>
        internal KeyboardGestureManager(SfInteractiveScrollView scrollView)
        {
            _scrollView = scrollView;
            _scrollView.AddKeyboardListener(this);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to remove the keyboard listener.
        /// </summary>
        internal void Dispose()
        {
            _scrollView?.RemoveKeyboardListener(this);
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Handles key press events and performs scrolling based on the pressed key.
        /// </summary>
        /// <param name="args">The keyboard event data.</param>
        void IKeyboardListener.OnKeyDown(KeyEventArgs args)
        {
            if (_scrollView == null)
                return;

            float scrollDelta = 60;
            switch (args.Key)
            {
                case KeyboardKey.Down:
                    _scrollView.ScrollToY(_scrollView.ScrollY + scrollDelta, true);
                    break;
                case KeyboardKey.Up:
                    _scrollView.ScrollToY(_scrollView.ScrollY - scrollDelta, true);
                    break;
                case KeyboardKey.Left:
                    _scrollView.ScrollToX(_scrollView.ScrollX - scrollDelta, true);
                    break;
                case KeyboardKey.Right:
                    _scrollView.ScrollToX(_scrollView.ScrollX + scrollDelta, true);
                    break;
                case KeyboardKey.Home:
                    if (args.IsShiftKeyPressed)
                        _scrollView.ScrollToX(0, true);
                    else
                        _scrollView.ScrollToY(0, true);
                    break;
                case KeyboardKey.End:
                    if (args.IsShiftKeyPressed)
                        _scrollView.ScrollToX(_scrollView.ContentSize.Width, true);
                    else
                        _scrollView.ScrollToY(_scrollView.ContentSize.Height, true);
                    break;
                case KeyboardKey.PageUp:
                    if (args.IsShiftKeyPressed)
                        _scrollView.ScrollToX(_scrollView.ScrollX - _scrollView.ViewportWidth, true);
                    else
                        _scrollView.ScrollToY(_scrollView.ScrollY - _scrollView.ViewportHeight, true);
                    break;
                case KeyboardKey.PageDown:
                    if (args.IsShiftKeyPressed)
                        _scrollView.ScrollToX(_scrollView.ScrollX + _scrollView.ViewportWidth, true);
                    else
                        _scrollView.ScrollToY(_scrollView.ScrollY + _scrollView.ViewportHeight, true);
                    break;
            }
        }

        /// <summary>
        /// Handles key release events.
        /// </summary>
        /// <param name="args">The keyboard event data.</param>
        void IKeyboardListener.OnKeyUp(KeyEventArgs args)
        {
            // The method implemented as a part of the interface.
        }

        #endregion
    }
}