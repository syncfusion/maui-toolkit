using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Platform;
using PlatformView = UIKit.UIView;

namespace Syncfusion.Maui.Toolkit
{
	/// <exclude/>
	public partial class SfViewHandler : LayoutHandler
	{

		/// <exclude/>
		protected override LayoutView CreatePlatformView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
			}

			return new LayoutViewExt((IDrawable)VirtualView);;
		}

		/// <exclude/>
		public void Invalidate()
		{
			 if (PlatformView is LayoutViewExt layoutViewExt)
			 {
				layoutViewExt.InvalidateDrawable();
			 }
		}

		/// <exclude/>
		public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
		{
			if (PlatformView is LayoutViewExt layoutViewExt)
			{
				layoutViewExt.DrawingOrder = drawingOrder;
			}
		}

		/// <exclude/>
		public void UpdateClipToBounds(bool clipToBounds)
		{
			if (PlatformView != null)
			{
				PlatformView.ClipsToBounds = clipToBounds;
			}
		}

		/// <exclude/>
		public new void Add(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			var index = PlatformView.Subviews.Length;
			if (PlatformView is LayoutViewExt layoutViewExt)
			{
				if (layoutViewExt.DrawingOrder == DrawingOrder.AboveContent)
				{
					if (index > 0)
					{
						PlatformView.InsertSubview(child.ToPlatform(MauiContext), index - 1);
					}
					else
					{
						PlatformView.InsertSubview(child.ToPlatform(MauiContext), index);
					}
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
			if (PlatformView is LayoutViewExt layoutViewExt)
			{
				layoutViewExt.InvalidateSemantics();
			}
		}

		/// <exclude/>
		protected override void DisconnectHandler(LayoutView platformView)
		{
			base.DisconnectHandler(platformView);
			foreach (var child in VirtualView)
			{
				child.Handler?.DisconnectHandler();
			}

			if (platformView is LayoutViewExt layoutViewExt)
			{
				layoutViewExt.ClearViews();
			}
		}
	}
}
