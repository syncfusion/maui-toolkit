using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
	/// <summary>
	/// Represents the <see cref="SfNavigationDrawer"/> control that contains multiple items that share the same space on the screen.
	/// </summary>
	public partial class SfNavigationDrawer
	{
		#region Fields

		/// <summary>
		/// Represents the native view of the navigation drawer.
		/// </summary>
		FrameworkElement? _navigationDrawerNativeView;

		/// <summary>
		/// It is used to handle the pointer's pressed state in the Manipulation events.
		/// </summary>
		/// <value><c>true</c> if the pointer is in pressed state; otherwise, <c>false</c>.</value>
		bool _isPressedState;

		/// <summary>
		/// Represents the initial point for touch interaction.
		/// </summary>
		Point _initialPoint = new(0, 0);

		#endregion

		#region Private Methods

		/// <summary>
		/// This method triggers on any touch interaction on <see cref="SfNavigationDrawer"/>.
		/// </summary>
		/// <param name="e">Event args.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
		{
		}

		/// <summary>
		/// Configures the touch interaction for the <see cref="SfNavigationDrawer"/>.
		/// </summary>
		void ConfigureTouch()
		{
			if (Handler != null && Handler.PlatformView != null)
			{
				WireEvents();
			}
			else
			{
				UnWireEvents();
			}
		}

		/// <summary>
		/// Wires the events for touch interaction.
		/// </summary>
		void WireEvents()
		{
			if (Handler != null && Handler.PlatformView != null && Handler.PlatformView is FrameworkElement)
			{
				_navigationDrawerNativeView = Handler.PlatformView as FrameworkElement;
				if (_navigationDrawerNativeView != null)
				{
					_navigationDrawerNativeView.ManipulationMode = ManipulationModes.All;
					_navigationDrawerNativeView.ManipulationDelta += NavigationDrawerNativeView_ManipulationDelta;
					_navigationDrawerNativeView.ManipulationCompleted += NavigationDrawerNativeView_ManipulationCompleted;
					_navigationDrawerNativeView.PointerCanceled += NavigationDrawerNativeView_PointerCanceled;
					_navigationDrawerNativeView.PointerReleased += NavigationDrawerNativeView_PointerReleased;
					_navigationDrawerNativeView.PointerPressed += NavigationDrawerNativeView_PointerPressed;
				}
			}
		}

		/// <summary>
		/// Unwire the events for touch interaction.
		/// </summary>
		void UnWireEvents()
		{
			if (Handler != null && _navigationDrawerNativeView != null)
			{
				_navigationDrawerNativeView = Handler.PlatformView as FrameworkElement;
				if (_navigationDrawerNativeView != null)
				{
					_navigationDrawerNativeView.ManipulationMode = ManipulationModes.All;
					_navigationDrawerNativeView.ManipulationDelta -= NavigationDrawerNativeView_ManipulationDelta;
					_navigationDrawerNativeView.ManipulationCompleted -= NavigationDrawerNativeView_ManipulationCompleted;
					_navigationDrawerNativeView.PointerCanceled -= NavigationDrawerNativeView_PointerCanceled;
					_navigationDrawerNativeView.PointerReleased -= NavigationDrawerNativeView_PointerReleased;
					_navigationDrawerNativeView.PointerPressed -= NavigationDrawerNativeView_PointerPressed;
				}
			}
		}

		/// <summary>
		/// Handles the pointer pressed event on the navigation drawer.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The pointer event arguments.</param>
		void NavigationDrawerNativeView_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(_navigationDrawerNativeView).Position;
            bool handled = false;

            // Prioritize primary drawer if it has priority or is already open/swiping
            if ((this.IsPrimaryDrawerPrioritized() && !_isSecondaryDrawerOpen) || _isDrawerOpen || _isPrimaryDrawerSwiping)
            {
                this.OnHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                _isPrimaryDrawerSwiping = true;
                handled = true;
            }
            // Handle secondary drawer if it's open/swiping and primary isn't prioritized
            else if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
            {
                this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                handled = true;
            }
            // Edge activation for secondary drawer (only if primary isn't prioritized or active)
            else if (this.SecondaryDrawerSettings != null && !_isPrimaryDrawerSwiping)
            {
                switch (this.SecondaryDrawerSettings.Position)
                {
                    case Position.Left:
                        if (point.X <= this.SecondaryDrawerSettings.TouchThreshold && point.X >= 0)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                            _isSecondaryDrawerSwiping = true;
                            handled = true;
                        }
                        break;
                    case Position.Right:
                        if (point.X >= (this.ScreenWidth - this.SecondaryDrawerSettings.TouchThreshold) && point.X <= this.ScreenWidth)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                            _isSecondaryDrawerSwiping = true;
                            handled = true;
                        }
                        break;
                    case Position.Top:
                        if (point.Y <= this.SecondaryDrawerSettings.TouchThreshold && point.Y >= 0)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                            _isSecondaryDrawerSwiping = true;
                            handled = true;
                        }
                        break;
                    case Position.Bottom:
                        if (point.Y >= (this.ScreenHeight - this.SecondaryDrawerSettings.TouchThreshold) && point.Y <= this.ScreenHeight)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                            _isSecondaryDrawerSwiping = true;
                            handled = true;
                        }
                        break;
                }
            }

            // Fallback to primary drawer if not handled by secondary and primary itself is not currently swiping
            if (!handled && !_isPrimaryDrawerSwiping && !_isSecondaryDrawerSwiping)
            {
                this.OnHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                _isPrimaryDrawerSwiping = true;
            }
			if (!_isDrawerOpen)
			{
				_isPressedState = true;
			}
			_navigationDrawerNativeView?.ReleasePointerCapture(e.Pointer);
		}

		/// <summary>
		/// Handles the pointer released event on the navigation drawer.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The pointer event arguments.</param>
		void NavigationDrawerNativeView_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(_navigationDrawerNativeView).Position;
			if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
            {
                this.OnSecondaryHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                _isSecondaryDrawerSwiping = false;
            }
            else
            {
                this.OnHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
                _isPrimaryDrawerSwiping = false;
            }
			_isPressedState = false;
		}

		/// <summary>
		/// Handles the pointer canceled event on the navigation drawer.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The pointer event arguments.</param>
		void NavigationDrawerNativeView_PointerCanceled(object sender, PointerRoutedEventArgs e)
		{
			if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
            {
                this.OnSecondaryHandleTouchInteraction(PointerActions.Cancelled, new Microsoft.Maui.Graphics.Point(0, 0));
                _isSecondaryDrawerSwiping = false;
            }
            else
            {
                this.OnHandleTouchInteraction(PointerActions.Cancelled, new Microsoft.Maui.Graphics.Point(0, 0));
                _isPrimaryDrawerSwiping = false;
            }
			_isPressedState = false;
		}

		/// <summary>
		/// Handles the manipulation completed event on the navigation drawer.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The manipulation event arguments.</param>
		void NavigationDrawerNativeView_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			var point = e.Position;
            if (_isPressed)
            {
                if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
                {
                    switch (this.SecondaryDrawerSettings.Position)
                    {
                        case Position.Left:
                            if (point.X <= this.SecondaryDrawerSettings.DrawerWidth || _isTransitionDifference)
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                        case Position.Right:
                            if (point.X >= this.ScreenWidth - this.SecondaryDrawerSettings.DrawerWidth)
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                        case Position.Top:
                            if (point.Y <= this.SecondaryDrawerSettings.DrawerHeight)
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                        case Position.Bottom:
                            if (point.Y >= this.ScreenHeight - this.SecondaryDrawerSettings.DrawerHeight)
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                    }
                    _isSecondaryDrawerSwiping = false;
                }
                else
                {
                    switch (this.DrawerSettings.Position)
                    {
                        case Position.Left:
                            if (point.X <= this.DrawerSettings.DrawerWidth || _isTransitionDifference)
                                this.OnHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                        case Position.Right:
                            if (point.X >= this.ScreenWidth - this.DrawerSettings.DrawerWidth)
                                this.OnHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                        case Position.Top:
                            if (point.Y <= this.DrawerSettings.DrawerHeight)
                                this.OnHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                        case Position.Bottom:
                            if (point.Y >= this.ScreenHeight - this.DrawerSettings.DrawerHeight)
                                this.OnHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
                            break;
                    }
                    _isPrimaryDrawerSwiping = false;
                }
            }
			_initialPoint = new Point(0, 0);
		}

		/// <summary>
		/// Handles the manipulation delta event on the navigation drawer.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The manipulation event arguments.</param>
		void NavigationDrawerNativeView_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			_initialPoint = new Point(e.Position.X, e.Position.Y);

			if (!_isPressed && _initialPoint.X != 0 && _initialPoint.Y != 0)
			{
				bool isgestureHandled = false;

                // Prioritize primary drawer gesture handling
                if (!_isPrimaryDrawerSwiping && this.IsPrimaryDrawerPrioritized() && !_isSecondaryDrawerOpen)
                {
                    switch (this.DrawerSettings.Position)
                    {
                        case Position.Left:
                            if (_drawerLayout != null && _initialPoint.X <= this.DrawerSettings.TouchThreshold && _initialPoint.X >= 0 && !_isPressedState)
                            {
                                this.OnHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isPrimaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                        case Position.Right:
                            if (_initialPoint.X >= _touchRightThreshold && _initialPoint.X >= 0 && !_isPressedState)
                            {
                                this.OnHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isPrimaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                        case Position.Top:
                            if (_initialPoint.Y <= this.DrawerSettings.TouchThreshold)
                            {
                                this.OnHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isPrimaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                        case Position.Bottom:
                            if (_initialPoint.Y >= _touchBottomThreshold)
                            {
                                this.OnHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isPrimaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                    }
                }

                // If primary didn't handle it, try secondary drawer
                if (!isgestureHandled && !_isSecondaryDrawerSwiping && !_isPrimaryDrawerSwiping)
                {
                    switch (this.SecondaryDrawerSettings.Position)
                    {
                        case Position.Left:
                            if (this.SecondaryDrawerSettings != null && _initialPoint.X <= this.SecondaryDrawerSettings.TouchThreshold && _initialPoint.X >= 0 && !_isPressedState)
                            {
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isSecondaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                        case Position.Right:
                            if (_initialPoint.X >= (this.ScreenWidth - this.SecondaryDrawerSettings.TouchThreshold) && _initialPoint.X >= 0 && !_isPressedState)
                            {
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isSecondaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                        case Position.Top:
                            if (_initialPoint.Y <= this.SecondaryDrawerSettings.TouchThreshold && !_isPressedState)
                            {
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isSecondaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                        case Position.Bottom:
                            if (_initialPoint.Y >= (this.ScreenHeight - this.SecondaryDrawerSettings.TouchThreshold) && !_isPressedState)
                            {
                                this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                                _isSecondaryDrawerSwiping = true;
                                isgestureHandled = true;
                            }
                            break;
                    }
                }
            
			}

			if (_initialPoint.X >= 0 && _initialPoint.Y >= 0 && _initialPoint.X <= ScreenWidth && _initialPoint.Y <= ScreenHeight)
			{
				if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
                {
                    this.OnSecondaryHandleTouchInteraction(PointerActions.Moved, new Point(e.Position.X, e.Position.Y));
                }
                else if (_isDrawerOpen || _isPrimaryDrawerSwiping)
                {
                    this.OnHandleTouchInteraction(PointerActions.Moved, new Point(e.Position.X, e.Position.Y));
                }
			}
		}
		
		bool IsPrimaryDrawerPrioritized()
        {
            if (this.SecondaryDrawerSettings != null && this.DrawerSettings.Position == this.SecondaryDrawerSettings.Position &&
                (this.DrawerSettings.DrawerHeaderView !=null || this.DrawerSettings.DrawerContentView !=null || this.DrawerSettings.DrawerFooterView!=null))
            {
                return true;
            }
            return false;
        }
		#endregion

		#region Override Methods

		/// <summary>
		/// Raises on handler changing.
		/// </summary>
		/// <param name="args">Relevant <see cref="HandlerChangingEventArgs"/>.</param>
		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
		{
			if (args.OldHandler != null)
			{
				UnWireEvents();
			}

			base.OnHandlerChanging(args);
		}

		#endregion
	}
}