using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.NumericUpDown
{
    internal partial class ProductInfo : INotifyPropertyChanged
    {
        private double _price;

        private double _totalPrice;

        private int _count = 0;

        private string _productName = "Hello";

        private string _productImage = "pizza.jpg";

        private string _productDescription = "Hi";

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                TotalPrice = value * Count;
                OnPropertyChanged(nameof(Price));
            }
        }
        public double TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                TotalPrice = value * Price;
                OnPropertyChanged(nameof(Count));
            }
        }

        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public string ProductImage
        {
            get { return _productImage; }
            set
            {
                _productImage = value;
                OnPropertyChanged(nameof(ProductImage));
            }
        }

        public string ProductDescription
        {
            get { return _productDescription; }
            set
            {
                _productDescription = value;
                OnPropertyChanged(nameof(ProductDescription));
            }
        }

        public ProductInfo()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
    }
}
