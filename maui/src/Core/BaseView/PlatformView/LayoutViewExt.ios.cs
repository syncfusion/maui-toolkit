using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using CoreGraphics;
using UIKit;
using Foundation;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Semantics;
using ILayout = Microsoft.Maui.ILayout;

namespace Syncfusion.Maui.Toolkit.Platform
{
	internal class LayoutViewExt : LayoutView, IUIAccessibilityContainer
	{
		private IGraphicsRenderer? _renderer;
		private CGColorSpace? _colorSpace;
		private CGRect _lastBounds;
		private DrawingOrder _drawingOrder = DrawingOrder.NoDraw;
		private NativePlatformGraphicsView? _nativeGraphicsView;

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
		private CGRect? _availableBounds;

		/// <summary>
		/// Holds the accessibility notification objects.
		/// </summary>
		private List<NSObject>? _notifications;

		public LayoutViewExt(IDrawable? drawable = null, IGraphicsRenderer? renderer = null)
		{
			Drawable = drawable;
			MauiView = (View)drawable!;
			Renderer = renderer;
			BackgroundColor = UIColor.Clear;
			Opaque = false;

			_notifications =
			[
				NSNotificationCenter.DefaultCenter.AddObserver(UIView.VoiceOverStatusDidChangeNotification, OnObserveNotification),
				NSNotificationCenter.DefaultCenter.AddObserver(UIView.SwitchControlStatusDidChangeNotification, OnObserveNotification),
				NSNotificationCenter.DefaultCenter.AddObserver(UIView.InvertColorsStatusDidChangeNotification, OnObserveNotification),
				NSNotificationCenter.DefaultCenter.AddObserver(UIView.ReduceMotionStatusDidChangeNotification, OnObserveNotification),
				NSNotificationCenter.DefaultCenter.AddObserver(UIView.BoldTextStatusDidChangeNotification, OnObserveNotification),
				NSNotificationCenter.DefaultCenter.AddObserver(UIView.DarkerSystemColorsStatusDidChangeNotification, OnObserveNotification),
               // NSNotificationCenter.DefaultCenter.AddObserver(UIView.OnOffSwitchLabelsDidChangeNotification, this.OnObserveNotification),
                NSNotificationCenter.DefaultCenter.AddObserver(UIView.GuidedAccessStatusDidChangeNotification, OnObserveNotification),
				NSNotificationCenter.DefaultCenter.AddObserver(UIView.SpeakScreenStatusDidChangeNotification, OnObserveNotification)
			];
		}

#pragma warning disable IDE0060 // Remove unused parameter
		public LayoutViewExt(IntPtr aPtr)
#pragma warning restore IDE0060 // Remove unused parameter
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

