namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// This class serves as an event data for the <see cref="LegendItemEventArgs"/> event. The event data holds the information about the legend item. 
    /// </summary>
    public class LegendItemEventArgs
    {
        /// <summary>
        /// Gets the corresponding legend item.
        /// </summary>
        public readonly ILegendItem LegendItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItemEventArgs"/> class.
        /// </summary>
        /// <param name="legendItem">Used to specifyt he legend item.</param>
        public LegendItemEventArgs(ILegendItem legendItem)
        {
            LegendItem = legendItem;
        }
    }
}
