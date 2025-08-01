namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Interface that holds the properties of column header view details.
    /// </summary>
    internal interface IColumnHeaderView
    {
        /// <summary>
        /// Gets the picker column header details for the picker view.
        /// </summary>
        PickerColumnHeaderView ColumnHeaderView { get; }

        /// <summary>
        /// Gets the picker column header template.
        /// </summary>
        DataTemplate ColumnHeaderTemplate { get; }
    }
}