		internal IDrawable? Drawable
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
				return _drawingOrder;
			}
			set
			{
				_drawingOrder = value;
				InitializeNativeGraphicsView();
				SetNeedsDisplay();
			}
		}

		internal IGraphicsRenderer? Renderer
		{
			get => _renderer;

			set
			{
				UpdateRenderer(value);
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

			if (DrawingOrder == DrawingOrder.BelowContent)
			{
				_renderer = graphicsRenderer ?? new DirectRenderer();

				_renderer.GraphicsView = [];
				_renderer.Drawable = new PlatformDrawableView(Drawable);
				_renderer.SizeChanged((float)Bounds.Width, (float)Bounds.Height);
			}
		}

		internal void InvalidateDrawable()
		{
			_renderer?.Invalidate();
			_nativeGraphicsView?.InvalidateDrawable();
			SetNeedsDisplay();
		}

		public void InvalidateDrawable(float x, float y, float w, float h)
		{
			_renderer?.Invalidate(x, y, w, h);
		}

		internal void InitializeNativeGraphicsView()
		{
			UpdateRenderer();
			if (DrawingOrder == DrawingOrder.AboveContent || DrawingOrder == DrawingOrder.AboveContentWithTouch)
			{
				if (_nativeGraphicsView == null)
				{
					if (MauiView != null)
					{
						_nativeGraphicsView = new NativePlatformGraphicsView((SfView?)MauiView)
						{
							BackgroundColor = UIColor.Clear,
						};
						if (DrawingOrder == DrawingOrder.AboveContent)
						{
							_nativeGraphicsView.UserInteractionEnabled = false;
						}
					}
				}

				if(_nativeGraphicsView is not null)
				{
					Add(_nativeGraphicsView);
				}
			}
			else if (_nativeGraphicsView != null)
			{
				_nativeGraphicsView.RemoveFromSuperview();
				SetNeedsDisplay();
			}

			InvalidateSemantics();
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
			if (View is SfView sfview && !sfview.IsLayoutControl)
			{
				layout.CrossPlatformMeasure(widthConstraint, heightConstraint);
			}
			else
			{
				// If the SuperView is a MauiView (backing a cross-platform ContentView or Layout), then measurement
				// has already happened via SizeThatFits and doesn't need to be repeated in LayoutSubviews. But we
				// _do_ need LayoutSubviews to make a measurement pass if the parent is something else (for example,
				// the window); there's no guarantee that SizeThatFits has been called in that case.
				if (!IsMeasureValid(widthConstraint, heightConstraint) && Superview is not Microsoft.Maui.Platform.MauiView)
				{
#if NET9_0
					layout.CrossPlatformMeasure(widthConstraint, heightConstraint);
					CacheMeasureConstraints(widthConstraint, heightConstraint);
#else
					CacheMeasureConstraints(widthConstraint, heightConstraint, layout.CrossPlatformMeasure(widthConstraint, heightConstraint));
#endif
				}
			}

			layout.CrossPlatformArrange(bounds);
			UpdateGraphicsViewBounds();
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
			if (DrawingOrder == DrawingOrder.BelowContent)
			{
				base.Draw(dirtyRect);
				var coreGraphics = UIGraphics.GetCurrentContext();

				if (Drawable == null)
				{
					return;
				}

				if (_colorSpace == null)
				{
					_colorSpace = CGColorSpace.CreateDeviceRGB();
				}

				coreGraphics.SetFillColorSpace(_colorSpace);
				coreGraphics.SetStrokeColorSpace(_colorSpace);
				coreGraphics.SetPatternPhase(PatternPhase);
				_renderer?.Draw(coreGraphics, dirtyRect.AsRectangleF());
			}

			if (this.GetAccessibilityElements() == null || _availableBounds != dirtyRect)
			{
				CreateSemantics();
				_availableBounds = dirtyRect;
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
				Pressed?.Invoke(this, new UIViewTouchEventArgs() { Point = new Microsoft.Maui.Graphics.Point((float)(point.X), (float)point.Y) });
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
				Moved?.Invoke(this, new UIViewTouchEventArgs() { Point = new Microsoft.Maui.Graphics.Point((float)(point.X), (float)point.Y) });
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
				Released?.Invoke(this, new UIViewTouchEventArgs() { Point = new Microsoft.Maui.Graphics.Point((float)(point.X), (float)point.Y) });
			}
		}

		/// <summary>
		/// Raised when a button is pressed.
		/// </summary>
		/// <param name="presses">A set of <see cref="UIPress"/> instances that represent the new presses that occurred.</param>
		/// <param name="evt">The event to which the presses belong.</param>
		public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
		{
			if (MauiView != null && !MauiView.HandleKeyPress(presses, evt))
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
			if (MauiView != null && !MauiView.HandleKeyRelease(presses, evt))
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
					UpdateGraphicsViewBounds();
					InvalidateSemantics();
					_lastBounds = newBounds;
					// Draw method not getting called on resizing the window
					SetNeedsDisplay();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				 ClearViews();
			}
		}

		/// <summary>
        /// To clear the view and notification
        /// </summary>
        internal void ClearViews()
        {
            ClearNativeGraphisView();
            RemoveNotification();
            this.SetAccessibilityElements(null);
        }

		/// <summary>
        /// To clear the native graphis view
        /// </summary>
        private void ClearNativeGraphisView()
        {       
            if (_nativeGraphicsView != null)
            {
                _nativeGraphicsView.SetAccessibilityElements(null);
                _nativeGraphicsView.Dispose();
                _nativeGraphicsView = null;
            }
        }

		/// <summary>
        /// To remove the notifications
        /// </summary>
        private void RemoveNotification()
        {
            if (_notifications != null)
            {
                for (int i = 0; i < _notifications.Count; i++)
                {
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_notifications[i]);
                }

                _notifications = null;
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
			if (_nativeGraphicsView != null)
			{
				if (_nativeGraphicsView.Bounds != Bounds)
				{
					_nativeGraphicsView.Frame = Bounds;
				}

				_nativeGraphicsView.InvalidateDrawable();
			}
		}

		private void OnObserveNotification(NSNotification notification)
		{
			InvalidateSemantics();
		}

		/// <summary>
		/// Invalidates the semantics nodes.
		/// </summary>
		internal void InvalidateSemantics()
		{
			CreateSemantics();
			_nativeGraphicsView?.InvalidateSemantics();
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

			List<object> accessibilityElements = [];
			if (DrawingOrder == DrawingOrder.BelowContent)
			{
				List<SemanticsNode>? semanticsNodes = null;

				if (MauiView is ISemanticsProvider provider)
				{
					semanticsNodes = provider.GetSemanticsNodes(Bounds.Width, Bounds.Height);
				}

				if (semanticsNodes != null)
				{
					CGRect accessibilityFrame = AccessibilityFrame;
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

			for (int i = 0; i < Subviews.Length; i++)
			{
				UIView view = Subviews[i];
				accessibilityElements.Add(view);
			}

			this.SetAccessibilityElements(NSArray.FromObjects([.. accessibilityElements]));
		}
	}

	internal class NativePlatformGraphicsView : PlatformGraphicsView, IUIAccessibilityContainer
	{
		WeakReference<SfView>? _mauiView;

		WeakReference<IDrawable>? _drawable;

		private CGRect? _availableBounds;

		internal NativePlatformGraphicsView(SfView? mauiView)
		{
			IsAccessibilityElement = false;
			MauiView = mauiView;
			DrawableView = new PlatformDrawableView((IDrawable?)MauiView);
            Drawable = DrawableView;
		}

		SfView? MauiView
		{
			get => _mauiView != null && _mauiView.TryGetTarget(out var v) ? v : null;
			set => _mauiView = value == null ? null : new(value);
		}

		IDrawable? DrawableView
        {
            get => _drawable != null && _drawable.TryGetTarget(out var v) ? v : null;
            set => _drawable = value == null ? null : new(value);
        }

		public override void Draw(CGRect dirtyRect)
		{
			base.Draw(dirtyRect);

			if (this.GetAccessibilityElements() == null || _availableBounds != dirtyRect)
			{
				CreateSemantics();
				_availableBounds = dirtyRect;
			}
		}

		/// <summary>
		/// Clear and create the semantics nodes.
		/// </summary>
		internal void InvalidateSemantics()
		{
			CreateSemantics();
		}

		private void CreateSemantics()
		{
			if (!UIAccessibility.IsVoiceOverRunning && !UIAccessibility.IsSwitchControlRunning && !UIAccessibility.IsSpeakScreenEnabled)
			{
				this.SetAccessibilityElements(null);
				return;
			}

			List<UIAccessibilityElement> accessibilityElements = [];
			List<SemanticsNode>? semanticsNodes = null;
			if (MauiView is ISemanticsProvider provider)
			{
				semanticsNodes = provider.GetSemanticsNodes(Bounds.Width, Bounds.Height);
			}

			if (semanticsNodes != null)
			{
				CGRect accessibilityFrame = AccessibilityFrame;
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

			this.SetAccessibilityElements(NSArray.FromObjects([.. accessibilityElements]));
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
			Bounds = CGRect.Empty;
			ParentBounds = CGRect.Empty;
			Parent = (UIView)container;
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
				if (Parent != null && Parent.Handle != IntPtr.Zero && Parent.AccessibilityFrame != ParentBounds)
				{
					ParentBounds = Parent.AccessibilityFrame;
					AccessibilityFrame = UIAccessibility.ConvertFrameToScreenCoordinates(Bounds, Parent);
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