using Syncfusion.Maui.Toolkit.Chips;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Syncfusion.Maui.ControlsGallery.Chips;
public class ChipViewModel:INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? name = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
    #region Fields

    /// <summary>
    /// The icon property to hold the chip image.
    /// </summary>
	string icon = "user.png";

    /// <summary>
    /// The television collection.
    /// </summary>
    internal ObservableCollection<string> televisionItems = new ObservableCollection<string>() { "Samsung", "LG" };

    /// <summary>
    /// The washer collection.
    /// </summary>
    internal ObservableCollection<string> washerItems = new ObservableCollection<string>() { "Whirlpool", "Kenmore" };

    /// <summary>
    /// The air conditioner collection.
    /// </summary>
    internal ObservableCollection<string> airConditionerItems = new ObservableCollection<string>() { "Mitsubishi", "Hitachi" };

    /// <summary>
    /// The choice collection.
    /// </summary>
    List<string> choiceItems = new List<string>() { "Washer", "Television", "Air Conditioner" };

    /// <summary>
    /// The result.
    /// </summary>
    ObservableCollection<string> result = new ObservableCollection<string>();

    /// <summary>
    /// The border color of chip.
    /// </summary>
    Color borderColor = Color.FromArgb("#A007A3");

    /// <summary>
    /// The background color of chip.
    /// </summary>
#if ANDROID || IOS
    Color backgroundColor = Color.FromArgb("#af2463");
#elif WINDOWS || MACCATALYST
    Color backgroundColor = Color.FromArgb("#A007A3");
#endif
    /// <summary>
    /// Represents the text color
    /// </summary>
#if ANDROID || IOS
    Color textColor = Color.FromArgb("#f2f3f4");
#elif WINDOWS || MACCATALYST
    Color textColor = Colors.White;
#endif

    /// <summary>
    /// Represents the visibility of image
    /// </summary>
    bool showImage = true;

    /// <summary>
    /// The border thickness of chip.
    /// </summary>
    int borderThickness = 1;

    /// <summary>
    /// The corner radius slider.
    /// </summary>
#if ANDROID || IOS
    int rightSlider = 20;
#elif WINDOWS || MACCATALYST
    int rightSlider = 0;
#endif

    /// <summary>
    /// The corner radius of chip.
    /// </summary>
#if ANDROID || IOS
    CornerRadius cornerRadius = 20;
#elif WINDOWS || MACCATALYST
    CornerRadius cornerRadius = 0;
#endif

    /// <summary>
    /// The default corner radius.
    /// </summary>
#if ANDROID || IOS
    int leftSlider = 20;
#elif WINDOWS || MACCATALYST
    int leftSlider = 0;  
#endif

    /// <summary>
    /// Represents the border width
    /// </summary>
    double borderWidth = 0;

    /// <summary>
    /// The text of chip.
    /// </summary>
    string text = "JAMES";

    /// <summary>
    /// The is show visual.
    /// </summary>
#if WINDOWS || MACCATALYST
    bool isShowVisual = false;
#elif ANDROID || IOS
    bool isShowVisual = false;
#endif

    /// <summary>
    /// The is show.
    /// </summary>
    bool isShow = true;

    /// <summary>
    /// The input text.
    /// </summary>
    string inputText = "";

    /// <summary>
    /// The visual mode.
    /// </summary>
    string visualMode = "None";

    /// <summary>
    /// The is shown close button.
    /// </summary>
#if WINDOWS || MACCATALYST
    bool isShownCloseButton = true;
#elif ANDROID || IOS
    bool isShownCloseButton = true;
