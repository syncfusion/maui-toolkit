using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using CoreGraphics;
using UIKit;
using System;
using Microsoft.Maui.Controls;
using Foundation;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections.Generic;
using Syncfusion.Maui.Toolkit.Semantics;
using Microsoft.Maui;
using ILayout = Microsoft.Maui.ILayout;

namespace Syncfusion.Maui.Toolkit.Platform
{
    internal class LayoutViewExt : LayoutView, IUIAccessibilityContainer
    {
        private IGraphicsRenderer? _renderer;
        private CGColorSpace? _colorSpace;
        private CGRect _lastBounds;
        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;
        private NativePlatformGraphicsView? nativeGraphicsView;

        WeakReference<IDrawable>? _drawable;
        WeakReference<View>? _mauiView;

        /// <summary>
        /// This event occurs while new touches occurred in a view.
        /// </summary>
        internal event EventHandler<UIViewTouchEventArgs>? Pressed;

        /// <summary>
        /// This event occurs when one or more touches associated with an event changed.
        /// </summary>
        internal event EventHandler<UIViewTouchEventArgs>? Moved;

        /// <summary>
        /// This event occurs when one or more fingers are raised from a view
        /// </summary>
        internal event EventHandler<UIViewTouchEventArgs>? Released;

        /// <summary>
        /// Used to hold the previous bounds value. The value used to update the semantics while the size changed.
        /// </summary>
        private CGRect? availableBounds;

        /// <summary>
        /// Holds the accessibility notification objects.
        /// </summary>
        private List<NSObject>? notifications;

        public LayoutViewExt(IDrawable? drawable = null, IGraphicsRenderer? renderer = null)
        {
            Drawable = drawable;
            MauiView = (View)drawable!;
            Renderer = renderer;
            BackgroundColor = UIColor.Clear;
            this.Opaque = false;

            this.notifications = new List<NSObject>
            {
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.VoiceOverStatusDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.SwitchControlStatusDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.InvertColorsStatusDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.ReduceMotionStatusDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.BoldTextStatusDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.DarkerSystemColorsStatusDidChangeNotification, this.OnObserveNotification),
               // NSNotificationCenter.DefaultCenter.AddObserver(UIView.OnOffSwitchLabelsDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.GuidedAccessStatusDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.SpeakScreenStatusDidChangeNotification, this.OnObserveNotification)
            };
        }

        public LayoutViewExt(IntPtr aPtr)
        {
            BackgroundColor = UIColor.Clear;
        }

        /// <summary>
        /// Returns a boolean value indicating whether this object can become the first responder.
        /// </summary>
        public override bool CanBecomeFirstResponder => MauiView != null && MauiView is IKeyboardListener keyboardListener && keyboardListener.CanBecomeFirstResponder;

        View? MauiView
        {
            get => _mauiView != null && _mauiView.TryGetTarget(out var v) ? v : null;
            set => _mauiView = value == null ? null : new(value);
        }

        IDrawable? Drawable
        {
            get => _drawable != null && _drawable.TryGetTarget(out var v) ? v : null;
            set
            {
                _drawable = value == null ? null : new(value);
                if (_renderer != null)
                {
                    _renderer.Drawable = new PlatformDrawableView(Drawable);
                    _renderer.Invalidate();
                }
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
                this.InitializeNativeGraphicsView();
                this.SetNeedsDisplay();
            }
        }

        internal IGraphicsRenderer? Renderer
        {
            get => _renderer;

            set
            {
                this.UpdateRenderer(value);
            }
        }

        private void UpdateRenderer(IGraphicsRenderer? graphicsRenderer = null)
        {
            if (_renderer != null)
            {
                _renderer.Drawable = null;
                _renderer.GraphicsView = null;
                _renderer.Dispose();
                _renderer = null;
            }

            if (this.DrawingOrder == DrawingOrder.BelowContent)
            {
                _renderer = graphicsRenderer ?? new DirectRenderer();

                _renderer.GraphicsView = new PlatformGraphicsView();
                _renderer.Drawable = new PlatformDrawableView(Drawable);
                _renderer.SizeChanged((float)Bounds.Width, (float)Bounds.Height);
            }
        }

        internal void InvalidateDrawable()
        {
            _renderer?.Invalidate();
            this.nativeGraphicsView?.InvalidateDrawable();
            this.SetNeedsDisplay();
        }

        public void InvalidateDrawable(float x, float y, float w, float h)
        {
            _renderer?.Invalidate(x, y, w, h);
        }

