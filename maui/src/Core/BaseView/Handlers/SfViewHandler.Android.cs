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

			layoutViewGroupExt = new LayoutViewGroupExt(Context, (SfView)VirtualView)
			{
				CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
				CrossPlatformArrange = VirtualView.CrossPlatformArrange
			};

			layoutViewGroupExt.SetClipChildren(true);

			return layoutViewGroupExt;
		}

		/// <exclude/>
		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			if (layoutViewGroupExt != null)
			{
				layoutViewGroupExt.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
				layoutViewGroupExt.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
			}
		}

		/// <exclude/>
		public void Invalidate()
		{
			PlatformView?.Invalidate();
		}

		/// <exclude/>
		public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
		{
			if (layoutViewGroupExt != null)
			{
				layoutViewGroupExt.DrawingOrder = drawingOrder;
			}
		}

		/// <exclude/>
		public void UpdateClipToBounds(bool clipToBounds)
		{
			if (layoutViewGroupExt != null)
			{
				layoutViewGroupExt.ClipsToBounds = clipToBounds;
			}
		}

		/// <summary>
		/// Invalidates the semantics nodes.
		/// </summary>
		internal void InvalidateSemantics()
		{
			layoutViewGroupExt?.InvalidateSemantics();
		}

		/// <exclude/>
		protected override void DisconnectHandler(LayoutViewGroup platformView)
		{
			base.DisconnectHandler(platformView);
			foreach (var child in VirtualView)
			{
				child.Handler?.DisconnectHandler();
			}

			if (layoutViewGroupExt != null)
			{
				layoutViewGroupExt = null;
			}
		}
	}
}
