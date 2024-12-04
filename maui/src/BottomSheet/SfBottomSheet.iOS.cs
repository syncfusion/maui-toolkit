using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.Platform;

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
                _layoutViewExt.UserInteractionEnabled = false;
            }
        }

        void UnwireEvents()
        {
            _layoutViewExt = null;
        }

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
