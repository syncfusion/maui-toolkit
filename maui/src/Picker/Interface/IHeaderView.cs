namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Interface that holds the properties of Picker header view.
    /// </summary>
    internal interface IHeaderView : IPickerCommon
    {
        /// <summary>
        /// Gets the settings of the picker footer view.
        /// </summary>
        PickerHeaderView HeaderView { get; }

        /// <summary>
        /// Gets the settings of the header template.
        /// </summary>
        DataTemplate HeaderTemplate { get; }

        /// <summary>
        /// Method to update after the time button clicked.
        /// </summary>
        void OnTimeButtonClicked();

        /// <summary>
        /// Method to update after the date button clicked.
        /// </summary>
        void OnDateButtonClicked();
    }
}