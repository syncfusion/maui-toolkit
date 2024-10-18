using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views.Animations;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;
using View = Android.Views.View;
using SwipeStartedEventArgs = Syncfusion.Maui.Toolkit.Carousel.SwipeStartedEventArgs;
using IList = System.Collections.IList;
using Microsoft.Maui.Platform;
using Microsoft.Maui;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Carousel.Platform;

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
        internal int CountItem = 0;

        /// <summary>
        /// The temp data source.
        /// </summary>
        internal List<PlatformCarouselItem>? TempDataSource { get; set; } = new List<PlatformCarouselItem>();

        /// <summary>
        /// The index of the is last.
        /// </summary>
        internal bool IsLastIndex = false, isImageClicked;

        /// <summary>
        /// The temp data source value.
        /// </summary>
        internal List<PlatformCarouselItem>? TempDataSourceValue;

        /// <summary>
        /// The allow load more data source.
        /// </summary>
        internal IList<PlatformCarouselItem>? AllowLoadMoreDataSource { get; set; } = new List<PlatformCarouselItem>();

        /// <summary>
        /// Items Source Collection
        /// </summary>
        internal List<PlatformCarouselItem>? ItemsSourceCollection { get; set; } = new List<PlatformCarouselItem>();

        /// <summary>
        /// The temp items source.
        /// </summary>
        internal List<PlatformCarouselItem>? TempItemsSource;

        /// <summary>
        /// The item spacing.
        /// </summary>
        private int itemSpacing = 5;

        /// <summary>
        /// The item spacing.
        /// </summary>
        private int minDiffernce = 7;

        /// <summary>
        /// The enableInteraction
        /// </summary>
        private bool enableInteraction = true;

        /// <summary>
        /// The isEnabled
        /// </summary>
        private bool isEnabled = true;

        /// <summary>
        /// The adapter.
        /// </summary>
        private ICarouselAdapter? adapter;

        /// <summary>
        /// The width of the control.
        /// </summary>
        private int controlWidth;

        /// <summary>
        /// The isSwipeRestricted.
        /// </summary>
        private bool isSwipeRestricted;

        /// <summary>
        /// The swipeEventCalled.
        /// </summary>
        private bool swipeEventCalled;


        /// <summary>
        /// The height of the control.
        /// </summary>
        private int controlHeight;

        /// <summary>
        /// The index of the selected.
        /// </summary>
        private int selectedIndex = 0;

        /// <summary>
        /// The m offset.
        /// </summary>
        private int moffset = 60;

        /// <summary>
        /// The height of the item.
        /// </summary>
        private int itemHeight = 500;

        /// <summary>
        /// AutomationId field.
        /// </summary>
        private string automationId = string.Empty;

        /// <summary>
        /// The width of the item.
        /// </summary>
        private int itemWidth = 300;

        /// <summary>
        /// The view mode.
        /// </summary>
        private ViewMode viewMode = ViewMode.Default;

        /// <summary>
        /// The rotation angle.
        /// </summary>
        private int rotationAngle = 45;

        /// <summary>
        /// The scale offset.
        /// </summary>
        private float scaleOffset = 0.7f;

        /// <summary>
        /// The selected item offset.
        /// </summary>
        private int selectedItemOffset = -37;

        /// <summary>
        /// The duration.
        /// </summary>
        private int duration = 600;

        /// <summary>
        /// The Scroll Mode
        /// </summary>
        private SwipeMovementMode swipeMovementMode = SwipeMovementMode.MultipleItems;

        /// <summary>
        /// The can execute first field
        /// </summary>
        private bool canExecuteFirst = false;

        /// <summary>
        /// The can execute swipe field
        /// </summary>
        private bool canExecuteSwipe = true;

        /// <summary>
        /// The horizontal grid view.
        /// </summary>
        private RecyclerView? horizontalGridView;

        /// <summary>
        /// The index of the previous.
        /// </summary>
        private int prevIndex;

        /// <summary>
        /// The previous offset.
        /// </summary>
        private int prevOffset;

        /// <summary>
        /// The previous angle.
        /// </summary>
        private int prevAngle;

        /// <summary>
        /// The previous offset.
        /// </summary>
        private float prevZOffset;

        /// <summary>
        /// The previous selected offset.
        /// </summary>
        private int prevSelectedOffset;

        /// <summary>
        /// The temp list.
        /// </summary>
        private List<PlatformCarouselItem>? tempList = new List<PlatformCarouselItem>();

        /// <summary>
        /// The greater than list.
        /// </summary>
        private List<PlatformCarouselItem>? nextItems = new List<PlatformCarouselItem>();

        /// <summary>
        /// The lesser than list.
        /// </summary>
        private List<PlatformCarouselItem>? previousItems = new List<PlatformCarouselItem>();

        /// <summary>
        /// The y position.
        /// </summary>
        private int yPos;

        /// <summary>
        /// The X position.
        /// </summary>
        private int xPos;

        /// <summary>
        /// The x.
        /// </summary>
        private int xvalue = 0, dX = 0, id, count, dY = 0, yvalue = 0;

        /// <summary>
        /// The is moving.
        /// </summary>
        private bool isMoving, isDown;

        /// <summary>
        /// The allow load more.
        /// </summary>
        private bool allowLoadMore = false;

        /// <summary>
        /// The is busy.
        /// </summary>
        private bool isBusy = false;

        /// <summary>
        /// The maximum indicator.
        /// </summary>
        private int maximumIndicator = 0;

        /// <summary>
        /// The is enabling load more button.
        /// </summary>
        private bool isEnablingLoadMoreButton = false;

        /// <summary>
        /// The loaded custom view.
        /// </summary>
        private View? loadedCustomView = null;

        /// <summary>
        /// The items source.
        /// </summary>
        private IEnumerable? itemsSource;

        /// <summary>
        /// The is virtualization.
        /// </summary>
        private bool isVirtualization = false;

        /// <summary>
        /// The is selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// The center position.
        /// </summary>
        private int centerPosition = 0, startPosition = 0, endPosition = 0, xPositionLeft = 0, xPositionRight = 0;

        /// <summary>
        /// The start.
        /// </summary>
        private int start = -1, end = -1, preStart = -1, preEnd = -1;

        /// <summary>
        /// The is sized changed.
        /// </summary>
        private bool isSizeChanged = false;

        /// <summary>
        /// The local resource.
        /// </summary>
        private ResourceManager? localresource;

        /// <summary>
        /// The linear layout manager.
        /// </summary>
        private LinearLayoutManager? linearLayoutManager;

        /// <summary>
        /// The linear layout.
        /// </summary>
        private LinearLayout? linearLayout;

        private bool isSwipeHappened = false;

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
#pragma warning disable CA1422
#pragma warning disable CS0618 
            AnimationCacheEnabled = true;
            DrawingCacheEnabled = true;
