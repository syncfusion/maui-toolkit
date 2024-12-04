namespace Syncfusion.Maui.Toolkit.NumericEntry
{
    /// <summary>
    /// Defines the behavior of value updating while typing in a text box during both the "Focus" and "Unfocus" states.
    /// </summary>
    public enum ValueChangeMode
    {
        /// <summary>
        /// Value will be updated while the text box is in focus.
        /// </summary>
        OnKeyFocus,

        /// <summary>
        /// Value will be updated when the text box loses focus.
        /// </summary>
        OnLostFocus
    }

	/// <summary>
	/// Enumeration representing the value states of the control.
	/// </summary>
	internal enum ValueStates
	{
		/// <summary>
		/// The control is in its normal state.
		/// </summary>
		Normal,

		/// <summary>
		/// The control is at its minimum value.
		/// </summary>
		Minimum,

		/// <summary>
		/// The control is at its maximum value.
		/// </summary>
		Maximum
	}
}
