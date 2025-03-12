using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.Expander;
public class InvoiceViewModel
{
	#region Fields

	string[] items = new string[]
	{
		"2018 Subaru Outback",
		"All-Weather Mats",
		"Door Edge Guard Kit",
		"Rear Bumper Cover",
		"Wheel Locks",
		"Gas Full Tank"
	};

	string[] prices = new string[]
	{
			"$35,705.00",
			"$101.00",
			"$162.00",
			"$107.00",
			"$81.00",
			"$64.00"
	};

	#endregion

	#region Constructor

	public InvoiceViewModel()
	{
		ItemInfo = new ObservableCollection<InvoiceItem>();
		for (int i = 0; i < items.Count(); i++)
		{
			var invoiceItem = new InvoiceItem();
			invoiceItem.ItemName = items[i];
			invoiceItem.ItemPrice = prices[i];
			ItemInfo.Add(invoiceItem);
		}
	}

	#endregion

	#region Properties

	public ObservableCollection<InvoiceItem> ItemInfo { get; set; }

	#endregion
}

