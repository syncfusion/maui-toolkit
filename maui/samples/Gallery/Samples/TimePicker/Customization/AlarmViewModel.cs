using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfTimePicker
{
    public class AlarmDetails : INotifyPropertyChanged
    {
        /// <summary>
        /// The alarm time.
        /// </summary>
        TimeSpan _alarmTime;

        /// <summary>
        /// The alarm message.
        /// </summary>
        string _alarmMessage = string.Empty;

        /// <summary>
        /// The is alarm enabled.
        /// </summary>
        bool _isAlarmEnabled = true;

        /// <summary>
        /// The alarm text color.
        /// </summary>
        Color _alarmTextColor = Colors.Black;

        /// <summary>
        /// The alarm secondary text color.
        /// </summary>
        Color _alarmSecondaryTextColor = Color.FromArgb("#49454F");

        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the alarm time.
        /// </summary>
        public TimeSpan AlarmTime
        {
            get
            {
                return _alarmTime;
            }
            set
            {
                _alarmTime = value;
                RaisePropertyChanged("AlarmTime");
            }
        }

        /// <summary>
        /// Gets or sets the alarm message.
        /// </summary>
        public string AlarmMessage
        {
            get
            {
                return _alarmMessage;
            }
            set
            {
                _alarmMessage = value;
                RaisePropertyChanged("AlarmMessage");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the alarm is enabled.
        /// </summary>
        public bool IsAlarmEnabled
        {
            get
            {
                return _isAlarmEnabled;
            }
            set
            {
                _isAlarmEnabled = value;
                RaisePropertyChanged("IsAlarmEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the alarm text color.
        /// </summary>
        public Color AlarmTextColor
        {
            get
            {
                return _alarmTextColor;
            }
            set
            {
                _alarmTextColor = value;
                RaisePropertyChanged("AlarmTextColor");
            }
        }

        /// <summary>
        /// Gets or sets the alarm secondary text color.
        /// </summary>
        public Color AlarmSecondaryTextColor
        {
            get
            {
                return _alarmSecondaryTextColor;
            }
            set
            {
                _alarmSecondaryTextColor = value;
                RaisePropertyChanged("AlarmSecondaryTextColor");
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
    }

    /// <summary>
    /// The view model class.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// The alarm collection.
        /// </summary>
        ObservableCollection<AlarmDetails> _alarmCollection;

        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the alarm collection.
        /// </summary>
        public ObservableCollection<AlarmDetails> AlarmCollection
        {
            get
            {
                return _alarmCollection;
            }
            set
            {
                _alarmCollection = value;
                RaisePropertyChanged("AlarmCollection");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            _alarmCollection = new ObservableCollection<AlarmDetails>()
            {
                new AlarmDetails() { AlarmTime = new TimeSpan(4, 0, 0), AlarmMessage = "Wake Up", IsAlarmEnabled = true, AlarmTextColor = _isLightTheme ? Colors.Black : Colors.White, AlarmSecondaryTextColor= _isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0") },
                new AlarmDetails() { AlarmTime = new TimeSpan(5, 0, 0), AlarmMessage = "Morning Workout", IsAlarmEnabled = true, AlarmTextColor = _isLightTheme ? Colors.Black : Colors.White, AlarmSecondaryTextColor= _isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0") },
                new AlarmDetails() { AlarmTime = new TimeSpan(13, 0, 0), AlarmMessage = "No Alarm Message", IsAlarmEnabled = false, AlarmTextColor = _isLightTheme ? Colors.Black.WithAlpha(0.5f) : Colors.White.WithAlpha(0.5f), AlarmSecondaryTextColor = _isLightTheme ? Color.FromArgb("#49454F").WithAlpha(0.5f) : Color.FromArgb("#CAC4D0").WithAlpha(0.5f) },
            };
        }

        /// <summary>
        /// Method to raise the property changed event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}