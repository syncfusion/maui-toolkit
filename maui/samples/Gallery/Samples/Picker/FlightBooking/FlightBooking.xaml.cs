using System.Globalization;
using Syncfusion.Maui.Toolkit.Popup;
using Syncfusion.Maui.Toolkit.Picker;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfPicker
{
    /// <summary>
    /// The Flight booking sample.
    /// </summary>
    public partial class FlightBooking : SampleView
    {
        /// <summary>
        /// The departure date.
        /// </summary>
        DateTime _from;

        /// <summary>
        /// The return date.
        /// </summary>
        DateTime _to;

        /// <summary>
        /// The list containing departure location details.
        /// </summary>
        List<string> _fromList;

        /// <summary>
        /// The list containing destination location details.
        /// </summary>
        List<string> _toList;

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// List of available countries.
        /// </summary>
        static readonly List<string> Countries = ["UK", "USA", "India", "UAE", "Germany"];

        /// <summary>
        /// List of cities in the UK.
        /// </summary>
        static readonly List<string> UkCities = ["London", "Manchester", "Cambridge", "Edinburgh", "Glasgow", "Birmingham"];

        /// <summary>
        /// List of cities in the USA.
        /// </summary>
        static readonly List<string> UsaCities = ["New York", "Seattle", "Washington", "Chicago", "Boston", "Los Angles"];

        /// <summary>
        /// List of cities in India.
        /// </summary>
        static readonly List<string> IndiaCities = ["Mumbai", "Bengaluru", "Chennai", "Pune", "Jaipur", "Delhi"];

        /// <summary>
        /// List of cities in UAE.
        /// </summary>
        static readonly List<string> UaeCities = ["Dubai", "Abu Dhabi", "Fujairah", "Sharjah", "Ajman", "AL Ain"];

        /// <summary>
        /// List of cities in Germany.
        /// </summary>
        static readonly List<string> GermanyCities = ["Berlin", "Munich", "Frankfurt", "Hamburg", "Cologne", "Bonn"];

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightBooking"/> class.
        /// </summary>
        public FlightBooking()
        {
            InitializeComponent();

            if (popup != null)
            {
                popup.FooterTemplate = GetFooterTemplate(popup);
                popup.ContentTemplate = GetContentTemplate(popup);
            }

            _from = DateTime.Now.Date;
            _to = DateTime.Now.Date;
            _fromList = ["India", "Chennai"];
            _toList = ["USA", "Boston"];
            PickerColumn countyColumn = new PickerColumn() { HeaderText = "Country", SelectedIndex = 2, ItemsSource = Countries, Width = 150 };
            PickerColumn countyColumn1 = new PickerColumn() { HeaderText = "Country", SelectedIndex = 1, ItemsSource = Countries, Width = 150 };
            PickerColumn cityColumn = new PickerColumn() { HeaderText = "City", SelectedIndex = 2, ItemsSource = IndiaCities, Width = 150 };
            PickerColumn cityColumn1 = new PickerColumn() { HeaderText = "City", SelectedIndex = 4, ItemsSource = UsaCities, Width = 150 };
            //// To initialize the picker based on the platform.
#if ANDROID || IOS
            mobileFromPicker.Columns = [countyColumn, cityColumn];
            mobileToPicker.Columns = [countyColumn1, cityColumn1];
            mobileGrid.IsVisible = true;
#else
            fromPicker.Columns = new System.Collections.ObjectModel.ObservableCollection<PickerColumn>() { countyColumn, cityColumn };
            toPicker.Columns = new System.Collections.ObjectModel.ObservableCollection<PickerColumn>() { countyColumn1, cityColumn1 };
            frame.IsVisible = true;
#endif
            string str = DateTime.Now.Day.ToString();
            string fromString = str + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToString() + "," + " " + DateTime.Now.Year.ToString();

#if ANDROID || IOS

            mobileDepartureDatePicker.OkButtonClicked += DepartureDatePicker_OkButtonClicked;
            mobileReturnDatePicker.OkButtonClicked += ReturnDatePicker_OkButtonClicked;

            mobileDepartureDatePicker.CancelButtonClicked += DepartureDatePicker_CancelButtonClicked;
            mobileReturnDatePicker.CancelButtonClicked += ReturnDatePicker_CancelButtonClicked;

            mobileDepartureDatePicker.Opened += DepartureDatePicker_OnPopUpOpened;
            mobileReturnDatePicker.Opened += ReturnDatePicker_OnPopUpOpened;

            mobileDepartureDateLabel.Text = fromString;
            mobileReturnDateLabel.Text = fromString;
            if (mobileReturnDatePicker.SelectedDate != null)
            {
                mobileReturnDatePicker.MinimumDate = mobileReturnDatePicker.SelectedDate.Value.Date;
            }

            mobileFromPicker.HeaderView.Height = 40;
            mobileFromPicker.HeaderView.Text = "FROM";
            mobileFromPicker.FooterView.Height = 40;

            mobileToPicker.HeaderView.Height = 40;
            mobileToPicker.HeaderView.Text = "TO";
            mobileToPicker.FooterView.Height = 40;

            mobileDepartureDatePicker.HeaderView.Height = 40;
            mobileDepartureDatePicker.HeaderView.Text = "Select a date";
            mobileDepartureDatePicker.FooterView.Height = 40;
            mobileDepartureDatePicker.MinimumDate = DateTime.Now;

            mobileReturnDatePicker.HeaderView.Height = 40;
            mobileReturnDatePicker.HeaderView.Text = "Select a date";
            mobileReturnDatePicker.FooterView.Height = 40;
#else
            departureDatePicker.OkButtonClicked += DepartureDatePicker_OkButtonClicked;
            returnDatePicker.OkButtonClicked += ReturnDatePicker_OkButtonClicked;

            departureDatePicker.CancelButtonClicked += DepartureDatePicker_CancelButtonClicked;
            returnDatePicker.CancelButtonClicked += ReturnDatePicker_CancelButtonClicked;

            departureDatePicker.Opened += DepartureDatePicker_OnPopUpOpened;
            returnDatePicker.Opened += ReturnDatePicker_OnPopUpOpened;

            departureDateLabel.Text = fromString;
            returnDateLabel.Text = fromString;
            if (returnDatePicker.SelectedDate != null)
            {
                returnDatePicker.MinimumDate = returnDatePicker.SelectedDate.Value.Date;
            }

            fromPicker.HeaderView.Height = 40;
            fromPicker.HeaderView.Text = "FROM";
            fromPicker.FooterView.Height = 40;

            toPicker.HeaderView.Height = 40;
            toPicker.HeaderView.Text = "TO";
            toPicker.FooterView.Height = 40;

            departureDatePicker.HeaderView.Height = 40;
            departureDatePicker.HeaderView.Text = "Select a date";
            departureDatePicker.FooterView.Height = 40;
            departureDatePicker.MinimumDate = DateTime.Now;

            returnDatePicker.HeaderView.Height = 40;
            returnDatePicker.HeaderView.Text = "Select a date";
            returnDatePicker.FooterView.Height = 40;
#endif
        }

        /// <summary>
        /// Method to handle the PopUpOpened event of the departure date picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void DepartureDatePicker_OnPopUpOpened(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileDepartureDatePicker.IsOpen = true;
            mobileDepartureDatePicker.SelectedDate = _from;
#else
            departureDatePicker.IsOpen = true;
            departureDatePicker.SelectedDate = _from;
#endif
        }

        /// <summary>
        /// Method to handle the PopUpOpened event of the return date picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void ReturnDatePicker_OnPopUpOpened(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileReturnDatePicker.IsOpen = true;
            mobileReturnDatePicker.MinimumDate = _from;
            mobileReturnDatePicker.SelectedDate = _to;
#else
            returnDatePicker.IsOpen = true;
            returnDatePicker.MinimumDate = _from;
            returnDatePicker.SelectedDate = _to;
#endif
        }

        /// <summary>
        /// Method to handle the ok button clicked event of the departure date picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void DepartureDatePicker_OkButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            if (mobileDepartureDatePicker.SelectedDate != null)
            {
                _from = mobileDepartureDatePicker.SelectedDate.Value.Date;
            }
            mobileDepartureDateLabel.Text = _from.Day.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_from.Month).ToString() + "," + " " + _from.Year.ToString();
            if (_from.Date > _to.Date)
            {
                _to = _from;
                mobileReturnDateLabel.Text = _to.Day.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_to.Month).ToString() + "," + " " + _to.Year.ToString();
            }

            mobileDepartureDatePicker.IsOpen = false;
