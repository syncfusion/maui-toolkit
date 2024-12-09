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
		static MainPageAndroid? MainPage;

		/// <summary>
		/// 
		/// </summary>
		public MainPageAndroid()
		{
			InitializeComponent();
			MainPage = this;
			allSamples.IsChecked = true;

			if (Application.Current != null)
			{
				AppTheme currentTheme = Application.Current.RequestedTheme;
				themePopupSwitch.IsToggled = (currentTheme == AppTheme.Dark);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static bool BackPressed()
		{
			if (MainPage != null)
			{

				if (MainPage.sortOptionGrid.IsVisible)
				{
					MainPage.sortOptionGrid.IsVisible = false;
					MainPage.sortTempGrid.IsVisible = false;
					return true;
				}

				if (MainPage.searchEntryGrid.IsVisible)
				{
					MainPage.searchEntryGrid.IsVisible = false;
					MainPage.searchListGrid.IsVisible = false;
					return true;
				}

				if (MainPage.NavigationDrawerGrid.IsVisible)
				{
					MainPage.GraylayoutGrid();
					return true;
				}

				return HandleSampleBackNavigation();
			}
			return false;
		}

		private static bool HandleSampleBackNavigation()
		{
			if (MainPage != null)
			{
				if (MainPage._isSampleLoadedByFilter)
				{
					if (MainPage.popUpSamplePage.IsVisible)
					{
						MainPage.UpdatePopUpPageUI(true);
						return true;
					}
					else if (MainPage.properties.IsVisible)
					{
						MainPage.HidePropertyWindow();
						return true;
					}
					else if (MainPage.UpdatedSortCollection.IsVisible || MainPage.sortAndFilteredGrid.IsVisible)
					{
						return false;
					}
					else if (!MainPage.UpdatedSortCollection.IsVisible)
					{
						if (MainPage.allSamples.IsChecked == true)
						{
							MainPage.NavigateBackToFilterList();
							return true;
						}
						else
						{
							MainPage.NavigateBackToSortPage();
							return true;
						}
					}
				}
				else
				{
					if (MainPage.popUpSamplePage.IsVisible)
					{
						MainPage.UpdatePopUpPageUI(true);
						return true;
					}
					else if (MainPage.properties.IsVisible)
					{
						MainPage.HidePropertyWindow();
						return true;
					}
					else if (!MainPage.controlListPage.IsVisible)
					{
						MainPage.NavigateBacktoControlsPage();
						return true;
					}
				}
			}

			return false;
		}


		/// <summary>
		/// 
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext != null)
			{
				ArrangeControlInColumn();
			}
		}

		private void ArrangeControlInColumn()
		{
			if (BindingContext is SamplesViewModel viewModel)
			{
				BindableLayout.SetItemsSource(columOneLayout, viewModel.AllControlCategories);
			}
		}

		private async void Control_Tapped(object sender, EventArgs e)
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

		private void HandleSampleCategories(SfEffectsViewAdv? control)
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

		private void LoadSamplePage(ControlModel controlModel, SampleCategoryModel? sampleCategoryModel = null)
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

		private static bool IsSampleCategoryContainsSubCategory(SampleCategoryModel sampleCategory)
		{
			return sampleCategory.SampleSubCategories?.Count > 1;
		}

		private void UpdateChipViewBindingContext(SampleCategoryModel? sampleSubCategory)
		{
			bool hasSubCategories = sampleSubCategory != null && IsSampleCategoryContainsSubCategory(sampleSubCategory);

			chipView.IsVisible = hasSubCategories;
			chipRowDefinition.Height = hasSubCategories ? 50 : 0;

			if (hasSubCategories)
			{
				chipView.BindingContext = sampleSubCategory;
			}
		}

		private void Entry_Focused(object sender, FocusEventArgs e)
		{
			searchListGrid.IsVisible = true;
			searchedSampleScrollViewer.IsVisible = true;
		}
		/// <summary>
		/// Method to nagivate back to filter list page
		/// </summary>
		private void NavigateBackToFilterList()
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
		/// Method to nagivate back to sort page
		/// </summary>
		private void NavigateBackToSortPage()
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
		/// Method to navigate back to control list page which is default page
		/// </summary>
		private void NavigateBacktoControlsPage()
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
		/// Method needed for all the navigation
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

		private async void Category_Tapped(object sender, EventArgs e)
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

		private void HandleSampleSubcatergories(SampleCategoryModel? sampleCategoryModel)
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

		private void LoadSearchedSample(SampleModel sampleModel)
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

		private void UpdateSearchedSampleUI(SampleModel? sampleModel)
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
		/// </summary>
		/// <param name="sampleModel">The sample model to update the UI with.</param>
		private void UpdateSampleUI(SampleModel? sampleModel)
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
		/// </summary>
		/// <param name="sampleModel">The sample model to update the UI with.</param>
		private void UpdatePopupSampleUI(SampleModel? sampleModel)
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
		/// </summary>
		/// <param name="element">The UI element to update.</param>
		/// <param name="data">The data to determine visibility.</param>
		/// <param name="columnWidth">The optional column width to adjust.</param>
		/// <param name="widthValue">The width to set if the element is visible.</param>
		private static void UpdateUIElementVisibility(VisualElement element, string? data, ColumnDefinition? columnWidth = null, double widthValue = 0)
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
		/// </summary>
		/// <param name="element">The image control to update visibility for.</param>
		/// <param name="columnWidth">The column width to adjust.</param>
		/// <param name="data">The data to check for visibility.</param>
		private static void UpdatePopupUIElementVisibility(Border element, ColumnDefinition columnWidth, string? data)
		{
			bool isVisible = !string.IsNullOrEmpty(data);
			element.IsVisible = isVisible;
			columnWidth.Width = isVisible ? 40 : 0;
		}

		private void ResetSettings()
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

		private void UpdatePopUpPageUI(bool isBackPressed = false)
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

		private async void SubCategory_Tapped(object sender, EventArgs e)
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

		private void LoadSampleBasedOnCategory(SampleSubCategoryModel sampleSubCategory)
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

		private void CallOnDisappearing()
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

		private void LoadCardView(SampleSubCategoryModel subCategory)
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


		private void AddToCardView(VerticalStackLayout parent, SampleModel sampleModel)
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

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
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

		private async void Search_Tapped(object sender, EventArgs e)
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

		private async void NavigationDrawer_Tapped(object sender, EventArgs e)
		{
			NavigationDrawerGrid.IsVisible = true;
			NavigationDrawerGrid.ZIndex = 1;
			await NavigationDrawerGrid.TranslateTo(0, 0, 250, Easing.CubicIn);
			Graylayout.IsVisible = true;
		}

		private void Graylayout_Tapped(object sender, EventArgs e)
		{
			GraylayoutGrid();
		}

		private async void GraylayoutGrid()
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
			await NavigationDrawerGrid.TranslateTo(-345, 0, 250, Easing.CubicIn);
			NavigationDrawerGrid.IsVisible = false;
		}

		private void NavigationContentGrid_Tapped(object sender, EventArgs e)
		{
			//When the navigation drawer grid is tapped, the navigation content grid and close navigation grid should not be hidden, For that we have added this method.
		}

		private void BackButtonPressed(object sender, EventArgs e)
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

		private void PopUpPageBackButtonPressed(object sender, EventArgs e)
		{
			UpdatePopUpPageUI(true);
		}

		private void PropertiesTabPressed(object sender, EventArgs e)
		{
			properties.IsVisible = true;
			properties.ZIndex = 1;
			properties.Opacity = 1;
			propertyTempGrid.IsVisible = true;
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

		private void SampleTitleGridTapped(object sender, EventArgs e)
		{
			HidePropertyWindow();
		}

		private void ControlListPageTempGridTapped(object sender, EventArgs e)
		{
			HideSearchWindow();
		}

		private void PropertiesCollapeImageTapped(object sender, EventArgs e)
		{
			HidePropertyWindow();
		}

		private void TempGridTapped(object sender, EventArgs e)
		{
			HidePropertyWindow();
		}

		private void HidePropertyWindow()
		{
			properties.IsVisible = false;
			propertyTempGrid.IsVisible = false;
		}

		private void SampleSearchImage_Clicked(object sender, EventArgs e)
		{
			HidePropertyWindow();
			HideSearchWindow();
		}

		private void HideSearchWindow()
		{
			searchEntryGrid.IsVisible = false;
			searchListGrid.IsVisible = false;
		}

		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			searchedSampleScrollViewer.IsVisible = true;
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

		}

		private void WhatsNewTappedPopup(object sender, EventArgs e)
		{

		}

		private void WhatsNewTappedSearchedSample(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Invoked when the sort option settings clicked
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void Setting_Tapped(object sender, EventArgs e)
		{
			sortTempGrid.IsVisible = true;
			sortOptionGrid.IsVisible = true;
			sortOptionGrid.ZIndex = 1;
		}

		/// <summary>
		/// Method when the back button pressed from sort option
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void BackIcon_Pressed(object sender, EventArgs e)
		{
			sortOptionGrid.IsVisible = false;
		}

		/// <summary>
		/// Invoked when the apply button in the sort option clicked
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void SortOptionApplyButtonClicked(object sender, EventArgs e)
		{

			/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net8.0-android)'
			Before:
						this.IsSampleLoadedByFilter = false;
						this.sortOptionGrid.IsVisible = false;
			After:
						IsSampleLoadedByFilter = false;
						this.sortOptionGrid.IsVisible = false;
			*/
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
				viewModel.exit = true;
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

				filterList.Clear();
			}
			_isSampleLoadedByFilter = true;
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
		/// Invoked when the sorted sample tapped
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void SortSampleTapGestureTapped(object sender, EventArgs e)
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

		private void SortOptionGridTapped(object sender, EventArgs e)
		{
			//When the sort option grid is tapped, the sort option grid and sort temp grid should not be hidden, For that we have added this method.
		}

		/// <summary>
		/// Invoked when the cancel button in the sort option
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void Cancel_Clicked(object sender, EventArgs e)
		{
			sortOptionGrid.IsVisible = false;
			sortTempGrid.IsVisible = false;
		}

		/// <summary>
		/// Temporary grid to make the background black color while sort option is visible
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void SortTempGridTapped(object sender, EventArgs e)
		{
			sortOptionGrid.IsVisible = false;
			sortTempGrid.IsVisible = false;
		}

		private async void DocumentationTapGestureRecognizer(object sender, EventArgs e)
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

		private async void ContactTapGestureRecognizer(object sender, EventArgs e)
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
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NoneOptionTapGestureTapped(object sender, EventArgs e)
		{
			noneOption.IsChecked = !noneOption.IsChecked;
		}

		/// <summary>
		/// Invoked when the ascending option is tapped on the sort grid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AscendingTapGestureTapped(object sender, EventArgs e)
		{
			ascending.IsChecked = !ascending.IsChecked;
		}

		/// <summary>
		/// Invoked when the descending option is tapped on the sort grid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DescendingTapGestureTapped(object sender, EventArgs e)
		{
			descending.IsChecked = !descending.IsChecked;
		}

		private void ThemePopupCloseIcon_Tapped(object sender, TappedEventArgs e)
		{
			_isThemePopupOpen = false;
			themePopup.IsVisible = false;
			Graylayout.IsVisible = false;
		}

		private void ThemeTapGestureRecognizer(object sender, TappedEventArgs e)
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
	}
}


