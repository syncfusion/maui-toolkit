
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Syncfusion.Maui.ControlsGallery.CustomView;
using Syncfusion.Maui.Toolkit.Themes;
using Application = Microsoft.Maui.Controls.Application;
using ScrollView = Microsoft.Maui.Controls.ScrollView;

namespace Syncfusion.Maui.ControlsGallery;
/// <summary>
/// 
/// </summary>
public partial class MainPageiOS : ContentPage
{
    SampleCategoryModel? previousSampleCategoryModel;
    SampleSubCategoryModel? subCategoryModel;
    SampleView? loadedSample;
    SampleModel? loadedSampleModel;
    VerticalStackLayout? verticalStackLayout;
    Uri? uri;
    bool IsSampleLoadedByFilter = false;
    bool programmaticUpdate = false;
    bool isThemePopupOpen = false;

    /// <summary>
    /// 
    /// </summary>
    public MainPageiOS()
    {
        InitializeComponent();
        On<iOS>().SetUseSafeArea(true);

        if (Application.Current != null)
        {
            AppTheme currentTheme = Application.Current.RequestedTheme;
            themePopupSwitch.IsToggled = (currentTheme == AppTheme.Dark);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (this.BindingContext != null)
        {
            this.ArrangeControlInColumn();
        }
    }


    private void ArrangeControlInColumn()
    {
        if (this.BindingContext is SamplesViewModel viewModel)
        {
            BindableLayout.SetItemsSource(this.columOneLayout, viewModel.AllControlCategories);
        }
    }

    private async void Control_Tapped(object sender, EventArgs e)
    {
        this.busyIndicatorPage.IsVisible = false;
        this.popUpSamplePage.Children.Clear();
        this.sampleGridView.Children.Clear();

        if (loadedSample != null)
        {
            loadedSample = null;
        }

        this.tabViewRowDefinition.Height = 55;
        if (!this.shadowGrid.IsVisible)
        {
            this.shadowGrid.IsVisible = true;
        }
        if (!this.tabView.IsVisible)
        {
            this.tabView.IsVisible = true;
        }

        await Task.Delay(100);
        var control = (sender as SfEffectsViewAdv);
        if (control == null)
        {
            return;
        }

        control.ForceRemoveEffects();
        this.searchEntryGrid.IsVisible = false;
        await Task.Delay(200);
        this.HandleSampleCategories(control);
        this.busyIndicatorMainPage.IsVisible = false;
        if (this.UpdatedSortCollection.IsVisible)
        {
            this.UpdatedSortCollection.IsVisible = false;
        }
    }
    private void HandleSampleCategories(SfEffectsViewAdv? control)
    {
        var controlObjectModel = (control?.BindingContext as ControlModel);
        if (controlObjectModel != null)
        {
            var sampleCategories = controlObjectModel.SampleCategories;
            if (sampleCategories != null && sampleCategories.Count == 1)
            {
                this.tabViewRowDefinition.Height = 0;
                this.tabView.IsVisible = false;
                this.shadowGrid.IsVisible = false;
            }

            if(sampleCategories != null)
            {
                sampleCategories[0].IsCategoryClicked = true;
                previousSampleCategoryModel = sampleCategories[0];
                LoadSamplePage(controlObjectModel, sampleCategories[0]);
            }
        }
    }

    private void LoadSamplePage(ControlModel controlModel, SampleCategoryModel? sampleCategoryModel = null)
    {
        this.sampleViewPage.BindingContext = controlModel;
        this.titleGrid.IsVisible = false;
        this.sampleTitleLabel.Text = controlModel.Title;
        this.sampleTitleGrid.IsVisible = true;
        this.controlListPage.IsVisible = false;
        this.sampleViewPage.IsVisible = true;
        if (sampleCategoryModel == null)
        {
            UpdateChipViewBindingContext(controlModel.SampleCategories![0]);
        }
        else
        {
            UpdateChipViewBindingContext(sampleCategoryModel);
        }

        LoadSampleBasedOnCategory(controlModel.SampleCategories![0].SampleSubCategories![0]);
    }

    private static bool IsSampleCategoryContainsSubCategory(SampleCategoryModel sampleCategory)
    {
        return sampleCategory.SampleSubCategories?.Count > 1;
    }

    private void UpdateChipViewBindingContext(SampleCategoryModel? sampleSubCategory)
    {
        bool hasSubCategories = sampleSubCategory != null && IsSampleCategoryContainsSubCategory(sampleSubCategory);

        this.chipView.IsVisible = hasSubCategories;
        this.chipRowDefinition.Height = hasSubCategories ? 50 : 0;

        if (hasSubCategories)
        {
            this.chipView.BindingContext = sampleSubCategory;
        }
    }


    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        this.searchListGrid.IsVisible = true;
        this.searchedSampleScrollViewer.IsVisible = true;
    }

