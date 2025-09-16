using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.Internals;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformRect = Android.Graphics.Rect;
using PlatformView = Android.Views.View;

namespace Syncfusion.Maui.Toolkit.Internals
{
	internal partial class SfWindowOverlay
	{
		#region Fields

		ViewGroup? _rootView;
		PlatformRect? _decorViewFrame;
		float _density = 1f;
		PlatformView? _overlayContent;

		/// <summary>
		/// WindowManagerLayoutParams for Window overlay.
		/// </summary>
		WindowManagerLayoutParams? _windowManagerLayoutParams;

		/// <summary>
		/// Window manager of platform window.
		/// </summary>
		IWindowManager? _windowManager;

		#endregion

		#region Internal Fields

		/// <summary>
		/// List of window overlay stacks.
		/// </summary>
		internal static List<WindowOverlayStack>? _stackList;

		/// <summary>
		/// Platform view of overlay container.
		/// </summary>
		internal WindowOverlayStack? _overlayStack;

		#endregion

		#region Internal Methods

		/// <summary>
		/// Returns a <see cref="WindowOverlayStack"/>.
		/// </summary>
		/// <param name="context">Passes the information about the view group.</param>
		/// <returns></returns>
		internal virtual WindowOverlayStack CreateStack(Context context)
		{
			WindowOverlayStack? windowOverlayStack = null;
			if (_window is not null && _window.Handler is not null)
			{
				IMauiContext? mauiContext = _window.Handler.MauiContext;
				if (mauiContext is not null)
				{
					windowOverlayStack = (WindowOverlayStack?)_overlayStackView?.ToPlatform(mauiContext);
				}
			}

			return windowOverlayStack is not null ? windowOverlayStack : new WindowOverlayStack(context);
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

			IMauiContext? context = child.Handler?.MauiContext ?? _window?.Handler?.MauiContext;
			if (context is not null)
			{
				PlatformView childView = child.ToPlatform(context);
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

			if (child.Handler is not null && _window is not null && _window.Handler is not null)
			{
				IMauiContext? context = child.Handler.MauiContext ?? _window.Handler.MauiContext;
				if (context is not null && relative.Handler is not null && relative.Handler.MauiContext is not null)
				{
					PlatformView childView = child.ToPlatform(context);
					PlatformView relativeView = relative.ToPlatform(relative.Handler.MauiContext);
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
				PlatformView childView = view.ToPlatform(view.Handler.MauiContext);
				childView.LayoutChange -= OnChildLayoutChanged;
				childView.RemoveFromParent();
				_positionDetails.Remove(childView);
			}
		}

		/// <summary>
		/// Removes the overlay from the current view hierarchy and cleans up associated resources.
		/// </summary>
		internal void RemoveOverlay()
		{
			if (_overlayContent is not null && _overlayContent.Parent is not null)
			{
				_overlayContent.RemoveFromParent();
			}

			if (_overlayStack is not null)
			{
				_overlayStack.LayoutChange -= OnOverlayStackLayoutChange;

				//Checking whether OverlayStack Parent is null or not.
				if (_overlayStack.Parent is not null && _windowManager is not null)
				{
					_windowManager.RemoveView(_overlayStack);
				}

				if (_stackList is not null && _stackList.Contains(_overlayStack))
				{
					_stackList.Remove(_overlayStack);
				}
			}

			// Disposing the view list if there is no overlay stack in the collection.
			if (_stackList is not null && _stackList.Count is 0)
			{
				_stackList = null;
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
				_density = WindowOverlayHelper._density;
				_rootView = WindowOverlayHelper._platformRootView;
				if (_rootView is not null)
				{
					if (_window.Handler is not null && _window.Handler is WindowHandler windowHandler && windowHandler.MauiContext is not null
						&& windowHandler.PlatformView is Activity platformActivity && windowHandler.MauiContext.Context is not null)
					{
						_overlayStack = CreateStack(windowHandler.MauiContext.Context);
						_windowManager = platformActivity.WindowManager;
						if (_overlayStack is not null)
						{
							_overlayStack.LayoutChange += OnOverlayStackLayoutChange;
							if (_windowManagerLayoutParams is null)
							{
								GetWindowManagerLayoutParams();
							}

							if (childView.Handler is not null && childView.Handler.MauiContext is not null)
							{
								_overlayContent = childView.ToPlatform(childView.Handler.MauiContext);
							}
							else
							{
								_overlayContent = childView.ToPlatform(windowHandler.MauiContext);
							}

							return true;
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
			float posX = (float)x * _density;
			float posY = (float)y * _density;

			if (_overlayStack is not null)
			{
				if (_windowManager is not null)
				{
					if (_windowManagerLayoutParams is null)
					{
						GetWindowManagerLayoutParams();
					}
					else
					{
						var decorViewContent = WindowOverlayHelper._decorViewContent;
						if (decorViewContent is not null)
						{
							_windowManagerLayoutParams.Width = decorViewContent.Width;
							_windowManagerLayoutParams.Height = decorViewContent.Height;
						}
					}

					if (_overlayStack.Parent is null)
					{
						_windowManager.AddView(_overlayStack, _windowManagerLayoutParams);
					}
					else
					{
						_windowManager.UpdateViewLayout(_overlayStack, _windowManagerLayoutParams);
					}
				}

				if (_overlayContent is not null)
				{
					// Stacking the popup overlay added in the application into the static collection to handle the blur effect in multiple popup scenarios.
					if (_stackList is null)
					{
						_stackList = [_overlayStack];
					}
					else if (!_stackList.Contains(_overlayStack))
					{
						_stackList.Add(_overlayStack);
					}

					if (_overlayContent.Parent is null)
					{
						_overlayStack.AddView(_overlayContent,
							new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
					}

					_overlayContent.SetX(posX);
					_overlayContent.SetY(posY);
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
				_overlayStack.LayoutChange -= OnOverlayStackLayoutChange;
				if (_overlayStack.Parent is not null && _windowManager is not null)
				{
					_windowManager.RemoveView(_overlayStack);
				}

				_windowManagerLayoutParams = null;
				_overlayStack = null;
			}

			_decorViewFrame?.Dispose();
			_decorViewFrame = null;
			_hasOverlayStackInRoot = false;
		}

		/// <summary>
		/// Dispose the objects in window overlay.
		/// </summary>
		internal void Dispose()
		{
			RemoveOverlay();
			_window = null;
			_overlayStack = null;
			_overlayContent = null;
			_windowManagerLayoutParams = null;
			_windowManager = null;
		}

		/// <summary>
		/// Helps to get the WindowManagerLayoutParams for Window overlay.
		/// </summary>
		/// <returns>Returns the WindowManagerLayoutParams.</returns>
		internal WindowManagerLayoutParams GetWindowManagerLayoutParams()
		{
			if (_windowManagerLayoutParams is null)
			{
				_windowManagerLayoutParams = new WindowManagerLayoutParams();
				var decorViewContent = WindowOverlayHelper._decorViewContent;
				if (decorViewContent is not null)
				{
					_windowManagerLayoutParams.Width = decorViewContent.Width;
					_windowManagerLayoutParams.Height = decorViewContent.Height;
				}
				_windowManagerLayoutParams.Format = Format.Translucent;
			}

			return _windowManagerLayoutParams;
		}

		/// <summary>
		/// Helps to get the WindowManagerLayoutParams for Window overlay.
		/// </summary>
		/// <returns>Returns the WindowManagerLayoutParams.</returns>
		internal void UpdateWindowManagerLayoutParamsSize()
		{
			var decorViewContent = WindowOverlayHelper._decorViewContent;
			if (_windowManagerLayoutParams is not null && decorViewContent is not null)
			{
				_windowManagerLayoutParams.Width = decorViewContent.Width;
				_windowManagerLayoutParams.Height = decorViewContent.Height;
				_windowManager?.UpdateViewLayout(_overlayStack, _windowManagerLayoutParams);
			}
		}

		#endregion

		#region Private Methods

		void UpdateChildPositionAndAlignment(
			PlatformView childView,
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

			float posX = (float)x * _density;
			float posY = (float)y * _density;
			details.X = posX;
			details.Y = posY;
			details.HorizontalAlignment = horizontalAlignment;
			details.VerticalAlignment = verticalAlignment;

			if (childView.Parent is null && _overlayStack is not null)
			{
				_overlayStack.AddView(childView,
					new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
				childView.LayoutChange += OnChildLayoutChanged;
			}

			if (childView.Width > 0 && childView.Height > 0)
			{
				AlignPosition(horizontalAlignment, verticalAlignment,
					childView.Width, childView.Height,
					ref posX, ref posY);
				childView.SetX(posX);
				childView.SetY(posY);
			}
		}

		void UpdateChildPositionAndAlignment(
			PlatformView childView,
			PlatformView relativeView,
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

			float posX = (float)x * _density;
			float posY = (float)y * _density;
			details.X = posX;
			details.Y = posY;
			details.HorizontalAlignment = horizontalAlignment;
			details.VerticalAlignment = verticalAlignment;
			details.Relative = relativeView;

			if (childView.Parent is null && _overlayStack is not null)
			{
				_overlayStack.AddView(childView,
					new ViewGroup.LayoutParams(
						ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
				childView.LayoutChange += OnChildLayoutChanged;
			}

			ChildAlignment(childView, relativeView, horizontalAlignment, verticalAlignment, posX, posY, x, y);
		}

		void ChildAlignment(
			PlatformView childView,
			PlatformView relativeView,
			WindowOverlayHorizontalAlignment horizontalAlignment,
			WindowOverlayVerticalAlignment verticalAlignment,
			float posX,
			float posY,
			double x = 0,
			double y = 0)
		{
			if (childView.Width > 0 && childView.Height > 0)
			{
				AlignPositionToRelative(horizontalAlignment, verticalAlignment,
					childView.Width, childView.Height,
					relativeView.Width, relativeView.Height,
					ref posX, ref posY);

				int[] location = new int[2];
				relativeView.GetLocationOnScreen(location);

				float relativePosX = posX + location[0] - (_decorViewFrame?.Left ?? 0f);
				float positionX = Math.Max(0, relativePosX > (float)((_decorViewFrame?.Right ?? 0f) - childView.Width) ?
					(float)((_decorViewFrame?.Right ?? 0f) - childView.Width - (_decorViewFrame?.Left ?? 0f)) : relativePosX);

				float relativePosY = posY + location[1] - (_decorViewFrame?.Top ?? 0f);
				float positionY = Math.Max(0, relativePosY > (float)((_decorViewFrame?.Bottom ?? 0f) - childView.Height) ?
					(float)((_decorViewFrame?.Bottom ?? 0f) - childView.Height - (_decorViewFrame?.Top ?? 0f)) : relativePosY);

				// TODO: Need to consider left decor view frame?
				childView?.SetX(positionX);
				childView?.SetY(positionY);
			}
		}

		void Initialize()
		{
			if (_hasOverlayStackInRoot || _window is null || _window.Content is null)
			{
				return;
			}

			_density = WindowOverlayHelper._density;
			_rootView = WindowOverlayHelper._platformRootView;

			if (_rootView is null || _window.Handler is not WindowHandler windowHandler || windowHandler.MauiContext is null || windowHandler.PlatformView is not Activity platformActivity)
			{
				return;
			}

			_decorViewFrame = WindowOverlayHelper._decorViewFrame;

			if (_overlayStack is null && windowHandler.MauiContext.Context is not null)
			{
				_overlayStack = CreateStack(windowHandler.MauiContext.Context);
				if (_windowManagerLayoutParams is null)
				{
					GetWindowManagerLayoutParams();
				}

				var platformWindow = WindowOverlayHelper.GetPlatformWindow();
				if (platformWindow is not null && platformWindow.WindowManager is not null)
				{
					_windowManager = platformWindow.WindowManager;
					_windowManager.AddView(_overlayStack, _windowManagerLayoutParams);
				}

				_overlayStack.LayoutChange += OnOverlayStackLayoutChange;
				_overlayStack.BringToFront();
			}

			_hasOverlayStackInRoot = true;
		}

		void OnOverlayStackLayoutChange(object? sender, PlatformView.LayoutChangeEventArgs e)
		{
			_decorViewFrame = WindowOverlayHelper._decorViewFrame;
		}

		void OnChildLayoutChanged(object? sender, PlatformView.LayoutChangeEventArgs e)
		{
			if (sender is null)
			{
				return;
			}

			PlatformView childView = (PlatformView)sender;
			if (_positionDetails.TryGetValue(childView, out PositionDetails? details) && details is not null)
			{
				float posX = details.X;
				float posY = details.Y;
				PlatformView? relativeView = details.Relative;

				if (relativeView is null && childView.Width > 0 && childView.Height > 0)
				{
					AlignPosition(details.HorizontalAlignment, details.VerticalAlignment,
						childView.Width, childView.Height,
						ref posX, ref posY);
					childView.SetX(posX);
					childView.SetY(posY);
				}
				else if (relativeView is not null && relativeView.Width > 0
					&& relativeView.Height > 0 && childView.Width > 0 && childView.Height > 0)
				{
					ChildLayoutAlignment(details, childView, relativeView, posX, posY);
				}
			}
		}

		void ChildLayoutAlignment(PositionDetails details, PlatformView childView, PlatformView relativeView, float posX, float posY)
		{
			AlignPositionToRelative(details.HorizontalAlignment, details.VerticalAlignment,
			childView.Width, childView.Height,
						relativeView.Width, relativeView.Height,
						ref posX, ref posY);

			int[] location = new int[2];
			relativeView.GetLocationOnScreen(location);

			float relativePosX = posX + location[0] - (_decorViewFrame?.Left ?? 0f);
			float positionX = Math.Max(0, relativePosX > (float)((_decorViewFrame?.Right ?? 0f) - childView.Width) ?
				(float)((_decorViewFrame?.Right ?? 0f) - childView.Width - (_decorViewFrame?.Left ?? 0f)) : relativePosX);

			float relativePosY = posY + location[1] - (_decorViewFrame?.Top ?? 0f);
			float positionY = Math.Max(0, relativePosY > (float)((_decorViewFrame?.Bottom ?? 0f) - childView.Height) ?
				(float)((_decorViewFrame?.Bottom ?? 0f) - childView.Height - (_decorViewFrame?.Top ?? 0f)) : relativePosY);

			// TODO: Need to consider left decor view frame?
			childView?.SetX(positionX);
			childView?.SetY(positionY);
		}

		void ClearChildren()
		{
			if (_overlayStack is not null && _positionDetails.Count > 0)
			{
				foreach (PlatformView view in _positionDetails.Keys)
				{
					view.LayoutChange -= OnChildLayoutChanged;
					view.RemoveFromParent();
				}

				_positionDetails.Clear();
			}
		}

		#endregion
	}
}
