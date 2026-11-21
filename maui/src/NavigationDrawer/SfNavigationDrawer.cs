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

		// Additional fields for swipe state

        internal bool _isPrimaryDrawerSwiping;

        internal bool _isSecondaryDrawerSwiping;

        // Primary drawer fields
        SfGrid? _greyOverlayGrid;

        SfGrid? _drawerLayout;
        
        bool _isDrawerOpen;

        // Secondary drawer fields

        SfGrid? _secondaryDrawerLayout;

        bool _isSecondaryDrawerOpen;

		View? _oldSecondaryHeaderView;

        View? _oldSecondaryFooterView;

		double _screenWidth;

		double _screenHeight;

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

		readonly DrawerCancelEventArgs _cancelOpenEventArgs = new DrawerCancelEventArgs(true);

        readonly DrawerCancelEventArgs _cancelCloseEventArgs = new DrawerCancelEventArgs(true);

        readonly DrawerCancelEventArgs _secondaryCancelOpenEventArgs = new DrawerCancelEventArgs(false);

        readonly DrawerCancelEventArgs _secondaryCancelCloseEventArgs = new DrawerCancelEventArgs(false);

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

		/// <summary>
        /// Identifies the <see cref="SecondaryDrawerSettings"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondaryDrawerSettings"/> bindable property.
        /// </value>
        public static readonly BindableProperty SecondaryDrawerSettingsProperty = 
			BindableProperty.Create
				(nameof(SecondaryDrawerSettings), 
				typeof(DrawerSettings), 
				typeof(SfNavigationDrawer), 
				new DrawerSettings(), 
				BindingMode.Default, 
				null, 
				propertyChanged: OnSecondaryDrawerSettingsChanged);		

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
			InitializeDrawer(true);
			InitializeGreyOverlayGrid();
			InitializeDrawer(false);
			UpdateAllChild();
			PositionUpdate();
			SecondaryPositionUpdate();
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
        /// Gets or sets the SecondaryDrawerSettings, which is a container for various customization options for the secondary drawer, allowing its appearance to be customized.
        /// </summary>
        public DrawerSettings SecondaryDrawerSettings
        {
            get { return (DrawerSettings)GetValue(SecondaryDrawerSettingsProperty); }
            set { SetValue(SecondaryDrawerSettingsProperty, value); }
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
					SecondaryPositionUpdate();
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
					SecondaryPositionUpdate();
				}
			}
		}

		#endregion

		#region Public Methods

        /// <summary>
        /// Methods to open or close the drawer based on the existing state in the SfNavigationDrawer.
        /// </summary>
        public void ToggleDrawer()
        {
            ToggleDrawer(true);
        }

        /// <summary>
        /// Methods to open or close the secondary drawer based on the existing state in the SfNavigationDrawer.
        /// </summary>
        public void ToggleSecondaryDrawer()
        {
            ToggleDrawer(false);
        }

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
		public void ToggleDrawer(bool isPrimaryDrawer)
		{
			 DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
            SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;
            bool isDrawerOpen = isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;

			if (_greyOverlayGrid != null && navigationDrawerLayout != null )
			{
				if ((settings.Position == Position.Top || settings.Position == Position.Bottom) && settings.DrawerHeight <= 0)
				{
					if (isPrimaryDrawer ? !_isSecondaryDrawerOpen : !_isDrawerOpen)
					{
						SetVisibility(false, isPrimaryDrawer);
					}

					return;
				}

				// Mutual exclusivity: If secondary drawer is open, close it first
                if (isPrimaryDrawer && SecondaryDrawerSettings != null && _isSecondaryDrawerOpen)
                {
                    SecondaryDrawerSettings.IsOpen = false;
                }
                // Mutual exclusivity: If primary drawer is open, close it first
                else if (!isPrimaryDrawer && DrawerSettings != null && _isDrawerOpen)
                {
                    DrawerSettings.IsOpen = false;
                }

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

				if (ContentView != null)
				{
					if (ContentView.AnimationIsRunning("contentViewTranslatePushAnimation"))
					{
						ContentView.AbortAnimation("contentViewTranslatePushAnimation");
					}
				}

				_actionFirstMoveOpen = true;
				_actionFirstMoveClose = true;

				if (!isPrimaryDrawer)
                {
                    SecondaryPositionUpdate();
                }

				switch (settings.Position)
				{
					case Position.Left:
						HandleLeftDrawer(isPrimaryDrawer);
						break;

					case Position.Right:
						HandleRightDrawer(isPrimaryDrawer);
						break;

					case Position.Top:
						HandleTopDrawer(isPrimaryDrawer);
						break;

					case Position.Bottom:
						HandleBottomDrawer(isPrimaryDrawer);
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
		internal virtual void OnDrawerClosed(DrawerEventArgs args)
		{
			DrawerClosed?.Invoke(this, args);
		}

		/// <summary>
		/// This method represents the action taken when the drawer is opened.
		/// </summary>
		/// <param name="args">The opened argument.</param>
		internal virtual void OnDrawerOpened(DrawerEventArgs args)
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
		internal virtual void OnDrawerOpening(DrawerCancelEventArgs args)
		{
			DrawerOpening?.Invoke(this, args);
		}

		/// <summary>
		/// This method represents the action taken when the drawer is closing.
		/// </summary>
		/// <param name="args">the closing operation.</param>
		internal virtual void OnDrawerClosing(DrawerCancelEventArgs args)
		{
			DrawerClosing?.Invoke(this, args);
		}

		#endregion

		#region Private Methods

		void HandleLeftDrawer(bool isPrimaryDrawer)
		{
			bool isDrawerOpen = isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;
			if (isDrawerOpen)
			{
#if !WINDOWS
				if (_isRTL)
				{
					DrawerRightOut(isPrimaryDrawer);
				}
				else
#endif
				{
					DrawerLeftOut(isPrimaryDrawer);
				}
			}
			else
			{
				UpdateGridOverlayTranslate(isPrimaryDrawer);
#if !WINDOWS
				if (_isRTL)
				{
					DrawerRightIn(isPrimaryDrawer);
				}
				else
#endif
				{
					DrawerLeftIn(isPrimaryDrawer);
				}
			}
		}

		void HandleRightDrawer(bool isPrimaryDrawer)
		{
			bool isDrawerOpen = isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;
			if (isDrawerOpen)
			{
#if !WINDOWS
				if (_isRTL)
				{
					DrawerLeftOut(isPrimaryDrawer);
				}
				else
#endif
				{
					DrawerRightOut(isPrimaryDrawer);
				}
			}
			else
			{
				UpdateGridOverlayTranslate(isPrimaryDrawer);
#if !WINDOWS
				if (_isRTL)
				{
					DrawerLeftIn(isPrimaryDrawer);
				}
				else
#endif
				{
					DrawerRightIn(isPrimaryDrawer);
				}
			}
		}

		void HandleTopDrawer(bool isPrimaryDrawer)
		{
			bool isDrawerOpen = isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;
			if (isDrawerOpen)
			{
				DrawerTopOut(isPrimaryDrawer);
			}
			else
			{
				UpdateGridOverlayTranslate(isPrimaryDrawer);
				DrawerTopIn(isPrimaryDrawer);
			}
		}

		void HandleBottomDrawer(bool isPrimaryDrawer)
		{
			bool isDrawerOpen = isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;
			if (isDrawerOpen)
			{
				DrawerBottomOut(isPrimaryDrawer);
			}
			else
			{
				UpdateGridOverlayTranslate(isPrimaryDrawer);
				DrawerBottomIn(isPrimaryDrawer);
			}
		}

		/// <summary>
		/// The method used to update all children of the SfNavigationDrawer.
		/// </summary>
		void UpdateAllChild()
		{
#if !WINDOWS
			UpdateDrawerFlowDirection();
            UpdateSecondaryDrawerFlowDirection();
#endif
            var primaryTransition = DrawerSettings?.Transition ?? Transition.SlideOnTop;
            var secondaryTransition = SecondaryDrawerSettings?.Transition ?? Transition.SlideOnTop;
            bool isPrimaryViewsInitialized = DrawerSettings?.DrawerContentView != null || DrawerSettings?.DrawerHeaderView  != null || DrawerSettings?.DrawerFooterView  != null;
            bool isSecondaryViewsInitialized = SecondaryDrawerSettings?.DrawerContentView != null || SecondaryDrawerSettings?.DrawerHeaderView  != null || SecondaryDrawerSettings?.DrawerFooterView  != null;
            // 1) Any REVEAL drawer must be BELOW ContentView
            if (primaryTransition == Transition.Reveal && _drawerLayout != null && isPrimaryViewsInitialized)
                AddChild(_drawerLayout);

            if (secondaryTransition == Transition.Reveal && _secondaryDrawerLayout != null && isSecondaryViewsInitialized)
                AddChild(_secondaryDrawerLayout);

            // 2) ContentView
            if (ContentView != null)
                AddChild(ContentView);

            // 3) Overlays (above content to dim it)
            if (_greyOverlayGrid != null && (isPrimaryViewsInitialized || isSecondaryViewsInitialized))
                AddChild(_greyOverlayGrid);

            // 4) Any NON-REVEAL drawer (SlideOnTop / Push) must be ABOVE overlays
            if (primaryTransition != Transition.Reveal && _drawerLayout != null && isPrimaryViewsInitialized)
                AddChild(_drawerLayout);

            if (secondaryTransition != Transition.Reveal && _secondaryDrawerLayout != null && isSecondaryViewsInitialized)
                AddChild(_secondaryDrawerLayout);
		}

		void AddChild(View? child)
		{
			if (child != null && Children.Contains(child) == false)
			{
				Children.Add(child);
			}
		}

		/// <summary>
		/// This method is used to initialize the drawer layout.
		/// </summary>
 		void InitializeDrawer(bool isPrimaryDrawer)
        {
            DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
            SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;

            if (settings != null)
            {
                navigationDrawerLayout = new SfGrid()
                {
                    BackgroundColor = settings.ContentBackground,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(settings.DrawerHeaderHeight) },
                        new RowDefinition(),
                        new RowDefinition { Height = new GridLength(settings.DrawerFooterHeight) },
                    },
                    RowSpacing = 0,
					IsClippedToBounds = true
                };
            }
            if (isPrimaryDrawer)
            {
                _drawerLayout = navigationDrawerLayout;
            }
            else
            {
                _secondaryDrawerLayout = navigationDrawerLayout;
            }
        }

		/// <summary>
		/// This method is used to initialize the greyOverlayGrid.
		/// </summary>
		void InitializeGreyOverlayGrid()
		{
			_greyOverlayGrid = new SfGrid() { BackgroundColor = GreyOverlayColor, Opacity = 0, };
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

        /// <summary>
        /// Handles touch interaction for the secondary drawer.
        /// </summary>
        private void OnSecondaryHandleTouchInteraction(PointerActions action, Point point)
        {
            // If both drawers share the same position, primary has swipe priority for opening.
            if (SecondaryDrawerSettings != null && DrawerSettings != null &&
                !_isSecondaryDrawerOpen && action == PointerActions.Pressed &&
                SecondaryDrawerSettings.Position == DrawerSettings.Position &&
                (DrawerSettings.DrawerHeaderView !=null || DrawerSettings.DrawerContentView !=null || DrawerSettings.DrawerFooterView!=null))
            {
                return;
            }

            if (SecondaryDrawerSettings!=null && SecondaryDrawerSettings.EnableSwipeGesture)
            {
                switch (action)
                {
                    case PointerActions.Pressed:
                        {
                            _oldPoint = point;
                            _initialTouchPoint = point;
                            _startTime = DateTime.Now;
                            _startPoint = point;

                            if (SecondaryDrawerSettings.Position == Position.Left || SecondaryDrawerSettings.Position == Position.Right)
                            {
                                _remainDrawerWidth = _isSecondaryDrawerOpen ? 0 : -SecondaryDrawerSettings.DrawerWidth;

                                switch (SecondaryDrawerSettings.Position)
                                {
                                    case Position.Left:
#if !WINDOWS
                                        if ((_initialTouchPoint.X <= SecondaryDrawerSettings.TouchThreshold && !_isSecondaryDrawerOpen && !_isRTL) || (_initialTouchPoint.X >= _touchRightThreshold && !_isSecondaryDrawerOpen && _isRTL) || _isSecondaryDrawerOpen)
#else
                                        if ((_initialTouchPoint.X <= SecondaryDrawerSettings.TouchThreshold && !_isSecondaryDrawerOpen)
                                            || _isSecondaryDrawerOpen)
#endif
                                        {
                                            FirstMoveActionStarted();
                                        }
                                        break;
                                    case Position.Right:
#if !WINDOWS
                                        if ((_initialTouchPoint.X >= _touchRightThreshold && !_isSecondaryDrawerOpen && !_isRTL) || (_initialTouchPoint.X <= SecondaryDrawerSettings.TouchThreshold && !_isSecondaryDrawerOpen && _isRTL) || _isSecondaryDrawerOpen)
#else
                                        if ((_initialTouchPoint.X >= ScreenWidth - SecondaryDrawerSettings.TouchThreshold && !_isSecondaryDrawerOpen)
                                            || _isSecondaryDrawerOpen)
#endif
                                        {
                                            FirstMoveActionStarted();
                                        }
                                        break;
                                }
                            }
                            else if (SecondaryDrawerSettings.Position == Position.Top || SecondaryDrawerSettings.Position == Position.Bottom)
                            {
                                _remainDrawerHeight = _isSecondaryDrawerOpen ? 0 : -SecondaryDrawerSettings.DrawerHeight;

                                switch (SecondaryDrawerSettings.Position)
                                {
                                    case Position.Top:
                                        if ((_initialTouchPoint.Y <= SecondaryDrawerSettings.TouchThreshold && !_isSecondaryDrawerOpen)
                                            || _isSecondaryDrawerOpen)
                                        {
                                            FirstMoveActionStarted();
                                        }
                                        break;
                                    case Position.Bottom:
                                        if ((_initialTouchPoint.Y >= ScreenHeight - SecondaryDrawerSettings.TouchThreshold && !_isSecondaryDrawerOpen)
                                            || _isSecondaryDrawerOpen)
                                        {
                                            FirstMoveActionStarted();
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    case PointerActions.Moved:
                        {
                            if (_isPressed && (point.X != _initialTouchPoint.X || point.Y != _initialTouchPoint.Y))
                            {
                                _isMoved = true;
                                _newPoint = point;
                                FindVelocity(point);
                                if (SecondaryDrawerSettings.Position == Position.Left || SecondaryDrawerSettings.Position == Position.Right)
                                {
                                    TranslateDrawerXPosition(false);
                                }
                                else
                                {
                                    TranslateDrawerYPosition(false);
                                }
                                _oldPoint = _newPoint;
                            }
                            break;
                        }
                    case PointerActions.Released:
                        {
                            if (_isPressed && _isMoved)
                            {
                                CompletedSecondaryDrawerSwipe();
                            }
                            else if (_isPressed && _isSecondaryDrawerOpen)
                            {
#if !WINDOWS
                                if (_isRTL)
                                {
                                    if ((_initialTouchPoint.X < ScreenWidth - SecondaryDrawerSettings.DrawerWidth && SecondaryDrawerSettings.Position == Position.Left)
                                    || (_initialTouchPoint.X > SecondaryDrawerSettings.DrawerWidth && SecondaryDrawerSettings.Position == Position.Right)
                                    || (_initialTouchPoint.Y > SecondaryDrawerSettings.DrawerHeight && SecondaryDrawerSettings.Position == Position.Top)
                                    || (_initialTouchPoint.Y < ScreenHeight - SecondaryDrawerSettings.DrawerHeight && SecondaryDrawerSettings.Position == Position.Bottom))
                                    {
                                        ToggleSecondaryDrawer();
                                    }
                                }
                                else
#endif
                                {
                                    if ((_initialTouchPoint.X > SecondaryDrawerSettings.DrawerWidth && SecondaryDrawerSettings.Position == Position.Left)
                                        || (_initialTouchPoint.X < ScreenWidth - SecondaryDrawerSettings.DrawerWidth && SecondaryDrawerSettings.Position == Position.Right)
                                        || (_initialTouchPoint.Y > SecondaryDrawerSettings.DrawerHeight && SecondaryDrawerSettings.Position == Position.Top)
                                        || (_initialTouchPoint.Y < ScreenHeight - SecondaryDrawerSettings.DrawerHeight && SecondaryDrawerSettings.Position == Position.Bottom))
                                    {
                                        ToggleSecondaryDrawer();
                                    }
                                }
                            }
                            _isPressed = false;
                            _isMoved = false;
                            break;
                        }
                    case PointerActions.Cancelled:
                        break;
                    case PointerActions.Entered:
                        break;
                }
            }
            else
            {
                if (action == PointerActions.Pressed && _isSecondaryDrawerOpen && SecondaryDrawerSettings != null)
                {
                    _initialTouchPoint = point;
                    if ((_initialTouchPoint.X > SecondaryDrawerSettings.DrawerWidth && SecondaryDrawerSettings.Position == Position.Left)
                        || (_initialTouchPoint.X < ScreenWidth - SecondaryDrawerSettings.DrawerWidth && SecondaryDrawerSettings.Position == Position.Right)
                        || (_initialTouchPoint.Y > SecondaryDrawerSettings.DrawerHeight && SecondaryDrawerSettings.Position == Position.Top)
                        || (_initialTouchPoint.Y < ScreenHeight - SecondaryDrawerSettings.DrawerHeight && SecondaryDrawerSettings.Position == Position.Bottom))
                    {
                        ToggleSecondaryDrawer();
                    }
                }
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
					TranslateDrawerXPosition(true);
				}
				else
				{
					TranslateDrawerYPosition(true);
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
				UpdateSecondaryDrawerFlowDirection();
				PositionUpdate();
				SecondaryPositionUpdate();
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

                    case "IsOpen":
                        UpdateIsOpen();
                        break;
				}
			}
		}

		void OnSecondaryDrawerSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (SecondaryDrawerSettings != null)
            {
                switch (e.PropertyName)
                {
                    case "DrawerContentView":
                        UpdateSecondaryDrawerContentView();
                        break;

                    case "DrawerHeaderView":
                        UpdateSecondaryDrawerHeaderView();
                        break;

                    case "DrawerFooterView":
                        UpdateSecondaryDrawerFooterView();
                        break;

                    case "ContentBackground":
                        UpdateSecondaryContentBackground();
                        break;

                    case "DrawerHeaderHeight":
                        UpdateSecondaryDrawerHeaderFooterSize();
                        break;

                    case "DrawerFooterHeight":
                        UpdateSecondaryDrawerHeaderFooterSize();
                        break;

                    case "DrawerWidth":
                        SecondaryDrawerSettings.UpdateDrawerWidth();
                        UpdateSecondaryDrawerLayoutSize();
                        break;

                    case "DrawerHeight":
                        SecondaryDrawerSettings.UpdateDrawerHeight();
                        UpdateSecondaryDrawerLayoutSize();
                        break;

                    case "Duration":
                        DurationPropertyUpdate(SecondaryDrawerSettings.Duration);
                        break;

                    case "Position":
                        SecondaryPositionUpdate();
                        break;

                    case "Transition":
                        UpdateSecondaryTransitionAnimation();
                        break;

                    case "TouchThreshold":
                        UpdateSecondaryTouchThreshold();
                        break;

                    case "IsOpen":
                        UpdateSecondaryIsOpen();
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
                UpdateIsOpen();
                UpdateAllChild();
			}
		}

        /// <summary>
        /// This method is used assigns the values of secondary drawer settings to the navigation drawer.
        /// </summary>
        void OnSecondaryDrawerSettingsChanged()
        {
            if (SecondaryDrawerSettings != null)
            {
                UpdateSecondaryDrawerContentView();
                UpdateSecondaryDrawerHeaderView();
                UpdateSecondaryDrawerFooterView();
                UpdateSecondaryDrawerHeaderFooterSize();
                UpdateSecondaryDrawerLayoutSize();
                DurationPropertyUpdate(SecondaryDrawerSettings.Duration);
                SecondaryPositionUpdate();
                UpdateSecondaryContentBackground();
                UpdateSecondaryIsOpen();
                UpdateAllChild();
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
                if ((DrawerSettings.IsOpen && !_isDrawerOpen) || (!DrawerSettings.IsOpen && _isDrawerOpen))
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
        void UpdateGridOverlayTranslate(bool isPrimaryDrawer = true)
        {
			var settings = GetDrawerSettings(isPrimaryDrawer);

			if (settings != null && ( settings.Position == Position.Left || settings.Position == Position.Right ||
					((settings.Position == Position.Top || settings.Position == Position.Bottom) && settings.DrawerHeight > 0)))
			{
				if ((settings.Transition == Transition.Reveal || settings.Transition == Transition.Push) &&  _greyOverlayGrid != null)
				{
					_greyOverlayGrid.TranslationX = 0;
				}
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
                        SetVisibility(true,true);
						_drawerLayout.TranslationX = ScreenWidth - DrawerSettings.DrawerWidth;
						if (DrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
						{
							_greyOverlayGrid.TranslationX = 0;
							ContentView.TranslationX = 0;
						}

						if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && ContentView != null)
						{
							ContentView.TranslationX = -DrawerSettings.DrawerWidth;
							_greyOverlayGrid.TranslationX = -DrawerSettings.DrawerWidth;
							ContentView.TranslationY = 0;
							_greyOverlayGrid.TranslationY = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
						{
							if (!SecondaryDrawerSettings.IsOpen)
							{
								SetVisibility(false, true);
							}
						}
						if (!SecondaryDrawerSettings.IsOpen)
						{
							_greyOverlayGrid.TranslationX = ScreenWidth;
						}
						if (DrawerSettings.Transition == Transition.Reveal && ContentView != null)
						{
							_drawerLayout.TranslationX = ScreenWidth - DrawerSettings.DrawerWidth;
							ContentView.TranslationX = 0;
						}
						else
						{
							_drawerLayout.TranslationX = ScreenWidth;
							if (DrawerSettings.Transition == Transition.Push && ContentView != null)
							{
								ContentView.TranslationX = 0;
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
                        SetVisibility(true,true);
						_drawerLayout.TranslationX = 0;
						if (DrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
						{
							_greyOverlayGrid.TranslationX = 0;
							ContentView.TranslationX = 0;
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && ContentView != null)
						{
							_greyOverlayGrid.TranslationX = DrawerSettings.DrawerWidth;
							ContentView.TranslationX = DrawerSettings.DrawerWidth;
							ContentView.TranslationY = 0;
							_greyOverlayGrid.TranslationY = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
						{
							if (!SecondaryDrawerSettings.IsOpen)
							{
								SetVisibility(false, true);
							}
						}

						if(!SecondaryDrawerSettings.IsOpen)
						{
							_greyOverlayGrid.TranslationX = ScreenWidth;
						}
						if (DrawerSettings.Transition == Transition.Reveal && ContentView != null)
						{
							_drawerLayout.TranslationX = 0;
							ContentView.TranslationX = 0;
						}
						else
						{
							_drawerLayout.TranslationX = -DrawerSettings.DrawerWidth;
							if (DrawerSettings.Transition == Transition.Push && ContentView != null)
							{
								ContentView.TranslationX = 0;
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
                        SetVisibility(true,true);
						_greyOverlayGrid.TranslationX = 0;
						_drawerLayout.TranslationY = -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2));
						if (DrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
						{
							ContentView.TranslationY = 0;
							ContentView.TranslationX = 0;
							_greyOverlayGrid.TranslationY = 0;
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && ContentView != null)
						{
							_drawerLayout.TranslationY -= _drawerMoveTop;
							_greyOverlayGrid.TranslationY = DrawerSettings.DrawerHeight;
							ContentView.TranslationY = DrawerSettings.DrawerHeight;
							ContentView.TranslationX = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
							{
								if (!SecondaryDrawerSettings.IsOpen)
								{
									SetVisibility(false, true);
								}
							}
							
						if(!SecondaryDrawerSettings.IsOpen)
						{
							_greyOverlayGrid.TranslationX = ScreenWidth;
						}
						if (DrawerSettings.Transition == Transition.Reveal && ContentView != null)
						{
							_drawerLayout.TranslationY = -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop;
							ContentView.TranslationY = 0;
						}
						else
						{
							_drawerLayout.TranslationY = -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight);
							if (DrawerSettings.Transition == Transition.Push && ContentView != null)
							{
								ContentView.TranslationY = 0;
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
						SetVisibility(true,true);
						_greyOverlayGrid.TranslationX = 0;
						_drawerLayout.TranslationY = (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2);
						if (DrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
						{
							ContentView.TranslationY = 0;
							ContentView.TranslationX = 0;
						}
						else if ((DrawerSettings.Transition == Transition.Push || DrawerSettings.Transition == Transition.Reveal) && ContentView != null)
						{
							_greyOverlayGrid.TranslationY = -DrawerSettings.DrawerHeight;
							ContentView.TranslationY = -DrawerSettings.DrawerHeight;
							ContentView.TranslationX = 0;
						}
					}
					else
					{
#if WINDOWS || MACCATALYST
						if ((!_drawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (DrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
						{
							if (!SecondaryDrawerSettings.IsOpen)
							{
								SetVisibility(false, true);
							}
						}
						
						if(!SecondaryDrawerSettings.IsOpen)
						{
						_greyOverlayGrid.TranslationX = ScreenWidth;
						}
						
						if (DrawerSettings.Transition == Transition.Reveal && ContentView != null)
						{
							_drawerLayout.TranslationY = (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2);
							ContentView.TranslationY = 0;
						}
						else
						{
							_drawerLayout.TranslationY = (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2);
							if (DrawerSettings.Transition == Transition.Push && ContentView != null)
							{
								ContentView.TranslationY = 0;
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
			if (_drawerLayout != null && ContentView != null)
			{
				if (_isRTL)
				{
					_drawerLayout.FlowDirection = ContentView.FlowDirection = FlowDirection.RightToLeft;
				}
				else
				{
					_drawerLayout.FlowDirection = ContentView.FlowDirection = FlowDirection.LeftToRight;
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
		void TranslateDrawerXPosition(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
            SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;
            bool isDrawerOpen = isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;
            DrawerCancelEventArgs cancelOpenEventArgs = isPrimaryDrawer ? _cancelOpenEventArgs : _secondaryCancelOpenEventArgs;
            DrawerCancelEventArgs cancelCloseEventArgs = isPrimaryDrawer ? _cancelCloseEventArgs : _secondaryCancelCloseEventArgs;
            // Determine if this swipe is an OPEN attempt
            bool isOpeningAttempt = !isDrawerOpen; 
            // Block only if trying to OPEN this drawer while the other drawer is already open
            if (isOpeningAttempt)
            {
                if (isPrimaryDrawer && _isSecondaryDrawerOpen) return;
                if (!isPrimaryDrawer && _isDrawerOpen) return;
            }
			var difference = _newPoint.X - _oldPoint.X;
			if (_greyOverlayGrid != null && _drawerLayout != null)
			{
#if !WINDOWS
                if ((settings.Position == Position.Left && !_isRTL) || (settings.Position == Position.Right && _isRTL))
#else
                if (settings.Position == Position.Left)
#endif
				{
					// Method to handle the swipe action for the left drawer
					HandleLeftDrawerSwipe(difference,isPrimaryDrawer,isDrawerOpen);
				}
#if !WINDOWS
                else if ((settings.Position == Position.Right && !_isRTL) || (settings.Position == Position.Left && _isRTL))
#else
                else if (settings.Position == Position.Right)
#endif
				{
					// Method to handle the swipe action for the right drawer
					HandleRightDrawerSwipe(difference,isPrimaryDrawer,isDrawerOpen);
				}
			}
		}

		void HandleLeftDrawerSwipe(double difference,bool isPrimaryDrawer,bool isDrawerOpen)
		{
            DrawerCancelEventArgs cancelOpenEventArgs = isPrimaryDrawer ? _cancelOpenEventArgs : _secondaryCancelOpenEventArgs;
			DrawerCancelEventArgs cancelCloseEventArgs = isPrimaryDrawer ? _cancelCloseEventArgs : _secondaryCancelCloseEventArgs;
            // Update remaining drawer width based on swipe difference
			_remainDrawerWidth += difference;
			if (isPrimaryDrawer)
			{
				ValidateRemainDrawerWidth();
			}
			else
			{
				ValidateSecondaryRemainDrawerWidth();
			}

			if (!isDrawerOpen)
			{
				// Check if it's the first swipe action to open the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
				{
					SetDrawerOpeningEvent(isPrimaryDrawer);
					if (!cancelOpenEventArgs.Cancel)
					{
						SetVisibility(true,isPrimaryDrawer);
					}
				}

				if (!cancelOpenEventArgs.Cancel)
				{
					LeftDrawerSwipe(difference,isPrimaryDrawer);
				}
			}
			else
			{
				// Check if it's the first swipe action to close the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
				{
					OnDrawerClosing(cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!cancelCloseEventArgs.Cancel)
				{
					LeftDrawerSwipe(difference,isPrimaryDrawer);
				}
			}
		}

		void HandleRightDrawerSwipe(double difference,bool isPrimaryDrawer,bool isDrawerOpen)
		{
			// Update remaining drawer width based on swipe difference
            DrawerCancelEventArgs cancelOpenEventArgs = isPrimaryDrawer ? _cancelOpenEventArgs : _secondaryCancelOpenEventArgs;
			DrawerCancelEventArgs cancelCloseEventArgs = isPrimaryDrawer ? _cancelCloseEventArgs : _secondaryCancelCloseEventArgs;
            _remainDrawerWidth -= difference;
			if (isPrimaryDrawer)
			{
				ValidateRemainDrawerWidth();
			}
			else
			{
				ValidateSecondaryRemainDrawerWidth();
			}

			if (!isDrawerOpen)
			{
				// Check if it's the first swipe action to open the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
				{
					SetDrawerOpeningEvent(isPrimaryDrawer);
					if (!cancelOpenEventArgs.Cancel)
					{
						SetVisibility(true, isPrimaryDrawer);
					}
				}

				if (!cancelOpenEventArgs.Cancel)
				{
					RightDrawerSwipe(difference, isPrimaryDrawer);
				}
			}
			else
			{
				// Check if it's the first swipe action to close the drawer
				if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
				{
					OnDrawerClosing(cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!cancelCloseEventArgs.Cancel)
				{
					RightDrawerSwipe(difference, isPrimaryDrawer);
				}
			}
		}

		/// <summary>
		/// This method employed to translate the drawer vertically (along the Y-axis) when swiping from the sides.
		/// </summary>
		void TranslateDrawerYPosition(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
            SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;
            bool isDrawerOpen = isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;
            DrawerCancelEventArgs cancelOpenEventArgs = isPrimaryDrawer ? _cancelOpenEventArgs : _secondaryCancelOpenEventArgs;
            DrawerCancelEventArgs cancelCloseEventArgs = isPrimaryDrawer ? _cancelCloseEventArgs : _secondaryCancelCloseEventArgs;

			var difference = _newPoint.Y - _oldPoint.Y;
            if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
            {
                if (settings.Position == Position.Top)
                {
					// Method to handle the swipe action for the top drawer
					HandleTopDrawerSwipe(difference,isPrimaryDrawer,isDrawerOpen);
				}
				else if (settings.Position == Position.Bottom)
				{
					// Method to handle the swipe action for the bottom drawer
					HandleBottomDrawerSwipe(difference,isPrimaryDrawer,isDrawerOpen);
				}
			}
		}

		void HandleTopDrawerSwipe(double difference,bool isPrimaryDrawer,bool isDrawerOpen)
		{
            DrawerCancelEventArgs cancelOpenEventArgs = isPrimaryDrawer ? _cancelOpenEventArgs : _secondaryCancelOpenEventArgs;
			DrawerCancelEventArgs cancelCloseEventArgs = isPrimaryDrawer ? _cancelCloseEventArgs : _secondaryCancelCloseEventArgs;
            DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
            SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;
            if (_greyOverlayGrid != null && navigationDrawerLayout != null)
			{
				_remainDrawerHeight += difference;
				if (isPrimaryDrawer)
				{
					ValidateRemainDrawerHeight();
				}
				else
				{
					ValidateSecondaryRemainDrawerHeight();
				}
				if (!isDrawerOpen)
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
					{
						SetDrawerOpeningEvent(isPrimaryDrawer);
						if (!cancelOpenEventArgs.Cancel)
						{
							SetVisibility(true, isPrimaryDrawer);
						}
#if WINDOWS
						if (!cancelOpenEventArgs.Cancel && settings.Transition == Transition.Push)
						{
							navigationDrawerLayout.TranslationY += _toolBarHeight;
						}
#elif ANDROID || IOS

						if (!cancelOpenEventArgs.Cancel && settings.Transition == Transition.Push)
						{
							navigationDrawerLayout.TranslationY += _drawerMoveTop;
						}
#endif
					}

					if (!cancelOpenEventArgs.Cancel)
					{
						TopDrawerSwipe(difference,isPrimaryDrawer);
					}
				}
				else
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
					{
						OnDrawerClosing(cancelCloseEventArgs);
						FirstMoveActionCompleted();
					}

					if (!cancelCloseEventArgs.Cancel)
					{
						TopDrawerSwipe(difference, isPrimaryDrawer);
					}
				}
			}
		}

		void HandleBottomDrawerSwipe(double difference,bool isPrimaryDrawer,bool isDrawerOpen)
		{
            DrawerCancelEventArgs cancelOpenEventArgs = isPrimaryDrawer ? _cancelOpenEventArgs : _secondaryCancelOpenEventArgs;
			DrawerCancelEventArgs cancelCloseEventArgs = isPrimaryDrawer ? _cancelCloseEventArgs : _secondaryCancelCloseEventArgs;
            DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
            SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;
			if (_greyOverlayGrid != null && navigationDrawerLayout != null)
			{
				_remainDrawerHeight -= difference;
				if (isPrimaryDrawer)
				{
					ValidateRemainDrawerHeight();
				}
				else
				{
					ValidateSecondaryRemainDrawerHeight();
				}

				if (!isDrawerOpen)
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference < 0)
					{
						SetDrawerOpeningEvent(isPrimaryDrawer);
						if (!cancelOpenEventArgs.Cancel)
						{
							SetVisibility(true, isPrimaryDrawer);
						}
					}

					if (!cancelOpenEventArgs.Cancel)
					{
						BottomDrawerSwipe(difference, isPrimaryDrawer);
					}
				}
				else
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose && difference > 0)
					{
						OnDrawerClosing(cancelCloseEventArgs);
						FirstMoveActionCompleted();
					}

					if (!cancelCloseEventArgs.Cancel)
					{
						BottomDrawerSwipe(difference,isPrimaryDrawer);
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
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationX == DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerLeftIn(true);
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationX == 0 && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerLeftIn(true);
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleLeftSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (ContentView.TranslationX == 0)
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerLeftOut(true);
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationX == -DrawerSettings.DrawerWidth)
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerLeftOut(true);
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
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerLeftIn(true);
					}
				}
				else if (ContentView != null && ContentView.TranslationX >= DrawerSettings.DrawerWidth / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationX == DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerLeftIn(true);
					}
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationX != -DrawerSettings.DrawerWidth && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationX != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerLeftOut(true);
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
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationX == -DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerRightIn(true);
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationX == (ScreenWidth - DrawerSettings.DrawerWidth) && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerRightIn(true);
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleRightSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (ContentView.TranslationX == 0)
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerRightOut(true);
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationX == ScreenWidth)
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerRightOut(true);
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
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerRightIn(true);
						_isTransitionDifference = false;
					}
				}
				else if (ContentView != null && ContentView.TranslationX <= -DrawerSettings.DrawerWidth / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationX == -DrawerSettings.DrawerWidth && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerRightIn(true);
						_isTransitionDifference = false;
					}
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationX != ScreenWidth && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationX != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerRightOut(true);
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
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationY == DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerTopIn(true);
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationY == -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerTopIn(true);
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleTopSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (ContentView.TranslationY == 0)
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerTopOut(true);
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationY == -((ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2)) - _drawerMoveTop)
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerTopOut(true);
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
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerTopIn(true);
					}
					_isTransitionDifference = false;
				}
				else if (ContentView != null && ContentView.TranslationY >= DrawerSettings.DrawerHeight / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationY == DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerTopIn(true);
					}
					_isTransitionDifference = false;
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationY != -((ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) + _toolBarHeight) && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationY != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerTopOut(true);
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
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if (DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationY == -DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerBottomIn(true);
						_isTransitionDifference = false;
					}
				}
				else
				{
					if (_drawerLayout.TranslationY == (ScreenHeight / 2) - (DrawerSettings.DrawerHeight / 2) && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerBottomIn(true);
						_isTransitionDifference = false;
					}
				}
			}
		}

		void HandleBottomSwipeOut()
		{
			if (_drawerLayout != null && _greyOverlayGrid != null && DrawerSettings != null && ContentView != null)
			{
				if ((_isDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isDrawerOpen)) && _isTransitionDifference)
				{
					if (DrawerSettings.Transition == Transition.Reveal)
					{
						if (ContentView.TranslationY == 0)
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerBottomOut(true);
							_isTransitionDifference = false;
						}
					}
					else
					{
						if (_drawerLayout.TranslationY == (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2))
						{
							UpdateToggleOutEvent(true);
						}
						else
						{
							DrawerBottomOut(true);
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
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerBottomIn(true);
					}
					_isTransitionDifference = false;
				}
				else if (ContentView != null && ContentView.TranslationY <= -DrawerSettings.DrawerHeight / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && DrawerSettings.Transition == Transition.Reveal)
				{
					if (ContentView.TranslationY == -DrawerSettings.DrawerHeight && (!_isDrawerOpen || _isTransitionDifference))
					{
						UpdateToggleInEvent(true);
					}
					else
					{
						DrawerBottomIn(true);
					}
					_isTransitionDifference = false;
				}
				else if (!_cancelOpenEventArgs.Cancel)
				{
					if ((_drawerLayout.TranslationY != (ScreenHeight / 2) + (DrawerSettings.DrawerHeight / 2) && DrawerSettings.Transition != Transition.Reveal) || (DrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationY != 0) || _isDrawerOpen || _isTransitionDifference)
					{
						DrawerBottomOut(true);
						_isTransitionDifference = false;
					}
				}
			}
		}

		/// <summary>
		/// This method updates the event-handling logic for toggling the drawer in an inward direction.
		/// </summary>
		private void UpdateToggleInEvent(bool isPrimaryDrawer)
		{
			if (GetDrawerCancelOpenEventArgs(isPrimaryDrawer).Cancel)
			{
				SetVisibility(false, isPrimaryDrawer);
				_isTransitionDifference = false;
				return;
			}

			_toggledEventArgs.IsOpen = true;
			_toggledEventArgs.IsPrimary = isPrimaryDrawer;
			if (isPrimaryDrawer)
			{
				_isDrawerOpen = true;
				DrawerSettings.IsOpen = true;
				OnDrawerOpened(new DrawerEventArgs(true));
			}
			else
			{
				_isSecondaryDrawerOpen = true;
				SecondaryDrawerSettings.IsOpen = true;
				OnDrawerOpened(new DrawerEventArgs(false));
			}
			_isTransitionDifference = false;
			OnDrawerToggled(_toggledEventArgs);
		}

		/// <summary>
		/// This method updates the event-handling logic for toggling the drawer in an outward direction.
		/// </summary>
		private void UpdateToggleOutEvent(bool isPrimaryDrawer)
		{
			_toggledEventArgs.IsOpen = false;
			_toggledEventArgs.IsPrimary = isPrimaryDrawer;
			if (isPrimaryDrawer)
			{
				_isDrawerOpen = false;
				DrawerSettings.IsOpen = false;
				_greyOverlayGrid!.TranslationX = ScreenWidth;
				SetVisibility(false,true);
				OnDrawerClosed(new DrawerEventArgs(true));
			}
			else
			{
				_isSecondaryDrawerOpen = false;
				SecondaryDrawerSettings.IsOpen = false;
				_greyOverlayGrid!.TranslationX = ScreenWidth;
				SetVisibility(false,false);
				OnDrawerClosed(new DrawerEventArgs(false));
			}
			_isTransitionDifference = false;
		   OnDrawerToggled(_toggledEventArgs);
		}

		/// <summary>
		/// This method is used to swipe the drawer to or from the left side.
		/// </summary>
		/// <param name="difference">The distance of the swipe.</param>
		/// <param name="isPrimaryDrawer">A boolean value indicating the primary drawer.</param>
		void LeftDrawerSwipe(double difference, bool isPrimaryDrawer)
		{
			DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
			SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(navigationDrawerLayout.TranslationX);
					navigationDrawerLayout.TranslationX = Math.Clamp(navigationDrawerLayout.TranslationX + difference, -settings.DrawerWidth, 0);
					transitionDifference = Math.Abs(Math.Round(navigationDrawerLayout.TranslationX) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(ContentView.TranslationX);
				}

				if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
				{
					_greyOverlayGrid.TranslationX = Math.Clamp(_greyOverlayGrid.TranslationX + difference, 0, settings.DrawerWidth);
					ContentView.TranslationX = Math.Clamp(ContentView.TranslationX + difference, 0, settings.DrawerWidth);
					if (settings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(ContentView.TranslationX) - oldTransition);
					}

#if WINDOWS
					if (ContentView.TranslationX != 0 && _greyOverlayGrid.TranslationX != 0)
					{
						SetVisibility(true, isPrimaryDrawer);
					}
#endif
				}

#if WINDOWS
				if (settings.Transition == Transition.SlideOnTop && navigationDrawerLayout.TranslationX != -settings.DrawerWidth)
				{
					SetVisibility(true, isPrimaryDrawer);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerWidth * (_opacity / settings.DrawerWidth)) + _opacity;
			}
		}

		/// <summary>
		/// This method is used to swipe the drawer to or from the right side.
		/// </summary>
		/// <param name="difference">The distance of the swipe.</param>
		/// <param name="isPrimaryDrawer">A boolean value indicating the primary drawer.</param>
		void RightDrawerSwipe(double difference, bool isPrimaryDrawer)
		{
			DrawerSettings? settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
			SfGrid? navigationDrawerLayout = isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(navigationDrawerLayout.TranslationX);
					navigationDrawerLayout.TranslationX = Math.Clamp(navigationDrawerLayout.TranslationX + difference, ScreenWidth - settings.DrawerWidth, ScreenWidth);
					transitionDifference = Math.Abs(Math.Round(navigationDrawerLayout.TranslationX) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(ContentView.TranslationX);
				}

				if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
				{
					_greyOverlayGrid.TranslationX = Math.Clamp(_greyOverlayGrid.TranslationX + difference, -settings.DrawerWidth, 0);
					ContentView.TranslationX = Math.Clamp(ContentView.TranslationX + difference, -settings.DrawerWidth, 0);
					if (settings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(ContentView.TranslationX) - oldTransition);
					}

#if WINDOWS
					if (ContentView.TranslationX != 0 && _greyOverlayGrid.TranslationX != 0)
					{
						SetVisibility(true, isPrimaryDrawer);
					}
#endif
				}

#if WINDOWS
				if (settings.Transition == Transition.SlideOnTop && navigationDrawerLayout.TranslationX != ScreenWidth)
				{
					SetVisibility(true, isPrimaryDrawer);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerWidth * (_opacity / settings.DrawerWidth)) + _opacity;
			}
		}

		/// <summary>
		/// This method is used to swipe the drawer to or from the top side.
		/// </summary>
		/// <param name="difference">The distance of the swipe.</param>
		/// <param name="isPrimaryDrawer">A boolean value indicating the primary drawer.</param>
		void TopDrawerSwipe(double difference, bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(navigationDrawerLayout.TranslationY);
					navigationDrawerLayout.TranslationY = Math.Clamp(navigationDrawerLayout.TranslationY + difference, -((ScreenHeight / 2) + (settings.DrawerHeight / 2) + _toolBarHeight), -((ScreenHeight / 2) - (settings.DrawerHeight / 2)) - _drawerMoveTop);
					transitionDifference = Math.Abs(Math.Round(navigationDrawerLayout.TranslationY) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(ContentView.TranslationY);
				}

				if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
				{
					_greyOverlayGrid.TranslationY = Math.Clamp(_greyOverlayGrid.TranslationY + difference, 0, settings.DrawerHeight);
					ContentView.TranslationY = Math.Clamp(ContentView.TranslationY + difference, 0, settings.DrawerHeight);
					if (settings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(ContentView.TranslationY) - oldTransition);
					}

#if WINDOWS
					if (ContentView.TranslationY != 0 && _greyOverlayGrid.TranslationY != 0)
					{
						SetVisibility(true, isPrimaryDrawer);
					}
#endif
				}

#if WINDOWS
				if (settings.Transition == Transition.SlideOnTop && navigationDrawerLayout.TranslationY != -((ScreenHeight / 2) + (settings.DrawerHeight / 2) + _toolBarHeight))
				{
					SetVisibility(true, isPrimaryDrawer);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerHeight * (_opacity / settings.DrawerHeight)) + _opacity;
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
		/// <param name="isPrimaryDrawer"></param> 
		void BottomDrawerSwipe(double difference, bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				double transitionDifference = 0;
				double oldTransition = 0;
				SetGreyOverLayGridTranslationX();

				if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
				{
					oldTransition = Math.Round(navigationDrawerLayout.TranslationY);
					navigationDrawerLayout.TranslationY = Math.Clamp(navigationDrawerLayout.TranslationY + difference, (ScreenHeight / 2) - (settings.DrawerHeight / 2), (ScreenHeight / 2) + (settings.DrawerHeight / 2));
					transitionDifference = Math.Abs(Math.Round(navigationDrawerLayout.TranslationY) - oldTransition);
				}
				else
				{
					oldTransition = Math.Round(ContentView.TranslationY);
				}

				if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
				{
					_greyOverlayGrid.TranslationY = Math.Clamp(_greyOverlayGrid.TranslationY + difference, -settings.DrawerHeight, 0);
					ContentView.TranslationY = Math.Clamp(ContentView.TranslationY + difference, -settings.DrawerHeight, 0);
					if (settings.Transition == Transition.Reveal)
					{
						transitionDifference = Math.Abs(Math.Round(ContentView.TranslationY) - oldTransition);
					}

#if WINDOWS
					if (ContentView.TranslationY != 0 && _greyOverlayGrid.TranslationY != 0)
					{
						SetVisibility(true, isPrimaryDrawer);
					}
#endif
				}

#if WINDOWS
				if (settings.Transition == Transition.SlideOnTop && navigationDrawerLayout.TranslationY != (ScreenHeight / 2) + (settings.DrawerHeight / 2))
				{
					SetVisibility(true, isPrimaryDrawer);
				}
#endif

				SetIsTransitionDifference(transitionDifference);
				_greyOverlayGrid.Opacity = (_remainDrawerHeight * (_opacity / settings.DrawerHeight)) + _opacity;
			}
		}

		/// <summary>
		/// This method is used to update the secondary drawer after swipe complete.
		/// </summary>

        private void CompletedSecondaryDrawerSwipe()
        {
            if (_secondaryDrawerLayout != null && _greyOverlayGrid != null && SecondaryDrawerSettings != null)
            {
#if !WINDOWS
                if ((SecondaryDrawerSettings.Position == Position.Left && !_isRTL) || (SecondaryDrawerSettings.Position == Position.Right && _isRTL))
#else
                if (SecondaryDrawerSettings.Position == Position.Left)
#endif
                {
                    if (_velocityX > 500)
                    {
                        if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                        {
                            if (ContentView.TranslationX == SecondaryDrawerSettings.DrawerWidth && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerLeftIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                        else
                        {
                            if (_secondaryDrawerLayout.TranslationX == 0 && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerLeftIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                    }
                    else if (_velocityX < -500)
                    {
                        if ((_isSecondaryDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isSecondaryDrawerOpen)) && _isTransitionDifference)
                        {
                            if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                            {
                                if (ContentView.TranslationX == 0)
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerLeftOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                            else
                            {
                                if (_secondaryDrawerLayout.TranslationX == -SecondaryDrawerSettings.DrawerWidth)
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerLeftOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                        }
                    }
                    else if (_secondaryDrawerLayout.TranslationX >= (-SecondaryDrawerSettings.DrawerWidth / 2) && SecondaryDrawerSettings.Transition != Transition.Reveal)
                    {
                        if (_secondaryDrawerLayout.TranslationX == 0 && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerLeftIn(false);
                        }
                    }
                    else if (ContentView != null && ContentView.TranslationX >= SecondaryDrawerSettings.DrawerWidth / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && SecondaryDrawerSettings.Transition == Transition.Reveal)
                    {
                        if (ContentView.TranslationX == SecondaryDrawerSettings.DrawerWidth && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerLeftIn(false);
                        }
                    }
                    else if (!_cancelOpenEventArgs.Cancel)
                    {
                        if ((_secondaryDrawerLayout.TranslationX != -SecondaryDrawerSettings.DrawerWidth && SecondaryDrawerSettings.Transition != Transition.Reveal) || (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationX != 0) || _isSecondaryDrawerOpen || _isTransitionDifference)
                        {
                            DrawerLeftOut(false);
                            _isTransitionDifference = false;
                        }
                    }
                }
#if !WINDOWS
                else if ((SecondaryDrawerSettings.Position == Position.Right && !_isRTL) || (SecondaryDrawerSettings.Position == Position.Left && _isRTL))
#else
                else if (SecondaryDrawerSettings.Position == Position.Right)
#endif
                {
                    if (_velocityX < -500)
                    {
                        if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                        {
                            if (ContentView.TranslationX == -SecondaryDrawerSettings.DrawerWidth && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerRightIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                        else
                        {
                            if (_secondaryDrawerLayout.TranslationX == (ScreenWidth - SecondaryDrawerSettings.DrawerWidth) && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerRightIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                    }
                    else if (_velocityX > 500)
                    {
                        if ((_isSecondaryDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isSecondaryDrawerOpen)) && _isTransitionDifference)
                        {
                            if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                            {
                                if (ContentView.TranslationX == 0)
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerRightOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                            else
                            {
                                if (_secondaryDrawerLayout.TranslationX == ScreenWidth)
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerRightOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                        }
                    }
                    else if (_remainDrawerWidth >= (-SecondaryDrawerSettings.DrawerWidth / 2) && SecondaryDrawerSettings.Transition != Transition.Reveal)
                    {
                        if (_secondaryDrawerLayout.TranslationX == (ScreenWidth - SecondaryDrawerSettings.DrawerWidth) && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerRightIn(false);
                            _isTransitionDifference = false;
                        }
                    }
                    else if (ContentView != null && ContentView.TranslationX <= -SecondaryDrawerSettings.DrawerWidth / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && SecondaryDrawerSettings.Transition == Transition.Reveal)
                    {
                        if (ContentView.TranslationX == -SecondaryDrawerSettings.DrawerWidth && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerRightIn(false);
                            _isTransitionDifference = false;
                        }
                    }
                    else if (!_cancelOpenEventArgs.Cancel)
                    {
                        if ((_secondaryDrawerLayout.TranslationX != ScreenWidth && SecondaryDrawerSettings.Transition != Transition.Reveal) || (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationX != 0) || _isSecondaryDrawerOpen || _isTransitionDifference)
                        {
                            DrawerRightOut(false);
                            _isTransitionDifference = false;
                        }
                    }
                }
                else if (SecondaryDrawerSettings.Position == Position.Top)
                {
                    if (_velocityY > 500)
                    {
                        if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                        {
                            if (ContentView.TranslationY == SecondaryDrawerSettings.DrawerHeight && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerTopIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                        else
                        {
                            if (_secondaryDrawerLayout.TranslationY == -((ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2)) - _drawerMoveTop && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerTopIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                    }
                    else if (_velocityY < -500)
                    {
                        if ((_isSecondaryDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isSecondaryDrawerOpen)) && _isTransitionDifference)
                        {
                            if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                            {
                                if (ContentView.TranslationY == 0)
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerTopOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                            else
                            {
                                if (_secondaryDrawerLayout.TranslationY == -((ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2)) - _drawerMoveTop)
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerTopOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                        }
                    }
                    else if (_remainDrawerHeight >= (-SecondaryDrawerSettings.DrawerHeight / 2) && SecondaryDrawerSettings.Transition != Transition.Reveal)
                    {
                        if (_secondaryDrawerLayout.TranslationY == -((ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2)) - _drawerMoveTop && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerTopIn(false);
                            _isTransitionDifference = false;
                        }
                    }
                    else if (ContentView != null && ContentView.TranslationY >= SecondaryDrawerSettings.DrawerHeight / 2 && _greyOverlayGrid.TranslationX != ScreenWidth && SecondaryDrawerSettings.Transition == Transition.Reveal)
                    {
                        if (ContentView.TranslationY == SecondaryDrawerSettings.DrawerHeight && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerTopIn(false);
                            _isTransitionDifference = false;
                        }
                    }
                    else if (!_cancelOpenEventArgs.Cancel)
                    {
                        if ((_secondaryDrawerLayout.TranslationY != -((ScreenHeight / 2) + (SecondaryDrawerSettings.DrawerHeight / 2) + _toolBarHeight) && SecondaryDrawerSettings.Transition != Transition.Reveal) || (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationY != 0) || _isSecondaryDrawerOpen || _isTransitionDifference)
                        {
                            DrawerTopOut(false);
                            _isTransitionDifference = false;
                        }
                    }
                }
                else if (SecondaryDrawerSettings.Position == Position.Bottom)
                {
                    if (_velocityY < -500)
                    {
                        if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                        {
                            if (ContentView.TranslationY == -SecondaryDrawerSettings.DrawerHeight && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerBottomIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                        else
                        {
                            if (_secondaryDrawerLayout.TranslationY == (ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2) && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                            {
                                UpdateToggleInEvent(false);
                            }
                            else
                            {
                                DrawerBottomIn(false);
                                _isTransitionDifference = false;
                            }
                        }
                    }
                    else if (_velocityY > 500)
                    {
                        if ((_isSecondaryDrawerOpen || (_greyOverlayGrid.TranslationX == 0 && !_isSecondaryDrawerOpen)) && _isTransitionDifference)
                        {
                            if (SecondaryDrawerSettings.Transition == Transition.Reveal)
                            {
                                if (ContentView.TranslationY == 0)
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerBottomOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                            else
                            {
                                if (_secondaryDrawerLayout.TranslationY == (ScreenHeight / 2) + (SecondaryDrawerSettings.DrawerHeight / 2))
                                {
                                    UpdateToggleOutEvent(false);
                                }
                                else
                                {
                                    DrawerBottomOut(false);
                                    _isTransitionDifference = false;
                                }
                            }
                        }
                    }
                    else if (_remainDrawerHeight >= (-SecondaryDrawerSettings.DrawerHeight / 2) && SecondaryDrawerSettings.Transition != Transition.Reveal)
                    {
                        if (_secondaryDrawerLayout.TranslationY == ((ScreenHeight / 2) + (SecondaryDrawerSettings.DrawerHeight / 2) - SecondaryDrawerSettings.DrawerHeight) && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerBottomIn(false);
                            _isTransitionDifference = false;
                        }
                    }
                    else if (ContentView != null && ContentView.TranslationY <= (-SecondaryDrawerSettings.DrawerHeight / 2) && _greyOverlayGrid.TranslationX != ScreenWidth && SecondaryDrawerSettings.Transition == Transition.Reveal)
                    {
                        if (ContentView.TranslationY == -SecondaryDrawerSettings.DrawerHeight && (!_isSecondaryDrawerOpen || _isTransitionDifference))
                        {
                            UpdateToggleInEvent(false);
                        }
                        else
                        {
                            DrawerBottomIn(false);
                            _isTransitionDifference = false;
                        }
                    }
                    else if (!_cancelOpenEventArgs.Cancel)
                    {
                        if ((_secondaryDrawerLayout.TranslationY != (ScreenHeight / 2) + (SecondaryDrawerSettings.DrawerHeight / 2) && SecondaryDrawerSettings.Transition != Transition.Reveal) || (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null && ContentView.TranslationY != 0) || _isSecondaryDrawerOpen || _isTransitionDifference)
                        {
                            DrawerBottomOut(false);
                            _isTransitionDifference = false;
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the left side.
		/// </summary>
		void DrawerLeftIn(bool isPrimaryDrawer)
		{
			 DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			bool isDrawerOpen = GetDrawerOpenState(isPrimaryDrawer);
			DrawerCancelEventArgs _cancelOpenEventArgs = GetDrawerCancelOpenEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				if (navigationDrawerLayout.TranslationX != 0 || (settings.Transition == Transition.Reveal && ContentView?.TranslationX != settings.DrawerWidth))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent(isPrimaryDrawer);
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration;
						if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
						{
							currentDuration = (Math.Abs(navigationDrawerLayout.TranslationX) / settings.DrawerWidth) * settings.Duration;
						}
						else
						{
							currentDuration = (Math.Abs(settings.DrawerWidth - _greyOverlayGrid.TranslationX) / settings.DrawerWidth) * settings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationX = d, navigationDrawerLayout.TranslationX, 0);
						var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (settings.Transition == Transition.SlideOnTop)
						{
							navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
							});
						}
						else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
						{
							var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, settings.DrawerWidth);
							var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationX = d, ContentView.TranslationX, settings.DrawerWidth);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
							ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
							DrawerPushAnimation(currentDuration, drawerAnimation, isPrimaryDrawer);
						}

						SetDrawerOpenState(isPrimaryDrawer, true);
						_toggledEventArgs.IsOpen = true;
						settings.IsOpen = true;
						_toggledEventArgs.IsPrimary = isPrimaryDrawer;
					}
					else
					{
						_greyOverlayGrid.TranslationX = ScreenWidth;
					}
				}
				else if (navigationDrawerLayout.TranslationX == 0)
				{
					SetDrawerOpenState(isPrimaryDrawer, true);
					settings.IsOpen = true;
				}
			}
		}

		/// <summary>
		///  Method used to provide the animation to the drawer when moving towards the left side.
		/// </summary>
		void DrawerLeftOut(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			DrawerCancelEventArgs _cancelCloseEventArgs = GetDrawerCancelCloseEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(_cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!_cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
					{
						currentDuration = (Math.Abs(settings.DrawerWidth + navigationDrawerLayout.TranslationX) / settings.DrawerWidth) * settings.Duration;
					}
					else
					{
						currentDuration = (Math.Abs(_greyOverlayGrid.TranslationX) / settings.DrawerWidth) * settings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationX = d, navigationDrawerLayout.TranslationX, -settings.DrawerWidth);
					var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (settings.Transition == Transition.SlideOnTop)
					{
						navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
							if (navigationDrawerLayout.TranslationX == -settings.DrawerWidth)
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
					}
					else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
					{
						var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationX = d, ContentView.TranslationX, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
						ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
if (ContentView.TranslationX == 0)
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						DrawerPushAnimation(currentDuration, drawerAnimation, isPrimaryDrawer);
					}

					SetDrawerOpenState(isPrimaryDrawer, false);
					_toggledEventArgs.IsOpen = false;
					settings.IsOpen = false;
				}
			}
		}

		void OnDrawerClosedToggledEvent(bool isPrimaryDrawer)
		{
			if (isPrimaryDrawer)
			{
				OnDrawerClosed(new DrawerEventArgs(true));
				OnDrawerToggled(_toggledEventArgs);
#if WINDOWS
			if (!_isDrawerOpen)
#endif
				{
					SetVisibility(false,true);
				}
			}
			else
			{
				OnDrawerClosed(new DrawerEventArgs(false));
				OnDrawerToggled(_toggledEventArgs);
#if WINDOWS
if (!_isSecondaryDrawerOpen)
#endif
				{
					SetVisibility(false,false);
				}
			}
		}
		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the right side.
		/// </summary>
		void DrawerRightIn(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			bool isDrawerOpen = GetDrawerOpenState(isPrimaryDrawer);
			DrawerCancelEventArgs _cancelOpenEventArgs = GetDrawerCancelOpenEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				if (navigationDrawerLayout.TranslationX != ScreenWidth - settings.DrawerWidth || (settings.Transition == Transition.Reveal && ContentView?.TranslationX != -settings.DrawerWidth))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent(isPrimaryDrawer);
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration;
						if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
						{
							currentDuration = (Math.Abs((ScreenWidth - settings.DrawerWidth) - navigationDrawerLayout.TranslationX) / settings.DrawerWidth) * settings.Duration;
						}
						else
						{
							currentDuration = (Math.Abs(settings.DrawerWidth + ContentView!.TranslationX) / settings.DrawerWidth) * settings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationX = d, navigationDrawerLayout.TranslationX, ScreenWidth - settings.DrawerWidth);
						var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (settings.Transition == Transition.SlideOnTop)
						{
							navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)settings.Duration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(settings.Duration / 2), easing: Easing.Linear);
							});
						}
						else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
						{
							var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, -settings.DrawerWidth);
							var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationX = d, ContentView.TranslationX, -settings.DrawerWidth);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(settings.Duration / 2), easing: Easing.Linear);
							ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
							DrawerPushAnimation(currentDuration, drawerAnimation, isPrimaryDrawer);
						}

						SetDrawerOpenState(isPrimaryDrawer, true);
						_toggledEventArgs.IsOpen = true;
						settings.IsOpen = true;
						_toggledEventArgs.IsPrimary = isPrimaryDrawer;
					}
					else
					{
						_greyOverlayGrid.TranslationX = ScreenWidth;
					}
				}
				else if (navigationDrawerLayout.TranslationX == ScreenWidth - settings.DrawerWidth)
				{
					SetDrawerOpenState(isPrimaryDrawer, true);
					settings.IsOpen = true;
				}
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving towards the right side.
		/// </summary>
		void DrawerRightOut(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			DrawerCancelEventArgs cancelCloseEventArgs = GetDrawerCancelCloseEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					if (settings.Transition == Transition.SlideOnTop || settings.Transition == Transition.Push)
					{
						currentDuration = (Math.Abs(ScreenWidth - navigationDrawerLayout.TranslationX) / settings.DrawerWidth) * settings.Duration;
					}
					else
					{
						currentDuration = (Math.Abs(ContentView.TranslationX) / settings.DrawerWidth) * settings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationX = d, navigationDrawerLayout.TranslationX, ScreenWidth);
					var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (settings.Transition == Transition.SlideOnTop)
					{
						navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)settings.Duration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
if (navigationDrawerLayout.TranslationX == ScreenWidth)
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(settings.Duration / 2), easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
					}
					else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
					{
						var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationX = d, ContentView.TranslationX, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
						ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
							if (ContentView.TranslationX == 0)
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						DrawerPushAnimation(currentDuration, drawerAnimation, isPrimaryDrawer);
					}

					SetDrawerOpenState(isPrimaryDrawer, false);
					_toggledEventArgs.IsOpen = false;
					settings.IsOpen = false;
				}
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the top side.
		/// </summary>
		void DrawerTopIn(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			bool isDrawerOpen = GetDrawerOpenState(isPrimaryDrawer);
			DrawerCancelEventArgs _cancelOpenEventArgs = GetDrawerCancelOpenEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				if (navigationDrawerLayout.TranslationY != -((ScreenHeight / 2) - (settings.DrawerHeight / 2)) - _drawerMoveTop || (settings.Transition == Transition.Reveal && ContentView?.TranslationY != settings.DrawerHeight))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent(isPrimaryDrawer);
						if (DeviceInfo.Platform == DevicePlatform.WinUI && settings.Transition != Transition.Reveal)
						{
							navigationDrawerLayout.TranslationY += _toolBarHeight;
						}
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration = 0;
						double pushDuration = 0;
						if (settings.Transition != Transition.Reveal)
						{
							currentDuration = Math.Abs(-((ScreenHeight / 2) - (settings.DrawerHeight / 2)) - navigationDrawerLayout.TranslationY) / settings.DrawerHeight * settings.Duration;
							if (settings.Transition == Transition.Push)
							{
								pushDuration = Math.Abs((ScreenHeight / 2) - (settings.DrawerHeight / 2) + navigationDrawerLayout.TranslationY) / settings.DrawerHeight * settings.Duration;
							}
						}
						else
						{
							currentDuration = Math.Abs(settings.DrawerHeight - ContentView?.TranslationY ?? 0) / settings.DrawerHeight * settings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationY = d, navigationDrawerLayout.TranslationY, -((ScreenHeight / 2) - (settings.DrawerHeight / 2)) - _drawerMoveTop);
						var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (settings.Transition == Transition.SlideOnTop)
						{
							navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
							});
						}
						else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
						{
							var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, settings.DrawerHeight);
							var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationY = d, ContentView.TranslationY, settings.DrawerHeight);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
							ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
							if (settings.Transition == Transition.Push)
							{
								navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)pushDuration, easing: Easing.Linear);
							}
						}

						SetDrawerOpenState(isPrimaryDrawer, true);
						_toggledEventArgs.IsOpen = true;
						settings.IsOpen = true;
						_toggledEventArgs.IsPrimary = isPrimaryDrawer;
					}
					else
					{
						_greyOverlayGrid.TranslationX = ScreenWidth;
					}
				}
				else if (navigationDrawerLayout.TranslationY == -((ScreenHeight / 2) - (settings.DrawerHeight / 2)) - _drawerMoveTop)
				{
					SetDrawerOpenState(isPrimaryDrawer, true);
					settings.IsOpen = true;
				}
			}
		}

		/// <summary>
		///  Method used to provide the animation to the drawer when moving towards the top side.
		/// </summary>
		void DrawerTopOut(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			DrawerCancelEventArgs cancelCloseEventArgs = GetDrawerCancelCloseEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null && ContentView != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					double pushDuration = 0;
					if (settings.Transition != Transition.Reveal)
					{
						currentDuration = (Math.Abs(((ScreenHeight / 2) + (settings.DrawerHeight / 2)) + navigationDrawerLayout.TranslationY) / settings.DrawerHeight) * settings.Duration;
						if (settings.Transition == Transition.Push)
						{
							pushDuration = Math.Abs((ScreenHeight / 2) + (settings.DrawerHeight / 2) + navigationDrawerLayout.TranslationY + _toolBarHeight) / settings.DrawerHeight * settings.Duration;
						}
					}
					else
					{
						currentDuration = Math.Abs(ContentView.TranslationY) / settings.DrawerHeight * settings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationY = d, navigationDrawerLayout.TranslationY, -((ScreenHeight / 2) + (settings.DrawerHeight / 2) + _toolBarHeight));
					var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (settings.Transition == Transition.SlideOnTop)
					{
						navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)settings.Duration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
if (navigationDrawerLayout.TranslationY == -((ScreenHeight / 2) + (settings.DrawerHeight / 2) + _toolBarHeight))
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(settings.Duration / 2), easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
					}
					else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
					{
						var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, 0);
						var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationY = d, ContentView.TranslationY, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
						ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
							if (ContentView.TranslationY == 0)
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						if (settings.Transition == Transition.Push)
						{
							navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)pushDuration, easing: Easing.Linear);
						}
					}

					SetDrawerOpenState(isPrimaryDrawer, false);
					_toggledEventArgs.IsOpen = false;
					settings.IsOpen = false;
				}
			}
		}

		/// <summary>
		/// Method used to provide the animation to the drawer when moving from the bottom side.
		/// </summary>
		void DrawerBottomIn(bool isPrimaryDrawer)
		{
			 DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			bool isDrawerOpen = GetDrawerOpenState(isPrimaryDrawer);
			DrawerCancelEventArgs _cancelOpenEventArgs = GetDrawerCancelOpenEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				if (navigationDrawerLayout.TranslationY != (ScreenHeight / 2) - (settings.DrawerHeight / 2) || (settings.Transition == Transition.Reveal && ContentView?.TranslationY != -settings.DrawerHeight))
				{
					if (_actionFirstMoveOpen && _actionFirstMoveClose)
					{
						SetDrawerOpeningEvent(isPrimaryDrawer);
					}

					if (!_cancelOpenEventArgs.Cancel)
					{
						double currentDuration = 0;
						if (settings.Transition != Transition.Reveal)
						{
							currentDuration = Math.Abs((ScreenHeight / 2) - (settings.DrawerHeight / 2) - navigationDrawerLayout.TranslationY) / settings.DrawerHeight * settings.Duration;
						}
						else
						{
							currentDuration = Math.Abs(settings.DrawerHeight + ContentView?.TranslationY ?? 0) / settings.DrawerHeight * settings.Duration;
						}

						currentDuration = ValidateCurrentDuration(currentDuration);
						var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationY = d, navigationDrawerLayout.TranslationY, (ScreenHeight / 2) - (settings.DrawerHeight / 2));
						var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0.5);
						var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, 0);
						if (settings.Transition == Transition.SlideOnTop)
						{
							navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear, finished: (v, e) =>
							{
								_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
							});
						}
						else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
						{
							var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, -settings.DrawerHeight);
							var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationY = d, ContentView.TranslationY, -settings.DrawerHeight);
							_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear);
							_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
							ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
							{
								OnDrawerOpenedToggledEvent(isPrimaryDrawer);
							});
						DrawerPushAnimation(currentDuration, drawerAnimation, isPrimaryDrawer);
					}

					SetDrawerOpenState(isPrimaryDrawer, true);
					_toggledEventArgs.IsOpen = true;
					settings.IsOpen = true;
					_toggledEventArgs.IsPrimary = isPrimaryDrawer;
				}
				else
				{
					_greyOverlayGrid.TranslationX = ScreenWidth;
				}
			}
				else if (navigationDrawerLayout.TranslationY == (ScreenHeight / 2) - (settings.DrawerHeight / 2))
				{
					SetDrawerOpenState(isPrimaryDrawer, true);
					settings.IsOpen = true;
				}
			}
		}

		/// <summary>
		///  Method used to provide the animation to the drawer when moving towards the bottom side.
		/// </summary>
		void DrawerBottomOut(bool isPrimaryDrawer)
		{
			DrawerSettings? settings = GetDrawerSettings(isPrimaryDrawer);
			SfGrid? navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
			DrawerCancelEventArgs cancelCloseEventArgs = GetDrawerCancelCloseEventArgs(isPrimaryDrawer);

			if (_greyOverlayGrid != null && navigationDrawerLayout != null && settings != null)
			{
				if (_actionFirstMoveOpen && _actionFirstMoveClose)
				{
					OnDrawerClosing(cancelCloseEventArgs);
					FirstMoveActionCompleted();
				}

				if (!cancelCloseEventArgs.Cancel)
				{
					double currentDuration;
					if (settings.Transition != Transition.Reveal)
					{
						currentDuration = Math.Abs((ScreenHeight / 2) + (settings.DrawerHeight / 2) - navigationDrawerLayout.TranslationY) / settings.DrawerHeight * settings.Duration;
					}
					else
					{
						currentDuration = (Math.Abs(ContentView?.TranslationY ?? 0) / settings.DrawerHeight) * settings.Duration;
					}

					currentDuration = ValidateCurrentDuration(currentDuration);
					var drawerAnimation = new Animation(d => navigationDrawerLayout.TranslationY = d, navigationDrawerLayout.TranslationY, (ScreenHeight / 2) + (settings.DrawerHeight / 2));
					var greyOverlayAnimation = new Animation(d => _greyOverlayGrid.Opacity = double.IsNaN(d) ? 0 : d, _greyOverlayGrid.Opacity, 0);
					var greyOverlayTranslateAnimation = new Animation(d => _greyOverlayGrid.TranslationX = d, _greyOverlayGrid.TranslationX, ScreenWidth);
					if (settings.Transition == Transition.SlideOnTop)
					{
						navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
if (navigationDrawerLayout.TranslationY == (ScreenHeight / 2) + (settings.DrawerHeight / 2))
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
					}
					else if ((settings.Transition == Transition.Push || settings.Transition == Transition.Reveal) && ContentView != null)
					{
						var greyOverlayTranslatePushAnimation = new Animation(d => _greyOverlayGrid.TranslationY = d, _greyOverlayGrid.TranslationY, 0);
						var contentViewTranslatePushAnimation = new Animation(d => ContentView.TranslationY = d, ContentView.TranslationY, 0);
						_greyOverlayGrid.Animate("greyOverlayTranslatePushAnimation", greyOverlayTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
							_greyOverlayGrid.Animate("greyOverlayTranslateAnimation", greyOverlayTranslateAnimation, length: 0, easing: Easing.Linear);
						});
						_greyOverlayGrid.Animate("greyOverlayAnimation", greyOverlayAnimation, length: (uint)(currentDuration / 2), easing: Easing.Linear);
						ContentView.Animate("contentViewTranslatePushAnimation", contentViewTranslatePushAnimation, length: (uint)currentDuration, easing: Easing.Linear, finished: (v, e) =>
						{
#if WINDOWS
							if (ContentView.TranslationY == 0)
#endif
							{
								OnDrawerClosedToggledEvent(isPrimaryDrawer);
							}
						});
						DrawerPushAnimation(currentDuration, drawerAnimation, isPrimaryDrawer);
					}

					SetDrawerOpenState(isPrimaryDrawer, false);
					_toggledEventArgs.IsOpen = false;
					settings.IsOpen = false;
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
		/// <param name="isPrimaryDrawer"></param>
        void SetVisibility(bool isVisible, bool isPrimaryDrawer)
        {
            if (_greyOverlayGrid != null)
            {
                var Settings = isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
                bool isViewsInitialized = Settings.DrawerContentView !=null || Settings.DrawerHeaderView!=null|| Settings.DrawerFooterView!=null;
                var navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
                if (navigationDrawerLayout != null && isViewsInitialized)
                {
                    navigationDrawerLayout.IsVisible = isVisible;
                    _greyOverlayGrid.IsVisible = isVisible;

#if MACCATALYST || IOS
                    var settings = GetDrawerSettings(isPrimaryDrawer);
                    if (settings is not null && settings.DrawerContentView is not null)
                    {
                        if (settings.DrawerContentView is View view)
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
        }

#if !WINDOWS
        void UpdateSecondaryDrawerFlowDirection()
        {
            if (_secondaryDrawerLayout != null && ContentView != null)
            {
                if (_isRTL)
                {
                    _secondaryDrawerLayout.FlowDirection = ContentView.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    _secondaryDrawerLayout.FlowDirection = ContentView.FlowDirection = FlowDirection.LeftToRight;
                }
            }
        }
#endif

        void UpdateSecondaryDrawerContentView()
        {
            if (_secondaryDrawerLayout != null)
            {
                if (SecondaryDrawerSettings.DrawerContentView != null)
                {
                    _secondaryDrawerLayout.Remove(SecondaryDrawerSettings.DrawerContentView);
                    _secondaryDrawerLayout.SetRow(SecondaryDrawerSettings.DrawerContentView, 1);
                    _secondaryDrawerLayout.Children.Add(SecondaryDrawerSettings.DrawerContentView);
#if !WINDOWS
                    UpdateSecondaryDrawerFlowDirection();
#endif
                }
            }
        }

        void UpdateSecondaryDrawerHeaderView()
        {
            if (_secondaryDrawerLayout != null)
            {
                UpdateSecondaryDrawerHeaderFooterSize();
                if (SecondaryDrawerSettings.DrawerHeaderView != null)
                {
                    _secondaryDrawerLayout.Remove(SecondaryDrawerSettings.DrawerHeaderView);
                    _secondaryDrawerLayout.SetRow(SecondaryDrawerSettings.DrawerHeaderView, 0);
                    _secondaryDrawerLayout.Children.Add(SecondaryDrawerSettings.DrawerHeaderView);
                    _oldSecondaryHeaderView = SecondaryDrawerSettings.DrawerHeaderView;
                }
                else
                {
                    _secondaryDrawerLayout.Remove(_oldSecondaryHeaderView);
                    _oldSecondaryHeaderView = SecondaryDrawerSettings.DrawerHeaderView;
                }
            }
        }

        void UpdateSecondaryDrawerFooterView()
        {
            if (_secondaryDrawerLayout != null)
            {
                UpdateSecondaryDrawerHeaderFooterSize();
                if (SecondaryDrawerSettings.DrawerFooterView != null)
                {
                    _secondaryDrawerLayout.Remove(SecondaryDrawerSettings.DrawerFooterView);
                    _secondaryDrawerLayout.SetRow(SecondaryDrawerSettings.DrawerFooterView, 2);
                    _secondaryDrawerLayout.Children.Add(SecondaryDrawerSettings.DrawerFooterView);
                    _oldSecondaryFooterView = SecondaryDrawerSettings.DrawerFooterView;
                }
                else
                {
                    _secondaryDrawerLayout.Remove(_oldSecondaryFooterView);
                    _oldSecondaryFooterView = SecondaryDrawerSettings.DrawerFooterView;
                }
            }
        }

        void UpdateSecondaryDrawerHeaderFooterSize()
        {
            if (_secondaryDrawerLayout != null)
            {
                double headerHeight = 0;
                double footerHeight = 0;
                if (SecondaryDrawerSettings.DrawerHeaderView != null)
                {
                    headerHeight = (SecondaryDrawerSettings.DrawerHeaderHeight < 0) ? (double)DrawerSettings.DrawerHeaderHeightProperty.DefaultValue : SecondaryDrawerSettings.DrawerHeaderHeight;
                    SecondaryDrawerSettings.DrawerHeaderView.IsVisible = headerHeight > 0;
                }

                if (SecondaryDrawerSettings.DrawerFooterView != null)
                {
                    footerHeight = (SecondaryDrawerSettings.DrawerFooterHeight < 0) ? (double)DrawerSettings.DrawerFooterHeightProperty.DefaultValue : SecondaryDrawerSettings.DrawerFooterHeight;
                    SecondaryDrawerSettings.DrawerFooterView.IsVisible = footerHeight > 0;
                }

                _secondaryDrawerLayout.RowDefinitions.Clear();
                _secondaryDrawerLayout.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition { Height = new GridLength(headerHeight) },
                    new RowDefinition(),
                    new RowDefinition { Height = new GridLength(footerHeight) },
                };
            }
        }

        void UpdateSecondaryDrawerLayoutSize()
        {
            if (_secondaryDrawerLayout != null)
            {
                SecondaryPositionUpdate();
            }
        }
        void UpdateSecondaryContentBackground()
        {
            if (_secondaryDrawerLayout != null && SecondaryDrawerSettings != null)
            {
                _secondaryDrawerLayout.BackgroundColor = SecondaryDrawerSettings.ContentBackground;
               
            }
        }

        void UpdateSecondaryIsOpen()
        {
            if (ScreenHeight > 0)
            {
                if ((SecondaryDrawerSettings.IsOpen && !_isSecondaryDrawerOpen) || (!SecondaryDrawerSettings.IsOpen && _isSecondaryDrawerOpen))
                {
                    if (!SecondaryDrawerSettings.IsOpen || DrawerSettings.IsOpen != SecondaryDrawerSettings.IsOpen)
                    {
                        ToggleSecondaryDrawer();
                    }
                }
            }
        }

        void UpdateSecondaryTransitionAnimation()
        {
            Children.Clear();
            UpdateAllChild();
            SecondaryPositionUpdate();
        }

        void UpdateSecondaryTouchThreshold()
        {
            if (SecondaryDrawerSettings != null)
            {
                if (SecondaryDrawerSettings.Position == Position.Right)
                {
                    _touchRightThreshold = ScreenWidth - SecondaryDrawerSettings.TouchThreshold;
                }
                else if (SecondaryDrawerSettings.Position == Position.Bottom)
                {
                    _touchBottomThreshold = ScreenHeight - SecondaryDrawerSettings.TouchThreshold;
                }
            }
        }

		void SecondaryPositionUpdate()
        {
            if (_secondaryDrawerLayout != null && _greyOverlayGrid != null && SecondaryDrawerSettings != null)
            {
#if !WINDOWS
                if ((SecondaryDrawerSettings.Position == Position.Right && !_isRTL) || (SecondaryDrawerSettings.Position == Position.Left && _isRTL))
#else
                if (SecondaryDrawerSettings.Position == Position.Right)
#endif
                {
                    _secondaryDrawerLayout.WidthRequest = SecondaryDrawerSettings.DrawerWidth;
                    _secondaryDrawerLayout.HeightRequest = ScreenHeight;
                    _secondaryDrawerLayout.TranslationY = 0;
                    if (_isSecondaryDrawerOpen)
                    {
                        SetVisibility(true,false);
                        _secondaryDrawerLayout.TranslationX = ScreenWidth - SecondaryDrawerSettings.DrawerWidth;
                        if (SecondaryDrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
                        {
                            _greyOverlayGrid.TranslationX = 0;
                            ContentView.TranslationX = 0;
                        }

                        if ((SecondaryDrawerSettings.Transition == Transition.Push || SecondaryDrawerSettings.Transition == Transition.Reveal) && ContentView != null)
                        {
                            ContentView.TranslationX = -SecondaryDrawerSettings.DrawerWidth;
                            _greyOverlayGrid.TranslationX = -SecondaryDrawerSettings.DrawerWidth;
                            ContentView.TranslationY = 0;
                            _greyOverlayGrid.TranslationY = 0;
                        }
                    }
                    else
                    {
#if WINDOWS || MACCATALYST
                        if ((!_secondaryDrawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (SecondaryDrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")))
#endif
                        {
                            if (!DrawerSettings.IsOpen)
                            {
                                SetVisibility(false, false);
                            }
                        }
                        if(!DrawerSettings.IsOpen)
                        {
                            _greyOverlayGrid.TranslationX = ScreenWidth;
                        }

                        if (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null)
                        {
                            _secondaryDrawerLayout.TranslationX = ScreenWidth - SecondaryDrawerSettings.DrawerWidth;
                            ContentView.TranslationX = 0;
                        }
                        else
                        {
                            _secondaryDrawerLayout.TranslationX = ScreenWidth;
                            if (SecondaryDrawerSettings.Transition == Transition.Push && ContentView != null)
                            {
                                ContentView.TranslationX = 0;
                            }
                        }
                    }

                    _touchRightThreshold = ScreenWidth - SecondaryDrawerSettings.TouchThreshold;
                }
#if !WINDOWS
                else if ((SecondaryDrawerSettings.Position == Position.Left && !_isRTL) || (SecondaryDrawerSettings.Position == Position.Right && _isRTL))
#else
                else if (SecondaryDrawerSettings.Position == Position.Left)
#endif
                {
                    _secondaryDrawerLayout.HorizontalOptions = LayoutOptions.Start;
                    _secondaryDrawerLayout.WidthRequest = SecondaryDrawerSettings.DrawerWidth;
                    _secondaryDrawerLayout.HeightRequest = ScreenHeight;
                    _secondaryDrawerLayout.TranslationY = 0;
                    if (_isSecondaryDrawerOpen)
                    {
                        SetVisibility(true,false);
                        _secondaryDrawerLayout.TranslationX = 0;
                        if (SecondaryDrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
                        {
                            _greyOverlayGrid.TranslationX = 0;
                            ContentView.TranslationX = 0;
                        }
                        else if ((SecondaryDrawerSettings.Transition == Transition.Push || SecondaryDrawerSettings.Transition == Transition.Reveal) && ContentView != null)
                        {
                            _greyOverlayGrid.TranslationX = SecondaryDrawerSettings.DrawerWidth;
                            ContentView.TranslationX = SecondaryDrawerSettings.DrawerWidth;
                            ContentView.TranslationY = 0;
                            _secondaryDrawerLayout.TranslationY = 0;
                        }
                    }
                    else
                    {
#if WINDOWS || MACCATALYST
                        if ((!_secondaryDrawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (SecondaryDrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")) && !DrawerSettings.IsOpen)
#endif
                        {
                            if (!DrawerSettings.IsOpen)
                            {
                                SetVisibility(false, false);
                            }
                        }
                        if(!DrawerSettings.IsOpen)
                        {
                            _greyOverlayGrid.TranslationX = ScreenWidth;
                        }
                        if (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null)
                        {
                            _secondaryDrawerLayout.TranslationX = 0;
                            ContentView.TranslationX = 0;
                        }
                        else
                        {
                            _secondaryDrawerLayout.TranslationX = -SecondaryDrawerSettings.DrawerWidth;
                            if (SecondaryDrawerSettings.Transition == Transition.Push && ContentView != null)
                            {
                                ContentView.TranslationX = 0;
                            }
                        }
                    }
#if !WINDOWS
                    if (_isRTL)
                    {
                        _touchRightThreshold = SecondaryDrawerSettings.TouchThreshold;
                    }
#endif
                }
                else if (SecondaryDrawerSettings.Position == Position.Top)
                {
                    _secondaryDrawerLayout.WidthRequest = ScreenWidth;
                    _secondaryDrawerLayout.HeightRequest = SecondaryDrawerSettings.DrawerHeight;
                    _secondaryDrawerLayout.TranslationX = 0;
                    if (_isSecondaryDrawerOpen)
                    {
                        SetVisibility(true,false);
                        _greyOverlayGrid.TranslationX = 0;
                        _secondaryDrawerLayout.TranslationY = -((ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2));
                        if (SecondaryDrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
                        {
                            ContentView.TranslationY = 0;
                            ContentView.TranslationX = 0;
                            _greyOverlayGrid.TranslationY = 0;
                        }
                        else if ((SecondaryDrawerSettings.Transition == Transition.Push || SecondaryDrawerSettings.Transition == Transition.Reveal) && ContentView != null)
                        {
                            _secondaryDrawerLayout.TranslationY -= _drawerMoveTop;
                            _greyOverlayGrid.TranslationY = SecondaryDrawerSettings.DrawerHeight;
                            ContentView.TranslationY = SecondaryDrawerSettings.DrawerHeight;
                            ContentView.TranslationX = 0;
                        }
                    }
                    else
                    {
#if WINDOWS || MACCATALYST
                        if ((!_secondaryDrawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (SecondaryDrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")) && !DrawerSettings.IsOpen)
#endif
                        {
                            if (!DrawerSettings.IsOpen)
                            {
                                SetVisibility(false, false);
                            }
                        }
                        if(!DrawerSettings.IsOpen)
                        {
                            _greyOverlayGrid.TranslationX = ScreenWidth;
                        }
                        if (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null)
                        {
                            _secondaryDrawerLayout.TranslationY = -((ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2));
                            ContentView.TranslationY = 0;
                        }
                        else
                        {
                            _secondaryDrawerLayout.TranslationY = -((ScreenHeight / 2) + (SecondaryDrawerSettings.DrawerHeight / 2) + _toolBarHeight);
                            if (SecondaryDrawerSettings.Transition == Transition.Push && ContentView != null)
                            {
                                ContentView.TranslationY = 0;
                                _secondaryDrawerLayout.TranslationY -= _drawerMoveTop;
                            }
                        }
                    }
                }
                else if (SecondaryDrawerSettings.Position == Position.Bottom)
                {
                    _secondaryDrawerLayout.WidthRequest = ScreenWidth;
                    _secondaryDrawerLayout.HeightRequest = SecondaryDrawerSettings.DrawerHeight;
                    _secondaryDrawerLayout.TranslationX = 0;
                    if (_isSecondaryDrawerOpen)
                    {
                        SetVisibility(true,false);
                        _greyOverlayGrid.TranslationX = 0;
                        _secondaryDrawerLayout.TranslationY = (ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2);
                        if (SecondaryDrawerSettings.Transition == Transition.SlideOnTop && ContentView != null)
                        {
                            ContentView.TranslationY = 0;
                            ContentView.TranslationX = 0;
                        }
                        else if ((SecondaryDrawerSettings.Transition == Transition.Push || SecondaryDrawerSettings.Transition == Transition.Reveal) && ContentView != null)
                        {
                            _greyOverlayGrid.TranslationY = -SecondaryDrawerSettings.DrawerHeight;
                            ContentView.TranslationY = -SecondaryDrawerSettings.DrawerHeight;
                            ContentView.TranslationX = 0;
                        }
                    }
                    else
                    {
#if WINDOWS || MACCATALYST
                        if ((!_secondaryDrawerLayout.AnimationIsRunning("drawerAnimation") && !_greyOverlayGrid.AnimationIsRunning("greyOverlayTranslateAnimation")) || (SecondaryDrawerSettings.Transition == Transition.Reveal && !ContentView.AnimationIsRunning("contentViewTranslatePushAnimation")) && !DrawerSettings.IsOpen)
#endif
                        {
                            if (!DrawerSettings.IsOpen)
                            {
                                SetVisibility(false, false);
                            }
                        }
                        if(!DrawerSettings.IsOpen)
                        {
                            _greyOverlayGrid.TranslationX = ScreenWidth;
                        }

                        if (SecondaryDrawerSettings.Transition == Transition.Reveal && ContentView != null)
                        {
                            _secondaryDrawerLayout.TranslationY = (ScreenHeight / 2) - (SecondaryDrawerSettings.DrawerHeight / 2);
                            ContentView.TranslationY = 0;
                        }
                        else
                        {
                            _secondaryDrawerLayout.TranslationY = (ScreenHeight / 2) + (SecondaryDrawerSettings.DrawerHeight / 2);
                            if (SecondaryDrawerSettings.Transition == Transition.Push && ContentView != null)
                            {
                                ContentView.TranslationY = 0;
                            }
                        }
                    }

                    _touchBottomThreshold = ScreenHeight - SecondaryDrawerSettings.TouchThreshold;
                }
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
        /// Methods to validate remain drawer height.
        /// </summary>
        void ValidateSecondaryRemainDrawerHeight()
        {
            if (_remainDrawerHeight < -SecondaryDrawerSettings.DrawerHeight)
            {
                _remainDrawerHeight = -SecondaryDrawerSettings.DrawerHeight;
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

		/// <summary>
        /// Methods to validate remain secondary drawer width.
        /// </summary>
        void ValidateSecondaryRemainDrawerWidth()
        {
            if (_remainDrawerWidth < -SecondaryDrawerSettings.DrawerWidth)
            {
                _remainDrawerWidth = -SecondaryDrawerSettings.DrawerWidth;
            }
            else if (_remainDrawerWidth > 0)
            {
                _remainDrawerWidth = 0;
            }
        }

		void SetDrawerOpeningEvent(bool isPrimaryDrawer)
        {
			DrawerCancelEventArgs cancelOpenEventArgs;
			if (isPrimaryDrawer)
            {
				cancelOpenEventArgs = _cancelOpenEventArgs;
            }
            else
            {
				cancelOpenEventArgs = _secondaryCancelOpenEventArgs;
            }

			OnDrawerOpening(cancelOpenEventArgs);
			SetVisibility(!cancelOpenEventArgs.Cancel, isPrimaryDrawer);
            FirstMoveActionCompleted();
        }
        void OnDrawerOpenedToggledEvent(bool isPrimaryDrawer)
        {
            if (isPrimaryDrawer)
            {
                OnDrawerOpened(new DrawerEventArgs(true));
                OnDrawerToggled(_toggledEventArgs);
            }
            else
            {
                OnDrawerOpened(new DrawerEventArgs(false));
                OnDrawerToggled(_toggledEventArgs);
            }
        }
        void DrawerPushAnimation(double currentDuration, Animation drawerAnimation, bool isPrimaryDrawer)
        {
            var settings = GetDrawerSettings(isPrimaryDrawer);
            var navigationDrawerLayout = GetDrawerLayout(isPrimaryDrawer);
            
            if (settings?.Transition == Transition.Push && navigationDrawerLayout != null)
            {
                navigationDrawerLayout.Animate("drawerAnimation", drawerAnimation, length: (uint)currentDuration, easing: Easing.Linear);
            }
        }

        /// <summary>
        /// Generic helper method to get drawer settings.
        /// </summary>
        DrawerSettings? GetDrawerSettings(bool isPrimaryDrawer)
        {
            return isPrimaryDrawer ? DrawerSettings : SecondaryDrawerSettings;
        }

        /// <summary>
        /// Generic helper method to get drawer layout.
        /// </summary>
        SfGrid? GetDrawerLayout(bool isPrimaryDrawer)
        {
            return isPrimaryDrawer ? _drawerLayout : _secondaryDrawerLayout;
        }

        /// <summary>
        /// Generic helper method to get drawer open state.
        /// </summary>
        bool GetDrawerOpenState(bool isPrimaryDrawer)
        {
            return isPrimaryDrawer ? _isDrawerOpen : _isSecondaryDrawerOpen;
        }

        /// <summary>
        /// Generic helper method to set drawer open state.
        /// </summary>
        void SetDrawerOpenState(bool isPrimaryDrawer, bool isOpen)
        {
            if (isPrimaryDrawer)
            {
                _isDrawerOpen = isOpen;
            }
            else
            {
                _isSecondaryDrawerOpen = isOpen;
            }
        }

        /// <summary>
        /// Generic helper method to get drawer cancel event args.
        /// </summary>
        DrawerCancelEventArgs GetDrawerCancelOpenEventArgs(bool isPrimaryDrawer)
        {
            return isPrimaryDrawer ? _cancelOpenEventArgs : _secondaryCancelOpenEventArgs;
        }

        /// <summary>
        /// Generic helper method to get drawer cancel close event args.
        /// </summary>
        DrawerCancelEventArgs GetDrawerCancelCloseEventArgs(bool isPrimaryDrawer)
        {
            return isPrimaryDrawer ? _cancelCloseEventArgs : _secondaryCancelCloseEventArgs;
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
			if (DrawerSettings != null && _drawerLayout != null && _secondaryDrawerLayout != null)
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

				if (widthConstraint > 0 && widthConstraint != double.PositiveInfinity && _secondaryDrawerLayout.WidthRequest != widthConstraint)
				{
					if (SecondaryDrawerSettings.Position == Position.Bottom || SecondaryDrawerSettings.Position == Position.Top)
					{
						_secondaryDrawerLayout.WidthRequest = widthConstraint;
					}
				}

				if (heightConstraint > 0 && heightConstraint != double.PositiveInfinity && _secondaryDrawerLayout.HeightRequest != heightConstraint)
				{
					if (SecondaryDrawerSettings.Position == Position.Left || SecondaryDrawerSettings.Position == Position.Right)
					{
						_secondaryDrawerLayout.HeightRequest = heightConstraint;
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
					UpdateSecondaryIsOpen();
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
			if (SecondaryDrawerSettings != null)
			{
				SfNavigationDrawer.SetInheritedBindingContext(SecondaryDrawerSettings, BindingContext);
			}
			UpdateAllChild();
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

		static void OnSecondaryDrawerSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var drawer = bindable as SfNavigationDrawer;
            if (drawer != null)
            {
                drawer.OnSecondaryDrawerSettingsChanged();
                drawer.SecondaryDrawerSettings.PropertyChanged -= drawer.OnSecondaryDrawerSettings_PropertyChanged;
                drawer.SecondaryDrawerSettings.PropertyChanged += drawer.OnSecondaryDrawerSettings_PropertyChanged;
            }

            if (oldValue != null)
            {
                if (oldValue is DrawerSettings previousSetting)
                {
                    previousSetting.PropertyChanged -= drawer!.OnSecondaryDrawerSettings_PropertyChanged;
                    previousSetting.BindingContext = null;
                    previousSetting.Parent = null;
                }
            }
            if (newValue != null)
            {
                if (newValue is DrawerSettings currentSetting && bindable is SfNavigationDrawer sfNavigationDrawer)
                {
                    currentSetting.Parent = sfNavigationDrawer;
                    SetInheritedBindingContext(sfNavigationDrawer.SecondaryDrawerSettings, sfNavigationDrawer.BindingContext);
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
				navigation.UpdateSecondaryDrawerFlowDirection();
				navigation.PositionUpdate();
				navigation.SecondaryPositionUpdate();
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
		public event EventHandler<DrawerEventArgs>? DrawerClosed;

		/// <summary>
		/// It is drawer opened event.
		/// </summary>
		public event EventHandler<DrawerEventArgs>? DrawerOpened;

		/// <summary>
		/// It is drawer toggled event.
		/// </summary>
		public event EventHandler<ToggledEventArgs>? DrawerToggled;

		/// <summary>
		/// It is drawer closing event.
		/// </summary>
		public event EventHandler<DrawerCancelEventArgs>? DrawerClosing;

		/// <summary>
		/// It is drawer opening event.
		/// </summary>
		public event EventHandler<DrawerCancelEventArgs>? DrawerOpening;

		#endregion
	}
}
