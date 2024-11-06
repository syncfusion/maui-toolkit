
namespace Syncfusion.Maui.Toolkit.Carousel
{

	/// <summary>
	/// The Items collection changed event arguments class 
	/// </summary>
	internal class ItemsCollectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The item field.
        /// </summary>
        private IList<PlatformCarouselItem>? item;

        /// <summary>
        /// The carousel field.
        /// </summary>
        private PlatformCarousel? sfcarousel;

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<PlatformCarouselItem>? Items
        {
            get { return this.item; }
            set { this.item = value; }
        }

        /// <summary>
        /// Gets or sets the carousel.
        /// </summary>
        /// <value>The carousel.</value>
        public PlatformCarousel? PlatformCarousel
        {
            get { return this.sfcarousel; }
            set { this.sfcarousel = value; }
        }
    }

	/// <summary>
	/// Provides data for the <see cref="SfCarousel.SelectionChanged"/> event.
	/// </summary>
	/// <remarks> This class contains information about the new and old selected items within the <see cref="SfCarousel"/>.</remarks>
	public class SelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the value of the previous item. This property can be used to identify the item that was previously selected in <see cref="SfCarousel"/>.
        /// </summary>
        /// <value>The previous item.</value>
        public object? OldItem { get; internal set; }

        /// <summary>
        /// Gets the value of the selected item. This property identifies the item that is currently selected in <see cref="SfCarousel"/>.
        /// </summary>
        /// <value>The current selected item.</value>
        public object? NewItem { get; internal set; }
    }

	/// <summary>
	/// Provides data for the <see cref="SfCarousel.SwipeStarted"/> event, indicating the direction of the swipe.
	/// </summary>
	/// <remarks>This class contains information about whether a swipe occurred on the left side. </remarks>
	public class SwipeStartedEventArgs : EventArgs
    {
        /// <summary>
        ///  Backing field for the <see cref="IsSwipedLeft"/> property.
        /// </summary>
        private bool isSwipedLeft;

        /// <summary>
        /// Gets a value indicating whether the swipe was to the left on the carousel.
        /// </summary>
        /// <value><c>true</c> if the swipe was to the left; otherwise, <c>false</c>.</value>
        public bool IsSwipedLeft
        {
            get { return isSwipedLeft; }
            internal set { isSwipedLeft = value; }
        }
    }
}
