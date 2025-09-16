using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using ContentView = Microsoft.Maui.Controls.ContentView;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.TabView
{

	/// <summary>
	/// Represents the SfHorizontalStackLayout class.
	/// </summary>
	internal partial class SfHorizontalContent : SfTabViewExt, ITouchListener, ITapGestureListener
	{
		#region Fields

		SfHorizontalStackLayout? _horizontalStackLayout;
		SfGrid? _parentGrid;
		double _contentWidth;
		readonly SfTabView? _tabView;
		readonly SfTabBar? _tabBar;
		bool _isPressed;
		bool _isMoved;
		Point _oldPoint;
		Point _newPoint;
		DateTime _startTime;
		Point _startPoint;
		double _velocityX;
		double _initialXPosition;
		bool? _isTowardsRight;
		int _visibleItemCount;
		int _currentIndex;
		int _previousVisibleIndex;
		int _nextVisibleIndex;
		bool _isPreviousItemVisible;
		bool _isNextItemVisible;
		SelectionChangingEventArgs? _selectionChangingEventArgs;
		bool _isSelectionProcessed;
#if WINDOWS
		bool _isItemRemoved;
#endif

#if IOS || MACCATALYST
		readonly SfHorizontalContentProxy _proxy;
#endif

		#endregion

		#region Bindable properties
		/// <summary>
		/// Identifies the <see cref="Items"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ItemsProperty =
		   BindableProperty.Create(
			   nameof(Items),
			   typeof(TabItemCollection),
			   typeof(SfHorizontalContent),
			   null,
			   propertyChanged: OnItemsChanged);

		/// <summary>
		/// Identifies the <see cref="SelectedIndex"/> bindable property.
		/// </summary>
		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(
				nameof(SelectedIndex),
				typeof(int),
				typeof(SfHorizontalContent),
				-1, BindingMode.TwoWay,
				propertyChanged: OnSelectedIndexChanged);

		/// <summary>
		/// Identifies the <see cref="ContentItemTemplate"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty ContentItemTemplateProperty =
		 BindableProperty.Create(
			 nameof(ContentItemTemplate),
			 typeof(DataTemplate),
			 typeof(SfHorizontalContent),
			 null,
			 propertyChanged: OnContentItemTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="ItemsSource"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(
				nameof(ItemsSource),
				typeof(IList),
				typeof(SfHorizontalContent),
				null,
				propertyChanged: OnItemsSourceChanged);

		/// <summary>
		/// Identifies the <see cref="ContentTransitionDuration"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ContentTransitionDuration"/> bindable property.
		/// </value>
		internal static readonly BindableProperty ContentTransitionDurationProperty =
		   BindableProperty.Create(
			   nameof(ContentTransitionDuration),
			   typeof(double),
			   typeof(SfHorizontalContent),
			   100d);

		/// <summary>
		/// Identifies the <see cref="EnableVirtualization"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="EnableVirtualization"/> bindable property.
		/// </value>
		internal static readonly BindableProperty EnableVirtualizationProperty =
		   BindableProperty.Create(
			   nameof(EnableVirtualization),
			   typeof(bool),
			   typeof(SfHorizontalContent),
			   false);

		/// <summary>
		/// Identifies the <see cref="AnimationEasing"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="AnimationEasing"/> bindable property.
		/// </value>
		internal static readonly BindableProperty AnimationEasingProperty =
		   BindableProperty.Create(
			   nameof(AnimationEasing),
			   typeof(Easing),
			   typeof(SfHorizontalContent),
			   Easing.Linear);
		
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value that can be used to modify the duration of content transition in tab View.
		/// </summary>
		/// <value>
		/// It accepts the double values and the default value is 100.
		/// </value>
		internal double ContentTransitionDuration
		{
			get => (double)GetValue(ContentTransitionDurationProperty);
			set => SetValue(ContentTransitionDurationProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that defines the collection of items.
		/// </summary>
		public TabItemCollection Items
		{
			get => (TabItemCollection)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that defines the selected index.
		/// </summary>
		public int SelectedIndex
		{
			get => (int)GetValue(SelectedIndexProperty);
			set => SetValue(SelectedIndexProperty, value);
		}

		/// <summary>
		/// Gets or sets the data template used to define the visual structure of the content items in the tab control.
		/// </summary>
		internal DataTemplate? ContentItemTemplate
		{
			get => (DataTemplate?)GetValue(ContentItemTemplateProperty);
			set => SetValue(ContentItemTemplateProperty, value);
		}

		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		internal IList? ItemsSource
		{
			get => (IList?)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether lazy loading is enabled during the initial load.
		/// </summary>
		/// <value>
		/// It accepts the bool values and the default value is false.
		/// </value>
		internal bool EnableVirtualization
		{
			get => (bool)GetValue(EnableVirtualizationProperty);
			set => SetValue(EnableVirtualizationProperty, value);
		}
		
		/// <summary>
		/// Gets or sets the easing function used for content transition animations.
		/// </summary>
		/// <value>
		/// An <see cref="Easing"/> function that controls the content transition animation. The default value is <see cref="Easing.Linear"/>.
		/// </value>
		internal Easing AnimationEasing
		{
			get => (Easing)GetValue(AnimationEasingProperty);
			set => SetValue(AnimationEasingProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that defines the content width of each tab header item.
		/// </summary>
		internal double ContentWidth
		{
			get
			{
				return _contentWidth;
			}
			set
			{
				if (_contentWidth != value)
				{
					_contentWidth = value;
					UpdateTabItemContentSize();
					UpdateTabItemContentPosition();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the tab content moves toward the right or left.
		/// </summary>
		bool? IsTowardsRight
		{
			get { return _isTowardsRight; }
			set
			{
				if (_isTowardsRight != value)
				{
					_isTowardsRight = value;
					if (_isTowardsRight != null)
					{
						_selectionChangingEventArgs = new SelectionChangingEventArgs();
						RaiseSelectionChanging();
					}
				}
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfHorizontalContent"/> class.
		/// </summary>
		public SfHorizontalContent(SfTabView sfTabView, SfTabBar sfTabBar)
		{
			_tabView = sfTabView;
			_tabBar = sfTabBar;
			Style = new Style(typeof(View));
			ClipToBounds = true;
			InitializeControl();
			this.AddTouchListener(this);
#if ANDROID
			this.SetValue(AutomationProperties.IsInAccessibleTreeProperty, false);
#endif

#if IOS || MACCATALYST
			this.AddGestureListener(this);
			_proxy = new(this);
#endif
			_selectionChangingEventArgs = new SelectionChangingEventArgs();
		}

		#endregion

		#region Override Methods
#if IOS || MACCATALYST
		protected override Microsoft.Maui.Graphics.Size ArrangeContent(Microsoft.Maui.Graphics.Rect bounds)
		{
			if (MinimumHeightRequest != bounds.Height || MinimumWidthRequest != bounds.Width)
			{
				if (bounds.Height > 0)
				{
					MinimumHeightRequest = bounds.Height;
				}
				MinimumWidthRequest = bounds.Width;
				ContentWidth = bounds.Width;
				UpdateTabItemContentSize();
				UpdateTabItemContentPosition();
			}
			return base.ArrangeContent(bounds);
		}
#endif

#if ANDROID || WINDOWS
		/// <summary>
		/// Measure Override method.
		/// </summary>
		/// <param name="widthConstraint">The available width constraint for the control.</param>
		/// <param name="heightConstraint">The available height constraint for the control.</param>
		/// <returns>It returns the size</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			if (widthConstraint > 0 && widthConstraint != double.PositiveInfinity )
			{
				ContentWidth = widthConstraint;
				UpdateTabItemContentSize();
				UpdateTabItemContentPosition();
				_tabBar?.UpdateTabIndicatorWidth();
			}
			if (heightConstraint > 0 && heightConstraint != double.PositiveInfinity )
			{
				UpdateTabItemContentSize();
				UpdateTabItemContentPosition();
				_tabBar?.UpdateTabIndicatorWidth();
			}

			return base.MeasureContent(widthConstraint, heightConstraint);
		}
#endif

#if !WINDOWS
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName == "FlowDirection")
			{
				UpdateTabItemContentPosition();
			}
			base.OnPropertyChanged(propertyName);
		}
#endif

		protected override void OnHandlerChanged()
		{
#if WINDOWS || IOS || MACCATALYST
			ConfigureTouch();
#endif
			base.OnHandlerChanged();
		}

		#endregion

		#region Private Methods

		void InitializeControl()
		{
			Items = [];
			_parentGrid = new SfGrid()
			{
				IsClippedToBounds = true, // Ensuring content bounds are clipped within the grid
				Style = new Style(typeof(SfGrid)),
				VerticalOptions = LayoutOptions.Fill, // Fill the available vertical space
				HorizontalOptions = LayoutOptions.Fill, // Fill the available horizontal space
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

			_horizontalStackLayout = new SfHorizontalStackLayout()
			{
				IsClippedToBounds = true, // Ensuring content bounds are clipped within the stack layout
				Style = new Style(typeof(SfHorizontalStackLayout)),
				VerticalOptions = LayoutOptions.Fill, // Fill the available vertical space
				HorizontalOptions = LayoutOptions.Start, // Align horizontally at the start
				Spacing = 0,
			};

			_parentGrid.Children.Add(_horizontalStackLayout);
			Content = _parentGrid;
			if (Items != null)
			{
				Items.CollectionChanged -= OnItemsCollectionChanged;
				Items.CollectionChanged += OnItemsCollectionChanged;
			}
		}

		void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (SfTabItem tabItem in e.OldItems)
				{
					ClearTabContent(tabItem, e.OldStartingIndex);
				}
			}

			if (e.NewItems != null && Items != null)
			{
				foreach (SfTabItem tabItem in e.NewItems)
				{
					var index = Items.IndexOf(tabItem);
					AddTabContentItems(tabItem, index);
				}
			}
			else
			{
				ClearItems();
			}
		}

		/// <summary>
		/// Invokes <see cref="SelectionChanging"/> event.
		/// </summary>
		/// <param name="args">args.</param>
		void RaiseSelectionChangingEvent(SelectionChangingEventArgs args)
		{
			SelectionChanging?.Invoke(this, args);
		}

		void ClearItems()
		{
			if (Items == null || Items.Count == 0)
			{
				_horizontalStackLayout?.Children.Clear();
			}
		}

		void ClearTabContent(SfTabItem tabItem, int index)
		{
			if (tabItem != null)
			{
				_horizontalStackLayout?.Children.RemoveAt(index);
			}
#if WINDOWS
			_isItemRemoved = true;
			UpdateDynamicChanges();
#endif
		}

		void UpdateSelectedIndex()
		{
			_isSelectionProcessed = true;
			if (EnableVirtualization)
			{
				LoadItemsContent(SelectedIndex);
			}

			UpdateTabItemContentPosition();
		}

		void UpdateItems()
		{
			int count = -1;

			if (Items != null)
			{
				_horizontalStackLayout?.Children.Clear();
				foreach (var item in Items)
				{
					if (item != null)
					{
						count++;
						AddTabContentItems(item, count);
					}
				}

				Items.CollectionChanged -= OnItemsCollectionChanged;
				Items.CollectionChanged += OnItemsCollectionChanged;
			}
			else
			{
				ClearItems();
			}
		}

		void UpdateItemsSource()
		{
			if (_horizontalStackLayout != null && ItemsSource != null)
			{
				BindableLayout.SetItemsSource(_horizontalStackLayout, ItemsSource);
			}

			if (ItemsSource != null && ContentItemTemplate != null)
			{
				if (ItemsSource is INotifyCollectionChanged oldNotifyTabItemsSource && oldNotifyTabItemsSource != null)
				{
					oldNotifyTabItemsSource.CollectionChanged -= OnTabItemsSourceCollectionChanged;
				}

				if (ItemsSource is INotifyCollectionChanged newNotifyTabItemsSource && newNotifyTabItemsSource != null)
				{
					newNotifyTabItemsSource.CollectionChanged += OnTabItemsSourceCollectionChanged;
				}

				if (_horizontalStackLayout != null && ItemsSource != null)
				{
					BindableLayout.SetItemTemplate(_horizontalStackLayout, ContentItemTemplate);
				}
			}

			// Update sizes and positions of tab items after updating the source
			UpdateDynamicChanges();
		}

		void OnTabItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateDynamicChanges();
		}

		void UpdateDynamicChanges()
		{
			UpdateTabItemContentSize();
			UpdateTabItemContentPosition();
		}

		void AddTabContentItems(SfTabItem item, int index = -1)
		{
			if (item != null)
			{
				if (item.Content != null)
				{
					SfGrid parentGrid = new();

					if (!EnableVirtualization || (EnableVirtualization && !(index != SelectedIndex && item.IsVisible)))
					{
						parentGrid = CreateParentGrid(item);
					}

					if (index >= 0)
					{
						if (EnableVirtualization && index != SelectedIndex && item.IsVisible)
						{
							var content = new BoxView
							{
								WidthRequest = 0,
								HeightRequest = 0,
								Opacity = 0
							};
							_horizontalStackLayout?.Children.Insert(index, content);
						}
						else
						{
							_horizontalStackLayout?.Children.Insert(index, parentGrid);
						}
					}
					else
					{
						_horizontalStackLayout?.Children.Add(parentGrid);
					}
				}
				else
				{
					SfGrid parentGrid = [];
					parentGrid.SetBinding(SfGrid.IsVisibleProperty, BindingHelper.CreateBinding(nameof(SfTabItem.IsVisible), getter: static (SfTabItem item)=> item.IsVisible, source: item));
					if (index >= 0)
					{
						_horizontalStackLayout?.Children.Insert(index, parentGrid);
					}
					else
					{
						_horizontalStackLayout?.Children.Add(parentGrid);
					}
				}

				item.PropertyChanged -= OnTabItemPropertyChanged;
				item.PropertyChanged += OnTabItemPropertyChanged;
			}

			if (SelectedIndex == -1 && Items?.Count > 0)
			{
				SelectedIndex = 0;
			}

			UpdateDynamicChanges();
		}

		SfGrid CreateParentGrid(SfTabItem item)
		{
			SfGrid parentGrid = new SfGrid()
			{
				IsClippedToBounds = true, // Ensuring content bounds are clipped within the grid
				VerticalOptions = LayoutOptions.Fill, // Fill the available vertical space
				HorizontalOptions = LayoutOptions.Fill, // Fill the available horizontal space
				Style = new Style(typeof(SfGrid)),
				ColumnSpacing = 0,
				RowSpacing = 0
			};
			parentGrid.Children.Add(item.Content);
			parentGrid.SetBinding(Grid.IsVisibleProperty, BindingHelper.CreateBinding(nameof(SfTabItem.IsVisible), getter: static (SfTabItem item) => item.IsVisible, source: item));
			return parentGrid;
		}

		void OnTabItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.PropertyName))
			{
				if (e.PropertyName.Equals(nameof(SfTabItem.Content), StringComparison.Ordinal))
				{
					if (sender is SfTabItem item && Items != null)
					{
						UpdateOnTabItemContentChanged(item);
					}
				}

				if (e.PropertyName.Equals(nameof(SfTabItem.IsVisible), StringComparison.Ordinal))
				{
					UpdateTabItemContentSize();
					UpdateTabItemContentPosition();
				}
			}
		}

		void UpdateOnTabItemContentChanged(SfTabItem item)
		{
			int index = Items.IndexOf(item);
			if (_horizontalStackLayout != null)
			{
				SfGrid parentGrid = (SfGrid)_horizontalStackLayout.Children[index];
				if (parentGrid != null)
				{
					parentGrid.Children.Clear();
					parentGrid.Children.Add(item.Content);
				}
			}
		}

		void UpdateTabItemContentSize()
		{
			if (_horizontalStackLayout != null)
			{
				if (ContentWidth > 0)
				{
					_horizontalStackLayout.WidthRequest = ContentWidth * GetCountVisibleItems();
				}
				foreach (var item in _horizontalStackLayout.Children)
				{
					if (item != null)
					{
						if (item is View child && child != null)
						{
							if (ContentWidth > 0)
							{
								child.WidthRequest = ContentWidth;
							}
							if (HeightRequest > 0)
							{
								child.HeightRequest = HeightRequest;
							}
						}
					}
				}
			}
		}

		void UpdateTabItemContentPosition()
		{
			if (ContentWidth > 0 && SelectedIndex >= 0)
			{
				if (ContentItemTemplate != null)
				{
					UpdateTabItemContentPositionWithTemplate();
				}
				else
				{
					UpdateTabItemContentPositionWithoutTemplate();
				}
			}
		}

		void UpdateTabItemContentPositionWithTemplate()
		{
			double xPosition = 0;

			if (SelectedIndex < _horizontalStackLayout?.Children.Count)
			{
				if (SelectedIndex >= 0)
				{
					xPosition = ContentWidth * SelectedIndex;
#if WINDOWS
					if (_isSelectionProcessed)
					{
						_horizontalStackLayout?.TranslateTo(-xPosition, 0, (uint)ContentTransitionDuration, AnimationEasing);
						_isSelectionProcessed = false;
					}
					else
					{
						if (_horizontalStackLayout is not null)
						{
							_horizontalStackLayout.TranslationX = -xPosition;
						}
					}
#else
					double targetX = ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) is not EffectiveFlowDirection.RightToLeft ? -xPosition : xPosition;
					if (_isSelectionProcessed)
					{
						_horizontalStackLayout?.TranslateTo(targetX, 0, (uint)ContentTransitionDuration, AnimationEasing);
						_isSelectionProcessed = false;
					}
					else
					{
						if (_horizontalStackLayout is not null)
							_horizontalStackLayout.TranslationX = targetX;
					}
#endif
				}
			}
		}

		void UpdateTabItemContentPositionWithoutTemplate()
		{
			double xPosition = ContentWidth * GetVisibleItemsCount();
			if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
			{
#if WINDOWS
				if (_isItemRemoved)
				{
					if (_horizontalStackLayout != null)
					{
						_horizontalStackLayout.TranslationX = -xPosition;
					}

					_isItemRemoved = false;
				}
				else
				{
					if (_isSelectionProcessed)
					{
						_horizontalStackLayout?.TranslateTo(-xPosition, 0, (uint)ContentTransitionDuration, AnimationEasing);
						_isSelectionProcessed = false;
					}
					else
					{
						if (_horizontalStackLayout is not null)
						{
							_horizontalStackLayout.TranslationX = -xPosition;
						}
					}
				}
#else
				double targetX = ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) is not EffectiveFlowDirection.RightToLeft ? -xPosition : xPosition;
				if (_isSelectionProcessed)
				{
					_horizontalStackLayout?.TranslateTo(targetX, 0, (uint)ContentTransitionDuration, AnimationEasing);
					_isSelectionProcessed = false;
				}
				else
				{
					if (_horizontalStackLayout is not null)
						_horizontalStackLayout.TranslationX = targetX;
				}
#endif
			}

		}

		int GetCountVisibleItems()
		{
			int count = 0;
			if (Items != null)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if (Items[i] != null && Items[i].IsVisible)
					{
						count++;
					}
				}
			}
			if (count == 0 && ContentItemTemplate != null)
			{
				count = _horizontalStackLayout?.Children.Count ?? 0;
			}
			return count;
		}

		int GetVisibleItemsCount()
		{
			int count = 0;
			if (Items != null)
			{
				for (int i = 0; i < SelectedIndex; i++)
				{
					if (Items.Count > i && Items[i] != null && Items[i].IsVisible)
					{
						count++;
					}
				}
			}

			return count;
		}

		/// <summary>
		/// This method used for handle the touch.
		/// </summary>
		/// <param name="e">The pointer event arguments.</param>
		public void OnTouch(PointerEventArgs e)
		{
			// This method is intentionally left empty as it is required by interface
		}

		/// <summary>
		/// This method used for handle the tap.
		/// </summary>
		public void OnTap(TapEventArgs e)
		{
			// This method is intentionally left empty as it is required by interface
		}

		/// <summary>
		/// This method used for handle the touch interaction.
		/// </summary>
		/// <param name="action">The pointer action.</param>
		/// <param name="point">The point value.</param>
		public void OnHandleTouchInteraction(PointerActions action, Point point)
		{
			if (_tabView != null && _tabView.EnableSwiping)
			{
				switch (action)
				{
					case PointerActions.Pressed:
						InitializeTouchData(point);
						break;

					case PointerActions.Moved:
						if (_isPressed)
						{
							HandleTouchMovement(point);
						}
						break;

					case PointerActions.Released:
						HandleTouchReleased();
						break;
				}
			}
		}

		void HandleTouchReleased()
		{
			if (_isPressed && _isMoved)
			{
				if (_visibleItemCount > 1 && _horizontalStackLayout != null &&
					_horizontalStackLayout.Children.Count > 1)
				{
					OnTabSwipeCompleted();
				}
			}
			IsTowardsRight = null;
			_isPressed = _isMoved = false;
		}

		/// <summary>
		/// Initializes touch data based on the provided pointer event arguments.
		/// </summary>
		/// <param name="point">point.</param>
		void InitializeTouchData(Point point)
		{
			_isPressed = true;
			_isNextItemVisible = false;
			_isPreviousItemVisible = false;
			_oldPoint = _startPoint = point;
			_startTime = DateTime.Now;
			_visibleItemCount = GetCountVisibleItems();
			if (ContentItemTemplate != null && _tabBar != null)
			{
				_currentIndex = _tabBar.SelectedIndex;
			}
			else
			{
				_currentIndex = GetVisibleItemsCount();
			}

#if !WINDOWS
			if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
			{
				_initialXPosition = -ContentWidth * _currentIndex;
			}
			else
			{
				_initialXPosition = ContentWidth * _currentIndex;
			}
#else
			_initialXPosition = -ContentWidth * _currentIndex;
#endif
		}

		/// <summary>
		/// This method is used to handle the touch movement.
		/// </summary>
		/// <param name="point">The touch point.</param>
		void HandleTouchMovement(Point point)
		{
			_isMoved = true;
			_newPoint = point;
			var difference = _newPoint.X - _oldPoint.X;
			UpdateDirection(difference);

			FindVelocity(point);
			if (_visibleItemCount > 1 && _horizontalStackLayout != null &&
				_horizontalStackLayout.Children.Count > 1)
			{
				UpdateItemVisibility();
				TranslateXPosition(difference);
			}

			_oldPoint = _newPoint;
		}

		void UpdateDirection(double difference)
		{
			if (_horizontalStackLayout != null)
			{
				var translateXPosition = _horizontalStackLayout.TranslationX + difference;
				if (difference != 0)
				{
#if !WINDOWS
					if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
					{
						IsTowardsRight = _initialXPosition > translateXPosition;
					}
					else
					{
						IsTowardsRight = _initialXPosition < translateXPosition;
					}
#else
					IsTowardsRight = _initialXPosition > translateXPosition;
#endif
				}
			}
		}

		/// <summary>
		/// This method is used to update the horizontal transition (X position) of the horizontal stack layout.
		/// </summary>
		/// <param name="difference">The position difference.</param>
		void TranslateXPosition(double difference)
		{
			if (_horizontalStackLayout != null && _tabBar != null)
			{
				if (_selectionChangingEventArgs != null && !_selectionChangingEventArgs.Cancel)
				{
					if (_tabBar.SelectedIndex == 0)
					{
						AdjustForFirstIndex(difference);
					}
					else if (_tabBar.SelectedIndex > 0 && _tabBar.SelectedIndex < _horizontalStackLayout.Children.Count - 1)
					{
						AdjustForMiddleIndices(difference);
					}
					else if (_tabBar.SelectedIndex == _horizontalStackLayout.Children.Count - 1)
					{
						AdjustForLastIndex(difference);
					}
				}
			}
		}

		void AdjustForFirstIndex(double difference)
		{
			if (_horizontalStackLayout != null && IsTowardsRight == true)
			{
				if (EnableVirtualization)
				{
					var index = _nextVisibleIndex;
					LoadItemsContent(index);
				}

#if !WINDOWS
				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
				{
					_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth, 0);
				}
				else
				{
					_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, 0, ContentWidth);
				}
#else
				_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth, 0);
#endif
			}
		}

		void AdjustForMiddleIndices(double difference)
		{
			if (_horizontalStackLayout != null)
			{
				if (_isPreviousItemVisible && _isNextItemVisible)
				{
					if (EnableVirtualization)
					{
						int index = -1;
						if (IsTowardsRight is not null)
						{
							index = IsTowardsRight == true ? _nextVisibleIndex : _previousVisibleIndex;
						}

						LoadItemsContent(index);
					}

#if !WINDOWS
					if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
					{
						_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * (_currentIndex + 1), -ContentWidth * (_currentIndex - 1));
					}
					else
					{
						_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, ContentWidth * (_currentIndex - 1), ContentWidth * (_currentIndex + 1));
					}
#else
					_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * (_currentIndex + 1), -ContentWidth * (_currentIndex - 1));
