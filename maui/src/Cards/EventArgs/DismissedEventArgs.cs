namespace Syncfusion.Maui.Toolkit.Cards
{
    /// <summary>
    /// Holds the dismissed event args.
    /// </summary>
    public class CardDismissedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a dismissed direction of the card view.
        /// </summary>
        public CardDismissDirection DismissDirection { get; internal set; }
    }
}