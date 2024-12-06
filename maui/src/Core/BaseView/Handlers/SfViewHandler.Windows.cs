using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Platform;

namespace Syncfusion.Maui.Toolkit
{
	/// <exclude/>
	public partial class SfViewHandler : LayoutHandler
	{
		private LayoutPanelExt? _layoutPanelExt;

		/// <exclude/>
		protected override LayoutPanel CreatePlatformView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
			}

			_layoutPanelExt = new LayoutPanelExt((SfView)VirtualView)
			{
				CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
				CrossPlatformArrange = VirtualView.CrossPlatformArrange
			};

			return _layoutPanelExt;
		}

		/// <exclude/>
		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			if (_layoutPanelExt != null)
			{
				_layoutPanelExt.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
				_layoutPanelExt.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
			}

		}

		/// <exclude/>
		public void Invalidate()
		{
			_layoutPanelExt?.Invalidate();
		}

		/// <exclude/>
		public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
		{
			if (_layoutPanelExt != null)
			{
				_layoutPanelExt.DrawingOrder = drawingOrder;
			}
		}

		/// <exclude/>
		public void UpdateClipToBounds(bool clipToBounds)
		{
			if (_layoutPanelExt != null)
			{
				_layoutPanelExt.ClipsToBounds = clipToBounds;
			}
		}

		/// <exclude/>
		public new void Add(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			var index = PlatformView.Children.Count;
			if (_layoutPanelExt != null)
			{
				if (_layoutPanelExt?.DrawingOrder == DrawingOrder.AboveContent)
				{
					if (index > 0)
					{
						PlatformView.Children.Insert(index - 1, child.ToPlatform(MauiContext));
					}
					else
					{
						PlatformView.Children.Insert(index, child.ToPlatform(MauiContext));
					}
				}
				else
				{
					PlatformView.Children.Insert(index, child.ToPlatform(MauiContext));
				}
			}
		}

		/// <exclude/>
		public new void Insert(int index, IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			if (_layoutPanelExt != null)
			{
				if (_layoutPanelExt?.DrawingOrder == DrawingOrder.BelowContent)
				{
					PlatformView.Children.Insert(index + 1, child.ToPlatform(MauiContext));
				}
				else
				{
					PlatformView.Children.Insert(index, child.ToPlatform(MauiContext));
				}
			}
		}

		/// <summary>
		/// Invalidates the semantics nodes.
		/// </summary>
		internal void InvalidateSemantics()
		{
			_layoutPanelExt?.InvalidateSemantics();
		}

		/// <exclude/>
		protected override void DisconnectHandler(LayoutPanel platformView)
		{
			base.DisconnectHandler(platformView);

			foreach (var child in VirtualView)
			{
				child.Handler?.DisconnectHandler();
			}
			if (_layoutPanelExt != null)
			{
				_layoutPanelExt.Dispose();
				_layoutPanelExt = null;
			}
		}
	}
}
