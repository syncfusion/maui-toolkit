using Syncfusion.Maui.ControlsGallery.CustomView;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.ControlsGallery
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MainPageAndroid : ContentPage
	{
		SampleCategoryModel? _previousSampleCategoryModel;
		SampleSubCategoryModel? _subCategoryModel;
		SampleView? _loadedSample;
		SampleModel? _loadedSampleModel;
		VerticalStackLayout? _verticalStackLayout;
		Uri? _uri;
		bool _isSampleLoadedByFilter;
		bool _programmaticUpdate;
		bool _isThemePopupOpen;
		static MainPageAndroid? _mainPage;

		/// <summary>
		/// Initializes the main page of the Android application.
		/// Sets up the initial state of the UI elements. Configures the theme based on the current application theme.
		/// </summary>
		public MainPageAndroid()
		{
			InitializeComponent();
			_mainPage = this;
			allSamples.IsChecked = true;

			if (Application.Current != null)
			{
				AppTheme currentTheme = Application.Current.RequestedTheme;
				themePopupSwitch.IsToggled = (currentTheme == AppTheme.Dark);
			}
		}

		/// <summary>
		/// Handles the back button press event, manages the visibility of various UI element and for handling back navigation within samples.
		/// </summary>
		/// <returns>True if the back action was handled, false otherwise.</returns>
		public static bool BackPressed()
		{
			if (_mainPage != null)
			{

				if (_mainPage.sortOptionGrid.IsVisible)
				{
					_mainPage.sortOptionGrid.IsVisible = false;
					_mainPage.sortTempGrid.IsVisible = false;
					return true;
				}

				if (_mainPage.searchEntryGrid.IsVisible)
				{
					_mainPage.searchEntryGrid.IsVisible = false;
					_mainPage.searchListGrid.IsVisible = false;
					return true;
				}

				if (_mainPage.NavigationDrawerGrid.IsVisible)
				{
					_mainPage.GraylayoutGrid();
					return true;
				}

				return HandleSampleBackNavigation();
			}
			return false;
		}

		/// <summary>
		/// Manages navigation when the back button is pressed within a sample.
		/// Handles different scenarios based on whether the sample was loaded by filter or not
		/// </summary>
		/// <returns>True if the back action was handled, false otherwise.</returns>
		static bool HandleSampleBackNavigation()
		{
			if (_mainPage != null)
			{
				if (_mainPage._isSampleLoadedByFilter)
				{
					if (_mainPage.popUpSamplePage.IsVisible)
					{
						_mainPage.UpdatePopUpPageUI(true);
						return true;
					}
					else if (_mainPage.properties.IsVisible)
					{
						_mainPage.HidePropertyWindow();
						return true;
					}
					else if (_mainPage.UpdatedSortCollection.IsVisible || _mainPage.sortAndFilteredGrid.IsVisible)
					{
						return false;
					}
					else if (!_mainPage.UpdatedSortCollection.IsVisible)
					{
						if (_mainPage.allSamples.IsChecked == true)
						{
							_mainPage.NavigateBackToFilterList();
							return true;
						}
						else
						{
							_mainPage.NavigateBackToSortPage();
							return true;
						}
					}
				}
				else
				{
					if (_mainPage.popUpSamplePage.IsVisible)
					{
						_mainPage.UpdatePopUpPageUI(true);
						return true;
					}
					else if (_mainPage.properties.IsVisible)
					{
						_mainPage.HidePropertyWindow();
						return true;
					}
					else if (!_mainPage.controlListPage.IsVisible)
					{
						_mainPage.NavigateBacktoControlsPage();
						return true;
					}
				}
			}

			return false;
		}


		/// <summary>
		/// Update the UI based on the new binding context.
		/// Ensures that the UI reflects the latest data from the view model.
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext != null)
			{
				ArrangeControlInColumn();
			}
		}

		/// <summary>
		/// Arranges the controls in a column layout based on the current view model and displays the list of controls in the main page.
		/// </summary>
		void ArrangeControlInColumn()
		{
			if (BindingContext is SamplesViewModel viewModel)
			{
				BindableLayout.SetItemsSource(columOneLayout, viewModel.AllControlCategories);
			}
		}

		/// <summary>
		/// Handles the tapped event for a control and manages visibility of various UI elements.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		async void Control_Tapped(object sender, EventArgs e)
		{
			busyIndicatorPage.IsVisible = false;
			popUpSamplePage.Children.Clear();
			sampleGridView.Children.Clear();

			if (_loadedSample != null)
			{
				_loadedSample = null;
			}

			tabViewRowDefinition.Height = 55;
			if (!shadowGrid.IsVisible)
			{
				shadowGrid.IsVisible = true;
			}

			if (!tabView.IsVisible)
			{
				tabView.IsVisible = true;
			}

			await Task.Delay(100);
			if (sender is not SfEffectsViewAdv control)
			{
				return;
			}

			control.ForceRemoveEffects();
			searchEntryGrid.IsVisible = false;
			await Task.Delay(200);
			HandleSampleCategories(control);
			busyIndicatorMainPage.IsVisible = false;

			if (UpdatedSortCollection.IsVisible)
			{
				UpdatedSortCollection.IsVisible = false;
			}
		}

		/// <summary>
		/// Processes the sample categories associated with the tapped control.
		/// Adjusts UI elements based on the number of categories.
		/// </summary>
		/// <param name="control">The control that was tapped.</param>
		void HandleSampleCategories(SfEffectsViewAdv? control)
		{
			if (control?.BindingContext is ControlModel controlObjectModel)
			{
				var sampleCategories = controlObjectModel.SampleCategories;
				if (sampleCategories != null && sampleCategories.Count == 1)
				{
					tabViewRowDefinition.Height = 0;
					tabView.IsVisible = false;
					shadowGrid.IsVisible = false;
				}

				if (sampleCategories != null)
				{
					sampleCategories[0].IsCategoryClicked = true;
					_previousSampleCategoryModel = sampleCategories[0];
					LoadSamplePage(controlObjectModel, sampleCategories[0]);
					tabView.SelectedIndex = 0;
				}
			}
		}

		/// <summary>
		/// Loads the sample page for a given control model and optional sample category.
		/// Initiates loading of the first sample in the category.
		/// </summary>
		/// <param name="controlModel">The control model to load.</param>
		/// <param name="sampleCategoryModel">Optional sample category model.</param>
		void LoadSamplePage(ControlModel controlModel, SampleCategoryModel? sampleCategoryModel = null)
		{
			sampleViewPage.BindingContext = controlModel;
			titleGrid.IsVisible = false;
			sampleTitleLabel.Text = controlModel.Title;
			sampleTitleGrid.IsVisible = true;
			controlListPage.IsVisible = false;
			sampleViewPage.IsVisible = true;
			if (sampleCategoryModel == null)
			{
				UpdateChipViewBindingContext(controlModel.SampleCategories![0]);
			}
			else
			{
				UpdateChipViewBindingContext(sampleCategoryModel);
			}

			chipScroll?.ScrollToAsync(0, 0, true);
			LoadSampleBasedOnCategory(controlModel.SampleCategories![0].SampleSubCategories![0]);
		}

		/// <summary>
		/// Checks if the SampleSubCategories collection of the given category has more than one item.
		/// </summary>
		/// <param name="sampleCategory">The sample category to check.</param>
		/// <returns>True if the category contains subcategories, false otherwise.</returns>
		static bool IsSampleCategoryContainsSubCategory(SampleCategoryModel sampleCategory)
		{
			return sampleCategory.SampleSubCategories?.Count > 1;
		}

		/// <summary>
		/// Sets the visibility and height of the chip view based on the presence of subcategories.
		/// Updates the chipView visibility and height, and sets its binding context if subcategories exist.
		/// </summary>
		/// <param name="sampleSubCategory">The sample subcategory to update the binding for.</param>
		void UpdateChipViewBindingContext(SampleCategoryModel? sampleSubCategory)
		{
			bool hasSubCategories = sampleSubCategory != null && IsSampleCategoryContainsSubCategory(sampleSubCategory);

			chipView.IsVisible = hasSubCategories;
			chipRowDefinition.Height = hasSubCategories ? 50 : 0;

			if (hasSubCategories)
			{
				chipView.BindingContext = sampleSubCategory;
			}
		}

		/// <summary>
		/// Handles the focus event of the search entry.Prepares the UI for the search functionality.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void Entry_Focused(object sender, FocusEventArgs e)
		{
			searchListGrid.IsVisible = true;
			searchedSampleScrollViewer.IsVisible = true;
		}

		/// <summary>
		/// Ensures proper UI state when returning to the filter list.
		/// </summary>
		void NavigateBackToFilterList()
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.CommonNavigation();
						this.UpdatedSortCollection.IsVisible = true;
			After:
						CommonNavigation();
						this.UpdatedSortCollection.IsVisible = true;
			*/
			CommonNavigation();
			UpdatedSortCollection.IsVisible = true;
		}

		/// <summary>
		/// Prepares the UI for displaying sorting options.
		/// </summary>
		void NavigateBackToSortPage()
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.CommonNavigation();
						this.sortAndFilteredGrid.IsVisible = true;
			After:
						CommonNavigation();
						this.sortAndFilteredGrid.IsVisible = true;
			*/
			CommonNavigation();
			sortAndFilteredGrid.IsVisible = true;
			searchedSampleView.IsVisible = false;
		}

		/// <summary>
		/// Resets the UI to its initial state, showing the list of all controls.
		/// </summary>
		void NavigateBacktoControlsPage()
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.CommonNavigation();
						this.controlListPage.IsVisible = true;
			After:
						CommonNavigation();
						this.controlListPage.IsVisible = true;
			*/
			CommonNavigation();
			controlListPage.IsVisible = true;
		}

		/// <summary>
		/// Resets various UI elements and clears the current sample.
		/// Prepares the UI for transitioning between different views.
		/// </summary>
		void CommonNavigation()
		{
			CallOnDisappearing();
			if (_loadedSampleModel != null)
			{
				_loadedSampleModel = null;
			}


			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						if (this.previousSampleCategoryModel != null)
						{
			After:
						if (previousSampleCategoryModel != null)
						{
			*/
			tabView.SelectedIndex = -1;
			if (_previousSampleCategoryModel != null)
			{
				_previousSampleCategoryModel.IsCategoryClicked = false;
			}

			sampleGridView.Children.Clear();
			titleGrid.IsVisible = true;
			sampleTitleGrid.IsVisible = false;
			sampleViewPage.IsVisible = false;
			searchSampleTitleGrid.IsVisible = false;
			searchListGrid.IsVisible = false;
			propertyTempGrid.IsVisible = false;
			properties.IsVisible = false;
			if (_subCategoryModel != null)
			{
				_subCategoryModel.IsSubCategoryClicked = false;
				_subCategoryModel = null;
			}
		}

		/// <summary>
		/// Handles the tapped event for a category and update the UI based on the selected category.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		async void Category_Tapped(object sender, EventArgs e)
		{
			var sampleCategoryModel = (sender as SfEffectsViewAdv)?.BindingContext as SampleCategoryModel;
			if (_previousSampleCategoryModel != null && sampleCategoryModel != null)
			{
				if (_previousSampleCategoryModel == sampleCategoryModel)
				{
					return;
				}

				_previousSampleCategoryModel.IsCategoryClicked = false;
			}

			busyIndicatorPage.IsVisible = true;
			await Task.Delay(200);
			HandleSampleSubcatergories(sampleCategoryModel);

			busyIndicatorPage.IsVisible = false;
		}

		/// <summary>
		/// Handles the sample subcategories for a given sample category model.
		/// Resets the state of previously selected subcategories if necessary.
		/// Updates the UI to reflect the newly selected category and its subcategories.
		/// </summary>
		/// <param name="sampleCategoryModel">The sample category model to handle.</param>
		void HandleSampleSubcatergories(SampleCategoryModel? sampleCategoryModel)
		{
			if (_subCategoryModel != null)
			{
				if (sampleCategoryModel != null && sampleCategoryModel.HasCategory)
				{
					if (sampleCategoryModel.SampleSubCategories != null && sampleCategoryModel.SampleSubCategories.Count > 0)
					{
						if (sampleCategoryModel.SampleSubCategories.Contains(_subCategoryModel))
						{
							return;
						}
					}
				}

				_subCategoryModel.IsSubCategoryClicked = false;
			}

			if (sampleCategoryModel != null)
			{
				sampleCategoryModel.IsCategoryClicked = true;
				_previousSampleCategoryModel = sampleCategoryModel;
				UpdateChipViewBindingContext(sampleCategoryModel);

				chipScroll?.ScrollToAsync(0, 0, true);
				LoadSampleBasedOnCategory(sampleCategoryModel.SampleSubCategories![0]);
			}
		}

		/// <summary>
		/// Loads a searched sample.
		/// Updates the UI to display the loaded sample and its associated elements.
		/// </summary>
		/// <param name="sampleModel">The sample model to load.</param>
		void LoadSearchedSample(SampleModel sampleModel)
		{
			_loadedSampleModel = sampleModel;
			try
			{
				var assemblyNameCollection = sampleModel?.AssemblyName?.FullName?.Split(",");
				var assemblyName = assemblyNameCollection![0] + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
				if (!BaseConfig.IsIndividualSB)
				{
					assemblyName = assemblyNameCollection[0] + "." + sampleModel?.ControlShortName + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
				}

				var sampleType = sampleModel?.AssemblyName?.GetType(assemblyName);
				searchedSampleSettingsImage.IsVisible = false;
				_loadedSample = Activator.CreateInstance(sampleType!) as SampleView;
				UpdateSearchedSampleUI(sampleModel);
				_loadedSample?.OnAppearing();
			}
			catch
			{

			}
		}

		/// <summary>
		/// Updates the UI for a searched sample.
		/// Clears and adds the loaded sample to the searchedSampleGrid.
		/// </summary>
		/// <param name="sampleModel">The sample model to update the UI with.</param>
		void UpdateSearchedSampleUI(SampleModel? sampleModel)
		{
			if (_loadedSample != null)
			{
				searchedSampleGrid.Children.Clear();
				searchedSampleGrid.Children.Add(_loadedSample);

				// Handle the option view if it exists
				if (_loadedSample.OptionView != null)
				{
					searchedSampleSettingsImage.IsVisible = true;
					optionViewGrid.Children.Clear();
					optionViewGrid.Children.Add(_loadedSample.OptionView);
					_loadedSample.OptionView = null; // Clear OptionView after using it
				}
				else
				{
					searchedSampleSettingsImage.IsVisible = false; // Hide if no option view
				}

				// Set visibility for the code viewer image
				searchedSampleCodeViewerImage.IsVisible = !string.IsNullOrEmpty(sampleModel?.CodeViewerPath);

				// Set visibility for the YouTube image
				searchedSampleYoutubeImage.IsVisible = !string.IsNullOrEmpty(sampleModel?.VideoLink);

				// Set visibility for the source link image
				searchedSampleSourceLinkImage.IsVisible = !string.IsNullOrEmpty(sampleModel?.SourceLink);
			}
		}

		/// <summary>
		/// Loads a sample and prepares the UI to display it.
		/// Creates an instance of the sample based on its assembly name and type and handles different scenarios for popup views and card layouts.
		/// </summary>
		/// <param name="sampleModel">The sample model to load.</param>
		/// <param name="parent">Optional parent layout for the sample.</param>
		/// <param name="isPopUpView">Whether the sample is loaded in a popup view.</param>
		/// <param name="isCardLayout">Whether the sample uses a card layout.</param>
		internal void LoadSample(SampleModel? sampleModel, CustomCardLayout? parent = null, bool isPopUpView = false, bool isCardLayout = false)
		{
			_loadedSampleModel = sampleModel;
			if (!busyIndicatorPage.IsVisible)
			{
				busyIndicatorPage.IsVisible = true;
			}

			// await Task.Delay(200);
			properties.Opacity = 0;
			try
			{
				var assemblyNameCollection = sampleModel?.AssemblyName?.FullName?.Split(",");
				var assemblyName = assemblyNameCollection![0] + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
				if (!BaseConfig.IsIndividualSB)
				{
					assemblyName = assemblyNameCollection[0] + "." + sampleModel?.ControlShortName + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
				}

				var sampleType = sampleModel?.AssemblyName?.GetType(assemblyName);

				/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
				Before:
								this.ResetSettings();
								if (!isPopUpView)
				After:
								ResetSettings();
								if (!isPopUpView)
				*/
				settingsImage.IsVisible = false;
				ResetSettings();
				if (!isPopUpView)
				{
					sampleGridView.Children.Clear();
				}

				popUpSamplePage.Children.Clear();
				_loadedSample = Activator.CreateInstance(sampleType!) as SampleView;
				if (_loadedSample != null)
				{
					_loadedSample.IsCardView = isCardLayout;
					_loadedSample.SetBusyIndicator(busyIndicatorPage);

					if (isPopUpView)
					{

						/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
						Before:
												this.UpdatePopUpPageUI();
												this.poptitleLabel.Text = sampleModel?.Title;
						After:
												UpdatePopUpPageUI();
												this.poptitleLabel.Text = sampleModel?.Title;
						*/
						UpdatePopUpPageUI();
						poptitleLabel.Text = sampleModel?.Title;
						UpdatePopupSampleUI(sampleModel);
					}
					else if (parent == null)
					{
						UpdateSampleUI(sampleModel);
					}
					else
					{
						codeViewerImage.IsVisible = false;
						parent.CardContent = _loadedSample;
					}
				}

				_loadedSample?.OnAppearing();
			}
			catch
			{
				busyIndicatorPage.IsVisible = false;
			}
		}

		/// <summary>
		/// Updates the sample UI based on the provided sample model.
		/// Configures visibility and width for various UI elements like code viewer and YouTube icon.
		/// </summary>
		/// <param name="sampleModel">The sample model to update the UI with.</param>
		void UpdateSampleUI(SampleModel? sampleModel)
		{
			// Update visibility and width for various UI elements
			MainPageAndroid.UpdateUIElementVisibility(codeViewerImage, sampleModel?.CodeViewerPath);
			MainPageAndroid.UpdateUIElementVisibility(youtubeIconGrid, sampleModel?.VideoLink, youtubeColumnWidth, 40);
			MainPageAndroid.UpdateUIElementVisibility(sourceLinkGrid, sampleModel?.SourceLink, sourceColumnWidth, 40);

			// Clear and add the option view if it exists
			optionViewGrid.Children.Clear();
			if (_loadedSample != null)
			{
				var optionView = _loadedSample.OptionView;
				_loadedSample.OptionView = null;

				if (optionView != null)
				{
					settingsImage.IsVisible = true;
					optionColumnWidth.Width = 50;
					optionViewGrid.Children.Add(optionView);
					SetInheritedBindingContext(optionView, _loadedSample.BindingContext);
				}

				// Add the loaded sample to the grid
				sampleGridView.Children.Add(_loadedSample);
			}
		}

		/// <summary>
		/// Updates the popup sample UI with the provided sample model.
		/// Configures visibility and width for popup-specific UI elements.
		/// </summary>
		/// <param name="sampleModel">The sample model to update the UI with.</param>
		void UpdatePopupSampleUI(SampleModel? sampleModel)
		{
			// Clear previous option view
			optionViewGrid.Children.Clear();

			// Temporarily store and nullify the option view
			if (_loadedSample != null)
			{
				var optionView = _loadedSample.OptionView;
				_loadedSample.OptionView = null;

				// Update the visibility and width for each popup element
				MainPageAndroid.UpdatePopupUIElementVisibility(youtubePopupImage, youtubePopupColumnWidth, sampleModel?.VideoLink);
				MainPageAndroid.UpdatePopupUIElementVisibility(sourcePopUpImage, sourcePopupColumnWidth, sampleModel?.SourceLink);
				MainPageAndroid.UpdatePopupUIElementVisibility(codeViewerPopUpImage, codeviewerPopupColumnWidth, sampleModel?.CodeViewerPath);

				// If option view exists, display it
				if (optionView != null)
				{
					optionPopupColumnWidth.Width = 40;
					settingsPopupImage.IsVisible = true;
					optionViewGrid.Children.Add(optionView);
					SetInheritedBindingContext(optionView, _loadedSample.BindingContext);
				}

				// Add the loaded sample to the popup page
				popUpSamplePage.Children.Add(_loadedSample);
			}
		}

		/// <summary>
		/// Updates the visibility of a UI element and optionally adjusts its column width.
		/// Sets the IsVisible property of the element and adjusts the Width of the column if provided.
		/// </summary>
		/// <param name="element">The UI element to update.</param>
		/// <param name="data">The data to determine visibility.</param>
		/// <param name="columnWidth">The optional column width to adjust.</param>
		/// <param name="widthValue">The width to set if the element is visible.</param>
		static void UpdateUIElementVisibility(VisualElement element, string? data, ColumnDefinition? columnWidth = null, double widthValue = 0)
		{
			bool isVisible = !string.IsNullOrEmpty(data);
			element.IsVisible = isVisible;

			if (columnWidth != null)
			{
				columnWidth.Width = isVisible ? widthValue : 0;
			}
		}

		/// <summary>
		/// Updates the visibility and column width of a popup element based on the given data.
		/// Sets the IsVisible property of the element and adjusts the Width of the column accordingly.
		/// </summary>
		/// <param name="element">The image control to update visibility for.</param>
		/// <param name="columnWidth">The column width to adjust.</param>
		/// <param name="data">The data to check for visibility.</param>
		static void UpdatePopupUIElementVisibility(Border element, ColumnDefinition columnWidth, string? data)
		{
			bool isVisible = !string.IsNullOrEmpty(data);
			element.IsVisible = isVisible;
			columnWidth.Width = isVisible ? 40 : 0;
		}

		/// <summary>
		/// Resets the settings for the UI elements and widths of multiple column definitions to their default values.
		/// </summary>
		void ResetSettings()
		{
			youtubePopupImage.IsVisible = false;
			sourcePopUpImage.IsVisible = false;
			youtubeIconGrid.IsVisible = false;
			sourceLinkGrid.IsVisible = false;

			optionColumnWidth.Width = 0;
			youtubeColumnWidth.Width = 0;
			sourceColumnWidth.Width = 0;
			whatsNewColumnWidth.Width = 0;
			youtubePopupColumnWidth.Width = 0;
			sourcePopupColumnWidth.Width = 0;
			whatsnewPopupColumnWidth.Width = 0;

		}

		/// <summary>
		/// Updates the UI for the popup page.
		/// Manages visibility of various UI elements based on whether the back button was pressed.
		/// </summary>
		/// <param name="isBackPressed">Whether the back button was pressed.</param>
		void UpdatePopUpPageUI(bool isBackPressed = false)
		{
			if (isBackPressed)
			{

				/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
				Before:
								this.CallOnDisappearing();
								this.sampleViewPage.IsVisible = true;
				After:
								CallOnDisappearing();
								this.sampleViewPage.IsVisible = true;
				*/
				CallOnDisappearing();
				sampleViewPage.IsVisible = true;
				CardPopUpViewTitleGrid.IsVisible = false;
				popUpSamplePage.IsVisible = false;
				poptitleLabel.Text = "";
				optionPopupColumnWidth.Width = 0;
				youtubePopupColumnWidth.Width = 0;
				sourcePopupColumnWidth.Width = 0;
				whatsnewPopupColumnWidth.Width = 0;
				settingsPopupImage.IsVisible = false;
				youtubePopupImage.IsVisible = false;
				sourcePopUpImage.IsVisible = false;
			}
			else
			{
				sampleViewPage.IsVisible = false;
				CardPopUpViewTitleGrid.IsVisible = true;
				popUpSamplePage.IsVisible = true;
			}
		}

		/// <summary>
		/// Handles the tapped event for a subcategory.
		/// Resets the state of previously selected subcategories if necessary.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		async void SubCategory_Tapped(object sender, EventArgs e)
		{
			var sampleSubCategory = (sender as SfEffectsViewAdv)?.BindingContext as SampleSubCategoryModel;
			if (sampleSubCategory != null && sampleSubCategory.IsSubCategoryClicked)
			{
				return;
			}

			busyIndicatorPage.IsVisible = true;

			if (_subCategoryModel != null)
			{
				_subCategoryModel.IsSubCategoryClicked = false;
			}

			await Task.Delay(200);

			if (sampleSubCategory != null)
			{
				LoadSampleBasedOnCategory(sampleSubCategory);
#if ANDROID
				if (sender is Element element)
				{
					chipScroll?.ScrollToAsync(element, ScrollToPosition.Center, true);
				}
#endif
			}
			busyIndicatorPage.IsVisible = false;
		}

		/// <summary>
		/// Loads a sample based on the selected category.
		/// Updates the UI to reflect the newly selected subcategory and its samples.
		/// </summary>
		/// <param name="sampleSubCategory">The subcategory model containing the sample(s) to load.</param>
		void LoadSampleBasedOnCategory(SampleSubCategoryModel sampleSubCategory)
		{
			if (_subCategoryModel != sampleSubCategory)
			{
				_subCategoryModel = sampleSubCategory;
				_subCategoryModel.IsSubCategoryClicked = true;
				if (sampleSubCategory != null && sampleSubCategory.CardLayouts != null)
				{
					var count = sampleSubCategory.CardLayouts.Count;
					if (count == 1)
					{
						var layout = sampleSubCategory.CardLayouts[0];
						var model = layout.Samples![0];
						if (_loadedSampleModel == null || model != _loadedSampleModel)
						{
							CallOnDisappearing();
							LoadSample(model);
						}
					}
					else
					{
						CallOnDisappearing();
						LoadCardView(sampleSubCategory);
					}
				}

			}
		}

		/// <summary>
		/// Calls the OnDisappearing method for visible samples.
		/// Ensures proper cleanup when navigating away from samples.
		/// </summary>
		void CallOnDisappearing()
		{
			if (!popUpSamplePage.IsVisible)
			{
				foreach (var item in sampleGridView)
				{
					if (item is SampleView sampleView)
					{
						sampleView.OnDisappearing();
					}
					else if (_verticalStackLayout?.Children.Count > 0)
					{
						foreach (var sample in _verticalStackLayout)
						{
							if (sample is CustomCardLayout cardLayout)
							{
								if (cardLayout.CardContent is SampleView cardContent)
								{
									cardContent.OnDisappearing();
								}
							}
						}
					}
				}
			}

			else if (popUpSamplePage.Children.Count > 0)
			{
				if (popUpSamplePage.Children[0] is SampleView sample)
				{
					sample.OnDisappearing();
				}
			}
		}

		/// <summary>
		/// Loads and displays a card view based on the subcategory's card layouts.
		/// Creates a scrollable layout containing cards for each sample in the subcategory.
		/// </summary>
		/// <param name="subCategory">The subcategory model containing the card layouts.</param>
		void LoadCardView(SampleSubCategoryModel subCategory)
		{
			sampleGridView.Children.Clear();
			ScrollView scroller = new();
			_verticalStackLayout = new VerticalStackLayout
			{
				Padding = 10,
				Spacing = 15
			};
			foreach (var item in subCategory.CardLayouts!)
			{
				AddToCardView(_verticalStackLayout, item.Samples![0]);
			}

			scroller.Content = _verticalStackLayout;
			sampleGridView.Children.Add(scroller);
		}

		/// <summary>
		/// Adds a new card layout for the specified sample model to the parent layout.
		/// Creates and configures a CustomCardLayout for the given sample.
		/// </summary>
		/// <param name="parent">The parent layout to add the card to.</param>
		/// <param name="sampleModel">The sample model that contains the data.</param>
		void AddToCardView(VerticalStackLayout parent, SampleModel sampleModel)
		{
			CustomCardLayout card = new(this, null, sampleModel.StatusTag!)
			{
				Title = sampleModel.Title!,
				SampleModel = sampleModel,
				ShowExpandIcon = sampleModel.ShowExpandIcon,
			};
			parent.Children.Add(card);
			LoadSample(sampleModel, card, false, true);
		}

		/// <summary>
		/// Handles the tap gesture for the search result item.
		/// Updates the UI to display the selected sample from the search results.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{

			searchedSampleScrollViewer.IsVisible = false;
			searchedSampleSettingsImage.IsVisible = false;
			searchedSampleCodeViewerImage.IsVisible = false;
			searchedSampleSourceLinkImage.IsVisible = false;
			searchedSampleYoutubeImage.IsVisible = false;
			searchedSampleView.IsVisible = true;
			if (sender is not SfEffectsViewAdv effectsView)
			{
				return;
			}

			effectsView.ForceRemoveEffects();

			var itemGrid = effectsView?.BindingContext;

			if (itemGrid is SearchModel searchModel && searchModel.Sample != null && searchModel.Control != null)
			{
				searchedSampleTitle.Text = searchModel.Sample.Title;
				searchSampleTitleLabel.Text = searchModel.Sample.ControlName;
				searchSampleTitleGrid.IsVisible = true;
				searchEntryGrid.IsVisible = false;
				LoadSearchedSample(searchModel.Sample);
			}
		}

		/// <summary>
		/// Handles the tap event for the search button.
		/// Prepares the UI for the search functionality by making relevant elements visible.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		async void Search_Tapped(object sender, EventArgs e)
		{
			searchEntryGrid.IsVisible = true;
			searchListGrid.IsVisible = true;
			searchListGrid.ZIndex = 1;
			searchedSampleScrollViewer.IsVisible = true;

			if (BindingContext is SamplesViewModel viewModel)
			{
				await Task.Delay(16);
				viewModel.PopulateSearchItem(string.Empty);
			}
		}

		/// <summary>
		/// Handles the tap gesture on the navigation drawer button.
		/// Makes the navigation drawer visible and brings it to the front of the UI.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		async void NavigationDrawer_Tapped(object sender, EventArgs e)
		{
			NavigationDrawerGrid.IsVisible = true;
			NavigationDrawerGrid.ZIndex = 1;
#if NET10_0
			await NavigationDrawerGrid.TranslateToAsync(0, 0, 250, Easing.CubicIn);
#else
			await NavigationDrawerGrid.TranslateTo(0, 0, 250, Easing.CubicIn);
#endif
			Graylayout.IsVisible = true;
		}

		/// <summary>
		/// Handles the tap event on the gray layout.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void Graylayout_Tapped(object sender, EventArgs e)
		{
			GraylayoutGrid();
		}

		/// <summary>
		/// Handles the closure of the theme popup and navigation drawer.
		/// Resets the state of the theme popup and hides the gray layout.
		/// </summary>
		async void GraylayoutGrid()
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.isThemePopupOpen = false;
						Graylayout.IsVisible = false;
			After:
						isThemePopupOpen = false;
						Graylayout.IsVisible = false;
			*/
			themePopup.IsVisible = false;
			_isThemePopupOpen = false;
			Graylayout.IsVisible = false;
#if NET10_0
			await NavigationDrawerGrid.TranslateToAsync(-345, 0, 250, Easing.CubicIn);
#else
			await NavigationDrawerGrid.TranslateTo(-345, 0, 250, Easing.CubicIn);
#endif
			NavigationDrawerGrid.IsVisible = false;
		}

		/// <summary>
		/// Handles the tap event on the navigation content grid.
		/// This method is intentionally left empty to prevent the navigation drawer from closing when its content is tapped.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void NavigationContentGrid_Tapped(object sender, EventArgs e)
		{
			//When the navigation drawer grid is tapped, the navigation content grid and close navigation grid should not be hidden, For that we have added this method.
		}

		/// <summary>
		/// Handles the back button press event.
		/// Determines the appropriate navigation action based on the current state.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void BackButtonPressed(object sender, EventArgs e)
		{
			if (_isSampleLoadedByFilter)
			{
				if (allSamples.IsChecked == true)
				{
					NavigateBackToFilterList();
				}
				else
				{
					NavigateBackToSortPage();
				}
			}
			else
			{
				NavigateBacktoControlsPage();
			}
		}

		/// <summary>
		/// Handles the back button press event for the popup page.
		/// Calls UpdatePopUpPageUI to close the popup and return to the previous view.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void PopUpPageBackButtonPressed(object sender, EventArgs e)
		{
			UpdatePopUpPageUI(true);
		}

		/// <summary>
		/// Handles the properties tab press event.
		/// Makes the properties panel visible and brings it to the front of the UI.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void PropertiesTabPressed(object sender, EventArgs e)
		{
			properties.IsVisible = true;
			properties.ZIndex = 1;
			properties.Opacity = 1;
			propertyTempGrid.IsVisible = true;
		}

		/// <summary>
		/// Handles the tap event for the code viewer.
		/// Constructs the GitHub URL for the sample's source code.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
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
		/// Calls HidePropertyWindow to hide the property window when the sample title is tapped.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void SampleTitleGridTapped(object sender, EventArgs e)
		{
			HidePropertyWindow();
		}

		/// <summary>
		/// Handles the tap event on the control list page temporary grid.
		/// Calls HideSearchWindow to hide the search window when tapped.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void ControlListPageTempGridTapped(object sender, EventArgs e)
		{
			HideSearchWindow();
		}

		/// <summary>
		/// Handles the tap event on the properties collapse image.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void PropertiesCollapeImageTapped(object sender, EventArgs e)
		{
			HidePropertyWindow();
		}

		/// <summary>
		/// Handles the tap event on the temporary grid.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void TempGridTapped(object sender, EventArgs e)
		{
			HidePropertyWindow();
		}

		/// <summary>
		/// Hides the property panel by setting its visibility to false.
		/// </summary>
		void HidePropertyWindow()
		{
			properties.IsVisible = false;
			propertyTempGrid.IsVisible = false;
		}

		/// <summary>
		/// Handles the click event of the sample search image.
		/// Hides both the property window and the search window.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void SampleSearchImage_Clicked(object sender, EventArgs e)
		{
			HidePropertyWindow();
			HideSearchWindow();
		}

		/// <summary>
		/// Hides the search entry and search list grid by setting its visibility to false.
		/// </summary>
		void HideSearchWindow()
		{
			searchEntryGrid.IsVisible = false;
			searchListGrid.IsVisible = false;
		}

		/// <summary>
		/// Makes the searched sample scroll viewer visible when text changes.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			searchedSampleScrollViewer.IsVisible = true;
		}

		/// <summary>
		/// Handles the YouTube icon tap event.
		/// Attempts to open the sample's YouTube video link in the device's default web browser.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
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
		/// Handles the source link tap event.
		/// Attempts to open the source link in the device's default web browser.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
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

		void WhatsNewTapped(object sender, EventArgs e)
		{

		}

		void WhatsNewTappedPopup(object sender, EventArgs e)
		{

		}

		void WhatsNewTappedSearchedSample(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Manages the visibility of sort temporary grid and sort option grid.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void Setting_Tapped(object sender, EventArgs e)
		{
			sortTempGrid.IsVisible = true;
			sortOptionGrid.IsVisible = true;
			sortOptionGrid.ZIndex = 1;
		}

		/// <summary>
		/// Handles the back icon press event from the sort option.
		/// Hides the sort option grid when the back icon is pressed.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void BackIcon_Pressed(object sender, EventArgs e)
		{
			sortOptionGrid.IsVisible = false;
		}

		/// <summary>
		/// Handles the apply button click event in the sort option.
		/// Applies the selected sorting and filtering options to the sample list.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void SortOptionApplyButtonClicked(object sender, EventArgs e)
		{
			_isSampleLoadedByFilter = false;
			sortOptionGrid.IsVisible = false;
			sortTempGrid.IsVisible = false;

			if (noneOption.IsChecked && ((newSamples.IsChecked == false && updatedSamples.IsChecked == false && allSamples.IsChecked == false) || allSamples.IsChecked == true))
			{
				controlListPage.IsVisible = true;
				UpdatedSortCollection.IsVisible = false;
				sortAndFilteredGrid.IsVisible = false;
				return;
			}

			List<string> filterList = [];
			if (BindingContext is SamplesViewModel viewModel)
			{
				ApplySortingAndFiltering(viewModel, filterList);
			}

			_isSampleLoadedByFilter = true;
		}

		/// <summary>
		/// This method handles the logic for sorting the list, populating filtered samples,
		/// and updating the visibility of various UI elements.
		/// </summary>
		/// <param name="viewModel">The view model containing the data and sorting/filtering methods.</param>
		/// <param name="filterList">A list to store the selected filter options.</param>
		void ApplySortingAndFiltering(SamplesViewModel viewModel, List<string> filterList)
		{
			viewModel.exit = true;
			SetSortOption(viewModel);
			if (newSamples.IsChecked == true)
			{
				filterList.Add("NewSamples");
			}

			if (updatedSamples.IsChecked == true)
			{
				filterList.Add("UpdatedSamples");
			}

			if (allSamples.IsChecked == true)
			{
				filterList.Add("AllSamples");
			}


			controlListPage.IsVisible = false;

			if (filterList.Contains("AllSamples") || filterList.Count == 0)
			{
				sortAndFilteredGrid.IsVisible = false;
				UpdatedSortCollection.IsVisible = true;
				viewModel.GetSortedList(filterList);
			}
			else
			{
				UpdatedSortCollection.IsVisible = false;
				sortAndFilteredGrid.IsVisible = true;
				viewModel.PopulateSortAndFilterSamples(filterList);
				UpdateFilteredGridsVisibility();
			}
			filterList.Clear();
		}

		/// <summary>
		/// Sets the sorting option for the view model based on the user's selection.
		/// </summary>
		/// <param name="viewModel">The view model to update with the selected sort option.</param>
		void SetSortOption(SamplesViewModel viewModel)
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
		/// Updates the visibility of filtered grids based on the selected filter options.
		/// This method ensures that the appropriate grids are displayed for new samples,
		/// updated samples, or both.
		/// </summary>
		void UpdateFilteredGridsVisibility()
		{
			if (newSamples.IsChecked == true && updatedSamples.IsChecked == false)
			{
				filteredGridNew.IsVisible = true;
				filteredGridUpdated.IsVisible = false;
			}
			else if (updatedSamples.IsChecked == true && newSamples.IsChecked == false)
			{
				filteredGridUpdated.IsVisible = true;
				filteredGridNew.IsVisible = false;
			}
			else
			{
				filteredGridNew.IsVisible = true;
				filteredGridUpdated.IsVisible = true;
			}
		}

		/// <summary>
		/// Updates the state of new and updated samples checkboxes to match the all samples checkbox.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The checkbox event arguments.</param>
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
		/// Handles the checkbox change event for new samples.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The checkbox event arguments.</param>
		void NewSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			HandleSampleCheckBoxChange(newSamples.IsChecked, updatedSamples.IsChecked);
		}

		/// <summary>
		/// Handles the checkbox change event for updated samples.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The checkbox event arguments.</param>
		void UpdatedSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
		{
			HandleSampleCheckBoxChange(newSamples.IsChecked, updatedSamples.IsChecked);
		}

		/// <summary>
		/// Handles the logic for updating the "All Samples" checkbox based on the state of other checkboxes.
		/// </summary>
		/// <param name="newSamplesChecked">The new state of the checkbox for new samples.</param>
		/// <param name="updatedSamplesChecked">The new state of the checkbox for updated samples.</param>
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
		/// Updates the UI to display the selected sample from the sorted list.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void SortSampleTapGestureTapped(object sender, EventArgs e)
		{
			if (BindingContext is SamplesViewModel viewModel)
			{
				viewModel.exit = true;
			}

			sortAndFilteredGrid.IsVisible = false;
			searchedSampleView.IsVisible = true;
			searchedSampleView.ZIndex = 1;
			searchListGrid.IsVisible = true;
			searchedSampleScrollViewer.IsVisible = false;
			if (sender is not SfEffectsViewAdv effectsView)
			{
				return;
			}

			effectsView.ForceRemoveEffects();

			var itemGrid = (effectsView?.BindingContext);

			if (itemGrid is SearchModel searchModel && searchModel.Sample != null && searchModel.Control != null)
			{
				searchedSampleTitle.Text = searchModel.Sample.Title;
				searchSampleTitleLabel.Text = searchModel.Sample.ControlName;
				searchSampleTitleGrid.IsVisible = true;
				LoadSearchedSample(searchModel.Sample);
			}
		}

		void SortOptionGridTapped(object sender, EventArgs e)
		{
			//When the sort option grid is tapped, the sort option grid and sort temp grid should not be hidden, For that we have added this method.
		}

		/// <summary>
		/// Hides the sort option grid and sort temporary grid.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void Cancel_Clicked(object sender, EventArgs e)
		{
			sortOptionGrid.IsVisible = false;
			sortTempGrid.IsVisible = false;
		}

		/// <summary>
		/// Temporary grid to make the background black color while sort option is visible.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void SortTempGridTapped(object sender, EventArgs e)
		{
			sortOptionGrid.IsVisible = false;
			sortTempGrid.IsVisible = false;
		}

		/// <summary>
		/// Handles the tap gesture on the documentation button.
		/// Attempts to open the documentation link in the device's default web browser.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		async void DocumentationTapGestureRecognizer(object sender, EventArgs e)
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
		/// Handles the tap gesture on the contact button.
		/// Attempts to open the contact link in the device's default web browser.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		async void ContactTapGestureRecognizer(object sender, EventArgs e)
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
		/// Invoked when the none option is tapped on the sort grid.
		/// Toggles the checked state of the none option.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void NoneOptionTapGestureTapped(object sender, EventArgs e)
		{
			noneOption.IsChecked = !noneOption.IsChecked;
		}

		/// <summary>
		/// Invoked when the ascending option is tapped on the sort grid.
		/// This allows users to select or deselect the ascending sorting option.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AscendingTapGestureTapped(object sender, EventArgs e)
		{
			ascending.IsChecked = !ascending.IsChecked;
		}

		/// <summary>
		/// Invoked when the descending option is tapped on the sort grid.
		/// This allows users to select or deselect the descending sorting option.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void DescendingTapGestureTapped(object sender, EventArgs e)
		{
			descending.IsChecked = !descending.IsChecked;
		}

		/// <summary>
		/// Handles the tap event on the close icon of the theme popup.
		/// Manages the visibility of theme popup.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void ThemePopupCloseIcon_Tapped(object sender, TappedEventArgs e)
		{
			_isThemePopupOpen = false;
			themePopup.IsVisible = false;
			Graylayout.IsVisible = false;
		}

		/// <summary>
		/// Handles the tap gesture on the theme toggle button.
		/// Ensures the navigation drawer is hidden when the theme popup is opened.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void ThemeTapGestureRecognizer(object sender, TappedEventArgs e)
		{
			if (_isThemePopupOpen == true)
			{
				_isThemePopupOpen = false;
				NavigationDrawerGrid.IsVisible = false;
				Graylayout.IsVisible = false;
				themePopup.IsVisible = false;
			}
			else
			{
				_isThemePopupOpen = true;
				NavigationDrawerGrid.IsVisible = false;
				Graylayout.IsVisible = true;
				themePopup.IsVisible = true;
			}
		}

		/// <summary>
		/// Handles the toggling of the theme switch.
		/// Updates the UserAppTheme of the application to match the selected theme.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The toggle event arguments.</param>
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