#endif

    /// <summary>
    /// The selected item.
    /// </summary>
    object selectedItem = "Television";

    /// <summary>
    /// The selected filter items collection.
    /// </summary>
    ObservableCollection<object> selectedFilterItems = new ObservableCollection<object>();

    /// <summary>s
    /// The input collection.
    /// </summary>
    ObservableCollection<string>? brands;

    /// <summary>
    /// The filter collection.
    /// </summary>
    List<string> filterChips = new List<string>();

    /// <summary>
    /// The action collection.
    /// </summary>
    List<string> actionChips = new List<string>() { "Search by brands", "Search by features" };

    /// <summary>
    /// The action command.
    /// </summary>
    ICommand? actionCommand;

    /// <summary>
    /// The place holder text.
    /// </summary>
    string placeHolderText = "Enter brand name";

    /// <summary>
    /// The color items filter collection.
    /// </summary>
    public List<string>? colorItems = new List<string>() { "Blue", "Grey", "Green", "Pink", "Sandal" };

    /// <summary>
    /// The add ons filter collection.
    /// </summary>
    List<string>? addOnItems = new List<string>() { "Fast Charge", "512 MB SD Card", "2 Years Extended Warranty" };

    /// <summary>
    /// The devivery option choice collection.
    /// </summary>
    List<string>? deliveryOptions = new List<string>() { "Free Delivery", "+$5 for 1 Day Delivery" };

    /// <summary>
    /// The control list choice collection.
    /// </summary>
    List<string>? controlsList = new List<string>() { "syncfusion", "dot-net", "android" };
    /// <summary>
    /// The selected color filter item.
    /// </summary>
    object selectedColorItem = "Blue";

    /// <summary>
    /// The image property to hold the speaker image..
    /// </summary>
    string image = "BlueSpeaker.png";
   
    /// <summary>
    /// The selected add on filter item.
    /// </summary>
    ObservableCollection<object> selectedaddOnItems = new() { "Fast Charge" };

    /// <summary>
    /// The selected delivery option choice item.
    /// </summary>
    object selectedDeliveryOptions = "Free Delivery";

    /// <summary>
    /// The total amount.
    /// </summary>
    int totalAmount = 0;

    /// <summary>
    /// The fast charge price.
    /// </summary>
    int fastChargePrice=0;

    /// <summary>
    /// The sd card price
    /// </summary>
    int sdCardPrice=0;

    /// <summary>
    /// The warrenty price.
    /// </summary>
    int warrentyPrice=0;

    /// <summary>
    /// The delivery date.
    /// </summary>
    DateTime deliveryDate=DateTime.Now;

    /// <summary>
    /// The date.
    /// </summary>
    string date = " ";

    /// <summary>
    /// The total price.
    /// </summary>
    string totalPrice = " ";

    /// <summary>
    /// The final amount.
    /// </summary>
    int finalAmount = 0;

    /// <summary>
    /// The final price.
    /// </summary>
    int finalPrice = 299;

#if WINDOWS || MACCATALYST
    /// <summary>
    /// The chip type.
    /// </summary>   
    SfChipsType chipType = SfChipsType.Input;
#endif
#endregion

    #region Properties

    /// <summary>
    /// Gets or Sets the image visibility
    /// </summary>
    public bool ShowImage
    {
        get
        {
            return showImage;
        }
        set
        {
            showImage = value;
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
            return textColor;
        }

        set
        {
            textColor = value;
            OnPropertyChanged("TextColor");
        }
    }
#elif WINDOWS || MACCATALYST
    public Color TextColor
    {
        get
        {
            return textColor;
        }

        set
        {
            textColor = value;
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
            return icon;
        }

        set
        {
            icon = value;
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
            return borderColor;
        }

        set
        {
            borderColor = value;
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
            return backgroundColor;
        }

        set
        {
            backgroundColor = value;
            OnPropertyChanged("BackgroundColor");
        }
    }
#elif WINDOWS || MACCATALYST
    public Color BackgroundColor
    {
        get
        {
            return backgroundColor;
        }

        set
        {
            backgroundColor = value;
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
            return borderThickness;
        }
        set
        {
            borderThickness = value;
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
            return rightSlider;
        }
        set
        {
            rightSlider = value;
            CornerRadius = new CornerRadius(cornerRadius.TopLeft, value, value, cornerRadius.BottomRight);
            OnPropertyChanged("RightCornerRadius");
        }
    }
