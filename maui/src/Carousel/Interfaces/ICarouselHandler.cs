using Microsoft.Maui;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// Defines the interface for handling platform-specific implementations of a carousel control.
	/// </summary>
	/// <exclude/>
	public interface ICarouselHandler : IViewHandler
    {
        /// <summary>
        /// Gets the cross-platform view representing the carousel control.
        /// </summary>
        new ICarousel VirtualView { get; }

        /// <summary>
        /// Gets the platform-specific representation of the carousel control.
        /// </summary>
        new PlatformCarousel PlatformView { get; }
    }
}
