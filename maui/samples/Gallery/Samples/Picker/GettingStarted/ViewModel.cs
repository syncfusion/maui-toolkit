using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfPicker
{
    public class ViewModel : INotifyPropertyChanged
    {
        private object? selectedItem = "Australia";

        private Brush selectionColor = Colors.Orange;

        public object? SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public Brush SelectionColor
        {
            get
            {
                return selectionColor;
            }
            set
            {
                selectionColor = value;
                RaisePropertyChanged("SelectionColor");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}