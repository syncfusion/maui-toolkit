using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.Maui.ControlsGallery.SparkChart;
using Syncfusion.Maui.Toolkit.SparkCharts;

namespace Syncfusion.Maui.ControlsGallery.SparkChart
{
	public class SparkTrackballViewModel
	{
		public ObservableCollection<ProductInfo> Products { get; }

		public SparkTrackballViewModel()
		{
			Products = new ObservableCollection<ProductInfo>
			{
				new ProductInfo
				{
					Name = "Smart phones",
					SalesData2023 = new List<double> { 1350, 1400, 1480, 1500, 1450, 1490, 1550 },
					SalesData2024 = new List<double> { 1420, 1500, 1650, 1580, 1490, 1520, 1600 },
					SalesData2025 = new List<double> { 1550, 1620, 1700, 1680, 1750, 1820, 1900 }
				},
				new ProductInfo
				{
					Name = "Laptops",
					SalesData2023 = new List<double> { 920, 960, 990, 1010, 980, 970, 1050 },
					SalesData2024 = new List<double> { 980, 1050, 1100, 1080, 1020, 1000, 1120 },
					SalesData2025 = new List<double> { 1200, 1180, 1220, 1250, 1300, 1280, 1350 }
				},
				new ProductInfo
				{
					Name = "Tablets",
					SalesData2023 = new List<double> { 540, 580, 610, 600, 640, 660, 690 },
					SalesData2024 = new List<double> { 620, 640, 700, 680, 720, 760, 780 },
					SalesData2025 = new List<double> { 800, 840, 860, 820, 880, 920, 940 }
				},
				new ProductInfo
				{
					Name = "Smart watches",
					SalesData2023 = new List<double> { 260, 280, 310, 300, 320, 340, 360 },
					SalesData2024 = new List<double> { 300, 320, 360, 350, 340, 365, 380 },
					SalesData2025 = new List<double> { 400, 420, 450, 440, 460, 480, 500 }
				},
				new ProductInfo
				{
					Name = "Desktops",
					SalesData2023 = new List<double> { 680, 660, 670, 690, 700, 710, 720 },
					SalesData2024 = new List<double> { 720, 700, 690, 710, 730, 725, 740 },
					SalesData2025 = new List<double> { 760, 750, 770, 780, 800, 790, 820 }
				},
				new ProductInfo
				{
					Name = "Gaming consoles",
					SalesData2023 = new List<double> { 1050, 1080, 1100, 1090, 1130, 1150, 1170 },
					SalesData2024 = new List<double> { 1120, 1150, 1180, 1160, 1200, 1230, 1250 },
					SalesData2025 = new List<double> { 1300, 1280, 1320, 1350, 1370, 1400, 1420 }
				},
				new ProductInfo
				{
					Name = "Smart TVs",
					SalesData2023 = new List<double> { 880, 920, 950, 970, 990, 1020, 1050 },
					SalesData2024 = new List<double> { 940, 980, 1020, 1040, 1070, 1100, 1130 },
					SalesData2025 = new List<double> { 1150, 1180, 1210, 1240, 1270, 1300, 1340 }
				},
				new ProductInfo
				{
					Name = "Wireless earbuds",
					SalesData2023 = new List<double> { 460, 480, 520, 510, 540, 560, 590 },
					SalesData2024 = new List<double> { 520, 550, 590, 580, 610, 630, 660 },
					SalesData2025 = new List<double> { 680, 710, 750, 740, 770, 800, 830 }
				},
				new ProductInfo
				{
					Name = "Home theatres",
					SalesData2023 = new List<double> { 380, 400, 430, 420, 450, 470, 490 },
					SalesData2024 = new List<double> { 430, 460, 490, 480, 510, 530, 550 },
					SalesData2025 = new List<double> { 570, 600, 630, 620, 650, 670, 700 }
				},
				new ProductInfo
				{
					Name = "VR headsets",
					SalesData2023 = new List<double> { 240, 260, 280, 270, 290, 310, 330 },
					SalesData2024 = new List<double> { 300, 320, 350, 340, 360, 380, 400 },
					SalesData2025 = new List<double> { 420, 450, 480, 470, 500, 520, 540 }
				},
				new ProductInfo
				{
					Name = "Printers",
					SalesData2023 = new List<double> { 520, 510, 530, 540, 550, 560, 580 },
					SalesData2024 = new List<double> { 560, 550, 570, 580, 590, 600, 620 },
					SalesData2025 = new List<double> { 640, 630, 650, 670, 680, 690, 710 }
				},
				new ProductInfo
				{
					Name = "Home assistants",
					SalesData2023 = new List<double> { 710, 740, 760, 780, 800, 830, 850 },
					SalesData2024 = new List<double> { 760, 790, 820, 850, 870, 900, 930 },
					SalesData2025 = new List<double> { 950, 980, 1010, 1040, 1080, 1100, 1130 }
				}
			};
		}
	}
}
