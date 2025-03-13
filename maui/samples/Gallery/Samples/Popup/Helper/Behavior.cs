using Syncfusion.Maui.ControlsGallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.ControlsGallery.Popup.SfPopup
{
    public class SampleViewBehavior : Behavior<SampleView>
    {
		#region Private Fields

		Syncfusion.Maui.Toolkit.Popup.SfPopup? _actionSheetPopup;

		#endregion

		#region Methods

		internal static Size GetScaledScreenSize(DisplayInfo info)
		{
			return new Size(info.Width / info.Density, info.Height / info.Density);
		}
		protected override void OnAttachedTo(SampleView sampleView)
		{
			_actionSheetPopup = sampleView.Resources["Actionsheet"] as Syncfusion.Maui.Toolkit.Popup.SfPopup;

			sampleView.SizeChanged += SampleView_SizeChanged;

			base.OnAttachedTo(sampleView);
		}

		protected override void OnDetachingFrom(SampleView sampleView)
		{
			// Unsubscribe from SizeChanged
			sampleView.SizeChanged -= SampleView_SizeChanged;
			_actionSheetPopup = null;
		}

		void SampleView_SizeChanged(object? sender, EventArgs e)
		{
			if (sender is SampleView sampleView)
			{
				var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
				var screenWidth = GetScaledScreenSize(displayInfo).Width;
				var screenHeight = GetScaledScreenSize(displayInfo).Height;

#if ANDROID || IOS
				if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
				{
					_actionSheetPopup!.WidthRequest = screenWidth;
				}
				else
				{
					_actionSheetPopup!.WidthRequest = 360;
				}

				_actionSheetPopup!.StartY = (int)(screenHeight - _actionSheetPopup!.HeightRequest);
#else
				_actionSheetPopup!.WidthRequest = 360;
#endif
#if IOS || MACCATALYST
				_actionSheetPopup.Refresh();
#endif
			}
		}
	}

	#endregion
}
