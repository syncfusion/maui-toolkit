using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Platform;
using PlatformView = UIKit.UIView;

namespace Syncfusion.Maui.Toolkit
{
	/// <exclude/>
	public partial class SfViewHandler : LayoutHandler
    {
        private LayoutViewExt? layoutViewExt;

		/// <exclude/>
		protected override LayoutView CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
            }

            this.layoutViewExt = new LayoutViewExt((IDrawable)VirtualView);

            return this.layoutViewExt;
        }

		/// <exclude/>
		public void Invalidate()
        {
            this.layoutViewExt?.InvalidateDrawable();
        }

		/// <exclude/>
		public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
        {
            if (this.layoutViewExt != null)
            {
                this.layoutViewExt.DrawingOrder = drawingOrder;
            }
        }

		/// <exclude/>
		public void UpdateClipToBounds(bool clipToBounds)
        {
            if (this.layoutViewExt != null)
            {
                this.layoutViewExt.ClipsToBounds = clipToBounds;
            }
        }

		/// <exclude/>
		public new void Add(IView child)
        {
            _ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

            var index = this.PlatformView.Subviews.Length;
            if (this.layoutViewExt != null)
            {
                if (this.layoutViewExt.DrawingOrder == DrawingOrder.AboveContent)
                {
                    if (index > 0)
                        PlatformView.InsertSubview(child.ToPlatform(MauiContext), index - 1);
                    else
                        PlatformView.InsertSubview(child.ToPlatform(MauiContext), index);
                }
                else
                {
                    PlatformView.InsertSubview(child.ToPlatform(MauiContext), index);
                }
            }
        }

        /// <summary>
        /// Invalidates the semantics nodes.
        /// </summary>
        internal void InvalidateSemantics()
        {
            this.layoutViewExt?.InvalidateSemantics();
        }

		/// <exclude/>
		protected override void DisconnectHandler(LayoutView platformView)
        {
            base.DisconnectHandler(platformView);
            foreach (var child in VirtualView)
            {
                child.Handler?.DisconnectHandler();
            }

            if (this.layoutViewExt != null)
            {
                this.layoutViewExt = null;
            }
        }
    }
}