#endif
				}
				else if (_isPreviousItemVisible)
				{
					if (EnableVirtualization)
					{
						var index = _previousVisibleIndex;
						LoadItemsContent(index);
					}

#if !WINDOWS
					if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
					{
						_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * _currentIndex, -ContentWidth * (_currentIndex - 1));
					}
					else
					{
						_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, ContentWidth * (_currentIndex - 1), ContentWidth * _currentIndex);
					}
#else
					_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * _currentIndex, -ContentWidth * (_currentIndex - 1));
#endif
				}
				else if (_isNextItemVisible)
				{
					if (EnableVirtualization)
					{
						var index = _nextVisibleIndex;
						LoadItemsContent(index);
					}

#if !WINDOWS
					if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
					{
						_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * (_currentIndex + 1), -ContentWidth * _currentIndex);
					}
					else
					{
						_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, ContentWidth * _currentIndex, ContentWidth * (_currentIndex + 1));
					}
#else
					_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * (_currentIndex + 1), -ContentWidth * _currentIndex);
#endif
				}
			}
		}

		void AdjustForLastIndex(double difference)
		{
			if (_horizontalStackLayout != null)
			{
				if (EnableVirtualization)
				{
					var index = _previousVisibleIndex;
					if (IsTowardsRight is false)
					{
						LoadItemsContent(index);
					}
				}

#if !WINDOWS
				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
				{
					_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * (_visibleItemCount - 1), -ContentWidth * (_visibleItemCount - 2));
				}
				else
				{
					_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, ContentWidth * (_visibleItemCount - 2), ContentWidth * (_visibleItemCount - 1));
				}
