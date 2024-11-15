using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WSize = global::Windows.Foundation.Size;
using WRect = global::Windows.Foundation.Rect;
using Microsoft.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Syncfusion.Maui.Toolkit.Semantics;

namespace Syncfusion.Maui.Toolkit.Platform
{
    internal class LayoutPanelExt : LayoutPanel
    {
        internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }
        internal Func<Rect, Size>? CrossPlatformArrange { get; set; }
        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;
        private NativeGraphicsView? nativeGraphicsView;
        WeakReference<SfView>? _mauiView;
        WeakReference<IDrawable>? _drawable;

        public LayoutPanelExt(SfView layout)
        {
            Drawable = layout;
            MauiView = layout;
            //ListView Focus rect not shown when navigated using keyboard arrow keys
            //this.IsTabStop = true; 
            this.AllowFocusOnInteraction = true;
            this.UseSystemFocusVisuals = true;
            SizeChanged += ContentPanelExt_SizeChanged;
        }

        private void ContentPanelExt_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            nativeGraphicsView?.Invalidate();
        }

        public DrawingOrder DrawingOrder
        {
            get
            {
                return this.drawingOrder;
            }
            set
            {
                this.drawingOrder = value;
                if (this.DrawingOrder == DrawingOrder.NoDraw)
                {
                    this.RemoveDrawableView();
                }
                else
                {
                    this.InitializeNativeGraphicsView();
                    this.ArrangeNativeGraphicsView();
                }
            }
        }

		public IDrawable? Drawable
		{
			get => _drawable != null && _drawable.TryGetTarget(out var v) ? v : null;
			set { _drawable = value == null ? null : new(value); }
		}

		SfView? MauiView
		{
			get => _mauiView != null && _mauiView.TryGetTarget(out var v) ? v : null;
			set => _mauiView = value == null ? null : new(value);
		}

        internal void InitializeNativeGraphicsView()
        {
            if (!Children.Contains(nativeGraphicsView) && MauiView != null)
            {
                nativeGraphicsView = new NativeGraphicsView(MauiView)
                {
                    Drawable = this.Drawable
                };
            }

            if (nativeGraphicsView != null)
            {
                if (this.DrawingOrder == DrawingOrder.AboveContentWithTouch || this.DrawingOrder == DrawingOrder.BelowContent)
                {
                    nativeGraphicsView.IsHitTestVisible = true;
                }
                else
                {
                    nativeGraphicsView.IsHitTestVisible = false;
                }
            }
        }

        internal void RemoveDrawableView()
        {
            if (nativeGraphicsView != null && Children.Contains(nativeGraphicsView))
            {
                Children.Remove(nativeGraphicsView);
            }
        }

        internal void ArrangeNativeGraphicsView()
        {
            if (nativeGraphicsView != null)
            {
                if (Children.Contains(nativeGraphicsView))
                {
                    Children.Remove(nativeGraphicsView);
                }

                if (this.DrawingOrder == DrawingOrder.AboveContentWithTouch || this.DrawingOrder == DrawingOrder.AboveContent)
                {
                    Children.Add(nativeGraphicsView);
                }
                else
                {
                    Children.Insert(0, nativeGraphicsView);
                }
            }
        }

        internal void Invalidate()
        {
            this.nativeGraphicsView?.Invalidate();
        }

        /// <summary>
        /// Invalidates the semantics nodes.
        /// </summary>
        internal void InvalidateSemantics()
        {
            if (this.nativeGraphicsView == null || this.nativeGraphicsView.SemanticsAutomationPeer == null)
            {
                return;
            }

            this.nativeGraphicsView.SemanticsAutomationPeer.InvalidateSemantics();
        }

        protected override WSize ArrangeOverride(WSize finalSize)
        {
            if (CrossPlatformArrange == null)
            {
                return base.ArrangeOverride(finalSize);
            }

            var width = finalSize.Width;
            var height = finalSize.Height;

            CrossPlatformArrange(new Rect(0, 0, width, height));

            if (ClipsToBounds)
            {
                if (Clip != null && (Clip.Bounds.Width != finalSize.Width || Clip.Bounds.Height != finalSize.Height))
                {
                    Clip = new RectangleGeometry { Rect = new WRect(0, 0, finalSize.Width, finalSize.Height) };
                }
            }

            nativeGraphicsView?.Arrange(new WRect(0, 0, width, height));

            return finalSize;

        }

        protected override WSize MeasureOverride(WSize availableSize)
        {
            if (CrossPlatformMeasure == null)
            {
                return base.MeasureOverride(availableSize);
            }

            var width = availableSize.Width;
            var height = availableSize.Height;

            var crossPlatformSize = CrossPlatformMeasure(width, height);

            width = crossPlatformSize.Width;
            height = crossPlatformSize.Height;

            nativeGraphicsView?.Measure(availableSize);

            return new WSize(width, height);
        }

        internal void Dispose()
        {
            if (this.nativeGraphicsView != null)
            {
                this.nativeGraphicsView = null;
            }
        }
    }

    /// <summary>
    /// TODO: Create the NativeGraphicsView by W2DGraphicsView source code for overriding the OnCreateAutomationPeer method.
    /// W2DGraphicsView class is sealed so that we used this class and we requested to make the remove the sealed class.
    /// https://github.com/dotnet/maui/issues/9460.
    /// </summary>
    internal class NativeGraphicsView : UserControl
    {
        private CanvasControl? _canvasControl;

        private readonly W2DCanvas _canvas;

        private RectF _dirty;

        WeakReference<IDrawable>? _drawable;
        WeakReference<SfView>? _mauiView;

        internal CustomAutomationPeer? SemanticsAutomationPeer;

		SfView? MauiView
		{
			get => _mauiView != null && _mauiView.TryGetTarget(out var v) ? v : null;
			set => _mauiView = value == null ? null : new(value);
		}

		public IDrawable? Drawable
		{
			get => _drawable != null && _drawable.TryGetTarget(out var v) ? v : null;
			set
			{
				_drawable = value == null ? null : new(value);
				Invalidate();
			}
		}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal NativeGraphicsView(SfView mauiView)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _canvas = new W2DCanvas();
            MauiView = mauiView;
            base.Loaded += UserControl_Loaded;
            base.Unloaded += UserControl_Unloaded;
        }

        internal void Invalidate()
        {
            _canvasControl?.Invalidate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _canvasControl = new CanvasControl();
            _canvasControl.Draw += OnDraw;
            base.Content = _canvasControl;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_canvasControl != null && !_canvasControl.IsLoaded)
            {
                _canvasControl.RemoveFromVisualTree();
                _canvasControl = null;
            }

            this.SemanticsAutomationPeer = null;
        }

        private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (Drawable != null)
            {
                _dirty.X = 0f;
                _dirty.Y = 0f;
                _dirty.Width = (float)sender.ActualWidth;
                _dirty.Height = (float)sender.ActualHeight;
                _canvas.Session = args.DrawingSession;
                _canvas.CanvasSize = new(_dirty.Width, _dirty.Height);
                Drawable.Draw(_canvas, _dirty);
            }
        }

        protected override AutomationPeer? OnCreateAutomationPeer()
        {
            if (this.MauiView == null)
            {
                return null;
            }

            //// Drawing order above content does not show the accessibility element
            //// because that disable the user interaction.
            //// Drawing order have support interaction by about content with interaction type.
            this.SemanticsAutomationPeer = new CustomAutomationPeer(this, MauiView);
            return this.SemanticsAutomationPeer;
        }
    }
}