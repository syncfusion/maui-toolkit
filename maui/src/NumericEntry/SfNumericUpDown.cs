using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using Syncfusion.Maui.Toolkit.NumericEntry;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.EntryView;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.NumericUpDown
{
	/// <summary>
	/// The <see cref="SfNumericUpDown"/> class allows users to adjust numeric values 
	/// using increment and decrement buttons or by direct input. It supports value range 
	/// constraints, step size, and culture-specific formatting.
	/// </summary>
	/// <example>
	/// The below example demonstrates how to initialize the <see cref="SfNumericUpDown"/>.
	/// # [XAML](#tab/tabid-1)
	/// <code Lang="XAML"><![CDATA[
	/// <numeric:SfNumericUpDown
	///       x:Name="numericUpDown"
	///       Value="10"
	///       Minimum="0"
	///       Maximum="100"
	///       WidthRequest="200"/>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code Lang="C#"><![CDATA[
	/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
	/// numericUpDown.Value = 10;
	/// numericUpDown.Minimum = 0;
	/// numericUpDown.Maximum = 100;
	/// numericUpDown.WidthRequest = 200;
	/// ]]></code>
	/// ***
	/// </example>

	public partial class SfNumericUpDown : SfNumericEntry, ITextInputLayout, IParentThemeElement
	{

		#region Fields

		/// <summary>
		/// Gets or sets the right margin of the document.
		/// </summary>
		double _rightMargin;

		/// <summary>
		/// Gets or sets the left margin of the document.
		/// </summary>
		double _leftMargin;

		/// <summary>
		/// Timer for handling long press events.
		/// </summary>
		bool _isPressOccurring = false;

		/// <summary>
		/// Color of the up button.
		/// </summary>
		Color _upButtonColor = Colors.Black;

		/// <summary>
		/// Color of the down button.
		/// </summary>
		Color _downButtonColor = Colors.Black;

		/// <summary>
		/// Represents the point where the touch event occurs.
		/// </summary>
		Point _touchPoint;

		/// <summary>
		/// Size of the up/down buttons in pixels.
		/// </summary>
		const int UpDownButtonSize = 28;

		/// <summary>
		/// Represents the size of the up/down button when placed vertically.
		/// </summary>
		const int VerticalUpDownButtonSize = 24;

		/// <summary>
		/// Padding around the buttons in pixels.
		/// </summary>
		const int ButtonPadding = 2;

		/// <summary>
		/// The initial delay in milliseconds before activating a long press.
		/// </summary>
		const int StartDelay = 500;

		/// <summary>
		/// The interval in milliseconds between repeated actions during a continuous long press.
		/// </summary>
		const int ContinueDelay = 200;

		/// <summary>
		/// Holds the view for the up button, used to increase the value.
		/// </summary>
		View? _upButtonView;

		/// <summary>
		/// Holds the view for the down button, used to decrease the value.
		/// </summary>
		View? _downButtonView;

		/// <summary>
		/// List of semantic nodes for numeric entry.
		/// </summary>
		readonly List<SemanticsNode> _numericUpDownSemanticsNodes = [];

		/// <summary>
		/// Size for the semantics of the down button.
		/// </summary>
		Size _downButtonSemanticsSize = Size.Zero;

		/// <summary>
		/// Size for the semantics of the up button.
		/// </summary>
		Size _upButtonSemanticsSize = Size.Zero;

		/// <summary>
		/// Rectangle for the down button's area.
		/// </summary>
		RectF _downButtonRectF;

		/// <summary>
		/// Rectangle for the up button's area.
		/// </summary>
		RectF _upButtonRectF;

		/// <summary>
		/// Temporary X position for the up/down buttons.
		/// </summary>
		float _tempUpDownX;

		/// <summary>
		/// Provides a cancellation token source for managing long press operations.
		/// This allows for cancellation of ongoing long press tasks when needed.
		/// </summary>
		CancellationTokenSource? _cancellationTokenSource;

#if ANDROID
		/// <summary>
		/// Padding around the buttons in pixels.
		/// </summary>
		const int AndroidButtonHeightPadding = 8;
#endif

		#endregion

		#region Bindiable Properties

		/// <summary>
		/// Identifies <see cref="AutoReverse"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="AutoReverse"/> bindable property.</value>
		public static readonly BindableProperty AutoReverseProperty =
			BindableProperty.Create(
				nameof(AutoReverse),
				typeof(bool),
				typeof(SfNumericEntry),
				false,
				BindingMode.Default,
				null,
				propertyChanged:OnAutoReversePropertyChanged);

		/// <summary>
		/// Identifies <see cref="UpDownPlacementMode"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="UpDownPlacementMode"/> bindable property.</value>
		public static readonly BindableProperty UpDownPlacementModeProperty =
			BindableProperty.Create(
				nameof(UpDownPlacementMode),
				typeof(NumericUpDownPlacementMode),
				typeof(SfNumericUpDown),
				NumericUpDownPlacementMode.Inline,
				BindingMode.Default,
				propertyChanged: OnSpinButtonPlacementChanged);

		/// <summary>
		/// Identifies <see cref="UpDownButtonColor"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="UpDownButtonColor"/> bindable property.</value>
		public static readonly BindableProperty UpDownButtonColorProperty =
			BindableProperty.Create(nameof(UpDownButtonColor),
				typeof(Color),
				typeof(SfNumericUpDown),
				Colors.Black,
				BindingMode.Default,
				propertyChanged: OnUpDownButtonColorPropertyChanged);

		/// <summary>
		/// Identifies <see cref="DownButtonTemplate"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="DownButtonTemplate"/> bindable property.</value>
		public static readonly BindableProperty DownButtonTemplateProperty =
			BindableProperty.Create(
				nameof(DownButtonTemplate),
				typeof(DataTemplate),
				typeof(SfNumericUpDown),
				null,
				BindingMode.Default,
				propertyChanged: OnDownButtonTemplatePropertyChanged);

		/// <summary>
		/// Identifies <see cref="UpButtonTemplateProperty"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="UpButtonTemplate"/> bindable property.</value>
		public static readonly BindableProperty UpButtonTemplateProperty =
			BindableProperty.Create(
				nameof(UpButtonTemplate),
				typeof(DataTemplate),
				typeof(SfNumericUpDown),
				null,
				BindingMode.Default,
				propertyChanged: OnUpButtonTemplatePropertyChanged);

		/// <summary>
		/// Identifies <see cref="SmallChange"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="SmallChange"/> bindable property.</value>
		public static readonly BindableProperty SmallChangeProperty =
			BindableProperty.Create(
				nameof(SmallChange),
				typeof(double),
				typeof(SfNumericUpDown),
				1.0,
				BindingMode.Default);

		/// <summary>
		/// Identifies <see cref="LargeChange"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="LargeChange"/> bindable property.</value>
		public static readonly BindableProperty LargeChangeProperty =
		BindableProperty.Create(
			nameof(LargeChange),
			typeof(double),
			typeof(SfNumericUpDown),
			10.0,
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="UpDownButtonAlignment"/> bindable property.
		/// </summary>
		public static readonly BindableProperty UpDownButtonAlignmentProperty =
			BindableProperty.Create(
				nameof(UpDownButtonAlignment),
				typeof(UpDownButtonAlignment),
				typeof(SfNumericUpDown),
				UpDownButtonAlignment.Right,
				propertyChanged: OnUpDownButtonAlignmentChanged);

		/// <summary>
		/// Identifies <see cref="UpDownButtonDisableColor"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="UpDownButtonDisableColor"/> bindable property.</value>
		internal static readonly BindableProperty UpDownButtonDisableColorProperty =
			BindableProperty.Create(nameof(UpDownButtonDisableColor),
				typeof(Color),
				typeof(SfNumericUpDown),
				Color.FromArgb("#611c1b1f"),
				BindingMode.Default,
				propertyChanged: OnUpDownButtonColorPropertyChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the SfNumericUpDown class.
		/// This control allows users to select a numeric value using up and down buttons.
		/// </summary>
		public SfNumericUpDown() : base(true)
		{
			ThemeElement.InitializeThemeResources(this, "SfNumericUpDownTheme");
			this.AddTouchListener(this);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether to enable or disable the cyclic behavior when the value reaches the minimum or maximum value. 
		/// </summary>
		/// <value>The default value is <c>false</c>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="AutoReverse"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericUpDown:NumericUpDown
		///     x:Name="numericUpDown"
		///     AutoReverse="True" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// NumericUpDown numericUpDown = new NumericUpDown();
		/// numericUpDown.AutoReverse = true;
		/// this.Content = numericUpDown;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public bool AutoReverse
		{
			get { return (bool)GetValue(AutoReverseProperty); }
			set { SetValue(AutoReverseProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the up and down buttons in the <see cref="NumericUpDown"/> control.
		/// </summary>
		/// <value>
		/// The default value is <see cref="Colors.Black"/>.
		/// </value>
		/// <example>
		/// To set the up and down button color to blue in XAML:
		/// <code Lang="XAML"><![CDATA[
		/// <numericUpDown:SfNumericUpDown
		///     x:Name="numericUpDown"
		///     UpDownButtonColor="Blue" />
		/// ]]></code>
		/// To set the up and down button color to blue in C#:
		/// <code Lang="C#"><![CDATA[
		/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
		/// numericUpDown.UpDownButtonColor = Colors.Blue;
		/// this.Content = numericUpDown;
		/// ]]></code>
		/// </example>
		public Color UpDownButtonColor
		{
			get { return (Color)GetValue(UpDownButtonColorProperty); }
			set { SetValue(UpDownButtonColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a Data template for down button.
		/// </summary>
		/// <value>
		/// The default value is null.
		/// </value>
		/// <example>
		/// Below is an example of how to define a custom template for the down button:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <numericUpDown:SfNumericUpDown x:Name="numericUpDown">
		///     <numericUpDown:SfNumericUpDown.DownButtonTemplate>
		///         <DataTemplate>
		///             <Grid>
		///                 <Label 
		///                     Text="-" 
		///                     HorizontalOptions="Center" 
		///                     VerticalOptions="Center"
		///                     FontSize="16" 
		///                     FontAttributes="Bold" />
		///             </Grid>
		///         </DataTemplate>
		///     </numericUpDown:SfNumericUpDown.DownButtonTemplate>
		/// </numericUpDown:NumericUpDown>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
		/// numericUpDown.DownButtonTemplate = new DataTemplate(() =>
		/// {
		///     Grid grid = new Grid();
		///     Label label = new Label
		///     {
		///         Text = "-",
		///         HorizontalOptions = LayoutOptions.Center,
		///         VerticalOptions = LayoutOptions.Center,
		///         FontSize = 16,
		///         FontAttributes = FontAttributes.Bold
		///     };
		///     grid.Children.Add(label);
		///     return grid;
		/// });
		/// ]]></code>
		/// ***
		/// </example>
		public DataTemplate DownButtonTemplate
		{
			get { return (DataTemplate)GetValue(DownButtonTemplateProperty); }
			set { SetValue(DownButtonTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets a Data template for up button.
		/// </summary>
		/// <value>
		/// The default value is null.
		/// </value>
		/// /// <example>
		/// Below is an example of how to define a custom template for the up button:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <numericUpDown:SfNumericUpDown x:Name="numericUpDown">
		///     <numericUpDown:SfNumericUpDown.UpButtonTemplate>
		///         <DataTemplate>
		///             <Grid>
		///                 <Label 
		///                     Text="+" 
		///                     HorizontalOptions="Center" 
		///                     VerticalOptions="Center"
		///                     FontSize="16" 
		///                     FontAttributes="Bold" />
		///             </Grid>
		///         </DataTemplate>
		///     </numericUpDown:SfNumericUpDown.UpButtonTemplate>
		/// </numericUpDown:SfNumericUpDown>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
		/// numericUpDown.UpButtonTemplate = new DataTemplate(() =>
		/// {
		///     Grid grid = new Grid();
		///     Label label = new Label
		///     {
		///         Text = "+",
		///         HorizontalOptions = LayoutOptions.Center,
		///         VerticalOptions = LayoutOptions.Center,
		///         FontSize = 16,
		///         FontAttributes = FontAttributes.Bold
		///     };
		///     grid.Children.Add(label);
		///     return grid;
		/// });
		/// ]]></code>
		/// ***
		/// </example>
		public DataTemplate UpButtonTemplate
		{
			get { return (DataTemplate)GetValue(UpButtonTemplateProperty); }
			set { SetValue(UpButtonTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the placement of buttons used to increment or decrement the <see cref="SfNumericEntry.Value"/> property.
		/// </summary>
		/// <value>
		/// The default value is <see cref="NumericUpDownPlacementMode.Inline"/>.
		/// </value>
		/// <example>
		/// To set the spin buttons to vertical in XAML:
		/// <code Lang="XAML"><![CDATA[
		/// <numericUpDown:SfNumericUpDown
		///     x:Name="numericUpDown"
		///     UpDownPlacementMode="InlineVertical" />
		/// ]]></code>
		/// To set the spin buttons to vertical in C#:
		/// <code Lang="C#"><![CDATA[
		/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
		/// numericUpDown.UpDownPlacementMode = NumericUpDownPlacementMode.InlineVertical;
		/// this.Content = numericUpDown;
		/// ]]></code>
		/// </example>
		public NumericUpDownPlacementMode UpDownPlacementMode
		{
			get { return (NumericUpDownPlacementMode)GetValue(UpDownPlacementModeProperty); }
			set { SetValue(UpDownPlacementModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that is added to or subtracted from <see cref="SfNumericEntry.Value"/> when a small change is made, such as with an arrow key or scrolling.
		/// </summary>
		/// <value>
		/// The default value is <c>1</c>.
		/// </value>
		/// <example>
		/// To set a small change value of 0.1 in XAML:
		/// <code Lang="XAML"><![CDATA[
		/// <numericUpDown:SfNumericUpDown
		///     x:Name="numericUpDown"
		///     SmallChange="0.1" />
		/// ]]></code>
		/// To set a small change value of 0.1 in C#:
		/// <code Lang="C#"><![CDATA[
		/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
		/// numericUpDown.SmallChange = 0.1;
		/// this.Content = numericUpDown;
		/// ]]></code>
		/// </example>
		public double SmallChange
		{
			get { return (double)GetValue(SmallChangeProperty); }
			set { SetValue(SmallChangeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that is added to or subtracted from <see cref="SfNumericEntry.Value"/> when a large change is made, such as with the PageUP and PageDown keys.
		/// </summary>
		/// <value>
		/// The default value is <c>10</c>.
		/// </value>
		/// <example>
		/// To set a large change value of 5 in XAML:
		/// <code Lang="XAML"><![CDATA[
		/// <numericUpDown:SfNumericUpDown
		///     x:Name="numericUpDown"
		///     LargeChange="5" />
		/// ]]></code>
		/// To set a large change value of 5 in C#:
		/// <code Lang="C#"><![CDATA[
		/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
		/// numericUpDown.LargeChange = 5;
		/// this.Content = numericUpDown;
		/// ]]></code>
		/// </example>
		public double LargeChange
		{
			get { return (double)GetValue(LargeChangeProperty); }
			set { SetValue(LargeChangeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the alignment of the buttons used to align increment or decrement the <see cref="SfNumericEntry.Value"/> property.
		/// </summary>
		/// <value> The default value is <c>Right</c>. </value>
		/// <example>
		/// To set a large change value of 5 in XAML:
		/// <code Lang="XAML"><![CDATA[
		/// <numericUpDown:SfNumericUpDown
		///     x:Name="numericUpDown"
		///     UpDownButtonAlignment="Both" />
		/// ]]></code>
		/// To set a large change value of 5 in C#:
		/// <code Lang="C#"><![CDATA[
		/// SfNumericUpDown numericUpDown = new SfNumericUpDown();
		/// numericUpDown.UpDownButtonAlignment = UpDownButtonAlignment.Both;
		/// this.Content = numericUpDown;
		/// ]]></code>
		/// </example>
		/// <remarks>
		/// In <see cref="NumericUpDownPlacementMode.InlineVertical"/> mode, if <see cref="UpDownButtonAlignment"/> is set to <c>Both</c>.
		/// The "UpDown" buttons is aligned to the right side.
		/// </remarks>
		public UpDownButtonAlignment UpDownButtonAlignment
		{
			get { return (UpDownButtonAlignment)GetValue(UpDownButtonAlignmentProperty); }
			set { SetValue(UpDownButtonAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the up and down buttons when they are disabled in the <see cref="NumericUpDown"/> control.
		/// </summary>
		internal Color UpDownButtonDisableColor
		{
			get { return (Color)GetValue(UpDownButtonDisableColorProperty); }
			set { SetValue(UpDownButtonDisableColorProperty, value); }
		}

		#endregion

		#region Property Changed

		/// <summary>
		/// Occurs when Autoreverse property is changed.
		/// </summary>
		static void OnAutoReversePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericUpDown numericUpDown)
			{
				numericUpDown.UpdateButtonColor(numericUpDown.Value);
			}
		}

		/// <summary>
		/// Occurs when spin button placement property is changed.
		/// </summary>
		static void OnSpinButtonPlacementChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericUpDown numericUpDown)
			{
				SfNumericUpDown.UpdateSpinButtonPlacement(numericUpDown);
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="DownButtonTemplate"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnDownButtonTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericUpDown numericUpDown)
			{
				numericUpDown.AddDownButtonTemplate((DataTemplate)newValue);
				numericUpDown.GetMinimumSize();
				numericUpDown.InvalidateDrawable();
				numericUpDown.UpdateTextInputLayoutUI();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="UpButtonTemplate"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnUpButtonTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericUpDown numericUpDown)
			{
				numericUpDown.AddUpButtonTemplate((DataTemplate)newValue);
				numericUpDown.GetMinimumSize();
				numericUpDown.InvalidateDrawable();
				numericUpDown.UpdateTextInputLayoutUI();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="UpDownButtonColorProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnUpDownButtonColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericUpDown numericUpDown)
			{
				numericUpDown.UpdateButtonColor(numericUpDown.Value);
				numericUpDown.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoke when UpDownButtonAlignment property is changed.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private static void OnUpDownButtonAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericUpDown numericUpDown)
			{
				SfNumericUpDown.UpdateSpinButtonPlacement(numericUpDown);
				numericUpDown.InvalidateDrawable();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the current touch point's position based on the provided touch event data.
		/// Adjusts the touch point for RTL (right-to-left) layouts if necessary.
		/// </summary>
		/// <param name="touch">The touch point on the screen.</param>
		void UpdateTouchPoint(Point touch)
		{
#if WINDOWS
            _touchPoint = IsRTL() ? new Point(Width - touch.X, touch.Y) : touch;
#else
			_touchPoint = touch;
#endif
		}

		/// <summary>
		/// Handles touch events for the control.
		/// </summary>
		/// <param name="e">The event data for the touch event, containing information about the pointer's position, state, and other relevant details.</param>
		public override void OnTouch(PointerEventArgs e)
		{
			if (IsTextInputLayout)
			{ return; }
			UpdateTouchPoint(e.TouchPoint);

			switch (e.Action)
			{
				case PointerActions.Pressed:
					HandlePressed(e);
					break;
				case PointerActions.Released:
				case PointerActions.Cancelled:
				case PointerActions.Exited:
					HandleReleasedOrCancelled(e);
					break;
			}
		}

		/// <summary>
		/// Handles the logic when a touch press is detected. This includes handling
		/// button presses and starting a long-press timer if applicable.
		/// </summary>
		/// <param name="e">The event arguments containing details about the touch press interaction.</param>
		void HandlePressed(PointerEventArgs e)
		{
#if WINDOWS
            if (_textBox != null && _textBox.IsFocused)
            {
                _isFocus = true;
            }
#endif
			HandleButtonPress(e);
		}

		/// <summary>
		/// Applying measure updates for button visibility based on spins and up or down placement settings.
		/// </summary>
		static void UpdateSpinButtonPlacement(SfNumericUpDown numericUpDown)
		{
			numericUpDown.AddUpButtonTemplate(numericUpDown.UpButtonTemplate);
			numericUpDown.AddDownButtonTemplate(numericUpDown.DownButtonTemplate);
			VisualStateManager.GoToState(numericUpDown, numericUpDown.UpDownPlacementMode.ToString());
			numericUpDown.UpdateEntryVisibility();
			numericUpDown.InvalidateMeasure();
			numericUpDown.UpdateTextInputLayoutUI();
			numericUpDown.GetMinimumSize();
		}

		/// <summary>
		/// Configures the layout for the up button, setting its position and size within the control.
		/// </summary>
		void SetUpButtonLayout()
		{
			if (UpButtonTemplate == null || _upButtonView == null)
			{
				return;
			}

			float xPosition;

			if (UpDownPlacementMode == NumericUpDownPlacementMode.InlineVertical)
			{
				xPosition = IsRTL() ? _tempUpDownX : _upButtonRectF.X;
				_upButtonView.Measure(_upButtonRectF.Width, _upButtonRectF.Height);
				AbsoluteLayout.SetLayoutBounds(_upButtonView, new RectF(xPosition, _upButtonRectF.Y, _upButtonRectF.Width, _upButtonRectF.Height));
			}
			else
			{
				xPosition = IsRTL()
					? (UpDownPlacementMode == NumericUpDownPlacementMode.Inline ? UpDownButtonAlignment == UpDownButtonAlignment.Left
												? _tempUpDownX :
												_tempUpDownX - ButtonSize : _tempUpDownX)
					: _downButtonRectF.X;

				if (UpDownButtonAlignment==UpDownButtonAlignment.Both && IsRTL())
				{
					xPosition = 4;
				}
				_upButtonView.Measure(_downButtonRectF.Width, _downButtonRectF.Height);
				AbsoluteLayout.SetLayoutBounds(_upButtonView, new RectF(xPosition, _downButtonRectF.Y, _downButtonRectF.Width, _downButtonRectF.Height));
			}
		}

		/// <summary>
		/// Configures the layout for the down button, setting its position and size within the control.
		/// </summary>
		void SetDownButtonLayout()
		{
			if (DownButtonTemplate == null || _downButtonView == null)
			{
				return;
			}

			float xPosition;

			if (IsInlineVerticalPlacement())
			{
				_downButtonView.Measure(_downButtonRectF.Width, _downButtonRectF.Height);

				xPosition = IsRTL()
					? (IsInlinePlacement() ? _tempUpDownX - ButtonSize : _tempUpDownX)
					: _downButtonRectF.X;

				AbsoluteLayout.SetLayoutBounds(_downButtonView, new RectF(xPosition, _downButtonRectF.Y, _downButtonRectF.Width, _downButtonRectF.Height));
			}
			else
			{
				_downButtonView.Measure(_upButtonRectF.Width, _upButtonRectF.Height);

				xPosition = IsRTL() ? UpDownButtonAlignment == UpDownButtonAlignment.Left
												? _tempUpDownX + ButtonSize : _tempUpDownX : _upButtonRectF.X;
				AbsoluteLayout.SetLayoutBounds(_downButtonView, new RectF(xPosition, _upButtonRectF.Y, _upButtonRectF.Width, _upButtonRectF.Height));
			}
		}

		/// <summary>
		/// Draws the up and down buttons on the provided canvas.
		/// </summary>
		/// <param name="canvas">The canvas on which the buttons are drawn.</param>
		void DrawButtons(ICanvas canvas)
		{
			bool customUpButton = UpButtonTemplate != null && _upButtonView != null;
			bool customDownButton = DownButtonTemplate != null && _downButtonView != null;

			// Set layout for the up button or draw default up button
			if (customUpButton)
			{
				SetUpButtonLayout();
			}
			else
			{
				RemoveExistingUporDownButtonView(_upButtonView);
				DrawUpButton(canvas);
			}

			// Set layout for the down button or draw default down button
			if (customDownButton)
			{
				SetDownButtonLayout();
			}
			else
			{
				RemoveExistingUporDownButtonView(_downButtonView);
				DrawDownButton(canvas);
			}
		}

		/// <summary>
		/// Draws the up button on the specified canvas.
		/// </summary>
		/// <param name="canvas">The canvas on which the up button is drawn.</param>
		void DrawUpButton(ICanvas canvas)
		{
			var upButton = IsInlineVerticalPlacement() ? _upButtonRectF : _downButtonRectF;
			canvas.StrokeColor = _upButtonColor;
			canvas.FillColor = _upButtonColor;
			_entryRenderer?.DrawUpButton(canvas, upButton);
		}

		/// <summary>
		/// Draws the down button on the specified canvas.
		/// </summary>
		/// <param name="canvas">The canvas on which the down button is drawn.</param>
		void DrawDownButton(ICanvas canvas)
		{
			var downButton = IsInlineVerticalPlacement() ? _downButtonRectF : _upButtonRectF;
			canvas.StrokeColor = _downButtonColor;
			canvas.FillColor = _downButtonColor;
			_entryRenderer?.DrawDownButton(canvas, downButton);
		}

		/// <summary>
		/// Draws the entry user interface elements within the specified rectangle on the given canvas.
		/// </summary>
		/// <param name="canvas">The canvas where the entry UI elements are drawn.</param>
		/// <param name="dirtyRect">The rectangle defining the area to be updated and redrawn.</param>
		internal override void DrawEntryUI(ICanvas canvas, Rect dirtyRect)
		{
			if (_textBox == null || IsTextInputLayout)
			{
				return;
			}

			if (_entryRenderer != null)
			{
				DrawButtons(canvas);
			}
			base.DrawEntryUI(canvas, dirtyRect);
		}

		/// <summary>
		/// Updates the color of the button based on the provided value.
		/// </summary>
		/// <param name="value">A nullable double that determines the color change. 
		/// If null, the button may revert to a default color.</param>
		internal override void UpdateButtonColor(double? value)
		{
			if (value == Minimum)
			{
				ChangeButtonColors(AutoReverse ? UpDownButtonColor : UpDownButtonDisableColor, UpDownButtonColor, ValueStates.Minimum);
			}
			else if (value == Maximum)
			{
				ChangeButtonColors(UpDownButtonColor, AutoReverse ? UpDownButtonColor : UpDownButtonDisableColor, ValueStates.Maximum);
			}
			else
			{
				ChangeButtonColors(UpDownButtonColor, UpDownButtonColor, ValueStates.Normal);
			}
			UpdateTextInputLayoutUI();
			UpdateEffectsRendererBounds();
			InvalidateDrawable();
		}

		/// <summary>
		/// Updates the bounds of the UI elements based on the specified rectangle.
		/// </summary>
		/// <param name="bounds">The rectangle defining the new bounds for the elements.</param>
		/// <param name="isOverride">A boolean indicating whether to override existing bounds. 
		/// Defaults to false, meaning existing bounds will be preserved unless specified.</param>
		internal override void UpdateElementsBounds(RectF bounds, bool isOverride = false)
		{
			if (!IsTextInputLayout)
			{
				switch (UpDownPlacementMode)
				{
					case NumericUpDownPlacementMode.Inline:
						ConfigureInlineButtonPositions(bounds);
						break;
					case NumericUpDownPlacementMode.InlineVertical:
						ConfigureVerticalButtonPositions(bounds);
						break;
				}
			}
			ConfigureClearButton(bounds);
			SetTextBoxMargin();
			_numericUpDownSemanticsNodes.Clear();
			InvalidateSemantics();
			base.UpdateElementsBounds(bounds, true);
		}

		/// <summary>
		/// Sets the margin for the text box within the numeric entry control based on its layout requirements.
		/// </summary>
		void SetTextBoxMargin()
		{
			if (_textBox != null)
			{
				_textBox.ButtonSize = ButtonSize + 4;
				_textBox.Margin = GetMarginBasedOnTextAlignment(_leftMargin, 0, _rightMargin, 0);
			}
		}

		/// <summary>
		/// Updates the size of the buttons used in the control, adjusting them to the specified size.
		/// </summary>
		/// <param name="size">The new size to which the buttons should be set, typically in device-independent units.</param>
		void UpdateButtonSize(float size)
		{
			_upButtonRectF.Width = _downButtonRectF.Width = size;
			_upButtonRectF.Height = _downButtonRectF.Height = size;
		}

		/// <summary>
		/// Gets the size of the up/down button based on its placement and availability.
		/// </summary>
		/// <returns>The size of the up/down button, depending on whether it is placed inline vertically.</returns>
		float GetUpDownButtonSize()
		{
			if(_upButtonView != null && _downButtonView != null && IsInlineVerticalPlacement())
			{
				return VerticalUpDownButtonSize;
			}
			return UpDownButtonSize;
		}

		/// <summary>
		/// Configures the vertical positioning of buttons within the given bounding rectangle.
		/// </summary>
		/// <param name="bounds">The bounding rectangle that defines the available space for button placement.</param>
		void ConfigureVerticalButtonPositions(RectF bounds)
		{
			float xOffset = bounds.X + bounds.Width - GetUpDownButtonSize() - ButtonPadding;
#if ANDROID
    xOffset -= 4;
#endif

			_upButtonRectF.X = IsRTL() ? 4 : xOffset;
			_downButtonRectF.X = _upButtonRectF.X;
#if !ANDROID
			if (IsRTL())
			{
				_downButtonRectF.X = _upButtonRectF.X = 0;
			}
#endif
			if (IsRTL())
			{
				_tempUpDownX = xOffset;
			}

			if (UpDownButtonAlignment==UpDownButtonAlignment.Left)
			{
				_upButtonRectF.X = IsRTL() ? xOffset : 4;
				_downButtonRectF.X = _upButtonRectF.X;
				if (IsRTL())
				{
					_tempUpDownX = 4;
				}
			}
			if (_upButtonView == null || _downButtonView == null)
			{
				_upButtonRectF.Y = bounds.Center.Y - (UpDownButtonSize * 0.75f) - ButtonPadding;
				_downButtonRectF.Y = bounds.Center.Y - (UpDownButtonSize * 0.25f) + ButtonPadding;
			}
			else
			{
				_upButtonRectF.Y = bounds.Center.Y - GetUpDownButtonSize()-ButtonPadding;
				_downButtonRectF.Y = _upButtonRectF.Bottom;
			}

			UpdateButtonSize(GetUpDownButtonSize());
		}

		/// <summary>
		/// Configures the inline (horizontal) positioning of buttons within the given bounding rectangle.
		/// </summary>
		/// <param name="bounds">The bounding rectangle that defines the available space for button placement.</param>
		void ConfigureInlineButtonPositions(RectF bounds)
		{
			float xOffset = bounds.X + bounds.Width - ButtonSize;
#if ANDROID
    xOffset -= 4;
#endif

			_upButtonRectF.X = IsRTL() ? 4 : xOffset;
			_downButtonRectF.X = IsRTL() ? _upButtonRectF.X + ButtonSize : _upButtonRectF.X - ButtonSize;
			_upButtonRectF.Y = _downButtonRectF.Y = bounds.Center.Y - (ButtonSize / 2);

			switch (UpDownButtonAlignment)
			{
				case UpDownButtonAlignment.Left:
					_downButtonRectF.X = IsRTL() ? xOffset : 4;
					_upButtonRectF.X = IsRTL() ? _downButtonRectF.X - ButtonSize : _downButtonRectF.X + ButtonSize;
					if (IsRTL())
					{
						_tempUpDownX = 4 ;
					}
					break;
				case UpDownButtonAlignment.Right:
					_upButtonRectF.X = IsRTL() ? 4 : xOffset;
					_downButtonRectF.X = IsRTL() ? _upButtonRectF.X + ButtonSize : _upButtonRectF.X - ButtonSize;
					if (IsRTL())
					{
						_tempUpDownX = xOffset;
					}
					break;
				case UpDownButtonAlignment.Both:
					_downButtonRectF.X = IsRTL() ? xOffset : 4;
					_upButtonRectF.X = IsRTL() ? 4 : xOffset;
					if (IsRTL())
					{
						_tempUpDownX = xOffset;
					}
					break;
			}
			
			UpdateButtonSize(GetUpDownButtonSize());
		}

		/// <summary>
		/// Updates the bounds of the effects renderer to ensure proper rendering of visual effects.
		/// </summary>
		internal override void UpdateEffectsRendererBounds()
		{
			base.UpdateEffectsRendererBounds();
			if (_valueStates != ValueStates.Minimum)
			{
				UpdateEffectsBounds( IsInlinePlacement() ?  _upButtonRectF : _downButtonRectF);
			}

			if (_valueStates != ValueStates.Maximum)
			{
				UpdateEffectsBounds(IsInlinePlacement() ? _downButtonRectF : _upButtonRectF);
			}
		}

		/// <summary>
		/// Changes the colors of the up and down buttons based on the specified colors and state.
		/// </summary>
		/// <param name="downColor">The color to apply to the down button.</param>
		/// <param name="upColor">The color to apply to the up button.</param>
		/// <param name="state">The current state of the buttons which might dictate how colors are applied (e.g., default, pressed).</param>
		void ChangeButtonColors(Color downColor, Color upColor, ValueStates state)
		{
			_downButtonColor = downColor;
			_upButtonColor = upColor;
			_valueStates = state;
		}

		/// <summary>
		/// Checks whether the current semantic data is still valid and does not need rebuilding.
		/// </summary>
		/// <returns>True if the semantic data is current, otherwise false.</returns>
		bool SemanticsDataIsCurrent()
		{
			return _numericUpDownSemanticsNodes.Count != 0 &&
				   (_clearButtonSemanticsSize == new Size(_clearButtonRectF.Width, _clearButtonRectF.Height) ||
					_downButtonSemanticsSize == new Size(_downButtonRectF.Width, _downButtonRectF.Height) ||
					_upButtonSemanticsSize == new Size(_upButtonRectF.Width, _upButtonRectF.Height));
		}

		/// <summary>
		/// Method called when the control's parent changes, allowing for adjustments related to the change in hierarchy or parent container.
		/// </summary>
		protected override void OnParentChanged()
		{
			base.OnParentChanged();
			OnNumericEntryParentChanged();
			_numericUpDownSemanticsNodes.Clear();
			InvalidateSemantics();
		}

		/// <summary>
		/// Updates the semantic sizes for the clear, down, and up buttons
		/// based on their current rectangle dimensions.
		/// </summary>
		void UpdateSemanticsSizes()
		{
			_clearButtonSemanticsSize = new Size(_clearButtonRectF.Width, _clearButtonRectF.Height);
			_downButtonSemanticsSize = new Size(_downButtonRectF.Width, _downButtonRectF.Height);
			_upButtonSemanticsSize = new Size(_upButtonRectF.Width, _upButtonRectF.Height);
		}

		/// <summary>
		/// Adds a semantics node for a given element in the control.
		/// </summary>
		/// <param name="bounds">The rectangular bounds of the element.</param>
		/// <param name="id">The ID of the semantics node.</param>
		/// <param name="description">The text description for the node, used for accessibility.</param>
		/// <param name="state">The state to identify the updown button is enabled or disabled.</param>
		void AddSemanticsNode(RectF bounds, int id, string description, bool state=true)
		{
			SemanticsNode node = new SemanticsNode
			{
				Id = id,
				Bounds = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height),
				Text = state ? $"{description}, double tap to activate" : $"{description}, disabled"
			};
			_numericUpDownSemanticsNodes.Add(node);
		}

		/// <summary>
		/// Retrieves the list of semantics nodes for accessibility support,
		/// which represent how the control is perceived by assistive technologies.
		/// </summary>
		/// <param name="width">The width constraint for the semantics nodes.</param>
		/// <param name="height">The height constraint for the semantics nodes.</param>
		/// <returns>A list of semantics nodes that describe the accessible elements within this control.</returns>
		protected override List<SemanticsNode> GetSemanticsNodesCore(double width, double height)
		{
			UpdateSemanticsSizes();

			if (SemanticsDataIsCurrent() || IsTextInputLayout)
			{
				if (IsTextInputLayout)
				{
					_numericUpDownSemanticsNodes.Clear();
				}
				return _numericUpDownSemanticsNodes;
			}
			var upbuttonstate = !(_valueStates == ValueStates.Maximum) || AutoReverse;
			var downbuttonstate = !(_valueStates == ValueStates.Minimum) || AutoReverse;
			var upButtonRectF = IsInlinePlacement() ? _downButtonRectF : _upButtonRectF;
			var downButtonRectF = IsInlinePlacement() ? _upButtonRectF : _downButtonRectF;
			if ((IsInlineVerticalPlacement() && !(UpDownButtonAlignment == UpDownButtonAlignment.Left)) || (!IsInlineVerticalPlacement() && UpDownButtonAlignment == UpDownButtonAlignment.Right))
			{
				if (_isClearButtonVisible)
				{
				AddSemanticsNode(_clearButtonRectF, 1, "Clear button");
			}

				AddSemanticsNode(upButtonRectF, 2, "Up button", upbuttonstate);
				AddSemanticsNode(downButtonRectF, 3, "Down button", downbuttonstate);
			}
			else if (UpDownButtonAlignment == UpDownButtonAlignment.Left)
			{
				AddSemanticsNode(upButtonRectF, 1, "Up button", upbuttonstate);
				AddSemanticsNode(downButtonRectF, 2, "Down button", downbuttonstate);

				if (_isClearButtonVisible)
				{
					AddSemanticsNode(_clearButtonRectF, 3, "Clear button");
			}
			}
			else
			{
				AddSemanticsNode(upButtonRectF, 1, "Up button", upbuttonstate);

				if (_isClearButtonVisible)
				{
					AddSemanticsNode(_clearButtonRectF, 2, "Clear button");
			}

				AddSemanticsNode(downButtonRectF, 3, "Down button", downbuttonstate);
			}

			return _numericUpDownSemanticsNodes;
		}

		/// <summary>
		/// Measures the content of the control to determine the desired size based on the given constraints.
		/// </summary>
		/// <param name="widthConstraint">The maximum width available for the content.</param>
		/// <param name="heightConstraint">The maximum height available for the content.</param>
		/// <returns>The desired size for the content, which should be within the given constraints.</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			// Default to an empty size
			Size measure = Size.Zero;

			// Measure the textbox if it exists
			if (_textBox != null)
			{
				measure = _textBox.Measure(widthConstraint, heightConstraint);
			}

			// Adjust width for inline button placement
			if (UpDownPlacementMode == NumericUpDownPlacementMode.Inline)
			{
				measure.Width += 2 * ButtonSize;
			}

			// Calculate measured width considering constraints
			double measuredWidth = double.IsInfinity(widthConstraint) ? measure.Width : Math.Min(measure.Width, widthConstraint);

			// Calculate measured height considering constraints
			double measuredHeight = double.IsInfinity(heightConstraint) ? measure.Height : Math.Min(measure.Height, heightConstraint);

			// Ensure minimum height if EntryVisibility is Collapsed
			if (EntryVisibility == Visibility.Collapsed)
			{
				measuredHeight = Math.Max(measuredHeight, ButtonSize);
			}

			return new Size(measuredWidth, measuredHeight);
		}

		internal override void GetMinimumSize()
		{
			if (IsTextInputLayout)
			{
				return;
			}

			// Calculate minimum height based on placement mode
			MinimumHeightRequest = UpDownPlacementMode == NumericUpDownPlacementMode.InlineVertical
				? SfNumericUpDown.DetermineMinimumHeightForVerticalPlacement(this)
				: SfNumericUpDown.DetermineMinimumHeightForInlinePlacement();

			// Set minimum width request commonly for both placement modes
			MinimumWidthRequest = 2 * ButtonSize;
		}

		static double DetermineMinimumHeightForVerticalPlacement(SfNumericUpDown numericUpDown)
		{
			// Use platform-specific directives only if necessary
			bool isVerticalTemplate = numericUpDown._upButtonView != null && numericUpDown._downButtonView != null;
			if (!isVerticalTemplate)
			{
#if !ANDROID
				return (2 * UpDownButtonSize) - (UpDownButtonSize / 3);
#else
				return 2 * UpDownButtonSize;
#endif
			}
			else
			{
#if !ANDROID
				return (2 * VerticalUpDownButtonSize) + ButtonPadding;
#else
				return (2 * VerticalUpDownButtonSize) + AndroidButtonHeightPadding + ButtonPadding;
#endif
			}
		}

		static double DetermineMinimumHeightForInlinePlacement()
		{
			// For inline placement, return height based on button size
			return ButtonSize;
		}

		internal override void UpdateTextInputLayoutUI()
		{
			base.UpdateTextInputLayoutUI();
			if (_textInputLayout != null)
			{
				_textInputLayout.ShowUpDownButton = true;
				_textInputLayout.IsUpDownVerticalAlignment = (UpDownPlacementMode == NumericUpDownPlacementMode.InlineVertical);
				_textInputLayout.IsUpDownAlignmentLeft = (UpDownButtonAlignment == UpDownButtonAlignment.Left);
				_textInputLayout.IsUpDownAlignmentBoth = (UpDownButtonAlignment == UpDownButtonAlignment.Both);
				OnNumericEntryParentChanged();
				_textInputLayout.DownButtonColor = _downButtonColor;
				_textInputLayout.UpButtonColor = _upButtonColor;
				_textInputLayout.UpIconTemplate = _upButtonView;
				_textInputLayout.DownIconTemplate = _downButtonView;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the placement mode is set to Inline.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the placement mode is Inline; otherwise, <c>false</c>.
		/// </returns>
		bool IsInlinePlacement()
		{
			return UpDownPlacementMode == NumericUpDownPlacementMode.Inline;
		}

		/// <summary>
		/// Gets a value indicating whether the placement mode is set to InlineVertical.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the placement mode is InlineVertical; otherwise, <c>false</c>.
		/// </returns>
		internal bool IsInlineVerticalPlacement()
		{
			return UpDownPlacementMode == NumericUpDownPlacementMode.InlineVertical;
		}

		/// <summary>
		/// Update the textbox visibility based on EntryVisibility property.
		/// </summary>
		void UpdateEntryVisibility()
		{
			if (_textBox == null)
			{
				return;
			}

			switch (EntryVisibility)
			{
				case Visibility.Collapsed:
					UpdateEntryVisualStates(false, "EntryCollapsed");
					break;
				case Visibility.Hidden:
					UpdateEntryVisualStates(false, "EntryHidden");
					break;
				default:
					UpdateEntryVisualStates(true, "EntryVisible");
					break;
			}
		}

		///<inheritdoc/>   
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfNumericUpDownStyles();
		}

		///<inheritdoc/>     
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
			SetDynamicResource(UpDownButtonDisableColorProperty, "SfNumericUpDownDisabledArrowColor");
		}

		///<inheritdoc/>   
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		/// <summary>
		/// Initializes or reinitializes the cancellation token source for long press operations.
		/// </summary>
		void InitializeTokenSource()
		{
			// Cancel any ongoing operation
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
		}

		/// <summary>
		/// Start the long press timer
		/// </summary>
		async void StartPressTimer()
		{
			InitializeTokenSource();

			_isPressOccurring = true;
			// If the delay was completed and not cancelled
			if (await IsLongPressActivate(StartDelay))
			{
				await RecursivePressedAsync(this);
		}
		}

		/// <summary>
		/// Determines if a long press has been activated based on a specified delay.
		/// </summary>
		/// <param name="delay">The duration in milliseconds to wait before considering the press as a long press.</param>
		/// <returns>True if the long press is activated, false otherwise.</returns>
		private async Task<bool> IsLongPressActivate(int delay)
		{
			if (_cancellationTokenSource == null)
			{
				return false;
			}

			try
			{
				var delayTask = Task.Delay(delay, _cancellationTokenSource.Token);
				var completedTask = await Task.WhenAny(delayTask, Task.Delay(Timeout.Infinite));
				return delayTask == completedTask && _isPressOccurring;
			}
			catch (Exception)
			{
				// The task was canceled, which means the press was released before the delay completed
				return false;
			}
		}

		/// <summary>
		/// Recursively call the increment and decrement value
		/// </summary>
		/// <param name="numericUpdown">The numericupdown instance</param>
		/// <returns></returns>
		static async Task RecursivePressedAsync(SfNumericUpDown numericUpdown)
    	{
			numericUpdown.HandleUpDownPressed();
			numericUpdown.InitializeTokenSource();

			// Wait for the long press duration without throwing TaskCanceledException
			// If the delay was completed and not cancelled
			if (await numericUpdown.IsLongPressActivate(ContinueDelay))
			{
        	await SfNumericUpDown.RecursivePressedAsync(numericUpdown);
    	}
		}

		/// <summary>
		/// Method that handles the logic when the clear button is tapped.
		/// </summary>
		void OnClearButtonTouchUp()
		{
			UpdateDisplayText(null);
			if (IsDescriptionNotSetByUser)
			{
				SemanticProperties.SetDescription(this, "Clear button pressed");
			}
			SemanticScreenReader.Announce(SemanticProperties.GetDescription(this));
			_numericUpDownSemanticsNodes.Clear();
			InvalidateSemantics();
		}

		/// <summary>
		/// Handles the logic when a touch release or cancellation is detected, stopping
		/// the long-press timer and triggering release-related actions.
		/// </summary>
		/// <param name="e">The event arguments detailing the touch release or cancel interaction.</param>
		void HandleReleasedOrCancelled(PointerEventArgs e)
		{
			StopPressTimer();
			if (e.Action == PointerActions.Released)
			{
				if (ClearButtonClicked(_touchPoint))
				{
					OnClearButtonTouchUp();
				}
				ClearHighlightIfNecessary();
			}
		}

		/// <summary>
		/// Stops the long press timer if it is not null.
		/// </summary>
		private void StopPressTimer()
		{
			_isPressOccurring = false;
			_cancellationTokenSource?.Cancel();
		}

		/// <summary>
		/// Checks whether to execute increase command.
		/// </summary>
		bool CanIncrease()
		{
			return Value == null || double.IsNaN((double)Value) || Value < Maximum || AutoReverse;

		}

		/// <summary>
		/// Increase the value by small change value.
		/// </summary>
		void Increase()
		{
			if (_textBox == null)
			{
				return;
			}
			if (_textBox.IsFocused == false)
			{
				// When control is unfocused, the display text is formatted and may contain symbols as well as a restricted number of fraction digits.
				// As a result, we should increase the value rather than the display text;
				// otherwise, the fraction values would be lost, and we will be unable to convert text into value.
				IncreaseValue((decimal)SmallChange);
			}
			else
			{
				// When a control has focus, the display text is unformatted and it contains only numeric values,
				// also its value may differ from the value property.As a result, we should increase the current display text value.
				IncreaseDisplayText((decimal)SmallChange);
				UpdateValue();
			}
		}

		/// <summary>
		/// Handles the logic when the up button is pressed, triggering an increase in value
		/// if applicable, and updating semantic properties.
		/// </summary>
		/// <param name="e">The event arguments for the up button press interaction.</param>
		void OnUpButtonPressed(PointerEventArgs e)
		{
			if (_textBox is not null && CanIncrease())
			{
				AnnounceButtonPress("Up button pressed");
				Increase();
			}
		}

		/// <summary>
		/// Applies auto-reverse logic on the given value.
		/// </summary>
		/// <param name="value">The value to adjust.</param>   
		void ApplyAutoReverse(ref decimal value)
		{
			if (AutoReverse)
			{
				if (Maximum != double.MaxValue && value > (decimal)Maximum)
				{
					value = (decimal)Minimum;
				}
				else if (Minimum != double.MinValue && value < (decimal)Minimum)
				{
					value = (decimal)Maximum;
				}
			}
		}

		/// <summary>
		/// Increase the display text value by given value.
		/// </summary>
		void IncreaseDisplayText(decimal delta)
		{
			if (_textBox != null)
			{
				decimal value;
				if (Culture != null)
				{
					value = decimal.TryParse(_textBox.Text, NumberStyles.Number, Culture, out value) ? value : 0;
				}
				else
				{
					value = decimal.TryParse(_textBox.Text, out value) ? value : 0;
				}

				value += delta;
				ApplyAutoReverse(ref value);
				_textBox.Text = RestrictFractionDigit(value.ToString(GetNumberFormat()));
				_textBox.CursorPosition = _textBox.Text.Length;
			}
		}

		/// <summary>
		/// To get the NumberFormat from number formatter, culture property or current UI culture.
		/// </summary>
		NumberFormatInfo GetNumberFormat()
		{
			var applyCulture = Culture ?? CultureInfo.CurrentUICulture;
			// Use custom format culture if specified
			if (CustomFormat != null && Culture == null)
			{
				applyCulture = CultureInfo.CurrentUICulture;
			}

			return applyCulture.NumberFormat;
		}

		/// <summary>
		/// Increase the value when clicking repeat button if control is unfocused.
		/// </summary>
		void IncreaseValue(decimal delta)
		{
			decimal value = decimal.TryParse(Value.ToString(), out value) ? value : 0;
			value += delta;
			ApplyAutoReverse(ref value);
			SetValue(ValueProperty, Convert.ToDouble(value));
		}

		internal override bool HandleKeyPressed(KeyboardKey key)
		{
			if ((key == KeyboardKey.Up && CanIncrease()) ||
					(key == KeyboardKey.Down && CanDecrease()))
			{
				var delta = key == KeyboardKey.Up ? SmallChange : -SmallChange;
				IncreaseDisplayText((decimal)delta);
				UpdateValue();
			}

			return true;
		}

		internal override bool HandleKeyPagePressed(KeyboardKey key)
		{
			if ((key == KeyboardKey.PageUp && CanIncrease()) ||
								(key == KeyboardKey.PageDown && CanDecrease()))
			{
				var delta = key == KeyboardKey.PageUp ? LargeChange : -LargeChange;
				IncreaseDisplayText((decimal)delta);
				UpdateValue();
			}

			return true;

		}

		/// <summary>
		/// Checks whether to execute decrease command.
		/// </summary>
		bool CanDecrease()
		{
			return Value == null || double.IsNaN((double)Value) || Value > Minimum || AutoReverse;
		}

		/// <summary>
		/// Handles the event when the down button is pressed.
		/// </summary>
		/// <param name="e">The pointer event arguments.</param>
		void OnDownButtonPressed(PointerEventArgs e)
		{
			if (_textBox != null && CanDecrease())
			{
				AnnounceButtonPress("Down button pressed");
				Decrease();
			}
		}

		/// <summary>
		/// Announces a button press using the semantic screen reader.
		/// </summary>
		/// <param name="message">The message to announce.</param>
		void AnnounceButtonPress(string message)
		{
			if (IsDescriptionNotSetByUser)
			{
				SemanticProperties.SetDescription(this, message);
			}
			SemanticScreenReader.Announce(SemanticProperties.GetDescription(this));
		}

		/// <summary>
		/// Decrease the value by small change value.
		/// </summary>
		void Decrease()
		{
			if (_textBox == null)
			{
				return;
			}
			if (_textBox.IsFocused == false)
			{
				// When a control has unfocused, First the value should be decreased then the display text value will be updated.
				IncreaseValue((decimal)-SmallChange);
			}
			else
			{
				// When a control has focus, First the display text value should be decreased then the value will be updated.
				IncreaseDisplayText((decimal)-SmallChange);
				UpdateValue();
			}
		}

#if WINDOWS

		/// <summary>
		/// Handles changes to the mouse wheel pointer, determining whether to initiate
		/// an increase or decrease in the numeric value based on the wheel movement.
		/// </summary>
		/// <param name="sender">The source of the event, typically the control itself.</param>
		/// <param name="e">Event arguments providing data for the pointer wheel event.</param>
		void OnPointerWheelChanged(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (e == null || _textBox == null || !_textBox.IsFocused)
			{
				return;
			}

			e.Handled = true;
			double delta = e.GetCurrentPoint(_textBox.ToPlatform()).Properties.MouseWheelDelta;
			
			ProcessWheelDelta(delta);
		}

		/// <summary>
		/// Determines the direction of the wheel delta and calls the appropriate method to increase or decrease the numeric value.
		/// </summary>
		/// <param name="delta">The amount of wheel movement. Positive values indicate upward movement; negative values indicate downward movement.</param>
		void ProcessWheelDelta(double delta)
		{
			if (delta > 0)
			{
				AttemptIncrease();
			}
			else if (delta < 0)
			{
				AttemptDecrease();
			}
		}


		/// <summary>
		/// Attempts to increase the numeric value by a small change amount.
		/// </summary>
		void AttemptIncrease()
		{
			if (CanIncrease())
			{
				IncreaseDisplayText((decimal)SmallChange);
				UpdateValue();
			}
		}


		/// <summary>
		/// Attempts to decrease the numeric value by a small change amount.
		/// </summary>
		void AttemptDecrease()
		{
			if (CanDecrease())
			{
				IncreaseDisplayText((decimal)-SmallChange);
				UpdateValue();
			}
		}
#endif

		/// <summary>
		/// Handles changes to the TextBox control in a .NET MAUI application.
		/// This method is triggered whenever the TextBox's state changes,
		/// allowing for custom behavior in response to those changes.
		/// </summary>
		/// <param name="sender">The source of the event, typically the TextBox.</param>
		/// <param name="e">Event arguments containing data related to the change event.</param>
		protected override void TextBox_HandlerChanged(object? sender, EventArgs e)
		{
			base.TextBox_HandlerChanged(sender, e);
			if (sender is SfEntryView textBox)
			{
#if WINDOWS
				if (textBox.Handler is not null && textBox.Handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox windowTextBox)
				{
					windowTextBox.PointerWheelChanged += OnPointerWheelChanged;
				}
#endif
			}
		}

		/// <summary>
		/// Handles the press event for the long press, typically triggering repeat actions such as incrementing or decrementing a value.
		/// </summary>
		void HandleUpDownPressed()
		{
			if (_textBox is null)
			{
				return;
			}

			bool isDownButtonPressed = _downButtonRectF.Contains(_touchPoint);
			bool isVerticalPlacement = IsInlineVerticalPlacement();

			if (isDownButtonPressed)
			{
				HandleDownButtonPress(isVerticalPlacement);
			}
			else if (_upButtonRectF.Contains(_touchPoint))
			{
				HandleUpButtonPress(isVerticalPlacement);
			}
		}

		/// <summary>
		/// Handles the logic when the down button is pressed, determining whether to increase or decrease the value 
		/// based on the button placement mode.
		/// </summary>
		/// <param name="isVerticalPlacement">A boolean indicating if the button placement is vertical.</param>
		void HandleDownButtonPress(bool isVerticalPlacement)
		{
			if (isVerticalPlacement)
			{
				TryDecrease();
			}
			else
			{
				TryIncrease();
			}
		}

		/// <summary>
		/// Handles the logic when the up button is pressed, determining whether to increase or decrease the value 
		/// based on the button placement mode.
		/// </summary>
		/// <param name="isVerticalPlacement">A boolean indicating if the button placement is vertical.</param>
		void HandleUpButtonPress(bool isVerticalPlacement)
		{
			if (isVerticalPlacement)
			{
				TryIncrease();
			}
			else
			{
				TryDecrease();
			}
		}

		/// <summary>
		/// Attempts to increase the numeric value, checking if the increase action is allowed.
		/// </summary>
		void TryIncrease()
		{
			if (CanIncrease())
			{
				Increase();
			}
		}

		/// <summary>
		/// Attempts to decrease the numeric value, checking if the decrease action is allowed.
		/// </summary>
		void TryDecrease()
		{
			if (CanDecrease())
			{
				Decrease();
			}
		}

		internal override void DownButtonPressed()
		{
			if (_textBox != null && CanDecrease())
			{
				Decrease();
			}
		}

		internal override void UpButtonPressed()
		{
			if (_textBox != null && CanIncrease())
			{
				Increase();
			}
		}

		/// <summary>
		/// Handles button press events, determining the necessary action based on the touch point's location 
		/// and the placement of the buttons.
		/// </summary>
		/// <param name="e">The event data associated with the button press.</param>
		void HandleButtonPress(PointerEventArgs e)
		{
			if (_downButtonRectF.Contains(_touchPoint))
			{
				if (IsInlineVerticalPlacement())
				{
					OnDownButtonPressed(e);
				}
				else
				{
					OnUpButtonPressed(e);
				}
				StartPressTimer();
			}
			else if (_upButtonRectF.Contains(_touchPoint))
			{
				if (IsInlineVerticalPlacement())
				{
					OnUpButtonPressed(e);
				}
				else
				{
					OnDownButtonPressed(e);
				}
				StartPressTimer();
			}
#if !WINDOWS
			else
			{
				Focus();
			}
#endif
		}

		/// <summary>
		/// Updates the visual states of the entry control, setting its visibility based on state name and the specified visibility parameter.
		/// </summary>
		/// <param name="visibility">A boolean indicating whether the entry should be visible.</param>
		/// <param name="stateName">The name of the state to apply to the entry.</param>
		void UpdateEntryVisualStates(bool visibility, string stateName)
		{
			if (_textBox != null)
			{
				VisualStateManager.GoToState(this, stateName);
				_textBox.IsVisible = visibility;
			}
		}

		/// <summary>
		/// Removes the specified button view from the control if it exists within the child elements.
		/// </summary>
		/// <param name="buttonView">The view of the button to be removed.</param>
		void RemoveExistingUporDownButtonView(View? buttonView)
		{
			if (buttonView != null && Children.Contains(buttonView))
			{
				Children.Remove(buttonView);
			}
		}

		internal void OnNumericEntryParentChanged()
		{
			if (IsTextInputLayout && _upButtonView!=null)
			{
				RemoveExistingUporDownButtonView(_upButtonView);
			}
			if (IsTextInputLayout && _downButtonView != null)
			{
				RemoveExistingUporDownButtonView(_downButtonView);
			}
		}

		/// <summary>
		/// Adds a new button view using the provided data template.
		/// </summary>
		/// <param name="buttonView">The button view reference to be set with the newly created view.</param>
		/// <param name="newView">The data template used to create the new button view.</param>
		void AddNewButtonView(ref View? buttonView, DataTemplate newView)
		{
			if (newView == null)
			{
				return;
			}

			object layout = newView.CreateContent();
			buttonView = ExtractViewFromLayout(layout);

			if (buttonView != null)
			{
				SetupViewBinding(buttonView, "IsVisible");
				AutomationProperties.SetIsInAccessibleTree(buttonView, false);
				if (!IsTextInputLayout)
				{
					Add(buttonView);
				}
			}
		}

		/// <summary>
		/// Sets up a binding for the specified view, linking its 'IsVisible' property to a given path in its data context.
		/// </summary>
		/// <param name="view">The view to be bound.</param>
		/// <param name="path">The path of the property in the data context to bind to.</param>
		static void SetupViewBinding(View view, string path)
		{
 			view.SetBinding(SfNumericEntry.IsVisibleProperty, BindingHelper.CreateBinding(nameof(View.IsVisible), getter: static(View view) => view.IsVisible, source:view));
		}

		/// <summary>
		/// Extracts a view from the provided layout content, checking if the content is a ViewCell or a direct View.
		/// </summary>
		/// <param name="layout">The layout content from which to extract the view.</param>
		/// <returns>The extracted View, or null if the content is not a view.</returns>
		static View? ExtractViewFromLayout(object layout)
		{
#if NET9_0
			if (layout is ViewCell viewCell)
			{
				return viewCell.View;
			}

			return layout as View;
#else
			return layout as View;
#endif
		}

		/// <summary>
		/// Adds the up button template to the view.
		/// </summary>
		/// <param name="newView">The new data template for the up button.</param>
		void AddUpButtonTemplate(DataTemplate newView)
		{
			RemoveExistingUporDownButtonView(_upButtonView);
			_upButtonView = null;
			if (UpButtonTemplate != null)
			{
				AddNewButtonView(ref _upButtonView, newView);
			}
		}

		/// <summary>
		/// Adds the down button template to the view.
		/// </summary>
		/// <param name="newView">The new data template for the down button.</param>
		void AddDownButtonTemplate(DataTemplate newView)
		{
			RemoveExistingUporDownButtonView(_downButtonView);
			_downButtonView = null;
			if (DownButtonTemplate != null)
			{
				AddNewButtonView(ref _downButtonView, newView);
			}
		}

		/// <summary>
		/// Configures the positioning and size of the clear button within the given bounding rectangle.
		/// </summary>
		/// <param name="bounds">The bounding rectangle that defines the available space for positioning the clear button.</param>
		void ConfigureClearButton(RectF bounds)
		{
			_leftMargin = 0;
			_rightMargin = 0;
			if (!_isClearButtonVisible)
			{
				UpDateMargin();
				return;
			}

			SetClearButtonPosition(bounds);
			SetClearButtonSize();
			UpdateTextBoxMargin();
		}

		/// <summary>
		/// Updates the margin values for the control based on the placement mode and button alignment.
		/// </summary>
		void UpDateMargin()
		{
			if(IsTextInputLayout)
			{
				return;
			}
			if (UpDownPlacementMode == NumericUpDownPlacementMode.InlineVertical)
			{
				if (UpDownButtonAlignment == UpDownButtonAlignment.Left)
				{
					_leftMargin = ButtonSize;
					_rightMargin = 0;
				}
				else if (UpDownButtonAlignment == UpDownButtonAlignment.Right || UpDownButtonAlignment == UpDownButtonAlignment.Both)
				{
					_rightMargin = ButtonSize;
					_leftMargin = 0;
				}
			}
			else if (UpDownPlacementMode == NumericUpDownPlacementMode.Inline)
			{
				if (UpDownButtonAlignment == UpDownButtonAlignment.Left)
				{
					_leftMargin = 2 * ButtonSize;
					_rightMargin = 0;
				}
				else if (UpDownButtonAlignment == UpDownButtonAlignment.Right)
				{
					_rightMargin = 2 * ButtonSize;
					_leftMargin = 0;
				}
				else
				{
					_leftMargin = ButtonSize;
					_rightMargin = ButtonSize;
				}
			}
			else
			{
				_leftMargin = _rightMargin = 0;
			}
		}

		/// <summary>
		/// Sets the position of the clear button within the control.
		/// </summary>
		/// <param name="bounds">The bounding rectangle defining the control's size and position.</param>
		void SetClearButtonPosition(RectF bounds)
		{
			bool rtl = IsRTL();

			if (UpDownButtonAlignment == UpDownButtonAlignment.Left)
			{
				_clearButtonRectF.X = rtl ? (bounds.X) : (bounds.Width - ButtonSize);
			}
			else if (UpDownButtonAlignment == UpDownButtonAlignment.Right)
			{
				_clearButtonRectF.X = rtl ? (_downButtonRectF.X + ButtonSize) : (_downButtonRectF.X - ButtonSize);
			}
			else
			{
				_clearButtonRectF.X = rtl ? (_upButtonRectF.X + ButtonSize) : (_upButtonRectF.X - ButtonSize);
			}
			_clearButtonRectF.Y = bounds.Center.Y - (ButtonSize / 2);
		}


		/// <summary>
		/// Sets the size of the clear button using the predefined button size.
		/// </summary>
		void SetClearButtonSize()
		{
			_clearButtonRectF.Width = ButtonSize;
			_clearButtonRectF.Height = ButtonSize;
		}

		/// <summary>
		/// Updates the left and right margins of the text box based on the current UpDownButtonAlignment.
		/// </summary>
		void UpdateTextBoxMargin()
		{
			if (IsTextInputLayout)
			{
				return;
			}
			switch (UpDownButtonAlignment)
			{
				case UpDownButtonAlignment.Left:
					_leftMargin = IsInlinePlacement() ? ButtonSize * 2 : ButtonSize;
					_rightMargin = ButtonSize;
					break;
				case UpDownButtonAlignment.Right:
					_rightMargin = IsInlinePlacement() ? ButtonSize * 3 : ButtonSize * 2;
					_leftMargin = 0;
					break;
				case UpDownButtonAlignment.Both:
					_leftMargin = IsInlinePlacement() ? ButtonSize : 0 ;
					_rightMargin = ButtonSize *2;
					break;
			}
		}

#endregion

	}
}
