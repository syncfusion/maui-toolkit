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
		internal ICarousel _formsCarousel;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Carousel.Platform.CustomCarouselAdapter"/> class
		/// </summary>
		/// <param name="sfCarousel">The carousel instance</param>
		public CustomCarouselAdapter(ICarousel sfCarousel)
		{
			_formsCarousel = sfCarousel;
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
			if ((_formsCarousel.ItemsSource is System.Collections.IList itemsSource && itemsSource.Count > 0))
			{
				if (_formsCarousel.ItemTemplate != null)
				{
					return GetTemplateView(index);
				}
				if (_formsCarousel.ItemTemplate == null)
				{
					return GetDefaultView(index);
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Private Methods
		 /// <summary>
        /// Sets parent to the template view.
        /// </summary>
        /// <param name="templateLayout"></param>
        /// <param name="formsCarousel"></param>
        void SetParentContent(View templateLayout, ICarousel formsCarousel)
        {
            if (templateLayout != null)
            {
                templateLayout.Parent = (Element)formsCarousel;
            }
        }
		
		/// <summary>
		/// Get the template view
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		Android.Views.View? GetTemplateView(int index)
		{
			if (_formsCarousel.ItemsSource == null || index >= _formsCarousel.ItemsSource.Count())
			{
				return null;
			}

			DataTemplate? template = GetDataTemplate(index);
			if (template == null)
			{
				return null;
			}

			View? templateLayout = CustomCarouselAdapter.CreateTemplateLayout(template);
			if (templateLayout == null)
			{
				return null;
			}

			SetParentContent(templateLayout, _formsCarousel);

			SetBindingContext(templateLayout, index);
			if (_formsCarousel.Handler == null || _formsCarousel.Handler.MauiContext == null)
			{
				return null;
			}

			return templateLayout.ToPlatform(_formsCarousel.Handler.MauiContext);

		}

		/// <summary>
		/// Gets the appropriate DataTemplate for the item at the specified index.
		/// </summary>
		/// <param name="index">The index of the carousel item.</param>
		/// <returns>The DataTemplate to use, or null if no template is available.</returns>
		DataTemplate? GetDataTemplate(int index)
		{
			if (_formsCarousel.ItemTemplate is DataTemplateSelector dataTemplateSelector)
			{
				return dataTemplateSelector.SelectTemplate(_formsCarousel.ItemsSource.ElementAt(index), null);
			}
			return _formsCarousel.ItemTemplate;
		}

		/// <summary>
		/// Creates the template layout from the given DataTemplate.
		/// </summary>
		/// <param name="template">The DataTemplate to create the layout from.</param>
		/// <returns>The created View, or null if unable to create a valid layout.</returns>
		static View? CreateTemplateLayout(DataTemplate template)
		{
			var templateInstance = template.CreateContent();
			return templateInstance switch
			{
				View view => view,
#if NET9_0
				ViewCell viewCell => viewCell.View,
#endif
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
			templateLayout.BindingContext = _formsCarousel.ItemsSource.ElementAt(index);
		}

		/// <summary>
		/// Get the default view
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		Android.Views.View? GetDefaultView(int index)
		{
			Label templateLayout = new Label
			{
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};
			if (_formsCarousel.ItemsSource != null && _formsCarousel.ItemsSource.Any())
			{
				if (index < _formsCarousel.ItemsSource.Count())
				{
					templateLayout.Text = _formsCarousel.ItemsSource.ElementAt(index).ToString();
				}
			}
			if (_formsCarousel.Handler != null && _formsCarousel.Handler.MauiContext != null)
			{
				return templateLayout.ToPlatform(_formsCarousel.Handler.MauiContext);
			}

			return null;
		}
#endregion
	}
}
