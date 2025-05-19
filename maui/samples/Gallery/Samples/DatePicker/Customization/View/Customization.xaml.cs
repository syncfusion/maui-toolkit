using Syncfusion.Maui.Toolkit.Popup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using Syncfusion.Maui.Toolkit.TextInputLayout;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfDatePicker;

/// <summary>
/// The customization page.
/// </summary>
public partial class Customization : SampleView
{
    /// <summary>
    /// The ToDo details.
    /// </summary>
    ToDoDetails? _toDoDetails;

    /// <summary>
    /// Initializes a new instance of the <see cref="Customization" /> class.
    /// </summary>
    public Customization()
    {
        InitializeComponent();
#if ANDROID || IOS
        datePicker1.HeaderView.Height = 50;
        datePicker1.HeaderView.Text = "Select the Date";
        datePicker1.FooterView.Height = 40;
#else
        datePicker.HeaderView.Height = 50;
        datePicker.HeaderView.Text = "Select the Date";
        datePicker.FooterView.Height = 40;
#endif
    }

    /// <summary>
    /// Invoked tap gesture tapped.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnTapGestureTapped(object sender, EventArgs e)
    {
#if ANDROID || IOS
        popup1.Reset();
        popup1.IsOpen = true;
#else
        popup.Reset();
        popup.IsOpen = true;
#endif
    }

    /// <summary>
    /// Invoked item tap gesture tapped.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnItemTapGestureTapped(object sender, EventArgs e)
    {
#if ANDROID || IOS
        if (sender is Grid grid && grid.BindingContext != null && grid.BindingContext is ToDoDetails details)
        {
            datePicker1.SelectedDate = details.Date;
            _toDoDetails = details;
        }

        datePicker1.IsOpen = true;
#else
        if (sender is Grid grid && grid.BindingContext != null && grid.BindingContext is ToDoDetails details)
        {
            datePicker.SelectedDate = details.Date;
            _toDoDetails = details;
        }

        datePicker.IsOpen = true;
#endif
    }

    /// <summary>
    /// Method to handle the date picker ok button clicked event.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnDatePickerOkButtonClicked(object sender, EventArgs e)
    {
        if (sender is Syncfusion.Maui.Toolkit.Picker.SfDatePicker picker && _toDoDetails != null && picker.SelectedDate?.Date != null)
        {
            if (_toDoDetails.Date != picker.SelectedDate)
            {
                _toDoDetails.Date = picker.SelectedDate.Value.Date;
            }

            _toDoDetails = null;
        }

#if ANDROID || IOS
        datePicker1.IsOpen = false;
#else
        datePicker.IsOpen = false;
#endif
    }

    /// <summary>
    /// Method to handle the date picker closed event.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnDatePickerClosed(object sender, EventArgs e)
    {
        _toDoDetails = null;
    }

    /// <summary>
    /// Method to handle the date picker cancel button clicked event.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnDatePickerCancelButtonClicked(object sender, EventArgs e)
    {
        if (sender is Syncfusion.Maui.Toolkit.Picker.SfDatePicker picker)
        {
            _toDoDetails = null;
            picker.IsOpen = false;
        }
    }

    /// <summary>
    /// Method to handle the popup item created event.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnPopupItemCreated(object sender, EventArgs e)
    {
        if (BindingContext != null && BindingContext is DatePickerCustomizationViewModel bindingContext && sender is ToDoDetails details)
        {
            bindingContext.DataSource.Add(details);
        }
    }
}

/// <summary>
/// The DatePickerCustomizationViewModel class.
/// </summary>
public class DatePickerCustomizationViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// The data source.
    /// </summary>
    ObservableCollection<ToDoDetails> _dataSource;

    /// <summary>
    /// Gets or sets the data source.
    /// </summary>
    public ObservableCollection<ToDoDetails> DataSource
    {
        get
        {
            return _dataSource;
        }
        set
        {
            _dataSource = value;
            RaisePropertyChanged("DataSource");
        }
    }

    /// <summary>
    /// Method to raise the property changed event.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="DatePickerCustomizationViewModel"/> class.
    /// </summary>
    public DatePickerCustomizationViewModel()
    {
        _dataSource = new ObservableCollection<ToDoDetails>()
        {
            new ToDoDetails() {Subject = "Get quote from travel agent", Date= DateTime.Now.Date},
            new ToDoDetails() {Subject = "Book flight ticket", Date= DateTime.Now.Date.AddDays(2)},
            new ToDoDetails() {Subject = "Buy travel guide book", Date= DateTime.Now.Date},
            new ToDoDetails() {Subject = "Register for sky diving", Date= DateTime.Now.Date.AddDays(8)},
        };
    }

    /// <summary>
    /// The property changed event.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
}

/// <summary>
/// The ToDoDetails class.
/// </summary>
public class ToDoDetails : INotifyPropertyChanged
{
    /// <summary>
    /// The subject.
    /// </summary>
    string _subject = string.Empty;

    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    public string Subject
    {
        get
        {
            return _subject;
        }
        set
        {
            _subject = value;
            RaisePropertyChanged("Subject");
        }
    }

