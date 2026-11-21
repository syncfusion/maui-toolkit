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
		Point _initialPoint = new(0, 0);

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

				_ = ev.GetPointerId(actionIndex);
				Point screenPoint = new Point(ev.GetX(actionIndex), ev.GetY(actionIndex));
				Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
				Point currentTouchPoint = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));

				switch (ev.Action)
				{
					case MotionEventActions.Down:
						return HandleActionDown(ev,currentTouchPoint);

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

		private bool HandleActionDown(MotionEvent? ev, Point currentTouchPoint)
		{
			if (ev != null)
			{
				_downX = ev.GetX();
				_downY = ev.GetY();
				_initialPoint = currentTouchPoint;
                if (_isSecondaryDrawerOpen && this.SecondaryDrawerSettings != null)
                {
                    if (DrawerSettings.Position == Position.Left || DrawerSettings.Position == Position.Right)
                    {
                        return HandleSecondaryHorizontalActionDown(currentTouchPoint);
                    }
                    return HandleSecondaryVerticalActionDown(currentTouchPoint);
                }
                if (DrawerSettings.Position == Position.Left || DrawerSettings.Position == Position.Right)
				{
					return HandleHorizontalActionDown(currentTouchPoint);
				}
                return HandleVerticalActionDown(currentTouchPoint);
            }
			return false;
		}

		private bool HandleSecondaryHorizontalActionDown(Point currentTouchPoint)
		{
			if (SecondaryDrawerSettings.Position == Position.Left)
			{
                // touch on content area when left drawer is open
                if (currentTouchPoint.X > this.SecondaryDrawerSettings.DrawerWidth)
                {
                    return true;
                }
			}
			else if (SecondaryDrawerSettings.Position == Position.Right)
			{
                if (currentTouchPoint.X < this.ScreenWidth - this.SecondaryDrawerSettings.DrawerWidth)
                {
                    return true;
                }
			}
			return false;
		}

		private bool HandleSecondaryVerticalActionDown(Point currentTouchPoint)
		{
			if (SecondaryDrawerSettings.Position == Position.Top)
			{
                if (currentTouchPoint.Y > this.SecondaryDrawerSettings.DrawerHeight)
                {
                    return true;
                }
			}
			else if (SecondaryDrawerSettings.Position == Position.Bottom)
			{
				if (currentTouchPoint.Y < this.ScreenHeight - this.SecondaryDrawerSettings.DrawerHeight)
                {
                    return true;
                }
			}
			return false;
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
            bool secondaryGesture = false;

            if (!_isPressed && (_initialPoint.X != 0 || _initialPoint.Y != 0))
			{
                // If both drawers share same edge, prioritize primary first (Windows parity)
                if (!_isPrimaryDrawerSwiping && this.IsPrimaryDrawerPrioritized() && !_isSecondaryDrawerOpen)
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
                secondaryGesture = _isPrimaryDrawerSwiping;
			    }
                
                if (!secondaryGesture && !_isSecondaryDrawerSwiping && !_isPrimaryDrawerSwiping && this.SecondaryDrawerSettings != null)
                {
                    if (this.SecondaryDrawerSettings.Position == Position.Left)
                    {
                        if (Math.Abs(_moveX - _downX) > 10)
                        {
                            return HandleSecondaryLeftDrawerMove(currentTouchPoint);
                        }
                    }
                    else if (this.SecondaryDrawerSettings.Position == Position.Right)
                    {
                        if (Math.Abs(_moveX - _downX) > 10)
                        {
                            return HandleSecondaryRightDrawerMove(currentTouchPoint);
                        }
                    }
                    else if (this.SecondaryDrawerSettings.Position == Position.Top || this.SecondaryDrawerSettings.Position == Position.Bottom)
                    {
                        return HandleSecondaryVerticalDrawerMove();
                    }
                }

                // If still nothing started, fallback to primary logic
                if (!_isSecondaryDrawerSwiping && !_isPrimaryDrawerSwiping)
                {
                    if (this.DrawerSettings.Position == Position.Left)
                    {
                        if (Math.Abs(_moveX - _downX) > 10)
                        {
                            if (_isRTL)
                            {
                                if ((!_isDrawerOpen && _initialPoint.X >= _touchRightThreshold) || _isDrawerOpen)
                                {
                                    this.OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                                    _isPrimaryDrawerSwiping = true;
                                    return true;
                                }
                            }
                            else
                            {
                                if ((!_isDrawerOpen && _initialPoint.X <= this.DrawerSettings.TouchThreshold) || _isDrawerOpen)
                                {
                                    this.OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                                    _isPrimaryDrawerSwiping = true;
                                    return true;
                                }
                            }
                        }
                    }
                    else if (this.DrawerSettings.Position == Position.Right)
                    {
                        if (Math.Abs(_moveX - _downX) > 10)
                        {
                            if (_isRTL)
                            {
                                if ((!_isDrawerOpen && _initialPoint.X <= this.DrawerSettings.TouchThreshold) || _isDrawerOpen)
                                {
                                    this.OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                                    _isPrimaryDrawerSwiping = true;
                                    return true;
                                }
                            }
                            else
                            {
                                if ((!_isDrawerOpen && _initialPoint.X >= _touchRightThreshold) || _isDrawerOpen)
                                {
                                    this.OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                                    _isPrimaryDrawerSwiping = true;
                                    return true;
                                }
                            }
                        }
                    }
                    else if (this.DrawerSettings.Position == Position.Top)
                    {
                        if (Math.Abs(_moveY - _downY) > 10)
                        {
                            if ((!_isDrawerOpen && _initialPoint.Y <= this.DrawerSettings.TouchThreshold) || _isDrawerOpen)
                            {
                                this.OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                                _isPrimaryDrawerSwiping = true;
                                return true;
                            }
                        }
                    }
                    else if (this.DrawerSettings.Position == Position.Bottom)
                    {
                        if (Math.Abs(_moveY - _downY) > 10)
                        {
                            if ((!_isDrawerOpen && _initialPoint.Y >= _touchBottomThreshold) || _isDrawerOpen)
                            {
                                this.OnHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                                _isPrimaryDrawerSwiping = true;
                                return true;
                            }
                        }
                    }
                }
            }
			return false;
		}

		private bool HandleLeftDrawerMove(Point initialPoint)
		{
			if (_isRTL)
			{
				if ((!_isDrawerOpen && initialPoint.X >= _touchRightThreshold) || _isDrawerOpen)
				{
					OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    _isPrimaryDrawerSwiping = true;
					return true;
				}
			}
			else
			{
				if ((!_isDrawerOpen && initialPoint.X <= DrawerSettings.TouchThreshold) || _isDrawerOpen)
				{
					OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    _isPrimaryDrawerSwiping = true;
					return true;
				}
			}
			return false;
		}

		private bool HandleRightDrawerMove(Point initialPoint)
		{
			if (_isRTL)
			{
				if ((!_isDrawerOpen && initialPoint.X <= DrawerSettings.TouchThreshold) || _isDrawerOpen)
				{
					OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    _isPrimaryDrawerSwiping = true;
					return true;
				}
			}
			else
			{
				if ((!_isDrawerOpen && initialPoint.X >= _touchRightThreshold) || _isDrawerOpen)
				{
					OnHandleTouchInteraction(PointerActions.Pressed, initialPoint);
                    _isPrimaryDrawerSwiping = true;
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
                        _isPrimaryDrawerSwiping = true;
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
                        _isPrimaryDrawerSwiping = true;
						return true;
					}
				}
			}
			return false;
		}

		private bool HandleSecondaryLeftDrawerMove(Point initialPoint)
		{
            if ((!_isSecondaryDrawerOpen && _initialPoint.X <= this.SecondaryDrawerSettings.TouchThreshold) || _isSecondaryDrawerOpen)
            {
                this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                _isSecondaryDrawerSwiping = true;
                return true;
            }
			return false;
		}

		private bool HandleSecondaryRightDrawerMove(Point initialPoint)
		{
            if ((!_isSecondaryDrawerOpen && _initialPoint.X >= (this.ScreenWidth - this.SecondaryDrawerSettings.TouchThreshold)) || _isSecondaryDrawerOpen)
            {
                this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                _isSecondaryDrawerSwiping = true;
                return true;
            }
			return false;
		}

		private bool HandleSecondaryVerticalDrawerMove()
		{
			if (DrawerSettings.Position == Position.Top)
			{
                if (Math.Abs(_moveY - _downY) > 10)
                {
                    if ((!_isSecondaryDrawerOpen && _initialPoint.Y <= this.SecondaryDrawerSettings.TouchThreshold))
                    {
                        this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                        _isSecondaryDrawerSwiping = true;
                        return true;
                    }
                }
			}
			else if (DrawerSettings.Position == Position.Bottom)
			{
                if (Math.Abs(_moveY - _downY) > 10)
                {
                    if ((!_isSecondaryDrawerOpen && _initialPoint.Y >= (this.ScreenHeight - this.SecondaryDrawerSettings.TouchThreshold)))
                    {
                        this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, _initialPoint);
                        _isSecondaryDrawerSwiping = true;
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

                        bool handled = false;

                        // Prioritize primary drawer if it has priority or is already open/swiping
                        if ((this.IsPrimaryDrawerPrioritized() && !_isSecondaryDrawerOpen) || _isDrawerOpen || _isPrimaryDrawerSwiping)
                        {
                            this.OnHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                            _isPrimaryDrawerSwiping = true;
                            handled = true;
                        }
                        // Handle secondary drawer if it's open/swiping and primary isn't prioritized
                        else if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                            handled = true;
                        }
                        // Edge activation for secondary drawer (only if primary isn't prioritized or active)
                        else if (this.SecondaryDrawerSettings != null && !_isPrimaryDrawerSwiping)
                        {
                            switch (this.SecondaryDrawerSettings.Position)
                            {
                                case Position.Left:
                                    if ((!_isRTL && e.TouchPoint.X <= this.SecondaryDrawerSettings.TouchThreshold && e.TouchPoint.X >= 0) ||
                                        (_isRTL && e.TouchPoint.X >= (this.ScreenWidth - this.SecondaryDrawerSettings.TouchThreshold) && e.TouchPoint.X <= this.ScreenWidth))
                                    {
                                        this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                                        _isSecondaryDrawerSwiping = true;
                                        handled = true;
                                    }
                                    break;
                                case Position.Right:
                                    if ((!_isRTL && e.TouchPoint.X >= (this.ScreenWidth - this.SecondaryDrawerSettings.TouchThreshold) && e.TouchPoint.X <= this.ScreenWidth) ||
                                        (_isRTL && e.TouchPoint.X <= this.SecondaryDrawerSettings.TouchThreshold && e.TouchPoint.X >= 0))
                                    {
                                        this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                                        _isSecondaryDrawerSwiping = true;
                                        handled = true;
                                    }
                                    break;
                                case Position.Top:
                                    if (e.TouchPoint.Y <= this.SecondaryDrawerSettings.TouchThreshold && e.TouchPoint.Y >= 0)
                                    {
                                        this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                                        _isSecondaryDrawerSwiping = true;
                                        handled = true;
                                    }
                                    break;
                                case Position.Bottom:
                                    if (e.TouchPoint.Y >= (this.ScreenHeight - this.SecondaryDrawerSettings.TouchThreshold) && e.TouchPoint.Y <= this.ScreenHeight)
                                    {
                                        this.OnSecondaryHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                                        _isSecondaryDrawerSwiping = true;
                                        handled = true;
                                    }
                                    break;
                            }
                        }

                        // Fallback to primary drawer if not handled by secondary and primary itself is not currently swiping
                        if (!handled && !_isPrimaryDrawerSwiping && !_isSecondaryDrawerSwiping)
                        {
                            this.OnHandleTouchInteraction(PointerActions.Pressed, e.TouchPoint);
                            _isPrimaryDrawerSwiping = true;
                        }

                        break;
                    }

                case PointerActions.Moved:
                    {
                        _touchMoveX = e.TouchPoint.X;
                        _touchMoveY = e.TouchPoint.Y;

                        if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Moved, e.TouchPoint);
                        }
                        else if (_isDrawerOpen || _isPrimaryDrawerSwiping)
                        {
                            this.OnHandleTouchInteraction(PointerActions.Moved, e.TouchPoint);
                        }
                        break;
                    }

                case PointerActions.Released:
                    {
                        if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Released, e.TouchPoint);
                            _isSecondaryDrawerSwiping = false;
                        }
                        else
                        {
                            this.OnHandleTouchInteraction(PointerActions.Released, e.TouchPoint);
                            _isPrimaryDrawerSwiping = false;
                        }
                        break;
                    }

                case PointerActions.Cancelled:
                    {
                        if (_isSecondaryDrawerOpen || _isSecondaryDrawerSwiping)
                        {
                            this.OnSecondaryHandleTouchInteraction(PointerActions.Cancelled, e.TouchPoint);
                            _isSecondaryDrawerSwiping = false;
                        }
                        else
                        {
                            this.OnHandleTouchInteraction(PointerActions.Cancelled, e.TouchPoint);
                            _isPrimaryDrawerSwiping = false;
                        }
                        break;
                    }

                case PointerActions.Entered:
                    {
                        break;
                    }
            }
		}

        // Mirrors Windows: primary gets priority when both drawers share the same edge
        private bool IsPrimaryDrawerPrioritized()
        {
            if (this.SecondaryDrawerSettings != null && this.DrawerSettings.Position == this.SecondaryDrawerSettings.Position && 
            (this.DrawerSettings.DrawerHeaderView !=null || this.DrawerSettings.DrawerContentView !=null || this.DrawerSettings.DrawerFooterView!=null))
            {
                return true;
            }
            return false;
        }

		#endregion
	}
}