using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.Platform;
using UIKit;
namespace Syncfusion.Maui.Toolkit.BottomSheet
{
	public partial class SfBottomSheet
	{
		#region Fields

		/// <summary>
		/// Gets the native view.
		/// </summary>
		LayoutViewExt? _layoutViewExt;

		#endregion

		#region Private Methods

		void ConfigureTouch()
        {
            if (Handler?.PlatformView is null)
            {
                UnwireEvents();
            }
            else
            {
                WireEvents();
            }
        }

        void WireEvents()
		{
			if (Handler?.PlatformView is LayoutViewExt nativeView)
			{
				_layoutViewExt = nativeView;
				// Sheet container must receive interactions
				_layoutViewExt.UserInteractionEnabled = false;
				_layoutViewExt.IsAccessibilityElement = false;
				_layoutViewExt.AccessibilityTraits = (UIAccessibilityTrait)UIAccessibilityTraits.None;
			}
		}

        void UnwireEvents()
        {
            _layoutViewExt = null;
        }

#if !MACCATALYST && IOS
		/// <summary>
		/// Updates accessibility tree for iOS/macOS after layout stabilization.
		/// 
		/// CRITICAL: This method is called with a 50ms delay from Show() to ensure:
		/// 1. UIView hierarchy is fully laid out
		/// 2. Accessibility APIs recognize the new hierarchy before VoiceOver queries it
		/// 3. First-time focus doesn't leak to background on macOS/iOS
		/// 
		/// When IsOpen is true:
		///   - Sheet is marked as modal (AccessibilityViewIsModal = true)
		///   - Background content is hidden (AccessibilityElementsHidden = true)
		///   - Overlay is hidden from accessibility tree
		///   - VoiceOver focus is trapped within sheet
		///   - Accessibility tree is refreshed via UIAccessibility notification
		/// 
		/// When IsOpen is false:
		///   - Sheet is unmarked as modal
		///   - Background content is made accessible again
		///   - Accessibility tree is refreshed to restore focus
		/// </summary>
		partial void UpdateAccessibilityiOS()
		{
			// Get native UIView references from MAUI handlers
			var nativeContent = Content?.Handler?.PlatformView as UIView;
			var nativeSheet = _bottomSheetContent?.Handler?.PlatformView as UIView;
			var nativeOverlay = _overlayGrid?.Handler?.PlatformView as UIView;
 			bool isSingle = IsSingleContentElement(Content);

			if (IsOpen && IsModal)
			{

				// === MODAL STATE: Trap focus within sheet ===

				// 1. Mark bottom sheet content as modal region
				// This instructs VoiceOver to limit focus/navigation to elements within this view
				// CRITICAL: Must be set BEFORE posting the accessibility notification
				if (nativeSheet != null)
				{
					nativeSheet.AccessibilityViewIsModal = true;
					nativeSheet.IsAccessibilityElement = false;  // Container, not a standalone element					

				}

				// 2. Hide background content from VoiceOver
				// This prevents VoiceOver from navigating to, focusing on, or reading background content
				if (nativeContent != null)
				{
					if(isSingle)
                    {
                        nativeContent.AccessibilityElementsHidden = true;
						nativeContent.IsAccessibilityElement = false;
                    }
					else
					{
						nativeContent.AccessibilityElementsHidden = false;
						nativeContent.IsAccessibilityElement = true;
					}
				}

				// 4. Notify VoiceOver to rebuild focus order and accessibility tree
				// This is CRITICAL for first-time open - tells VoiceOver about the new modal state
				// Use ScreenChanged to indicate major UI change (like modal appearing)
				if (nativeSheet != null)
				{
					UIAccessibility.PostNotification(
						UIAccessibilityPostNotification.ScreenChanged,
						nativeSheet);
				}

				if(Content != null)
				{
					UpdateContentAccessibility(Content,true);
				}
			}
			else
			{

				// === NON-MODAL STATE: Restore background accessibility ===
				// 1. Unmark sheet as modal
				if (nativeSheet != null)
				{
					nativeSheet.AccessibilityViewIsModal = false;
				}

				// 2. Restore background accessibility
				if (nativeContent != null)
				{
					if(isSingle)
                    {
						nativeContent.AccessibilityElementsHidden = false;
						nativeContent.IsAccessibilityElement = true;
                    }
					else
					{
						nativeContent.AccessibilityElementsHidden = true;
						nativeContent.IsAccessibilityElement = false;
					}
				}

				// 4. Notify VoiceOver to restore focus to background
				if (nativeContent != null)
				{
					UIAccessibility.PostNotification(
						UIAccessibilityPostNotification.ScreenChanged,
						nativeContent);
				}

				if(Content != null)
				{
					UpdateContentAccessibility(Content,false);
				}
			}
		}

		bool IsSingleContentElement(View? content)
		{
			if (content == null)
				return false;

			if (content is Layout layout)
				return layout.Children.Count == 1;

			
			if (content is IContentView contentView &&
					contentView.Content is Layout innerLayout)
				{
					return innerLayout.Children.Count == 1;
				}

			return true;
		}
#endif

		#endregion

		#region Override Methods

		/// <summary>
		/// Raises on handler changing.
		/// </summary>
		/// <param name="args">Relevant <see cref="HandlerChangingEventArgs"/>.</param>
		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
		{
			if (args.OldHandler is not null)
            {
                UnwireEvents();
            }

            base.OnHandlerChanging(args);
		}

		#endregion
	}
}
