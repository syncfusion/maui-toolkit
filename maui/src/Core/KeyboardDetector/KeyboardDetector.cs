namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Detects keyboard events and handles related functionality.
	/// </summary>
	public partial class KeyboardDetector : IDisposable
	{
		private List<IKeyboardListener> keyboardListeners;
		internal readonly View MauiView;
		private bool _disposed;
		private bool isViewListenerAdded;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyboardDetector"/> class with the specified MAUI view.
		/// </summary>
		/// <param name="mauiView">The MAUI view to associate with the keyboard detector.</param>
		public KeyboardDetector(View mauiView)
		{
			MauiView = mauiView;
			keyboardListeners = [];
			if (mauiView.Handler != null)
			{
				SubscribeNativeKeyEvents(mauiView);
			}
			else
			{
				mauiView.HandlerChanged += MauiView_HandlerChanged;
				mauiView.HandlerChanging += MauiView_HandlerChanging;
			}
		}

		private void MauiView_HandlerChanged(object? sender, EventArgs e)
		{
			if (sender is View view && view.Handler != null)
			{
				SubscribeNativeKeyEvents(view);
			}
		}

		private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
		{
			UnsubscribeNativeKeyEvents(e.OldHandler);
		}

		/// <summary>
		/// Releases all resources used by the <see cref="KeyboardDetector"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="KeyboardDetector"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">A boolean value indicating whether to release both managed and unmanaged resources (true) or only unmanaged resources (false).</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;

			if (disposing)
			{
				isViewListenerAdded = false;
				ClearListeners();
				Unsubscribe(MauiView);
			}
		}

		/// <summary>
		/// Adds a keyboard listener to the <see cref="KeyboardDetector"/>.
		/// </summary>
		/// <param name="listener">The keyboard listener to add.</param>
		public void AddListener(IKeyboardListener listener)
		{
			keyboardListeners ??= [];

			if (!keyboardListeners.Contains(listener))
			{
				keyboardListeners.Add(listener);
			}

			// If dynamically call AddKeyboardListener mehtod or call after the MauiView's handler set, native listeners will be created in the below code. 
			if (!isViewListenerAdded)
			{
				CreateNativeListener();
			}
			isViewListenerAdded = true;
		}


		/// <summary>
		/// Removes all keyboard listeners from the <see cref="KeyboardDetector"/>.
		/// </summary>
		public void ClearListeners()
		{
			keyboardListeners!.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="KeyboardDetector"/> has any keyboard listeners.
		/// </summary>
		/// <returns><c>true</c> if there are listeners; otherwise, <c>false</c>.</returns>
		public bool HasListener()
		{
			return keyboardListeners?.Count > 0;
		}

		/// <summary>
		/// Removes a keyboard listener from the <see cref="KeyboardDetector"/>.
		/// </summary>
		/// <param name="listener">The keyboard listener to remove.</param>
		public void RemoveListener(IKeyboardListener listener)
		{
			if (listener is IKeyboardListener keyListener && keyboardListeners != null && keyboardListeners.Contains(keyListener))
			{
				keyboardListeners.Remove(keyListener);
			}
		}

		internal void OnKeyAction(KeyEventArgs args)
		{
			if (keyboardListeners.Count == 0 || args.Key == KeyboardKey.None)
			{
				return;
			}

			if (args.KeyAction == KeyActions.PreviewKeyDown)
			{
				foreach (IKeyboardListener listener in keyboardListeners)
				{
					listener.OnPreviewKeyDown(args);
				}
			}
			else if (args.KeyAction == KeyActions.KeyDown)
			{
				foreach (IKeyboardListener listener in keyboardListeners)
				{
					listener.OnKeyDown(args);
				}
			}
			else
			{
				foreach (IKeyboardListener listener in keyboardListeners)
				{
					listener.OnKeyUp(args);
				}
			}
		}

		/// <summary>
		/// Unsubscribes from events or notifications related to the specified MAUI view.
		/// </summary>
		/// <param name="mauiView">The MAUI view to unsubscribe from, if any.</param>
		private void Unsubscribe(View? mauiView)
		{
			if (mauiView != null)
			{
				UnsubscribeNativeKeyEvents(mauiView.Handler!);
				mauiView.HandlerChanged -= MauiView_HandlerChanged;
				mauiView.HandlerChanging -= MauiView_HandlerChanging;
				mauiView = null;
			}
		}
	}
}
