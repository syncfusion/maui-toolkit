using Syncfusion.Maui.Toolkit.Calendar;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using Syncfusion.Maui.Toolkit.Popup;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
    internal class FlightBookingBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Holds the departure date text input layout.
        /// </summary>
        SfTextInputLayout? _views;

        /// <summary>
        /// Holds the return date text input layout.
        /// </summary>
        SfTextInputLayout? _textInputLayout;

        /// <summary>
        /// Holds the trailing label for return date text input layout.
        /// </summary>
        Label? _trailingLabel;

        /// <summary>
        /// Holds the flight booking border.
        /// </summary>
        Border? _border;

        /// <summary>
        /// Holds the return date calendar.
        /// </summary>
        SfCalendar? _calendar1;

        /// <summary>
        /// Holds the departure date calendar.
        /// </summary>
        SfCalendar? _calendar;

        /// <summary>
        /// Holds the return date stack layout.
        /// </summary>
        StackLayout? _returnStackLayout;

        /// <summary>
        /// Holds the single trip radio button.
        /// </summary>
        RadioButton? _singleTrip;

        /// <summary>
        /// Holds the radio group is round trip radio button.
        /// </summary>
        RadioButton? _roundTrip;

        /// <summary>
        /// Holds the departure date label.
        /// </summary>
        Label? _fromLabel1;

        /// <summary>
        /// Holds the return date label.
        /// </summary>
        Label? _fromLabel;

        /// <summary>
        /// Holds the return date.
        /// </summary>
        Label? _returnDate;

        /// <summary>
        /// Holds the selected date.
        /// </summary>
        DateTime? _selectedDate;

        /// <summary>
        /// Holds the search button
        /// </summary>
        Button? _searchButton;

        /// <summary>
        /// Holds the popup.
        /// </summary>
        SfPopup? _popup;

        /// <summary>
        /// Check whether is dark or light theme.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Begins when the behavior attached to the view.
        /// </summary>
        /// <param name="bindable">The sample view.</param>
        protected override void OnAttachedTo(SampleView bindable)
        {
            base.OnAttachedTo(bindable);
            _selectedDate = DateTime.Now.AddDays(1);
            _singleTrip = bindable.Content.FindByName<RadioButton>("singleTrip");
            _border = bindable.Content.FindByName<Border>("border");
#if ANDROID
            if (_border != null)
            {
                _border.Stroke = Colors.Transparent;
            }
#endif
            if (_singleTrip != null)
            {
                _singleTrip.CheckedChanged += RadioButton_CheckedChanged;
            }

            _returnStackLayout = bindable.Content.FindByName<StackLayout>("returnStackLayout");
            _calendar = bindable.Content.FindByName<SfCalendar>("calendar");
            if (_calendar != null)
            {
                _calendar.SelectedDate = DateTime.Now;
                _calendar.ActionButtonClicked += Calendar_ActionButtonClicked;
                _calendar.ActionButtonCanceled += Calendar_ActionButtonCanceled;
            }

            _roundTrip = bindable.Content.FindByName<RadioButton>("roundTrip");
            if (_roundTrip != null)
            {
                _roundTrip.CheckedChanged += RadioButton_CheckedChanged;
                _roundTrip.IsChecked = true;
            }

            _textInputLayout = bindable.Content.FindByName<SfTextInputLayout>("textInputLayout");
            if (_textInputLayout != null)
            {
                var tap = new TapGestureRecognizer();
                tap.Tapped += OnLabelTapped;
                _textInputLayout.GestureRecognizers.Add(tap);
            }

            _fromLabel = bindable.Content.FindByName<Label>("fromLabel");
            if (_fromLabel  != null)
            {
                _fromLabel.Text = DateTime.Now.Date.ToString("dd MMM yyyy");
            }

            if (_fromLabel1 != null)
            {   
                _fromLabel1.Text = _selectedDate.Value.Date.ToString("dd MMM yyyy");
            }

            _searchButton = bindable.Content.FindByName<Button>("searchButton");
            if (_searchButton != null)
            {
                _searchButton.Clicked += SearchButton_Clicked;
            }
        }

        /// <summary>
        /// Method to get the search button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void SearchButton_Clicked(object? sender, EventArgs e)
        {
            _popup = new SfPopup();
            _popup.Show();
            _popup.ShowHeader = false;
            _popup.ShowFooter = true;
            _popup.FooterHeight = 80;
            _popup.HeightRequest = 200;
            _popup.PopupStyle = new PopupStyle { CornerRadius = 15, MessageFontSize = 16 };
            _popup.ContentTemplate = GetContentTemplate(_popup);
            _popup.FooterTemplate = GetFooterTemplate(_popup);
            Random randomNumber = new Random();
            int index = randomNumber.Next(0,20);

            _popup.Message = index + " flights are available on that dates to depart from Cleveland(CLE) to Chicago(CHI) ";
        }

        /// <summary>
        /// Method to get the return date event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void OnLabelTapped(object? sender, TappedEventArgs e)
        {
            if (_calendar == null)
            {
                return;
            }

            _calendar.IsVisible = true;
            _calendar.IsOpen = true;
            _calendar.Mode = CalendarMode.Dialog;
        }

        /// <summary>
        /// Method to get the departure date calendar action button cancel event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void Calendar_ActionButtonCanceled(object? sender, EventArgs e)
        {
            if (_calendar == null)
            {
                return;
            }

            _calendar.IsOpen = false;
        }

        /// <summary>
        /// Method to get the departure date calendar action button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void Calendar_ActionButtonClicked(object? sender, CalendarSubmittedEventArgs e)
        {
            if (_calendar == null)
            {
                return;
            }

            if (_calendar.SelectedDate != null && _calendar1 != null && _fromLabel != null && _fromLabel1 != null)
            {
                _fromLabel.Text = _calendar.SelectedDate.Value.Date.ToString("dd MMM yyyy");
                _selectedDate = _calendar.SelectedDate.Value;
                _calendar1.MinimumDate = _calendar.SelectedDate.Value;
                _calendar1.SelectedDate = _calendar.SelectedDate.Value;
                _fromLabel1.Text = _fromLabel.Text;
            }

            _calendar.IsOpen = false;
        }

        /// <summary>
        /// Method to get the radio button check changed event,=.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void RadioButton_CheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
            var radioButton = (RadioButton?)sender;
            var text = radioButton?.Content as string;

            if (text == "One-way")
            {
                if (_returnStackLayout == null)
                {
                    return;
                }
#if WINDOWS || MACCATALYST
                foreach (var child in _returnStackLayout.Children.ToList())
                {
                    _returnStackLayout.Children.Remove(child);
                }

                // Create and add the returnDate label back to the StackLayout
                _returnDate = new Label
                {
                    Text = "Return Date",
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 18,
                    TextColor = Colors.Gray,
                    VerticalTextAlignment = TextAlignment.Center,
#if MACCATALYST
                    Margin = new Thickness(0,20,0,0),
#else
                    Margin = new Thickness(0,0,0,0),
#endif
                };

                if (_returnStackLayout != null)
                {
                    _returnStackLayout.Children.Add(_returnDate);
                    _returnDate.VerticalTextAlignment = TextAlignment.Center;
                    _returnDate.VerticalOptions = LayoutOptions.Center;

                    _returnStackLayout.HorizontalOptions = LayoutOptions.Center;
                    _returnStackLayout.VerticalOptions = LayoutOptions.Center;
                }

#elif ANDROID || IOS
                if (_views != null)
                {
                    _views.IsEnabled = false;
                }

                if (_fromLabel1 != null)
                {
                    _fromLabel1.IsEnabled = false;
                    _fromLabel1.TextColor = Colors.Gray;
                }

                if (_trailingLabel != null)
                {
                    _trailingLabel.TextColor = Colors.Gray;
                }
#endif
            }
            else if (text == "Round-Trip")
            {
                if (_returnStackLayout == null)
                {
                    return;
                }

                foreach (var child in _returnStackLayout.Children.ToList())
                {
                     _returnStackLayout.Children.Remove(child);
                }

                _returnStackLayout.HorizontalOptions = LayoutOptions.Start;
                _returnStackLayout.VerticalOptions = LayoutOptions.Start;
                _returnDate = new Label
                {
#if ANDROID || IOS
                    Margin = new Thickness(5,0,0,0),
#elif WINDOWS || MACCATALYST
                    Margin = new Thickness(0,0,0,0),
#endif
                    Text = "Return Date:",
                    TextColor = Colors.Gray,
                };

                _returnStackLayout.Children.Add(_returnDate);

                _views = new SfTextInputLayout
                {
#if WINDOWS || MACCATALYST
                    Margin = new Thickness(0,10,0,0),
                    WidthRequest = 150,
#elif ANDROID || IOS
                    WidthRequest = 280,
                    Margin = new Thickness(10, 10, 0, 0),
#endif
                    ShowHint = false,
                    HeightRequest = 65,
                    ContainerType = ContainerType.Outlined,
                    TrailingViewPosition = ViewPosition.Inside,
                    ContainerBackground = Colors.Transparent
                };

                // Add necessary children to the new TextInputLayout
                _fromLabel1 = new Label
                {
                    Margin = new Thickness(15, 0, 0, 15),
                    
                    HeightRequest = 40,
                    // Set your text here
                    VerticalTextAlignment = TextAlignment.Center
                };

                if (_selectedDate != null)
                {
                    _fromLabel1.Text = _selectedDate!.Value.Date.ToString("dd MMM yyyy");
                }

                _views.Content = _fromLabel1;

                _trailingLabel = new Label
                {
                    Text = "\ue757", // Set your text here
                    FontSize = 20,
                    FontFamily = "MauiSampleFontIcon",
                    HeightRequest = 20
                };
                _trailingLabel.SetDynamicResource(Label.TextColorProperty, "SfDataFormNormalEditorStroke");
                _views.TrailingView = _trailingLabel;

                // Add tap gesture recognizer to TextInputLayout
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped1; ;
                _views.GestureRecognizers.Add(tapGestureRecognizer);

                // Add the new TextInputLayout to the StackLayout
                _returnStackLayout.Children.Add(_views);

                _calendar1 = new SfCalendar();
                _calendar1.IsVisible = false;
                if (_calendar != null && _calendar.SelectedDate != null)
                {
                    _calendar1.MinimumDate = _calendar.SelectedDate.Value;
                }

                _calendar1.Background = Colors.Transparent;
                _calendar1.FooterView.ShowActionButtons = true;
                _calendar1.FooterView.ShowTodayButton = true;
                _calendar1.ActionButtonClicked += Calendar1_ActionButtonClicked1;
                _calendar1.ActionButtonCanceled += Calendar1_ActionButtonCanceled1;
                _returnStackLayout.Children.Add(_calendar1);
            }
        }

        /// <summary>
        /// Method to get the return date calendar cancel click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Calendar1_ActionButtonCanceled1(object? sender, EventArgs e)
        {
            if (_calendar1 != null)
            {
                _calendar1.IsOpen = false;
            }
        }

        /// <summary>
        /// Method to get the return date calendar action click event. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void Calendar1_ActionButtonClicked1(object? sender, CalendarSubmittedEventArgs e)
        {
            if (_fromLabel1 != null && _calendar1 != null)
            {
                _fromLabel1.Text = _calendar1?.SelectedDate?.Date.ToString("dd MMM yyyy");
                _selectedDate = _calendar1?.SelectedDate;
            }

            if (_calendar1 != null)
            {
                _calendar1.IsOpen = false;
            }
        }

        /// <summary>
        /// Method to get the label tap gesture recognizer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void TapGestureRecognizer_Tapped1(object? sender, TappedEventArgs e)
        {
            if (_calendar1 == null)
            {
                return;
            }

            _calendar1.IsVisible = true;
            _calendar1.SelectedDate = _selectedDate;
            _calendar1.Mode = CalendarMode.Dialog;
            _calendar1.IsOpen = true;
        }

        /// <summary>
        /// Method to get footer template.
        /// </summary>
        /// <param name="popup">The popup.</param>
        /// <returns>Returns the data template.</returns>
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
                    CornerRadius = 20,
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
        /// Method to get the ok button style.
        /// </summary>
        /// <returns></returns>
        Style GetOkButtonStyle()
        {
            return new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.CornerRadiusProperty, Value = 15 },
                    new Setter { Property = Button.BorderColorProperty, Value = Color.FromArgb("#6750A4") },
                    new Setter { Property = Button.BorderWidthProperty, Value = 1 },
                    new Setter { Property = Button.BackgroundColorProperty, Value = GetDynamicColor("SfCalendarTodayHighlightColor") },
                    new Setter { Property = Button.TextColorProperty, Value = GetDynamicColor() },
                    new Setter { Property = Button.FontSizeProperty, Value = 14 },
                }
            };
        }

        /// <summary>
        /// Method the get the dynamic color based on theme.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns></returns>
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
        /// Method to get the content template.
        /// </summary>
        /// <param name="popup">The pop up.</param>
        /// <returns>The data template.</returns>
        DataTemplate GetContentTemplate(SfPopup popup)
        {
            var contentTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
                var label = new Label
                {
                    FontFamily = "OpenSansRegular",
                    LineBreakMode = LineBreakMode.WordWrap,
                    Padding = new Thickness(20, 20, 0, 0),
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start
                };

                label.BindingContext = popup;
                label.SetBinding(Label.TextProperty, "Message");
                var stackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 2, 0, 0),
                    HeightRequest = 1,
                };

                stackLayout.BackgroundColor = _isLightTheme ? Color.FromArgb("#611c1b1f") : Color.FromArgb("#61e6e1e5");
                grid.Children.Add(label);
                grid.Children.Add(stackLayout);
                Grid.SetRow(label, 0);
                Grid.SetRow(stackLayout, 1);
                return grid;
            });

            return contentTemplate;
        }

        /// <summary>
        /// Begins when the behavior attached to the view.
        /// </summary>
        /// <param name="bindable">The sample view.</param>
        protected override void OnDetachingFrom(SampleView bindable)
        {
            base.OnDetachingFrom(bindable);
            if (_singleTrip != null)
            {
                _singleTrip.CheckedChanged -= RadioButton_CheckedChanged;
            }

            if (_roundTrip != null)
            {
                _roundTrip.CheckedChanged -= RadioButton_CheckedChanged;
            }

            if (_calendar != null)
            {
                _calendar.ActionButtonClicked -= Calendar_ActionButtonClicked;
                _calendar.ActionButtonCanceled -= Calendar_ActionButtonCanceled;
            }

            if (_searchButton != null)
            {
                _searchButton.Clicked -= SearchButton_Clicked;
            }
        }
    }
}