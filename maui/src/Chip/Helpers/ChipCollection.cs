using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Chips
{
    /// <summary>
    /// Defines the type for adding collection of SfChip.
    /// </summary>
    public class ChipCollection : ObservableCollection<SfChip>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChipCollection"/> class.
        /// </summary>
        /// <param name="group">Chip group.</param>
        public ChipCollection(SfChipGroup group)
        {
            if (group != null)
            {
                CollectionChanged += group.ItemsCollectionChanged;
            }
        }

        #endregion
    }
}