    private async void Category_Tapped(object sender, EventArgs e)
    {
        var sampleCategoryModel = ((sender as SfEffectsViewAdv)?.BindingContext as SampleCategoryModel);
        if (previousSampleCategoryModel != null)
        {
            if (sampleCategoryModel == previousSampleCategoryModel)
            {
                return;
            }
            previousSampleCategoryModel.IsCategoryClicked = false;
        }
        this.busyIndicatorPage.IsVisible = true;
        await Task.Delay(200);
       
        HandleSampleSubcatergories(sampleCategoryModel);
        this.busyIndicatorPage.IsVisible = false;
    }

    private void HandleSampleSubcatergories(SampleCategoryModel? sampleCategoryModel)
    {
        if (subCategoryModel != null)
        {
            if (sampleCategoryModel != null && sampleCategoryModel.HasCategory)
            {
                if (sampleCategoryModel.SampleSubCategories != null && sampleCategoryModel.SampleSubCategories.Count > 0)
                {
                    if (sampleCategoryModel.SampleSubCategories.Contains(subCategoryModel))
                    {
                        return;
                    }
                }
            }
            this.subCategoryModel.IsSubCategoryClicked = false;
        }

        if (sampleCategoryModel != null)
        {
            sampleCategoryModel.IsCategoryClicked = true;
            previousSampleCategoryModel = sampleCategoryModel;
            this.UpdateChipViewBindingContext(sampleCategoryModel);

            LoadSampleBasedOnCategory(sampleCategoryModel.SampleSubCategories![0]);
        }
    }

    private void LoadSearchedSample(SampleModel sampleModel)
    {
        loadedSampleModel = sampleModel;
        try
        {
            var assemblyNameCollection = sampleModel?.AssemblyName?.FullName?.Split(",");
            var assemblyName = assemblyNameCollection![0] + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
            if (!BaseConfig.IsIndividualSB)
            {
                assemblyName = assemblyNameCollection[0] + "." + sampleModel?.ControlShortName + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
            }
            var sampleType = sampleModel?.AssemblyName?.GetType(assemblyName);
            this.searchedSampleSettingsImage.IsVisible = false;
            loadedSample = Activator.CreateInstance(sampleType!) as SampleView;

            UpdateSearchedSampleUI(sampleModel);
            loadedSample?.OnAppearing();
        }
        catch
        {

        }
    }
    private void UpdateSearchedSampleUI(SampleModel? sampleModel)
    {
        if (loadedSample != null)
        {
            searchedSampleGrid.Children.Clear();
            searchedSampleGrid.Children.Add(loadedSample);

            // Handle the option view if it exists
            if (loadedSample.OptionView != null)
            {
                searchedSampleSettingsImage.IsVisible = true;
                optionViewGrid.Children.Clear();
                optionViewGrid.Children.Add(loadedSample.OptionView);
                loadedSample.OptionView = null; // Clear OptionView after using it
            }
            else
            {
                searchedSampleSettingsImage.IsVisible = false; // Hide if no option view
            }

            // Set visibility for the code viewer image
            searchedSampleViewerImage.IsVisible = !string.IsNullOrEmpty(sampleModel?.CodeViewerPath);

            // Set visibility for the YouTube image
            searchedSampleYoutubeImage.IsVisible = !string.IsNullOrEmpty(sampleModel?.VideoLink);

            // Set visibility for the source link image
            searchedSampleSourceLinkImage.IsVisible = !string.IsNullOrEmpty(sampleModel?.SourceLink);
        }
    }