#else
            if (departureDatePicker.SelectedDate != null)
            {
               _from = departureDatePicker.SelectedDate.Value.Date;
            }
            departureDateLabel.Text = _from.Day.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_from.Month).ToString() + "," + " " + _from.Year.ToString();
            if (_from.Date > _to.Date)
            {
                _to = _from;
                returnDateLabel.Text = _to.Day.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_to.Month).ToString() + "," + " " + _to.Year.ToString();
            }

            departureDatePicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Method to handle the ok button clicked event of the return date picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void ReturnDatePicker_OkButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            if (mobileReturnDatePicker.SelectedDate != null)
            {
                _to = mobileReturnDatePicker.SelectedDate.Value.Date;
            }

            mobileReturnDateLabel.Text = _to.Day.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_to.Month).ToString() + "," + " " + _to.Year.ToString();
            mobileReturnDatePicker.IsOpen = false;
#else
            if (returnDatePicker.SelectedDate != null)
            {
                _to = returnDatePicker.SelectedDate.Value.Date;
            }

            returnDateLabel.Text = _to.Day.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_to.Month).ToString() + "," + " " + _to.Year.ToString();
            returnDatePicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Method to handle the Cancel button clicked event of the departure date picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void DepartureDatePicker_CancelButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileDepartureDatePicker.IsOpen = false;
