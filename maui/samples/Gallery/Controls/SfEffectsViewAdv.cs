using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.ControlsGallery.CustomView
{
    internal class SfEffectsViewAdv : SfEffectsView, ITouchListener, IGestureListener
    {
        public SfEffectsViewAdv()
        {


        }
        public new void OnTouch(PointerEventArgs e)
        {
            if (e.Action == PointerActions.Entered)
            {
                this.ApplyEffects(SfEffects.Highlight, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
            }
            else if (e.Action == PointerActions.Released)
            {
                this.Reset();
            }
            else if (e.Action == PointerActions.Cancelled)
            {
                this.Reset();
            }
            else if (e.Action == PointerActions.Exited)
            {
                this.Reset();
            }
            else if (e.Action == PointerActions.Pressed)
            {
                this.ApplyEffects(SfEffects.Ripple, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
            }
        }

        internal void ForceRemoveEffects()
        {
            this.Reset();
        }
    }
}
