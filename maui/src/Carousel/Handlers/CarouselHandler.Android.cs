using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Toolkit.Internals;
using Microsoft.Maui.Platform;


namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// Provides a handler for the carousel control on the Android platform.
	/// </summary>
	/// <exclude/>
	public partial class CarouselHandler : ViewHandler<ICarousel, PlatformCarousel>
	{
		#region Override Methods
		/// <summary>
		/// Creates a new instance of the platform-specific carousel view.
		/// </summary>
		/// <exclude/>
		/// <returns>The Android platform carousel view.</returns>
		protected override PlatformCarousel CreatePlatformView()
		{
			return new PlatformCarousel(Context);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Maps the AllowLoadMore property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapAllowLoadMore(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateAllowLoadMore(virtualView);
		}

		/// <summary>
		/// Maps the EnableVirtualization property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapEnableVirtualization(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateEnableVirtualization(virtualView);
		}

		/// <summary>
		/// Maps the LoadMoreItemsCount property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapLoadMoreItemsCount(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateLoadMoreItemsCount(virtualView);
		}

		/// <summary>
		/// Maps the SelectedIndex property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapSelectedIndex(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateSelectedIndex(virtualView);
		}

		/// <summary>
		/// Maps the ItemsSource property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemsSource(CarouselHandler handler, ICarousel virtualView)
		{
			if (virtualView.ItemTemplate != null && virtualView.ItemsSource != null && virtualView.ItemsSource.Any())
			{
				handler.PlatformView.ItemsSource = virtualView.ItemsSource;
			}

			if (virtualView.ItemsSource != null && !virtualView.ItemsSource.Any())
			{
				handler.PlatformView.ItemsSource = Enumerable.Empty<object>();
			}
		}

		/// <summary>
		/// Maps the LoadMoreView property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapLoadMoreView(CarouselHandler handler, ICarousel virtualView)
		{
			if (handler.PlatformView == null || handler.MauiContext == null)
			{
				return;
			}

			if (virtualView.LoadMoreView != null)
			{
				virtualView.LoadMoreView.InputTransparent = true;
			}

			Microsoft.Maui.Controls.View? content = virtualView.LoadMoreView;

			if (content != null)
			{
				handler.PlatformView.LoadMoreView = content.ToPlatform(handler.MauiContext);
			}
			else if (virtualView.AllowLoadMore)
			{
				if (GetDefaultLoadMore(virtualView) is Microsoft.Maui.Controls.View nativeElement)
				{
					handler.PlatformView.LoadMoreView = nativeElement.ToPlatform(handler.MauiContext);
				}
			}
		}

		/// <summary>
		/// Maps the ItemTemplate property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemTemplate(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateItemTemplate(virtualView);
			CarouselHandler.MapItemsSource(handler, virtualView);
		}

		/// <summary>
		/// Maps the ViewMode property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapViewMode(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateViewMode(virtualView);
		}

		/// <summary>
		/// Maps the ItemSpacing property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemSpacing(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateItemSpacing(virtualView);
		}

		/// <summary>
		/// Maps the RotationAngle property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapRotationAngle(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateRotationAngle(virtualView);
		}

		/// <summary>
		/// Maps the Offset property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapOffset(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateOffset(virtualView);
		}

		/// <summary>
		/// Maps the ScaleOffset property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapScaleOffset(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateScaleOffset(virtualView);
		}

		/// <summary>
		/// Maps the SelectedItemOffset property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapSelectedItemOffset(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateSelectedItemOffset(virtualView);
		}

		/// <summary>
		/// Maps the Duration property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapDuration(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateDuration(virtualView);
		}

		/// <summary>
		/// Maps the ItemWidth property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemWidth(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateItemWidth(virtualView);
		}

		/// <summary>
		/// Maps the ItemHeight property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemHeight(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateItemHeight(virtualView);
		}

		/// <summary>
		/// Maps the EnableInteraction property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapEnableInteraction(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateEnableInteraction(virtualView);
		}

		/// <summary>
		/// Maps the SwipeMovementMode property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapSwipeMovementMode(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateSwipeMovementMode(virtualView);
		}

		/// <summary>
		/// Maps the IsEnabled property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapIsEnabled(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateIsEnabled(virtualView);
		}

		/// <summary>
		/// Maps the MoveNext method to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="carousel"></param>
		/// <param name="arg3"></param>
		private static void MapMoveNext(ICarouselHandler handler, ICarousel carousel, object? arg3)
		{
			handler?.PlatformView.MoveNext();
		}

		/// <summary>
		/// Maps the MovePrevious method to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="carousel"></param>
		/// <param name="arg3"></param>
		private static void MapMovePrevious(ICarouselHandler handler, ICarousel carousel, object? arg3)
		{
			handler?.PlatformView.MovePrevious();
		}

		/// <summary>
		/// Maps the LoadMore method to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="carousel"></param>
		/// <param name="arg3"></param>
		private static void MapLoadMore(ICarouselHandler handler, ICarousel carousel, object? arg3)
		{
			handler?.PlatformView.LoadMore();
		}
		#endregion
	}
}
