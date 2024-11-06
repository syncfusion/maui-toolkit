using System.Collections;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Carousel
{
    #region LinearModeCollectionLayout

    /// <summary>
    /// Linear mode collection layout class.
    /// </summary>
    internal class LinearModeCollectionLayout : UICollectionViewDelegateFlowLayout
    {
        /// <summary>
        /// The carousel field.
        /// </summary>
        private PlatformCarousel _carousel;

        /// <summary>
        /// It indicates swipe is leftswipe or rightswipe.
        /// </summary>
        private bool _isSwipedLeft;

        /// <summary>
        /// The last content offset field.
        /// </summary>
        private nfloat _lastContentOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Syncfusion.PlatformCarousel.iOS.LinearModeCollectionLayout"/> class.
        /// </summary>
        /// <param name="carousel">Carousel class.</param>
        public LinearModeCollectionLayout(PlatformCarousel carousel)
        {
            _carousel = carousel;
        }

        /// <summary>
        /// Selected Item method.
        /// </summary>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (_carousel.LinearDataSource != null && indexPath.Row >= (int)_carousel.LinearDataSource.Count)
            {
                if (_carousel.AllowLoadMore)
                {
                    _carousel.LoadOtherItem();
                }
            }
            else
            {
                _carousel.SelectedIndex = indexPath.Row;
            }
        }

        /// <summary>
        /// Identify scrolling.
        /// </summary>
        /// <param name="scrollView">Scroll view.</param>
        public override void Scrolled(UIScrollView scrollView)
        {
            if (_carousel != null && _carousel.LinearMode != null)
            {
                _carousel.LinearMode.IsScrolling = true;

                if (_lastContentOffset > scrollView.ContentOffset.X)
                {
                    _isSwipedLeft = true;

                }
                else if (_lastContentOffset < scrollView.ContentOffset.X)
                {
                    _isSwipedLeft = false;
                }

                if (_carousel.SwipeStartEventArgs == null)
                {
                    _carousel.SwipeStartEventArgs = new SwipeStartedEventArgs();
                }

                if (_carousel.SwipeEventCalled)
                {
                    if (_isSwipedLeft)
                    {
                        _carousel.SwipeStartEventArgs.IsSwipedLeft = true;
                    }
                    else
                    {
                        _carousel.SwipeStartEventArgs.IsSwipedLeft = false;
                    }

                    _carousel.OnSwipeStarted(_carousel.SwipeStartEventArgs);
                }

                _lastContentOffset = scrollView.ContentOffset.X;
                _carousel.SwipeEventCalled = false;

            }
        }

        /// <summary>
        /// Dragging started.
        /// </summary>
        /// <param name="scrollView">Scroll view.</param>
        public override void DraggingStarted(UIScrollView scrollView)
        {
            if (_carousel != null)
            {
                _carousel.SwipeEventCalled = true;
            }
        }

        /// <summary>
        /// Dragging ended.
        /// </summary>
        /// <param name="scrollView">Scroll view.</param>
        /// <param name="willDecelerate">willDecelerate bool property.</param>
        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            _carousel?.OnSwipeEnded(EventArgs.Empty);
        }

        /// <summary>
        /// Gets the size for item method.
        /// </summary>
        /// <returns>The size for item.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="layout">Layout value.</param>
        /// <param name="indexPath">Index path.</param>
        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return new CGSize(_carousel.ItemWidth, _carousel.ItemHeight);
        }

        /// <summary>
        /// Gets the inset for section method.
        /// </summary>
        /// <returns>The inset for section.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="layout">Layout value.</param>
        /// <param name="section">Section value.</param>
        public override UIEdgeInsets GetInsetForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new UIEdgeInsets(0, 0, 0, 0);
        }

        /// <summary>
        /// Gets the minimum line spacing for section method.
        /// </summary>
        /// <returns>The minimum line spacing for section.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="layout">Layout value.</param>
        /// <param name="section">Section value.</param>
        public override nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            if (_carousel != null)
            {
                return _carousel.ItemSpacing;
            }
            else
            {
                return 5;
            }
        }
    }

    #endregion

    #region LinearModeCollectionSource

    /// <summary>
    /// Linear mode collection source class.
    /// </summary>
    internal class LinearModeCollectionSource : UICollectionViewDataSource
    {
        /// <summary>
        /// The carousel field.
        /// </summary>
        private PlatformCarousel _carousel;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearModeCollectionSource"/> class.
        /// </summary>
        /// <param name="carousel"> the Carousel.</param>
        public LinearModeCollectionSource(PlatformCarousel carousel)
        {
            _carousel = carousel;
        }

        /// <summary>
        /// Numbers the of sections method.
        /// </summary>
        /// <returns>The sections count.</returns>
        /// <param name="collectionView">Collection view.</param>
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        /// <summary>
        /// Gets the items count method.
        /// </summary>
        /// <returns>The items count.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="section">Section value.</param>
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            if (_carousel == null)
            {
                return 0;
            }

            return CalculateItemCount();
        }
    

        /// <summary>
        /// Calculates the total number of items to be displayed in the carousel.
        /// </summary>
        /// <returns>The total number of items, including the "Load More" item if applicable.</returns>
        nint CalculateItemCount()
        {
            if(_carousel.LinearDataSource == null)
            {  
                return 0; 
            }

            if (!_carousel.AllowLoadMore)
            {
                return (nint)_carousel.LinearDataSource.Count;
            }

            if (_carousel.DataSource != null)
            {
                return _carousel.LinearDataSource.Count == _carousel.DataSource.Count
                    ? (nint)_carousel.LinearDataSource.Count
                    : (nint)_carousel.LinearDataSource.Count + 1;
            }
            return 0;
        }

        /// <summary>
        /// Gets the cell method.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            UICollectionViewCell cell = (UICollectionViewCell)collectionView.DequeueReusableCell(@"cell", indexPath);

            if (_carousel.EnableVirtualization && _carousel.IsLinearVirtual)
            {
                return GetVirtualizedCell(cell, collectionView, indexPath);
            }
            else
            {
                return GetRegularCell(cell, collectionView, indexPath);
            }
        }

        /// <summary>
        /// Gets a virtualized cell for the specified index path.
        /// </summary>
        /// <param name="cell">The reusable cell to configure.</param>
        /// <param name="collectionView">The collection view requesting the cell.</param>
        /// <param name="indexPath">The index path specifying the location of the cell.</param>
        /// <returns>A configured UICollectionViewCell object.</returns>
        UICollectionViewCell GetVirtualizedCell(UICollectionViewCell cell, UICollectionView collectionView, NSIndexPath indexPath)
        {
            ClearExistingSubviews(cell);
            if(_carousel.LinearVirtualDataSource != null)
            {
                object? convertitem = ((IList)_carousel.LinearVirtualDataSource)[(int)indexPath.Row];
                if (convertitem != null)
                {
                    ConvertItem(convertitem, indexPath.Row);
                    PlatformCarouselItem? item = _carousel.GetVirtualizedItem(convertitem, indexPath.Row);
                    if (item != null)
                        ConfigureCellWithItem(cell, item);
                }
            }
            SelectAndScrollIfNeeded(collectionView, indexPath);
            return cell;
        }

        /// <summary>
        /// Gets a regular (non-virtualized) cell for the specified index path.
        /// </summary>
        /// <param name="cell">The reusable cell to configure.</param>
        /// <param name="collectionView">The collection view requesting the cell.</param>
        /// <param name="indexPath">The index path specifying the location of the cell.</param>
        /// <returns>A configured UICollectionViewCell object.</returns>
        UICollectionViewCell GetRegularCell(UICollectionViewCell cell, UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (_carousel.LinearDataSource != null && (int)_carousel.LinearDataSource.Count - 1 == indexPath.Row)
            {
                _carousel.IsLoaded = false;
            }

            if (_carousel.LinearDataSource != null && (int)_carousel.LinearDataSource.Count > indexPath.Row)
            {
                return ConfigureRegularCell(cell, collectionView, indexPath);
            }
            else if (_carousel.AllowLoadMore)
            {
                return ConfigureLoadMoreCell(cell, collectionView, indexPath);
            }

            return cell;
        }

        /// <summary>
        /// Configures a regular (non-virtualized) cell with carousel item data.
        /// </summary>
        UICollectionViewCell ConfigureRegularCell(UICollectionViewCell cell, UICollectionView collectionView, NSIndexPath indexPath)
        {
            ClearExistingSubviews(cell);

            if (_carousel.AllowLoadMore && _carousel.IsLinearVirtual)
            {
                ConvertVirtualItemForLoadMore(indexPath);
            }

            PlatformCarouselItem carouselItem = _carousel.LinearDataSource!=null? _carousel.LinearDataSource.GetItem<PlatformCarouselItem>((nuint)indexPath.Row): new PlatformCarouselItem();
            ConfigureCellAccessibility(cell, carouselItem, indexPath);
            ConfigureCellWithItem(cell, carouselItem);

            carouselItem.UserInteractionEnabled = false;
            SelectAndScrollIfNeeded(collectionView, indexPath);

            return cell;
        }

        /// <summary>
        /// Configures a cell for the "Load More" functionality.
        /// </summary>
        UICollectionViewCell ConfigureLoadMoreCell(UICollectionViewCell cell, UICollectionView collectionView, NSIndexPath indexPath)
        {
            ClearExistingSubviews(cell);

            PlatformCarouselItem item = CreateLoadMoreItem();
            ConfigureLoadMoreAccessibility(cell, item);

            SelectAndScrollIfNeeded(collectionView, indexPath);

            return cell;
        }

        /// <summary>
        /// Clears existing subviews from a cell.
        /// </summary>
        void ClearExistingSubviews(UICollectionViewCell cell)
        {
            foreach (UIView view in cell.Subviews)
            {
                view.RemoveFromSuperview();
            }
        }

        /// <summary>
        /// Converts an item for virtualization.
        /// </summary>
        void ConvertItem(object convertitem, nint index)
        {
            ConversionEventArgs arg = new ConversionEventArgs
            {
                Index = (int)index,
                Item = convertitem
            };
            _carousel.OnConversion(arg);
        }

        /// <summary>
        /// Configures a cell with a carousel item.
        /// </summary>
        void ConfigureCellWithItem(UICollectionViewCell cell, PlatformCarouselItem item)
        {
            if (item.View != null)
            {
                item.View.Frame = new CGRect(0, 0, _carousel.ItemWidth, _carousel.ItemHeight);
                cell.AddSubview(item.View);
            }
        }

        /// <summary>
        /// Selects and scrolls to the cell if it's the selected index and not currently scrolling.
        /// </summary>
        void SelectAndScrollIfNeeded(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (_carousel.SelectedIndex == indexPath.Row && !_carousel.LinearMode.IsScrolling)
            {
                NSIndexPath pathValue = NSIndexPath.FromRowSection(indexPath.Row, 0);
                collectionView.SelectItem(pathValue, false, UICollectionViewScrollPosition.Right);
                collectionView.ScrollToItem(indexPath, UICollectionViewScrollPosition.CenteredHorizontally, false);
            }
        }

        /// <summary>
        /// Converts a virtual item for load more functionality.
        /// </summary>
        void ConvertVirtualItemForLoadMore(NSIndexPath indexPath)
        {
            if(_carousel.LinearVirtualDataSource != null)
            {
                object? convertitem = ((IList)_carousel.LinearVirtualDataSource)[(int)indexPath.Row];
                if (convertitem != null)
                {
                    ConvertItem(convertitem, indexPath.Row);
                }
            }
            if (_carousel.LinearDataSource != null && _carousel.DataSource != null)
            {
                _carousel.LinearDataSource.ReplaceObject(indexPath.Row, _carousel.DataSource.GetItem<PlatformCarouselItem>((nuint)indexPath.Row));
            }
        }

        /// <summary>
        /// Configures accessibility for a regular cell.
        /// </summary>
        void ConfigureCellAccessibility(UICollectionViewCell cell, PlatformCarouselItem carouselItem, NSIndexPath indexPath)
        {
            if (cell != null)
            {
                cell.IsAccessibilityElement = true;
                if (_carousel.DataSource != null)
                {
                    string accessibilityText = $"{carouselItem.AutomationId} PlatformCarouselItem {(int)indexPath.Row + 1} of {(int)_carousel.DataSource.Count}";
                    cell.AccessibilityLabel = accessibilityText;
                    cell.AccessibilityIdentifier = accessibilityText;
                }
            }
        }

        /// <summary>
        /// Creates a load more item.
        /// </summary>
        PlatformCarouselItem CreateLoadMoreItem()
        {
            PlatformCarouselItem item = new PlatformCarouselItem
            {
                View = _carousel.Loadmore,
                InternalCarousel = _carousel
            };
            if(item.View != null)
                item.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            item.Frame = new CGRect(0, 0, _carousel.ItemWidth, _carousel.ItemHeight);
            if (item.View != null)
            {
                item.View.Frame = item.Subviews[0].Frame;
                item.AddSubview(item.View);
            }
            item.UserInteractionEnabled = false;
            return item;
        }

        /// <summary>
        /// Configures accessibility for the load more cell.
        /// </summary>
        void ConfigureLoadMoreAccessibility(UICollectionViewCell cell, PlatformCarouselItem item)
        {
            if (cell != null)
            {
                string accessibilityText = $"{_carousel.AutomationId} Load More. Tap to load more items";
                cell.AccessibilityLabel = accessibilityText;
                cell.AccessibilityIdentifier = accessibilityText;
                cell.AddSubview(item);
            }
        }

    }
    #endregion

    #region SfLinearMode

    /// <summary>
    /// linear mode class.
    /// </summary>
    internal class SfLinearMode : UIView
    {
        /// <summary>
        /// The view field.
        /// </summary>
        private UIView? _linearView;

        /// <summary>
        /// The is scrolling.
        /// </summary>
        private bool _isScrolling = false;

        /// <summary>
        /// The view collection field.
        /// </summary>
        private UICollectionView? _viewCollection;

        /// <summary>
        /// The flow layout field.
        /// </summary>
        private UICollectionViewFlowLayout? _flowLayout;

        /// <summary>
        /// The carousel field.
        /// </summary>
        private PlatformCarousel _carousel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLinearMode"/> class.
        /// </summary>
        /// <param name="carousel">Carousel parameter.</param>
        public SfLinearMode(PlatformCarousel carousel)
        {
            _carousel = carousel;
        }

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
                OnFramePropertyChanged();
                
            }
        }

        void OnFramePropertyChanged()
        {
            InitializeLinearView();
            InitializeFlowLayout();
            InitializeCollectionView();
            ConfigureCollectionView();
            AddViewsToHierarchy();
        }

        /// <summary>
        /// Initializes the linear view with the current frame dimensions.
        /// </summary>
        void InitializeLinearView()
        {
            LinearView = new UIView
            {
                Frame = new CGRect(0, 0, Frame.Size.Width, Frame.Size.Height)
            };
        }

        /// <summary>
        /// Initializes the flow layout for the collection view.
        /// </summary>
        void InitializeFlowLayout()
        {
            _flowLayout = new UICollectionViewFlowLayout
            {
                ScrollDirection = UICollectionViewScrollDirection.Horizontal
            };

            if (_carousel != null)
            {
                _flowLayout.ItemSize = new CGSize(_carousel.ItemWidth, _carousel.ItemHeight);
            }
        }

        /// <summary>
        /// Initializes the collection view with the current frame and flow layout.
        /// </summary>
        void InitializeCollectionView()
        {
            if (_flowLayout == null)
                return;
            _viewCollection = new UICollectionView(new CGRect(0, 0, Frame.Size.Width, 200), _flowLayout)
            {
                BackgroundColor = UIColor.Clear,
                ShowsHorizontalScrollIndicator = false
            };
            if(Carousel != null)
            {
                _viewCollection.SemanticContentAttribute = Carousel.IsRTL ? UISemanticContentAttribute.ForceRightToLeft : UISemanticContentAttribute.ForceLeftToRight;
            }
        }

        /// <summary>
        /// Configures the collection view with data source, delegate, and cell registration.
        /// </summary>
        void ConfigureCollectionView()
        {
            if (_carousel == null || _viewCollection == null) return;

            _viewCollection.DataSource = new LinearModeCollectionSource(_carousel);
            _viewCollection.Delegate = new LinearModeCollectionLayout(_carousel);
            _viewCollection.RegisterClassForCell(typeof(UICollectionViewCell), "cell");
        }

        /// <summary>
        /// Adds the configured views to the view hierarchy.
        /// </summary>
        void AddViewsToHierarchy()
        {
            if (_viewCollection == null || LinearView == null)
                return;
            LinearView.AddSubview(_viewCollection);
            AddSubview(LinearView);
        }

        /// <summary>
        /// Gets or sets the view collection.
        /// </summary>
        /// <value>The view collection.</value>
        public UICollectionView? ViewCollection
        {
            get
            {
                return _viewCollection;
            }

            set
            {
                _viewCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets the carousel field.
        /// </summary>
        /// <value>The carousel.</value>
        internal PlatformCarousel Carousel
        {
            get
            {
                return _carousel;
            }

            set
            {
                _carousel = value;
                UpdateFrame();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Syncfusion.PlatformCarousel.iOS.SfLinearMode"/> is scrolling.
        /// </summary>
        /// <value><c>true</c> if is scrolling; otherwise, <c>false</c>.</value>
        internal bool IsScrolling
        {
            get
            {
                return _isScrolling;
            }

            set
            {
                _isScrolling = value;
            }
        }

        /// <summary>
        /// Gets or sets the v.
        /// </summary>
        /// <value>The v.</value>
        internal UIView? LinearView
        {
            get
            {
                return _linearView;
            }

            set
            {
                _linearView = value;
            }
        }

        /// <summary>
        /// Layouts the sub views method.
        /// </summary>
        public override void LayoutSubviews()
        {
            UpdateFrame();
            base.LayoutSubviews();
        }

        /// <summary>
        /// Updates the frame method.
        /// </summary>
        void UpdateFrame()
        {
            if(LinearView != null)
                LinearView.Frame = new CGRect(0, (_carousel.Frame.Size.Height / 2) - (_carousel.ItemHeight / 2), _carousel.Frame.Size.Width, _carousel.ItemHeight);
            if(_viewCollection != null)
            {
                _viewCollection.Frame = new CGRect(0, 0, _carousel.Frame.Size.Width, _carousel.ItemHeight);
                if(_carousel.DataSource != null)
                    _viewCollection.ContentSize = new CGSize(_carousel.ItemWidth * _carousel.DataSource.Count, _carousel.ItemHeight);
            }
        }
    }

    #endregion
}