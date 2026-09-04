using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Syncfusion.Maui.ControlsGallery.GridSplitter;
using System.Windows.Input;

public class DashboardViewModel : BaseViewModel
{
	private Customer? selectedCustomer;

	private PhotoItem? selectedPhoto;

	public ICommand MarkAsFavoriteCommand { get; }

	public ICommand ResetPhotoSelectionCommand { get; }

	public bool HasNoSelection => selectedPhoto is null;

	private string searchText = string.Empty;

	private bool showActive = true;
	private bool showInactive = true;
    private string selectedRegion = "All Regions";
    private string selectedStatus = "All Status";
    private string selectedType = "All Types";
	public ICommand ApplyFilterCommand { get; }
	public ICommand ClearFilterCommand { get; }
	public ObservableCollection<Customer> Customers { get; }

	public ObservableCollection<Customer> FilteredCustomers { get; }

    public ObservableCollection<ActivityItem> AllActivities { get; } = new();

    public ObservableCollection<ActivityItem> Activities { get; } = new();

    public ObservableCollection<PhotoItem> Photos { get; set; }
    public ObservableCollection<string> Regions { get; } =
    [
        "All Regions",
        "West",
        "East"
    ];

	public ObservableCollection<string> StatusList { get; } =
	[
		"All Status",
        "Active",
        "Inactive",
        "Pending"
	];

	public ObservableCollection<string> CustomerTypes { get; } =
	[
		"All Types",
        "Premium",
        "Standard",
        "Enterprise"
	];

    public static Color RegistrationColor => Application.Current!.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#2989F9") : Color.FromArgb("#3068F7"); // Blue
    public static Color OrderColor  => Application.Current!.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#EA8D03") : Color.FromArgb("#DE7207"); // Emerald
    public static Color UpdateColor  => Application.Current!.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#10B981") : Color.FromArgb("#059669"); // Amber
    private static Color PaymentColor => Application.Current!.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#9455FC") : Color.FromArgb("#8E4AFC"); // Violet
    private static Color SupportColor  => Application.Current!.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#EA8D03") : Color.FromArgb("#DE7207"); // Orange
    public static Color SuccessColor => Application.Current!.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#10AD4F") : Color.FromArgb("#4A9608");
    public static Color DangerColor  => Application.Current!.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#9B4848") : Color.FromArgb("#FF4E4E"); // Red
    public string CustomerCountText
    {
        get
        {
            return $"Customers ({FilteredCustomers.Count})";
        }
    }

    public Customer? SelectedCustomer
    {
        get => selectedCustomer;
        set
        {
            if (SetProperty(ref selectedCustomer, value))
            {
                LoadCustomerActivities();
            }
        }
    }

    public PhotoItem? SelectedPhoto
    {
        get => selectedPhoto;
        set
        {
            if (SetProperty(ref selectedPhoto, value))
            {
                OnPropertyChanged(nameof(HasNoSelection));

                if (value is not null)
                {
                    foreach (var photo in Photos)
                    {
                        photo.IsSelected = ReferenceEquals(photo, value);
                    }
                }
                else
                {
                    ClearPhotoSelection();
                }

                ((Command)MarkAsFavoriteCommand).ChangeCanExecute();
            }
        }
    }

	public string SearchText
	{
		get => searchText;
		set
		{
			if (SetProperty(ref searchText, value))
			{
				ApplyFilters();
			}
		}
	}



    public string SelectedRegion
    {
        get => selectedRegion;
        set
        {
            SetProperty(ref selectedRegion, value);
        }
    }

    public string SelectedStatus
    {
        get => selectedStatus;
        set
        {
            SetProperty(ref selectedStatus, value);
        }
    }

    public string SelectedType
    {
        get => selectedType;
        set
        {
            SetProperty(ref selectedType, value);
        }
    }

    public bool ShowActive
    {
        get => showActive;
        set
        {
            SetProperty(ref showActive, value);
        }
    }

    public bool ShowInactive
    {
        get => showInactive;
        set
        {
            SetProperty(ref showInactive, value);
        }
    }

	public DashboardViewModel()
	{
		Customers = new ObservableCollection<Customer>();

		FilteredCustomers = new ObservableCollection<Customer>();

		Activities = new ObservableCollection<ActivityItem>();

		LoadCustomers();

		LoadActivities();

		ApplyFilters();

		SelectedCustomer = FilteredCustomers.FirstOrDefault();

		LoadCustomerActivities();

		ApplyFilterCommand = new Command(ApplyFilters);

		ClearFilterCommand = new Command(ClearFilters);

		Photos = new ObservableCollection<PhotoItem>();

		LoadPhotos();

		MarkAsFavoriteCommand = new Command(MarkAsFavorite, () => SelectedPhoto is not null);

		ResetPhotoSelectionCommand = new Command(ResetPhotoSelection);
	}

