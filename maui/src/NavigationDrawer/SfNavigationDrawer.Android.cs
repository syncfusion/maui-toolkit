using Android.Views;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
    public partial class SfNavigationDrawer
    {
        #region Fields

        /// <summary>
        /// Represents the initial point for touch events.
        /// </summary>
        Point _initialPoint = new Point(0, 0);

        /// <summary>
        /// Represents the X coordinate of the initial touch down position.
        /// </summary>
        double _downX;

        /// <summary>
        /// Represents the Y coordinate of the initial touch down position.
        /// </summary>
        double _downY;

        /// <summary>
        /// Represents the X coordinate of the current touch move position.
        /// </summary>
        double _moveX;

        /// <summary>
        /// Represents the Y coordinate of the current touch move position.
        /// </summary>
        double _moveY;

        /// <summary>
        /// Represents the X coordinate of the initial touch down position for touch events.
        /// </summary>
        double _touchDownX;

        /// <summary>
        /// Represents the Y coordinate of the initial touch down position for touch events.
        /// </summary>
        double _touchDownY;

        /// <summary>
        /// Represents the X coordinate of the current touch move position for touch events.
        /// </summary>
        double _touchMoveX;

        /// <summary>
        /// Represents the Y coordinate of the current touch move position for touch events.
        /// </summary>
        double _touchMoveY;

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// Determines whether to intercept touch events for the navigation drawer.
        /// </summary>
        /// <param name="ev">The motion event.</param>
        /// <returns><c>true</c> if the touch event should be intercepted; otherwise, <c>false</c>.</returns>
        internal override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (ev != null)
            {
                int actionIndex = ev.ActionIndex;
                int pointerId = ev.GetPointerId(actionIndex);
                Point screenPoint = new Point(ev.GetX(actionIndex), ev.GetY(actionIndex));
                Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                Point currentTouchPoint = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));

                switch (ev.Action)
                {
                    case MotionEventActions.Down:
                        return HandleActionDown(currentTouchPoint);

                    case MotionEventActions.Up:
                        _initialPoint = new Point(0, 0);
                        break;

                    case MotionEventActions.Move:
                        return HandleActionMove(ev, currentTouchPoint);
                }
            }

            return base.OnInterceptTouchEvent(ev);
        }

        #endregion

        #region Private Methods

        private bool HandleActionDown(Point currentTouchPoint)
        {
            _downX = currentTouchPoint.X;
            _downY = currentTouchPoint.Y;
            _initialPoint = currentTouchPoint;

            if (DrawerSettings.Position == Position.Left || DrawerSettings.Position == Position.Right)
            {
                return HandleHorizontalActionDown(currentTouchPoint);
            }

            return HandleVerticalActionDown(currentTouchPoint);
        }

        private bool HandleHorizontalActionDown(Point currentTouchPoint)
        {
            if (DrawerSettings.Position == Position.Left)
            {
                if (_isRTL)
                {
                    if (_isDrawerOpen && currentTouchPoint.X < ScreenWidth - DrawerSettings.DrawerWidth)
                    {
                        return true;
                    }
                }
                else
                {
                    if (_isDrawerOpen && currentTouchPoint.X > DrawerSettings.DrawerWidth)
                    {
                        return true;
                    }
                }
            }
            else if (DrawerSettings.Position == Position.Right)
            {
                if (_isRTL)
                {
                    if (_isDrawerOpen && currentTouchPoint.X > DrawerSettings.DrawerWidth)
                    {
                        return true;
                    }
                }
                else
                {
                    if (_isDrawerOpen && currentTouchPoint.X < ScreenWidth - DrawerSettings.DrawerWidth)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HandleVerticalActionDown(Point currentTouchPoint)
        {
            if (DrawerSettings.Position == Position.Top)
            {
                return _isDrawerOpen && currentTouchPoint.Y > DrawerSettings.DrawerHeight;
            }
            else if (DrawerSettings.Position == Position.Bottom)
            {
                return _isDrawerOpen && currentTouchPoint.Y < ScreenHeight - DrawerSettings.DrawerHeight;
            }
            return false;
        }

        private bool HandleActionMove(MotionEvent ev, Point currentTouchPoint)
        {
            _moveX = ev.GetX();
            _moveY = ev.GetY();

            if (!_isPressed && (_initialPoint.X != 0 || _initialPoint.Y != 0))
            {
                if (DrawerSettings.Position == Position.Left)
                {
                    if (Math.Abs(_moveX - _downX) > 10)
                    {
                        return HandleLeftDrawerMove(currentTouchPoint);
                    }
                }
                else if (DrawerSettings.Position == Position.Right)
                {
                    if (Math.Abs(_moveX - _downX) > 10)
                    {
                        return HandleRightDrawerMove(currentTouchPoint);
                    }
                }
                else if (DrawerSettings.Position == Position.Top || DrawerSettings.Position == Position.Bottom)
                {
                    return HandleVerticalDrawerMove();
                }
            }
            return false;
        }

        private bool HandleLeftDrawerMove(Point initialPoint)
        {
            if (_isRTL)
            {
                if ((!_isDrawerOpen && initialPoint.X >= _touchRightThreshold) || this._isDrawerOpen)
                {
                    OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    return true;
                }
            }
            else
            {
                if ((!_isDrawerOpen && initialPoint.X <= DrawerSettings.TouchThreshold) || this._isDrawerOpen)
                {
                    OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    return true;
                }
            }
            return false;
        }

        private bool HandleRightDrawerMove(Point initialPoint)
        {
            if (_isRTL)
            {
                if ((!_isDrawerOpen && initialPoint.X <= DrawerSettings.TouchThreshold) || this._isDrawerOpen)
                {
                    OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    return true;
                }
            }
            else
            {
                if ((!_isDrawerOpen && initialPoint.X >= _touchRightThreshold) || this._isDrawerOpen)
                {
                    OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    return true;
                }
            }
            return false;
        }

        private bool HandleVerticalDrawerMove()
        {
            if (DrawerSettings.Position == Position.Top)
            {
                if (Math.Abs(_moveY - _downY) > 10)
                {
                    if (!_isDrawerOpen && _initialPoint.Y <= DrawerSettings.TouchThreshold) 
                    {
                        OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                        return true;
                    }
                }
            }
            else if (DrawerSettings.Position == Position.Bottom)
            {
                if (Math.Abs(_moveY - _downY) > 10)
                {
                    if (!_isDrawerOpen && _initialPoint.Y >= _touchBottomThreshold) 
                    {
                        OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Handles touch input events when a pointer enters the control.
        /// </summary>
        /// <param name="e">Pointer event arguments containing information about the touch event.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            switch (e.Action)
            {
                case PointerActions.Pressed:
                    {
                        _touchDownX = e.TouchPoint.X;
                        _touchDownY = e.TouchPoint.Y;
                        OnHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                        break;
                    }

                case PointerActions.Moved:
                    {
                        _touchMoveX = e.TouchPoint.X;
                        _touchMoveY = e.TouchPoint.Y;
                        if (DrawerSettings.Position == Position.Left || DrawerSettings.Position == Position.Right)
                        {
                            if (Math.Abs(_touchMoveX - _touchDownX) > 10)
                            {
                                OnHandleTouchInteraction(PointerActions.Moved, e.TouchPoint);
                            }
                        }
                        else if (DrawerSettings.Position == Position.Top || DrawerSettings.Position == Position.Bottom)
                        {
                            if (Math.Abs(_touchMoveY - _touchDownY) > 10)
                            {
                                OnHandleTouchInteraction(PointerActions.Moved, e.TouchPoint);
                            }
                        }
                        break;
                    }
                case PointerActions.Released:
                    {
                        OnHandleTouchInteraction(PointerActions.Released, e.TouchPoint);
                        break;
                    }

                case PointerActions.Cancelled:
                    {
                        break;
                    }

                case PointerActions.Entered:
                    {
                        break;
                    }
            }
        }

        #endregion
    }
}