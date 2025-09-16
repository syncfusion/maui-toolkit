using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformPoint = Windows.Foundation.Point;
using PrimitivePopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;

namespace Syncfusion.Maui.Toolkit.Internals
{
	internal partial class SfWindowOverlay
	{
		#region Fields

		UIElement? _rootView;
		WindowOverlayStack? _overlayStack;
		PrimitivePopup? _primitivePopup;
		FrameworkElement? _overlayContent;

		#endregion

		#region Internal Methods

		/// <summary>
		/// Returns a <see cref="WindowOverlayStack"/>.
		/// </summary>
		internal virtual WindowOverlayStack CreateStack()
		{
			WindowOverlayStack? windowOverlayStack = null;
			if (_window is not null && _window.Handler is not null)
			{
				IMauiContext? context = _window.Handler.MauiContext;
				if (context is not null)
				{
					windowOverlayStack = (WindowOverlayStack?)_overlayStackView?.ToPlatform(context);
				}
			}

			return windowOverlayStack is not null ? windowOverlayStack : new WindowOverlayStack();
		}

		/// <summary>
		/// Adds or updates the child layout absolutely to the overlay stack.
		/// </summary>
		/// <param name="child">Adds the child to the floating window.</param>
		/// <param name="x">Positions the child in the x point from the application left.</param>
		/// <param name="y">Positions the child in the y point from the application top.</param>
		/// <param name="horizontalAlignment">The horizontal alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Left"/>, the child left position will starts from the x.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Right"/>, the child right position will starts from the x.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Center"/>, the child center position will be the x.</description></item>
		/// </list></param>
		/// <param name="verticalAlignment">The vertical alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Top"/>, the child top position will starts from the y.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Bottom"/>, the child bottom position will starts from the y.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Center"/>, the child center position will be the y.</description></item>
		/// </list></param>
		internal void AddOrUpdate(
			MauiView child,
			double x,
			double y,
			WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left,
			WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
		{
			AddToWindow();
			if (!_hasOverlayStackInRoot || _overlayStack is null || child is null)
			{
				return;
			}

			IMauiContext? context = _window?.Handler?.MauiContext;
			if (context is not null)
			{
				FrameworkElement childView = child.ToPlatform(context);
				UpdateChildPositionAndAlignment(childView, x, y, horizontalAlignment, verticalAlignment);
			}
		}

		/// <summary>
		/// Adds or updates the child layout relatively to the overlay stack. After the relative positioning, the x and y will the added
		/// with the left and top positions.
		/// </summary>
		/// <param name="child">Adds the child to the floating window.</param>
		/// <param name="relative">Positions the child relatively to the relative view.</param>
		/// <param name="x">Adds the x point to the child left after the relative positioning.</param>
		/// <param name="y">Adds the y point to the child top after the relative positioning.</param>
		/// <param name="horizontalAlignment">The horizontal alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Left"/>, the child left position will starts from the relative.Left.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Right"/>, the child right position will starts from the relative.Right.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Center"/>, the child center position will be the relative.Center.</description></item>
		/// </list></param>
		/// <param name="verticalAlignment">The vertical alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Top"/>, the child bottom position will starts from the relative.Top.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Bottom"/>, the child top position will starts from the relative.Bottom.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Center"/>, the child center position will be the relative.Center.</description></item>
		/// </list></param>
		internal void AddOrUpdate(
			MauiView child,
			MauiView relative,
			double x = 0,
			double y = 0,
			WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left,
			WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
		{
			AddToWindow();
			if (!_hasOverlayStackInRoot || _overlayStack is null || child is null || relative is null)
			{
				return;
			}

			if (relative.Width < 0 || relative.Height < 0)
			{
				// TODO: Handle relative view layout here, if needed.
				return;
			}

			if (_window is not null && _window.Handler is not null)
			{
				IMauiContext? context = _window.Handler.MauiContext;
				if (context is not null && relative.Handler is not null && relative.Handler.MauiContext is not null)
				{
					FrameworkElement childView = child.ToPlatform(context);
					FrameworkElement relativeView = relative.ToPlatform(relative.Handler.MauiContext);
					UpdateChildPositionAndAlignment(childView, relativeView, horizontalAlignment, verticalAlignment, x, y);
				}
			}
		}

		/// <summary>
		/// Eliminates the view from the floating window.
		/// </summary>
		/// <param name="view">Specifies the view to be removed from the floating window.</param>
		internal void Remove(MauiView view)
		{
			if (_hasOverlayStackInRoot && view is not null
				&& view.Handler is not null && view.Handler.MauiContext is not null)
			{
				FrameworkElement childView = view.ToPlatform(view.Handler.MauiContext);
				childView.LayoutUpdated -= OnChildLayoutChanged;
				_overlayStack?.Children.Remove(childView);
				_positionDetails.Remove(childView);
			}
		}

		/// <summary>
		/// Removes the overlay from the current view hierarchy and cleans up associated resources.
		/// </summary>
		internal void RemoveOverlay()
		{
			if (_primitivePopup is not null)
			{
				_primitivePopup.IsOpen = false;
				_primitivePopup.Child = null;
			}

			if (_overlayStack is not null && _overlayStack.Children is not null)
			{
				_overlayStack.Children.Clear();
			}
		}

		/// <summary>
		/// Adds a child view to the current window.
		/// </summary>
		/// <remarks>This method ensures that the child view is added to the window and converted to its
		/// platform-specific representation. If the <paramref name="childView"/> is <see langword="null"/> or the
		/// overlay stack is not initialized, the method exits without performing any action.</remarks>
		/// <param name="childView">The child view to be added. Must not be <see langword="null"/>.</param>
		internal bool AddToOverlay(MauiView childView)
		{
			_window = WindowOverlayHelper._window;
			if (_window is not null && _window.Content is not null)
			{
				_rootView = WindowOverlayHelper._platformRootView;
				if (_rootView is not null && _window.Handler is not null && _window.Handler is WindowHandler windowHandler && windowHandler.MauiContext is not null
						 && windowHandler.MauiContext is not null && _rootView.XamlRoot is not null)
				{
					_primitivePopup = new PrimitivePopup();
					_overlayStack ??= CreateStack();
					_primitivePopup.XamlRoot = _rootView.XamlRoot;
					_overlayContent = childView.ToPlatform(windowHandler.MauiContext);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Positions the popup at the specified coordinates relative to the screen.
		/// </summary>
		/// <remarks>The method ensures that the popup is added to the view hierarchy if it is not already
		/// present.  The coordinates are automatically scaled based on the device's screen density.</remarks>
		/// <param name="x">The horizontal position, in device-independent units (DIPs), where the popup should be placed. Defaults to
		/// 0.</param>
		/// <param name="y">The vertical position, in device-independent units (DIPs), where the popup should be placed. Defaults to 0.</param>
		internal void PositionOverlayContent(double x = 0, double y = 0)
		{
			float posX = (float)x;
			float posY = (float)y;

			if (_overlayStack is not null)
			{
				if (_overlayStack.Parent is null && _primitivePopup is not null)
				{
					if (_rootView is not null && _rootView.XamlRoot is not null && _primitivePopup.XamlRoot is null)
					{
						_primitivePopup.XamlRoot = _rootView.XamlRoot;
					}

					_primitivePopup.Child = _overlayStack;
					_primitivePopup.IsOpen = true;
				}

				UpdateOverlaySize();
				_overlayStack.SetValue(Canvas.ZIndexProperty, 99);

				if (_overlayContent is not null)
				{
					if (_overlayContent.Parent is null)
					{
						_overlayStack.Children.Add(_overlayContent);
					}

					Canvas.SetLeft(_overlayContent, posX);
					Canvas.SetTop(_overlayContent, posY);
				}
			}
		}

		/// <summary>
		/// Removes the current overlay window from root view with all its children.
		/// </summary>
		internal void RemoveFromWindow()
		{
			if (_overlayStack is not null)
			{
				ClearChildren();
				if (_primitivePopup is not null)
				{
					_primitivePopup.Child = null;
				}

				_primitivePopup = null;
				_overlayStack = null;
			}

			_hasOverlayStackInRoot = false;
		}

		/// <summary>
		/// Dispose the objects in window overlay.
		/// </summary>
		internal void Dispose()
		{
			RemoveOverlay();
			_window = null;
			_overlayContent = null;
			_primitivePopup = null;
			_overlayStack = null;
		}

		/// <summary>
		/// Update the size of overlaystack.
		/// </summary>
		internal void UpdateOverlaySize()
		{
			if (_overlayStack is not null && WindowOverlayHelper._platformWindow is not null)
			{
				_overlayStack.Width = WindowOverlayHelper._platformWindow.Bounds.Width;
				_overlayStack.Height = WindowOverlayHelper._platformWindow.Bounds.Height;
			}
		}

		#endregion

		#region Private Methods

		void UpdateChildPositionAndAlignment(
			FrameworkElement childView,
			double x,
			double y,
			WindowOverlayHorizontalAlignment horizontalAlignment,
			WindowOverlayVerticalAlignment verticalAlignment)
		{
			PositionDetails details;
			if (_positionDetails.ContainsKey(childView))
			{
				details = _positionDetails[childView];
			}
			else
			{
				details = new PositionDetails();
				_positionDetails.Add(childView, details);
			}

			float posX = (float)x;
			float posY = (float)y;
			details.X = posX;
			details.Y = posY;
			details.HorizontalAlignment = horizontalAlignment;
			details.VerticalAlignment = verticalAlignment;

			if (_overlayStack is not null && !_overlayStack.Children.Contains(childView))
			{
				_overlayStack.Children.Add(childView);
				childView.LayoutUpdated += OnChildLayoutChanged;
			}

			if (childView.DesiredSize.Width <= 0 || childView.DesiredSize.Height <= 0)
			{
				childView.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
			}
			AlignPosition(horizontalAlignment, verticalAlignment,
				(float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
				ref posX, ref posY);
			Canvas.SetLeft(childView, posX);
			Canvas.SetTop(childView, posY);
		}

		void UpdateChildPositionAndAlignment(
			FrameworkElement childView,
			FrameworkElement relativeView,
			WindowOverlayHorizontalAlignment horizontalAlignment,
			WindowOverlayVerticalAlignment verticalAlignment,
			double x = 0,
			double y = 0)
		{
			PositionDetails details;
			if (_positionDetails.ContainsKey(childView))
			{
				details = _positionDetails[childView];
			}
			else
			{
				details = new PositionDetails();
				_positionDetails.Add(childView, details);
			}

			float posX = (float)x;
			float posY = (float)y;
			details.X = posX;
			details.Y = posY;
			details.HorizontalAlignment = horizontalAlignment;
			details.VerticalAlignment = verticalAlignment;
			details.Relative = relativeView;

			if (_overlayStack is not null && !_overlayStack.Children.Contains(childView))
			{
				_overlayStack.Children.Add(childView);
				childView.LayoutUpdated += OnChildLayoutChanged;
			}

			if (childView.DesiredSize.Width <= 0 && childView.DesiredSize.Height <= 0)
			{
				childView.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
			}

			ChildAlignment(childView, relativeView, horizontalAlignment, verticalAlignment, posX, posY, x, y);
		}

		void ChildAlignment(
			FrameworkElement childView,
			FrameworkElement relativeView,
			WindowOverlayHorizontalAlignment horizontalAlignment,
			WindowOverlayVerticalAlignment verticalAlignment,
			float posX,
			float posY,
			double x = 0,
			double y = 0)
		{
			AlignPositionToRelative(horizontalAlignment, verticalAlignment,
				(float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
				relativeView.ActualSize.X, relativeView.ActualSize.Y,
				ref posX, ref posY);

			GeneralTransform transform = relativeView.TransformToVisual(_rootView);
			PlatformPoint relativeViewOrigin = transform.TransformPoint(new PlatformPoint(0, 0));

			if (_rootView is null)
			{
				return;
			}

			var relativePosX = posX + relativeViewOrigin.X;
			double positionX = Math.Max(0, relativePosX > (_rootView.DesiredSize.Width - childView.DesiredSize.Width) ?
				(_rootView.DesiredSize.Width - childView.DesiredSize.Width) : relativePosX);

			var RelativePosY = posY + relativeViewOrigin.Y;
			double positionY = Math.Max(0, RelativePosY > (_rootView.DesiredSize.Height - childView.DesiredSize.Height) ?
				(_rootView.DesiredSize.Height - childView.DesiredSize.Height) : RelativePosY);

			Canvas.SetLeft(childView, positionX);
			Canvas.SetTop(childView, positionY);
		}

		void Initialize()
		{
			if (_hasOverlayStackInRoot || _window is null || _window.Content is null)
			{
				return;
			}

			_rootView = WindowOverlayHelper._platformRootView;

			if (_rootView is null)
			{
				return;
			}

			_overlayStack ??= CreateStack();

			_primitivePopup = new PrimitivePopup();
			UpdateOverlaySize();
			_primitivePopup.Child = _overlayStack;
			if (_rootView.XamlRoot is not null)
			{
				_primitivePopup.XamlRoot = _rootView.XamlRoot;
				_primitivePopup.IsOpen = true;
			}

			_overlayStack.SetValue(Canvas.ZIndexProperty, 99);
			_hasOverlayStackInRoot = true;
		}

		void OnChildLayoutChanged(object? sender, object e)
		{
			if (sender is null)
			{
				return;
			}

			FrameworkElement childView = (FrameworkElement)sender;
			if (_positionDetails.TryGetValue(childView, out PositionDetails? details) && details is not null)
			{
				FrameworkElement? relativeView = details.Relative;
				float posX = details.X;
				float posY = details.Y;
				if (relativeView is null && childView.Width > 0 && childView.Height > 0)
				{
					AlignPosition(details.HorizontalAlignment, details.VerticalAlignment,
						(float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
						ref posX, ref posY);
					Canvas.SetLeft(childView, posX);
					Canvas.SetTop(childView, posY);
				}
				else if (relativeView is not null && relativeView.Width > 0 && relativeView.Height > 0)
				{
					ChildLayoutAlignment(details, childView, relativeView, posX, posY);
				}
			}
		}

		void ChildLayoutAlignment(PositionDetails details, FrameworkElement childView, FrameworkElement relativeView, float posX, float posY)
		{
			AlignPositionToRelative(details.HorizontalAlignment, details.VerticalAlignment,
						(float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
						relativeView.ActualSize.X, relativeView.ActualSize.Y,
						ref posX, ref posY);

			GeneralTransform transform = relativeView.TransformToVisual(_rootView);
			PlatformPoint relativeViewOrigin = transform.TransformPoint(new PlatformPoint(0, 0));

			if (_rootView is null)
			{
				return;
			}

			double relativePosX = posX + relativeViewOrigin.X - details.X;
			double positionX = Math.Max(details.X, relativePosX > (_rootView.DesiredSize.Width - childView.DesiredSize.Width) ?
				(_rootView.DesiredSize.Width - childView.DesiredSize.Width) : relativePosX);

			double relativePosY = posY + relativeViewOrigin.Y - details.Y;
			double positionY = Math.Max(details.Y, relativePosY > (_rootView.DesiredSize.Height - childView.DesiredSize.Height) ?
				(_rootView.DesiredSize.Height - childView.DesiredSize.Height) : relativePosY);

			Canvas.SetLeft(childView, positionX);
			Canvas.SetTop(childView, positionY);
		}

		void ClearChildren()
		{
			if (_overlayStack is not null && _positionDetails.Count > 0)
			{
				foreach (FrameworkElement childView in _positionDetails.Keys)
				{
					childView.LayoutUpdated -= OnChildLayoutChanged;
					_overlayStack.Children.Remove(childView);
				}

				_positionDetails.Clear();
			}
		}

		#endregion
	}
}