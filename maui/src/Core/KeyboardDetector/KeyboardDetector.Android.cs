using MauiView = Microsoft.Maui.Controls.View;
using Android.Views;
using View = Android.Views.View;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Detects keyboard events and handles related functionality on the Android platfrom.
	/// </summary>
	public partial class KeyboardDetector
	{
		internal void SubscribeNativeKeyEvents(MauiView? mauiView)
		{
			if (mauiView != null)
			{
				var handler = mauiView.Handler;

				if (handler?.PlatformView is View nativeView)
				{
					if (keyboardListeners.Count > 0)
					{
						nativeView.KeyPress += PlatformView_KeyPress;
					}
				}
			}
		}

		internal void CreateNativeListener()
		{
			SubscribeNativeKeyEvents(MauiView);
		}

		private void PlatformView_KeyPress(object? sender, Android.Views.View.KeyEventArgs e)
		{
			KeyboardKey key = KeyboardListenerExtension.ConvertToKeyboardKey(e.KeyCode);
			var args = new KeyEventArgs(key)
			{
				IsShiftKeyPressed = e.Event!.MetaState.HasFlag(MetaKeyStates.ShiftOn),
				IsCtrlKeyPressed = e.Event!.MetaState.HasFlag(MetaKeyStates.CtrlOn),
				IsAltKeyPressed = e.Event!.MetaState.HasFlag(MetaKeyStates.AltOn),
				IsCapsLockOn = e.Event!.MetaState.HasFlag(MetaKeyStates.CapsLockOn) || e.Event!.MetaState.HasFlag(MetaKeyStates.ShiftLeftOn),
				IsNumLockOn = e.Event!.MetaState.HasFlag(MetaKeyStates.NumLockOn),
				IsScrollLockOn = e.Event!.MetaState.HasFlag(MetaKeyStates.ScrollLockOn),
				IsCommandKeyPressed = false,
				KeyAction = e.Event.Action != KeyEventActions.Up ? KeyActions.KeyDown : KeyActions.KeyUp
			};
			OnKeyAction(args);
			e.Handled = args.Handled;
		}

		internal void UnsubscribeNativeKeyEvents(IElementHandler handler)
		{
			if (handler != null)
			{
				if (handler.PlatformView is View nativeView)
				{
					nativeView.KeyPress -= PlatformView_KeyPress;
				}
			}
		}
	}
}
