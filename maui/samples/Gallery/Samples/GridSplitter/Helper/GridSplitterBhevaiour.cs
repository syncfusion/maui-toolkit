using  Syncfusion.Maui.Toolkit.GridSplitter;

namespace Syncfusion.Maui.ControlsGallery.GridSplitter;

public class GridSplitterBehaviour : Behavior<SampleView>
{
	DashboardViewModel? _viewModel;

	// Desktop named elements
	Entry? _searchBar;
	Microsoft.Maui.Controls.Picker? _regionPicker;
	Microsoft.Maui.Controls.Picker? _statusPicker;
	Microsoft.Maui.Controls.Picker? _typePicker;
	Button? _addCustomerButton;
	Button? _viewAllButton;
	Label? _closeDetailsIcon;
	CollectionView? _customersCollectionView;

	// Mobile named elements
	CollectionView? _photosCollectionView;

	// GridSplitter control
	Syncfusion.Maui.Toolkit.GridSplitter.SfGridSplitter? _gridSplitter;

	protected override void OnAttachedTo(SampleView bindable)
	{
		base.OnAttachedTo(bindable);

		// Reuse the XAML-set BindingContext when it is already a DashboardViewModel so that
		// inner <X.BindingContext> blocks (which the XAML keeps for compiled-binding support)
		// all resolve to the same VM instance and stay in sync.
		if (bindable.BindingContext is DashboardViewModel existing)
		{
			_viewModel = existing;
		}
		else
		{
			_viewModel = new DashboardViewModel();
			bindable.BindingContext = _viewModel;
		}

		// Desktop wiring (named elements may not exist on the mobile view)
		_searchBar = bindable.FindByName<Entry>("SearchBar");
		_regionPicker = bindable.FindByName<Microsoft.Maui.Controls.Picker>("RegionPicker");
		_statusPicker = bindable.FindByName<Microsoft.Maui.Controls.Picker>("StatusPicker");
		_typePicker = bindable.FindByName<Microsoft.Maui.Controls.Picker>("TypePicker");
		_addCustomerButton = bindable.FindByName<Button>("AddCustomerButton");
		_viewAllButton = bindable.FindByName<Button>("ViewAllButton");
		_closeDetailsIcon = bindable.FindByName<Label>("CloseDetailsIcon");
		_customersCollectionView = bindable.FindByName<CollectionView>("CustomersCollectionView");
		_photosCollectionView = bindable.FindByName<CollectionView>("PhotosCollectionView");
		_gridSplitter = bindable.FindByName<Syncfusion.Maui.Toolkit.GridSplitter.SfGridSplitter>("GridSplitter");

		if (_searchBar is not null)
		{
			_searchBar.TextChanged += OnSearchBarTextChanged;
		}

		if (_regionPicker is not null)
		{
			_regionPicker.SelectedIndexChanged += OnRegionPickerChanged;
		}

		if (_statusPicker is not null)
		{
			_statusPicker.SelectedIndexChanged += OnStatusPickerChanged;
		}

		if (_typePicker is not null)
		{
			_typePicker.SelectedIndexChanged += OnTypePickerChanged;
		}

		if (_addCustomerButton is not null)
		{
			_addCustomerButton.Clicked += OnAddCustomerClicked;
		}

		if (_viewAllButton is not null)
		{
			_viewAllButton.Clicked += OnViewAllClicked;
		}

		if (_closeDetailsIcon is not null)
		{
			TapGestureRecognizer closeTap = new TapGestureRecognizer();
			closeTap.Tapped += OnCloseDetailsTapped;
			_closeDetailsIcon.GestureRecognizers.Add(closeTap);
		}

		if (_customersCollectionView is not null)
		{
			_customersCollectionView.SelectionChanged += OnCustomersSelectionChanged;
		}

		if (_photosCollectionView is not null)
		{
			_photosCollectionView.SelectionChanged += OnPhotosSelectionChanged;
		}

		if (_gridSplitter is not null)
		{
			_gridSplitter.ResizeStopped += OnGridSplitterResizeStopped;
			_gridSplitter.Collapsed += OnGridSplitterPaneCollapsed;
		}
	}

