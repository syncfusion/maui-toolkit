namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Interface that holds the common properties of picker.
    /// </summary>
    internal interface IPickerCommon
    {
        /// <summary>
        /// Gets a value indicating whether the view is in RTL flow direction or not.
        /// </summary>
        bool IsRTLLayout { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the picker has to be in open or close.
        /// </summary>
        bool IsOpen { get; set; }
    }
}