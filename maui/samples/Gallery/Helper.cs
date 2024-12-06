using System.Reflection;
using System.Xml.Linq;
namespace Syncfusion.Maui.ControlsGallery
{
	// All the code in this file is included in all platforms.
	/// <summary>
	/// 
	/// </summary>
	public class Helper
	{
	}

	/// <summary>
	/// 
	/// </summary>
	public class ControlTile
	{
		/// <summary>
		/// 
		/// </summary>
		public string Name = string.Empty;

		/// <summary>
		/// 
		/// </summary>
		public Rect Bounds;
	}

	/// <summary>
	/// 
	/// </summary>
	public class PositionHelper
	{
		/// <summary>
		/// 
		/// </summary>
		public static int WindowHeight = 100;

		/// <summary>
		/// 
		/// </summary>
		public static int Column;

		/// <summary>
		/// 
		/// </summary>
		public static Rect currentBounds = new() { Y = -30 };

		/// <summary>
		/// 
		/// </summary>
		public static Rect CurrentBounds
		{
			get
			{
				return currentBounds;
			}
			set
			{
				currentBounds = value;
				if ((value.Y + value.Height) >= WindowHeight)
				{
					Column++;
					currentBounds.Y = -30;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static List<ControlTile> ControlTiles = [];

	}

	/// <summary>
	/// 
	/// </summary>
	public static class BaseConfig
	{
		/// <summary>
		/// 
		/// </summary>
		public static bool IsIndividualSB = false;

		/// <summary>
		/// 
		/// </summary>
		public static Assembly? AssemblyName = null;

		/// <summary>
		/// 
		/// </summary>
		public static Dictionary<string, Stream> AvailableControlStreamList = [];

		/// <summary>
		/// 
		/// </summary>
		public static SBDevice RunTimeDevicePlatform { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public static SBLayout RunTimeDeviceLayout { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public static Page MainPageInit(Assembly appInfo)
		{
			//if (Application.Current != null)
			//{
			//    Application.Current.UserAppTheme = AppTheme.Light;
			//}

			GetDeviceInfo();
			AssemblyName = appInfo;
			var commonNameSpace = appInfo.GetName().Name;
			var stream = appInfo.GetManifestResourceStream(commonNameSpace + ".ControlList.xml");
			if (stream != null)
			{
				var document = XDocument.Load(stream);
				var AllProducts = from AllProduct in document.Descendants("Assemblies") select AllProduct;

				foreach (var assembly in AllProducts)
				{

					var qualifiedInfo = assembly.Attribute("QualifiedInfo")?.Value;
					var assemblyInfo = assembly.Attribute("Prefix")?.Value;
					var products = from product in document.Descendants("Assembly") select product;

					foreach (var item in products)
					{
						var name = item.Attribute("Name")?.Value;
						var assemblyQualifiedInfo = assemblyInfo + "." + "ControlConfig" + "," + assemblyInfo + "," + qualifiedInfo;

						var type = Type.GetType(assemblyQualifiedInfo);
						if (type != null)
						{
							var ControlListPath = type.GetTypeInfo().Assembly;
							var controlStream = ControlListPath.GetManifestResourceStream(commonNameSpace + "." + "SampleList." + name + "SamplesList.xml");
							if (controlStream != null)
							{
								if (!string.IsNullOrEmpty(name))
								{
									Syncfusion.Maui.ControlsGallery.BaseConfig.AvailableControlStreamList.Add(name, controlStream);
								}
							}
						}
					}

				}
			}
#if ANDROID
			var androidPage = new MainPageAndroid();
			NavigationPage.SetHasNavigationBar(androidPage, false);
			var mainPage = new NavigationPage(androidPage);
#elif IOS
			var iosPage = new MainPageiOS();
			NavigationPage.SetHasNavigationBar(iosPage, false);
			var mainPage = new NavigationPage(iosPage);
#elif WINDOWS
			var windowsPage = new MainPageWindows();
			NavigationPage.SetHasNavigationBar(windowsPage, false);
			var mainPage = new NavigationPage(windowsPage);
#else
			var macPage = new MainPageMacCatalyst();
			NavigationPage.SetHasNavigationBar(macPage, false);
			var mainPage = new NavigationPage(macPage);
#endif
			mainPage.BindingContext = PopulateViewModel();
			return mainPage;

		}

		/// <summary>
		/// 
		/// </summary>
		public static void GetDeviceInfo()
		{
#if ANDROID
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDevicePlatform = Syncfusion.Maui.ControlsGallery.SBDevice.Android;
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDeviceLayout = Syncfusion.Maui.ControlsGallery.SBLayout.Mobile;
#elif IOS
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDevicePlatform = Syncfusion.Maui.ControlsGallery.SBDevice.iOS;
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDeviceLayout = Syncfusion.Maui.ControlsGallery.SBLayout.Mobile;
#elif MACCATALYST
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDevicePlatform = Syncfusion.Maui.ControlsGallery.SBDevice.Mac;
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDeviceLayout = Syncfusion.Maui.ControlsGallery.SBLayout.Desktop;
#else
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDevicePlatform = Syncfusion.Maui.ControlsGallery.SBDevice.Windows;
			Syncfusion.Maui.ControlsGallery.BaseConfig.RunTimeDeviceLayout = Syncfusion.Maui.ControlsGallery.SBLayout.Desktop;
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		public static SamplesViewModel PopulateViewModel()
		{
			return new SamplesViewModel();
		}
	}
}
