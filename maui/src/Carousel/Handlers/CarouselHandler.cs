using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// Handles the functionality and behavior of the carousel control.
	/// </summary>
	/// <exclude/>
	public partial class CarouselHandler : ICarouselHandler
    {
		#region Public Methods
		/// <summary>
		/// Maps cross-platform property changes to native property changes.
		/// </summary>
		public static PropertyMapper<ICarousel, CarouselHandler> Mapper =
            new((PropertyMapper)ViewMapper)
            {
                [nameof(ICarousel.AllowLoadMore)] = MapAllowLoadMore,
                [nameof(ICarousel.EnableVirtualization)] = MapEnableVirtualization,
                [nameof(ICarousel.LoadMoreItemsCount)] = MapLoadMoreItemsCount,
                [nameof(ICarousel.SelectedIndex)] = MapSelectedIndex,
                [nameof(ICarousel.ItemsSource)] = MapItemsSource,
                [nameof(ICarousel.LoadMoreView)] = MapLoadMoreView,
                [nameof(ICarousel.ItemTemplate)] = MapItemTemplate,
                [nameof(ICarousel.ViewMode)] = MapViewMode,
                [nameof(ICarousel.ItemSpacing)] = MapItemSpacing,
                [nameof(ICarousel.RotationAngle)] = MapRotationAngle,
                [nameof(ICarousel.Offset)] = MapOffset,
                [nameof(ICarousel.ScaleOffset)] = MapScaleOffset,
                [nameof(ICarousel.SelectedItemOffset)] = MapSelectedItemOffset,
                [nameof(ICarousel.Duration)] = MapDuration,
                [nameof(ICarousel.ItemWidth)] = MapItemWidth,
                [nameof(ICarousel.ItemHeight)] = MapItemHeight,
                [nameof(ICarousel.EnableInteraction)] = MapEnableInteraction,
                [nameof(ICarousel.SwipeMovementMode)] = MapSwipeMovementMode,
                [nameof(ICarousel.IsEnabled)] = MapIsEnabled,
                [nameof(ICarousel.FlowDirection)] = MapFlowDirection,
            };


        /// <summary>
        /// Maps the cross-platform methods to the native methods.
        /// </summary>
        public static CommandMapper<ICarousel, ICarouselHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(ICarousel.MoveNext)] = MapMoveNext,
            [nameof(ICarousel.MovePrevious)] = MapMovePrevious,
            [nameof(ICarousel.LoadMore)] = MapLoadMore,
        };
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the CarouselHandler class.
		/// </summary>
		public CarouselHandler() : base(Mapper, CommandMapper)
		{
		}
		#endregion

		#region Fields

		/// <summary>
		/// Gets the virtual view associated with the handler.
		/// </summary>
		ICarousel ICarouselHandler.VirtualView => VirtualView;

        /// <summary>
        /// Gets the platform-specific view associated with the handler.
        /// </summary>
        PlatformCarousel ICarouselHandler.PlatformView => PlatformView;
		#endregion

		#region Override Methods
		/// <summary>
		/// The connect handler override method
		/// </summary>
		/// <exclude/>
		/// <param name="platformView"></param>
		protected override void ConnectHandler(PlatformCarousel platformView)
        {
            platformView.Connect(VirtualView);
            base.ConnectHandler(platformView);
        }

		/// <summary>
		/// The disconnect handler override method
		/// </summary>
		/// <exclude/>
		/// <param name="platformView"></param>
		protected override void DisconnectHandler(PlatformCarousel platformView)
        {
            platformView.Disconnect();
            base.DisconnectHandler(platformView);
        }
		#endregion

		#region Private Methods
		/// <summary>
		/// Gets the default view for loading more items.
		/// </summary>
		/// <param name="virtualView">The carousel view</param>
		/// <returns>Returns the default view for loading more items.</returns>
		private static object GetDefaultLoadMore(ICarousel virtualView)
        {
            var loadMoreView = new Border()
            {
                Stroke = Microsoft.Maui.Graphics.Color.FromArgb("#CAC4D0"),
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle() { CornerRadius = 8 },
                HeightRequest = virtualView.ItemHeight,
                WidthRequest = virtualView.ItemWidth,
                BackgroundColor = Microsoft.Maui.Graphics.Color.FromArgb("#FFFBFE"),
#if ANDROID
                InputTransparent = true,
#endif
            };
            loadMoreView.Content = new Label()
            {
                Text = "Load More",
#if ANDROID || IOS
                FontSize = 16,
#else
                FontSize = 22,
#endif
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            return loadMoreView;
        }
		#endregion
	}
}
