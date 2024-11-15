using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Platform;

namespace Syncfusion.Maui.Toolkit
{
	/// <exclude/>
	public partial class SfViewHandler : LayoutHandler
    {
        private LayoutViewGroupExt? layoutViewGroupExt;

		/// <exclude/>
		protected override LayoutViewGroup CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
            }

            this.layoutViewGroupExt = new LayoutViewGroupExt(Context, (SfView)VirtualView)
            {
                CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
                CrossPlatformArrange = VirtualView.CrossPlatformArrange
            };

            this.layoutViewGroupExt.SetClipChildren(true);

            return this.layoutViewGroupExt;
        }

		/// <exclude/>
		public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);

            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
                this.layoutViewGroupExt.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
            }
        }

		/// <exclude/>
		public void Invalidate()
        {
            this.PlatformView?.Invalidate();
        }

		/// <exclude/>
		public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
        {
            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt.DrawingOrder = drawingOrder;
            }
        }

		/// <exclude/>
		public void UpdateClipToBounds(bool clipToBounds)
        {
            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt.ClipsToBounds = clipToBounds;
            }
        }

        /// <summary>
        /// Invalidates the semantics nodes.
        /// </summary>
        internal void InvalidateSemantics()
        {
            this.layoutViewGroupExt?.InvalidateSemantics();
        }

		/// <exclude/>
		protected override void DisconnectHandler(LayoutViewGroup platformView)
        {
            base.DisconnectHandler(platformView);
            foreach (var child in VirtualView)
            {
                child.Handler?.DisconnectHandler();
            }

            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt = null;
            }
        }
    }
}
