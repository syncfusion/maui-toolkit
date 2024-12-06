using Syncfusion.Maui.Toolkit.Chips;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Syncfusion.Maui.ControlsGallery.Chips
{
	public partial class ChipViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string? name = null) =>
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		#region Fields

		/// <summary>
		/// The icon property to hold the chip image.
		/// </summary>
		string _icon = "user.png";

		/// <summary>
		/// The television collection.
		/// </summary>
		internal ObservableCollection<string> _televisionItems = ["Samsung", "LG"];

		/// <summary>
		/// The washer collection.
		/// </summary>
		internal ObservableCollection<string> _washerItems = ["Whirlpool", "Kenmore"];

		/// <summary>
		/// The air conditioner collection.
		/// </summary>
		internal ObservableCollection<string> _airConditionerItems = ["Mitsubishi", "Hitachi"];

		/// <summary>
		/// The choice collection.
		/// </summary>
		List<string> _choiceItems = ["Washer", "Television", "Air Conditioner"];

		/// <summary>
		/// The result.
		/// </summary>
		ObservableCollection<string> _result = [];

		/// <summary>
		/// The border color of chip.
		/// </summary>
		Color _borderColor = Color.FromArgb("#A007A3");

		/// <summary>
		/// The background color of chip.
		/// </summary>
#if ANDROID || IOS
		Color _backgroundColor = Color.FromArgb("#af2463");
#elif WINDOWS || MACCATALYST
    Color _backgroundColor = Color.FromArgb("#A007A3");
#endif
		/// <summary>
		/// Represents the text color
		/// </summary>
#if ANDROID || IOS
		Color _textColor = Color.FromArgb("#f2f3f4");
#elif WINDOWS || MACCATALYST
    Color _textColor = Colors.White;
#endif

		/// <summary>
		/// Represents the visibility of image
		/// </summary>
		bool _showImage = true;

		/// <summary>
		/// The border thickness of chip.
		/// </summary>
		int _borderThickness = 1;

		/// <summary>
		/// The corner radius slider.
		/// </summary>
#if ANDROID || IOS
		int _rightSlider = 20;
#elif WINDOWS || MACCATALYST
    int _rightSlider = 0;
#endif

		/// <summary>
		/// The corner radius of chip.
		/// </summary>
#if ANDROID || IOS
		CornerRadius _cornerRadius = 20;
#elif WINDOWS || MACCATALYST
    CornerRadius _cornerRadius = 0;
#endif

		/// <summary>
		/// The default corner radius.
		/// </summary>
#if ANDROID || IOS
		int _leftSlider = 20;
#elif WINDOWS || MACCATALYST
    int _leftSlider = 0;  
#endif

		/// <summary>
		/// Represents the border width
		/// </summary>
		double _borderWidth = 0;

		/// <summary>
		/// The text of chip.
		/// </summary>
		string _text = "JAMES";

		/// <summary>
		/// The is show visual.
		/// </summary>
#if WINDOWS || MACCATALYST
    bool _isShowVisual = false;
#elif ANDROID || IOS
		bool _isShowVisual = false;
#endif

		/// <summary>
		/// The is show.
		/// </summary>
		bool _isShow = true;

		/// <summary>
		/// The input text.
		/// </summary>
		string _inputText = "";

		/// <summary>
		/// The visual mode.
		/// </summary>
		string _visualMode = "None";

		/// <summary>
		/// The is shown close button.
		/// </summary>
#if WINDOWS || MACCATALYST
    bool _isShownCloseButton = true;
#elif ANDROID || IOS
		bool _isShownCloseButton = true;
