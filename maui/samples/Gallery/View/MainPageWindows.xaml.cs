using Syncfusion.Maui.ControlsGallery.CustomView;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Themes;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MainPageWindows : ContentPage
	{

		int _columnCount;
		SampleCategoryModel? _sampleCategory;
		SampleView? _loadedSample;
		SampleSubCategoryModel? _selectedSampleSubCategoryModel;
		CardLayoutModel? _selectedCardLayoutModel;
		SampleModel? _loadedSampleModel;
		Uri? _uri;
		bool _isSortPage;
		double _tempWidth;
		int _sortColumnCount;
		int _sortColumnCountNew;
		bool _isFilterPage;
		bool _loopExit;
		bool _isThemePopupOpen;
		bool _isAllListAdded;
		bool _programmaticUpdate;
		readonly Dictionary<SampleModel, ControlModel> _filteredCollection = [];

		List<ControlModel>? _sortColumnOneCollection;
		List<ControlModel>? _sortColumnTwoCollection;
		List<ControlModel>? _sortColumnThreeCollection;

		ObservableCollection<SearchModel>? _filterColumnOneCollectionNew;
		ObservableCollection<SearchModel>? _filterColumnTwoCollectionNew;
		ObservableCollection<SearchModel>? _filterColumnOneCollectionUpdated;
		ObservableCollection<SearchModel>? _filterColumnTwoCollectionUpdated;

		/// <summary>
		/// Initializes the UI state, and configures the theme toggle based on the current application theme.
		/// </summary>
		public MainPageWindows()
		{
			InitializeComponent();
			allSamples.IsChecked = true;

			if (Application.Current != null)
			{
				AppTheme currentTheme = Application.Current.RequestedTheme;
				themePopupSwitch.IsToggled = (currentTheme == AppTheme.Dark);
			}
		}

		/// <summary>
		/// Determines the appropriate column layout based on the new width.
		/// Updates the layout for sorted, filtered, or regular views accordingly.
		/// </summary>
		/// <param name="width">The allocated width.</param>
		/// <param name="height">The allocated height.</param>
		protected async override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			await Task.Delay(10);
			_tempWidth = width;
			MainThread.BeginInvokeOnMainThread(() =>
			{
				if (_isSortPage)
				{
					UpdateAllSortedColumn(width, false, false, sortedGrid, sortedcolumOneLayout, sortedcolumTwoLayout, sortedcolumThreeLayout);
				}
				else if (_isFilterPage)
				{
					UpdateAllSortedColumn(width, false, true, filteredGridNewSample, filteredColumnOneLayoutNew, filteredColumnTwoLayoutNew, filteredColumnThreeLayoutNew);
					UpdateAllSortedColumn(width, false, false, filteredGridUpdatedSample, filteredColumnOneLayoutUpdated, filteredColumnTwoLayoutUpdated, filteredColumnThreeLayoutUpdated);
				}
				else
				{
					UpdateColumn(width);
				}
			});

		}

		/// <summary>
		/// Updates the column layout based on the current width.
		/// Hides and shows the grid during the update to prevent visual artifacts.
		/// </summary>
		/// <param name="width">The current width of the page.</param>
		void UpdateColumn(double width)
		{
			// Determine the new column count based on the width
			int newColumnCount = (width > 1200) ? 3 : 2;

			// If the column count hasn't changed, no need to update
			if (_columnCount == newColumnCount)
			{
				return;
			}

			// Set the new column count
			_columnCount = newColumnCount;

			// Hide the grid before making changes
			pageGrid.IsVisible = false;

			// Clear and reconfigure the column definitions
			UpdateGridColumns(newColumnCount);

			// Arrange controls based on the column count
			ArrangeControlOnColumn(newColumnCount == 2);

			// Show the grid after updating
			pageGrid.IsVisible = true;
		}

		/// <summary>
		/// Updates the grid columns based on the specified column count.
		/// Ensures the grid has the correct number of columns for the current layout.
		/// </summary>
		/// <param name="columnCount">The number of columns to create.</param>
		void UpdateGridColumns(int columnCount)
		{
			controlListGrid.ColumnDefinitions.Clear();
			for (int i = 0; i < columnCount; i++)
			{
				controlListGrid.ColumnDefinitions.Add(new ColumnDefinition());
			}
		}

		/// <summary>
		/// Updates the layout of sorted or filtered columns based on width and sample type.
		/// Checks if an update is needed, then proceeds to update the grid and arrange controls.
		/// </summary>
		/// <param name="width">The current width of the page.</param>
		/// <param name="isSortedChanged">Indicates if sorting has changed.</param>
		/// <param name="isNewSample">Indicates if it's a new sample.</param>
		/// <param name="sortAndFilterGrid">The grid to update.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void UpdateAllSortedColumn(double width, bool isSortedChanged, bool isNewSample, Grid sortAndFilterGrid, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			// Determine the new column count based on width
			int newColumnCount = (width > 1200) ? 3 : 2;

			// Check if sorting needs to be updated
			if (!NeedsColumnUpdate(newColumnCount, isSortedChanged, isNewSample))
			{
				return;
			}

			// Set the appropriate column count
			UpdateSortColumnCount(newColumnCount, isNewSample);

			// Hide the grid while making changes
			pageGrid.IsVisible = false;

			// Clear and update the column definitions based on the current page
			UpdateGridColumns(sortAndFilterGrid, newColumnCount);

			// Arrange controls based on the column layout
			ArrangeSortedControlOnColumn(newColumnCount == 2, isNewSample, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);

			// Show the grid after updating
			pageGrid.IsVisible = true;
		}

		/// <summary>
		/// Determines if a column update is needed based on the current state.
		/// </summary>
		/// <param name="newColumnCount">The new column count.</param>
		/// <param name="isSortedChanged">Indicates if sorting has changed.</param>
		/// <param name="isNewSample">Indicates if it's a new sample.</param>
		/// <returns>True if an update is needed, false otherwise.</returns>
		bool NeedsColumnUpdate(int newColumnCount, bool isSortedChanged, bool isNewSample)
		{
			return isNewSample ? (_sortColumnCountNew != newColumnCount || isSortedChanged) : (_sortColumnCount != newColumnCount || isSortedChanged);
		}

		/// <summary>
		/// Updates the sort column count for either new or updated samples. 
		/// Sets the appropriate count variable based on the isNewSample parameter. 
		/// </summary>
		/// <param name="newColumnCount">The new column count.</param>
		/// <param name="isNewSample">Indicates if it's a new sample.</param>
		void UpdateSortColumnCount(int newColumnCount, bool isNewSample)
		{
			if (isNewSample)
			{
				_sortColumnCountNew = newColumnCount;
			}
			else
			{
				_sortColumnCount = newColumnCount;
			}
		}

		/// <summary>
		/// Clears existing column definitions from the provided grid. 
		/// Adds new column definitions based on the page type and column count. Limits the number of columns for filtered views.
		/// </summary>
		/// <param name="grid">The grid to update.</param>
		/// <param name="columnCount">The number of columns to create.</param>
		void UpdateGridColumns(Grid grid, int columnCount)
		{
			grid.ColumnDefinitions.Clear();
			// Add column definitions based on page type
			int columnsToAdd = _isSortPage ? columnCount : Math.Min(columnCount, 2); // Filters use only up to 2 columns
			for (int i = 0; i < columnsToAdd; i++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition());
			}
		}

		/// <summary>
		/// Arranges controls dynamically based on the specified column count. 
		/// Determines the number of columns (2 or 3) based on the isTwoColumn parameter.
		/// </summary>
		/// <param name="isTwoColumn">Indicates if the layout should use two columns.</param>
		internal void ArrangeControlOnColumn(bool isTwoColumn)
		{
			if (BindingContext is SamplesViewModel viewModel)
			{
				int columnCount = isTwoColumn ? 2 : 3;
				ArrangeControl(viewModel, columnCount);
			}
		}

		/// <summary>
		/// Distributes control categories evenly across columns. Manages the visibility of the third column based on the column count. 
		/// Clears previous bindings and sets new item sources for each column layout.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="columnCount">The number of columns to use.</param>
		void ArrangeControl(SamplesViewModel viewModel, int columnCount)
		{
			// Initialize collections for each column
			var columnCollections = new List<List<ControlCategoryModel>>();
			for (int i = 0; i < columnCount; i++)
			{
				columnCollections.Add([]);
			}

			// Track item counts in each column
			var columnItemCounts = new int[columnCount];

			// Distribute items across columns
			foreach (var item in viewModel.AllControlCategories)
			{
				int minIndex = Array.IndexOf(columnItemCounts, columnItemCounts.Min());
				if (item.AllControls != null)
				{
					columnItemCounts[minIndex] += item.AllControls.Count + 1;
				}

				columnCollections[minIndex].Add(item);
			}

			// Handle visibility of the third column
			columThreeLayout.IsVisible = (columnCount == 3);

			// Clear previous binding before setting new data
			ClearLayouts();

			// Bind data to each column layout
			BindableLayout.SetItemsSource(columOneLayout, columnCollections[0]);
			BindableLayout.SetItemsSource(columTwoLayout, columnCollections[1]);
			if (columnCount == 3)
			{
				BindableLayout.SetItemsSource(columThreeLayout, columnCollections[2]);
			}
		}

		/// <summary>
		/// Clears existing bindings from all column layouts. Sets the ItemsSource of columOneLayout, columTwoLayout, and columThreeLayout to null. 
		/// </summary>
		void ClearLayouts()
		{
			BindableLayout.SetItemsSource(columOneLayout, null);
			BindableLayout.SetItemsSource(columTwoLayout, null);
			BindableLayout.SetItemsSource(columThreeLayout, null);
		}

		/// <summary>
		/// Arranges items for the sort page by dividing them evenly between two columns. Populates _sortColumnOneCollection and _sortColumnTwoCollection with sorted items. 
		/// Updates the visibility and item sources of the vertical layouts accordingly.
		/// </summary>
		/// <param name="isTwoColumn">Indicates if the layout should use two columns.</param>
		/// <param name="isNewSample">Indicates if it's a new sample.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void ArrangeSortedControlOnColumn(bool isTwoColumn, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			if (BindingContext is SamplesViewModel viewModel)
			{
				if (isTwoColumn)
				{
					ArrangeSortedControlInTwoColumn(viewModel, isNewSample, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
				}
				else
				{
					ArrangeSortedControlInThreeColumn(viewModel, isNewSample, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
				}
			}
		}

		/// <summary>
		/// Arranges sorted controls in two columns.Determines whether to arrange for the sort page or filter page.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="isNewSample">Indicates if it's a new sample.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void ArrangeSortedControlInTwoColumn(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			if (_isSortPage)
			{
				ArrangeSortPage(viewModel, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}
			else
			{
				ArrangeFilterPage(viewModel, isNewSample, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}
		}

		/// <summary>
		/// Arranges items for the sort page by dividing them evenly between two columns.
		/// Populates the first two vertical layouts with sorted items.
		/// </summary>
		/// <param name="viewModel">The samples view model containing the sorted list.</param>
		/// <param name="verticalOneLayout">The first vertical layout to populate with items.</param>
		/// <param name="verticalTwoLayout">The second vertical layout to populate with items.</param>
		/// <param name="verticalThreeLayout">The third vertical layout, which is hidden in this arrangement.</param>
		void ArrangeSortPage(SamplesViewModel viewModel, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			int columnTwoCount = 0;
			_sortColumnOneCollection = [];
			_sortColumnTwoCollection = [];

			int columnOneCount = viewModel.UpdatedSortedList.Count / 2 + viewModel.UpdatedSortedList.Count % 2;
			foreach (var item in viewModel.UpdatedSortedList)
			{
				if (columnTwoCount < columnOneCount)
				{
					columnTwoCount += 1;
					_sortColumnOneCollection.Add(item);
				}
				else
				{
					_sortColumnTwoCollection.Add(item);
				}
			}

			sortedcolumThreeLayout.IsVisible = false;
			BindableLayout.SetItemsSource(verticalThreeLayout, null);
			BindableLayout.SetItemsSource(verticalOneLayout, _sortColumnOneCollection);
			BindableLayout.SetItemsSource(verticalTwoLayout, _sortColumnTwoCollection);
		}

		/// <summary>
		/// Arranges items for the filter page based on whether they are new or updated samples.
		/// </summary>
		/// <param name="viewModel">The samples view model containing the data to be filtered.</param>
		/// <param name="isNewSample">Indicates if the arrangement is for new samples (true) or updated samples (false).</param>
		/// <param name="verticalOneLayout">The first vertical layout to populate with items.</param>
		/// <param name="verticalTwoLayout">The second vertical layout to populate with items.</param>
		/// <param name="verticalThreeLayout">The third vertical layout, used in some scenarios.</param>
		void ArrangeFilterPage(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			if (newSamples.IsChecked == true && isNewSample)
			{
				ArrangeNewSamples(viewModel, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}

			if (updatedSamples.IsChecked == true && !isNewSample)
			{
				ArrangeUpdatedSamples(viewModel, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}
		}

		/// <summary>
		/// Arranges new samples in the first vertical layout.
		/// Clears existing item sources and populates _filterColumnOneCollectionNew with new samples.
		/// </summary>
		/// <param name="viewModel">The samples view model containing the sorted list.</param>
		/// <param name="verticalOneLayout">The vertical layout to populate with new samples.</param>
		/// <param name="verticalTwoLayout">The second vertical layout, which is cleared in this arrangement.</param>
		/// <param name="verticalThreeLayout">The third vertical layout, which is cleared in this arrangement.</param>
		void ArrangeNewSamples(SamplesViewModel viewModel, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			_filterColumnOneCollectionNew = [];
			MainPageWindows.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			BindableLayout.SetItemsSource(verticalOneLayout, _filterColumnOneCollectionNew);
			_loopExit = false;
			int index = 0;
			_isAllListAdded = false;

			_ = viewModel.FilterNewSampleCount;
			foreach (var item in viewModel.SortedList)
			{
				if (item.Control != null && item.Control.StatusTag == "New")
				{
					if (_loopExit)
					{
						return;
					}

					_filterColumnOneCollectionNew.Add(item!);
					index++;
				}
			}

			_isAllListAdded = true;
			filteredColumnThreeLayoutNew.IsVisible = false;
			filteredColumnTwoLayoutNew.IsVisible = false;
		}

		/// <summary>
		/// Arranges updated samples in the first vertical layout.
		/// Clears existing item sources and populates _filterColumnOneCollectionUpdated with updated samples. 
		/// </summary>
		/// <param name="viewModel">The samples view model containing the sorted list.</param>
		/// <param name="verticalOneLayout">The vertical layout to populate with updated samples.</param>
		/// <param name="verticalTwoLayout">The second vertical layout, which is hidden in this arrangement.</param>
		/// <param name="verticalThreeLayout">The third vertical layout, which is hidden in this arrangement.</param>
		void ArrangeUpdatedSamples(SamplesViewModel viewModel, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			_filterColumnOneCollectionUpdated = [];
			MainPageWindows.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			BindableLayout.SetItemsSource(verticalOneLayout, _filterColumnOneCollectionUpdated);
			_loopExit = false;
			int index = 0;
			_isAllListAdded = false;
			foreach (var item in viewModel.SortedList)
			{
				if (item.Control != null && item.Control.StatusTag == "Updated")
				{
					if (_loopExit)
					{
						return;
					}

					_filterColumnOneCollectionUpdated.Add(item!);
					index++;
				}
			}

			_isAllListAdded = true;
			filteredColumnTwoLayoutUpdated.IsVisible = false;
			filteredColumnThreeLayoutUpdated.IsVisible = false;
		}

		/// <summary>
		/// Arranges sorted controls in three columns based on whether it's a sort page or a filter page.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="isNewSample">Indicates if it's a new sample.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void ArrangeSortedControlInThreeColumn(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			if (_isSortPage)
			{
				ArrangeSortPageInThreeColumns(viewModel, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}
			else
			{
				ArrangeFilterPageInThreeColumns(viewModel, isNewSample, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}
		}

		/// <summary>
		/// Arranges the sort page items in three columns. Calculates the distribution of items across the three columns. 
		/// Populates the three column collections and updates the layout bindings.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void ArrangeSortPageInThreeColumns(SamplesViewModel viewModel, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			int columnOneCount;
			int columnTwoCount;

			_sortColumnOneCollection = [];
			_sortColumnTwoCollection = [];
			_sortColumnThreeCollection = [];

			int totalCount = viewModel.UpdatedSortedList.Count;
			if (totalCount % 3 == 0)
			{
				columnOneCount = columnTwoCount = totalCount / 3;
			}
			else if (totalCount % 3 == 1)
			{
				columnOneCount = totalCount / 3 + 1;
				columnTwoCount = totalCount / 3;
			}
			else
			{
				columnOneCount = columnTwoCount = totalCount / 3 + 1;
			}

			DistributeItemsInThreeColumns(viewModel.UpdatedSortedList, columnOneCount, columnTwoCount);

			sortedcolumThreeLayout.IsVisible = true;
			MainPageWindows.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			BindableLayout.SetItemsSource(verticalOneLayout, _sortColumnOneCollection);
			BindableLayout.SetItemsSource(verticalTwoLayout, _sortColumnTwoCollection);
			BindableLayout.SetItemsSource(verticalThreeLayout, _sortColumnThreeCollection);
		}

		/// <summary>
		/// DDistributes items into three columns based on the calculated column counts. Populates _sortColumnOneCollection, _sortColumnTwoCollection, and _sortColumnThreeCollection. 
		/// Ensures even distribution of items across the three columns.
		/// </summary>
		/// <param name="items">The list of items to distribute.</param>
		/// <param name="columnOneCount">The number of items for the first column.</param>
		/// <param name="columnTwoCount">The number of items for the second column.</param>
		void DistributeItemsInThreeColumns(IEnumerable<object> items, int columnOneCount, int columnTwoCount)
		{
			int columnOneTempCount = 0;
			int columnTwoTempCount = 0;

			foreach (var item in items)
			{
				if (columnOneTempCount < columnOneCount)
				{
					columnOneTempCount += 1;
					_sortColumnOneCollection?.Add((ControlModel)item);
				}
				else if (columnTwoTempCount < columnTwoCount)
				{
					columnTwoTempCount += 1;
					_sortColumnTwoCollection?.Add((ControlModel)item);
				}
				else
				{
					_sortColumnThreeCollection?.Add((ControlModel)item);
				}
			}
		}

		/// <summary>
		/// Determines whether to arrange new samples or updated samples based on the current selection.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="isNewSample">Indicates if it's a new sample.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void ArrangeFilterPageInThreeColumns(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			if (newSamples.IsChecked == true && isNewSample)
			{
				ArrangeNewSamplesInThreeColumns(viewModel, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}
			if (updatedSamples.IsChecked == true && !isNewSample)
			{
				ArrangeUpdatedSamplesInThreeColumns(viewModel, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			}
		}

		/// <summary>
		/// Arranges new samples in three columns. Updates the visibility and bindings of the filtered grid layouts.
		/// Populates _filterColumnOneCollectionNew and _filterColumnTwoCollectionNew with new samples.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void ArrangeNewSamplesInThreeColumns(SamplesViewModel viewModel, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
		     int columnOneCount;
			 int columnTwoCount = 0;
			_filterColumnOneCollectionNew = [];
			_filterColumnTwoCollectionNew = [];
			MainPageWindows.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			BindableLayout.SetItemsSource(verticalOneLayout, _filterColumnOneCollectionNew);
			BindableLayout.SetItemsSource(verticalTwoLayout, _filterColumnTwoCollectionNew);
			columnOneCount = viewModel.FilterNewSampleCount / 2 + viewModel.FilterNewSampleCount % 2;
			_loopExit = false;
			int index = 0;
			_isAllListAdded = false;
			foreach (var item in viewModel.SortedList)
			{
				if (item.Control != null && item.Control.StatusTag == "New")
				{
					if (_loopExit)
					{
						return;
					}

					if (columnTwoCount < columnOneCount)
					{
						columnTwoCount += 1;
						_filterColumnOneCollectionNew.Add(item!);
					}
					else
					{
						_filterColumnTwoCollectionNew.Add(item);
					}

					index++;
				}
			}

			_isAllListAdded = true;
			filteredGridNewSample.IsVisible = true;
			filteredColumnTwoLayoutNew.IsVisible = true;
			filteredColumnThreeLayoutNew.IsVisible = false;
		}

		/// <summary>
		/// Arranges updated samples in three columns. 
		/// Populates _filterColumnOneCollectionUpdated and _filterColumnTwoCollectionUpdated with updated samples.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		void ArrangeUpdatedSamplesInThreeColumns(SamplesViewModel viewModel, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			int columnOneCount;
			 int columnTwoCount = 0;
			_filterColumnOneCollectionUpdated = [];
			_filterColumnTwoCollectionUpdated = [];
			MainPageWindows.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
			BindableLayout.SetItemsSource(verticalOneLayout, _filterColumnOneCollectionUpdated);
			BindableLayout.SetItemsSource(verticalTwoLayout, _filterColumnTwoCollectionUpdated);
			columnOneCount = viewModel.FilterUpdatedSampleCount / 2 + viewModel.FilterUpdatedSampleCount % 2;
			_loopExit = false;
			int index = 0;
			_isAllListAdded = false;
			foreach (var item in viewModel.SortedList)
			{
				if (item.Control != null && item.Control.StatusTag == "Updated")
				{
					if (_loopExit)
					{
						return;
					}

					if (columnTwoCount < columnOneCount)
					{
						columnTwoCount += 1;
						_filterColumnOneCollectionUpdated.Add(item!);
					}
					else
					{
						_filterColumnTwoCollectionUpdated.Add(item);
					}

					index++;
				}
			}

			_isAllListAdded = true;
			filteredGridUpdatedSample.IsVisible = true;
			filteredColumnTwoLayoutUpdated.IsVisible = true;
			filteredColumnThreeLayoutUpdated.IsVisible = false;
		}

		/// <summary>
		/// Clears the item source for the specified vertical layouts.
		/// </summary>
		/// <param name="verticalOneLayout">The first vertical layout.</param>
		/// <param name="verticalTwoLayout">The second vertical layout.</param>
		/// <param name="verticalThreeLayout">The third vertical layout.</param>
		static void ClearItemsSource(VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			BindableLayout.SetItemsSource(verticalOneLayout, null);
			BindableLayout.SetItemsSource(verticalTwoLayout, null);
			BindableLayout.SetItemsSource(verticalThreeLayout, null);
		}

		/// <summary>
		/// Handles the tapped event for a control, initiating the loading of new sample categories.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		async void Control_Tapped(object sender, EventArgs e)
		{
			busyIndicatorMainPage.IsVisible = true;
			await Task.Delay(100);
			if (sender is SfEffectsViewAdv { BindingContext: ControlModel controlObjectModel })
			{
				LoadSamplePage(controlObjectModel);
				MainPageWindows.ExpandSubCategory(controlObjectModel);
				UpdateSelectionUIToFirstItem(controlObjectModel.SampleCategories?[0]);
			}

			busyIndicatorMainPage.IsVisible = false;
			busyIndicatorPage.IsVisible = false;
		}

		/// <summary>
		/// Expands the subcategories of a control model.
		/// Sets the visibility of status tags for categories
		/// </summary>
		/// <param name="controlObjectModel">The control model to expand.</param>
		static void ExpandSubCategory(ControlModel controlObjectModel)
		{
			foreach (var item in controlObjectModel.SampleCategories!)
			{
				if (item.HasCategory)
				{
					item.CollapseImage = "\ue70b";
					item.IsCollapsed = true;
					item.CategoryStatusTag = false;
				}
				else
				{
					item.CategoryStatusTag = !string.IsNullOrEmpty(item.StatusTag);
				}
			}
		}

		/// <summary>
		/// Updates the selection UI to the first item in the provided sample category model.
		/// Sets the appropriate selection states and updates the UI accordingly.
		/// </summary>
		/// <param name="sampleCategoryModel">The sample category model to update.</param>
		void UpdateSelectionUIToFirstItem(SampleCategoryModel? sampleCategoryModel)
		{
			if (sampleCategoryModel == null)
			{
				return;
			}

			if (sampleCategoryModel.HasCategory)
			{
				sampleCategoryModel.IsCollapsed = true;
				if (sampleCategoryModel?.SampleSubCategories?[0] != null)
				{
					sampleCategoryModel.SelectedCategory = sampleCategoryModel.SampleSubCategories[0];
					_selectedSampleSubCategoryModel = sampleCategoryModel.SampleSubCategories[0];
					_selectedSampleSubCategoryModel.IsSubCategoryClicked = true;
				}
			}
			else
			{
				_sampleCategory = sampleCategoryModel;
				_sampleCategory.IsSelected = true;
			}
		}

		/// <summary>
		/// Loads a sample page for a given control model and optional sample model.
		/// </summary>
		/// <param name="controlModel">The control model to load.</param>
		/// <param name="sampleModel">The optional sample model to load.</param>
		void LoadSamplePage(ControlModel controlModel, SampleModel? sampleModel = null)
		{
			LoadSamplePage(controlModel, null, sampleModel);
		}

		/// <summary>
		/// Loads a sample page for a specific control and subcategory.
		/// Updates the binding context and visibility of relevant UI elements.
		/// </summary>
		/// <param name="controlModel">The control model to load.</param>
		/// <param name="subCategoryModel">The subcategory model to load.</param>
		/// <param name="sampleModel">The optional sample model to load.</param>
		void LoadSamplePage(ControlModel controlModel, SampleSubCategoryModel? subCategoryModel, SampleModel? sampleModel = null)
		{
			sampleViewPage.BindingContext = controlModel;
			controlListPage.IsVisible = false;
			sampleViewPage.IsVisible = true;
			if (subCategoryModel == null)
			{
				UpdateChipViewBindingContext(controlModel.SampleCategories![0].SampleSubCategories![0], sampleModel);
			}
			else
			{
				UpdateChipViewBindingContext(subCategoryModel, sampleModel);
			}
		}

		/// <summary>
		/// Handles the text changed event for the search entry.
		/// Shows or hides the search list grid based on the input length.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > 1 && !searchListGrid.IsVisible)
			{
				searchListGrid.IsVisible = true;
				searchListGrid.ZIndex = 1;
			}
			else if (e.NewTextValue.Length <= 1)
			{
				searchListGrid.IsVisible = false;
			}
		}

		/// <summary>
		/// Handles the unfocused event for the search entry.
		/// Hides the search list grid when the search entry loses focus.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void Entry_Unfocused(object sender, FocusEventArgs e)
		{
			searchListGrid.IsVisible = false;
		}

		/// <summary>
		/// Handles the back button press event, returning to the control list page.
		/// Cleans up the loaded sample, resets selection states, and updates visibility.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void BackButtonPressed(object sender, EventArgs e)
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.loadedSample?.OnDisappearing();
			After:
						loadedSample?.OnDisappearing();
			*/
			busyIndicatorMainPage.IsVisible = true;
			_loadedSample?.OnDisappearing();

			if (_loadedSampleModel != null)
			{
				_loadedSampleModel = null;
			}

			controlListPage.IsVisible = true;
			sampleViewPage.IsVisible = false;

			if (_sampleCategory != null)
			{
				_sampleCategory.IsSelected = false;
			}
			if (_selectedSampleSubCategoryModel != null)
			{
				_selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
			}

			busyIndicatorMainPage.IsVisible = false;

			if (_isFilterPage && !_isAllListAdded)
			{
				if (newSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(_tempWidth, true, true, filteredGridNewSample, filteredColumnOneLayoutNew, filteredColumnTwoLayoutNew, filteredColumnThreeLayoutNew);
				}
				else if (updatedSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(_tempWidth, true, false, filteredGridUpdatedSample, filteredColumnOneLayoutUpdated, filteredColumnTwoLayoutUpdated, filteredColumnThreeLayoutUpdated);
				}
			}
		}

		/// <summary>
		/// Manages category selection and collapse/expand behavior. 
		/// Toggles category collapse state or updates the selected category based on its type.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		async void Category_Tapped(object sender, EventArgs e)
		{
			var sampleCategoryModel = ((sender as Grid)?.BindingContext as SampleCategoryModel);
			if (sampleCategoryModel == null)
			{
				return;
			}

			if (sampleCategoryModel.HasCategory)
			{
				ToggleCategoryCollapse(sampleCategoryModel);
			}
			else
			{
				await UpdateSelectedCategory(sampleCategoryModel);
			}
		}

		/// <summary>
		/// Toggles the collapse state of a category. 
		/// Updates the collapse image and category status tag based on the new state.
		/// </summary>
		/// <param name="sampleCategoryModel"></param>
		void ToggleCategoryCollapse(SampleCategoryModel sampleCategoryModel)
		{
			sampleCategoryModel.IsCollapsed = !sampleCategoryModel.IsCollapsed;
			sampleCategoryModel.CollapseImage = sampleCategoryModel.IsCollapsed ? "\ue70b" : "\ue708";
			sampleCategoryModel.CategoryStatusTag = sampleCategoryModel.IsCollapsed ? false : !string.IsNullOrEmpty(sampleCategoryModel.StatusTag);
		}

		/// <summary>
		/// Updates the UI for a selected category asynchronously. 
		/// Clears previous selections, sets the new selection, and updates the chip view. 
		/// </summary>
		/// <param name="sampleCategoryModel"></param>
		async Task UpdateSelectedCategory(SampleCategoryModel sampleCategoryModel)
		{
			if (_sampleCategory == sampleCategoryModel)
			{
				return;
			}

			busyIndicatorPage.IsVisible = true;
			await Task.Delay(10);

			if (!sampleCategoryModel.IsSelected)
			{
				ClearPreviousSelections();
				sampleCategoryModel.IsSelected = true;
				UpdateChipViewBindingContext(sampleCategoryModel.SampleSubCategories![0]);
				_sampleCategory = sampleCategoryModel;
			}

			busyIndicatorPage.IsVisible = false;
		}

		/// <summary>
		/// Clears all previous selections in the UI.
		/// Resets the selected state of categories, subcategories, and card layouts.
		/// </summary>
		void ClearPreviousSelections()
		{
			if (_sampleCategory != null)
			{
				_sampleCategory.IsSelected = false;
			}
				
			if (_selectedSampleSubCategoryModel != null)
			{
				_selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
			}
				
			if (_selectedCardLayoutModel != null)
			{
				_selectedCardLayoutModel.IsSelected = false;
			}
		}

		/// <summary>
		/// Loads and displays a specified sample.
		/// Handles any exceptions that occur during the loading process.
		/// </summary>
		/// <param name="sampleModel">The sample model to load.</param>
		void LoadSample(SampleModel sampleModel)
		{
			busyIndicatorPage.IsVisible = true;

			try
			{
				PrepareSampleModel(sampleModel);
				SetupSampleUI(sampleModel);
				_loadedSample?.OnAppearing();
			}
			catch
			{
				busyIndicatorPage.IsVisible = false;
			}
		}

		/// <summary>
		/// Prepares a sample model for display in the UI. 
		/// Updates various UI elements with sample information and loads the appropriate sample type.
		/// </summary>
		/// <param name="sampleModel">The sample model to prepare.</param>
		void PrepareSampleModel(SampleModel sampleModel)
		{
			if (sampleModel.SamplePath != null && sampleModel.SamplePath.StartsWith('/'))
			{
				sampleModel.SamplePath = sampleModel.SamplePath.Remove(0, 1);
			}

			samplePathLabel.Text = sampleModel.SamplePath;
			descriptionLabel.Text = sampleModel.Description;
			IsDescrptionNotEmpty(descriptionLabel.Text!);

			var assemblyNameCollection = sampleModel.AssemblyName?.FullName?.Split(",");
			var assemblyName = assemblyNameCollection?[0] + "." + sampleModel.ControlName + "." + sampleModel.SampleName;
			if (!BaseConfig.IsIndividualSB)
			{
				assemblyName = assemblyNameCollection?[0] + "." + sampleModel.ControlShortName + "." + sampleModel.ControlName + "." + sampleModel.SampleName;
			}

			var sampleType = sampleModel.AssemblyName?.GetType(assemblyName);

			_loadedSample?.OnDisappearing();


			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.UpdatePropertyWindow();
			After:
						UpdatePropertyWindow();
			*/
			sampleGridView.Children.Clear();
			propertyGrid.Children.Clear();
			UpdatePropertyWindow();

			_loadedSample = Activator.CreateInstance(sampleType!) as SampleView;
			_loadedSampleModel = sampleModel;
		}

		/// <summary>
		/// Sets up the user interface for a sample based on the provided SampleModel.
		/// Configures visibility of various UI elements based on sample properties.
		/// </summary>
		/// <param name="sampleModel">The sample model to set up.</param>
		void SetupSampleUI(SampleModel sampleModel)
		{
			if (_loadedSample != null)
			{
				_loadedSample.SetBusyIndicator(busyIndicatorPage);
				var optionView = _loadedSample.OptionView;
				_loadedSample.OptionView = null;

				OptionIconGrid.IsVisible = optionView != null;
				if (optionView != null)
				{
					propertyGrid.Children.Add(optionView);
					SetInheritedBindingContext(optionView, _loadedSample.BindingContext);
				}

				youtubeIconGrid.IsVisible = !string.IsNullOrEmpty(sampleModel.VideoLink);
				sourceLinkGrid.IsVisible = !string.IsNullOrEmpty(sampleModel.SourceLink);
				codeViewerGrid.IsVisible = !string.IsNullOrEmpty(sampleModel.CodeViewerPath);

				if (sampleModel.IsGettingStarted)
				{
					GettingStartedSampleView gettingStartedSampleView = new GettingStartedSampleView
					{
						GettingStartedContent = _loadedSample!,
						FrameWidth = _loadedSample.WidthRequest
					};
					sampleGridView.Children.Add(gettingStartedSampleView);
				}
				else
				{
					sampleGridView.Children.Add(_loadedSample);
				}
			}
		}

		/// <summary>
		/// Adjusts the height of the labelRowDefinition based on whether the description is empty or not.
		/// </summary>
		/// <param name="labelText">The text to check.</param>
		void IsDescrptionNotEmpty(string labelText)
		{
			if (string.IsNullOrEmpty(labelText))
			{
				labelRowDefinition.Height = 0;
			}
			else
			{
				labelRowDefinition.Height = GridLength.Auto;
			}
		}

		/// <summary>
		/// Updates the visibility of the property window.
		/// </summary>
		void UpdatePropertyWindow()
		{
			propertyFrame.IsVisible = false;
		}

		/// <summary>
		/// Manages the UI interactions for selecting a subcategory.
		/// Handles deselection of previous subcategories and selection of the new subcategory.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		async void SubCategory_Tapped(object sender, EventArgs e)
		{
			var effectsView = sender as SfEffectsViewAdv;
			await Task.Delay(200);
			var sampleSubCategory = (effectsView)?.BindingContext as SampleSubCategoryModel;
			if (sampleSubCategory != null && sampleSubCategory.IsSubCategoryClicked)
			{
				return;
			}

			if (_selectedSampleSubCategoryModel != null)
			{
				_selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
			}

			if (sampleSubCategory != null)
			{
				busyIndicatorPage.IsVisible = true;
				if (_sampleCategory != null)
				{
					_sampleCategory.IsSelected = false;
					_sampleCategory = null;
				}

				_selectedSampleSubCategoryModel = sampleSubCategory;
				_selectedSampleSubCategoryModel.IsSubCategoryClicked = true;
				UpdateChipViewBindingContext(sampleSubCategory);
			}
		}

		/// <summary>
		/// Updates the chip view based on the selected subcategory. 
		/// Manages visibility and height of the chip view based on card layouts. 
		/// </summary>
		/// <param name="sampleSubCategory">The sample subcategory to update.</param>
		/// <param name="sampleModel">The optional sample model to update.</param>
		void UpdateChipViewBindingContext(SampleSubCategoryModel? sampleSubCategory, SampleModel? sampleModel = null)
		{
			if (sampleSubCategory == null)
			{
				return;
			}

			if (MainPageWindows.IsSampleSubCategoryContainsCard(sampleSubCategory))
			{
				chipView.IsVisible = true;
				chipRowDefinition.Height = 50;
				chipView.BindingContext = sampleSubCategory;
			}
			else
			{
				chipView.IsVisible = false;
				chipRowDefinition.Height = 0;
			}

			if (sampleModel == null)
			{
				if (_selectedCardLayoutModel != null)
				{
					_selectedCardLayoutModel.IsSelected = false;
				}

				_selectedCardLayoutModel = sampleSubCategory.CardLayouts![0];
				_selectedCardLayoutModel.IsSelected = true;
				sampleModel = _selectedCardLayoutModel.Samples![0];
			}

			LoadSample(sampleModel);
		}

		/// <summary>
		/// Checks if a sample subcategory contains card layouts.
		/// </summary>
		/// <param name="sampleSubCategory">The sample subcategory to check.</param>
		/// <returns>True if the subcategory contains cards, false otherwise.</returns>
		static bool IsSampleSubCategoryContainsCard(SampleSubCategoryModel sampleSubCategory)
		{
			return sampleSubCategory.CardLayouts?.Count > 1;
		}

		/// <summary>
		/// Manages the selection of a card layout within the UI.
		/// Loads the sample associated with the selected card layout.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void Chip_Tapped(object sender, EventArgs e)
		{
			var cardModel = ((sender as Grid)?.BindingContext as CardLayoutModel);
			if (_selectedCardLayoutModel != null)
			{
				if (_selectedCardLayoutModel == cardModel)
				{
					return;
				}

				_selectedCardLayoutModel.IsSelected = false;
			}

			chipScroll.ScrollToAsync(sender as Element, ScrollToPosition.Center, true);
			_selectedCardLayoutModel = cardModel;
			if (_selectedCardLayoutModel != null)
			{
				_selectedCardLayoutModel.IsSelected = true;
				LoadSample(_selectedCardLayoutModel.Samples![0]);
			}
		}

		/// <summary>
		/// Updates the selection based on the tapped search result.
		/// Clears the search text and updates the UI to display the selected sample.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.loopExit = true;
						await Task.Delay(200);
			After:
						loopExit = true;
						await Task.Delay(200);
			*/
			busyIndicatorMainPage.IsVisible = true;
			_loopExit = true;
			await Task.Delay(200);
			var itemGrid = ((sender as SfEffectsView)?.BindingContext);

			if (itemGrid is SearchModel searchModel && searchModel.Sample != null && searchModel.Control != null)
			{
				UpdatedSelectionFromSearch(searchModel.Sample, searchModel.Control);
			}

			busyIndicatorMainPage.IsVisible = true;
			searchView.Text = string.Empty;
		}

		/// <summary>
		/// Updates UI selection states based on search results. Iterates through categories, subcategories, and card layouts to find the matching sample. 
		/// Updates selection states and loads the appropriate sample page when a match is found.
		/// </summary>
		/// <param name="sampleModel">The sample model selected.</param>
		/// <param name="controlModel">The control model selected.</param>
		void UpdatedSelectionFromSearch(SampleModel sampleModel, ControlModel controlModel)
		{
			if (controlModel.SampleCategories == null || sampleModel == null)
			{
				return;
			}

			MainPageWindows.ExpandSubCategory(controlModel);
			foreach (var category in controlModel.SampleCategories)
			{
				if (category.SampleSubCategories != null)
				{
					foreach (var subCategory in category.SampleSubCategories)
					{
						if (subCategory.CardLayouts != null)
						{
							foreach (var cards in subCategory.CardLayouts)
							{
								if (cards.Samples != null && cards.Samples[0] == sampleModel)
								{
									if (_sampleCategory != null)
									{
										_sampleCategory.IsSelected = false;
										_sampleCategory = category;
										if (!_sampleCategory.HasCategory)
										{
											_sampleCategory.IsSelected = true;
										}
										else if (subCategory.CardLayouts.Count > 0 && !_sampleCategory.HasCategory)
										{
											_sampleCategory.IsSelected = true;
										}
										else
										{
											_sampleCategory.IsCollapsed = true;
										}
									}

									if (_selectedSampleSubCategoryModel != null)
									{
										_selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
										_selectedSampleSubCategoryModel = subCategory;
										_selectedSampleSubCategoryModel.IsSubCategoryClicked = true;
									}

									if (_selectedCardLayoutModel != null)
									{
										_selectedCardLayoutModel.IsSelected = false;
									}

									_selectedCardLayoutModel = cards;
									_selectedCardLayoutModel.IsSelected = true;
									LoadSamplePage(controlModel, subCategory, sampleModel);
									return;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Toggles the visibility of the property frame when the option button is tapped.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void OptionButton_Tapped(object sender, EventArgs e)
		{
			propertyFrame.IsVisible = !propertyFrame.IsVisible;
		}

		/// <summary>
		/// Updates the property window when the collapse right button is tapped.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void CollapseRightButtonTapped(object sender, EventArgs e)
		{
			UpdatePropertyWindow();
		}

		/// <summary>
		/// Constructs the URL for the sample code on GitHub.
		/// Opens the code in the default browser.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		async void CodeViewerTapped(object sender, EventArgs e)
		{
			try
			{
				var assemblyNameCollection = _loadedSampleModel?.AssemblyName?.FullName?.Split(",");
				if (assemblyNameCollection != null)
				{
					var projectName = assemblyNameCollection[0];
					if (!Syncfusion.Maui.ControlsGallery.BaseConfig.IsIndividualSB)
					{
						projectName = assemblyNameCollection[0] + "." + _loadedSampleModel?.ControlShortName;
					}

					string address = "https://github.com/syncfusion/maui-demos/tree/master/MAUI/" + _loadedSampleModel?.ControlShortName + "/" + projectName + "/Samples/" + _loadedSampleModel?.CodeViewerPath;
					_uri = new Uri(address);
					await Browser.Default.OpenAsync(address, BrowserLaunchMode.SystemPreferred);
				}
			}
			catch (Exception)
			{
				// An unexpected error occured. No browser may be installed on the device.
			}
		}

		/// <summary>
		/// Opens the YouTube link in the default browser when the YouTube icon is tapped.
		/// Uses the VideoLink from the loaded sample model to construct the URL. 
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		async void YoutubeIconTapped(object sender, EventArgs e)
		{
			try
			{
				if (_loadedSampleModel != null && _loadedSampleModel.VideoLink != null)
				{
					string address = _loadedSampleModel.VideoLink.ToString();
					_uri = new Uri(address);
					await Browser.Default.OpenAsync(address, BrowserLaunchMode.SystemPreferred);
				}
			}
			catch (Exception)
			{
				// An unexpected error occured. No browser may be installed on the device.
			}
		}

		/// <summary>
		/// Opens the source link in the default browser when tapped.
		/// Uses the SourceLink from the loaded sample model to construct the URL.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		async void SourceLinkTapped(object sender, EventArgs e)
		{
			try
			{
				if (_loadedSampleModel != null && _loadedSampleModel.SourceLink != null)
				{
					string address = _loadedSampleModel.SourceLink.ToString();
					_uri = new Uri(address);
					await Browser.Default.OpenAsync(address, BrowserLaunchMode.SystemPreferred);
				}
			}
			catch (Exception)
			{
				// An unexpected error occured. No browser may be installed on the device.
			}
		}

        /// <summary>
		/// Placeholder for handling the "What's New" tapped event.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		void WhatsNewTapped(object sender, EventArgs e)
		{
			// Add your code
		}

		/// <summary>
		/// Invoked when the sort option settings clicked.
		/// Shows the sort option grid and gray overlay.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		void SortIconTapped(object sender, EventArgs e)
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.loopExit = true;
						this.tempGrid.IsVisible = true;
			After:
						loopExit = true;
						this.tempGrid.IsVisible = true;
			*/
			_loopExit = true;
			tempGrid.IsVisible = true;
			tempGrid.ZIndex = 1;
			sortOptionGrid.IsVisible = true;
			sortOptionGrid.ZIndex = 2;
			Graylayout.IsVisible = true;
		}

		/// <summary>
		/// Invoked when the close button clicked in the sort option.
		/// Hides the sort option grid and related UI elements.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		void CloseButtonClicked(object sender, EventArgs e)
		{
			sortOptionGrid.IsVisible = false;
			tempGrid.IsVisible = false;
			Graylayout.IsVisible = false;
			if (_isFilterPage && !_isAllListAdded)
			{
				if (newSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(_tempWidth, true, true, filteredGridNewSample, filteredColumnOneLayoutNew, filteredColumnTwoLayoutNew, filteredColumnThreeLayoutNew);
				}
				else if (updatedSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(_tempWidth, true, false, filteredGridUpdatedSample, filteredColumnOneLayoutUpdated, filteredColumnTwoLayoutUpdated, filteredColumnThreeLayoutUpdated);
				}
			}
		}

		/// <summary>
		/// Placeholder method for when the sort grid is tapped.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		void SortGridTapped(object sender, EventArgs e)
		{
			//When the sort option grid is tapped, the sort option grid and sort temp grid should not be hidden, For that we have added this method.
		}

		/// <summary>
		/// Applies sorting and filtering options and updates the UI accordingly. 
		/// Determines whether to reset, sort, or filter based on the current selections. 
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		void ApplyButtonClicked(object sender, EventArgs e)
		{
			HideSortOptions();

			if (ShouldResetSortingAndFiltering())
			{
				ResetSortingAndFiltering();
				return;
			}

			if (BindingContext is SamplesViewModel viewModel)
			{
				SetSortingOption(viewModel);
				List<string> filterList = GetFilterList();

				if (IsSortingRequired(filterList))
				{
					ShowSortedGrid(viewModel, filterList);
				}
				else
				{
					ShowFilteredGrid(viewModel, filterList);
				}
			}
		}

		/// <summary>
		/// Hides the sort options UI elements.Sets visibility of various grids and elements related to sorting and filtering.
		/// </summary>
		void HideSortOptions()
		{
			sortOptionGrid.IsVisible = false;
			Graylayout.IsVisible = false;
			tempGrid.IsVisible = false;
			filteredGridNewSample.IsVisible = false;
			filteredGridUpdatedSample.IsVisible = false;
		}

		/// <summary>
		/// Determines if sorting and filtering should be reset based on current checkbox states.
		/// </summary>
		/// <returns>True if reset is required, otherwise false.</returns>
		bool ShouldResetSortingAndFiltering()
		{
			return (allSamples.IsChecked && noneOption.IsChecked) ||
				   (!allSamples.IsChecked && !newSamples.IsChecked && !updatedSamples.IsChecked && noneOption.IsChecked);
		}

		/// <summary>
		/// Resets all sorting and filtering options and updates the UI accordingly.
		/// </summary>
		void ResetSortingAndFiltering()
		{
			_isSortPage = false;
			_isFilterPage = false;
			UpdateColumn(_tempWidth);
			sortedGrid.IsVisible = false;
			sortedGridScrollViewer.IsVisible = false;
			controlListScrollViewer.IsVisible = true;
			controlListGrid.IsVisible = true;
			controlListFrame.IsVisible = true;
		}

		/// <summary>
		/// Sets the sorting option on the view model based on selected radio buttons.
		/// </summary>
		/// <param name="viewModel">The samples view model to update.</param>
		void SetSortingOption(SamplesViewModel viewModel)
		{
			if (noneOption.IsChecked)
			{
				viewModel.SortOption = SortOption.None;
			}
			else if (ascending.IsChecked)
			{
				viewModel.SortOption = SortOption.Ascending;
			}
			else if (descending.IsChecked)
			{
				viewModel.SortOption = SortOption.Descending;
			}
		}

		/// <summary>
		/// Generates a list of filter options based on selected checkboxes.
		/// </summary>
		/// <returns>A list of string representing selected filter options.</returns>
		List<string> GetFilterList()
		{
			List<string> filterList = [];
			if (newSamples.IsChecked)
			{
				filterList.Add("NewSamples");
			}
			if (updatedSamples.IsChecked)
			{
				filterList.Add("UpdatedSamples");
			}
			if (allSamples.IsChecked)
			{
				filterList.Add("AllSamples");
			}

			return filterList;
		}

		/// <summary>
		/// Determines if sorting is required based on the current filter list.
		/// </summary>
		/// <param name="filterList">The current list of filters.</param>
		/// <returns>True if sorting is required, otherwise false.</returns>
		bool IsSortingRequired(List<string> filterList)
		{
			return filterList.Count == 0 || filterList.Count == 3 || allSamples.IsChecked;
		}

		/// <summary>
		/// Displays the sorted grid and updates the UI for sorted view.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="filterList">The list of applied filters.</param>
		void ShowSortedGrid(SamplesViewModel viewModel, List<string> filterList)
		{
			_isSortPage = true;
			_isFilterPage = false;

			HideFilteredGrids();
			sortedGridScrollViewer.IsVisible = true;
			controlListGrid.IsVisible = false;
			controlListFrame.IsVisible = false;
			sortedGrid.IsVisible = true;

			viewModel.GetSortedList(filterList);
			UpdateAllSortedColumn(_tempWidth, true, false, sortedGrid, sortedcolumOneLayout, sortedcolumTwoLayout, sortedcolumThreeLayout);
		}

		/// <summary>
		/// Displays the filtered grid and updates the UI for filtered view.
		/// </summary>
		/// <param name="viewModel">The samples view model.</param>
		/// <param name="filterList">The list of applied filters.</param>
		void ShowFilteredGrid(SamplesViewModel viewModel, List<string> filterList)
		{
			_isSortPage = false;
			_isFilterPage = true;

			HideSortedGrids();
			controlListScrollViewer.IsVisible = false;
			controlListGrid.IsVisible = false;
			controlListFrame.IsVisible = false;
			filteredScrollViewer.IsVisible = true;

			viewModel.PopulateSortAndFilterSamples(filterList);
			UpdateFilteredGrids();
		}

		/// <summary>
		/// Hides all filtered grid UI elements.
		/// </summary>
		void HideFilteredGrids()
		{
			filteredGridNewSample.IsVisible = false;
			filteredGridUpdatedSample.IsVisible = false;
		}

		/// <summary>
		/// Hides all sorted grid UI elements.
		/// </summary>
	    void HideSortedGrids()
		{
			sortedGrid.IsVisible = false;
			sortedGridScrollViewer.IsVisible = false;
		}

		/// <summary>
		/// Updates the filtered grids based on selected filter options.
		/// </summary>
		void UpdateFilteredGrids()
		{
			if (newSamples.IsChecked)
			{
				filteredGridNewSample.IsVisible = true;
				UpdateAllSortedColumn(_tempWidth, true, true, filteredGridNewSample, filteredColumnOneLayoutNew, filteredColumnTwoLayoutNew, filteredColumnThreeLayoutNew);
			}

			if (updatedSamples.IsChecked)
			{
				filteredGridUpdatedSample.IsVisible = true;
				UpdateAllSortedColumn(_tempWidth, true, false, filteredGridUpdatedSample, filteredColumnOneLayoutUpdated, filteredColumnTwoLayoutUpdated, filteredColumnThreeLayoutUpdated);
			}
		}


		/// <summary>
		/// Invoked when the filter check boxes changed dynamically.
		/// Synchronizes the state of "New Samples" and "Updated Samples" checkboxes.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">CheckedChangedEventArgs</param>
		void AllSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			if (!_programmaticUpdate)
			{
				_programmaticUpdate = true;
				newSamples.IsChecked = e.Value;
				updatedSamples.IsChecked = e.Value;
				_programmaticUpdate = false;
			}
		}

		/// <summary>
		/// Invoked when the new samples check box changes dynamically.
		/// Ensures consistent behavior across sample type selections.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">CheckedChangedEventArgs</param>
		void NewSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			HandleSampleCheckBoxChange(newSamples.IsChecked, updatedSamples.IsChecked);
		}

		/// <summary>
		/// Invoked when the updated samples check box changes dynamically.Maintains consistency in filter selections.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">CheckedChangedEventArgs</param>
		void UpdatedSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			HandleSampleCheckBoxChange(newSamples.IsChecked, updatedSamples.IsChecked);
		}

		/// <summary>
		/// Manages the state of checkboxes when individual sample type checkboxes are changed.
		/// </summary>
		/// <param name="newSamplesChecked">State of "New Samples" checkbox.</param>
		/// <param name="updatedSamplesChecked">State of "Updated Samples" checkbox.</param>
		void HandleSampleCheckBoxChange(bool? newSamplesChecked, bool? updatedSamplesChecked)
		{
			if (!_programmaticUpdate)
			{
				_programmaticUpdate = true;
				if (newSamplesChecked == false || updatedSamplesChecked == false)
				{
					allSamples.IsChecked = false;
				}

				_programmaticUpdate = false;
			}
		}

		/// <summary>
		/// Handles the tapped event on the Documentation link, opening the documentation in a browser.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		async void DocumentationTapTapped(object sender, EventArgs e)
		{
			try
			{
				string address = "https://help.syncfusion.com/maui-toolkit/introduction/overview";
				_uri = new Uri(address);
				await Browser.Default.OpenAsync(address, BrowserLaunchMode.SystemPreferred);
			}
			catch (Exception)
			{
				// An unexpected error occured. No browser may be installed on the device.
			}
		}

		/// <summary>
		/// Handles the tapped event on the Pricing link. (Implementation pending)
		/// Can be expanded to show pricing information or navigate to a pricing page.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		void PricingTapTapped(object sender, EventArgs e)
		{
			//Home Page UI PricingTap Tapped, write your code 
		}

		/// <summary>
		/// Handles the tapped event on the Contact link, opening the contact page in a browser.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		async void ContactTapTapped(object sender, EventArgs e)
		{
			try
			{
				string address = "https://mauitoolkit.syncfusion.com/create";
				_uri = new Uri(address);
				await Browser.Default.OpenAsync(address, BrowserLaunchMode.SystemPreferred);
			}
			catch (Exception)
			{
				// An unexpected error occured. No browser may be installed on the device.
			}
		}

		/// <summary>
		/// Toggles the visibility of the theme change popup.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		void ChangeThemeTapTapped(object sender, EventArgs e)
		{
			_isThemePopupOpen = !_isThemePopupOpen;
			themePopup.IsVisible = _isThemePopupOpen;
			Graylayout.IsVisible = _isThemePopupOpen;
			if (_isThemePopupOpen)
			{
				themePopup.ZIndex = 1;
			}
		}

		/// <summary>
		/// Handles tapping on the gray overlay, closing all popups and option grids.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		void Graylayout_Tapped(object sender, TappedEventArgs e)
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.isThemePopupOpen = false;
						this.Graylayout.IsVisible = false;
			After:
						isThemePopupOpen = false;
						this.Graylayout.IsVisible = false;
			*/
			sortOptionGrid.IsVisible = false;
			themePopup.IsVisible = false;
			_isThemePopupOpen = false;
			Graylayout.IsVisible = false;
		}

		/// <summary>
		/// Handles tapping on the theme popup close icon, closing the theme popup.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		void ThemePopupCloseIcon_Tapped(object sender, TappedEventArgs e)
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.isThemePopupOpen = false;
						this.themePopup.IsVisible = false;
			After:
						isThemePopupOpen = false;
						this.themePopup.IsVisible = false;
			*/
			_isThemePopupOpen = false;
			themePopup.IsVisible = false;
			Graylayout.IsVisible = false;
		}

		/// <summary>
		/// Handles toggling of the theme switch, changing the application theme accordingly.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments containing the new toggle state.</param>
		void themePopupSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			if (Application.Current != null)
			{
				ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
				if (mergedDictionaries != null)
				{
					var theme = mergedDictionaries.OfType<SyncfusionThemeResourceDictionary>().FirstOrDefault();
					if (theme != null)
					{
						if (themePopupSwitch.IsToggled == false)
						{
							theme.VisualTheme = SfVisuals.MaterialLight;
							Application.Current.UserAppTheme = AppTheme.Light;
						}
						else
						{
							theme.VisualTheme = SfVisuals.MaterialDark;
							Application.Current.UserAppTheme = AppTheme.Dark;
						}
					}
				}
			}
		}
	}
}
