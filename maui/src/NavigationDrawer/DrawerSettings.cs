namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
	/// <summary>
	/// Drawer settings class for customize the drawer.
	/// </summary>
	public partial class DrawerSettings : Element
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="DrawerWidth"/> bindable property.
		/// </summary>
		public static readonly BindableProperty DrawerWidthProperty =
			BindableProperty.Create(
				nameof(DrawerWidth),
				typeof(double),
				typeof(DrawerSettings),
				200d,
				BindingMode.Default,
				null,
				propertyChanged: OnDrawerWidthChanged);

		/// <summary>
		/// Identifies the <see cref="DrawerHeight"/> bindable property.
		/// </summary>
		public static readonly BindableProperty DrawerHeightProperty =
			BindableProperty.Create(
				nameof(DrawerHeight),
				typeof(double),
				typeof(DrawerSettings),
				500d,
				BindingMode.Default,
				null,
				propertyChanged: OnDrawerHeightChanged);

		/// <summary>
		/// Identifies the <see cref="DrawerHeaderHeight"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="DrawerHeaderView"/> can be removed by setting the <see cref="DrawerHeaderHeight"/> to zero.
		/// </remarks>
		public static readonly BindableProperty DrawerHeaderHeightProperty =
			BindableProperty.Create(
				nameof(DrawerHeaderHeight),
				typeof(double),
				typeof(DrawerSettings),
				50d,
				BindingMode.Default,
				null,
				propertyChanged: OnDrawerHeaderHeightChanged);

		/// <summary>
		/// Identifies the <see cref="DrawerFooterHeight"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="DrawerFooterView"/> can be removed by setting the <see cref="DrawerFooterHeight"/> to zero.
		/// </remarks>
		public static readonly BindableProperty DrawerFooterHeightProperty =
			BindableProperty.Create(
				nameof(DrawerFooterHeight),
				typeof(double),
				typeof(DrawerSettings),
				50d,
				BindingMode.Default,
				null,
				propertyChanged: OnDrawerFooterHeightChanged);

		/// <summary>
		/// Identifies the <see cref="DrawerHeaderView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="DrawerHeaderView"/> can be removed by setting the <see cref="DrawerHeaderHeight"/> to zero.
		/// </remarks>
		public static readonly BindableProperty DrawerHeaderViewProperty =
			BindableProperty.Create(
				nameof(DrawerHeaderView),
				typeof(View),
				typeof(DrawerSettings),
				null,
				BindingMode.Default,
				null,
				propertyChanged: DrawerHeaderViewChanged);

		/// <summary>
		/// Identifies the <see cref="DrawerContentView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="DrawerContentView"/> is mandatory to allocate space for the drawer.
		/// </remarks>
		public static readonly BindableProperty DrawerContentViewProperty =
			BindableProperty.Create(
				nameof(DrawerContentView),
				typeof(View),
				typeof(DrawerSettings),
				null,
				BindingMode.Default,
				null,
				propertyChanged: DrawerContentViewChanged);

		/// <summary>
		/// Identifies the <see cref="DrawerFooterView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="DrawerFooterView"/> can be removed by setting the <see cref="DrawerFooterHeight"/> to zero.
		/// </remarks>
		public static readonly BindableProperty DrawerFooterViewProperty =
			BindableProperty.Create(
				nameof(DrawerFooterView),
				typeof(View),
				typeof(DrawerSettings),
				null,
				BindingMode.Default,
				null,
				propertyChanged: DrawerFooterViewChanged);

		/// <summary>
		/// Identifies the <see cref="Position"/> bindable property.
		/// </summary>
		public static readonly BindableProperty PositionProperty =
			BindableProperty.Create(
				nameof(Position),
				typeof(Position),
				typeof(DrawerSettings),
				Position.Left,
				BindingMode.Default,
				null,
				propertyChanged: PositionChanged);

		/// <summary>
		/// Identifies the <see cref="Duration"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Duration"/> property for the <see cref="SfNavigationDrawer"/> is measured in milliseconds.
		/// </remarks>
		public static readonly BindableProperty DurationProperty =
			BindableProperty.Create(
				nameof(Duration),
				typeof(double),
				typeof(DrawerSettings),
				400d,
				BindingMode.Default,
				null,
				propertyChanged: DurationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnableSwipeGesture"/> bindable property.
		/// </summary>
		public static readonly BindableProperty EnableSwipeGestureProperty =
			BindableProperty.Create(
				nameof(EnableSwipeGesture),
				typeof(bool),
				typeof(DrawerSettings),
				true,
				BindingMode.Default,
				null);

		/// <summary>
		/// Identifies the <see cref="ContentBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ContentBackgroundProperty =
			BindableProperty.Create(
				nameof(ContentBackground),
				typeof(Color),
				typeof(DrawerSettings),
				Color.FromArgb("F7F2FB"),
				BindingMode.Default,
				null,
				propertyChanged: OnContentBackgroundChanged);

		/// <summary>
		/// Identifies the <see cref="Transition"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TransitionProperty =
			BindableProperty.Create(
				nameof(Transition),
				typeof(Transition),
				typeof(DrawerSettings),
				Transition.SlideOnTop,
				BindingMode.Default,
				null,
				propertyChanged: OnTransitionPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TouchThreshold"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TouchThresholdProperty =
			BindableProperty.Create(
				nameof(TouchThreshold),
				typeof(double),
				typeof(DrawerSettings),
				120d,
				BindingMode.Default,
				null,
				propertyChanged: OnTouchThresholdPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AnimationEasing"/> bindable property.
		/// </summary>
		public static readonly BindableProperty AnimationEasingProperty = BindableProperty.Create(nameof(AnimationEasing), typeof(Easing), typeof(DrawerSettings), Easing.Linear);


		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="DrawerSettings"/> class.
		/// </summary>
		public DrawerSettings()
		{
			SetDynamicResource(ContentBackgroundProperty, "SfNavigationDrawerContentBackground");
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value that can be used to modify the drawer's width.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerWidth"/> values and the default value is 200d.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     DrawerWidth = 300
		/// };
		/// ]]></code>
		/// </example>
		public double DrawerWidth
		{
			get { return (double)GetValue(DrawerWidthProperty); }
			set { SetValue(DrawerWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to modify the drawer's height.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerHeight"/> values and the default value is 500d.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     DrawerHeight = 600
		/// };
		/// ]]></code>
		/// </example>
		public double DrawerHeight
		{
			get { return (double)GetValue(DrawerHeightProperty); }
			set { SetValue(DrawerHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the navigation drawer's header height.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerHeaderHeight"/> values and the default value is 50d.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     DrawerHeaderHeight = 100
		/// };
		/// ]]></code>
		/// </example>
		public double DrawerHeaderHeight
		{
			get { return (double)GetValue(DrawerHeaderHeightProperty); }
			set { SetValue(DrawerHeaderHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the navigation drawer's footer height.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerFooterHeight"/> values and the default value is 50d.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     DrawerFooterHeight = 100
		/// };
		/// ]]></code>
		/// </example>
		public double DrawerFooterHeight
		{
			get { return (double)GetValue(DrawerFooterHeightProperty); }
			set { SetValue(DrawerFooterHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view that can be used to customize the drawer header view of SfNavigationDrawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerHeaderView"/> values and the default value is null.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     DrawerHeaderView = new Label { Text = "Header" }
		/// };
		/// ]]></code>
		/// </example>
		public View DrawerHeaderView
		{
			get { return (View)GetValue(DrawerHeaderViewProperty); }
			set
			{
				DetachOldViewFromDrawerView(DrawerHeaderViewProperty);
				SetValue(DrawerHeaderViewProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the view that can be used to customize the drawer content view of SfNavigationDrawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerContentView"/> values and the default value is null.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     DrawerContentView = new StackLayout
		///     {
		///         Children =
		///         {
		///             new Label { Text = "Content" },
		///             new Button { Text = "Click Me" }
		///         }
		///     }
		/// };
		/// ]]></code>
		/// </example>
		public View DrawerContentView
		{
			get { return (View)GetValue(DrawerContentViewProperty); }
			set
			{
				DetachOldViewFromDrawerView(DrawerContentViewProperty);
				SetValue(DrawerContentViewProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the view that can be used to customize the drawer footer view of SfNavigationDrawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DrawerFooterView"/> values and the default value is null.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     DrawerFooterView = new StackLayout
		///     {
		///         Children =
		///         {
		///             new Label { Text = "Footer" },
		///             new Button { Text = "Close" }
		///         }
		///     }
		/// };
		/// ]]></code>
		/// </example>
		public View DrawerFooterView
		{
			get { return (View)GetValue(DrawerFooterViewProperty); }
			set
			{
				DetachOldViewFromDrawerView(DrawerFooterViewProperty);
				SetValue(DrawerFooterViewProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the drawer's position.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Position"/> values and the default value is <see cref="Position.Left"/>.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     Position = Position.Right
		/// };
		/// ]]></code>
		/// </example>
		public Position Position
		{
			get { return (Position)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to modify the animation duration of the drawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Duration"/> values and the default value is 400d.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     Duration = 300
		/// };
		/// ]]></code>
		/// </example>
		public double Duration
		{
			get { return (double)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the swiping gesture to open the drawer is enabled or not.
		/// </summary>
		/// <value>
		/// It accepts <see cref="EnableSwipeGesture"/> values and the default value is true.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     EnableSwipeGesture = true
		/// };
		/// ]]></code>
		/// </example>
		public bool EnableSwipeGesture
		{
			get { return (bool)GetValue(EnableSwipeGestureProperty); }
			set { SetValue(EnableSwipeGestureProperty, value); }
		}

		/// <summary>
		/// Gets or sets the easing function that controls the acceleration and deceleration of the drawer's open and close animations.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Easing"/> values and the default value is Easing.Linear.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     AnimationEasing = Easing.SpringIn
		/// };
		/// ]]></code>
		/// </example>
		public Easing AnimationEasing
		{
			get => (Easing)GetValue(AnimationEasingProperty); 
			set => SetValue(AnimationEasingProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the background color of the drawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="ContentBackground"/> values and the default value is "#F7F2FB".
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     ContentBackground = Colors.Blue
		/// };
		/// ]]></code>
		/// </example>
		public Color ContentBackground
		{
			get { return (Color)GetValue(ContentBackgroundProperty); }
			set { SetValue(ContentBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to modify the animation of the drawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Transition"/> values and the default value is <see cref="Transition.SlideOnTop"/>.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     Transition = Transition.SlideOnTop
		/// };
		/// ]]></code>
		/// </example>
		public Transition Transition
		{
			get { return (Transition)GetValue(TransitionProperty); }
			set { SetValue(TransitionProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to modify the touch threshold of the drawer.
		/// </summary>
		/// <value>
		/// It accepts <see cref="TouchThreshold"/> values and the default value is 120d.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var drawerSettings = new DrawerSettings
		/// {
		///     TouchThreshold = 50
		/// };
		/// ]]></code>
		/// </example>
		public double TouchThreshold
		{
			get { return (double)GetValue(TouchThresholdProperty); }
			set { SetValue(TouchThresholdProperty, value); }
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// This method is used to assign the default when the value of drawer width is lesser than zero.
		/// </summary>
		internal void UpdateDrawerWidth()
		{
			if (DrawerWidth < 0)
			{
				DrawerWidth = (double)DrawerWidthProperty.DefaultValue;
			}
		}

		/// <summary>
		/// This method is used to assign the default when the value of drawer height is lesser than zero.
		/// </summary>
		internal void UpdateDrawerHeight()
		{
			if (DrawerHeight < 0)
			{
				DrawerHeight = (double)DrawerHeightProperty.DefaultValue;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Detaches the old view from the drawer view.
		/// </summary>
		/// <param name="bindableProperty">The bindable property.</param>
		void DetachOldViewFromDrawerView(BindableProperty bindableProperty)
		{
			if (GetValue(bindableProperty) is View oldView)
			{
				(oldView.Parent as Layout)?.Children.Remove(oldView);
			}
		}

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Invoked whenever the <see cref="DrawerWidthProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnDrawerWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.UpdateDrawerWidth();
				settings.OnPropertyChanged(nameof(DrawerWidth));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DrawerWidthProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnDrawerHeightChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.UpdateDrawerHeight();
				settings.OnPropertyChanged(nameof(DrawerHeight));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DrawerHeaderHeightProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnDrawerHeaderHeightChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(DrawerHeaderHeight));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DrawerFooterHeightProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnDrawerFooterHeightChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(DrawerFooterHeight));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DrawerHeaderViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void DrawerHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(DrawerHeaderView));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DrawerContentViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void DrawerContentViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(DrawerContentView));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DrawerFooterViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void DrawerFooterViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(DrawerFooterView));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="PositionProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void PositionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(Position));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DurationProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void DurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(Duration));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ContentBackgroundProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnContentBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(ContentBackground));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="TransitionProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnTransitionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(Transition));
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="TouchThresholdProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnTouchThresholdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DrawerSettings settings)
			{
				settings.OnPropertyChanged(nameof(TouchThreshold));
			}
		}

		#endregion
	}
}
