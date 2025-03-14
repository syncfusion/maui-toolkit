namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Defines constant that specify how the PopupView is Sized.
	/// </summary>
	public enum PopupAutoSizeMode
	{
		/// <summary>
		/// PopupView's width will be automatically adjusted based on the contents of its template.
		/// </summary>
		Width,

		/// <summary>
		/// PopupView's height will be automatically adjusted based on the contents of its template.
		/// </summary>
		Height,

		/// <summary>
		/// PopupView's width and height will be automatically adjusted based on the contents of its template.
		/// </summary>
		Both,

		/// <summary>
		/// PopupView's size will not be determined by the contents of its template. This is the default value.
		/// </summary>
		None,
	}

	/// <summary>
	/// Built-in layout styles of the PopupView.
	/// </summary>
	public enum PopupButtonAppearanceMode
	{
		/// <summary>
		/// A layout with one button in the footer.
		/// </summary>
		OneButton,

		/// <summary>
		/// A layout with two buttons in the footer.
		/// </summary>
		TwoButton,
	}

	/// <summary>
	/// Defines constants that specifies the intensity of the blur effect applied to the overlay.
	/// </summary>
	public enum PopupBlurIntensity
	{
		/// <summary>
		/// Specifies that a light blur effect will be used for the overlay.
		/// </summary>
		Light,

		/// <summary>
		/// Specifies that a very light blur effect will be used for the overlay.
		/// </summary>
		ExtraLight,

		/// <summary>
		/// Specifies a very dark blur effect intensity for the overlay.
		/// </summary>
		ExtraDark,

		/// <summary>
		/// Specifies a dark blur effect intensity for the overlay.
		/// </summary>
		Dark,

		/// <summary>
		/// Specifies that the overlay’s blur effect intensity will be a custom float value set in <see cref="PopupStyle.BlurRadius"/>.
		/// </summary>
		Custom,
	}

	/// <summary>
	/// Defines constants that specifies whether the overlay should be transparent or blurred.
	/// </summary>
	public enum PopupOverlayMode
	{
		/// <summary>
		/// Specifies that the overlay will be transparent with opacity applied based on value of <see cref="PopupStyle.OverlayColor"/> alpha.
		/// </summary>
		Transparent,

		/// <summary>
		/// Specifies that the overlay will be blurred based on the intensity of <see cref="PopupStyle.BlurIntensity"/>.
		/// </summary>
		Blur,
	}

	/// <summary>
	/// Positions the popup view relative to the given view.
	/// </summary>
	public enum PopupRelativePosition
	{
		/// <summary>
		/// Displays the popup at the top of the given view.
		/// </summary>
		AlignTop,

		/// <summary>
		/// Displays the popup to the left of the given view.
		/// </summary>
		AlignToLeftOf,

		/// <summary>
		/// Displays the popup to the right of the given view.
		/// </summary>
		AlignToRightOf,

		/// <summary>
		/// Displays the popup at the bottom of the given view.
		/// </summary>
		AlignBottom,

		/// <summary>
		/// Displays the popup at the top left position of the given view.
		/// </summary>
		AlignTopLeft,

		/// <summary>
		/// Displays the popup at the top right position of the given view.
		/// </summary>
		AlignTopRight,

		/// <summary>
		/// Displays the popup at the bottom left position of the given view.
		/// </summary>
		AlignBottomLeft,

		/// <summary>
		/// Displays the popup at the bottom right position of the given view.
		/// </summary>
		AlignBottomRight,
	}

	/// <summary>
	/// Defines constants that specifies the built-in animation easing effects available in <see cref="SfPopup"/> when opening and closing the popup.
	/// </summary>
	public enum PopupAnimationEasing
	{
		/// <summary>
		/// This easing function will smoothly accelerate the animation to its final value.
		/// </summary>
		SinIn,

		/// <summary>
		///  This easing function will smoothly decelerate the animation to its final value.
		/// </summary>
		SinOut,

		/// <summary>
		/// This easing function will smoothly accelerate the animation at the beginning and then smoothly decelerates the animation at the end.
		/// </summary>
		SinInOut,

		/// <summary>
		/// This easing function will use a constant velocity to animate the view and is the default type.
		/// </summary>
		Linear,
	}

	/// <summary>
	/// Built-in animations available in <see cref="SfPopup"/>, which is applied when the
	/// PopupView opens and closes in the screen.
	/// </summary>
	public enum PopupAnimationMode
	{
		/// <summary>
		/// zoom-out animation will be applied when the PopupView opens and zoom-in animation
		/// will be applied when the PopupView closes.
		/// </summary>
		Zoom,

		/// <summary>
		/// Fade-out animation will be applied when the PopupView opens and Fade-in animation
		/// will be applied when the PopupView closes.
		/// </summary>
		Fade,

		/// <summary>
		/// PopupView will be animated from left-to-right, when it opens and it will be animated
		/// from right-to-left when the PopupView closes.
		/// </summary>
		SlideOnLeft,

		/// <summary>
		/// PopupView will be animated from right-to-left, when it opens and it will be animated
		/// from left-to-right when the PopupView closes.
		/// </summary>
		SlideOnRight,

		/// <summary>
		/// PopupView will be animated from top-to-bottom, when it opens and it will be animated
		/// from bottom-to-top when the PopupView closes.
		/// </summary>
		SlideOnTop,

		/// <summary>
		/// PopupView will be animated from bottom-to-top, when it opens and it will be animated
		/// from top-to-bottom when the PopupView closes.
		/// </summary>
		SlideOnBottom,

		/// <summary>
		/// Animation will not be applied.
		/// </summary>
		None,
	}
}