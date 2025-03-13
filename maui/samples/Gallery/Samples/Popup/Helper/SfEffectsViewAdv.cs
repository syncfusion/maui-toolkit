using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.ControlsGallery.Popup.SfPopup
{
	internal class SfEffectsViewAdv : SfEffectsView, ITouchListener
	{

		#region Constructor

		public SfEffectsViewAdv()
		{

		}

		#endregion

		#region Methods
		public new void OnTouch(PointerEventArgs e)
		{
#if ANDROID
			{
				return;
			}
#else

			if (e.Action == PointerActions.Entered)
			{
				ApplyEffects(SfEffects.Highlight, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
			}
			else if (e.Action == PointerActions.Pressed)
			{
				ApplyEffects(SfEffects.Ripple, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
			}
			else if (e.Action == PointerActions.Released || e.Action == PointerActions.Cancelled || e.Action == PointerActions.Exited)
			{
				Reset();
			}
#endif
		}

		#endregion
	}
}
