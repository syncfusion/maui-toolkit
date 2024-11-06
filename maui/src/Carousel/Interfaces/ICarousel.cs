
namespace Syncfusion.Maui.Toolkit.Carousel
{
    /// <summary>
    /// Defines the properties and actions for a carousel control interface in the Syncfusion Maui toolkit.
    /// </summary>
    public interface ICarousel : IView
    {
        /// <summary>
        /// Gets or sets a value indicating whether the ability to load more items is enabled in the carousel load more.
        /// </summary>
        bool AllowLoadMore { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether item virtualization is enabled, which can enhance performance by reusing item templates.
        /// </summary>
        bool EnableVirtualization { get; set; }

        /// <summary>
        /// Gets or sets how many additional items to load when more are requested by the carousel's load more feature.
        /// </summary>
        int LoadMoreItemsCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the currently selected carousel item, which is typically centered in the control.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the collection of items displayed by the carousel, allowing each item to have a distinct view.
        /// </summary>
        IEnumerable<object> ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets a custom view for loading additional items, replacing the default "Load More" notice.
        /// </summary>
        View LoadMoreView { get; set; }

        /// <summary>
        /// Gets or sets the template used to define the visual appearance of each carousel item.
        /// </summary>
        DataTemplate ItemTemplate { get; set; }

        /// <summary>
        /// Gets or sets the layout mode for carousel items, allowing for either a default view or a horizontal stack.
        /// </summary>
        ViewMode ViewMode { get; set; }

        /// <summary>
        /// Gets or sets the spacing between items in the carousel, applicable only in linear view mode.
        /// </summary>
        int ItemSpacing { get; set; }

        /// <summary>
        /// Gets or sets the tilt angle for unselected carousel items in the default view, creating a 3D effect.
        /// </summary>
        int RotationAngle { get; set; }

        /// <summary>
        /// Gets or sets the distance between items in the default view, used for spacing them apart.
        /// </summary>
        float Offset { get; set; }

        /// <summary>
        /// Gets or sets the scale offset applied to unselected items, differentiating them from the selected item in the default view.
        /// </summary>
        float ScaleOffset { get; set; }

        /// <summary>
        /// Gets or sets the separation between the selected item and its neighbors in the default view.
        /// </summary>
        int SelectedItemOffset { get; set; }

        /// <summary>
        /// Gets or sets the animation duration for item selection transitions in the carousel.
        /// </summary>
        int Duration { get; set; }

        /// <summary>
        /// Gets or sets the width of each carousel item, allowing customization for better alignment.
        /// </summary>
        int ItemWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of each carousel item, allowing customization for better visual balance.
        /// </summary>
        int ItemHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if users can interact with the carousel, such as swiping.
        /// </summary>
        bool EnableInteraction { get; set; }

        /// <summary>
        /// Gets or sets the swipe mode to control how users navigate through items.
        /// </summary>
        SwipeMovementMode SwipeMovementMode { get; set; }

        /// <summary>
        /// Advances to the next item in the carousel.
        /// </summary>
        void MoveNext() { }

        /// <summary>
        /// Returns to the previous item in the carousel.
        /// </summary>
        void MovePrevious() { }

        /// <summary>
        /// Initiates the loading of additional items into the carousel.
        /// </summary>
        void LoadMore() { }

        /// <summary>
        /// Triggers the event for when the selected item changes.
        /// </summary>
        /// <param name="args">The event arguments containing information about the selection change.</param>
        void RaiseSelectionChanged(SelectionChangedEventArgs args) { }

        /// <summary>
        /// Triggers the event for the beginning of a swipe gesture.
        /// </summary>
        /// <param name="args">The event arguments relating to the swipe start.</param>
        void RaiseSwipeStarted(SwipeStartedEventArgs args) { }

        /// <summary>
        /// Triggers the event for when a swipe gesture ends.
        /// </summary>
        /// <param name="args">The event arguments relating to the swipe end.</param>
        void RaiseSwipeEnded(EventArgs args) { }
    }
}
