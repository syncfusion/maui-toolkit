using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Syncfusion.Maui.ControlsGallery.BottomSheet.BottomSheet
{
    public class ItemViewModel
    {
		public ObservableCollection<Item> Items { get; set; }

		public ItemViewModel()
        {

			Items = new ObservableCollection<Item>()
			{
				new Item() { FoodName = "FarmHouse", SubName="New Hand Tossed", Price = 15, ImageName = "pizza1.png", Description = "Delightful combination of onion, capsicum, tomato, and grilled mushroom. Perfect for a veggie lover!" },
				new Item() { FoodName = "Margherita", SubName="Cheese Burst", Price = 12, ImageName = "pizza2.png", Description = "A classic delight with 100% real mozzarella cheese. Simple yet timeless." },
				new Item() { FoodName = "Peppy Paneer", SubName="Fresh Pan", Price = 13, ImageName = "pizza3.png", Description = "Flavorful trio of juicy paneer, crisp capsicum, and spicy red paprika. A tangy treat!" },
				new Item() { FoodName = "Veg Extravaganza", SubName="New Hand Tossed", Price = 20, ImageName = "pizza4.png", Description = "Loaded with black olives, corn, jalapeños, and extra cheese. A veggie lover's dream!" },
				new Item() { FoodName = "Chicken Dominator", SubName="New Hand Tossed", Price = 23, ImageName = "pizza5.png", Description = "Loaded with double pepper barbecue chicken, peri-peri chicken, chicken tikka & grilled chicken rashers" },
				new Item() { FoodName = "Smoked Chicken Gourmet", SubName="Cheese Burst", Price = 25, ImageName = "pizza6.png", Description = "Juicy chicken with bocconcini, olives, bell peppers, and basil pesto. A gourmet delight!" },
				new Item() { FoodName = "Pepper Barbecue Chicken", SubName="Thin Crust", Price = 18, ImageName = "pizza7.png", Description = "Zesty pepper barbecue chicken for that extra zing. A smoky, spicy treat!" },
				new Item() { FoodName = "Cheese Volcano Chicken", SubName="Fresh Pan", Price = 16, ImageName = "pizza8.png", Description = "Molten cheese center with Pepper BBQ and Peri Peri Chicken. Overflowing with flavor!" },
				new Item() { FoodName = "Paneer Makhani", SubName = "New Hand Tossed", Price = 14, ImageName = "pizza9.png", Description = "Indian-style paneer with rich makhani sauce. A delightful desi twist!" },
				new Item() { FoodName = "Mexican Green Wave", SubName = "Cheese Burst", Price = 13, ImageName = "pizza10.png", Description = "Spicy Mexican herbs with onion, capsicum, and jalapeños. A fiesta of flavors!" },
				new Item() { FoodName = "Italian Delight", SubName = "Fresh Pan", Price = 15, ImageName = "pizza11.png", Description = "Olives, bell peppers, and sun-dried tomatoes. A classic Italian indulgence." },
				new Item() { FoodName = "Hawaiian Paradise", SubName = "New Hand Tossed", Price = 17, ImageName = "pizza12.png", Description = "Juicy pineapple chunks, ham, and melted mozzarella. Sweet and savory perfection." },
				new Item() { FoodName = "BBQ Veggie Blast", SubName = "Cheese Burst", Price = 19, ImageName = "pizza13.png", Description = "Grilled veggies topped with BBQ sauce and cheese. A smoky, tangy explosion!" },
				new Item() { FoodName = "Triple Cheese Treat", SubName = "Thin Crust", Price = 21, ImageName = "pizza14.png", Description = "Mozzarella, cheddar, and parmesan cheese overload. A cheesy paradise!" },
				new Item() { FoodName = "Tandoori Paneer", SubName = "Fresh Pan", Price = 22, ImageName = "pizza15.png", Description = "Tandoori-flavored paneer with spicy toppings. Perfect for spice lovers!" },
				new Item() { FoodName = "Chicken Fiesta", SubName = "Cheese Burst", Price = 24, ImageName = "pizza16.png", Description = "Grilled chicken with onion and capsicum. A hearty and delicious choice!" },
				new Item() { FoodName = "Veggie Supreme", SubName = "New Hand Tossed", Price = 18, ImageName = "pizza17.png", Description = "A mix of broccoli, mushrooms, baby corn, and capsicum. A supreme veggie delight!" }
			};
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
