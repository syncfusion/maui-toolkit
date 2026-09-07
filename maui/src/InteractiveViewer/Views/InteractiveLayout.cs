namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
    using Syncfusion.Maui.Toolkit.Internals;

    /// <summary>
    /// Represents a layout that hosts and displays content within the <see cref="SfInteractiveViewer"/>.
    /// </summary>
    internal class InteractiveLayout : InteractiveBaseLayout
    {
        #region Fields

        /// <summary>
        /// The interactive viewer info.
        /// </summary>
        readonly IInteractiveViewerInfo? _interactiveViewerInfo;

        /// <summary>
        /// Stores the content size before zooming.
        /// </summary>
        Size _initialContentSize = Size.Zero;

        /// <summary>
        /// Holds the content view displayed within the interactive viewer.
        /// </summary>
        View? _contentView;

        /// <summary>
        /// Stores the horizontal alignment ratio used for viewport centering.
        /// </summary>
        double _horizontalAlignmentRatio = 0.5;

        /// <summary>
        /// Stores the vertical alignment ratio used for viewport centering.
        /// </summary>
        double _verticalAlignmentRatio = 0.5;

        /// <summary>
        /// Stores the initial horizontal zoom factor.
        /// </summary>
        double _initialZoomFactorX = 1;

        /// <summary>
        /// Stores the initial vertical zoom factor.
        /// </summary>
        double _initialZoomFactorY = 1;

        /// <summary>
        /// Stores the zoom origin in viewport coordinates.
        /// </summary>
        Point _zoomOrigin = Point.Zero;

        /// <summary>
        /// Stores the available size of the interactive layout.    
        /// </summary>
        Size _availableSize = Size.Zero;

        /// <summary>
        /// Stores the measured content size.
        /// </summary>
        Size _contentSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveLayout"/> class with the specified interactive viewer information.
        /// </summary>
        /// <param name="interactiveViewerInfo">The <see cref="IInteractiveViewerInfo"/> providing information about the interactive viewer and its content.</param>
        internal InteractiveLayout(IInteractiveViewerInfo? interactiveViewerInfo)
        {
            this._interactiveViewerInfo = interactiveViewerInfo;
            this.InitializeItemsView();
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Method to update the content displayed in the interactive layout.
        /// </summary>
        /// <param name="contentView">The view to be displayed as the content of the interactive layout.</param>
        internal void UpdateContentView(View contentView)
        {
            this.ResetView();
            Remove(_contentView);

            _contentView = contentView;
            _contentView.HorizontalOptions = LayoutOptions.Center;
            _contentView.VerticalOptions = LayoutOptions.Center;
            Add(_contentView);
        }

        /// <summary>
        /// Rotates the interactive view by 90 degrees clockwise.
        /// </summary>
        internal async Task RotateView()
        {
            ResetZoom();
            RotateView(90);
        }

        /// <summary>
        /// Method to update the available size of the interactive layout.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        internal void UpdateAvailableSize(Size availableSize)
        {
            if (_availableSize == availableSize)
            {
                return;
            }

            _availableSize = availableSize;
        }

        /// <summary>
        /// Removes the handler associated with the content view of the interactive layout.
        /// </summary>
        internal void RemoveItemsViewHandler()
        {
            foreach (var child in this.Children)
            {
                if (child is View layout)
                {
                    layout.Handler?.DisconnectHandler();
                }
            }

            Children.Clear();
            _contentView = null;
        }

        /// <summary>
        /// Processes the zoom started event.
        /// </summary>
        /// <param name="e">The zoom event arguments.</param>
        internal void ProcessOnZoomStarted(ZoomEventArgs e)
        {
            if (_contentView == null)
            {
                return;
            }

            UpdateContentSize(GetContentSize());

            AnchorX = this.GetDesiredAnchorX(e.ZoomFactor, e.ZoomOrigin.X);
            TranslationX = this.GetDesiredTranslationX(this.AnchorX, this.Scale);
            AnchorY = this.GetDesiredAnchorY(e.ZoomFactor, e.ZoomOrigin.Y);
            TranslationY = GetDesiredTranslationY(this.AnchorY, this.Scale);
        }

        /// <summary>
        /// Processes the zoom changed event.
        /// </summary>
        /// <param name="e">The zoom event arguments.</param>
        internal void ProcessOnZoomChanged(ZoomEventArgs e)
        {
            if (_contentView == null || _interactiveViewerInfo == null)
            {
                return;
            }

            Size scrollViewSize = _interactiveViewerInfo.GetScrollViewDesiredSize();
            if (AnchorX == _horizontalAlignmentRatio && _initialContentSize.Width > 0 &&
                _initialContentSize.Width * e.ZoomFactor > scrollViewSize.Width)
            {
                AnchorX = GetDesiredAnchorX(e.ZoomFactor, e.ZoomOrigin.X +
                    (((_initialContentSize.Width * Scale) - scrollViewSize.Width) * _horizontalAlignmentRatio * 2));
                TranslationX = GetDesiredTranslationX(AnchorX, Scale);
            }

            if (AnchorY == _verticalAlignmentRatio && _initialContentSize.Height > 0 &&
                _initialContentSize.Height * e.ZoomFactor > scrollViewSize.Height)
            {
                AnchorY = GetDesiredAnchorY(e.ZoomFactor, e.ZoomOrigin.Y +
                    (((_initialContentSize.Height * Scale) - scrollViewSize.Height) * _verticalAlignmentRatio * 2));
                TranslationY = GetDesiredTranslationY(AnchorY, Scale);
            }

            Scale = e.ZoomFactor;
        }

        /// <summary>
        /// Processes the zoom ended event.
        /// </summary>
        /// <param name="e">The zoom event arguments.</param>
        internal void ProcessOnZoomEnded(ZoomEventArgs e)
        {
            if (_contentView == null || _interactiveViewerInfo == null)
            {
                return;
            }

            double xZoomChange = e.ZoomFactor / _initialZoomFactorX;
            double yZoomChange = e.ZoomFactor / _initialZoomFactorY;

            double scrollX = _interactiveViewerInfo.GetScrollX();
            double scrollY = _interactiveViewerInfo.GetScrollY();

            double xOffset = Math.Max(0, (_zoomOrigin.X * (xZoomChange - 1)) + scrollX);
            double yOffset = Math.Max(0, (_zoomOrigin.Y * (yZoomChange - 1)) + scrollY);

            double width = (_initialContentSize.Width + 10) * e.ZoomFactor;
            double height = (_initialContentSize.Height + 10) * e.ZoomFactor;

            _interactiveViewerInfo.UpdateContentSize(new Size(width, height), false);
            TranslationX = GetDesiredTranslationX(AnchorX, Scale);
            TranslationY = GetDesiredTranslationY(AnchorY, Scale);

            if (xOffset != scrollX || yOffset != scrollY)
            {
                _interactiveViewerInfo.ScrollToAsync(xOffset, yOffset);
            }
        }

        /// <summary>
        /// Processes size change updates for the interactive viewer.
        /// </summary>
        internal void ProcessOnSizeChanged()
        {
            InvalidateMeasureContent();
            ResetZoom();
        }

        /// <summary>
        /// Method to reset the interactive view to its default state.
        /// </summary>
        internal void ResetView()
        {
            ResetZoom();
            this.InvalidateMeasure();
            if (_contentView == null)
                return;

            _contentView.RotationX = 0;
            _contentView.RotationY = 0;
            _contentView.Rotation = 0;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the view and its content.
        /// </summary>
        void InitializeItemsView()
        {
            if (this._interactiveViewerInfo == null ||
                this._interactiveViewerInfo.Content == null)
            {
                return;
            }

            _contentView = _interactiveViewerInfo.Content;
            _contentView.HorizontalOptions = LayoutOptions.Center;
            _contentView.VerticalOptions = LayoutOptions.Center;
            Add(_contentView);
        }

        /// <summary>
        /// Method to update the content size.
        /// </summary>
        /// <param name="contentSize">The content size.</param>
        void UpdateContentSize(Size contentSize)
        {
            _initialContentSize.Width = contentSize.Width;
            _initialContentSize.Height = contentSize.Height;
            _interactiveViewerInfo?.UpdateContentSize(contentSize, true);
        }

        /// <summary>
        /// Method to get the content size.
        /// </summary>
        Size GetContentSize()
        {
            if (_contentView != null && _contentSize.IsZero)
            {
                return _availableSize;
            }

            return _contentSize;
        }

        /// <summary>
        /// Method to get the desired horizontal origin.
        /// </summary>
        /// <param name="desiredZoomFactor">The zoom factor.</param>
        /// <param name="desiredZoomOriginX">The horizontal origin.</param>
        /// <returns>The desired horizontal origin.</returns>
        double GetDesiredOriginX(double desiredZoomFactor, double desiredZoomOriginX)
        {
            double viewportWidth = _interactiveViewerInfo?.GetViewportSize().Width ?? 0;

            if (_initialContentSize.Width <= 0 ||
                _initialContentSize.Width * desiredZoomFactor <= viewportWidth)
            {
                return _horizontalAlignmentRatio;
            }

            return desiredZoomOriginX / (Scale * _initialContentSize.Width);
        }

        /// <summary>
        /// Method to get the desired vertical origin.
        /// </summary>
        /// <param name="desiredZoomFactor">The zoom factor.</param>
        /// <param name="desiredZoomOriginY">The vertical origin.</param>
        /// <returns>The desired vertical origin.</returns>
        double GetDesiredOriginY(double desiredZoomFactor, double desiredZoomOriginY)
        {
            double viewportHeight = _interactiveViewerInfo?.GetViewportSize().Height ?? 0;

            if (_initialContentSize.Height <= 0 ||
                _initialContentSize.Height * desiredZoomFactor <= viewportHeight)
            {
                return _verticalAlignmentRatio;
            }

            return desiredZoomOriginY / (Scale * _initialContentSize.Height);
        }

        /// <summary>
        /// Gets the desired horizontal zoom anchor.
        /// </summary>
        /// <param name="desiredZoomFactor">The target zoom factor.</param>
        /// <param name="desiredZoomOriginX">The horizontal zoom origin.</param>
        /// <returns>The desired horizontal anchor.</returns>
        double GetDesiredAnchorX(double desiredZoomFactor, double desiredZoomOriginX)
        {
            _zoomOrigin.X = desiredZoomOriginX;
            double originX = GetDesiredOriginX(desiredZoomFactor, desiredZoomOriginX);
            _initialZoomFactorX = Scale;
            return originX;
        }

        /// <summary>
        /// Gets the desired vertical zoom anchor.
        /// </summary>
        /// <param name="desiredZoomFactor">The target zoom factor.</param>
        /// <param name="desiredZoomOriginY">The vertical zoom origin.</param>
        /// <returns>The desired vertical anchor.</returns>
        double GetDesiredAnchorY(double desiredZoomFactor, double desiredZoomOriginY)
        {
            _zoomOrigin.Y = desiredZoomOriginY;
            double originY = GetDesiredOriginY(desiredZoomFactor, desiredZoomOriginY);
            _initialZoomFactorY = Scale;
            return originY;
        }

        /// <summary>
        /// Gets the horizontal translation for the specified anchor and zoom factor.
        /// </summary>
        /// <param name="anchorX">The horizontal anchor position.</param>
        /// <param name="zoomFactor">The zoom factor.</param>
        /// <returns>The calculated horizontal translation.</returns>
        double GetDesiredTranslationX(double anchorX, double zoomFactor)
        {
            return (anchorX - _horizontalAlignmentRatio) * (_initialContentSize.Width * (zoomFactor - 1));
        }

        /// <summary>
        /// Gets the vertical translation for the specified anchor and zoom factor.
        /// </summary>
        /// <param name="anchorY">The vertical anchor position.</param>
        /// <param name="zoomFactor">The zoom factor.</param>
        /// <returns>The calculated vertical translation.</returns>
        double GetDesiredTranslationY(double anchorY, double zoomFactor)
        {
            return (anchorY - _verticalAlignmentRatio) * (_initialContentSize.Height * (zoomFactor - 1));
        }

        /// <summary>
        /// Method to rotate the content view.
        /// </summary>
        /// <param name="rotation">The rotation value.</param>
        void RotateView(double rotation)
        {
            if (_contentView == null)
                return;

            double actualRotation = rotation;
#if ANDROID || IOS
            //// In Android and iOS, if the view is already flipped then we have to make rotation in reverse direction.
            if ((_contentView.RotationX == 180 || _contentView.RotationY == 180)
                && !(_contentView.RotationX == 180 && _contentView.RotationY == 180))
            {
                actualRotation *= -1;
            }
#endif
            _contentView.Rotation += actualRotation;
            InvalidateMeasureContent();
            LayoutMeasure(_availableSize.Width, _availableSize.Height);
        }

        /// <summary>
        /// Method to invalidate the measure content.
        /// </summary>
        void InvalidateMeasureContent()
        {
            this.InvalidateMeasure();
#if ANDROID
            this.Measure(_availableSize.Width, _availableSize.Height);
#endif
            if (_contentView != null && _contentView is IView view)
            {
                view.InvalidateMeasure();
                view.InvalidateArrange();
            }
        }

        /// <summary>
        /// Method to reset the zoom and pan of the interactive layout.
        /// </summary>
        void ResetZoom()
        {
            _interactiveViewerInfo?.ResetZoomAndPan();

            AnchorX = 0.5;
            AnchorY = 0.5;
            TranslationX = 0;
            TranslationY = 0;
            _initialContentSize = Size.Zero;
            _initialZoomFactorX = 1;
            _initialZoomFactorY = 1;
            _zoomOrigin = Point.Zero;
            UpdateContentSize(new Size(0, 0));
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Measures the size of the layout content based on the provided width and height constraints.
        /// </summary>
        /// <param name="widthConstraint">The maximum width constraint for the layout.</param>
        /// <param name="heightConstraint">The maximum height constraint for the layout.</param>
        /// <returns>The measured size of the layout content.</returns>
        internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
        {
            if (_contentView == null)
            {
                return Size.Zero;
            }

            double width = double.IsFinite(widthConstraint) ? widthConstraint : _availableSize.Width;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : _availableSize.Height;

            //// If rotation is 90 or 270 then the layout dimensions are changed so that image will be stretched based on available size.
            if (Math.Abs(_contentView.Rotation / 90 % 2) == 1)
            {
                (width, height) = (height, width);
            }

            Size contentRenderingSize = new Size(width, height);
            foreach (var child in this.Children)
            {
                if (child is View)
                {
#if IOS
                    //// In iOS if we measure child object it renders in the original image dimensions.
                    _contentSize = contentRenderingSize = _contentView.Measure(width, height);
#else
                    _contentSize = contentRenderingSize = child.Measure(width, height);
#endif
                }
            }

            //// If rotation is 90 or 270 then the layout dimensions are changed so that image will be stretched based on available size.
            if (_contentView.Rotation != 0 && Math.Abs(_contentView.Rotation / 90 % 2) == 1)
            {
                (contentRenderingSize.Width, contentRenderingSize.Height) = (contentRenderingSize.Height, contentRenderingSize.Width);
                _contentSize = contentRenderingSize;
            }

            return contentRenderingSize;
        }

        /// <summary>
        /// Arranges the layout content within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds within which the layout content should be arranged.</param>
        /// <returns>The arranged size of the layout content.</returns>
        internal override Size LayoutArrangeChildren(Rect bounds)
        {
            foreach (var child in this.Children)
            {
                child.Arrange(new Rect(0, 0, bounds.Width, bounds.Height));
            }

            return bounds.Size;
        }

        #endregion
    }
}