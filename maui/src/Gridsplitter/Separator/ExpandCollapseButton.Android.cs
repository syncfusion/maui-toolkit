using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Android-specific partial of <see cref="ExpandCollapseButton"/>.
    /// Implements the touch path on Android. The button has no
    /// platform-specific behavior beyond the cross-platform press /
    /// release / click handling, so the implementation is shared with
    /// Windows, iOS, and MacCatalyst.
    /// </summary>
    internal sealed partial class ExpandCollapseButton
    {
        /// <summary>
        /// Android implementation of <see cref="ITouchListener.OnTouch"/>.
        /// Tracks the pressed state and raises <see cref="Clicked"/> on
        /// release while still pressed.
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
        /// Android implementation of <see cref="ITouchListener.OnScrollWheel"/>.
        /// No-op: the button does not consume scroll input.
        /// </summary>
        /// <param name="e">Scroll event arguments.</param>
        public void OnScrollWheel(ScrollEventArgs e)
        {
            // No scroll-wheel behavior on the button.
        }
    }
}