#endif

		/// <summary>
		/// The selected item.
		/// </summary>
		object _selectedItem = "Television";

		/// <summary>
		/// The selected filter items collection.
		/// </summary>
		ObservableCollection<object> _selectedFilterItems = [];

		/// <summary>s
		/// The input collection.
		/// </summary>
		ObservableCollection<string>? _brands;

		/// <summary>
		/// The filter collection.
		/// </summary>
		List<string> _filterChips = [];

		/// <summary>
		/// The action collection.
		/// </summary>
		List<string> _actionChips = ["Search by brands", "Search by features"];

		/// <summary>
		/// The action command.
		/// </summary>
		ICommand? _actionCommand;

		/// <summary>
		/// The place holder text.
		/// </summary>
		string _placeHolderText = "Enter brand name";

		/// <summary>
		/// The color items filter collection.
		/// </summary>
		public List<string>? colorItems = ["Blue", "Grey", "Green", "Pink", "Sandal"];

		/// <summary>
		/// The add ons filter collection.
		/// </summary>
		List<string>? _addOnItems = ["Fast Charge", "512 MB SD Card", "2 Years Extended Warranty"];

		/// <summary>
		/// The devivery option choice collection.
		/// </summary>
		List<string>? _deliveryOptions = ["Free Delivery", "+$5 for 1 Day Delivery"];

		/// <summary>
		/// The control list choice collection.
		/// </summary>
		List<string>? _controlsList = ["syncfusion", "dot-net", "android"];
		/// <summary>
		/// The selected color filter item.
		/// </summary>
		object _selectedColorItem = "Blue";

		/// <summary>
		/// The image property to hold the speaker image..
		/// </summary>
		string _image = "blueSpeaker.png";

		/// <summary>
		/// The selected add on filter item.
		/// </summary>
		ObservableCollection<object> _selectedaddOnItems = ["Fast Charge"];

		/// <summary>
		/// The selected delivery option choice item.
		/// </summary>
		object _selectedDeliveryOptions = "Free Delivery";

		/// <summary>
		/// The total amount.
		/// </summary>
		int _totalAmount = 0;

		/// <summary>
		/// The fast charge price.
		/// </summary>
		int _fastChargePrice = 0;

		/// <summary>
		/// The sd card price
		/// </summary>
		int _sdCardPrice = 0;

		/// <summary>
		/// The warrenty price.
		/// </summary>
		int _warrentyPrice = 0;

		/// <summary>
		/// The delivery date.
		/// </summary>
		DateTime _deliveryDate = DateTime.Now;

		/// <summary>
		/// The date.
		/// </summary>
		string _date = " ";

		/// <summary>
		/// The total price.
		/// </summary>
		string _totalPrice = " ";

		/// <summary>
		/// The final amount.
		/// </summary>
		int _finalAmount = 0;

		/// <summary>
		/// The final price.
		/// </summary>
		int _finalPrice = 299;


    /// <summary>
    /// The chip type.
    /// </summary>   
    SfChipsType _chipType = SfChipsType.Input;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the image visibility
		/// </summary>
		public bool ShowImage
		{
			get
			{
				return _showImage;
			}
			set
			{
				_showImage = value;
				OnPropertyChanged("ShowImage");
			}
		}

		/// <summary>
		/// Gets or Sets the text color
		/// </summary>
#if ANDROID || IOS
		public Color TextColor
		{
			get
			{
				return _textColor;
			}

			set
			{
				_textColor = value;
				OnPropertyChanged("TextColor");
			}
		}
#elif WINDOWS || MACCATALYST
    public Color TextColor
    {
        get
        {
            return _textColor;
        }

        set
        {
            _textColor = value;
            OnPropertyChanged("TextColor");
        }
    }
#endif

		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// <value>The chip icon.</value>
		public string Icon
		{
			get
			{
				return _icon;
			}

			set
			{
				_icon = value;
			}
		}

		/// <summary>
		/// Gets or sets the color of the border of chip.
		/// </summary>
		/// <value>The color of the border of chip.</value>
		public Color BorderStroke
		{
			get
			{
				return _borderColor;
			}

			set
			{
				_borderColor = value;
				OnPropertyChanged("BorderStroke");
			}
		}

		/// <summary>
		/// Gets or sets the background color of chip.
		/// </summary>
		/// <value>The background color of chip</value>
#if ANDROID || IOS
		public Color BackgroundColor
		{
			get
			{
				return _backgroundColor;
			}

			set
			{
				_backgroundColor = value;
				OnPropertyChanged("BackgroundColor");
			}
		}
#elif WINDOWS || MACCATALYST
    public Color BackgroundColor
    {
        get
        {
            return _backgroundColor;
        }

        set
        {
            _backgroundColor = value;
            OnPropertyChanged("BackgroundColor");
        }
    }
