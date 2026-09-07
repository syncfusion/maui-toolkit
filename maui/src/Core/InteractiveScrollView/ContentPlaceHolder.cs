namespace Syncfusion.Maui.Toolkit.Internals
{
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using System;

    /// <summary>
    /// Represents a content container that hosts scrollable and zoomable content, manages gesture interactions, and dispatches touch-related events to the appropriate child elements.
    /// </summary>
    internal class ContentPlaceHolder : AbsoluteLayout
    {
        #region Fields

        /// <summary>
        /// Handles pan, zoom, tap, double-tap, and long-press interactions.
        /// </summary>
        PanZoomListener _panZoomListener;

#if ANDROID

        /// <summary>
        /// Stores the current touch location during gesture operations.
        /// </summary>
        Point _location = new Point(0, 0);

        /// <summary>
        /// Stores the current zoom scale factor during pinch gestures.
        /// </summary>
        double _scale = 1;

        /// <summary>
        /// Stores the bounds of the ancestor view during gesture calculations.
        /// </summary>
        Rect _ancestorBounds = Rect.Zero;

        /// <summary>
        /// Stores the position of the page relative to the screen during gesture handling.
        /// </summary>
        Point _pagePosition = Point.Zero;

        /// <summary>
        /// Represents the child view associated with the page position for event routing.
        /// </summary>
        IView? _targetChildForPagePosition = null;

        /// <summary>
        /// Stores a temporary point used during coordinate calculations and transformations.
        /// </summary>
        Point _point = Point.Zero;

#endif

        #endregion

        #region Event

        /// <summary>
        /// Occurs when a tap gesture is detected on the content.
        /// </summary>
        internal event EventHandler<TapEventArgs>? Tapped;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPlaceHolder"/> class.
        /// </summary>
        /// <param name="panZoomListener">The listener responsible for handling gesture interactions.</param>
        internal ContentPlaceHolder(PanZoomListener panZoomListener)
        {
#if IOS
#if NET9_0
            this.IgnoreSafeArea = true;
#else
            this.SafeAreaEdges = SafeAreaEdges.None;
#endif
#endif
            _panZoomListener = panZoomListener;
            _panZoomListener.OnTouch += OnPanZoomListenerTouch;
            _panZoomListener.OnTap += OnPanZoomListenerTapped;
            _panZoomListener.OnLongPress += OnPanZoomListenerLongPress;
            _panZoomListener.OnDoubleTap += OnPanZoomListenerDoubleTap;

            this.ChildAdded += OnChildAdded;
            this.ChildRemoved += OnChildRemoved;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Registers pan and zoom gesture listeners.
        /// </summary>
        internal void AddZoomGestures()
        {
            if (_panZoomListener != null)
            {
                this.AddGestureListener(_panZoomListener);
                this.AddKeyboardListener(_panZoomListener);
                this.AddTouchListener(_panZoomListener);
            }
        }

        /// <summary>
        /// Removes registered gesture, keyboard, and touch listeners.
        /// </summary>
        internal void RemoveZoomGestures()
        {
            this.ClearKeyboardListeners();
            this.ClearTouchListeners();
            this.ClearGestureListeners();
        }

        /// <summary>
        /// Updates the layout position of the hosted content based on its alignment settings.
        /// </summary>
        internal void UpdateLayoutContent()
        {
            if (this.Children.Count > 0 && this.Children[0] is View content)
            {
                this.SetLayoutFlags(content, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional);
                double horizontalAlignmentProportion = GetAlignmentProportion(content.HorizontalOptions.Alignment);
                double verticalAlignmentProportion = GetAlignmentProportion(content.VerticalOptions.Alignment);
#if IOS || MACCATALYST
                // The following workaround line of code (for iOS and MAC alone) is added due to the existing content alignment issue in .NET MAUI (MAC) with RTL flow direction. GitHub link https://github.com/dotnet/maui/issues/9970
                if (this.FlowDirection == FlowDirection.RightToLeft)
                    horizontalAlignmentProportion = 1 - horizontalAlignmentProportion;
#endif
                this.SetLayoutBounds(content, new Rect(horizontalAlignmentProportion, verticalAlignmentProportion, AutoSize, AutoSize));
            }
        }

        /// <summary>
        /// Requests the specified size for the content host.
        /// </summary>
        /// <param name="width">The requested width.</param>
        /// <param name="height">The requested height.</param>
        internal void RequestSize(double width, double height)
        {
            this.WidthRequest = width;
            this.HeightRequest = height;
        }

        /// <summary>
        /// Removes gesture handlers and clears the hosted content.
        /// </summary>
        internal void Unload()
        {
            RemoveZoomGestures();
            if (Children.Count > 0)
            {
                Children.Clear();
            }
        }

        /// <summary>
        /// Clears the hosted content and resets the requested size.
        /// </summary>
        internal void Reset()
        {
            if (Children.Count > 0)
            {
                this.Children.Clear();
            }

            RequestSize(-1, -1);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the alignment proportion used by the absolute layout.
        /// </summary>
        /// <param name="layoutAlignment">The alignment value.</param>
        /// <returns>A proportional alignment value between 0 and 1.</returns>
        double GetAlignmentProportion(LayoutAlignment layoutAlignment)
        {
            switch (layoutAlignment)
            {
                case LayoutAlignment.Start:
                    return 0;
                case LayoutAlignment.End:
                    return 1;
                default:
                    return 0.5;
            }
        }

        /// <summary>
        /// Handles the double-tap gesture and resets the interaction state.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The tap event data.</param>
        void OnPanZoomListenerDoubleTap(object? sender, TapEventArgs e)
        {
#if ANDROID
            // Android-specific gesture handling: verify content is available before processing
            if (Children.Count > 0)
            {
                OnDoubleTap(Children[0] as View, e);
            }

            // Reset interaction state after gesture completion
            _location = new Point(0, 0);
            _scale = 1;
            _ancestorBounds = Rect.Zero;
            _pagePosition = Point.Zero;
            _targetChildForPagePosition = null;
            _point = Point.Zero;
#endif
        }

        /// <summary>
        /// Handles the long-press gesture and resets the interaction state.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The long press event data.</param>
        void OnPanZoomListenerLongPress(object? sender, LongPressEventArgs e)
        {
#if ANDROID
            // Android-specific gesture handling: verify content is available before processing
            if (Children.Count > 0)
            {
                OnLongPress(Children[0] as View, e);
            }

            // Reset interaction state after gesture completion
            _location = new Point(0, 0);
            _scale = 1;
            _ancestorBounds = Rect.Zero;
            _pagePosition = Point.Zero;
            _targetChildForPagePosition = null;
            _point = Point.Zero;
#endif
        }

        /// <summary>
        /// Handles the tap gesture and resets the interaction state.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The tap event data.</param>
        void OnPanZoomListenerTapped(object? sender, TapEventArgs e)
        {
            Tapped?.Invoke(this, e);
#if ANDROID
            // Android-specific gesture handling: verify content is available before processing
            if (Children.Count > 0)
            {
                OnTap(Children[0] as View, e);
            }

            // Reset interaction state after gesture completion
            _location = new Point(0, 0);
            _scale = 1;
            _ancestorBounds = Rect.Zero;
            _pagePosition = Point.Zero;
            _targetChildForPagePosition = null;
            _point = Point.Zero;
#endif
        }

        /// <summary>
        /// Handles the touch gesture and resets the interaction state.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The pointer event data.</param>
        void OnPanZoomListenerTouch(object? sender, PointerEventArgs e)
        {
#if ANDROID
            // Android-specific gesture handling: verify content is available before processing
            if (Children.Count > 0)
            {
                OnTouch(Children[0] as View, e);
            }

            // Reset interaction state after gesture completion
            _location = new Point(0, 0);
            _scale = 1;
            _ancestorBounds = Rect.Zero;
            _pagePosition = Point.Zero;
            _targetChildForPagePosition = null;
            _point = Point.Zero;
#endif
        }

#if ANDROID

        /// <summary>
        /// Method to get the bounds scaled around its center point.
        /// </summary>
        /// <param name="bounds">The bounds to scale.</param>
        /// <param name="scale">The scale factor.</param>
        /// <returns>A scaled rectangle.</returns>
        Rect GetScaledRectangle(Rect bounds, double scale)
        {
            double centerX = bounds.X + bounds.Width / 2f;
            double centerY = bounds.Y + bounds.Height / 2f;
            double newWidth = bounds.Width * scale;
            double newHeight = bounds.Height * scale;
            double newX = centerX - newWidth / 2f;
            double newY = centerY - newHeight / 2f;
            return new Rect(newX, newY, newWidth, newHeight);
        }

        /// <summary>
        /// Method to get the page position for the specified listener type.
        /// </summary>
        (Point pagePosition, IView? targetChild) TryGetPagePositionWithChild<T>(SfView targetView, Point tapPoint, double currentScale, Point currentLocation, Rect currentAncestorBounds) where T : class
        {
            currentScale *= targetView.Scale;
            if (targetView != Children[0])
            {
                currentLocation = new Point(currentLocation.X + targetView.Bounds.X, currentLocation.Y + targetView.Bounds.Y);
                if (currentScale < 1 && currentLocation.Y < 0)
                    currentLocation.Y = 0;
            }

            if (currentAncestorBounds == Rect.Zero)
                currentAncestorBounds = targetView.Bounds;

            for (int i = targetView.Children.Count - 1; i >= 0; i--)
            {
                IView child = targetView.Children[i];
                Rect scaledBounds = GetScaledRectangle(currentAncestorBounds, currentScale);
                Rect childBoundsUnscaled = new Rect(child.Frame.X, child.Frame.Y, child.Frame.Width, child.Frame.Height);

                Point scaledPoint = new Point(
                    (tapPoint.X - scaledBounds.X - (currentLocation.X * currentScale)) / currentScale,
                    (tapPoint.Y - scaledBounds.Y - (currentLocation.Y * currentScale)) / currentScale);

                // Check if child implements the required listener interface
                if (child is T && childBoundsUnscaled.Contains(scaledPoint))
                {
                    Point pagePos = new Point(scaledPoint.X - child.Frame.X, scaledPoint.Y - child.Frame.Y);
                    return (pagePos, child);
                }

                // Recursive search for nested SfViews
                if (child is SfView childSfView)
                {
                    var nestedResult = TryGetPagePositionWithChild<T>(childSfView, tapPoint, currentScale, currentLocation, currentAncestorBounds);
                    if (nestedResult.pagePosition != Point.Zero)
                    {
                        return nestedResult;
                    }
                }
            }

            return (Point.Zero, null);
        }

        /// <summary>
        /// Handles tap gestures for child views.
        /// </summary>
        /// <param name="view">The view to inspect.</param>
        /// <param name="e">The tap event data.</param>
        void OnTap(View? view, TapEventArgs e)
        {
            if (view is Layout layout)
            {
                _scale *= layout.Scale;
                if (layout != Children[0] || _scale <= 1)
                    _location = new Point(_location.X + layout.Bounds.X, _location.Y + layout.Bounds.Y);

                for (int i = layout.Children.Count - 1; i >= 0; i--)
                {
                    IView child = layout.Children[i];
                    Rect childBounds = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                    Point scaledPoint = new Point(e.TapPoint.X / _scale, e.TapPoint.Y / _scale);
                    if (child is ITapGestureListener listener && childBounds.Contains(scaledPoint))
                    {
                        Point adjustedTapPoint = new Point(e.TapPoint.X - _location.X * _scale, e.TapPoint.Y - _location.Y * _scale);
                        TapEventArgs tapEventArgs = new TapEventArgs(adjustedTapPoint, 1);
                        listener.OnTap(tapEventArgs);
                        break;
                    }

                    if (child is Layout || child is SfView || child is ContentView)
                    {
                        OnTap(child as View, e);
                    }
                }

                if (layout != Children[0] || _scale <= 1)
                    _location = new Point(_location.X - layout.Bounds.X, _location.Y - layout.Bounds.Y);

                _scale /= layout.Scale;
            }
            else if (view is SfView targetView)
            {
                if (targetView == Children[0])
                {
                    var result = TryGetPagePositionWithChild<ITapGestureListener>(targetView, e.TapPoint, _scale, _location, _ancestorBounds);
                    _pagePosition = result.pagePosition;
                    _targetChildForPagePosition = result.targetChild;
                }

                _scale *= targetView.Scale;
                if (targetView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X + targetView.Bounds.X, _location.Y + targetView.Bounds.Y);
                }

                for (int i = targetView.Children.Count - 1; i >= 0; i--)
                {
                    IView child = targetView.Children[i];

                    // Old calculation
                    Rect childBoundsOld = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                    Point scaledPointOld = new Point(e.TapPoint.X / _scale, e.TapPoint.Y / _scale);
                    bool oldContains = childBoundsOld.Contains(scaledPointOld);

                    // New calculation - only true if THIS child is the target
                    bool newContains = _pagePosition != Point.Zero && _targetChildForPagePosition != null &&
                                       ReferenceEquals(child, _targetChildForPagePosition);

                    if (child is ITapGestureListener listener && (oldContains || newContains))
                    {
                        TapEventArgs tapEventArgs;

                        if (oldContains && newContains)
                        {
                            Point adjustedTapPoint = new Point(e.TapPoint.X - _location.X * _scale, e.TapPoint.Y - _location.Y * _scale);
                            _point = adjustedTapPoint;
                            tapEventArgs = new TapEventArgs(adjustedTapPoint, 1, _pagePosition);
                        }
                        else if (oldContains && !newContains)
                        {
                            Point adjustedTapPoint = new Point(e.TapPoint.X - _location.X * _scale, e.TapPoint.Y - _location.Y * _scale);
                            _point = adjustedTapPoint;
                            tapEventArgs = new TapEventArgs(adjustedTapPoint, 1);
                        }
                        else
                        {
                            if (_point != Point.Zero)
                                tapEventArgs = new TapEventArgs(_point, 1, _pagePosition);
                            else
                                tapEventArgs = new TapEventArgs(Point.Zero, 1, _pagePosition);
                        }

                        listener.OnTap(tapEventArgs);
                        break;
                    }

                    if (child is Layout || child is SfView || child is ContentView)
                    {
                        OnTap(child as View, e);
                    }
                }

                if (targetView != Children[0] || _scale <= 1)
                    _location = new Point(_location.X - targetView.Bounds.X, _location.Y - targetView.Bounds.Y);

                _scale /= targetView.Scale;
            }
            else if (view is ContentView contentView)
            {
                _scale *= contentView.Scale;
                if (contentView != Children[0] || _scale <= 1)
                    _location = new Point(_location.X + contentView.Bounds.X, _location.Y + contentView.Bounds.Y);

                IView child = contentView.Content;
                Rect childBounds = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                Point scaledPoint = new Point(e.TapPoint.X / _scale, e.TapPoint.Y / _scale);
                if (child is ITapGestureListener listener && childBounds.Contains(scaledPoint))
                {
                    Point adjustedTapPoint = new Point(e.TapPoint.X - _location.X * _scale, e.TapPoint.Y - _location.Y * _scale);
                    TapEventArgs tapEventArgs = new TapEventArgs(adjustedTapPoint, 1);
                    listener.OnTap(tapEventArgs);
                }
                else if (child is Layout || child is SfView || child is ContentView)
                {
                    OnTap(child as View, e);
                }

                if (contentView != Children[0] || _scale <= 1)
                    _location = new Point(_location.X - contentView.Bounds.X, _location.Y - contentView.Bounds.Y);

                _scale /= contentView.Scale;
            }
        }

        /// <summary>
        /// Handles touch gestures for child views.
        /// </summary>
        /// <param name="view">The view to inspect.</param>
        /// <param name="e">The pointer event data.</param>
        void OnTouch(View? view, PointerEventArgs e)
        {
            if (view is Layout layout)
            {
                _scale *= layout.Scale;
                if (layout != Children[0] || _scale <= 1)
                    _location = new Point(_location.X + layout.Bounds.X, _location.Y + layout.Bounds.Y);

                for (int i = layout.Children.Count - 1; i >= 0; i--)
                {
                    IView child = layout.Children[i];
                    Rect childBounds = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                    Point scaledPoint = new Point(e.TouchPoint.X / _scale, e.TouchPoint.Y / _scale);
                    if (child is ITouchListener listener && childBounds.Contains(scaledPoint))
                    {
                        Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                        PointerEventArgs pointerEventArgs = new PointerEventArgs(e.Id, e.Action, adjustedTouchPoint);
                        listener.OnTouch(pointerEventArgs);
                        break;
                    }

                    if (child is Layout || child is SfView || child is ContentView)
                    {
                        OnTouch(child as View, e);
                    }
                }

                if (layout != Children[0] || _scale <= 1)
                    _location = new Point(_location.X - layout.Bounds.X, _location.Y - layout.Bounds.Y);

                _scale /= layout.Scale;
            }
            else if (view is SfView targetView)
            {
                // Calculate page position BEFORE modifying scale/location
                if (targetView == Children[0])
                {
                    var result = TryGetPagePositionWithChild<ITouchListener>(targetView, e.TouchPoint, _scale, _location, _ancestorBounds);
                    _pagePosition = result.pagePosition;
                    _targetChildForPagePosition = result.targetChild;
                }

                _scale *= targetView.Scale;
                if (targetView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X + targetView.Bounds.X, _location.Y + targetView.Bounds.Y);
                }

                for (int i = targetView.Children.Count - 1; i >= 0; i--)
                {
                    IView child = targetView.Children[i];

                    // Old calculation
                    Rect childBoundsOld = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                    Point scaledPointOld = new Point(e.TouchPoint.X / _scale, e.TouchPoint.Y / _scale);
                    bool oldContains = childBoundsOld.Contains(scaledPointOld);

                    // New calculation - only true if THIS child is the target
                    bool newContains = _pagePosition != Point.Zero &&
                                       _targetChildForPagePosition != null &&
                                       ReferenceEquals(child, _targetChildForPagePosition);

                    if (child is ITouchListener listener && (oldContains || newContains))
                    {
                        PointerEventArgs pointerEventArgs;

                        if (oldContains && newContains)
                        {
                            Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                            _point = adjustedTouchPoint;
                            pointerEventArgs = new PointerEventArgs(e.Id, e.Action, adjustedTouchPoint, _pagePosition);
                        }
                        else if (oldContains && !newContains)
                        {
                            Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                            _point = adjustedTouchPoint;
                            pointerEventArgs = new PointerEventArgs(e.Id, e.Action, adjustedTouchPoint);
                        }
                        else
                        {
                            if (_point != Point.Zero)
                            {
                                pointerEventArgs = new PointerEventArgs(e.Id, e.Action, _point, _pagePosition);
                                return;
                            }

                            pointerEventArgs = new PointerEventArgs(e.Id, e.Action, Point.Zero, _pagePosition);
                        }

                        listener.OnTouch(pointerEventArgs);
                        break;
                    }

                    if (child is Layout || child is SfView || child is ContentView)
                    {
                        OnTouch(child as View, e);
                    }
                }

                if (targetView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X - targetView.Bounds.X, _location.Y - targetView.Bounds.Y);
                }

                _scale /= targetView.Scale;
            }
            else if (view is ContentView contentView)
            {
                _scale *= contentView.Scale;
                if (contentView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X + contentView.Bounds.X, _location.Y + contentView.Bounds.Y);
                }

                IView child = contentView.Content;
                Rect childBounds = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                Point scaledPoint = new Point(e.TouchPoint.X / _scale, e.TouchPoint.Y / _scale);
                if (child is ITouchListener listener && childBounds.Contains(scaledPoint))
                {
                    Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                    PointerEventArgs pointerEventArgs = new PointerEventArgs(e.Id, e.Action, adjustedTouchPoint);
                    listener.OnTouch(pointerEventArgs);
                }
                else if (child is Layout || child is SfView || child is ContentView)
                {
                    OnTouch(child as View, e);
                }

                if (contentView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X - contentView.Bounds.X, _location.Y - contentView.Bounds.Y);
                }

                _scale /= contentView.Scale;
            }
        }

        /// <summary>
        /// Handles long-press gestures for child views.
        /// </summary>
        /// <param name="view">The view to inspect.</param>
        /// <param name="e">The long press event data.</param>
        void OnLongPress(View? view, LongPressEventArgs e)
        {
            if (view is Layout layout)
            {
                _scale *= layout.Scale;
                if (layout != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X + layout.Bounds.X, _location.Y + layout.Bounds.Y);
                }

                for (int i = layout.Children.Count - 1; i >= 0; i--)
                {
                    IView child = layout.Children[i];
                    Rect childBounds = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                    Point scaledPoint = new Point(e.TouchPoint.X / _scale, e.TouchPoint.Y / _scale);
                    if (child is ILongPressGestureListener listener && childBounds.Contains(scaledPoint))
                    {
                        Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                        LongPressEventArgs pointerEventArgs = new LongPressEventArgs(adjustedTouchPoint);
                        listener.OnLongPress(pointerEventArgs);
                        break;
                    }

                    if (child is Layout || child is SfView || child is ContentView)
                    {
                        OnLongPress(child as View, e);
                    }
                }

                if (layout != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X - layout.Bounds.X, _location.Y - layout.Bounds.Y);
                }

                _scale /= layout.Scale;
            }
            else if (view is SfView targetView)
            {
                // Calculate page position before modifying scale/location
                if (targetView == Children[0])
                {
                    var result = TryGetPagePositionWithChild<ILongPressGestureListener>(targetView, e.TouchPoint, _scale, _location, _ancestorBounds);
                    _pagePosition = result.pagePosition;
                    _targetChildForPagePosition = result.targetChild;
                }

                _scale *= targetView.Scale;
                if (targetView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X + targetView.Bounds.X, _location.Y + targetView.Bounds.Y);
                }

                for (int i = targetView.Children.Count - 1; i >= 0; i--)
                {
                    IView child = targetView.Children[i];

                    // Old calculation
                    Rect childBoundsOld = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                    Point scaledPointOld = new Point(e.TouchPoint.X / _scale, e.TouchPoint.Y / _scale);
                    bool oldContains = childBoundsOld.Contains(scaledPointOld);

                    // New calculation - only true if THIS child is the target
                    bool newContains = _pagePosition != Point.Zero &&
                                       _targetChildForPagePosition != null &&
                                       ReferenceEquals(child, _targetChildForPagePosition);

                    if (child is ILongPressGestureListener listener && (oldContains || newContains))
                    {
                        LongPressEventArgs pointerEventArgs;

                        if (oldContains && newContains)
                        {
                            Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                            _point = adjustedTouchPoint;
                            pointerEventArgs = new LongPressEventArgs(adjustedTouchPoint, _pagePosition);
                        }
                        else if (oldContains && !newContains)
                        {
                            Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                            _point = adjustedTouchPoint;
                            pointerEventArgs = new LongPressEventArgs(adjustedTouchPoint);
                            continue;
                        }
                        else
                        {
                            if (_point != Point.Zero)
                            {
                                pointerEventArgs = new LongPressEventArgs(_point, _pagePosition);
                                return;
                            }

                            pointerEventArgs = new LongPressEventArgs(Point.Zero, _pagePosition);
                        }

                        listener.OnLongPress(pointerEventArgs);
                        break;
                    }

                    if (child is Layout || child is SfView || child is ContentView)
                    {
                        OnLongPress(child as View, e);
                    }
                }

                if (targetView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X - targetView.Bounds.X, _location.Y - targetView.Bounds.Y);
                }

                _scale /= targetView.Scale;
            }
            else if (view is ContentView contentView)
            {
                _scale *= contentView.Scale;
                if (contentView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X + contentView.Bounds.X, _location.Y + contentView.Bounds.Y);
                }

                IView child = contentView.Content;
                Rect childBounds = new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
                Point scaledPoint = new Point(e.TouchPoint.X / _scale, e.TouchPoint.Y / _scale);
                if (child is ILongPressGestureListener listener && childBounds.Contains(scaledPoint))
                {
                    Point adjustedTouchPoint = new Point(e.TouchPoint.X - _location.X * _scale, e.TouchPoint.Y - _location.Y * _scale);
                    LongPressEventArgs pointerEventArgs = new LongPressEventArgs(adjustedTouchPoint);
                    listener.OnLongPress(pointerEventArgs);
                }
                else if (child is Layout || child is SfView || child is ContentView)
                {
                    OnLongPress(child as View, e);
                }

                if (contentView != Children[0] || _scale <= 1)
                {
                    _location = new Point(_location.X - contentView.Bounds.X, _location.Y - contentView.Bounds.Y);
                }

                _scale /= contentView.Scale;
            }
        }

        /// <summary>
        /// Handles double-tap gestures for child views.
        /// </summary>
        /// <param name="view">The view to inspect.</param>
        /// <param name="e">The tap event data.</param>
        void OnDoubleTap(View? view, TapEventArgs e)
        {
            switch (view)
            {
                case Layout layout:
                    HandleLayoutDoubleTap(layout, e);
                    break;

                case SfView sfView:
                    HandleSfViewDoubleTap(sfView, e);
                    break;

                case ContentView contentView:
                    HandleContentViewDoubleTap(contentView, e);
                    break;
            }
        }

        /// <summary>
        /// Handles double-tap gestures for child views.
        /// </summary>
        /// <param name="layout">The layout to inspect.</param>
        /// <param name="e">The double-tap event data.</param>
        void HandleLayoutDoubleTap(Layout layout, TapEventArgs e)
        {
            EnterViewScope(layout);
            for (int i = layout.Children.Count - 1; i >= 0; i--)
            {
                IView child = layout.Children[i];
                Rect childBounds = GetChildBounds(child);
                Point scaledPoint = GetScaledPoint(e.TapPoint);
                if (child is IDoubleTapGestureListener listener && childBounds.Contains(scaledPoint))
                {
                    listener.OnDoubleTap(CreateTapEventArgs(e.TapPoint));
                    break;
                }

                ProcessNestedDoubleTap(child, e);
            }
        }

        /// <summary>
        /// Routes a double-tap gesture to the appropriate child listener in the specified <see cref="SfView"/> 
        /// </summary>
        /// <param name="view">The view to inspect.</param>
        /// <param name="e">The double-tap event data.</param>
        void HandleSfViewDoubleTap(SfView view, TapEventArgs e)
        {
            InitializeDoubleTapPagePosition(view, e);
            EnterViewScope(view);
            for (int i = view.Children.Count - 1; i >= 0; i--)
            {
                IView child = view.Children[i];
                if (child is IDoubleTapGestureListener listener && TryCreateDoubleTapArgs(child, e, out TapEventArgs? tapArgs)
                    && tapArgs is not null)
                {
                    listener.OnDoubleTap(tapArgs);
                    break;
                }

                ProcessNestedDoubleTap(child, e);
            }
        }

        /// <summary>
        /// Routes a double-tap gesture to the content of the specified <see cref="ContentView"/>.
        /// </summary>
        /// <param name="contentView">The content view to inspect.</param>
        /// <param name="e">The double-tap event data.</param>
        void HandleContentViewDoubleTap(ContentView contentView, TapEventArgs e)
        {
            EnterViewScope(contentView);
            IView child = contentView.Content;

            Rect childBounds = GetChildBounds(child);
            Point scaledPoint = GetScaledPoint(e.TapPoint);

            if (child is IDoubleTapGestureListener listener && childBounds.Contains(scaledPoint))
            {
                listener.OnDoubleTap(CreateTapEventArgs(e.TapPoint));
                return;
            }

            ProcessNestedDoubleTap(child, e);
        }

        /// <summary>
        /// Updates the gesture scope for the specified view.
        /// </summary>
        /// <param name="view">The view being entered.</param>
        void EnterViewScope(View view)
        {
            _scale *= view.Scale;
            if (view != Children[0] || _scale <= 1)
            {
                _location = new Point(_location.X + view.Bounds.X, _location.Y + view.Bounds.Y);
            }
        }

        /// <summary>
        /// Gets the bounds of the specified child view.
        /// </summary>
        /// <param name="child">The child view.</param>
        /// <returns>The child view bounds.</returns>
        Rect GetChildBounds(IView child)
        {
            return new Rect(child.Frame.X + _location.X, child.Frame.Y + _location.Y, child.Frame.Width, child.Frame.Height);
        }

        /// <summary>
        /// Gets the tap location adjusted for the current scale.
        /// </summary>
        /// <param name="tapPoint">The tap location.</param>
        /// <returns>The scaled tap location.</returns>
        Point GetScaledPoint(Point tapPoint)
        {
            return new Point(tapPoint.X / _scale, tapPoint.Y / _scale);
        }

        /// <summary>
        /// Creates tap event data with a location adjusted for the current scale and offset.
        /// </summary>
        /// <param name="tapPoint">The tap location.</param>
        /// <returns>The generated tap event data.</returns>
        TapEventArgs CreateTapEventArgs(Point tapPoint)
        {
            Point adjustedTapPoint = new Point(tapPoint.X - _location.X * _scale, tapPoint.Y - _location.Y * _scale);
            return new TapEventArgs(adjustedTapPoint, 1);
        }

        /// <summary>
        /// Processes double-tap gestures for nested container views.
        /// </summary>
        /// <param name="child">The child view to inspect.</param>
        /// <param name="e">The double-tap event data.</param>
        void ProcessNestedDoubleTap(IView child, TapEventArgs e)
        {
            if (child is Layout || child is SfView || child is ContentView)
            {
                OnDoubleTap(child as View, e);
            }
        }

        /// <summary>
        /// Initializes the page position for a double-tap gesture.
        /// </summary>
        /// <param name="view">The view to inspect.</param>
        /// <param name="e">The double-tap event data.</param>
        void InitializeDoubleTapPagePosition(SfView view, TapEventArgs e)
        {
            if (view != Children[0])
            {
                return;
            }

            var result = TryGetPagePositionWithChild<IDoubleTapGestureListener>(view, e.TapPoint, _scale, _location, _ancestorBounds);

            _pagePosition = result.pagePosition;
            _targetChildForPagePosition = result.targetChild;
        }

        /// <summary>
        /// Creates double-tap event data for the specified child view.
        /// </summary>
        /// <param name="child">The child view.</param>
        /// <param name="e">The double-tap event data.</param>
        /// <param name="tapEventArgs">The generated double-tap event data, if successful.</param>
        /// <returns>
        /// <see langword="true"/> if the event data was created; otherwise, <see langword="false"/>.
        /// </returns>
        bool TryCreateDoubleTapArgs(IView child, TapEventArgs e, out TapEventArgs? tapEventArgs)
        {
            Rect childBounds = GetChildBounds(child);
            Point scaledPoint = GetScaledPoint(e.TapPoint);

            bool oldContains = childBounds.Contains(scaledPoint);
            bool newContains = _pagePosition != Point.Zero && _targetChildForPagePosition != null && ReferenceEquals(child, _targetChildForPagePosition);
            tapEventArgs = null;
            if (!oldContains && !newContains)
            {
                return false;
            }

            Point adjustedTapPoint = new Point(e.TapPoint.X - _location.X * _scale, e.TapPoint.Y - _location.Y * _scale);
            if (oldContains && newContains)
            {
                _point = adjustedTapPoint;
                tapEventArgs = new TapEventArgs(adjustedTapPoint, 1, _pagePosition);
            }
            else if (oldContains)
            {
                _point = adjustedTapPoint;
                tapEventArgs = new TapEventArgs(adjustedTapPoint, 1);
            }
            else
            {
                tapEventArgs = _point != Point.Zero
                    ? new TapEventArgs(_point, 1, _pagePosition)
                    : new TapEventArgs(Point.Zero, 1, _pagePosition);
            }

            return true;
        }
#endif

        /// <summary>
        /// Handles child removal and cleans up event subscriptions.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The element event data.</param>
        void OnChildRemoved(object? sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= OnChildPropertyChanged;
        }

        /// <summary>
        /// Handles layout-related property changes for child elements.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The property change event data.</param>
        void OnChildPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(HorizontalOptions):
                case nameof(VerticalOptions):
                    UpdateLayoutContent();
                    break;
            }
        }

        /// <summary>
        /// Handles child addition and registers required event handlers.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The element event data.</param>
        void OnChildAdded(object? sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged += OnChildPropertyChanged;
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Handles child addition and registers event handlers.
        /// </summary>
        ~ContentPlaceHolder()
        {
            this.ChildAdded -= OnChildAdded;
            this.ChildRemoved -= OnChildRemoved;
        }

        #endregion
    }
}