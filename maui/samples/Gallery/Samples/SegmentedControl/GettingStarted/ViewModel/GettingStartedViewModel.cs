using System.Collections.ObjectModel;
using System.ComponentModel;
using Syncfusion.Maui.Toolkit.SegmentedControl;

namespace Syncfusion.Maui.ControlsGallery.Buttons.SfSegmentedControl
{
    
    /// <summary>
    /// Providing data to support getting started with the application.
    /// </summary>
    public class GettingStartedViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// The index of the selected item.
        /// </summary>
        int selectedColoredIndex = -1;

        /// <summary>
        /// The list of speaker fill colors information.
        /// </summary>
        List<SfSegmentItem>? speakerColorOptionsInfo;

        /// <summary>
        /// The selected delivery options.
        /// </summary>
        int selectedDeliveryOptions = -1;

        /// <summary>
        /// The delivery date.
        /// </summary>
        DateTime? deliveryDate = DateTime.Now;

        /// <summary>
        /// The total price.
        /// </summary>
        string? totalPrice;

        /// <summary>
        /// The total amount.
        /// </summary>
        int totalAmount;

        /// <summary>
        /// The date
        /// </summary>
        string? date;

        /// <summary>
        /// The selected item size index.
        /// </summary>
        int selectedWarrantyOption;

