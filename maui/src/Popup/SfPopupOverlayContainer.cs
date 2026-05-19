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
			if (this._popup.ShowOverlayAlways)
			{
				// The touch listener is added only when ShowOverlayAlways is true.
				// When ShowOverlayAlways is false, touch must pass through to the controls behind.
				// If we handle the touch here, it would prevent the touch from reaching the underlying controls.
				this.AddTouchListener(this);
			}
#endif
		}

		#endregion

		/// <summary>
		/// Gets a value indicating whether the native overlay container should handle touch
		/// interactions itself or allow touches to pass through to underlying views.
		/// </summary>
		/// <remarks>
		/// When this property returns <c>true</c>, the overlay will capture touch events
		/// (for example to detect taps on the overlay to close the popup). When it
		/// returns <c>false</c>, the overlay will allow touches to pass through so
		/// underlying controls can receive them. The value is driven by
		/// <see cref="SfPopup.ShowOverlayAlways"/> on the associated popup instance.
		/// </remarks>
		internal override bool canHandleTouch
		{
			get { return !this._popup.ShowOverlayAlways; }
		}

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

		/// <summary>
		/// This method will be invoked when the touch interaction is made on the container.
		/// </summary>
		/// <param name="x">Value of X position.</param>
		/// <param name="y">Value of Y position.</param>
		internal override void ProcessTouchInteraction(float x, float y)
		{
			ClosePopupIfRequired(new Point(x, y));
		}

		/// <summary>
		/// Used to set background color for the container.
		/// </summary>
		/// <param name="overlayColor">The color to set as the background color for the PopupOverlayContainer.</param>
		internal void ApplyBackgroundColor(Brush overlayColor)
		{
			Background = overlayColor;
		}

		/// <summary>
		/// Attempts to close the popup; the Closing event may cancel the action.
		/// </summary>
		internal override void ProcessBackButtonPressed()
		{
			if (_popup.IsOpen)
			{
				_popup.IsOpen = false;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Used to check whether the touch is made outside popup or not.
		/// </summary>
		/// <param name="touchPoint">The touch point of the container view.</param>
		void ClosePopupIfRequired(Point touchPoint)
		{
#if ANDROID
            // Only the topmost popup should respond to outside-touch events.
            if (!this._popup.ShowOverlayAlways && PopupExtension.TopMostOpenPopup != this._popup)
            {
                return;
            }
#endif

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