using System;

namespace Syncfusion.Maui.Toolkit.EffectsView
{
    /// <summary>
    /// Specifies the type of the animation for the views in <see cref="SfEffectsView" />.
    /// </summary>
    /// <remarks>This enumeration has a Flags attribute that allows a bitwise combination of its member values.</remarks>
    [Flags]
    public enum SfEffects
    {
        /// <summary>
        /// No effect.
        /// </summary>
        None = 0,

        /// <summary>
        /// Smooth transition on the background color of the view.
        /// </summary>
        Highlight = 1 << 0,

        /// <summary>
        /// Ripple is a growable circle, and it grows till the whole layout is filled.
        /// It depends on <see cref="SfEffectsView.InitialRippleFactor"/>.
        /// </summary>
        Ripple = 1 << 1,

        /// <summary>
        /// Scale is smooth scaling transition from actual size to the specified
        /// <see cref="SfEffectsView.ScaleFactor"/> in pixels.
        /// </summary>
        Scale = 1 << 2,

        /// <summary>
        /// Selection is smooth color transition to denote the view state as Selected.
        /// </summary>
        Selection = 1 << 3,

        /// <summary>
        /// Rotation is a circular movement of the view based on the specified <see cref="SfEffectsView.Angle"/>.
        /// </summary>
        Rotation = 1 << 4,
    }

    /// <summary>
    /// Specifies the start position of the ripple effects.
    /// </summary>
    public enum RippleStartPosition
    {
        /// <summary>
        /// Ripple starts from the left of the view.
        /// </summary>
        Left,

        /// <summary>
        /// Ripple starts from the top of the view.
        /// </summary>
        Top,

        /// <summary>
        /// Ripple starts from the right of the view.
        /// </summary>
        Right,

        /// <summary>
        /// Ripple starts from the bottom of the view.
        /// </summary>
        Bottom,

        /// <summary>
        /// Ripple starts from the center of the view.
        /// </summary>
        Default,

        /// <summary>
        /// Ripple starts from the TopLeft of the view.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Ripple starts from the TopRight of the view.
        /// </summary>
        TopRight,

        /// <summary>
        /// Ripple starts from the BottomLeft of the view.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Ripple starts from the BottomRight of the view.
        /// </summary>
        BottomRight,
    }

    /// <summary>
    /// Specifies the effect for AutoResetEffect.
    /// </summary>
    [Flags]
    public enum AutoResetEffects
    {
        /// <summary>
        /// No effect.
        /// </summary>
        None = 0,

        /// <summary>
        /// Smooth transition on the background color of the view.
        /// </summary>
        Highlight = 1 << 0,

        /// <summary>
        /// Ripple is a growable circle, and it grows till the whole layout is filled.
        /// It depends on <see cref="SfEffectsView.InitialRippleFactor"/>.
        /// </summary>
        Ripple = 1 << 1,

        /// <summary>
        /// Scale is smooth scaling transition from actual size to the specified
        /// <see cref="SfEffectsView.ScaleFactor"/> in pixels.
        /// </summary>
        Scale = 1 << 2,
    }
}
