using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// iOS / MacCatalyst partial of <see cref="ExpandCollapseButton"/>.
    /// Implements the touch path on iOS and MacCatalyst. The button
    /// has no platform-specific behavior beyond the cross-platform
    /// press / release / click handling, so the implementation is
    /// shared with Windows and Android.
    /// </summary>
    internal sealed partial class ExpandCollapseButton
    {
        /// <summary>
        /// iOS / MacCatalyst implementation of
        /// <see cref="ITouchListener.OnTouch"/>. Tracks the pressed
        /// state and raises <see cref="Clicked"/> on release while
        /// still pressed.
        /// </summary>
        /// <param name="e">Pointer event arguments.</param>
        public void OnTouch(PointerEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            switch (e.Action)
            {
                case PointerActions.Entered:
                case PointerActions.Moved:
                    if (!IsPointerOver)
                    {
                        IsPointerOver = true;
                        PointerOverChanged?.Invoke(this, EventArgs.Empty);
                    }
                    InvalidateDrawable();
                    break;

                case PointerActions.Pressed:
                    _isPressed = true;
                    _clickRaised = false;
                    InvalidateDrawable();
                    break;

                case PointerActions.Released:
                    if (_isPressed)
                    {
                        _isPressed = false;
                        InvalidateDrawable();
                        RaiseClicked();
                    }
                    break;

                case PointerActions.Cancelled:
                case PointerActions.Exited:
                    if (_isPressed)
                    {
                        _isPressed = false;
                        InvalidateDrawable();
                    }
                    if (IsPointerOver)
                    {
                        IsPointerOver = false;
                        PointerOverChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
            }
        }

        /// <summary>
        /// iOS / MacCatalyst implementation of
        /// <see cref="ITouchListener.OnScrollWheel"/>. No-op: the
        /// button does not consume scroll input.
        /// </summary>
        /// <param name="e">Scroll event arguments.</param>
        public void OnScrollWheel(ScrollEventArgs e)
        {
            // No scroll-wheel behavior on the button.
        }
    }
}