#else
            departureDatePicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Method to handle the Cancel button clicked event of the return date picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void ReturnDatePicker_CancelButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileReturnDatePicker.IsOpen = false;
#else
            returnDatePicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Method to handle the tap gesture.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
#if ANDROID || IOS
            mobileFromPicker.IsOpen = true;
            if (mobileFromPicker.Columns[0].ItemsSource is List<string> country)
            {
                int selectedIndex = country.IndexOf(_fromList[0]);
                List<string> cities = GetCityList(Countries[selectedIndex]);
                int citySelectedIndex = cities.IndexOf(_fromList[1]);
                PickerColumn cityColumn = new PickerColumn() { HeaderText = "City", SelectedIndex = citySelectedIndex, ItemsSource = cities, Width = 150 };
                if (mobileFromPicker.Columns[1].ItemsSource != cities)
                {
                    mobileFromPicker.Columns[1] = cityColumn;
                }
                else
                {
                    mobileFromPicker.Columns[1].SelectedIndex = citySelectedIndex;
                }

                mobileFromPicker.Columns[0].SelectedIndex = selectedIndex;
            }

#else
            fromPicker.IsOpen = true;
            if (fromPicker.Columns[0].ItemsSource is List<string> country)
            {
                int selectedIndex = country.IndexOf(_fromList[0]);
                List<string> cities = GetCityList(Countries[selectedIndex]);
                int citySelectedIndex = cities.IndexOf(_fromList[1]);
                PickerColumn cityColumn = new PickerColumn() { HeaderText = "City", SelectedIndex = citySelectedIndex, ItemsSource = cities, Width = 150 };
                if (fromPicker.Columns[1].ItemsSource != cities)
                {
                    fromPicker.Columns[1] = cityColumn;
                }
                else
                {
                    fromPicker.Columns[1].SelectedIndex = citySelectedIndex;
                }

                fromPicker.Columns[0].SelectedIndex = selectedIndex;
            }
