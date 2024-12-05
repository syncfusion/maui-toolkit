using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.BottomSheet.BottomSheet
{
    public class Item : INotifyPropertyChanged
    {
		int _quantity = 1;
		double _price;
		double _totalPrice;

		public string FoodName { get; set; } = string.Empty;

		public string SubName { get; set; } = string.Empty;

		public string Description {  get; set; } = string.Empty;

        public string ImageName { get; set; } = string.Empty;

		public double Price
		{
			get => _price;
			set
			{
				_price = value;
				OnPropertyChanged(nameof(Price));
				UpdateTotalPrice();
			}
		}

		public int Quantity
		{
			get => _quantity;
			set
			{
				_quantity = value;
				OnPropertyChanged(nameof(Quantity));
				UpdateTotalPrice();
			}
		}

		public double TotalPrice
		{
			get => _totalPrice;
			set
			{
				_totalPrice = value;
				OnPropertyChanged(nameof(TotalPrice));
			}
		}

		private void UpdateTotalPrice()
		{
			TotalPrice = Price * Quantity;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
