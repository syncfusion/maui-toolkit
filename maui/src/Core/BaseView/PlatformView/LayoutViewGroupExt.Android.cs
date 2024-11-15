using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Microsoft.Maui.Graphics.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using Microsoft.Maui.Platform;
using Android.Runtime;
using ARect = Android.Graphics.Rect;
using Rectangle = Microsoft.Maui.Graphics.Rect;
using Size = Microsoft.Maui.Graphics.Size;
using Android.Views.Accessibility;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.NavigationDrawer;

namespace Syncfusion.Maui.Toolkit.Platform
{
    internal class LayoutViewGroupExt : LayoutViewGroup, AccessibilityManager.IAccessibilityStateChangeListener
    {
        private int _width, _height;
        private PlatformCanvas? _canvas;
        private ScalingCanvas? _scalingCanvas;
        private float _scale = 1;
        private Color? _backgroundColor;
        private readonly Context _context;
        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;
        readonly ARect _clipRect = new();
		WeakReference<IDrawable>? _drawable;
		WeakReference<SfView>? _mauiView;


		/// <summary>
		/// Holds the accessibility delegate instance to handle the accessibility hovering.
		/// </summary>
		private CustomAccessibilityDelegate? customAccessibilityDelegate;

        public LayoutViewGroupExt(Context context) : base(context)
        {
            this.Initialize();
            _context = context;
        }

        public LayoutViewGroupExt(Context context, Microsoft.Maui.Controls.View drawable) : base(context)
        {
            this.Initialize();
            _context = context;
            Drawable = drawable as IDrawable;
            MauiView = (SfView)drawable;
            this.AddAccessibility(context, drawable);
        }

        public LayoutViewGroupExt(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            var context = Context;
            ArgumentNullException.ThrowIfNull(context);
            _context = context;
        }

        public LayoutViewGroupExt(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.Initialize();
            _context = context;
        }

        public LayoutViewGroupExt(Context context, IAttributeSet attrs, IDrawable? drawable = null) : base(context, attrs)
        {
            _context = context;
            Drawable = drawable;
        }

        public LayoutViewGroupExt(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            this.Initialize();
            _context = context;
        }

        public LayoutViewGroupExt(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            this.Initialize();
            _context = context;
        }

		SfView? MauiView
		{
			get => _mauiView != null && _mauiView.TryGetTarget(out var v) ? v : null;
			set => _mauiView = value == null ? null : new(value);
		}

		internal IDrawable? Drawable
		{
			get => _drawable != null && _drawable.TryGetTarget(out var v) ? v : null;
			set
			{
				_drawable = value == null ? null : new(value);
				Invalidate();
			}
		}

        private void Initialize()
        {
            this.SetWillNotDraw(true);
            if (Resources != null && Resources.DisplayMetrics != null)
            {
                _scale = Resources.DisplayMetrics.Density;
            }
        }

        internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }

        internal Func<Rectangle, Size>? CrossPlatformArrange { get; set; }