    internal void LoadSample(SampleModel? sampleModel, CustomCardLayout? parent = null, bool isPopUpView = false,bool isCardView = false)
    {
        loadedSampleModel = sampleModel;
        if (!busyIndicatorPage.IsVisible)
        {
            busyIndicatorPage.IsVisible = true;
        }
        // await Task.Delay(200);
        this.properties.Opacity = 0;
        try
        {
            var assemblyNameCollection = sampleModel?.AssemblyName?.FullName?.Split(",");
            var assemblyName = assemblyNameCollection![0] + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
            if (!BaseConfig.IsIndividualSB)
            {
                assemblyName = assemblyNameCollection[0] + "." + sampleModel?.ControlShortName + "." + sampleModel?.ControlName + "." + sampleModel?.SampleName;
            }
            var sampleType = sampleModel?.AssemblyName?.GetType(assemblyName);
            this.settingsImage.IsVisible = false;
            this.ResetSettings();
            if (!isPopUpView)
            {
                this.sampleGridView.Children.Clear();
            }
            this.popUpSamplePage.Children.Clear();
            loadedSample = Activator.CreateInstance(sampleType!) as SampleView;
            if (loadedSample != null)
            {
                loadedSample.IsCardView = isCardView;
                loadedSample.SetBusyIndicator(this.busyIndicatorPage);

                if (isPopUpView)
                {
                   this.UpdatePopUpPageUI();
                    this.poptitleLabel.Text = sampleModel?.Title;
                    UpdatePopupSampleUI(sampleModel);
                }
                else if (parent == null)
                {
                    UpdateSampleUI(sampleModel);
                }
                else
                {
                    this.codeViewerImage.IsVisible = false;
                    parent.CardContent = loadedSample;
                }
            }
            
            loadedSample?.OnAppearing();
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
        UpdateUIElementVisibility(codeViewerImage, sampleModel?.CodeViewerPath);
        UpdateUIElementVisibility(youtubeIconGrid, sampleModel?.VideoLink, youtubeColumnWidth, 40);
        UpdateUIElementVisibility(sourceLinkGrid, sampleModel?.SourceLink, sourceColumnWidth, 40);

        // Clear and add the option view if it exists
        optionViewGrid.Children.Clear();
        if (loadedSample != null)
        {
            var optionView = loadedSample.OptionView;
            loadedSample.OptionView = null;

            if (optionView != null)
            {
                settingsImage.IsVisible = true;
                optionColumnWidth.Width = 50;
                optionViewGrid.Children.Add(optionView);
                SetInheritedBindingContext(optionView, loadedSample.BindingContext);
            }

            // Add the loaded sample to the grid
            sampleGridView.Children.Add(loadedSample);
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
        if (loadedSample != null)
        {
            var optionView = loadedSample.OptionView;
            loadedSample.OptionView = null;

            // Update the visibility and width for each popup element
            UpdatePopupUIElementVisibility(youtubePopupImage, youtubePopupColumnWidth, sampleModel?.VideoLink);
            UpdatePopupUIElementVisibility(sourcePopUpImage, sourcePopupColumnWidth, sampleModel?.SourceLink);
            UpdatePopupUIElementVisibility(codeViewerPopUpImage, codeviewerPopupColumnWidth, sampleModel?.CodeViewerPath);

            // If option view exists, display it
            if (optionView != null)
            {
                optionPopupColumnWidth.Width = 40;
                settingsPopupImage.IsVisible = true;
                optionViewGrid.Children.Add(optionView);
                SetInheritedBindingContext(optionView, loadedSample.BindingContext);
            }

            // Add the loaded sample to the popup page
            popUpSamplePage.Children.Add(loadedSample);
        }
    }
    /// <summary>
    /// Updates the visibility of a UI element and optionally adjusts its column width.
    /// </summary>
    /// <param name="element">The UI element to update.</param>
    /// <param name="data">The data to determine visibility.</param>
    /// <param name="columnWidth">The optional column width to adjust.</param>
    /// <param name="widthValue">The width to set if the element is visible.</param>
    private void UpdateUIElementVisibility(Microsoft.Maui.Controls.VisualElement element, string? data, ColumnDefinition? columnWidth = null, double widthValue = 0)
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
    private void UpdatePopupUIElementVisibility(Border element, ColumnDefinition columnWidth, string? data)
    {
        bool isVisible = !string.IsNullOrEmpty(data);
        element.IsVisible = isVisible;
        columnWidth.Width = isVisible ? 40 : 0;
    }
    private void ResetSettings()
    {
        this.youtubePopupImage.IsVisible = false;
        this.sourcePopUpImage.IsVisible = false;
        this.optionColumnWidth.Width = 0;
        this.youtubeColumnWidth.Width = 0;
        this.sourceColumnWidth.Width = 0;
        this.youtubePopupColumnWidth.Width = 0;
        this.sourcePopupColumnWidth.Width = 0;
    }

    private void UpdatePopUpPageUI(bool isBackPressed = false)
    {
        if (isBackPressed)
        {
            this.CallOnDisappearing();
            this.sampleViewPage.IsVisible = true;
            this.CardPopUpViewTitleGrid.IsVisible = false;
            this.popUpSamplePage.IsVisible = false;
            this.poptitleLabel.Text = "";
            this.optionPopupColumnWidth.Width = 0;
            this.settingsPopupImage.IsVisible = false;
            this.youtubePopupColumnWidth.Width = 0;
            this.youtubePopupImage.IsVisible = false;
            this.sourcePopupColumnWidth.Width = 0;
            this.sourcePopUpImage.IsVisible = false;
            this.whatsnewPopupColumnWidth.Width = 0;
        }
        else
        {
            this.sampleViewPage.IsVisible = false;
            this.CardPopUpViewTitleGrid.IsVisible = true;
            this.popUpSamplePage.IsVisible = true;
        }
    }

    private async void SubCategory_Tapped(object sender, EventArgs e)
    {
        var sampleSubCategory = (sender as SfEffectsViewAdv)?.BindingContext as SampleSubCategoryModel;
        if (sampleSubCategory != null && sampleSubCategory.IsSubCategoryClicked)
        {
            return;
        }
        this.busyIndicatorPage.IsVisible = true;

        if (subCategoryModel != null)
        {
            subCategoryModel.IsSubCategoryClicked = false;
        }

        await Task.Delay(200);

        if (sampleSubCategory != null)
        {
            LoadSampleBasedOnCategory(sampleSubCategory);
#if ANDROID
            if(sender is Element element)
            { 
                this.chipScroll?.ScrollToAsync(element, ScrollToPosition.Center, true);
            }
#endif
        }
        this.busyIndicatorPage.IsVisible = false;
    }

    private void LoadSampleBasedOnCategory(SampleSubCategoryModel sampleSubCategory)
    {
        if (subCategoryModel != sampleSubCategory)
        {
            subCategoryModel = sampleSubCategory;
            subCategoryModel.IsSubCategoryClicked = true;
            if (sampleSubCategory != null && sampleSubCategory.CardLayouts != null)
            {
                var count = sampleSubCategory.CardLayouts.Count;
                if (count == 1)
                {
                    var layout = sampleSubCategory.CardLayouts[0];
                    var model = layout.Samples![0];
                    if (loadedSampleModel == null || model != loadedSampleModel)
                    {
                        this.CallOnDisappearing();
                        LoadSample(model);
                    }
                }
                else
                {
                    this.CallOnDisappearing();
                    LoadCardView(sampleSubCategory);
                }
            }
            
        }
    }

    private void CallOnDisappearing()
    {
        if(!this.popUpSamplePage.IsVisible)
        {
            foreach (var item in this.sampleGridView)
            {
                if (item is SampleView sampleView)
                {
                    sampleView.OnDisappearing();
                }
                else if (this.verticalStackLayout?.Children.Count > 0)
                {
                    foreach (var sample in this.verticalStackLayout)
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
        else if(this.popUpSamplePage.Children.Count > 0)
        {
            if(this.popUpSamplePage.Children[0] is SampleView sample)
            {
                sample.OnDisappearing();
            }
        }
    }

    private void LoadCardView(SampleSubCategoryModel subCategory)
    {
        this.sampleGridView.Children.Clear();
        ScrollView scroller = new();
        verticalStackLayout = new VerticalStackLayout
        {
            Padding = 10,
            Spacing = 15
        };
        foreach (var item in subCategory.CardLayouts!)
        {
            AddToCardView(verticalStackLayout, item.Samples![0]);
        }
        scroller.Content = verticalStackLayout;
        this.sampleGridView.Children.Add(scroller);
    }


    private void AddToCardView(VerticalStackLayout parent, SampleModel sampleModel)
    {
        CustomCardLayout card = new(null, this, sampleModel.StatusTag!)
        {
            Title = sampleModel.Title!,
            SampleModel = sampleModel,
            ShowExpandIcon = sampleModel.ShowExpandIcon,
        };
        parent.Children.Add(card);
        LoadSample(sampleModel, card,false,true);
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        this.searchedSampleScrollViewer.IsVisible = false;
        this.searchedSampleSettingsImage.IsVisible = false;
        this.searchedSampleViewerImage.IsVisible = false;
        this.searchedSampleView.IsVisible = true;
        this.searchedSampleSourceLinkImage.IsVisible = false;
        this.searchedSampleYoutubeImage.IsVisible = false;
        if (sender is not SfEffectsViewAdv effectsView)
        {
            return;
        }
        effectsView.ForceRemoveEffects();

        var itemGrid = (effectsView?.BindingContext);
  
        if (itemGrid is SearchModel searchModel && searchModel.Sample != null && searchModel.Control != null)
        {
            this.searchedSampleTitle.Text = searchModel.Sample.Title;
            this.searchSampleTitleLabel.Text = searchModel.Sample.ControlName;
            this.searchSampleTitleGrid.IsVisible = true;
            this.searchEntryGrid.IsVisible = false;
            LoadSearchedSample(searchModel.Sample);
        }
    }

    private async void Search_Tapped(object sender, EventArgs e)
    {
        this.searchEntryGrid.IsVisible = true;
        this.searchListGrid.IsVisible = true;

        this.searchedSampleScrollViewer.IsVisible = true;

        if (this.BindingContext is SamplesViewModel viewModel)
        {
            await Task.Delay(16);
            viewModel.PopulateSearchItem(String.Empty);
        }
    }

    private async void NavigationDrawer_Tapped(object sender, EventArgs e)
    {
        NavigationDrawerGrid.IsVisible = true;
        NavigationDrawerGrid.ZIndex = 1;
        await NavigationDrawerGrid.TranslateTo(0, 0, 250, Easing.SinIn);
        Graylayout.IsVisible = true;
    }

    private void Graylayout_Tapped(object sender, EventArgs e)
    {
        NavigationDrawerGrid.TranslateTo(-500, 0, 250, Easing.SinIn);
        Graylayout.IsVisible = false;
        NavigationDrawerGrid.IsVisible = false;
        this.themePopup.IsVisible = false;
        this.isThemePopupOpen = false;
    }

    private void NavigationContentGrid_Tapped(object sender, EventArgs e)
    {
        //When the navigation drawer grid is tapped, the navigation content grid and close navigation grid should not be hidden, For that we have added this method.
    }


    private void BackButtonPressed(object sender, EventArgs e)
    {
        if(IsSampleLoadedByFilter)
        {
            this.NavigateBackToSortPage();
        }
        else
        {
            this.NavigateBacktoControlsPage();
        }
    }

    /// <summary>
    /// Method to navigate back to the filter list page.
    /// </summary>
    private void NavigateBackToFilterList()
    {
        this.CommonNavigation();
        this.UpdatedSortCollection.IsVisible = true;
    }

    /// <summary>
    /// Method to navigate back to sort page
    /// </summary>
    private void NavigateBackToSortPage()
    {
        this.CommonNavigation();
        this.sortAndFilteredGrid.IsVisible = true;
        this.searchedSampleView.IsVisible = false;
    }

    /// <summary>
    /// Method to navigate back to the controls page which is the default page
    /// </summary>
    private void NavigateBacktoControlsPage()
    {
        this.CommonNavigation();
        this.controlListPage.IsVisible = true;
    }

    /// <summary>
    /// Common method needed for all navigation
    /// </summary>
    void CommonNavigation()
    {
        this.CallOnDisappearing();
        if (this.loadedSampleModel != null)
        {
            this.loadedSampleModel = null;
        }

        if (this.previousSampleCategoryModel != null)
        {
            this.previousSampleCategoryModel.IsCategoryClicked = false;
        }
        this.sampleGridView.Children.Clear();
        this.titleGrid.IsVisible = true;
        this.sampleTitleGrid.IsVisible = false;
        this.sampleViewPage.IsVisible = false;
        this.searchSampleTitleGrid.IsVisible = false;
        this.searchListGrid.IsVisible = false;
        this.propertyTempGrid.IsVisible = false;
        this.properties.IsVisible = false;
        if (subCategoryModel != null)
        {
            this.subCategoryModel.IsSubCategoryClicked = false;
            this.subCategoryModel = null;
        }
    }

    private void PopUpPageBackButtonPressed(object sender, EventArgs e)
    {
        this.UpdatePopUpPageUI(true);
    }

    private void PropertiesTabPressed(object sender, EventArgs e)
    {
        this.properties.IsVisible = true;
        this.properties.ZIndex = 1;
        this.properties.Opacity = 1;
        this.propertyTempGrid.IsVisible = true;
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

 

    private void WhatsNewTapped(object sender, EventArgs e)
    {

    }

    private void WhatsNewTappedPopup(object sender, EventArgs e)
    {

    }

    private void WhatsNewTappedSearchedSample(object sender, EventArgs e)
    {

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
        this.properties.IsVisible = false;
        this.propertyTempGrid.IsVisible = false;
    }


    private void SampleSearchImage_Clicked(object sender, EventArgs e)
    {
        HidePropertyWindow();
        HideSearchWindow();
    }

    private void HideSearchWindow()
    {
        this.searchEntryGrid.IsVisible = false;
        this.searchListGrid.IsVisible = false;
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        this.searchedSampleScrollViewer.IsVisible = true;
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

    /// <summary>
    /// Invoked when the sort option settings clicked
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    private void Setting_Tapped(object sender, EventArgs e)
    {
        this.sortTempGrid.IsVisible = true;
        this.sortOptionGrid.IsVisible = true;
        this.sortOptionGrid.ZIndex = 1;
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
        this.IsSampleLoadedByFilter = false;
        this.sortOptionGrid.IsVisible = false;
        this.sortTempGrid.IsVisible = false;
        if (noneOption.IsChecked && ((newSamples.IsChecked == false && updatedSamples.IsChecked == false && allSamples.IsChecked == false) || allSamples.IsChecked == true))
        {
            this.controlListPage.IsVisible = true;
            this.UpdatedSortCollection.IsVisible = false;
            this.sortAndFilteredGrid.IsVisible = false;
            return;
        }
        List<string> filterList = new List<string>();
        if (this.BindingContext is SamplesViewModel viewModel)
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
            this.controlListPage.IsVisible = false;
            if (filterList.Contains("AllSamples") || filterList.Count == 0)
            {
                this.sortAndFilteredGrid.IsVisible = false;
                this.UpdatedSortCollection.IsVisible = true;
                viewModel.GetSortedList(filterList);
            }
            else
            {
                this.UpdatedSortCollection.IsVisible = false;
                this.sortAndFilteredGrid.IsVisible = true;               
                viewModel.PopulateSortAndFilterSamples(filterList);
                if (newSamples.IsChecked == true && updatedSamples.IsChecked == false)
                {
                    this.filteredGridNew.IsVisible = true;
                    this.filteredGridUpdated.IsVisible = false;
                }
                else if (updatedSamples.IsChecked == true && newSamples.IsChecked == false)
                {
                    this.filteredGridUpdated.IsVisible = true;
                    this.filteredGridNew.IsVisible = false;
                }
                else
                {
                    this.filteredGridNew.IsVisible = true;
                    this.filteredGridUpdated.IsVisible = true;
                }
            }
            filterList.Clear();
        }
        this.IsSampleLoadedByFilter = true;
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
    /// Invoked when the new Ssample check box changes dynamically.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">CheckedChangedEventArgs</param>
    private void NewSamplesCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        HandleSampleCheckBoxChange(newSamples.IsChecked, updatedSamples.IsChecked);
    }

    /// <summary>
    /// Invoked when the UpdateSample check boxes changes dynamically.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">CheckedChangedEventArgs</param>
    private void UpdatedSamplesCheckBoxChanged(object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
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
    /// Invoked when the sorted sample tapped
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    private void SortSampleTapGestureTapped(object sender, EventArgs e)
    {
        if (this.BindingContext is SamplesViewModel viewModel)
        {
            viewModel.exit = true;
        }
        this.sortAndFilteredGrid.IsVisible = false;
        this.searchedSampleView.IsVisible = true;
        this.searchedSampleView.ZIndex = 1;
        this.searchListGrid.IsVisible = true;
        this.searchedSampleScrollViewer.IsVisible = false;
        if (sender is not SfEffectsViewAdv effectsView)
        {
            return;
        }

        effectsView.ForceRemoveEffects();

        var itemGrid = (effectsView?.BindingContext);

        if (itemGrid is SearchModel searchModel && searchModel.Sample != null && searchModel.Control != null)
        {
            this.searchedSampleTitle.Text = searchModel.Sample.Title;
            this.searchSampleTitleLabel.Text = searchModel.Sample.ControlName;
            this.searchSampleTitleGrid.IsVisible = true;
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
        this.sortOptionGrid.IsVisible = false;
        this.sortTempGrid.IsVisible = false;
    }

    /// <summary>
    /// Temporary grid to make the background black color while sort option is visible
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    private void SortTempGridTapped(object sender, EventArgs e)
    {
        this.sortOptionGrid.IsVisible = false;
        this.sortTempGrid.IsVisible = false;
    }

    private async void DocumentationTapGestureRecognizer(object sender, EventArgs e)
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

    private async void ContactTapGestureRecognizer(object sender, EventArgs e)
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

    private void ThemeTapGestureRecognizer(object sender, EventArgs e)
    {
        // ThemeTapGestureRecognizer gets hit when tab the change theme in NavigationDrawerGrid
        if (isThemePopupOpen == true)
        {
            isThemePopupOpen = false;
            this.NavigationDrawerGrid.IsVisible = false;
            this.Graylayout.IsVisible = false;
            this.themePopup.IsVisible = false;

        }
        else
        {
            isThemePopupOpen = true;
            this.NavigationDrawerGrid.IsVisible = false;
            this.Graylayout.IsVisible = true;
            this.themePopup.IsVisible = true;
        }
    }

    private void ThemePopupCloseIcon_Tapped(object sender, TappedEventArgs e)
    {
        this.isThemePopupOpen = false;
        themePopup.IsVisible = false;
        this.Graylayout.IsVisible = false;
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

