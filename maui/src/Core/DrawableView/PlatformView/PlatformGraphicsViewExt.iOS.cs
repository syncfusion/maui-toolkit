using System;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Platform
{
    /// <summary>
    /// Native view for <see cref="SfDrawableView"/>.
    /// </summary>
    public class PlatformGraphicsViewExt : PlatformGraphicsView
    {
        #region Fields

        WeakReference<View>? _mauiView;

        #endregion

        #region Properties

        View? MauiView
        {
            get => _mauiView != null && _mauiView.TryGetTarget(out var v) ? v : null;
            set => _mauiView = value == null ? null : new(value);
        }

        /// <summary>
        /// Returns a boolean value indicating whether this object can become the first responder.
        /// </summary>
        public override bool CanBecomeFirstResponder => (MauiView is IKeyboardListener) && (MauiView as IKeyboardListener)!.CanBecomeFirstResponder;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformGraphicsViewExt"/> class.
        /// </summary>
        /// <param name="drawable">Instance of virtual view.</param>
        public PlatformGraphicsViewExt(IDrawable? drawable = null)
        {
            MauiView = (View?)drawable;
            Drawable = new PlatformDrawableView((IDrawable?)MauiView);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Raised when a button is pressed.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the new presses that occurred.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.MauiView != null && !this.MauiView.HandleKeyPress(presses, evt))
            {
                base.PressesBegan(presses, evt);
            }
        }

        /// <summary>
        /// Raised when a button is released.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the buttons that the user is no longer pressing.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.MauiView != null && !this.MauiView.HandleKeyRelease(presses, evt))
            {
                base.PressesEnded(presses, evt);
            }
        }

        #endregion
    }
}
