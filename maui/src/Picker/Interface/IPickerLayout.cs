namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Interface that holds the picker layout details.
    /// </summary>
    internal interface IPickerLayout
    {
        /// <summary>
        /// Gets the picker view details.
        /// </summary>
        IPickerView PickerInfo { get; }

        /// <summary>
        /// Gets the current column details.
        /// </summary>
        PickerColumn Column { get; }

        /// <summary>
        /// Gets the scroll offset value of the scroll view.
        /// </summary>
        double ScrollOffset { get; }

		/// <summary>
		/// Method to update the picker tapped item index.
		/// </summary>
		/// <param name="tappedIndex">The tapped index.</param>
		/// <param name="isTapped">Is tap gesture used</param>
		/// <param name="isInitialLoading">Check whether is initial loading.</param>
		void UpdateSelectedIndexValue(int tappedIndex, bool isTapped, bool isInitialLoading = false);
    }
}