#pragma warning restore CS0618
#pragma warning restore CA1422
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
                return isVirtualization;
            }

            set
            {
                isVirtualization = value;
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
                return itemsSource ?? Enumerable.Empty<object>();
            }

            set
            {
                if (value != null)
                {
                    itemsSource = value;
                    OnItemsSourceChanged();
                }
            }
        }

        void OnItemsSourceChanged()
        {
            if (ItemsSource is INotifyCollectionChanged source)
            {
                // The collection changed triggered in MAUI, so we commented here
               // source.CollectionChanged -= Handle_CollectionChanged;
               // source.CollectionChanged += Handle_CollectionChanged;
            }

            TempDataSource = new List<PlatformCarouselItem>();
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
                return itemSpacing;
            }

            set
            {
                itemSpacing = value;
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
                return enableInteraction;
            }

            set
            {
                enableInteraction = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the Scroll Mode of <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/>
        /// </summary>
        public SwipeMovementMode SwipeMovementMode
        {
            get
            {
                return swipeMovementMode;
            }

            set
            {
                swipeMovementMode = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Com.Syncfusion.Carousel.PlatformCarousel"/> IsEnabled or not
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
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
                return adapter;
            }

            set
            {
                adapter = value;
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
                return selectedIndex;
            }

            set
            {
                selectedIndex = value;
                OnSelectedIndexChanged(value);
            }
        }

        void OnSelectedIndexChanged(int value)
        {
			var isSelectedIndexChanged = false;
            if (virtualView != null && virtualView.SelectedIndex != value)
            {
				isSelectedIndexChanged = true;
                virtualView.SelectedIndex = value;
            }

            if (ChildCount > 0)
            {
                if (TempItemsSource != null && IsWithinBounds(selectedIndex, TempDataSource, TempItemsSource.Count))
                {
                    Selection(isSelectedIndexChanged);
                }
                else
                {
                    SelectedIndex = prevIndex;
                }
            }
        }

        bool IsWithinBounds(int index, List<PlatformCarouselItem>? tempDataSource, int sourceCount)
        {
            return  (index >= 0) && ((tempDataSource != null) && (index <= tempDataSource.Count - 1) || (index <= sourceCount - 1));
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public int Offset
        {
            get
            {
                return moffset;
            }

            set
            {
                moffset = value;
                OnOffsetChanged();
            }
        }

        void OnOffsetChanged()
        {
            if (viewMode == ViewMode.Default)
            {
                if (prevOffset != moffset)
                {
                    prevOffset = moffset;
                    if (TempDataSource != null && TempDataSource.Count > selectedIndex)
                    {
                        Arrange(TempDataSource[selectedIndex], true);
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
                return itemHeight;
            }

            set
            {
                itemHeight = value;
                OnItemHeightChanged();
            }
        }

        void OnItemHeightChanged()
        {
            if (itemHeight <= 0 && viewMode == ViewMode.Linear)
            {
                itemHeight = 5;
            }

            if (TempDataSource != null)
            {
                Refresh();
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
                return itemWidth;
            }

            set
            {
                itemWidth = value;
                OnItemWidthChanged();
            }
        }

        void OnItemWidthChanged()
        {
            if (itemWidth <= 0 && viewMode == ViewMode.Linear)
            {
                itemWidth = 5;
            }

            if (TempDataSource != null)
            {
                Refresh();
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
                return viewMode;
            }

            set
            {
                viewMode = value;
                RemoveAllViews();
                OnViewModeChanged();
                if (viewMode == ViewMode.Default)
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
                return rotationAngle;
            }

            set
            {
                rotationAngle = value;
                OnRotationAngleChanged();
            }
        }

        void OnRotationAngleChanged()
        {
            if (viewMode == ViewMode.Default && rotationAngle != prevAngle)
            {
                prevAngle = rotationAngle;
                if (TempDataSource != null && TempDataSource.Count > selectedIndex)
                {
                    Arrange(TempDataSource[selectedIndex], true);
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
                return selectedItemOffset;
            }

            set
            {
                selectedItemOffset = value;
                OnSelectedItemOffsetChanged();
            }
        }

        void OnSelectedItemOffsetChanged()
        {
            if (viewMode == ViewMode.Default && selectedItemOffset != prevSelectedOffset)
            {
                prevSelectedOffset = selectedItemOffset;
                if (TempDataSource != null && TempDataSource.Count > selectedIndex)
                {
                    Arrange(TempDataSource[selectedIndex], true);
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
                return duration;
            }

            set
            {
                duration = value;
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
                return scaleOffset;
            }

            set
            {
                scaleOffset = value;
                OnScalOffsetChanged();
            }
        }

        void OnScalOffsetChanged()
        {
            if (viewMode == ViewMode.Default && scaleOffset != prevZOffset)
            {
                prevZOffset = scaleOffset;
                if (TempDataSource != null && TempDataSource.Count > selectedIndex)
                {
                    Arrange(TempDataSource[selectedIndex], true);
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
                return maximumIndicator;
            }

            set
            {
                if (maximumIndicator != value)
                {
                    maximumIndicator = value;
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
                return allowLoadMore;
            }

            set
            {
                allowLoadMore = value;
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
                return loadedCustomView;
            }

            set
            {
                loadedCustomView = value;
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
                return automationId;
            }

            set
            {
                if (automationId != value)
                {
                    automationId = value;
                    if (automationId == null)
                    {
                        automationId = string.Empty;
                    }
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
                return isBusy;
            }

            set
            {
                isBusy = value;
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
                return controlHeight;
            }

            set
            {
                controlHeight = value;
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
                return controlWidth;
            }

            set
            {
                controlWidth = value;
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
                return localresource;
            }

            set
            {
                localresource = value;
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
                return horizontalGridView;
            }

            set
            {
                horizontalGridView = value;
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
        private SwipeStartedEventArgs? swipeStartedEventArgs;

        #endregion

        #region Public Methods

        /// <summary>
        /// Load More method.
        /// </summary>
        public void LoadMore()
        {
            if (TempDataSource == null) return;

            if (horizontalGridView != null)
            {
                var itemAdapter = (horizontalGridView.GetAdapter() as ItemAdapter);
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
            if (view == null) return;

            PlatformCarouselItem? item = view as PlatformCarouselItem ?? view.Parent as PlatformCarouselItem;

            if (item == null || item.IsItemSelected) return;

            if (item.Index <= CountItem - 1)
            {
                isSelected = true;
                SelectedIndex = item.Index;
                isSelected = false;
                canExecuteSwipe = false;
            }

            if (item.Index == CountItem && LoadMoreItemsCount > 0)
            {
                LoadMoreDefaultMode(item.Index);
            }
        }

        /// <summary>
        /// Refresh the carousel.
        /// </summary>
        public void RefreshCarousel()
        {
            if (viewMode != ViewMode.Default) return;

            PlatformCarouselItem? itemToArrange = null;

            if (!isVirtualization)
            {
                if (TempDataSource != null && TempDataSource.Count > selectedIndex)
                {
                    itemToArrange = TempDataSource[selectedIndex];
                }
            }
            else
            {
                if (TempItemsSource != null && TempItemsSource.Count > selectedIndex)
                {
                    itemToArrange = TempItemsSource[selectedIndex];
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
            if ((selectedIndex < CountItem - 1 && flowFactor)
                || (SelectedIndex > 0 && !flowFactor))
            {
                isSelected = true;
                SelectedIndex = selectedIndex + (flowFactor ? 1 : -1);
                isSelected = false;
            }
        }

        /// <summary>
        /// Moves the previous.
        /// </summary>
        public void MovePrevious()
        {
            bool flowFactor = LayoutDirection == Android.Views.LayoutDirection.Ltr;

            if (selectedIndex > CountItem - 1) return;

            isSelected = true;

            if ((selectedIndex > 0 && flowFactor) ||
                (SelectedIndex < CountItem - 1 && !flowFactor))
            {
                SelectedIndex = selectedIndex - (flowFactor ? 1 : -1);
            }
            else
            {
                SelectedIndex = 0;
            }

            isSelected = false;
        }

		/// <summary>
		/// On the intercept touch event.
		/// </summary>
		/// <exclude/>
		/// <returns><c>true</c>, if intercept touch event was enabled, <c>false</c> otherwise.</returns>
		/// <param name="ev">Event values.</param>
		public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (ev == null) return false;

            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    HandleActionDown(ev);
                    break;

                case MotionEventActions.Move:
                    HandleActionMove(ev);
                    break;
            }

            return EnableInteraction && IsEnabled
                ? isMoving || base.OnInterceptTouchEvent(ev)
                : true;
        }

        /// <summary>
        /// Handles the Action Down event.
        /// </summary>
        /// <param name="ev">The MotionEvent.</param>
        void HandleActionDown(MotionEvent ev)
        {
            xvalue = (int)ev.RawX;
            isMoving = false;
            isDown = true;
            isSwipeRestricted = true;
            swipeEventCalled = false;
        }

        /// <summary>
        /// Handles the Action Move event.
        /// </summary>
        /// <param name="ev">The MotionEvent.</param>
        void HandleActionMove(MotionEvent ev)
        {
            dX = (int)ev.RawX;
            dY = (int)ev.RawY;
            int diff = dX - xvalue;
            int diffY = dY - yvalue;

            swipeStartedEventArgs ??= new SwipeStartedEventArgs();

            bool isVerticalSwipe = Math.Abs(diffY) > Math.Abs(diff);
            Parent?.RequestDisallowInterceptTouchEvent(!isVerticalSwipe);

            yvalue = dY;

            if (diff > GetDimen(minDiffernce))
            {
                if (viewMode == ViewMode.Linear && !swipeEventCalled)
                {
                    TriggerSwipeStarted(true);
                }
            }
            else if (diff < -GetDimen(minDiffernce))
            {
                if (viewMode == ViewMode.Linear && !swipeEventCalled)
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
                if (isDown)
                {
                    isMoving = true;
                    isDown = false;
                }

                if (diff > GetDimen(85))
                {
                    xvalue = dX;
                    dX = 0;
                    isMoving = true;
                }
            }
            else if (diff < -GetDimen(45))
            {
                if (isDown)
                {
                    isMoving = true;
                    isDown = false;
                }

                if (diff < -GetDimen(85))
                {
                    xvalue = dX;
                    dX = 0;
                    isMoving = true;
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
            if (e == null || TempDataSource == null) return false;

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
            xvalue = (int)e.RawX;
            yvalue = (int)e.RawY;
            count = selectedIndex;
            isSwipeRestricted = true;
        }

        /// <summary>
        /// Handles the touch event Action Move.
        /// </summary>
        /// <param name="e">The MotionEvent.</param>
        void HandleTouchActionMove(MotionEvent e)
        {
            dX = (int)e.GetX();
            int diff = dX - xvalue;
            swipeStartedEventArgs ??= new SwipeStartedEventArgs();

            if (diff > GetDimen(id) && SwipeMovementMode == SwipeMovementMode.MultipleItems)
            {
                HandleSwipeNext();
            }
            else if (diff > GetDimen(15) && SwipeMovementMode == SwipeMovementMode.SingleItem)
            {
                HandleSingleItemSwipeNext();
            }
            else if (diff < -GetDimen(id) && SwipeMovementMode == SwipeMovementMode.MultipleItems)
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

            if (viewMode == ViewMode.Default)
            {
                ArrangeSelectedIndexItems();
            }

            if (!isSwipeRestricted)
            {
                OnSwipeEnded(EventArgs.Empty);
                isSwipeHappened = true;
            }

            isSwipeRestricted = false;
            swipeEventCalled = false;
        }

        /// <summary>
        /// Handles swipe next action when SwipeMovementMode is MultipleItems.
        /// </summary>
        void HandleSwipeNext()
        {
            isMoving = true;

            if (viewMode == ViewMode.Default && EnableInteraction && IsEnabled && canExecuteFirst)
            {
                if (!swipeEventCalled && SelectedIndex > 0)
                {
                    TriggerSwipeStarted(true);
                }
                else
                {
                    isSwipeRestricted = true;
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
            isMoving = true;

            if (viewMode == ViewMode.Default && EnableInteraction && IsEnabled)
            {
                if (!swipeEventCalled && selectedIndex < CountItem - 1)
                {
                    TriggerSwipeStarted(false);
                }
                else
                {
                    isSwipeRestricted = true;
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
            isMoving = true;

            if (viewMode == ViewMode.Default && EnableInteraction && IsEnabled && canExecuteFirst)
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
            isMoving = true;

            if (viewMode == ViewMode.Default && EnableInteraction && IsEnabled)
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
            count--;
            xvalue = dX;
            yvalue = dY;
            dX = 0;
            canExecuteFirst = true;
        }

        /// <summary>
        /// Adjusts for large swipe movements.
        /// </summary>
        /// <param name="diff">The difference in the X position between the current and initial touch points.</param>
        void AdjustForLargeSwipe(int diff)
        {
            if (isMoving && (Math.Abs(dX + xvalue) > GetDimen(125)))
            {
                id = 50;
                isMoving = true;
            }
        }

        /// <summary>
        /// Resets touch-related variables to their default states.
        /// </summary>
        void ResetTouchVariables()
        {
            canExecuteFirst = false;
            canExecuteSwipe = true;

            isMoving = false;
            isDown = false;
            id = 85;
            xvalue = 0;
            dX = 0;
            yvalue = 0;
            dY = 0;
        }

        /// <summary>
        /// Arranges items based on the selected index.
        /// </summary>
        void ArrangeSelectedIndexItems()
        {
            if (!isVirtualization)
            {
                var selectedItem = TempDataSource?[selectedIndex];
                if (selectedItem != null)
                {
                    Arrange(selectedItem, true);
                }
            }
            else
            {
                if (itemsSource != null && TempItemsSource != null && TempItemsSource.Count > selectedIndex)
                {
                    Arrange(TempItemsSource[selectedIndex], true);
                }
            }
        }

        /// <summary>
        /// To trigger the swipe started event
        /// </summary>
        /// <param name="isSwipedLeft"></param>
        void TriggerSwipeStarted(bool isSwipedLeft)
        {
            isSwipeRestricted = false;
            if (swipeStartedEventArgs != null)
            {
                swipeStartedEventArgs.IsSwipedLeft = isSwipedLeft;
                OnSwipeStarted(swipeStartedEventArgs);
            }
            swipeEventCalled = true;
        }

        /// <summary>
        /// To swipe the next item
        /// </summary>
        void SwipeNext()
        {
            if (SwipeMovementMode == SwipeMovementMode.SingleItem && canExecuteSwipe)
            {
                if (!swipeEventCalled && selectedIndex < CountItem - 1)
                {
                    TriggerSwipeStarted(false);
                }
                else
                {
                    isSwipeRestricted = true;
                }

                MoveNext();
                canExecuteSwipe = false;
            }
        }

        /// <summary>
        /// To swipe the previous item
        /// </summary>
        void SwipePrevious()
        {
            if (SwipeMovementMode == SwipeMovementMode.SingleItem && canExecuteSwipe)
            {
                if (!swipeEventCalled && SelectedIndex > 0)
                {
                    TriggerSwipeStarted(true);
                }
                else
                {
                    isSwipeRestricted = true;
                }

                MovePrevious();
                canExecuteSwipe = false;
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
            virtualView?.RaiseSelectionChanged(args);
        }

        /// <summary>
        /// On SwipeStarted.
        /// </summary>
        /// <param name="args">Arguments of SwipeStarted event.</param>
        internal void OnSwipeStarted(SwipeStartedEventArgs args)
        {
            virtualView?.RaiseSwipeStarted(args);
        }

        /// <summary>
        /// On SwipeEnded.
        /// </summary>
        /// <param name="args">Arguments of SwipeEnded event.</param>
        internal virtual void OnSwipeEnded(EventArgs args)
        {
            virtualView?.RaiseSwipeEnded(args);
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
                Items = isVirtualization ? TempItemsSource : TempDataSource,
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
            tempList?.Clear();

            bool hasItems = (TempDataSource != null && TempDataSource.Count > 0) ||
                            (isVirtualization && ItemsSource != null && TempItemsSource != null && TempItemsSource.Count > 0);

            if (hasItems)
            {
                RemoveAllViews();

                // Handle the default view mode
                if (viewMode == ViewMode.Default)
                {
                    if (isSizeChanged)
                    {
                        if (!isVirtualization)
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
            if (!isVirtualization)
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

            if (viewMode != ViewMode.Default) return;

            if (isVirtualization)
            {
                Refresh();
            }

            if (changed)
            {
                if (!isVirtualization)
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
            if (TempDataSource != null && TempDataSource.Count > selectedIndex)
            {
                var instance = TempDataSource[selectedIndex];
                Arrange(instance, false);
            }
        }

        /// <summary>
        /// Arranges the selected item from the temporary items source.
        /// </summary>
        void ArrangeSelectedItemFromTempItemsSource()
        {
            if (TempItemsSource != null && TempItemsSource.Count > selectedIndex)
            {
                var instance = TempItemsSource[selectedIndex];
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

            if (viewMode == ViewMode.Default)
            {
                isSizeChanged = true;

                if (!isVirtualization)
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
            if (itemsSource != null && w >= 20 && h >= 20)
            {
                SizedChangedRefresh(w, h);
                Refresh();
            }

            if (itemsSource != null && w >= 20 && h >= 20)
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
            TempDataSourceValue = null;
            AllowLoadMoreDataSource = null;
            itemsSource = null;
            TempItemsSource = null;
            tempList = null;
            ItemsSourceCollection = null;
            adapter = null;
            loadedCustomView = null;
            previousItems = null;
            nextItems = null;

            DisposeAndClear(ref linearLayoutManager);
            DisposeAndClear(ref linearLayout);
        }

        /// <summary>
        /// Disposes and clears a disposable resource.
        /// </summary>
        /// <param name="disposable">The disposable resource reference.</param>
        void DisposeAndClear<T>(ref T? disposable) where T : class, IDisposable
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
                    PlatformCarouselItem selection = (PlatformCarouselItem)TempDataSource[selectedIndex];
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
                var itemsToAdd = AllowLoadMoreDataSource.Count >= maximumIndicator
                                 ? maximumIndicator
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
            if(Context != null)
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
                CountItem = ItemsSourceCollection.Count;
            }
        }

        /// <summary>
        /// Get the default load more view
        /// </summary>
        /// <returns></returns>
        Android.Views.View GetDefaultLoadMoreView()
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
            if (TempDataSource == null || AllowLoadMoreDataSource == null) return;

            if (index < 0 || index >= AllowLoadMoreDataSource.Count) return;  // Ensure index is within bounds

            var item = AllowLoadMoreDataSource[index];
            TempDataSource.Add(item);
            if(TempDataSourceValue != null)
            {
                TempDataSourceValue.Add(item);
                ItemsSourceCollection = TempDataSourceValue;
            }
            AllowLoadMoreDataSource.RemoveAt(index);
        }

        /// <summary>
        /// Handles the selection logic for the carousel.
        /// </summary>
        void Selection(bool canStopEvent)
        {
            if (viewMode == ViewMode.Default)
            {
                DeselectCurrentItem();
                OnIndexChanged(selectedIndex);
            }
            else
            {
                Refresh();
            }

            UpdateSelectionChangedEventArgs();
			// Preventing the event from being triggered multiple times
			if (!canStopEvent)
			{
				RefreshSelection(selectedIndex);
			}
            prevIndex = selectedIndex;
        }

        /// <summary>
        /// Deselects the currently selected item.
        /// </summary>
        void DeselectCurrentItem()
        {
            if (!isVirtualization)
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
            if (TempDataSource != null && TempDataSource.Count > 0 && TempDataSource.Count > selectedIndex)
            {
                TempDataSource[selectedIndex].IsItemSelected = false;
            }
        }

        /// <summary>
        /// Deselects the current item from the virtualized data sources.
        /// </summary>
        void DeselectVirtualizedItems()
        {
            if (TempItemsSource != null && TempItemsSource.Count > selectedIndex)
            {
                TempItemsSource[selectedIndex].IsItemSelected = false;
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
            if (sender == null || TempDataSource == null) return;

            var items = ItemsSource as IList;
            bool hasItemsChanged = items != null && items.Count != TempDataSource.Count;

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
                CountItem = TempDataSource.Count;
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
                TempItemsSource?.Add(carouselItem);
            }
        }

        /// <summary>
        /// Removes an item from the collection at the specified index.
        /// </summary>
        /// <param name="index">The index at which to remove the item.</param>
        void RemoveItem(int index)
        {
            if (TempDataSource == null) return;

            if (TempDataSource.Count > 0)
            {
                TempDataSource.RemoveAt(index + 1);
            }

            TempItemsSource?.RemoveAt(index + 1);
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
                CountItem = ItemsSourceCollection.Count;
            OnIndexChanged(selectedIndex);
        }

        /// <summary>
        /// Adds items to the temporary data source if not in virtualization mode.
        /// </summary>
        void AddItemsToTempDataSource()
        {
            if (!isVirtualization && TempDataSource != null && ItemsSourceCollection != null && ItemsSourceCollection.Count > 0)
            {
                TempDataSource.AddRange(ItemsSourceCollection);
            }
        }

        /// <summary>
        /// Updates the selected index to ensure it's within the valid range.
        /// </summary>
        void UpdateSelectedIndex()
        {
            if (ItemsSourceCollection != null && ItemsSourceCollection.Count > 0 && selectedIndex >= ItemsSourceCollection.Count)
            {
                SelectedIndex = ItemsSourceCollection.Count - 1;
            }
        }


        /// <summary>
        /// Populates the items source with carousel items.
        /// </summary>
        void PopulateItemsSource()
        {
            TempItemsSource = new List<PlatformCarouselItem>();
            var dummyCollection = new List<PlatformCarouselItem>();
            var items = ItemsSource as IList;

            if (items == null) return;

            for (int count = 0; count < items.Count; count++)
            {
                var carouselItem = CreateCarouselItem(count);
                if(carouselItem != null)
                {
                    TempItemsSource.Add(carouselItem);
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
            if(Context != null)
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
                TempDataSourceValue = new List<PlatformCarouselItem>();
                if (TempItemsSource != null && TempItemsSource.Count > maximumIndicator)
                {
                    for (int i = 0; i < maximumIndicator; i++)
                    {
                        AllowLoadMoreDataSource = dummyCollection;
                        TempDataSourceValue.Add(AllowLoadMoreDataSource[0]);
                        AllowLoadMoreDataSource.RemoveAt(0);
                    }

                    ItemsSourceCollection = TempDataSourceValue;
                    isEnablingLoadMoreButton = true;
                }
                else
                {
                    ItemsSourceCollection = dummyCollection;
                    if (TempItemsSource != null)
                        isEnablingLoadMoreButton = TempItemsSource.Count == maximumIndicator || TempItemsSource.Count < maximumIndicator;
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
            isSizeChanged = true;
            OnDataChanged();

            if (allowLoadMore && isEnablingLoadMoreButton && Context != null && TempDataSource != null && TempDataSource.Count > 0)
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
        /// <param name="selectedIndexArgs">Selected index.</param>
        void RefreshSelection(int selectedIndexArgs)
        {
            if (SelectionChangedEventArgs != null)
            {
                SelectionChangedEventArgs.OldItem = GetMauiItem(prevIndex);
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
            if (virtualView?.ItemsSource == null) return null;

            int index = 0;
            foreach (var item in virtualView.ItemsSource)
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

            if (viewMode == ViewMode.Default)
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
            if (!isVirtualization)
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
            if (TempDataSource != null && TempDataSource.Count > 0 && selectedIndex >= 0 && TempDataSource.Count > selectedIndex)
            {
                Arrange(TempDataSource[selectedIndex], false);
            }
        }

        /// <summary>
        /// Arranges virtualized items.
        /// </summary>
        void ArrangeVirtualizedItems()
        {
            if (selectedIndex < 0) return;

            if (itemsSource != null && TempItemsSource != null && TempItemsSource.Count > 0 && TempItemsSource.Count > selectedIndex)
            {
                Arrange(TempItemsSource[selectedIndex], false);
            }
        }

        /// <summary>
        /// Initializes temporary data sources based on the provided value.
        /// </summary>
        /// <param name="value">The new data source value.</param>
        void InitializeTempDataSources(IList<PlatformCarouselItem> value)
        {
            TempDataSource = new List<PlatformCarouselItem>();
            TempDataSourceValue = new List<PlatformCarouselItem>();
        }

        /// <summary>
        /// Sets the enabling load more button flag based on the provided value.
        /// </summary>
        /// <param name="value">The new data source value.</param>
        void SetEnablingLoadMoreButton(IList<PlatformCarouselItem> value)
        {
            if (LoadMoreItemsCount > 0 && value.Count > maximumIndicator)
            {
                AllowLoadMoreDataSource = value;
                for (int i = 0; i < maximumIndicator; i++)
                {
                    TempDataSourceValue?.Add(AllowLoadMoreDataSource[0]);
                    AllowLoadMoreDataSource.RemoveAt(0);  // Removed first element to avoid index issues
                }

                isEnablingLoadMoreButton = true;
            }
            else
            {
                isEnablingLoadMoreButton = false;
            }
        }

        /// <summary>
        /// Invokes the load more functionality.
        /// </summary>
        void AllowLoadMoreInvoke()
        {
            if (TempDataSource == null) return;

            if (itemsSource != null)
            {
                HandleLoadMoreForItemsSource();
            }
        }

        /// <summary>
        /// Handles load more functionality for itemsSource.
        /// </summary>
        void HandleLoadMoreForItemsSource()
        {
            if (ItemsSourceCollection == null || TempDataSource == null) return;

            RemoveAllViews();

            if (ItemsSourceCollection.Count < TempDataSource.Count)
            {
                TempDataSource.RemoveAt(ItemsSourceCollection.Count);
            }

            if (ItemsSourceCollection.Count == TempDataSource.Count)
            {
                CountItem = ItemsSourceCollection.Count;
                Refresh();
                if (allowLoadMore)
                    AddLoadMoreItem();
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
            if(Resources != null)
                return TypedValue.ApplyDimension(ComplexUnitType.Dip, value, Resources.DisplayMetrics);
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
                if (!isSelected || isVirtualization)
                {
                    Refresh();
                }

                if (!isVirtualization)
                {
                    ChangeSelectedItem(TempDataSource, selectedIndex);
                }
                else
                {
                    ChangeSelectedItem(TempItemsSource, selectedIndex);
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
                if (!isVirtualization)
                {
                    OnSelectedItemChanged(TempDataSource[selectedIndex]);
                }
                else
                {
                    if (isSizeChanged)
                    {
                        if (itemsSource != null && TempItemsSource != null)
                        {
                            OnSelectedItemChanged(TempItemsSource[selectedIndex]);
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
            if (viewMode == ViewMode.Default)
            {
                PlatformCarouselItem instance = (PlatformCarouselItem)selectedItemArg;
                Arrange(instance, true);
            }
        }

        /// <summary>
        /// Removes the item from parent.
        /// </summary>
        /// <param name="item">carousel Item.</param>
        void RemoveItemFromParent(PlatformCarouselItem item)
        {
            if(item.Parent != null)
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
            if (!isVirtualization)
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
            if(TempDataSource != null)
            {
                foreach (View temp in TempDataSource)
                {
                    var newItem = (PlatformCarouselItem)temp;
                    if (newItem != instance)
                    {
                        newItem.IsItemSelected = false;
                        ArrangeItems(newItem, rotationAngle, scaleOffset, animate, newItem.Index);
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
            if (start == -1 || end == -1 || TempDataSource == null) return;

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
            for (int i = start; i <= end; i++)
            {
                var item = source[count];
                if (i != selectedIndex)
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
            int endItemIndex = end - (start - preStart);
            if (!isSelected || index <= endItemIndex)
            {
                ArrangeItems(item, rotationAngle, scaleOffset, animate, index);
            }
            else
            {
                ArrangeItems(item, rotationAngle, scaleOffset, false, index);
            }
        }

        /// <summary>
        /// Arranges the items' Z positions depending on their selection state.
        /// </summary>
        void ArrangeItemsPosition()
        {
            if (!isSelected) return;

            BringItemsToFront(nextItems, true);
            BringItemsToFront(previousItems, false);

            if (viewMode == ViewMode.Default)
            {
                BringSelectedItemToFront();
            }

            if (isSwipeHappened && viewMode == ViewMode.Default)
            {
                isSwipeHappened = false;
                Refresh();
            }
        }

        /// <summary>
        /// Brings the items in the list to the front.
        /// </summary>
        /// <param name="items">The list of items.</param>
        /// <param name="reverse">Whether to reverse the order when bringing to front.</param>
        void BringItemsToFront(List<PlatformCarouselItem>? items, bool reverse)
        {
            if (items == null) return;

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
            if (ItemsSource != null && TempItemsSource != null && TempItemsSource.Count > 0)
            {
                BringChildToFront((PlatformCarouselItem)TempItemsSource[selectedIndex]);
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
            int position = index - selectedIndex;

            SetVerticalPosition(item);
           
            if (item.IsItemSelected)
            {
                ArrangeSelectedItem(item, zOffset, rotationAngleArgs, isanimate);               
            }
            else
            {
                ArrangeNonSelectedItem(item, index, position, zOffset,ref rotationAngleArgs, isanimate, flowFactor);                          
            }

            item.SetOnClickListener(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void SetVerticalPosition(PlatformCarouselItem item)
        {
            if(isVirtualization)
            {
                yPos = (controlHeight / 2) - (itemHeight / 2);
            }
            else if(item.LayoutParameters != null)
            {
                yPos = (controlHeight / 2) - (item.LayoutParameters.Height / 2);
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
            xPos = (Width / 2) - (itemWidth / 2);
            AnimateItem(item, zOffset, rotationAngleArgs, animate);
            item.BringToFront();
            item.SetY(yPos);
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
            if (index < selectedIndex)
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
            if (index == (selectedIndex - 1))
            {
                if (!isVirtualization)
                {
                    if (flowFactor)
                    {
                        if (item.LayoutParameters != null)
                            xPos = (controlWidth / 2) - item.LayoutParameters.Width - selectedItemOffset;
                    }
                    else
                    {
                        xPos = (controlWidth / 2) + selectedItemOffset;
                    }
                }
                else
                {
                    if (flowFactor)
                    {
                        xPos = (controlWidth / 2) - itemWidth - selectedItemOffset;
                    }
                    else
                    {
                        xPos = (controlWidth / 2) + selectedItemOffset;
                    }
                }
            }
            else
            {
                if (!isVirtualization)
                {
                    if (flowFactor)
                    {
                        if (item.LayoutParameters != null)
                            xPos = (controlWidth / 2) - item.LayoutParameters.Width + (moffset * (position + 1)) - selectedItemOffset;
                    }
                    else
                    {
                        xPos = (controlWidth / 2) + ((-moffset * (position + 1)) + selectedItemOffset);
                    }
                }
                else
                {
                    if (flowFactor)
                    {
                        xPos = (controlWidth / 2) - itemWidth + (moffset * (position + 1)) - selectedItemOffset;
                    }
                    else
                    {
                        xPos = (controlWidth / 2) + ((-moffset * (position + 1)) + selectedItemOffset);
                    }
                }
            }

            previousItems?.Add(item);

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
            if (index == (selectedIndex + 1))
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
                xPos = (controlWidth / 2) + (moffset * (position - 1)) + selectedItemOffset;
            }
            else
            {
                xPos = (controlWidth / 2) - (moffset * (position - 1)) - ((!isVirtualization ? itemWidth : item.LayoutParameters != null ? item.LayoutParameters.Width : 0)+ selectedItemOffset);
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
                xPos = (controlWidth / 2) + selectedItemOffset;
            }
            else
            {
                xPos = (controlWidth / 2) - ((!isVirtualization ? itemWidth : item.LayoutParameters != null ? item.LayoutParameters.Width : 0) + selectedItemOffset);
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
            nextItems?.Add(item);
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
            Animate(item, zOffset, (item.GetX() != 0 && animate) ? duration : 0, rotationAngleArgs, xPos);
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
            item.SetY(yPos);
            using (var interpolate = new DecelerateInterpolator(1))
            {
                item.Animate()?.RotationY(nrotationangle)
                    .ScaleY(zOffset)
                    .ScaleX(zOffset)
                    .X(pos)
                    .SetDuration(mduration)
                    .WithLayer()
                    .SetInterpolator(interpolate).Start();
            }
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

            if (TempItemsSource != null)
            {
                CalculateItemPositions(TempItemsSource.Count);
            }

            UpdateItems();
        }

        /// <summary>
        /// Saves the previous start and end positions.
        /// </summary>
        void SavePreviousPosition()
        {
            if (start != -1 && end != -1)
            {
                preStart = start;
                preEnd = end;
            }
        }

        /// <summary>
        /// Initializes position-related variables.
        /// </summary>
        void InitializePositionVariables()
        {
            centerPosition = MeasuredWidth / 2;
            startPosition = -centerPosition;
            endPosition = centerPosition * 2;
            xPositionLeft = (controlWidth / 2) - (itemWidth + selectedItemOffset) - ((moffset * selectedIndex) - 1);
            xPositionRight = (controlWidth / 2) + selectedItemOffset + moffset;
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
                if (startPosition < xPositionLeft)
                {
                    if ((start == -1 || preStart == start) && !startEnabled)
                    {
                        start = i;
                        startEnabled = true;
                    }
                }

                if (i > selectedIndex)
                {
                    if (endPosition < xPositionRight)
                    {
                        end = i;
                        break;
                    }

                    xPositionRight += moffset;
                }

                end = i;
                xPositionLeft += moffset;
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
            return start != -1 && end != -1;
        }

        /// <summary>
        /// Checks if there are no previous start and end positions.
        /// </summary>
        /// <returns><c>true</c> if there are no previous start and end positions; otherwise, <c>false</c>.</returns>
        bool HasNoPreviousPositions()
        {
            return preStart == -1 && preEnd == -1;
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
            for (int i = start; i <= end; i++)
            {
                AddItemToTempDataSource(TempItemsSource, i);
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
            if (TempDataSource == null || TempDataSource.Count == 0) return;

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
                    carouselItem.LayoutParameters = new LayoutParams(itemWidth, itemHeight);
                    carouselItem.Index = count;
                    CarouselItemAccessibility(carouselItem);
                    RemoveItemFromParent(carouselItem);

                    if (count <= selectedIndex)
                    {
                        AddView(carouselItem, count);
                    }
                    else
                    {
                        tempList?.Add(carouselItem);
                    }
                }
                ArrangeItemsBasedOnVirtualization();
            }
        }

        /// <summary>
        /// Adds items from the temporary list to the view.
        /// </summary>
        void AddItemsToView()
        {
            if (tempList == null) return;

            ReverseTempList();

            foreach (var item in tempList)
            {
                RemoveItemFromParent(item);
                AddView(item);
                BringToFrontIfNeeded(item);
            }

            tempList.Clear();
        }

        /// <summary>
        /// Reverses the temporary list.
        /// </summary>
        void ReverseTempList()
        {
            if (tempList != null)
            {
                var newList = new List<PlatformCarouselItem>(tempList);
                tempList.Clear();
                newList.Reverse();

                foreach (var item in newList)
                {
                    tempList.Add(item);
                }
            }
        }

        /// <summary>
        /// Brings the item to the front if needed based on the view mode.
        /// </summary>
        /// <param name="item">The item to bring to front.</param>
        void BringToFrontIfNeeded(PlatformCarouselItem item)
        {
            if (viewMode != ViewMode.Default) return;

            if (ItemsSource != null && TempItemsSource != null && TempItemsSource.Count > 0)
            {
                BringChildToFront(TempItemsSource[selectedIndex]);
            }
        }

        /// <summary>
        /// Arranges carousel items linearly.
        /// </summary>
        void ItemsArrangeLinear()
        {
            RemoveAllViews();

            if (TempDataSource == null || TempDataSource.Count == 0) return;

            InitializeHorizontalGridView();
            ConfigureHorizontalGridView();
            AddGridViewToLinearLayout();

            AddView(linearLayout);
        }

        /// <summary>
        /// Initializes the horizontal grid view.
        /// </summary>
        void InitializeHorizontalGridView()
        {
            if(Context != null)
            {
                horizontalGridView = new RecyclerView(Context);
                linearLayoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false);
                horizontalGridView.SetLayoutManager(linearLayoutManager);
            }
        }

        /// <summary>
        /// Configures the horizontal grid view by setting its adapter and item decoration.
        /// </summary>
        void ConfigureHorizontalGridView()
        {
            if (TempDataSource != null && Context != null)
            {
                var itemAdapter = new ItemAdapter(Context, this, itemHeight, itemWidth, TempDataSource);
                horizontalGridView?.SetAdapter(itemAdapter);
            }

            var itemDecoration = new SpaceItemDecoration(ItemSpacing);
            horizontalGridView?.AddItemDecoration(itemDecoration);

            horizontalGridView?.ScrollToPosition(SelectedIndex);
        }

        /// <summary>
        /// Adds the horizontal grid view to a linear layout.
        /// </summary>
        void AddGridViewToLinearLayout()
        {
            linearLayout = new LinearLayout(Context);
            linearLayout.SetGravity(GravityFlags.CenterVertical);
            linearLayout.AddView(horizontalGridView);
        }

        /// <summary>
        /// Add the refresh items.
        /// </summary>
        void AddRefreshItems()
        {
            if (!isSelected)
            {
                ArrangeItemsIfDefaultViewMode();
            }
            else if (isSelected && preEnd != -1 && preStart != -1)
            {
                if (preStart < start)
                {
                    RemoveItemsFromStart(start - preStart);
                }
                else
                {
                    InsertItemsAtStart(preStart - start);
                }

                if (preEnd > end)
                {
                    RemoveItemsFromEnd(preEnd - end);
                }
                else
                {
                    InsertItemsAtEnd(end - preEnd);
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
                CreateItemContent(i, start + i);
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
            if (TempDataSource == null) return;

            for (int i = 1; i <= count; i++)
            {
                CreateItemContent(TempDataSource.Count, preEnd + i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void ArrangeItemsIfDefaultViewMode()
        {
            if (viewMode == ViewMode.Default)
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
            tempList?.Clear();
            if (TempDataSource != null && TempDataSource.Count > 0)
            {
                int count = start;
                for (int datasourceindex = 0; datasourceindex < TempDataSource.Count; datasourceindex++)
                {
                    PlatformCarouselItem PlatformCarouselItem = TempDataSource[datasourceindex] as PlatformCarouselItem;
                    PlatformCarouselItem.ParentItem = this;
                    PlatformCarouselItem.LayoutParameters = new LayoutParams(itemWidth, itemHeight);
                    PlatformCarouselItem.Index = count;
                    CarouselItemAccessibility(PlatformCarouselItem);
                    RemoveItemFromParent(PlatformCarouselItem);
                    if (count <= selectedIndex)
                    {
                        AddView(PlatformCarouselItem);
                    }
                    else
                    {
                        tempList?.Add(PlatformCarouselItem);
                    }

                    count++;
                }
                if(tempList != null)
                {
                    var newList = new List<PlatformCarouselItem>(tempList.Capacity);
                    newList.AddRange(tempList);
                    tempList.Clear();
                    for (int i = newList.Count - 1; i >= 0; i--)
                    {
                        tempList.Add(newList[i]);
                    }

                    foreach (PlatformCarouselItem reverseItem in tempList)
                    {
                        RemoveItemFromParent((PlatformCarouselItem)reverseItem);
                        AddView(reverseItem);
                        if (viewMode == ViewMode.Default)
                        {
                            if (itemsSource != null && TempItemsSource != null)
                            {
                                BringChildToFront((PlatformCarouselItem)TempItemsSource[selectedIndex]);
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

            if (TempItemsSource != null && TempItemsSource.Count > 0)
            {
                PlatformCarouselItem = TempItemsSource[v] as PlatformCarouselItem;
            }
            if(TempDataSource != null && PlatformCarouselItem != null)
                TempDataSource.Insert(i, PlatformCarouselItem);
        }

        /// <summary>
        /// To set PlatformCarouselItem accessibility properties
        /// </summary>
        /// <param name="item">carousel item</param>
        void CarouselItemAccessibility(PlatformCarouselItem item)
        {
            if (itemsSource != null)
            {
                item.ContentDescription = item.AutomationId + " PlatformCarouselItem " + (item.Index + 1) + " of " + ((IList)itemsSource).Count;
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
                LoadMoreItemsCount = mauiView.LoadMoreItemsCount;
        }

        /// <summary>
        /// To update the selected index
        /// </summary>
        /// <param name="mauiView"></param>
        internal void UpdateSelectedIndex(ICarousel mauiView)
        {
            if (mauiView.ItemsSource != null)
            {
                if (mauiView.SelectedIndex >= mauiView.ItemsSource.Count() && mauiView.ItemsSource.Count() > 0)
                    SelectedIndex = mauiView.ItemsSource.Count() - 1;
                else if (mauiView.SelectedIndex < 0)
                    SelectedIndex = 0;
                else
                    SelectedIndex = mauiView.SelectedIndex;
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
        private int itemSpacing;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceItemDecoration"/> class.
        /// </summary>
        /// <param name="space">The total space to apply between items, divided equally on all sides.</param>
        public SpaceItemDecoration(int space)
        {
            itemSpacing = space / 2;
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
            outRect.Left = itemSpacing;
            outRect.Right = itemSpacing;
            outRect.Bottom = itemSpacing;
            outRect.Top = itemSpacing;
        }

    }
	#endregion
}