	private void LoadPhotos()
	{
		Photos.Add(new PhotoItem
		{
			Image = "india.jpg",
			Name = "Taj Mahal",
			Title = "Taj Mahal at Sunrise",
			Description = "The Taj Mahal is an ivory-white marble mausoleum built by Mughal emperor Shah Jahan in memory of his wife Mumtaz Mahal. It is one of the most recognized landmarks in the world.",
			Dimensions = "6000 x 4000",
			CapturedDate = new DateTime(2024, 3, 14, 6, 32, 0),
			Location = "Agra, India",
			Photographer = "Ravi Kumar",
			Category = "World Heritage",
			FileSize = "12.4 MB"
		});

		Photos.Add(new PhotoItem
		{
			Image = "italy.jpg",
			Name = "Colosseum",
			Title = "Ancient Colosseum of Rome",
			Description = "The Colosseum is the largest ancient amphitheatre ever built and remains one of the greatest architectural achievements of the Roman Empire.",
			Dimensions = "5472 x 3648",
			CapturedDate = new DateTime(2024, 4, 2, 17, 10, 0),
			Location = "Rome, Italy",
			Photographer = "Ravi Kumar",
			Category = "Historical Landmark",
			FileSize = "9.8 MB"
		});

		Photos.Add(new PhotoItem
		{
			Image = "kremlin.jpg",
			Name = "Red Square",
			Title = "Saint Basil's Cathedral",
			Description = "Located in Moscow's Red Square, Saint Basil's Cathedral is famous for its colorful onion-shaped domes and distinctive Russian architecture.",
			Dimensions = "5184 x 3456",
			CapturedDate = new DateTime(2024, 1, 18, 8, 45, 0),
			Location = "Moscow, Russia",
			Photographer = "Ravi Kumar",
			Category = "Architecture",
			FileSize = "11.2 MB"
		});

		Photos.Add(new PhotoItem
		{
			Image = "germany.jpg",
			Name = "Historic Ruins",
			Title = "Historic Architecture",
			Description = "A dramatic view of a historic monument and sculpture showcasing Europe's rich architectural heritage.",
			Dimensions = "6000 x 4000",
			CapturedDate = new DateTime(2024, 5, 21, 19, 5, 0),
			Location = "Germany",
			Photographer = "Ravi Kumar",
			Category = "Heritage",
			FileSize = "14.6 MB"
		});

		Photos.Add(new PhotoItem
		{
			Image = "france.jpg",
			Name = "Eiffel Tower",
			Title = "Eiffel Tower in Paris",
			Description = "The Eiffel Tower is the most famous landmark in Paris and one of the most visited monuments in the world.",
			Dimensions = "4000 x 6000",
			CapturedDate = new DateTime(2024, 2, 9, 6, 30, 0),
			Location = "Paris, France",
			Photographer = "Ravi Kumar",
			Category = "Landmark",
			FileSize = "10.1 MB"
		});

		Photos.Add(new PhotoItem
		{
			Image = "canada.jpg",
			Name = "CN Tower",
			Title = "Toronto Skyline",
			Description = "The CN Tower dominates Toronto's skyline and remains one of Canada's most recognizable landmarks.",
			Dimensions = "7360 x 4912",
			CapturedDate = new DateTime(2024, 6, 5, 5, 50, 0),
			Location = "Toronto, Canada",
			Photographer = "Ravi Kumar",
			Category = "Cityscape",
			FileSize = "16.3 MB"
		});
	}

	private void ResetPhotoSelection()
    {
        SelectedPhoto = null;
    }

    private async void MarkAsFavorite()
    {
        if (SelectedPhoto is null)
            return;

        string fact = SelectedPhoto.Name switch
        {
            "Taj Mahal" =>
                "The Taj Mahal was completed in 1653 and is recognized as a UNESCO World Heritage Site.",

            "Colosseum" =>
                "The Colosseum could hold an estimated 50,000 spectators for gladiatorial contests and public events.",

            "Red Square" =>
                "Red Square has been the political and historical center of Moscow for centuries and is a UNESCO World Heritage Site.",

            "Historic Ruins" =>
                "Germany is home to more than 50 UNESCO World Heritage Sites, reflecting its rich cultural and historical heritage.",

            "Eiffel Tower" =>
                "The Eiffel Tower can grow by up to 15 centimeters during hot weather because iron expands when heated.",

            "CN Tower" =>
                "The CN Tower was the world's tallest free-standing structure from 1975 until 2007.",
                _ => "Interesting landmark."
        };

        var page = Application.Current?.Windows[0]?.Page;

        if (page != null)
        {
            await page.DisplayAlertAsync(
                "Did You Know?",
                fact,
                "OK");
        }
        
        SelectedPhoto = null;
    }
    
