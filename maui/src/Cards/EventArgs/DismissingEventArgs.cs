using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Cards
{
    /// <summary>
    /// Holds the dismissing event args.
    /// </summary>
    public class CardDismissingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets a dismissed direction of the card view.
        /// </summary>
        public CardDismissDirection DismissDirection { get; internal set; }
    }
}