        public Color? BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                Invalidate();
            }
        }

        internal DrawingOrder DrawingOrder
        {
            get
            {
                return this.drawingOrder;
            }
            set
            {
                this.drawingOrder = value;
                this.UpdateDrawable();
                //// Update the semantics node order while the drawing order changed.
                this.customAccessibilityDelegate?.UpdateChildOrder(this.drawingOrder != DrawingOrder.BelowContent);
            }
        }

        private void UpdateDrawable()
        {
            if (this.DrawingOrder == DrawingOrder.NoDraw)
            {
                this.SetWillNotDraw(true);
                if (_canvas != null)
                {
                    _canvas.Dispose();
                    _canvas = null;
                }
                if (_scalingCanvas != null)
                {
                    _scalingCanvas = null;
                }
            }
            else
            {
                this.SetWillNotDraw(false);
                if (_canvas == null)
                {
                    _canvas = new PlatformCanvas(_context);
                }
                if (_scalingCanvas == null)
                {
                    _scalingCanvas = new ScalingCanvas(_canvas);
                }
            }
        }

        /// <summary>
        /// Invalidates the semantics nodes.
        /// </summary>
        internal void InvalidateSemantics()
        {
            AccessibilityManager? accessibility = (AccessibilityManager?)this.Context?.GetSystemService(Context.AccessibilityService!);
            //// Skip the accessibility creation while accessibility is disabled.
            if (accessibility == null || !accessibility.IsEnabled)
            {
                return;
            }

            this.customAccessibilityDelegate?.InvalidateSemantics();
        }

        protected override void DispatchDraw(Canvas? canvas)
        {
            if (canvas != null)
            {
                if (this.DrawingOrder == DrawingOrder.AboveContent)
                {
                    base.DispatchDraw(canvas);
                    this.DrawContent(canvas);
                }
                else
                {
                    this.DrawContent(canvas);
                    base.DispatchDraw(canvas);
                }
            }
        }

        protected override bool DispatchHoverEvent(MotionEvent? e)
        {
            //// Check the virtual view hovering while accessibility enabled.
            if (this.Context != null)
            {
                AccessibilityManager? accessibility = (AccessibilityManager?)this.Context.GetSystemService(Context.AccessibilityService!);
                if (accessibility != null && accessibility.IsEnabled && this.customAccessibilityDelegate != null && this.customAccessibilityDelegate.DispatchHoverEvent(e))
                {
                    return true;
                }
            }

            return base.DispatchHoverEvent(e);
        }

        /// <summary>
        /// Triggered when the accessibility status changed.
        /// </summary>
        /// <param name="enabled">The accessibility enabled.</param>
        void AccessibilityManager.IAccessibilityStateChangeListener.OnAccessibilityStateChanged(bool enabled)
        {
            if (!enabled || (this.customAccessibilityDelegate != null && AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(this) == this.customAccessibilityDelegate))
            {
                return;
            }

            Microsoft.Maui.Controls.View? drawableView = (Microsoft.Maui.Controls.View?)Drawable;
            if (drawableView == null)
            {
                return;
            }

            this.customAccessibilityDelegate = new CustomAccessibilityDelegate(this, drawableView, this.DrawingOrder != DrawingOrder.BelowContent);
            AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(this, this.customAccessibilityDelegate);
        }

        private void AddAccessibility(Context context, Microsoft.Maui.Controls.View view)
        {
            AccessibilityManager? accessibility = (AccessibilityManager?)context.GetSystemService(Context.AccessibilityService!);
            accessibility?.AddAccessibilityStateChangeListener(this);
            //// Skip the accessibility creation while accessibility is disabled.
            if (accessibility == null || !accessibility.IsEnabled)
            {
                return;
            }

            this.customAccessibilityDelegate = new CustomAccessibilityDelegate(this, view, this.DrawingOrder != DrawingOrder.BelowContent);
            AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(this, this.customAccessibilityDelegate);
        }

        private void DrawContent(Canvas? androidCanvas)
        {
            if (Drawable == null) return;

            var dirtyRect = new Microsoft.Maui.Graphics.RectF(0, 0, _width, _height);

            if (_canvas != null)
            {
                _canvas.Canvas = androidCanvas;
                if (_backgroundColor != null)
                {
                    _canvas.FillColor = _backgroundColor;
                    _canvas.FillRectangle(dirtyRect);
                    _canvas.FillColor = Colors.White;
                }

                _scalingCanvas?.ResetState();
                _scalingCanvas?.Scale(_scale, _scale);
                //Since we are using a scaling canvas, we need to scale the rectangle
                dirtyRect.Height /= _scale;
                dirtyRect.Width /= _scale;
                Drawable.Draw(_scalingCanvas, dirtyRect);
                _canvas.Canvas = null;
            }
        }

        protected override void OnSizeChanged(int width, int height, int oldWidth, int oldHeight)
        {
            base.OnSizeChanged(width, height, oldWidth, oldHeight);
            _width = width;
            _height = height;
        }

        internal void OnMeasureBase(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (CrossPlatformMeasure == null)
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                return;
            }

            var deviceIndependentWidth = widthMeasureSpec.ToDouble(_context);
            var deviceIndependentHeight = heightMeasureSpec.ToDouble(_context);

            var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            var measure = CrossPlatformMeasure(deviceIndependentWidth, deviceIndependentHeight);

            var width = widthMode == MeasureSpecMode.Exactly ? deviceIndependentWidth : measure.Width;
            var height = heightMode == MeasureSpecMode.Exactly ? deviceIndependentHeight : measure.Height;

            var platformWidth = _context.ToPixels(width);
            var platformHeight = _context.ToPixels(height);

            platformWidth = Math.Max(MinimumWidth, platformWidth);
            platformHeight = Math.Max(MinimumHeight, platformHeight);

            SetMeasuredDimension((int)platformWidth, (int)platformHeight);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (CrossPlatformArrange == null || _context == null)
            {
                return;
            }

            var destination = _context.ToCrossPlatformRectInReferenceFrame(l, t, r, b);

            CrossPlatformArrange(destination);

            if (ClipsToBounds)
            {
                _clipRect.Right = r - l;
                _clipRect.Bottom = b - t;
                ClipBounds = _clipRect;
            }
            else
            {
                ClipBounds = null;
            }
        }

        /// <summary>
        /// Overrides the event interception behavior to support pull-to-refresh functionality.
        /// </summary>
        /// <param name="ev">The MotionEvent to intercept.</param>
        /// <returns>True if the event should be intercepted; otherwise, returns base result.</returns>
        /// <remarks>
        /// <see cref="PullToRefreshBase"/> will not receive touch in touch event if its pullable content handles touch, 
        /// hence we need to intercept the touch event to ensure proper handling.
        /// </remarks>
        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            // Checks whether the Drawable is PullToRefreshExt, if it is, then invokes PullToRefreshExt.OnInterceptTouchEvent
            // and intercept if it returns true.
            if (this.Drawable is PullToRefreshBase pullToRefresh && pullToRefresh.OnInterceptTouchEvent(ev))
            {
                return true;
            }

            if (this.Drawable is SfNavigationDrawerExt navigationDrawer && navigationDrawer.OnInterceptTouchEvent(ev))
            {
                return true;
            }

            if (this.Drawable is SfTabViewExt tabView && tabView.OnInterceptTouchEvent(ev))
            {
                return true;
            }

            // If Drawable is not PullToRefreshExt or if PullToRefreshExt doesn't intercept the touch event, proceed with base behavior.
            return base.OnInterceptTouchEvent(ev);
        }

        /// <summary>
        /// This method is called when the view is detached from a window.
        /// </summary>
        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();

            // Remove the accessibility state change listener while the view detached from window. to avoid the memory leak.
            AccessibilityManager? accessibility = (AccessibilityManager?)_context?.GetSystemService(Context.AccessibilityService!);
            accessibility?.RemoveAccessibilityStateChangeListener(this);
        }

        /// <summary>
        /// This Method is called upon request layout from it virtual view or one of it descendent view.
        /// </summary>
        public override void RequestLayout()
        {
            base.RequestLayout();
        }
    }
}
