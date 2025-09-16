using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.Helper;
using System.Runtime.CompilerServices;
#if IOS || MACCATALYST
using UIKit;
#endif

namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
	/// <summary>
	/// Represents the <see cref="SfNavigationDrawer"/> control that contains multiple items that share the same space on the screen.
	/// </summary>
	public partial class SfNavigationDrawer : SfNavigationDrawerExt, ITouchListener, ITapGestureListener, IParentThemeElement
	{
		#region Fields

		SfGrid? _greyOverlayGrid;

		bool _isDrawerOpen;

		double _screenWidth;

		double _screenHeight;

		SfGrid? _drawerLayout;

		SfGrid? _mainContentGrid;

		double _touchRightThreshold;

		double _touchBottomThreshold;

		double _remainDrawerWidth;

		double _remainDrawerHeight;

		Point _initialTouchPoint;

		double _toolBarHeight = 1;

		double _drawerMoveTop;

		bool _isPressed;

		bool _isMoved;

		Point _oldPoint;

		Point _newPoint;

		readonly double _opacity = 0.5;

		bool _actionFirstMoveOpen;

		bool _actionFirstMoveClose;

		bool _isTransitionDifference;

		readonly ToggledEventArgs _toggledEventArgs = new();

		readonly CancelEventArgs _cancelOpenEventArgs = new();

		readonly CancelEventArgs _cancelCloseEventArgs = new();

		DateTime _startTime;

		Point _startPoint;

		double _velocityX;

		double _velocityY;

		View? _oldHeaderView;

		View? _oldFooterView;

#if !WINDOWS
		// Workaround for RTL (Right-to-Left) layout issue - The coordinate points are not calculated correctly in RTL layouts, 
		// causing incorrect positioning. This flag helps to apply RTL-specific adjustments.
		bool _isRTL;
#endif

#if IOS || MACCATALYST
		readonly SfNavigationDrawerProxy _proxy;
#endif
		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ContentView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// It is mandatory to set the <see cref="ContentView"/> property for the <see cref="SfNavigationDrawer"/> when initializing.
		/// </remarks>
		public static readonly BindableProperty ContentViewProperty =
			BindableProperty.Create(
				nameof(ContentView),
				typeof(View),
				typeof(SfNavigationDrawer),
				null,
				BindingMode.Default,
				null,
				propertyChanged: OnContentViewChanged);

		/// <summary>
		/// Identifies the <see cref="DrawerSettings"/> bindable property.
		/// </summary>
		public static readonly BindableProperty DrawerSettingsProperty =
			BindableProperty.Create(
				nameof(DrawerSettings),
				typeof(DrawerSettings),
				typeof(SfNavigationDrawer),
				new DrawerSettings(),
				BindingMode.Default,
				null,
				propertyChanged: OnDrawerSettingsChanged);

#if !WINDOWS
		/// <summary>
		/// Identifies the <see cref="FlowDirection"/> bindable property.
		/// </summary>
		public static readonly new BindableProperty FlowDirectionProperty =
			BindableProperty.Create(
				nameof(FlowDirection),
				typeof(FlowDirection),
				typeof(SfNavigationDrawer),
				FlowDirection.LeftToRight,
				BindingMode.Default,
				null,
				propertyChanged: OnFlowDirectionChanged);
#endif

		/// <summary>
		/// Identifies the <see cref="IsOpen"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IsOpenProperty =
			BindableProperty.Create(
				nameof(IsOpen),
				typeof(bool),
				typeof(SfNavigationDrawer),
				false,
				BindingMode.TwoWay,
				null,
				propertyChanged: OnIsOpenChanged);

		#endregion

		#region Internal Bindable Properties

		/// <summary>
		/// Identifies the <see cref="GreyOverlayColor"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty GreyOverlayColorProperty =
			BindableProperty.Create(
				nameof(GreyOverlayColor),
				typeof(Color),
				typeof(SfNavigationDrawer),
				Color.FromArgb("80000000"),
				BindingMode.Default,
				null,
				propertyChanged: OnGreyColorChanged);

		/// <summary>
		/// Identifies the <see cref="ContentBackgroundColor"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty ContentBackgroundColorProperty =
			BindableProperty.Create(
				nameof(ContentBackgroundColor),
				typeof(Color),
				typeof(SfNavigationDrawer),
				Color.FromArgb("#F7F2FB"),
				BindingMode.Default,
				null,
				propertyChanged: OnContentBackgroundColorChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfNavigationDrawer"/> class.
		/// </summary>
		public SfNavigationDrawer()
		{
			ThemeElement.InitializeThemeResources(this, "SfNavigationDrawerTheme");
			InitializeDrawer();
			InitializeGreyOverlayGrid();
			InitializeContentViewGrid();
			UpdateAllChild();
			PositionUpdate();
			this.AddTouchListener(this);
#if IOS || MACCATALYST
			this.AddGestureListener(this);
			_proxy = new(this);
#endif
			BackgroundColor = Colors.Transparent;
			SetDynamicResource(GreyOverlayColorProperty, "SfNavigationDrawerGreyLayoutBackground");
			SetDynamicResource(ContentBackgroundColorProperty, "SfNavigationDrawerContentBackground");
#if IOS
			IgnoreSafeArea = true;
#endif
#if !WINDOWS
			((SfView)this).FlowDirection = FlowDirection.LeftToRight;
			Loaded += SfNavigationDrawer_Loaded;
#endif
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the view that can be used to customize the content view of SfNavigationDrawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="ContentView"/> values and the default value is null.
		/// </value>
		/// <example>
		/// The below examples shows, how to use the <see cref="ContentView"/> property in the <see cref="SfNavigationDrawer"/>.
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <navigationdrawer:SfNavigationDrawer>
		/// <navigationdrawer:SfNavigationDrawer.ContentView>
		///  <StackLayout>
		///    <Label Text = "Drawer Content" />
		///  </StackLayout>
		/// </navigationdrawer:SfNavigationDrawer.ContentView>
		/// </navigationdrawer:SfNavigationDrawer>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// var navigationDrawer = new SfNavigationDrawer
		/// {
		///     ContentView = new StackLayout
		///     {
		///         Children = 
		///         {
		///             new Label { Text = "Drawer Content" }
		///         }
		///     }
		/// };
		/// ]]></code>
		/// </example>
		public View ContentView
		{
			get { return (View)GetValue(ContentViewProperty); }
			set { SetValue(ContentViewProperty, value); }
		}

		/// <summary>
		/// Gets or sets the settings for the navigation drawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerSettings"/> values and the default value is a new instance of <see cref="DrawerSettings"/>.
		/// </value>
		/// <example>
		/// The below examples shows, how to use the <see cref="DrawerSettings"/> property in the <see cref="SfNavigationDrawer"/>.
		/// # [XAML](#tab/tabid-3)
		/// <code Lang="XAML"><![CDATA[
		/// <navigationdrawer:SfNavigationDrawer>
		/// <navigationdrawer:SfNavigationDrawer.DrawerSettings>
		/// <navigationdrawer:DrawerSettings DrawerWidth = "500" DrawerHeight="300">
		///    <navigationdrawer:DrawerSettings.DrawerHeaderView>
		///        <Label Text = "Header" />
		///    </navigationdrawer:DrawerSettings.DrawerHeaderView>
		///    <navigationdrawer:DrawerSettings.DrawerFooterView>
		///        <Label Text = "Footer" />
		///    </navigationdrawer:DrawerSettings.DrawerFooterView>
		///    <navigationdrawer:DrawerSettings.DrawerContentView>
		///        <Label Text = "Drawer Content" />
		///    </navigationdrawer:DrawerSettings.DrawerContentView>
		///    </navigationdrawer:DrawerSettings>
		/// </navigationdrawer:SfNavigationDrawer.DrawerSettings>
		/// </navigationdrawer:SfNavigationDrawer>
		/// ]]></code>
		/// # [C#](#tab/tabid-4)
		/// <code Lang="C#"><![CDATA[
		/// var navigationDrawer = new SfNavigationDrawer
		/// {
		///     DrawerSettings = new DrawerSettings
		///     {
		///         DrawerWidth = 300,
		///         DrawerHeight = 500,
		///         DrawerHeaderView = new Label { Text = "Header" },
		///         DrawerContentView = new StackLayout
		///         {
		///             Children = 
		///             {
		///                 new Label { Text = "Drawer Content" }
		///             }
		///         },
		///         DrawerFooterView = new Label { Text = "Footer" }
		///     }
		/// };
		/// ]]></code>
		/// </example>
		public DrawerSettings DrawerSettings
		{
			get { return (DrawerSettings)GetValue(DrawerSettingsProperty); }
			set { SetValue(DrawerSettingsProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to open or close the drawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="IsOpen"/> values and the default value is false.
		/// </value>
		/// <example>
		/// The below examples shows, how to use the <see cref="IsOpen"/> property in the <see cref="SfNavigationDrawer"/>.
		/// # [XAML](#tab/tabid-5)
		/// <code Lang="XAML"><![CDATA[
		/// <navigationdrawer:SfNavigationDrawer IsOpen = "True" />
		/// ]]></code>
		/// # [C#](#tab/tabid-6)
		/// <code Lang="C#"><![CDATA[
		/// var navigationDrawer = new SfNavigationDrawer
		/// {
		///     IsOpen = true
		/// };
		/// ]]></code>
		/// </example>
		public bool IsOpen
		{
			get { return (bool)GetValue(IsOpenProperty); }
			set { SetValue(IsOpenProperty, value); }
		}

		// Workaround for RTL (Right-to-Left) layout issue - The coordinate points are not calculated correctly in RTL layouts, 
		// causing incorrect positioning. This flag helps to apply RTL-specific adjustments.
#if !WINDOWS
		/// <summary>
		/// Gets or sets a value of the flow direction of the navigation drawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="FlowDirection"/> values and the default value is <see cref="FlowDirection.LeftToRight"/>.
		/// </value>
		/// <example>
		/// The below examples shows, how to use the <see cref="FlowDirection"/> property in the <see cref="SfNavigationDrawer"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <navigationdrawer:SfNavigationDrawer FlowDirection="RightToLeft" />
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var navigationDrawer = new SfNavigationDrawer
		/// {
		///     FlowDirection = FlowDirection.RightToLeft
		/// };
		/// ]]></code>
		/// </example>
		public new FlowDirection FlowDirection
		{
			get { return (FlowDirection)GetValue(FlowDirectionProperty); }
			set { SetValue(FlowDirectionProperty, value); }
		}

#endif

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the grey overlay color to customize the background color of the grey overlay grid.
		/// </summary>
		internal Color GreyOverlayColor
		{
			get { return (Color)GetValue(GreyOverlayColorProperty); }
			set { SetValue(GreyOverlayColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color value to customize the background color of the drawer content view.
		/// </summary>
		internal Color ContentBackgroundColor
		{
			get { return (Color)GetValue(ContentBackgroundColorProperty); }
			set { SetValue(ContentBackgroundColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to know the width of screen.
		/// </summary>
		internal double ScreenWidth
		{
			get
			{
				return _screenWidth;
			}

			set
			{
				if (_screenWidth != value)
				{
					_screenWidth = value;
#if !ANDROID
					PositionUpdate();
#endif
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that can be used to know the height of screen.
		/// </summary>
		internal double ScreenHeight
		{
			get
			{
				return _screenHeight;
			}

			set
			{
				if (_screenHeight != value)
				{
					_screenHeight = value;
					PositionUpdate();
				}
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Methods to open or close the drawer based on the existing state in the SfNavigationDrawer.
		/// </summary>
		/// <remarks>
		/// This method enables to open or close the drawer programmatically based on the existing state in the SfNavigationDrawer.
		/// </remarks>
		/// <example>
		/// <code><![CDATA[
		/// private void HamburgerButton_Clicked(object sender, EventArgs e)
		/// {
		///     navigationDrawer.ToggleDrawer();
		/// }
		/// ]]></code>
		/// </example>
		public void ToggleDrawer()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null)
			{
				if (_greyOverlayGrid.AnimationIsRunning("greyOverlayAnimation"))
				{
					_greyOverlayGrid.AbortAnimation("greyOverlayAnimation");
				}

				if (_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation"))
				{
					_greyOverlayGrid.AbortAnimation("greyOverlayTranslateAnimation");
				}

				if (_drawerLayout.AnimationIsRunning("drawerAnimation"))
				{
					_drawerLayout.AbortAnimation("drawerAnimation");
				}

				if (_mainContentGrid != null)
				{
					if (_mainContentGrid.AnimationIsRunning("contentViewTranslatePushAnimation"))
					{
						_mainContentGrid.AbortAnimation("contentViewTranslatePushAnimation");
					}
				}

				_actionFirstMoveOpen = true;
				_actionFirstMoveClose = true;

				switch (DrawerSettings.Position)
				{
					case Position.Left:
						HandleLeftDrawer();
						break;

					case Position.Right:
						HandleRightDrawer();
						break;

					case Position.Top:
						HandleTopDrawer();
						break;

					case Position.Bottom:
						HandleBottomDrawer();
						break;
				}
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// This method represents the action taken when the drawer is closed.
		/// </summary>
		/// <param name="args">The closed argument.</param>
		internal virtual void OnDrawerClosed(EventArgs args)
		{
			DrawerClosed?.Invoke(this, args);
		}

		/// <summary>
		/// This method represents the action taken when the drawer is opened.
		/// </summary>
		/// <param name="args">The opened argument.</param>
		internal virtual void OnDrawerOpened(EventArgs args)
		{
			DrawerOpened?.Invoke(this, args);
		}

		/// <summary>
		/// This method represents the action taken when the drawer is toggled.
		/// </summary>
		/// <param name="args">The toggled argument.</param>
		internal virtual void OnDrawerToggled(ToggledEventArgs args)
		{
			DrawerToggled?.Invoke(this, args);
		}

		/// <summary>
		/// This method represents the action taken when the drawer is opening.
		/// </summary>
		/// <param name="args">The opening argument.</param>
		internal virtual void OnDrawerOpening(CancelEventArgs args)
		{
			DrawerOpening?.Invoke(this, args);
		}

		/// <summary>
		/// This method represents the action taken when the drawer is closing.
		/// </summary>
		/// <param name="args">the closing operation.</param>
		internal virtual void OnDrawerClosing(CancelEventArgs args)
		{
			DrawerClosing?.Invoke(this, args);
		}

		#endregion

		#region Private Methods

		void HandleLeftDrawer()
		{
			if (_isDrawerOpen)
			{
#if !WINDOWS
				if (_isRTL)
				{
					DrawerRightOut();
				}
				else
#endif
				{
					DrawerLeftOut();
				}
			}
			else
			{
				UpdateGridOverlayTranslate();
#if !WINDOWS
				if (_isRTL)
				{
					DrawerRightIn();
				}
				else
#endif
				{
					DrawerLeftIn();
				}
			}
		}

		void HandleRightDrawer()
		{
			if (_isDrawerOpen)
			{
#if !WINDOWS
				if (_isRTL)
				{
					DrawerLeftOut();
				}
				else
#endif
				{
					DrawerRightOut();
				}
			}
			else
			{
				UpdateGridOverlayTranslate();
#if !WINDOWS
				if (_isRTL)
				{
					DrawerLeftIn();
				}
				else
#endif
				{
					DrawerRightIn();
				}
			}
		}

		void HandleTopDrawer()
		{
			if (_isDrawerOpen)
			{
				DrawerTopOut();
			}
			else
			{
				UpdateGridOverlayTranslate();
				DrawerTopIn();
			}
		}

		void HandleBottomDrawer()
		{
			if (_isDrawerOpen)
			{
				DrawerBottomOut();
			}
			else
			{
				UpdateGridOverlayTranslate();
				DrawerBottomIn();
			}
		}

		/// <summary>
		/// The method used to update all children of the SfNavigationDrawer.
		/// </summary>
		void UpdateAllChild()
		{
#if !WINDOWS
			UpdateDrawerFlowDirection();
#endif
			if (_mainContentGrid != null)
			{
				_mainContentGrid.Children.Clear();
				_mainContentGrid.Children.Add(ContentView);
			}

			if (DrawerSettings.Transition == Transition.Reveal)
			{
				AddChild(_drawerLayout);
				AddChild(_mainContentGrid);
				AddChild(_greyOverlayGrid);
			}
			else
			{
				AddChild(_mainContentGrid);
				AddChild(_greyOverlayGrid);
				AddChild(_drawerLayout);
			}
		}

		void AddChild(View? child)
		{
			if (child != null)
			{
				Children.Add(child);
			}
		}

		/// <summary>
		/// This method is used to initialize the drawer layout.
		/// </summary>
		void InitializeDrawer()
		{
			_drawerLayout = new SfGrid()
			{
				BackgroundColor = DrawerSettings.ContentBackground,
				RowDefinitions =
				{
					new RowDefinition { Height = new GridLength(DrawerSettings.DrawerHeaderHeight) },
					new RowDefinition(),
					new RowDefinition { Height = new GridLength(DrawerSettings.DrawerFooterHeight) },
				},
				RowSpacing = 0,
			};
		}

		/// <summary>
		/// This method is used to initialize the greyOverlayGrid.
		/// </summary>
		void InitializeGreyOverlayGrid()
		{
			_greyOverlayGrid = new SfGrid() { BackgroundColor = GreyOverlayColor, Opacity = 0, };
		}

		void InitializeContentViewGrid()
		{
			_mainContentGrid = new SfGrid()
			{
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
				Background = Colors.Transparent
			};
		}

		void OnHandleTouchInteraction(PointerActions action, Point point)
		{
			if (!DrawerSettings.EnableSwipeGesture)
			{
				HandleTouchWhenDrawerOpen(action, point);
				return;
			}

			switch (action)
			{
				case PointerActions.Pressed:
					HandlePointerPressed(point);
					break;

				case PointerActions.Moved:
					HandlePointerMoved(point);
					break;

				case PointerActions.Released:
					HandlePointerReleased(point);
					break;

				case PointerActions.Cancelled:
				case PointerActions.Entered:
					break;
			}
		}

		void HandleTouchWhenDrawerOpen(PointerActions action, Point point)
		{
			if (action == PointerActions.Pressed && _isDrawerOpen)
			{
				_initialTouchPoint = point;
				if (IsTouchOutsideDrawerBounds())
				{
					ToggleDrawer();
				}
			}
		}

		bool IsTouchOutsideDrawerBounds()
		{
			return (_initialTouchPoint.X > DrawerSettings.DrawerWidth && DrawerSettings.Position == Position.Left)
				|| (_initialTouchPoint.X < ScreenWidth - DrawerSettings.DrawerWidth && DrawerSettings.Position == Position.Right)
				|| (_initialTouchPoint.Y > DrawerSettings.DrawerHeight && DrawerSettings.Position == Position.Top)
				|| (_initialTouchPoint.Y < ScreenHeight - DrawerSettings.DrawerHeight && DrawerSettings.Position == Position.Bottom);
		}

		void HandlePointerPressed(Point point)
		{
			_oldPoint = point;
			_initialTouchPoint = point;
			_startTime = DateTime.Now;
			_startPoint = point;

			if (DrawerSettings.Position == Position.Left || DrawerSettings.Position == Position.Right)
			{
				_remainDrawerWidth = _isDrawerOpen ? 0 : -DrawerSettings.DrawerWidth;

				switch (DrawerSettings.Position)
				{
					case Position.Left:
						{
#if !WINDOWS
							if ((_initialTouchPoint.X <= DrawerSettings.TouchThreshold && !_isDrawerOpen && !_isRTL) || (_initialTouchPoint.X >= _touchRightThreshold && !_isDrawerOpen && _isRTL) || _isDrawerOpen)
#else
							if ((_initialTouchPoint.X <= DrawerSettings.TouchThreshold && !_isDrawerOpen) || _isDrawerOpen)
#endif
							{
								FirstMoveActionStarted();
							}

							break;
						}

					case Position.Right:
						{
#if !WINDOWS
							if ((_initialTouchPoint.X >= _touchRightThreshold && !_isDrawerOpen && !_isRTL) || (_initialTouchPoint.X <= DrawerSettings.TouchThreshold && !_isDrawerOpen && _isRTL) || _isDrawerOpen)
#else
							if ((_initialTouchPoint.X >= _touchRightThreshold && !_isDrawerOpen) || _isDrawerOpen)
#endif
							{
								FirstMoveActionStarted();
							}

							break;
						}
				}
			}
			else if (DrawerSettings.Position == Position.Top || DrawerSettings.Position == Position.Bottom)
			{
				_remainDrawerHeight = _isDrawerOpen ? 0 : -DrawerSettings.DrawerHeight;

				switch (DrawerSettings.Position)
				{
					case Position.Top:
						{
							if ((_initialTouchPoint.Y <= DrawerSettings.TouchThreshold && !_isDrawerOpen) || _isDrawerOpen)
							{
								FirstMoveActionStarted();
							}

							break;
						}

					case Position.Bottom:
						{
							if ((_initialTouchPoint.Y >= _touchBottomThreshold && !_isDrawerOpen) || _isDrawerOpen)
							{
								FirstMoveActionStarted();
							}

							break;
						}
				}
			}
		}

		void HandlePointerMoved(Point point)
		{
			if (_isPressed && (point.X != _initialTouchPoint.X || point.Y != _initialTouchPoint.Y))
			{
				_isMoved = true;
				_newPoint = point;
				FindVelocity(point);

				if (DrawerSettings.Position == Position.Left || DrawerSettings.Position == Position.Right)
				{
					TranslateDrawerXPosition();
				}
				else
				{
					TranslateDrawerYPosition();
				}

				_oldPoint = _newPoint;
			}
		}

		void HandlePointerReleased(Point point)
		{
			if (_isPressed && _isMoved)
			{
				CompletedDrawerSwipe();
			}
			else if (_isPressed && _isDrawerOpen)
			{
#if !WINDOWS
				if (_isRTL)
				{
					if ((_initialTouchPoint.X < ScreenWidth - DrawerSettings.DrawerWidth && DrawerSettings.Position == Position.Left)
					|| (_initialTouchPoint.X > DrawerSettings.DrawerWidth && DrawerSettings.Position == Position.Right)
					|| (_initialTouchPoint.Y > DrawerSettings.DrawerHeight && DrawerSettings.Position == Position.Top)
					|| (_initialTouchPoint.Y < ScreenHeight - DrawerSettings.DrawerHeight && DrawerSettings.Position == Position.Bottom))
					{
						ToggleDrawer();
					}
				}
				else
#endif
				{
					if ((_initialTouchPoint.X > DrawerSettings.DrawerWidth && DrawerSettings.Position == Position.Left)
						|| (_initialTouchPoint.X < ScreenWidth - DrawerSettings.DrawerWidth && DrawerSettings.Position == Position.Right)
						|| (_initialTouchPoint.Y > DrawerSettings.DrawerHeight && DrawerSettings.Position == Position.Top)
						|| (_initialTouchPoint.Y < ScreenHeight - DrawerSettings.DrawerHeight && DrawerSettings.Position == Position.Bottom))
					{
						ToggleDrawer();
					}
				}
			}

			_isPressed = false;
			_isMoved = false;
		}

#if !WINDOWS
		/// <summary>
		/// Method that is called when the navigation drawer is loaded.
		/// </summary>
		/// <param name="sender">sender.</param>
		/// <param name="e">e.</param>
		void SfNavigationDrawer_Loaded(object? sender, EventArgs e)
		{
			if (Parent is Layout parentLayout && parentLayout.FlowDirection != FlowDirection.MatchParent)
			{
				_isRTL = parentLayout.FlowDirection == FlowDirection.RightToLeft;
				UpdateDrawerFlowDirection();
				PositionUpdate();
			}

			Unloaded += SfNavigationDrawer_Unloaded;
		}

		/// <summary>
		/// Method that is called when the navigation drawer is unloaded.
		/// </summary>
		/// <param name="sender">sender.</param>
		/// <param name="e">e.</param>
		void SfNavigationDrawer_Unloaded(object? sender, EventArgs e)
		{
			Loaded -= SfNavigationDrawer_Loaded;
			Unloaded -= SfNavigationDrawer_Unloaded;
		}
#endif

		/// <summary>
		/// Methods to find the velocity of swiping.
		/// </summary>
		/// <param name="point">Touch point</param>
		void FindVelocity(Point point)
		{
			TimeSpan duration = DateTime.Now - _startTime;
			double distanceX = point.X - _startPoint.X;
			double distanceY = point.Y - _startPoint.Y;

			_velocityX = distanceX / duration.TotalSeconds;
			_velocityY = distanceY / duration.TotalSeconds;
		}

		/// <summary>
		/// The event handler method updates the properties when changed from drawer settings.
		/// </summary>
		/// <param name="sender">sender.</param>
		/// <param name="e">e.</param>
		void OnDefaultDrawerSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (DrawerSettings != null)
			{
				switch (e.PropertyName)
				{
					case "DrawerContentView":
						UpdateDrawerContentView();
						break;

					case "DrawerHeaderView":
						UpdateDrawerHeaderView();
						break;

					case "DrawerFooterView":
						UpdateDrawerFooterView();
						break;

					case "ContentBackground":
						UpdateContentBackground();
						break;

					case "DrawerHeaderHeight":
						UpdateDrawerHeaderFooterSize();
						break;

					case "DrawerFooterHeight":
						UpdateDrawerHeaderFooterSize();
						break;

					case "DrawerWidth":
						DrawerSettings.UpdateDrawerWidth();
						UpdateDrawerLayoutSize();
						break;

					case "DrawerHeight":
						DrawerSettings.UpdateDrawerHeight();
						UpdateDrawerLayoutSize();
						break;

					case "Duration":
						DurationPropertyUpdate(DrawerSettings.Duration);
						break;

					case "Position":
						PositionUpdate();
						break;

					case "Transition":
						UpdateTransitionAnimation();
						break;

					case "TouchThreshold":
						UpdateTouchThreshold();
						break;
				}
			}
		}

		/// <summary>
		/// This method is used assigns the values of drawer settings to the navigation drawer.
		/// </summary>
		void OnDefaultDrawerSettingsChanged()
		{
			if (DrawerSettings != null)
			{
				UpdateDrawerContentView();
				UpdateDrawerHeaderView();
				UpdateDrawerFooterView();
				UpdateDrawerHeaderFooterSize();
				UpdateDrawerLayoutSize();
				DurationPropertyUpdate(DrawerSettings.Duration);
				PositionUpdate();
				UpdateContentBackground();
				UpdateTransitionAnimation();
			}
		}

		/// <summary>
		/// This method is used to update the drawer header view.
		/// </summary>
		void UpdateDrawerHeaderView()
		{
			if (_drawerLayout != null)
			{
				UpdateDrawerHeaderFooterSize();
				if (DrawerSettings.DrawerHeaderView != null)
				{
					_drawerLayout.Remove(DrawerSettings.DrawerHeaderView);
					_drawerLayout.SetRow(DrawerSettings.DrawerHeaderView, 0);
					_drawerLayout.Children.Add(DrawerSettings.DrawerHeaderView);
				}
				else
				{
					_drawerLayout.Remove(_oldHeaderView);
				}

				_oldHeaderView = DrawerSettings.DrawerHeaderView;
			}
		}

		/// <summary>
		/// This method is used to update the drawer content view.
		/// </summary>
		void UpdateDrawerContentView()
		{
			if (_drawerLayout != null && DrawerSettings.DrawerContentView != null)
			{
				_drawerLayout.Remove(DrawerSettings.DrawerContentView);
				_drawerLayout.SetRow(DrawerSettings.DrawerContentView, 1);
				_drawerLayout.Children.Add(DrawerSettings.DrawerContentView);
#if !WINDOWS
				UpdateDrawerFlowDirection();
#endif
			}
		}

		/// <summary>
		/// This method is used to update the drawer footer view.
		/// </summary>
		void UpdateDrawerFooterView()
		{
			if (_drawerLayout != null)
			{
				UpdateDrawerHeaderFooterSize();
				if (DrawerSettings.DrawerFooterView != null)
				{
					_drawerLayout.Remove(DrawerSettings.DrawerFooterView);
					_drawerLayout.SetRow(DrawerSettings.DrawerFooterView, 2);
					_drawerLayout.Children.Add(DrawerSettings.DrawerFooterView);
				}
				else
				{
					_drawerLayout.Remove(_oldFooterView);
				}

				_oldFooterView = DrawerSettings.DrawerFooterView;
			}
		}

		/// <summary>
		/// This method is used to update the drawer layout size.
		/// </summary>
		void UpdateDrawerLayoutSize()
		{
			if (_drawerLayout != null)
			{
				PositionUpdate();
			}
		}

		/// <summary>
		/// This method is used to update the drawer header and footer size.
		/// </summary>
		void UpdateDrawerHeaderFooterSize()
		{
			if (_drawerLayout != null)
			{
				double headerHeight = 0;
				double footerHeight = 0;
				if (DrawerSettings.DrawerHeaderView != null)
				{
					headerHeight = (DrawerSettings.DrawerHeaderHeight < 0) ? (double)DrawerSettings.DrawerHeaderHeightProperty.DefaultValue : DrawerSettings.DrawerHeaderHeight;
					DrawerSettings.DrawerHeaderView.IsVisible = headerHeight > 0;
				}

				if (DrawerSettings.DrawerFooterView != null)
				{
					footerHeight = (DrawerSettings.DrawerFooterHeight < 0) ? (double)DrawerSettings.DrawerFooterHeightProperty.DefaultValue : DrawerSettings.DrawerFooterHeight;
					DrawerSettings.DrawerFooterView.IsVisible = footerHeight > 0;
				}

				_drawerLayout.RowDefinitions.Clear();
				_drawerLayout.RowDefinitions =
				[
					new RowDefinition { Height = new GridLength(headerHeight) },
					new RowDefinition(),
					new RowDefinition { Height = new GridLength(footerHeight) },
				];
			}
		}

		/// <summary>
		/// This method is used to clear and update the children to the SfNavigationDrawer.
		/// </summary>
		void UpdateContentView()
		{
			Children.Clear();
			UpdateAllChild();
		}

		/// <summary>
		/// This method is used to update the drawer to be open or not.
		/// </summary>
		void UpdateIsOpen()
		{
			if (ScreenHeight > 0)
			{
				if ((IsOpen && !_isDrawerOpen) || (!IsOpen && _isDrawerOpen))
				{
					ToggleDrawer();
				}
			}
		}

		/// <summary>
		/// This method is used to clear and update the children and position to the SfNavigationDrawer.
		/// </summary>
		void UpdateTransitionAnimation()
		{
			UpdateContentView();
			PositionUpdate();
		}

		/// <summary>
		/// This method is used to update the size of right and bottom position threshold to the SfNavigationDrawer.
		/// </summary>
		void UpdateTouchThreshold()
		{
			if (DrawerSettings != null)
			{
				if (DrawerSettings.Position == Position.Right)
				{
					_touchRightThreshold = ScreenWidth - DrawerSettings.TouchThreshold;
				}
				else if (DrawerSettings.Position == Position.Bottom)
				{
					_touchBottomThreshold = ScreenHeight - DrawerSettings.TouchThreshold;
				}
			}
		}

		void UpdateGridOverlayTranslate()
		{
			SetVisibility(true);
			if ((DrawerSettings.Transition == Transition.Reveal || DrawerSettings.Transition == Transition.Push) && _greyOverlayGrid != null)
			{
				_greyOverlayGrid.TranslationX = 0;
			}
		}

		/// <summary>
		/// This method is used to update the position of the drawer layout and greyOverlayGrid.
		/// </summary>
		void PositionUpdate()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null)
			{
#if !WINDOWS
				if ((DrawerSettings.Position == Position.Right && !_isRTL) || (DrawerSettings.Position == Position.Left && _isRTL))
#else
				if (DrawerSettings.Position == Position.Right)
#endif
				{
					_drawerLayout.WidthRequest = DrawerSettings.DrawerWidth;
					_drawerLayout.HeightRequest = ScreenHeight;
					_drawerLayout.TranslationY = 0;
					if (_isDrawerOpen)
					{
						SetVisibility(true);
						_drawerLayout.TranslationX = ScreenWidth - DrawerSettings.DrawerWidth;
						if (DrawerSettings.Transition == Transition.SlideOnTop && _mainContentGrid != null)
						{
							_greyOverlayGrid.TranslationX = 0;
							_mainContentGrid.TranslationX = 0;
						}

						if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							_mainContentGrid.TranslationX = -DrawerSettings.DrawerWidth;
							_greyOverlayGrid.TranslationX = -DrawerSettings.DrawerWidth;
							_mainContentGrid.TranslationY = 0;
							_greyOverlayGrid.TranslationY = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !_mainContentGrid.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
						{
							SetVisibility(false);
						}

						_greyOverlayGrid.TranslationX = ScreenWidth;

						if (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null)
						{
							_drawerLayout.TranslationX = ScreenWidth - DrawerSettings.DrawerWidth;
							_mainContentGrid.TranslationX = 0;
							_mainContentGrid.BackgroundColor = _mainContentGrid.BackgroundColor ?? ContentBackgroundColor;
						}
						else
						{
							_drawerLayout.TranslationX = ScreenWidth;
							if (DrawerSettings.Transition == Transition.Push && _mainContentGrid != null)
							{
								_mainContentGrid.TranslationX = 0;
							}
						}
					}

					_touchRightThreshold = ScreenWidth - DrawerSettings.TouchThreshold;
				}
#if !WINDOWS
				else if ((DrawerSettings.Position == Position.Left && !_isRTL) || (DrawerSettings.Position == Position.Right && _isRTL))
#else
				else if (DrawerSettings.Position == Position.Left)
#endif
				{
					_drawerLayout.HorizontalOptions = LayoutOptions.Start;
					_drawerLayout.WidthRequest = DrawerSettings.DrawerWidth;
					_drawerLayout.HeightRequest = ScreenHeight;
					_drawerLayout.TranslationY = 0;
					if (_isDrawerOpen)
					{
						SetVisibility(true);
						_drawerLayout.TranslationX = 0;
						if (DrawerSettings.Transition == Transition.SlideOnTop && _mainContentGrid != null)
						{
							_greyOverlayGrid.TranslationX = 0;
							_mainContentGrid.TranslationX = 0;
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							_greyOverlayGrid.TranslationX = DrawerSettings.DrawerWidth;
							_mainContentGrid.TranslationX = DrawerSettings.DrawerWidth;
							_mainContentGrid.TranslationY = 0;
							_greyOverlayGrid.TranslationY = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !_mainContentGrid.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
						{
							SetVisibility(false);
						}

						_greyOverlayGrid.TranslationX = ScreenWidth;
						if (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null)
						{
							_drawerLayout.TranslationX = 0;
							_mainContentGrid.TranslationX = 0;
							_mainContentGrid.BackgroundColor = _mainContentGrid.BackgroundColor ?? ContentBackgroundColor;
						}
						else
						{
							_drawerLayout.TranslationX = -DrawerSettings.DrawerWidth;
							if (DrawerSettings.Transition == Transition.Push && _mainContentGrid != null)
							{
								_mainContentGrid.TranslationX = 0;
							}
						}
					}
#if !WINDOWS
					if (_isRTL)
					{
						_touchRightThreshold = DrawerSettings.TouchThreshold;
					}
#endif
				}
				else if (DrawerSettings.Position == Position.Top)
				{
					_drawerLayout.WidthRequest = ScreenWidth;
					_drawerLayout.HeightRequest = DrawerSettings.DrawerHeight;
					_drawerLayout.TranslationX = 0;
					if (_isDrawerOpen)
					{
						SetVisibility(true);
						_greyOverlayGrid.TranslationX = 0;
						_drawerLayout.TranslationY = -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2));
						if (DrawerSettings.Transition == Transition.SlideOnTop && _mainContentGrid != null)
						{
							_mainContentGrid.TranslationY = 0;
							_mainContentGrid.TranslationX = 0;
							_greyOverlayGrid.TranslationY = 0;
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							_drawerLayout.TranslationY -= _drawerMoveTop;
							_greyOverlayGrid.TranslationY = DrawerSettings.DrawerHeight;
							_mainContentGrid.TranslationY = DrawerSettings.DrawerHeight;
							_mainContentGrid.TranslationX = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !_mainContentGrid.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
						{
							SetVisibility(false);
						}

						_greyOverlayGrid.TranslationX = ScreenWidth;
						if (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null)
						{
							_drawerLayout.TranslationY = -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop;
							_mainContentGrid.TranslationY = 0;
							_mainContentGrid.BackgroundColor = _mainContentGrid.BackgroundColor ?? ContentBackgroundColor;
						}
						else
						{
							_drawerLayout.TranslationY = -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight);
							if (DrawerSettings.Transition == Transition.Push && _mainContentGrid != null)
							{
								_mainContentGrid.TranslationY = 0;
								_drawerLayout.TranslationY -= _drawerMoveTop;
							}
						}
					}
				}
				else if (DrawerSettings.Position == Position.Bottom)
				{
					_drawerLayout.WidthRequest = ScreenWidth;
					_drawerLayout.HeightRequest = DrawerSettings.DrawerHeight;
					_drawerLayout.TranslationX = 0;
					if (_isDrawerOpen)
					{
						SetVisibility(true);
						_greyOverlayGrid.TranslationX = 0;
						_drawerLayout.TranslationY = (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2);
						if (DrawerSettings.Transition == Transition.SlideOnTop && _mainContentGrid != null)
						{
							_mainContentGrid.TranslationY = 0;
							_mainContentGrid.TranslationX = 0;
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							_greyOverlayGrid.TranslationY = -DrawerSettings.DrawerHeight;
							_mainContentGrid.TranslationY = -DrawerSettings.DrawerHeight;
							_mainContentGrid.TranslationX = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !_mainContentGrid.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
						{
							SetVisibility(false);
						}

						_greyOverlayGrid.TranslationX = ScreenWidth;

						if (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null)
						{
							_drawerLayout.TranslationY = (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2);
							_mainContentGrid.TranslationY = 0;
							_mainContentGrid.BackgroundColor = _mainContentGrid.BackgroundColor ?? ContentBackgroundColor;
						}
						else
						{
							_drawerLayout.TranslationY = (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2);
							if (DrawerSettings.Transition == Transition.Push && _mainContentGrid != null)
							{
								_mainContentGrid.TranslationY = 0;
							}
						}
					}

					_touchBottomThreshold = ScreenHeight - DrawerSettings.TouchThreshold;
				}
			}
		}

#if !WINDOWS
		void UpdateDrawerFlowDirection()
		{
			if (_drawerLayout != null && _mainContentGrid != null)
			{
				if (_isRTL)
				{
					_drawerLayout.FlowDirection = _mainContentGrid.FlowDirection = FlowDirection.RightToLeft;
				}
				else
				{
					_drawerLayout.FlowDirection = _mainContentGrid.FlowDirection = FlowDirection.LeftToRight;
				}
			}
		}
#endif

		/// <summary>
		/// This method is used to update the animation duration.
		/// </summary>
		/// <param name="newValue">newValue.</param>
		void DurationPropertyUpdate(double newValue)
		{
			if (newValue <= 0)
			{
				DrawerSettings.Duration = 1;
			}
		}

		/// <summary>
		/// This method is used to update the drawer content background.
		/// </summary>
		void UpdateContentBackground()
		{
			if (_drawerLayout != null && DrawerSettings != null)
			{
				_drawerLayout.BackgroundColor = DrawerSettings.ContentBackground;
			}
		}

		/// <summary>
		/// This method employed to translate the drawer horizontally (along the X-axis) when swiping from the sides.
		/// </summary>
		void TranslateDrawerXPosition()
		{
			var difference = _newPoint.X - _oldPoint.X;
			if (_greyOverlayGrid != null && _drawerLayout != null)
			{
#if !WINDOWS
				if ((DrawerSettings.Position == Position.Left && !_isRTL) || (DrawerSettings.Position == Position.Right && _isRTL))
#else
				if (DrawerSettings.Position == Position.Left)
#endif
				{
					// Method to handle the swipe action for the left drawer
					HandleLeftDrawerSwipe(difference);
				}
#if !WINDOWS
				else if ((DrawerSettings.Position == Position.Right && !_isRTL) || (DrawerSettings.Position == Position.Left && _isRTL))
#else
				else if (DrawerSettings.Position == Position.Right)
#endif
				{
					// Method to handle the swipe action for the right drawer
					HandleRightDrawerSwipe(difference);
				}
			}
		}

		void HandleLeftDrawerSwipe(double difference)
		{
			// Update remaining drawer width based on swipe difference
			_remainDrawerWidth += difference;
			ValidateRemainDrawerWidth();

			if (!_isDrawerOpen)
			{
				// Check if it's the first swipe action to open the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
				{
					SetDrawerOpeningEvent();
					if (!_cancelOpenEventArgs.Cancel)
					{
						SetVisibility(true);
					}
				}

				if (!_cancelOpenEventArgs.Cancel)
				{
					LeftDrawerSwipe(difference);
				}
			}
			else
			{
				// Check if it's the first swipe action to close the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
				{
					OnDrawerClosing(_cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!_cancelCloseEventArgs.Cancel)
				{
					LeftDrawerSwipe(difference);
				}
			}
		}

		void HandleRightDrawerSwipe(double difference)
		{
			// Update remaining drawer width based on swipe difference
			_remainDrawerWidth -= difference;
			ValidateRemainDrawerWidth();

			if (!_isDrawerOpen)
			{
				// Check if it's the first swipe action to open the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
				{
					SetDrawerOpeningEvent();
					if (!_cancelOpenEventArgs.Cancel)
					{
						SetVisibility(true);
					}
				}

				if (!_cancelOpenEventArgs.Cancel)
				{
					RightDrawerSwipe(difference);
				}
			}
			else
			{
				// Check if it's the first swipe action to close the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
				{
					OnDrawerClosing(_cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!_cancelCloseEventArgs.Cancel)
				{
					RightDrawerSwipe(difference);
				}
			}
		}

		/// <summary>
		/// This method employed to translate the drawer vertically (along the Y-axis) when swiping from the sides.
		/// </summary>
		void TranslateDrawerYPosition()
		{
			var difference = _newPoint.Y - _oldPoint.Y;
			if (DrawerSettings.Position == Position.Top)
			{
				// Method to handle the swipe action for the top drawer
				HandleTopDrawerSwipe(difference);
			}
			else if (DrawerSettings.Position == Position.Bottom)
			{
				// Method to handle the swipe action for the bottom drawer
				HandleBottomDrawerSwipe(difference);
			}
		}

		void HandleTopDrawerSwipe(double difference)
		{
			if (_greyOverlayGrid != null && _drawerLayout != null)
			{
				_remainDrawerHeight += difference;
				ValidateRemainDrawerHeight();

				if (!_isDrawerOpen)
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
					{
						SetDrawerOpeningEvent();
						if (!_cancelOpenEventArgs.Cancel)
						{
							SetVisibility(true);
						}
#if WINDOWS
						if (!_cancelOpenEventArgs.Cancel && DrawerSettings.Transition == Transition.Push)
						{
							_drawerLayout.TranslationY += _toolBarHeight;
						}
#elif ANDROID || IOS

						if (!_cancelOpenEventArgs.Cancel && DrawerSettings.Transition == Transition.Push)
						{
							_drawerLayout.TranslationY += _drawerMoveTop;
						}
#endif
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						TopDrawerSwipe(difference);
					}
				}
				else
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
					{
						OnDrawerClosing(_cancelCloseEventArgs);
						FirstMoveActionCompleted();
					}

					if (!_cancelCloseEventArgs.Cancel)
					{
						TopDrawerSwipe(difference);
					}
				}
			}
		}

		void HandleBottomDrawerSwipe(double difference)
		{
			if (_greyOverlayGrid != null && _drawerLayout != null)
			{
				_remainDrawerHeight -= difference;
				ValidateRemainDrawerHeight();

				if (!_isDrawerOpen)
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
					{
						SetDrawerOpeningEvent();
						if (!_cancelOpenEventArgs.Cancel)
						{
							SetVisibility(true);
						}
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						BottomDrawerSwipe(difference);
					}
				}
				else
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
					{
						OnDrawerClosing(_cancelCloseEventArgs);
						FirstMoveActionCompleted();
					}

					if (!_cancelCloseEventArgs.Cancel)
					{
						BottomDrawerSwipe(difference);
					}
				}
			}
		}

		/// <summary>
		/// This method is used to update the drawer after swipe complete.
		/// </summary>
		void CompletedDrawerSwipe()
		{
			if (DrawerSettings != null)
			{
#if !WINDOWS
				if ((DrawerSettings.Position == Position.Left && !_isRTL) || (DrawerSettings.Position == Position.Right && _isRTL))
#else
				if (DrawerSettings.Position == Position.Left)
#endif
				{
					HandleLeftSwipeCompletion();
				}
#if !WINDOWS
				else if ((DrawerSettings.Position == Position.Right && !_isRTL) || (DrawerSettings.Position == Position.Left && _isRTL))
#else
				else if (DrawerSettings.Position == Position.Right)
#endif
				{
					HandleRightSwipeCompletion();
				}
				else if (DrawerSettings.Position == Position.Top)
				{
					HandleTopSwipeCompletion();
				}
				else if (DrawerSettings.Position == Position.Bottom)
				{
					HandleBottomSwipeCompletion();
				}
			}
		}

		void HandleLeftSwipeCompletion()
		{
			if (_velocityX > 500)
			{
				HandleLeftSwipeIn();
			}
			else if (_velocityX < -500)
			{
				HandleLeftSwipeOut();
			}
			else
			{
				HandleLeftSwipeByPosition();
			}
		}

		void HandleLeftSwipeIn()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationX == DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerLeftIn();
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationX == 0 && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerLeftIn();
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleLeftSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (_mainContentGrid.TranslationX == 0)
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerLeftOut();
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationX == -DrawerSettings.DrawerWidth)
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerLeftOut();
							_isTransitionDifference = false;
						}
					}
				}
			}
		}

		void HandleLeftSwipeByPosition()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null)
			{
				if (_drawerLayout.TranslationX >= (-DrawerSettings.DrawerWidth / 2) && DrawerSettings.Transition != Transition.Reveal)
				{
					if (_drawerLayout.TranslationX == 0 && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerLeftIn();
					}
				}
				else if (_mainContentGrid != null && _mainContentGrid.TranslationX >= DrawerSettings.DrawerWidth / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationX == DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerLeftIn();
					}
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationX != -DrawerSettings.DrawerWidth && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null && _mainContentGrid.TranslationX != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerLeftOut();
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleRightSwipeCompletion()
		{
			if (_velocityX < -500)
			{
				HandleRightSwipeIn();
			}
			else if (_velocityX > 500)
			{
				HandleRightSwipeOut();
			}
			else
			{
				HandleRightSwipeByPosition();
			}
		}

		void HandleRightSwipeIn()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationX == -DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerRightIn();
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationX == (ScreenWidth - DrawerSettings.DrawerWidth) && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerRightIn();
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleRightSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (_mainContentGrid.TranslationX == 0)
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerRightOut();
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationX == ScreenWidth)
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerRightOut();
							_isTransitionDifference = false;
						}
					}
				}
			}
		}

		void HandleRightSwipeByPosition()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null)
			{
				if (_remainDrawerWidth >= (-DrawerSettings.DrawerWidth / 2) && DrawerSettings.Transition != Transition.Reveal)
				{
					if (_drawerLayout.TranslationX == (ScreenWidth - DrawerSettings.DrawerWidth) && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerRightIn();
						_isTransitionDifference = false;
					}
				}
				else if (_mainContentGrid != null && _mainContentGrid.TranslationX <= -DrawerSettings.DrawerWidth / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationX == -DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerRightIn();
						_isTransitionDifference = false;
					}
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationX != ScreenWidth && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null && _mainContentGrid.TranslationX != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerRightOut();
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleTopSwipeCompletion()
		{
			if (_velocityY > 500)
			{
				HandleTopSwipeIn();
			}
			else if (_velocityY < -500)
			{
				HandleTopSwipeOut();
			}
			else
			{
				HandleTopSwipeByPosition();
			}
		}

		void HandleTopSwipeIn()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationY == DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerTopIn();
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationY == -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerTopIn();
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleTopSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (_mainContentGrid.TranslationY == 0)
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerTopOut();
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationY == -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop)
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerTopOut();
							_isTransitionDifference = false;
						}
					}
				}
			}
		}

		void HandleTopSwipeByPosition()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null)
			{
				if (_remainDrawerHeight >= (-DrawerSettings.DrawerHeight / 2) && DrawerSettings.Transition != Transition.Reveal)
				{
					if (_drawerLayout.TranslationY == -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerTopIn();
					}
					_isTransitionDifference = false;
				}
				else if (_mainContentGrid != null && _mainContentGrid.TranslationY >= DrawerSettings.DrawerHeight / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationY == DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerTopIn();
					}
					_isTransitionDifference = false;
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationY != -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight) && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null && _mainContentGrid.TranslationY != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerTopOut();
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleBottomSwipeCompletion()
		{
			if (_velocityY < -500)
			{
				HandleBottomSwipeIn();
			}
			else if (_velocityY > 500)
			{
				HandleBottomSwipeOut();
			}
			else
			{
				HandleBottomSwipeByPosition();
			}
		}

		void HandleBottomSwipeIn()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationY == -DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerBottomIn();
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationY == (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2) && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerBottomIn();
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleBottomSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && _mainContentGrid != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (_mainContentGrid.TranslationY == 0)
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerBottomOut();
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationY == (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2))
						{
							UpdateToggleOutEvent();
						}
						else
						{
							DrawerBottomOut();
							_isTransitionDifference = false;
						}
					}
				}
			}
		}

		void HandleBottomSwipeByPosition()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null)
			{
				if (_remainDrawerHeight >= (-DrawerSettings.DrawerHeight / 2) && DrawerSettings.Transition != Transition.Reveal)
				{
					if (_drawerLayout.TranslationY == (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2) - DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerBottomIn();
					}
					_isTransitionDifference = false;
				}
				else if (_mainContentGrid != null && _mainContentGrid.TranslationY <= -DrawerSettings.DrawerHeight / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (_mainContentGrid.TranslationY == -DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent();
					}
					else
					{
						DrawerBottomIn();
					}
					_isTransitionDifference = false;
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationY != (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid != null && _mainContentGrid.TranslationY != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerBottomOut();
						_isTransitionDifference = false;
					}
				}
			}
		}

		/// <summary>
		/// This method updates the event-handling logic for toggling the drawer in an inward direction.
		/// </summary>
		void UpdateToggleInEvent()
		{
			_toggledEventArgs.IsOpen = true;
			_isDrawerOpen = true;
			IsOpen = true;
			_isTransitionDifference = false;
			OnDrawerOpened(new EventArgs());
			OnDrawerToggled(_toggledEventArgs);
		}

		/// <summary>
		/// This method updates the event-handling logic for toggling the drawer in an outward direction.
		/// </summary>
		void UpdateToggleOutEvent()
		{
			if (_greyOverlayGrid != null)
			{
				_toggledEventArgs.IsOpen = false;
				_isDrawerOpen = false;
				IsOpen = false;
				_greyOverlayGrid.TranslationX = ScreenWidth;
				SetVisibility(false);
				_isTransitionDifference = false;
				OnDrawerClosed(new EventArgs());
				OnDrawerToggled(_toggledEventArgs);
			}
		}

		/// <summary>
		/// This method is used to swipe the drawer to or from the left side.
		/// </summary>
		/// <param name="difference">The distance of the swipe.</param>
		void LeftDrawerSwipe(double difference)
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(_drawerLayout.TranslationX);
					_drawerLayout.TranslationX = Math.Clamp(_drawerLayout.TranslationX + difference, -DrawerSettings.DrawerWidth, 0);
					transitionDifference = Math.Abs(Math.Round(_drawerLayout.TranslationX) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(_mainContentGrid.TranslationX);
				}

				if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
				{
					_greyOverlayGrid.TranslationX = Math.Clamp(_greyOverlayGrid.TranslationX + difference, 0, DrawerSettings.DrawerWidth);
					_mainContentGrid.TranslationX = Math.Clamp(_mainContentGrid.TranslationX + difference, 0, DrawerSettings.DrawerWidth);
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(_mainContentGrid.TranslationX) - oldTransition);
					}

