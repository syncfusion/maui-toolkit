namespace Syncfusion.Maui.Toolkit.Cards
{
    /// <summary>
    /// Holds the visible card index changed event args.
    /// </summary>
    public class CardVisibleIndexChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current swiping card index.
        /// </summary>
        public int? OldIndex { get; internal set; }

        /// <summary>
        /// Gets the next possible card index.
        /// </summary>
        public int? NewIndex { get; internal set; }
    }
}
