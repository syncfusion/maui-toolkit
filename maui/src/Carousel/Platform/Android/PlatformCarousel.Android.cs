using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views.Animations;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System.Collections;
using System.Collections.Specialized;
using System.Resources;
using View = Android.Views.View;
using IList = System.Collections.IList;
using Syncfusion.Maui.Toolkit.Carousel.Platform;
using Rect = Android.Graphics.Rect;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// Represents a platform-specific handler for connecting a carousel view.
	/// </summary>
	/// <exclude/>
	public partial class PlatformCarousel : FrameLayout, View.IOnClickListener
	{
		#region Fields

		/// <summary>
		/// The count item.
		/// </summary>
		internal int _countItem;

		/// <summary>
		/// The temp data source.
		/// </summary>
		internal List<PlatformCarouselItem>? TempDataSource { get; set; } = [];

		/// <summary>
		/// The index of the is last.
		/// </summary>
		internal bool _isLastIndex, _isImageClicked;

		/// <summary>
		/// The temp data source value.
		/// </summary>
		internal List<PlatformCarouselItem>? _tempDataSourceValue;

		/// <summary>
		/// The allow load more data source.
		/// </summary>
		internal IList<PlatformCarouselItem>? AllowLoadMoreDataSource { get; set; } = [];

		/// <summary>
		/// Items Source Collection
		/// </summary>
		internal List<PlatformCarouselItem>? ItemsSourceCollection { get; set; } = [];

		/// <summary>
		/// The temp items source.
		/// </summary>
		internal List<PlatformCarouselItem>? _tempItemsSource;

		/// <summary>
		/// The item spacing.
		/// </summary>
		private int _itemSpacing = 5;

		/// <summary>
		/// The item spacing.
		/// </summary>
		private readonly int _minDiffernce = 7;

		/// <summary>
		/// The enableInteraction
		/// </summary>
		private bool _enableInteraction = true;

		/// <summary>
		/// The isEnabled
		/// </summary>
		private bool _isEnabled = true;

		/// <summary>
		/// The adapter.
		/// </summary>
		private ICarouselAdapter? _adapter;

		/// <summary>
		/// The width of the control.
		/// </summary>
		private int _controlWidth;

		/// <summary>
		/// The isSwipeRestricted.
		/// </summary>
		private bool _isSwipeRestricted;

		/// <summary>
		/// The swipeEventCalled.
		/// </summary>
		private bool _swipeEventCalled;


		/// <summary>
		/// The height of the control.
		/// </summary>
		private int _controlHeight;

		/// <summary>
		/// The index of the selected.
		/// </summary>
		private int _selectedIndex;

		/// <summary>
		/// The m offset.
		/// </summary>
		private int _moffset = 60;

		/// <summary>
		/// The height of the item.
		/// </summary>
		private int _itemHeight = 500;

		/// <summary>
		/// AutomationId field.
		/// </summary>
		private string _automationId = string.Empty;

		/// <summary>
		/// The width of the item.
		/// </summary>
		private int _itemWidth = 300;

		/// <summary>
		/// The view mode.
		/// </summary>
		private ViewMode _viewMode = ViewMode.Default;

		/// <summary>
		/// The rotation angle.
		/// </summary>
		private int _rotationAngle = 45;

		/// <summary>
		/// The scale offset.
		/// </summary>
		private float _scaleOffset = 0.7f;

		/// <summary>
		/// The selected item offset.
		/// </summary>
		private int _selectedItemOffset = -37;

		/// <summary>
		/// The duration.
		/// </summary>
		private int _duration = 600;

		/// <summary>
		/// The Scroll Mode
		/// </summary>
		private SwipeMovementMode _swipeMovementMode = SwipeMovementMode.MultipleItems;

		/// <summary>
		/// The can execute first field
		/// </summary>
		private bool _canExecuteFirst;

		/// <summary>
		/// The can execute swipe field
		/// </summary>
		private bool _canExecuteSwipe = true;

		/// <summary>
		/// The horizontal grid view.
		/// </summary>
		private RecyclerView? _horizontalGridView;

		/// <summary>
		/// The index of the previous.
		/// </summary>
		private int _prevIndex;

		/// <summary>
		/// The previous offset.
		/// </summary>
		private int _prevOffset;

		/// <summary>
		/// The previous angle.
		/// </summary>
		private int _prevAngle;

		/// <summary>
		/// The previous offset.
		/// </summary>
		private float _prevZOffset;

		/// <summary>
		/// The previous selected offset.
		/// </summary>
		private int _prevSelectedOffset;

		/// <summary>
		/// The temp list.
		/// </summary>
		private List<PlatformCarouselItem>? _tempList = [];

		/// <summary>
		/// The greater than list.
		/// </summary>
		private List<PlatformCarouselItem>? _nextItems = [];

		/// <summary>
		/// The lesser than list.
		/// </summary>
		private List<PlatformCarouselItem>? _previousItems = [];

		/// <summary>
		/// The y position.
		/// </summary>
		private int _yPos;

		/// <summary>
		/// The X position.
		/// </summary>
		private int _xPos;

		/// <summary>
		/// The x.
		/// </summary>
		private int _xvalue, _dX, _id, _count, _dY, _yvalue;

		/// <summary>
		/// The is moving.
		/// </summary>
		private bool _isMoving, _isDown;

		/// <summary>
		/// The allow load more.
		/// </summary>
		private bool _allowLoadMore;

		/// <summary>
		/// The is busy.
		/// </summary>
		private bool _isBusy;

		/// <summary>
		/// The maximum indicator.
		/// </summary>
		private int _maximumIndicator;

		/// <summary>
		/// The is enabling load more button.
		/// </summary>
		private bool _isEnablingLoadMoreButton;

		/// <summary>
		/// The loaded custom view.
		/// </summary>
		private View? _loadedCustomView;

		/// <summary>
		/// The items source.
		/// </summary>
		private IEnumerable? _itemsSource;

		/// <summary>
		/// The is virtualization.
		/// </summary>
		private bool _isVirtualization;

		/// <summary>
		/// The is selected.
		/// </summary>
		private bool _isSelected;

		/// <summary>
		/// The center position.
		/// </summary>
		private int _centerPosition, _startPosition, _endPosition, _xPositionLeft, _xPositionRight;

		/// <summary>
		/// The start.
		/// </summary>
		private int _start = -1, _end = -1, _preStart = -1, _preEnd = -1;

		/// <summary>
		/// The is sized changed.
		/// </summary>
		private bool _isSizeChanged;

		/// <summary>
		/// The local resource.
		/// </summary>
		private ResourceManager? _localresource;

		/// <summary>
		/// The linear layout manager.
		/// </summary>
		private LinearLayoutManager? _linearLayoutManager;

		/// <summary>
		/// The linear layout.
		/// </summary>
		private LinearLayout? _linearLayout;

		private bool _isSwipeHappened;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PlatformCarousel"/> class.
		/// </summary>
		/// <param name="context">Context of the carousel.</param>
		public PlatformCarousel(Context context) : base(context)
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlatformCarousel"/> class.
		/// </summary>
		/// <param name="context">Context of the carousel.</param>
		/// <param name="attribute">Attribute set value.</param>
		public PlatformCarousel(Context context, IAttributeSet attribute) : base(context, attribute)
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlatformCarousel"/> class.
		/// </summary>
		/// <param name="context">The Context.</param>
		/// <param name="attribute">Attribute set.</param>
		/// <param name="style">Default style.</param>
		public PlatformCarousel(Context context, IAttributeSet attribute, int style) : base(context, attribute, style)
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlatformCarousel"/> class.
		/// </summary>
		/// <param name="context">Context set.</param>
		/// <param name="attribute">Attribute set.</param>
		/// <param name="style">Default style.</param>
		/// <param name="resource">Default resource.</param>
		public PlatformCarousel(Context context, IAttributeSet attribute, int style, int resource) : base(context, attribute, style, resource)
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlatformCarousel"/> class.
		/// </summary>
		/// <param name="a">The a</param>
		/// <param name="b">The b</param>    
		public PlatformCarousel(IntPtr a, JniHandleOwnership b)
			: base(a, b)

		{
			Initialize();
		}

		void Initialize()
		{

			ClipToOutline = true;
#if !ANDROID22_0_OR_GREATER
			AnimationCacheEnabled = true;
			DrawingCacheEnabled = true;
#endif
			LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when collection changed.
		/// </summary>
		internal event EventHandler<ItemsCollectionChangedEventArgs>? CollectionChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/> is virtualization.
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
			}
		}

		/// <summary>
		/// Gets or sets items source
		/// </summary>
		/// <value>The item source.</value>
		public IEnumerable ItemsSource
		{
			get
			{
				return _itemsSource ?? Enumerable.Empty<object>();
			}

			set
			{
				if (value != null)
				{
					_itemsSource = value;
					OnItemsSourceChanged();
				}
			}
		}

		void OnItemsSourceChanged()
		{
			if (ItemsSource is INotifyCollectionChanged)
			{
				// The collection changed triggered in MAUI, so we commented here
				// source.CollectionChanged -= Handle_CollectionChanged;
				// source.CollectionChanged += Handle_CollectionChanged;
			}

			TempDataSource = [];
			AddItems();
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
				_itemSpacing = value;
				Refresh();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/> EnableInteraction or not
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
			}
		}

		/// <summary>
		/// Gets or sets a value indicating the Scroll Mode of <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/>
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
		/// Gets or sets a value indicating whether this <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/> IsEnabled or not
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
			}
		}

		/// <summary>
		/// Gets or sets the adapter.
		/// </summary>
		/// <value>The adapter.</value>
		public ICarouselAdapter? Adapter
		{
			get
			{
				return _adapter;
			}

			set
			{
				_adapter = value;
			}
		}

		/// <summary>
		/// Gets or sets the index of the selected.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}

			set
			{
				_selectedIndex = value;
				OnSelectedIndexChanged(value);
			}
		}

		void OnSelectedIndexChanged(int value)
		{
			var isSelectedIndexChanged = false;
			if (_virtualView != null && _virtualView.SelectedIndex != value)
			{
				isSelectedIndexChanged = true;
				_virtualView.SelectedIndex = value;
			}

			if (ChildCount > 0)
			{
				if (_tempItemsSource != null && PlatformCarousel.IsWithinBounds(_selectedIndex, TempDataSource, _tempItemsSource.Count))
				{
					Selection(isSelectedIndexChanged);
				}
				else
				{
					SelectedIndex = _prevIndex;
				}
			}
		}

		static bool IsWithinBounds(int index, List<PlatformCarouselItem>? tempDataSource, int sourceCount)
		{
			return (index >= 0) && ((tempDataSource != null) && (index <= tempDataSource.Count - 1) || (index <= sourceCount - 1));
		}

		/// <summary>
		/// Gets or sets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public int Offset
		{
			get
			{
				return _moffset;
			}

			set
			{
				_moffset = value;
				OnOffsetChanged();
			}
		}

		void OnOffsetChanged()
		{
			if (_viewMode == ViewMode.Default)
			{
				if (_prevOffset != _moffset)
				{
					_prevOffset = _moffset;
					if (TempDataSource != null && TempDataSource.Count > _selectedIndex)
					{
						Arrange(TempDataSource[_selectedIndex], true);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the height of the item.
		/// </summary>
		/// <value>The height of the item.</value>
		public int ItemHeight
		{
			get
			{
				return _itemHeight;
			}

			set
			{
				_itemHeight = value;
				OnItemHeightChanged();
			}
		}

		void OnItemHeightChanged()
		{
			if (_itemHeight <= 0 && _viewMode == ViewMode.Linear)
			{
				_itemHeight = 5;
			}

			if (TempDataSource != null)
			{
				Refresh();
			}

			if (_viewMode == ViewMode.Linear && _linearLayout is not null && _virtualView is not null)
			{
				_linearLayout.InvalidateMeasure(_virtualView);
			}
		}

		/// <summary>
		/// Gets or sets the width of the item.
		/// </summary>
		/// <value>The width of the item.</value>
		public int ItemWidth
		{
			get
			{
				return _itemWidth;
			}

			set
			{
				_itemWidth = value;
				OnItemWidthChanged();
			}
		}

		void OnItemWidthChanged()
		{
			if (_itemWidth <= 0 && _viewMode == ViewMode.Linear)
			{
				_itemWidth = 5;
			}

			if (TempDataSource != null)
			{
				Refresh();
			}

			if (_viewMode == ViewMode.Linear && _linearLayout is not null && _virtualView is not null)
			{
				_linearLayout.InvalidateMeasure(_virtualView);
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
				RemoveAllViews();
				OnViewModeChanged();
				if (_viewMode == ViewMode.Default)
				{
					RefreshCarousel();
				}
			}
		}

		/// <summary>
		/// Gets or sets the rotation angle.
		/// </summary>
		/// <value>The rotation angle.</value>
		public int RotationAngle
		{
			get
			{
				return _rotationAngle;
			}

			set
			{
				_rotationAngle = value;
				OnRotationAngleChanged();
			}
		}

		void OnRotationAngleChanged()
		{
			if (_viewMode == ViewMode.Default && _rotationAngle != _prevAngle)
			{
				_prevAngle = _rotationAngle;
				if (TempDataSource != null && TempDataSource.Count > _selectedIndex)
				{
					Arrange(TempDataSource[_selectedIndex], true);
				}
			}
		}

		/// <summary>
		/// Gets or sets the selected item offset.
		/// </summary>
		/// <value>The selected item offset.</value>
		public int SelectedItemOffset
		{
			get
			{
				return _selectedItemOffset;
			}

			set
			{
				_selectedItemOffset = value;
				OnSelectedItemOffsetChanged();
			}
		}

		void OnSelectedItemOffsetChanged()
		{
			if (_viewMode == ViewMode.Default && _selectedItemOffset != _prevSelectedOffset)
			{
				_prevSelectedOffset = _selectedItemOffset;
				if (TempDataSource != null && TempDataSource.Count > _selectedIndex)
				{
					Arrange(TempDataSource[_selectedIndex], true);
				}
			}
		}

		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		/// <value>The duration.</value>
		public int Duration
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
		/// Gets or sets the scale offset.
		/// </summary>
		/// <value>The scale offset.</value>
		public float ScaleOffset
		{
			get
			{
				return _scaleOffset;
			}

			set
			{
				_scaleOffset = value;
				OnScalOffsetChanged();
			}
		}

		void OnScalOffsetChanged()
		{
			if (_viewMode == ViewMode.Default && _scaleOffset != _prevZOffset)
			{
				_prevZOffset = _scaleOffset;
				if (TempDataSource != null && TempDataSource.Count > _selectedIndex)
				{
					Arrange(TempDataSource[_selectedIndex], true);
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum items count.
		/// </summary>
		/// <value>The maximum items count.</value>
		public int LoadMoreItemsCount
		{
			get
			{
				return _maximumIndicator;
			}

			set
			{
				if (_maximumIndicator != value)
				{
					_maximumIndicator = value;
					AllowLoadMoreInvoke();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/> allow load more.
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
				AllowLoadMoreInvoke();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/> allow load more.
		/// </summary>
		/// <value><c>true</c> if allow load more; otherwise, <c>false</c>.</value>
		public View? LoadMoreView
		{
			get
			{
				return _loadedCustomView;
			}

			set
			{
				_loadedCustomView = value;
				AllowLoadMoreInvoke();
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
					_automationId ??= string.Empty;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/> is busy.
		/// </summary>
		/// <value><c>true</c> if is busy; otherwise, <c>false</c>.</value>
		internal bool IsBusy
		{
			get
			{
				return _isBusy;
			}

			set
			{
				_isBusy = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of the control.
		/// </summary>
		/// <value>The height of the control.</value>
		internal int ControlHeight
		{
			get
			{
				return _controlHeight;
			}

			set
			{
				_controlHeight = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the control.
		/// </summary>
		/// <value>The width of the control.</value>
		internal int ControlWidth
		{
			get
			{
				return _controlWidth;
			}

			set
			{
				_controlWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the local resource.
		/// </summary>
		/// <value>The local resource.</value>
		internal ResourceManager? LocalResource
		{
			get
			{
				return _localresource;
			}

			set
			{
				_localresource = value;
				SfCarouselResources.ResourceManager = value;
			}
		}

		/// <summary>
		/// Gets or sets the horizontal grid view.
		/// </summary>
		/// <value>The horizontal grid view.</value>
		internal RecyclerView? HorizontalGridView
		{
			get
			{
				return _horizontalGridView;
			}

			set
			{
				_horizontalGridView = value;
			}
		}

		/// <summary>
		/// Gets or sets the selection changed event arguments.
		/// </summary>
		/// <value>The selection changed event arguments.</value>
		internal SelectionChangedEventArgs? SelectionChangedEventArgs { get; set; }


		/// <summary>
		/// Gets or sets the items collection changed event arguments.
		/// </summary>
		/// <value>The items collection changed event arguments.</value>
		internal ItemsCollectionChangedEventArgs? ItemsCollectionChangedEventArgs { get; set; }

		/// <summary>
		/// Gets or sets the Swipe started event arguments.
		/// </summary>
		/// <value>The  SwipeStarted event arguments.</value>
		private SwipeStartedEventArgs? _swipeStartedEventArgs;

		#endregion

		#region Public Methods

		/// <summary>
		/// Load More method.
		/// </summary>
		public void LoadMore()
		{
			if (TempDataSource == null)
			{
				return;
			}

			if (_horizontalGridView != null)
			{
				var itemAdapter = (_horizontalGridView.GetAdapter() as ItemAdapter);
				if (itemAdapter != null)
				{
					if (AllowLoadMore)
					{
						itemAdapter.LoadMore(TempDataSource.Count - 1);
					}
					else
					{
						itemAdapter.LoadMore(TempDataSource.Count);
					}
				}
			}
			else if (ViewMode == ViewMode.Default)
			{
				LoadMoreDefaultMode(TempDataSource.Count - 1);
			}
		}

		/// <summary>
		/// On the click.
		/// </summary>
		/// <param name="view">Clicked view.</param>
		public void OnClick(View? view)
		{
			if (view == null)
			{
				return;
			}

			PlatformCarouselItem? item = view as PlatformCarouselItem ?? view.Parent as PlatformCarouselItem;

			if (item == null || item.IsItemSelected)
			{
				return;
			}

			if (item.Index <= _countItem - 1)
			{
				_isSelected = true;
				SelectedIndex = item.Index;
				_isSelected = false;
				_canExecuteSwipe = false;
			}

			if (item.Index == _countItem && LoadMoreItemsCount > 0)
			{
				LoadMoreDefaultMode(item.Index);
			}
		}

		/// <summary>
		/// Refresh the carousel.
		/// </summary>
		public void RefreshCarousel()
		{
			if (_viewMode != ViewMode.Default)
			{
				return;
			}

			PlatformCarouselItem? itemToArrange = null;

			if (!_isVirtualization)
			{
				if (TempDataSource != null && TempDataSource.Count > _selectedIndex)
				{
					itemToArrange = TempDataSource[_selectedIndex];
				}
			}
			else
			{
				if (_tempItemsSource != null && _tempItemsSource.Count > _selectedIndex)
				{
					itemToArrange = _tempItemsSource[_selectedIndex];
				}
			}

			if (itemToArrange != null)
			{
				Arrange(itemToArrange, true);
				UpdateLayout();
			}
		}

		/// <summary>
		/// Moves the next.
		/// </summary>
		public void MoveNext()
		{
			bool flowFactor = LayoutDirection == Android.Views.LayoutDirection.Ltr;
			if ((_selectedIndex < _countItem - 1 && flowFactor)
				|| (SelectedIndex > 0 && !flowFactor))
			{
				_isSelected = true;
				SelectedIndex = _selectedIndex + (flowFactor ? 1 : -1);
				_isSelected = false;
			}
		}

		/// <summary>
		/// Moves the previous.
		/// </summary>
		public void MovePrevious()
		{
			bool flowFactor = LayoutDirection == Android.Views.LayoutDirection.Ltr;

			if (_selectedIndex > _countItem - 1)
			{
				return;
			}

			_isSelected = true;

			if ((_selectedIndex > 0 && flowFactor) ||
				(SelectedIndex < _countItem - 1 && !flowFactor))
			{
				SelectedIndex = _selectedIndex - (flowFactor ? 1 : -1);
			}
			else
			{
				SelectedIndex = 0;
			}

			_isSelected = false;
		}

		/// <summary>
		/// On the intercept touch event.
		/// </summary>
		/// <exclude/>
		/// <returns><c>true</c>, if intercept touch event was enabled, <c>false</c> otherwise.</returns>
		/// <param name="ev">Event values.</param>
		public override bool OnInterceptTouchEvent(MotionEvent? ev)
		{
			if (ev == null)
			{
				return false;
			}

			switch (ev.Action)
			{
				case MotionEventActions.Down:
					HandleActionDown(ev);
					break;

				case MotionEventActions.Move:
					HandleActionMove(ev);
					break;
			}

			return !EnableInteraction || !IsEnabled
|| _isMoving || base.OnInterceptTouchEvent(ev);
		}

		/// <summary>
		/// Handles the Action Down event.
		/// </summary>
		/// <param name="ev">The MotionEvent.</param>
		void HandleActionDown(MotionEvent ev)
		{
			_xvalue = (int)ev.RawX;
			_isMoving = false;
			_isDown = true;
			_isSwipeRestricted = true;
			_swipeEventCalled = false;
		}

		/// <summary>
		/// Handles the Action Move event.
		/// </summary>
		/// <param name="ev">The MotionEvent.</param>
		void HandleActionMove(MotionEvent ev)
		{
			_dX = (int)ev.RawX;
			_dY = (int)ev.RawY;
			int diff = _dX - _xvalue;
			int diffY = _dY - _yvalue;

			_swipeStartedEventArgs ??= new SwipeStartedEventArgs();

			bool isVerticalSwipe = Math.Abs(diffY) > Math.Abs(diff);
			Parent?.RequestDisallowInterceptTouchEvent(!isVerticalSwipe);

			_yvalue = _dY;

			if (diff > GetDimen(_minDiffernce))
			{
				if (_viewMode == ViewMode.Linear && !_swipeEventCalled)
				{
					TriggerSwipeStarted(true);
				}
			}
			else if (diff < -GetDimen(_minDiffernce))
			{
				if (_viewMode == ViewMode.Linear && !_swipeEventCalled)
				{
					TriggerSwipeStarted(false);
				}
			}

			HandleSwipeMovement(diff);
		}

		/// <summary>
		/// Handles the swipe movement logic.
		/// </summary>
		/// <param name="diff">The difference in X position between current and initial touch points.</param>
		void HandleSwipeMovement(int diff)
		{
			if (diff > GetDimen(45))
			{
				if (_isDown)
				{
					_isMoving = true;
					_isDown = false;
				}

				if (diff > GetDimen(85))
				{
					_xvalue = _dX;
					_dX = 0;
					_isMoving = true;
				}
			}
			else if (diff < -GetDimen(45))
			{
				if (_isDown)
				{
					_isMoving = true;
					_isDown = false;
				}

				if (diff < -GetDimen(85))
				{
					_xvalue = _dX;
					_dX = 0;
					_isMoving = true;
				}
			}
		}

		/// <summary>
		/// On the touch event.
		/// </summary>
		/// <exclude/>
		/// <returns><c>true</c>, if touch event was enabled, <c>false</c> otherwise.</returns>
		/// <param name="e">motion events.</param>
		public override bool OnTouchEvent(MotionEvent? e)
		{
			if (e == null || TempDataSource == null)
			{
				return false;
			}

			if (TempDataSource.Count > 0)
			{
				switch (e.Action)
				{
					case MotionEventActions.Down:
						HandleTouchActionDown(e);
						break;

					case MotionEventActions.Move:
						HandleTouchActionMove(e);
						break;

					case MotionEventActions.Up:
						HandleTouchActionUp();
						break;
				}
			}

			return true;
		}

		/// <summary>
		/// Handles the touch event Action Down.
		/// </summary>
		/// <param name="e">The MotionEvent.</param>
		void HandleTouchActionDown(MotionEvent e)
		{
			_xvalue = (int)e.RawX;
			_yvalue = (int)e.RawY;
			_count = _selectedIndex;
			_isSwipeRestricted = true;
		}

		/// <summary>
		/// Handles the touch event Action Move.
		/// </summary>
		/// <param name="e">The MotionEvent.</param>
		void HandleTouchActionMove(MotionEvent e)
		{
			_dX = (int)e.GetX();
			int diff = _dX - _xvalue;
			_swipeStartedEventArgs ??= new SwipeStartedEventArgs();

			if (diff > GetDimen(_id) && SwipeMovementMode == SwipeMovementMode.MultipleItems)
			{
				HandleSwipeNext();
			}
			else if (diff > GetDimen(15) && SwipeMovementMode == SwipeMovementMode.SingleItem)
			{
				HandleSingleItemSwipeNext();
			}
			else if (diff < -GetDimen(_id) && SwipeMovementMode == SwipeMovementMode.MultipleItems)
			{
				HandleSwipePrevious();
			}
			else if (diff < -GetDimen(15) && SwipeMovementMode == SwipeMovementMode.SingleItem)
			{
				HandleSingleItemSwipePrevious();
			}

			AdjustForLargeSwipe(diff);
		}

		/// <summary>
		/// Handles the touch event Action Up.
		/// </summary>
		void HandleTouchActionUp()
		{
			ResetTouchVariables();

			if (_viewMode == ViewMode.Default)
			{
				ArrangeSelectedIndexItems();
			}

			if (!_isSwipeRestricted)
			{
				OnSwipeEnded(EventArgs.Empty);
				_isSwipeHappened = true;
			}

			_isSwipeRestricted = false;
			_swipeEventCalled = false;
		}

		/// <summary>
		/// Handles swipe next action when SwipeMovementMode is MultipleItems.
		/// </summary>
		void HandleSwipeNext()
		{
			_isMoving = true;

			if (_viewMode == ViewMode.Default && EnableInteraction && IsEnabled && _canExecuteFirst)
			{
				if (!_swipeEventCalled && SelectedIndex > 0)
				{
					TriggerSwipeStarted(true);
				}
				else
				{
					_isSwipeRestricted = true;
				}

				MovePrevious();
			}

			UpdateTouchVariablesAfterSwipe();
		}

		/// <summary>
		/// Handles swipe previous action when SwipeMovementMode is MultipleItems.
		/// </summary>
		void HandleSwipePrevious()
		{
			_isMoving = true;

			if (_viewMode == ViewMode.Default && EnableInteraction && IsEnabled)
			{
				if (!_swipeEventCalled && _selectedIndex < _countItem - 1)
				{
					TriggerSwipeStarted(false);
				}
				else
				{
					_isSwipeRestricted = true;
				}

				MoveNext();
			}

			UpdateTouchVariablesAfterSwipe();
		}

		/// <summary>
		/// Handles single item swipe next action.
		/// </summary>
		void HandleSingleItemSwipeNext()
		{
			_isMoving = true;

			if (_viewMode == ViewMode.Default && EnableInteraction && IsEnabled && _canExecuteFirst)
			{
				SwipePrevious();
			}

			UpdateTouchVariablesAfterSwipe();
		}

		/// <summary>
		/// Handles single item swipe previous action.
		/// </summary>
		void HandleSingleItemSwipePrevious()
		{
			_isMoving = true;

			if (_viewMode == ViewMode.Default && EnableInteraction && IsEnabled)
			{
				SwipeNext();
			}

			UpdateTouchVariablesAfterSwipe();
		}

		/// <summary>
		/// Updates the touch-related variables after a swipe action.
		/// </summary>
		void UpdateTouchVariablesAfterSwipe()
		{
			_count--;
			_xvalue = _dX;
			_yvalue = _dY;
			_dX = 0;
			_canExecuteFirst = true;
		}

		/// <summary>
		/// Adjusts for large swipe movements.
		/// </summary>
		/// <param name="diff">The difference in the X position between the current and initial touch points.</param>
		void AdjustForLargeSwipe(int diff)
		{
			if (_isMoving && (Math.Abs(_dX + _xvalue) > GetDimen(125)))
			{
				_id = 50;
				_isMoving = true;
			}
		}

		/// <summary>
		/// Resets touch-related variables to their default states.
		/// </summary>
		void ResetTouchVariables()
		{
			_canExecuteFirst = false;
			_canExecuteSwipe = true;

			_isMoving = false;
			_isDown = false;
			_id = 85;
			_xvalue = 0;
			_dX = 0;
			_yvalue = 0;
			_dY = 0;
		}

		/// <summary>
		/// Arranges items based on the selected index.
		/// </summary>
		void ArrangeSelectedIndexItems()
		{
			if (!_isVirtualization)
			{
				var selectedItem = TempDataSource?[_selectedIndex];
				if (selectedItem != null)
				{
					Arrange(selectedItem, true);
				}
			}
			else
			{
				if (_itemsSource != null && _tempItemsSource != null && _tempItemsSource.Count > _selectedIndex)
				{
					Arrange(_tempItemsSource[_selectedIndex], true);
				}
			}
		}

		/// <summary>
		/// To trigger the swipe started event
		/// </summary>
		/// <param name="isSwipedLeft"></param>
		void TriggerSwipeStarted(bool isSwipedLeft)
		{
			_isSwipeRestricted = false;
			if (_swipeStartedEventArgs != null)
			{
				_swipeStartedEventArgs.IsSwipedLeft = isSwipedLeft;
				OnSwipeStarted(_swipeStartedEventArgs);
			}
			_swipeEventCalled = true;
		}

		/// <summary>
		/// To swipe the next item
		/// </summary>
		void SwipeNext()
		{
			if (SwipeMovementMode == SwipeMovementMode.SingleItem && _canExecuteSwipe)
			{
				if (!_swipeEventCalled && _selectedIndex < _countItem - 1)
				{
					TriggerSwipeStarted(false);
				}
				else
				{
					_isSwipeRestricted = true;
				}

				MoveNext();
				_canExecuteSwipe = false;
			}
		}

		/// <summary>
		/// To swipe the previous item
		/// </summary>
		void SwipePrevious()
		{
			if (SwipeMovementMode == SwipeMovementMode.SingleItem && _canExecuteSwipe)
			{
				if (!_swipeEventCalled && SelectedIndex > 0)
				{
					TriggerSwipeStarted(true);
				}
				else
				{
					_isSwipeRestricted = true;
				}

				MovePrevious();
				_canExecuteSwipe = false;
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// On the selection changed.
		/// </summary>
		/// <param name="args">Arguments of selection changed event.</param>
		internal virtual void OnSelectionChanged(SelectionChangedEventArgs args)
		{
			_virtualView?.RaiseSelectionChanged(args);
		}

		/// <summary>
		/// On SwipeStarted.
		/// </summary>
		/// <param name="args">Arguments of SwipeStarted event.</param>
		internal void OnSwipeStarted(SwipeStartedEventArgs args)
		{
			_virtualView?.RaiseSwipeStarted(args);
		}

		/// <summary>
		/// On SwipeEnded.
		/// </summary>
		/// <param name="args">Arguments of SwipeEnded event.</param>
		internal virtual void OnSwipeEnded(EventArgs args)
		{
			_virtualView?.RaiseSwipeEnded(args);
		}

		/// <summary>
		/// On the collection changed.
		/// </summary>
		/// <param name="args">Arguments of collection changed event.</param>
		internal virtual void OnCollectionChanged(ItemsCollectionChangedEventArgs args)
		{
			CollectionChanged?.Invoke(this, args);
		}

		/// <summary>
		/// On the data changed.
		/// </summary>
		internal void OnDataChanged()
		{
			ItemsCollectionChangedEventArgs = new ItemsCollectionChangedEventArgs
			{
				Items = _isVirtualization ? _tempItemsSource : TempDataSource,
				PlatformCarousel = this
			};

			OnCollectionChanged(ItemsCollectionChangedEventArgs);
			Refresh();
		}

		/// <summary>
		/// Refreshes the carousel by clearing and re-adding items based on the current state.
		/// </summary>
		internal void Refresh()
		{
			_tempList?.Clear();

			bool hasItems = (TempDataSource != null && TempDataSource.Count > 0) ||
							(_isVirtualization && ItemsSource != null && _tempItemsSource != null && _tempItemsSource.Count > 0);

			if (hasItems)
			{
				RemoveAllViews();

				// Handle the default view mode
				if (_viewMode == ViewMode.Default)
				{
					if (_isSizeChanged)
					{
						if (!_isVirtualization)
						{
							AddCarouselItem();
						}
						else
						{
							PositionCalculation();
						}
					}
				}
				else
				{
					RefreshNonDefaultViewMode();
				}
			}
			else if (Handle != IntPtr.Zero)
			{
				RemoveAllViews();
			}
		}

		/// <summary>
		/// Refreshes the carousel in non-default view modes.
		/// </summary>
		void RefreshNonDefaultViewMode()
		{
			if (!_isVirtualization)
			{
				ItemsArrangeLinear();
			}
			else
			{
				TempDataSource?.Clear();

				if (ItemsSourceCollection != null && ItemsSourceCollection.Count > 0)
				{
					foreach (var localItem in ItemsSourceCollection)
					{
						TempDataSource?.Add(localItem);
					}
				}
				ItemsArrangeLinear();
			}
		}
		#endregion

		#region Override Methods
		/// <summary>
		/// Called when the layout is changed.
		/// </summary>
		/// <exclude/>
		/// <param name="changed">If set to <c>true</c> indicates the layout has changed.</param>
		/// <param name="left">Left coordinate.</param>
		/// <param name="top">Top coordinate.</param>
		/// <param name="right">Right coordinate.</param>
		/// <param name="bottom">Bottom coordinate.</param>
		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);

			if (_viewMode != ViewMode.Default)
			{
				return;
			}

			if (_isVirtualization)
			{
				Refresh();
			}

			if (changed)
			{
				if (!_isVirtualization)
				{
					ArrangeSelectedItemFromTempDataSource();
				}
				else
				{
					ArrangeSelectedItemFromTempItemsSource();
				}

				UpdateLayout();
			}
		}

		/// <summary>
		/// Arranges the selected item from the temporary data source.
		/// </summary>
		void ArrangeSelectedItemFromTempDataSource()
		{
			if (TempDataSource != null && TempDataSource.Count > _selectedIndex)
			{
				var instance = TempDataSource[_selectedIndex];
				Arrange(instance, false);
			}
		}

		/// <summary>
		/// Arranges the selected item from the temporary items source.
		/// </summary>
		void ArrangeSelectedItemFromTempItemsSource()
		{
			if (_tempItemsSource != null && _tempItemsSource.Count > _selectedIndex)
			{
				var instance = _tempItemsSource[_selectedIndex];
				Arrange(instance, false);
			}
		}

		/// <summary>
		/// Called when the size is changed.
		/// </summary>
		/// <exclude/>
		/// <param name="w">The new width.</param>
		/// <param name="h">The new height.</param>
		/// <param name="oldw">The old width value.</param>
		/// <param name="oldh">The old height value.</param>
		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			ControlWidth = w;
			ControlHeight = h;

			if (_viewMode == ViewMode.Default)
			{
				_isSizeChanged = true;

				if (!_isVirtualization)
				{
					SizedChangedRefresh(w, h);
				}
				else
				{
					HandleVirtualizationResize(w, h);
				}
			}

			base.OnSizeChanged(w, h, oldw, oldh);
		}

		/// <summary>
		/// Handles size changes when virtualization is enabled.
		/// </summary>
		/// <param name="w">The new width.</param>
		/// <param name="h">The new height.</param>
		void HandleVirtualizationResize(int w, int h)
		{
			if (_itemsSource != null && w >= 20 && h >= 20)
			{
				SizedChangedRefresh(w, h);
				Refresh();
			}

			if (_itemsSource != null && w >= 20 && h >= 20)
			{
				Refresh();
			}
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the PlatformCarousel.
		/// </summary>
		/// <exclude/>
		/// <param name="disposing">If set to <c>true</c>, it disposes managed resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ReleaseManagedResources();
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Releases managed resources.
		/// </summary>
		void ReleaseManagedResources()
		{
			TempDataSource = null;
			_tempDataSourceValue = null;
			AllowLoadMoreDataSource = null;
			_itemsSource = null;
			_tempItemsSource = null;
			_tempList = null;
			ItemsSourceCollection = null;
			_adapter = null;
			_loadedCustomView = null;
			_previousItems = null;
			_nextItems = null;

			PlatformCarousel.DisposeAndClear(ref _linearLayoutManager);
			PlatformCarousel.DisposeAndClear(ref _linearLayout);
		}

		/// <summary>
		/// Disposes and clears a disposable resource.
		/// </summary>
		/// <param name="disposable">The disposable resource reference.</param>
		static void DisposeAndClear<T>(ref T? disposable) where T : class, IDisposable
		{
			disposable?.Dispose();
			disposable = null;
		}


		#endregion

		#region Private Methods

		/// <summary>
		/// Sized the changed refresh.
		/// </summary>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		void SizedChangedRefresh(int w, int h)
		{
			if (TempDataSource != null && TempDataSource.Count > 0)
			{
				if (w >= 20 && h >= 20)
				{
					PlatformCarouselItem selection = (PlatformCarouselItem)TempDataSource[_selectedIndex];
					Arrange(selection, true);
					Refresh();
				}
			}
		}

		/// <summary>
		/// Load More method for the Default mode.
		/// </summary>
		/// <param name="index">The Index.</param>
		void LoadMoreDefaultMode(int index)
		{
			if (AllowLoadMore)
			{
				RemoveViewAt(index);
				TempDataSource?.RemoveAt(index);
			}

			if (AllowLoadMoreDataSource != null)
			{
				AddLoadMoreItemsFromDataSource();

				OnDataChanged();

				if (AllowLoadMoreDataSource.Count > 0 && AllowLoadMore)
				{
					AddLoadMoreItem();
				}

				UpdateCountItem();
				UpdateLayout();
			}
		}

		/// <summary>
		/// Adds load more items from the data source.
		/// </summary>
		void AddLoadMoreItemsFromDataSource()
		{
			if (AllowLoadMoreDataSource != null)
			{
				var itemsToAdd = AllowLoadMoreDataSource.Count >= _maximumIndicator
								 ? _maximumIndicator
								 : AllowLoadMoreDataSource.Count;

				for (int i = 0; i < itemsToAdd; i++)
				{
					AddLoadMoreItems(0);
				}
			}
		}

		/// <summary>
		/// Adds the load more item to the data source.
		/// </summary>
		void AddLoadMoreItem()
		{
			if (Context != null)
			{
				PlatformCarouselItem loadMoreItem = new PlatformCarouselItem(Context);
				LoadMoreAccessibility(loadMoreItem);
				loadMoreItem.ContentView = LoadMoreView ?? GetDefaultLoadMoreView();
				TempDataSource?.Add(loadMoreItem);
			}
		}

		/// <summary>
		/// Updates the CountItem property based on the current data source.
		/// </summary>
		void UpdateCountItem()
		{
			if (ItemsSourceCollection != null && ItemsSourceCollection.Count > 0)
			{
				_countItem = ItemsSourceCollection.Count;
			}
		}

		/// <summary>
		/// Get the default load more view
		/// </summary>
		/// <returns></returns>
		View GetDefaultLoadMoreView()
		{
			return new TextView(Context)
			{
				Text = SfCarouselResources.LoadMoreText,
				Gravity = GravityFlags.Center,
				TextSize = 18
			};
		}

		/// <summary>
		/// Add the load more items
		/// </summary>
		/// <param name="index"></param>
		void AddLoadMoreItems(int index)
		{
			if (TempDataSource == null || AllowLoadMoreDataSource == null)
			{
				return;
			}

			if (index < 0 || index >= AllowLoadMoreDataSource.Count)
			{
				return;  // Ensure index is within bounds
			}

			var item = AllowLoadMoreDataSource[index];
			TempDataSource.Add(item);
			if (_tempDataSourceValue != null)
			{
				_tempDataSourceValue.Add(item);
				ItemsSourceCollection = _tempDataSourceValue;
			}
			AllowLoadMoreDataSource.RemoveAt(index);
		}

		/// <summary>
		/// Handles the selection logic for the carousel.
		/// </summary>
		void Selection(bool canStopEvent)
		{
			if (_viewMode == ViewMode.Default)
			{
				DeselectCurrentItem();
				OnIndexChanged(_selectedIndex);
			}
			else
			{
				Refresh();
			}

			UpdateSelectionChangedEventArgs();
			// Preventing the event from being triggered multiple times
			if (!canStopEvent)
			{
				RefreshSelection();
			}
			_prevIndex = _selectedIndex;
		}

		/// <summary>
		/// Deselects the currently selected item.
		/// </summary>
		void DeselectCurrentItem()
		{
			if (!_isVirtualization)
			{
				DeselectTempDataSourceItem();
			}
			else
			{
				DeselectVirtualizedItems();
			}
		}

		/// <summary>
		/// Deselects the current item from the temporary data source.
		/// </summary>
		void DeselectTempDataSourceItem()
		{
			if (TempDataSource != null && TempDataSource.Count > 0 && TempDataSource.Count > _selectedIndex)
			{
				TempDataSource[_selectedIndex].IsItemSelected = false;
			}
		}

		/// <summary>
		/// Deselects the current item from the virtualized data sources.
		/// </summary>
		void DeselectVirtualizedItems()
		{
			if (_tempItemsSource != null && _tempItemsSource.Count > _selectedIndex)
			{
				_tempItemsSource[_selectedIndex].IsItemSelected = false;
			}
		}

		/// <summary>
		/// Updates the selection changed event arguments.
		/// </summary>
		void UpdateSelectionChangedEventArgs()
		{
			SelectionChangedEventArgs = new SelectionChangedEventArgs();
		}

		/// <summary>
		/// Handles the collection changed event.
		/// </summary>
		/// <param name="sender">Sender value.</param>
		/// <param name="e">Event arguments.</param>
		void Handle_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (sender == null || TempDataSource == null)
			{
				return;
			}

			bool hasItemsChanged = ItemsSource is IList items && items.Count != TempDataSource.Count;

			if (hasItemsChanged)
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					AddNewItem(e.NewStartingIndex);
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					RemoveItem(e.NewStartingIndex);
				}

				OnDataChanged();
				_countItem = TempDataSource.Count;
				UpdateLayout();
			}
		}

		/// <summary>
		/// Adds a new item to the collection at the specified index.
		/// </summary>
		/// <param name="index">The index at which to add the new item.</param>
		void AddNewItem(int index)
		{
			if (Context != null)
			{
				var carouselItem = new PlatformCarouselItem(Context)
				{
					ParentItem = this,
					LayoutParameters = new ViewGroup.LayoutParams(ItemWidth, ItemHeight),
					Index = index
				};

				TempDataSource?.Add(carouselItem);
				_tempItemsSource?.Add(carouselItem);
			}
		}

		/// <summary>
		/// Removes an item from the collection at the specified index.
		/// </summary>
		/// <param name="index">The index at which to remove the item.</param>
		void RemoveItem(int index)
		{
			if (TempDataSource == null)
			{
				return;
			}

			if (TempDataSource.Count > 0)
			{
				TempDataSource.RemoveAt(index + 1);
			}

			_tempItemsSource?.RemoveAt(index + 1);
		}

		/// <summary>
		/// Adds items to the carousel and handles load more functionality.
		/// </summary>
		void AddItems()
		{
			PopulateItemsSource();

			UpdateSelectedIndex();

			AddItemsToTempDataSource();

			FinalizeItemAddition();
			if (ItemsSourceCollection != null)
			{
				_countItem = ItemsSourceCollection.Count;
			}

			OnIndexChanged(_selectedIndex);
		}

		/// <summary>
		/// Adds items to the temporary data source if not in virtualization mode.
		/// </summary>
		void AddItemsToTempDataSource()
		{
			if (!_isVirtualization && TempDataSource != null && ItemsSourceCollection != null && ItemsSourceCollection.Count > 0)
			{
				TempDataSource.AddRange(ItemsSourceCollection);
			}
		}

		/// <summary>
		/// Updates the selected index to ensure it's within the valid range.
		/// </summary>
		void UpdateSelectedIndex()
		{
			if (ItemsSourceCollection != null && ItemsSourceCollection.Count > 0 && _selectedIndex >= ItemsSourceCollection.Count)
			{
				SelectedIndex = ItemsSourceCollection.Count - 1;
			}
		}


		/// <summary>
		/// Populates the items source with carousel items.
		/// </summary>
		void PopulateItemsSource()
		{
			_tempItemsSource = [];
			var dummyCollection = new List<PlatformCarouselItem>();

			if (ItemsSource is not IList items)
			{
				return;
			}

			for (int count = 0; count < items.Count; count++)
			{
				var carouselItem = CreateCarouselItem(count);
				if (carouselItem != null)
				{
					_tempItemsSource.Add(carouselItem);
					dummyCollection.Add(carouselItem);
				}
			}

			SetupLoadMoreFunctionality(dummyCollection);
		}

		/// <summary>
		/// Creates a new carousel item with the specified index.
		/// </summary>
		/// <param name="index">The index of the item.</param>
		/// <returns>A new PlatformCarouselItem instance.</returns>
		PlatformCarouselItem? CreateCarouselItem(int index)
		{
			if (Context != null)
			{
				return new PlatformCarouselItem(Context)
				{
					Index = index,
					LayoutParameters = new ViewGroup.LayoutParams(ItemWidth, ItemHeight)
				};
			}
			return null;
		}

		/// <summary>
		/// Sets up the load more functionality for the carousel.
		/// </summary>
		/// <param name="dummyCollection">The collection of dummy items.</param>
		void SetupLoadMoreFunctionality(List<PlatformCarouselItem> dummyCollection)
		{
			if (LoadMoreItemsCount > 0)
			{
				_tempDataSourceValue = [];
				if (_tempItemsSource != null && _tempItemsSource.Count > _maximumIndicator)
				{
					for (int i = 0; i < _maximumIndicator; i++)
					{
						AllowLoadMoreDataSource = dummyCollection;
						_tempDataSourceValue.Add(AllowLoadMoreDataSource[0]);
						AllowLoadMoreDataSource.RemoveAt(0);
					}

					ItemsSourceCollection = _tempDataSourceValue;
					_isEnablingLoadMoreButton = true;
				}
				else
				{
					ItemsSourceCollection = dummyCollection;
					if (_tempItemsSource != null)
					{
						_isEnablingLoadMoreButton = _tempItemsSource.Count == _maximumIndicator || _tempItemsSource.Count < _maximumIndicator;
					}
				}
			}
			else
			{
				ItemsSourceCollection = dummyCollection;
			}
		}

		/// <summary>
		/// Finalizes the item addition process.
		/// </summary>
		void FinalizeItemAddition()
		{
			_isSizeChanged = true;
			OnDataChanged();

			if (_allowLoadMore && _isEnablingLoadMoreButton && Context != null && TempDataSource != null && TempDataSource.Count > 0)
			{
				var loadMoreItem = new PlatformCarouselItem(Context);
				LoadMoreAccessibility(loadMoreItem);
				loadMoreItem.ContentView = LoadMoreView ?? GetDefaultLoadMoreView();
				TempDataSource.Add(loadMoreItem);
			}
		}

		/// <summary>
		/// Refresh the selection.
		/// </summary>
		void RefreshSelection()
		{
			if (SelectionChangedEventArgs != null)
			{
				SelectionChangedEventArgs.OldItem = GetMauiItem(_prevIndex);
				SelectionChangedEventArgs.NewItem = GetMauiItem(SelectedIndex);
				OnSelectionChanged(SelectionChangedEventArgs);
			}
		}

		/// <summary>
		/// Retrieves the Maui item at the specified index.
		/// </summary>
		/// <param name="selectedIndex">The index of the item to retrieve.</param>
		/// <returns>The Maui item at the specified index, or null if not found.</returns>
		object? GetMauiItem(int selectedIndex)
		{
			if (_virtualView?.ItemsSource == null)
			{
				return null;
			}

			int index = 0;
			foreach (var item in _virtualView.ItemsSource)
			{
				if (index == selectedIndex)
				{
					return item;
				}
				index++;
			}

			return null;
		}

		/// <summary>
		/// Updates the layout.
		/// </summary>
		async void UpdateLayout()
		{
			await Task.Delay(1);

			if (_viewMode == ViewMode.Default)
			{
				Refresh();
			}

			if (TempDataSource != null)
			{
				ArrangeItemsBasedOnVirtualization();
			}
		}

		/// <summary>
		/// Arranges items based on the virtualization setting.
		/// </summary>
		void ArrangeItemsBasedOnVirtualization()
		{
			if (!_isVirtualization)
			{
				ArrangeNonVirtualizedItems();
			}
			else
			{
				ArrangeVirtualizedItems();
			}
		}

		/// <summary>
		/// Arranges non-virtualized items.
		/// </summary>
		void ArrangeNonVirtualizedItems()
		{
			if (TempDataSource != null && TempDataSource.Count > 0 && _selectedIndex >= 0 && TempDataSource.Count > _selectedIndex)
			{
				Arrange(TempDataSource[_selectedIndex], false);
			}
		}

		/// <summary>
		/// Arranges virtualized items.
		/// </summary>
		void ArrangeVirtualizedItems()
		{
			if (_selectedIndex < 0)
			{
				return;
			}

			if (_itemsSource != null && _tempItemsSource != null && _tempItemsSource.Count > 0 && _tempItemsSource.Count > _selectedIndex)
			{
				Arrange(_tempItemsSource[_selectedIndex], false);
			}
		}

		/// <summary>
		/// Invokes the load more functionality.
		/// </summary>
		void AllowLoadMoreInvoke()
		{
			if (TempDataSource == null)
			{
				return;
			}

			if (_itemsSource != null)
			{
				HandleLoadMoreForItemsSource();
			}
		}

		/// <summary>
		/// Handles load more functionality for itemsSource.
		/// </summary>
		void HandleLoadMoreForItemsSource()
		{
			if (ItemsSourceCollection == null || TempDataSource == null)
			{
				return;
			}

			RemoveAllViews();

			if (ItemsSourceCollection.Count < TempDataSource.Count)
			{
				TempDataSource.RemoveAt(ItemsSourceCollection.Count);
			}

			if (ItemsSourceCollection.Count == TempDataSource.Count)
			{
				_countItem = ItemsSourceCollection.Count;
				Refresh();
				if (_allowLoadMore)
				{
					AddLoadMoreItem();
				}
			}

			UpdateLayout();
		}

		/// <summary>
		/// Gets the dimension.
		/// </summary>
		/// <returns>The dimension.</returns>
		/// <param name="value">Res identifier.</param>
		double GetDimen(int value)
		{
			if (Resources != null)
			{
				return TypedValue.ApplyDimension(ComplexUnitType.Dip, value, Resources.DisplayMetrics);
			}

			return 0;
		}

		/// <summary>
		/// Gets the density of the android resource.
		/// </summary>
		/// <returns>The density of the android resource.</returns>
		double GetDensity()
		{
			double density = 0;
			if (Context != null && Context.Resources != null && Context.Resources.DisplayMetrics != null)
			{
				density = Context.Resources.DisplayMetrics.Density;
			}

			return density;
		}

		/// <summary>
		/// Called when the selected index is changed.
		/// </summary>
		/// <param name="selectedIndex">The new selected index.</param>
		void OnIndexChanged(int selectedIndex)
		{
			if (HasValidDataSource())
			{
				if (!_isSelected || _isVirtualization)
				{
					Refresh();
				}

				if (!_isVirtualization)
				{
					ChangeSelectedItem(TempDataSource, selectedIndex);
				}
				else
				{
					ChangeSelectedItem(_tempItemsSource, selectedIndex);
				}
			}
		}

		/// <summary>
		/// Checks if there is a valid data source.
		/// </summary>
		/// <returns><c>true</c> if there is a valid data source; otherwise, <c>false</c>.</returns>
		bool HasValidDataSource()
		{
			return (TempDataSource != null && TempDataSource.Count > 0);
		}

		/// <summary>
		/// Changes the selected item in the specified data source.
		/// </summary>
		/// <param name="dataSource">The data source.</param>
		/// <param name="selectedIndex">The selected index.</param>
		void ChangeSelectedItem(IList<PlatformCarouselItem>? dataSource, int selectedIndex)
		{
			if (dataSource != null && dataSource.Count > selectedIndex)
			{
				OnSelectedItemChanged(dataSource[selectedIndex]);
			}
		}

		/// <summary>
		/// On the view mode changed.
		/// </summary>
		void OnViewModeChanged()
		{
			Refresh();
			if ((TempDataSource != null && TempDataSource.Count > 0))
			{
				if (!_isVirtualization)
				{
					OnSelectedItemChanged(TempDataSource[_selectedIndex]);
				}
				else
				{
					if (_isSizeChanged)
					{
						if (_itemsSource != null && _tempItemsSource != null)
						{
							OnSelectedItemChanged(_tempItemsSource[_selectedIndex]);
						}
					}
				}
			}
		}

		/// <summary>
		/// On the selected item changed.
		/// </summary>
		/// <param name="selectedItemArg">Selected item.</param>
		void OnSelectedItemChanged(object selectedItemArg)
		{
			if (_viewMode == ViewMode.Default)
			{
				PlatformCarouselItem instance = (PlatformCarouselItem)selectedItemArg;
				Arrange(instance, true);
			}
		}

		/// <summary>
		/// Removes the item from parent.
		/// </summary>
		/// <param name="item">carousel Item.</param>
		static void RemoveItemFromParent(PlatformCarouselItem item)
		{
			if (item.Parent != null)
			{
				ViewGroup parentItem = (ViewGroup)item.Parent;
				parentItem?.RemoveView(item);
			}
		}

		/// <summary>
		/// Arranges the items.
		/// </summary>
		/// <param name="instance">The carousel item instance to be arranged.</param>
		/// <param name="animate">Indicates whether to animate the arrangement.</param>
		void Arrange(PlatformCarouselItem instance, bool animate)
		{
			if (!_isVirtualization)
			{
				ArrangeNonVirtualizedItems(instance, animate);
			}
			else
			{
				ArrangeVirtualizedItems(instance, animate);
			}
		}

		/// <summary>
		/// Arranges non-virtualized items.
		/// </summary>
		/// <param name="instance">The selected carousel item instance.</param>
		/// <param name="animate">Indicates whether to animate the arrangement.</param>
		void ArrangeNonVirtualizedItems(PlatformCarouselItem instance, bool animate)
		{
			if (TempDataSource != null)
			{
				foreach (View temp in TempDataSource)
				{
					var newItem = (PlatformCarouselItem)temp;
					if (newItem != instance)
					{
						newItem.IsItemSelected = false;
						ArrangeItems(newItem, _rotationAngle, _scaleOffset, animate, newItem.Index);
					}
					else
					{
						instance.IsItemSelected = true;
						ArrangeItems(instance, 0, 1f, animate, newItem.Index);
					}
				}
			}
			ArrangeItemsPosition();
		}

		/// <summary>
		/// Arranges virtualized items.
		/// </summary>
		/// <param name="instance">The selected carousel item instance.</param>
		/// <param name="animate">Indicates whether to animate the arrangement.</param>
		void ArrangeVirtualizedItems(PlatformCarouselItem instance, bool animate)
		{
			if (_start == -1 || _end == -1 || TempDataSource == null)
			{
				return;
			}

			if (TempDataSource != null)
			{
				ArrangeVirtualizedDataSource(instance, animate, TempDataSource);
			}
		}

		/// <summary>
		/// Arranges items in a virtualized data source.
		/// </summary>
		/// <param name="instance">The selected carousel item instance.</param>
		/// <param name="animate">Indicates whether to animate the arrangement.</param>
		/// <param name="source">The virtualized data source.</param>
		void ArrangeVirtualizedDataSource(PlatformCarouselItem instance, bool animate, IList<PlatformCarouselItem> source)
		{
			int count = 0;
			for (int i = _start; i <= _end; i++)
			{
				var item = source[count];
				if (i != _selectedIndex)
				{
					item.IsItemSelected = false;
					ArrangeNonSelectedItem(item, animate, i);
				}
				else
				{
					instance.IsItemSelected = true;
					ArrangeItems(instance, 0, 1f, animate, i);
				}
				count++;
			}
		}

		/// <summary>
		/// Arranges a non-selected item.
		/// </summary>
		/// <param name="item">The item to arrange.</param>
		/// <param name="animate">Indicates whether to animate the arrangement.</param>
		/// <param name="index">The index of the item.</param>
		void ArrangeNonSelectedItem(PlatformCarouselItem item, bool animate, int index)
		{
			int endItemIndex = _end - (_start - _preStart);
			if (!_isSelected || index <= endItemIndex)
			{
				ArrangeItems(item, _rotationAngle, _scaleOffset, animate, index);
			}
			else
			{
				ArrangeItems(item, _rotationAngle, _scaleOffset, false, index);
			}
		}

		/// <summary>
		/// Arranges the items' Z positions depending on their selection state.
		/// </summary>
		void ArrangeItemsPosition()
		{
			if (!_isSelected)
			{
				return;
			}

			PlatformCarousel.BringItemsToFront(_nextItems, true);
			PlatformCarousel.BringItemsToFront(_previousItems, false);

			if (_viewMode == ViewMode.Default)
			{
				BringSelectedItemToFront();
			}

			if (_isSwipeHappened && _viewMode == ViewMode.Default)
			{
				_isSwipeHappened = false;
				Refresh();
			}
		}

		/// <summary>
		/// Brings the items in the list to the front.
		/// </summary>
		/// <param name="items">The list of items.</param>
		/// <param name="reverse">Whether to reverse the order when bringing to front.</param>
		static void BringItemsToFront(List<PlatformCarouselItem>? items, bool reverse)
		{
			if (items == null)
			{
				return;
			}

			var newList = new List<PlatformCarouselItem>(items);
			items.Clear();

			if (reverse)
			{
				newList.Reverse();
			}

			foreach (var item in newList)
			{
				item.BringToFront();
			}
		}

		/// <summary>
		/// Brings the selected item to the front.
		/// </summary>
		void BringSelectedItemToFront()
		{
			if (ItemsSource != null && _tempItemsSource != null && _tempItemsSource.Count > 0)
			{
				BringChildToFront((PlatformCarouselItem)_tempItemsSource[_selectedIndex]);
			}
		}

		/// <summary>
		/// Arranges the items.
		/// </summary>
		/// <param name="item">Item of carousel.</param>
		/// <param name="rotationAngleArgs">Rotation angle arguments.</param>
		/// <param name="zOffset">Z offset.</param>
		/// <param name="isanimate">If set to <c>true</c> is animate.</param>
		/// <param name="index">Index value.</param>
		void ArrangeItems(PlatformCarouselItem item, int rotationAngleArgs, float zOffset, bool isanimate, int index)
		{
			bool flowFactor = LayoutDirection == Android.Views.LayoutDirection.Ltr;
			int position = index - _selectedIndex;

			SetVerticalPosition(item);

			if (item.IsItemSelected)
			{
				ArrangeSelectedItem(item, zOffset, rotationAngleArgs, isanimate);
			}
			else
			{
				ArrangeNonSelectedItem(item, index, position, zOffset, ref rotationAngleArgs, isanimate, flowFactor);
			}

			item.SetOnClickListener(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		void SetVerticalPosition(PlatformCarouselItem item)
		{
			if (_isVirtualization)
			{
				_yPos = (_controlHeight / 2) - (_itemHeight / 2);
			}
			else if (item.LayoutParameters != null)
			{
				_yPos = (_controlHeight / 2) - (item.LayoutParameters.Height / 2);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="zOffset"></param>
		/// <param name="rotationAngleArgs"></param>
		/// <param name="animate"></param>
		void ArrangeSelectedItem(PlatformCarouselItem item, float zOffset, int rotationAngleArgs, bool animate)
		{
			_xPos = (Width / 2) - (_itemWidth / 2);
			AnimateItem(item, zOffset, rotationAngleArgs, animate);
			item.BringToFront();
			item.SetY(_yPos);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="position"></param>
		/// <param name="zOffset"></param>
		/// <param name="rotationAngleArgs"></param>
		/// <param name="animate"></param>
		/// <param name="flowFactor"></param>
		void ArrangeNonSelectedItem(PlatformCarouselItem item, int index, int position, float zOffset, ref int rotationAngleArgs, bool animate, bool flowFactor)
		{
			if (index < _selectedIndex)
			{
				ArrangePreviousItem(item, index, position, flowFactor, ref rotationAngleArgs);
			}
			else
			{
				ArrangeNextItem(item, index, position, flowFactor, ref rotationAngleArgs);
			}

			AnimateItem(item, zOffset, rotationAngleArgs, animate);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="position"></param>
		/// <param name="flowFactor"></param>
		/// <param name="rotationAngleArgs"></param>
		void ArrangePreviousItem(PlatformCarouselItem item, int index, int position, bool flowFactor, ref int rotationAngleArgs)
		{
			if (index == (_selectedIndex - 1))
			{
				if (!_isVirtualization)
				{
					if (flowFactor)
					{
						if (item.LayoutParameters != null)
						{
							_xPos = (_controlWidth / 2) - item.LayoutParameters.Width - _selectedItemOffset;
						}
					}
					else
					{
						_xPos = (_controlWidth / 2) + _selectedItemOffset;
					}
				}
				else
				{
					if (flowFactor)
					{
						_xPos = (_controlWidth / 2) - _itemWidth - _selectedItemOffset;
					}
					else
					{
						_xPos = (_controlWidth / 2) + _selectedItemOffset;
					}
				}
			}
			else
			{
				if (!_isVirtualization)
				{
					if (flowFactor)
					{
						if (item.LayoutParameters != null)
						{
							_xPos = (_controlWidth / 2) - item.LayoutParameters.Width + (_moffset * (position + 1)) - _selectedItemOffset;
						}
					}
					else
					{
						_xPos = (_controlWidth / 2) + ((-_moffset * (position + 1)) + _selectedItemOffset);
					}
				}
				else
				{
					if (flowFactor)
					{
						_xPos = (_controlWidth / 2) - _itemWidth + (_moffset * (position + 1)) - _selectedItemOffset;
					}
					else
					{
						_xPos = (_controlWidth / 2) + ((-_moffset * (position + 1)) + _selectedItemOffset);
					}
				}
			}

			_previousItems?.Add(item);

			rotationAngleArgs = (flowFactor ? 1 : -1) * rotationAngleArgs;
		}

		/// <summary>
		/// Arranges the next item in the carousel.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="position"></param>
		/// <param name="flowFactor"></param>
		/// <param name="rotationAngleArgs"></param>
		void ArrangeNextItem(PlatformCarouselItem item, int index, int position, bool flowFactor, ref int rotationAngleArgs)
		{
			if (index == (_selectedIndex + 1))
			{
				ArrangeImmediateNextItem(flowFactor, item);
			}
			else
			{
				ArrangeFurtherNextItem(flowFactor, position, item);
			}

			AdjustRotationForNextItem(flowFactor, ref rotationAngleArgs);
			AddItemToNextItems(item);
		}

		/// <summary>
		/// Arranges items further away from the selected item.
		/// </summary>
		/// <param name="isLeftToRight">Indicates if the layout direction is left to right.</param>
		/// <param name="position">The position relative to the selected item.</param>
		void ArrangeFurtherNextItem(bool isLeftToRight, int position, PlatformCarouselItem item)
		{
			if (isLeftToRight)
			{
				_xPos = (_controlWidth / 2) + (_moffset * (position - 1)) + _selectedItemOffset;
			}
			else
			{
				_xPos = (_controlWidth / 2) - (_moffset * (position - 1)) - ((!_isVirtualization ? _itemWidth : item.LayoutParameters != null ? item.LayoutParameters.Width : 0) + _selectedItemOffset);
			}
		}

		/// <summary>
		/// Arranges the item immediately next to the selected item.
		/// </summary>
		/// <param name="isLeftToRight">Indicates if the layout direction is left to right.</param>
		void ArrangeImmediateNextItem(bool isLeftToRight, PlatformCarouselItem item)
		{
			if (isLeftToRight)
			{
				_xPos = (_controlWidth / 2) + _selectedItemOffset;
			}
			else
			{
				_xPos = (_controlWidth / 2) - ((!_isVirtualization ? _itemWidth : item.LayoutParameters != null ? item.LayoutParameters.Width : 0) + _selectedItemOffset);
			}
		}

		/// <summary>
		/// Adjusts the rotation angle for the next item based on layout direction.
		/// </summary>
		/// <param name="isLeftToRight">Indicates if the layout direction is left to right.</param>
		/// <param name="rotationAngle">The rotation angle to adjust.</param>
		void AdjustRotationForNextItem(bool isLeftToRight, ref int rotationAngle)
		{
			if (isLeftToRight)
			{
				rotationAngle = -rotationAngle;
			}
		}

		/// <summary>
		/// Adds the item to the list of next items.
		/// </summary>
		/// <param name="item">The carousel item to add.</param>
		void AddItemToNextItems(PlatformCarouselItem item)
		{
			_nextItems?.Add(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="zOffset"></param>
		/// <param name="rotationAngleArgs"></param>
		/// <param name="animate"></param>
		void AnimateItem(PlatformCarouselItem item, float zOffset, int rotationAngleArgs, bool animate)
		{
			Animate(item, zOffset, (item.GetX() != 0 && animate) ? _duration : 0, rotationAngleArgs, _xPos);
		}

		/// <summary>
		/// animate the carousel view
		/// </summary>
		/// <param name="item"> carousel item</param>
		/// <param name="zOffset"> three dimension offset</param>
		/// <param name="mduration">duration value</param>
		/// <param name="nrotationangle"> rotation angle</param>
		/// <param name="pos"> position of view</param>
		void Animate(PlatformCarouselItem item, float zOffset, int mduration, int nrotationangle, float pos)
		{
			item.SetY(_yPos);
			using var interpolate = new DecelerateInterpolator(1);
			item.Animate()?.RotationY(nrotationangle)
				.ScaleY(zOffset)
				.ScaleX(zOffset)
				.X(pos)
				.SetDuration(mduration)
				.WithLayer()
				.SetInterpolator(interpolate).Start();
		}

		/// <summary>
		/// Method to set accessibility property to load more item.
		/// </summary>
		/// <param name="loadMoreItem">load more item</param>
		void LoadMoreAccessibility(PlatformCarouselItem loadMoreItem)
		{
			if (loadMoreItem != null)
			{
				loadMoreItem.ContentDescription = AutomationId + " Load More. Tap to load more items";
			}
		}

		/// <summary>
		/// Position the calculation for the carousel items.
		/// </summary>
		void PositionCalculation()
		{
			SavePreviousPosition();
			InitializePositionVariables();

			if (_tempItemsSource != null)
			{
				CalculateItemPositions(_tempItemsSource.Count);
			}

			UpdateItems();
		}

		/// <summary>
		/// Saves the previous start and end positions.
		/// </summary>
		void SavePreviousPosition()
		{
			if (_start != -1 && _end != -1)
			{
				_preStart = _start;
				_preEnd = _end;
			}
		}

		/// <summary>
		/// Initializes position-related variables.
		/// </summary>
		void InitializePositionVariables()
		{
			_centerPosition = MeasuredWidth / 2;
			_startPosition = -_centerPosition;
			_endPosition = _centerPosition * 2;
			_xPositionLeft = (_controlWidth / 2) - (_itemWidth + _selectedItemOffset) - ((_moffset * _selectedIndex) - 1);
			_xPositionRight = (_controlWidth / 2) + _selectedItemOffset + _moffset;
		}

		/// <summary>
		/// Calculates item positions based on the number of items.
		/// </summary>
		/// <param name="itemCount">The number of items.</param>
		void CalculateItemPositions(int itemCount)
		{
			bool startEnabled = false;

			for (int i = 0; i < itemCount; i++)
			{
				if (_startPosition < _xPositionLeft)
				{
					if ((_start == -1 || _preStart == _start) && !startEnabled)
					{
						_start = i;
						startEnabled = true;
					}
				}

				if (i > _selectedIndex)
				{
					if (_endPosition < _xPositionRight)
					{
						_end = i;
						break;
					}

					_xPositionRight += _moffset;
				}

				_end = i;
				_xPositionLeft += _moffset;
			}
		}

		/// <summary>
		/// Updates the carousel items based on the calculated positions.
		/// </summary>
		void UpdateItems()
		{
			if (HasStartAndEndPositions() && HasNoPreviousPositions())
			{
				ClearTempDataSource();
				PopulateTempDataSource();
			}

			AddRefreshItems();
		}

		/// <summary>
		/// Checks if the start and end positions are valid.
		/// </summary>
		/// <returns><c>true</c> if start and end positions are valid; otherwise, <c>false</c>.</returns>
		bool HasStartAndEndPositions()
		{
			return _start != -1 && _end != -1;
		}

		/// <summary>
		/// Checks if there are no previous start and end positions.
		/// </summary>
		/// <returns><c>true</c> if there are no previous start and end positions; otherwise, <c>false</c>.</returns>
		bool HasNoPreviousPositions()
		{
			return _preStart == -1 && _preEnd == -1;
		}

		/// <summary>
		/// Clears the temporary data source.
		/// </summary>
		void ClearTempDataSource()
		{
			TempDataSource?.Clear();
		}

		/// <summary>
		/// Populates the temporary data source with items from the main data sources.
		/// </summary>
		void PopulateTempDataSource()
		{
			for (int i = _start; i <= _end; i++)
			{
				AddItemToTempDataSource(_tempItemsSource, i);
			}
		}

		/// <summary>
		/// Adds an item to the temporary data source if it exists.
		/// </summary>
		/// <param name="source">The data source to add items from.</param>
		/// <param name="index">The index of the item to add.</param>
		void AddItemToTempDataSource(IList<PlatformCarouselItem>? source, int index)
		{
			if (source != null && source.Count > 0 && index < source.Count)
			{
				TempDataSource?.Add(source[index]);
			}
		}

		/// <summary>
		/// Adds carousel items to the view.
		/// </summary>
		void AddCarouselItem()
		{
			if (TempDataSource == null || TempDataSource.Count == 0)
			{
				return;
			}

			PopulateTempList();
			AddItemsToView();
		}

		/// <summary>
		/// Populates the temporary list with carousel items.
		/// </summary>
		void PopulateTempList()
		{
			if (TempDataSource != null)
			{
				for (int count = 0; count < TempDataSource.Count; count++)
				{
					var carouselItem = TempDataSource[count];
					carouselItem.ParentItem = this;
					carouselItem.LayoutParameters = new LayoutParams(_itemWidth, _itemHeight);
					carouselItem.Index = count;
					CarouselItemAccessibility(carouselItem);
					PlatformCarousel.RemoveItemFromParent(carouselItem);

					if (count <= _selectedIndex)
					{
						AddView(carouselItem, count);
					}
					else
					{
						_tempList?.Add(carouselItem);
					}
				}
			}
		}

		/// <summary>
		/// Adds items from the temporary list to the view.
		/// </summary>
		void AddItemsToView()
		{
			if (_tempList == null)
			{
				return;
			}

			ReverseTempList();

			foreach (var item in _tempList)
			{
				PlatformCarousel.RemoveItemFromParent(item);
				AddView(item);
				BringToFrontIfNeeded(item);
			}

			_tempList.Clear();
		}

		/// <summary>
		/// Reverses the temporary list.
		/// </summary>
		void ReverseTempList()
		{
			if (_tempList != null)
			{
				var newList = new List<PlatformCarouselItem>(_tempList);
				_tempList.Clear();
				newList.Reverse();

				foreach (var item in newList)
				{
					_tempList.Add(item);
				}
			}
		}

		/// <summary>
		/// Brings the item to the front if needed based on the view mode.
		/// </summary>
		/// <param name="item">The item to bring to front.</param>
		void BringToFrontIfNeeded(PlatformCarouselItem item)
		{
			if (_viewMode != ViewMode.Default)
			{
				return;
			}

			if (ItemsSource != null && _tempItemsSource != null && _tempItemsSource.Count > 0)
			{
				BringChildToFront(_tempItemsSource[_selectedIndex]);
			}
		}

		/// <summary>
		/// Arranges carousel items linearly.
		/// </summary>
		void ItemsArrangeLinear()
		{
			RemoveAllViews();

			if (TempDataSource == null || TempDataSource.Count == 0)
			{
				return;
			}

			InitializeHorizontalGridView();
			ConfigureHorizontalGridView();
			AddGridViewToLinearLayout();

			AddView(_linearLayout);
		}

		/// <summary>
		/// Initializes the horizontal grid view.
		/// </summary>
		void InitializeHorizontalGridView()
		{
			if (Context != null)
			{
				_horizontalGridView = new RecyclerView(Context);
				_linearLayoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false);
				_horizontalGridView.SetLayoutManager(_linearLayoutManager);
			}
		}

		/// <summary>
		/// Configures the horizontal grid view by setting its adapter and item decoration.
		/// </summary>
		void ConfigureHorizontalGridView()
		{
			if (TempDataSource != null && Context != null)
			{
				var itemAdapter = new ItemAdapter(Context, this, _itemHeight, _itemWidth, TempDataSource);
				_horizontalGridView?.SetAdapter(itemAdapter);
			}

			var itemDecoration = new SpaceItemDecoration(ItemSpacing);
			_horizontalGridView?.AddItemDecoration(itemDecoration);

			_horizontalGridView?.ScrollToPosition(SelectedIndex);
		}

		/// <summary>
		/// Adds the horizontal grid view to a linear layout.
		/// </summary>
		void AddGridViewToLinearLayout()
		{
			_linearLayout = new LinearLayout(Context);
			_linearLayout.SetGravity(GravityFlags.CenterVertical);
			_linearLayout.AddView(_horizontalGridView);
		}

		/// <summary>
		/// Add the refresh items.
		/// </summary>
		void AddRefreshItems()
		{
			if (!_isSelected)
			{
				ArrangeItemsIfDefaultViewMode();
			}
			else if (_isSelected && _preEnd != -1 && _preStart != -1)
			{
				if (_preStart < _start)
				{
					RemoveItemsFromStart(_start - _preStart);
				}
				else
				{
					InsertItemsAtStart(_preStart - _start);
				}

				if (_preEnd > _end)
				{
					RemoveItemsFromEnd(_preEnd - _end);
				}
				else
				{
					InsertItemsAtEnd(_end - _preEnd);
				}

				ArrangeItemsIfDefaultViewMode();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="count"></param>
		void RemoveItemsFromStart(int count)
		{
			if (TempDataSource != null && TempDataSource.Count >= count)
			{
				for (int i = 0; i < count; i++)
				{
					TempDataSource.RemoveAt(0);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="count"></param>
		void InsertItemsAtStart(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CreateItemContent(i, _start + i);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="count"></param>
		void RemoveItemsFromEnd(int count)
		{
			if (TempDataSource != null && TempDataSource.Count >= count)
			{
				for (int i = 0; i < count; i++)
				{
					TempDataSource.RemoveAt(TempDataSource.Count - 1);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="count"></param>
		void InsertItemsAtEnd(int count)
		{
			if (TempDataSource == null)
			{
				return;
			}

			for (int i = 1; i <= count; i++)
			{
				CreateItemContent(TempDataSource.Count, _preEnd + i);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void ArrangeItemsIfDefaultViewMode()
		{
			if (_viewMode == ViewMode.Default)
			{
				ItemsArrange();
			}
		}

		/// <summary>
		/// Items the arrange.
		/// </summary>
		void ItemsArrange()
		{
			RemoveAllViews();
			_tempList?.Clear();
			if (TempDataSource != null && TempDataSource.Count > 0)
			{
				int count = _start;
				for (int datasourceindex = 0; datasourceindex < TempDataSource.Count; datasourceindex++)
				{
					PlatformCarouselItem PlatformCarouselItem = TempDataSource[datasourceindex] as PlatformCarouselItem;
					PlatformCarouselItem.ParentItem = this;
					PlatformCarouselItem.LayoutParameters = new LayoutParams(_itemWidth, _itemHeight);
					PlatformCarouselItem.Index = count;
					CarouselItemAccessibility(PlatformCarouselItem);
					PlatformCarousel.RemoveItemFromParent(PlatformCarouselItem);
					if (count <= _selectedIndex)
					{
						AddView(PlatformCarouselItem);
					}
					else
					{
						_tempList?.Add(PlatformCarouselItem);
					}

					count++;
				}
				if (_tempList != null)
				{
					var newList = new List<PlatformCarouselItem>(_tempList.Capacity);
					newList.AddRange(_tempList);
					_tempList.Clear();
					for (int i = newList.Count - 1; i >= 0; i--)
					{
						_tempList.Add(newList[i]);
					}

					foreach (PlatformCarouselItem reverseItem in _tempList)
					{
						PlatformCarousel.RemoveItemFromParent((PlatformCarouselItem)reverseItem);
						AddView(reverseItem);
						if (_viewMode == ViewMode.Default)
						{
							if (_itemsSource != null && _tempItemsSource != null)
							{
								BringChildToFront((PlatformCarouselItem)_tempItemsSource[_selectedIndex]);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates the content of the item.
		/// </summary>
		/// <param name="i">The index.</param>
		/// <param name="v">V value.</param>
		void CreateItemContent(int i, int v)
		{
			PlatformCarouselItem? PlatformCarouselItem = null;

			if (_tempItemsSource != null && _tempItemsSource.Count > 0)
			{
				PlatformCarouselItem = _tempItemsSource[v] as PlatformCarouselItem;
			}
			if (TempDataSource != null && PlatformCarouselItem != null)
			{
				TempDataSource.Insert(i, PlatformCarouselItem);
			}
		}

		/// <summary>
		/// To set PlatformCarouselItem accessibility properties
		/// </summary>
		/// <param name="item">carousel item</param>
		void CarouselItemAccessibility(PlatformCarouselItem item)
		{
			if (_itemsSource != null)
			{
				item.ContentDescription = item.AutomationId + " PlatformCarouselItem " + (item.Index + 1) + " of " + ((IList)_itemsSource).Count;
			}
		}


		/// <summary>
		/// To update the allow laod more
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateAllowLoadMore(ICarousel mauiView)
		{
			AllowLoadMore = mauiView.AllowLoadMore;
		}

		/// <summary>
		/// To update the enable virtualization
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateEnableVirtualization(ICarousel mauiView)
		{
			if (mauiView.EnableVirtualization && !mauiView.AllowLoadMore)
			{
				EnableVirtualization = true;
			}
			else
			{
				EnableVirtualization = false;
			}
		}

		/// <summary>
		/// To update the load more items count
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateLoadMoreItemsCount(ICarousel mauiView)
		{
			if (mauiView.AllowLoadMore)
			{
				LoadMoreItemsCount = mauiView.LoadMoreItemsCount;
			}
		}

		/// <summary>
		/// To update the selected index
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateSelectedIndex(ICarousel mauiView)
		{
			if (mauiView.ItemsSource != null)
			{
				if (mauiView.SelectedIndex >= mauiView.ItemsSource.Count() && mauiView.ItemsSource.Any())
				{
					SelectedIndex = mauiView.ItemsSource.Count() - 1;
				}
				else if (mauiView.SelectedIndex < 0)
				{
					SelectedIndex = 0;
				}
				else
				{
					SelectedIndex = mauiView.SelectedIndex;
				}
			}
		}

		/// <summary>
		/// To update the item template
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateItemTemplate(ICarousel mauiView)
		{
			Adapter = new CustomCarouselAdapter(mauiView);
		}

		/// <summary>
		/// To update the view mode
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateViewMode(ICarousel mauiView)
		{
			ViewMode = mauiView.ViewMode;
		}

		/// <summary>
		/// To update the item spacing
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateItemSpacing(ICarousel mauiView)
		{
			ItemSpacing = (int)(mauiView.ItemSpacing * GetDensity());
		}

		/// <summary>
		/// To update the rotation angle
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateRotationAngle(ICarousel mauiView)
		{
			RotationAngle = mauiView.RotationAngle;
		}

		/// <summary>
		/// To update the off set
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateOffset(ICarousel mauiView)
		{
			Offset = (int)(mauiView.Offset * GetDensity());
		}

		/// <summary>
		/// To update the scale offset
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateScaleOffset(ICarousel mauiView)
		{
			ScaleOffset = mauiView.ScaleOffset;
		}

		/// <summary>
		/// To update the selected item offset 
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateSelectedItemOffset(ICarousel mauiView)
		{
			SelectedItemOffset = (int)(mauiView.SelectedItemOffset * GetDensity());
		}

		/// <summary>
		/// To update the duration
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateDuration(ICarousel mauiView)
		{
			Duration = mauiView.Duration;
		}

		/// <summary>
		/// To update the item width
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateItemWidth(ICarousel mauiView)
		{
			ItemWidth = (int)(mauiView.ItemWidth * GetDensity());
		}

		/// <summary>
		/// To update the item height
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateItemHeight(ICarousel mauiView)
		{
			ItemHeight = (int)(mauiView.ItemHeight * GetDensity());
		}

		/// <summary>
		/// To update the enable interaction
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateEnableInteraction(ICarousel mauiView)
		{
			EnableInteraction = mauiView.EnableInteraction;
		}

		/// <summary>
		/// To update the swipe movement mode
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateSwipeMovementMode(ICarousel mauiView)
		{
			SwipeMovementMode = mauiView.SwipeMovementMode;
		}

		/// <summary>
		/// To update the swipe movement mode
		/// </summary>
		/// <param name="mauiView"></param>
		internal void UpdateIsEnabled(ICarousel mauiView)
		{
			IsEnabled = mauiView.IsEnabled;
		}

		#endregion

	}

	#region SpaceItemDecoration
	/// <summary>
	/// Provides consistent spacing between items in a RecyclerView.
	/// </summary>
	internal class SpaceItemDecoration : RecyclerView.ItemDecoration
	{
		/// <summary>
		/// The space size used for half of the item spacing.
		/// </summary>
		private readonly int _itemSpacing;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpaceItemDecoration"/> class.
		/// </summary>
		/// <param name="space">The total space to apply between items, divided equally on all sides.</param>
		public SpaceItemDecoration(int space)
		{
			_itemSpacing = space / 2;
		}

		/// <summary>
		/// Sets the spacing between items in the RecyclerView.
		/// </summary>
		/// <exclude/>
		/// <param name="outRect">The Rect of offsets for the item.</param>
		/// <param name="view">The child view to decorate.</param>
		/// <param name="parent">The RecyclerView containing the view.</param>
		/// <param name="state">The current RecyclerView.State of the RecyclerView.</param>
		public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
		{
			outRect.Left = _itemSpacing;
			outRect.Right = _itemSpacing;
			outRect.Bottom = _itemSpacing;
			outRect.Top = _itemSpacing;
		}

	}
	#endregion
}