        internal void InitializeNativeGraphicsView()
        {
            this.UpdateRenderer();
            if (this.DrawingOrder == DrawingOrder.AboveContent || this.DrawingOrder == DrawingOrder.AboveContentWithTouch)
            {
                if (nativeGraphicsView == null)
                {
                    if (MauiView != null)
                    {
                        nativeGraphicsView = new NativePlatformGraphicsView((SfView?)MauiView)
                        {
                            BackgroundColor = UIColor.Clear,
                        };
                        if (this.DrawingOrder == DrawingOrder.AboveContent)
                        {
                            this.nativeGraphicsView.UserInteractionEnabled = false;
                        }
                    }
                }

                this.Add(nativeGraphicsView);
            }
            else if (nativeGraphicsView != null)
            {
                this.nativeGraphicsView.RemoveFromSuperview();
                SetNeedsDisplay();
            }

            this.InvalidateSemantics();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (View is not ILayout layout)
            {
                return;
            }

			var bounds = AdjustForSafeArea(Bounds).ToRectangle();
			var widthConstraint = bounds.Width;
			var heightConstraint = bounds.Height;
			
			if (!IsMeasureValid(widthConstraint, heightConstraint) && Superview is not Microsoft.Maui.Platform.MauiView)
			{
				layout.CrossPlatformMeasure(bounds.Width, bounds.Height);
				CacheMeasureConstraints(widthConstraint, heightConstraint);
			}

            layout.CrossPlatformArrange(bounds);
            this.UpdateGraphicsViewBounds();
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            if (View is not ILayout layout)
            {
                return base.SizeThatFits(size);
            }

            var width = size.Width;
            var height = size.Height;

            var crossPlatformSize = layout.CrossPlatformMeasure(width, height);

            return crossPlatformSize.ToCGSize();
        }

        /// <summary>
        /// This Method is called upon SetNeedsLayout from its virtual view or one of it descendent view.
        /// </summary>
        public override void SetNeedsLayout()
        {
            base.SetNeedsLayout();
        }

        public override void Draw(CGRect dirtyRect)
        {
            if (this.DrawingOrder == DrawingOrder.BelowContent)
            {
                base.Draw(dirtyRect);
                var coreGraphics = UIGraphics.GetCurrentContext();

                if (Drawable == null) return;

                if (_colorSpace == null)
                {
                    _colorSpace = CGColorSpace.CreateDeviceRGB();
                }

                coreGraphics.SetFillColorSpace(_colorSpace);
                coreGraphics.SetStrokeColorSpace(_colorSpace);
                coreGraphics.SetPatternPhase(PatternPhase);
                _renderer?.Draw(coreGraphics, dirtyRect.AsRectangleF());
            }

            if (this.GetAccessibilityElements() == null || this.availableBounds != dirtyRect)
            {
                this.CreateSemantics();
                this.availableBounds = dirtyRect;
            }
        }

        /// <summary>
        /// Raised when touches began.
        /// </summary>
        public override void TouchesBegan(NSSet touches, UIEvent? evt)
        {
            base.TouchesBegan(touches, evt);
            if (touches.AnyObject is UITouch touch)
            {
                CGPoint point = touch.LocationInView(this);
                this.Pressed?.Invoke(this, new UIViewTouchEventArgs() { Point = new Microsoft.Maui.Graphics.Point((float)(point.X), (float)point.Y) });
            }
        }

        /// <summary>
        /// Raised when touches moved.
        /// </summary>
        public override void TouchesMoved(NSSet touches, UIEvent? evt)
        {
            base.TouchesMoved(touches, evt);
            if (touches.AnyObject is UITouch touch)
            {
                CGPoint point = touch.LocationInView(this);
                this.Moved?.Invoke(this, new UIViewTouchEventArgs() { Point = new Microsoft.Maui.Graphics.Point((float)(point.X), (float)point.Y) });
            }
        }

        /// <summary>
        /// Raised when touches ended.
        /// </summary>
        public override void TouchesEnded(NSSet touches, UIEvent? evt)
        {
            base.TouchesEnded(touches, evt);
            if (touches.AnyObject is UITouch touch)
            {
                CGPoint point = touch.LocationInView(this);
                this.Released?.Invoke(this, new UIViewTouchEventArgs() { Point = new Microsoft.Maui.Graphics.Point((float)(point.X), (float)point.Y) });
            }
        }

