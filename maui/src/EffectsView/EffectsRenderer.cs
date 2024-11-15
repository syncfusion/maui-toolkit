using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.EffectsView
{
	/// <summary>
	/// Represents the EffectsRenderer class.
	/// </summary>
	internal class EffectsRenderer : ITouchListener
    {
        #region Fields

        readonly IDrawable? _drawable;

        ICanvas? _currentCanvas;

        bool _shouldDrawHighlight = false;

        bool _shouldDrawRipple = false;

        readonly HighlightEffectLayer? _highlightEffectLayer;

        readonly RippleEffectLayer? _rippleEffectLayer;

        RectF _rippleBounds;

        RectF _highlightBounds;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectsRenderer"/> class.
        /// </summary>
        /// <param name="drawableView">The drawable view.</param>
        public EffectsRenderer(View drawableView)
        {
            drawableView.AddTouchListener(this);

            if (drawableView is IDrawable dView)
            {
                _drawable = dView;
                _rippleEffectLayer = new RippleEffectLayer(RippleColorBrush, 1000, dView, drawableView);
                _highlightEffectLayer = new HighlightEffectLayer(HighlightBrush, dView);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the highlight color brush value.
        /// </summary>
        internal Brush HighlightBrush { get; set; } = new SolidColorBrush(Color.FromRgba(200, 200, 200, 50));

        /// <summary>
        /// Gets or sets the ripple color brush value.
        /// </summary>
        internal Brush RippleColorBrush { get; set; } = new SolidColorBrush(Color.FromRgba(150, 150, 150, 75));

        /// <summary>
        /// Gets or sets the ripple animation duration value.
        /// </summary>
        internal double RippleAnimationDuration { get; set; } = 400;

        /// <summary>
        /// Gets the value indicates whether to draw highlight.
        /// </summary>
        internal bool ShouldDrawHighlight
        {
            get
            {
                return _shouldDrawHighlight;
            }
            private set
            {
                if (ShouldDrawHighlight != value)
                {
                    _shouldDrawHighlight = value;
                    InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Gets the value indicates whether to draw ripple.
        /// </summary>
        internal bool ShouldDrawRipple
        {
            get
            {
                return _shouldDrawRipple;
            }
            private set
            {
                if (_shouldDrawRipple != value)
                {
                    _shouldDrawRipple = value;
                    InvalidateDrawable();
                }
            }

        }

        /// <summary>
        /// Gets the ripple bounds value.
        /// </summary>
        internal RectF RippleBounds
        {
            get
            {
                return _rippleBounds;
            }
            private set
            {
                if (_rippleBounds != value)
                {
                    _rippleBounds = value;
                    InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Gets the highlight bounds value.
        /// </summary>
        internal RectF HighlightBounds
        {
            get
            {
                return _highlightBounds;
            }
            private set
            {
                if (_highlightBounds != value)
                {
                    _highlightBounds = value;
                    InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Gets or sets the ripple bounds collection value.
        /// </summary>
        internal ObservableCollection<RectF> RippleBoundsCollection { get; set; } = new();

        /// <summary>
        /// Gets or sets the highlight bounds collection value.
        /// </summary>
        internal ObservableCollection<RectF> HighlightBoundsCollection { get; set; } = new();

        /// <summary>
        /// Gets or sets the value indicates whether the flowdirection is RTL or not.
        /// </summary>
        internal bool IsRTL { get; set; } = false;

        /// <summary>
        /// Gets or sets the Control Width value.
        /// </summary>
        internal double ControlWidth { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value indicates whether pressed or not.
        /// </summary>
        internal bool IsPressed { get; set; } = false;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Invokes animation completed event.
        /// </summary>
        /// <param name="eventArgs">Animation completed events argument.</param>
        internal void RaiseAnimationCompletedEvent(EventArgs eventArgs)
        {
            AnimationCompleted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Method to draw effects.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="isCornerRadius">The value indicates whether to get corner radius canvas.</param>
        internal void DrawEffects(ICanvas canvas, bool isCornerRadius = true)
        {
            if (HighlightBounds.Width > 0 && HighlightBounds.Height > 0)
            {
				canvas.CanvasSaveState();
                _currentCanvas = isCornerRadius ? GetCornerRadiusCanvas(canvas, HighlightBounds) : canvas;
                DrawHighlight(HighlightBounds);
                canvas.CanvasRestoreState();
            }
            if (RippleBounds.Width > 0 && RippleBounds.Height > 0)
            {
				canvas.CanvasSaveState();
                _currentCanvas = isCornerRadius ? GetCornerRadiusCanvas(canvas, RippleBounds) : canvas;
                DrawRipple(RippleBounds);
                canvas.CanvasRestoreState();
            }
        }

        /// <summary>
        /// Method to remove ripple.
        /// </summary>
        internal void RemoveRipple()
        {
            _rippleEffectLayer?.OnRippleAnimationFinished();
        }

        /// <summary>
        /// Method to remove highlight.
        /// </summary>
        internal void RemoveHighlight()
        {
            if (_highlightEffectLayer != null)
            {
                HighlightBounds = new RectF(0, 0, 0, 0);
                _highlightEffectLayer.UpdateHighlightBounds();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get corner radius canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="bounds">The rectangle.</param>
        /// <returns></returns>
        ICanvas GetCornerRadiusCanvas(ICanvas canvas, RectF bounds)
        {
            PathF cornerRadiusPath = new();
            //Below is the calculation since ripple effect cut in the border
            bounds.Top += 3;
            bounds.Left += 3;
            bounds.Height -= 6;
            bounds.Width -= 6;
            cornerRadiusPath.AppendRoundedRectangle(bounds, bounds.Height / 2, false);
            canvas.ClipPath(cornerRadiusPath);
            return canvas;
        }

        /// <summary>
        /// Method to draw ripple.
        /// </summary>
        /// <param name="rectF">The rectangle.</param>
        void DrawRipple(RectF rectF)
        {
            if (ShouldDrawRipple && _rippleEffectLayer != null && _currentCanvas != null)
                _rippleEffectLayer.DrawRipple(_currentCanvas, rectF, RippleColorBrush, true);
        }

        /// <summary>
        /// Method to draw highlight.
        /// </summary>
        /// <param name="rectF">The rectangle.</param>
        void DrawHighlight(RectF rectF)
        {
            if (ShouldDrawHighlight && _highlightEffectLayer != null && _currentCanvas != null)
                _highlightEffectLayer.DrawHighlight(_currentCanvas, rectF, HighlightBrush);
        }

        /// <summary>
        /// Method to invalidate drawable.
        /// </summary>
        void InvalidateDrawable()
        {
            if (_drawable is IDrawableLayout drawableLayout)
                drawableLayout.InvalidateDrawable();
            else if (_drawable is IDrawableView drawableView)
                drawableView.InvalidateDrawable();
        }

        /// <summary>
        /// Method to check whether the bounds contains the point. 
        /// </summary>
        /// <param name="p">The point.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="isRipple">Is ripple value.</param>
        void CheckBoundsContainsPoint(Point p, ObservableCollection<RectF> bounds, bool isRipple)
        {
            foreach (var item in bounds)
            {
                if (item.Contains(p))
                {
                    if (isRipple)
                    {
                        RippleBounds = item;
                        ShouldDrawRipple = true;
                    }
                    else
                    {
                        HighlightBounds = item;
                        ShouldDrawHighlight = true;
                    }
                    return;
                }
            }

            if (isRipple)
            {
                ShouldDrawRipple = false;
            }
            else
            {
                ShouldDrawHighlight = false;
            }

        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Touch action method.
        /// </summary>
        /// <param name="e">The touch event arguments.</param>
        public void OnTouch(PointerEventArgs e)
        {
            Point touchPoint = e.TouchPoint;
            if (IsRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                touchPoint = e.TouchPoint;
                touchPoint.X = ControlWidth - touchPoint.X;
            }
            else
            {
                touchPoint = e.TouchPoint;
            }
            if (e.Action == PointerActions.Moved)
            {
                if (IsPressed)
                {
                    return;
                }
                CheckBoundsContainsPoint(touchPoint, HighlightBoundsCollection, false);
            }
            else if (e.Action == PointerActions.Pressed)
            {
                IsPressed = true;
                CheckBoundsContainsPoint(touchPoint, RippleBoundsCollection, true);
                if (ShouldDrawRipple && _rippleEffectLayer != null)
                {
                    _rippleEffectLayer.StartRippleAnimation(touchPoint, RippleColorBrush, RippleAnimationDuration, 0f, true, _rippleBounds.Width, _rippleBounds.Height, this);
                }
                else
                {
                    if (_rippleBounds.Width > 0 && _rippleBounds.Height > 0)
                        RemoveRipple();
                }
            }
            else if (e.Action == PointerActions.Released)
            {
                IsPressed = false;
                if (_rippleBounds.Width > 0 && _rippleBounds.Height > 0)
                    RemoveRipple();
#if ANDROID
                if (_highlightBounds.Width > 0 && _highlightBounds.Height > 0)
                    RemoveHighlight();
#endif
            }
            else if (e.Action == PointerActions.Cancelled || e.Action == PointerActions.Exited)
            {
                ShouldDrawHighlight = false;
                ShouldDrawRipple = false;
                if (_rippleBounds.Width > 0 && _rippleBounds.Height > 0)
                    RemoveRipple();
                if (_highlightBounds.Width > 0 && _highlightBounds.Height > 0)
                    RemoveHighlight();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// The <see cref="AnimationCompleted"/> event occurs when the ripple animation is finished.
        /// </summary>
        internal event EventHandler? AnimationCompleted;

        #endregion
    }
}
