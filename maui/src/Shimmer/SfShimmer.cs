using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Shimmer
{
	/// <summary>
	/// Represents a loading indicator control that provides modern animations when data is being loaded.
	/// </summary>
	public partial class SfShimmer : SfContentView, IShimmer, IParentThemeElement
	{
		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="AnimationDuration"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AnimationDuration"/> property determines the duration of wave animation of the <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty AnimationDurationProperty =
			BindableProperty.Create(
				nameof(AnimationDuration),
				typeof(double),
				typeof(SfShimmer),
				1000d,
				propertyChanged: OnAnimationDurationChanged);

		/// <summary>
		/// Identifies the <see cref="Fill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Fill"/> property determines the background color of the <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty FillProperty =
			BindableProperty.Create(
				nameof(Fill),
				typeof(Brush),
				typeof(SfShimmer),
				null,
				defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#F7F2FB")),
				propertyChanged: OnFillPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CustomView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CustomView"/> property is used to customize the view using framework elements. <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty CustomViewProperty =
			BindableProperty.Create(
				nameof(CustomView),
				typeof(View),
				typeof(SfShimmer),
				null,
				propertyChanged: OnCustomViewChanged);

		/// <summary>
		/// Identifies the <see cref="IsActive"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsActive"/> property determines whether to load the actual content of the <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty IsActiveProperty =
			BindableProperty.Create(
				nameof(IsActive),
				typeof(bool),
				typeof(SfShimmer),
				true,
				propertyChanged: OnIsActivePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="WaveColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="WaveColor"/> property determines wave color in <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty WaveColorProperty =
			BindableProperty.Create(
				nameof(WaveColor),
				typeof(Color),
				typeof(SfShimmer),
				Color.FromArgb("#FFFFFF"));

		/// <summary>
		/// Identifies the <see cref="WaveWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="WaveWidth"/> property determines wave width in <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty WaveWidthProperty =
			BindableProperty.Create(
				nameof(WaveWidth),
				typeof(double),
				typeof(SfShimmer),
				200d,
				propertyChanged: OnWaveWidthChanged);

		/// <summary>
		/// Identifies the <see cref="Type"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Type"/> property determines view type of the <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty TypeProperty =
			BindableProperty.Create(
				nameof(Type),
				typeof(ShimmerType),
				typeof(SfShimmer),
				ShimmerType.CirclePersona,
				propertyChanged: OnTypeChanged);

		/// <summary>
		/// Identifies the <see cref="WaveDirection"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="WaveDirection"/> property determines the wave direction in <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty WaveDirectionProperty =
			BindableProperty.Create(
				nameof(WaveDirection),
				typeof(ShimmerWaveDirection),
				typeof(SfShimmer),
				ShimmerWaveDirection.Default,
				propertyChanged: OnWaveDirectionChanged);

		/// <summary>
		/// Identifies the <see cref="RepeatCount"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RepeatCount"/> property determines the number of times the built-in view animation is repeated in the <see cref="SfShimmer"/>.
		/// </remarks>
		public static readonly BindableProperty RepeatCountProperty =
			BindableProperty.Create(
				nameof(RepeatCount),
				typeof(int),
				typeof(SfShimmer),
				1,
				propertyChanged: OnRepeatCountPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShimmerBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShimmerBackground"/> property determines the background color of the <see cref="SfShimmer"/>.
		/// </remarks>
		internal static readonly BindableProperty ShimmerBackgroundProperty =
		  BindableProperty.Create(
			  nameof(ShimmerBackground),
			  typeof(Color),
			  typeof(SfShimmer),
			  defaultValueCreator: bindable => Color.FromArgb("#FFFBFE"),
			  propertyChanged: OnShimmerBackgroundChanged);

		#endregion

		#region Fields

		/// <summary>
		/// Backing field to store the <see cref="ShimmerDrawable"/> instance.
		/// </summary>
		WeakReference<ShimmerDrawable>? _shimmerDrawable;

		/// <summary>
		/// Holds the available size for the shimmer.
		/// </summary>
		Size _availableSize = Size.Zero;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfShimmer"/> class.
		/// </summary>
		public SfShimmer()
		{
			ThemeElement.InitializeThemeResources(this, "SfShimmerTheme");
			BackgroundColor = ShimmerBackground;
			ShimmerDrawable = new ShimmerDrawable(this);

			// Add the actual ShimmerDrawable instance, not the WeakReference
			Add(ShimmerDrawable);
#if IOS
			IgnoreSafeArea = true;
#endif
		}


		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the duration of the wave animation in milliseconds.
		/// </summary>
		/// <value>The default value of <see cref="AnimationDuration"/> is 1000 milliseconds.</value>
		/// <example>
		/// The following code demonstrates how to use the AnimationDuration property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer"
		///                    AnimationDuration="1500">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.AnimationDuration = 1500;
		/// ]]></code>
		/// </example>
		public double AnimationDuration
		{
			get { return (double)GetValue(AnimationDurationProperty); }
			set { SetValue(AnimationDurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background color of the shimmer view. 
		/// </summary>
		/// <value>The default value of <see cref="Fill"/> is "#F3EDF6".</value>
		/// <remarks>
		/// If the brush is a gradient brush, it only applies the first color.
		/// </remarks>
		/// <example>
		/// The following code demonstrates how to use the Fill property in the Shimmer.
		/// # [XAML](#tab/tabid-3)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer" 
		///                    Fill="AliceBlue">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-4)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.Fill = Brush.AliceBlue;
		/// ]]></code>
		/// </example>
		public Brush Fill
		{
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		/// <summary>
		/// Gets or sets the custom view that is used for the loading view.
		/// </summary>
		/// <value>The default value of <see cref="CustomView"/> is null.</value>
		/// <example>
		/// The following code demonstrates how to use the CustomView property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-5)
		/// <code Lang="XAML"><![CDATA[
		///  <shimmer:SfShimmer x:Name="Shimmer">
		///      <shimmer:SfShimmer.CustomView>
		///          <Grid HeightRequest="50" WidthRequest="200">
		///              <Grid.RowDefinitions>
		///                  <RowDefinition/>
		///                  <RowDefinition/>
		///              </Grid.RowDefinitions>
		///              <Grid.ColumnDefinitions>
		///                  <ColumnDefinition Width="0.25*"/>
		///                  <ColumnDefinition Width="0.75*"/>
		///              </Grid.ColumnDefinitions>
		/// 
		///              <shimmer:ShimmerView ShapeType="Circle" Grid.RowSpan="2"/>
		///              <shimmer:ShimmerView Grid.Column="1" Margin="5"/>
		///              <shimmer:ShimmerView ShapeType="RoundedRectangle" Grid.Row="1" Grid.Column="1" Margin="5"/>
		///          </Grid>
		///      </shimmer:SfShimmer.CustomView>
		///  </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-6)
		/// <code Lang="C#"><![CDATA[
		/// Grid grid = new Grid
		/// {
		///     HeightRequest = 50, 
		///     WidthRequest = 200,
		///     RowDefinitions =
		///     {
		///         new RowDefinition(),
		///         new RowDefinition(),
		///     },
		///     ColumnDefinitions =
		///     {
		///         new ColumnDefinition { Width = new GridLength(0.25, GridUnitType.Star) },
		///         new ColumnDefinition { Width = new GridLength(0.75, GridUnitType.Star) }
		///     }
		/// };
		/// 
		/// ShimmerView circleView = new ShimmerView() { ShapeType = ShimmerShapeType.Circle};
		/// grid.SetRowSpan(circleView, 2);
		/// grid.Add(circleView);
		/// grid.Add(new ShimmerView { Margin = 5 }, 1);
		/// grid.Add(new ShimmerView { Margin = 5, ShapeType = ShimmerShapeType.RoundedRectangle }, 1, 1);
		/// Shimmer.CustomView = grid;
		/// ]]></code>
		/// </example>
		public View CustomView
		{
			get { return (View)GetValue(CustomViewProperty); }
			set { SetValue(CustomViewProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to load the actual content of the <see cref="SfShimmer"/>.
		/// </summary>
		/// <value>The default value of <see cref="IsActive"/> is true.</value>
		/// <example>
		/// The following code demonstrates how to use the IsActive property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer"
		///                    IsActive="True">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.IsActive = true;
		/// ]]></code>
		/// </example>
		public bool IsActive
		{
			get { return (bool)GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}

		/// <summary>
		/// Gets or sets the shimmer wave color.
		/// </summary>
		/// <value>The default value of <see cref="WaveColor"/> is "#FFFBFE".</value>
		/// <example>
		/// The following code demonstrates how to use the WaveColor property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-9)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer"
		///                    WaveColor="AliceBlue">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-10)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.WaveColor = Colors.AliceBlue;
		/// ]]></code>
		/// </example>
		public Color WaveColor
		{
			get { return (Color)GetValue(WaveColorProperty); }
			set { SetValue(WaveColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the width of the wave.
		/// </summary>
		/// <value>The default value of <see cref="WaveWidth"/> is 200.</value>
		/// <example>
		/// The following code demonstrates how to use the WaveWidth property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-11)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer"
		///                    WaveWidth="150">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-12)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.WaveWidth = 150;
		/// ]]></code>
		/// </example>
		public double WaveWidth
		{
			get { return (double)GetValue(WaveWidthProperty); }
			set { SetValue(WaveWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the built-in shimmer view type. 
		/// </summary>
		/// <value>The default value of <see cref="Type"/> is <see cref="ShimmerType.CirclePersona"/>.</value>
		/// <example>
		/// The following code demonstrates how to use the Type property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-13)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer"
		///                    Type="Article">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-14)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.Type = ShimmerType.Article;
		/// ]]></code>
		/// </example>
		public ShimmerType Type
		{
			get { return (ShimmerType)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the animation direction for Shimmer.
		/// </summary>
		/// <value>The default value of <see cref="WaveDirection"/> is <see cref="ShimmerWaveDirection.Default"/>.</value>
		/// <example>
		/// The following code demonstrates how to use the WaveDirection property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-15)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer" 
		///                    WaveDirection="RightToLeft">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-16)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.WaveDirection = ShimmerWaveDirection.RightToLeft;
		/// ]]></code>
		/// </example>
		public ShimmerWaveDirection WaveDirection
		{
			get { return (ShimmerWaveDirection)GetValue(WaveDirectionProperty); }
			set { SetValue(WaveDirectionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the number of times the built-in view should be repeated.
		/// </summary>
		/// <value>The default value of <see cref="RepeatCount"/> is 1.</value>
		/// <remarks>
		///  The repeat count is applicable only to the built-in views and not to custom views.
		/// </remarks>
		/// <example>
		/// The following code demonstrates how to use the RepeatCount property in the <see cref="SfShimmer"/>.
		/// # [XAML](#tab/tabid-17)
		/// <code Lang="XAML"><![CDATA[
		/// <shimmer:SfShimmer x:Name="Shimmer"
		///                    RepeatCount="2">
		/// </shimmer:SfShimmer>
		/// ]]></code>
		/// # [C#](#tab/tabid-18)
		/// <code Lang="C#"><![CDATA[
		/// Shimmer.RepeatCount = 2;
		/// ]]></code>
		/// </example>
		public int RepeatCount
		{
			get { return (int)GetValue(RepeatCountProperty); }
			set { SetValue(RepeatCountProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background color of the <see cref="SfShimmer"/>.
		/// </summary>
		internal Color ShimmerBackground
		{
			get { return (Color)GetValue(ShimmerBackgroundProperty); }
			set { SetValue(ShimmerBackgroundProperty, value); }
		}

		internal ShimmerDrawable? ShimmerDrawable
		{
			get => _shimmerDrawable != null && _shimmerDrawable.TryGetTarget(out var v) ? v : null;
			set => _shimmerDrawable = value == null ? null : new(value);
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Measures the content of the shimmer.
		/// </summary>
		/// <exclude/>
		/// <param name="widthConstraint">The width constraint.</param>
		/// <param name="heightConstraint">The height constraint.</param>
		/// <returns>The size of the content.</returns>
		/// <exclude/>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			// Measure behavior :
			// 1. If the widthConstraint and heightConstraint are finite, then provided height and width will be returned.
			// 2. If the width is infinite then content width will be returned along with finite height constraint.
			// 3. If the height is infinite then content height will be returned along with finite width constraint.
			// 4. If the width and height are infinite then content width and height will be returned.
			// 5. If the width or height, or both are not finite, and also the measured content size is also zero in that case default size will be returned.
			bool isValidHeight = double.IsFinite(heightConstraint);
			bool isValidWidth = double.IsFinite(widthConstraint);

			// Calculate the initial measured size, considering padding.
			var padding = Padding;
			Size measuredSize = new Size(isValidWidth ? widthConstraint : 300, isValidHeight ? heightConstraint : 300);
			measuredSize = new Size(Math.Max(measuredSize.Width - padding.HorizontalThickness, 0), Math.Max(measuredSize.Height - padding.VerticalThickness, 0));

			foreach (var child in Children)
			{
				if (child == Content)
				{
					// When the height constraint or width constraint is not valid(infinite), we use double.PositiveInfinity to allow
					// the content to determine its own natural height or width without any restrictions.
					// This ensures that the content can lay out properly even when no specific height is provided by the parent.
					Size contentSize = child.Measure(isValidWidth ?  measuredSize.Width : double.PositiveInfinity, isValidHeight ? measuredSize.Height : double.PositiveInfinity);

					// If the returned content size is zero, the custom view or the shimmer drawable may not get rendered.
					// Because we are measuring the custom view or the shimmer drawable with the content size.
					contentSize = contentSize == Size.Zero ? measuredSize : contentSize;

					// Update measured size based on content size and constraints.
					measuredSize = new Size(isValidWidth ? measuredSize.Width : contentSize.Width, isValidHeight ? measuredSize.Height : contentSize.Height);
				}
				else if (child == CustomView || child == ShimmerDrawable)
				{
					child.Measure(measuredSize.Width, measuredSize.Height);
				}
			}

			_availableSize = measuredSize;
			return _availableSize;
		}

		/// <summary>
		/// Called when the <see cref="SfContentView.Content"/> of the shimmer changes.
		/// </summary>
		/// <exclude/>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		/// <exclude/>
		protected override void OnContentChanged(object oldValue, object newValue)
		{
			if (oldValue is View oldView && Children.Contains(oldView))
			{
				Remove(oldView);
				if (oldView.Handler != null && oldView.Handler.PlatformView != null)
				{
					oldView.Handler.DisconnectHandler();
				}
			}

			if (newValue is View newView)
			{
				// Adding it in 0th index to make sure it is at the bottom of the stack(1st index - custom view, 2nd index - shimmer drawable)
				// So when the shimmer is active, the shimmer drawable will be visible (hiding the content) else content will be visible.
				Insert(0, newView);

				// Shimmer content visible only when the shimmer is inactive.
				newView.Opacity = IsActive ? 0 : 1;
			}
		}

		/// <summary>
		/// Method to clean up resources when the handler is changed.
		/// </summary>
		protected override void OnHandlerChanged()
		{
			base.OnHandlerChanged();
			if (Handler == null)
			{
				if (ShimmerDrawable != null)
				{
					if (AnimationExtensions.AnimationIsRunning(ShimmerDrawable, "ShimmerAnimation"))
					{
						ShimmerDrawable.AbortAnimation("ShimmerAnimation");
					}
					ShimmerDrawable.Dispose();
					if (ShimmerDrawable.Handler != null && ShimmerDrawable.Handler.PlatformView != null)
					{
						ShimmerDrawable.Handler.DisconnectHandler();
					}
					if (Children.Contains(ShimmerDrawable))
					{
						Remove(ShimmerDrawable);
					}
					ShimmerDrawable = null;
				}

				if (CustomView != null)
				{
					if (CustomView.Handler != null && CustomView.Handler.PlatformView != null)
					{
						CustomView.Handler.DisconnectHandler();
					}
					if (Children.Contains(CustomView))
					{
						Remove(CustomView);
					}
					// Clear parent relationship explicitly (Mac/iOS specific)
					if (CustomView.Parent == this)
					{
						CustomView.Parent = null;
					}
					// Dispose if disposable
					if (CustomView is IDisposable disposableView)
					{
						disposableView.Dispose();
					}
				}

				Children.Clear();
			}
		}

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Called when the <see cref="CustomView"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnCustomViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer)
			{
				return;
			}

			View newView = (View)newValue;
			if (oldValue is View oldView && shimmer.Children.Contains(oldView))
			{
				shimmer.Remove(oldView);
				if (oldView.Handler != null && oldView.Handler.PlatformView != null)
				{
					oldView.Handler.DisconnectHandler();
				}
			}

			if (newView == null)
			{
				shimmer.ShimmerDrawable?.UpdateShimmerDrawable();

				return;
			}

			// Setting the input transparent to true to transfer the touch events from custom view to the content.
			newView.InputTransparent = true;

			// The custom view should be in index before the shimmer drawable.
			// Reason : 
			// Because we are drawing the custom view children in shimmer drawable. In order to get the correct bounds for each children 
			// in custom view, we need to add or insert the custom view prior to shimmer drawable to get the view measured and arranged. 
			// In that way we can get proper view bounds.
			if (shimmer.IsActive)
			{
				if (shimmer.ShimmerDrawable != null && shimmer.Children.Contains(shimmer.ShimmerDrawable))
				{
					int index = Math.Max(shimmer.Children.Count - 1, 0);
					shimmer.Insert(index, newView);
				}
				else
				{
					shimmer.Add(newView);
				}
			}

			// Setting the opacity to 0 to hide the custom view.
			// A small line is clearly visible on view it its is 1;
			newView.Opacity = 0;
			shimmer.ShimmerDrawable?.UpdateShimmerDrawable();
		}

		/// <summary>
		/// Called when the <see cref="IsActive"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnIsActivePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer)
			{
				return;
			}

			//// Issue Scenario: In Android platform when we use the shimmer along with the PullToRefresh, collection view and change the IsActive property on refresh
			//// "Cannot access disposed object" exception occurs after certain number of refresh and page navigations.
			//// Fix: So we are check if the Android native view is still valid before proceeding
			//// This prevents "Cannot access disposed object" exceptions that occur when
			//// trying to interact with platform views that have been disposed during
			//// page navigation or when the control is removed from the visual tree.
#if ANDROID
			var layoutHandler = shimmer.Handler;
			if (layoutHandler != null)
			{
				var nativeView = layoutHandler.PlatformView as Android.Views.View;
				if (nativeView == null || nativeView.Handle == IntPtr.Zero || nativeView.Context == null)
				{
					return;
				}
			}
#endif

			bool isActive = (bool)newValue;
			if (isActive)
			{
				if (shimmer.CustomView != null && !shimmer.Children.Contains(shimmer.CustomView))
				{
					shimmer.Add(shimmer.CustomView);
				}

				shimmer.ShimmerDrawable = new ShimmerDrawable(shimmer);
				shimmer.Add(shimmer.ShimmerDrawable);
				if (shimmer.Content != null)
				{
					// Shimmer content does not visible while the shimmer is active.
					shimmer.Content.Opacity = 0;
				}
			}
			else
			{
				if (shimmer.Content != null)
				{
					//// Need to visible the Shimmer content while the shimmer is inactive.
					shimmer.Content.Opacity = 1;
				}

				if (shimmer.ShimmerDrawable == null)
				{
					return;
				}

				if (AnimationExtensions.AnimationIsRunning(shimmer.ShimmerDrawable, "ShimmerAnimation"))
				{
					shimmer.ShimmerDrawable.AbortAnimation("ShimmerAnimation");
				}

				if (shimmer.ShimmerDrawable != null)
				{
					shimmer.Remove(shimmer.ShimmerDrawable);

					if (shimmer.ShimmerDrawable.Handler != null && shimmer.ShimmerDrawable.Handler.PlatformView != null)
					{
						shimmer.ShimmerDrawable.Handler.DisconnectHandler();
					}

					if (shimmer.CustomView != null)
					{
						shimmer.Remove(shimmer.CustomView);
					}

					shimmer.ShimmerDrawable = null;
				}
			}
		}

		/// <summary>
		/// Called when the <see cref="AnimationDuration"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnAnimationDurationChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer || shimmer.ShimmerDrawable == null || shimmer._availableSize == Size.Zero)
			{
				return;
			}

			shimmer.ShimmerDrawable.CreateWaveAnimator();
		}

		/// <summary>
		/// Called when the <see cref="WaveWidth"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnWaveWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer || shimmer.ShimmerDrawable == null || shimmer._availableSize == Size.Zero)
			{
				return;
			}

			shimmer.ShimmerDrawable.CreateWaveAnimator();
		}

		/// <summary>
		/// Called when the <see cref="Type"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer || shimmer.ShimmerDrawable == null || shimmer._availableSize == Size.Zero)
			{
				return;
			}

			shimmer.ShimmerDrawable.UpdateShimmerDrawable();
		}

		/// <summary>
		/// Called when the <see cref="WaveDirection"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnWaveDirectionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer || shimmer.ShimmerDrawable == null || shimmer._availableSize == Size.Zero)
			{
				return;
			}

			shimmer.ShimmerDrawable.CreateWavePaint();
		}

		/// <summary>
		/// called when the <see cref="RepeatCount"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnRepeatCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer || shimmer.CustomView != null || shimmer.ShimmerDrawable == null || shimmer._availableSize == Size.Zero)
			{
				return;
			}

			shimmer.ShimmerDrawable.UpdateShimmerDrawable();
		}

		/// <summary>
		/// called when the <see cref="Fill"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfShimmer shimmer || shimmer.ShimmerDrawable == null || shimmer._availableSize == Size.Zero)
			{
				return;
			}

			// OnDraw method won't due to animation duration was 0. in that case we are invalidating manually to update the fill.
			if (shimmer.AnimationDuration <= 0)
			{
				shimmer.ShimmerDrawable.InvalidateDrawable();
			}
		}

		/// <summary>
		/// called when the <see cref="ShimmerBackground"/> property changes.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnShimmerBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfShimmer shimmer)
			{
				shimmer.BackgroundColor = shimmer.ShimmerBackground;
			}
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Gets the theme dictionary for the parent theme element.
		/// This method is declared only in IParentThemeElement and you need to implement this method only in main control.
		/// </summary>
		/// <returns>The theme dictionary.</returns>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfShimmerStyles();
		}

		/// <summary>
		/// This method will be called when a theme dictionary that contains the value for your control key is merged in application. 
		/// </summary>
		/// <param name="oldTheme">The old theme.</param>
		/// <param name="newTheme">The new theme.</param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
			SetDynamicResource(ShimmerBackgroundProperty, "SfShimmerNormalBackground");
		}

		/// <summary>
		/// This method will be called when users merge a theme dictionary that contains value for “SyncfusionTheme” dynamic resource key.
		/// </summary>
		/// <param name="oldTheme">The old theme.</param>
		/// <param name="newTheme">The new theme.</param>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		#endregion

	}
}