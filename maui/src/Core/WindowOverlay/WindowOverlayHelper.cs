using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Handlers;

#if ANDROID
using Android.App;
using Android.Views;
using AndroidX.AppCompat.Widget;
using PlatformRect = Android.Graphics.Rect;
using PlatformRootView = Android.Views.ViewGroup;
using PlatformView = Android.Views.View;
using Window = Android.Views.Window;
#elif IOS || MACCATALYST
using PlatformRootView = UIKit.UIView;
#elif WINDOWS
using PlatformRootView = Microsoft.UI.Xaml.UIElement;
using PlatformWindow = Microsoft.UI.Xaml.Window;
using Microsoft.UI.Xaml;
#else
using PlatformRootView = System.Object;
#endif

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Helper class for <see cref="WindowOverlay"/>.
    /// </summary>
    internal static class WindowOverlayHelper
    {
		#region Fields

		/// <summary>
		/// Gets the application window.
		/// </summary>
		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		internal static IWindow? _window
		{
			get => GetActiveWindow();
		}
		/// <summary>
		/// Gets the device density.
		/// </summary>
		internal static float _density => _window is not null ? _window.RequestDisplayDensity() : 1f;

		/// <summary>
		/// Gets the root view of the device.
		/// </summary>
		internal static PlatformRootView? _platformRootView => GetPlatformRootView();

#if ANDROID

		/// <summary>
		/// Gets the decor view frame.
		/// </summary>
		internal static PlatformRect? _decorViewFrame => UpdateDecorFrame();

		internal static PlatformView? _decorViewContent => GetPlatformWindow()?.DecorView;

#elif WINDOWS
        internal static PlatformWindow? _platformWindow => GetPlatformWindow();
#endif

		#endregion

		#region Private Methods

		/// <summary>
		/// Helps to get the current active window.
		/// </summary>
		/// <returns> Current active window.</returns>
		[DynamicDependency("IsActivated", typeof(Microsoft.Maui.Controls.Window))]
		[RequiresUnreferencedCode("Uses reflection to access Window.IsActivated property")]
		static IWindow? GetActiveWindow()
        {
            var windowCollection = (IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application)?.Windows;
            if (windowCollection is not null)
            {
                foreach (var window in windowCollection)
                {
                    if (window is not null)
                    {
						if (window.Handler is not null && window.Handler.GetType().Name.Contains("EmbeddedWindowHandler", StringComparison.Ordinal))
                        {
                            return window;
                        }

                        var propertyInfo = window.GetType().GetProperty("IsActivated", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (propertyInfo is not null)
                        {
                            var isActivated = (bool)(propertyInfo?.GetValue(window))!;
                            if (isActivated)
                            {
                                return window;
                            }
                        }
                    }
                }
            }

#pragma warning disable CS0618 // Type or member is obsolete
			return IPlatformApplication.Current is { Application: Microsoft.Maui.Controls.Application { MainPage: { Window: var currentWindow } } } ? currentWindow : null;
#pragma warning restore CS0618 // Type or member is obsolete
		}

		/// <summary>
		/// Helps to get the root view of the device for each platform.
		/// </summary>
		/// <returns>Root view of the device.</returns>
		static PlatformRootView? GetPlatformRootView()
		{
			PlatformRootView? rootView = null;
			var application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
			if (_window is not null)
			{
#if ANDROID
				if (_window.Content is not null && _window.Content.Handler is not null)
				{
					rootView = _window.Content.ToPlatform() as ViewGroup;
				}

#pragma warning disable XAOBS001 // Type or member is obsolete
                while (rootView != null && rootView is not ContentFrameLayout)
				{
					if (rootView.Parent is not null)
					{
						rootView = rootView.Parent as ViewGroup;
					}
					else
					{
						if(rootView is ContentFrameLayout)
                        {
                            rootView = null;
                        }

                        break;
					}
				}
#pragma warning restore XAOBS001 // Type or member is obsolete
#elif IOS
				if (_window.Handler is not null)
				{
					rootView = _window.ToPlatform();
				}
#elif WINDOWS
                if (_window.Handler is not null && _window.Handler.PlatformView is Microsoft.UI.Xaml.Window platformWindow)
                {
					try
					{
                    	rootView = platformWindow.Content as UIElement;
						return rootView;
					}
					catch
					{
						return null;
					}
                }
#endif
			}

			return rootView;
		}

#if ANDROID

		/// <summary>
		/// Gets the activity window for Android.
		/// </summary>
		/// <returns>Returns the window of the platform view.</returns>
		internal static Window? GetPlatformWindow()
		{
			if (_window is not null && _window.Handler is WindowHandler windowHandler && windowHandler.PlatformView is Activity platformActivity)
			{
				if (platformActivity is null || platformActivity.WindowManager is null
					|| platformActivity.WindowManager.DefaultDisplay is null)
				{
					return null;
				}

				return platformActivity.Window;
			}

			return null;
		}

		/// <summary>
		/// Helps to get the decor view frame for Android.
		/// </summary>
		/// <returns>Returns the decor view frame.</returns>
		static PlatformRect? UpdateDecorFrame()
		{
			PlatformRect? decorViewFrame = null;
			var platformWindow = GetPlatformWindow();

			if (platformWindow is not null)
			{
				if (decorViewFrame is null)
				{
					decorViewFrame = new PlatformRect();
				}
				else if (decorViewFrame.Handle != IntPtr.Zero)
				{
					decorViewFrame.SetEmpty();
				}

				platformWindow.DecorView.GetWindowVisibleDisplayFrame(decorViewFrame);
			}

			return decorViewFrame;
		}

#elif WINDOWS
        static PlatformWindow? GetPlatformWindow()
        {
            return _window?.Handler is not null ? _window.Handler.PlatformView as PlatformWindow : null;
        }
#endif

		#endregion
	}
}