#if WINDOWS
					if (_mainContentGrid.TranslationX != 0 && _greyOverlayGrid.TranslationX != 0)
					{
						SetVisibility(true);
					}
#endif
				}

#if WINDOWS
				if (DrawerSettings.Transition == Transition.SlideOnTop && _drawerLayout.TranslationX != -DrawerSettings.DrawerWidth)
				{
					SetVisibility(true);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerWidth * (_opacity / DrawerSettings.DrawerWidth)) + _opacity;
			}
		}

		/// <summary>
		/// This method is used to swipe the drawer to or from the right side.
		/// </summary>
		/// <param name="difference">The distance of the swipe.</param>
		void RightDrawerSwipe(double difference)
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(_drawerLayout.TranslationX);
					_drawerLayout.TranslationX = Math.Clamp(_drawerLayout.TranslationX + difference, ScreenWidth - DrawerSettings.DrawerWidth, ScreenWidth);
					transitionDifference = Math.Abs(Math.Round(_drawerLayout.TranslationX) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(_mainContentGrid.TranslationX);
				}

				if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
				{
					_greyOverlayGrid.TranslationX = Math.Clamp(_greyOverlayGrid.TranslationX + difference, -DrawerSettings.DrawerWidth, 0);
					_mainContentGrid.TranslationX = Math.Clamp(_mainContentGrid.TranslationX + difference, -DrawerSettings.DrawerWidth, 0);
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(_mainContentGrid.TranslationX) - oldTransition);
					}