#endif
        }

        /// <summary>
        /// Method to handle the tap gesture.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
        {
#if ANDROID || IOS
            mobileToPicker.IsOpen = true;
            if (mobileToPicker.Columns[0].ItemsSource is List<string> country)
            {
                int selectedIndex = country.IndexOf(_toList[0]);
                List<string> cities = GetCityList(Countries[selectedIndex]);
                int citySelectedIndex = cities.IndexOf(_toList[1]);
                PickerColumn cityColumn = new PickerColumn() { HeaderText = "City", SelectedIndex = citySelectedIndex, ItemsSource = cities, Width = 150 };
                if (mobileToPicker.Columns[1].ItemsSource != cities)
                {
                    mobileToPicker.Columns[1] = cityColumn;
                }
                else
                {
                    mobileToPicker.Columns[1].SelectedIndex = citySelectedIndex;
                }

                mobileToPicker.Columns[0].SelectedIndex = selectedIndex;
            }
#else
            toPicker.IsOpen = true;
            if (toPicker.Columns[0].ItemsSource is List<string> country)
            {
                int selectedIndex = country.IndexOf(_toList[0]);
                List<string> cities = GetCityList(Countries[selectedIndex]);
                int citySelectedIndex = cities.IndexOf(_toList[1]);
                PickerColumn cityColumn = new PickerColumn() { HeaderText = "City", SelectedIndex = citySelectedIndex, ItemsSource = cities, Width = 150 };
                if (toPicker.Columns[1].ItemsSource != cities)
                {
                    toPicker.Columns[1] = cityColumn;
                }
                else
                {
                    toPicker.Columns[1].SelectedIndex = citySelectedIndex;
                }

                toPicker.Columns[0].SelectedIndex = selectedIndex;
            }
#endif
        }

        /// <summary>
        /// Method to handle the selection changed event of the from picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void FromPicker_SelectionChanged(System.Object sender, PickerSelectionChangedEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                return;
            }

            string country = Countries[e.NewValue];
            List<string> cities = GetCityList(country);
            PickerColumn cityColumn = new PickerColumn() { HeaderText = "City", SelectedIndex = 0, ItemsSource = cities, Width = 150 };

#if ANDROID || IOS
            if (mobileFromPicker.Columns[1].ItemsSource != cities)
            {
                mobileFromPicker.Columns[1] = cityColumn;
            }
#else
            if (fromPicker.Columns[1].ItemsSource != cities)
            {
                fromPicker.Columns[1] = cityColumn;
            }
#endif
        }

        /// <summary>
        /// Method to handle the selection changed event of the to picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void ToPicker_SelectionChanged(System.Object sender, PickerSelectionChangedEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                return;
            }

            string country = Countries[e.NewValue];
            List<string> cities = GetCityList(country);
            PickerColumn cityColumn = new PickerColumn() { HeaderText = "City", SelectedIndex = 0, ItemsSource = cities, Width = 150 };

#if ANDROID || IOS
            if (mobileToPicker.Columns[1].ItemsSource != cities)
            {
                mobileToPicker.Columns[1] = cityColumn;
            }
#else
            if (toPicker.Columns[1].ItemsSource != cities)
            {
                toPicker.Columns[1] = cityColumn;
            }
