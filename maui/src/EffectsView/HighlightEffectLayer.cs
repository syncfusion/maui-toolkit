using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.EffectsView
{
    /// <summary>
    /// Represents the HighlightEffectLayer class.
    /// </summary>
    internal class HighlightEffectLayer
    {
        #region Fields

        /// <summary>
        /// Represents the highlight transparency factor.
        /// </summary>
        const float _highlightTransparencyFactor = 0.08f;

        /// <summary>
        /// Represents highlight bounds.
        /// </summary>
        Rect _highlightBounds;

        /// <summary>
        /// Represents the highlight color.
        /// </summary>
        Brush _highlightColor = new SolidColorBrush(Colors.Black);

        readonly IDrawable _drawable;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEffectLayer"/> class.
        /// </summary>
        /// <param name="highlightColor">The highlight color.</param>
        /// <param name="drawable">The drawable.</param>
        public HighlightEffectLayer(Brush highlightColor, IDrawable drawable)
        {
            _highlightColor = highlightColor;
            _drawable = drawable;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets highLight effect layer width.
        /// </summary>
        internal double Width { get; set; }

        /// <summary>
        /// Gets or sets highLight effect layer height
        /// </summary>
        internal double Height { get; set; }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to draw highlight.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rectangle.</param>
        /// <param name="highlightColorValue">The hightlight color value.</param>
        internal void DrawHighlight(ICanvas canvas, RectF rectF, Brush highlightColorValue)
        {
            if (_highlightColor != null)
            {
                canvas.SetFillPaint(highlightColorValue, rectF);
                canvas.FillRectangle(rectF);
            }
        }

        /// <summary>
        /// The draw method of HighlightEffectLayer
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        internal void DrawHighlight(ICanvas canvas)
        {
            if (_highlightColor != null)
            {
                canvas.Alpha = _highlightTransparencyFactor;
                DrawHighlight(canvas, _highlightBounds, _highlightColor);
            }
        }

        /// <summary>
        /// Update highlight bounds method.
        /// </summary>
        /// <param name="width">The width property.</param>
        /// <param name="height">The height property.</param>
        /// <param name="highlightColor">The highlight color.</param>
        internal void UpdateHighlightBounds(double width = 0, double height = 0, Brush? highlightColor = null)
        {
            highlightColor ??= new SolidColorBrush(Colors.Transparent);

            _highlightColor = highlightColor;
            _highlightBounds = new Rect(0, 0, width, height);
            if (_drawable is IDrawableLayout drawableLayout)
                drawableLayout.InvalidateDrawable();
            else if (_drawable is IDrawableView drawableView)
                drawableView.InvalidateDrawable();
        }
    }

    #endregion
}
