#if WINDOWS10_0_19041_0
using Windows.UI.Core;
using Microsoft.UI.Input;
using Key = Windows.System.VirtualKey;
#elif __ANDROID__
using Key = Android.Views.Keycode;
#elif IOS
using UIKit;
using Key = UIKit.UIKey;
using Foundation;
#endif
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Represents the extension class that create instance for <see cref="KeyboardDetector"/> class and set to target class.
    /// </summary>
    public static class KeyboardListenerExtension
    {
        internal static BindableProperty KeyboardDetectorProperty = BindableProperty.Create(nameof(KeyboardDetector), typeof(KeyboardDetector), typeof(View), null);

        /// <summary>
        /// Create the keyboard detector and add the listener to it.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="listener"></param>
        public static void AddKeyboardListener(this View target, IKeyboardListener listener)
        {
            var keyboardDetector = target.GetValue(KeyboardDetectorProperty) as KeyboardDetector;

            if (keyboardDetector == null)
            {
                keyboardDetector = new KeyboardDetector(target);
                target.SetValue(KeyboardDetectorProperty, keyboardDetector);
            }

            keyboardDetector.AddListener(listener);
        }

        /// <summary>
        /// Remove the listener and keyboard detector. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="listener"></param>
        public static void RemoveKeyboardListener(this View target, IKeyboardListener listener)
        {
            var keyboardDetector = target.GetValue(KeyboardDetectorProperty) as KeyboardDetector;

            if (keyboardDetector != null)
            {
                keyboardDetector.RemoveListener(listener);
                if (!keyboardDetector.HasListener())
                {
                    keyboardDetector.Dispose();
                    target.SetValue(KeyboardDetectorProperty, null);
                }
            }
        }

#if IOS
        /// <summary>
        /// Processes the <see cref="IKeyboardListener.OnKeyDown(KeyEventArgs)"/> when the key press event is triggered from native view".
        /// </summary>
        /// <param name="target">The view where the keyboard listener has been added.</param>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the new presses that occurred.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        /// <returns>Returns whether the routed event is handled or not.</returns>
        /// <remarks>This method is applicable only for iOS platform.</remarks>
        internal static bool HandleKeyPress(this View target, NSSet<UIPress> presses, UIPressesEvent evt)
        {
            var keyboardDetector = target.GetValue(KeyboardDetectorProperty) as KeyboardDetector;
            if (keyboardDetector != null)
            {
                return keyboardDetector.HandleKeyActions(presses, evt, KeyActions.KeyDown);
            }

            return false;
        }

        /// <summary>
        /// Processes the <see cref="IKeyboardListener.OnKeyUp(KeyEventArgs)"/> when the key release event is triggered from native view".
        /// </summary>
        /// <param name="target">The view where the keyboard listener has been added.</param>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the buttons that the user is no longer pressing.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        /// <returns>Returns whether the routed event is handled or not.</returns>
        /// <remarks>This method is applicable only for iOS platform.</remarks>
        internal static bool HandleKeyRelease(this View target, NSSet<UIPress> presses, UIPressesEvent evt)
        {
            var keyboardDetector = target.GetValue(KeyboardDetectorProperty) as KeyboardDetector;
            if (keyboardDetector != null)
            {
                return keyboardDetector.HandleKeyActions(presses, evt, KeyActions.KeyUp);
            }

            return false;
        }
#endif

        /// <summary>
        /// Clear the listeners and keyboard detector.
        /// </summary>
        /// <param name="target"></param>
        public static void ClearKeyboardListeners(this View target)
        {
            var keyboardDetector = target.GetValue(KeyboardDetectorProperty) as KeyboardDetector;

            if (keyboardDetector != null)
            {
                keyboardDetector.ClearListeners();
                keyboardDetector.Dispose();
                target.SetValue(KeyboardDetectorProperty, null);
            }
        }

#if __ANDROID__ || WINDOWS10_0_19041_0 || IOS
        /// <summary>
        /// Convert native keys to <see cref="KeyboardKey"/>.
        /// </summary>
        /// <param name="argsKey">The native key</param>
        /// <returns>Returns the converted <see cref="KeyboardKey"/></returns>
        internal static KeyboardKey ConvertToKeyboardKey(Key argsKey)
        {
            KeyboardKey keyboardKey = KeyboardKey.None;
#if __ANDROID__ || WINDOWS10_0_19041_0
            switch (argsKey)
            {
                case Key.Space:
                    keyboardKey = KeyboardKey.Space;
                    break;
                case Key.Tab:
                    keyboardKey = KeyboardKey.Tab;
                    break;
                case Key.Home:
                    keyboardKey = KeyboardKey.Home;
                    break;
                case Key.PageDown:
                    keyboardKey = KeyboardKey.PageDown;
                    break;
                case Key.PageUp:
                    keyboardKey = KeyboardKey.PageUp;
                    break;
                case Key.Back:
                    keyboardKey = KeyboardKey.Back;
                    break;
#if !__ANDROID__
                case Key.End:
                    keyboardKey = KeyboardKey.End;
                    break;
                case Key.Up:
                    keyboardKey = KeyboardKey.Up;
                    break;
                case Key.Down:
                    keyboardKey = KeyboardKey.Down;
                    break;
                case Key.Right:
                    keyboardKey = KeyboardKey.Right;
                    break;
                case Key.Left:
                    keyboardKey = KeyboardKey.Left;
                    break;
                case Key.Delete:
                    keyboardKey = KeyboardKey.Delete;
                    break;
                case Key.Add:
                    keyboardKey = KeyboardKey.Add;
                    break;
                case Key.Subtract:
                    keyboardKey = KeyboardKey.Subtract;
                    break;
                case Key.Multiply:
                    keyboardKey = KeyboardKey.Multiply;
                    break;
                case Key.Divide:
                    keyboardKey = KeyboardKey.Divide;
                    break;
                case Key.Print:
                    keyboardKey = KeyboardKey.Print;
                    break;
                case Key.CapitalLock:
                    keyboardKey = KeyboardKey.CapsLock;
                    break;
                case Key.NumberKeyLock:
                    keyboardKey = KeyboardKey.NumLock;
                    break;
                case Key.Scroll:
                    keyboardKey = KeyboardKey.ScrollLock;
                    break;
                case Key.Decimal:
                    keyboardKey = KeyboardKey.Decimal;
                    break;
                case Key.Shift:
                    keyboardKey = KeyboardKey.Shift;
                    break;
                case Key.Control:
                    keyboardKey = KeyboardKey.Ctrl;
                    break;
#elif __ANDROID__
                case Key.DpadUp:
                    keyboardKey = KeyboardKey.Up;
                    break;
                case Key.DpadDown:
                    keyboardKey = KeyboardKey.Down;
                    break;
                case Key.DpadRight:
                    keyboardKey = KeyboardKey.Right;
                    break;
                case Key.DpadLeft:
                    keyboardKey = KeyboardKey.Left;
                    break;               
                case Key.Del:
                    keyboardKey = KeyboardKey.Delete;
                    break;
                case Key.NumpadAdd:
                    keyboardKey = KeyboardKey.Add;
                    break;
                case Key.NumpadSubtract:
                case Key.Minus:
                    keyboardKey = KeyboardKey.Subtract;
                    break;
                case Key.NumpadMultiply:
                    keyboardKey = KeyboardKey.Multiply;
                    break;
                case Key.NumpadDivide:
                    keyboardKey = KeyboardKey.Divide;                   
                    break;
                case Key.Equals:
                    keyboardKey = KeyboardKey.Equals;
                    break;
                case Key.Sysrq:
                    keyboardKey = KeyboardKey.Print;
                    break;
                case Key.CapsLock:
                    keyboardKey = KeyboardKey.CapsLock;
                    break;
                case Key.NumLock:
                    keyboardKey = KeyboardKey.NumLock;
                    break;
                case Key.ScrollLock:
                    keyboardKey = KeyboardKey.ScrollLock;
                    break;
                case Key.NumpadDot:
                    keyboardKey = KeyboardKey.Decimal;
                    break;
                case Key.ShiftLeft:
                case Key.ShiftRight:
                    keyboardKey = KeyboardKey.Shift;
                    break;
                case Key.CtrlLeft:
                case Key.CtrlRight:
                    keyboardKey = KeyboardKey.Ctrl;
                    break;
                case Key.MoveHome:
                    keyboardKey = KeyboardKey.Home;
                    break;
                case Key.MoveEnd:
                    keyboardKey = KeyboardKey.End;
                    break;
#endif
                case Key.Enter:
                    keyboardKey = KeyboardKey.Enter;
                    break;
                case Key.Escape:
                    keyboardKey = KeyboardKey.Escape;
                    break;
                case Key.Insert:
                    keyboardKey = KeyboardKey.Insert;
                    break;
                case Key.Help:
                    keyboardKey = KeyboardKey.Help;
                    break;
                case Key.Menu:
                    keyboardKey = KeyboardKey.Alt;
                    break;
                case Key.A:
                    keyboardKey = KeyboardKey.A;
                    break;
                case Key.B:
                    keyboardKey = KeyboardKey.B;
                    break;
                case Key.C:
                    keyboardKey = KeyboardKey.C;
                    break;
                case Key.D:
                    keyboardKey = KeyboardKey.D;
                    break;
                case Key.E:
                    keyboardKey = KeyboardKey.E;
                    break;
                case Key.F:
                    keyboardKey = KeyboardKey.F;
                    break;
                case Key.G:
                    keyboardKey = KeyboardKey.G;
                    break;
                case Key.H:
                    keyboardKey = KeyboardKey.H;
                    break;
                case Key.I:
                    keyboardKey = KeyboardKey.I;
                    break;
                case Key.J:
                    keyboardKey = KeyboardKey.J;
                    break;
                case Key.K:
                    keyboardKey = KeyboardKey.K;
                    break;
                case Key.L:
                    keyboardKey = KeyboardKey.L;
                    break;
                case Key.M:
                    keyboardKey = KeyboardKey.M;
                    break;
                case Key.N:
                    keyboardKey = KeyboardKey.N;
                    break;
                case Key.O:
                    keyboardKey = KeyboardKey.O;
                    break;
                case Key.P:
                    keyboardKey = KeyboardKey.P;
                    break;
                case Key.Q:
                    keyboardKey = KeyboardKey.Q;
                    break;
                case Key.R:
                    keyboardKey = KeyboardKey.R;
                    break;
                case Key.S:
                    keyboardKey = KeyboardKey.S;
                    break;
                case Key.T:
                    keyboardKey = KeyboardKey.T;
                    break;
                case Key.U:
                    keyboardKey = KeyboardKey.U;
                    break;
                case Key.V:
                    keyboardKey = KeyboardKey.V;
                    break;
                case Key.W:
                    keyboardKey = KeyboardKey.W;
                    break;
                case Key.X:
                    keyboardKey = KeyboardKey.X;
                    break;
                case Key.Y:
                    keyboardKey = KeyboardKey.Y;
                    break;
                case Key.Z:
                    keyboardKey = KeyboardKey.Z;
                    break;
#if WINDOWS10_0_19041_0
                case Key.Number0:
                    keyboardKey = KeyboardKey.Num0;
                    break;
                case Key.Number1:
                    keyboardKey = KeyboardKey.Num1;
                    break;
                case Key.Number2:
                    keyboardKey = KeyboardKey.Num2;
                    break;
                case Key.Number3:
                    keyboardKey = KeyboardKey.Num3;
                    break;
                case Key.Number4:
                    keyboardKey = KeyboardKey.Num4;
                    break;
                case Key.Number5:
                    keyboardKey = KeyboardKey.Num5;
                    break;
                case Key.Number6:
                    keyboardKey = KeyboardKey.Num6;
                    break;
                case Key.Number7:
                    keyboardKey = KeyboardKey.Num7;
                    break;
                case Key.Number8:
                    keyboardKey = KeyboardKey.Num8;
                    break;
                case Key.Number9:
                    keyboardKey = KeyboardKey.Num9;
                    break;
                case Key.F13:
                    keyboardKey = KeyboardKey.F13;
                    break;
                case Key.F14:
                    keyboardKey = KeyboardKey.F14;
                    break;
                case Key.F15:
                    keyboardKey = KeyboardKey.F15;
                    break;
                case Key.F16:
                    keyboardKey = KeyboardKey.F16;
                    break;
                case Key.F17:
                    keyboardKey = KeyboardKey.F17;
                    break;
                case Key.F18:
                    keyboardKey = KeyboardKey.F18;
                    break;
                case Key.F19:
                    keyboardKey = KeyboardKey.F19;
                    break;
                case Key.F20:
                    keyboardKey = KeyboardKey.F20;
                    break;
                case Key.F21:
                    keyboardKey = KeyboardKey.F21;
                    break;
                case Key.F22:
                    keyboardKey = KeyboardKey.F22;
                    break;
                case Key.F23:
                    keyboardKey = KeyboardKey.F23;
                    break;
                case Key.F24:
                    keyboardKey = KeyboardKey.F24;
                    break;
#elif __ANDROID__
                case Key.Num0:
                    keyboardKey = KeyboardKey.Num0;
                    break;
                case Key.Num1:
                    keyboardKey = KeyboardKey.Num1;
                    break;
                case Key.Num2:
                    keyboardKey = KeyboardKey.Num2;
                    break;
                case Key.Num3:
                    keyboardKey = KeyboardKey.Num3;
                    break;
                case Key.Num4:
                    keyboardKey = KeyboardKey.Num4;
                    break;
                case Key.Num5:
                    keyboardKey = KeyboardKey.Num5;
                    break;
                case Key.Num6:
                    keyboardKey = KeyboardKey.Num6;
                    break;
                case Key.Num7:
                    keyboardKey = KeyboardKey.Num7;
                    break;
                case Key.Num8:
                    keyboardKey = KeyboardKey.Num8;
                    break;
                case Key.Num9:
                    keyboardKey = KeyboardKey.Num9;
                    break;
#endif
                case Key.F1:
                    keyboardKey = KeyboardKey.F1;
                    break;
                case Key.F2:
                    keyboardKey = KeyboardKey.F2;
                    break;
                case Key.F3:
                    keyboardKey = KeyboardKey.F3;
                    break;
                case Key.F4:
                    keyboardKey = KeyboardKey.F4;
                    break;
                case Key.F5:
                    keyboardKey = KeyboardKey.F5;
                    break;
                case Key.F6:
                    keyboardKey = KeyboardKey.F6;
                    break;
                case Key.F7:
                    keyboardKey = KeyboardKey.F7;
                    break;
                case Key.F8:
                    keyboardKey = KeyboardKey.F8;
                    break;
                case Key.F9:
                    keyboardKey = KeyboardKey.F9;
                    break;
                case Key.F10:
                    keyboardKey = KeyboardKey.F10;
                    break;
                case Key.F11:
                    keyboardKey = KeyboardKey.F11;
                    break;
                case Key.F12:
                    keyboardKey = KeyboardKey.F12;
                    break;
            }
#elif IOS
            switch(argsKey.KeyCode)
            {
                case UIKeyboardHidUsage.KeyboardDownArrow:
                    keyboardKey = KeyboardKey.Down;
                    break;
                case UIKeyboardHidUsage.KeyboardUpArrow:
                    keyboardKey = KeyboardKey.Up;
                    break;
                case UIKeyboardHidUsage.KeyboardLeftArrow:
                    keyboardKey = KeyboardKey.Left;
                    break;
                case UIKeyboardHidUsage.KeyboardRightArrow:
                    keyboardKey = KeyboardKey.Right;
                    break;
                case UIKeyboardHidUsage.KeyboardLeftShift:
                case UIKeyboardHidUsage.KeyboardRightShift:
                    keyboardKey = KeyboardKey.Shift;
                    break;
                case UIKeyboardHidUsage.KeyboardLeftControl:
                case UIKeyboardHidUsage.KeyboardRightControl:
                    keyboardKey = KeyboardKey.Ctrl;
                    break;
                case UIKeyboardHidUsage.KeyboardLeftGui:
                case UIKeyboardHidUsage.KeyboardRightGui:
                    keyboardKey = KeyboardKey.Command;
                    break;
                case UIKeyboardHidUsage.KeyboardLeftAlt:
                case UIKeyboardHidUsage.KeyboardRightAlt:
                    keyboardKey = KeyboardKey.Alt;
                    break;
                case UIKeyboardHidUsage.KeyboardTab:
                    keyboardKey = KeyboardKey.Tab;
                    break;
                case UIKeyboardHidUsage.KeyboardHome:
                    keyboardKey = KeyboardKey.Home;
                    break;
                case UIKeyboardHidUsage.KeyboardEnd:
                    keyboardKey = KeyboardKey.End;
                    break;
                case UIKeyboardHidUsage.KeyboardPageUp:
                    keyboardKey = KeyboardKey.PageUp;
                    break;
                case UIKeyboardHidUsage.KeyboardPageDown:
                    keyboardKey = KeyboardKey.PageDown;
                    break;
                case UIKeyboardHidUsage.KeyboardReturnOrEnter:
                    keyboardKey = KeyboardKey.Enter;
                    break;
                case UIKeyboardHidUsage.KeyboardEscape:
                    keyboardKey = KeyboardKey.Escape;
                    break;
                case UIKeyboardHidUsage.KeyboardDeleteOrBackspace:
                    keyboardKey = KeyboardKey.Back;
                    break;
                case UIKeyboardHidUsage.KeyboardSpacebar:
                    keyboardKey = KeyboardKey.Space;
                    break;
                case UIKeyboardHidUsage.KeyboardDeleteForward:
                    keyboardKey = KeyboardKey.Delete;
                    break;
                case UIKeyboardHidUsage.KeyboardHelp:
                    keyboardKey = KeyboardKey.Insert;
                    break;
                case UIKeyboardHidUsage.KeypadPlus:
                    keyboardKey = KeyboardKey.Add;
                    break;
                case UIKeyboardHidUsage.KeyboardHyphen:
                case UIKeyboardHidUsage.KeypadHyphen:
                    keyboardKey = KeyboardKey.Subtract;
                    break;
                case UIKeyboardHidUsage.KeypadAsterisk:
                    keyboardKey = KeyboardKey.Multiply;
                    break;
                case UIKeyboardHidUsage.KeyboardSlash:
                case UIKeyboardHidUsage.KeypadSlash:
                    keyboardKey = KeyboardKey.Divide;
                    break;
                case UIKeyboardHidUsage.KeyboardPeriod:
                case UIKeyboardHidUsage.KeypadPeriod:
                    keyboardKey = KeyboardKey.Decimal;
                    break;
                case UIKeyboardHidUsage.KeyboardA:
                    keyboardKey = KeyboardKey.A;
                    break;
                case UIKeyboardHidUsage.KeyboardB:
                    keyboardKey = KeyboardKey.B;
                    break;
                case UIKeyboardHidUsage.KeyboardC:
                    keyboardKey = KeyboardKey.C;
                    break;
                case UIKeyboardHidUsage.KeyboardD:
                    keyboardKey = KeyboardKey.D;
                    break;
                case UIKeyboardHidUsage.KeyboardE:
                    keyboardKey = KeyboardKey.E;
                    break;
                case UIKeyboardHidUsage.KeyboardF:
                    keyboardKey = KeyboardKey.F;
                    break;
                case UIKeyboardHidUsage.KeyboardG:
                    keyboardKey = KeyboardKey.G;
                    break;
                case UIKeyboardHidUsage.KeyboardH:
                    keyboardKey = KeyboardKey.H;
                    break;
                case UIKeyboardHidUsage.KeyboardI:
                    keyboardKey = KeyboardKey.I;
                    break;
                case UIKeyboardHidUsage.KeyboardJ:
                    keyboardKey = KeyboardKey.J;
                    break;
                case UIKeyboardHidUsage.KeyboardK:
                    keyboardKey = KeyboardKey.K;
                    break;
                case UIKeyboardHidUsage.KeyboardL:
                    keyboardKey = KeyboardKey.L;
                    break;
                case UIKeyboardHidUsage.KeyboardM:
                    keyboardKey = KeyboardKey.M;
                    break;
                case UIKeyboardHidUsage.KeyboardN:
                    keyboardKey = KeyboardKey.N;
                    break;
                case UIKeyboardHidUsage.KeyboardO:
                    keyboardKey = KeyboardKey.O;
                    break;
                case UIKeyboardHidUsage.KeyboardP:
                    keyboardKey = KeyboardKey.P;
                    break;
                case UIKeyboardHidUsage.KeyboardQ:
                    keyboardKey = KeyboardKey.Q;
                    break;
                case UIKeyboardHidUsage.KeyboardR:
                    keyboardKey = KeyboardKey.R;
                    break;
                case UIKeyboardHidUsage.KeyboardS:
                    keyboardKey = KeyboardKey.S;
                    break;
                case UIKeyboardHidUsage.KeyboardT:
                    keyboardKey = KeyboardKey.T;
                    break;
                case UIKeyboardHidUsage.KeyboardU:
                    keyboardKey = KeyboardKey.U;
                    break;
                case UIKeyboardHidUsage.KeyboardV:
                    keyboardKey = KeyboardKey.V;
                    break;
                case UIKeyboardHidUsage.KeyboardW:
                    keyboardKey = KeyboardKey.W;
                    break;
                case UIKeyboardHidUsage.KeyboardX:
                    keyboardKey = KeyboardKey.X;
                    break;
                case UIKeyboardHidUsage.KeyboardY:
                    keyboardKey = KeyboardKey.Y;
                    break;
                case UIKeyboardHidUsage.KeyboardZ:
                    keyboardKey = KeyboardKey.Z;
                    break;
                case UIKeyboardHidUsage.KeyboardCapsLock:
                    keyboardKey = KeyboardKey.CapsLock;
                    break;
                case UIKeyboardHidUsage.KeyboardScrollLock:
                    keyboardKey = KeyboardKey.ScrollLock;
                    break;
                case UIKeyboardHidUsage.KeypadNumLock:
                    keyboardKey = KeyboardKey.NumLock;
                    break;
                case UIKeyboardHidUsage.Keyboard0:
                case UIKeyboardHidUsage.Keypad0:
                    keyboardKey = KeyboardKey.Num0;
                    break;
                case UIKeyboardHidUsage.Keyboard1:
                case UIKeyboardHidUsage.Keypad1:
                    keyboardKey = KeyboardKey.Num1;
                    break;
                case UIKeyboardHidUsage.Keypad2:
                case UIKeyboardHidUsage.Keyboard2:
                    keyboardKey = KeyboardKey.Num2;
                    break;
                case UIKeyboardHidUsage.Keyboard3:
                case UIKeyboardHidUsage.Keypad3:
                    keyboardKey = KeyboardKey.Num3;
                    break;
                case UIKeyboardHidUsage.Keyboard4:
                case UIKeyboardHidUsage.Keypad4:
                    keyboardKey = KeyboardKey.Num4;
                    break;
                case UIKeyboardHidUsage.Keyboard5:
                case UIKeyboardHidUsage.Keypad5:
                    keyboardKey = KeyboardKey.Num5;
                    break;
                case UIKeyboardHidUsage.Keyboard6:
                case UIKeyboardHidUsage.Keypad6:
                    keyboardKey = KeyboardKey.Num6;
                    break;
                case UIKeyboardHidUsage.Keyboard7:
                case UIKeyboardHidUsage.Keypad7:
                    keyboardKey = KeyboardKey.Num7;
                    break;
                case UIKeyboardHidUsage.Keyboard8:
                case UIKeyboardHidUsage.Keypad8:
                    keyboardKey = KeyboardKey.Num8;
                    break;
                case UIKeyboardHidUsage.Keyboard9:
                case UIKeyboardHidUsage.Keypad9:
                    keyboardKey = KeyboardKey.Num9;
                    break;
                case UIKeyboardHidUsage.KeyboardF1:
                    keyboardKey = KeyboardKey.F1;
                    break;
                case UIKeyboardHidUsage.KeyboardF2:
                    keyboardKey = KeyboardKey.F2;
                    break;
                case UIKeyboardHidUsage.KeyboardF3:
                    keyboardKey = KeyboardKey.F3;
                    break;
                case UIKeyboardHidUsage.KeyboardF4:
                    keyboardKey = KeyboardKey.F4;
                    break;
                case UIKeyboardHidUsage.KeyboardF5:
                    keyboardKey = KeyboardKey.F5;
                    break;
                case UIKeyboardHidUsage.KeyboardF6:
                    keyboardKey = KeyboardKey.F6;
                    break;
                case UIKeyboardHidUsage.KeyboardF7:
                    keyboardKey = KeyboardKey.F7;
                    break;
                case UIKeyboardHidUsage.KeyboardF8:
                    keyboardKey = KeyboardKey.F8;
                    break;
                case UIKeyboardHidUsage.KeyboardF9:
                    keyboardKey = KeyboardKey.F9;
                    break;
                case UIKeyboardHidUsage.KeyboardF10:
                    keyboardKey = KeyboardKey.F10;
                    break;
                case UIKeyboardHidUsage.KeyboardF11:
                    keyboardKey = KeyboardKey.F11;
                    break;
                case UIKeyboardHidUsage.KeyboardF12:
                    keyboardKey = KeyboardKey.F12;
                    break;
                case UIKeyboardHidUsage.KeyboardF13:
                    keyboardKey = KeyboardKey.F13;
                    break;
                case UIKeyboardHidUsage.KeyboardF14:
                    keyboardKey = KeyboardKey.F14;
                    break;
                case UIKeyboardHidUsage.KeyboardF15:
                    keyboardKey = KeyboardKey.F15;
                    break;
                case UIKeyboardHidUsage.KeyboardF16:
                    keyboardKey = KeyboardKey.F16;
                    break;
                case UIKeyboardHidUsage.KeyboardF17:
                    keyboardKey = KeyboardKey.F17;
                    break;
                case UIKeyboardHidUsage.KeyboardF18:
                    keyboardKey = KeyboardKey.F18;
                    break;
                case UIKeyboardHidUsage.KeyboardF19:
                    keyboardKey = KeyboardKey.F19;
                    break;
                case UIKeyboardHidUsage.KeyboardF20:
                    keyboardKey = KeyboardKey.F20;
                    break;
                case UIKeyboardHidUsage.KeyboardF21:
                    keyboardKey = KeyboardKey.F21;
                    break;
                case UIKeyboardHidUsage.KeyboardF22:
                    keyboardKey = KeyboardKey.F22;
                    break;
                case UIKeyboardHidUsage.KeyboardF23:
                    keyboardKey = KeyboardKey.F23;
                    break;
                case UIKeyboardHidUsage.KeyboardF24:
                    keyboardKey = KeyboardKey.F24;
                    break;
                case UIKeyboardHidUsage.KeyboardEqualSign:
                    keyboardKey = KeyboardKey.Equals;
                    break;
            }
#endif

#if WINDOWS10_0_19041_0
            // MAUI-3924 -  In WinUI for keys like =,-,[,] etc Key is detected only as integer value and those keys were not in VirtualKey enum.
            if (keyboardKey == KeyboardKey.None && int.TryParse(argsKey.ToString(), out int value))
            {
                keyboardKey = (KeyboardKey)value;
            }
#endif
            return keyboardKey;
        }

#endif

#if WINDOWS10_0_19041_0
        /// <summary>
        /// Helper method to identify whether the Shift key is pressed.
        /// </summary>
        /// <returns>Returns true if shift key is pressed otherwise false.</returns>
        internal static bool CheckKeyPressedState(KeyboardKey keyboardKey)
        {
            switch (keyboardKey)
            {
                case KeyboardKey.CapsLock:
                    if (InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.CapitalLock).HasFlag(CoreVirtualKeyStates.Locked))
                    {
                        return true;
                    }
                    break;
                case KeyboardKey.Alt:
                    if (InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down))
                    {
                        return true;
                    }
                    break;
                case KeyboardKey.Shift:
                    if (InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down))
                    {
                        return true;
                    }
                    break;
                case KeyboardKey.Ctrl:
                    if (InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down))
                    {
                        return true;
                    }
                    break;
                case KeyboardKey.NumLock:
                    if (InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.NumberKeyLock).HasFlag(CoreVirtualKeyStates.Locked))
                    {
                        return true;
                    }
                    break;
                case KeyboardKey.ScrollLock:
                    if (InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.Scroll).HasFlag(CoreVirtualKeyStates.Locked))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
#endif
    }
}
