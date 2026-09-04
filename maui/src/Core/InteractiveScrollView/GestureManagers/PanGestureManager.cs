namespace Syncfusion.Maui.Toolkit.Internals
{
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;

    /// <summary>
    /// Represents pan gesture interactions logic for <see cref="SfInteractiveScrollView"/> and translates them into scrolling operations.
    /// </summary>
    /// <remarks>
    /// Pan-based scrolling is implemented for Windows platforms. On Android and iOS, scrolling interactions are handled by the corresponding native scroll views.
    /// </remarks>
    internal class PanGestureManager
    {
        #region Fields

        /// <summary>
        /// Stores the content translation at the start of a pan operation.
        /// </summary>
        Point? _translationPositionAtStart = null;

        /// <summary>
        /// Stores the accumulated translation during a pan operation.
        /// </summary>
        Point _totalTranslatedPosition = Point.Zero;

        /// <summary>
        /// Stores the scroll offset at the start of a pan operation.
        /// </summary>
        Point _scrollOffsetAtStart = Point.Zero;

        /// <summary>
        /// Holds the pan gesture listener.
        /// </summary>
        PanZoomListener _panListener;

        /// <summary>
        /// Holds the associated interactive scroll view.
        /// </summary>
        SfInteractiveScrollView _scrollView;

        #endregion

        #region Construtor

        /// <summary>
        /// Initializes a new instance of the <see cref="PanGestureManager"/> class.
        /// </summary>
        /// <param name="scrollView">The interactive scroll view.</param>
        /// <param name="panListener">The pan gesture listener.</param>
        internal PanGestureManager(SfInteractiveScrollView scrollView, PanZoomListener panListener)
        {
            _scrollView = scrollView;
            _panListener = panListener;
            WireEvents();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Handles pan update events and performs scrolling operations.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The pan event arguments.</param>
        internal void OnPanUpdated(object? sender, PanEventArgs e)
        {
            if (_scrollView == null)
                return;

            View? content = _scrollView.Content;
            if (content != null)
            {
                if (e.Status == GestureStatus.Started && !_scrollView.IsScrolling)
                {
                    _translationPositionAtStart = new Point(content.TranslationX, content.TranslationY);
                    _scrollOffsetAtStart.X = _scrollView.ScrollX;
                    _scrollOffsetAtStart.Y = _scrollView.ScrollY;
                }

                if (e.Status == GestureStatus.Running && _translationPositionAtStart != null)
                {
                    _totalTranslatedPosition.X += e.TranslatePoint.X;
                    _totalTranslatedPosition.Y += e.TranslatePoint.Y;
                    if (!_scrollView.IsScrolling)
                    {
                        if (e.TranslatePoint.X != 0 || e.TranslatePoint.Y != 0)
                        {
                            if (_scrollView.ContentSize.Width >= _scrollView.Width)
                                content.TranslationX = _translationPositionAtStart.Value.X +
                                System.Math.Clamp(_totalTranslatedPosition.X, _scrollOffsetAtStart.X + _scrollView.Width - _scrollView.ContentSize.Width,
                                _scrollOffsetAtStart.X);

                            if (_scrollView.ContentSize.Height >= _scrollView.Height)
                                content.TranslationY = _translationPositionAtStart.Value.Y +
                                System.Math.Clamp(_totalTranslatedPosition.Y, _scrollOffsetAtStart.Y + _scrollView.Height - _scrollView.ContentSize.Height,
                                _scrollOffsetAtStart.Y);

                            ScrollChangedEventArgs eventArgs = new ScrollChangedEventArgs(
                                _scrollOffsetAtStart.X + (_translationPositionAtStart.Value.X - content.TranslationX),
                                _scrollOffsetAtStart.Y + (_translationPositionAtStart.Value.Y - content.TranslationY),
                                _scrollView.ScrollX, _scrollView.ScrollY);
                            _scrollView.OnScrollChanged(eventArgs);
                        }
                    }
                }

                if (e.Status == GestureStatus.Completed && _translationPositionAtStart != null)
                {
                    _scrollView.ScrollTo(_scrollOffsetAtStart.X + (_translationPositionAtStart.Value.X - content.TranslationX),
                        _scrollOffsetAtStart.Y + (_translationPositionAtStart.Value.Y - content.TranslationY), false);
                    content.TranslationX = _translationPositionAtStart.Value.X;
                    content.TranslationY = _translationPositionAtStart.Value.Y;
                    ResetValues();
                }

                if (e.Status == GestureStatus.Canceled && _translationPositionAtStart != null)
                {
                    if (_translationPositionAtStart.Value.Distance(new Point(content.TranslationX, content.TranslationY)) != 0)
                    {
                        _scrollView.ScrollTo(_scrollOffsetAtStart.X + (_translationPositionAtStart.Value.X - content.TranslationX),
                            _scrollOffsetAtStart.Y + (_translationPositionAtStart.Value.Y - content.TranslationY), false);
                        content.TranslationX = _translationPositionAtStart.Value.X;
                        content.TranslationY = _translationPositionAtStart.Value.Y;
                    }

                    ResetValues();
                }
            }
        }

        /// <summary>
        /// Resets the pan gesture tracking values.
        /// </summary>
        internal void ResetValues()
        {
            _translationPositionAtStart = null;
            _totalTranslatedPosition = Point.Zero;
            _scrollOffsetAtStart = Point.Zero;
        }

        /// <summary>
        /// Removes subscribed pan gesture event handlers.
        /// </summary>
        internal void Dispose()
        {
            UnWireEvents();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles pan update events and performs scrolling operations.
        /// </summary>
        void WireEvents()
        {
            _panListener.PanUpdated += OnPanUpdated;
        }

        /// <summary>
        /// Unsubscribes from pan events.
        /// </summary>
        void UnWireEvents()
        {
            _panListener.PanUpdated -= OnPanUpdated;
        }

        #endregion
    }
}