    /// <summary>
    /// The date.
    /// </summary>
    DateTime _date = DateTime.Now.Date;

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public DateTime Date
    {
        get
        {
            return _date;
        }
        set
        {
            _date = value;
            DateString = _date.Date == DateTime.Now.Date ? "Due today" : _date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            RaisePropertyChanged("Date");
        }
    }

    /// <summary>
    /// The date string.
    /// </summary>
    string _dateString = "Due today";

    /// <summary>
    /// Gets or sets the date string.
    /// </summary>
    public string DateString
    {
        get
        {
            return _dateString;
        }
        set
        {
            _dateString = value;
            RaisePropertyChanged("DateString");
        }
    }

    /// <summary>
    /// Method to raise the property changed event.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// The property changed event.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
}

/// <summary>
/// The DateTimeToColorConverter class.
/// </summary>
public class DateTimeToColorConverter : IValueConverter
{
    /// <summary>
    /// Check the application theme is light or dark.
    /// </summary>
    readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

    /// <summary>
    /// Method to convert the date time to color.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">The target type</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The color based on the date time.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && value is DateTime date)
        {
            if (date.Date == DateTime.Now.Date)
            {
                return _isLightTheme ? Color.FromArgb("#B3261E") : Color.FromArgb("#F2B8B5");
            }
            else if (date.Date < DateTime.Now.Date)
            {
                return _isLightTheme ? Color.FromArgb("#49454F").WithAlpha(0.5f) : Color.FromArgb("#CAC4D0").WithAlpha(0.5f);
            }

            return _isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0");
        }

        return _isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0");
    }

    /// <summary>
    /// Method to convert back the color to date time.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">The target type</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>Empty string.</returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}

/// <summary>
/// The CustomPopUp class.
/// </summary>
public class CustomPopUp : SfPopup
{
    /// <summary>
    /// The SfDatePicker instance.
    /// </summary>
    readonly Syncfusion.Maui.Toolkit.Picker.SfDatePicker _picker;

    /// <summary>
    /// The Entry instance.
    /// </summary>
    readonly Entry _entry;

    /// <summary>
    /// The SfTextInputLayout instance.
    /// </summary>
    readonly SfTextInputLayout _textInput;

    /// <summary>
    /// Initialize the new instance of the <see cref="CustomPopUp"/> class.
    /// </summary>
    public CustomPopUp()
    {
        _picker = new Syncfusion.Maui.Toolkit.Picker.SfDatePicker();
        StackLayout stack = new StackLayout();
        Label label = new Label();
        label.Text = "Subject";
        label.Margin = new Thickness(10, 4);
        label.FontSize = 12;
        stack.Add(label);
        _textInput = new SfTextInputLayout();
        _textInput.Hint = "Title";
        _textInput.HelperText = "Enter Title";
        _entry = new Entry();
        _entry.HeightRequest = 40;
        _entry.Margin = new Thickness(5, 0);
        _textInput.Content = _entry;
        stack.Add(_textInput);
        Label label1 = new Label();
        label1.Text = "Select the date";
        label1.FontSize = 12;
        label1.Margin = new Thickness(10, 5);
        stack.Add(label1);
        _picker.FooterView.Height = 40;
        _picker.HeightRequest = 280;
        _picker.OkButtonClicked += OnPickerOkButtonClicked;
        _picker.CancelButtonClicked += OnPickerCancelButtonClicked;
        stack.Add(_picker);
        stack.VerticalOptions = LayoutOptions.Center;
        ContentTemplate = new DataTemplate(() =>
        {
            return stack;
        });

        HeaderTemplate = new DataTemplate(() =>
        {
            return new Label() { Text = "Add a task", FontSize = 20, HeightRequest = 40, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
        });

#if ANDROID || IOS || MACCATALYST
        HeightRequest = 470;
#else
        HeightRequest = 450;
#endif
        WidthRequest = 300;
        ShowFooter = false;
        ShowHeader = true;
        HeaderHeight = 40;
        PopupStyle.CornerRadius = new CornerRadius(5);
    }

    /// <summary>
    /// Invokes when the picker cancel button clicked.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnPickerCancelButtonClicked(object? sender, EventArgs e)
    {
        Reset();
        IsOpen = false;
    }

    /// <summary>
    /// Invokes when the picker ok button clicked.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event args.</param>
    void OnPickerOkButtonClicked(object? sender, EventArgs e)
    {
        if (_picker.SelectedDate != null)
        {
            OnCreated?.Invoke(new ToDoDetails() { Date = _picker.SelectedDate.Value, Subject = _entry.Text == string.Empty ? "No Title" : _entry.Text }, new EventArgs());
        }

        IsOpen = false;
    }

    /// <summary>
    /// Method to reset the picker and entry.
    /// </summary>
    public void Reset()
    {
        if (_picker != null)
        {
            _picker.SelectedDate = DateTime.Now.Date;
        }

        if (_entry != null)
        {
            _entry.Text = string.Empty;
        }
    }

    /// <summary>
    /// The event handler for the created event.
    /// </summary>
    public event EventHandler? OnCreated;
}