#else
				_horizontalStackLayout.TranslationX = Math.Clamp(_horizontalStackLayout.TranslationX + difference, -ContentWidth * (_visibleItemCount - 1), -ContentWidth * (_visibleItemCount - 2));
#endif
			}
		}

		/// <summary>
		/// This method is used to replace the placeholder view with a tab item when the EnableVirtualization is true.
		/// </summary>
		/// <param name="index">This index position of the child in the HorizontalStackLayout to replaced with a tab item.</param>
		void LoadItemsContent(int index)
		{
			if (_horizontalStackLayout != null)
			{
				if (Items is not null && index >= 0 && index < _horizontalStackLayout.Children.Count && _horizontalStackLayout.Children[index] is BoxView)
				{
					SfGrid parentGrid = CreateParentGrid(Items[index]);
					_horizontalStackLayout?.Children.RemoveAt(index);
					_horizontalStackLayout?.Children.Insert(index, parentGrid);
					UpdateDynamicChanges();
				}
			}
		}

		/// <summary>
		/// This method is used to update the tab item position after swipe complete.
		/// </summary>
		void OnTabSwipeCompleted()
		{
			if (_horizontalStackLayout != null && _tabBar != null)
			{
				double velocityThreshold = 500;
				double translationThreshold = 0.75;

				if (_velocityX < -velocityThreshold)
				{
					HandleBackwardSwipeVelocity();
				}
				else if (_velocityX >= velocityThreshold)
				{
					HandleForwardSwipeVelocity();
				}
				else if (_horizontalStackLayout.TranslationX <= _initialXPosition - (ContentWidth * translationThreshold))
				{
					HandleTranslationThresholdDecrease();
				}
				else if (_horizontalStackLayout.TranslationX >= _initialXPosition + (ContentWidth * translationThreshold))
				{
					HandleTranslationThresholdIncrease();
				}
				else
				{
					MoveToCurrentTabItem();
				}
			}
		}

		void HandleTranslationThresholdIncrease()
		{

#if !WINDOWS
			HandleThresholdIncreaseForNonWindows();
#else
			HandleThresholdIncreaseForWindows();
#endif
		}

		void HandleThresholdIncreaseForNonWindows()
		{
			if (_horizontalStackLayout != null)
			{
				double tabMoveTo;
				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
				{
					tabMoveTo = SelectedIndex - 1;
					if (tabMoveTo >= 0 && _isPreviousItemVisible)
					{
						MoveToPreviousTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
				else
				{
					tabMoveTo = SelectedIndex + 1;
					if (tabMoveTo <= _horizontalStackLayout.Children.Count - 1 && _isNextItemVisible)
					{
						MoveToNextTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
			}
		}

		void HandleThresholdIncreaseForWindows()
		{
			double tabMoveTo = SelectedIndex - 1;
			if (tabMoveTo >= 0 && _isPreviousItemVisible)
			{
				MoveToPreviousTabItem();
			}
			else
			{
				MoveToCurrentTabItem();
			}

		}

		void HandleTranslationThresholdDecrease()
		{
#if !WINDOWS
			HandleThresholdDecreaseForNonWindows();
#else
			HandleThresholdDecreaseForWindows();
#endif
		}

		void HandleThresholdDecreaseForNonWindows()
		{
			if (_horizontalStackLayout != null)
			{
				double tabMoveTo;
				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
				{
					tabMoveTo = SelectedIndex + 1;
					if (tabMoveTo <= _horizontalStackLayout.Children.Count - 1 && _isNextItemVisible)
					{
						MoveToNextTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
				else
				{
					tabMoveTo = SelectedIndex - 1;
					if (tabMoveTo >= 0 && _isPreviousItemVisible)
					{
						MoveToPreviousTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
			}
		}

		void HandleThresholdDecreaseForWindows()
		{
			if (_horizontalStackLayout != null)
			{
				double tabMoveTo = SelectedIndex + 1;
				if (tabMoveTo <= _horizontalStackLayout.Children.Count - 1 && _isNextItemVisible)
				{
					MoveToNextTabItem();
				}
				else
				{
					MoveToCurrentTabItem();
				}
			}
		}

		void HandleForwardSwipeVelocity()
		{
#if !WINDOWS
			HandleForwardSwipeVelocityForNonWindows();
#else
			HandleForwardSwipeVelocityForWindows();
#endif
		}

		void HandleForwardSwipeVelocityForNonWindows()
		{
			if (_horizontalStackLayout != null)
			{
				double tabMoveTo;
				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
				{
					tabMoveTo = SelectedIndex - 1;
					if (tabMoveTo >= 0 && _isPreviousItemVisible && _selectionChangingEventArgs != null && !_selectionChangingEventArgs.Cancel && IsTowardsRight == false)
					{
						MoveToPreviousTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
				else
				{
					tabMoveTo = SelectedIndex + 1;
					if (tabMoveTo <= _horizontalStackLayout.Children.Count - 1 && _isNextItemVisible && _selectionChangingEventArgs != null && !_selectionChangingEventArgs.Cancel && IsTowardsRight == true)
					{
						MoveToNextTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
			}
		}

		void HandleForwardSwipeVelocityForWindows()
		{
			if (_horizontalStackLayout != null)
			{
				double tabMoveTo = SelectedIndex - 1;
				if (tabMoveTo >= 0 && _isPreviousItemVisible &&
					_selectionChangingEventArgs != null &&
					!_selectionChangingEventArgs.Cancel &&
					IsTowardsRight == false)
				{
					MoveToPreviousTabItem();
				}
				else
				{
					MoveToCurrentTabItem();
				}
			}
		}

		void HandleBackwardSwipeVelocity()
		{
			if (_horizontalStackLayout != null)
			{
#if !WINDOWS
				HandleBackwardSwipeVelocityForNonWindows();
#else
				HandleBackwardSwipeVelocityForWindows();
#endif
			}
		}

		void HandleBackwardSwipeVelocityForNonWindows()
		{
			if (_horizontalStackLayout != null)
			{
				double tabMoveTo;
				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
				{
					tabMoveTo = SelectedIndex + 1;
					if (tabMoveTo <= _horizontalStackLayout.Children.Count - 1 && _isNextItemVisible && _selectionChangingEventArgs != null && !_selectionChangingEventArgs.Cancel && IsTowardsRight == true)
					{
						MoveToNextTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
				else
				{
					tabMoveTo = SelectedIndex - 1;
					if (tabMoveTo >= 0 && _isPreviousItemVisible && _selectionChangingEventArgs != null && !_selectionChangingEventArgs.Cancel && IsTowardsRight == false)
					{
						MoveToPreviousTabItem();
					}
					else
					{
						MoveToCurrentTabItem();
					}
				}
			}
		}

		void HandleBackwardSwipeVelocityForWindows()
		{
			if (_horizontalStackLayout != null)
			{
				double tabMoveTo = SelectedIndex + 1;
				if (tabMoveTo <= _horizontalStackLayout.Children.Count - 1 && _isNextItemVisible && _selectionChangingEventArgs != null && !_selectionChangingEventArgs.Cancel && IsTowardsRight == true)
				{
					MoveToNextTabItem();
				}
				else
				{
					MoveToCurrentTabItem();
				}
			}
		}

		/// <summary>
		/// This method is used to update the swipe towards the right.
		/// </summary>
		void MoveToNextTabItem()
		{
			if (_horizontalStackLayout == null)
			{
				return;
			}
#if !WINDOWS
			if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
			{
				_horizontalStackLayout.TranslateTo(-ContentWidth * (_currentIndex + 1), 0, 100, AnimationEasing);
			}
			else
			{
				_horizontalStackLayout.TranslateTo(ContentWidth * (_currentIndex + 1), 0, 100, AnimationEasing);
			}
#else
			_horizontalStackLayout.TranslateTo(-ContentWidth * (_currentIndex + 1), 0, 100, AnimationEasing);
#endif

			SelectNextItem();
		}

		void SelectNextItem()
		{
			if (_tabBar != null)
			{
				if (ContentItemTemplate != null)
				{
					_tabBar.SelectedIndex = _currentIndex + 1;
				}
				else
				{
					_tabBar.SelectedIndex = GetNextVisibleItemIndex();
				}
			}
		}

		/// <summary>
		/// This method is used to update the swipe towards the left.
		/// </summary>
		void MoveToPreviousTabItem()
		{
#if !WINDOWS
			if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
			{
				_horizontalStackLayout?.TranslateTo(-ContentWidth * (_currentIndex - 1), 0, 100, AnimationEasing);
			}
			else
			{
				_horizontalStackLayout?.TranslateTo(ContentWidth * (_currentIndex - 1), 0, 100, AnimationEasing);
			}
#else
			_horizontalStackLayout?.TranslateTo(-ContentWidth * (_currentIndex - 1), 0, 100, AnimationEasing);
#endif
			SelectPreviousTabItem();
		}

		void SelectPreviousTabItem()
		{
			if (_tabBar != null)
			{
				if (ContentItemTemplate != null)
				{
					_tabBar.SelectedIndex = _currentIndex - 1;
				}
				else
				{
					_tabBar.SelectedIndex = GetNextVisibleItemIndex();
				}
			}
		}

		/// <summary>
		/// This method is used to update the current position.
		/// </summary>
		void MoveToCurrentTabItem()
		{
			if (_horizontalStackLayout == null)
			{
				return;
			}
#if !WINDOWS
			if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) != EffectiveFlowDirection.RightToLeft)
			{
				_horizontalStackLayout.TranslateTo(-ContentWidth * _currentIndex, 0, 100, AnimationEasing);
			}
			else
			{
				_horizontalStackLayout.TranslateTo(ContentWidth * _currentIndex, 0, 100, AnimationEasing);
			}
#else
			_horizontalStackLayout.TranslateTo(-ContentWidth * _currentIndex, 0, 100, AnimationEasing);
#endif
		}

		/// <summary>
		/// Calculates the swipe velocity.
		/// </summary>
		/// <param name="point">The touch point.</param>
		void FindVelocity(Point point)
		{
			TimeSpan duration = DateTime.Now - _startTime;
			double distanceX = point.X - _startPoint.X;
			_velocityX = distanceX / duration.TotalSeconds;
		}

		/// <summary>
		/// Raises the event to notify that the tab selection is changing when navigating between tab contents by swiping.
		/// </summary>
		void RaiseSelectionChanging()
		{
			if (_selectionChangingEventArgs != null)
			{
				if (Items != null && Items.Count > 0)
				{
					_selectionChangingEventArgs.Index = GetNextVisibleItemIndex();
					if (_selectionChangingEventArgs.Index != SelectedIndex)
					{
						RaiseSelectionChangingEvent(_selectionChangingEventArgs);
					}
				}
				else if (ItemsSource != null && ItemsSource.Count > 0 && _tabBar != null)
				{
					_selectionChangingEventArgs.Index = GetNextItemIndex();
					if (_selectionChangingEventArgs.Index != _tabBar.SelectedIndex)
					{
						RaiseSelectionChangingEvent(_selectionChangingEventArgs);
					}
				}
			}
		}

		/// <summary>
		/// This method is used to get an index of next item from the selected index.
		/// </summary>
		int GetNextItemIndex()
		{
			if (ItemsSource == null || ItemsSource.Count == 0)
			{
				return 0;
			}

			int increment = IsTowardsRight == true ? 1 : -1;
			int newIndex = 0;
			if (_tabBar != null)
			{
				newIndex = _tabBar.SelectedIndex + increment;
			}

			return SfHorizontalContent.ClampIndex(newIndex, 0, ItemsSource.Count - 1);
		}

		static int ClampIndex(int index, int min, int max)
		{
			return Math.Max(min, Math.Min(max, index));
		}

		/// <summary>
		/// This method is used to get an index of next visible item from the selected index.
		/// </summary>
		/// <returns></returns>
		int GetNextVisibleItemIndex()
		{
			if (Items == null || Items?.Count == 0 || SelectedIndex < 0 || SelectedIndex >= Items?.Count)
			{
				return 0;
			}

			int increment = IsTowardsRight == true ? 1 : -1;
			int index = SelectedIndex + increment;

			// Loop to find the next visible item
			while (index >= 0 && index < Items?.Count && !Items[index].IsVisible)
			{
				index += increment;
			}

			TabItemCollection sfTabItems = [];
			if (Items != null)
			{
				sfTabItems = Items;
			}

			return Math.Max(0, Math.Min(sfTabItems.Count - 1, index));
		}

		/// <summary>
		/// This method is used to check the visibility of nearby items of the selected item.
		/// </summary>
		void UpdateItemVisibility()
		{
			if (ContentItemTemplate != null && _horizontalStackLayout != null && _tabBar != null)
			{
				UpdateVisibilityWithTemplate(_horizontalStackLayout, _tabBar);
			}
			else
			{
				UpdateVisibilityWithoutTemplate();
			}
		}

		void UpdateVisibilityWithoutTemplate()
		{
			_isNextItemVisible = false;
			_isPreviousItemVisible = false;

			if (Items != null && Items.Count > 0)
			{
				for (int i = SelectedIndex + 1; i < Items.Count; i++)
				{
					if (Items[i].IsVisible == true)
					{
						_isNextItemVisible = true;
						_nextVisibleIndex = i;
						break;
					}
				}

				for (int i = SelectedIndex - 1; i >= 0; i--)
				{
					if (Items[i].IsVisible == true)
					{
						_isPreviousItemVisible = true;
						_previousVisibleIndex = i;
						break;
					}
				}
			}
		}

		void UpdateVisibilityWithTemplate(SfHorizontalStackLayout horizontalStackLayout, SfTabBar tabBar)
		{
			double selectedIndex = tabBar.SelectedIndex;
			if (horizontalStackLayout.Children != null)
			{
				int childrenCount = horizontalStackLayout.Children.Count;
				_isNextItemVisible = selectedIndex < childrenCount - 1;
			}
			_isPreviousItemVisible = selectedIndex > 0;
		}
		#endregion

		#region Events

		/// <summary>
		/// Selection Changing event.
		/// </summary>
		public event EventHandler<SelectionChangingEventArgs>? SelectionChanging;

		#endregion

		#region Property Changed

		static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfHorizontalContent)?.UpdateItems();
		static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfHorizontalContent)?.UpdateSelectedIndex();
		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfHorizontalContent)?.UpdateItemsSource();
		static void OnContentItemTemplateChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfHorizontalContent)?.UpdateItemsSource();

		#endregion
	}
}