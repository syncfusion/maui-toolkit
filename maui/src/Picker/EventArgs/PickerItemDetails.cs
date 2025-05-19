namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which contains picker item info.
    /// </summary>
    public class PickerItemDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PickerItemDetails"/> class.
        /// </summary>
        /// <param name="data">Picker item details.</param>
        public PickerItemDetails(object data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets item value.
        /// </summary>
        public object Data { get; internal set; }
    }
}