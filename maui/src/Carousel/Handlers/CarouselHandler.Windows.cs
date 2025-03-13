using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// Provides a handler for the carousel control on the Windows platform.
	/// </summary>
	/// <exclude/>
	public partial class CarouselHandler : ViewHandler<ICarousel, PlatformCarousel>
	{
		/// <summary>
		/// Creates a new instance of the platform-specific carousel view.
		/// </summary>
		/// <returns>The Windows platform carousel view.</returns>
		protected override PlatformCarousel CreatePlatformView()
		{
			return new PlatformCarousel();
		}

		/// <summary>
		/// Maps the AllowLoadMore property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapAllowLoadMore(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.AllowLoadMore = virtualView.AllowLoadMore && virtualView.LoadMoreItemsCount > 0;
		}

		/// <summary>
		/// Maps the EnableVirtualization property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapEnableVirtualization(CarouselHandler handler, ICarousel virtualView)
		{
			if (virtualView.EnableVirtualization && !virtualView.AllowLoadMore)
			{
				handler.PlatformView.EnableVirtualization = true;
			}
			else
			{
				handler.PlatformView.EnableVirtualization = false;
			}
		}

		/// <summary>
		/// Maps the LoadMoreItemsCount property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapLoadMoreItemsCount(CarouselHandler handler, ICarousel virtualView)
		{
			if (virtualView.AllowLoadMore)
			{
				if (virtualView.SelectedIndex >= virtualView.LoadMoreItemsCount)
				{
					virtualView.SelectedIndex = virtualView.LoadMoreItemsCount - 1;
				}
			}
			handler.PlatformView.LoadMoreItemsCount = virtualView.LoadMoreItemsCount;
		}

		/// <summary>
		/// Maps the SelectedIndex property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapSelectedIndex(CarouselHandler handler, ICarousel virtualView)
		{
			if (virtualView.ItemsSource != null)
			{
				if (virtualView.SelectedIndex >= virtualView.ItemsSource.Count() && virtualView.ItemsSource.Any())
				{
					handler.PlatformView.SelectedIndex = virtualView.ItemsSource.Count() - 1;
				}
				else if (virtualView.SelectedIndex < 0)
				{
					handler.PlatformView.SelectedIndex = 0;
				}
				else
				{
					handler.PlatformView.SelectedIndex = virtualView.SelectedIndex;
				}
			}
			if (virtualView.SelectedIndex != handler.PlatformView.SelectedIndex)
			{
				virtualView.SelectedIndex = handler.PlatformView.SelectedIndex;
			}
		}

		/// <summary>
		/// Maps the ItemsSource property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemsSource(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.UpdateItemSource(handler, virtualView);
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

			Microsoft.Maui.Controls.View? content = virtualView.LoadMoreView;
			if (content != null)
			{
				if (content.HeightRequest == -1)
				{
					content.MinimumHeightRequest = virtualView.ItemHeight;
				}

				if (content.WidthRequest == -1)
				{
					content.MinimumWidthRequest = virtualView.ItemWidth;
				}

				var nativeElement = content.ToPlatform(handler.MauiContext);
				handler.PlatformView.LoadMoreView = nativeElement;
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
			//Provided along with Item Source.
		}

		/// <summary>
		/// Maps the ViewMode property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapViewMode(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.ViewMode = virtualView.ViewMode == ViewMode.Linear ? ViewMode.Linear : ViewMode.Default;
		}

		/// <summary>
		/// Maps the ItemSpacing property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemSpacing(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.ItemSpace = virtualView.ItemSpacing;
		}

		/// <summary>
		/// Maps the RotationAngle property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapRotationAngle(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.RotationAngle = virtualView.RotationAngle;
		}

		/// <summary>
		/// Maps the Offset property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapOffset(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.Offset = virtualView.Offset;
		}

		/// <summary>
		/// Maps the ScaleOffset property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapScaleOffset(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.ScaleOffset = virtualView.ScaleOffset;
		}

		/// <summary>
		/// Maps the SelectedItemOffset property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapSelectedItemOffset(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.SelectedItemOffset = virtualView.SelectedItemOffset;
		}

		/// <summary>
		/// Maps the Duration property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapDuration(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.Duration = TimeSpan.FromMilliseconds(virtualView.Duration);
		}

		/// <summary>
		/// Maps the ItemWidth property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemWidth(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.ItemWidth = virtualView.ItemWidth;
		}

		/// <summary>
		/// Maps the ItemHeight property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapItemHeight(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.ItemHeight = virtualView.ItemHeight;
#pragma warning disable IDE0031
			SfCarouselLinearPanel? linerPanel = handler.PlatformView.CarouselLinearPanel;
			if (linerPanel != null)
			{
				linerPanel.InvalidateMeasure();
			}
#pragma warning restore IDE0031
		}

		/// <summary>
		/// Maps the EnableInteraction property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapEnableInteraction(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.EnableInteraction = virtualView.EnableInteraction;
		}

		/// <summary>
		/// Maps the SwipeMovementMode property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapSwipeMovementMode(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.SwipeMovementMode = virtualView.SwipeMovementMode;
		}

		/// <summary>
		/// Maps the MoveNext method to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="carousel"></param>
		/// <param name="arg3"></param>
		private static void MapMoveNext(ICarouselHandler handler, ICarousel carousel, object? arg3)
		{
			handler.PlatformView.MoveNext();
		}

		/// <summary>
		/// Maps the MovePrevious method to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="carousel"></param>
		/// <param name="arg3"></param>
		private static void MapMovePrevious(ICarouselHandler handler, ICarousel carousel, object? arg3)
		{
			handler.PlatformView.MovePrevious();
		}

		/// <summary>
		/// Maps the LoadMore method to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="carousel"></param>
		/// <param name="arg3"></param>
		private static void MapLoadMore(ICarouselHandler handler, ICarousel carousel, object? arg3)
		{
			handler.PlatformView.LoadMore();
		}

		/// <summary>
		/// Maps the IsEnabled property to the platform-specific implementation.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		private static void MapIsEnabled(CarouselHandler handler, ICarousel virtualView)
		{
			handler.PlatformView.IsEnabled = virtualView.IsEnabled;
		}
	}
}