        /// <summary>
        /// Raised when a button is pressed.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the new presses that occurred.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.MauiView != null && !this.MauiView.HandleKeyPress(presses, evt))
            {
                base.PressesBegan(presses, evt);
            }
        }

        /// <summary>
        /// Raised when a button is released.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the buttons that the user is no longer pressing.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.MauiView != null && !this.MauiView.HandleKeyRelease(presses, evt))
            {
                base.PressesEnded(presses, evt);
            }
        }

        public override CGRect Bounds
        {
            get => base.Bounds;

            set
            {
                var newBounds = value;
                if (_lastBounds.Width != newBounds.Width || _lastBounds.Height != newBounds.Height)
                {
                    base.Bounds = value;
                    _renderer?.SizeChanged((float)newBounds.Width, (float)newBounds.Height);
                    _renderer?.Invalidate();
                    this.UpdateGraphicsViewBounds();
                    this.InvalidateSemantics();
                    _lastBounds = newBounds;
                    // Draw method not getting called on resizing the window
                    this.SetNeedsDisplay();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (nativeGraphicsView != null)
                {
                    nativeGraphicsView.SetAccessibilityElements(null);
                    nativeGraphicsView.Dispose();
                    nativeGraphicsView = null;
                }

                //// Remove the accessibility notifications.
                if (this.notifications != null)
                {
                    for (int i = 0; i < this.notifications.Count; i++)
                    {
                        NSNotificationCenter.DefaultCenter.RemoveObserver(this.notifications[i]);
                    }

                    this.notifications = null;
                }

                this.SetAccessibilityElements(null);
            }
        }

        protected virtual CGSize PatternPhase
        {
            get
            {
                var px = Frame.X;
                var py = Frame.Y;
                return new CGSize(px, py);
            }
        }

        private void UpdateGraphicsViewBounds()
        {
            if (this.nativeGraphicsView != null)
            {
                if (this.nativeGraphicsView.Bounds != this.Bounds)
                    this.nativeGraphicsView.Frame = this.Bounds;
                this.nativeGraphicsView.InvalidateDrawable();
            }
        }

        private void OnObserveNotification(NSNotification notification)
        {
            this.InvalidateSemantics();
        }

        /// <summary>
        /// Invalidates the semantics nodes.
        /// </summary>
        internal void InvalidateSemantics()
        {
            this.CreateSemantics();
            this.nativeGraphicsView?.InvalidateSemantics();
        }

        /// <summary>
        /// Create the semantics mode for the view.
        /// </summary>
        private void CreateSemantics()
        {
            if (!UIAccessibility.IsVoiceOverRunning && !UIAccessibility.IsSwitchControlRunning && !UIAccessibility.IsSpeakScreenEnabled)
            {
                this.SetAccessibilityElements(null);
                return;
            }

            List<Object> accessibilityElements = new List<Object>();
            if (this.DrawingOrder == DrawingOrder.BelowContent)
            {
                List<SemanticsNode>? semanticsNodes = null;

                if (this.MauiView is ISemanticsProvider)
                {
                    semanticsNodes = ((ISemanticsProvider)this.MauiView).GetSemanticsNodes(this.Bounds.Width, this.Bounds.Height);
                }

                if (semanticsNodes != null)
                {
                    CGRect accessibilityFrame = this.AccessibilityFrame;
                    for (int i = 0; i < semanticsNodes.Count; i++)
                    {
                        SemanticsNode semanticsNode = semanticsNodes[i];
                        CustomAccessibilityElement element = new CustomAccessibilityElement(this)
                        {
                            AccessibilityHint = semanticsNode.Text,
                            AccessibilityLabel = semanticsNode.Text,
                            AccessibilityIdentifier = semanticsNode.Id.ToString(),
                            IsAccessibilityElement = true,
                            AccessibilityTraits = semanticsNode.IsTouchEnabled ? (ulong)UIAccessibilityTrait.Button : (ulong)UIAccessibilityTrait.None,
                            AccessibilityFrame = UIAccessibility.ConvertFrameToScreenCoordinates(new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height), this),
                            Bounds = new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height),
                            Parent = this,
                            ParentBounds = accessibilityFrame
                        };

                        accessibilityElements.Add(element);
                    }
                }
            }

            for (int i = 0; i < this.Subviews.Length; i++)
            {
                UIView view = this.Subviews[i];
                accessibilityElements.Add(view);
            }

            this.SetAccessibilityElements(NSArray.FromObjects(accessibilityElements.ToArray()));
        }
    }

    internal class NativePlatformGraphicsView : PlatformGraphicsView, IUIAccessibilityContainer
    {
        WeakReference<SfView>? _mauiView;

        private CGRect? availableBounds;

        internal NativePlatformGraphicsView(SfView? mauiView)
        {
            IsAccessibilityElement = false;
            MauiView = mauiView;
            Drawable = new PlatformDrawableView(MauiView);
        }

        SfView? MauiView
        {
            get => _mauiView != null && _mauiView.TryGetTarget(out var v) ? v : null;
            set => _mauiView = value == null ? null : new(value);
        }

        public override void Draw(CGRect dirtyRect)
        {
            base.Draw(dirtyRect);

            if (this.GetAccessibilityElements() == null || availableBounds != dirtyRect)
            {
                this.CreateSemantics();
                availableBounds = dirtyRect;
            }
        }

        /// <summary>
        /// Clear and create the semantics nodes.
        /// </summary>
        internal void InvalidateSemantics()
        {
            this.CreateSemantics();
        }

        private void CreateSemantics()
        {
            if (!UIAccessibility.IsVoiceOverRunning && !UIAccessibility.IsSwitchControlRunning && !UIAccessibility.IsSpeakScreenEnabled)
            {
                this.SetAccessibilityElements(null);
                return;
            }

            List<UIAccessibilityElement> accessibilityElements = new List<UIAccessibilityElement>();
            List<SemanticsNode>? semanticsNodes = null;
            if (this.MauiView is ISemanticsProvider)
            {
                semanticsNodes = ((ISemanticsProvider)this.MauiView).GetSemanticsNodes(this.Bounds.Width, this.Bounds.Height);
            }

            if (semanticsNodes != null)
            {
                CGRect accessibilityFrame = this.AccessibilityFrame;
                for (int i = 0; i < semanticsNodes.Count; i++)
                {
                    SemanticsNode semanticsNode = semanticsNodes[i];
                    CustomAccessibilityElement element = new CustomAccessibilityElement(this)
                    {
                        AccessibilityHint = semanticsNode.Text,
                        AccessibilityLabel = semanticsNode.Text,
                        AccessibilityIdentifier = semanticsNode.Id.ToString(),
                        IsAccessibilityElement = true,
                        AccessibilityTraits = semanticsNode.IsTouchEnabled ? (ulong)UIAccessibilityTrait.Button : (ulong)UIAccessibilityTrait.None,
                        AccessibilityFrame = UIAccessibility.ConvertFrameToScreenCoordinates(new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height), this),
                        Bounds = new CGRect(semanticsNode.Bounds.Left, semanticsNode.Bounds.Top, semanticsNode.Bounds.Width, semanticsNode.Bounds.Height),
                        Parent = this,
                        ParentBounds = accessibilityFrame,
                    };

                    accessibilityElements.Add(element);
                }
            }

            this.SetAccessibilityElements(NSArray.FromObjects(accessibilityElements.ToArray()));
        }
    }

    /// <summary>
    /// Holds the accessibility element details.
    /// </summary>
    class CustomAccessibilityElement : UIAccessibilityElement
    {
        /// <summary>
        /// Hold the bounds of the accessibility element.
        /// </summary>
        public CGRect Bounds { get; set; }

        /// <summary>
        /// Holds the owner or parent of the accessibility element.
        /// </summary>
        public UIView? Parent { get; set; }

        /// <summary>
        /// Holds the owner or parent accessiblity frame value.
        /// </summary>
        public CGRect ParentBounds { get; set; }

        public CustomAccessibilityElement(NSObject container) : base(container)
        {
            this.Bounds = CGRect.Empty;
            this.ParentBounds = CGRect.Empty;
            this.Parent = (UIView)container;
        }

        protected CustomAccessibilityElement(NSObjectFlag t) : base(t)
        {
        }

        protected internal CustomAccessibilityElement(ObjCRuntime.NativeHandle handle) : base(handle)
        {
        }

        public override CGRect AccessibilityFrame
        {
            get
            {
                //// Update the accessibility frame position while the parent position changed.
                if (this.Parent != null && this.Parent.Handle != IntPtr.Zero && this.Parent.AccessibilityFrame != this.ParentBounds)
                {
                    ParentBounds = this.Parent.AccessibilityFrame;
                    this.AccessibilityFrame = UIAccessibility.ConvertFrameToScreenCoordinates(this.Bounds, this.Parent);
                }

                return base.AccessibilityFrame;
            }
            set => base.AccessibilityFrame = value;
        }
    }

    /// <summary>
    /// EventArgs for native touch events.
    /// </summary>
    internal class UIViewTouchEventArgs : EventArgs
    {
        /// <summary>
        /// Holds the coordinates of touch points
        /// </summary>
        internal Microsoft.Maui.Graphics.Point Point { get; set; }
    }

    internal class PlatformDrawableView : IDrawable
    {
        WeakReference<IDrawable>? _drawable;

        internal PlatformDrawableView(IDrawable? drawable)
        {
            Drawable = drawable;
        }

        IDrawable? Drawable
        {
            get => _drawable != null && _drawable.TryGetTarget(out var v) ? v : null;
            set => _drawable = value == null ? null : new(value);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Drawable?.Draw(canvas, dirtyRect);
        }
    }
}