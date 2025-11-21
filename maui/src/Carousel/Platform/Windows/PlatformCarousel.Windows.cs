using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using AutomationProperties = Microsoft.UI.Xaml.Automation.AutomationProperties;
using ControlTemplate = Microsoft.UI.Xaml.Controls.ControlTemplate;
using DataTemplate = Microsoft.UI.Xaml.DataTemplate;
using Point = Windows.Foundation.Point;
using Rect = Windows.Foundation.Rect;
using ResourceDictionary = Microsoft.UI.Xaml.ResourceDictionary;
using Size = Windows.Foundation.Size;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// The Carousel control allows the user to navigate between their items with rich animated UI
	/// </summary>
	/// <exclude/>
	public partial class PlatformCarousel : Selector, IDisposable
	{
		#region Bindable properties

		/// <summary>
		/// Using a DependencyProperty as the backing store for ViewMode.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty ViewModeProperty =
			DependencyProperty.Register("ViewMode", typeof(ViewMode), typeof(PlatformCarousel), new PropertyMetadata(ViewMode.Default, new PropertyChangedCallback(OnViewModeChanged)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for AllowLoadMore.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty AllowLoadMoreProperty =
			DependencyProperty.Register("AllowLoadMore", typeof(bool), typeof(PlatformCarousel), new PropertyMetadata(false, OnAllowLoadMoreChanged));

		/// <summary>
		///   Using a DependencyProperty as the backing store for LoadMoreItemsCount.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty LoadMoreItemsCountProperty =
			DependencyProperty.Register("LoadMoreItemsCount", typeof(int), typeof(PlatformCarousel), new PropertyMetadata(0));

		/// <summary>
		/// Using a DependencyProperty as the backing store for LoadMoreView.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty LoadMoreViewProperty =
			DependencyProperty.Register("LoadMoreView", typeof(object), typeof(PlatformCarousel), new PropertyMetadata(null, OnLoadMoreViewChanged));

		/// <summary>
		///  Using a DependencyProperty as the backing store for ItemsSource. This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly new DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(object), typeof(PlatformCarousel), new PropertyMetadata(null, OnItemsSourceChanged));

		/// <summary>
		/// Using a DependencyProperty as the backing store for Offset.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty OffsetProperty =
			DependencyProperty.Register("Offset", typeof(double), typeof(PlatformCarousel), new PropertyMetadata(60d, new PropertyChangedCallback(RefreshCarouselItems)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PlatformCarousel), new PropertyMetadata(Orientation.Horizontal, new PropertyChangedCallback(OnOrientationChanging)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for RotationAngle.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty RotationAngleProperty =
			DependencyProperty.Register("RotationAngle", typeof(double), typeof(PlatformCarousel), new PropertyMetadata(45d, new PropertyChangedCallback(RefreshCarouselItems)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for SelectedItemOffset.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty SelectedItemOffsetProperty =
			DependencyProperty.Register("SelectedItemOffset", typeof(double), typeof(PlatformCarousel), new PropertyMetadata(120d, new PropertyChangedCallback(RefreshCarouselItems)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for ZOffset.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty ZOffsetProperty =
			DependencyProperty.Register("ZOffset", typeof(double), typeof(PlatformCarousel), new PropertyMetadata(0.0, new PropertyChangedCallback(RefreshCarouselItems)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty DurationProperty =
			DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(PlatformCarousel), new PropertyMetadata(TimeSpan.FromMilliseconds(600)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for EasingFunction.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty EasingFunctionProperty =
		   DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(PlatformCarousel), new PropertyMetadata(new CubicEase()));

		/// <summary>
		/// Using a DependencyProperty as the backing store for ScaleOffset.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty ScaleOffsetProperty =
			DependencyProperty.Register("ScaleOffset", typeof(double), typeof(PlatformCarousel), new PropertyMetadata(0.7d, new PropertyChangedCallback(RefreshCarouselItems)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for SelectedTemplate.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty SelectedItemTemplateProperty =
			DependencyProperty.Register("SelectedItemTemplate", typeof(DataTemplate), typeof(PlatformCarousel), new PropertyMetadata(null));

		/// <summary>
		/// Using a DependencyProperty as the backing store for ViewMode.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty ItemSpaceProperty =
			DependencyProperty.Register("ItemSpace", typeof(int), typeof(PlatformCarousel), new PropertyMetadata(10, new PropertyChangedCallback(OnItemSpaceChanged)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for ItemWidth.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty ItemWidthProperty =
			DependencyProperty.Register("ItemWidth", typeof(double), typeof(PlatformCarousel), new PropertyMetadata(100d, new PropertyChangedCallback(RefreshCarouselItems)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for ItemHeight.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty ItemHeightProperty =
			DependencyProperty.Register("ItemHeight", typeof(double), typeof(PlatformCarousel), new PropertyMetadata(100d, new PropertyChangedCallback(RefreshCarouselItems)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for EnableVirtualization.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty EnableVirtualizationProperty =
			DependencyProperty.Register("EnableVirtualization", typeof(bool), typeof(PlatformCarousel), new PropertyMetadata(false));

		/// <summary>
		/// Using the DependencyProperty as the backing store for EnableNavigationButtonProperty.
		/// </summary>
		internal static readonly DependencyProperty EnableNavigationButtonProperty =
			DependencyProperty.Register("EnableNavigationButton", typeof(bool), typeof(PlatformCarousel), new PropertyMetadata(true));

		/// <summary>
		/// Using a DependencyProperty as the backing store for ViewMode.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty SwipeMovementModeProperty =
			DependencyProperty.Register("SwipeMovementMode", typeof(SwipeMovementMode), typeof(PlatformCarousel), new PropertyMetadata(SwipeMovementMode.MultipleItems));

		/// <summary>
		/// Using a DependencyProperty as the backing store for EnableInteraction. This enables animation, styling, binding, etc...
		/// </summary>
		internal static readonly DependencyProperty EnableInteractionProperty = DependencyProperty.Register("EnableInteraction", typeof(bool), typeof(PlatformCarousel), new PropertyMetadata(true, OnEnableInteractionChanged));

		#endregion

		#region Fields

		/// <summary>
		/// Initializes a new instance of the queue
		/// </summary>
		internal Queue _queue = new();

		/// <summary>
		/// Maintain a temp collection for DataVirtualization
		/// </summary>
		internal IEnumerable? _tempCollection;

		/// <summary>
		/// Disable with out animation scroll
		/// </summary>
		internal bool _isDisableScrollChange;

		/// <summary>
		/// Update intermediate scroll
		/// </summary>
		internal bool _isIntermediate;

		/// <summary>
		/// Maintain collection for UIVirtualization
		/// </summary>
		internal ObservableCollection<object>? _tabCollection;

		/// <summary>
		/// Maintain collection for virtualized UI elements.
		/// </summary>
		internal ObservableCollection<object> _virtualItemList = [];

		/// <summary>
		/// Declaration for CDesiredDeceleration 
		/// </summary>
		private const double CDesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

		/// <summary>
		/// Initializes a new instance of the ItemsPresenter
		/// </summary>
		private ItemsPresenter? _itemsPresenter;

		/// <summary>
		/// TapOffset value. 
		/// </summary>
		private double? _tapOffset;

		/// <summary>
		/// AutomationId field.
		/// </summary>
		private string _automationId = string.Empty;

		/// <summary>
		/// Initializes a new instance of the CarouselItem 
		/// </summary>
		private PlatformCarouselItem? _previousSelectedItem;

		/// <summary>
		/// Initialize object is null.
		/// </summary>
		private object? _internalSelectedItem;

		/// <summary>
		/// Initialize the value of center is zero.
		/// </summary>
		private double _center;

		/// <summary>
		/// Initialize the value to zero.
		/// </summary>
		private double _vCenter;

		/// <summary>
		/// Initializes a new instance of the  CarouselPanel
		/// </summary>
		private SfCarouselPanel? _carouselItemsPanel;

		/// <summary>
		/// Initializes a new instance of the  CarouselLinearPanel
		/// </summary>
		private SfCarouselLinearPanel? _carouselLinearPanel;

		/// <summary>
		/// Gets or sets the value to restrict the selection change while swiping
		/// </summary>
		private bool _isManipulatedData;

		/// <summary>
		/// Declare value as false 
		/// </summary>
		private bool _canSelect;

		/// <summary>
		/// Initializes a new instance of the ScrollViewer <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> class.
		/// </summary>
		private ScrollViewer? _scrollViewer;

		/// <summary>
		/// Gets or sets the value to restrict the selection change while enable virtualization
		/// </summary>
		private bool _isSelectionNotChange;

		/// <summary>
		/// Maintain indexes
		/// </summary>
		private int _start = -1, _end = -1, _preStart = -1, _preEnd = -1;

		/// <summary>
		/// Selection change update
		/// </summary>
		private bool _isSelectionChanged;

		/// <summary>
		/// Maintain Previous offset
		/// </summary>
		private double _preOffset;

		/// <summary>
		/// Maintain Horizontal and Vertical offset
		/// </summary>
		private double _hOffset, _vOffset;

		/// <summary>
		/// Handler for managing the carousel's platform-specific functionality
		/// </summary>
		private CarouselHandler? _carouselHandler;

		/// <summary>
		/// Indicates whether swipe execution is allowed
		/// </summary>
		private bool _canExecuteSwipe = true;

		/// <summary>
		/// Indicates whether scroll execution is allowed
		/// </summary>
		private bool _canExecuteScroll = true;

		/// <summary>
		/// Indicates whether the swipe ended.
		/// </summary>
		private bool _isSwipeEnded = true;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/> class.
		/// </summary>
		public PlatformCarousel()
		{
			DefaultStyleKey = typeof(PlatformCarousel);
			Loaded += SfCarousel_Loaded;
			Unloaded += SfCarousel_Unloaded;
			Items.VectorChanged += Items_VectorChanged;
			AutomationProperties.SetName(this, _automationId);
			AutomationProperties.SetAutomationId(this, AutomationId);
		}

		#endregion

		#region Events

		/// <summary>
		/// Initializes a new instance of the ItemsCollectionChangedHandler
		/// </summary>
		internal event ItemsCollectionChangedHandler? ItemsCollectionChanged;

		/// <summary>
		/// Initializes a new instance of the ItemLoadedHandler
		/// </summary>
		internal event ItemLoadedHandler? ItemLoaded;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the ViewMode.
		/// </summary>
		/// <value>
		/// By default set to Default.
		/// </value>      
		public ViewMode ViewMode
		{
			get { return (ViewMode)GetValue(ViewModeProperty); }
			set { SetValue(ViewModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether enable or disable allow load more
		/// </summary>
		public bool AllowLoadMore
		{
			get { return (bool)GetValue(AllowLoadMoreProperty); }
			set { SetValue(AllowLoadMoreProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum items count.
		/// </summary>
		public int LoadMoreItemsCount
		{
			get { return (int)GetValue(LoadMoreItemsCountProperty); }
			set { SetValue(LoadMoreItemsCountProperty, value); }
		}

		/// <summary>
		///  Gets or sets the load more view.
		/// </summary>
		public object LoadMoreView
		{
			get { return (object)GetValue(LoadMoreViewProperty); }
			set { SetValue(LoadMoreViewProperty, value); }
		}

		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		public new object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the space between items.
		/// </summary>
		/// <value>
		/// The default value is 60.
		/// </value>
		public double Offset
		{
			get { return (double)GetValue(OffsetProperty); }
			set { SetValue(OffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the Orientation
		/// </summary>
		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the distance between the current item and other.
		/// </summary>
		/// <value>
		/// The default value is 120.
		/// </value>
		public double SelectedItemOffset
		{
			get { return (double)GetValue(SelectedItemOffsetProperty); }
			set { SetValue(SelectedItemOffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the rotation angle for <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/>.
		/// </summary>
		/// <remarks>
		/// Rotation angel of the items.
		/// </remarks>
		/// <value>
		/// The default value is 45.
		/// </value>
		public double RotationAngle
		{
			get { return (double)GetValue(RotationAngleProperty); }
			set { SetValue(RotationAngleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the zooming offset.
		/// </summary>
		/// <value>
		/// The default value is zero. The ZOffset should be set in between 0 and 1.
		/// </value>
		public double ZOffset
		{
			get { return (double)GetValue(ZOffsetProperty); }
			set { SetValue(ZOffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the length of time for which this timeline plays, not counting
		/// repetitions.
		/// </summary>
		/// <remarks>
		/// Specify the time taken for move an item.
		/// </remarks>
		/// <value>
		/// The default value is <see cref="T:System.TimeSpan"/>.
		/// </value>
		public TimeSpan Duration
		{
			get { return (TimeSpan)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the easing function applied to this animation with <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/>.
		/// </summary>
		/// <remarks>
		/// Customize the animation effect.
		/// </remarks>
		/// <value>
		/// The default value is <see
		/// cref="N:Windows.UI.Xaml.Media.Animation.EasingFunctionBase"/>.
		/// </value>
		public EasingFunctionBase EasingFunction
		{
			get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
			set { SetValue(EasingFunctionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the scale offset with <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/>.
		/// </summary>
		/// <value>
		/// The default value is 0.7.
		/// </value>
		public double ScaleOffset
		{
			get { return (double)GetValue(ScaleOffsetProperty); }
			set { SetValue(ScaleOffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the template for the selected item
		/// </summary>
		public Microsoft.UI.Xaml.DataTemplate SelectedItemTemplate
		{
			get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
			set { SetValue(SelectedItemTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the ItemSpace
		/// </summary>
		public int ItemSpace
		{
			get { return (int)GetValue(ItemSpaceProperty); }
			set { SetValue(ItemSpaceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ItemHeight
		/// </summary>
		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether enable or disable Is Virtualization
		/// </summary>
		public bool EnableVirtualization
		{
			get { return (bool)GetValue(EnableVirtualizationProperty); }
			set { SetValue(EnableVirtualizationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the ItemWidth.
		/// </summary>
		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}

		/// <summary>
		///  Gets or sets a value indicating whether the item is enabled.
		/// </summary>
		internal bool IsManipulatedData
		{
			get { return _isManipulatedData; }
			set { _isManipulatedData = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is enabled.
		/// </summary>
		internal ScrollViewer? ScrollViewer
		{
			get { return _scrollViewer; }
			set { _scrollViewer = value; }
		}

		/// <summary>
		/// Gets or sets the AutomationId
		/// </summary>
		internal string AutomationId
		{
			get { return _automationId; }
			set
			{
				if (_automationId != value)
				{
					_automationId = value;
					_automationId ??= string.Empty;

					OnAutomationIdChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is enabled.
		/// </summary>
		internal bool CanSelect
		{
			get { return _canSelect; }
			set { _canSelect = value; }
		}

		/// <summary>
		/// Gets or sets the Items panel
		/// </summary>
		internal SfCarouselPanel? CarouselItemsPanel
		{
			get { return _carouselItemsPanel; }
			set { _carouselItemsPanel = value; }
		}

		/// <summary>
		/// Gets or sets the Linear panel
		/// </summary>
		internal SfCarouselLinearPanel? CarouselLinearPanel
		{
			get { return _carouselLinearPanel; }
			set { _carouselLinearPanel = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether enable or disable is busy
		/// </summary>
		public SwipeMovementMode SwipeMovementMode
		{
			get { return (SwipeMovementMode)GetValue(SwipeMovementModeProperty); }
			set { SetValue(SwipeMovementModeProperty, value); }
		}

		/// <summary>
		/// Gets or Sets a value indicating whether enable or disable NavigationButton
		/// </summary>
		internal bool EnableNavigationButton
		{
			get { return (bool)GetValue(EnableNavigationButtonProperty); }
			set { SetValue(EnableNavigationButtonProperty, value); }
		}

		/// <summary>
		/// Gets or Sets a value indicating whether enable or disable Interaction 
		/// </summary>
		internal bool EnableInteraction
		{
			get { return (bool)GetValue(EnableInteractionProperty); }
			set { SetValue(EnableInteractionProperty, value); }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// To dispose unused objects
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Moves the selection next <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/>.
		/// </summary>
		/// <remarks>
		/// The MoveNext method moves the current item position, one position forward.
		/// Return value is void.
		/// </remarks>
		public void MoveNext()
		{
			bool isForward = FlowDirection == Microsoft.UI.Xaml.FlowDirection.LeftToRight;
			SelectionChangedEventArgs args = new SelectionChangedEventArgs();
			if (!EnableVirtualization)
			{
				ChangeSelectionToNextItem(args, isForward);
			}
			else
			{
				MoveNextVirtualized(isForward, args);
			}
		}

		/// <summary>
		/// Change selection to next available item
		/// </summary>
		/// <param name="args"></param>
		/// <param name="isForward"></param>
		void ChangeSelectionToNextItem(SelectionChangedEventArgs args, bool isForward)
		{
			args.OldItem = GetMauiItem(SelectedIndex);
			ValidateIndexToMoveNext(isForward);
			args.NewItem = GetMauiItem(SelectedIndex);
			if (args.OldItem != args.NewItem && ViewMode == ViewMode.Default)
			{
				OnSelectionChanged(args);
			}
		}

		/// <summary>
		/// Validate Selected Index to move next item
		/// </summary>
		/// <param name="isForward"></param>
		void ValidateIndexToMoveNext(bool isForward)
		{
			if ((SelectedIndex < (Items.Count - 1)) && isForward)
			{
				SelectedIndex++;

				if (_virtualView != null)
				{
					_virtualView.SelectedIndex++;
				}
			}
			else if (SelectedIndex > 0 && !isForward)
			{
				SelectedIndex--;

				if (_virtualView != null)
				{
					_virtualView.SelectedIndex--;
				}
			}
		}

		/// <summary>
		/// Move to next virtualized item
		/// </summary>
		/// <param name="isForward"></param>
		/// <param name="args"></param>
		void MoveNextVirtualized(bool isForward, SelectionChangedEventArgs args)
		{
			if (_tempCollection is IList tempCollection && ((SelectedIndex < tempCollection.Count - 1 && isForward) || (SelectedIndex > 0 && !isForward)))
			{
				int newIndex = SelectedIndex + (isForward ? 1 : -1);
				var newItem = _virtualItemList[newIndex] as PlatformCarouselItem;
				HandleSelectionChange(args, newIndex, newItem);
			}
		}

		/// <summary>
		/// Handle selection change 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="newIndex"></param>
		/// <param name="newItem"></param>
		void HandleSelectionChange(SelectionChangedEventArgs args, int newIndex, PlatformCarouselItem? newItem)
		{
			var containerItem = ContainerFromIndex(newIndex) as PlatformCarouselItem;
			if (containerItem == null || (containerItem != null && containerItem != newItem))
			{
				_isSelectionNotChange = true;
			}

			if (SelectedIndex != newIndex)
			{
				TriggerSelectionChange(args, newItem, newIndex, false);
			}
		}

		/// <summary>
		/// To trigger selection change
		/// </summary>
		/// <param name="args"></param>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="isSingleTapSelection"></param>
		void TriggerSelectionChange(SelectionChangedEventArgs args, PlatformCarouselItem? item, int index, bool isSingleTapSelection)
		{
			args.OldItem = GetMauiItem(SelectedIndex);
			SelectedIndex = index;

			if (_virtualView != null)
			{
				_virtualView.SelectedIndex = index;
			}

			if (EnableVirtualization)
			{
				_isSelectionNotChange = false;
				SelectedItem = item;
			}

			args.NewItem = GetMauiItem(SelectedIndex);
			if (args.NewItem != args.OldItem && isSingleTapSelection)
			{
				OnSelectionChanged(args);
			}
		}

		/// <summary>
		/// Refresh the layout and rearrange the children with <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/>.
		/// </summary>
		/// <remarks>
		/// Return value is void.
		/// </remarks>
		public void Refresh()
		{
			ArrangeChildren();
		}

		/// <summary>
		/// Load more method.
		/// </summary>
		public void LoadMore()
		{
			if (LoadMoreItemsCount > 0 && _queue.Count > 0)
			{
				UpdateSource();
			}
		}

		/// <summary>
		///  Retrieves the Maui item corresponding to the given selected index.
		/// </summary>
		/// <param name="selectedIndex"></param>
		/// <returns></returns>
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
		/// Moves the selection to previous <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/>.
		/// </summary>
		/// <remarks>
		/// The MovePrevious method moves the current item position, one position backward.
		/// Return value is void.
		/// </remarks>
		public void MovePrevious()
		{
			bool isBackward = !(FlowDirection == Microsoft.UI.Xaml.FlowDirection.LeftToRight);
			SelectionChangedEventArgs args = new SelectionChangedEventArgs();
			if (!EnableVirtualization)
			{
				ChangeSelectionToNextItem(args, isBackward);
			}
			else
			{
				if (_tempCollection is IList tempCollection && ((SelectedIndex > 0 && !isBackward) || (SelectedIndex < tempCollection.Count - 1 && isBackward)))
				{
					int newIndex = SelectedIndex + (isBackward ? 1 : -1);
					var newItem = _virtualItemList[newIndex] as PlatformCarouselItem;
					HandleSelectionChange(args, newIndex, newItem);
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Method for refresh 
		/// </summary>
		/// <param name="item">instance contains event</param>
		internal void Refresh(PlatformCarouselItem item)
		{
			if (_carouselItemsPanel != null && !EnableVirtualization)
			{
				ArrangeAllChildrenInPanel(true, true);
			}
			else
			{
				if (ViewMode == ViewMode.Default && EnableVirtualization && _virtualItemList != null)
				{
					int index = ((IList)_virtualItemList).IndexOf(item);
					ArrangeChild(item, index, false);
				}
			}
		}

		/// <summary>
		/// Arrange all children in item panel
		/// </summary>
		/// <param name="isAnimation"></param>
		/// <param name="updateAutomationProperties"></param>
		void ArrangeAllChildrenInPanel(bool isAnimation, bool updateAutomationProperties)
		{
			if (_carouselItemsPanel == null)
			{
				return;
			}

			foreach (var element in _carouselItemsPanel.Children)
			{
				if (element is PlatformCarouselItem carouselItem)
				{
					int carouselItemIndex = _carouselItemsPanel.Children.IndexOf(carouselItem);
					ArrangeChild(carouselItem, carouselItemIndex, isAnimation);
					if (updateAutomationProperties)
					{
						AutomationProperties.SetName(carouselItem, carouselItem.AutomationId + " PlatformCarouselItem " + (carouselItemIndex + 1) + " of " + _carouselItemsPanel.Children.Count);
						AutomationProperties.SetAutomationId(carouselItem, carouselItem.AutomationId + " PlatformCarouselItem " + (carouselItemIndex + 1) + " of " + _carouselItemsPanel.Children.Count);
					}
				}
			}
		}

		/// <summary>
		/// Method to Move Horizontal Item 
		/// </summary>
		/// <param name="item">The item</param>
		/// <param name="isDisableAnimation">is disable animation</param>
		internal async void MoveHorizontalItem(PlatformCarouselItem item, bool isDisableAnimation)
		{
			if (ScrollViewer != null && item != null)
			{
				Point position = GetItemPosition(item);

				double currentHOffset = ScrollViewer.HorizontalOffset;
				double centerPoint = (ActualWidth - item.Width + ItemSpace) / 2;
				double movePoint = position.X - centerPoint;
				if (!isDisableAnimation)
				{
					await System.Threading.Tasks.Task.Delay(1);
				}
				ScrollViewer.ScrollToHorizontalOffset(currentHOffset + movePoint);
			}
		}

		/// <summary>
		/// method to Move Vertical Item. 
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="isDisableAnimation">is disable animation.</param>
		internal async void MoveVerticalItem(PlatformCarouselItem item, bool isDisableAnimation)
		{
			if (ScrollViewer != null && item != null)
			{
				Point position = GetItemPosition(item);

				double centerPoint = (ActualHeight - item.ActualHeight + ItemSpace) / 2;
				double movePoint = position.Y - centerPoint;

				if (!isDisableAnimation)
				{
					await System.Threading.Tasks.Task.Delay(1);
				}
				ScrollViewer.ChangeView(null, ScrollViewer.VerticalOffset + movePoint, null, isDisableAnimation);
			}
		}

		/// <summary>
		/// Get item position
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Point GetItemPosition(PlatformCarouselItem item)
		{
			var uiElement = item.TransformToVisual(this);
			return uiElement.TransformPoint(new Point(0, 0));
		}

		/// <summary>
		/// Carousel item arrangement method.
		/// </summary>
		/// <param name="sender">object as sender.</param>
		internal void ArrangeCarouselItem(object sender)
		{
			PlatformCarouselItem? item = sender as PlatformCarouselItem;
			int index = GetItemIndex(item);

			if (ViewMode == ViewMode.Default)
			{
				if (AllowLoadMore)
				{
					ArrangeAllChildrenInPanel(true, false);
				}
				else
				{
					if (item != null)
					{
						ArrangeChild(item, index, false);
					}
				}
			}
		}

		/// <summary>
		/// Get item index
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		int GetItemIndex(PlatformCarouselItem? item)
		{
			if (!EnableVirtualization)
			{
				return IndexFromContainer(item);
			}
			else
			{
				if (item != null)
				{
					return _virtualItemList.IndexOf(item);
				}
			}

			return -1;
		}

		/// <summary>
		/// UpdateChildren Method.
		/// </summary>
		internal void UpdateChildren()
		{
			if (Items.Count > 0)
			{
				ArrangeChildren();
			}
		}

		/// <summary>
		/// Method for Arrange Children items.
		/// </summary>
		internal void ArrangeChildren()
		{
			if (ViewMode == ViewMode.Default)
			{
				if (!AllowLoadMore)
				{
					if (!EnableVirtualization)
					{
						foreach (var element in Items)
						{
							PlatformCarouselItem? item = ContainerFromItem(element) as PlatformCarouselItem;
							if (item != null)
							{
								int index = IndexFromContainer(item);
								ArrangeChild(item, index, false);
							}
						}
					}
					else
					{
						int count = 0;
						for (int i = _start; i <= _end; i++)
						{
							PlatformCarouselItem? item = ContainerFromIndex(count) as PlatformCarouselItem;
							if (item != null)
							{
								ArrangeChild(item, i, false);
							}
							count++;
						}
					}
				}
				else
				{
					ArrangeAllChildrenInPanel(false, false);
				}
			}
			else
			{
				UpdateLinearPanel();
			}
		}

		/// <summary>
		/// Update linear panel
		/// </summary>
		internal void UpdateLinearPanel()
		{
			if (!AllowLoadMore)
			{
				foreach (var element in Items)
				{
					if (element is PlatformCarouselItem item)
					{
						item.Height = ItemHeight;
						item.Width = ItemWidth;
					}
				}
			}
			else
			{
				if (_carouselLinearPanel == null)
				{
					return;
				}

				foreach (var element in _carouselLinearPanel.Children)
				{
					if (element is PlatformCarouselItem item)
					{
						item.Height = ItemHeight;
						item.Width = ItemWidth;
					}
				}
			}
		}

		/// <summary>
		///  Arrange Child
		/// </summary>
		/// <param name="element">element which can be arranged.</param>
		/// <param name="index">index of element</param>
		/// <param name="isAnimation">enable or disable animation.</param>
		internal void ArrangeChild(PlatformCarouselItem element, int index, bool isAnimation)
		{
			double horizontalCenterPoint = ActualWidth / 2;
			double verticalCenterPoint = ActualHeight / 2;
			int selectedIndex = index - SelectedIndex;
			double indexFactor = 0;
			AdjustElementSize(ref element);
			if (selectedIndex < 0)
			{
				indexFactor = -1;
			}
			else if (selectedIndex > 0)
			{
				indexFactor = 1;
			}
			CalculateCenters(element, horizontalCenterPoint, verticalCenterPoint, index, selectedIndex);

			double scaleOffset = indexFactor == 0 ? 1 : ScaleOffset;
			int zIndex = CalculateZIndex(selectedIndex);

			if (Orientation == Orientation.Vertical)
			{
				ArrangeVerticalElements(element, zIndex, indexFactor, scaleOffset, isAnimation);
			}
			else
			{
				ArrangeHorizontalElements(element, zIndex, indexFactor, scaleOffset, isAnimation);
			}

			SetElementPosition(element, horizontalCenterPoint, verticalCenterPoint);
		}

		/// <summary>
		/// Arranges a carousel item vertically within the carousel.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="zIndex"></param>
		/// <param name="indexFactor"></param>
		/// <param name="scaleOffset"></param>
		/// <param name="isAnimation"></param>
		void ArrangeVerticalElements(PlatformCarouselItem element, int zIndex, double indexFactor, double scaleOffset, bool isAnimation)
		{
			if (((_vCenter + element.ActualHeight) < 0 || _vCenter > ActualHeight)
				 && ((element.Top + element.ActualHeight) < 0 || element.Top > ActualHeight)
				 && !((_vCenter + element.ActualHeight) < 0 && element.Top > ActualHeight)
				 && !((element.Top + element.ActualHeight) < 0 && _vCenter > ActualHeight))
			{
				element.Arrange(_vCenter, zIndex, RotationAngle * indexFactor, ZOffset * Math.Abs(indexFactor), scaleOffset, Duration, EasingFunction, false, isAnimation);
			}
			else
			{
				element.Arrange(_vCenter, zIndex, RotationAngle * -indexFactor, ZOffset * Math.Abs(indexFactor), scaleOffset, Duration, EasingFunction, true, isAnimation);
			}
		}

		/// <summary>
		/// Arranges a carousel item horizontally within the carousel.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="zIndex"></param>
		/// <param name="indexFactor"></param>
		/// <param name="scaleOffset"></param>
		/// <param name="isAnimation"></param>
		void ArrangeHorizontalElements(PlatformCarouselItem element, int zIndex, double indexFactor, double scaleOffset, bool isAnimation)
		{
			if (((_center + element.ActualWidth) < 0 || _center > ActualWidth)
				&& ((element.Left + element.ActualWidth) < 0 || element.Left > ActualWidth)
				&& !((_center + element.ActualWidth) < 0 && element.Left > ActualWidth)
				&& !((element.Left + element.ActualWidth) < 0 && _center > ActualWidth))
			{
				element.Arrange(_center, zIndex, RotationAngle * indexFactor, ZOffset * Math.Abs(indexFactor), scaleOffset, Duration, EasingFunction, false, isAnimation);
			}
			else
			{
				element.Arrange(_center, zIndex, RotationAngle * indexFactor, ZOffset * Math.Abs(indexFactor), scaleOffset, Duration, EasingFunction, true, isAnimation);
			}
		}

		/// <summary>
		///  Adjusts the size of a carousel item to match the specified item dimensions.
		/// </summary>
		/// <param name="element"></param>
		void AdjustElementSize(ref PlatformCarouselItem element)
		{
			if (element.ActualWidth != ItemWidth)
			{
				element.Width = ItemWidth;
			}

			if (element.ActualHeight != ItemHeight)
			{
				element.Height = ItemHeight;
			}
		}

		/// <summary>
		/// Calculates the center positions for a carousel item based on its index and the carousel's current state.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="horizontalCenterPoint"></param>
		/// <param name="verticalCenterPoint"></param>
		/// <param name="index"></param>
		/// <param name="selectedIndex"></param>
		void CalculateCenters(PlatformCarouselItem element, double horizontalCenterPoint, double verticalCenterPoint, int index, int selectedIndex)
		{
			if (index == SelectedIndex)
			{
				_center = horizontalCenterPoint - (element.ActualWidth / 2);
				_vCenter = verticalCenterPoint - (element.ActualHeight / 2);
			}
			else
			{
				if (index < SelectedIndex)
				{
					if (index == (SelectedIndex - 1))
					{
						_center = horizontalCenterPoint - element.ActualWidth - SelectedItemOffset;
						_vCenter = verticalCenterPoint - element.ActualHeight - SelectedItemOffset;
					}
					else
					{
						_center = horizontalCenterPoint - element.ActualWidth + (Offset * (selectedIndex + 1)) - SelectedItemOffset;
						_vCenter = verticalCenterPoint - element.ActualHeight + (Offset * (selectedIndex + 1)) - SelectedItemOffset;
					}
				}
				else
				{
					if (index == (SelectedIndex + 1))
					{
						_center = horizontalCenterPoint + SelectedItemOffset;
						_vCenter = verticalCenterPoint + SelectedItemOffset;
					}
					else
					{
						_center = horizontalCenterPoint + (Offset * (selectedIndex - 1)) + SelectedItemOffset;
						_vCenter = verticalCenterPoint + (Offset * (selectedIndex - 1)) + SelectedItemOffset;
					}
				}
			}
		}

		/// <summary>
		/// Calculates the z-index for a carousel item based on its position relative to the selected item.
		/// </summary>
		/// <param name="selectedIndex"></param>
		/// <returns></returns>
		int CalculateZIndex(int selectedIndex)
		{
			int zIndex = 0;
			if (!AllowLoadMore)
			{
				if (!EnableVirtualization)
				{
					zIndex = Items.Count - Math.Abs(selectedIndex);
				}
				else if (_tempCollection != null)
				{
					zIndex = ((IList)_tempCollection).Count - Math.Abs(selectedIndex);
				}
			}
			else if (_carouselItemsPanel != null)
			{
				zIndex = _carouselItemsPanel.Children.Count - 1 - Math.Abs(selectedIndex);
			}
			return zIndex;
		}


		/// <summary>
		/// Sets the final position of a carousel item within the carousel's layout.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="horizontalCenterPoint"></param>
		/// <param name="verticalCenterPoint"></param>
		void SetElementPosition(PlatformCarouselItem element, double horizontalCenterPoint, double verticalCenterPoint)
		{
			if (Orientation == Orientation.Vertical)
			{
				element.Top = _vCenter;
				element.Left = horizontalCenterPoint - (element.ActualWidth / 2);
			}
			else
			{
				element.Left = _center;
				element.Top = element.ActualHeight < ActualHeight ? verticalCenterPoint - (element.ActualHeight / 2) : 0;
			}
		}

		/// <summary>
		/// Method to select an item
		/// </summary>
		internal void UpdateSelection()
		{
			if (SelectedItem != null)
			{
				if (ContainerFromItem(SelectedItem) is PlatformCarouselItem item)
				{
					_previousSelectedItem = SelectedItem as PlatformCarouselItem;
					if (_previousSelectedItem != null)
					{
						_previousSelectedItem.IsSelected = true;
					}
					SelectItem(item);
				}
			}
		}

		/// <summary>
		/// Method for update source
		/// </summary>
		internal void UpdateSource()
		{
			if (_queue.Count != 0)
			{
				if (_queue.Count < LoadMoreItemsCount)
				{
					DequeueItems(_queue.Count);
				}
				else
				{
					DequeueItems(LoadMoreItemsCount);
				}
			}
			RemoveLoadMoreView();
		}

		/// <summary>
		/// Dequeue items
		/// </summary>
		/// <param name="count"></param>
		void DequeueItems(int count)
		{
			for (int i = 0; i < count; i++)
			{
				((IList)base.ItemsSource).Add(_queue.Dequeue());
			}
			UpdateLinearPanel();
		}

		/// <summary>
		/// Method to remove load more view
		/// </summary>
		void RemoveLoadMoreView()
		{
			if (_queue.Count <= 0 && AllowLoadMore)
			{
				RemoveLoadMoreViewFromParent();
			}
		}

		/// <summary>
		/// Remove load more item from parent
		/// </summary>
		void RemoveLoadMoreViewFromParent()
		{
			if (ViewMode == ViewMode.Default)
			{
				if (CarouselItemsPanel != null && CarouselItemsPanel.Children.Count > 0 && ((PlatformCarouselItem)CarouselItemsPanel.Children[(^1)]).Tag != null && ((PlatformCarouselItem)CarouselItemsPanel.Children[(^1)]).Tag.ToString() == "Load More")
				{
					((PlatformCarouselItem)CarouselItemsPanel.Children[(^1)]).Content = null;
					CarouselItemsPanel.Children.RemoveAt(CarouselItemsPanel.Children.Count - 1);
				}
			}
			else
			{
				if (CarouselLinearPanel != null && CarouselLinearPanel.Children.Count > 0 && ((PlatformCarouselItem)CarouselLinearPanel.Children[(^1)]).Tag != null && ((PlatformCarouselItem)CarouselLinearPanel.Children[(^1)]).Tag.ToString() == "Load More")
				{
					((PlatformCarouselItem)CarouselLinearPanel.Children[(^1)]).Content = null;
					CarouselLinearPanel.Children.RemoveAt(CarouselLinearPanel.Children.Count - 1);
				}
			}
		}

		#region Virtualization Implementation

		/// <summary>
		/// Validate index based on ViewMode
		/// </summary>
		/// <param name="size">The size</param>
		internal void ValidateIndex(Size size)
		{
			if (ViewMode == ViewMode.Default)
			{
				DefaultViewModeIndex(size);
			}
			else
			{
				LinearViewModeIndex(size);
			}
		}

		/// <summary>
		/// Main method for isVirtualization, it perform remove or add items when swipe or select the items
		/// </summary>
		internal void UpdateItems()
		{
			if ((_start != -1 && _end != -1 && _preStart == -1 && _preEnd == -1) || _preEnd < _start)
			{
				if ((_carouselItemsPanel != null && (_carouselItemsPanel.Children.Count == 0 || _preEnd < _start)) ||
					(_carouselLinearPanel != null && (_carouselLinearPanel.Children.Count == 0 || _preEnd < _start)))
				{
					_tabCollection?.Clear();
					base.ItemsSource = _tabCollection;
					//Initially add the items based on start and end position
					for (int i = _start; i <= _end; i++)
					{
						CreateItemContent(-1, i);
					}
				}
			}
			else
			{
				if (_carouselItemsPanel == null && (_carouselItemsPanel != null && _carouselItemsPanel.Children.Count == 0))
				{
					return;
				}

				RemoveOrInsertStartingItems();
				RemoveOrInsertEndingItems();
			}
		}

		/// <summary>
		/// Remove or insert item in start
		/// </summary>
		void RemoveOrInsertStartingItems()
		{
			if (_preStart < _start)
			{
				RemoveItemsFromStart(_start - _preStart);
			}
			else
			{
				InsertItemsAtStart(_preStart - _start);
			}
		}

		/// <summary>
		/// Remove or insert item in end
		/// </summary>
		void RemoveOrInsertEndingItems()
		{
			if (_preEnd > _end)
			{
				RemoveItemsFromEnd(_preEnd - _end);
			}
			else
			{
				InsertItemsAtEnd(_end - _preEnd);
			}
		}

		/// <summary>
		/// Remove item in start
		/// </summary>
		/// <param name="count"></param>
		void RemoveItemsFromStart(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_tabCollection?.RemoveAt(0);
			}
		}

		/// <summary>
		/// Insert item in start
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
		/// Remove item in end
		/// </summary>
		/// <param name="count"></param>
		void RemoveItemsFromEnd(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_tabCollection?.RemoveAt(_tabCollection.Count - 1);
			}
		}

		/// <summary>
		/// Insert item in end
		/// </summary>
		/// <param name="count"></param>
		void InsertItemsAtEnd(int count)
		{
			for (int i = 1; i <= count; i++)
			{
				CreateItemContent(Items.Count, _preEnd + i);
			}
		}

		/// <summary>
		/// Update Scroll offset with out animation
		/// </summary>
		internal void UpdateScroll()
		{
			if (ScrollViewer != null)
			{
				if (Orientation == Orientation.Horizontal)
				{
					ScrollViewer.ChangeView(_hOffset, null, null, true);
				}
				else
				{
					ScrollViewer.ChangeView(null, _vOffset, null, true);
				}
			}

		}

		#endregion

		#endregion

		#region Override Methods

		/// <summary>
		/// To remove all the instance which is used in Carousel
		/// </summary>        
		/// <param name="disposing">The disposing</param>      
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Loaded -= SfCarousel_Loaded;
				Unloaded -= SfCarousel_Unloaded;

				_itemsPresenter = null;

				_internalSelectedItem = null;

				if (CarouselItemsPanel != null)
				{
					CarouselItemsPanel.Dispose();
					CarouselItemsPanel = null;
				}

				if (Items != null && Items.Count > 0)
				{
					foreach (var menuItems in Items)
					{
						PlatformCarouselItem? carousel = menuItems as PlatformCarouselItem;
						carousel?.Dispose();
					}
				}

				if (_previousSelectedItem != null)
				{
					_previousSelectedItem.Dispose();
					_previousSelectedItem = null;
				}

				if (ScrollViewer != null)
				{
					ScrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
					ScrollViewer = null;
				}

				if (Items != null)
				{
					Items.VectorChanged -= Items_VectorChanged;
				}
			}
		}

		/// <summary>
		/// Checks whether the item is a <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/>
		/// </summary>
		/// <exclude/>
		/// <param name="item">object as item</param>
		/// <returns>
		/// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is PlatformCarouselItem;
		}

		/// <summary>
		/// Gets a <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/>  for override.
		/// </summary>
		/// <exclude/>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new PlatformCarouselItem();
		}

		/// <summary>
		/// Sets the properties for the <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/> item.
		/// </summary>
		/// <exclude/>
		/// <param name="element">DependencyObject as element</param>
		/// <param name="item">object as item</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			PlatformCarouselItem? carouselItem = element as PlatformCarouselItem;

			if (carouselItem != null)
			{
				carouselItem.ParentItemsControl = this;
				Microsoft.UI.Xaml.Data.Binding binding = new Microsoft.UI.Xaml.Data.Binding
				{
					Path = new PropertyPath("SelectedItemTemplate"),
					Source = this
				};
				carouselItem.SetBinding(PlatformCarouselItem.SelectedTemplateProperty, binding);
				carouselItem.Selected -= SelectCarouselItem;
				carouselItem.Selected += SelectCarouselItem;
				if (!EnableVirtualization || ViewMode != ViewMode.Linear)
				{
					carouselItem.GotFocus -= SelectCarouselItem;
					carouselItem.GotFocus += SelectCarouselItem;
				}
				if (SelectedItem != null && SelectedItem == item && !carouselItem.IsSelected)
				{
					carouselItem.IsSelected = true;
				}

				if (ItemLoaded != null)
				{
					ItemLoadedArgs arg = new ItemLoadedArgs();

					if (!EnableVirtualization)
					{
						arg.Index = IndexFromContainer(carouselItem as PlatformCarouselItem);
					}
					else if (_tempCollection != null)
					{
						arg.Index = ((IList)_tempCollection).IndexOf(carouselItem);
					}

					arg.Item = carouselItem;
					ItemLoaded(this, arg);
				}

				if (carouselItem.Content == null)
				{
					base.PrepareContainerForItemOverride(element, item);
				}
			}
		}

		/// <summary>
		/// manipulation started method.
		/// </summary>
		/// <exclude/>
		/// <param name="e">ManipulationStartedRoutedEventArgs as e</param>
		protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
		{
			CanSelect = false;
			_tapOffset = 0.0;
			base.OnManipulationStarted(e);
		}

		/// <summary>
		/// Sets the selected item as the next or previous carousal item <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/> based on the mouse movement
		/// </summary>
		/// <exclude/>
		/// <param name="e">PointerRoutedEventArgs as e</param>
		protected override async void OnPointerWheelChanged(PointerRoutedEventArgs e)
		{
			if (EnableInteraction)
			{
				if (SwipeMovementMode == SwipeMovementMode.MultipleItems)
				{
					MoveItem(e);
				}
				else
				{
					if (_canExecuteScroll)
					{
						MoveItem(e);
						_canExecuteScroll = false;
						await Task.Delay(Duration);
						_canExecuteScroll = true;
					}
				}
			}
			PlatformCarouselItem? selectItem = SelectedItem != null ? ContainerFromItem(SelectedItem) as PlatformCarouselItem : null;
			if (selectItem != null)
			{
				selectItem.IsPointerOver = false;
			}

			base.OnPointerWheelChanged(e);
		}

		/// <summary>
		/// Method to move item based on mouse movements
		/// </summary>
		/// <param name="e"></param>
		void MoveItem(PointerRoutedEventArgs e)
		{
			if (e.GetCurrentPoint(this).Properties.MouseWheelDelta > 100)
			{
				MovePrevious();
			}
			else if (e.GetCurrentPoint(this).Properties.MouseWheelDelta < -100)
			{
				MoveNext();
			}
		}

		/// <summary>
		/// Sets the selected item as the next or previous carousal item <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/> based on the keys pressed.
		/// </summary>
		/// <exclude/>
		/// <param name="e"> KeyRoutedEventArgs as e</param>
		protected override void OnKeyDown(KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Left || e.Key == Windows.System.VirtualKey.Down)
			{
				MovePrevious();
			}
			else if (e.Key == Windows.System.VirtualKey.Right || e.Key == Windows.System.VirtualKey.Up)
			{
				MoveNext();
			}
			else if (e.Key == Windows.System.VirtualKey.PageUp || e.Key == Windows.System.VirtualKey.Home)
			{
				SelectedIndex = 0;

				if (_virtualView != null)
				{
					_virtualView.SelectedIndex = 0;
				}
			}
			else if (e.Key == Windows.System.VirtualKey.PageDown || e.Key == Windows.System.VirtualKey.End)
			{
				SelectedIndex = Items.Count - 1;

				if (_virtualView != null)
				{
					_virtualView.SelectedIndex = Items.Count - 1;
				};
			}

			base.OnKeyDown(e);
		}

		/// <summary>
		/// Sets the translation information associated with the event.
		/// </summary>
		/// <exclude/>
		/// <param name="e">ManipulationInertiaStartingRoutedEventArgs as e</param>
		protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e)
		{
			e.TranslationBehavior.DesiredDeceleration = CDesiredDeceleration;
			base.OnManipulationInertiaStarting(e);
		}

		/// <summary>
		/// Sets the selected index
		/// </summary>
		/// <exclude/>
		/// <param name="e">ManipulationDeltaRoutedEventArgs as e</param>
		protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
		{
			IsManipulatedData = true;
			if (EnableInteraction)
			{
				if (Orientation == Orientation.Horizontal)
				{
					_tapOffset += e.Delta.Translation.X;
				}
				else
				{
					_tapOffset += e.Delta.Translation.Y;
				}
			}

			SwipeStartedEventArgs swipeStartedEventArgs = new SwipeStartedEventArgs();

			if (SwipeMovementMode == SwipeMovementMode.MultipleItems)
			{
				if (_tapOffset > 100)
				{
					SwipeBackward(swipeStartedEventArgs);
				}
				else if (_tapOffset < -100)
				{
					SwipeForward(swipeStartedEventArgs);
				}
			}
			else
			{
				if (_canExecuteSwipe)
				{
					if (_tapOffset > 0)
					{
						SwipeBackward(swipeStartedEventArgs);
					}
					else if (_tapOffset < -0)
					{
						SwipeForward(swipeStartedEventArgs);
					}
					_canExecuteSwipe = false;
				}
			}
			if (e.IsInertial)
			{
				e.Complete();
			}

			base.OnManipulationDelta(e);
		}

		/// <summary>
		/// Handles the forward swiping action in the carousel.
		/// </summary>
		/// <param name="swipeStartedEventArgs"></param>
		void SwipeForward(SwipeStartedEventArgs swipeStartedEventArgs)
		{
			swipeStartedEventArgs.IsSwipedLeft = false;
			if (_isSwipeEnded)
			{
				OnSwipeStarted(swipeStartedEventArgs);
			}

			MoveNext();
			_tapOffset = 0.0;
		}

		/// <summary>
		/// Handles the backward swiping action in the carousel.
		/// </summary>
		/// <param name="swipeStartedEventArgs"></param>
		void SwipeBackward(SwipeStartedEventArgs swipeStartedEventArgs)
		{
			swipeStartedEventArgs.IsSwipedLeft = true;
			if (_isSwipeEnded)
			{
				OnSwipeStarted(swipeStartedEventArgs);
			}

			MovePrevious();
			_tapOffset = 0.0;
		}

		/// <summary>
		/// Sets the selected item as <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/>
		/// <exclude/>
		/// </summary>
		/// <param name="e">ManipulationCompletedRoutedEventArgs as e</param>
		protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
		{
			CanSelect = true;
			_canExecuteSwipe = true;
			if (SelectedItem != null)
			{
				if (ContainerFromItem(SelectedItem) is PlatformCarouselItem selectedItem)
				{
					selectedItem.ContentTemplate = SelectedItemTemplate ?? selectedItem.NormalContentTemplate;
				}
			}

			base.OnManipulationCompleted(e);
			if (!_isSwipeEnded)
			{
				OnSwipeEnded(new EventArgs());
			}
		}

		/// <summary>
		/// Sets the selected item when the selection has been changed by the user
		/// </summary>
		/// <exclude/>
		/// <param name="args">DependencyPropertyChangedEventArgs as args</param>
		/// <seealso cref="T:Syncfusion.UI.Xaml.Controls.Layout.PlatformCarouselItem"/>
		protected override void OnSelectionChanged(DependencyPropertyChangedEventArgs args)
		{
			PlatformCarouselItem? newValue = null;
			if (SelectedIndex >= 0 && (SelectedIndex < Items.Count || EnableVirtualization))
			{
				if (EnableVirtualization && _isSelectionNotChange)
				{
					return;
				}

				if (args.OldValue != null)
				{
					_previousSelectedItem = args.OldValue as PlatformCarouselItem ?? ContainerFromItem(args.OldValue) as PlatformCarouselItem;
				}

				if (args.NewValue != null || args.NewValue is PlatformCarouselItem)
				{
					newValue = ContainerFromItem(args.NewValue) as PlatformCarouselItem;
				}

				if (newValue != null)
				{
					newValue.IsSelected = true;
					SelectItem(newValue);
				}

				if (args.NewValue == null && SelectedItem == null)
				{
					SetSelectedIndex();
				}

				base.OnSelectionChanged(args);
			}

			if (ItemsSource == null && Items.Count == 0)
			{
				_internalSelectedItem = args.NewValue;
			}
		}

		/// <summary>
		/// Set selected index
		/// </summary>
		void SetSelectedIndex()
		{
			if (EnableVirtualization && _tempCollection is IList tempCollection && SelectedIndex < tempCollection.Count)
			{
				var item = tempCollection[SelectedIndex];
				var carouselitem = ContainerFromItem(item);
				if (carouselitem == null)
				{
					ValidateIndex(new Size() { Width = ActualWidth, Height = ActualHeight });
					Refresh();
				}
				if (item != null)
				{
					SelectedItem = item;
				}
			}
			else
			{
				SelectedIndex = -1;

				if (_virtualView != null)
				{
					_virtualView.SelectedIndex = -1;
				}

				Refresh();
			}
		}

		/// <summary>
		/// method for pointer entered
		/// </summary>
		/// <exclude/>
		/// <param name="e">PointerRoutedEventArgs as e</param>
		protected override void OnPointerEntered(PointerRoutedEventArgs e)
		{
			base.OnPointerEntered(e);
		}

		/// <summary>
		/// Sets the size for the arranging items
		/// </summary>
		/// <exclude/>
		/// <param name="finalSize"> the finalSize</param>
		/// <returns> The size </returns>
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size size = base.ArrangeOverride(finalSize);
			if (_itemsPresenter != null)
			{
				AdjustCoverItems();

				if (ViewMode == ViewMode.Default)
				{
					ArrangeDefaultViewMode();
				}
				else
				{
					ArrangeLinearViewMode();
				}
			}

			if (this != null)
			{
				Microsoft.Maui.Platform.WrapperView? parentElement = Parent as Microsoft.Maui.Platform.WrapperView;
				if (parentElement != null && parentElement.FlowDirection != FlowDirection)
				{
					parentElement.FlowDirection = FlowDirection;
				}
			}

			return size;
		}

		/// <summary>
		/// AdjustCoverItems
		/// </summary>
		void AdjustCoverItems()
		{
			if (_itemsPresenter != null)
			{
				if (Orientation == Orientation.Vertical)
				{
					AdjustCoverItemsForVerticalOrientation();

				}
				else
				{
					AdjustCoverItemsForHorizontalOrientation();

				}
			}

		}

		/// <summary>
		/// Adjusts the width of carousel items for vertical orientation.
		/// </summary>
		void AdjustCoverItemsForVerticalOrientation()
		{
			if (_itemsPresenter != null)
			{
				if (!AllowLoadMore)
				{
					foreach (var item in Items)
					{
						PlatformCarouselItem? coverItem = ContainerFromItem(item) as PlatformCarouselItem;
						if (coverItem != null && coverItem.Width.Equals(double.NaN))
						{
							coverItem.MaxWidth = _itemsPresenter.ActualWidth;
						}
					}
				}
				else
				{

					if (_carouselItemsPanel != null)
					{
						foreach (var item in _carouselItemsPanel.Children)
						{
							PlatformCarouselItem? coverItem = item as PlatformCarouselItem;
							if (coverItem != null && coverItem.Width.Equals(double.NaN))
							{
								coverItem.MaxWidth = _itemsPresenter.ActualWidth;
							}
						}
					}
				}
			}

		}

		/// <summary>
		/// Adjusts the height and layout of carousel items for horizontal orientation.
		/// </summary>
		void AdjustCoverItemsForHorizontalOrientation()
		{
			if (_itemsPresenter != null)
			{
				if (!AllowLoadMore)
				{
					foreach (var item in Items)
					{
						PlatformCarouselItem? coverItem = ContainerFromItem(item) as PlatformCarouselItem;
						if (coverItem != null)
						{
							if (coverItem.Height.Equals(double.NaN))
							{
								coverItem.MaxHeight = _itemsPresenter.ActualHeight;
							}

							coverItem.UpdateLayout();
						}
					}
				}
				else
				{
					if (_carouselItemsPanel != null)
					{
						foreach (var item in _carouselItemsPanel.Children)
						{
							PlatformCarouselItem? coverItem = item as PlatformCarouselItem;
							if (coverItem != null)
							{
								if (coverItem.Height.Equals(double.NaN))
								{
									coverItem.MaxHeight = _itemsPresenter.ActualHeight;
								}

								coverItem.UpdateLayout();
							}
						}
					}
				}
			}

		}

		/// <summary>
		///  Arranges carousel items in the default view mode.
		/// </summary>
		void ArrangeDefaultViewMode()
		{
			if (_itemsPresenter != null)
			{
				double m = _itemsPresenter.ActualWidth / 2;
				double m1 = _itemsPresenter.ActualHeight / 2;
				if (_carouselItemsPanel != null)
				{
					for (int index = 0; index < _carouselItemsPanel.Children.Count; index++)
					{
						PlatformCarouselItem? item = _carouselItemsPanel.Children[index] as PlatformCarouselItem;
						int b = index - SelectedIndex;
						double mu = 0;
						if (b < 0)
						{
							mu = -1;
						}
						else if (b > 0)
						{
							mu = 1;
						}
						if (item != null)
						{
							double x = (m + ((b * Offset) + (SelectedItemOffset * mu))) - (item.ActualWidth / 2);
							double x1 = (m1 + ((b * Offset) + (SelectedItemOffset * mu))) - (item.ActualHeight / 2);
							double s = mu == 0 ? 1 : ScaleOffset;
							if (Orientation == Orientation.Vertical)
							{
								item.Top = x1;
								item.Left = m - (item.ActualWidth / 2);
								item.XRotation = RotationAngle * mu;
								item.YRotation = 0;
							}
							else
							{
								item.Left = x;
								if (item.ActualHeight < _itemsPresenter.ActualHeight)
								{
									item.Top = m1 - (item.ActualHeight / 2);
								}
								else
								{
									item.Top = 0;
								}

								item.YRotation = RotationAngle * mu;
								item.XRotation = 0;
							}

							item.ZOffset = ZOffset * Math.Abs(mu);
							item.Scale = s;
						}
					}
				}
			}
		}

		/// <summary>
		/// Arranges carousel items in the linear view mode.
		/// </summary>
		void ArrangeLinearViewMode()
		{
			if (Orientation == Orientation.Horizontal)
			{
				if (SelectedIndex != -1)
				{
					if (!EnableVirtualization && !AllowLoadMore)
					{
						MoveHorizontalItem((PlatformCarouselItem)ContainerFromIndex(SelectedIndex), false);
					}
				}
			}
			else
			{
				if (SelectedIndex != -1)
				{
					if (!EnableVirtualization && !AllowLoadMore)
					{
						MoveVerticalItem((PlatformCarouselItem)ContainerFromIndex(SelectedIndex), false);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the Framework elements on applying the templates.
		/// </summary>
		/// <exclude/>
		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_itemsPresenter = (ItemsPresenter)GetTemplateChild("ItemsPresenter");
			CarouselItemsPanel = (SfCarouselPanel)GetTemplateChild("ItemCanvas");
			CarouselLinearPanel = (SfCarouselLinearPanel)GetTemplateChild("ItemPanel");
			ScrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");

			if (ScrollViewer != null)
			{
				ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
				ScrollViewer.DirectManipulationCompleted += ScrollViewer_DirectManipulationCompleted;
				ScrollViewer.DirectManipulationStarted += ScrollViewer_DirectManipulationStarted;

				if (ViewMode == ViewMode.Linear)
				{
					ScrollViewer.HorizontalScrollMode = EnableInteraction ? Microsoft.UI.Xaml.Controls.ScrollMode.Enabled : Microsoft.UI.Xaml.Controls.ScrollMode.Disabled;
				}
			}
		}

		/// <summary>
		/// Carousel Measure Override Method.
		/// </summary>
		/// <exclude/>
		/// <param name="availableSize">The available Size</param>
		/// <returns>Available size has returned.</returns>
		protected override Size MeasureOverride(Size availableSize)
		{
			if (!_isDisableScrollChange)
			{
				ValidateIndex(availableSize);
			}

			return base.MeasureOverride(availableSize);
		}
		#endregion

		#region Property Changed Methods

		/// <summary>
		/// method for On load more view changed
		/// </summary>
		/// <param name="d">The source of the event</param>
		/// <param name="e">instance contains event data</param>
		private static void OnLoadMoreViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PlatformCarousel carousel = (PlatformCarousel)d;
			carousel.OnLoadMoreViewChanged();
		}

		/// <summary>
		/// method for item space changed.
		/// </summary>
		/// <param name="d">DependencyObject as d</param>
		/// <param name="e">DependencyPropertyChangedEventArgs as e</param>
		private static void OnItemSpaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PlatformCarousel carousel = (PlatformCarousel)d;
			if (carousel != null && carousel.CarouselLinearPanel != null)
			{
				carousel.CarouselLinearPanel.InvalidateMeasure();
				carousel.CarouselLinearPanel.InvalidateArrange();
			}
		}

		/// <summary>
		/// On ViewMode Changed method
		/// </summary>
		/// <param name="obj">The source of the event.</param>
		/// <param name="e">instance containing the event data.</param>
		private static void OnViewModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (obj is PlatformCarousel control)
			{
				control.OnViewModeChanged(e);
			}
		}

		/// <summary>
		/// On EnableInteraction changed method
		/// </summary>
		/// <param name="d">The source of the event</param>
		/// <param name="e">instance containing the event data.</param>
		private static void OnEnableInteractionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is PlatformCarousel control)
			{
				if (control.ViewMode == ViewMode.Linear && control.ScrollViewer != null)
				{
					control.ScrollViewer.HorizontalScrollMode = control.EnableInteraction
						? Microsoft.UI.Xaml.Controls.ScrollMode.Enabled
						: Microsoft.UI.Xaml.Controls.ScrollMode.Disabled;
				}
			}
		}


		/// <summary>
		/// On Orientation Changing
		/// </summary>
		/// <param name="obj">The source of the event</param>
		/// <param name="args">instance containing the event data</param>
		private static void OnOrientationChanging(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (obj is PlatformCarousel instance && args.OldValue.ToString() != args.NewValue.ToString())
			{
				instance.UpdateLayout();
				instance.InvalidateMeasure();
				instance.InvalidateArrange();
				instance.Refresh();
			}
		}

		/// <summary>
		/// method to refresh an item
		/// </summary>
		/// <param name="sender">The source of the event</param>
		/// <param name="e">instance containing the event data</param>
		private static void RefreshCarouselItems(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is PlatformCarousel control && !Windows.ApplicationModel.DesignMode.DesignModeEnabled)
			{
				control.Dispatcher?.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { control.Refresh(); }).AsTask();
				control.Refresh();
			}
		}

		/// <summary>
		/// method for item source changed.
		/// </summary>
		/// <param name="d">The source of the event</param>
		/// <param name="e">instant contains event data</param>
		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PlatformCarousel carousel = (PlatformCarousel)d;
			carousel.OnItemsSourceChanged();
		}

		/// <summary>
		/// On Allow Load More Changed method
		/// </summary>
		/// <param name="d">The source of the event.</param>
		/// <param name="e">instance containing the event data.</param>
		private static void OnAllowLoadMoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PlatformCarousel? control = d as PlatformCarousel;
			control?.OnAllowLoadMoreChanged();
		}

		/// <summary>
		/// method for view changed
		/// </summary>
		/// <param name="sender">object as sender</param>
		/// <param name="e">ScrollViewerViewChangedEventArgs as e</param>
		void ScrollViewer_ViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
		{
			if (EnableVirtualization)
			{
				if (e.IsIntermediate)
				{
					if (Orientation == Orientation.Horizontal)
					{
						HandleHorizontalScroll();
					}
					else
					{
						HandleVerticalScroll();
					}
				}
				else
				{
					if (_isSelectionChanged)
					{
						UpdateScrollOffsetOnRendering();
						_isSelectionChanged = false;
					}
				}
			}
		}

		/// <summary>
		/// Handles the horizontal scrolling behavior of the carousel in virtualized mode.
		/// </summary>
		void HandleHorizontalScroll()
		{
			if (_scrollViewer != null && _scrollViewer.HorizontalOffset == _scrollViewer.ExtentWidth - _scrollViewer.ViewportWidth)
			{
				if (_isSelectionChanged)
				{
					return;
				}

				_isIntermediate = true;
				if (_tempCollection != null)
				{
					if (ScrollViewer != null && ScrollViewer.HorizontalOffset > _preOffset && _end != ((IList)_tempCollection).Count - 1)
					{
						AddForward(false);
					}
				}
			}
		}

		/// <summary>
		/// Handles the vertical scrolling behavior of the carousel in virtualized mode.
		/// </summary>
		void HandleVerticalScroll()
		{
			if (_scrollViewer != null && _scrollViewer.VerticalOffset == _scrollViewer.ExtentHeight - _scrollViewer.ViewportHeight)
			{
				if (_isSelectionChanged)
				{
					return;
				}

				_isIntermediate = true;
				if (ScrollViewer != null && ScrollViewer.VerticalOffset > _preOffset && _tempCollection != null && _end != ((IList)_tempCollection).Count - 1)
				{
					AddForward(false);
				}
			}
		}

		/// <summary>
		/// Method for Remove old View.
		/// </summary>
		void RemoveOldView()
		{
			if (AllowLoadMore)
			{
				if (CarouselItemsPanel != null)
				{
					if ((CarouselItemsPanel.Children.Count > 0 && (((PlatformCarouselItem)CarouselItemsPanel.Children[^1]).Tag != null))
						&& (((PlatformCarouselItem)CarouselItemsPanel.Children[^1]).Tag.ToString() == "Load More"))
					{
						((PlatformCarouselItem)CarouselItemsPanel.Children[^1]).Content = null;
					}
				}

				if (CarouselLinearPanel != null)
				{
					if ((CarouselLinearPanel.Children.Count > 0 && (((PlatformCarouselItem)CarouselLinearPanel.Children[^1]).Tag != null))
						&& (((PlatformCarouselItem)CarouselLinearPanel.Children[^1]).Tag.ToString() == "Load More"))
					{
						((PlatformCarouselItem)CarouselLinearPanel.Children[^1]).Content = null;
					}
				}
			}
		}

		/// <summary>
		/// method for manipulation started
		/// </summary>
		/// <param name="sender">object as sender</param>
		/// <param name="e">object as e</param>
		private void ScrollViewer_DirectManipulationStarted(object? sender, object e)
		{
			if (EnableInteraction)
			{
				_isDisableScrollChange = false;
				if (ScrollViewer != null)
				{
					_preOffset = Orientation == Orientation.Horizontal ? ScrollViewer.HorizontalOffset : ScrollViewer.VerticalOffset;
				}
			}
		}

		/// <summary>
		/// method for manipulation completed.
		/// </summary>
		/// <param name="sender">object as sender</param>
		/// <param name="e">object as e</param>
		private void ScrollViewer_DirectManipulationCompleted(object? sender, object e)
		{
			if (EnableVirtualization)
			{
				HandleVirtualizationScroll();
			}
		}


		/// <summary>
		/// Handles virtualization scrolling based on the current scroll direction and orientation.
		/// </summary>
		void HandleVirtualizationScroll()
		{
			if (_isSelectionChanged)
			{
				return;
			}

			if (Orientation == Orientation.Horizontal)
			{
				if (ScrollViewer != null)
				{
					if (ScrollViewer.HorizontalOffset > _preOffset)
					{
						AddForward(true);
					}
					else
					{
						AddBackward(true);
					}
				}
			}
			else
			{
				if (ScrollViewer != null)
				{
					if (ScrollViewer.VerticalOffset > _preOffset)
					{
						AddForward(true);
					}
					else
					{
						AddBackward(true);
					}
				}
			}

			_isIntermediate = false;
		}

		/// <summary>
		/// Carousel Loaded method.
		/// </summary>
		/// <param name="sender">object as sender.</param>
		/// <param name="e">RoutedEvent Argument as e.</param>
		private void SfCarousel_Loaded(object sender, RoutedEventArgs e)
		{
			if (Orientation == Orientation.Vertical)
			{
				Microsoft.UI.Xaml.VisualStateManager.GoToState(this, "Vertical", true);
			}
			if (SelectedIndex >= 0)
			{
				Refresh();
			}
			RefreshSelectedItem();
			if (SelectedItem == null && _internalSelectedItem != null)
			{
				SelectedItem = _internalSelectedItem;
			}

			UpdateSelection();
		}

		/// <summary>
		/// selected item method.
		/// </summary>
		void RefreshSelectedItem()
		{
			var selectedItem = Items.OfType<PlatformCarouselItem>().Where(item => item.IsSelected).FirstOrDefault();
			if (selectedItem != null)
			{
				int index = IndexFromContainer(selectedItem as PlatformCarouselItem);
				if (SelectedIndex != index)
				{
					SelectedIndex = index;

					if (_virtualView != null)
					{
						_virtualView.SelectedIndex = index;
					}
				}
			}
		}

		/// <summary>
		///  Carousel item selection method.
		/// </summary>
		/// <param name="sender">object as sender.</param>
		/// <param name="e">RoutedEvent Argument as e.</param>
		private void SelectCarouselItem(object sender, RoutedEventArgs e)
		{
			if (EnableInteraction)
			{
				PlatformCarouselItem? item = sender as PlatformCarouselItem;
				if (item == null)
				{
					return;
				}
				if (item != null)
				{
					int index = EnableVirtualization ? GetVirtualizationItemIndex(item) : IndexFromContainer(item);

					if (SelectedIndex != index)
					{
						SelectionChangedEventArgs args = new SelectionChangedEventArgs();
						TriggerSelectionChange(args, item, index, true);
					}
				}
			}
		}

		/// <summary>
		/// Get item index
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		int GetVirtualizationItemIndex(PlatformCarouselItem item)
		{
			int containerIndex = IndexFromContainer(item);
			if (_virtualItemList != null)
			{
				int index = ((IList)_virtualItemList).IndexOf(item);

				if (containerIndex != index)
				{
					_isSelectionNotChange = true;
				}

				return index;
			}
			return -1;
		}

		/// <summary>
		/// method for select item
		/// </summary>
		/// <param name="item">CarouselItem as item</param>
		void SelectItem(PlatformCarouselItem item)
		{
			if (item == null)
			{
				return;
			}
			if (_previousSelectedItem != null && _previousSelectedItem.IsSelected)
			{
				_previousSelectedItem.IsSelected = false;
				_previousSelectedItem.ApplyContentTemplate();
				_previousSelectedItem = null;
			}
			item.ApplyContentTemplate();
			int index = IndexFromContainer(item);
			if (ViewMode == ViewMode.Linear)
			{
				_isDisableScrollChange = false;
				_isSelectionChanged = true;
				if (Orientation == Orientation.Horizontal)
				{
					MoveHorizontalItem(item, false);
				}
				else
				{
					MoveVerticalItem(item, false);
				}
			}
			else if (index >= 0)
			{
				ValidateIndex(new Size() { Width = ActualWidth, Height = ActualHeight });
				UpdateChildren();
			}
		}

		/// <summary>
		/// Item changed method
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="event">instance containing the event data.</param>
		private void Items_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
		{
			if (ItemsCollectionChanged != null)
			{
				ItemCollection? itemCollection = sender as ItemCollection;
				if (itemCollection != null)
				{
					ItemsCollectionChanged(this, new ItemsCollectionChangedArgs(itemCollection));
				}
				else
				{
					ItemsCollectionChanged(this, new ItemsCollectionChangedArgs(Items as ItemCollection));
				}
			}
		}

		/// <summary>
		/// Carousel Unloaded method.
		/// </summary>
		/// <param name="sender">object as sender.</param>
		/// <param name="e">RoutedEvent Argument as e.</param>
		private void SfCarousel_Unloaded(object sender, RoutedEventArgs e)
		{
			Loaded -= SfCarousel_Loaded;
			Unloaded -= SfCarousel_Unloaded;
		}

		/// <summary>
		/// method for load more view
		/// </summary>
		private void OnLoadMoreViewChanged()
		{
			if (LoadMoreItemsCount > 0 && AllowLoadMore)
			{
				if (ViewMode == ViewMode.Linear)
				{
					if (_carouselLinearPanel != null && ((PlatformCarouselItem)_carouselLinearPanel.Children[^1]).Tag != null)
					{
						PlatformCarouselItem? carouselItem = _carouselLinearPanel.Children[^1] as PlatformCarouselItem;
						if (carouselItem != null)
						{
							carouselItem.Content = LoadMoreView;
						}
					}
				}
				else
				{
					if (_carouselItemsPanel != null && ((PlatformCarouselItem)_carouselItemsPanel.Children[^1]).Tag != null)
					{
						PlatformCarouselItem? carouselItem = _carouselItemsPanel.Children[^1] as PlatformCarouselItem;
						if (carouselItem != null)
						{
							carouselItem.Content = LoadMoreView;
						}
					}
				}
			}
		}

		/// <summary>
		/// method for view mode changed
		/// </summary>
		/// <param name="e"></param>
		void OnViewModeChanged(DependencyPropertyChangedEventArgs e)
		{
			ResourceDictionary rd = new ResourceDictionary() { Source = new Uri("ms-appx:///Syncfusion.Maui.Toolkit/Carousel/Platform/Windows/Styles/CarouselStyles.xaml") };
			RemoveOldView();
			if ((ViewMode)e.NewValue == ViewMode.Linear)
			{
				ItemsPanelTemplate? linearpanel = rd["LinearPanel"] as ItemsPanelTemplate;
				ControlTemplate? linearTemplate = rd["LinearTemplate"] as ControlTemplate;
				Template = linearTemplate;
				ItemsPanel = linearpanel;
			}
			else
			{
				RemoveOldView();
				ItemsPanelTemplate? defaultpanel = rd["DefaultPanel"] as ItemsPanelTemplate;
				ControlTemplate? defaultTemplate = rd["DefaultTemplate"] as ControlTemplate;
				Template = defaultTemplate;
				ItemsPanel = defaultpanel;
			}
		}

		/// <summary>
		/// method for item source changed.
		/// </summary>
		private void OnItemsSourceChanged()
		{
			if (ItemsSource is IEnumerable)
			{
				if (ItemsSource != null && AllowLoadMore)
				{
					if (ItemsSource != null)
					{
						SetQueue();
						int loadedItems = 0;

						if (base.ItemsSource != null)
						{
							loadedItems = ((IList)base.ItemsSource).Count;
						}

						ObservableCollection<object> tempCollection = [];
						int itemsToLoad = Math.Min(LoadMoreItemsCount, _queue.Count);

						if (loadedItems > 0)
						{
							itemsToLoad = loadedItems;
						}
						if (itemsToLoad > _queue.Count)
						{
							itemsToLoad = _queue.Count;
						}

						if (base.ItemsSource != null)
						{
							for (int i = 0; i < loadedItems; i++)
							{
								if (((IList)base.ItemsSource).Count > 0)
								{
									((IList)base.ItemsSource).RemoveAt(0);
								}
							}
						}

						for (int i = 0; i < itemsToLoad; i++)
						{
							if (_queue.Count > 0)
							{
								var queueItem = _queue.Dequeue();
								if (queueItem != null)
								{
									tempCollection.Add(queueItem);
									if (base.ItemsSource != null)
									{
										((IList)base.ItemsSource).Add(queueItem);
									}
								}
							}
						}
						base.ItemsSource ??= tempCollection;
					}
				}
				else if (EnableVirtualization)
				{
					_start = _preStart = _end = _preEnd = -1;
					_tempCollection = ItemsSource as IEnumerable;
					_virtualItemList.Clear();
					var tempCollection = _tempCollection as IList;
					for (int i = 0; i < tempCollection?.Count; i++)
					{
						var virtualItem = GetVirtualizedItem(i);
						if (virtualItem != null)
						{
							_virtualItemList.Add(virtualItem);
						}
					}
					_tabCollection = [];
					if (ItemsSource != null && ((IList)ItemsSource).Count <= 0)
					{
						base.ItemsSource = ItemsSource;
					}
					else
					{
						ValidateIndex(new Size() { Width = ActualWidth, Height = ActualHeight });
					}
				}
				else
				{
					base.ItemsSource = ItemsSource;
				}
			}
			else
			{
				throw new InvalidOperationException("Input was not in the correct format. Please provide IEnumerable input");
			}
		}

		/// <summary>
		/// Populates the queue with native view items from the item source.
		/// </summary>
		void SetQueue()
		{
			int j = 0;
			_queue.Clear();
			foreach (var item in ((IEnumerable)ItemsSource))
			{
				View? contentView = null;
				if (_virtualView != null && _virtualView.ItemTemplate != null)
				{
					contentView = GetItemView(_virtualView, j);
					j++;
				}
				if (contentView != null && _carouselHandler != null && _virtualView != null)
				{
					_queue.Enqueue(GetNativeViewItem(_virtualView, contentView, _carouselHandler));
				}
			}
		}

		/// <summary>
		/// On Allow Load More Changed method
		/// </summary>
		private void OnAllowLoadMoreChanged()
		{
			if (!AllowLoadMore && LoadMoreItemsCount == 0)
			{
				ObservableCollection<object> collection = [.. ((IList)base.ItemsSource), .. _queue];

				base.ItemsSource = collection;
			}
		}

		/// <summary>
		/// Add or insert the items
		/// </summary>
		/// <param name="position">The position</param>
		/// <param name="index">The index</param>
		void CreateItemContent(int position, int index)
		{
			var tempCollection = _tempCollection as IList;
			if ((tempCollection != null && tempCollection.Count <= index) || index == -1)
			{
				return;
			}

			if (tempCollection != null)
			{
				var item = _virtualItemList[index];

				if (item != null && _tabCollection != null)
				{
					if (position != -1)
					{
						_tabCollection.Insert(position, item);
					}
					else
					{
						_tabCollection.Add(item);
					}
				}
			}

		}

		/// <summary>
		/// Retrieves the virtualized item as a native view.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem? GetVirtualizedItem(int index)
		{
			View? contentView = null;
			if (_virtualView != null && _virtualView.ItemTemplate != null)
			{
				contentView = GetItemView(_virtualView, index);
			}
			if (contentView != null && _carouselHandler != null && _virtualView != null)
			{
				return (GetNativeViewItem(_virtualView, contentView, _carouselHandler));
			}

			return null;
		}

		/// <summary>
		/// On AutomationId changed method
		/// </summary>
		void OnAutomationIdChanged()
		{
			AutomationProperties.SetName(this, AutomationId);
			AutomationProperties.SetAutomationId(this, AutomationId);
		}

		#region Virtualization Scroll Calculation for LinearMode

		/// <summary>
		/// Calculate first and last visible index
		/// </summary>
		/// <param name="size">pass available carousel size</param>
		void LinearViewModeIndex(Size size)
		{
			bool isUpdateStartPosition = false;
			if (size.Width == 0)
			{
				return;
			}
			UpdatePreviousIndices();
			double horizontalCenterPoint = (size.Width - ItemWidth + ItemSpace) / 2;
			double verticalCenterPoint = (size.Height - ItemHeight + ItemSpace) / 2;
			double startRegion = -(size.Width * 3);
			double endRegion = size.Width * 3;
			double itemRegion = 0;
			if (Orientation == Orientation.Vertical)
			{
				startRegion = -(size.Height * 3);
				endRegion = size.Height * 3;
			}
			UpdateLinearStartEndPosition(horizontalCenterPoint, verticalCenterPoint, isUpdateStartPosition, itemRegion, startRegion, endRegion);
			UpdateItems();
		}

		/// <summary>
		/// Updates the start and end positions for linear orientation based on the center points.
		/// </summary>
		/// <param name="horizontalCenterPoint"></param>
		/// <param name="verticalCenterPoint"></param>
		/// <param name="isUpdateStartPosition"></param>
		/// <param name="itemRegion"></param>
		/// <param name="startRegion"></param>
		/// <param name="endRegion"></param>
		void UpdateLinearStartEndPosition(double horizontalCenterPoint, double verticalCenterPoint, bool isUpdateStartPosition, double itemRegion, double startRegion, double endRegion)
		{
			if (_tempCollection != null)
			{
				for (int i = 0; i < ((IList)_tempCollection).Count; i++)
				{
					int selectionindex = i - SelectedIndex;

					if (Orientation == Orientation.Horizontal)
					{
						itemRegion = horizontalCenterPoint + (((ItemWidth / 2) + ItemSpace) * selectionindex);
					}
					else
					{
						itemRegion = verticalCenterPoint + (((ItemHeight / 2) + ItemSpace) * selectionindex);
					}

					if (startRegion < itemRegion && !isUpdateStartPosition)
					{
						if (_start == -1 || _preStart == _start)
						{
							_start = i;
							isUpdateStartPosition = true;
						}
					}
					else if (endRegion < itemRegion)
					{
						_end = i;
						break;
					}
					_end = i;
				}
			}
		}

		/// <summary>
		/// Backward calculation for LinearMode items
		/// </summary>
		/// <param name="isRemove">is Remove</param>
		void AddBackward(bool isRemove)
		{
			_isDisableScrollChange = true;
			double totalCount;
			int itemCount;
			double itemWidth = ItemWidth + ItemSpace;
			double itemHeight = ItemHeight + ItemSpace;

			totalCount = CalculateTotalCount(false);
			UpdateOffsets();
			itemCount = (int)totalCount;

			AdjustStartIndex(itemCount, isRemove);
			if (isRemove)
			{
				AdjustEndIndexOnRemove(itemCount);
			}

			if (Orientation == Orientation.Horizontal)
			{
				_hOffset += itemWidth * itemCount;
			}
			else
			{
				_vOffset += itemHeight * itemCount;
			}
			UpdateItems();
		}

		/// <summary>
		/// Adjusts the start index based on item count and removal flag.
		/// </summary>
		/// <param name="itemCount"></param>
		/// <param name="isRemove"></param>
		void AdjustStartIndex(int itemCount, bool isRemove)
		{
			_preEnd = _end;
			if ((!_isIntermediate && isRemove) || (_isIntermediate && !isRemove))
			{
				_preStart = _start;

				if (_start - itemCount > 0)
				{
					_start -= itemCount;
				}
				else
				{
					_ = itemCount + (_start - itemCount);
					_start = 0;
				}
			}
		}

		/// <summary>
		///  Adjusts the end index when items are removed.
		/// </summary>
		/// <param name="itemCount"></param>
		void AdjustEndIndexOnRemove(int itemCount)
		{
			if (_preStart != 0 && _virtualItemList != null && _tabCollection != null)
			{
				int firstIndex = ((IList)_virtualItemList).IndexOf(((IList)_tabCollection)[0]);
				if (firstIndex == _start && itemCount != 0)
				{
					if ((_preStart - itemCount) == _start || _start == 0)
					{
						_preStart = _start;

						if ((_start - itemCount) < 0)
						{
							itemCount += (_start - itemCount);
						}
					}
					else
					{
						int tempStart = _preStart - itemCount;
						_preStart = _start;
						_start = tempStart;
					}
				}
				_end -= itemCount;
			}
		}

		/// <summary>
		/// Forward calculation for LinearMode items
		/// </summary>
		/// <param name="isRemove">is Remove</param>
		void AddForward(bool isRemove)
		{
			_isDisableScrollChange = true;
			double totalCount;
			int itemCount;
			double itemWidth = ItemWidth + ItemSpace;
			double itemHeight = ItemHeight + ItemSpace;

			totalCount = CalculateTotalCount(true);
			UpdateOffsets();
			itemCount = (int)totalCount;
			AdjustEndIndex(itemCount, isRemove);
			if (isRemove)
			{
				AdjustStartIndexOnRemove(itemCount);
			}

			_hOffset -= itemWidth * itemCount;
			_vOffset -= itemHeight * itemCount;
			UpdateItems();
		}

		/// <summary>
		/// Adjusts the start index when items are removed.
		/// </summary>
		void AdjustStartIndexOnRemove(int itemCount)
		{
			if (_tempCollection != null && _preEnd != ((IList)_tempCollection).Count - 1 && _tabCollection != null)
			{
				int lastIndex = ((IList)_virtualItemList).IndexOf(((IList)_tabCollection)[(_tabCollection as IList).Count - 1]);
				if (lastIndex == _end && itemCount != 0)
				{
					if ((_preEnd + itemCount) == _end || _end == ((IList)_tempCollection).Count - 1)
					{
						_preEnd = _end;
						if ((_end + itemCount) > ((IList)_tempCollection).Count - 1)
						{
							itemCount -= ((_end + itemCount) - (((IList)_tempCollection).Count - 1));
						}
					}
					else
					{
						int tempEnd = _preEnd + itemCount;
						_preEnd = _end;
						_end = tempEnd;
					}
				}
				_start += itemCount;
			}
		}
		/// <summary>
		/// Updates the horizontal and vertical offset values from the ScrollViewer.
		/// </summary>
		void UpdateOffsets()
		{
			if (ScrollViewer != null)
			{
				_hOffset = ScrollViewer.HorizontalOffset;
				_vOffset = ScrollViewer.VerticalOffset;
			}
		}

		/// <summary>
		/// Adjusts the end index based on item count and removal flag.
		/// </summary>
		/// <param name="itemCount"></param>
		/// <param name="isRemove"></param>
		void AdjustEndIndex(int itemCount, bool isRemove)
		{
			_preStart = _start;
			if ((!_isIntermediate && isRemove) || (_isIntermediate && !isRemove))
			{
				_preEnd = _end;
				if (_tempCollection != null)
				{
					if ((_end + itemCount) < ((IList)_tempCollection).Count - 1)
					{
						_end += itemCount;
					}
					else
					{
						_ = itemCount - ((_end + itemCount) - (((IList)_tempCollection).Count - 1));
						_end = ((IList)_tempCollection).Count - 1;
					}
				}
			}
		}

		/// <summary>
		/// Calculates the total count of items based on the scroll direction.
		/// </summary>
		/// <param name="isForward"></param>
		/// <returns></returns>
		double CalculateTotalCount(bool isForward)
		{
			if (ScrollViewer == null)
			{
				return 0;
			}

			if (isForward)
			{
				double delta = Orientation == Orientation.Horizontal
								? ScrollViewer.HorizontalOffset - _preOffset
								: ScrollViewer.VerticalOffset - _preOffset;

				return Math.Truncate(delta / (ItemWidth + ItemSpace));
			}
			else
			{
				double delta = Orientation == Orientation.Horizontal
							   ? _preOffset - ScrollViewer.HorizontalOffset
							   : _preOffset - ScrollViewer.VerticalOffset;

				return Math.Truncate(delta / (ItemWidth + ItemSpace));
			}
		}

		/// <summary>
		/// Update entire position
		/// </summary>
		void UpdateScrollOffsetOnRendering()
		{
			ValidateIndex(new Size() { Width = ActualWidth, Height = ActualHeight });
		}

		#endregion

		#region Scroll Calculation for DefaultMode

		/// <summary>
		/// Calculate first and last visible index
		/// </summary>
		/// <param name="size">pass available carousel size</param>
		void DefaultViewModeIndex(Size size)
		{
			bool isUpdateStartPosition = false;

			if (size.Width == 0)
			{
				return;
			}
			UpdatePreviousIndices();
			double horizontalCenterPoint = size.Width / 2;
			double verticalCenterPoint = size.Height / 2;
			UpdateDefaultStartEndPosition(horizontalCenterPoint, verticalCenterPoint, isUpdateStartPosition);
			UpdateItems();
		}

		/// <summary>
		/// Updates the default start and end positions based on the center points and selection state.
		/// </summary>
		void UpdateDefaultStartEndPosition(double horizontalCenterPoint, double verticalCenterPoint, bool isUpdateStartPosition)
		{
			double hCenter;
			if (_tempCollection != null)
			{
				for (int i = 0; i < ((IList)_tempCollection).Count; i++)
				{
					int selectedIndex = i - SelectedIndex;
					double indexFactor = 0;
					if (selectedIndex < 0)
					{
						indexFactor = -1;
					}
					else if (selectedIndex > 0)
					{
						indexFactor = 1;
					}
					hCenter = (horizontalCenterPoint + ((selectedIndex * Offset) + (SelectedItemOffset * indexFactor))) - (ItemWidth / 2);
					_ = (verticalCenterPoint + ((selectedIndex * Offset) + (SelectedItemOffset * indexFactor))) - (ItemHeight / 2);
					if (Orientation == Orientation.Horizontal)
					{
						double startRegion = -horizontalCenterPoint;
						double endRegion = horizontalCenterPoint + horizontalCenterPoint;
						if (startRegion < hCenter && !isUpdateStartPosition)
						{
							if (_start == -1 || _preStart == _start)
							{
								_start = i;
								isUpdateStartPosition = true;
							}
						}
						else if (endRegion < hCenter)
						{
							_end = i;
							break;
						}
						_end = i;
					}
				}
			}
		}

		/// <summary>
		/// Updates the previous start and end indices 
		/// </summary>
		void UpdatePreviousIndices()
		{
			if (_start != -1 && _end != -1)
			{
				_preStart = _start;
				_preEnd = _end;
			}
		}

		/// <summary>
		/// Reset previous start and end indices
		/// </summary>
		internal void ResetPreviousIndices()
		{
			if (_start != -1 && _end != -1 && _preStart != -1 && _preEnd != -1)
			{
				_preStart = _preEnd = -1;
			}
		}

		#endregion

		#endregion

		#region Internal Methods

		/// <summary>
		/// Updates the item source for the carousel based on the provided virtual view.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="virtualView"></param>
		internal void UpdateItemSource(CarouselHandler handler, ICarousel virtualView)
		{
			if (virtualView != null && virtualView.ItemsSource != null)
			{
				if (virtualView.ItemWidth > 0 && virtualView.ItemHeight > 0)
				{
					_carouselHandler = handler;
					int i = 0;
					if (virtualView.ItemsSource != null)
					{
						if (!AllowLoadMore && !EnableVirtualization)
						{
							foreach (PlatformCarouselItem item in Items.Cast<PlatformCarouselItem>())
							{
								item.Content = null;
							}

							Items.Clear();
							foreach (var item in virtualView.ItemsSource)
							{
								View? contentView = null;
								if (virtualView.ItemTemplate != null)
								{
									contentView = GetItemView(virtualView, i);
									i++;
								}
								if (contentView != null)
								{
									Items.Add(GetNativeViewItem(virtualView, contentView, handler));
								}
							}
						}
						else
						{
							if (ItemsSource == null || ItemsSource != virtualView.ItemsSource)
							{
								ItemsSource = virtualView.ItemsSource;
								if (ItemsSource is INotifyCollectionChanged source)
								{
									source.CollectionChanged -= Source_CollectionChanged;
									source.CollectionChanged += Source_CollectionChanged;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Event to refresh ItemsSource
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			OnItemsSourceChanged();
			if (AllowLoadMore)
			{
				RemoveLoadMoreViewFromParent();
				if (_queue.Count > 0)
				{
					CarouselLinearPanel?.AddLoadMoreItem();
					CarouselItemsPanel?.AddLoadMoreItem();
				}
			}
		}

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

			_isSwipeEnded = false;
		}

		/// <summary>
		/// On SwipeEnded.
		/// </summary>
		/// <param name="args">Arguments of SwipeEnded event.</param>
		internal virtual void OnSwipeEnded(EventArgs args)
		{
			_virtualView?.RaiseSwipeEnded(args);

			_isSwipeEnded = true;
		}

		/// <summary>
		/// Sets parent to the template view.
		/// </summary>
		/// <param name="templateLayout"></param>
		/// <param name="formsCarousel"></param>
		private static void SetParentContent(View templateLayout, ICarousel formsCarousel)
		{
			if (templateLayout != null)
			{
				templateLayout.Parent = (Element)formsCarousel;
			}
		}

		/// <summary>
		/// Retrieves a view for the specified item index in the carousel.
		/// </summary>
		/// <param name="FormsCarousel"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		internal static View? GetItemView(ICarousel FormsCarousel, int index)
		{
			if ((FormsCarousel.ItemsSource is System.Collections.IList itemSource && itemSource.Count > 0))
			{
				if (FormsCarousel.ItemTemplate != null)
				{
					Microsoft.Maui.Controls.DataTemplate? template = null;
					View? templateLayout;
					if (FormsCarousel.ItemTemplate is Microsoft.Maui.Controls.DataTemplateSelector)
					{
						if (FormsCarousel.ItemsSource != null && FormsCarousel.ItemTemplate is Microsoft.Maui.Controls.DataTemplateSelector dataTemplateSelector)
						{
							template = dataTemplateSelector.SelectTemplate(FormsCarousel.ItemsSource.ElementAt(index), null);
						}
					}
					else
					{
						template = FormsCarousel.ItemTemplate;
					}

					if (template == null)
					{
						return null;
					}

					var templateInstance = template.CreateContent();
					if (templateInstance is View)
					{
						templateLayout = templateInstance as View;
					}
					else
					{
#if NET10_0_OR_GREATER
						templateLayout = ((View)templateInstance);
#else
						templateLayout = ((ViewCell)templateInstance).View;
#endif
					}

					if (templateLayout == null)
					{
						return null;
					}

					SetParentContent(templateLayout, FormsCarousel);
					if (FormsCarousel.ItemsSource != null && FormsCarousel.ItemsSource.Any())
					{
						if (index < FormsCarousel.ItemsSource.Count())
						{
							templateLayout.BindingContext = FormsCarousel.ItemsSource.ElementAt(index);
						}
					}

					return templateLayout;
				}
				if (FormsCarousel.ItemTemplate == null)
				{
					Label templateLayout = new Label
					{
						VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center,
						HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center
					};
					if (FormsCarousel.ItemsSource != null && FormsCarousel.ItemsSource.Any())
					{
						if (index < FormsCarousel.ItemsSource.Count())
						{
							templateLayout.Text = FormsCarousel.ItemsSource.ElementAt(index).ToString();
						}
					}

					return templateLayout;
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

		/// <summary>
		/// Gets Native Item for Carousel.
		/// </summary>
		internal static PlatformCarouselItem GetNativeViewItem(ICarousel virtualView, View contentView, ICarouselHandler handler)
		{
			PlatformCarouselItem carouselItem = new PlatformCarouselItem() { Width = virtualView.ItemWidth, Height = virtualView.ItemHeight };
			if (handler.MauiContext != null)
			{
				carouselItem.Content = contentView.ToPlatform(handler.MauiContext);
			}

			return carouselItem;
		}

#endregion

	}

	#region SfCarouselPanel

	/// <summary>
	/// Used to group and arrange the collections of <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/>
	/// </summary>
	/// <exclude/>
	/// <remarks>
	/// CarouselPanel is a <see cref="N:Windows.UI.Xaml.Controls.Canvas"/>.<see
	/// href=""/>
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CsWinRT1029:Class not trimming / AOT compatible", Justification = "<Pending>")]
	public partial class SfCarouselPanel : Canvas, IDisposable
	{
		#region Fields

		/// <summary>
		/// Initializes a new instance of the Parent Items Control in private
		/// </summary>
		private PlatformCarousel? _parentItemsControl;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.SfCarouselPanel"/> class.
		/// </summary>
		public SfCarouselPanel()
		{
			Loaded += SfCarouselPanel_Loaded;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets or set the Parent Items Control
		/// </summary>
		internal PlatformCarousel? ParentItemsControl
		{
			get
			{
				if (_parentItemsControl == null)
				{
					_parentItemsControl = ItemsControl.GetItemsOwner(this) as PlatformCarousel;
				}

				return _parentItemsControl;
			}
		}

		/// <summary>
		/// To clear unused objects
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#region Override Methods 

		/// <summary>
		/// Carousel Measure Override Method.
		/// </summary>
		/// <exclude/>
		/// <param name="availableSize">The available Size</param>
		/// <returns>Available size has returned.</returns>
		protected override Size MeasureOverride(Size availableSize)
		{
			double width = 0.0;
			double height = 0.0;

			foreach (UIElement element in Children)
			{
				element.Measure(availableSize);
				height = Math.Max(height, element.DesiredSize.Height);
				width += element.DesiredSize.Width;
			}

			if ((height == 0) && (width == 0) && ((availableSize.Height == double.PositiveInfinity) || (availableSize.Width == double.PositiveInfinity)))
			{
				availableSize.Height = 0;
				availableSize.Width = 0;
				return availableSize;
			}
			else if (width != 0.0 || height != 0.0)
			{
				if (width == 0.0)
				{
					width = availableSize.Width;
				}

				if (height == 0.0)
				{
					height = availableSize.Height;
				}

				return new Size(width, height);
			}
			else
			{
				return availableSize;
			}
		}

		/// <summary>
		/// To remove all the instance which is used in Accordion
		/// </summary>
		/// <exclude/>
		/// <param name="disposing">The disposing</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Loaded -= SfCarouselPanel_Loaded;
				_parentItemsControl = null;
			}
		}
		#endregion

		/// <summary>
		/// Carousel Panel Loaded Method.
		/// </summary>
		/// <param name="sender">object as sender</param>
		/// <param name="e">RoutedEvent Argument as e</param>
		private void SfCarouselPanel_Loaded(object sender, RoutedEventArgs e)
		{
			AddLoadMoreItem();
		}

		/// <summary>
		/// Add Load More item to the carousel.
		/// </summary>
		internal void AddLoadMoreItem()
		{
			if (ParentItemsControl != null)
			{
				ParentItemsControl.CarouselItemsPanel = this;
				if (ParentItemsControl.AllowLoadMore && Children.Count > 0)
				{
					if ((Children.Count > 0 && (((PlatformCarouselItem)Children[^1]).Tag == null))
					|| (((PlatformCarouselItem)Children[^1]).Tag.ToString() != "Load More"))
					{
						if (ParentItemsControl.ItemsSource is IList itemSource && itemSource.Count > Children.Count)
						{
							if (ParentItemsControl.LoadMoreView == null)
							{
								TextBlock textBlock = new TextBlock
								{
									Text = SfCarouselResources.LoadMoreText
								};
								textBlock.SetValue(AutomationProperties.NameProperty, ParentItemsControl.AutomationId + " Load More. Tap to load more items");
								textBlock.SetValue(AutomationProperties.AutomationIdProperty, ParentItemsControl.AutomationId + " Load More");
								Children.Add(new PlatformCarouselItem() { Content = textBlock, Tag = "Load More", ParentItemsControl = ParentItemsControl });
							}
							else
							{
								Children.Add(new PlatformCarouselItem() { Content = ParentItemsControl.LoadMoreView, Tag = "Load More", ParentItemsControl = ParentItemsControl });
							}
						}
					}
				}
				else if (ParentItemsControl.EnableVirtualization)
				{
					ParentItemsControl.ResetPreviousIndices();
					ParentItemsControl.UpdateItems();
				}
			}
		}

		#endregion
	}

	#endregion

	#region SfCarouselLinearPanel

	/// <summary>
	/// Carousel Linear Panel
	/// </summary>
	/// <exclude/>
	public partial class SfCarouselLinearPanel : Panel
	{
		/// <summary>
		/// Initializes a new instance of the  Carousel Control.
		/// </summary>
		private PlatformCarousel? _parentItemsControl;

		/// <summary>
		/// Initializes a new instance of the <see cref="Syncfusion.Maui.Toolkit.Carousel.SfCarouselLinearPanel"/> class.
		/// </summary>
		public SfCarouselLinearPanel()
		{
			Loaded += SfCarouselLinearPane_Loaded;
		}

		#region Methods

		/// <summary>
		/// Gets or set Parent Items Control
		/// </summary>
		internal PlatformCarousel? ParentItemsControl
		{
			get
			{
				if (_parentItemsControl == null)
				{
					_parentItemsControl = ItemsControl.GetItemsOwner(this) as PlatformCarousel;
				}

				return _parentItemsControl;
			}
		}

		/// <summary>
		/// Carousel Measure Override Method.
		/// </summary>
		/// <exclude/>
		/// <param name="availableSize">The available size</param>
		/// <returns>available size has returned.</returns>
		protected override Size MeasureOverride(Size availableSize)
		{
			double width = 0.0;
			double height = 0.0;

			foreach (UIElement element in Children)
			{
				PlatformCarouselItem? item = element as PlatformCarouselItem;
				if (item != null)
				{
					if (ParentItemsControl != null && ParentItemsControl.EnableVirtualization)
					{
						(item as PlatformCarouselItem).Height = ParentItemsControl.ItemHeight;
						(item as PlatformCarouselItem).Width = ParentItemsControl.ItemWidth;
					}

					element.Measure(availableSize);
					if (ParentItemsControl != null)
					{
						if (ParentItemsControl.Orientation == Orientation.Horizontal)
						{
							height = element.DesiredSize.Height;
							width += element.DesiredSize.Width + ParentItemsControl.ItemSpace;
						}
						else
						{
							width = element.DesiredSize.Width;
							height += element.DesiredSize.Height + ParentItemsControl.ItemSpace;
						}
					}
				}
			}
			return FinalizeSize(availableSize, width, height);
		}

		/// <summary>
		/// Determines the final size of the element based on available size and specified dimensions.
		/// </summary>
		/// <param name="availableSize"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		Size FinalizeSize(Size availableSize, double width, double height)
		{
			if (width != 0.0 || height != 0.0 || (Children != null && Children.Count == 0))
			{
				if (Children.Count > 0)
				{
					if (width == 0.0)
					{
						width = availableSize.Width;
					}

					if (height == 0.0)
					{
						height = availableSize.Height;
					}
				}

				return new Size(width, height);
			}
			else
			{
				return availableSize;
			}
		}

		/// <summary>
		/// Arrange Override method
		/// </summary>
		/// <exclude/>
		/// <param name="finalSize"> final size of the carousel</param>
		/// <returns>final size has returned</returns>
		protected override Size ArrangeOverride(Size finalSize)
		{
			ArrangeChildItems(finalSize);

			if (ParentItemsControl != null && ParentItemsControl.ViewMode == ViewMode.Linear && ParentItemsControl.EnableVirtualization)
			{
				if (ParentItemsControl.SelectedIndex != -1 && !ParentItemsControl._isDisableScrollChange)
				{
					if (ParentItemsControl._virtualItemList is IList virtualItemList && virtualItemList.Count > 0 && virtualItemList[ParentItemsControl.SelectedIndex] is PlatformCarouselItem item)
					{
						if (ParentItemsControl.Orientation == Orientation.Horizontal)
						{
							ParentItemsControl.MoveHorizontalItem(item, true);
						}
						else
						{
							ParentItemsControl.MoveVerticalItem(item, true);
						}
					}
				}
				else if (ParentItemsControl._isDisableScrollChange && !ParentItemsControl._isIntermediate)
				{
					ParentItemsControl.UpdateScroll();
				}
			}
			return finalSize;
		}

		/// <summary>
		/// Arranges child elements within the specified final size.
		/// </summary>
		private void ArrangeChildItems(Size finalSize)
		{
			double x = 0.0;
			double y = 0.0;
			foreach (UIElement element in Children)
			{
				element.Measure(finalSize);
				int index = Children.IndexOf(element);
				if (ParentItemsControl != null)
				{
					((PlatformCarouselItem)element).Arrange(0, 0, 0, 0, 1, TimeSpan.FromMilliseconds(0), ParentItemsControl.EasingFunction, true, false);
				}

				if (ParentItemsControl != null && ParentItemsControl.Orientation == Orientation.Horizontal)
				{
					if (element.DesiredSize.Height != 0.0 && element.DesiredSize.Height < finalSize.Height)
					{
						y = (finalSize.Height / 2) - (element.DesiredSize.Height / 2);
					}

					element.Arrange(new Rect(x, y, element.DesiredSize.Width, element.DesiredSize.Height));
					element.UpdateLayout();
					x += element.DesiredSize.Width + ParentItemsControl.ItemSpace;
				}
				else
				{
					if (element.DesiredSize.Width != 0.0 && element.DesiredSize.Width < finalSize.Width)
					{
						x = (finalSize.Width / 2) - (element.DesiredSize.Width / 2);
					}

					element.Arrange(new Rect(x, y, element.DesiredSize.Width, element.DesiredSize.Height));
					if (ParentItemsControl != null)
					{
						y += element.DesiredSize.Height + ParentItemsControl.ItemSpace;
					}
				}
				AutomationProperties.SetName(element, ((PlatformCarouselItem)element).AutomationId + " PlatformCarouselItem " + (index + 1) + " of " + Children.Count);
				AutomationProperties.SetAutomationId(element, ((PlatformCarouselItem)element).AutomationId + " PlatformCarouselItem " + (index + 1) + " of " + Children.Count);
			}
		}

		/// <summary>
		/// Carousel Linear Panel Loaded Method. 
		/// </summary>
		/// <param name="sender">object as sender.</param>
		/// <param name="e">RoutedEvent Argument as e.</param>
		void SfCarouselLinearPane_Loaded(object sender, RoutedEventArgs e)
		{
			if (ParentItemsControl != null)
			{
				ParentItemsControl.CarouselLinearPanel = this;
				if (ParentItemsControl.SelectedIndex != -1)
				{
					if (ParentItemsControl.Orientation == Orientation.Horizontal)
					{
						ParentItemsControl.MoveHorizontalItem(((PlatformCarouselItem)ParentItemsControl.ContainerFromIndex(ParentItemsControl.SelectedIndex)), false);
					}
					else
					{
						ParentItemsControl.MoveVerticalItem(((PlatformCarouselItem)ParentItemsControl.ContainerFromIndex(ParentItemsControl.SelectedIndex)), false);
					}
				}
			}
			AddLoadMoreItem();
		}

		/// <summary>
		/// Add Load More item to the carousel.
		/// </summary>
		internal void AddLoadMoreItem()
		{
			if (ParentItemsControl != null)
			{
				if (ParentItemsControl.AllowLoadMore && Children.Count > 0)
				{
					if (((Children.Count > 0) && (((PlatformCarouselItem)Children[^1]).Tag == null))
					  || ((PlatformCarouselItem)Children[^1]).Tag.ToString() != "Load More")
					{
						if (((IList)ParentItemsControl.ItemsSource).Count > Children.Count)
						{
							if (ParentItemsControl.LoadMoreView == null)
							{
								TextBlock textBlock = new TextBlock
								{
									Text = SfCarouselResources.LoadMoreText
								};
								textBlock.SetValue(AutomationProperties.NameProperty, ParentItemsControl.AutomationId + " Load More. Tap to load more items");
								textBlock.SetValue(AutomationProperties.AutomationIdProperty, ParentItemsControl.AutomationId + " Load More");
								Children.Add(new PlatformCarouselItem() { Content = textBlock, Tag = "Load More", ParentItemsControl = ParentItemsControl });
							}
							else
							{
								Children.Add(new PlatformCarouselItem() { Content = ParentItemsControl.LoadMoreView, Tag = "Load More", ParentItemsControl = ParentItemsControl });
							}
						}
					}
				}
				else if (ParentItemsControl.EnableVirtualization)
				{
					ParentItemsControl.ResetPreviousIndices();
					ParentItemsControl.UpdateItems();
				}
			}
		}

		#endregion
	}

	#endregion

	#region OrientationToBoolean 

	/// <summary>
	/// inherit OrientationToBoolean
	/// </summary>
	/// <exclude/>
	public class OrientationToBoolean : IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is Orientation orientation)
			{
				return orientation == Orientation.Vertical;
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion
}