#if WINDOWS
					if (_mainContentGrid.TranslationX != 0 && _greyOverlayGrid.TranslationX != 0)
					{
						SetVisibility(true);
					}
#endif
				}

#if WINDOWS
				if (DrawerSettings.Transition == Transition.SlideOnTop && _drawerLayout.TranslationX != ScreenWidth)
				{
					SetVisibility(true);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerWidth * (_opacity / DrawerSettings.DrawerWidth)) + _opacity;
			}
		}

		/// <summary>
		/// This method is used to swipe the drawer to or from the top side.
		/// </summary>
		/// <param name="difference">The distance of the swipe.</param>
		void TopDrawerSwipe(double difference)
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(_drawerLayout.TranslationY);
					_drawerLayout.TranslationY = Math.Clamp(_drawerLayout.TranslationY + difference, -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight), -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop);
					transitionDifference = Math.Abs(Math.Round(_drawerLayout.TranslationY) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(_mainContentGrid.TranslationY);
				}

				if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
				{
					_greyOverlayGrid.TranslationY = Math.Clamp(_greyOverlayGrid.TranslationY + difference, 0, DrawerSettings.DrawerHeight);
					_mainContentGrid.TranslationY = Math.Clamp(_mainContentGrid.TranslationY + difference, 0, DrawerSettings.DrawerHeight);
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(_mainContentGrid.TranslationY) - oldTransition);
					}

