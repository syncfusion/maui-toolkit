namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Interface that holds the properties of Picker footer view.
    /// </summary>
    internal interface IFooterView : IPickerCommon
    {
        /// <summary>
        /// Gets the settings of the picker footer view.
        /// </summary>
        PickerFooterView FooterView { get; }

        /// <summary>
        /// Gets the settings of picker footer template.
        /// </summary>
        DataTemplate FooterTemplate { get; }

        /// <summary>
        /// Method to update after the confirm button clicked.
        /// </summary>
        void OnConfirmButtonClicked();

        /// <summary>
        /// Method to update after the cancel button clicked.
        /// </summary>
        void OnCancelButtonClicked();
    }
}