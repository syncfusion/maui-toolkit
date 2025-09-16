using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using MauiView = Microsoft.Maui.Controls.View;

namespace Syncfusion.Maui.Toolkit.Internals
{
	internal partial class SfWindowOverlay
	{
		#region Fields

		UIView? _rootView;
		WindowOverlayStack? _overlayStack;
		UIView? _overlayContent;
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

					if (windowOverlayStack is not null && _overlayStackView is not null)
					{
						windowOverlayStack._canHandleTouch = !_overlayStackView.canHandleTouch;
					}
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

			if (_window is not null && _window.Handler is not null)
			{
				IMauiContext? context = _window.Handler.MauiContext;
				if (context is not null)
				{
					UIView childView = child.ToPlatform(context);
					UpdateChildPositionAndAlignment(childView, x, y, horizontalAlignment, verticalAlignment);
				}
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

			if (relative.Frame.Width < 0 || relative.Frame.Height < 0)
			{
				// TODO: Handle relative view layout here, if needed.
				return;
			}

			if (_window is not null && _window.Handler is not null)
			{
				IMauiContext? context = _window.Handler.MauiContext;
				if (context is not null && relative.Handler is not null && relative.Handler.MauiContext is not null)
				{
					UIView childView = child.ToPlatform(context);
					UIView relativeView = relative.ToPlatform(relative.Handler.MauiContext);
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
				UIView childView = view.ToPlatform(view.Handler.MauiContext);
				childView.RemoveFromSuperview();
				_positionDetails.Remove(childView);
			}
		}

		/// <summary>
		/// Removes the overlay from the current view hierarchy and cleans up associated resources.
		/// </summary>
		internal void RemoveOverlay()
		{
			if (_overlayStack is not null)
			{
				_overlayStack.ClearSubviews();
				_overlayStack.RemoveFromSuperview();
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
				if (_rootView is not null)
				{
					UIWindow? keyWindow = null;
					try
					{
						if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
						{
							var connectedScenes = UIApplication.SharedApplication.ConnectedScenes;
							if (connectedScenes is not null)
							{
								keyWindow = connectedScenes
											.ToArray()
											.OfType<UIWindowScene>()
											.SelectMany(scene => scene.Windows)
											.FirstOrDefault(window => window.IsKeyWindow)!;
							}
						}
						else
						{
							keyWindow = UIApplication.SharedApplication.KeyWindow!;
						}
					}
					catch
					{
						return false;
					}

					if (keyWindow == null && _rootView is UIWindow)
					{
						keyWindow = _rootView as UIWindow;
					}

					if (keyWindow is not null)
					{
						_rootView = keyWindow;
						_overlayStack ??= CreateStack();
						if (_overlayStack is not null)
						{
							// Need to set the Parent Size for Child.
							_overlayStack.AutoresizingMask = UIViewAutoresizing.All;
							if (_window.Handler is not null && _window.Handler is WindowHandler windowHandler && windowHandler.MauiContext is not null)
							{
								_overlayContent = childView.ToPlatform(windowHandler.MauiContext);
								return true;
							}
						}
					}
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

			// Refresh KeyWindow since mopup can be added, which will have a new UI window.
			// We initialized rootView at the start, but mopup could have opened in the middle, requiring a refresh.
			UIWindow? keyWindow = null;
			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				var connectedScenes = UIApplication.SharedApplication.ConnectedScenes;
				if (connectedScenes is not null)
				{
					keyWindow = connectedScenes
								 .ToArray()
								 .OfType<UIWindowScene>()
								 .SelectMany(scene => scene.Windows)
								 .FirstOrDefault(window => window.IsKeyWindow)!;
				}
			}
			else
			{
				keyWindow = UIApplication.SharedApplication.KeyWindow!;
			}

			if (_rootView != keyWindow)
			{
				_rootView = keyWindow;
			}

			if (_overlayStack is not null)
			{
				if (_rootView is not null && !_overlayStack.IsDescendantOfView(_rootView))
				{
					if (_rootView != _overlayStack.Superview)
					{
						_overlayStack.RemoveFromSuperview();
						_rootView.AddSubview(_overlayStack);
					}

					_rootView.BringSubviewToFront(_overlayStack);
					_overlayStack.Frame = _rootView.Frame;
				}

				if (_overlayContent is not null)
				{
					if (!_overlayContent.IsDescendantOfView(_overlayStack))
					{
						_overlayStack.AddSubview(_overlayContent);
					}

					_overlayContent.Frame = _overlayStack.Frame;
					_overlayContent.SizeToFit();

					if (!_overlayContent.Frame.IsEmpty)
					{
						AlignPosition(WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment.Top,
							(float)_overlayContent.Frame.Width, (float)_overlayContent.Frame.Height,
							ref posX, ref posY);
						_overlayContent.Frame = new CGRect(posX, posY, _overlayContent.Frame.Width, _overlayContent.Frame.Height);
					}
				}
			}
		}

		/// <summary>
		/// Dispose the objects in window overlay.
		/// </summary>
		internal void Dispose()
		{
			RemoveOverlay();
			_window = null;
			_overlayContent = null;
			_hasOverlayStackInRoot = false;
			_overlayStack = null;
		}

		/// <summary>
		/// Removes the current overlay window from root view with all its children.
		/// </summary>
		internal void RemoveFromWindow()
		{
			if (_overlayStack is not null)
			{
				ClearChildren();
				_overlayStack.RemoveFromSuperview();
				_overlayStack = null;
			}

			_hasOverlayStackInRoot = false;
		}

		#endregion

		#region Private Methods

		void UpdateChildPositionAndAlignment(UIView childView,
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

			if (_overlayStack is not null && !childView.IsDescendantOfView(_overlayStack))
			{
				_overlayStack.AddSubview(childView);
				childView.Frame = _overlayStack.Frame;
			}

			// When AutoSizeMode is set, view size is not updated properly. Need to measure the view when AutoSizeMode is set.
			childView.SizeToFit();

			if (!childView.Frame.IsEmpty)
			{
				AlignPosition(horizontalAlignment, verticalAlignment,
					(float)childView.Frame.Width, (float)childView.Frame.Height,
					ref posX, ref posY);
				childView.Frame = new CGRect(posX, posY, childView.Frame.Width, childView.Frame.Height);
			}
		}

		void UpdateChildPositionAndAlignment(
			UIView childView,
			UIView relativeView,
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

			if (_overlayStack is not null && !childView.IsDescendantOfView(_overlayStack))
			{
				_overlayStack.AddSubview(childView);
				childView.Frame = _overlayStack.Frame;
				childView.SizeToFit();
			}

			ChildAlignment(childView, relativeView, horizontalAlignment, verticalAlignment, posX, posY, x, y);
		}

		void ChildAlignment(
			UIView childView,
			UIView relativeView,
			WindowOverlayHorizontalAlignment horizontalAlignment,
			WindowOverlayVerticalAlignment verticalAlignment,
			float posX,
			float posY,
			double x = 0,
			double y = 0)
		{
			// TODO: No need to handle child layout?
			AlignPositionToRelative(horizontalAlignment, verticalAlignment,
				(float)childView.Frame.Width, (float)childView.Frame.Height,
				(float)relativeView.Frame.Width, (float)relativeView.Frame.Height,
				ref posX, ref posY);

			CGPoint relativeViewOrigin = relativeView.ConvertPointToView(new CGPoint(0, 0), _rootView);

			if (_rootView is null)
			{
				return;
			}

			double relativePosX = posX + relativeViewOrigin.X;
			double positionX = Math.Max(0, relativePosX > (float)(_rootView.Frame.Width - childView.Frame.Width) ?
				(float)(_rootView.Frame.Width - childView.Frame.Width) : relativePosX);
			double relativePosY = posY + relativeViewOrigin.Y;
			double positionY = Math.Max(0, relativePosY > (float)(_rootView.Frame.Height - childView.Frame.Height) ?
				(float)(_rootView.Frame.Height - childView.Frame.Height) : relativePosY);

			childView.Frame = new CGRect(
				positionX,
				positionY,
				childView.Frame.Width,
				childView.Frame.Height);
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

			UIWindow? keyWindow = null;
			//To display the popup above the Mopup.
			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				var connectedScenes = UIApplication.SharedApplication.ConnectedScenes;
				if (connectedScenes is not null)
				{
					keyWindow = connectedScenes
								.ToArray()
								.OfType<UIWindowScene>()
								.SelectMany(scene => scene.Windows)
								.FirstOrDefault(window => window.IsKeyWindow)!;
				}
			}
			else
			{
				keyWindow = UIApplication.SharedApplication.KeyWindow!;
			}

			if (keyWindow is not null && _rootView != keyWindow)
			{
				_rootView = keyWindow;
			}

			if (!(_rootView.Subviews.Contains(_overlayStack)))
			{
				_rootView.AddSubview(_overlayStack);
				_rootView.BringSubviewToFront(_overlayStack);
				_overlayStack.Frame = _rootView.Frame;
				//Set the AutoresizingMask for overlayStack.
				_overlayStack.AutoresizingMask = UIViewAutoresizing.All;
			}

			_hasOverlayStackInRoot = true;
		}

		void ClearChildren()
		{
			if (_overlayStack is not null && _positionDetails.Count > 0)
			{
				_overlayStack.ClearSubviews();
				_positionDetails.Clear();
			}
		}

		#endregion
	}
}