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
		int columnCount = 0;
		SampleCategoryModel? sampleCategory;
		SampleView? loadedSample;
		SampleSubCategoryModel? selectedSampleSubCategoryModel;
		CardLayoutModel? selectedCardLayoutModel;
		SampleModel? loadedSampleModel;
		Uri? uri;
		bool isSortPage = false;
		double tempWidth;
		int sortColumnCount = 0;
		int sortColumnCountNew = 0;
		bool isFilterPage = false;
		bool loopExit = false;
		bool isThemePopupOpen = false;
		bool isAllListAdded = false;
		bool programmaticUpdate = false;
		Dictionary<SampleModel, ControlModel> FilteredCollection = new();

		List<ControlModel>? sortColumnOneCollection;
		List<ControlModel>? sortColumnTwoCollection;
		List<ControlModel>? sortColumnThreeCollection;

		ObservableCollection<SearchModel>? filterColumnOneCollectionNew;
		ObservableCollection<SearchModel>? filterColumnTwoCollectionNew;
		ObservableCollection<SearchModel>? filterColumnOneCollectionUpdated;
		ObservableCollection<SearchModel>? filterColumnTwoCollectionUpdated;

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
			tempWidth = width;
			MainThread.BeginInvokeOnMainThread(() =>
			{
				if (isSortPage)
				{
					UpdateAllSortedColumn(width, false, false, this.sortedGrid, this.sortedcolumOneLayout, this.sortedcolumTwoLayout, this.sortedcolumThreeLayout);
				}
				else if (isFilterPage)
				{
					UpdateAllSortedColumn(width, false, true, this.filteredGridNewSample, this.filteredColumnOneLayoutNew, this.filteredColumnTwoLayoutNew, this.filteredColumnThreeLayoutNew);
					UpdateAllSortedColumn(width, false, false, this.filteredGridUpdatedSample, this.filteredColumnOneLayoutUpdated, this.filteredColumnTwoLayoutUpdated, this.filteredColumnThreeLayoutUpdated);
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
			if (columnCount == newColumnCount)
			{
				return;
			}
			// Set the new column count
			columnCount = newColumnCount;

			// Hide the grid before making changes
			this.pageGrid.IsVisible = false;

			// Clear and reconfigure the column definitions
			UpdateGridColumns(newColumnCount);

			// Arrange controls based on the column count
			ArrangeControlOnColumn(newColumnCount == 2);

			// Show the grid after updating
			this.pageGrid.IsVisible = true;
		}

		// Helper method to update the column definitions
		private void UpdateGridColumns(int columnCount)
		{
			this.controlListGrid.ColumnDefinitions.Clear();
			for (int i = 0; i < columnCount; i++)
			{
				this.controlListGrid.ColumnDefinitions.Add(new ColumnDefinition());
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
			this.pageGrid.IsVisible = false;

			// Clear and update the column definitions based on the current page
			UpdateGridColumns(sortAndFilterGrid, newColumnCount);

			// Arrange controls based on the column layout
			ArrangeSortedControlOnColumn(newColumnCount == 2, isNewSample, verticalOneLayout, verticalTwoLayout, verticalThreeLayout);

			// Show the grid after updating
			this.pageGrid.IsVisible = true;
		}
		// Helper method to determine if an update is needed
		private bool NeedsColumnUpdate(int newColumnCount, bool isSortedChanged, bool isNewSample)
		{
			return isNewSample ? (sortColumnCountNew != newColumnCount || isSortedChanged) : (sortColumnCount != newColumnCount || isSortedChanged);
		}

		// Helper method to update the sort column count
		private void UpdateSortColumnCount(int newColumnCount, bool isNewSample)
		{
			if (isNewSample)
			{
				sortColumnCountNew = newColumnCount;
			}
			else
			{
				sortColumnCount = newColumnCount;
			}
		}

		// Helper method to update the grid columns
		private void UpdateGridColumns(Grid grid, int columnCount)
		{
			grid.ColumnDefinitions.Clear();
			// Add column definitions based on page type
			int columnsToAdd = isSortPage ? columnCount : Math.Min(columnCount, 2); // Filters use only up to 2 columns
			for (int i = 0; i < columnsToAdd; i++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition());
			}
		}

		private void ArrangeSortedControlOnColumn(bool isTwoColumn, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			if (this.BindingContext is SamplesViewModel viewModel)
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
			if (this.BindingContext is SamplesViewModel viewModel)
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
				columnCollections.Add(new List<ControlCategoryModel>());
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
			BindableLayout.SetItemsSource(this.columOneLayout, columnCollections[0]);
			BindableLayout.SetItemsSource(this.columTwoLayout, columnCollections[1]);
			if (columnCount == 3)
			{
				BindableLayout.SetItemsSource(this.columThreeLayout, columnCollections[2]);
			}
		}

		// Helper method to clear existing bindings
		private void ClearLayouts()
		{
			BindableLayout.SetItemsSource(this.columOneLayout, null);
			BindableLayout.SetItemsSource(this.columTwoLayout, null);
			BindableLayout.SetItemsSource(this.columThreeLayout, null);
		}

		private void ArrangeSortedControlInTwoColumn(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{

			if (isSortPage)
			{
				int columnTwoCount = 0;
				sortColumnOneCollection = new List<ControlModel>();
				sortColumnTwoCollection = new List<ControlModel>();

				int columnOneCount = viewModel.UpdatedSortedList.Count / 2 + viewModel.UpdatedSortedList.Count % 2;
				foreach (var item in viewModel.UpdatedSortedList)
				{

					if (columnTwoCount < columnOneCount)
					{
						columnTwoCount += 1;
						sortColumnOneCollection.Add(item);
					}
					else
					{
						sortColumnTwoCollection.Add(item);
					}
				}
				BindableLayout.SetItemsSource(verticalThreeLayout, null);
				BindableLayout.SetItemsSource(verticalOneLayout, sortColumnOneCollection);
				BindableLayout.SetItemsSource(verticalTwoLayout, sortColumnTwoCollection);
				sortedcolumThreeLayout.IsVisible = false;
			}
			else
			{
				if (newSamples.IsChecked == true && isNewSample)
				{
					filterColumnOneCollectionNew = new ObservableCollection<SearchModel>();
					ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
					BindableLayout.SetItemsSource(verticalOneLayout, filterColumnOneCollectionNew);
					loopExit = false;
					int index = 0;
					isAllListAdded = false;
					foreach (var item in viewModel.SortedList)
					{
						if (item.Control != null && item.Control.StatusTag == "New")
						{
							if (loopExit)
							{
								return;
							}
							filterColumnOneCollectionNew.Add(item!);
							index++;
						}
					}

					isAllListAdded = true;
					filteredColumnThreeLayoutNew.IsVisible = false;
					filteredColumnTwoLayoutNew.IsVisible = false;
				}

				if (updatedSamples.IsChecked == true && !isNewSample)
				{

					filterColumnOneCollectionUpdated = new ObservableCollection<SearchModel>();
					ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
					BindableLayout.SetItemsSource(verticalOneLayout, filterColumnOneCollectionUpdated);

					loopExit = false;
					int index = 0;
					isAllListAdded = false;
					foreach (var item in viewModel.SortedList)
					{
						if (item.Control != null && item.Control.StatusTag == "Updated")
						{
							if (loopExit)
							{
								return;
							}
							filterColumnOneCollectionUpdated.Add(item!);
							index++;
						}
					}

					isAllListAdded = true;
					filteredColumnTwoLayoutUpdated.IsVisible = false;
					filteredColumnThreeLayoutUpdated.IsVisible = false;
				}

			}
		}
		private void ArrangeSortedControlInThreeColumn(SamplesViewModel viewModel, bool isNewSample, VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			int columnOneCount = 0;
			int columnTwoCount = 0;
			int columnOneTempCount = 0;
			int columnTwoTempCount = 0;

			if (isSortPage)
			{
				sortColumnOneCollection = new List<ControlModel>();
				sortColumnTwoCollection = new List<ControlModel>();
				sortColumnThreeCollection = new List<ControlModel>();

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
						sortColumnOneCollection.Add(item);
					}
					else if (columnTwoTempCount < columnTwoCount)
					{
						columnTwoTempCount += 1;
						sortColumnTwoCollection.Add(item);
					}
					else
					{
						sortColumnThreeCollection.Add(item);
					}
				}

				ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
				BindableLayout.SetItemsSource(verticalOneLayout, sortColumnOneCollection);
				BindableLayout.SetItemsSource(verticalTwoLayout, sortColumnTwoCollection);
				BindableLayout.SetItemsSource(verticalThreeLayout, sortColumnThreeCollection);
				sortedcolumThreeLayout.IsVisible = true;
			}
			else
			{
				if (newSamples.IsChecked == true && isNewSample)
				{
					columnTwoCount = 0;

					filterColumnOneCollectionNew = new ObservableCollection<SearchModel>();
					filterColumnTwoCollectionNew = new ObservableCollection<SearchModel>();
					ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
					BindableLayout.SetItemsSource(verticalOneLayout, filterColumnOneCollectionNew);
					BindableLayout.SetItemsSource(verticalTwoLayout, filterColumnTwoCollectionNew);

					columnOneCount = viewModel.FilterNewSampleCount / 2 + viewModel.FilterNewSampleCount % 2;

					int index = 0;
					loopExit = false;
					isAllListAdded = false;
					foreach (var item in viewModel.SortedList)
					{
						if (item.Control != null && item.Control.StatusTag == "New")
						{
							if (loopExit)
							{
								return;
							}
							if (columnTwoCount < columnOneCount)
							{
								columnTwoCount += 1;
								filterColumnOneCollectionNew.Add(item!);
							}
							else
							{
								filterColumnOneCollectionNew.Add(item);
							}
							index++;
						}
					}
					isAllListAdded = true;
					this.filteredGridNewSample.IsVisible = true;
					filteredColumnTwoLayoutNew.IsVisible = true;
					filteredColumnThreeLayoutNew.IsVisible = false;
				}
				if (updatedSamples.IsChecked == true && !isNewSample)
				{
					columnTwoCount = 0;

					filterColumnOneCollectionUpdated = new ObservableCollection<SearchModel>();
					filterColumnTwoCollectionUpdated = new ObservableCollection<SearchModel>();
					ClearItemsSource(verticalOneLayout, verticalTwoLayout, verticalThreeLayout);
					BindableLayout.SetItemsSource(verticalOneLayout, filterColumnOneCollectionUpdated);
					BindableLayout.SetItemsSource(verticalTwoLayout, filterColumnTwoCollectionUpdated);

					columnOneCount = viewModel.FilterUpdatedSampleCount / 2 + viewModel.FilterUpdatedSampleCount % 2;

					int index = 0;
					loopExit = false;
					isAllListAdded = false;
					foreach (var item in viewModel.SortedList)
					{
						if (item.Control != null && item.Control.StatusTag == "Updated")
						{
							if (loopExit)
							{
								return;
							}
							if (columnTwoCount < columnOneCount)
							{
								columnTwoCount += 1;
								filterColumnOneCollectionUpdated.Add(item!);
							}
							else
							{
								filterColumnTwoCollectionUpdated.Add(item);
							}
							index++;
						}
					}

					isAllListAdded = true;
					this.filteredGridUpdatedSample.IsVisible = true;
					filteredColumnTwoLayoutUpdated.IsVisible = true;
					filteredColumnThreeLayoutUpdated.IsVisible = false;
				}
			}
		}

		private void ClearItemsSource(VerticalStackLayout verticalOneLayout, VerticalStackLayout verticalTwoLayout, VerticalStackLayout verticalThreeLayout)
		{
			BindableLayout.SetItemsSource(verticalOneLayout, null);
			BindableLayout.SetItemsSource(verticalTwoLayout, null);
			BindableLayout.SetItemsSource(verticalThreeLayout, null);
		}

		private async void Control_Tapped(object sender, EventArgs e)
		{
			this.busyIndicatorMainPage.IsVisible = true;
			await Task.Delay(100);
			if ((sender as SfEffectsViewAdv)?.BindingContext is ControlModel controlObjectModel)
			{
				LoadSamplePage(controlObjectModel);
				UpdateSelectionUIToFirstItem(controlObjectModel.SampleCategories?[0]);
			}

			this.busyIndicatorMainPage.IsVisible = false;
			this.busyIndicatorPage.IsVisible = false;
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
					selectedSampleSubCategoryModel = sampleCategoryModel.SampleSubCategories[0];
					selectedSampleSubCategoryModel.IsSubCategoryClicked = true;
				}
			}
			else
			{
				sampleCategory = sampleCategoryModel;
				sampleCategory.IsSelected = true;
			}
		}

		private void LoadSamplePage(ControlModel controlModel, SampleModel? sampleModel = null)
		{
			LoadSamplePage(controlModel, null, sampleModel);
		}
		private void LoadSamplePage(ControlModel controlModel, SampleSubCategoryModel? subCategoryModel, SampleModel? sampleModel = null)
		{
			this.sampleViewPage.BindingContext = controlModel;
			this.controlListPage.IsVisible = false;
			this.sampleViewPage.IsVisible = true;
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
			var textValue = (sender as Entry);

			if (e.NewTextValue.Length > 1 && !searchListGrid.IsVisible)
			{
				this.searchListGrid.IsVisible = true;
				this.searchListGrid.ZIndex = 1;
			}
			else if (e.NewTextValue.Length <= 1)
			{
				this.searchListGrid.IsVisible = false;
			}
		}

		private void Entry_Unfocused(object sender, FocusEventArgs e)
		{
			this.searchListGrid.IsVisible = false;
		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
			this.busyIndicatorMainPage.IsVisible = true;

			this.loadedSample?.OnDisappearing();

			if (this.loadedSampleModel != null)
			{
				this.loadedSampleModel = null;
			}

			this.controlListPage.IsVisible = true;
			this.sampleViewPage.IsVisible = false;

			if (sampleCategory != null)
			{
				this.sampleCategory.IsSelected = false;
			}
			if (this.selectedSampleSubCategoryModel != null)
			{
				this.selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
			}
			this.busyIndicatorMainPage.IsVisible = false;

			if (isFilterPage && !isAllListAdded)
			{
				if (newSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(tempWidth, true, true, this.filteredGridNewSample, this.filteredColumnOneLayoutNew, this.filteredColumnTwoLayoutNew, this.filteredColumnThreeLayoutNew);
				}
				else if (updatedSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(tempWidth, true, false, this.filteredGridUpdatedSample, this.filteredColumnOneLayoutUpdated, this.filteredColumnTwoLayoutUpdated, this.filteredColumnThreeLayoutUpdated);
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
					if (sampleCategory != sampleCategoryModel)
					{
						this.busyIndicatorPage.IsVisible = true;
						await Task.Delay(10);
						if (!sampleCategoryModel.IsSelected)
						{
							if (sampleCategory != null)
							{
								sampleCategory.IsSelected = false;
							}
							if (selectedSampleSubCategoryModel != null)
							{
								selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
							}
							if (selectedCardLayoutModel != null)
							{
								selectedCardLayoutModel.IsSelected = false;
							}

							sampleCategoryModel.IsSelected = true;
							UpdateChipViewBindingContext(sampleCategoryModel.SampleSubCategories![0]);
							sampleCategory = sampleCategoryModel;
						}
						this.busyIndicatorPage.IsVisible = false;
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
				loadedSample?.OnAppearing();
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

			this.samplePathLabel.Text = sampleModel.SamplePath;
			this.descriptionLabel.Text = sampleModel.Description;
			IsDescrptionNotEmpty(descriptionLabel.Text!);

			var assemblyNameCollection = sampleModel.AssemblyName?.FullName?.Split(",");
			var assemblyName = assemblyNameCollection?[0] + "." + sampleModel.ControlName + "." + sampleModel.SampleName;
			if (!BaseConfig.IsIndividualSB)
			{
				assemblyName = assemblyNameCollection?[0] + "." + sampleModel.ControlShortName + "." + sampleModel.ControlName + "." + sampleModel.SampleName;
			}
			var sampleType = sampleModel.AssemblyName?.GetType(assemblyName);

			loadedSample?.OnDisappearing();

			this.sampleGridView.Children.Clear();
			this.propertyGrid.Children.Clear();
			this.UpdatePropertyWindow();

			loadedSample = Activator.CreateInstance(sampleType!) as SampleView;
			loadedSampleModel = sampleModel;
		}

		private void SetupSampleUI(SampleModel sampleModel)
		{
			if (loadedSample != null)
			{
				loadedSample.SetBusyIndicator(this.busyIndicatorPage);
				var optionView = loadedSample.OptionView;
				loadedSample.OptionView = null;

				OptionIconGrid.IsVisible = optionView != null;
				if (optionView != null)
				{
					this.propertyGrid.Children.Add(optionView);
					SetInheritedBindingContext(optionView, loadedSample.BindingContext);
				}

				youtubeIconGrid.IsVisible = !string.IsNullOrEmpty(sampleModel.VideoLink);
				sourceLinkGrid.IsVisible = !string.IsNullOrEmpty(sampleModel.SourceLink);
				codeViewerGrid.IsVisible = !string.IsNullOrEmpty(sampleModel.CodeViewerPath);
				if (sampleModel.IsGettingStarted)
				{
					GettingStartedSampleView gettingStartedSampleView = new GettingStartedSampleView
					{
						GettingStartedContent = loadedSample!,
						FrameWidth = loadedSample.WidthRequest
					};
					this.sampleGridView.Children.Add(gettingStartedSampleView);
				}
				else
				{
					this.sampleGridView.Children.Add(loadedSample);
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
			this.propertyFrame.IsVisible = false;
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
			if (selectedSampleSubCategoryModel != null)
			{
				selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
			}

			if (sampleSubCategory != null)
			{
				this.busyIndicatorPage.IsVisible = true;
				if (sampleCategory != null)
				{
					sampleCategory.IsSelected = false;
					sampleCategory = null;
				}

				selectedSampleSubCategoryModel = sampleSubCategory;
				selectedSampleSubCategoryModel.IsSubCategoryClicked = true;

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
				this.chipView.IsVisible = true;
				this.chipRowDefinition.Height = 50;
				this.chipView.BindingContext = sampleSubCategory;
			}
			else
			{
				this.chipView.IsVisible = false;
				this.chipRowDefinition.Height = 0;
			}

			if (sampleModel == null)
			{
				if (selectedCardLayoutModel != null)
				{
					selectedCardLayoutModel.IsSelected = false;
				}
				selectedCardLayoutModel = sampleSubCategory.CardLayouts![0];
				selectedCardLayoutModel.IsSelected = true;
				sampleModel = selectedCardLayoutModel.Samples![0];
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
			if (selectedCardLayoutModel != null)
			{
				if (selectedCardLayoutModel == cardModel)
				{
					return;
				}
				selectedCardLayoutModel.IsSelected = false;
			}

#if WINDOWS
        this.chipScroll.ScrollToAsync(sender as Element, ScrollToPosition.Center, true);
#endif

			selectedCardLayoutModel = cardModel;
			if (selectedCardLayoutModel != null)
			{
				selectedCardLayoutModel.IsSelected = true;
				LoadSample(selectedCardLayoutModel.Samples![0]);
			}
		}

		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			this.busyIndicatorMainPage.IsVisible = true;
			loopExit = true;
			await Task.Delay(200);
			var itemGrid = ((sender as SfEffectsView)?.BindingContext);

			if (itemGrid is SearchModel searchModel && searchModel.Sample != null && searchModel.Control != null)
			{
				UpdatedSelectionFromSearch(searchModel.Sample, searchModel.Control);
			}

			this.busyIndicatorMainPage.IsVisible = true;
			this.searchView.Text = String.Empty;
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
									if (sampleCategory != null)
									{
										sampleCategory.IsSelected = false;
										sampleCategory = category;
										if (!sampleCategory.HasCategory)
										{
											sampleCategory.IsSelected = true;
										}
										else if (subCategory.CardLayouts.Count > 0 && !sampleCategory.HasCategory)
										{
											sampleCategory.IsSelected = true;
										}
										else
										{
											sampleCategory.IsCollapsed = true;
										}
									}
									if (selectedSampleSubCategoryModel != null)
									{
										selectedSampleSubCategoryModel.IsSubCategoryClicked = false;
										selectedSampleSubCategoryModel = subCategory;
										selectedSampleSubCategoryModel.IsSubCategoryClicked = true;
									}

									if (selectedCardLayoutModel != null)
									{
										selectedCardLayoutModel.IsSelected = false;
									}
									selectedCardLayoutModel = cards;
									selectedCardLayoutModel.IsSelected = true;
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
			this.propertyFrame.IsVisible = !this.propertyFrame.IsVisible;
		}

		private void CollapseRightButtonTapped(object sender, EventArgs e)
		{
			this.UpdatePropertyWindow();
		}

		private async void CodeViewerTapped(object sender, EventArgs e)
		{
			try
			{
				var assemblyNameCollection = loadedSampleModel?.AssemblyName?.FullName?.Split(",");
				if (assemblyNameCollection != null)
				{
					var projectName = assemblyNameCollection[0];
					if (!Syncfusion.Maui.ControlsGallery.BaseConfig.IsIndividualSB)
					{
						projectName = assemblyNameCollection[0] + "." + loadedSampleModel?.ControlShortName;
					}
					string address = "https://github.com/syncfusion/maui-demos/tree/master/MAUI/" + loadedSampleModel?.ControlShortName + "/" + projectName + "/Samples/" + loadedSampleModel?.CodeViewerPath;
					uri = new Uri(address);
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
				if (loadedSampleModel != null && loadedSampleModel.VideoLink != null)
				{
					string address = loadedSampleModel.VideoLink.ToString();
					uri = new Uri(address);
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
				if (loadedSampleModel != null && loadedSampleModel.SourceLink != null)
				{
					string address = loadedSampleModel.SourceLink.ToString();
					uri = new Uri(address);
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
			this.loopExit = true;
			this.tempGrid.IsVisible = true;
			this.tempGrid.ZIndex = 1;
			this.sortOptionGrid.IsVisible = true;
			this.sortOptionGrid.ZIndex = 2;
			this.Graylayout.IsVisible = true;
		}

		/// <summary>
		/// Invoked when the close button clicked in the sort option
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void CloseButtonClicked(object sender, EventArgs e)
		{
			this.sortOptionGrid.IsVisible = false;
			this.tempGrid.IsVisible = false;
			this.Graylayout.IsVisible = false;
			if (isFilterPage && !isAllListAdded)
			{
				if (newSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(tempWidth, true, true, this.filteredGridNewSample, this.filteredColumnOneLayoutNew, this.filteredColumnTwoLayoutNew, this.filteredColumnThreeLayoutNew);
				}
				else if (updatedSamples.IsChecked == true)
				{
					UpdateAllSortedColumn(tempWidth, true, false, this.filteredGridUpdatedSample, this.filteredColumnOneLayoutUpdated, this.filteredColumnTwoLayoutUpdated, this.filteredColumnThreeLayoutUpdated);
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

			if (this.BindingContext is SamplesViewModel viewModel)
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
			isSortPage = false;
			isFilterPage = false;
			UpdateColumn(tempWidth);
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
			List<string> filterList = new List<string>();
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
			isSortPage = true;
			isFilterPage = false;
			HideFilteredGrids();
			sortedGridScrollViewer.IsVisible = true;
			controlListGrid.IsVisible = false;
			controlListScrollViewer.IsVisible = false;
			sortedGrid.IsVisible = true;

			viewModel.GetSortedList(filterList);
			UpdateAllSortedColumn(tempWidth, true, false, sortedGrid, sortedcolumOneLayout, sortedcolumTwoLayout, sortedcolumThreeLayout);
		}

		private void ShowFilteredGrid(SamplesViewModel viewModel, List<string> filterList)
		{
			isSortPage = false;
			isFilterPage = true;

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
				UpdateAllSortedColumn(tempWidth, true, true, filteredGridNewSample, filteredColumnOneLayoutNew, filteredColumnTwoLayoutNew, filteredColumnThreeLayoutNew);
			}
			if (updatedSamples.IsChecked)
			{
				filteredGridUpdatedSample.IsVisible = true;
				UpdateAllSortedColumn(tempWidth, true, false, filteredGridUpdatedSample, filteredColumnOneLayoutUpdated, filteredColumnTwoLayoutUpdated, filteredColumnThreeLayoutUpdated);
			}
		}

		/// <summary>
		/// Invoked when the filter check boxes changed dynamically.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">CheckedChangedEventArgs</param>
		private void AllSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			if (!programmaticUpdate)
			{
				programmaticUpdate = true;
				newSamples.IsChecked = e.Value;
				updatedSamples.IsChecked = e.Value;
				programmaticUpdate = false;
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
			if (!programmaticUpdate)
			{
				programmaticUpdate = true;
				if (newSamplesChecked == false || updatedSamplesChecked == false)
				{
					allSamples.IsChecked = false;
				}
				programmaticUpdate = false;
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
				uri = new Uri(address);
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
				uri = new Uri(address);
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
			isThemePopupOpen = !isThemePopupOpen;
			this.themePopup.IsVisible = isThemePopupOpen;
			this.Graylayout.IsVisible = isThemePopupOpen;
			if (isThemePopupOpen)
			{
				this.themePopup.ZIndex = 1;
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
			this.sortOptionGrid.IsVisible = false;
			this.themePopup.IsVisible = false;
			this.isThemePopupOpen = false;
			this.Graylayout.IsVisible = false;
		}

		private void ThemePopupCloseIcon_Tapped(object sender, TappedEventArgs e)
		{
			this.isThemePopupOpen = false;
			this.themePopup.IsVisible = false;
			this.Graylayout.IsVisible = false;
		}


	}
}