     private void ClearPhotoSelection()
    {
        foreach (var photo in Photos)
        {
            photo.IsSelected = false;
        }
    }

private string GetRegion(string location)
{
    var westCities = new[]
    {
        "San Francisco",
        "Los Angeles",
        "San Jose",
        "Sacramento",
        "Seattle",
        "Denver",
        "Austin",
        "Dallas",
        "San Antonio"
    };

    return westCities.Any(city => location.StartsWith(city))
        ? "West"
        : "East";
}
	private void LoadCustomers()
	{
		Customers.Add(new Customer
		{
			Name = "John Smith",
			Type = "Premium",
			Initials = "JS",
			CustomerId = "CUS-1001",
			Email = "john.smith@demo.com",
			Phone = "+1 (555) 123-4567",
			Status = "Active",
			Location = "San Francisco, CA, USA",
			TotalOrders = 24,
			TotalSpent = 54320,
			JoinedDate = new DateTime(2024, 5, 12),
			LastOrderDate = new DateTime(2024, 5, 28),
			CustomerSinceDays = 16,
			Avatar = "emp_01.png"
		});

		Customers.Add(new Customer
		{
			Name = "Sarah John",
			Type = "Standard",
			Initials = "SJ",
			CustomerId = "CUS-1002",
			Email = "sarah.j@demo.com",
			Phone = "+1 (555) 234-5678",
			Status = "Active",
			Location = "New York, NY, USA",
			TotalOrders = 17,
			TotalSpent = 32890,
			JoinedDate = new DateTime(2024, 5, 10),
			LastOrderDate = new DateTime(2024, 5, 25),
			CustomerSinceDays = 18,
			Avatar = "emp_03.png"
		});

		Customers.Add(new Customer
		{
			Name = "David Wilson",
			Initials = "DW",
			CustomerId = "CUS-1003",
			Email = "david.w@demo.com",
			Phone = "+1 (555) 345-6789",
			Type = "Standard",
            Status = "Inactive",
            Location = "Denver, CO, USA",
			TotalOrders = 5,
			TotalSpent = 4200,
			JoinedDate = new DateTime(2024, 5, 8),
			LastOrderDate = new DateTime(2024, 5, 12),
			CustomerSinceDays = 20,
			Avatar = "emp_02.png"
		});

		Customers.Add(new Customer
		{
			Name = "Mike Brown",
			Type = "Standard",
			Initials = "MB",
			CustomerId = "CUS-1004",
			Email = "mike.b@demo.com",
			Phone = "+1 (555) 456-7890",
			Status = "Active",
			Location = "Dallas, TX, USA",
			TotalOrders = 14,
			TotalSpent = 18200,
			JoinedDate = new DateTime(2024, 5, 6),
			LastOrderDate = new DateTime(2024, 5, 27),
			CustomerSinceDays = 22,
			Avatar = "emp_05.png"
		});

		Customers.Add(new Customer
		{
			Name = "Emma Davis",
			Type = "Enterprise",
			Initials = "ED",
			CustomerId = "CUS-1005",
			Email = "emma.d@demo.com",
			Phone = "+1 (555) 567-8901",
			Status = "Active",
			Location = "Boston, MA, USA",
			TotalOrders = 10,
			TotalSpent = 8500,
			JoinedDate = new DateTime(2024, 5, 4),
			LastOrderDate = new DateTime(2024, 5, 22),
			CustomerSinceDays = 24,
			Avatar = "emp_08.png"
		});

		Customers.Add(new Customer
		{
			Name = "James Taylor",
			Initials = "JT",
			CustomerId = "CUS-1006",
			Email = "james.t@demo.com",
			Phone = "+1 (555) 678-9012",
            Type = "Enterprise",
            Status = "Pending",
            Location = "Miami, FL, USA",
			TotalOrders = 30,
			TotalSpent = 60500,
			JoinedDate = new DateTime(2024, 5, 2),
			LastOrderDate = new DateTime(2024, 5, 29),
			CustomerSinceDays = 26,
			Avatar = "emp_06.png"
		});

		Customers.Add(new Customer
		{
			Name = "Olivia Martin",
			Type = "Enterprise",
			Initials = "OM",
			CustomerId = "CUS-1007",
			Email = "olivia.m@demo.com",
			Phone = "+1 (555) 789-0123",
			Status = "Active",
			Location = "Austin, TX, USA",
			TotalOrders = 12,
			TotalSpent = 14500,
			JoinedDate = new DateTime(2024, 4, 30),
			LastOrderDate = new DateTime(2024, 5, 20),
			CustomerSinceDays = 28,
			Avatar = "emp_09.png"
		});

		Customers.Add(new Customer
		{
			Name = "Sophia Lee",
			Initials = "SL",
			CustomerId = "CUS-1008",
			Email = "sophia.l@demo.com",
			Phone = "+1 (555) 901-2233",
            Type = "Standard",
            Status = "Inactive",
            Location = "Chicago, IL, USA",
			TotalOrders = 8,
			TotalSpent = 5500,
			JoinedDate = new DateTime(2024, 4, 28),
			LastOrderDate = new DateTime(2024, 5, 10),
			CustomerSinceDays = 30,
			Avatar = "peoplecircle9.png"
		});

		Customers.Add(new Customer
		{
			Name = "William Garcia",
			Type = "Standard",
			Initials = "WG",
			CustomerId = "CUS-1009",
			Email = "william.g@demo.com",
			Phone = "+1 (555) 443-8899",
			Status = "Active",
			Location = "Atlanta, GA, USA",
			TotalOrders = 18,
			TotalSpent = 26750,
			JoinedDate = new DateTime(2024, 4, 25),
			LastOrderDate = new DateTime(2024, 5, 24),
			CustomerSinceDays = 33,
			Avatar = "peoplecircle5.png"
		});

		Customers.Add(new Customer
		{
			Name = "Mia Tom",
			Type = "Premium",
			Initials = "MT",
			CustomerId = "CUS-1010",
			Email = "mia.t@demo.com",
			Phone = "+1 (555) 778-8899",
			Status = "Active",
			Location = "Seattle, WA, USA",
			TotalOrders = 21,
			TotalSpent = 35210,
			JoinedDate = new DateTime(2024, 4, 20),
			LastOrderDate = new DateTime(2024, 5, 26),
			CustomerSinceDays = 38,
			Avatar = "peoplecircle16.png"
		});

		Customers.Add(new Customer
		{
			Name = "Vincy Andrew",
			Type = "Premium",
			Initials = "VA",
			CustomerId = "CUS-1011",
			Email = "vincy.a@demo.com",
			Phone = "+1 (555) 101-2026",
			Status = "Active",
			Location = "Los Angeles, CA, USA",
			TotalOrders = 25,
			TotalSpent = 47200,
			JoinedDate = new DateTime(2024, 3, 12),
			LastOrderDate = new DateTime(2024, 5, 27),
			CustomerSinceDays = 77,
			Avatar = "peoplecircle16.png"
		});

		Customers.Add(new Customer
		{
			Name = "Noah Robert",
			Type = "Standard",
			Initials = "NR",
			CustomerId = "CUS-1012",
			Email = "noah.r@demo.com",
			Phone = "+1 (555) 102-2027",
			Status = "Active",
			Location = "San Antonio, TX, USA",
			TotalOrders = 12,
			TotalSpent = 13600,
			JoinedDate = new DateTime(2024, 3, 10),
			LastOrderDate = new DateTime(2024, 5, 24),
			CustomerSinceDays = 79,
			Avatar = "peoplecircle25.png"
		});

		Customers.Add(new Customer
		{
			Name = "Ava Mitchell",
			Initials = "AM",
			CustomerId = "CUS-1013",
			Email = "ava.m@demo.com",
			Phone = "+1 (555) 103-2028",
            Type = "Premium",
            Status = "Pending",
            Location = "Boston, MA, USA",
			TotalOrders = 17,
			TotalSpent = 22450,
			JoinedDate = new DateTime(2024, 3, 8),
			LastOrderDate = new DateTime(2024, 5, 18),
			CustomerSinceDays = 81,
			Avatar = "peoplecircle16.png"
		});

		Customers.Add(new Customer
		{
			Name = "Liam Turner",
			Type = "Premium",
			Initials = "LT",
			CustomerId = "CUS-1014",
			Email = "liam.t@demo.com",
			Phone = "+1 (555) 104-2029",
			Status = "Active",
			Location = "Tampa, FL, USA",
			TotalOrders = 29,
			TotalSpent = 58900,
			JoinedDate = new DateTime(2024, 3, 5),
			LastOrderDate = new DateTime(2024, 5, 29),
			CustomerSinceDays = 84,
			Avatar = "person1.png"
		});

		Customers.Add(new Customer
		{
			Name = "Emily Robin",
			Type = "Enterprise",
			Initials = "ER",
			CustomerId = "CUS-1015",
			Email = "emily.r@demo.com",
			Phone = "+1 (555) 105-2030",
			Status = "Inactive",
			Location = "Cleveland, OH, USA",
			TotalOrders = 4,
			TotalSpent = 2800,
			JoinedDate = new DateTime(2024, 3, 2),
			LastOrderDate = new DateTime(2024, 4, 30),
			CustomerSinceDays = 87,
			Avatar = "person.png"
		});

		Customers.Add(new Customer
		{
			Name = "Mason Hall",
			Type = "Enterprise",
			Initials = "MH",
			CustomerId = "CUS-1016",
			Email = "mason.h@demo.com",
			Phone = "+1 (555) 106-2031",
			Status = "Active",
			Location = "Indianapolis, IN, USA",
			TotalOrders = 15,
			TotalSpent = 19875,
			JoinedDate = new DateTime(2024, 2, 28),
			LastOrderDate = new DateTime(2024, 5, 22),
			CustomerSinceDays = 90,
			Avatar = "peoplecircle23.png"
		});

		Customers.Add(new Customer
		{
			Name = "Abigail Lewis",
			Type = "Premium",
			Initials = "AL",
			CustomerId = "CUS-1017",
			Email = "abigail.l@demo.com",
			Phone = "+1 (555) 107-2032",
			Status = "Active",
			Location = "Sacramento, CA, USA",
			TotalOrders = 22,
			TotalSpent = 41300,
			JoinedDate = new DateTime(2024, 2, 25),
			LastOrderDate = new DateTime(2024, 5, 28),
			CustomerSinceDays = 93,
			Avatar = "peoplecircle18.png"
		});

		Customers.Add(new Customer
		{
			Name = "Elijah Walker",
			Type = "Standard",
			Initials = "EW",
			CustomerId = "CUS-1018",
			Email = "elijah.w@demo.com",
			Phone = "+1 (555) 108-2033",
			Status = "Pending",
			Location = "Dallas, TX, USA", // East Region
			TotalOrders = 9,
			TotalSpent = 8750,
			JoinedDate = new DateTime(2024, 2, 22),
			LastOrderDate = new DateTime(2024, 5, 15),
			CustomerSinceDays = 96,
			Avatar = "peoplecircle17.png"
		});

		Customers.Add(new Customer
		{
			Name = "Ella Young",
			Type = "Enterprise",
			Initials = "EY",
			CustomerId = "CUS-1019",
			Email = "ella.y@demo.com",
			Phone = "+1 (555) 109-2034",
			Status = "Active",
			Location = "Pittsburgh, PA, USA",
			TotalOrders = 18,
			TotalSpent = 25780,
			JoinedDate = new DateTime(2024, 2, 20),
			LastOrderDate = new DateTime(2024, 5, 21),
			CustomerSinceDays = 98,
			Avatar = "peoplecircle25.png"
		});

		Customers.Add(new Customer
		{
			Name = "Logan King",
			Type = "Premium",
			Initials = "LK",
			CustomerId = "CUS-1020",
			Email = "logan.k@demo.com",
			Phone = "+1 (555) 110-2035",
			Status = "Active",
			Location = "Louisville, KY, USA",
			TotalOrders = 27,
			TotalSpent = 49850,
			JoinedDate = new DateTime(2024, 2, 20),
			LastOrderDate = new DateTime(2024, 5, 21),
			CustomerSinceDays = 98,
			Avatar = "peoplecircle26.png"
		});
        Customers.Add(new Customer
        {
            Name = "Ryan Cooper",
            Type = "Standard",
            Initials = "RC",
            CustomerId = "CUS-1021",
            Email = "ryan.c@demo.com",
            Phone = "+1 (555) 111-2036",
            Status = "Pending",
            Location = "Dallas, TX, USA",
            TotalOrders = 11,
            TotalSpent = 9800,
            JoinedDate = new DateTime(2024, 2, 18),
            LastOrderDate = new DateTime(2024, 5, 19),
            CustomerSinceDays = 100,
            Avatar = "peoplecircle5.png"
        });
        Customers.Add(new Customer
        {
            Name = "Grace Parker",
            Type = "Enterprise",
            Initials = "GP",
            CustomerId = "CUS-1022",
            Email = "grace.p@demo.com",
            Phone = "+1 (555) 112-2037",
            Status = "Pending",
            Location = "Austin, TX, USA",
            TotalOrders = 16,
            TotalSpent = 21450,
            JoinedDate = new DateTime(2024, 2, 15),
            LastOrderDate = new DateTime(2024, 5, 23),
            CustomerSinceDays = 103,
            Avatar = "peoplecircle9.png"
        });
        Customers.Add(new Customer
        {
            Name = "Daniel Moore",
            Type = "Enterprise",
            Initials = "DM",
            CustomerId = "CUS-1023",
            Email = "daniel.m@demo.com",
            Phone = "+1 (555) 113-2038",
            Status = "Inactive",
            Location = "San Jose, CA, USA",
            TotalOrders = 6,
            TotalSpent = 7200,
            JoinedDate = new DateTime(2024, 2, 14),
            LastOrderDate = new DateTime(2024, 4, 18),
            CustomerSinceDays = 104,
            Avatar = "emp_07.png"
        });
        Customers.Add(new Customer
        {
            Name = "Nathan Scott",
            Type = "Premium",
            Initials = "NS",
            CustomerId = "CUS-1024",
            Email = "nathan.s@demo.com",
            Phone = "+1 (555) 114-2039",
            Status = "Inactive",
            Location = "Denver, CO, USA",
            TotalOrders = 8,
            TotalSpent = 12500,
            JoinedDate = new DateTime(2024, 2, 12),
            LastOrderDate = new DateTime(2024, 4, 25),
            CustomerSinceDays = 106,
            Avatar = "peoplecircle18.png"
        });
        Customers.Add(new Customer
        {
            Name = "Chloe Adams",
            Type = "Premium",
            Initials = "CA",
            CustomerId = "CUS-1025",
            Email = "chloe.a@demo.com",
            Phone = "+1 (555) 115-2040",
            Status = "Pending",
            Location = "Sacramento, CA, USA",
            TotalOrders = 13,
            TotalSpent = 18650,
            JoinedDate = new DateTime(2024, 2, 10),
            LastOrderDate = new DateTime(2024, 5, 12),
            CustomerSinceDays = 108,
            Avatar = "peoplecircle26.png"
        });
        Customers.Add(new Customer
        {
            Name = "Oliver Brooks",
            Type = "Premium",
            Initials = "OB",
            CustomerId = "CUS-1026",
            Email = "oliver.b@demo.com",
            Phone = "+1 (555) 116-2041",
            Status = "Inactive",
            Location = "Louisville, KY, USA",
            TotalOrders = 7,
            TotalSpent = 9100,
            JoinedDate = new DateTime(2024, 2, 8),
            LastOrderDate = new DateTime(2024, 4, 20),
            CustomerSinceDays = 110,
            Avatar = "peoplecircle23.png"
        });
        Customers.Add(new Customer
        {
            Name = "Sophie Clark",
            Type = "Standard",
            Initials = "SC",
            CustomerId = "CUS-1027",
            Email = "sophie.c@demo.com",
            Phone = "+1 (555) 117-2042",
            Status = "Pending",
            Location = "Richmond, VA, USA",
            TotalOrders = 10,
            TotalSpent = 9850,
            JoinedDate = new DateTime(2024, 2, 5),
            LastOrderDate = new DateTime(2024, 5, 14),
            CustomerSinceDays = 113,
            Avatar = "peoplecircle16.png"
        });
	}

