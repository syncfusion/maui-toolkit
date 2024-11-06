using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.Carousel.Platform
{
    /// <summary>
    /// The Custom carousel adapter class
    /// </summary>
	internal class CustomCarouselAdapter : ICarouselAdapter
    {
		#region Fields
		/// <summary>
		/// Instance for the SfCarousel class.
		/// </summary>
		internal ICarousel FormsCarousel;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Carousel.Platform.CustomCarouselAdapter"/> class
		/// </summary>
		/// <param name="sfCarousel">The carousel instance</param>
		public CustomCarouselAdapter(ICarousel sfCarousel)
        {
            FormsCarousel = sfCarousel;
        }
		#endregion

		#region Public Methods
		/// <summary>
		/// Gets the item view.
		/// </summary>
		/// <returns>The item view.</returns>
		/// <param name="carousel">Carousel control.</param>
		/// <param name="index">Index of carousel item.</param>
		public Android.Views.View? GetItemView(PlatformCarousel carousel, int index)
        {
            if ((FormsCarousel.ItemsSource is System.Collections.IList itemsSource && itemsSource.Count > 0))
            {
                if (FormsCarousel.ItemTemplate != null)
                {
                    return GetTemplateView(index);
                }
                if (FormsCarousel.ItemTemplate == null)
                {
                    return GetDefaultView(index);
                }
                else
                    return null;
            }
            else
                return null;
        }
		#endregion

		#region Private Methods
		/// <summary>
		/// Get the template view
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		Android.Views.View? GetTemplateView(int index)
        {
            if (FormsCarousel.ItemsSource == null || index >= FormsCarousel.ItemsSource.Count())
            {
                return null;
            }

            DataTemplate? template = GetDataTemplate(index);
            if (template == null)
            {
                return null;
            }

            View? templateLayout = CreateTemplateLayout(template);
            if (templateLayout == null)
            {
                return null;
            }
			
            SetBindingContext(templateLayout, index);
            if (FormsCarousel.Handler == null || FormsCarousel.Handler.MauiContext == null)
                return null;
            return templateLayout.ToPlatform(FormsCarousel.Handler.MauiContext);

        }

        /// <summary>
        /// Gets the appropriate DataTemplate for the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the carousel item.</param>
        /// <returns>The DataTemplate to use, or null if no template is available.</returns>
        DataTemplate? GetDataTemplate(int index)
        {
            if (FormsCarousel.ItemTemplate is DataTemplateSelector dataTemplateSelector)
            {
                return dataTemplateSelector.SelectTemplate(FormsCarousel.ItemsSource.ElementAt(index), null);
            }
            return FormsCarousel.ItemTemplate;
        }

        /// <summary>
        /// Creates the template layout from the given DataTemplate.
        /// </summary>
        /// <param name="template">The DataTemplate to create the layout from.</param>
        /// <returns>The created View, or null if unable to create a valid layout.</returns>
        View? CreateTemplateLayout(DataTemplate template)
        {
            var templateInstance = template.CreateContent();
            return templateInstance switch
            {
                View view => view,
                ViewCell viewCell => viewCell.View,
                _ => null
            };
        }

        /// <summary>
        /// Sets the binding context for the template layout.
        /// </summary>
        /// <param name="templateLayout">The template layout to set the binding context for.</param>
        /// <param name="index">The index of the carousel item.</param>
        void SetBindingContext(View templateLayout, int index)
        {
            templateLayout.BindingContext = FormsCarousel.ItemsSource.ElementAt(index);
        }

        /// <summary>
        /// Get the default view
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Android.Views.View? GetDefaultView(int index)
        {
            Label templateLayout = new Label();
            templateLayout.VerticalTextAlignment = TextAlignment.Center;
            templateLayout.HorizontalTextAlignment = TextAlignment.Center;
            if (FormsCarousel.ItemsSource != null && FormsCarousel.ItemsSource.Count() > 0)
            {
                if (index < FormsCarousel.ItemsSource.Count())
                    templateLayout.Text = FormsCarousel.ItemsSource.ElementAt(index).ToString();
            }
            if (FormsCarousel.Handler != null && FormsCarousel.Handler.MauiContext != null)
                return templateLayout.ToPlatform(FormsCarousel.Handler.MauiContext);
            return null;
        }
		#endregion
	}
}
