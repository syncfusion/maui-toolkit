namespace Syncfusion.Maui.Toolkit.Expander
{
	#region Enums

	/// <summary>
	/// Specifies the position of the expander icon within the <see cref="SfExpander"/> control.
	/// </summary>
	public enum ExpanderIconPosition
    {
        /// <summary>
        /// Specifies that the expander icon is positioned on the left side of the Expander control.
        /// </summary>
        Start,

        /// <summary>
        /// Specifies that the expander icon is positioned on the right side of the Expander control.
        /// </summary>
        End,

        /// <summary>
        /// Specifies that the expander icon will not be shown on the header.
        /// </summary>
        None,
    }

    /// <summary>
    /// Specifies the easing function for the animation of <see cref="SfExpander"/> control.
    /// </summary>
    public enum ExpanderAnimationEasing
    {
        /// <summary>
        /// This easing function will use a constant velocity to animate the view and is the default type.
        /// </summary>
        Linear,

        /// <summary>
        /// This easing function will smoothly accelerate the animation to its final value.
        /// </summary>
        SinIn,

        /// <summary>
        /// This easing function will smoothly accelerate the animation at the beginning and then smoothly decelerates the animation at the end.
        /// </summary>
        SinInOut,

        /// <summary>
        /// This easing function will smoothly decelerate the animation to its final value.
        /// </summary>
        SinOut,

        /// <summary>
        /// This easing function causes no animation to decelerate towards the final value.
        /// </summary>
        None,
    }

	#endregion
}