        /// <summary>
        /// The file path or URL of an image associated with this object.
        /// </summary>
        string image = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GettingStartedViewModel"/> class.
        /// </summary>
        public GettingStartedViewModel()
        {
            this.InitializeSegmentItemsColorsInfo();
            this.InitializeAddOnItemsInfo();
            this.InitializeSpeakerDeliveryOptionInfo();
            this.InitializeSpeakerResultsInfo();
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
                return this.speakerColorOptionsInfo;
            }
            set
            {
                if (this.speakerColorOptionsInfo != value)
                {
                    this.speakerColorOptionsInfo = value;
                    this.OnPropertyChanged(nameof(SpeakerColorOptionsInfo));
                }
            }
        }

        /// <summary>
        /// Gets or sets the date associated with the speaker.
        /// </summary>
        public string? Date
        {
            get { return this.date; }
            set
            {
                this.date = value;
                this.OnPropertyChanged(nameof(Date));
            }
        }

        /// <summary>
        /// Gets or sets the total price of the speaker.
        /// </summary>
        public string? TotalPrice
        {
            get { return this.totalPrice; }
            set
            {
                this.totalPrice = value;
                this.OnPropertyChanged(nameof(TotalPrice));
            }
        }

        /// <summary>
        /// Gets or sets the delivery date of the speaker.
        /// </summary>
        public DateTime? DeliveryDate
        {
            get
            {
                return this.deliveryDate;
            }
            set
            {
                this.deliveryDate = value;
                this.OnPropertyChanged(nameof(DeliveryDate));
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected delivery option.
        /// </summary>
        public int SelectedDeliveryOption
        {
            get
            {
                return this.selectedDeliveryOptions;
            }
            set
            {
                this.selectedDeliveryOptions = value;
                this.UpdateTotalPriceAndDeliveryDate();
                this.UpdateTotalPriceBasedOnSize();
                this.OnPropertyChanged(nameof(SelectedDeliveryOption));
            }
        }

        /// <summary>
        /// Gets or sets the selected add-on item index.
        /// </summary>
        public int SelectedWarrantyOption
        {
            get
            {
                return this.selectedWarrantyOption;
            }
            set
            {
                if (this.selectedWarrantyOption != value)
                {
                    this.selectedWarrantyOption = value;
                    this.UpdateSelectedSegmentItemsValue();
                    this.OnPropertyChanged(nameof(SelectedWarrantyOption));
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
                return this.selectedColoredIndex;
            }
            set
            {
                if (this.selectedColoredIndex != value)
                {
                    this.selectedColoredIndex = value;
                    this.OnPropertyChanged(nameof(selectedColoredIndex));
                    this.UpdateSelectedSegmentItemsValue();
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
                return image;
            }
            set
            {
                image = value;
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
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameter));
        }

        /// <summary>
        /// Initializes the collection of add on segment items info.
        /// </summary>
        private void InitializeAddOnItemsInfo()
        {
            this.SpeakerWarrantyOptionsInfo = new ObservableCollection<SfSegmentItem>()
            {
                 new SfSegmentItem() { Text = "1 Year" },
                 new SfSegmentItem() { Text = "2 Years" },
                 new SfSegmentItem() { Text = "3 Years" }
            };
        }

        /// <summary>
        /// Initializes the collection of segments items colors info.
        /// </summary>
        private void InitializeSegmentItemsColorsInfo()
        {
            this.SpeakerColorOptionsInfo = new List<SfSegmentItem>()
            {
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#8EAEDC"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#8EAEDC"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#A4AAB4"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#A4AAB4"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#7DC59D"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#7DC59D"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#F5878F"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#F5878F"), FontSize = 25, FontFamily = "SegmentIcon" } },
                new SfSegmentItem() { Text = "\uE91F", SelectedSegmentBackground = Color.FromArgb("#C7B690"), TextStyle = new SegmentTextStyle() { TextColor = Color.FromArgb("#C7B690"), FontSize = 25, FontFamily = "SegmentIcon" } },
            };

            this.SelectedColoredIndex = 0;
        }

        /// <summary>
        /// Initializes the collection of speaker delivery option segment items.
        /// </summary>
        private void InitializeSpeakerDeliveryOptionInfo()
        {
            this.SpeakerDeliveryOptionsInfo = new ObservableCollection<SfSegmentItem>()
            {
                new SfSegmentItem() { Text = "Free Delivery" , Width = 120 },
                new SfSegmentItem() { Text = "+$6 for 1 Day Delivery" , Width = 180},
            };

            this.selectedDeliveryOptions = 0;
        }

        /// <summary>
        /// Initializes the collection of speaker results option segment items.
        /// </summary>
        private void InitializeSpeakerResultsInfo()
        {
            this.SpeakerResultsOptionInfo = new ObservableCollection<SfSegmentItem>()
            {
                new SfSegmentItem() { Text = "Add to Cart"},
            };
        }

        /// <summary>
        /// Updates the total price and delivery date based on the selected delivery option.
        /// </summary>
        private void UpdateTotalPriceAndDeliveryDate()
        {
            if(this.DeliveryDate != null)
            {
                if (this.selectedDeliveryOptions == 0)
                {
                    this.totalAmount = this.FinalPrice;
#if WINDOWS || MACCATALYST
                    this.Date = " (Delivery by " + this.DeliveryDate.Value.AddDays(5).ToLongDateString() + ")";
#else
                    this.Date = " (Delivery by " + this.DeliveryDate.Value.AddDays(5).ToString("ddd, MMM dd, yyyy") + ")";
#endif
                    this.TotalPrice = "$" + this.totalAmount;
                }
                else
                {
                    this.totalAmount += 6;
#if WINDOWS || MACCATALYST
                    this.Date = " (Delivery by " + this.DeliveryDate.Value.AddDays(1).ToLongDateString() + ")";
#else
                    this.Date = " (Delivery by " + this.DeliveryDate.Value.AddDays(1).ToString("ddd, MMM dd, yyyy") + ")";
#endif
                    this.TotalPrice = "$" + this.totalAmount;
                }
            }
        }

        /// <summary>
        /// Updates the total price based on the selected warranty year.
        /// </summary>
        private void UpdateTotalPriceBasedOnSize()
        {
            if (this.SpeakerWarrantyOptionsInfo == null || this.SpeakerWarrantyOptionsInfo.Count == 0 || this.SpeakerWarrantyOptionsInfo.Count < this.selectedWarrantyOption)
            {
                return;
            }

            SfSegmentItem? selectedYear = this.SpeakerWarrantyOptionsInfo[this.selectedWarrantyOption];
            string sizeText = selectedYear.Text;
            int warrantyPriceIncrement = 0;
            switch (sizeText)
            {
                case "1 Year":
                    warrantyPriceIncrement = 0;
                    break;
                case "2 Years":
                    warrantyPriceIncrement = 60;
                    break;
                default:
                    warrantyPriceIncrement = 120;
                    break;
            }

            this.TotalPrice = "$ " + (this.totalAmount + warrantyPriceIncrement);
        }

        /// <summary>
        /// Updates the segment items value based on the selected speaker style.
        /// </summary>
        private void UpdateSelectedSegmentItemsValue()
        {
            if (this.SpeakerColorOptionsInfo == null || this.SpeakerColorOptionsInfo.Count == 0 || this.SpeakerColorOptionsInfo.Count < this.selectedColoredIndex)
            {
                return;
            }

            if (this.SelectedColoredIndex == 0)
            {
                this.Image = "bluespeaker.png";
            }
            else if (this.SelectedColoredIndex == 1)
            {
                this.Image = "greyspeaker.png";
            }
            else if (this.SelectedColoredIndex == 2)
            {
                this.Image = "greenspeaker.png";
            }
            else if (this.SelectedColoredIndex == 3)
            {
                this.Image = "pinkspeaker.png";
            }
            else if (this.SelectedColoredIndex == 4)
            {
                this.Image = "sandalsspeaker.png";
            }

            this.totalAmount = 399;
            this.TotalPrice = "$ " + this.totalAmount;
            this.FinalPrice = this.totalAmount;

            // Update the total price and delivery date information.
            this.UpdateTotalPriceAndDeliveryDate();

            // Update the total price based on size.
            this.UpdateTotalPriceBasedOnSize();
        }

        #endregion
    }
}