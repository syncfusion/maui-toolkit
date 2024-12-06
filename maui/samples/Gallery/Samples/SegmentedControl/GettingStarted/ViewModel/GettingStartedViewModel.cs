using System.Collections.ObjectModel;
using System.ComponentModel;
using Syncfusion.Maui.Toolkit.SegmentedControl;

namespace Syncfusion.Maui.ControlsGallery.Buttons.SfSegmentedControl
{
    
    /// <summary>
    /// Providing data to support getting started with the application.
    /// </summary>
    public partial class GettingStartedViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// The index of the selected item.
        /// </summary>
        int _selectedColoredIndex = -1;

        /// <summary>
        /// The list of speaker fill colors information.
        /// </summary>
        List<SfSegmentItem>? _speakerColorOptionsInfo;

        /// <summary>
        /// The selected delivery options.
        /// </summary>
        int _selectedDeliveryOptions = -1;

        /// <summary>
        /// The delivery date.
        /// </summary>
        DateTime? _deliveryDate = DateTime.Now;

        /// <summary>
        /// The total price.
        /// </summary>
        string? _totalPrice;

        /// <summary>
        /// The total amount.
        /// </summary>
        int _totalAmount;

        /// <summary>
        /// The date
        /// </summary>
        string? _date;

        /// <summary>
        /// The selected item size index.
        /// </summary>
        int _selectedWarrantyOption;

