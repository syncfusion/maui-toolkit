namespace Syncfusion.Maui.Toolkit.Carousel
{
    using System;
    using CoreGraphics;
    using Foundation;
    using Microsoft.Maui.Platform;
    using UIKit;

	/// <summary>
	/// Platform carousel item item class.
	/// </summary>
	/// <exclude/>
	public partial class PlatformCarouselItem : UIView
    {
        #region Fields

        /// <summary>
        /// The view field.
        /// </summary>
        private UIView? _view = null;

        /// <summary>
        /// The index field.
        /// </summary>
        private nint _index;

        /// <summary>
        /// The image view field.
        /// </summary>
        private UIImageView? _imageView;

        /// <summary>
        /// The image field.
        /// </summary>
        private UIImage? _image;

        /// <summary>
        /// AutomationId field.
        /// </summary>
        private string _automationId = string.Empty;

        /// <summary>
        /// The internal carousel field.
        /// </summary>
        private PlatformCarousel? _internalCarousel;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformCarouselItem"/> class.
        /// </summary>
        public PlatformCarouselItem()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the frame.
        /// </summary>
        /// <value>The frame.</value>
        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }

            set
            {
                base.Frame = value;
            }
        }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public UIView? View
        {
            get
            {
                return _view;
            }

            set
            {
                _view = value;
            }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public UIImage? Image
        {
            internal get
            {
                return _image;
            }

            set
            {
                _image = value;
                if (_imageView != null)
                {
                    _imageView.Image = _image;
                    SetNeedsDisplay();
                }
            }
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public nint Index
        {
            internal get
            {
                return (nint)_index;
            }

            set
            {
                _index = value;
                SetNeedsDisplay();
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the carousel.
        /// </summary>
        /// <value>The carousel.</value>
        internal PlatformCarousel? InternalCarousel
        {
            get
            {
                return _internalCarousel;
            }

            set
            {
                _internalCarousel = value;
            }
        }

        /// <summary>
        /// Gets or sets the AutomationId
        /// </summary>
        internal string AutomationId
        {
            get
            {
                return _automationId;
            }

            set
            {
                if (_automationId != value)
                {
                    _automationId = value;
                    if (_automationId == null)
                    {
                        _automationId = string.Empty;
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Touch ended method.
        /// </summary>
        /// <param name="touches">Touches value.</param>
        /// <param name="evt">Event parameter.</param>
#pragma warning disable CS8765
        public override void TouchesEnded(NSSet touches, UIEvent evt)
#pragma warning restore CS8765
        {
            UITouch touch = (UITouch)touches.AnyObject;
            CGPoint touchPoint = (CGPoint)touch.LocationInView(this);
            UIView? subView = HitTest(touchPoint, null);
            UIView? topView = GetTopView(subView);

            if (topView != null && IsValidInteraction(topView))
            {
                UpdateCarouselItemSelected(topView);
                UpdateCarouselCenterPosition(topView);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">boolean type</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                foreach (UIView subView in Subviews)
                {
                    subView.RemoveFromSuperview();
                    subView.Dispose();
                }

                RemoveFromSuperview();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks if the interaction with the given view is valid.
        /// </summary>
        /// <param name="topView">The view to check for valid interaction.</param>
        /// <returns>True if the interaction is valid; otherwise, false.</returns>
        bool IsValidInteraction(UIView topView)
        {
            return topView.IsKindOfClass(Class) == true && _internalCarousel != null && _internalCarousel.EnableInteraction;
        }

        /// <summary>
        /// Updates the center position of the carousel based on the selected item.
        /// </summary>
        /// <param name="topView">The view representing the selected carousel item.</param>
        void UpdateCarouselCenterPosition(UIView topView)
        {
            if (_internalCarousel == null)
                return;

            if (((PlatformCarouselItem)topView).Index <= (int)_internalCarousel.DatasourceEndIndex)
            {
                _internalCarousel.UpdateCenterPosition((PlatformCarouselItem)topView);
            }
        }

        /// <summary>
        /// Updates the selected state of the carousel item.
        /// </summary>
        /// <param name="topView">The view representing the selected carousel item.</param>
        void UpdateCarouselItemSelected(UIView topView)
        {
            if (_internalCarousel == null)
                return;

            if (_internalCarousel.SelectedIndex != ((PlatformCarouselItem)topView).Index)
            {
                _internalCarousel.IsCarouselInvoked = false;
                if (_internalCarousel.EnableVirtualization)
                {
                    _internalCarousel.IsTapped = true;
                }

                _internalCarousel.SelectedIndex = ((PlatformCarouselItem)topView).Index;
                if (_internalCarousel.AllowLoadMore)
                {
                    if (((PlatformCarouselItem)topView).Index > (int)_internalCarousel.DatasourceEndIndex)
                    {
                        _internalCarousel.LoadOtherItem();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the topmost view in the view hierarchy that matches the class type.
        /// </summary>
        /// <param name="subView">The initial subview to start the search from.</param>
        /// <returns>The topmost view that matches the class type, or null if not found.</returns>
        UIView? GetTopView(UIView? subView)
        {
            UIView? topView = subView;
            while (topView?.Superview != null && !topView.IsKindOfClass(Class))
            {
                topView = topView.Superview;
            }
            return topView;
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize()
        {
            UserInteractionEnabled = true;
            _imageView = new UIImageView();
            _imageView.Frame = Frame;
            _imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            AddSubview(_imageView);
            ClipsToBounds = true;
        }

        #endregion
    }
}