#if WINDOWS
					if (_mainContentGrid.TranslationY != 0 && _greyOverlayGrid.TranslationY != 0)
					{
						SetVisibility(true);
					}
#endif
				}

#if WINDOWS
				if (DrawerSettings.Transition == Transition.SlideOnTop && _drawerLayout.TranslationY != -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight))
				{
					SetVisibility(true);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerHeight * (_opacity / DrawerSettings.DrawerHeight)) + _opacity;
			}
		}

		/// <summary>
		/// Methods to ensure the transition difference.
		/// </summary>
		/// <param name="transitionDifference"></param>
		void SetIsTransitionDifference(double transitionDifference)
		{
			if (transitionDifference > 0)
			{
				_isTransitionDifference = true;
			}
		}

		/// <summary>
		/// This method is used to swipe the drawer to or from the bottom side.
		/// </summary>
		/// <param name="difference">The distance of the swipe.</param>
		void BottomDrawerSwipe(double difference)
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(_drawerLayout.TranslationY);
					_drawerLayout.TranslationY = Math.Clamp(_drawerLayout.TranslationY + difference, (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2), (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2));
					transitionDifference = Math.Abs(Math.Round(_drawerLayout.TranslationY) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(_mainContentGrid.TranslationY);
				}

				if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
				{
					_greyOverlayGrid.TranslationY = Math.Clamp(_greyOverlayGrid.TranslationY + difference, -DrawerSettings.DrawerHeight, 0);
					_mainContentGrid.TranslationY = Math.Clamp(_mainContentGrid.TranslationY + difference, -DrawerSettings.DrawerHeight, 0);
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(_mainContentGrid.TranslationY) - oldTransition);
					}

