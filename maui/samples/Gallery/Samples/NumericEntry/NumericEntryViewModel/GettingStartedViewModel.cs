using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.NumericEntry
{
    public partial class GettingStartedViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;  
        public void OnPropertyChanged(string name)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

        private string _placeholderText = "Enter Fahrenheit value";       
        private double _minimum = -100;
        private double _maximum = 1000;       
        private double? _celsiusValue = 0.0d;
        private double? _kelvinValue = 273.15d;
        private double? _rankineValue = 491.67d;
        private double? _fahrenheitValue = 32.0d;
        private bool _allowNull = true;


        /// <summary>
        /// Get or Sets the PlaceholderText
        /// </summary>
        /// <value>The PlaceholderText.</value>
        public string PlaceholderText
        {
            get
            {
                return _placeholderText;
            }
            set
            {
                _placeholderText = value;
                OnPropertyChanged("PlaceholderText");
            }
        }

        /// <summary>
        /// Get or Sets the FahrenheitValue
        /// </summary>
        /// <value>The FahrenheitValue.</value>
        public double? FahrenheitValue
        {
            get
            {
                return _fahrenheitValue;
            }
            set
            {
                _fahrenheitValue = value;
                OnPropertyChanged("FahrenheitValue");
                OnFahrenheitValueChanged();

            }
        }

        /// <summary>
        /// Get or Sets the CelsiusValue
        /// </summary>
        /// <value>The CelsiusValue.</value>
        public double? CelsiusValue
        {
            get
            {
                return _celsiusValue;
            }
            set
            {
                _celsiusValue = value;
                OnPropertyChanged("CelsiusValue");
            }
        }

        /// <summary>
        /// Get or Sets the KelvinValue
        /// </summary>
        /// <value>The KelvinValue.</value>
        public double? KelvinValue
        {
            get
            {
                return _kelvinValue;
            }
            set
            {
                _kelvinValue = value;
                OnPropertyChanged("KelvinValue");
            }
        }

        /// <summary>
        /// Get or Sets the RankineValue
        /// </summary>
        /// <value>The RankineValue.</value>
        public double? RankineValue
        {
            get
            {
                return _rankineValue;
            }
            set
            {
                _rankineValue = value;
                OnPropertyChanged("RankineValue");
            }
        }       

        /// <summary>
        /// Get or Sets the Minimum
        /// </summary>
        /// <value>The Minimum.</value>
        public double Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
                OnPropertyChanged("Minimum");
            }
        }

        /// <summary>
        /// Get or Sets the Maximum
        /// </summary>
        /// <value>The Maximum.</value>
        public double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
                OnPropertyChanged("Maximum");
            }
        }


        /// <summary>
        /// Get or Sets the AllowNull
        /// </summary>
        /// <value>The AllowNull.</value>
        public bool AllowNull
        {
            get
            {
                return _allowNull;
            }
            set
            {
                _allowNull = value;
                OnPropertyChanged("AllowNull");
            }
        }

        private void OnFahrenheitValueChanged()
        {
            CelsiusValue = (_fahrenheitValue - 32) * 5 / 9;
            KelvinValue = (_fahrenheitValue - 32) * 5 / 9 + 273.15;
            RankineValue = _fahrenheitValue + 459.67;
        }

        public GettingStartedViewModel()
        {
            
        }

    }

}