#elif WINDOWS || MACCATALYST
    public int RightCornerRadius
    {
        get
        {
            return rightSlider;
        }
        set
        {
            rightSlider = value;
            CornerRadius = new CornerRadius(cornerRadius.TopLeft, value, value, cornerRadius.BottomRight);
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
            return leftSlider;
        }
        set
        {
            leftSlider = value;
            CornerRadius = new CornerRadius(value,cornerRadius.TopRight,cornerRadius.BottomLeft, value);
            OnPropertyChanged("LeftCornerRadius");
        }
    }
#elif MACCATALYST || WINDOWS
    public int LeftCornerRadius
    {
        get
        {
            return leftSlider;
        }
        set
        {
            leftSlider = value;
            CornerRadius = new CornerRadius(value, cornerRadius.TopRight, cornerRadius.BottomLeft, value);
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
            return borderWidth;
        }
        set
        {
            borderWidth = value;
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
            return cornerRadius;
        }
        set
        {

            cornerRadius = value;
            OnPropertyChanged("CornerRadius");
        }
    }
#elif WINDOWS || MACCATALYST
    public CornerRadius CornerRadius
    {
        get
        {
            return cornerRadius;
        }
        set
        {

            cornerRadius = value;
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
            return text;
        }
        set
        {
            text = value;
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
            return isShow;
        }
        set
        {
            isShow = value;
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
            return isShowVisual;
        }
        set
        {
            isShowVisual = value;
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
            return isShowVisual;
        }
        set
        {
            isShowVisual = value;
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
            return isShownCloseButton;
        }
        set
        {
            isShownCloseButton = value;
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
            return isShownCloseButton;
        }
        set
        {
            isShownCloseButton = value;
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
            return visualMode;
        }
        set
        {
            visualMode = value;
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
            return selectedItem;
        }
        set
        {
            selectedItem = value;
            if (selectedItem != null)
            {
                if (!string.IsNullOrEmpty("SelectedItem"))
                {
                    if (selectedItem.Equals("Television"))
                    {
                        InputItems = televisionItems;
                        FilterItems = new List<string>() { "LED", "LCD", "Wi-Fi", "4K", "Ultra HD" };
                    }
                    else if (selectedItem.Equals("Washer"))
                    {
                        InputItems = washerItems;
                        FilterItems = new List<string>() { "Front Load", "Top Load" };
                    }
                    else if (selectedItem.Equals("Air Conditioner"))
                    {
                        InputItems = airConditionerItems;
                        FilterItems = new List<string>() { "Window", "Portable", "Hybrid" };
                    }
                }
                selectedFilterItems.Clear();
            }
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
            return inputText;
        }
        set
        {
            inputText = value;
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
            return actionCommand;
        }
        set
        {
            actionCommand = value;
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
            return placeHolderText;
        }
        set
        {
            placeHolderText = value;
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
            return brands;
        }
        set
        {
            brands = value;
            if (brands != null && brands.Count == 0)
            {
                filterChips.Clear();
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
            return filterChips;
        }
        set
        {
            filterChips = value;
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
            return actionChips;
        }
        set
        {
            actionChips = value;
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
            return choiceItems;
        }
        set
        {
            choiceItems = value;
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
            return result;
        }
        set
        {
            result = value;
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
            return selectedFilterItems;
        }
        set 
        { 
            selectedFilterItems = value; 
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
            return addOnItems;
        }
        set
        {
            addOnItems = value;
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
            return deliveryOptions;
        }
        set
        {
            deliveryOptions = value;
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
            return controlsList;
        }
        set
        {
            controlsList = value;
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
            return selectedColorItem;
        }
        set
        {
            selectedColorItem= value;
            if (selectedColorItem != null)
            {
                if (!string.IsNullOrEmpty("SelectedColorItem"))
                {
                    if (selectedColorItem.Equals("Blue"))
                    {
                        Image = "BlueSpeaker.png";
                    }
                    else if (selectedColorItem.Equals("Grey"))
                    {
                        Image = "GreySpeaker.png";
                    }
                    else if (selectedColorItem.Equals("Green"))
                    {
                        Image = "GreenSpeaker.png";
                    }
                    else if (selectedColorItem.Equals("Pink"))
                    {
                        Image = "PinkSpeaker.png";
                    }
                    else if (selectedColorItem.Equals("Sandal"))
                    {
                        Image = "SandalsSpeaker.png";
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
            return image;
        }
        set
        {
            image = value;
            OnPropertyChanged(nameof(Image));
        }
    }

    /// <summary>
    /// Gets or sets the fast charge price.
    /// </summary>
    /// <value>The FastChargePrice.</value>
    public int FastChargePrice
    {
        get { return fastChargePrice; }
        set { fastChargePrice = value; OnPropertyChanged(nameof(FastChargePrice)); }
    }

    /// <summary>
    /// Gets or sets the sd card price.
    /// </summary>
    /// <value>The SDCardPrice.</value>
    public int SDCardPrice
    {
        get { return sdCardPrice; }
        set { sdCardPrice = value; OnPropertyChanged(nameof(SDCardPrice)); }
    }

    /// <summary>
    /// Gets or sets the warrenty price.
    /// </summary>
    /// <value>The WarrentyPrice.</value>
    public int WarrentyPrice
    {
        get { return warrentyPrice; }
        set { warrentyPrice = value; OnPropertyChanged(nameof(WarrentyPrice)); }
    }

    /// <summary>
    /// Gets or sets the selected add on items.
    /// </summary>
    /// <value>The SelectedAddOnItems.</value>
    public ObservableCollection<object> SelectedAddOnItems
    {
        get
        {
            return selectedaddOnItems;
        }
        set
        {
            selectedaddOnItems=value;
            OnPropertyChanged(nameof(SelectedAddOnItems));
        }
    }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The Date.</value>
    public string Date
    {
        get { return date; }
        set { date = value; OnPropertyChanged(nameof(Date)); }
    }

    /// <summary>
    /// Gets or sets the total price.
    /// </summary>
    /// <value>The TotalPrice.</value>
    public string TotalPrice
    {
        get { return totalPrice; }
        set { totalPrice = value; OnPropertyChanged(nameof(TotalPrice)); }
    }

    /// <summary>
    /// Gets or sets the selected delivery option.
    /// </summary>
    /// <value>The DeliveryOption.</value>
    public object SelectedDeliveryOption
    {
        get
        {
            return selectedDeliveryOptions;
        }
        set
        {
            selectedDeliveryOptions = value;
            if (SelectedDeliveryOption != null)
            {
                if (selectedDeliveryOptions.Equals("Free Delivery"))
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
            return totalAmount; 
        }
        set
        {
            totalAmount=value;
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
            return finalAmount;
        }
        set
        { 
            finalAmount=value; 
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
            return finalPrice;
        }
        set
        {
            finalPrice = value;
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
            return deliveryDate;
        }
        set
        {
            deliveryDate = value;
            OnPropertyChanged(nameof(DeliveryDate));
        }
    }
#if WINDOWS || MACCATALYST
    public SfChipsType ChipType
    {
        get
        {
            return chipType;
        }
        set
        {
            chipType = value;
            OnPropertyChanged(nameof(ChipType));
        }
    }
#endif
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
        if (obj != null && obj.ToString().Equals("Search by brands", StringComparison.Ordinal) && selectedItem != null && brands.Count > 0)
        {
            foreach (var brand in brands)
            {
                Result.Add(new Random().Next(1, 5).ToString() + " " + brand.ToString() + " " + selectedItem.ToString() + " found");
            }
        }

        else if (FilterItems.Count > 0 && selectedItem != null &&  selectedFilterItems.Count > 0 && brands.Count > 0)
        {


            foreach (var feature in selectedFilterItems)
            {
                foreach (var brand in brands)
                {
                    Result.Add(new Random().Next(1, 5).ToString() + " " + feature.ToString() + " " + selectedItem.ToString() + " found in " + brand.ToString());
                }
            }
        }
        else
        {
            Result.Add("No results found.");
        }
    }
}
