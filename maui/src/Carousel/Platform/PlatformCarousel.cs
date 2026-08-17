namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// Represents a platform-specific handler for connecting a carousel view.
	/// </summary>
	/// <exclude/>
	public partial class PlatformCarousel
	{
		private ICarousel? _virtualView;

		/// <summary>
		/// Connects the Maui carousel view with the platform-specific implementation.
		/// </summary>
		/// <param name="mauiView">The carousel view to be connected.</param>
		internal void Connect(ICarousel mauiView)
		{
			_virtualView = mauiView;
		}

		/// <summary>
		/// Disconnects the carousel view, releasing any platform-specific resources.
		/// </summary>
		internal void Disconnect()
		{
#if MACCATALYST || IOS
            if (_handler != null)
            {
                if (GestureRecognizer != null)
                {
                    RemoveGestureRecognizer(GestureRecognizer);
                }

                foreach (var view in Subviews)
                {
                    if (view is PlatformCarouselItem item)
                    {
                        item.InternalCarousel = null;
                    }
                }

                _handler = null;
            }
#elif ANDROID
            if (carouselHandler != null)
            {
                carouselHandler = null;
            }
#endif

			_virtualView = null;
		}

	}

}
