using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Popup
{
#if !ANDROID
    internal class SfPopupOverlayContainer : WindowOverlayContainer
#else
	internal class SfPopupOverlayContainer : WindowOverlayContainer, ITouchListener
#endif
	{
		#region Fields

		SfPopup _popup;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfPopupOverlayContainer"/> class.
		/// </summary>
		/// <param name="sfpopup">The instance of the SfPopup.</param>
		internal SfPopupOverlayContainer(SfPopup sfpopup)
		{
			_popup = sfpopup;
#if ANDROID
			this.AddTouchListener(this);
#endif
		}

		#endregion

		#region ITouchListener Callback
#if ANDROID
		/// <summary>
		/// This method will be called based on the view touch.
		/// </summary>
		/// <param name="e">The PanEventArgs object.</param>
		public void OnTouch(Toolkit.Internals.PointerEventArgs e)
		{
			if (e.Action is PointerActions.Pressed)
			{
				ClosePopupIfRequired(e.TouchPoint);
			}
		}
#endif
		#endregion

		#region Internal Methods
#if !ANDROID
		/// <summary>
		/// This method will be invoked when the touch interaction is made on the container.
		/// </summary>
		/// <param name="x">Value of X position.</param>
		/// <param name="y">Value of Y position.</param>
		internal override void ProcessTouchInteraction(float x, float y)
		{
			ClosePopupIfRequired(new Point(x, y));
		}
#endif
		/// <summary>
		/// Used to set background color for the container.
		/// </summary>
		/// <param name="overlayColor">The color to set as the background color for the PopupOverlayContainer.</param>
		internal void ApplyBackgroundColor(Brush overlayColor)
		{
			Background = overlayColor;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Used to check whether the touch is made outside popup or not.
		/// </summary>
		/// <param name="touchPoint">The touch point of the container view.</param>
		void ClosePopupIfRequired(Point touchPoint)
		{
			if (_popup.IsOpen && !_popup._isPopupAnimationInProgress && !_popup._isContainerAnimationInProgress)
			{
				// TODO: CanLayoutForGivenSizeAndPosition
				// Checking whether the container layout spans the full width and height.
				// Detecting whether a touch occurs inside or outside the PopupView.
				if (_popup.CanLayoutForGivenSizeAndPosition() && _popup._popupView is not null && !((touchPoint.Y > _popup._popupView.GetY())
				   && (touchPoint.Y < (_popup._popupView.GetY() + _popup._popupViewHeight))
				   && (touchPoint.X > _popup._popupView.GetX())
				   && (touchPoint.X < (_popup._popupView.GetX() + _popup._popupViewWidth))))
				{
					// To Close the Popup, if the touch is made outside the popup view.
					_popup.ClosePopupIfRequired();
				}
			}
		}

		#endregion
	}
}