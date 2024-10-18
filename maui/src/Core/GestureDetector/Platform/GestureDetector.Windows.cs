using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Provides gesture detection functionality for the Windows platform.
    /// </summary>
    public partial class GestureDetector
    {
        #region Fields

        bool _isPinchStarted;
        bool _isPanStarted;
        bool _isPinching;
        bool _isPanning;
        private bool _isDoubleTapDetected = false;
        Windows.Foundation.Point _touchMovePoint;
        readonly Dictionary<uint, Windows.Foundation.Point> _touchPointers = new Dictionary<uint, Windows.Foundation.Point>();
        ManipulationModes _defaultManipulationMode = ManipulationModes.System;
        Point _panVelocity;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Subscribes to native gesture events on the platform view.
        /// </summary>
        internal void SubscribeNativeGestureEvents(View? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIElement? nativeView = handler?.PlatformView as UIElement;
                if (nativeView != null)
                {
                    _defaultManipulationMode = nativeView.ManipulationMode;

                    SubscribeTapEvents(nativeView);
                    SubscribeDoubleTapEvents(nativeView);
                    SubscribeLongPressEvents(nativeView);
                    SubscribePanAndPinchEvents(nativeView);
                    SubscribeRightTapEvents(nativeView);
                }
            }
        }

        internal void CreateNativeListener()
        {
            SubscribeNativeGestureEvents(MauiView);
        }

        /// <summary>
        /// Unsubscribes from all native gesture events and resets related states.
        /// </summary>
        internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIElement? nativeView = handler.PlatformView as UIElement;

                if (nativeView != null)
                {
                    nativeView.ManipulationMode = _defaultManipulationMode;
                    nativeView.Tapped -= PlatformView_Tapped;
                    nativeView.DoubleTapped -= PlatformView_DoubleTapped;
                    nativeView.Holding -= PlatformView_Holding;
                    nativeView.PointerPressed -= PlatformView_PointerPressed;
                    nativeView.PointerMoved -= PlatformView_PointerMoved;
                    nativeView.PointerReleased -= PlatformView_PointerReleased;
                    nativeView.PointerCanceled -= PlatformView_PointerCanceled;
                    nativeView.PointerExited -= PlatformView_PointerExited;
                    nativeView.PointerCaptureLost -= PlatformView_PointerCaptureLost;
                    nativeView.ManipulationInertiaStarting -= OnNativeViewManipulationInertiaStarting;
                    nativeView.RightTapped -= PlatformView_RightTapped;
                }
            }
            _touchPointers.Clear();
        }

        #endregion

        #region Private Methods

        void SubscribeTapEvents(UIElement nativeView)
        {
            if (_tapGestureListeners != null && _tapGestureListeners.Count > 0)
            {
                nativeView.Tapped += PlatformView_Tapped;
            }
        }

        void SubscribeDoubleTapEvents(UIElement nativeView)
        {
            if (_doubleTapGestureListeners != null && _doubleTapGestureListeners.Count > 0)
            {
                nativeView.DoubleTapped += PlatformView_DoubleTapped;
            }
        }

        void SubscribeLongPressEvents(UIElement nativeView)
        {
            if (_longPressGestureListeners != null && _longPressGestureListeners.Count > 0)
            {
                nativeView.Holding += PlatformView_Holding;
            }
        }

        void SubscribePanAndPinchEvents(UIElement nativeView)
        {
            if ((_panGestureListeners != null && _panGestureListeners.Count > 0) || 
                (_pinchGestureListeners != null && _pinchGestureListeners.Count > 0))
            {
                nativeView.PointerPressed += PlatformView_PointerPressed;
                nativeView.PointerMoved += PlatformView_PointerMoved;
                nativeView.PointerReleased += PlatformView_PointerReleased;
                nativeView.PointerCanceled += PlatformView_PointerCanceled;
                nativeView.PointerExited += PlatformView_PointerExited;
                nativeView.PointerCaptureLost += PlatformView_PointerCaptureLost;
            }

            if (_panGestureListeners != null && _panGestureListeners.Count > 0)
            {
                // Manipulation inertia event is wired to get the pan velocity value.
                nativeView.ManipulationInertiaStarting += OnNativeViewManipulationInertiaStarting;
            }
        }

        void SubscribeRightTapEvents(UIElement nativeView)
        {
            if (_rightTapGestureListeners != null && _rightTapGestureListeners.Count > 0)
            {
                nativeView.RightTapped += PlatformView_RightTapped;
            }
        }

        void AddPointer(Pointer pointer, Windows.Foundation.Point position)
        {
            if (!_touchPointers.ContainsKey(pointer.PointerId))
            {
                _touchPointers.Add(pointer.PointerId, position);
            }
        }

        void RemovePointer(Pointer pointer)
        {
            if (_touchPointers.ContainsKey(pointer.PointerId))
            {
                _touchPointers.Remove(pointer.PointerId);
            }
        }

        void PlatformView_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            OnPointerEnd(sender, e, false);
        }

        void OnPointerEnd(object sender, PointerRoutedEventArgs e, bool isReleasedProperly)
        {
            RemovePointer(e.Pointer);
            PinchCompleted(e, isReleasedProperly);
            PanCompleted(e, isReleasedProperly);
            if (sender is UIElement platformView)
                platformView.ManipulationMode = _defaultManipulationMode;
        }

        void PlatformView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            _touchMovePoint = e.GetCurrentPoint(sender as UIElement).Position;
            OnPinch(e, _touchMovePoint);
            OnPan(e, _touchMovePoint);
        }

        void PlatformView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            OnPointerEnd(sender, e, true);
        }

        void PlatformView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            OnPointerEnd(sender, e, false);
        }

        void PlatformView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            OnPointerEnd(sender, e, true);
        }

        void PlatformView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            AddPointer(e.Pointer, e.GetCurrentPoint(sender as UIElement).Position);
            SetManipulationMode(sender);
        }

        void SetManipulationMode(object sender)
        {
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                if (_touchPointers.Count == 1 &&
                    _panGestureListeners != null &&
                    _panGestureListeners.Count > 0)
                    nativeView.ManipulationMode = ManipulationModes.TranslateX
                                  | ManipulationModes.TranslateY
                                  | ManipulationModes.TranslateInertia;

                if (_touchPointers.Count == 2 && 
                    _pinchGestureListeners != null && 
                    _pinchGestureListeners.Count > 0)
                    nativeView.ManipulationMode = ManipulationModes.Scale;

                if (_touchPointers.Count > 2)
                    nativeView.ManipulationMode = _defaultManipulationMode;
            }
        }

        /// <summary>
        /// Invoked when inertia starts during a pan action. This sets the pan velocity in PanEventArgs after the pointer up action.
        /// </summary>
        void OnNativeViewManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            if (_panGestureListeners == null || _panGestureListeners.Count == 0)
                return;

            // Store the pan velocity for use in the pan completed event.
            // This captures the "fling" velocity at the end of the pan gesture.
            // The velocity is normalized across all platforms by multiplying by 1000.
            this._panVelocity = new Point(e.Velocities.Linear.X * 1000, e.Velocities.Linear.Y * 1000);
        }

        void OnPan(PointerRoutedEventArgs args, Windows.Foundation.Point position)
        {
            uint pointerID = args.Pointer.PointerId;
            if (_panGestureListeners == null || 
                _panGestureListeners.Count == 0 || 
                _touchPointers.Count != 1 || 
                !_touchPointers.ContainsKey(pointerID))
                return;

            var oldTouchPoint = new Point(_touchPointers[pointerID].X, _touchPointers[pointerID].Y);
            _touchPointers[pointerID] = position;
            var touchPoint = new Point(_touchMovePoint.X, _touchMovePoint.Y);
            var translationPoint = new Point(touchPoint.X - oldTouchPoint.X, touchPoint.Y - oldTouchPoint.Y);
            GestureStatus state = GestureStatus.Started;
            _isPanning = true;
            foreach (var listener in _panGestureListeners)
            {
                if (_isPanStarted)
                    state = GestureStatus.Running;

                // Pan velocity is only set on pan completed action, that will be called after the manipulation inertia triggered.
                PanEventArgs eventArgs = new PanEventArgs((relativeTo) => TouchDetector.GetPosition(relativeTo, args), state, touchPoint, translationPoint, Point.Zero);
                listener.OnPan(eventArgs);
            }
            _isPanStarted = true;
        }

        void OnPinch(PointerRoutedEventArgs args, Windows.Foundation.Point position)
        {
            uint pointerID = args.Pointer.PointerId;
            if (_pinchGestureListeners == null || 
                _pinchGestureListeners.Count == 0 || 
                _touchPointers.Count != 2 || 
                !_touchPointers.ContainsKey(pointerID))
                return;

            if (_isPanStarted)
                PanCompleted(args, false);

            List<uint> keys = _touchPointers.Keys.ToList();

            //Pointers poistion before the pinch.
            Windows.Foundation.Point oldPointerPosition1 = _touchPointers[keys[0]];
            Windows.Foundation.Point oldPointerPosition2 = _touchPointers[keys[1]];

            _touchPointers[pointerID] = position;

            //Pointers poistion after the pinch.
            Windows.Foundation.Point newPointerPosition1 = _touchPointers[keys[0]];
            Windows.Foundation.Point newPointerPosition2 = _touchPointers[keys[1]];

            keys.Clear();

            CalculateAndPerformPinchGesture(args, oldPointerPosition1, oldPointerPosition2, newPointerPosition1, newPointerPosition2);
        }

        void CalculateAndPerformPinchGesture(PointerRoutedEventArgs args,
            Windows.Foundation.Point oldPosition1, Windows.Foundation.Point oldPosition2,
            Windows.Foundation.Point newPosition1, Windows.Foundation.Point newPosition2)
        {
            //Sum of two points before the pinch to calculate the changes in the distance and origin of zoom.
            Point oldCumulativePoint = new Point(oldPosition1.X + oldPosition2.X,
                oldPosition1.Y + oldPosition2.Y);

            //Distance between the two touch pointers before the pinch.
            double oldDistance = MathUtils.GetDistance(oldPosition1.X, oldPosition2.X,
                oldPosition1.Y, oldPosition2.Y);

            if (!_isPinchStarted)
                PerformPinch(args, new Point(oldCumulativePoint.X / 2, oldCumulativePoint.Y / 2),
                    Point.Zero, 1);

            //Sum of two points after the pinch to calculate the changes in the distance and origin of zoom.
            Point newCumulativePoint = new Point(newPosition1.X + newPosition2.X,
                newPosition1.Y + newPosition2.Y);

            //Distance between the two touch pointers after the pinch.
            double newDistance = MathUtils.GetDistance(newPosition1.X, newPosition2.X,
                newPosition1.Y, newPosition2.Y);

            //Mid point of two touch pointers.
            Point scalePoint = new Point(newCumulativePoint.X / 2, newCumulativePoint.Y / 2);
            //Calculate the x and y translation change before and after pinch.
            Point translationPoint = new Point(oldCumulativePoint.X / 2, oldCumulativePoint.Y / 2);
            float scale = (float)(newDistance / oldDistance);

            PerformPinch(args, scalePoint, translationPoint, scale);
        }

        void PerformPinch(PointerRoutedEventArgs args, Point scalePoint, Point translationPoint, float scale)
        {
            if (_pinchGestureListeners == null)
                return;
            GestureStatus state = GestureStatus.Started;
            double angle = MathUtils.GetAngle(scalePoint.X, translationPoint.X, scalePoint.Y, translationPoint.Y);
            _isPinching = true;
            foreach (var listener in _pinchGestureListeners)
            {
                if (_isPinchStarted)
                {
                    state = GestureStatus.Running;
                }
                PinchEventArgs eventArgs = new PinchEventArgs((relativeTo) => TouchDetector.GetPosition(relativeTo, args), state, scalePoint, angle, scale);
                listener.OnPinch(eventArgs);
            }
            _isPinchStarted = true;
        }

        void PinchCompleted(PointerRoutedEventArgs args, bool isCompleted, Point scalePoint = new Point(), double angle = double.NaN, float scale = 1)
        {
            if (_pinchGestureListeners == null || _pinchGestureListeners.Count == 0 || !_isPinching) return;

            foreach (var listener in _pinchGestureListeners)
            {
                PinchEventArgs eventArgs = new PinchEventArgs((relativeTo) => TouchDetector.GetPosition(relativeTo, args), isCompleted ? GestureStatus.Completed : GestureStatus.Canceled, scalePoint, angle, scale);
                listener.OnPinch(eventArgs);
            }
            _isPinching = false;
            _touchPointers.Clear();
            _isPinchStarted = false;
        }

        void PanCompleted(PointerRoutedEventArgs args, bool isCompleted)
        {
            if (_panGestureListeners == null || _panGestureListeners.Count == 0 || !_isPanning) return;

            foreach (var listener in _panGestureListeners)
            {
                PanEventArgs eventArgs = new PanEventArgs((relativeTo) => TouchDetector.GetPosition(relativeTo, args), isCompleted ? GestureStatus.Completed : GestureStatus.Canceled, new Point(_touchMovePoint.X, _touchMovePoint.Y), Point.Zero, _panVelocity);
                listener.OnPan(eventArgs);
            }
            _isPanning = false;
            _isPanStarted = false;
            _panVelocity = Point.Zero;
        }

        void PlatformView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            if (_longPressGestureListeners == null || _longPressGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in _longPressGestureListeners)
            {
                if (e.HoldingState == Microsoft.UI.Input.HoldingState.Started)
                {
                    listener.OnLongPress(new LongPressEventArgs((relativeTo) => TouchDetector.GetPosition(relativeTo, e), new Point(touchPoint.X, touchPoint.Y)));
                }
            }

            if (_longPressGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        void PlatformView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            if (_doubleTapGestureListeners == null || _doubleTapGestureListeners.Count == 0) return;

            this._isDoubleTapDetected = true;
            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in _doubleTapGestureListeners)
            {
                listener.OnDoubleTap(new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 2));
            }

            if (_doubleTapGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        void PlatformView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            if (_tapGestureListeners == null || _tapGestureListeners.Count == 0)
            {
                return;
            }

            var touchPoint = e.GetPosition(sender as UIElement);

            if (_doubleTapGestureListeners != null && 
                _doubleTapGestureListeners.Count > 0 && 
                !_doubleTapGestureListeners[0].IsRequiredSingleTapGestureRecognizerToFail)
            {
                InvokeSingleTap(new Point(touchPoint.X, touchPoint.Y));
            }
            else
            {
                foreach (var listener in _tapGestureListeners)
                {
                    listener.OnTap(MauiView, new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 1));
                    listener.OnTap(new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 1));
                }
            }

            if (_tapGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        async void InvokeSingleTap(object? point)
        {
            await Task.Delay(300);
            if (this._isDoubleTapDetected)
            {
                this._isDoubleTapDetected = false;
                return;
            }

            if (_tapGestureListeners == null || _tapGestureListeners.Count == 0) return;
            if (point != null && point is Point touchPoint)
            {
                foreach (var listener in _tapGestureListeners)
                {
                    listener.OnTap(MauiView, new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 1));
                    listener.OnTap(new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 1));
                }
            }
        }

        void PlatformView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            if (_rightTapGestureListeners == null || _rightTapGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in _rightTapGestureListeners)
            {
                var pointerDeviceType = PointerDeviceType.Mouse;
                if (e.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Touch)
                {
                    pointerDeviceType = PointerDeviceType.Touch;
                }

                var rightTapEventArgs = new RightTapEventArgs(new Point(touchPoint.X, touchPoint.Y), pointerDeviceType);
                listener.OnRightTap(MauiView, rightTapEventArgs);
                listener.OnRightTap(rightTapEventArgs);
            }

            if (_rightTapGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        #endregion
    }
}