	private void ApplyFilters()
    {
        FilteredCustomers.Clear();

       var filtered = Customers.Where(c =>
    (SelectedRegion == "All Regions" ||
     GetRegion(c.Location) == SelectedRegion) &&
    (SelectedStatus == "All Status" ||
     c.Status == SelectedStatus) &&
    (SelectedType == "All Types" ||
     c.Type == SelectedType));

        foreach (var customer in filtered)
        {
            FilteredCustomers.Add(customer);
        }

        if (FilteredCustomers.Any())
        {
            SelectedCustomer = FilteredCustomers.First();
        }

        OnPropertyChanged(nameof(CustomerCountText));
    }

    internal void ClearFilters()
    {
        SearchText = string.Empty;

        SelectedRegion = "All Regions";

        SelectedStatus = "All Status";

        SelectedType = "All Types";

        ApplyFilters();
    }

    private void LoadActivities()
    {
        AllActivities.Clear();

        // CUS-1001

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1001",
            Title = "Customer Registered",
            Description = "John Smith has been registered as a new customer.",
            Time = new DateTime(2024, 5, 12, 9, 15, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1001",
            Title = "Order Placed",
            Description = "Order #ORD-1024 has been placed.",
            Time = new DateTime(2024, 5, 12, 10, 20, 0),
            Icon = "\ue73e",
            Color = SuccessColor
        });