#endif
		/// <summary>
		/// Gets or sets the thickness of border.
		/// </summary>
		/// <value>The border thickness.</value>
		public int BorderThickness
		{
			get
			{
				return _borderThickness;
			}
			set
			{
				_borderThickness = value;
				OnPropertyChanged("BorderThickness");
			}
		}

		/// <summary>
		/// Gets or sets the slider value.
		/// </summary>
		/// <value>The slider value.</value>
#if ANDROID || IOS
		public int RightCornerRadius
		{
			get
			{
				return _rightSlider;
			}
			set
			{
				_rightSlider = value;
				CornerRadius = new CornerRadius(_cornerRadius.TopLeft, value, value, _cornerRadius.BottomRight);
				OnPropertyChanged("RightCornerRadius");
			}
		}
#elif WINDOWS || MACCATALYST
    public int RightCornerRadius
    {
        get
        {
            return _rightSlider;
        }
        set
        {
            _rightSlider = value;
            CornerRadius = new CornerRadius(_cornerRadius.TopLeft, value, value, _cornerRadius.BottomRight);
            OnPropertyChanged("RightCornerRadius");
        }
    }
#endif

		/// <summary>
		/// Gets or sets the slider value.
		/// </summary>
		/// <value>The slider value.</value>
#if ANDROID || IOS
		public int LeftCornerRadius
		{
			get
			{
				return _leftSlider;
			}
			set
			{
				_leftSlider = value;
				CornerRadius = new CornerRadius(value, _cornerRadius.TopRight, _cornerRadius.BottomLeft, value);
				OnPropertyChanged("LeftCornerRadius");
			}
		}
#elif MACCATALYST || WINDOWS
    public int LeftCornerRadius
    {
        get
        {
            return _leftSlider;
        }
        set
        {
            _leftSlider = value;
            CornerRadius = new CornerRadius(value, _cornerRadius.TopRight, _cornerRadius.BottomLeft, value);
            OnPropertyChanged("LeftCornerRadius");
        }
    }
#endif

		/// <summary>
		/// Gets or sets the border width.
		/// </summary>
		public double BorderWidth
		{
			get
			{
				return _borderWidth;
			}
			set
			{
				_borderWidth = value;
				OnPropertyChanged("BorderWidth");
			}
		}


		/// <summary>
		/// Gets or sets the corner radius.
		/// </summary>
		/// <value>The corner radius.</value>
#if ANDROID || IOS
		public CornerRadius CornerRadius
		{
			get
			{
				return _cornerRadius;
			}
			set
			{

				_cornerRadius = value;
				OnPropertyChanged("CornerRadius");
			}
		}
#elif WINDOWS || MACCATALYST
    public CornerRadius CornerRadius
    {
        get
        {
            return _cornerRadius;
        }
        set
        {

            _cornerRadius = value;
            OnPropertyChanged("CornerRadius");
        }
    }
