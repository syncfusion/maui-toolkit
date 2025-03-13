using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.Cards.SfCards
{
    public class CardDetails : INotifyPropertyChanged
    {
        #region Feilds

        /// <summary>
        /// Represents the card name.
        /// </summary>
        string _cardName = string.Empty;

        /// <summary>
        /// Represents the card number.
        /// </summary>
        string _cardNumber = string.Empty;

        /// <summary>
        /// Represents the card holder name.
        /// </summary>
        string _cardHolderName = string.Empty;

        /// <summary>
        /// Represents the card due date.
        /// </summary>
        string _cardDueDate = string.Empty;

        /// <summary>
        /// Represents the card due amount.
        /// </summary>
        string _cardDueAmount = string.Empty;

        /// <summary>
        /// Represents the due indicator color.
        /// </summary>
        Color _dueIndicatorColor = Colors.Transparent;

        #endregion

        #region Properties

        public string CardName
        {
            get
            {
                return _cardName;
            }
            set
            {
                _cardName = value;
                RaisePropertyChanged("CardName");
            }
        }

        public string CardNumber
        {
            get
            {
                return _cardNumber;
            }
            set
            {
                _cardNumber = value;
                RaisePropertyChanged("CardNumber");
            }
        }

        public string CardHolderName
        {
            get
            {
                return _cardHolderName;
            }
            set
            {
                _cardHolderName = value;
                RaisePropertyChanged("CardHolderName");
            }
        }

        public string CardDueDate
        {
            get
            {
                return _cardDueDate;
            }
            set
            {
                _cardDueDate = value;
                RaisePropertyChanged("CardDueDate");
            }
        }

        public string CardDueAmount
        {
            get
            {
                return _cardDueAmount;
            }
            set
            {
                _cardDueAmount = value;
                RaisePropertyChanged("CardDueAmount");
            }
        }

        public Color DueIndicatorColor
        {
            get
            {
                return _dueIndicatorColor;
            }
            set
            {
                _dueIndicatorColor = value;
                RaisePropertyChanged("DueIndicatorColor");
            }
        }

        #endregion

        #region Property Changed Method

        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }

	public class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Represents the first card details.
        /// </summary>
        CardDetails _firstCardDetails;

        /// <summary>
        /// Represents the second card details.
        /// </summary>
        CardDetails _secondCardDetails;

        /// <summary>
        /// Represents the third card details.
        /// </summary>
        CardDetails _thirdCardDetails;

        /// <summary>
        /// Represents the fourth card details.
        /// </summary>
        CardDetails _fourthCardDetails;

        /// <summary>
        /// Represents the fifth card details.
        /// </summary>
        CardDetails _fifthCardDetails;

        /// <summary>
        /// Represents the corner radius.
        /// </summary>
        double _cornerRadius = 7;

        /// <summary>
        /// Represents the fade out on swiping.
        /// </summary>
        bool _fadeOutOnSwiping = false;

        /// <summary>
        /// Represents the swipe to dismiss.
        /// </summary>
        bool _swipeToDismiss = false;

        #endregion

        #region Constructor

        public ViewModel()
        {
            _firstCardDetails = new CardDetails()
            {
                CardName = "Wells Fargo",
                CardNumber = "XXXX 4976",
                CardHolderName = "Williamson S",
                CardDueDate = $"Due on {DateTime.Now.AddDays(1).ToString("dd")} {DateTime.Now.AddDays(1).ToString("MMM")}",
                CardDueAmount = "$457",
                DueIndicatorColor = Color.FromArgb("#8A1F1F")
            };

            _secondCardDetails = new CardDetails()
            {
                CardName = "Chase Platinum",
                CardNumber = "XXXX 1863",
                CardHolderName = "Williamson S",
                CardDueDate = $"Due on {DateTime.Now.AddDays(5).ToString("dd")} {DateTime.Now.AddDays(5).ToString("MMM")}",
                CardDueAmount = "$300",
                DueIndicatorColor = Color.FromArgb("#875700")
            };

            _thirdCardDetails = new CardDetails()
            {
                CardName = "KeyBank Business",
                CardNumber = "XXXX 0417",
                CardHolderName = "Williamson S",
                CardDueDate = $"Due on {DateTime.Now.AddDays(15).ToString("dd")} {DateTime.Now.AddDays(15).ToString("MMM")}",
                CardDueAmount = "$160",
                DueIndicatorColor = Color.FromArgb("#535353")
            };

            _fourthCardDetails = new CardDetails()
            {
                CardName = "American Express",
                CardNumber = "XXXX 2810",
                CardHolderName = "Williamson S",
                CardDueDate = $"Due on {DateTime.Now.AddDays(5).ToString("dd")} {DateTime.Now.AddDays(5).ToString("MMM")}",
                CardDueAmount = "$320",
                DueIndicatorColor = Color.FromArgb("#875700")
            };

            _fifthCardDetails = new CardDetails()
            {
                CardName = "Bank of America",
                CardNumber = "XXXX 0063",
                CardHolderName = "Williamson S",
                CardDueDate = $"Paid on {DateTime.Now.AddDays(-2).ToString("dd")} {DateTime.Now.AddDays(-2).ToString("MMM")}",
                CardDueAmount = "$250",
                DueIndicatorColor = Color.FromArgb("#1B7A1F")
            };
        }

        #endregion

        #region Properties

        public CardDetails FirstCardDetails
        {
            get
            {
                return _firstCardDetails;
            }
            set
            {
                _firstCardDetails = value;
                OnPropertyChanged("FirstCardDetails");
            }
        }

        public CardDetails SecondCardDetails
        {
            get
            {
                return _secondCardDetails;
            }
            set
            {
                _secondCardDetails = value;
                OnPropertyChanged("SecondCardDetails");
            }
        }

        public CardDetails ThirdCardDetails
        {
            get
            {
                return _thirdCardDetails;
            }
            set
            {
                _thirdCardDetails = value;
                OnPropertyChanged("ThirdCardDetails");
            }
        }

        public CardDetails FourthCardDetails
        {
            get
            {
                return _fourthCardDetails;
            }
            set
            {
                _fourthCardDetails = value;
                OnPropertyChanged("FourthCardDetails");
            }
        }

        public CardDetails FifthCardDetails
        {
            get
            {
                return _fifthCardDetails;
            }
            set
            {
                _fifthCardDetails = value;
                OnPropertyChanged("FifthCardDetails");
            }
        }

        public string IndicatorPosition { get; set; } = "Left";

        public string SwipeDirection { get; set; } = "Right";

        public double CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                _cornerRadius = Math.Round(value);
                OnPropertyChanged("CornerRadius");
            }
        }

        public bool FadeOutOnSwiping
        {
            get
            {
                return _fadeOutOnSwiping;
            }
            set
            {
                _fadeOutOnSwiping = value;
                OnPropertyChanged("FadeOutOnSwiping");
            }
        }

        public bool SwipeToDismiss
        {
            get
            {
                return _swipeToDismiss;
            }
            set
            {
                _swipeToDismiss = value;
                OnPropertyChanged("SwipeToDismiss");
            }
        }

        #endregion

        #region Property Changed Method

        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }
}