#if WINDOWS
					if (_mainContentGrid.TranslationY != 0 && _greyOverlayGrid.TranslationY != 0)
					{
						SetVisibility(true);
					}
#endif
				}

#if WINDOWS
				if (DrawerSettings.Transition == Transition.SlideOnTop && _drawerLayout.TranslationY != (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2))
				{
					SetVisibility(true);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerHeight * (_opacity / DrawerSettings.DrawerHeight)) + _opacity;
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the left side.
		/// </summary>
		void DrawerLeftIn()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				if (_drawerLayout.TranslationX != 0 || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid.TranslationX != DrawerSettings.DrawerWidth))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent();
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration;
						var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
						if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
						{
							currentDuration = (Math.Abs(_drawerLayout.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
						}
						else
						{
							currentDuration = (Math.Abs(DrawerSettings.DrawerWidth - _greyOverlayGrid.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationX = d, _drawerLayout.TranslationX, 0);
						Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (DrawerSettings.Transition == Transition.SlideOnTop)
						{
							_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
							});
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, DrawerSettings.DrawerWidth);
							Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationX = d, _mainContentGrid.TranslationX, DrawerSettings.DrawerWidth);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
							_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							DrawerPushAnimation(currentDuration, drawerAnimation);
						}

						_isDrawerOpen = true;
						_toggledEventArgs.IsOpen = true;
						IsOpen = true;
					}
					else
					{
						_greyOverlayGrid.TranslationX = ScreenWidth;
					}
				}
				else if (_drawerLayout.TranslationX == 0)
				{
					_isDrawerOpen = true;
				}
			}
		}

		/// <summary>
		///  Method used to provide the animation to the drawer when moving towards the left side.
		/// </summary>
		void DrawerLeftOut()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(_cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!_cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
					if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
					{
						currentDuration = (Math.Abs(DrawerSettings.DrawerWidth + _drawerLayout.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
					}
					else
					{
						currentDuration = (Math.Abs(_greyOverlayGrid.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationX = d, _drawerLayout.TranslationX, -DrawerSettings.DrawerWidth);
					Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (DrawerSettings.Transition == Transition.SlideOnTop)
					{
						_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_drawerLayout.TranslationX == -DrawerSettings.DrawerWidth)
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
					}
					else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
					{
						Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationX = d, _mainContentGrid.TranslationX, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
						_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_mainContentGrid.TranslationX == 0)
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						DrawerPushAnimation(currentDuration, drawerAnimation);
					}

					_isDrawerOpen = false;
					_toggledEventArgs.IsOpen = false;
					IsOpen = false;
				}
			}
		}

		void OnDrawerClosedToggledEvent()
		{
			OnDrawerClosed(new EventArgs());
			OnDrawerToggled(_toggledEventArgs);
#if WINDOWS
			if (!_isDrawerOpen)
#endif
			{
				SetVisibility(false);
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the right side.
		/// </summary>
		void DrawerRightIn()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				if (_drawerLayout.TranslationX != ScreenWidth - DrawerSettings.DrawerWidth || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid.TranslationX != -DrawerSettings.DrawerWidth))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent();
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration;
						var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
						if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
						{
							currentDuration = (Math.Abs((ScreenWidth - DrawerSettings.DrawerWidth) - _drawerLayout.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
						}
						else
						{
							currentDuration = (Math.Abs(DrawerSettings.DrawerWidth + _mainContentGrid.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationX = d, _drawerLayout.TranslationX, ScreenWidth - DrawerSettings.DrawerWidth);
						Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (DrawerSettings.Transition == Transition.SlideOnTop)
						{
							_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)DrawerSettings.Duration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(DrawerSettings.Duration / 2), easing: animationEasing);
							});
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, -DrawerSettings.DrawerWidth);
							Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationX = d, _mainContentGrid.TranslationX, -DrawerSettings.DrawerWidth);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(DrawerSettings.Duration / 2), easing: animationEasing);
							_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							DrawerPushAnimation(currentDuration, drawerAnimation);
						}

						_isDrawerOpen = true;
						_toggledEventArgs.IsOpen = true;
						IsOpen = true;
					}
					else
					{
						_greyOverlayGrid.TranslationX = ScreenWidth;
					}
				}
				else if (_drawerLayout.TranslationX == ScreenWidth - DrawerSettings.DrawerWidth)
				{
					_isDrawerOpen = true;
					_toggledEventArgs.IsOpen = true;
				}
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving towards the right side.
		/// </summary>
		void DrawerRightOut()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(_cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!_cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
					if (DrawerSettings.Transition == Transition.SlideOnTop || DrawerSettings.Transition == Transition.Push)
					{
						currentDuration = (Math.Abs(ScreenWidth - _drawerLayout.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
					}
					else
					{
						currentDuration = (Math.Abs(_mainContentGrid.TranslationX) / DrawerSettings.DrawerWidth) * DrawerSettings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationX = d, _drawerLayout.TranslationX, ScreenWidth);
					Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (DrawerSettings.Transition == Transition.SlideOnTop)
					{
						_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)DrawerSettings.Duration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_drawerLayout.TranslationX == ScreenWidth)
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(DrawerSettings.Duration / 2), easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
					}
					else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
					{
						Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationX = d, _mainContentGrid.TranslationX, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
						_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_mainContentGrid.TranslationX == 0)
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						DrawerPushAnimation(currentDuration, drawerAnimation);
					}

					_isDrawerOpen = false;
					_toggledEventArgs.IsOpen = false;
					IsOpen = false;
				}
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the top side.
		/// </summary>
		void DrawerTopIn()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				if (_drawerLayout.TranslationY != -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid.TranslationY != DrawerSettings.DrawerHeight))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent();
						if (DeviceInfo.Platform == DevicePlatform.WinUI && DrawerSettings.Transition != Transition.Reveal)
						{
							_drawerLayout.TranslationY += _toolBarHeight;
						}
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration = 0;
						double pushDuration = 0;
						var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
						if (DrawerSettings.Transition != Transition.Reveal)
						{
							currentDuration = Math.Abs(-((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerLayout.TranslationY) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
							if (DrawerSettings.Transition == Transition.Push)
							{
								pushDuration = Math.Abs((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2) + _drawerLayout.TranslationY) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
							}
						}
						else
						{
							currentDuration = Math.Abs(DrawerSettings.DrawerHeight - _mainContentGrid.TranslationY) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationY = d, _drawerLayout.TranslationY, -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop);
						Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (DrawerSettings.Transition == Transition.SlideOnTop)
						{
							_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
							});
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, DrawerSettings.DrawerHeight);
							Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationY = d, _mainContentGrid.TranslationY, DrawerSettings.DrawerHeight);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
							_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							if (DrawerSettings.Transition == Transition.Push)
							{
								_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)pushDuration, easing: animationEasing);
							}
						}

						_isDrawerOpen = true;
						_toggledEventArgs.IsOpen = true;
						IsOpen = true;
					}
					else
					{
						_greyOverlayGrid.TranslationX = ScreenWidth;
					}
				}
				else if (_drawerLayout.TranslationY == -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop)
				{
					_isDrawerOpen = true;
					_toggledEventArgs.IsOpen = true;
				}
			}
		}

		/// <summary>
		///  Method used to provide the animation to the drawer when moving towards the top side.
		/// </summary>
		void DrawerTopOut()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(_cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!_cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					double pushDuration = 0;
					var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
					if (DrawerSettings.Transition != Transition.Reveal)
					{
						currentDuration = (Math.Abs(((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2)) + _drawerLayout.TranslationY) / DrawerSettings.DrawerHeight) * DrawerSettings.Duration;
						if (DrawerSettings.Transition == Transition.Push)
						{
							pushDuration = Math.Abs((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _drawerLayout.TranslationY + _toolBarHeight) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
						}
					}
					else
					{
						currentDuration = Math.Abs(_mainContentGrid.TranslationY) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationY = d, _drawerLayout.TranslationY, -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight));
					Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (DrawerSettings.Transition == Transition.SlideOnTop)
					{
						_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)DrawerSettings.Duration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_drawerLayout.TranslationY == -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight))
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(DrawerSettings.Duration / 2), easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
					}
					else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
					{
						Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, 0);
						Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationY = d, _mainContentGrid.TranslationY, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
						_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_mainContentGrid.TranslationY == 0)
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						if (DrawerSettings.Transition == Transition.Push)
						{
							_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)pushDuration, easing: animationEasing);
						}
					}

					_isDrawerOpen = false;
					_toggledEventArgs.IsOpen = false;
					IsOpen = false;
				}
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the bottom side.
		/// </summary>
		void DrawerBottomIn()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				if (_drawerLayout.TranslationY != (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2) || (DrawerSettings.Transition == Transition.Reveal && _mainContentGrid.TranslationY != -DrawerSettings.DrawerHeight))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent();
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration = 0;
						var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
						if (DrawerSettings.Transition != Transition.Reveal)
						{
							currentDuration = Math.Abs((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2) - _drawerLayout.TranslationY) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
						}
						else
						{
							currentDuration = Math.Abs(DrawerSettings.DrawerHeight + _mainContentGrid.TranslationY) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationY = d, _drawerLayout.TranslationY, (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2));
						Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (DrawerSettings.Transition == Transition.SlideOnTop)
						{
							_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
							});
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
						{
							Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, -DrawerSettings.DrawerHeight);
							Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationY = d, _mainContentGrid.TranslationY, -DrawerSettings.DrawerHeight);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
							_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent();
							});
							DrawerPushAnimation(currentDuration, drawerAnimation);
						}

						_isDrawerOpen = true;
						_toggledEventArgs.IsOpen = true;
						IsOpen = true;
					}
					else
					{
						_greyOverlayGrid.TranslationX = ScreenWidth;
					}
				}
				else if (_drawerLayout.TranslationY == (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2))
				{
					_isDrawerOpen = true;
					_toggledEventArgs.IsOpen = true;
				}
			}
		}

		/// <summary>
		///  Method used to provide the animation to the drawer when moving towards the bottom side.
		/// </summary>
		void DrawerBottomOut()
		{
			if (_greyOverlayGrid != null && _drawerLayout != null && _mainContentGrid != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(_cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!_cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
					if (DrawerSettings.Transition != Transition.Reveal)
					{
						currentDuration = Math.Abs((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) - _drawerLayout.TranslationY) / DrawerSettings.DrawerHeight * DrawerSettings.Duration;
					}
					else
					{
						currentDuration = (Math.Abs(_mainContentGrid.TranslationY) / DrawerSettings.DrawerHeight) * DrawerSettings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					Animation drawerAnimation = new Animation(d => _drawerLayout.TranslationY = d, _drawerLayout.TranslationY, (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2));
					Animation greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					Animation greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (DrawerSettings.Transition == Transition.SlideOnTop)
					{
						_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_drawerLayout.TranslationY == (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2))
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
					}
					else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && _mainContentGrid != null)
					{
						Animation greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, 0);
						Animation contentViewTranslatePushAnimation = new Animation(d => _mainContentGrid.TranslationY = d, _mainContentGrid.TranslationY, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: animationEasing);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: animationEasing);
						_mainContentGrid.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: animationEasing, finished: (v, e) =>
						{
#if WINDOWS
							if (_mainContentGrid.TranslationY == 0)
#endif
							{
								OnDrawerClosedToggledEvent();
							}
						});
						DrawerPushAnimation(currentDuration, drawerAnimation);
					}

					_isDrawerOpen = false;
					_toggledEventArgs.IsOpen = false;
					IsOpen = false;
				}
			}
		}

		/// <summary>
		/// This method is used to signal that the first move action has started.
		/// </summary>
		void FirstMoveActionStarted()
		{
			_isPressed = true;
			_actionFirstMoveClose = true;
			_actionFirstMoveOpen = true;
		}

		/// <summary>
		/// This method is used to signal that the first move action has completed.
		/// </summary>
		void FirstMoveActionCompleted()
		{
			_actionFirstMoveOpen = false;
			_actionFirstMoveClose = false;
		}

		/// <summary>
		/// This method is used to provide the visibility for the drawerLayout and greyOverlayGrid.
		/// </summary>
		/// <param name="isVisible"></param>
		void SetVisibility(bool isVisible)
		{
			if (_drawerLayout != null && _greyOverlayGrid != null)
			{
				_drawerLayout.IsVisible = isVisible;
				_greyOverlayGrid.IsVisible = isVisible;

				// Workaround for framework issue https://github.com/dotnet/maui/issues/27434: Content inside DrawerContentView does not resize correctly on macOS and iOS.
				// This forces an invalidation of the measure to ensure proper layout updates.
#if MACCATALYST || IOS
				if (DrawerSettings is not null && DrawerSettings.DrawerContentView is not null)
				{
					if (DrawerSettings.DrawerContentView is View view)
					{
						if (view.IsVisible)
						{
							(view as IView)?.InvalidateMeasure();
						}
					}
				}
#endif
			}
		}

		/// <summary>
		/// Methods to validate current duration.
		/// </summary>
		/// <param name="currentDuration">Current duration.</param>
		/// <returns></returns>
		static double ValidateCurrentDuration(double currentDuration)
		{
			if (currentDuration <= 0)
			{
				currentDuration = 1;
			}

			return currentDuration;
		}

		/// <summary>
		/// Methods to set zero to the X translation delta of GreyOverLayGrid.
		/// </summary>
		void SetGreyOverLayGridTranslationX()
		{
			if (_greyOverlayGrid != null && _greyOverlayGrid.TranslationX == ScreenWidth)
			{
				_greyOverlayGrid.TranslationX = 0;
			}
		}

		/// <summary>
		/// Methods to validate remain drawer height.
		/// </summary>
		void ValidateRemainDrawerHeight()
		{
			if (_remainDrawerHeight < -DrawerSettings.DrawerHeight)
			{
				_remainDrawerHeight = -DrawerSettings.DrawerHeight;
			}
			else if (_remainDrawerHeight > 0)
			{
				_remainDrawerHeight = 0;
			}
		}

		/// <summary>
		/// Methods to validate remain drawer width.
		/// </summary>
		void ValidateRemainDrawerWidth()
		{
			if (_remainDrawerWidth < -DrawerSettings.DrawerWidth)
			{
				_remainDrawerWidth = -DrawerSettings.DrawerWidth;
			}
			else if (_remainDrawerWidth > 0)
			{
				_remainDrawerWidth = 0;
			}
		}

		void SetDrawerOpeningEvent()
		{
			OnDrawerOpening(_cancelOpenEventArgs);
			FirstMoveActionCompleted();
		}

		void OnDrawerOpenedToggledEvent()
		{
			OnDrawerOpened(new EventArgs());
			OnDrawerToggled(_toggledEventArgs);
		}

		void DrawerPushAnimation(double currentDuration, Animation drawerAnimation)
		{
			var animationEasing = this.DrawerSettings.AnimationEasing ?? Easing.Linear;
			if (DrawerSettings.Transition == Transition.Push)
			{
				_drawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: animationEasing);
			}
		}

		#endregion

		#region Override Methods

