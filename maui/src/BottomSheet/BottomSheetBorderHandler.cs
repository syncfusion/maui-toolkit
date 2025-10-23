#if ANDROID
using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.BottomSheet
{
	/// <summary>
	/// Represents the <see cref="BottomSheetBorderHandler"/> Android handler, that creates its native platform view with edge-aware touch interception.
	/// </summary>
	internal class BottomSheetBorderHandler : BorderHandler
	{
		protected override ContentViewGroup CreatePlatformView()
		{
			// Correctly typed as IContentView by ContentViewHandler
			var content = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} must be set.");

			if (content is not BottomSheetBorder border)
			{
				throw new InvalidOperationException(
					$"Expected {typeof(BottomSheetBorder).FullName}, got {content.GetType().FullName}. " +
					$"Verify handler registration maps BottomSheetBorder -> BottomSheetBorderHandler.");
			}

			// Use our custom ContentViewGroup subclass that intercepts touch
			var pv = new BottomSheetBorderPlatformView(Context, border);
			pv.SetClipChildren(true);
			return pv;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
		}
	}
}
#endif