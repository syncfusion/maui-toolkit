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
        Point _initialPoint = new Point(0, 0);

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
            OnHandleTouchInteraction(PointerActions.Pressed, new Microsoft.Maui.Graphics.Point(point.X, point.Y));

            if (!_isDrawerOpen)
            {
                _isPressedState = true;
            }
        }

        /// <summary>
        /// Handles the pointer released event on the navigation drawer.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The pointer event arguments.</param>
        void NavigationDrawerNativeView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(_navigationDrawerNativeView).Position;
            OnHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(point.X, point.Y));
            _isPressedState = false;
        }

        /// <summary>
        /// Handles the pointer canceled event on the navigation drawer.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The pointer event arguments.</param>
        void NavigationDrawerNativeView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            OnHandleTouchInteraction(PointerActions.Cancelled, new Microsoft.Maui.Graphics.Point(0, 0));
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
                bool shouldHandleTouch = DrawerSettings.Position switch
                {
                    Position.Left => point.X <= DrawerSettings.DrawerWidth || _isTransitionDifference,
                    Position.Right => point.X >= ScreenWidth - DrawerSettings.DrawerWidth,
                    Position.Top => point.Y <= DrawerSettings.DrawerHeight,
                    Position.Bottom => point.Y >= ScreenHeight - DrawerSettings.DrawerHeight,
                    _ => false
                };

                if (shouldHandleTouch)
                {
                    OnHandleTouchInteraction(PointerActions.Released, new Microsoft.Maui.Graphics.Point(e.Position.X, e.Position.Y));
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
                bool shouldHandleTouch = DrawerSettings.Position switch
                {
                    Position.Left => _drawerLayout != null && _initialPoint.X <= DrawerSettings.TouchThreshold && _initialPoint.X >= 0 && !_isPressedState,
                    Position.Right => _initialPoint.X >= _touchRightThreshold && _initialPoint.X >= 0 && !_isPressedState,
                    Position.Top => _initialPoint.Y <= DrawerSettings.TouchThreshold,
                    Position.Bottom => _initialPoint.Y >= _touchBottomThreshold,
                    _ => false
                };

                if (shouldHandleTouch)
                {
                    OnHandleTouchInteraction(PointerActions.Pressed, new Point(e.Position.X, e.Position.Y));
                }
            }

            if (_initialPoint.X >= 0 && _initialPoint.Y >= 0 && _initialPoint.X <= ScreenWidth && _initialPoint.Y <= ScreenHeight)
            {
                OnHandleTouchInteraction(PointerActions.Moved, new Point(e.Position.X, e.Position.Y));
            }
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