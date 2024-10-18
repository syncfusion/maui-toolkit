using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Syncfusion.Maui.Toolkit.Carousel
{
    /// <summary>
    /// Sf carousel resources.
    /// </summary>
    internal static class SfCarouselResources
    {
		#region Properties
#if ANDROID
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        internal static Android.Content.Context? Context
        {
            get;
            set;
        }
#endif

		/// <summary>
		/// Gets or sets the resource manager.
		/// </summary>
		/// <value>The resource manager.</value>
		internal static ResourceManager? ResourceManager
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        internal static CultureInfo Culture
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
        }

        /// <summary>
        /// Gets the localized string for load more text.
        /// </summary>
        /// <value>The items count.</value>
        internal static string LoadMoreText
        {
            get
            {
                return string.IsNullOrEmpty(GetString(Culture, "LoadMoreText")) ? "Load More" : GetString(Culture, "LoadMoreText");
            }
        }
		#endregion

		#region Methods

		/// <summary>
		/// Gets the localized string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="culture">The culture.</param>
		/// <param name="text">The text.</param>
		internal static string GetString(CultureInfo culture, string text)
        {
            string value = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(text) && culture != null && ResourceManager != null)
                {
                    var resourceValue = ResourceManager.GetString(text, culture);
                    if (!string.IsNullOrEmpty(resourceValue))
                    {
                        value = resourceValue;
                    }
                }
            }
            catch
            {
            }
            return value;
        }
		#endregion
	}
}
