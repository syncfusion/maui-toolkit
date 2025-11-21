using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Resources;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.Carousel
{

	/// <summary>
	/// Represents a platform-specific handler for connecting a carousel view.
	/// </summary>
	/// <exclude/>
	public partial class PlatformCarousel : UIView
    {

		#region Private Variables

		private const float Angle45 = 45;

        private const float Angle90 = 90;

        private const float Angle180 = 180;

        private const float Angle360 = 360;

        private const float MaxIncrement = 80;

        private const float MinIncrement = 50;


        /// <summary>
        /// The load more.
        /// </summary>
        private UIView? _loadMore;

        /// <summary>
        /// The is tapped.
        /// </summary>
        private bool _isTapped = false;

        /// <summary>
        /// The linear mode.
        /// </summary>
        private SfLinearMode? _linearMode;

        /// <summary>
        /// The custom gesture.
        /// </summary>
        private CustomCarouselPanGesture? _customGesture;

        /// <summary>
        /// The carousel handler
        /// </summary>
        private CarouselHandler? _handler;

        /// <summary>
        /// The pan gesture recognizer.
        /// </summary>
        internal UIPanGestureRecognizer? _gestureRecognizer;

        /// <summary>
        /// The data source start index.
        /// </summary>
        private nint _dataSourceStartIndex = 0;

        /// <summary>
        /// The data source end index.
        /// </summary>
        private nint _dataSourceEndIndex = 0;

        /// <summary>
        /// The is loaded.
        /// </summary>
        private bool _isLoaded;

        /// <summary>
        /// The isSwipeRestricted.
        /// </summary>
        private bool _isSwipeRestricted;

        /// <summary>
        /// The touch interaction for child view.
        /// </summary>
        private bool _enableInteraction = true;

        /// <summary>
        /// Indicates whether control is enabled or disabled.
        /// </summary>
        private bool _isEnabled;

        /// <summary>
        /// Indicates whether the control is in right-to-left (RTL) mode.
        /// </summary>
        private bool _isRTL;

        /// <summary>
        /// The scroll mode.
        /// </summary>
        private SwipeMovementMode _swipeMovementMode = SwipeMovementMode.MultipleItems;

        private bool _canExecuteSwipe = true;

        /// <summary>
        /// The load more item.
        /// </summary>
        private PlatformCarouselItem _loadMoreItem = [];

        /// <summary>
        /// The view mode field.
        /// </summary>
        private ViewMode _viewMode;

        /// <summary>
        /// The data source field.
        /// </summary>
        private NSMutableArray? _dataSource;

        /// <summary>
        /// The selected index field.
        /// </summary>
        private nint _selectedIndex;

        /// <summary>
        /// The selected item offset field.
        /// </summary>
        private nfloat _selectedItemOffset;

        /// <summary>
        /// The rotation angle field.
        /// </summary>
        private nfloat _rotationAngle;

        /// <summary>
        /// The offset field.
        /// </summary>
        private nfloat _offset;

        /// <summary>
        /// The scale offset field.
        /// </summary>
        private nfloat _scaleOffset;

        /// <summary>
        /// The item height field.
        /// </summary>
        private nfloat _itemHeight;

        /// <summary>
        /// The item width field.
        /// </summary>
        private nfloat _itemWidth;

        /// <summary>
        /// The duration field.
        /// </summary>
        private nfloat _duration;

        /// <summary>
        /// The item spacing field.
        /// </summary>
        private int _itemSpacing;

        /// <summary>
        /// The called field.
        /// </summary>
        private bool _isCarouselInvoked;

        /// <summary>
        /// The selected updated field.
        /// </summary>
        private bool _isSelectedUpdated;

        /// <summary>
        /// The updated selected value field.
        /// </summary>
        private nint _updatedSelectedValue;

        /// <summary>
        /// The touch point x field.
        /// </summary>
        private nfloat _touchX;

        /// <summary>
        /// The increment field.
        /// </summary>
        private nfloat _increment;

        /// <summary>
        /// The temp x  field.
        /// </summary>
        private nfloat _tempX;

        /// <summary>
        /// The last x field.
        /// </summary>
        private nfloat _lastX;

        /// <summary>
        /// The switch float field.
        /// </summary>
        private nfloat _switchFloat;

        /// <summary>
        /// The last value field.
        /// </summary>
        private nfloat _lastValue;

        /// <summary>
        /// The allow load more field.
        /// </summary>
        private bool _allowLoadMore;

        /// <summary>
        /// The load more text.
        /// </summary>
        private string? _loadMoreText;

        /// <summary>
        /// The is virtualization.
        /// </summary>
        private bool _isVirtualization;

        /// <summary>
        /// The maximum visible count.
        /// </summary>
        private int _maximumVisibleCount = 0;

        /// <summary>
        /// The linear data source.
        /// </summary>
        private NSMutableArray? _linearDataSource;

        /// <summary>
        /// The x forms data source.
        /// </summary>
        private object? _linearVirtualDataSource;

        /// <summary>
        /// The x forms.
        /// </summary>
        private bool _isLinearVirtual;

        /// <summary>
        /// The tapped difference.
        /// </summary>
        private nint _tappedDifference = 0;

        /// <summary>
        /// The is custom view.
        /// </summary>
        private bool _isCustomView = false;

        /// <summary>
        /// The text changed.
        /// </summary>
        private bool _textChanged = false;

		/// <summary>
		/// The base view.
		/// </summary>
		private readonly UIView _baseView = [];

        /// <summary>
        /// AutomationId field.
        /// </summary>
        private string _automationId = string.Empty;

        /// <summary>
        /// The resource.
        /// </summary>
        private ResourceManager? _resource;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> class.
		/// </summary>
		public PlatformCarousel()
        {
            Initialize();
        }

		#endregion

		#region Public Variables

		/// <summary>
		/// The selection changed event handler
		/// </summary>
		/// <param name="sender"> sender value is carousel</param>
		/// <param name="e">changed indexed carousel</param>
		public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);

        /// <summary>
        /// The collection changed event handler
        /// </summary>
        /// <param name="sender">carousel value</param>
        /// <param name="e">changed items</param>
        internal delegate void CollectionChangedEventHandler(object sender, CollectionChangedEventArgs e);

        /// <summary>
        /// The Native converter event handler.
        /// </summary>
        /// <param name="sender">carousel value</param>
        /// <param name="e">conversion items</param>
        internal delegate void NativeConverterEventHandler(object sender, ConversionEventArgs e);

        /// <summary>
        /// Occurs when collection changed.
        /// </summary>
        internal event CollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Occurs when conversion invoked.
        /// </summary>
        internal event NativeConverterEventHandler? ConversionInvoked;

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
            ClipsToBounds = true;
            if (IsDataSourceAvailable() && _viewMode == ViewMode.Default && !_isVirtualization)
            {
                ArrangeItem();
                LinearViewLoading();
            }
            else
            {
                UpdateViewMode();
                if (_isVirtualization && _viewMode == ViewMode.Default)
                {
                    LinearViewLoading();
                }
            }
        }

        /// <summary>
        /// Gets or sets the view mode.
        /// </summary>
        /// <value>The view mode.</value>
        public ViewMode ViewMode
        {
            get
            {
                return _viewMode;
            }

            set
            {
                _viewMode = value;
                UpdateViewMode();
            }
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public NSMutableArray? DataSource
        {
            get
            {
                return _dataSource;
            }

            set
            {
                _dataSource = value;
                OnDataSourcePropertyChanged(value);
            }
        }

        void OnDataSourcePropertyChanged(NSMutableArray? value)
        {
            if (value != null && value.Count > 0)
            {
                _linearDataSource = [];
                if (LoadMoreItemsCount > 0)
                {
                    var count = _maximumVisibleCount;
                    if (_dataSource != null && count > GetItemCount())
                    {
                        count = (int)_dataSource.Count;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        if (_dataSource != null)
						{
							_linearDataSource.Add(_dataSource.GetItem<NSObject>((nuint)i));
						}
					}
                }
                else
                {
                    _linearDataSource = value;
                }
            }

            if (_dataSource != null)
            {
                if (_dataSource.Count - 1 < (nuint)_selectedIndex)
                {
                    _selectedIndex = 0;
                }
            }

            if (!_isSelectedUpdated)
            {
                _selectedIndex = _updatedSelectedValue;
            }

            UpdateViewMode();
            CollectionChangedEventArgs args = new CollectionChangedEventArgs
            {
                NewCollection = _dataSource
            };
            OnCollectionChanged(args);
        }

        /// <summary>
        /// Gets or sets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public nint SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex != value)
                {
                    OnSelectedIndexPropertyChanged(value);
                }
            }
        }

        void OnSelectedIndexPropertyChanged(nint value)
        {
            SetSelectedIndex(value);
        }

        /// <summary>
        /// Gets or sets the selected item offset.
        /// </summary>
        /// <value>The selected item offset.</value>
        public nfloat SelectedItemOffset
        {
            get
            {
                return _selectedItemOffset;
            }

            set
            {
                _selectedItemOffset = value;
                OnSelectedItemOffsetPropertyChanged();
            }
        }

        void OnSelectedItemOffsetPropertyChanged()
        {
            if (_dataSource == null)
            {
                Initialize();
            }
            OnOffSetPropertyChanged();
        }

        /// <summary>
        /// Gets or sets the rotation angle.
        /// </summary>
        /// <value>The rotation angle.</value>
        public nfloat RotationAngle
        {
            get
            {
                return _rotationAngle;
            }

            set
            {
                _rotationAngle = value;
                OnRotationAnglePropertyChanged();
            }
        }

        void OnRotationAnglePropertyChanged()
        {
            OnOffSetPropertyChanged();
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public nfloat Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
                OnOffSetPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the scale offset.
        /// </summary>
        /// <value>The scale offset.</value>
        public nfloat ScaleOffset
        {
            get
            {
                return _scaleOffset;
            }

            set
            {
                _scaleOffset = value;
                OnOffSetPropertyChanged();
            }
        }

        void OnOffSetPropertyChanged()
        {
            if (IsDataSourceAvailable())
            {
                DefaultViewLoading();
            }

            SetNeedsDisplay();
        }

        /// <summary>
        /// Gets or sets the height of the item.
        /// </summary>
        /// <value>The height of the item.</value>
        public nfloat ItemHeight
        {
            get
            {
                return _itemHeight;
            }

            set
            {
                _itemHeight = value;
                UpdateSize();
            }
        }

        /// <summary>
        /// Gets or sets the width of the item.
        /// </summary>
        /// <value>The width of the item.</value>
        public nfloat ItemWidth
        {
            get
            {
                return _itemWidth;
            }

            set
            {
                _itemWidth = value;
                UpdateSize();
            }
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public nfloat Duration
        {
            get
            {
                return _duration;
            }

            set
            {
                _duration = value;
            }
        }

        /// <summary>
        /// Gets or sets the item spacing.
        /// </summary>
        /// <value>The item spacing.</value>
        public int ItemSpacing
        {
            get
            {
                return _itemSpacing;
            }

            set
            {
                if (_itemSpacing != value)
                {
                    _itemSpacing = value;
                    UpdateSize();
                    SetNeedsDisplay();
                }
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> allow load more.
		/// </summary>
		/// <value><c>true</c> if allow load more; otherwise, <c>false</c>.</value>
		public bool AllowLoadMore
        {
            get
            {
                return _allowLoadMore;
            }

            set
            {
                _allowLoadMore = value;
                AllowLoadMoreItems();
            }
        }

        /// <summary>
        /// Disables virtualization for the carousel.
        /// </summary>
        void DisableVirtualization()
        {
            _isVirtualization = false;
        }

        /// <summary>
        /// Initializes the data source from the existing data source.
        /// </summary>
        void InitializeDataSourceFromDataSource()
        {
            if (_dataSource != null)
            {
                _linearDataSource = [];
                int count = Math.Min(_maximumVisibleCount, (int)_dataSource.Count);

                for (int i = 0; i < count; i++)
                {
                    _linearDataSource.Add(_dataSource.GetItem<NSObject>((nuint)i));
                }
            }
        }

        /// <summary>
        /// Handles the data source when "Load More" is not enabled.
        /// </summary>
        void HandleDataSourceWithoutLoadMore()
        {
            if (_isLinearVirtual && _dataSource != null)
            {
                for (int i = 0; i < (int)_dataSource.Count; i++)
                {
                    object? convertItem = PlatformCarousel.GetConvertItem(_linearVirtualDataSource, i);
                    SetConversionArguments(convertItem, i);
                }
            }

            _linearDataSource = _dataSource;
        }

        /// <summary>
        /// Adds the "Load More" functionality to the carousel.
        /// </summary>
        void AllowLoadMoreItems()
        {
            if (LoadMoreItemsCount > 0)
            {
                DisableVirtualization();
            }

            if (IsDataSourceAvailable() && LoadMoreItemsCount > 0)
            {
                InitializeDataSourceFromDataSource();
            }
            else if (IsDataSourceAvailable() && !_allowLoadMore)
            {
                HandleDataSourceWithoutLoadMore();
            }

            UpdateViewMode();
        }

        /// <summary>
        /// Gets or sets the maximum visible count.
        /// </summary>
        /// <value>The maximum visible count.</value>
        public int LoadMoreItemsCount
        {
            get
            {
                return _maximumVisibleCount;
            }

            set
            {
                _maximumVisibleCount = value;
                OnLoadMoreItemsCountPropertyChanged();
            }
        }

        void OnLoadMoreItemsCountPropertyChanged()
        {
            if (_maximumVisibleCount <= 0 && _virtualView != null)
			{
				_maximumVisibleCount = _virtualView.ItemsSource.Count();
			}

			AllowLoadMoreItems();
        }

        /// <summary>
        /// Gets or sets the load more.
        /// </summary>
        /// <value>The load more.</value>
        public UIView? LoadMoreView
        {
            get
            {
                return Loadmore;
            }

            set
            {
                OnLoadMoreViewPropertyChanged(value);
            }
        }

        void OnLoadMoreViewPropertyChanged(UIView? value)
        {
            Loadmore = value;
            LoadMoreItem.View = Loadmore;
            if (value != null && LoadMoreItem.View != null)
            {
                LoadMoreItem.View.Frame = LoadMoreItem.Frame;
            }

            if (value != null)
            {
                _isCustomView = true;
            }
            else
            {
                _isCustomView = false;
            }

            AddLoadMore();
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> is virtualization.
		/// </summary>
		/// <value><c>true</c> if is virtualization; otherwise, <c>false</c>.</value>
		public bool EnableVirtualization
        {
            get
            {
                return _isVirtualization;
            }

            set
            {
                _isVirtualization = value;
                OnEnableVirtualizationPropertyChanged();
            }
        }

        void OnEnableVirtualizationPropertyChanged()
        {
            if (_isVirtualization)
            {
                _allowLoadMore = false;
            }

            UpdateViewMode();
        }

        /// <summary>
        /// The SwipeStarted event arguments.
        /// </summary>
        internal SwipeStartedEventArgs? SwipeStartEventArgs { get; set; }

        /// <summary>
        /// Gets or sets the load more text.
        /// </summary>
        /// <value>The load more text.</value>
        internal string? LoadMoreText
        {
            get
            {
                return _loadMoreText;
            }

            set
            {
                _loadMoreText = value;
                if (!_isCustomView)
                {
                    _textChanged = true;
                    AddLoadMore();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should apply right-to-left (RTL) flow direction.
        /// </summary>
        public bool IsRTL
        {
            get
            {
                return _isRTL;
            }

            set
            {
                _isRTL = value;
                UpdateFlowDirection();
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether control is enabled or disabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                EnableOrDisableInteraction();
            }
        }

        void EnableOrDisableInteraction()
        {
            UserInteractionEnabled = IsInteractionEnable();
            Subviews.ToList().ForEach(subView => subView.UserInteractionEnabled = IsInteractionEnable());
            OnEnableInteractionPropertyChanged();
        }

        bool IsInteractionEnable()
        {
            return IsEnabled && EnableInteraction;
        }

        /// <summary>
        /// Enable the touch interaction for child views.
        /// </summary>
        public bool EnableInteraction
        {
            get
            {
                return _enableInteraction;
            }

            set
            {
                _enableInteraction = value;
                EnableOrDisableInteraction();
            }
        }

        void OnEnableInteractionPropertyChanged()
        {
            if (ViewMode == ViewMode.Linear && LinearMode != null && LinearMode.ViewCollection != null)
            {
                LinearMode.ViewCollection.UserInteractionEnabled = IsInteractionEnable();
            }
        }

        /// <summary>
        /// SwipeMovementMode
        /// </summary>
        public SwipeMovementMode SwipeMovementMode
        {
            get
            {
                return _swipeMovementMode;
            }

            set
            {
                _swipeMovementMode = value;
            }
        }

        /// <summary>
        /// The swipeEventCalled.
        /// </summary>
        internal bool SwipeEventCalled { get; set; }

        /// <summary>
        /// Gets or sets the XForms data source.
        /// </summary>
        /// <value>The XForms data source.</value>
        internal object? LinearVirtualDataSource
        {
            get
            {
                return _linearVirtualDataSource;
            }

            set
            {
                OnLinearVirtualDataSourceChanged(value);
            }
        }

        void OnLinearVirtualDataSourceChanged(object? value)
        {
            bool isNew = false;
            if (value != null && _linearVirtualDataSource != null && ((IList)_linearVirtualDataSource).Count > 0)
            {
                for (int i = 0; i < ((IList)value).Count; i++)
                {
                    if (((IList)_linearVirtualDataSource)[i] != ((IList)value)[i])
                    {
                        isNew = true;
                        break;
                    }
                }
            }
            else
            {
                isNew = true;
            }

            if ((_isVirtualization || LoadMoreItemsCount > 0) && _isLinearVirtual && isNew)
            {
                _linearVirtualDataSource = value!;
                NSMutableArray array = [];
                for (int i = 0; i < ((IList)_linearVirtualDataSource).Count; i++)
                {
                    array.Add(new NSObject());
                }

                DataSource = array;
            }
        }

        /// <summary>
        /// Gets or sets the linear data source.
        /// </summary>
        /// <value>The linear data source.</value>
        internal NSMutableArray? LinearDataSource
        {
            get
            {
                return _linearDataSource;
            }

            set
            {
                _linearDataSource = value;
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> XForms.
		/// </summary>
		/// <value><c>true</c> if XForms; otherwise, <c>false</c>.</value>
		internal bool IsLinearVirtual
        {
            get
            {
                return _isLinearVirtual;
            }

            set
            {
                _isLinearVirtual = value;
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> is called.
		/// </summary>
		/// <value><c>true</c> if is called; otherwise, <c>false</c>.</value>
		internal bool IsCarouselInvoked
        {
            get
            {
                return _isCarouselInvoked;
            }

            set
            {
                _isCarouselInvoked = value;
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> is
		/// selected updated.
		/// </summary>
		/// <value><c>true</c> if is selected updated; otherwise, <c>false</c>.</value>
		internal bool IsSelectedUpdated
        {
            get
            {
                return _isSelectedUpdated;
            }

            set
            {
                _isSelectedUpdated = value;
            }
        }

        /// <summary>
        /// Gets or sets the updated selected value.
        /// </summary>
        /// <value>The updated selected value.</value>
        internal nint UpdatedSelectedValue
        {
            get
            {
                return _updatedSelectedValue;
            }

            set
            {
                _updatedSelectedValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the load more.
        /// </summary>
        /// <value>The load more.</value>
        internal UIView? Loadmore
        {
            get
            {
                return _loadMore;
            }

            set
            {
                _loadMore = value;
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> is tapped.
		/// </summary>
		/// <value><c>true</c> if is tapped; otherwise, <c>false</c>.</value>
		internal bool IsTapped
        {
            get
            {
                return _isTapped;
            }

            set
            {
                _isTapped = value;
            }
        }

        /// <summary>
        /// Gets or sets the linear mode.
        /// </summary>
        /// <value>The linear mode.</value>
        internal SfLinearMode LinearMode
        {
            get
            {
                return _linearMode!;
            }

            set
            {
                _linearMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the data source start index.
        /// </summary>
        /// <value>The data source start index.</value>
        internal nint DatasourceStartIndex
        {
            get
            {
                return _dataSourceStartIndex;
            }

            set
            {
                _dataSourceStartIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the data source end index.
        /// </summary>
        /// <value>The data source end index.</value>
        internal nint DatasourceEndIndex
        {
            get
            {
                return _dataSourceEndIndex;
            }

            set
            {
                _dataSourceEndIndex = value;
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> is loaded.
		/// </summary>
		/// <value><c>true</c> if is loaded; otherwise, <c>false</c>.</value>
		internal bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }

            set
            {
                _isLoaded = value;
            }
        }

        /// <summary>
        /// Gets or sets the load more item.
        /// </summary>
        /// <value>The load more item.</value>
        internal PlatformCarouselItem LoadMoreItem
        {
            get
            {
                return _loadMoreItem;
            }

            set
            {
                _loadMoreItem = value;
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
                OnAutomationIdPropertyChanged(value);
            }
        }

        void OnAutomationIdPropertyChanged(string value)
        {
            if(_automationId != value)
            {
                _automationId = value;
                _automationId ??= string.Empty;

                if (AllowLoadMore)
                {
                    LoadMoreAccessibility();
                }
            }
        }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        internal ResourceManager? Resource
        {
            get
            {
                return _resource;
            }

            set
            {
                _resource = value;
                SfCarouselResources.ResourceManager = value;
            }
        }



		#endregion
		/// <summary>
		/// Overrides the layout subviews method to handle accessibility.
		/// </summary>
		public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            IsAccessibilityElement = false;
        }

        /// <summary>
        /// Handles the awakening from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            Initialize();
            base.AwakeFromNib();
        }        

        /// <summary>
        /// Load more method.
        /// </summary>
        public void LoadMore()
        {
            LoadOtherItem();
        }

        /// <summary>
        /// Moves to the next item in the carousel.
        /// </summary>
        public void MoveNext()
        {
            int tempCount = GetItemCount();

            if (tempCount != 0)
            {
                if ((nuint)_selectedIndex != (nuint)tempCount - 1)
                {
                    if (_isVirtualization)
                    {
                        IsTapped = true;
                    }

                    if (tempCount > 0)
                    {
                        HandleSelectionChange(tempCount);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the selection change when moving to the next item.
        /// </summary>
        /// <param name="tempCount">The total count of items.</param>
        void HandleSelectionChange(int tempCount)
        {
			SelectionChangedEventArgs args = new SelectionChangedEventArgs
			{
				OldItem = GetMauiItem((int)SelectedIndex)
			};

			SelectedIndex++;
            _isCarouselInvoked = true;

            args.NewItem = GetMauiItem((int)SelectedIndex);
            OnSelectionChanged(args);
         }

        /// <summary>
        /// Gets the MAUI item at the specified index.
        /// </summary>
        /// <param name="selectedIndex">The index of the item to retrieve.</param>
        /// <returns>The object at the specified index in the ItemsSource, or null if not found.</returns>
        object? GetMauiItem(int selectedIndex)
        {
            if (_virtualView != null)
            {
                int i = 0;
                foreach (var item in _virtualView.ItemsSource)
                {
                    if (selectedIndex == i)
                    {
                        return item;
                    }

                    i++;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the item count and source information.
        /// </summary>
        /// <returns>A tuple containing the item count, and flags indicating the source type.</returns>
        int GetItemCount()
        {
            int itemCount = 0;
            if (_dataSource!= null && IsDataSourceAvailable())
            {
                itemCount = (int)_dataSource.Count;
            }

            return itemCount;
        }

        /// <summary>
        /// Gets a PlatformCarouselItem based on the current data source configuration.
        /// </summary>
        /// <param name="isDatasource">Indicates if the data source is being used.</param>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>A PlatformCarouselItem, or null if not found.</returns>
        PlatformCarouselItem? GetPlatformCarouselItem(bool isDatasource, nint index)
        {
            PlatformCarouselItem? platformCarouselItem = null;

            if (isDatasource)
            {
                if (_isVirtualization && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear && _linearVirtualDataSource != null)
                {
                    var item = ((IList)_linearVirtualDataSource)[(int)index];

                    if (item != null)
                    {
                        platformCarouselItem = GetVirtualizedItem(item, index);
                    }

                    if (platformCarouselItem != null)
                    {
                        platformCarouselItem.Index = index;
                    }
                }
                else
                {
                    if (_dataSource != null)
                    {
                        platformCarouselItem = _dataSource.GetItem<PlatformCarouselItem>((nuint)index);
                    }
                }

            }
            return platformCarouselItem;

        }
        
        /// <summary>
        /// Moves to the previous item in the carousel.
        /// </summary>
        public void MovePrevious()
        {
            int tempcount = GetItemCount();

            if (_selectedIndex != 0)
            {
                if (_isVirtualization)
                {
                    IsTapped = true;
                }

                if (tempcount > 0)
                {
                    HandleSelectionChangeForPrevious(tempcount);
                }
            }
        }

        void HandleSelectionChangeForPrevious(int tempCount)
        {
			SelectionChangedEventArgs args = new SelectionChangedEventArgs
			{
				OldItem = GetMauiItem((int)SelectedIndex)
			};

			SelectedIndex = _selectedIndex - 1;

            if (tempCount > 0)
            {
                _isCarouselInvoked = true;
            }
            args.NewItem = GetMauiItem((int)SelectedIndex);
            OnSelectionChanged(args);
          }

        /// <summary>
        /// Recalculates the positions of items to the left of the selected item.
        /// </summary>
        void RecalculateLeftPositions()
        {
            bool isDatasource = IsDataSourceAvailable();

            nint j = _selectedIndex - 1;
            if (_isVirtualization)
            {
                j = _selectedIndex - _dataSourceStartIndex - 1;
            }

            for (nint i = _dataSourceStartIndex; i < _selectedIndex; i++)
            {
                PlatformCarouselItem? leftItem = GetPlatformCarouselItem(isDatasource, i);
                if (leftItem != null)
                {
                    UpdateDefaultLeftPosition(leftItem, -(_offset * j));
                }
                j--;
            }
        }

        /// <summary>
        /// Recalculates the positions of items to the right of the selected item.
        /// </summary>
        void RecalculateRightPositions()
        {
            bool isDatasource = IsDataSourceAvailable();

            nint j = 0;
            for (nint i = _selectedIndex + 1; i <= _dataSourceEndIndex; i++)
            {
                PlatformCarouselItem? rightItem = GetPlatformCarouselItem(isDatasource, i);

                if (rightItem != null)
                {
                    UpdateDefaultRightPosition(rightItem, _offset * j);
                }

                j++;
                if (_allowLoadMore && i == _dataSourceEndIndex)
                {
                    UpdateDefaultRightPosition(_loadMoreItem, _offset * j);
                }
            }
        }

        /// <summary>
        /// Centers the specified view in the carousel.
        /// </summary>
        /// <param name="view">The view to center.</param>
        void CenterView(PlatformCarouselItem view)
        {
            BringSubviewToFront(view);
            CALayer layer = view.Layer;
            CATransform3D rotationAndPerspectiveTransform = CATransform3D.Identity;

            if (_virtualView != null)
            {
                UIView.Animate((nfloat)_virtualView.Duration / 1000, () =>
                {
#pragma warning disable CA1422
                    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
#pragma warning restore CA1422

                    view.Frame = new CGRect((Bounds.Size.Width / 2) - (_itemWidth / 2), (Bounds.Size.Height / 2) - (_itemHeight / 2), _itemWidth, _itemHeight);

                    ApplyTransformBasedOnLayoutDirection(layer, rotationAndPerspectiveTransform);
                });
            }
        }

        /// <summary>
        /// Updates the center position of the carousel item in default view mode.
        /// </summary>
        /// <param name="view">The carousel item to center.</param>
        internal void UpdateCenterPosition(PlatformCarouselItem view)
        {
            if (_selectedIndex != view.Index)
            {
                SelectedIndex = view.Index;

                RecalculateRightPositions();
                RecalculateLeftPositions();
            }

            CenterView(view);
        }

        /// <summary>
        /// Raises the SelectionChanged event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        internal virtual void OnSelectionChanged(SelectionChangedEventArgs args)
        {
			if (_virtualView != null)
			{
				if (_virtualView.SelectedIndex != (int)_selectedIndex)
				{
					_virtualView.SelectedIndex = (int)_selectedIndex;
				}

				_virtualView.RaiseSelectionChanged(args);
			}
		}

        /// <summary>
        /// Raises the SwipeStarted event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        internal virtual void OnSwipeStarted(SwipeStartedEventArgs args)
        {
            _virtualView?.RaiseSwipeStarted(args);
        }

        /// <summary>
        /// Raises the SwipeEnded event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        internal virtual void OnSwipeEnded(EventArgs args)
        {
            _virtualView?.RaiseSwipeEnded(args);
        }

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        internal virtual void OnCollectionChanged(CollectionChangedEventArgs args)
        {
            if (CollectionChanged != null && CollectionChanged != null)
            {
                CollectionChanged(this, args);
            }
        }

        /// <summary>
        /// Loads additional items into the carousel.
        /// </summary>
        internal void LoadOtherItem()
        {
            PlatformCarouselItem item = [];
            int count = _maximumVisibleCount;
            int tempCount = GetItemCount(); 

            if ((tempCount - _dataSourceEndIndex - 1) < _maximumVisibleCount)
            {
                count = (int)(tempCount - _dataSourceEndIndex - 1);
            }

            if (_viewMode == ViewMode.Default)
            {
                if ((tempCount - _dataSourceEndIndex - 1) > 0)
                {
                    LoadMoreItem.RemoveFromSuperview();
                }

                object? convertItem;
                for (int i = 0; i < count; i++)
                {
                    _dataSourceEndIndex++;

                    if (_isLinearVirtual)
                    {
                        if (_linearVirtualDataSource is IList list)
                        {
                            convertItem = PlatformCarousel.GetConvertItem(_linearVirtualDataSource, (int)_dataSourceEndIndex);
                            SetConversionArguments(convertItem, _dataSourceEndIndex);
                            if (_dataSource != null)
                            {
                                item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_dataSourceEndIndex);
                            }
                        }
                    }
                    else
                    {
                        if (_dataSource != null)
                        {
                            item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_dataSourceEndIndex);
                        }
                    }
                    AddItemToSubView(item, _dataSourceEndIndex);

                    if (_allowLoadMore && _dataSourceEndIndex + 1 < tempCount)
                    {
                        if (i + 1 == count)
                        {
                            AddLoadMoreItem();
                        }
                    }
                }

                LinearViewLoading();
            }
            else
            {
                LoadItemsForLinearMode(tempCount, ref item);
            }
        }

        /// <summary>
        /// Adds the "Load More" item to the carousel.
        /// </summary>
        void AddLoadMoreItem()
        {
            LoadMoreItem.Frame = GetFrameRect();
            LoadMoreItem.Index = _dataSourceEndIndex + 1;
            LoadMoreItem.InternalCarousel = this;
            LoadMoreItem.IsAccessibilityElement = true;
            LoadMoreAccessibility();
            AddSubview(LoadMoreItem);
        }

        /// <summary>
        /// Loads items for the linear view mode.
        /// </summary>
        /// <param name="tempCount">Total count of items.</param>
        /// <param name="item">Reference to a PlatformCarouselItem.</param>
        void LoadItemsForLinearMode(int tempCount, ref PlatformCarouselItem item)
        {
            int count = _maximumVisibleCount;
            if (_linearDataSource != null)
            {
                if ((tempCount - (int)_linearDataSource.Count - 1) < _maximumVisibleCount)
                {
                    count = tempCount - (int)_linearDataSource.Count;
                }

                count += (int)_linearDataSource.Count;

                for (int i = (int)_linearDataSource.Count; i < count; i++)
                {
                    if (_dataSource != null && IsDataSourceAvailable())
                    {
                        _linearDataSource.Add(_dataSource.GetItem<NSObject>((nuint)i));
                    }
                }
            }
            LinearMode.ViewCollection?.ReloadData();
        }

        /// <summary>
        /// Sets the conversion arguments for an item.
        /// </summary>
        /// <param name="convertItem">The item to convert.</param>
        /// <param name="index">The index of the item.</param>
        void SetConversionArguments(object? convertItem, nint index)
        {
			ConversionEventArgs arg = new ConversionEventArgs
			{
				Index = (int)index,
				Item = convertItem
			};
			OnConversion(arg);

        }

        /// <summary>
        /// Adds a carousel item to the subview.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="index">The index at which to add the item.</param>
        /// <param name="isRightSideItem">Whether to use the same frame for positioning.</param>
        void AddItemToSubView(PlatformCarouselItem item, nint index, bool isRightSideItem = true)
        {
            item.Index = (int)index;
            item.Frame = GetItemFrameRect(isRightSideItem,(int)index);
            if (item.View != null)
            {
                item.View.Frame = item.Subviews[0].Frame;
                item.UserInteractionEnabled = IsInteractionEnable();
                item.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
                item.AddSubview(item.View);
            }
            ConfigureCarouselItem(item);
        }


        CGRect GetItemFrameRect(bool isRightSideItem, int index)
        {
            nfloat x = isRightSideItem ? (Bounds.Size.Width / 2) + _selectedItemOffset + (index * _offset) : -(Bounds.Size.Width / 2) + _selectedItemOffset + (index * _offset);
            return new CGRect(x, (Bounds.Size.Height / 2) - (_itemHeight / 2), _itemWidth, _itemHeight);
        }

        /// <summary>
        /// Handles the conversion of items.
        /// </summary>
        /// <param name="args">Conversion event arguments.</param>
        internal virtual void OnConversion(ConversionEventArgs args)
        {
			ConversionInvoked?.Invoke(this, args);
		}

        /// <summary>
        /// Releases the unmanaged resources used by the PlatformCarousel and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">If set to true, release both managed and unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_dataSource != null)
                {
                    _dataSource.RemoveAllObjects();
                    _dataSource = null;
                }

                if (LinearDataSource != null)
                {
                    LinearDataSource.RemoveAllObjects();
                    LinearDataSource = null;
                }

                if (LoadMoreItem != null)
                {
                    LoadMoreItem.RemoveFromSuperview();
                    LoadMoreItem.Dispose();
                }

                if (LinearMode != null)
                {
                    LinearMode.RemoveFromSuperview();
                    LinearMode.Dispose();
                }

                if (_linearVirtualDataSource != null)
                {
                    _linearVirtualDataSource = null;
                }

                foreach (var subView in Subviews)
                {
                    subView.RemoveFromSuperview();
                    subView.Dispose();
                }
            }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize()
        {
            InitializeDataSources();
            InitializeDefaultValues();
            InitializeGestureRecognizers();
        }

        /// <summary>
        /// Initialize the gestures 
        /// </summary>
        void InitializeGestureRecognizers()
        {
            _gestureRecognizer = new UIPanGestureRecognizer(PanHandle);
            _customGesture = new CustomCarouselPanGesture(this);
            _gestureRecognizer.Delegate = _customGesture;
            AddGestureRecognizer(_gestureRecognizer);
        }

        /// <summary>
        /// Initialize the gestures 
        /// </summary>
        void InitializeDefaultValues()
        {
            _tempX = 20;
            _isCarouselInvoked = false;
            _selectedIndex = 0;
            _itemSpacing = 5;
            _rotationAngle = Angle45;
            _offset = 60;
            _isSelectedUpdated = false;
            _itemWidth = 170;
            _itemHeight = 300;
            _duration = 0.60f;
            _touchX = 100;
            _lastX = 0;
            _scaleOffset = (nfloat)0.7;
            _selectedItemOffset = 60;
            _increment = MaxIncrement;
            ClipsToBounds = false;
            _allowLoadMore = false;
            IsLoaded = true;
            _viewMode = ViewMode.Default;
        }

        /// <summary>
        /// Initialize the data sources 
        /// </summary>
        void InitializeDataSources()
        {
            _dataSource = [];
            _linearDataSource = [];
            _linearVirtualDataSource = new ObservableCollection<object>();
        }

        /// <summary>
        /// Gets the frame rectangle for a carousel item.
        /// </summary>
        /// <returns>A CGRect representing the frame of a carousel item.</returns>
        CGRect GetFrameRect()
        {
            return new CGRect((Bounds.Size.Width / 2) - (_itemWidth / 2), (Bounds.Size.Height / 2) - (_itemHeight / 2), _itemWidth, _itemHeight);
        }

        /// <summary>
        /// Handles the pan gesture for carousel interaction.
        /// </summary>
        void PanHandle()
        {
            if (_gestureRecognizer == null)
            {
                return;
            }

            CGPoint stopLocation = new CGPoint(0, 0);
            SwipeStartEventArgs ??= new SwipeStartedEventArgs();

            switch (_gestureRecognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    HandleGestureBegan();
                    break;
                case UIGestureRecognizerState.Changed:
                    HandleGestureChanged(ref stopLocation);
                    break;
                case UIGestureRecognizerState.Ended:
                case UIGestureRecognizerState.Cancelled:
                    HandleGestureEnded();
                    break;
            }

            _lastX = stopLocation.X;
        }

        /// <summary>
        /// Handles the gesture when it changes state.
        /// </summary>
        /// <param name="stopLocation">Reference to the stop location of the gesture.</param>
        void HandleGestureChanged(ref CGPoint stopLocation)
        {
            if (_gestureRecognizer == null)
			{
				return;
			}

			stopLocation = _gestureRecognizer.TranslationInView(this);
            nfloat velocityX = _gestureRecognizer.VelocityInView(this).X;
            nfloat diff = _lastX - stopLocation.X;

            if (diff < 0.0f)
            {
                HandleSwipeLeft(ref stopLocation, velocityX);
            }
            else if (diff > 0.0f)
            {
                HandleSwipeRight(ref stopLocation, velocityX);
            }
        }

        /// <summary>
        /// Handles the swipe right gesture.
        /// </summary>
        /// <param name="stopLocation">Reference to the stop location of the gesture.</param>
        /// <param name="velocityX">The velocity of the swipe in the X direction.</param>
        void HandleSwipeRight(ref CGPoint stopLocation, nfloat velocityX)
        {
            if (_switchFloat == 0)
            {
                _increment = MaxIncrement;

                if (_tempX < -MaxIncrement)
                {
                    _increment = MinIncrement;
                }

                _tempX -= _increment;
                _switchFloat = 1;
            }

            if (_tempX > stopLocation.X)
            {
                _increment = MaxIncrement;

                if (_tempX < -MaxIncrement)
                {
                    _increment = MinIncrement;
                }

                if (_tempX != 0)
                {
                    if (SwipeStartEventArgs == null)
                    {
                        return;
                    }
                    if (EnableInteraction)
                    {
                        if (SwipeMovementMode == SwipeMovementMode.MultipleItems)
                        {
                            if (!SwipeEventCalled)
                            {
                                if (DataSource != null && SelectedIndex < (nint)DataSource.Count - 1)
                                {
                                    OnMultipleSwipe(false);
                                }
                                else
                                {
                                    _isSwipeRestricted = true;
                                }
                            }

                            MoveNext();
                        }

                        SwipeNext();
                    }
                }

                _tempX -= _increment;
            }

        }

        /// <summary>
        /// Handles the swipe left gesture.
        /// </summary>
        /// <param name="stopLocation">Reference to the stop location of the gesture.</param>
        /// <param name="velocityX">The velocity of the swipe in the X direction.</param>
        void HandleSwipeLeft(ref CGPoint stopLocation, nfloat velocityX)
        {
            _lastValue = velocityX;
            if (_switchFloat == 1)
            {
                _increment = MaxIncrement;
                if (_tempX < - MaxIncrement)
                {
                    _increment = MinIncrement;
                }

                _tempX += _increment;
                _switchFloat = 0;
            }

            if (stopLocation.X > _tempX)
            {
                if (SwipeStartEventArgs == null)
                {
                    return;
                }
                _increment = MaxIncrement;
                if (_tempX > MaxIncrement)
                {
                    _increment = MinIncrement;
                }

                if (_tempX != 0)
                {
                    if (EnableInteraction)
                    {
                        if (SwipeMovementMode == SwipeMovementMode.MultipleItems)
                        {
                            if (!SwipeEventCalled)
                            {
                                if (SelectedIndex > 0)
                                {
                                    OnMultipleSwipe(true);
                                }
                                else
                                {
                                    _isSwipeRestricted = true;
                                }
                            }

                            MovePrevious();
                        }

                        SwipePrevious();
                    }
                }

                _touchX += _touchX;
                _tempX += _increment;
            }
        }

        void OnMultipleSwipe(bool isLeftSwipe)
        {
            if(SwipeStartEventArgs==null)
            {
                return;
            }
            _isSwipeRestricted = false;
            if(isLeftSwipe)
            {
                SwipeStartEventArgs.IsSwipedLeft = true;
            }
            else
            {
                SwipeStartEventArgs.IsSwipedLeft = false;
            }
            
            OnSwipeStarted(SwipeStartEventArgs);
            SwipeEventCalled = true;
        }

        /// <summary>
        /// Handles the beginning of a gesture.
        /// </summary>
        void HandleGestureBegan()
        {
            if (_gestureRecognizer != null)
            {
                _tempX = 0;
                CGPoint startLocation = _gestureRecognizer.TranslationInView(this);
                _touchX = MaxIncrement;
                _increment = MaxIncrement;
                _isSwipeRestricted = true;
            }
        }

        /// <summary>
        /// Handles the end of a gesture.
        /// </summary>
        void HandleGestureEnded()
        {
            _tempX = 0;
            if (!_isSwipeRestricted)
            {
                OnSwipeEnded(EventArgs.Empty);
            }

            SwipeEventCalled = false;
            _isSwipeRestricted = false;
            _canExecuteSwipe = true;
        }

        /// <summary>
        /// Executes a swipe to the next item if conditions are met.
        /// </summary>
        void SwipeNext()
        {
            if (SwipeMovementMode == SwipeMovementMode.SingleItem && _canExecuteSwipe)
            {
                if (!SwipeEventCalled)
                {
                    if (DataSource != null && SelectedIndex < (nint)DataSource.Count - 1)
                    {
                        _isSwipeRestricted = false;
                        if (SwipeStartEventArgs != null)
                        {
                            SwipeStartEventArgs.IsSwipedLeft = false;
                            OnSwipeStarted(SwipeStartEventArgs);
                        }
                        SwipeEventCalled = true;
                    }
                    else
                    {
                        _isSwipeRestricted = true;
                    }
                }

                MoveNext();

                _canExecuteSwipe = false;
            }
        }

        /// <summary>
        /// Executes a swipe to the previous item if conditions are met.
        /// </summary>
        void SwipePrevious()
        {
            if (SwipeMovementMode == SwipeMovementMode.SingleItem && _canExecuteSwipe)
            {
                if (!SwipeEventCalled)
                {
                    if (SelectedIndex > 0)
                    {
                        _isSwipeRestricted = false;
                        if (SwipeStartEventArgs != null)
                        {
                            SwipeStartEventArgs.IsSwipedLeft = true;
                            OnSwipeStarted(SwipeStartEventArgs);
                        }
                        SwipeEventCalled = true;
                    }
                    else
                    {
                        _isSwipeRestricted = true;
                    }
                }

                MovePrevious();

                _canExecuteSwipe = false;
            }
        }

        bool IsDataSourceAvailable()
        {
            return _dataSource != null && _dataSource.Count > 0;
        }

        /// <summary>
        /// Sets the selected index of the carousel.
        /// </summary>
        /// <param name="value">The new index to set.</param>
        void SetSelectedIndex(nint value)
        {
            int tempCount = GetItemCount();

            _updatedSelectedValue = value;
            if (value < tempCount)
            {
                _tappedDifference = value - _selectedIndex;
                if (IsDataSourceAvailable())
                {
                    int count = tempCount - 1;
                    if (LoadMoreItemsCount > 0 && _viewMode == ViewMode.Default)
                    {
                        count = (int)_dataSourceEndIndex;
                    }
                    else if (_viewMode == ViewMode.Linear && _linearDataSource != null)
                    {
                        count = (int)_linearDataSource.Count;
                    }

                    if (count >= value)
                    {
                        _isSelectedUpdated = true;
                        SelectionChangedEventArgs args = new SelectionChangedEventArgs();

                        if (!SwipeEventCalled)
                        {
                            args.OldItem = GetMauiItem((int)_selectedIndex);
                        }

                        _selectedIndex = value;
                        PlatformCarouselItem? newValue = GetNewItem();

                        if (_isVirtualization && !IsTapped && newValue != null && !newValue.IsDescendantOfView(this) && _viewMode == ViewMode.Default)
                        {
                            UpdateViewMode();
                        }

                        HandleViewModeUpdate(tempCount,args);
                    }

                    SetNeedsDisplay();
                }
            }
        }

        /// <summary>
        /// Retrieves a new carousel item based on the data source.
        /// </summary>
        /// <returns>A new PlatformCarouselItem.</returns>
        PlatformCarouselItem? GetNewItem()
        {
            PlatformCarouselItem? newValue = [];
            if (_dataSource != null)
            {
                if (_dataSource.GetItem<NSObject>((nuint)_selectedIndex) as PlatformCarouselItem == null && _isLinearVirtual)
                {
                    object convertItem = _dataSource.GetItem<NSObject>((nuint)_selectedIndex);
                    SetConversionArguments(convertItem, _selectedIndex);
                }

                if (_isVirtualization && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear)
                {
                    if (_linearVirtualDataSource is IList list && list != null)
                    {
                        var obj = list[(int)_selectedIndex];
                        if (obj != null)
                        {
                            newValue = GetVirtualizedItem(obj, _selectedIndex);
                            if (newValue != null)
                            {
                                newValue.Index = _selectedIndex;
                            }
                        }
                    }
                }
                else
                {
                    newValue = _dataSource.GetItem<PlatformCarouselItem>((nuint)_selectedIndex);
                }

            }

            return newValue;
        }

        /// <summary>
        /// Handles updating the view mode based on the current state.
        /// </summary>
        /// <param name="tempCount">The total count of items.</param>
        /// <param name="args">The SelectionChangedEventArgs for the update.</param>
        void HandleViewModeUpdate(int tempCount, SelectionChangedEventArgs args)
        {
            if (_viewMode == ViewMode.Default)
            {
                if (tempCount > 0)
                {
                    DefaultViewLoading();
                    if (!_isCarouselInvoked && !SwipeEventCalled)
                    {
                        args.NewItem = GetMauiItem((int)SelectedIndex);
                        OnSelectionChanged(args);
                    }
                    else
                    {
                        _isCarouselInvoked = false;
                    }
                }
            }
            else
            {
                if (LinearMode != null)
                {
                    NSIndexPath pathValue = NSIndexPath.FromRowSection(_selectedIndex, 0);
                    LinearMode.ViewCollection?.ScrollToItem(pathValue, UICollectionViewScrollPosition.CenteredHorizontally, false);
					args.NewItem = GetMauiItem((int)SelectedIndex);
					OnSelectionChanged(args);
				}
            }
        }

		/// <summary>
		/// Retrieves an item from the source list at the specified index.
		/// </summary>
		/// <param name="sourceList">The source list to retrieve the item from.</param>
		/// <param name="index">The index of the item to retrieve.</param>
		/// <returns>The item at the specified index, or null if not found.</returns>
		static object? GetConvertItem(object? sourceList, int index)
        {
            object? obj = null;
            if (sourceList != null && sourceList is IList list)
            {
                obj = list[index];
            }
            return obj;
        }

        /// <summary>
        /// Arranges a single item to the right of the selected item.
        /// </summary>
        /// <param name="index">The index of the item to arrange.</param>
        /// <param name="offset">The offset for positioning the item.</param>
        private void ArrangeRightItem(nint index, ref nint offset)
        {
            PlatformCarouselItem? rightItem = GetLeftAndRightItem(index);
            if (rightItem != null)
            {
                UpdateDefaultRightPosition(rightItem, _offset * offset);
            }

            if (_allowLoadMore && index == _dataSourceEndIndex)
            {
                UpdateDefaultRightPosition(LoadMoreItem, _offset * (offset + 1));
            }
        }

        /// <summary>
        /// Loads the default view of the carousel.
        /// </summary>
        void DefaultViewLoading()
        {
            HandleDeafaultVirtualizationIfNeeded();

            PlatformCarouselItem? view = GetLeftAndRightItem(_selectedIndex);

            nint offset = 0;
            ArrangeDefaultRightItems(ref offset);

            ArrangeDeafultLeftItems(ref offset);
            ArrangeCenterSelectedItem(view);
        }

        /// <summary>
        /// Handles virtualization scenarios if needed.
        /// </summary>
        private void HandleDeafaultVirtualizationIfNeeded()
        {
            if (_isVirtualization && _isLinearVirtual && IsTapped)
            {
                HandleTapVirtualization();
            }
            else if (_isVirtualization && IsTapped)
            {
                HandleTapWithoutXForms();
            }
        }

        /// <summary>
        /// Arranges items to the right of the selected item.
        /// </summary>
        private void ArrangeDefaultRightItems(ref nint offset)
        {
            for (nint i = _selectedIndex + 1; i <= _dataSourceEndIndex; i++)
            {
                ArrangeRightItem(i, ref offset);
                offset++;
            }

            if (_allowLoadMore && _selectedIndex == _dataSourceEndIndex)
            {
                UpdateDefaultRightPosition(LoadMoreItem, _offset * offset);
            }
        }

        /// <summary>
        /// Centers the selected item in the view.
        /// </summary>
        /// <param name="selectedItem">The item to be centered.</param>
        void ArrangeCenterSelectedItem(PlatformCarouselItem? selectedItem)
        {
            if (selectedItem != null)
            {
                BringSubviewToFront(selectedItem);
                UpdateCenterPosition(selectedItem);
            }
        }

        /// <summary>
        /// Arranges items to the left of the selected item.
        /// </summary>
        void ArrangeDeafultLeftItems(ref nint offset)
        {
            offset = CalculateLeftOffset();
            for (nint i = _dataSourceStartIndex; i < _selectedIndex; i++)
            {
                ArrangeLeftItem(i, offset);
                offset--;
            }
        }

        /// <summary>
        /// Arranges a single item to the left of the selected item.
        /// </summary>
        /// <param name="index">The index of the item to arrange.</param>
        /// <param name="offset">The offset for positioning the item.</param>
        void ArrangeLeftItem(nint index, nint offset)
        {
            PlatformCarouselItem? leftItem = GetLeftAndRightItem(index);
            if (leftItem != null)
            {
                UpdateDefaultLeftPosition(leftItem, -(_offset * offset));
            }
        }

        /// <summary>
        /// Calculates the initial offset for left items.
        /// </summary>
        /// <returns>The calculated offset.</returns>
        nint CalculateLeftOffset()
        {
            if (_isVirtualization)
            {
                return _selectedIndex - _dataSourceStartIndex - 1;
            }
            return _selectedIndex - 1;
        }

		/// <summary>
		/// Handles tap events when virtualization is enabled without XForms.
		/// </summary>
		void HandleTapWithoutXForms()
        {
            int tempCount = GetItemCount();

            if (_tappedDifference > 0)
            {
                LoadForwardItemsWithoutXForms(tempCount);
            }
            else if (_tappedDifference < 0)
            {
                LoadBackwardItemsWithoutXForms(tempCount);
            }

            _tappedDifference = 0;
            IsTapped = false;
        }

        /// <summary>
        /// Loads forward items when virtualization is enabled without XForms.
        /// </summary>
        /// <param name="tempCount">The total count of items.</param>
        void LoadForwardItemsWithoutXForms(int tempCount)
        {
            for (int i = 0; i < Math.Abs(_tappedDifference); i++)
            {
                PlatformCarouselItem item;
                if (_dataSourceEndIndex + 1 < tempCount)
                {
                    _dataSourceEndIndex++;
                    if (_dataSource != null)
                    {
                        item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_dataSourceEndIndex);
                        AddItemToSubView(item, _dataSourceEndIndex);
                    }

                    if ((int)_dataSourceStartIndex < _selectedIndex)
                    {
                        PlatformCarouselItem? deletedItem = null;
                        if (_dataSource != null)
                        {
                            deletedItem = _dataSource.GetItem<PlatformCarouselItem>((nuint)_dataSourceStartIndex);
                        }

                        if (deletedItem != null && (deletedItem.Frame.X + deletedItem.Frame.Size.Width) < Frame.X)
                        {
                            deletedItem.RemoveFromSuperview();
                            _dataSourceStartIndex++;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Loads backward items when virtualization is enabled without XForms.
        /// </summary>
        /// <param name="tempCount">The total count of items.</param>
        void LoadBackwardItemsWithoutXForms(int tempCount)
        {
            PlatformCarouselItem item;
            for (int i = 0; i < Math.Abs(_tappedDifference); i++)
            {
                if (_dataSourceStartIndex - 1 != -1)
                {
                    _dataSourceStartIndex--;
                    if (_dataSource != null)
                    {
                        item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_dataSourceStartIndex);
                        item.Index = _dataSourceStartIndex;
                        item.Frame = new CGRect(-Bounds.Size.Width, (Bounds.Size.Height / 2) - (_itemHeight / 2) + ((_itemHeight / 2) * (1 - _scaleOffset)), _itemWidth, _itemHeight - ((_itemHeight / 2) * (1 - _scaleOffset) * 2));
                        ConfigureItemView(item);
                        ConfigureCarouselItem(item);
                    }
                    
                    if ((int)_dataSourceEndIndex > _selectedIndex)
                    {
                        PlatformCarouselItem deletedItem = [];
                        
                        if (_dataSource != null)
                        {
                            deletedItem = _dataSource.GetItem<PlatformCarouselItem>((nuint)_dataSourceEndIndex);
                        }

                        if (IsItemOutOfView(deletedItem))
                        {
                            deletedItem.RemoveFromSuperview();
                            _dataSourceEndIndex--;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Handles tap events when virtualization is enabled with XForms.
        /// </summary>
        void HandleTapVirtualization()
        {
            if (_tappedDifference > 0)
            {
                LoadForwardItems();
            }
            else if (_tappedDifference < 0)
            {
                LoadBackwardItems();
            }

            _tappedDifference = 0;
            IsTapped = false;
        }

        /// <summary>
        /// Handles tap events when virtualization is enabled with XForms.
        /// </summary>
        void LoadForwardItems()
        {
            object? convertItem;
            for (int i = 0; i < Math.Abs(_tappedDifference); i++)
            {
				if (_dataSource != null && _dataSourceEndIndex + 1 < (int)_dataSource.Count)
				{
					_dataSourceEndIndex++;
					convertItem = PlatformCarousel.GetConvertItem(_linearVirtualDataSource, i);
					SetConversionArguments(convertItem, _dataSourceEndIndex);
					PlatformCarouselItem? item;
					if (convertItem != null && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear)
					{
						item = GetVirtualizedItem(convertItem, _dataSourceEndIndex);
					}
					else
					{
						item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_dataSourceEndIndex);
					}

					if (item != null)
					{
						AddItemToSubView(item, _dataSourceEndIndex);
						if ((int)_dataSourceStartIndex < _selectedIndex)
						{
							PlatformCarouselItem? deletedItem;

							if (_virtualView != null && _virtualView.ViewMode == ViewMode.Linear)
							{
								if (_linearVirtualDataSource is IList list && list[(int)_dataSourceEndIndex] is PlatformCarouselItem carouselItem && carouselItem != null)
								{
									deletedItem = GetVirtualizedItem(item, _dataSourceEndIndex);
								}
								else
								{
									deletedItem = null;
								}
							}
							else
							{
								deletedItem = _dataSource?.GetItem<PlatformCarouselItem>((nuint)_dataSourceStartIndex);
							}

							if (deletedItem != null && (deletedItem.Frame.X + deletedItem.Frame.Size.Width) < Frame.X)
							{
								deletedItem.RemoveFromSuperview();
								_dataSourceStartIndex++;
							}
						}
					}
				}
			}
        }

        /// <summary>
        /// Loads backward items when virtualization is enabled.
        /// </summary>
        void LoadBackwardItems()
        {
            for (int i = 0; i < Math.Abs(_tappedDifference); i++)
            {
                if (_dataSourceStartIndex - 1 == -1)
				{
					break;
				}

				_dataSourceStartIndex--;
                PlatformCarouselItem? item = CreateAndConfigureBackwardItem();

                if (item != null)
                {
                    AddBackwardItemToView(item);
                    RemoveExcessForwardItem();
                }
            }
        }


        /// <summary>
        /// Creates and configures an item for backward loading.
        /// </summary>
        /// <returns>A configured PlatformCarouselItem, or null if creation fails.</returns>
        PlatformCarouselItem? CreateAndConfigureBackwardItem()
        {
            object? convertItem = PlatformCarousel.GetConvertItem(_linearVirtualDataSource, (int)_dataSourceStartIndex);
            SetConversionArguments(convertItem, _dataSourceStartIndex);

            PlatformCarouselItem? item = GetCarouselItem(convertItem);
            if (item == null)
			{
				return null;
			}

			ConfigureBackwardItem(item);
            return item;
        }

        /// <summary>
        /// Retrieves a carousel item based on the current configuration.
        /// </summary>
        /// <param name="convertItem">The item to be converted.</param>
        /// <returns>A PlatformCarouselItem, or null if retrieval fails.</returns>
        PlatformCarouselItem? GetCarouselItem(object? convertItem)
        {
            if (IsLinearVirtualMode(convertItem) && convertItem != null)
            {
                return GetVirtualizedItem(convertItem, _dataSourceEndIndex);
            }
            return _dataSource?.GetItem<PlatformCarouselItem>((nuint)_dataSourceEndIndex);
        }

        /// <summary>
        /// Determines if the carousel is in linear virtual mode.
        /// </summary>
        /// <param name="convertItem">The item to be converted.</param>
        /// <returns>True if in linear virtual mode, false otherwise.</returns>
        bool IsLinearVirtualMode(object? convertItem)
        {
            return convertItem != null && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear;
        }

        /// <summary>
        /// Configures a backward item with necessary properties and layout.
        /// </summary>
        /// <param name="item">The item to be configured.</param>
        void ConfigureBackwardItem(PlatformCarouselItem item)
        {
            item.Index = _dataSourceStartIndex;
            CarouselItemAccessibility(item, item.Index);
            item.InternalCarousel = this;
            item.Frame = CalculateBackwardItemFrame();
            ConfigureItemView(item);
            item.IsAccessibilityElement = true;
        }

        /// <summary>
        /// Calculates the frame for a backward item.
        /// </summary>
        /// <returns>A CGRect representing the frame for the backward item.</returns>
        CGRect CalculateBackwardItemFrame()
        {
            return new CGRect(
                -Bounds.Size.Width,
                (Bounds.Size.Height / 2) - (_itemHeight / 2) + ((_itemHeight / 2) * (1 - _scaleOffset)),
                _itemWidth,
                _itemHeight - ((_itemHeight / 2) * (1 - _scaleOffset) * 2)
            );
        }

        /// <summary>
        /// Configures the view of an item.
        /// </summary>
        /// <param name="item">The item whose view is to be configured.</param>
        void ConfigureItemView(PlatformCarouselItem item)
        {
            if (item.View != null)
            {
                item.View.Frame = item.Subviews[0].Frame;
                item.UserInteractionEnabled = IsInteractionEnable();
                item.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
                item.AddSubview(item.View);
            }
        }

        /// <summary>
        /// Adds a backward item to the view.
        /// </summary>
        /// <param name="item">The item to be added to the view.</param>
        void AddBackwardItemToView(PlatformCarouselItem item)
        {
            AddSubview(item);
        }

        /// <summary>
        /// Removes excess forward items that are no longer visible.
        /// </summary>
        void RemoveExcessForwardItem()
        {
            if ((int)_dataSourceEndIndex <= _selectedIndex)
			{
				return;
			}

			PlatformCarouselItem? deletedItem = GetItemToDelete();
            if (deletedItem != null && IsItemOutOfView(deletedItem))
            {
                deletedItem.RemoveFromSuperview();
                _dataSourceEndIndex--;
            }
        }

        /// <summary>
        /// Gets the item that should be considered for deletion.
        /// </summary>
        /// <returns>A PlatformCarouselItem that might be deleted, or null if none found.</returns>
        PlatformCarouselItem? GetItemToDelete()
        {
            if (IsLinearVirtualMode(null))
            {
                return GetVirtualItemToDelete();
            }
            return _dataSource?.GetItem<PlatformCarouselItem>((nuint)_dataSourceStartIndex);
        }

        /// <summary>
        /// Gets a virtual item that should be considered for deletion.
        /// </summary>
        /// <returns>A virtualized PlatformCarouselItem that might be deleted, or null if none found.</returns>
        PlatformCarouselItem? GetVirtualItemToDelete()
        {
            if (_linearVirtualDataSource is IList list && list.Count > _dataSourceEndIndex)
            {
                object? obj = list[(int)_dataSourceEndIndex];
                return obj != null ? GetVirtualizedItem(obj, _dataSourceEndIndex) : null;
            }
            return null;
        }

        /// <summary>
        /// Determines if an item is out of the view bounds.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True if the item is out of view, false otherwise.</returns>
        bool IsItemOutOfView(PlatformCarouselItem item)
        {
            return item.Frame.X > Frame.Size.Width + Frame.X;
        }

        /// <summary>
        /// Configures properties of the PlatformCarouselItem.
        /// </summary>
        /// <param name="item">The PlatformCarouselItem to configure.</param>
        void ConfigureCarouselItem(PlatformCarouselItem item)
        {
            item.InternalCarousel = this;
            item.IsAccessibilityElement = true;
            CarouselItemAccessibility(item, item.Index);
            AddSubview(item);
        }

        /// <summary>
        /// Loading this instance of linear mode.
        /// </summary>
        void LinearViewLoading()
        {
            int tempCount = GetItemCount();
            nint offset = 0;
            if (tempCount <= 0)
			{
				return;
			}

			PlatformCarouselItem? selectedItem = GetSelectedItem();
            if (selectedItem == null)
			{
				return;
			}

			ArrangeRightItems(ref offset);
            ArrangeLeftItems(ref offset);
            CenterSelectedItem(selectedItem);
        }

        /// <summary>
        /// Gets the selected item based on the current configuration.
        /// </summary>
        /// <returns>The selected PlatformCarouselItem, or null if not found.</returns>
        PlatformCarouselItem? GetSelectedItem()
        {
            if (_isVirtualization && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear)
            {
                return GetVirtualizedSelectedItem();
            }
            return _dataSource?.GetItem<PlatformCarouselItem>((nuint)_selectedIndex);
        }

        /// <summary>
        /// Gets the virtualized selected item.
        /// </summary>
        /// <returns>The virtualized PlatformCarouselItem, or null if not found.</returns>
        PlatformCarouselItem? GetVirtualizedSelectedItem()
        {
            if (_linearVirtualDataSource is IList list && list.Count > _selectedIndex)
            {
                var listItem = list[(int)_selectedIndex];
                if (listItem != null)
                {
                    var item = GetVirtualizedItem(listItem, _selectedIndex);
                    if (item != null)
                    {
                        item.Index = _selectedIndex;
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Arranges items to the right of the selected item.
        /// </summary>
        void ArrangeRightItems(ref nint offset)
        {
            for (nint i = _selectedIndex + 1; i <= _dataSourceEndIndex; i++)
            {
                PlatformCarouselItem? rightItem = GetLeftAndRightItem(i);
                if (rightItem != null)
                {
                    UpdateLinearRightPosition(rightItem, _offset * offset);
                }
                offset++;

                if (ShouldAddLoadMoreItem(i))
                {
                    UpdateLinearRightPosition(LoadMoreItem, _offset * offset);
                }
            }

            if (_allowLoadMore && _selectedIndex == _dataSourceEndIndex)
            {
                UpdateLinearRightPosition(LoadMoreItem, _offset * offset);
            }
        }

        /// <summary>
        /// Determines if the load more item should be added at the current index.
        /// </summary>
        /// <param name="index">The current index being processed.</param>
        /// <returns>True if the load more item should be added; otherwise, false.</returns>
        bool ShouldAddLoadMoreItem(nint index)
        {
            return _allowLoadMore && index == _dataSourceEndIndex;
        }

        /// <summary>
        /// Arranges items to the left of the selected item.
        /// </summary>
        void ArrangeLeftItems(ref nint offset)
        {
            offset = _isVirtualization ? _selectedIndex - _dataSourceStartIndex - 1 : _selectedIndex - 1;

            for (nint i = _dataSourceStartIndex; i < _selectedIndex; i++)
            {
                PlatformCarouselItem? leftItem = GetLeftAndRightItem(i);
                if (leftItem != null)
                {
                    UpdateLinearLeftPosition(leftItem, -(_offset * offset));
                }
                offset--;
            }
        }

        /// <summary>
        /// Centers the selected item in the view.
        /// </summary>
        /// <param name="selectedItem">The item to be centered.</param>
        void CenterSelectedItem(PlatformCarouselItem selectedItem)
        {
            BringSubviewToFront(selectedItem);
            UpdateLinearCenterPosition(selectedItem);
        }
        /// <summary>
        /// Arranges the item.
        /// </summary>
        void ArrangeItem()
        {
            int tempCount = GetItemCount();

            if (!_isVirtualization && !_allowLoadMore && !_isLinearVirtual)
            {
                for (nint i = 0; i < tempCount; i++)
                {
                    PlatformCarouselItem? item = null;
                    if (_dataSource != null)
                    {
                        item = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                    }

                    if (item != null)
                    {
                        item.InternalCarousel = this;
                        item.Frame = GetFrameRect();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a carousel item at the specified index, either from the item source or the data source.
        /// </summary>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>The <see cref="PlatformCarouselItem"/> at the specified index, or null if not found.</returns>
        PlatformCarouselItem? GetLeftAndRightItem(nint index)
        {
            PlatformCarouselItem? carouselItem = [];
            if (_isVirtualization && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear)
            {
                if (_linearVirtualDataSource is IList list && list[(int)index] is var item && item != null)
                {
                    carouselItem = GetVirtualizedItem(item, index);
                    if (carouselItem != null)
                    {
                        carouselItem.Index = index;
                    }
                }
            }
            else
            {
                if (_dataSource != null)
                {
                    carouselItem = _dataSource.GetItem<PlatformCarouselItem>((nuint)index);
                }
            }
            return carouselItem;
        }

        /// <summary>
        /// Updates the center position of linear mode.
        /// </summary>
        /// <param name="view">View value.</param>
        void UpdateLinearCenterPosition(PlatformCarouselItem view)
        {
            if (_selectedIndex != view.Index)
            {
                SelectedIndex = view.Index;
                int tempCount = GetItemCount();
                nint j = 0;

                for (nint i = _selectedIndex + 1; i < tempCount; i++)
                {
                    PlatformCarouselItem? rightItem = GetLeftAndRightItem(i);
                    if (rightItem != null)
                    {
                        UpdateLinearRightPosition(rightItem, _offset * j);
                    }
                    j++;
                    if (_allowLoadMore)
                    {
                        if (i + 1 == tempCount)
                        {
                            PlatformCarouselItem item = LoadMoreItem;
                            UpdateLinearRightPosition(item, _offset * j);
                        }
                    }
                }

                j = _selectedIndex - 1;
                for (nint i = 0; i < _selectedIndex; i++)
                {
                    PlatformCarouselItem? leftItem = GetLeftAndRightItem(i);

                    if (leftItem != null)
                    {
                        UpdateLinearLeftPosition(leftItem, -(_offset * j));
                    }
                    j--;
                }
            }

            BringSubviewToFront(view);
            CALayer layer = view.Layer;
            CATransform3D rotationAndPerspectiveTransform = CATransform3D.Identity;
            view.Frame = GetFrameRect();
            ApplyTransformBasedOnLayoutDirection(layer, rotationAndPerspectiveTransform);
        }

        /// <summary>
        /// Updates the default left position of a carousel item.
        /// </summary>
        /// <param name="view">The item to update.</param>
        /// <param name="value">The offset to apply.</param>
        void UpdateDefaultLeftPosition(PlatformCarouselItem view, nfloat value)
        {
            BringSubviewToFront(view);
            CALayer layer = view.Layer;
            CATransform3D rotationAndPerspectiveTransform = PlatformCarousel.CreateRotationAndPerspectiveTransform(-0.0005f);
            nfloat tempValue = GetRotationAngle();
            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Rotate(PlatformCarousel.DegreeValue(tempValue), 0.0f, 1.0f, 0.0f);
            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Scale(_scaleOffset, _scaleOffset, 1);
            if (_virtualView != null)
            {
                UIView.Animate((nfloat)_virtualView.Duration / 1000, () =>
                {

#pragma warning disable CA1422
                    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
#pragma warning restore CA1422
                    view.Frame = new CGRect(
                        (Bounds.Size.Width / 2) - (_selectedItemOffset + _itemWidth) + value,
                        (Bounds.Size.Height - ItemHeight) / 2,
                        _itemWidth,
                        ItemHeight);

                    layer.AnchorPoint = new CGPoint(0.5, 0.5);

                    ApplyTransformBasedOnLayoutDirection(layer, rotationAndPerspectiveTransform);
                });
            }
        }

        /// <summary>
        /// Updates the left position of linear mode.
        /// </summary>
        /// <param name="view">View value.</param>
        /// <param name="value">Value list.</param>
        void UpdateLinearLeftPosition(PlatformCarouselItem view, nfloat value)
        {
            BringSubviewToFront(view);
            CALayer layer = view.Layer;
            CATransform3D rotationAndPerspectiveTransform = PlatformCarousel.CreateRotationAndPerspectiveTransform(-0.0005f);
            nfloat tempValue = GetRotationAngle();

            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Rotate(PlatformCarousel.DegreeValue(tempValue), 0.0f, 1.0f, 0.0f);
            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Scale(_scaleOffset, _scaleOffset, 1);
            view.Frame = new CGRect(
                (Bounds.Size.Width / 2) - _selectedItemOffset - _itemWidth + value,
                (Bounds.Size.Height - ItemHeight) / 2,
                _itemWidth,
                ItemHeight);
            layer.AnchorPoint = new CGPoint(0.5, 0.5);
            ApplyTransformBasedOnLayoutDirection(layer, rotationAndPerspectiveTransform);
        }

        nfloat GetRotationAngle()
        {
            nfloat tempValue = _rotationAngle;
            if (tempValue > Angle90)
            {
                tempValue = Angle360 - _rotationAngle;
            }
            return tempValue;
        }

		/// <summary>
		/// degree value calculation
		/// </summary>
		/// <param name="degree">calculation of degree</param>
		/// <returns>degree value</returns>
		static nfloat DegreeValue(nfloat degree)
        {
            return (nfloat)((Math.PI * degree) / Angle180);
        }

        /// <summary>
        /// Updates the frame of the carousel item.
        /// </summary>
        /// <param name="view">The PlatformCarouselItem whose frame is to be updated.</param>
        /// <param name="value">The offset value for positioning the item.</param>
        void UpdateViewFrame(PlatformCarouselItem view, nfloat value)
        {
            view.Frame = new CGRect(
                x: (Bounds.Size.Width / 2) + _selectedItemOffset + value,
                y: (Bounds.Size.Height - ItemHeight) / 2,
                width: _itemWidth,
                height: ItemHeight
            );
        }

		/// <summary>
		/// Creates a rotation and perspective transform for the carousel item.
		/// </summary>
		/// <param name="value">The offset value used in calculating the rotation angle.</param>
		/// <returns>A CATransform3D object representing the rotation and perspective transform.</returns>
		static CATransform3D CreateRotationAndPerspectiveTransform(nfloat value)
        {
            CATransform3D transform = CATransform3D.Identity;
            transform.M34 = value;
            return transform;
        }


        /// <summary>
        /// Updates the default right position of a carousel item.
        /// </summary>
        /// <param name="view">The item to update.</param>
        /// <param name="value">The offset to apply.</param>
        void UpdateDefaultRightPosition(PlatformCarouselItem view, nfloat value)
        {
            SendSubviewToBack(view);
            CALayer layer = view.Layer;
            CATransform3D rotationAndPerspectiveTransform = PlatformCarousel.CreateRotationAndPerspectiveTransform(-0.0005f);
            nfloat rotationAngle = GetRotationAngle();

            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Rotate(-PlatformCarousel.DegreeValue(rotationAngle), 0.0f, 1.0f, 0.0f);
            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Scale(_scaleOffset, _scaleOffset, 1);
            if (_virtualView != null)
            {
                UIView.Animate((nfloat)_virtualView.Duration / 1000, () =>
                {
#pragma warning disable CA1422
                    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
#pragma warning restore CA1422

                    UpdateViewFrame(view, value);

                    layer.AnchorPoint = new CGPoint(0.5, 0.5);

                    ApplyTransformBasedOnLayoutDirection(layer, rotationAndPerspectiveTransform);
                });
            }
        }

        /// <summary>
        /// Applies a transform to the layer based on the layout direction.
        /// </summary>
        /// <param name="layer">The layer to apply the transform to.</param>
        /// <param name="cATransform3D">The initial transform to apply.</param>
        void ApplyTransformBasedOnLayoutDirection(CALayer layer, CATransform3D cATransform3D)
        {
            CATransform3D transform = IsRTL ? CATransform3D.MakeScale(-1, 1, 1).Concat(cATransform3D) : cATransform3D;
            layer.Transform = transform;
        }

        /// <summary>
        /// Updates the right position of linear mode.
        /// </summary>
        /// <param name="view">View value.</param>
        /// <param name="value">Value list.</param>
        void UpdateLinearRightPosition(PlatformCarouselItem view, nfloat value)
        {
            SendSubviewToBack(view);
            CALayer layer = view.Layer;
            CATransform3D rotationAndPerspectiveTransform = PlatformCarousel.CreateRotationAndPerspectiveTransform((nfloat)(-0.0005));
            nfloat tempValue = GetRotationAngle();

            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Rotate(-PlatformCarousel.DegreeValue(tempValue), 0.0f, 1.0f, 0.0f);
            rotationAndPerspectiveTransform = rotationAndPerspectiveTransform.Scale(_scaleOffset, _scaleOffset, 1);
            view.Frame = new CGRect((Bounds.Size.Width / 2) + _selectedItemOffset + value,
                (Bounds.Size.Height - ItemHeight) / 2,
                _itemWidth,
                ItemHeight);
            layer.AnchorPoint = new CGPoint(0.5, 0.5);
            ApplyTransformBasedOnLayoutDirection(layer, rotationAndPerspectiveTransform);
        }

        /// <summary>
        /// Gets a virtualized item from the given object and index.
        /// </summary>
        /// <param name="carouselItem">The object to convert to a PlatformCarouselItem.</param>
        /// <param name="index">The index of the item.</param>
        /// <returns>A PlatformCarouselItem representing the virtualized item.</returns>
        internal PlatformCarouselItem? GetVirtualizedItem(object carouselItem, nint index)
        {
            if (_handler != null)
            {
                View? contentView = null;
                if (_virtualView != null && _virtualView.ItemTemplate != null)
                {
                    contentView = GetItemView(_virtualView, this, (int)index);
                }

                if (contentView != null)
                {
                    return (GetNativeViewItem(contentView, _handler));
                }
            }
            return null;
        }

        /// <summary>
        /// Configures the carousel to use virtualization in the default view mode.
        /// </summary>
        /// <param name="item">A reference to the current <see cref="PlatformCarouselItem"/>.</param>
        /// <param name="j">A reference to the current index offset.</param>
        void SetVirtualizationDefaultMode(ref PlatformCarouselItem item, ref int j)
        {

            _dataSourceEndIndex = _selectedIndex;
            _dataSourceStartIndex = _selectedIndex;

            if (_linearVirtualDataSource != null && ((IList)_linearVirtualDataSource).Count > 0)
            {
                LoadVirtualizationItemsForward(ref item, ref j);
                LoadVirtualizationItemsBackward(ref item, ref j);
            }

            if (IsDataSourceAvailable())
            {
                ArrangeItem();
                LinearViewLoading();

                if (_isVirtualization && _virtualView != null && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear)
                {
                    if (_linearVirtualDataSource is IList list && list[(int)SelectedIndex] is object obj && obj != null)
                    {
                        PlatformCarouselItem? virtualizedItem = GetVirtualizedItem(obj, _selectedIndex);
                        if (virtualizedItem != null) 
                        {
                            item = virtualizedItem;
                        }
                        item.Index = _selectedIndex;
                    }

                }
                else
                {
                    if (_dataSource != null)
                    {
                        item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_selectedIndex);
                    }
                }

                UpdateLinearCenterPosition(item);
            }
        }

        /// <summary>
        /// Loads virtualization items in backward direction, adding necessary items to the carousel.
        /// </summary>
        /// <param name="item">A reference to the current <see cref="PlatformCarouselItem"/>.</param>
        /// <param name="j">A reference to the current index offset.</param>
        void LoadVirtualizationItemsBackward(ref PlatformCarouselItem item, ref int j)
        {
			for (int i = (int)_selectedIndex - 1; i >= 0; i--)
            {
                j = (int)_selectedIndex - (int)_dataSourceStartIndex - 1;
                if (i != -1)
                {
					var convertItem = PlatformCarousel.GetConvertItem(_linearVirtualDataSource, i);
					ConvertItemToCarouselItem(convertItem, ref item, i);
                    AddItemToSubView(item, i);
                    _dataSourceStartIndex = i;
                    UpdateLinearLeftPosition(item, -(_offset * j));
                    if (item.Frame.X + item.Frame.Width < Frame.X)
                    {
                        break;
                    }
                }
            }
        }

        void ConvertItemToCarouselItem(object? convertItem, ref PlatformCarouselItem item, int index)
        {
            SetConversionArguments(convertItem, index);
            if (convertItem != null && _virtualView != null && _virtualView.ViewMode == ViewMode.Linear)
            {
                PlatformCarouselItem? virtualizedItem = GetVirtualizedItem(convertItem, index);
                if (virtualizedItem != null) 
                {
                    item = virtualizedItem;
                }
            }
            else
            {
                if (_dataSource != null)
                {
                    _dataSource.GetItem<PlatformCarouselItem>((nuint)index).Index = index;
                    item = _dataSource.GetItem<PlatformCarouselItem>((nuint)index);
                }
            }
        }

        /// <summary>
        /// Loads virtualization items in forward direction, adding necessary items to the carousel.
        /// </summary>
        /// <param name="item">A reference to the current <see cref="PlatformCarouselItem"/>.</param>
        /// <param name="j">A reference to the current index offset.</param>
        void LoadVirtualizationItemsForward(ref PlatformCarouselItem item, ref int j)
        {
            if (_linearVirtualDataSource == null)
            {
                return;
            }

            object? convertItem = null;

            for (int i = (int)_selectedIndex; i < (nint)((IList)_linearVirtualDataSource).Count; i++)
            {
                if (_linearVirtualDataSource is IList list && list != null)
				{
					convertItem = list[i];
				}

				ConvertItemToCarouselItem(convertItem, ref item, i);
                AddItemToSubView(item, i);
                _dataSourceEndIndex = i;
                UpdateLinearRightPosition(item, _offset * j);
                j++;
                if (item.Frame.X > Frame.Size.Width + Frame.X)
                {
                    break;
                }
            }
        }

        void ResetSubviewsAndSublayers()
        {
            foreach (UIView view in Subviews)
            {
                view.RemoveFromSuperview();
            }

            if (Layer.Sublayers != null)
            {
                foreach (CALayer subLayer in Layer.Sublayers)
                {
                    subLayer.RemoveFromSuperLayer();
                }
            }
        }

        /// <summary>
        /// Handles the view mode with virtualization and xForms.
        /// </summary>
        void HandleVirtualizationAndXForms()
        {
            PlatformCarouselItem item = [];
            int j = 0;
            if (_viewMode == ViewMode.Default)
            {
                SetVirtualizationDefaultMode(ref item, ref j);
            }
            else
            {
                SetLinearMode();
            }
        }

        /// <summary>
        /// Handles the view mode with "load more" items and xForms.
        /// </summary>
        void HandleLoadMoreItemsAndXForms()
        {
            if (_viewMode == ViewMode.Default)
            {
                SetLoadMoreDefaultMode();
            }
            else
            {
                SetLinearModeForXForms();
            }
        }

        /// <summary>
        /// Sets up the "Load More" functionality for the default view mode.
        /// </summary>
        void SetLoadMoreDefaultMode()
        {
            if (_selectedIndex >= _maximumVisibleCount)
            {
                _selectedIndex = 0;
            }

            LoadDataSourceAndAddItems();

            if (_dataSource != null && _dataSource.Count > 0)
            {
                ArrangeItem();
                LinearViewLoading();
                PlatformCarouselItem item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_selectedIndex);
                UpdateLinearCenterPosition(item);
            }
        }

        /// <summary>
        /// Configures the carousel to use linear mode, specifically for XForms.
        /// </summary>
        void SetLinearModeForXForms()
        {
            if (IsDataSourceAvailable())
            {
                LinearMode = new SfLinearMode(this);
                if (_allowLoadMore)
                {
                    AddLoadMore();
                    if (_selectedIndex >= _maximumVisibleCount)
                    {
                        _selectedIndex = 0;
                    }
                }

                LinearMode.Frame = new CGRect(0, 0, Frame.Size.Width, Frame.Size.Height);
                LinearMode.Carousel = this;
                AddLinearModeToView();
            }
        }

        /// <summary>
        /// Configures the carousel for linear view mode.
        /// </summary>
        void SetLinearMode()
        {
            if (IsDataSourceAvailable())
            {
				LinearMode = new SfLinearMode(this)
				{
					Frame = new CGRect(0, 0, Frame.Size.Width, Frame.Size.Height),
					Carousel = this
				};
				AddLinearModeToView();
            }
        }

        /// <summary>
        /// Updates the view mode of the carousel.
        /// </summary>
        void UpdateViewMode()
        {
            ResetSubviewsAndSublayers();
            UserInteractionEnabled = IsInteractionEnable();
            if (_isVirtualization && _isLinearVirtual)
            {
                HandleVirtualizationAndXForms();
            }
            else if (LoadMoreItemsCount > 0 && _isLinearVirtual)
            {
                HandleLoadMoreItemsAndXForms();
            }
            else
            {
                HandleRegularMode();
            }
            UpdateFlowDirection();
        }

        /// <summary>
        /// Handles the regular mode of the carousel, which is neither virtualized nor has load more functionality.
        /// </summary>
        void HandleRegularMode()
        {
            int tempCount = GetItemCount();

            if (_viewMode == ViewMode.Default)
            {
                HandleDefaultViewMode(tempCount);
            }
            else
            {
                HandleLinearViewMode(tempCount);
            }
        }

        /// <summary>
        /// Handles the default view mode of the carousel.
        /// </summary>
        /// <param name="tempCount">The total count of items in the carousel.</param>
        void HandleDefaultViewMode(int tempCount)
        {
            if (_dataSource == null)
			{
				return;
			}

			PlatformCarouselItem item = [];

            if (!_allowLoadMore && !_isVirtualization)
            {
                LoadItemsWithoutLoadMore(tempCount, item);
            }
            else if (_allowLoadMore && !_isVirtualization)
            {
                LoadItemsWithLoadMore(tempCount, item);
            }
            else if (_isVirtualization && !_allowLoadMore)
            {
                InitializeVirtualization(tempCount, item);
            }

            if (IsDataSourceAvailable())
            {
                ArrangeItem();
                LinearViewLoading();
                if (_dataSource != null)
                {
                    item = _dataSource.GetItem<PlatformCarouselItem>((nuint)_selectedIndex);
                }
                UpdateLinearCenterPosition(item);
            }
        }

        /// <summary>
        /// Handles the linear view mode of the carousel.
        /// </summary>
        /// <param name="tempCount">The total count of items in the carousel.</param>
        void HandleLinearViewMode(int tempCount)
        {
            if (!IsDataSourceAvailable())
			{
				return;
			}

			LinearMode = new SfLinearMode(this);
            ConfigureLinearMode(tempCount);
            AddLinearModeToView();
        }

        /// <summary>
        /// Configures the linear mode of the carousel.
        /// </summary>
        /// <param name="tempCount">The total count of items in the carousel.</param>
        void ConfigureLinearMode(int tempCount)
        {
            if (_allowLoadMore)
            {
                AddLoadMore();
                _selectedIndex = _selectedIndex >= _maximumVisibleCount ? 0 : _selectedIndex;
            }
            else
            {
                _selectedIndex = _selectedIndex >= tempCount ? 0 : _selectedIndex;
            }

            LinearMode.Frame = new CGRect(0, 0, Frame.Size.Width, Frame.Size.Height);
            LinearMode.Carousel = this;
        }

        /// <summary>
        /// Adds the configured linear mode to the view and scrolls to the selected item.
        /// </summary>
        void AddLinearModeToView()
        {
            NSIndexPath pathValue = NSIndexPath.FromRowSection(_selectedIndex, 0);
            LinearMode.ViewCollection?.ScrollToItem(pathValue, UICollectionViewScrollPosition.CenteredHorizontally, false);
            AddSubview(LinearMode);
        }

        /// <summary>
        /// Applies a right-to-left transform if the user interface layout direction is right-to-left.
        /// </summary>
        void UpdateFlowDirection()
        {
            // Reset the transformation for Linear ViewMode
            if (_viewMode == ViewMode.Linear)
            {
                Transform = CGAffineTransform.MakeScale(1, 1);
                return;
            }

            // Apply the appropriate transformation based on RTL setting
            SemanticContentAttribute = IsRTL ? UISemanticContentAttribute.ForceRightToLeft : UISemanticContentAttribute.ForceLeftToRight;
            Transform = CGAffineTransform.MakeScale(IsRTL ? -1 : 1, 1);
        }

        /// <summary>
        /// Initializes virtualization for the carousel, loading items as needed.
        /// </summary>
        /// <param name="tempCount">The total count of items to be processed.</param>
        /// <param name="item">A reference to a <see cref="PlatformCarouselItem"/> used to initialize new items.</param>
        void InitializeVirtualization(int tempCount, PlatformCarouselItem item)
        {
            _dataSourceEndIndex = _selectedIndex;
            _dataSourceStartIndex = _selectedIndex;
            if ((IsDataSourceAvailable()))
            {
                int j = 0;
                for (nint i = _selectedIndex; i < tempCount; i++)
                {
                    if (_dataSource != null)
                    {
						item = item = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                        AddItemToSubView(item, i);
                    }

                    _dataSourceEndIndex = i;
					PlatformCarouselItem platformCarousel = [];
                    if (_dataSource != null)
                    {
                        platformCarousel = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                    }

                    UpdateLinearRightPosition(platformCarousel, _offset * j);
                    j++;
                    if (item.Frame.X > Frame.Size.Width + Frame.X)
                    {
                        break;
                    }
                }

                for (nint i = _selectedIndex - 1; i >= 0; i--)
                {
                    j = (int)_selectedIndex - (int)_dataSourceStartIndex - 1;
                    if (i != -1)
                    {
                        if (_dataSource != null)
                        {
                            item = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                        }
                        AddItemToSubView(item, i, false);

                        _dataSourceStartIndex = i;
                        PlatformCarouselItem carouselItem = [];
                        if (_dataSource != null)
                        {
                            carouselItem = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                        }

                        UpdateLinearLeftPosition(carouselItem, -(_offset * j));
                        if (item.Frame.X + item.Frame.Width < Frame.X)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads items into the carousel with the "Load More" functionality enabled.
        /// </summary>
        /// <param name="tempCount">The total count of items to be processed.</param>
        /// <param name="item">A reference to a PlatformCarouselItem used to initialize new items.</param>
        void LoadItemsWithLoadMore(int tempCount, PlatformCarouselItem item)
        {
            if (_selectedIndex >= _maximumVisibleCount)
            {
                _selectedIndex = 0;
            }

            if ((IsDataSourceAvailable()))
            {
                _dataSourceStartIndex = 0;
                _dataSourceEndIndex = 0;
                if (_maximumVisibleCount > tempCount)
                {
                    _maximumVisibleCount = tempCount;
                }

                for (nint i = 0; i < _maximumVisibleCount; i++)
                {
                    item = [];
                    if (_dataSource != null)
                    {
                        item = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                        item.UserInteractionEnabled = IsInteractionEnable();
                        AddItemToSubView(item, i);
                    }
                    _dataSourceEndIndex = i;
                    if (_allowLoadMore)
                    {
                        if (i + 1 == _maximumVisibleCount)
                        {
                            InitializeLoadMoreItem();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads items into the carousel without the "Load More" functionality.
        /// </summary>
        /// <param name="tempCount">The total count of items to be processed.</param>
        /// <param name="item">A reference to a <see cref="PlatformCarouselItem"/> used to initialize new items.</param>
        void LoadItemsWithoutLoadMore(int tempCount, PlatformCarouselItem item)
        {
            _dataSourceStartIndex = 0;
            _dataSourceEndIndex = 0;
            for (nint i = 0; i < tempCount; i++)
            {
                if (_dataSource != null)
                {
                    item = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                    AddItemToSubView(item, i);
                }

                _dataSourceEndIndex = i;
            }
        }

        /// <summary>
        /// Loads data source and adds items for "load more" scenario.
        /// </summary>
        void LoadDataSourceAndAddItems()
        {
            if (_linearVirtualDataSource != null && ((IList)_linearVirtualDataSource).Count > 0)
            {
                PlatformCarouselItem item;
                object? convertItem;
                _dataSourceStartIndex = 0;
                _dataSourceEndIndex = 0;
                if (_maximumVisibleCount > ((IList)_linearVirtualDataSource).Count)
                {
                    _maximumVisibleCount = ((IList)_linearVirtualDataSource).Count;
                }

                for (nint i = 0; i < _maximumVisibleCount; i++)
                {
                    if (_dataSource != null && _linearVirtualDataSource != null && _linearVirtualDataSource is IList list)
                    {
                        convertItem = list[(int)i];
                        SetConversionArguments(convertItem, i);
                        item = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                        AddItemToSubView(item, i);
                        _dataSourceEndIndex = i;
                    }
                    if (_allowLoadMore)
                    {
                        if (i + 1 == _maximumVisibleCount)
                        {
                            InitializeLoadMoreItem();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the "Load More" item for the carousel.
        /// </summary>
        void InitializeLoadMoreItem()
        {
            LoadMoreItem.RemoveFromSuperview();
            LoadMoreItem.Frame = GetFrameRect();
            AddLoadMore();

            LoadMoreItem.UserInteractionEnabled = IsInteractionEnable();
            LoadMoreItem.Index = _dataSourceEndIndex + 1;
            LoadMoreItem.InternalCarousel = this;
            LoadMoreItem.IsAccessibilityElement = true;
            LoadMoreAccessibility();
            AddSubview(LoadMoreItem);
        }

        /// <summary>
        /// Method to set accessibility properties to PlatformCarouselItem
        /// </summary>
        void CarouselItemAccessibility(PlatformCarouselItem item, nint index)
        {
            if (_dataSource != null)
            {
                item.AccessibilityLabel = item.AutomationId + " SfCarouselItem " + (index + 1) + " of " + (int)_dataSource.Count;
                item.AccessibilityIdentifier = item.AutomationId + " SfCarouselItem " + (index + 1) + " of " + (int)_dataSource.Count;
            }
        }

        /// <summary>
        /// Method to set accessibility properties to load more carousel item.
        /// </summary>
        void LoadMoreAccessibility()
        {
            if (LoadMoreItem != null)
            {
                LoadMoreItem.AccessibilityLabel = AutomationId + " Load More. Tap to load more items";
                LoadMoreItem.AccessibilityIdentifier = AutomationId + " Load More. Tap to load more items";
            }
        }

        /// <summary>
        /// Removes the existing "Load More" view if it exists.
        /// </summary>
        void RemoveExsitingLoadMore()
        {
            Loadmore?.RemoveFromSuperview();
        }

        /// <summary>
        /// Determines if a new "Load More" view should be created.
        /// </summary>
        /// <returns>True if a new "Load More" view should be created; otherwise, false.</returns>
        bool ShouldCreateNewLoadMore()
        {
            return Loadmore == null || _textChanged;
        }

        /// <summary>
        /// Creates and configures a new "Load More" view.
        /// </summary>
        void CreateAndConfigureLoadMore()
        {
            UILabel label = new UILabel
            {
                Text = string.IsNullOrEmpty(_loadMoreText) ? SfCarouselResources.LoadMoreText : _loadMoreText,
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.LightGray
            };

            if (_viewMode == ViewMode.Linear)
            {
                ConfigureLinearLoadMore(label);
            }
            else
            {
                ConfigureDefaultLoadMore(label);
            }

            _textChanged = false;
            _isCustomView = false;
        }

        /// <summary>
        /// Configures the "Load More" view for default mode.
        /// </summary>
        /// <param name="label">The UILabel to be configured as the "Load More" view.</param>
        void ConfigureDefaultLoadMore(UILabel label)
        {
            ConfigureLoadMoreItem(label);
        }

        /// <summary>
        /// Updates the "Load More" view for linear mode.
        /// </summary>
        void UpdateLinearLoadMore()
        {
            if(Loadmore != null)
            {
                Loadmore.Frame = new CGRect(0, 0, _itemWidth, _itemHeight);
                Loadmore.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            }
        }

        /// <summary>
        /// Updates the "Load More" view for default mode.
        /// </summary>
        void UpdateDefaultLoadMore()
        {
            if (LoadMoreItem != null)
            {

/* Unmerged change from project 'Syncfusion.Maui.Toolkit (net9.0-ios)'
Before:
                RemoveExtraSubviews(LoadMoreItem);
                if(Loadmore != null)
After:
				PlatformCarousel.RemoveExtraSubviews(LoadMoreItem);
                if(Loadmore != null)
*/
				RemoveExtraSubviews(LoadMoreItem);
                if(Loadmore != null)
				{
					ConfigureLoadMoreItem(Loadmore);
				}
			}
        }

		/// <summary>
		/// Removes extra subviews from the LoadMoreItem.
		/// </summary>
		/// <param name="loadMoreItem">The LoadMoreItem to remove extra subviews from.</param>
		static void RemoveExtraSubviews(UIView loadMoreItem)
        {
            foreach (var item in loadMoreItem.Subviews)
            {
                if (item != loadMoreItem.Subviews[0])
                {
                    item.RemoveFromSuperview();
                }
            }
        }

        /// <summary>
        /// Refreshes the carousel view based on the current view mode.
        /// </summary>
        void RefreshCarouselView()
        {
            if (_viewMode == ViewMode.Default)
            {
                if (IsDataSourceAvailable())
                {
                    LinearViewLoading();
                }
            }
            else
            {
                LinearMode.ViewCollection?.ReloadData();
            }
        }

        /// <summary>
        /// Updates the existing "Load More" view.
        /// </summary>
        void UpdateExistingLoadMore()
        {
            if (_viewMode == ViewMode.Linear)
            {
                UpdateLinearLoadMore();
            }
            else
            {
                UpdateDefaultLoadMore();
            }
        }

        /// <summary>
        /// Configures the "Load More" view for linear mode.
        /// </summary>
        /// <param name="label">The UILabel to be configured as the "Load More" view.</param>
        void ConfigureLinearLoadMore(UILabel label)
        {
            Loadmore = label;
            Loadmore.Frame = new CGRect(0, 0, _itemWidth, _itemHeight);
            Loadmore.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
        }

        /// <summary>
        /// Adds the load more.
        /// </summary>
        void AddLoadMore()
        {

            RemoveExsitingLoadMore();
            if (ShouldCreateNewLoadMore())
            {
                CreateAndConfigureLoadMore();
            }
            else if (_isCustomView || Loadmore != null)
            {
                UpdateExistingLoadMore();
            }

            RefreshCarouselView();
        }

        void ConfigureLoadMoreItem(UIView view)
        {
            LoadMoreItem.View = view;
            LoadMoreItem.AddSubview(LoadMoreItem.View);
            LoadMoreItem.View.Frame = LoadMoreItem.Subviews[0].Frame;
            LoadMoreItem.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
        }


        /// <summary>
        /// Updates the size of the carousel items.
        /// </summary>
        void UpdateSize()
        {
            int tempCount = GetItemCount();

            if (_viewMode == ViewMode.Default)
            {
                if ((_dataSource != null) && !_isVirtualization && !_allowLoadMore && !_isLinearVirtual)
                {

                    for (nfloat i = 0; i < tempCount; i++)
                    {
                        PlatformCarouselItem view = [];
                        if (_dataSource != null)
                        {
                            view = _dataSource.GetItem<PlatformCarouselItem>((nuint)i);
                        }
                        view.Frame = new CGRect(view.Frame.Location.X, (Bounds.Size.Height / 2) - (_itemHeight / 2), _itemWidth, _itemHeight);
                    }

                    LinearViewLoading();
                }
                else
                {
                    foreach (var view in Subviews)
                    {
                        view.Frame = new CGRect(view.Frame.Location.X, (Bounds.Size.Height / 2) - (_itemHeight / 2), _itemWidth, _itemHeight);
                    }

                    LinearViewLoading();
                }
            }
            else
            {
                UpdateViewMode();
            }
        }

        /// <summary>
        /// This method set the data source.
        /// </summary>
        /// <param name="handler"></param>
        void SetDataSource(CarouselHandler handler)
        {
			if (_virtualView != null && (_virtualView.ItemsSource is IList))
            {
                NSMutableArray array = [];
                int i = 0;
                foreach (var item in _virtualView.ItemsSource)
                {
                    View? contentView = null;
                    if (_virtualView.ItemTemplate != null)
                    {
                        contentView = GetItemView(_virtualView, this, i);
                        i++;
                    }

                    if (contentView != null)
                    {
                        array.Add(GetNativeViewItem(contentView, handler));
                    }
                }

                if (array.Count == 0)
				{
					SelectedIndex = 0;
				}
				else if (((int)SelectedIndex) >= (int)array.Count)
				{
					SelectedIndex = ((int)array.Count) - 1;
				}

				DataSource = array;
                SelectedIndex = SelectedIndex;
            }
        }

        /// <summary>
        /// Retrieves the native view for a specified carousel item in the given PlatformCarousel.
        /// </summary>
        /// <param name="formsCarousel">The forms carousel instance used to obtain the native view.</param>
        /// <param name="carousel">The platform-specific carousel instance containing the items.</param>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>The native UIView associated with the specified index, or null if not found.</returns>
        View? GetItemView(ICarousel formsCarousel, PlatformCarousel carousel, int index)
        {
            if ((formsCarousel.ItemsSource is IList && (formsCarousel.ItemsSource is IList list) && list.Count > 0))
            {
                if (formsCarousel.ItemTemplate != null)
                {
                    return PlatformCarousel.CreateTemplatedView(formsCarousel, index);
                }
                if (formsCarousel.ItemTemplate == null)
                {
                    return PlatformCarousel.CreateDefaultLabelView(formsCarousel, index);
                }
                else
				{
					return null;
				}
			}
            else
			{
				return null;
			}
		}

		static DataTemplate? GetDataTemplate(ICarousel formsCarousel, int index)
        {
            if (formsCarousel.ItemTemplate is DataTemplateSelector dataTemplateSelector)
            {
                return dataTemplateSelector.SelectTemplate(formsCarousel.ItemsSource?.ElementAt(index), null);
            }
            return formsCarousel.ItemTemplate;
        }

		static View? CreateViewFromTemplate(DataTemplate template)
        {
            var templateInstance = template.CreateContent();
            if (templateInstance is View view)
            {
                return view;
            }
#if NET10_0_OR_GREATER
            return templateInstance as View;
#else
            return (templateInstance as ViewCell)?.View;
#endif
        }

		static View? CreateTemplatedView(ICarousel formsCarousel, int index)
        {
			if(formsCarousel is not null)
			{
				DataTemplate? template = PlatformCarousel.GetDataTemplate(formsCarousel, index);
				if (template is null)
				{
					return null;
				}

				View? templateLayout = PlatformCarousel.CreateViewFromTemplate(template);
				if (templateLayout is null)
				{
					return null;
				}

				PlatformCarousel.SetParentContent(templateLayout, formsCarousel);
				PlatformCarousel.SetBindingContext(templateLayout, formsCarousel, index);
				return templateLayout;
			}

			return null;
        }

		/// <summary>
		/// Sets parent to the template view.
		/// </summary>
		/// <param name="templateLayout"></param>
		/// <param name="formsCarousel"></param>
		static void SetParentContent(View templateLayout, ICarousel formsCarousel)
		{
			if (templateLayout != null)
			{
				templateLayout.Parent = (Element)formsCarousel;
			}
		}

		static void SetBindingContext(View templateLayout, ICarousel formsCarousel, int index)
        {
            if (formsCarousel.ItemsSource != null && index < formsCarousel.ItemsSource.Count())
            {
                templateLayout.BindingContext = formsCarousel.ItemsSource.ElementAt(index);
            }
        }

		static View? CreateDefaultLabelView(ICarousel formsCarousel, int index)
        {
            if (formsCarousel.ItemsSource == null || index >= formsCarousel.ItemsSource.Count())
            {
                return null;
            }

            return new Label
            {
                Text = formsCarousel.ItemsSource.ElementAt(index).ToString(),
                VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center,
                HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center
            };
        }

        /// <summary>
        /// Gets Native Item for Carousel.
        /// </summary>
        public PlatformCarouselItem GetNativeViewItem(View contentView, ICarouselHandler handler)
        {
            PlatformCarouselItem carouselItem = [];
            if (handler.MauiContext != null)
            {
                carouselItem.View = contentView.ToPlatform(handler.MauiContext);
            }
            return carouselItem;
        }

        #region Internal Methods

        /// <summary>
        /// Updates the items source for the specified PlatformCarousel based on the provided handler and virtual view.
        /// </summary>
        /// <param name="handler">The carousel handler responsible for managing the carousel interactions and updates.</param>
        /// <param name="virtualView">The virtual view of the carousel which contains the items to be displayed.</param>
        internal void UpdateItemSource(CarouselHandler handler, ICarousel virtualView)
        {
            if (virtualView.ItemsSource != null && virtualView.EnableVirtualization && virtualView.ViewMode == ViewMode.Linear && !virtualView.AllowLoadMore)
            {
                IsLinearVirtual = true;
                _handler = handler;
                ObservableCollection<object> array = [.. virtualView.ItemsSource];
                LinearVirtualDataSource = array;
            }
            else if (virtualView.ItemsSource != null)
            {
                SetDataSource(handler);
            }
        }

        #endregion

    }

    /// <summary>
    /// Collection changed event arguments class.
    /// </summary>
    internal class CollectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The new collection field.
        /// </summary>
        NSMutableArray? _newCollection;

        /// <summary>
        /// Gets or sets the new collection.
        /// </summary>
        /// <value>The new collection.</value>
        public NSMutableArray? NewCollection
        {
            get
            {
                return _newCollection;
            }

            set
            {
                _newCollection = value;
            }
        }
    }

    /// <summary>
    /// Conversion event arguments.
    /// </summary>
    internal class ConversionEventArgs : EventArgs
    {
        /// <summary>
        /// The new collection field.
        /// </summary>
        private object? _item;

        /// <summary>
        /// The index.
        /// </summary>
        private int _index;

        /// <summary>
        /// Gets or sets the new collection.
        /// </summary>
        /// <value>The new collection.</value>
        public object? Item
        {
            get
            {
                return _item;
            }

            internal set
            {
                _item = value;
            }
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get
            {
                return _index;
            }

            internal set
            {
                _index = value;
            }
        }
    }
}