#if ANDROID
		/// <summary>
		/// Measure Override method
		/// </summary>
		/// <param name="widthConstraint">Width.</param>
		/// <param name="heightConstraint">Height.</param>
		/// <returns>It returns the size</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			if (DrawerSettings != null && _drawerLayout != null)
			{
				if (widthConstraint > 0 && widthConstraint != double.PositiveInfinity && _drawerLayout.WidthRequest != widthConstraint)
				{
					if (DrawerSettings.Position == Position.Bottom || DrawerSettings.Position == Position.Top)
					{
						_drawerLayout.WidthRequest = widthConstraint;
					}
				}

				if (heightConstraint > 0 && heightConstraint != double.PositiveInfinity && _drawerLayout.HeightRequest != heightConstraint)
				{
					if (DrawerSettings.Position == Position.Left || DrawerSettings.Position == Position.Right)
					{
						_drawerLayout.HeightRequest = heightConstraint;
					}
				}
			}

			return base.MeasureContent(widthConstraint, heightConstraint);
		}
#endif

		/// <summary>
		/// OnSizeAllocated method.
		/// </summary>
		/// <exclude/>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			if (width >= 0)
			{
				ScreenWidth = width;
			}

			if (height >= 0)
			{
				if (DeviceInfo.Platform == DevicePlatform.WinUI && Window != null)
				{
					_toolBarHeight = Window.Height - height;
				}

				ScreenHeight = height;
				if (IsLoaded)
				{
					UpdateIsOpen();
				}
			}

			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				_drawerMoveTop = 1;
			}
		}

		/// <summary>
		/// Call when the binding context is changed.
		/// </summary>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (DrawerSettings != null)
			{
				SfNavigationDrawer.SetInheritedBindingContext(DrawerSettings, BindingContext);
			}
		}

