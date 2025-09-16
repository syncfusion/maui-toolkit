using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
	/// <summary>
	/// <see cref="SfPullToRefresh"/> enables interaction to refresh the loaded view. This control allows users to trigger a refresh action by performing the pull-to-refresh gesture.
	/// </summary>
	public partial class SfPullToRefresh : PullToRefreshBase, ITouchListener, IParentThemeElement
	{
		#region Fields

		/// <summary>
		/// Below field maintains the current pulling progress rate.
		/// </summary>
		double _progressRate = 0;

		/// <summary>
		/// Below field maintains the view created from <see cref="SfPullToRefresh.PullingViewTemplate"/>.
		/// </summary>
		View? _pullingTemplateView;

		/// <summary>
		/// Below field maintains the view created from <see cref="SfPullToRefresh.RefreshingViewTemplate"/>.
		/// </summary>
		View? _refreshingTemplateView;

		/// <summary>
		/// Indicates whether the child is IPullToRefresh or not.
		/// </summary>
		bool _isIPullToRefresh = false;

		/// <summary>
		/// Instance of refresh view.
		/// </summary>
		SfProgressCircleView _progressCircleView;

		/// <summary>
		/// Indicates whether the pulling action is in progress.
		/// </summary>
		bool _isPulling;

		/// <summary>
		/// Indicates whether the refreshing is in progress.
		/// </summary>
		bool _actualIsRefreshing;

		/// <summary>
		/// Indicates the touch X coordinate.
		/// </summary>
		double _downX = 0;

		/// <summary>
		/// Indicates the touch Y coordinate.
		/// </summary>
		double _downY = 0;

		/// <summary>
		/// The distance that has been pulled during a user's interaction, typically used for tracking the pull-to-refresh progress.
		/// </summary>
		double _distancePulled = 0.0d;

		/// <summary>
		/// Indicates the distance between the <see cref="SfProgressCircleView"/> and <see cref="PullableContent"/> in push mode.
		/// </summary>
		const double _pullableContentMargin = 8;

		/// <summary>
		/// Indicates whether a touch is currently being pressed on the control.
		/// </summary>
		/// <remarks>
		/// Since we are receiving move action for mouse movement across our control in WinUI and Mac,
		/// we maintain this field to check whether the control is pressed and moved to detect whether it has been pulled.
		/// </remarks>
		bool _isPressed;

		/// <summary>
		/// Indicates whether <see cref="ProgressCircleView"/> is currently rotating.
		/// </summary>
		bool _isCircleRotating = false;

		/// <summary>
		/// Indicates the previous arrange bounds of the <see cref="SfPullToRefresh"/>.
		/// </summary>
		Rect _previousBounds;

		/// <summary>
		/// Indicates the previous measured size of the <see cref="SfPullToRefresh"/>.
		/// </summary>
		Size _previousMeasuredSize;

		/// <summary>
		/// Represents the number of attempts to loops through to retrieve the specified child of <see cref="SfPullToRefresh.PullableContent"/> based on touch points.
		/// </summary>
		int _childLoopCount = 0;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="RefreshCommand"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RefreshCommandProperty"/> property determines the command that will be executed when a refresh is triggered.
		/// </remarks>
		public static readonly BindableProperty RefreshCommandProperty =
			BindableProperty.Create(
				nameof(RefreshCommand),
				typeof(ICommand),
				typeof(SfPullToRefresh),
				null,
				BindingMode.Default,
				null);

		/// <summary>
		/// Identifies the <see cref="RefreshCommandParameter"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RefreshCommandParameterProperty"/> property determines the parameter that will be passed to the <see cref="RefreshCommand"/> when it is executed.
		/// </remarks>
		public static readonly BindableProperty RefreshCommandParameterProperty =
		   BindableProperty.Create(
			   nameof(RefreshCommandParameter),
			   typeof(object),
			   typeof(SfPullToRefresh),
			   null,
			   BindingMode.Default,
			   null);

		/// <summary>
		/// Identifies the <see cref="PullingThreshold"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PullingThresholdProperty"/> property determines the distance that needs to be pulled before the refresh action is triggered.
		/// </remarks>
		public static readonly BindableProperty PullingThresholdProperty =
			BindableProperty.Create(
				nameof(PullingThreshold),
				typeof(double),
				typeof(SfPullToRefresh),
				200d,
				BindingMode.TwoWay,
				null);

		/// <summary>
		/// Identifies the <see cref="CanRestrictChildTouch"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CanRestrictChildTouchProperty"/> property determines whether touch interactions on child elements should be restricted when the pull-to-refresh action is in progress.
		/// </remarks>
		public static readonly BindableProperty CanRestrictChildTouchProperty =
			BindableProperty.Create(
				nameof(CanRestrictChildTouch),
				typeof(bool),
				typeof(SfPullToRefresh),
				false,
				BindingMode.OneWay,
				null,
				propertyChanged: OnCanRestrictChildTouchChanged);

		/// <summary>
		/// Identifies the <see cref="PullableContent"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PullableContentProperty"/> property determines the content that can be pulled to trigger the refresh action.
		/// </remarks>
		public static readonly BindableProperty PullableContentProperty =
			BindableProperty.Create(
				nameof(PullableContent),
				typeof(View),
				typeof(SfPullToRefresh),
				null,
				BindingMode.TwoWay,
				null,
				propertyChanged: OnPullableContentChanged);

		/// <summary>
		/// Identifies the <see cref="TransitionMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="TransitionModeProperty"/> property determines the type of transition animation that occurs when the pull-to-refresh action is triggered.
		/// </remarks>
		public static readonly BindableProperty TransitionModeProperty =
			BindableProperty.Create(
				nameof(TransitionMode),
				typeof(PullToRefreshTransitionType),
				typeof(SfPullToRefresh),
				PullToRefreshTransitionType.SlideOnTop,
				BindingMode.TwoWay);

		/// <summary>
		/// Identifies the <see cref="ProgressBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ProgressBackgroundProperty"/> property determines the background brush of the progress circle displayed during the pull-to-refresh action.
		/// </remarks>
		public static readonly BindableProperty ProgressBackgroundProperty =
			BindableProperty.Create(
				nameof(ProgressBackground),
				typeof(Brush),
				typeof(SfPullToRefresh),
				new SolidColorBrush(Color.FromArgb("#F3EDF7")),
				BindingMode.TwoWay,
				null,
				propertyChanged: OnProgressBackgroundChanged);

		/// <summary>
		/// Identifies the <see cref="RefreshViewThreshold"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RefreshViewThresholdProperty"/> property determines the threshold distance that needs to be pulled before the refresh action is triggered.
		/// </remarks>
		public static readonly BindableProperty RefreshViewThresholdProperty =
			BindableProperty.Create(
				nameof(RefreshViewThreshold),
				typeof(double),
				typeof(SfPullToRefresh),
				50d,
				BindingMode.TwoWay,
				null);

		/// <summary>
		/// Identifies the <see cref="RefreshViewHeight"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RefreshViewHeightProperty"/> property determines the height of the refresh view that is displayed during the pull-to-refresh action.
		/// </remarks>
		public static readonly BindableProperty RefreshViewHeightProperty =
			BindableProperty.Create(
				nameof(RefreshViewHeight),
				typeof(double),
				typeof(SfPullToRefresh),
				48d,
				BindingMode.TwoWay,
				null,
				propertyChanged: OnRefreshViewHeightChanged);

		/// <summary>
		/// Identifies the <see cref="RefreshViewWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RefreshViewWidthProperty"/> property determines the width of the refresh view that is displayed during the pull-to-refresh action.
		/// </remarks>
		public static readonly BindableProperty RefreshViewWidthProperty =
			BindableProperty.Create(
				nameof(RefreshViewWidth),
				typeof(double),
				typeof(SfPullToRefresh),
				48d,
				BindingMode.TwoWay,
				null,
				propertyChanged: OnRefreshViewWidthChanged);

		/// <summary>
		/// Identifies the <see cref="RefreshingViewTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RefreshingViewTemplateProperty"/> property determines the template for the view that is displayed while the pull-to-refresh action is in progress. 
		/// </remarks>
		public static readonly BindableProperty RefreshingViewTemplateProperty =
			BindableProperty.Create(
				nameof(RefreshingViewTemplate),
				typeof(DataTemplate),
				typeof(SfPullToRefresh),
				null,
				BindingMode.TwoWay,
				propertyChanged: OnRefreshingViewTemplatePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PullingViewTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PullingViewTemplateProperty"/> property determines the template for the view that is displayed while the user is pulling to refresh.
		/// </remarks>
		public static readonly BindableProperty PullingViewTemplateProperty =
			BindableProperty.Create(
				nameof(PullingViewTemplate),
				typeof(DataTemplate),
				typeof(SfPullToRefresh),
				null,
				BindingMode.TwoWay,
				propertyChanged: OnPullingViewTemplatePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsRefreshing"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsRefreshingProperty"/> property determines whether the pull-to-refresh action is currently active.
		/// </remarks>
		public static readonly BindableProperty IsRefreshingProperty =
			BindableProperty.Create(
				nameof(IsRefreshing),
				typeof(bool),
				typeof(SfPullToRefresh),
				false,
				BindingMode.TwoWay,
				null,
				propertyChanged: OnIsRefreshingChanged);

		/// <summary>
		/// Identifies the <see cref="ProgressThickness"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ProgressThicknessProperty"/> property determines the thickness of the progress indicator displayed during the pull-to-refresh action.
		/// </remarks>
		public static readonly BindableProperty ProgressThicknessProperty =
			BindableProperty.Create(
				nameof(ProgressThickness),
				typeof(double),
				typeof(SfPullToRefresh),
				SfPullToRefresh.GetDefaultProgressThickness(),
				BindingMode.TwoWay,
				null,
				propertyChanged: OnProgressThicknessChanged);

		/// <summary>
		/// Identifies the <see cref="ProgressColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ProgressColorProperty"/> property determines the color of the progress indicator displayed during the pull-to-refresh action.
		/// </remarks>
		public static readonly BindableProperty ProgressColorProperty =
			BindableProperty.Create(
				nameof(ProgressColor),
				typeof(Color),
				typeof(SfPullToRefresh),
				Color.FromArgb("6750A4"),
				BindingMode.TwoWay,
				null,
				propertyChanged: OnProgressColorChanged);

		/// <summary>
		/// Identifies the <see cref="HasShadow"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HasShadowProperty"/> property determines whether the progress indicator displayed during the pull-to-refresh action should have a shadow.
		/// </remarks>
		internal static readonly BindableProperty HasShadowProperty =
		BindableProperty.Create(
			nameof(HasShadow),
			typeof(bool),
			typeof(SfPullToRefresh),
			true,
			BindingMode.TwoWay,
			null);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfPullToRefresh"/> class.
		/// </summary>
		public SfPullToRefresh()
		{
			_progressCircleView = new SfProgressCircleView(this);
			Children.Add(_progressCircleView);
			ClipToBounds = true;
			ThemeElement.InitializeThemeResources(this, "SfPullToRefreshTheme");
		
			// Ensures the refreshing animation starts if IsRefreshing was set via global styles.
			if (this.IsRefreshing && !this.ActualIsRefreshing)
			{
				this.StartRefreshing();
			}

			this.IsLayoutControl = true;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the maximum pulling Y position of the <see cref="ProgressCircleView"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="double"/>. The default value is 200d.
		/// </value>
		/// <remarks>
		/// This property determines how far the user can pull down before the refresh action is triggered.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="PullingThreshold"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh PullingThreshold="250"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     PullingThreshold = 250
		/// };
		/// ]]></code>
		/// </example>
		public double PullingThreshold
		{
			get { return (double)GetValue(PullingThresholdProperty); }
			set { SetValue(PullingThresholdProperty, value); }
		}

		/// <summary>
		/// Gets or sets the transition mode of <see cref="SfPullToRefresh"/>. 
		/// </summary>
		/// <value>
		/// It accepts <see cref="PullToRefreshTransitionType"/>. The default is <see cref="PullToRefreshTransitionType.SlideOnTop"/>.
		/// </value>
		/// <remarks>
		/// In <see cref="PullToRefreshTransitionType.SlideOnTop"/> mode, the progress circle view will be layout over the <see cref="SfPullToRefresh.PullableContent"/> based on pulling progress.
		/// In <see cref="PullToRefreshTransitionType.Push"/> mode, the <see cref="SfPullToRefresh.PullableContent"/> will layout below the progress circle view. Both the circle view and <see cref="SfPullToRefresh.PullableContent"/> will be moved based on pulling progress.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="TransitionMode"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh TransitionMode="Push"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     TransitionMode = PullToRefreshTransitionType.Push
		/// };
		/// ]]></code>
		/// </example>
		public PullToRefreshTransitionType TransitionMode
		{
			get { return (PullToRefreshTransitionType)GetValue(TransitionModeProperty); }
			set { SetValue(TransitionModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the <see cref="ProgressCircleView"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Color"/>. The default color is Color.FromArgb("6750A4").
		/// </value>
		/// <remarks>
		/// This property allows you to customize the color of the progress circle view displayed during the pull-to-refresh action.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="ProgressColor"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh ProgressColor="Red"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     ProgressColor = Colors.Red
		/// };
		/// ]]></code>
		/// </example>
		public Color ProgressColor
		{
			get { return (Color)GetValue(ProgressColorProperty); }
			set { SetValue(ProgressColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background of the <see cref="ProgressCircleView"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Brush"/>. The default background is Color.FromArgb("#F3EDF7").
		/// </value>
		/// <remarks>
		/// This property allows you to customize the background appearance of the progress circle view.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="ProgressBackground"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh ProgressBackground="LightGray"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     ProgressBackground = new SolidColorBrush(Colors.LightGray)
		/// };
		/// ]]></code>
		/// </example>
		public Brush ProgressBackground
		{
			get { return (Brush)GetValue(ProgressBackgroundProperty); }
			set { SetValue(ProgressBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the thickness of the <see cref="ProgressCircleView"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="double"/>. The default value is 3d.
		/// </value>
		/// <remarks>
		/// This property allows you to customize the thickness of the progress circle view.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="ProgressThickness"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh ProgressThickness="5"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///    ProgressThickness = 5
		/// };
		/// ]]></code>
		/// </example>
		public double ProgressThickness
		{
			get { return (double)GetValue(ProgressThicknessProperty); }
			set { SetValue(ProgressThicknessProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="SfPullToRefresh"/> is in refreshing state.
		/// </summary>
		/// <value>
		/// It accepts <see cref="bool"/>. The default value is false.
		/// </value>
		/// <remarks>
		/// This property allows you to programmatically start or stop the refreshing state of the <see cref="SfPullToRefresh"/> control.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="IsRefreshing"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh IsRefreshing="True"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     IsRefreshing = true
		/// };
		/// ]]></code>
		/// </example>
		public bool IsRefreshing
		{
			get { return (bool)GetValue(IsRefreshingProperty); }
			set { SetValue(IsRefreshingProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="PullableContent"/> touch interactions should be allowed or not.
		/// </summary>
		/// <value>
		/// It accepts <see cref="bool"/>. The default value is false.
		/// </value>
		/// <remarks>
		/// This property allows you to control whether the touch interactions with the <see cref="PullableContent"/> should be restricted. 
		/// When set to true, touch interactions with the <see cref="PullableContent"/> will be restricted.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="CanRestrictChildTouch"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh CanRestrictChildTouch="true"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     CanRestrictChildTouch = true
		/// };
		/// ]]></code>
		/// </example>
		public bool CanRestrictChildTouch
		{
			get { return (bool)GetValue(CanRestrictChildTouchProperty); }
			set { SetValue(CanRestrictChildTouchProperty, value); }
		}

		/// <summary>
		/// Gets or sets the pullable content of <see cref="SfPullToRefresh"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="View"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// The <see cref="PullableContent"/> property allows you to specify the content that users can pull down to initiate a refresh action. 
		/// This content is usually a view such as a <see cref="Label"/>, <see cref="ListView"/>, or any other <see cref="View"/>.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="PullableContent"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh x:Name="pullToRefresh"
		///                        PullingThreshold="120"
		///                        RefreshViewHeight="30"
		///                        RefreshViewThreshold="30"
		///                        RefreshViewWidth="30">
		///     <local:SfPullToRefresh.PullableContent>
		///             <Label x:Name="month"
		///                    TextColor="White"
		///                    HorizontalTextAlignment="Center"
		///                    VerticalTextAlignment="Start" />
		///     </local:SfPullToRefresh.PullableContent>
		/// </local:SfPullToRefresh>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///      PullingThreshold = 120,
		///      RefreshViewHeight = 30,
		///      RefreshViewThreshold = 30,
		///      RefreshViewWidth = 30,
		///      PullableContent = new Label
		///      {
		///          TextColor = Colors.White,
		///          HorizontalTextAlignment = TextAlignment.Center,
		///          VerticalTextAlignment = TextAlignment.Start
		///      }
		/// };
		/// ]]></code>
		/// </example>
		public View PullableContent
		{
			get { return (View)GetValue(PullableContentProperty); }
			set { SetValue(PullableContentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the starting position of the progress circle view in <see cref="PullToRefreshTransitionType.SlideOnTop"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="double"/>. The default value is 50d.
		/// </value>
		/// <remarks>
		/// This property is used to set the threshold at which the refresh view starts appearing when the user pulls down to refresh.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="RefreshViewThreshold"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh RefreshViewThreshold="100"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///      RefreshViewThreshold = 100
		/// };
		/// ]]></code>
		/// </example>
		public double RefreshViewThreshold
		{
			get { return (double)GetValue(RefreshViewThresholdProperty); }
			set { SetValue(RefreshViewThresholdProperty, value); }
		}

		/// <summary>
		/// Gets or sets the height of the <see cref="ProgressCircleView"/>. 
		/// </summary>
		/// <value>
		/// It accepts <see cref="double"/>. The default value is 48d.
		/// </value>
		/// <remarks>
		/// This property is used to set the height of the refresh view that appears when the user pulls down to refresh.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="RefreshViewHeight"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh RefreshViewHeight="60"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///      RefreshViewHeight = 60
		/// };
		/// ]]></code>
		/// </example>
		public double RefreshViewHeight
		{
			get { return (double)GetValue(RefreshViewHeightProperty); }
			set { SetValue(RefreshViewHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets the width of the <see cref="ProgressCircleView"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="double"/>. The default value is 48d.
		/// </value>
		/// <remarks>
		/// This property is used to set the width of the refresh view that appears when the user pulls down to refresh.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="RefreshViewWidth"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh RefreshViewWidth="60"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     RefreshViewWidth = 60
		/// };
		/// ]]></code>
		/// </example>
		public double RefreshViewWidth
		{
			get { return (double)GetValue(RefreshViewWidthProperty); }
			set { SetValue(RefreshViewWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the template to be displayed as the refresh content when <see cref="Pulling"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DataTemplate"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// This property allows you to customize the appearance of the refresh view when the user is pulling down to refresh.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="PullingViewTemplate"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh>
		///      <local:SfPullToRefresh.PullingViewTemplate>
		///          <DataTemplate>
		///              <StackLayout>
		///                  <Label Text="Pull to refresh..." />
		///              </StackLayout>
		///          </DataTemplate>
		///      </local:SfPullToRefresh.PullingViewTemplate>
		/// </local:SfPullToRefresh>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///     PullingViewTemplate = new DataTemplate(() =>
		///     {
		///        var stackLayout = new StackLayout();
		///        var label = new Label { Text = "Pull to refresh..." };
		///        stackLayout.Children.Add(label);
		///        stackLayout.Children.Add(activityIndicator);
		///        return stackLayout;
		///     })
		/// };
		/// ]]></code>
		/// </example>
		public DataTemplate? PullingViewTemplate
		{
			get { return (DataTemplate)GetValue(PullingViewTemplateProperty); }
			set { SetValue(PullingViewTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the template to be displayed as the refresh content on <see cref="Refreshing"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DataTemplate"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// This property allows you to customize the appearance of the refresh view when the user is refreshing.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="RefreshingViewTemplate"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh>
		///     <local:SfPullToRefresh.RefreshingViewTemplate>
		///         <DataTemplate>
		///             <StackLayout>
		///                 <Label Text="Refreshing..." />
		///                 <ActivityIndicator IsRunning="True" />
		///             </StackLayout>
		///         </DataTemplate>
		///     </local:SfPullToRefresh.RefreshingViewTemplate>
		/// </local:SfPullToRefresh>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh
		/// {
		///      RefreshingViewTemplate = new DataTemplate(() =>
		///      {
		///           var stackLayout = new StackLayout();
		///           var label = new Label { Text = "Refreshing..." };
		///           var activityIndicator = new ActivityIndicator { IsRunning = true };
		///           stackLayout.Children.Add(label);
		///           stackLayout.Children.Add(activityIndicator);
		///           return stackLayout;
		///      })
		/// };
		/// ]]></code>
		/// </example>
		public DataTemplate? RefreshingViewTemplate
		{
			get { return (DataTemplate)GetValue(RefreshingViewTemplateProperty); }
			set { SetValue(RefreshingViewTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the refresh command for <see cref="SfPullToRefresh"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="ICommand"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// The command's <c>CanExecute()</c> method will be triggered when the pulling action is performed.
		/// If <c>false</c> is returned from <c>CanExecute()</c>, the pulling will be canceled, and the command will not be executed, and refreshing does not happen.
		/// If <c>true</c> is returned from <c>CanExecute()</c>, the command will be executed on refreshing.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="RefreshCommand"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfPullToRefresh RefreshCommand="{Binding ViewRefreshCommand}"
		///                            RefreshCommandParameter="{Binding .}">
		/// </local:SfPullToRefresh>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// public MainPage()
		/// {
		///     InitializeComponent();
		/// 
		///     SfPullToRefresh pullToRefresh = new SfPullToRefresh
		///     {
		///         RefreshCommand = new Command<object>(RefreshMethod, CanExecuteRefreshMethod),
		///         RefreshCommandParameter = this
		///     };
		/// 
		///     // Add your content to pullToRefresh
		/// }
		/// 
		/// private bool CanExecuteRefreshMethod(object obj)
		/// {
		///     return true;
		/// }
		/// 
		/// private async void RefreshMethod(object parameter)
		/// {
		///     var pullToRefresh = parameter as SfPullToRefresh;
		///     if (pullToRefresh is not null)
		///     {
		///          pullToRefresh.IsRefreshing = true;
		///          await Task.Delay(1200); // Simulate a refresh
		///          // Refresh your data here
		///          pullToRefresh.IsRefreshing = false;
		///     }
		/// }
		/// ]]></code>
		/// </example>
		public ICommand RefreshCommand
		{
			get
			{
				return (ICommand)GetValue(RefreshCommandProperty);
			}

			set
			{
				SetValue(RefreshCommandProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the parameter of the <see cref="SfPullToRefresh.RefreshCommand"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="object"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// This property allows you to pass additional data to the command when it is executed.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="RefreshCommandParameter"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <ContentPage>
		///    <local:SfPullToRefresh RefreshCommand="{Binding ViewRefreshCommand}"
		///                            RefreshCommandParameter="{Binding .}">
		///    </local:SfPullToRefresh>
		/// </ContentPage>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// public MainPage()
		/// {
		///     InitializeComponent();
		/// 
		///     SfPullToRefresh pullToRefresh = new SfPullToRefresh
		///     {
		///          RefreshCommand = new Command<object>(RefreshMethod),
		///          RefreshCommandParameter = this
		///     };
		/// 
		/// }
		/// 
		/// private async void RefreshMethod(object parameter)
		/// {
		///     var pullToRefresh = parameter as SfPullToRefresh;
		///     if (pullToRefresh is not null)
		///     {
		///         pullToRefresh.IsRefreshing = true;
		///         await Task.Delay(1200); // Simulate a refresh
		///         pullToRefresh.IsRefreshing = false;
		///     }
		/// }
		/// ]]></code>
		/// </example>
		public object RefreshCommandParameter
		{
			get
			{
				return (object)GetValue(RefreshCommandParameterProperty);
			}

			set
			{
				SetValue(RefreshCommandParameterProperty, value);
			}
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="SfPullToRefresh.ProgressCircleView"/> has shadow or not.
		/// </summary>
		internal bool HasShadow
		{
			get { return (bool)GetValue(HasShadowProperty); }
			set { SetValue(HasShadowProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the pullable content is in pulling are not.
		/// </summary>
		internal bool IsPulling
		{
			get
			{
				return _isPulling;
			}

			set
			{
				if (value != _isPulling)
				{
					_isPulling = value;
					if (value)
					{
						ProgressCircleView.CheckIfAndSetTemplate();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="SfPullToRefresh"/> is in refreshing state.
		/// </summary>
		internal bool ActualIsRefreshing
		{
			get
			{
				return _actualIsRefreshing;
			}

			set
			{
				if (_actualIsRefreshing != value)
				{
					_actualIsRefreshing = value;
					if (!_actualIsRefreshing)
					{
						// When removing children, the content is not automatically set to null, so we explicitly set it to null. 
						ProgressCircleView.Content = null!;
						RaiseRefreshedEvent(this);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets SfProgressCircleView of <see cref="SfPullToRefresh"/>.
		/// </summary>
		internal SfProgressCircleView ProgressCircleView
		{
			get
			{
				return _progressCircleView;
			}

			set
			{
				_progressCircleView = value;
				OnPropertyChanged("ProgressCircleView");
			}
		}

		/// <summary>
		/// Gets the width of <see cref="ProgressCircleView"/> with shadow size consideration.
		/// </summary>
		internal double CircleViewWidth
		{
			get
			{
				return RefreshViewWidth + (GetShadowSpace(false) * 2);
			}
		}

		/// <summary>
		/// Gets the height of the <see cref="ProgressCircleView"/> with shadow size consideration.
		/// </summary>
		internal double CircleViewHeight
		{
			get
			{
				return RefreshViewHeight + GetShadowSpace(true) + GetShadowSpace(false);
			}
		}

		internal double ProgressRate
		{
			get
			{
				return _progressRate;
			}
			set
			{
				_progressRate = value;
			}
		}

		internal View? PullingTemplateView
		{
			get
			{
				return _pullingTemplateView;
			}
			set
			{
				_pullingTemplateView = value;
			}
		}

		internal View? RefreshingTemplateView
		{
			get
			{
				return _refreshingTemplateView;
			}
			set
			{
				_refreshingTemplateView = value;
			}
		}

		internal bool IsIPullToRefresh
		{
			get
			{
				return _isIPullToRefresh;
			}
			set
			{
				_isIPullToRefresh = value;
			}
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// Starts refreshing the <see cref="SfPullToRefresh.PullableContent"/> and displays the refreshing animation.
		/// </summary>
		/// <remarks>
		/// This method initiates the refreshing process and displays the refreshing animation. If the control is currently in the pulling state, it will cancel the pulling state before starting the refresh.
		/// </remarks>
		/// <example>
		/// The following C# code demonstrates how to use the <see cref="StartRefreshing"/> method:
		/// <code>
		/// <![CDATA[
		/// public partial class MainPage : ContentPage
		/// {
		///     public MainPage()
		///     {
		///         InitializeComponent();
		///     }
		///
		///     private void OnRefreshButtonClicked(object sender, EventArgs e)
		///     {
		///         pullToRefresh.StartRefreshing();
		///     }
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public void StartRefreshing()
		{
			if (IsPulling)
			{
				RaisePullingCancelled();
			}

			if (!ActualIsRefreshing)
			{
				BeginRefreshing(true);
			}
		}

		/// <summary>
		/// Ends refreshing the <see cref="SfPullToRefresh.PullableContent"/> and stops the refreshing animation.
		/// </summary>
		/// <remarks>
		/// This method stops the refreshing animation and sets the internal state to indicate that the refresh has ended.
		/// </remarks>
		/// <example>
		/// The following C# code demonstrates how to use the <see cref="EndRefreshing"/> method:
		/// <code>
		///  <![CDATA[
		/// public partial class MainPage : ContentPage
		/// {
		///     public MainPage()
		///     {
		///         InitializeComponent();
		///     }
		///
		///     private void OnRefreshCompleted(object sender, EventArgs e)
		///     {
		///         pullToRefresh.EndRefreshing();
		///     }
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public void EndRefreshing()
		{
			ActualIsRefreshing = false;
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Methods returns the shadow blur radius based on side.
		/// </summary>
		/// <param name="isTop">Indicates whether the top spacing is needed.</param>
		/// <returns>Returns the shadow blur radius.</returns>
		internal double GetShadowSpace(bool isTop)
		{
			const double TopShadowSpace = 1.0;
			const double BottomShadowSpace = 3.0;
			const double NoShadowSpace = 0.0;

			if (HasShadow)
			{
				return isTop ? TopShadowSpace : BottomShadowSpace;
			}
			else
			{
				return NoShadowSpace;
			}
		}

		/// <summary>
		/// Manually arranges the <see cref="PullableContent"/> and <see cref="ProgressCircleView"/>.
		/// </summary>
		/// <param name="skipPullableContent">Indicates whether the <see cref="PullableContent"/> should be arranged or not.</param>
		/// <param name="bounds">Bounds of the <see cref="SfPullToRefresh"/>.</param>
		internal void ManualArrangeContent(bool skipPullableContent, Rect bounds)
		{
			ArrangeSfProgressCircleView();
			if (!skipPullableContent)
			{
				ArrangePullableContent(bounds);
			}

			_previousBounds.Width = bounds.Width;
			_previousBounds.Height = bounds.Height;
		}

		/// <summary>
		/// Measures the <see cref="SfPullToRefresh.ProgressCircleView"/>.
		/// </summary>
		/// <param name="widthConstraint">The available width for <see cref="SfPullToRefresh"/>.</param>
		/// <param name="heightConstraint">The available height for <see cref="SfPullToRefresh"/>.</param>
		internal void MeasureSfProgressCircleView(double widthConstraint = 0, double heightConstraint = 0)
		{
			if (ProgressCircleView.Content is not null)
			{
				(ProgressCircleView as IView).Measure(widthConstraint, heightConstraint);
			}
			else
			{
				(ProgressCircleView as IView).Measure(CircleViewWidth, CircleViewHeight);
			}
		}

		/// <summary>
		/// Gets the bounds based on framework assigned height based on safe area region.
		/// </summary>
		/// <returns>Return the rect based on height passed in arrange pass to pull to refresh control.</returns>
		internal Rect GetBounds()
		{
			Rect bounds = Bounds;
#if IOS && !MACCATALYST
			bounds.Height = _previousBounds.Height;
#endif

			return bounds;
		}

		/// <summary>
		/// Gets the specific child view at the given touch point.
		/// </summary>
		/// <param name="element">The child view to be retrieved.</param>
		/// <param name="touchPoint">The current touch point coordinates.</param>
		/// <returns>The child view located at the specified touch point.</returns>
		internal bool IsChildElementScrolled(IVisualTreeElement? element, Point touchPoint)
		{
			bool interceptResult = false;
			if (element is null)
			{
				return false;
			}

			var view = element as View;
			if (view is null || view.Handler is null || view.Handler.PlatformView is null)
			{
				return false;
			}

			// Checks for IPullToRefresh.
			PullToRefreshHelpers.CheckChildren(view as IView, this, out _isIPullToRefresh, out interceptResult);
			if (_isIPullToRefresh)
			{
				return interceptResult;
			}
			else
			{
				// If the current child offset is greater than 0, we return here and no need to loop their innerChild.
				if (GetChildScrollOffset(view.Handler.PlatformView) > 0)
				{
					return true;
				}

				// This condition is when IContentView content is not null and child count is 0
				if (view is IContentView contentView && element.GetVisualChildren().Count == 0 && contentView.Content is not null && contentView.Content is IVisualTreeElement contentElement)
				{
					element = contentElement;
				}

				foreach (var childView in element.GetVisualChildren().OfType<View>())
				{
					if (childView is null || childView.Handler is null || childView.Handler.PlatformView is null)
					{
						return false;
					}

					var childNativeView = childView.Handler.PlatformView;

					// Here items X and Y position converts based on screen.
					Point locationOnScreen = ChildLocationToScreen(childNativeView);
					var bottom = locationOnScreen.Y + childView.Bounds.Height;
					var right = locationOnScreen.X + childView.Bounds.Width;

					// We loop through child only 10 times.
					if (touchPoint.Y >= locationOnScreen.Y && touchPoint.Y <= bottom && touchPoint.X >= locationOnScreen.X && touchPoint.X <= right && _childLoopCount <= 10)
					{
						_childLoopCount++;
						return IsChildElementScrolled(childView, touchPoint);
					}
				}
			}

			_childLoopCount = 0;
			return false;
		}

		/// <summary>
		/// Calling this method cancels the pulling and refreshing process.
		/// </summary>
		/// <returns>true indicating that the pulling action has been cancelled.</returns>
		internal bool CancelPulling()
		{
			RaisePullingCancelled();
			return true;
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the default value for <see cref="SfPullToRefresh.ProgressThickness"/> based on platform.
		/// </summary>
		/// <returns>Returns the default value for <see cref="SfPullToRefresh.ProgressThickness"/> based on platform.</returns>
		static double GetDefaultProgressThickness()
		{
			return 3d;
		}

		double CalculateYPosition()
		{
			double y = 0;
#if IOS && !MACCATALYST
			y = _previousBounds.Y;
#endif
			if (IsPulling)
			{
				y += _distancePulled;
				if (TransitionMode == PullToRefreshTransitionType.Push)
				{
					y -= ProgressCircleView.Content is null ? RefreshViewHeight + _pullableContentMargin : ProgressCircleView.CircleViewBounds.Height + _pullableContentMargin;
				}
			}
			else
			{
				y += RefreshViewThreshold;
			}

			return y;
		}

		void ApplyPlatformSpecificClipping(double y)
		{
#if WINDOWS
			if (TransitionMode == PullToRefreshTransitionType.Push)
			{
				ProgressCircleView.Clip = new RectangleGeometry(new Rect(0, y < 0 ? -y : 0, ProgressCircleView.CircleViewBounds.Width, ProgressCircleView.CircleViewBounds.Height));
			}
			else if (ProgressCircleView.Clip is not null)
			{
				ProgressCircleView.Clip = new RectangleGeometry(new Rect(0, 0, ProgressCircleView.CircleViewBounds.Width, ProgressCircleView.CircleViewBounds.Height));
			}
#endif
		}

		/// <summary>
		/// This method arranges the <see cref="SfPullToRefresh._progressCircleView"/> based on <see cref="SfPullToRefresh.TransitionMode"/> and current state of pulling operation.
		/// </summary>
		void ArrangeSfProgressCircleView()
		{
			if (ProgressCircleView is not null)
			{
				if (!IsPulling && !ActualIsRefreshing)
				{
					HideSfProgressCircleView();
				}
				else
				{
					if (!ProgressCircleView.ProcessedBounds.Equals(Bounds))
					{
						ProgressCircleView.UpdateCircleViewBounds();
					}

					double y = CalculateYPosition();

					if (ProgressCircleView.Content is null)
					{
						y -= GetShadowSpace(true);
					}

					ApplyPlatformSpecificClipping(y);

					var bounds = ProgressCircleView.CircleViewBounds;
					bounds.Y = y;
					ProgressCircleView.CircleViewBounds = bounds;

					(ProgressCircleView as IView).Arrange(new Rect(ProgressCircleView.CircleViewBounds.X, y, ProgressCircleView.CircleViewBounds.Width, ProgressCircleView.CircleViewBounds.Height));
					if (ProgressCircleView.Content is null && IsPulling)
					{
						ProgressCircleView.InvalidateDrawable();
					}
				}
			}
		}

		/// <summary>
		/// This method arranges the <see cref="SfPullToRefresh.PullableContent"/> based on <see cref="SfPullToRefresh.TransitionMode"/> and current state of pulling operation.
		/// </summary>
		/// <param name="bounds">Bounds of the <see cref="SfPullToRefresh"/>.</param>
		/// <param name="animate">This flag indicates whether to animate the <see cref="SfPullToRefresh.PullableContent"/>.</param>
		void ArrangePullableContent(Rect bounds, bool animate = false)
		{
			if (PullableContent is not null)
			{
				bounds.Y = 0;
#if IOS && !MACCATALYST
				bounds.Y = _previousBounds.Y;
#endif
				bounds.X = 0;
				if (!PullableContent.AnimationIsRunning("PushBackAnimation"))
				{
					if (TransitionMode == PullToRefreshTransitionType.Push)
					{
						if (IsPulling)
						{
							bounds.Y += _distancePulled;
						}
						else if (ActualIsRefreshing)
						{
							double circleViewHeight = 0;
							if (ProgressCircleView.Content is not null && RefreshingTemplateView is not null)
							{
								circleViewHeight = ProgressCircleView.Content.DesiredSize.Height;
							}
							else
							{
								circleViewHeight = RefreshViewHeight;
							}

							bounds.Y += RefreshViewThreshold + circleViewHeight + _pullableContentMargin;
						}
					}

					if (!animate)
					{
						(PullableContent as IView).Arrange(bounds);
					}
					else
					{
						PullableContentLayoutAnimation(PullableContent);
					}
				}
			}
		}

		/// <summary>
		/// Animates the <see cref="SfPullToRefresh.PullableContent"/> Y to 0.
		/// </summary>
		/// <param name="element">Instance of <see cref="PullableContent"/>.</param>
		void PullableContentLayoutAnimation(View element)
		{
			const int rate = 5;
			const int length = 300;
			double endvalue = 0;
#if IOS && !MACCATALYST
			endvalue = _previousBounds.Y;
#endif
			// LayoutTo was not working so added animation to perform Layout animation.
			Animation animation = new Animation(
								callback: d =>
								{
#if IOS && !MACCATALYST

									(element as IView).Arrange(new Rect(0, d, Width, _previousBounds.Height));
#else
#if ANDROID
									(element as IView).Measure(Width, Height);
#endif
									(element as IView).Arrange(new Rect(0, d, Width, Height));
#endif
								},
								start: element.Bounds.Y,
								end: endvalue,
								easing: Easing.CubicIn);
			animation.Commit(element, "PushBackAnimation", rate, length);
		}

		/// <summary>
		/// Methods to check whether the pullable content can be pulled or not.
		/// </summary>
		/// <returns>Return whether to process touch or not.</returns>
		bool CanProcessTouch()
		{
			if ((PullableContent is not null && PullableContent.AnimationIsRunning("PushBackAnimation")) || ActualIsRefreshing)
			{
				return false;
			}

			return true;
		}

		void UpdateProgressRate(double pulledDistance)
		{
			if (TransitionMode == PullToRefreshTransitionType.SlideOnTop)
			{
				ProgressRate = (pulledDistance - RefreshViewThreshold) / (PullingThreshold - RefreshViewThreshold);
			}
			else
			{
				double refreshContentHeight = RefreshViewHeight;

				if (PullingTemplateView is not null)
				{
					refreshContentHeight = PullingTemplateView.DesiredSize.Height;
				}

				refreshContentHeight += _pullableContentMargin;

				ProgressRate = (pulledDistance - refreshContentHeight) / PullingThreshold;
			}
		}

		bool HandlePullingEvent()
		{
			var pullToRefresh = PullToRefreshHelpers.GetIPullToRefreshElement(PullableContent as IView, this);
			if (pullToRefresh is not null)
			{
				var cancel = false;

				if (pullToRefresh is IPullToRefresh pullToRefreshElement)
				{
					pullToRefreshElement.Pulling(ProgressRate, this, out cancel);
				}

				if (pullToRefresh == PullableContent && cancel)
				{
					return CancelPulling();
				}
			}

			if (Pulling is not null)
			{
				PullingEventArgs args = new PullingEventArgs() { Progress = ProgressRate };
				Pulling(this, args);
				if (args.Cancel)
				{
					return CancelPulling();
				}
			}

			return false;
		}

		/// <summary>
		/// Raises the Pulling event.
		/// </summary>
		/// <param name="pulledDistance">Represents the progress of the pulling.</param>
		/// <returns><b>true</b>, if the progress has reached hundred percent.</returns>
		bool RaisePullingEvent(double pulledDistance)
		{
			const double MaxProgressRate = 100;

			if (TransitionMode == PullToRefreshTransitionType.SlideOnTop && pulledDistance < RefreshViewThreshold)
			{
				if (ProgressRate > 0)
				{
					ProgressRate = 0;
				}

				HideSfProgressCircleView();
				return false;
			}

			UpdateProgressRate(pulledDistance);
			ProgressRate = Math.Min(MaxProgressRate, Math.Round(ProgressRate * MaxProgressRate));
			ProgressRate = Math.Max(0, ProgressRate);

			if (HandlePullingEvent())
			{
				return true;
			}

			if (RefreshCommand is not null && !RefreshCommand.CanExecute(RefreshCommandParameter))
			{
				return CancelPulling();
			}

			if (ActualIsRefreshing)
			{
				return true;
			}

			IsPulling = true;

			bool canSkipArrange = TransitionMode == PullToRefreshTransitionType.SlideOnTop;
			ManualArrangeContent(canSkipArrange, GetBounds());
			return false;
		}

		/// <summary>
		/// Raises the refreshing event.
		/// </summary>
		/// <param name="sender">The instance of <see cref="SfPullToRefresh"/>.</param>
		void RaiseRefreshingEvent(object sender)
		{
			if (ProgressRate < 100 || (Refreshing is null && RefreshCommand is null))
			{
				RaisePullingCancelled();
				return;
			}

			BeginRefreshing(false);
		}

		/// <summary>
		/// This methods begins the refreshing operation.
		/// </summary>
		/// <param name="isProgrammatic">Represents refreshing was programmatic or from <see cref="RaiseRefreshingEvent(object)"/>.</param>
		void BeginRefreshing(bool isProgrammatic)
		{
			IsPulling = false;
			ActualIsRefreshing = true;
			ProgressRate = 100;
			if (RefreshingTemplateView is not null && ProgressCircleView is not null)
			{
				ProgressCircleView.CheckIfAndSetTemplate();
			}
			else
			{
				if (ProgressCircleView is not null && ProgressCircleView.Content is not null)
				{
					ProgressCircleView.UpdateContent();
				}
			}

			var pullToRefresh = PullToRefreshHelpers.GetIPullToRefreshElement(PullableContent, this);

			if (pullToRefresh is not null && pullToRefresh is IPullToRefresh pullToRefreshElement)
			{
				pullToRefreshElement.Refreshing(this);
			}

			bool canSkipArrange = TransitionMode == PullToRefreshTransitionType.SlideOnTop;
			ManualArrangeContent(canSkipArrange, Bounds);

			if (ProgressCircleView is not null && ProgressCircleView.Content is null && isProgrammatic)
			{
				Rotate();
				return;
			}

			if (Refreshing is not null)
			{
				Refreshing(this, EventArgs.Empty);
			}

			RefreshCommand?.Execute(RefreshCommandParameter);

			if (ProgressCircleView is not null && ProgressCircleView.Content is null)
			{
				Rotate();
			}
		}

		/// <summary>
		/// Handles the touch interaction of <see cref="SfPullToRefresh"/>.
		/// </summary>
		/// <param name="action">Represent the type of <see cref="PointerActions"/>.</param>
		/// <param name="point">Represents the touch point.</param>
		void HandleTouchInteraction(PointerActions action, Point point)
		{
			if (CanProcessTouch())
			{
				point = new Point(Math.Round(point.X, 2), Math.Round(point.Y, 2));
				if (action == PointerActions.Pressed)
				{
					_isPressed = true;
					_distancePulled = 0;
					_downY = point.Y;
					_downX = point.X;
				}

				if (_isPressed)
				{
					switch (action)
					{
						case PointerActions.Moved:
							_distancePulled = point.Y - _downY;
							if (!ActualIsRefreshing)
							{
								double circleHeight = ProgressCircleView.Content is null ? RefreshViewHeight : ProgressCircleView.Content.DesiredSize.Height;
								double maxPullingThreshold = PullingThreshold + (TransitionMode == PullToRefreshTransitionType.Push ? circleHeight + _pullableContentMargin : 0);
								if (_distancePulled > maxPullingThreshold)
								{
									_distancePulled = maxPullingThreshold;
								}
								else if (_distancePulled < 0)
								{
									_distancePulled = 0;
								}

								RaisePullingEvent(_distancePulled);
							}

							break;
						case PointerActions.Released:
							if (IsPulling)
							{
								RaiseRefreshingEvent(this);
							}

							ResetTouchFields();
							break;
						case PointerActions.Cancelled:
						case PointerActions.Exited:
							RaisePullingCancelled();
							ResetTouchFields();
							break;
					}
				}
			}
		}

		/// <summary>
		/// This methods resets all the fields related to pulling and refreshing operation.
		/// </summary>
		void ResetValues()
		{
			ResetTouchFields();
			HideSfProgressCircleView();
			ProgressRate = 0;
		}

		/// <summary>
		/// This methods resets all the helper fields related to touch.
		/// </summary>
		void ResetTouchFields()
		{
			_distancePulled = 0;
			_isPressed = false;
			_downY = 0;
			_downX = 0;
		}

		/// <summary>
		/// This methods hides the <see cref="SfProgressCircleView"/>.
		/// </summary>
		void HideSfProgressCircleView()
		{
			if (ProgressCircleView is not null)
			{
				(ProgressCircleView as IView).Arrange(new Rect(0, 0, 0, 0));
				ProgressCircleView.ResetArcAngle();
			}
		}

		/// <summary>
		/// Raises the Refreshed event.
		/// </summary>
		/// <param name="sender">Instance of <see cref="SfPullToRefresh"/>.</param>
		void RaiseRefreshedEvent(object sender)
		{
			ResetValues();
			var pullToRefresh = PullToRefreshHelpers.GetIPullToRefreshElement(PullableContent as IView, this);
			if (pullToRefresh is not null && pullToRefresh is IPullToRefresh pullToRefreshElement)
			{
				pullToRefreshElement.Refreshed(this);
			}

			// Included condition for battery saver logic for android platform.
			HideSfProgressCircleView();
			var canAnimate = TransitionMode == PullToRefreshTransitionType.Push;
#if ANDROID
			canAnimate = Battery.Default.EnergySaverStatus is not EnergySaverStatus.On;
#endif
			ArrangePullableContent(GetBounds(), canAnimate);
			(this as IView).InvalidateMeasure();
			if (Refreshed is not null)
			{
				Refreshed(this, EventArgs.Empty);
			}
		}


		/// <summary>
		/// The method cancels the pulling operation.
		/// </summary>
		void RaisePullingCancelled()
		{
			ResetValues();
			var pullToRefresh = PullToRefreshHelpers.GetIPullToRefreshElement(PullableContent as IView, this);
			if (pullToRefresh is not null && pullToRefresh is IPullToRefresh pullToRefreshElement)
			{
				pullToRefreshElement.PullingCancelled(this);
			}

			if (IsPulling)
			{
				IsPulling = false;
				(this as IView).InvalidateMeasure();
			}
		}

		/// <summary>
		/// Rotates the progress animation when refreshing the pullable content.
		/// </summary>
		async void Rotate()
		{
			if (!_isCircleRotating)
			{
				try
				{
					while (ActualIsRefreshing && ProgressCircleView is not null && ProgressCircleView.IsVisible && RefreshingViewTemplate is null)
					{
						_isCircleRotating = true;
						ProgressCircleView.ComputeArcAngle();
						ProgressCircleView.InvalidateDrawable();
						await Task.Delay(15);
					}
				}
				catch
				{
#if WINDOWS
					const int contentLength = 300;
					const int circleViewLength = 250;
					await PullableContent.LayoutTo(new Rect(0, RefreshViewHeight * 2, Width, Height - (RefreshViewHeight * 2)), contentLength).ConfigureAwait(true);
					await ProgressCircleView.LayoutTo(new Rect((Width / 2) - (RefreshViewWidth / 2), RefreshViewHeight / 1.5, RefreshViewWidth, RefreshViewHeight), circleViewLength).ConfigureAwait(true);
					HideSfProgressCircleView();
#endif
				}

				_isCircleRotating = false;
			}
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Raised when handler gets changed.
		/// </summary>
		/// <exclude/>
		protected override void OnHandlerChanged()
		{
			base.OnHandlerChanged();
			ConfigTouch();
		}

		/// <summary>
		/// Measures the <see cref="SfPullToRefresh.PullableContent"/> and <see cref="SfPullToRefresh.ProgressCircleView"/>
		/// </summary>
		/// <param name="widthConstraint">The available width for <see cref="SfPullToRefresh"/>.</param>
		/// <param name="heightConstraint">The available height for <see cref="SfPullToRefresh"/>.</param>
		/// <returns>Returns the required size.</returns>
		/// <exclude/>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			if (!IsPulling || _previousMeasuredSize != new Size(widthConstraint, heightConstraint))
			{
				if (PullableContent is not null)
				{
					(PullableContent as IView).Measure(widthConstraint, heightConstraint);
				}

				double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
				double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
				double screenWidth = 300;
				double screenHeight = 300;
#if !WINDOWS
				screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
				screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
				width = screenWidth;
				height = screenHeight;
#else
                if (width == 0)
                {
                    width = screenWidth;
                }
                if (height == 0)
                {
                    height = screenHeight;
                }
#endif
				MeasureSfProgressCircleView(width, height);
				_previousMeasuredSize = new Size(width, height);
			}

			return _previousMeasuredSize;
		}

		/// <summary>
		/// Methods arranges the <see cref="SfPullToRefresh.PullableContent"/> and <see cref="SfPullToRefresh.ProgressCircleView"/>.
		/// </summary>
		/// <param name="bounds">Bounds of the <see cref="SfPullToRefresh"/>.</param>
		/// <returns>Returns the bounds of the <see cref="SfPullToRefresh"/>.</returns>
		/// <exclude/>
		protected override Size ArrangeContent(Rect bounds)
		{
			if (!IsPulling || !bounds.Equals(_previousBounds))
			{
				ManualArrangeContent(false, bounds);
				_previousBounds = bounds;
			}

			return new Size(bounds.Width, bounds.Height);
		}

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Occurs when the IsRefreshing property is changed.
		/// </summary>
		/// <param name="bindable">The SfPullToRefresh as bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnIsRefreshingChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null)
			{
				if (pullToRefresh.IsRefreshing)
				{
					pullToRefresh.StartRefreshing();
				}
				else
				{
					pullToRefresh.EndRefreshing();
				}
			}
		}

		/// <summary>
		/// Occurs when the CanRestrictChildTouch property is changed.
		/// </summary>
		/// <param name="bindable">The SfPullToRefresh as bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnCanRestrictChildTouchChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;

			// OnCanRestrictChildTouchChanged method triggers before OnPullableContentChanged method. So, we have to check whether the PullableContent is null or not.
			if (pullToRefresh is not null && pullToRefresh.PullableContent is not null)
			{
				// Should not make the pullable content as InputTransparent for WinUI platform as making it will restrict touch listening for pullable content.
#if !WINDOWS
				pullToRefresh.PullableContent.InputTransparent = pullToRefresh.CanRestrictChildTouch;
#endif
			}
		}

		/// <summary>
		/// Occurs when the PullableContent property is changed.
		/// </summary>
		/// <param name="bindable">The SfPullToRefresh as bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnPullableContentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null)
			{
				if (oldValue is View view && pullToRefresh.Contains(oldValue))
				{
					pullToRefresh.Remove(view);
				}

				if (newValue is View pullableView)
				{
					// Changed to insert from add. Because of runtime changes in windows causes the ProgressCircleView arrange behind pullableContent.
					pullToRefresh.Children.Insert(0, pullableView);

					// Added here, because when setting CanRestrictChildTouch true from xaml it will not affect the changes since pullable content was not yet initialized.
					pullToRefresh.PullableContent.InputTransparent = pullToRefresh.CanRestrictChildTouch;
				}

				(pullToRefresh as IView).InvalidateMeasure();
			}
		}

		/// <summary>
		/// Occurs when the <see cref="SfPullToRefresh.PullingViewTemplate"/> gets changed.
		/// </summary>
		/// <param name="bindable">The SfPullToRefresh as bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnPullingViewTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null && oldValue is not null)
			{
				if (pullToRefresh.ProgressCircleView is not null && pullToRefresh.ProgressCircleView.Content is not null && pullToRefresh.PullingTemplateView is not null && pullToRefresh.ProgressCircleView.Content == pullToRefresh.PullingTemplateView)
				{
					pullToRefresh.ProgressCircleView.UpdateContent();
				}

				pullToRefresh.PullingTemplateView = null;
			}

			if (pullToRefresh is not null && newValue is not null && pullToRefresh.ProgressCircleView is not null)
			{
				pullToRefresh.ProgressCircleView.InitializePullingViewTemplate();
			}

			if (pullToRefresh is not null && pullToRefresh.IsPulling && pullToRefresh.ProgressCircleView is not null)
			{
				if (newValue is not null)
				{
					pullToRefresh.ProgressCircleView.CheckIfAndSetTemplate();
				}
				else
				{
					pullToRefresh.ProgressCircleView.UpdateCircleViewBounds();
					pullToRefresh.MeasureSfProgressCircleView(pullToRefresh.Bounds.Width, pullToRefresh.Bounds.Height);
				}
			}
		}

		/// <summary>
		/// Occurs when the <see cref="SfPullToRefresh.RefreshingViewTemplate"/> gets changed.
		/// </summary>
		/// <param name="bindable">The SfPullToRefresh as bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnRefreshingViewTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null && pullToRefresh.ProgressCircleView is not null)
			{
				if (oldValue is not null)
				{
					if (pullToRefresh.ProgressCircleView.Content is not null && pullToRefresh.RefreshingTemplateView is not null && pullToRefresh.ProgressCircleView.Content == pullToRefresh.RefreshingTemplateView)
					{
						pullToRefresh.ProgressCircleView.UpdateContent();
					}

					pullToRefresh.RefreshingTemplateView = null;
				}

				if (newValue is not null)
				{
					pullToRefresh.ProgressCircleView.InitializeRefreshViewViewTemplate();
				}

				if (pullToRefresh.ActualIsRefreshing)
				{
					if (newValue is not null)
					{
						pullToRefresh.ProgressCircleView.CheckIfAndSetTemplate();
					}
					else
					{
						pullToRefresh.ProgressCircleView.UpdateCircleViewBounds();
						pullToRefresh.MeasureSfProgressCircleView(pullToRefresh.Bounds.Width, pullToRefresh.Bounds.Height);
						pullToRefresh.Rotate();
					}
				}
			}
		}

		/// <summary>
		/// Occurs when the RefreshViewHeight property is changed.
		/// </summary>
		/// <param name="bindable">The <see cref="SfPullToRefresh"/> as <see cref="BindableObject"/>.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new Value.</param>
		static void OnRefreshViewHeightChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null && pullToRefresh.ProgressCircleView is not null)
			{
				pullToRefresh.ProgressCircleView.UpdateDrawProperties();
				if ((pullToRefresh.IsPulling || pullToRefresh.ActualIsRefreshing) && pullToRefresh.ProgressCircleView.Content is null)
				{
					pullToRefresh.MeasureSfProgressCircleView(pullToRefresh.Bounds.Width, pullToRefresh.Bounds.Height);
					pullToRefresh.ProgressCircleView.UpdateCircleViewBounds();
					pullToRefresh.ManualArrangeContent(pullToRefresh.TransitionMode == PullToRefreshTransitionType.SlideOnTop, pullToRefresh.GetBounds());
				}
			}
		}

		/// <summary>
		/// Occurs when the RefreshViewWidth property is changed.
		/// </summary>
		/// <param name="bindable">The <see cref="SfPullToRefresh"/> as <see cref="BindableObject"/>.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnRefreshViewWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null && pullToRefresh.ProgressCircleView is not null)
			{
				pullToRefresh.ProgressCircleView.UpdateDrawProperties();
				if ((pullToRefresh.IsPulling || pullToRefresh.ActualIsRefreshing) && pullToRefresh.ProgressCircleView.Content is null)
				{
					pullToRefresh.MeasureSfProgressCircleView(pullToRefresh.Width, pullToRefresh.Height);
					pullToRefresh.ProgressCircleView.UpdateCircleViewBounds();
					pullToRefresh.ManualArrangeContent(pullToRefresh.TransitionMode == PullToRefreshTransitionType.SlideOnTop, pullToRefresh.GetBounds());
				}
			}
		}

		/// <summary>
		/// Occurs when the ProgressThickness property is changed.
		/// </summary>
		/// <param name="bindable">The <see cref="SfPullToRefresh"/> as <see cref="BindableObject"/>.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnProgressThicknessChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null)
			{
				// No need for do invalidate in Pulling because on pulling circle layout will be updated subsequently and draw also refreshes.
				if (pullToRefresh.ActualIsRefreshing && pullToRefresh.ProgressCircleView is not null && pullToRefresh.ProgressCircleView.Content is null)
				{
					pullToRefresh.ProgressCircleView.InvalidateDrawable();
				}
			}
		}

		/// <summary>
		/// Occurs when the ProgressBackground property is changed.
		/// </summary>
		/// <param name="bindable">The <see cref="SfPullToRefresh"/> as <see cref="BindableObject"/>.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnProgressBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null)
			{
				// No need for do invalidate in Pulling because on pulling circle layout will be updated subsequently and draw also refreshes.
				if (pullToRefresh.ActualIsRefreshing && pullToRefresh.ProgressCircleView is not null && pullToRefresh.ProgressCircleView.Content is null)
				{
					pullToRefresh.ProgressCircleView.InvalidateDrawable();
				}
			}
		}

		/// <summary>
		/// Occurs when the ProgressColor property is changed.
		/// </summary>
		/// <param name="bindable">The <see cref="SfPullToRefresh"/> as <see cref="BindableObject"/>.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnProgressColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfPullToRefresh? pullToRefresh = bindable as SfPullToRefresh;
			if (pullToRefresh is not null)
			{
				// No need for do invalidate in Pulling because on pulling circle layout will be updated subsequently and draw also refreshes.
				if (pullToRefresh.ActualIsRefreshing && pullToRefresh.ProgressCircleView is not null && pullToRefresh.ProgressCircleView.Content is null)
				{
					pullToRefresh.ProgressCircleView.InvalidateDrawable();
				}
			}
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Method invokes to Get initial set of color from Theme dictionary.
		/// </summary>
		/// <returns> Returns the <see cref ="SfPullToRefresh"/> theme dictionary.</returns>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfPullToRefreshStyles();
		}

		/// <summary>
		/// Method invokes when control theme changes.
		/// </summary>
		/// <param name="oldTheme">Represents the  old theme.</param>
		/// <param name="newTheme">Represents the  new theme.</param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		/// <summary>
		/// Method invokes at whenever common theme changes.
		/// </summary>
		/// <param name="oldTheme">Represents the  old theme.</param>
		/// <param name="newTheme">Represents the  new theme.</param>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the pulling operation is performed.
		/// </summary>
		/// <seealso cref="Refreshing"/>
		/// <seealso cref="Refreshed"/>
		/// <example>
		/// Here is an example of how to register the <see cref="Pulling"/> event.
		/// 
		///  # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh();
		/// pullToRefresh.Pulling += OnPullToRefreshPulling;
		/// 
		/// private void PullToRefresh_Pulling(object? sender, Toolkit.PullToRefresh.PullingEventArgs e)
		/// {
		///    args.Cancel = false;
		///    var progress = args.Progress;
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<PullingEventArgs>? Pulling;

		/// <summary>
		/// Occurs when the refreshing operation starts.
		/// </summary>
		/// <seealso cref="Pulling"/>
		/// <seealso cref="Refreshed"/>
		/// <example>
		/// Here is an example of how to register the <see cref="Refreshing"/> event.
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh();
		/// pullToRefresh.Refreshing += OnPullToRefreshRefreshing;
		/// 
		/// private async void OnPullToRefreshRefreshing(object sender, EventArgs args)
		/// {
		///    pullToRefresh.IsRefreshing = true;
		///    await Task.Delay(2000);
		///    pullToRefresh.IsRefreshing = false;
		///	}
		/// ]]></code>
		/// </example>
		public event EventHandler<EventArgs>? Refreshing;

		/// <summary>
		/// Occurs when the refreshing operation completes.
		/// </summary>
		/// <seealso cref="Pulling"/>
		/// <seealso cref="Refreshing"/>
		/// <example>
		/// Here is an example of how to register the <see cref="Refreshed"/> event.
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfPullToRefresh pullToRefresh = new SfPullToRefresh();
		/// pullToRefresh.Refreshed += OnPullToRefreshRefreshed;
		/// 
		/// private void OnPullToRefreshRefreshed(object sender, EventArgs args)
		/// {
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<EventArgs>? Refreshed;

		#endregion
	}
}