#endif
        }

        /// <summary>
        /// Method to handle the cancel button click event of the from picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void FromPicker_CancelButtonClicked(System.Object sender, System.EventArgs e)
        {
#if ANDROID || IOS
            mobileFromPicker.IsOpen = false;
#else
            fromPicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Method to handle the cancel button click event of the to picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void ToPicker_CancelButtonClicked(System.Object sender, System.EventArgs e)
        {
#if ANDROID || IOS
            mobileToPicker.IsOpen = false;
#else
            toPicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Method to handle the Ok button click event of the from picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void FromPicker_OkButtonClicked(System.Object sender, System.EventArgs e)
        {
            if (sender is Syncfusion.Maui.Toolkit.Picker.SfPicker picker)
            {
                int countryColumnIndex = picker.Columns[0].SelectedIndex;
                int cityColumnIndex = picker.Columns[1].SelectedIndex;
                string country = Countries[countryColumnIndex];
                List<string> cities = GetCityList(country);
                string city = cities[cityColumnIndex];
                _fromList = [country, city];
#if ANDROID || IOS
                mobileFromLabel.Text = $"{city}, {country}";
                mobileFromPicker.IsOpen = false;
#else
                fromLabel.Text = $"{city}, {country}";
                fromPicker.IsOpen = false;
#endif
            }
        }

        /// <summary>
        /// Method to handle the Ok button click event of the to picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void ToPicker_OkButtonClicked(System.Object sender, System.EventArgs e)
        {
            if (sender is Syncfusion.Maui.Toolkit.Picker.SfPicker picker)
            {
                int countryColumnIndex = picker.Columns[0].SelectedIndex;
                int cityColumnIndex = picker.Columns[1].SelectedIndex;
                string country = Countries[countryColumnIndex];
                List<string> cities = GetCityList(country);
                string city = cities[cityColumnIndex];
                _toList = [country, city];
#if ANDROID || IOS
                mobileToLabel.Text = $"{city}, {country}";
                mobileToPicker.IsOpen = false;
#else
                toLabel.Text = $"{city}, {country}";
                toPicker.IsOpen = false;
#endif
            }
        }

        /// <summary>
        /// Method to get the city list based on the country.
        /// </summary>
        /// <param name="country">The country name.</param>
        /// <returns>City list based on the country.</returns>
        List<string> GetCityList(string country)
        {
            if (country == "UK")
            {
                return UkCities;
            }
            else if (country == "USA")
            {
                return UsaCities;
            }
            else if (country == "India")
            {
                return IndiaCities;
            }
            else if (country == "UAE")
            {
                return UaeCities;
            }
            else if (country == "Germany")
            {
                return GermanyCities;
            }

            return [];
        }

        /// <summary>
        /// Method to get the dynamic color.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The color.</returns>
        Color GetDynamicColor(string? resourceName = null)
        {
            if (resourceName != null && App.Current != null && App.Current.Resources.TryGetValue(resourceName, out var colorValue) && colorValue is Color color)
            {
                return color;
            }
            else
            {
                if (App.Current != null && App.Current.RequestedTheme == AppTheme.Light)
                {
                    return Color.FromRgb(0xFF, 0xFF, 0xFF);
                }
                else if (App.Current != null && App.Current.RequestedTheme == AppTheme.Dark)
                {
                    return Color.FromRgb(0x38, 0x1E, 0x72);
                }
            }

            return Colors.Transparent;
        }

        /// <summary>
        /// Method to get the Ok button style.
        /// </summary>
        /// <returns>The button style.</returns>
        Style GetOkButtonStyle()
        {
            return new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.CornerRadiusProperty, Value = 20 },
                    new Setter { Property = Button.BorderColorProperty, Value = Color.FromArgb("#6750A4") },
                    new Setter { Property = Button.BorderWidthProperty, Value = 1 },
                    new Setter { Property = Button.BackgroundColorProperty, Value = GetDynamicColor("SfPickerSelectionStroke") },
                    new Setter { Property = Button.TextColorProperty, Value = GetDynamicColor() },
                    new Setter { Property = Button.FontSizeProperty, Value = 14 },
                }
            };
        }

        /// <summary>
        /// Method to get the footer template.
        /// </summary>
        /// <param name="popup">The pop up.</param>
        /// <returns>The data template.</returns>
        DataTemplate GetFooterTemplate(SfPopup popup)
        {
            var footerTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnSpacing = 12,
                    Padding = new Thickness(24)
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                var oKButton = new Button
                {
                    Text = "OK",
                    Style = GetOkButtonStyle(),
                    WidthRequest = 96,
                    HeightRequest = 40
                };

                oKButton.Clicked += (sender, args) =>
                {
                    popup.Dismiss();
                };

                grid.Children.Add(oKButton);
                Grid.SetColumn(oKButton, 1);

                return grid;
            });

            return footerTemplate;
        }

        /// <summary>
        /// Method to get the content template.
        /// </summary>
        /// <param name="popup">The pop up.</param>
        /// <returns>The data template.</returns>
        DataTemplate GetContentTemplate(SfPopup popup)
        {
            var footerTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });

                var label = new Label
                {
					FontFamily = "OpenSansRegular",
                    LineBreakMode = LineBreakMode.WordWrap,
                    Padding = new Thickness(24, 24, 0, 0),
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start
                };

                label.BindingContext = popup;
                label.SetBinding(Label.TextProperty, "Message");

                var stackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 10, 0, 0),
                    HeightRequest = 1,
                };

                stackLayout.BackgroundColor = _isLightTheme ? Color.FromArgb("#611c1b1f") : Color.FromArgb("#61e6e1e5");
                grid.Children.Add(label);
                grid.Children.Add(stackLayout);

                Grid.SetRow(label, 0);
                Grid.SetRow(stackLayout, 1);

                return grid;
            });

            return footerTemplate;
        }

        /// <summary>
        /// Method to handle the search button clicked event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void SearchButton_Clicked(object sender, EventArgs e)
        {
            if (popup == null)
            {
                return;
            }

            Random randomNumber = new Random();
            int index = randomNumber.Next(0, 50);

#if ANDROID || IOS
            popup.Message = index + " Flights are available on that dates to depart from " + mobileFromLabel.Text.ToString();
#else
            popup.Message = index + " Flights are available on that dates to depart from " + fromLabel.Text.ToString();
#endif
            popup.Show();
        }
    }
}
