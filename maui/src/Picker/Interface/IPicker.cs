using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Interface that holds the properties of picker.
    /// </summary>
    internal interface IPicker : IPickerView, IColumnHeaderView, IPickerCommon, IFooterView, IHeaderView
    {
        /// <summary>
        /// Gets the picker columns details for the picker view.
        /// </summary>
        ObservableCollection<PickerColumn> Columns { get; }

        /// <summary>
        /// Gets the selection settings for the picker.
        /// </summary>
        PickerSelectionView SelectionView { get; }

        /// <summary>
        /// Gets the column divider color.
        /// </summary>
        Color ColumnDividerColor { get; }

		/// <summary>
		/// Method to update the picker tapped item index.
		/// </summary>
		/// <param name="tappedIndex">The tapped index.</param>
		/// <param name="childIndex">The column child index.</param>
		/// <param name="isTapped">Is tap gesture used</param>
		/// <param name="isInitialLoading">Check whether is initial loading.</param>
		void UpdateSelectedIndexValue(int tappedIndex, int childIndex, bool isTapped, bool isInitialLoading = false);
    }
}