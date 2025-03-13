using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.Localization
{
	/// <summary>
	/// Provides access to localization resources.
	/// </summary>
	public class LocalizationResourceAccessor
	{

		#region Properties

		/// <summary>
		/// Gets or sets the resource manager.
		/// </summary>
		/// <value>The resource manager.</value>
		public static ResourceManager? ResourceManager
		{
			get;
			set;
		}

		private static CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

		/// <summary>
		/// Gets the culture.
		/// </summary>
		/// <value>The culture.</value>
		internal static CultureInfo Culture
		{
			get
			{
				return culture;
			}
			set
			{
				culture = value;
			}
		}



		/// <summary>
		/// Gets the localized string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="text">Text type</param>
		public static string? GetString(string text)
		{
			string? value = string.Empty;

			try
			{
				if (ResourceManager != null)
				{
					value = ResourceManager.GetString(text, Culture);
				}
			}
			catch
			{

			}

			return value;
		}

		/// <summary>
		/// Initializes the default localization resource with the specified base name.
		/// </summary>
		/// <param name="baseName">The base name of the resource to initialize.</param>
		/// /// <param name="resourceType">The optional type used to determine the assembly where resources are located. Defaults to the calling assembly if not specified.</param>/// 
		public static void InitializeDefaultResource(string baseName, Type? resourceType = null)
		{
			if (LocalizationResourceAccessor.ResourceManager == null)
			{
				Assembly assembly = resourceType?.Assembly ?? Assembly.GetCallingAssembly();
				LocalizationResourceAccessor.ResourceManager = new System.Resources.ResourceManager(baseName, assembly);
				LocalizationResourceAccessor.Culture = new CultureInfo("en-US");
			}
		}

		#endregion
	}

}
