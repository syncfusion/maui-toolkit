using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.NumericEntry
{
    public partial class CultureAndFormattingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

       
        private bool _showGroupSeparator = true;
        private int _minimumIntegerDigits = 5;
        private int _minimumFractionDigits = 2;
        private int _maximumFractionDigits = 3;
        private string _centimeterCustomFormat = string.Empty;
        private string _poundCustomFormat = string.Empty;
        private string _kilobyteCustomFormat = string.Empty;
        private string _litreCustomFormat = string.Empty;


        /// <summary>
        /// Get or Sets the MinimumIntegerDigits
        /// </summary>
        /// <value>The MinimumIntegerDigits.</value>
        public int MinimumIntegerDigits
        {
            get
            {
                return _minimumIntegerDigits;
            }
            set
            {
                if (value < 0)
                {
                    _minimumIntegerDigits = 0;
                }
                else
                {
                    _minimumIntegerDigits = value;
                }
                OnPropertyChanged("MinimumIntegerDigits");
                OnCustomFormatStringChanged();
            }
        }

        /// <summary>
        /// Get or Sets the MinimumFractionDigits
        /// </summary>
        /// <value>The MinimumFractionDigits.</value>
        public int MinimumFractionDigits
        {
            get
            {
                return _minimumFractionDigits;
            }
            set
            {
                if (value < 0)
                {
                    _minimumFractionDigits = 0;
                }
                else
                {
                    _minimumFractionDigits = value;
                }
                OnPropertyChanged("MinimumFractionDigits");
                OnCustomFormatStringChanged();
            }
        }

        /// <summary>
        /// Get or Sets the MaximumFractionDigits
        /// </summary>
        /// <value>The MaximumFractionDigits.</value>
        public int MaximumFractionDigits
        {
            get
            {
                return _maximumFractionDigits;
            }
            set
            {
                if (value < 0)
                {
                    _maximumFractionDigits = 0;
                }
                else
                {
                    _maximumFractionDigits = value;
                }
                OnPropertyChanged("MaximumFractionDigits");
                OnCustomFormatStringChanged();
            }
        }

        /// <summary>
        /// Get or Sets the ShowGroupSeparator
        /// </summary>
        /// <value>The ShowGroupSeparator.</value>
        public bool ShowGroupSeparator
        {
            get
            {
                return _showGroupSeparator;
            }
            set
            {
                _showGroupSeparator = value;
                OnPropertyChanged("ShowGroupSeparator");
                OnCustomFormatStringChanged();
            }
        }

        /// <summary>
        /// Get or Sets the CentimeterCustomFormat
        /// </summary>
        /// <value>The CentimeterCustomFormat.</value>
        public string CentimeterCustomFormat
        {
            get
            {
                return _centimeterCustomFormat;
            }
            set
            {
                _centimeterCustomFormat = value;
                OnPropertyChanged("CentimeterCustomFormat");
            }
        }

        /// <summary>
        /// Get or Sets the PoundCustomFormat
        /// </summary>
        /// <value>The PoundCustomFormat.</value>
        public string PoundCustomFormat
        {
            get
            {
                return _poundCustomFormat;
            }
            set
            {
                _poundCustomFormat = value;
                OnPropertyChanged("PoundCustomFormat");
            }
        }

        /// <summary>
        /// Get or Sets the KilobyteCustomFormat
        /// </summary>
        /// <value>The KilobyteCustomFormat.</value>
        public string KilobyteCustomFormat
        {
            get
            {
                return _kilobyteCustomFormat;
            }
            set
            {
                _kilobyteCustomFormat = value;
                OnPropertyChanged("KilobyteCustomFormat");
            }
        }

        /// <summary>
        /// Get or Sets the LitreCustomFormat
        /// </summary>
        /// <value>The LitreCustomFormat.</value>
        public string LitreCustomFormat
        {
            get
            {
                return _litreCustomFormat;
            }
            set
            {
                _litreCustomFormat = value;
                OnPropertyChanged("LitreCustomFormat");
            }
        }


        private void OnCustomFormatStringChanged()
        {
            string minIntegerFormat = "";
            string minFractionFormat = "";
            string maxFractionFormat = "";

            //Adding group seperator in custom format string.
            if (ShowGroupSeparator)
            {
                minIntegerFormat = minIntegerFormat.PadRight(minIntegerFormat.Length + 1, '#');
                minIntegerFormat = minIntegerFormat.PadRight(minIntegerFormat.Length + 1, ',');
            }

            //Adding minimum integer digits in custom format string.
            if (MinimumIntegerDigits > 0)
            {
                minIntegerFormat = minIntegerFormat.PadRight(minIntegerFormat.Length + MinimumIntegerDigits, '0');
            }
            else
            {
                minIntegerFormat = minIntegerFormat.PadRight(minIntegerFormat.Length + 1, '#');
            }

            //Adding minimum and maximum fraction digits in custom format string.
            minFractionFormat = minFractionFormat.PadRight(MinimumFractionDigits, '0');
            maxFractionFormat = maxFractionFormat.PadRight(Math.Abs(MaximumFractionDigits - MinimumFractionDigits), '#');

            //Creating the custom format string
            string customFormat = string.Format("{0}.{1}{2} ",
                minIntegerFormat,
                minFractionFormat,
                maxFractionFormat

                );

            //Assigning created custom format string to CustomFormat property.
            CentimeterCustomFormat = customFormat + "cm";
            PoundCustomFormat = customFormat + "lb";
            KilobyteCustomFormat = customFormat + "kb";
            LitreCustomFormat = customFormat + "ℓ";
        }

        public CultureAndFormattingViewModel()
        {

        }

    }
}
