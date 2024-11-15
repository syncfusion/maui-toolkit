namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// This interface used to call the OnKeyDown method.
    /// </summary>
    public interface IKeyboardListener
    {
        /// <summary>
        /// Gets a value indicating whether the view can become the first responder to listen the keyboard actions.
        /// </summary>
        /// <remarks>This property will be considered only in iOS Platform.</remarks>
        /// <remarks>Enabling this property alone will not listen to keyboard activity; you must additionally add a keyboard listener and the view must be in focus.</remarks>
        bool CanBecomeFirstResponder
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Invoke on key down action.
        /// </summary>
        /// <param name="args"></param>
        void OnKeyDown(KeyEventArgs args);

        /// <summary>
        /// Invoke on key up action.
        /// </summary>
        /// <param name="args"></param>
        void OnKeyUp(KeyEventArgs args);

        /// <summary>
        /// Invoke before the key down action.
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>This method will be triggered only in WinUI Platform.</remarks>
        void OnPreviewKeyDown(KeyEventArgs args) { }

    }

    /// <summary>
    /// Describes the possible values of keyboard interactions.
    /// </summary>
    public enum KeyboardKey
    {
        /// <summary>
        /// Describe that default keyboard interaction type as None.
        /// </summary>
        None,

        /// <summary>
        /// Specifies the keyboard interaction type as Down arrow.
        /// </summary>
        Down,

        /// <summary>
        /// Specifies the keyboard interaction type as Up arrow.
        /// </summary>
        Up,

        /// <summary>
        /// Specifies the keyboard interaction type as Left arrow.
        /// </summary>
        Left,

        /// <summary>
        /// Specifies the keyboard interaction type as Right arrow.
        /// </summary>
        Right,

        /// <summary>
        /// Specifies the keyboard interaction type as Shift key.
        /// </summary>
        Shift,

        /// <summary>
        /// Specifies the keyboard interaction type as Control key.
        /// </summary>
        Ctrl,

        /// <summary>
        /// Specifies the keyboard interaction type as Command key.
        /// </summary>
        Command,

        /// <summary>
        /// Specifies the keyboard interaction type as Alt key.
        /// </summary>
        Alt,

        /// <summary>
        /// Specifies the keyboard interaction type as Tab key.
        /// </summary>
        Tab,

        /// <summary>
        /// Specifies the keyboard interaction type as Home key.
        /// </summary>
        Home,

        /// <summary>
        /// Specifies the keyboard interaction type as End key.
        /// </summary>
        End,

        /// <summary>
        /// Specifies the keyboard interaction type as PageUp key.
        /// </summary>
        PageUp,

        /// <summary>
        /// Specifies the keyboard interaction type as PageDown key.
        /// </summary>
        PageDown,

        /// <summary>
        /// Specifies the keyboard interaction type as Enter key.
        /// </summary>
        Enter,

        /// <summary>
        /// Specifies the keyboard interaction type as Escape down key.
        /// </summary>
        Escape,

        /// <summary>
        /// Specifies the keyboard interaction type as A key.
        /// </summary>
        Back,

        /// <summary>
        /// Specifies the keyboard interaction type as Space key.
        /// </summary>
        Space,

        /// <summary>
        /// Specifies the keyboard interaction type as Delete key.
        /// </summary>
        Delete,

        /// <summary>
        /// Specifies the keyboard interaction type as Print key.
        /// </summary>
        Print,

        /// <summary>
        /// Specifies the keyboard interaction type as Insert key.
        /// </summary>
        Insert,

        /// <summary>
        /// Specifies the keyboard interaction type as Help key.
        /// </summary>
        Help,

        /// <summary>
        /// Specifies the keyboard interaction type as Add key.
        /// </summary>
        /// <remarks>Specified as a numeric key value when pressing a key combination in the WinUI platform only.</remarks>
        Add,

        /// <summary>
        /// Specifies the keyboard interaction type as Substract key.
        /// </summary>
        /// <remarks>Specified as a numeric key value when pressing a key combination in the WinUI platform only.</remarks>
        Subtract,

        /// <summary>
        /// Specifies the keyboard interaction type as Multiply key.
        /// </summary>
        Multiply,

        /// <summary>
        /// Specifies the keyboard interaction type as Divide key.
        /// </summary>
        Divide,

        /// <summary>
        /// Specifies the keyboard interaction type as Equals key.
        /// </summary>
        Equals,

        /// <summary>
        /// Specifies the keyboard interaction type as Decimal key.
        /// </summary>
        Decimal,

        /// <summary>
        /// Specifies the keyboard interaction type as A key.
        /// </summary>
        A,

        /// <summary>
        /// Specifies the keyboard interaction type as B key.
        /// </summary>
        B,

        /// <summary>
        /// Specifies the keyboard interaction type as C key.
        /// </summary>
        C,

        /// <summary>
        /// Specifies the keyboard interaction type as D key.
        /// </summary>
        D,

        /// <summary>
        /// Specifies the keyboard interaction type as E key.
        /// </summary>
        E,

        /// <summary>
        /// Specifies the keyboard interaction type as F key.
        /// </summary>
        F,

        /// <summary>
        /// Specifies the keyboard interaction type as G key.
        /// </summary>
        G,

        /// <summary>
        /// Specifies the keyboard interaction type as H key.
        /// </summary>
        H,

        /// <summary>
        /// Specifies the keyboard interaction type as I key.
        /// </summary>
        I,

        /// <summary>
        /// Specifies the keyboard interaction type as J key.
        /// </summary>
        J,

        /// <summary>
        /// Specifies the keyboard interaction type as K key.
        /// </summary>
        K,

        /// <summary>
        /// Specifies the keyboard interaction type as L key.
        /// </summary>
        L,

        /// <summary>
        /// Specifies the keyboard interaction type as M key.
        /// </summary>
        M,

        /// <summary>
        /// Specifies the keyboard interaction type as N key.
        /// </summary>
        N,

        /// <summary>
        /// Specifies the keyboard interaction type as O key.
        /// </summary>
        O,

        /// <summary>
        /// Specifies the keyboard interaction type as P key.
        /// </summary>
        P,

        /// <summary>
        /// Specifies the keyboard interaction type as Q key.
        /// </summary>
        Q,

        /// <summary>
        /// Specifies the keyboard interaction type as R key.
        /// </summary>
        R,

        /// <summary>
        /// Specifies the keyboard interaction type as S key.
        /// </summary>
        S,

        /// <summary>
        /// Specifies the keyboard interaction type as T key.
        /// </summary>
        T,

        /// <summary>
        /// Specifies the keyboard interaction type as U key.
        /// </summary>
        U,

        /// <summary>
        /// Specifies the keyboard interaction type as V key.
        /// </summary>
        V,

        /// <summary>
        /// Specifies the keyboard interaction type as W key.
        /// </summary>
        W,

        /// <summary>
        /// Specifies the keyboard interaction type as X key.
        /// </summary>
        X,

        /// <summary>
        /// Specifies the keyboard interaction type as Y key.
        /// </summary>
        Y,

        /// <summary>
        /// Specifies the keyboard interaction type as Z key.
        /// </summary>
        Z,

        /// <summary>
        /// Specifies the keyboard interaction type as Caps key.
        /// </summary>
        CapsLock,

        /// <summary>
        /// Specifies the keyboard interaction type as NumLock key.
        /// </summary>
        NumLock,

        /// <summary>
        /// Specifies the keyboard interaction type as ScrollLock key.
        /// </summary>
        ScrollLock,

        /// <summary>
        /// Specifies the keyboard interaction type as 0 key.
        /// </summary>
        Num0,

        /// <summary>
        /// Specifies the keyboard interaction type as 1 key.
        /// </summary>
        Num1,

        /// <summary>
        /// Specifies the keyboard interaction type as 2 key.
        /// </summary>
        Num2,

        /// <summary>
        /// Specifies the keyboard interaction type as 3 key.
        /// </summary>
        Num3,

        /// <summary>
        /// Specifies the keyboard interaction type as 4 key.
        /// </summary>
        Num4,

        /// <summary>
        /// Specifies the keyboard interaction type as 5 key.
        /// </summary>
        Num5,

        /// <summary>
        /// Specifies the keyboard interaction type as 6 key.
        /// </summary>
        Num6,

        /// <summary>
        /// Specifies the keyboard interaction type as 7 key.
        /// </summary>
        Num7,

        /// <summary>
        /// Specifies the keyboard interaction type as 8 key.
        /// </summary>
        Num8,

        /// <summary>
        /// Specifies the keyboard interaction type as 9 key.
        /// </summary>
        Num9,

        /// <summary>
        /// Specifies the keyboard interaction type as F1 key.
        /// </summary>
        F1,

        /// <summary>
        /// Specifies the keyboard interaction type as F2 key.
        /// </summary>
        F2,

        /// <summary>
        /// Specifies the keyboard interaction type as F3 key.
        /// </summary>
        F3,

        /// <summary>
        /// Specifies the keyboard interaction type as F4 key.
        /// </summary>
        F4,

        /// <summary>
        /// Specifies the keyboard interaction type as F5 key.
        /// </summary>
        F5,

        /// <summary>
        /// Specifies the keyboard interaction type as F6 key.
        /// </summary>
        F6,

        /// <summary>
        /// Specifies the keyboard interaction type as F7 key.
        /// </summary>
        F7,

        /// <summary>
        /// Specifies the keyboard interaction type as F8 key.
        /// </summary>
        F8,

        /// <summary>
        /// Specifies the keyboard interaction type as F9 key.
        /// </summary>
        F9,

        /// <summary>
        /// Specifies the keyboard interaction type as F10 key.
        /// </summary>
        F10,

        /// <summary>
        /// Specifies the keyboard interaction type as F11 key.
        /// </summary>
        F11,

        /// <summary>
        /// Specifies the keyboard interaction type as F12 key.
        /// </summary>
        F12,

        /// <summary>
        /// Specifies the keyboard interaction type as F13 key.
        /// </summary>
        F13,

        /// <summary>
        /// Specifies the keyboard interaction type as F14 key.
        /// </summary>
        F14,

        /// <summary>
        /// Specifies the keyboard interaction type as F15 key.
        /// </summary>
        F15,

        /// <summary>
        /// Specifies the keyboard interaction type as F16 key.
        /// </summary>
        F16,

        /// <summary>
        /// Specifies the keyboard interaction type as F17 key.
        /// </summary>
        F17,

        /// <summary>
        /// Specifies the keyboard interaction type as F18 key.
        /// </summary>
        F18,

        /// <summary>
        /// Specifies the keyboard interaction type as F19 key.
        /// </summary>
        F19,

        /// <summary>
        /// Specifies the keyboard interaction type as F20 key.
        /// </summary>
        F20,

        /// <summary>
        /// Specifies the keyboard interaction type as F21 key.
        /// </summary>
        F21,

        /// <summary>
        /// Specifies the keyboard interaction type as F22 key.
        /// </summary>
        F22,

        /// <summary>
        /// Specifies the keyboard interaction type as F23 key.
        /// </summary>
        F23,

        /// <summary>
        /// Specifies the keyboard interaction type as F24 key.
        /// </summary>
        F24

    }
}