        // CUS-1002

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1002",
            Title = "Profile Updated",
            Description = "Contact information updated.",
            Time = new DateTime(2024, 5, 14, 11, 00, 0),
            Icon = "\ue718",
            Color = UpdateColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1002",
            Title = "Feedback Submitted",
            Description = "Feedback submitted successfully.",
            Time = new DateTime(2024, 5, 15, 15, 45, 0),
            Icon = "\ue725",
            Color = SuccessColor
        });

        // CUS-1003

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1003",
            Title = "Customer Registered",
            Description = "Account successfully created.",
            Time = new DateTime(2024, 5, 2, 8, 30, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1003",
            Title = "Support Ticket Created",
            Description = "Issue reported regarding billing.",
            Time = new DateTime(2024, 5, 3, 13, 10, 0),
            Icon = "\ue77b",
            Color = SupportColor
        });

        // CUS-1004

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1004",
            Title = "Payment Received",
            Description = "Payment of $850 received.",
            Time = new DateTime(2024, 5, 6, 11, 20, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1004",
            Title = "Order Delivered",
            Description = "Latest purchase delivered.",
            Time = new DateTime(2024, 5, 9, 17, 30, 0),
            Icon = "\ue722",
            Color = SuccessColor
        });

        // CUS-1005

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1005",
            Title = "Customer Registered",
            Description = "New account created.",
            Time = new DateTime(2024, 5, 7, 9, 00, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1005",
            Title = "Payment Received",
            Description = "Payment confirmed.",
            Time = new DateTime(2024, 5, 8, 16, 40, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1006

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1006",
            Title = "Order Placed",
            Description = "Order #ORD-1106 created.",
            Time = new DateTime(2024, 5, 12, 14, 00, 0),
            Icon = "\ue73e",
            Color = SuccessColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1006",
            Title = "Support Ticket Created",
            Description = "Product enquiry created.",
            Time = new DateTime(2024, 5, 13, 9, 10, 0),
            Icon = "\ue77b",
            Color = SupportColor
        });

        // CUS-1007

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1007",
            Title = "Profile Updated",
            Description = "Location updated.",
            Time = new DateTime(2024, 5, 16, 12, 12, 0),
            Icon = "\ue718",
            Color = UpdateColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1007",
            Title = "Order Delivered",
            Description = "Order successfully delivered.",
            Time = new DateTime(2024, 5, 18, 18, 15, 0),
            Icon = "\ue718",
            Color = SuccessColor
        });

        // CUS-1008

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1008",
            Title = "Customer Registered",
            Description = "Account opened.",
            Time = new DateTime(2024, 5, 9, 10, 20, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1008",
            Title = "Account Inactivated",
            Description = "Customer account moved to inactive state.",
            Time = new DateTime(2024, 5, 20, 10, 55, 0),
            Icon = "\ue71c",
            Color = DangerColor
        });

        // CUS-1009

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1009",
            Title = "Order Placed",
            Description = "Enterprise order processed.",
            Time = new DateTime(2024, 5, 22, 8, 45, 0),
            Icon = "\ue73e",
            Color = SuccessColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1009",
            Title = "Payment Received",
            Description = "$4,200 payment received.",
            Time = new DateTime(2024, 5, 22, 13, 30, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1010

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1010",
            Title = "Customer Registered",
            Description = "Customer onboarded successfully.",
            Time = new DateTime(2024, 5, 24, 11, 30, 0),
            Icon = "\ue760",
            Color = PaymentColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1010",
            Title = "Feedback Submitted",
            Description = "Positive feedback received.",
            Time = new DateTime(2024, 5, 30, 15, 20, 0),
            Icon = "✓",
            Color = SuccessColor
        });

        // CUS-1011

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1011",
            Title = "Order Placed",
            Description = "Order #ORD-2101 created.",
            Time = new DateTime(2024, 6, 1, 9, 0, 0),
            Icon = "\ue73e",
            Color = SuccessColor
        });

        // CUS-1012

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1012",
            Title = "Payment Received",
            Description = "Invoice payment completed.",
            Time = new DateTime(2024, 6, 2, 14, 10, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1013

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1013",
            Title = "Support Ticket Created",
            Description = "Issue reported by customer.",
            Time = new DateTime(2024, 6, 3, 11, 45, 0),
            Icon = "\ue77b",
            Color = SupportColor
        });

        // CUS-1014

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1014",
            Title = "Order Delivered",
            Description = "Shipment delivered successfully.",
            Time = new DateTime(2024, 6, 5, 17, 25, 0),
            Icon = "\ue718",
            Color = SuccessColor
        });

        // CUS-1015

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1015",
            Title = "Customer Registered",
            Description = "New enterprise customer onboarded.",
            Time = new DateTime(2024, 6, 7, 10, 10, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1015",
            Title = "Payment Received",
            Description = "$8,500 payment received.",
            Time = new DateTime(2024, 6, 8, 16, 50, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1011
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1011",
            Title = "Payment Received",
            Description = "Payment of $4,720 received successfully.",
            Time = new DateTime(2024, 6, 1, 14, 20, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1012
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1012",
            Title = "Order Delivered",
            Description = "Customer order delivered successfully.",
            Time = new DateTime(2024, 6, 3, 17, 30, 0),
            Icon = "\ue718",
            Color = SuccessColor
        });

        // CUS-1013
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1013",
            Title = "Profile Updated",
            Description = "Business information updated.",
            Time = new DateTime(2024, 6, 4, 15, 10, 0),
            Icon = "\ue760",
            Color = UpdateColor
        });

        // CUS-1014
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1014",
            Title = "Payment Received",
            Description = "Premium package payment completed.",
            Time = new DateTime(2024, 6, 6, 11, 25, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1015
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1015",
            Title = "Account Inactivated",
            Description = "Customer account marked as inactive.",
            Time = new DateTime(2024, 6, 10, 9, 45, 0),
            Icon = "\ue71c",
            Color = DangerColor
        });

        // CUS-1016
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1016",
            Title = "Order Placed",
            Description = "Enterprise order created successfully.",
            Time = new DateTime(2024, 6, 11, 10, 30, 0),
            Icon = "\ue73e",
            Color = SupportColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1016",
            Title = "Payment Received",
            Description = "Payment of $19,875 recorded.",
            Time = new DateTime(2024, 6, 12, 14, 15, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1017
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1017",
            Title = "Order Placed",
            Description = "Premium order submitted.",
            Time = new DateTime(2024, 6, 13, 9, 20, 0),
            Icon = "\ue73e",
            Color = SuccessColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1017",
            Title = "Order Delivered",
            Description = "Order delivered to customer location.",
            Time = new DateTime(2024, 6, 14, 16, 45, 0),
            Icon = "\ue718",
            Color = SuccessColor
        });

        // CUS-1018
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1018",
            Title = "Support Ticket Created",
            Description = "Question raised about shipment timeline.",
            Time = new DateTime(2024, 6, 15, 13, 5, 0),
            Icon = "\ue77b",
            Color = SupportColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1018",
            Title = "Ticket Resolved",
            Description = "Support request resolved successfully.",
            Time = new DateTime(2024, 6, 16, 15, 20, 0),
            Icon = "\ue718",
            Color = SuccessColor
        });

        // CUS-1019
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1019",
            Title = "Payment Received",
            Description = "Enterprise invoice paid successfully.",
            Time = new DateTime(2024, 6, 17, 11, 40, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1019",
            Title = "Order Delivered",
            Description = "Bulk shipment completed.",
            Time = new DateTime(2024, 6, 18, 18, 10, 0),
            Icon = "\ue718",
            Color = SuccessColor
        });

        // CUS-1020
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1020",
            Title = "Payment Received",
            Description = "Payment of $49,850 received.",
            Time = new DateTime(2024, 6, 19, 10, 55, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1020",
            Title = "Order Delivered",
            Description = "Premium customer order delivered.",
            Time = new DateTime(2024, 6, 20, 17, 25, 0),
            Icon = "\ue718",
            Color = SuccessColor
        });
        // CUS-1021
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1021",
            Title = "Customer Registered",
            Description = "Standard customer account created.",
            Time = new DateTime(2024, 6, 21, 10, 15, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1021",
            Title = "Support Ticket Created",
            Description = "Order inquiry ticket submitted.",
            Time = new DateTime(2024, 6, 22, 14, 30, 0),
            Icon = "\ue77b",
            Color = SupportColor
        });

        // CUS-1022
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1022",
            Title = "Order Placed",
            Description = "Enterprise order initiated.",
            Time = new DateTime(2024, 6, 23, 9, 45, 0),
            Icon = "\ue73e",
            Color = SuccessColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1022",
            Title = "Payment Received",
            Description = "Payment successfully recorded.",
            Time = new DateTime(2024, 6, 24, 16, 20, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1023
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1023",
            Title = "Customer Registered",
            Description = "Enterprise customer onboarded.",
            Time = new DateTime(2024, 6, 25, 11, 10, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1023",
            Title = "Account Inactivated",
            Description = "Customer account marked inactive.",
            Time = new DateTime(2024, 6, 26, 15, 45, 0),
            Icon = "\ue71c",
            Color = DangerColor
        });

        // CUS-1024
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1024",
            Title = "Profile Updated",
            Description = "Customer information updated.",
            Time = new DateTime(2024, 6, 27, 13, 15, 0),
            Icon = "\ue718",
            Color = UpdateColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1024",
            Title = "Account Inactivated",
            Description = "Premium account moved to inactive state.",
            Time = new DateTime(2024, 6, 28, 10, 30, 0),
            Icon = "\ue71c",
            Color = DangerColor
        });

        // CUS-1025
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1025",
            Title = "Order Placed",
            Description = "Premium customer order submitted.",
            Time = new DateTime(2024, 6, 29, 9, 15, 0),
            Icon = "\ue73e",
            Color = SuccessColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1025",
            Title = "Payment Pending",
            Description = "Awaiting payment confirmation.",
            Time = new DateTime(2024, 6, 29, 17, 40, 0),
            Icon = "\ue786",
            Color = PaymentColor
        });

        // CUS-1026
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1026",
            Title = "Customer Registered",
            Description = "Premium customer successfully registered.",
            Time = new DateTime(2024, 6, 30, 8, 50, 0),
            Icon = "\ue760",
            Color = RegistrationColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1026",
            Title = "Account Inactivated",
            Description = "Customer subscription expired.",
            Time = new DateTime(2024, 7, 1, 14, 20, 0),
            Icon = "\ue71c",
            Color = DangerColor
        });

        // CUS-1027
        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1027",
            Title = "Support Ticket Created",
            Description = "Pending account verification request.",
            Time = new DateTime(2024, 7, 2, 11, 35, 0),
            Icon = "\ue77b",
            Color = SupportColor
        });

        AllActivities.Add(new ActivityItem
        {
            CustomerId = "CUS-1027",
            Title = "Profile Updated",
            Description = "Contact details updated successfully.",
            Time = new DateTime(2024, 7, 3, 16, 10, 0),
            Icon = "\ue718",
            Color = UpdateColor
        });
    }
  
    private void LoadCustomerActivities()
    {
        Activities.Clear();

        if (SelectedCustomer == null)
            return;

        var customerActivities =
            AllActivities.Where(x =>
                x.CustomerId == SelectedCustomer.CustomerId);

        foreach (var activity in customerActivities)
        {
            Activities.Add(activity);
        }
    }

}

public class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(
		[CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this,
			new PropertyChangedEventArgs(propertyName));
	}

	protected bool SetProperty<T>(
		ref T backingStore,
		T value,
		[CallerMemberName] string propertyName = "")
	{
		if (EqualityComparer<T>.Default.Equals(backingStore, value))
			return false;

		backingStore = value;
		OnPropertyChanged(propertyName);
		return true;
	}
}