#if !WINDOWS
		/// <summary>
		/// Raised when a property value changes.
		/// </summary>
		/// <exclude/>
		/// <param name="propertyName"></param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName == "FlowDirection")
			{
				if (FlowDirection == FlowDirection.RightToLeft)
				{
					_isRTL = true;
				}
				else if (FlowDirection == FlowDirection.LeftToRight)
				{
					_isRTL = false;
				}
				else if (Parent is Layout parentLayout && parentLayout.FlowDirection != FlowDirection.MatchParent)
				{
					_isRTL = parentLayout.FlowDirection == FlowDirection.RightToLeft;
				}
			}

			base.OnPropertyChanged(propertyName);
		}
#endif

		/// <summary>
		/// Raised when handler gets changed.
		/// </summary>
		/// <exclude/>
		protected override void OnHandlerChanged()
		{
#if WINDOWS || IOS || MACCATALYST
			ConfigureTouch();
#endif
			base.OnHandlerChanged();
		}

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Invoked whenever the <see cref="ContentViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnContentViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as SfNavigationDrawer)?.UpdateContentView();
		}

		static void OnDrawerSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var drawer = bindable as SfNavigationDrawer;
			if (drawer != null)
			{
				drawer.OnDefaultDrawerSettingsChanged();
				drawer.DrawerSettings.PropertyChanged -= drawer.OnDefaultDrawerSettings_PropertyChanged;
				drawer.DrawerSettings.PropertyChanged += drawer.OnDefaultDrawerSettings_PropertyChanged;
			}

			if (oldValue != null)
			{
				if (oldValue is DrawerSettings previousSetting)
				{
					previousSetting.PropertyChanged -= drawer!.OnDefaultDrawerSettings_PropertyChanged;
					previousSetting.BindingContext = null;
					previousSetting.Parent = null;
				}
			}
			if (newValue != null)
			{
				if (newValue is DrawerSettings currentSetting && bindable is SfNavigationDrawer sfNavigationDrawer)
				{
					currentSetting.Parent = sfNavigationDrawer;
					SetInheritedBindingContext(sfNavigationDrawer.DrawerSettings, sfNavigationDrawer.BindingContext);
				}
			}

		}

		/// <summary>
		/// Invoked whenever the <see cref="GreyOverlayColorProperty"/> is set.
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnGreyColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNavigationDrawer navigation && navigation != null && navigation._greyOverlayGrid != null)
			{
				navigation._greyOverlayGrid.Background = (Color)newValue;
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ContentBackgroundColorProperty"/> is set.
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnContentBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNavigationDrawer navigation && navigation != null)
			{
				navigation.DrawerSettings.ContentBackground = (Color)newValue;
			}
		}


		/// <summary>	 
		/// Invoked whenever the <see cref="IsOpenProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnIsOpenChanged(BindableObject bindable, object oldValue, object newValue)
		{
			{
				if (bindable is not SfNavigationDrawer drawer)
				{
					return;
				}

				if (drawer.IsLoaded)
				{
					drawer.UpdateIsOpen();
				}
			}
		}