	protected override void OnDetachingFrom(SampleView bindable)
	{
		if (_searchBar is not null)
		{
			_searchBar.TextChanged -= OnSearchBarTextChanged;
			_searchBar = null;
		}

		if (_regionPicker is not null)
		{
			_regionPicker.SelectedIndexChanged -= OnRegionPickerChanged;
			_regionPicker = null;
		}

		if (_statusPicker is not null)
		{
			_statusPicker.SelectedIndexChanged -= OnStatusPickerChanged;
			_statusPicker = null;
		}

		if (_typePicker is not null)
		{
			_typePicker.SelectedIndexChanged -= OnTypePickerChanged;
			_typePicker = null;
		}

		if (_addCustomerButton is not null)
		{
			_addCustomerButton.Clicked -= OnAddCustomerClicked;
			_addCustomerButton = null;
		}

		if (_viewAllButton is not null)
		{
			_viewAllButton.Clicked -= OnViewAllClicked;
			_viewAllButton = null;
		}

		if (_closeDetailsIcon is not null)
		{
			if (_closeDetailsIcon.GestureRecognizers.Count > 0)
			{
				_closeDetailsIcon.GestureRecognizers.Clear();
			}

			_closeDetailsIcon = null;
		}

		if (_customersCollectionView is not null)
		{
			_customersCollectionView.SelectionChanged -= OnCustomersSelectionChanged;
			_customersCollectionView = null;
		}

		if (_photosCollectionView is not null)
		{
			_photosCollectionView.SelectionChanged -= OnPhotosSelectionChanged;
			_photosCollectionView = null;
		}

		if (_gridSplitter is not null)
		{
			_gridSplitter.ResizeStopped -= OnGridSplitterResizeStopped;
			_gridSplitter.Collapsed -= OnGridSplitterPaneCollapsed;
			_gridSplitter = null;
		}

		_viewModel = null;
		base.OnDetachingFrom(bindable);
	}

	private void OnSearchBarTextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_viewModel is not null)
		{
			_viewModel.SearchText = e.NewTextValue ?? string.Empty;
		}
	}

	private void OnRegionPickerChanged(object? sender, EventArgs e)
	{
		if (_viewModel is not null && _regionPicker is not null)
		{
			_viewModel.SelectedRegion = _regionPicker.SelectedItem as string ?? "All Regions";
		}
	}

	private void OnStatusPickerChanged(object? sender, EventArgs e)
	{
		if (_viewModel is not null && _statusPicker is not null)
		{
			_viewModel.SelectedStatus = _statusPicker.SelectedItem as string ?? "All Status";
		}
	}

	private void OnTypePickerChanged(object? sender, EventArgs e)
	{
		if (_viewModel is not null && _typePicker is not null)
		{
			_viewModel.SelectedType = _typePicker.SelectedItem as string ?? "All Types";
		}
	}

	private void OnAddCustomerClicked(object? sender, EventArgs e)
	{
		if (_viewModel is null)
		{
			return;
		}

		int nextNumber = _viewModel.Customers.Count + 1001;
		var newCustomer = new Customer
		{
			Name = $"New Customer {nextNumber}",
			Type = "Standard",
			Initials = "NC",
			CustomerId = $"CUS-{nextNumber}",
			Email = $"new.customer{nextNumber}@demo.com",
			Phone = $"+1 (555) {nextNumber:000}-0000",
			Status = "Pending",
			Location = "San Francisco, CA, USA",
			TotalOrders = 0,
			TotalSpent = 0,
			JoinedDate = DateTime.Now,
			LastOrderDate = DateTime.Now,
			CustomerSinceDays = 0,
			Avatar = "person.png"
		};

		_viewModel.Customers.Add(newCustomer);
		_viewModel.FilteredCustomers.Add(newCustomer);
		_viewModel.SelectedCustomer = newCustomer;
	}

	private void OnViewAllClicked(object? sender, EventArgs e)
	{
		_viewModel?.ClearFilters();
	}

	private void OnCloseDetailsTapped(object? sender, TappedEventArgs e)
	{
		if (_viewModel is not null)
		{
			_viewModel.SelectedCustomer = null!;
		}
	}

	private void OnCustomersSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (_viewModel is not null && e.CurrentSelection.FirstOrDefault() is Customer customer)
		{
			_viewModel.SelectedCustomer = customer;
		}
	}

	private void OnPhotosSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (_viewModel is not null && e.CurrentSelection.FirstOrDefault() is PhotoItem photo)
		{
			_viewModel.SelectedPhoto = photo;
		}
	}

	private void OnGridSplitterResizeStopped(object? sender, GridSplitterResizeStoppedEventArgs e)
	{
		// Hook reserved for committing layout changes after a resize gesture completes.
	}

	private void OnGridSplitterPaneCollapsed(object? sender, GridSplitterPaneCollapsedEventArgs e)
	{
		// Hook reserved for reacting to a SplitterPane being collapsed.
	}

}
