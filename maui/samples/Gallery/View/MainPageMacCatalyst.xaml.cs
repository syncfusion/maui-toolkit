using Syncfusion.Maui.ControlsGallery.CustomView;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.EffectsView;
namespace Syncfusion.Maui.ControlsGallery
{

	/// <summary>
	/// 
	/// </summary>
	public partial class MainPageMacCatalyst : ContentPage
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
		/// 
		/// </summary>
		public MainPageMacCatalyst()
		{
			InitializeComponent();
			if (BaseConfig.IsIndividualSB)
			{
				searchImageGrid.IsVisible = false;
			}

			if (Application.Current != null)
			{
				AppTheme currentTheme = Application.Current.RequestedTheme;
				themePopupSwitch.IsToggled = (currentTheme == AppTheme.Dark);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private List<ControlCategoryModel>? ColumnOneCollection { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private List<ControlCategoryModel>? ColumnTwoCollection { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private List<ControlCategoryModel>? ColumnThreeCollection { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
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

		private void UpdateColumn(double width)
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

		// Helper method to update the column definitions
		private void UpdateGridColumns(int columnCount)
		{
			controlListGrid.ColumnDefinitions.Clear();
			for (int i = 0; i < columnCount; i++)
			{
				controlListGrid.ColumnDefinitions.Add(new ColumnDefinition());
			}

		}

		private void UpdateAllSortedColumn(double width, bool isSortedChanged, bool isNewSample, Grid sortAndFilterGrid, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
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
		// Helper method to determine if an update is needed
		private bool NeedsColumnUpdate(int newColumnCount, bool isSortedChanged, bool isNewSample)
		{
			return isNewSample ? (_sortColumnCountNew != newColumnCount || isSortedChanged) : (_sortColumnCount != newColumnCount || isSortedChanged);
		}

		// Helper method to update the sort column count
		private void UpdateSortColumnCount(int newColumnCount, bool isNewSample)
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

		// Helper method to update the grid columns
		private void UpdateGridColumns(Grid grid, int columnCount)
		{
			grid.ColumnDefinitions.Clear();
			// Add column definitions based on page type
			int columnsToAdd = _isSortPage ? columnCount : Math.Min(columnCount, 2); // Filters use only up to 2 columns
			for (int i = 0; i < columnsToAdd; i++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition());
			}
		}

		private void ArrangeSortedControlOnColumn(bool isTwoColumn, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
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


		private void ArrangeControlOnColumn(bool isTwoColumn)
		{
			if (BindingContext is SamplesViewModel viewModel)
			{
				int columnCount = isTwoColumn ? 2 : 3;
				ArrangeControl(viewModel, columnCount);
			}
		}

		private void ArrangeControl(SamplesViewModel viewModel, int columnCount)
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

		// Helper method to clear existing bindings
		private void ClearLayouts()
		{
			BindableLayout.SetItemsSource(columOneLayout, null);
			BindableLayout.SetItemsSource(columTwoLayout, null);
			BindableLayout.SetItemsSource(columThreeLayout, null);
		}

		private void ArrangeSortedControlInTwoColumn(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{

			if (_isSortPage)
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
				BindableLayout.SetItemsSource(verticalThreeLayout, null);
				BindableLayout.SetItemsSource(verticalOneLayout, _sortColumnOneCollection);
				BindableLayout.SetItemsSource(verticalTwoLayout, _sortColumnTwoCollection);
				sortedcolumThreeLayout.IsVisible = false;
			}
			else
			{
				if (newSamples.IsChecked == true && isNewSample)
				{
					_filterColumnOneCollectionNew = [];
					MainPageMacCatalyst.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
					BindableLayout.SetItemsSource(verticalOneLayout, _filterColumnOneCollectionNew);
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
							_filterColumnOneCollectionNew.Add(item!);
							index++;
						}
					}

					_isAllListAdded = true;
					filteredColumnThreeLayoutNew.IsVisible = false;
					filteredColumnTwoLayoutNew.IsVisible = false;
				}

				if (updatedSamples.IsChecked == true && !isNewSample)
				{

					_filterColumnOneCollectionUpdated = [];
					MainPageMacCatalyst.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
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

			}
		}
		private void ArrangeSortedControlInThreeColumn(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			int columnOneTempCount = 0;
			int columnTwoTempCount = 0;

			int columnTwoCount;
			int columnOneCount;
			if (_isSortPage)
			{
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

				foreach (var item in viewModel.UpdatedSortedList)
				{
					if (columnOneTempCount < columnOneCount)
					{
						columnOneTempCount += 1;
						_sortColumnOneCollection.Add(item);
					}
					else if (columnTwoTempCount < columnTwoCount)
					{
						columnTwoTempCount += 1;
						_sortColumnTwoCollection.Add(item);
					}
					else
					{
						_sortColumnThreeCollection.Add(item);
					}
				}

				MainPageMacCatalyst.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
				BindableLayout.SetItemsSource(verticalOneLayout, _sortColumnOneCollection);
				BindableLayout.SetItemsSource(verticalTwoLayout, _sortColumnTwoCollection);
				BindableLayout.SetItemsSource(verticalThreeLayout, _sortColumnThreeCollection);
				sortedcolumThreeLayout.IsVisible = true;
			}
			else
			{
				if (newSamples.IsChecked == true && isNewSample)
				{
					columnTwoCount = 0;

					_filterColumnOneCollectionNew = [];
					_filterColumnTwoCollectionNew = [];
					MainPageMacCatalyst.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
					BindableLayout.SetItemsSource(verticalOneLayout, _filterColumnOneCollectionNew);
					BindableLayout.SetItemsSource(verticalTwoLayout, _filterColumnTwoCollectionNew);

					columnOneCount = viewModel.FilterNewSampleCount / 2 + viewModel.FilterNewSampleCount % 2;

					int index = 0;
					_loopExit = false;
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
								_filterColumnOneCollectionNew.Add(item);
							}
							index++;
						}
					}
					_isAllListAdded = true;
					filteredGridNewSample.IsVisible = true;
					filteredColumnTwoLayoutNew.IsVisible = true;
					filteredColumnThreeLayoutNew.IsVisible = false;
				}
				if (updatedSamples.IsChecked == true && !isNewSample)
				{
					columnTwoCount = 0;

					_filterColumnOneCollectionUpdated = [];
					_filterColumnTwoCollectionUpdated = [];
					MainPageMacCatalyst.ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
					BindableLayout.SetItemsSource(verticalOneLayout, _filterColumnOneCollectionUpdated);
					BindableLayout.SetItemsSource(verticalTwoLayout, _filterColumnTwoCollectionUpdated);

					columnOneCount = viewModel.FilterUpdatedSampleCount / 2 + viewModel.FilterUpdatedSampleCount % 2;

					int index = 0;
					_loopExit = false;
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
			}
		}

		private static void ClearItemsSource(VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			BindableLayout.SetItemsSource(verticalOneLayout, null);
			BindableLayout.SetItemsSource(verticalTwoLayout, null);
			BindableLayout.SetItemsSource(verticalThreeLayout, null);
		}

		private async void Control_Tapped(object sender, EventArgs e)
		{
			busyIndicatorMainPage.IsVisible = true;
			await Task.Delay(100);
			if (sender is SfEffectsViewAdv { BindingContext: ControlModel controlObjectModel })
			{
				LoadSamplePage(controlObjectModel);
				UpdateSelectionUIToFirstItem(controlObjectModel.SampleCategories?[0]);
			}

			busyIndicatorMainPage.IsVisible = false;
			busyIndicatorPage.IsVisible = false;
		}

		private void UpdateSelectionUIToFirstItem(SampleCategoryModel? sampleCategoryModel)
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

		private void LoadSamplePage(ControlModel controlModel, SampleModel? sampleModel = null)
		{
			LoadSamplePage(controlModel, null, sampleModel);
		}
		private void LoadSamplePage(ControlModel controlModel, SampleSubCategoryModel? subCategoryModel, SampleModel? sampleModel = null)
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

		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
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

		private void Entry_Unfocused(object sender, FocusEventArgs e)
		{
			searchListGrid.IsVisible = false;
		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
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

		private async void Category_Tapped(object sender, EventArgs e)
		{
			var sampleCategoryModel = ((sender as Grid)?.BindingContext as SampleCategoryModel);
			if (sampleCategoryModel != null)
			{
				if (sampleCategoryModel.HasCategory)
				{
					sampleCategoryModel.IsCollapsed = !sampleCategoryModel.IsCollapsed;
					if (sampleCategoryModel.IsCollapsed)
					{
						sampleCategoryModel.CollapseImage = "\ue70b";
					}
					else
					{
						sampleCategoryModel.CollapseImage = "\ue708";
					}
				}
				else
				{
					if (_sampleCategory != sampleCategoryModel)
					{
						busyIndicatorPage.IsVisible = true;
						await Task.Delay(10);
						if (!sampleCategoryModel.IsSelected)
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

							sampleCategoryModel.IsSelected = true;
							UpdateChipViewBindingContext(sampleCategoryModel.SampleSubCategories![0]);
							_sampleCategory = sampleCategoryModel;
						}
						busyIndicatorPage.IsVisible = false;
					}
				}
			}
		}

		private void LoadSample(SampleModel sampleModel)
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

		private void PrepareSampleModel(SampleModel sampleModel)
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

		private void SetupSampleUI(SampleModel sampleModel)
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

		private void IsDescrptionNotEmpty(string labelText)
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

		private void UpdatePropertyWindow()
		{
			propertyFrame.IsVisible = false;
		}

		private async void SubCategory_Tapped(object sender, EventArgs e)
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

		private void UpdateChipViewBindingContext(SampleSubCategoryModel? sampleSubCategory, SampleModel? sampleModel = null)
		{
			if (sampleSubCategory == null)
			{
				return;
			}
			if (MainPageMacCatalyst.IsSampleSubCategoryContainsCard(sampleSubCategory))
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

		private static bool IsSampleSubCategoryContainsCard(SampleSubCategoryModel sampleSubCategory)
		{
			return sampleSubCategory.CardLayouts?.Count > 1;
		}

		private void Chip_Tapped(object sender, EventArgs e)
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

#if WINDOWS
			chipScroll.ScrollToAsync(sender as Element, ScrollToPosition.Center, true);
#endif

			_selectedCardLayoutModel = cardModel;
			if (_selectedCardLayoutModel != null)
			{
				_selectedCardLayoutModel.IsSelected = true;
				LoadSample(_selectedCardLayoutModel.Samples![0]);
			}
		}

		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
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

		private void UpdatedSelectionFromSearch(SampleModel sampleModel, ControlModel controlModel)
		{
			if (controlModel.SampleCategories == null || sampleModel == null)
			{
				return;
			}
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

		private void OptionButton_Tapped(object sender, EventArgs e)
		{
			propertyFrame.IsVisible = !propertyFrame.IsVisible;
		}

		private void CollapseRightButtonTapped(object sender, EventArgs e)
		{
			UpdatePropertyWindow();
		}

		private async void CodeViewerTapped(object sender, EventArgs e)
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

		private async void YoutubeIconTapped(object sender, EventArgs e)
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

		private async void SourceLinkTapped(object sender, EventArgs e)
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

		private void WhatsNewTapped(object sender, EventArgs e)
		{
			// Add your code
		}

		/// <summary>
		/// Invoked when the sort option settings clicked
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void SortIconTapped(object sender, EventArgs e)
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
		/// Invoked when the close button clicked in the sort option
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void CloseButtonClicked(object sender, EventArgs e)
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


		private void SortGridTapped(object sender, EventArgs e)
		{
			//When the sort option grid is tapped, the sort option grid and sort temp grid should not be hidden, For that we have added this method.
		}

		/// <summary>
		/// Invoked when the apply button in the sort option clicked
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void ApplyButtonClicked(object sender, EventArgs e)
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

		private void HideSortOptions()
		{
			sortOptionGrid.IsVisible = false;
			Graylayout.IsVisible = false;
			tempGrid.IsVisible = false;
			filteredGridNewSample.IsVisible = false;
			filteredGridUpdatedSample.IsVisible = false;
		}

		private bool ShouldResetSortingAndFiltering()
		{
			return (allSamples.IsChecked && noneOption.IsChecked) ||
				   (!allSamples.IsChecked && !newSamples.IsChecked && !updatedSamples.IsChecked && noneOption.IsChecked);
		}

		private void ResetSortingAndFiltering()
		{
			_isSortPage = false;
			_isFilterPage = false;
			UpdateColumn(_tempWidth);
			sortedGrid.IsVisible = false;
			sortedGridScrollViewer.IsVisible = false;
			controlListScrollViewer.IsVisible = true;
			controlListGrid.IsVisible = true;
			controlListScrollViewer.IsVisible = true;
		}

		private void SetSortingOption(SamplesViewModel viewModel)
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

		private List<string> GetFilterList()
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

		private bool IsSortingRequired(List<string> filterList)
		{
			return filterList.Count == 0 || filterList.Count == 3 || allSamples.IsChecked;
		}

		private void ShowSortedGrid(SamplesViewModel viewModel, List<string> filterList)
		{
			_isSortPage = true;
			_isFilterPage = false;
			HideFilteredGrids();
			sortedGridScrollViewer.IsVisible = true;
			controlListGrid.IsVisible = false;
			controlListScrollViewer.IsVisible = false;
			sortedGrid.IsVisible = true;

			viewModel.GetSortedList(filterList);
			UpdateAllSortedColumn(_tempWidth, true, false, sortedGrid, sortedcolumOneLayout, sortedcolumTwoLayout, sortedcolumThreeLayout);
		}

		private void ShowFilteredGrid(SamplesViewModel viewModel, List<string> filterList)
		{
			_isSortPage = false;
			_isFilterPage = true;

			HideSortedGrids();
			controlListScrollViewer.IsVisible = false;
			controlListGrid.IsVisible = false;
			filteredScrollViewer.IsVisible = true;

			viewModel.PopulateSortAndFilterSamples(filterList);
			UpdateFilteredGrids();
		}

		private void HideFilteredGrids()
		{
			filteredGridNewSample.IsVisible = false;
			filteredGridUpdatedSample.IsVisible = false;
		}

		private void HideSortedGrids()
		{
			sortedGrid.IsVisible = false;
			sortedGridScrollViewer.IsVisible = false;
		}

		private void UpdateFilteredGrids()
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
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">CheckedChangedEventArgs</param>
		private void AllSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
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
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">CheckedChangedEventArgs</param>
		private void NewSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			HandleSampleCheckBoxChange(newSamples.IsChecked, updatedSamples.IsChecked);
		}

		/// <summary>
		/// Invoked when the updated samples check box changes dynamically.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">CheckedChangedEventArgs</param>
		private void UpdatedSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			HandleSampleCheckBoxChange(newSamples.IsChecked, updatedSamples.IsChecked);
		}

		private void HandleSampleCheckBoxChange(bool? newSamplesChecked, bool? updatedSamplesChecked)
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void DocumentationTapTapped(object sender, EventArgs e)
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PricingTapTapped(object sender, EventArgs e)
		{
			//Home Page UI PricingTap Tapped, write your code 
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void ContactTapTapped(object sender, EventArgs e)
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangeThemeTapTapped(object sender, EventArgs e)
		{
			_isThemePopupOpen = !_isThemePopupOpen;
			themePopup.IsVisible = _isThemePopupOpen;
			Graylayout.IsVisible = _isThemePopupOpen;
			if (_isThemePopupOpen)
			{
				themePopup.ZIndex = 1;
			}
		}

		private void themePopupSwitch_Toggled(object sender, ToggledEventArgs e)
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

		private void Graylayout_Tapped(object sender, TappedEventArgs e)
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

		private void ThemePopupCloseIcon_Tapped(object sender, TappedEventArgs e)
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


	}
}