        /// <summary>
        /// The file path or URL of an image associated with this object.
        /// </summary>
        string _image = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GettingStartedViewModel"/> class.
        /// </summary>
        public GettingStartedViewModel()
        {
            InitializeSegmentItemsColorsInfo();
            InitializeAddOnItemsInfo();
            InitializeSpeakerDeliveryOptionInfo();
            InitializeSpeakerResultsInfo();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the collection of speaker results option information.
        /// </summary>
        public ObservableCollection<SfSegmentItem>? SpeakerResultsOptionInfo { get; private set; }

        /// <summary>
        /// Gets or sets the collection of speaker delivery options information.
        /// </summary>
        public ObservableCollection<SfSegmentItem>? SpeakerDeliveryOptionsInfo { get; private set; }

        /// <summary>
        /// Gets or sets the collection of speaker add on items options information.
        /// </summary>
        public ObservableCollection<SfSegmentItem>? SpeakerWarrantyOptionsInfo { get; private set; }

        /// <summary>
        /// Represents the final price of the speaker.
        /// </summary>
        public int FinalPrice { get; private set; }

        /// <summary>
        /// Gets or sets the list of segment items colors information.
        /// </summary>
        public List<SfSegmentItem>? SpeakerColorOptionsInfo
        {
            get
            {
                return _speakerColorOptionsInfo;
            }
            set
            {
                if (_speakerColorOptionsInfo != value)
                {
                    _speakerColorOptionsInfo = value;
                    OnPropertyChanged(nameof(SpeakerColorOptionsInfo));
                }
            }
        }

        /// <summary>
        /// Gets or sets the date associated with the speaker.
        /// </summary>
        public string? Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        /// <summary>
        /// Gets or sets the total price of the speaker.
        /// </summary>
        public string? TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        /// <summary>
        /// Gets or sets the delivery date of the speaker.
        /// </summary>
        public DateTime? DeliveryDate
        {
            get
            {
                return _deliveryDate;
            }
            set
            {
                _deliveryDate = value;
                OnPropertyChanged(nameof(DeliveryDate));
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected delivery option.
        /// </summary>
        public int SelectedDeliveryOption
        {
            get
            {
                return _selectedDeliveryOptions;
            }
            set
            {
                _selectedDeliveryOptions = value;
                UpdateTotalPriceAndDeliveryDate();
                UpdateTotalPriceBasedOnSize();
                OnPropertyChanged(nameof(SelectedDeliveryOption));
            }
        }

        /// <summary>
        /// Gets or sets the selected add-on item index.
        /// </summary>
        public int SelectedWarrantyOption
        {
            get
            {
                return _selectedWarrantyOption;
            }
            set
            {
                if (_selectedWarrantyOption != value)
                {
                    _selectedWarrantyOption = value;
                    UpdateSelectedSegmentItemsValue();
                    OnPropertyChanged(nameof(SelectedWarrantyOption));
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected colored item index.
        /// </summary>
        public int SelectedColoredIndex
        {
            get
            {
                return _selectedColoredIndex;
            }
            set
            {
                if (_selectedColoredIndex != value)
                {
                    _selectedColoredIndex = value;
                    OnPropertyChanged(nameof(_selectedColoredIndex));
                    UpdateSelectedSegmentItemsValue();
                }
            }
        }

        /// <summary>
        /// Gets or sets the image path or URL.
        /// </summary>
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Private Methods

        /// <summary>
        /// Method used to raise the property changed event.
        /// </summary>
        /// <param name="parameter">Represents the property name.</param>
        private void OnPropertyChanged(string parameter)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameter));
        }

        /// <summary>
        /// Initializes the collection of add on segment items info.
        /// </summary>
        private void InitializeAddOnItemsInfo()
        {
            SpeakerWarrantyOptionsInfo =
			[
				 new SfSegmentItem() { Text = "1 Year" },
                 new SfSegmentItem() { Text = "2 Years" },
                 new SfSegmentItem() { Text = "3 Years" }
            ];
        }

        /// <summary>
        /// Initializes the collection of segments items colors info.
        /// </summary>
        private void InitializeSegmentItemsColorsInfo()
        {
            SpeakerColorOptionsInfo =
			[
				new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#8EAEDC"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#8EAEDC"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#A4AAB4"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#A4AAB4"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#7DC59D"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#7DC59D"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#F5878F"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#F5878F"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#C7B690"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#C7B690"), FontSize = 25, FontFamily = "SegmentIcon" } },
            ];

            SelectedColoredIndex = 0;
        }

        /// <summary>
        /// Initializes the collection of speaker delivery option segment items.
        /// </summary>
        private void InitializeSpeakerDeliveryOptionInfo()
        {
            SpeakerDeliveryOptionsInfo =
			[
				new SfSegmentItem() { Text = "Free Delivery" , Width = 120 },
                new SfSegmentItem() { Text = "+$6 for 1 Day Delivery" , Width = 180},
            ];

            _selectedDeliveryOptions = 0;
        }

        /// <summary>
        /// Initializes the collection of speaker results option segment items.
        /// </summary>
        private void InitializeSpeakerResultsInfo()
        {
            SpeakerResultsOptionInfo =
			[
				new SfSegmentItem() { Text = "Add to Cart"},
            ];
        }

        /// <summary>
        /// Updates the total price and delivery date based on the selected delivery option.
        /// </summary>
        private void UpdateTotalPriceAndDeliveryDate()
        {
            if(DeliveryDate != null)
            {
                if (_selectedDeliveryOptions == 0)
                {
                    _totalAmount = FinalPrice;
#if WINDOWS || MACCATALYST
                    Date = " (Delivery by " + DeliveryDate.Value.AddDays(5).ToLongDateString() + ")";
#else
                    Date = " (Delivery by " + DeliveryDate.Value.AddDays(5).ToString("ddd, MMM dd, yyyy") + ")";
#endif
                    TotalPrice = "$" + _totalAmount;
                }
                else
                {
                    _totalAmount += 6;
#if WINDOWS || MACCATALYST
                    Date = " (Delivery by " + DeliveryDate.Value.AddDays(1).ToLongDateString() + ")";
#else
                    Date = " (Delivery by " + DeliveryDate.Value.AddDays(1).ToString("ddd, MMM dd, yyyy") + ")";
#endif
                    TotalPrice = "$" + _totalAmount;
                }
            }
        }

        /// <summary>
        /// Updates the total price based on the selected warranty year.
        /// </summary>
        private void UpdateTotalPriceBasedOnSize()
        {
            if (SpeakerWarrantyOptionsInfo == null || SpeakerWarrantyOptionsInfo.Count == 0 || SpeakerWarrantyOptionsInfo.Count < _selectedWarrantyOption)
            {
                return;
            }

            SfSegmentItem? selectedYear = SpeakerWarrantyOptionsInfo[_selectedWarrantyOption];
            string sizeText = selectedYear.Text;
            int warrantyPriceIncrement = 0;
			warrantyPriceIncrement = sizeText switch
			{
				"1 Year" => 0,
				"2 Years" => 60,
				_ => 120,
			};
			TotalPrice = "$ " + (_totalAmount + warrantyPriceIncrement);
        }

        /// <summary>
        /// Updates the segment items value based on the selected speaker style.
        /// </summary>
        private void UpdateSelectedSegmentItemsValue()
        {
            if (SpeakerColorOptionsInfo == null || SpeakerColorOptionsInfo.Count == 0 || SpeakerColorOptionsInfo.Count < _selectedColoredIndex)
            {
                return;
            }

            if (SelectedColoredIndex == 0)
            {
                Image = "bluespeaker.png";
            }
            else if (SelectedColoredIndex == 1)
            {
                Image = "greyspeaker.png";
            }
            else if (SelectedColoredIndex == 2)
            {
                Image = "greenspeaker.png";
            }
            else if (SelectedColoredIndex == 3)
            {
                Image = "pinkspeaker.png";
            }
            else if (SelectedColoredIndex == 4)
            {
                Image = "sandalsspeaker.png";
            }

            _totalAmount = 399;
            TotalPrice = "$ " + _totalAmount;
            FinalPrice = _totalAmount;

            // Update the total price and delivery date information.
            UpdateTotalPriceAndDeliveryDate();

            // Update the total price based on size.
            UpdateTotalPriceBasedOnSize();
        }

        #endregion
    }
}