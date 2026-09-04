using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Cross-platform implementation of <see cref="ExpandCollapseButton"/>.
    /// Platform-specific behavior is provided by the corresponding platform
    /// partial classes (Windows / iOS / Android).
    /// </summary>
    internal sealed partial class ExpandCollapseButton
    {
        /// <summary>
        /// Handles the standard touch path for the button: tracks pressed
        /// state, raises <see cref="Clicked"/> on release while still pressed,
        /// and re-renders the border thickness on Entered/Exited.
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
        /// Standard implementation of <see cref="ITouchListener.OnScrollWheel"/>.
        /// No-op: the button does not consume scroll wheel input.
        /// </summary>
        /// <param name="e">Scroll event arguments.</param>
        public void OnScrollWheel(ScrollEventArgs e)
        {
            // No scroll-wheel behavior on the button.
        }
    }
}