#endif

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				OnPropertyChanged("Text");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Chip.ChipViewModel"/> is show.
		/// </summary>
		/// <value><c>true</c> if is show; otherwise, <c>false</c>.</value>
		public bool IsShownIcon
		{
			get
			{
				return _isShow;
			}
			set
			{
				_isShow = value;
				OnPropertyChanged("IsShownIcon");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Chip.ChipViewModel"/> is show visual.
		/// </summary>
		/// <value><c>true</c> if is show visual; otherwise, <c>false</c>.</value>
#if WINDOWS || MACCATALYST
    public bool IsShownSelection
    {
        get
        {
            return _isShowVisual;
        }
        set
        {
            _isShowVisual = value;
            if (value) 
            {
                ChipType = SfChipsType.Filter;
                IsShownCloseButton = false;
            }
            else if(IsShownCloseButton)
            {
                ChipType = SfChipsType.Input;
            }
            else
            {
                ChipType = SfChipsType.Action;
            }
            OnPropertyChanged("IsShownSelection");
        }
    }
#elif ANDROID || IOS
		public bool IsShownSelection
		{
			get
			{
				return _isShowVisual;
			}
			set
			{
				_isShowVisual = value;
				OnPropertyChanged("IsShownSelection");
			}
		}
#endif

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Chip.ChipViewModel"/> is show visual.
		/// </summary>
		/// <value><c>true</c> if is show visual; otherwise, <c>false</c>.</value>
#if WINDOWS || MACCATALYST
    public bool IsShownCloseButton
    {
        get
        {
            return _isShownCloseButton;
        }
        set
        {
            _isShownCloseButton = value;
            if (value)
            {
                ChipType = SfChipsType.Input;
                IsShownSelection = false;
            }
            else if (IsShownSelection)
            {
                ChipType = SfChipsType.Filter;
            }
            else
            {
                ChipType = SfChipsType.Action;
            }
            OnPropertyChanged("IsShownCloseButton");
        }
    }
#elif ANDROID || IOS
		public bool IsShownCloseButton
		{
			get
			{
				return _isShownCloseButton;
			}
			set
			{
				_isShownCloseButton = value;
				OnPropertyChanged("IsShownCloseButton");
			}
		}
#endif


		/// <summary>
		/// Gets or sets the visual mode.
		/// </summary>
		/// <value>The visual mode.</value>
		public string VisualMode
		{
			get
			{
				return _visualMode;
			}
			set
			{
				_visualMode = value;
				OnPropertyChanged("VisualMode");
			}
		}

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public object SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				_selectedItem = value;
				if (_selectedItem != null)
				{
					if (!string.IsNullOrEmpty("SelectedItem"))
					{
						if (_selectedItem.Equals("Television"))
						{
							InputItems = _televisionItems;
							FilterItems = ["LED", "LCD", "Wi-Fi", "4K", "Ultra HD"];
						}
						else if (_selectedItem.Equals("Washer"))
						{
							InputItems = _washerItems;
							FilterItems = ["Front Load", "Top Load"];
						}
						else if (_selectedItem.Equals("Air Conditioner"))
						{
							InputItems = _airConditionerItems;
							FilterItems = ["Window", "Portable", "Hybrid"];
						}
					}
					_selectedFilterItems.Clear();
				}
				Result.Clear();
				Result.Add("No results found");
				OnPropertyChanged("SelectedItem");
			}
		}

		/// <summary>
		/// Gets or sets the input text.
		/// </summary>
		/// <value>The input text.</value>
		public string InputText
		{
			get
			{
				return _inputText;
			}
			set
			{
				_inputText = value;
				OnPropertyChanged("InputText");
			}
		}

		/// <summary>
		/// Gets or sets the action command.
		/// </summary>
		/// <value>The action command.</value>
		public ICommand? ActionCommand
		{
			get
			{
				return _actionCommand;
			}
			set
			{
				_actionCommand = value;
			}
		}

		/// <summary>
		/// Gets or sets the place holder text.
		/// </summary>
		/// <value>The place holder text.</value>
		public string PlaceHolderText
		{
			get
			{
				return _placeHolderText;
			}
			set
			{
				_placeHolderText = value;
				OnPropertyChanged("PlaceHolderText");
			}
		}

		/// <summary>
		/// Gets or sets the input collection.
		/// </summary>
		/// <value>The input collection.</value>
		public ObservableCollection<string>? InputItems
		{
			get
			{
				return _brands;
			}
			set
			{
				_brands = value;
				if (_brands != null && _brands.Count == 0)
				{
					_filterChips.Clear();
					Result.Add("No results found");

				}

				OnPropertyChanged("InputItems");
			}
		}

		/// <summary>
		/// Gets or sets the filter collection.
		/// </summary>
		/// <value>The filter collection.</value>
		public List<string> FilterItems
		{
			get
			{
				return _filterChips;
			}
			set
			{
				_filterChips = value;
				OnPropertyChanged("FilterItems");
			}
		}

		/// <summary>
		/// Gets or sets the action collection.
		/// </summary>
		/// <value>The action collection.</value>
		public List<string> ActionItems
		{
			get
			{
				return _actionChips;
			}
			set
			{
				_actionChips = value;
				OnPropertyChanged("ActionItems");
			}
		}

		/// <summary>
		/// Gets or sets the choice collection.
		/// </summary>
		/// <value>The choice collection.</value>
		public List<string> ChoiceItems
		{
			get
			{
				return _choiceItems;
			}
			set
			{
				_choiceItems = value;
				OnPropertyChanged("ChoiceItems");
			}
		}

		/// <summary>
		/// Gets or sets the result.
		/// </summary>
		/// <value>The result.</value>
		public ObservableCollection<string> Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;
				OnPropertyChanged("Result");
			}
		}

		/// <summary>
		/// Gets or sets the selected filter items.
		/// </summary>
		/// <value>The SelectedFilterItems.</value>
		public ObservableCollection<object> SelectedFilterItems
		{
			get
			{
				return _selectedFilterItems;
			}
			set
			{
				_selectedFilterItems = value;
				OnPropertyChanged(nameof(SelectedFilterItems));
			}
		}

		/// <summary>
		/// Gets or sets the color items.
		/// </summary>
		/// <value>The ColorItems.</value>
		public List<string>? ColorItems
		{
			get
			{
				return colorItems;
			}
			set
			{
				colorItems = value;
				OnPropertyChanged(nameof(ColorItems));
			}
		}

		/// <summary>
		/// Gets or sets the add on items.
		/// </summary>
		/// <value>The AddOnItems.</value>
		public List<string>? AddOnItems
		{
			get
			{
				return _addOnItems;
			}
			set
			{
				_addOnItems = value;
				OnPropertyChanged(nameof(AddOnItems));
			}
		}

		/// <summary>
		/// Gets or sets the delivery options.
		/// </summary>
		/// <value>The DeliveryOptions.</value>
		public List<string>? DeliveryOptions
		{
			get
			{
				return _deliveryOptions;
			}
			set
			{
				_deliveryOptions = value;
				OnPropertyChanged(nameof(DeliveryOptions));
			}
		}

		/// <summary>
		/// Gets or sets the contols list.
		/// </summary>
		/// <value>The ControlsList.</value>
		public List<string>? ControlsList
		{
			get
			{
				return _controlsList;
			}
			set
			{
				_controlsList = value;
				OnPropertyChanged(nameof(ControlsList));
			}
		}


		/// <summary>
		/// Gets or sets the selected color items.
		/// </summary>
		/// <value>The SelectedColorItem.</value>
		public object SelectedColorItem
		{
			get
			{
				return _selectedColorItem;
			}
			set
			{
				_selectedColorItem = value;
				if (_selectedColorItem != null)
				{
					if (!string.IsNullOrEmpty("SelectedColorItem"))
					{
						if (_selectedColorItem.Equals("Blue"))
						{
							Image = "bluespeaker.png";
						}
						else if (_selectedColorItem.Equals("Grey"))
						{
							Image = "greyspeaker.png";
						}
						else if (_selectedColorItem.Equals("Green"))
						{
							Image = "greenspeaker.png";
						}
						else if (_selectedColorItem.Equals("Pink"))
						{
							Image = "pinkspeaker.png";
						}
						else if (_selectedColorItem.Equals("Sandal"))
						{
							Image = "sandalsspeaker.png";
						}
					}
				}
				OnPropertyChanged(nameof(SelectedColorItem));
			}
		}

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The Image.</value>
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

		/// <summary>
		/// Gets or sets the fast charge price.
		/// </summary>
		/// <value>The FastChargePrice.</value>
		public int FastChargePrice
		{
			get { return _fastChargePrice; }
			set { _fastChargePrice = value; OnPropertyChanged(nameof(FastChargePrice)); }
		}

		/// <summary>
		/// Gets or sets the sd card price.
		/// </summary>
		/// <value>The SDCardPrice.</value>
		public int SDCardPrice
		{
			get { return _sdCardPrice; }
			set { _sdCardPrice = value; OnPropertyChanged(nameof(SDCardPrice)); }
		}

		/// <summary>
		/// Gets or sets the warrenty price.
		/// </summary>
		/// <value>The WarrentyPrice.</value>
		public int WarrentyPrice
		{
			get { return _warrentyPrice; }
			set { _warrentyPrice = value; OnPropertyChanged(nameof(WarrentyPrice)); }
		}

		/// <summary>
		/// Gets or sets the selected add on items.
		/// </summary>
		/// <value>The SelectedAddOnItems.</value>
		public ObservableCollection<object> SelectedAddOnItems
		{
			get
			{
				return _selectedaddOnItems;
			}
			set
			{
				_selectedaddOnItems = value;
				OnPropertyChanged(nameof(SelectedAddOnItems));
			}
		}

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>The Date.</value>
		public string Date
		{
			get { return _date; }
			set { _date = value; OnPropertyChanged(nameof(Date)); }
		}

		/// <summary>
		/// Gets or sets the total price.
		/// </summary>
		/// <value>The TotalPrice.</value>
		public string TotalPrice
		{
			get { return _totalPrice; }
			set { _totalPrice = value; OnPropertyChanged(nameof(TotalPrice)); }
		}

		/// <summary>
		/// Gets or sets the selected delivery option.
		/// </summary>
		/// <value>The DeliveryOption.</value>
		public object SelectedDeliveryOption
		{
			get
			{
				return _selectedDeliveryOptions;
			}
			set
			{
				_selectedDeliveryOptions = value;
				if (SelectedDeliveryOption != null)
				{
					if (_selectedDeliveryOptions.Equals("Free Delivery"))
					{
						TotalAmount = FinalAmount;
						Date = "(Get by " + DeliveryDate.AddDays(5).ToLongDateString() + " )";
						TotalPrice = "$ " + TotalAmount;
					}
					else
					{
						TotalAmount += 5;
						Date = "(Get by " + DeliveryDate.AddDays(1).ToLongDateString() + " )";
						TotalPrice = "$ " + TotalAmount;
					}
				}
				OnPropertyChanged(nameof(SelectedDeliveryOption));
			}
		}

		/// <summary>
		/// Gets or sets the total amount.
		/// </summary>
		/// <value>The TotalAmount.</value>
		public int TotalAmount
		{
			get
			{
				return _totalAmount;
			}
			set
			{
				_totalAmount = value;
				OnPropertyChanged(nameof(TotalAmount));
			}
		}

		/// <summary>
		/// Gets or sets the final amount.
		/// </summary>
		/// <value>The FinalAmount.</value>
		public int FinalAmount
		{
			get
			{
				return _finalAmount;
			}
			set
			{
				_finalAmount = value;
				OnPropertyChanged(nameof(FinalAmount));
			}
		}

		/// <summary>
		/// Gets or sets the final price.
		/// </summary>
		/// <value>The FinalPrice.</value>
		public int FinalPrice
		{
			get
			{
				return _finalPrice;
			}
			set
			{
				_finalPrice = value;
				OnPropertyChanged(nameof(FinalPrice));
			}
		}

		/// <summary>
		/// Gets or sets the delivery date.
		/// </summary>
		/// <value>The DeliveryDate.</value>
		public DateTime DeliveryDate
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

    public SfChipsType ChipType
    {
        get
        {
            return _chipType;
        }
        set
        {
            _chipType = value;
            OnPropertyChanged(nameof(ChipType));
        }
    }
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Chip.ChipViewModel"/> class.
		/// </summary>
		[Obsolete]
		public ChipViewModel()
		{
			ActionCommand = new Command(HandleAction);
		}

		/// <summary>
		/// Handles the action.
		/// </summary>
		/// <param name="obj">Object.</param>
		void HandleAction(object obj)
		{
			Result.Clear();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
			if (obj != null && obj.ToString().Equals("Search by brands", StringComparison.Ordinal) && _selectedItem != null && _brands.Count > 0)
			{
				foreach (var brand in _brands)
				{
					Result.Add(new Random().Next(1, 5).ToString() + " " + brand.ToString() + " " + _selectedItem.ToString() + " found");
				}
			}

			else if (FilterItems.Count > 0 && _selectedItem != null && _selectedFilterItems.Count > 0 && _brands.Count > 0)
			{


				foreach (var feature in _selectedFilterItems)
				{
					foreach (var brand in _brands)
					{
						Result.Add(new Random().Next(1, 5).ToString() + " " + feature.ToString() + " " + _selectedItem.ToString() + " found in " + brand.ToString());
					}
				}
			}
			else
			{
				Result.Add("No results found.");
			}
		}
	}
}