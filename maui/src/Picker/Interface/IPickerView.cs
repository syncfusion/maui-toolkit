namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Interface that holds the properties of picker view.
    /// </summary>
    internal interface IPickerView
    {
        /// <summary>
        /// Gets the item height for the picker view.
        /// </summary>
        double ItemHeight { get; }

        /// <summary>
        /// Gets the selected text style for the picker item.
        /// </summary>
        PickerTextStyle SelectedTextStyle { get; }

        /// <summary>
        /// Gets the unselected text style for the picker item.
        /// </summary>
        PickerTextStyle TextStyle { get; }

        /// <summary>
        /// Gets the disabled text style for the picker item.
        /// </summary>
        PickerTextStyle DisabledTextStyle { get; }

        /// <summary>
        /// Gets the picker items text display mode.
        /// </summary>
        PickerTextDisplayMode TextDisplayMode { get; }

        /// <summary>
        /// Gets the item template for the picker item.
        /// </summary>
        DataTemplate ItemTemplate { get; }

        /// <summary>
        /// Gets a value indicating whether the parent value have valid value because dynamic scrolling on dialog opening does not scroll because the picker stack layout does not have a parent value.
        /// </summary>
        bool IsValidParent { get; }
    }
}