#if !WINDOWS
		/// <summary>
		/// Invoked whenever the <see cref="ContentBackgroundColorProperty"/> is set.
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnFlowDirectionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNavigationDrawer navigation && navigation != null)
			{
				navigation.UpdateDrawerFlowDirection();
				navigation.PositionUpdate();
			}
		}
#endif

		#endregion

		#region Interface Implementation

		/// <summary>
		/// This method used for handle the touch.
		/// </summary>
		/// <param name="e">e.</param>
		public static void OnTouch(PointerEventArgs e)
		{
			// throw new NotImplementedException();
		}

		/// <summary>
		/// This method used for handle the tap.
		/// </summary>
		public void OnTap(TapEventArgs e)
		{
		}

		/// <inheritdoc/>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfNavigationDrawerStyle();
		}

		/// <inheritdoc/>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
			// throw new NotImplementedException();
		}

		/// <inheritdoc/>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
			// throw new NotImplementedException();
		}

		#endregion

		#region Events

		/// <summary>
		/// It is drawer closed event.
		/// </summary>
		public event EventHandler<EventArgs>? DrawerClosed;

		/// <summary>
		/// It is drawer opened event.
		/// </summary>
		public event EventHandler<EventArgs>? DrawerOpened;

		/// <summary>
		/// It is drawer toggled event.
		/// </summary>
		public event EventHandler<ToggledEventArgs>? DrawerToggled;

		/// <summary>
		/// It is drawer closing event.
		/// </summary>
		public event EventHandler<CancelEventArgs>? DrawerClosing;

		/// <summary>
		/// It is drawer opening event.
		/// </summary>
		public event EventHandler<CancelEventArgs>? DrawerOpening;

		#endregion
	}
}
