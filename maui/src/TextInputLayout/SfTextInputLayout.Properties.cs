using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace Syncfusion.Maui.Toolkit.TextInputLayout
{
	public partial class SfTextInputLayout
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="IsLayoutFocused"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsLayoutFocused"/> property indicates whether the layout has focus or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		static readonly BindableProperty IsLayoutFocusedProperty =
		   BindableProperty.Create(
			   nameof(IsLayoutFocused),
			   typeof(bool),
			   typeof(SfTextInputLayout),
			   false,
			   BindingMode.Default,
			   null,
			   OnIsLayoutFocusedChanged);

		/// <summary>
		/// Identifies the <see cref="ContainerType"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ContainerType"/> property determines the appearance of the background and border in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ContainerTypeProperty =
			BindableProperty.Create(
				nameof(ContainerType),
				typeof(ContainerType),
				typeof(SfTextInputLayout),
				ContainerType.Filled,
				BindingMode.Default,
				null,
				OnContainerTypePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LeadingView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="LeadingView"/> property specifies the display of icons or other visual elements in the leading view of  <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty LeadingViewProperty =
			BindableProperty.Create(
				nameof(LeadingView),
				typeof(View),
				typeof(SfTextInputLayout),
				null,
				BindingMode.Default,
				null,
				OnLeadingViewChanged);

		/// <summary>
		/// Identifies the <see cref="TrailingView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="TrailingView"/> property specifies the display of icons or other visual elements in the trailing view of  <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty TrailingViewProperty =
			BindableProperty.Create(
				nameof(TrailingView),
				typeof(View),
				typeof(SfTextInputLayout),
				null,
				BindingMode.Default,
				null,
				OnTrailingViewChanged);

		/// <summary>
		/// Identifies the <see cref="ShowLeadingView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowLeadingView"/> property determines whether the leading view should be displayed in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ShowLeadingViewProperty =
			BindableProperty.Create(
				nameof(ShowLeadingView),
				typeof(bool),
				typeof(SfTextInputLayout),
				true,
				BindingMode.Default,
				null,
				OnShowLeadingViewPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowTrailingView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowTrailingView"/> property determines whether the trailing view should be displayed in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ShowTrailingViewProperty =
			BindableProperty.Create(
				nameof(ShowTrailingView),
				typeof(bool),
				typeof(SfTextInputLayout),
				true,
				BindingMode.Default,
				null,
				OnShowTrailingViewPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowHint"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowHint"/> property determines whether the hint text should be displayed or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ShowHintProperty =
			BindableProperty.Create(
				nameof(ShowHint),
				typeof(bool),
				typeof(SfTextInputLayout),
				true,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowCharCount"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowCharCount"/> property determines whether the character count should be displayed or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindableProperty ShowCharCountProperty =
			BindableProperty.Create(
				nameof(ShowCharCount),
				typeof(bool),
				typeof(SfTextInputLayout),
				false, BindingMode.Default,
				null, OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowHelperText"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowHelperText"/> property determines whether the helper text should be displayed or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ShowHelperTextProperty =
			BindableProperty.Create(
				nameof(ShowHelperText),
				typeof(bool),
				typeof(SfTextInputLayout),
				true,
				BindingMode.TwoWay,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HasError"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HasError"/> property determines whether the error text should be displayed or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty HasErrorProperty =
			BindableProperty.Create(
				nameof(HasError),
				typeof(bool),
				typeof(SfTextInputLayout),
				false,
				BindingMode.Default,
				null,
				OnHasErrorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsHintAlwaysFloated"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsHintAlwaysFloated"/> property determines whether the hint text should always float or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty IsHintAlwaysFloatedProperty =
			BindableProperty.Create(
				nameof(IsHintAlwaysFloated),
				typeof(bool),
				typeof(SfTextInputLayout),
				false,
				BindingMode.Default,
				null,
				OnHintAlwaysFloatedPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Stroke"/> property determines border color of the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(
				nameof(Stroke),
				typeof(Color),
				typeof(SfTextInputLayout),
				_defaultStrokeColor,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ContainerBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ContainerBackground"/> property determines background color of the container in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ContainerBackgroundProperty =
			BindableProperty.Create(
				nameof(ContainerBackground),
				typeof(Brush),
				typeof(SfTextInputLayout),
				_defaultContainerBackground,
				BindingMode.Default,
				null,
				OnContainerBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="OutlineCornerRadius"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="OutlineCornerRadius"/> property specifies the corner radius of the outline border in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty OutlineCornerRadiusProperty =
			BindableProperty.Create(
				nameof(OutlineCornerRadius),
				typeof(double),
				typeof(SfTextInputLayout),
				3.5d,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="FocusedStrokeThickness"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="FocusedStrokeThickness"/>  property determines the stroke thickness of the border when the <see cref="SfTextInputLayout"/> is focused.
		/// </remarks>
		public static readonly BindableProperty FocusedStrokeThicknessProperty =
			BindableProperty.Create(
				nameof(FocusedStrokeThickness),
				typeof(double),
				typeof(SfTextInputLayout),
				2d,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="UnfocusedStrokeThickness"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="UnfocusedStrokeThickness"/> property determines stroke thickness of the border when the <see cref="SfTextInputLayout"/> is unfocused.
		/// </remarks>
		public static readonly BindableProperty UnfocusedStrokeThicknessProperty =
			BindableProperty.Create(
				nameof(UnfocusedStrokeThickness),
				typeof(double),
				typeof(SfTextInputLayout),
				1d,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CharMaxLength"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CharMaxLength"/> property determines maximum character length of the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty CharMaxLengthProperty =
			BindableProperty.Create(
				nameof(CharMaxLength),
				typeof(int),
				typeof(SfTextInputLayout),
				int.MaxValue,
				BindingMode.Default,
				null,
				OnCharMaxLengthPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Hint"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Hint"/> property determines hint text in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty HintProperty =
			BindableProperty.Create(
				nameof(Hint),
				typeof(string),
				typeof(SfTextInputLayout),
				string.Empty,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HelperText"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HelperText"/> property determines helper text of the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty HelperTextProperty =
			BindableProperty.Create(
				nameof(HelperText),
				typeof(string),
				typeof(SfTextInputLayout),
				string.Empty,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ErrorText"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ErrorText"/> property determines error text of the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ErrorTextProperty =
			BindableProperty.Create(
				nameof(ErrorText),
				typeof(string),
				typeof(SfTextInputLayout),
				string.Empty,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ReserveSpaceForAssistiveLabels"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ReserveSpaceForAssistiveLabels"/> property determines the reserved space for assistive labels such as helper text, error text, and character counters in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ReserveSpaceForAssistiveLabelsProperty =
		 BindableProperty.Create(
			 nameof(ReserveSpaceForAssistiveLabels),
			 typeof(bool),
			 typeof(SfTextInputLayout),
			 true,
			 BindingMode.Default,
			 null,
			 OnReserveSpacePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LeadingViewPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="LeadingViewPosition"/> property determines leading view position in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty LeadingViewPositionProperty =
			BindableProperty.Create(
				nameof(LeadingViewPosition),
				typeof(ViewPosition),
				typeof(SfTextInputLayout),
				ViewPosition.Inside,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TrailingViewPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="TrailingViewPosition"/> property determines trailing view position in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty TrailingViewPositionProperty =
			BindableProperty.Create(
				nameof(TrailingViewPosition),
				typeof(ViewPosition),
				typeof(SfTextInputLayout),
				ViewPosition.Inside,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="InputViewPadding"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="InputViewPadding"/> property determines padding for the input view in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty InputViewPaddingProperty =
			BindableProperty.Create(
				nameof(InputViewPadding),
				typeof(Thickness),
				typeof(SfTextInputLayout),
				new Thickness(-1, -1, -1, -1),
				BindingMode.Default,
				null,
				OnInputViewPaddingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnablePasswordVisibilityToggle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="EnablePasswordVisibilityToggle"/> property determines whether need to display the password visibility toggle in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty EnablePasswordVisibilityToggleProperty =
			BindableProperty.Create(
				nameof(EnablePasswordVisibilityToggle),
				typeof(bool),
				typeof(SfTextInputLayout),
				false,
				BindingMode.Default,
				null,
				OnEnablePasswordTogglePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnableFloating"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="EnableFloating"/> property determines whether the Label should float when the input is focused or unfocused in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty EnableFloatingProperty =
			BindableProperty.Create(
				nameof(EnableFloating),
				typeof(bool),
				typeof(SfTextInputLayout),
				true,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnableHintAnimation"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="EnableHintAnimation"/> property determines whether need to enable the animation for hint text when input view is focused or unfocused in <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty EnableHintAnimationProperty =
			BindableProperty.Create(
				nameof(EnableHintAnimation),
				typeof(bool),
				typeof(SfTextInputLayout),
				true,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ClearButtonColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ClearButtonColor"/> property determines color of the clear button in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindableProperty ClearButtonColorProperty =
			BindableProperty.Create(
				nameof(ClearButtonColor),
				typeof(Color),
				typeof(SfTextInputLayout),
				ClearIconStrokeColor,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HintLabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HintLabelStyle"/> property determines the Label style of the hint text in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty HintLabelStyleProperty =
			BindableProperty.Create(
				nameof(HintLabelStyle),
				typeof(LabelStyle),
				typeof(SfTextInputLayout),
				null,
				BindingMode.Default,
				null,
				defaultValueCreator: bindale => GetHintLabelStyleDefaultValue(),
				propertyChanged: OnHintLableStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HintLabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HelperLabelStyle"/> property determines the Label style of the helper text in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty HelperLabelStyleProperty =
			BindableProperty.Create(
				nameof(HelperLabelStyle),
				typeof(LabelStyle),
				typeof(SfTextInputLayout),
				null,
				BindingMode.Default,
				null,
				defaultValueCreator: bindale => GetHelperLabelStyleDefaultValue(),
				propertyChanged: OnHelperLableStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ErrorLabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ErrorLabelStyle"/> property determines the Label style of the error text in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty ErrorLabelStyleProperty =
			BindableProperty.Create(
				nameof(ErrorLabelStyle),
				typeof(LabelStyle),
				typeof(SfTextInputLayout),
				null,
				BindingMode.Default,
				null,
				defaultValueCreator: bindale => GetErrorLabelStyleDefaultValue(),
				propertyChanged: OnErrorLableStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CounterLabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CounterLabelStyle"/> property determines the Label style of the counter text in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindableProperty CounterLabelStyleProperty =
			BindableProperty.Create(
				nameof(CounterLabelStyle),
				typeof(LabelStyle),
				typeof(SfTextInputLayout),
				null,
				BindingMode.Default,
				null,
				defaultValueCreator: bindale => GetCounterLabelStyleDefaultValue(),
				propertyChanged: OnCounterLableStylePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CurrentActiveColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CurrentActiveColor"/> property determines the current active color of the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindablePropertyKey CurrentActiveColorKey =
		   BindableProperty.CreateReadOnly(
			   nameof(CurrentActiveColor),
			   typeof(Color),
			   typeof(SfTextInputLayout),
			   Color.FromRgba("#79747E"),
			   BindingMode.Default,
			   null,
			   null);

		/// <summary>
		/// Identifies the <see cref="IsHintFloated"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsHintFloated"/> property determines whether the hint text is floated or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindableProperty IsHintFloatedProperty =
			BindableProperty.Create(
				nameof(IsHintFloated),
				typeof(bool),
				typeof(SfTextInputLayout),
				false,
				BindingMode.Default,
				null,
				OnIsHintFloatedPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="DownIconTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="DownIconTemplate"/> property determines the template for the "down" icon in the user interface.
		/// It gets or sets a <see cref="DownIconTemplate"/> that represents the icon or visual indication for downward actions.
		/// </remarks>
		internal static readonly BindableProperty DownIconTemplateProperty =
				 BindableProperty.Create(nameof(DownIconTemplate), 
					typeof(View), 
					typeof(SfTextInputLayout), 
					null, BindingMode.OneWay, 
					null, 
					OnUpDownTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="UpIconTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="UpIconTemplate"/> property defines the template for the "up" icon in the user interface.
		/// It gets or sets a <see cref="UpIconTemplateProperty"/> that acts as the representation of upward actions or indicators.
		/// </remarks>
		internal static readonly BindableProperty UpIconTemplateProperty =
		 BindableProperty.Create(nameof(UpIconTemplate), 
					typeof(View), 
					typeof(SfTextInputLayout), 
					null, 
					BindingMode.OneWay, 
					null, 
					OnUpDownTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="CurrentActiveColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CurrentActiveColor"/> property determines current active color of the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty CurrentActiveColorProperty = CurrentActiveColorKey.BindableProperty;

		/// <summary>
		/// Identifies the <see cref="IsEnabled"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsEnabled"/> property determines  whether the control is enabled or not in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static new readonly BindableProperty IsEnabledProperty =
			BindableProperty.Create(
				nameof(IsEnabled),
				typeof(bool),
				typeof(SfTextInputLayout),
				true,
				BindingMode.Default,
				null,
				OnEnabledPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HintTextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HintTextColor"/> property determines the color of the  hint text in <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindableProperty HintTextColorProperty =
		 BindableProperty.Create(
			 nameof(HintTextColor),
			 typeof(Color),
			 typeof(LabelStyle),
			 Color.FromArgb("#49454F"));

		/// <summary>
		/// Identifies the <see cref="HelperTextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HelperTextColor"/> property determines the color of the helper text in <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindableProperty HelperTextColorProperty =
		 BindableProperty.Create(
			 nameof(HelperTextColor),
			 typeof(Color),
			 typeof(LabelStyle),
			 Color.FromArgb("#49454F"));

		/// <summary>
		/// Identifies the <see cref="ClearButtonPath"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ClearButtonPath"/> property allows for the customization of the appearance of clear button in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		internal static readonly BindableProperty ClearButtonPathProperty =
		 BindableProperty.Create(
			 nameof(ClearButtonPath),
			 typeof(Path),
			 typeof(SfTextInputLayout),
			 null,
			 BindingMode.OneWay,
			 null,
			 OnClearButtonPathChanged);

		/// <summary>
		/// Gets or sets a value that indicates whether to show the up down button.
		/// </summary>
		/// <value><c>false</c> if disable up down button; otherwise, <c>true</c>.</value>
		/// <remarks>This property supports for SfNumericUpDown control only.</remarks>
		internal static readonly BindableProperty ShowUpDownButtonProperty =
			BindableProperty.Create(
				nameof(ShowUpDownButton),
				typeof(bool),
				typeof(SfTextInputLayout),
				false, BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Gets or sets a value that indicates whether to show the clear button.
		/// </summary>
		/// <value><c>false</c> if disable clear button; otherwise, <c>true</c>.</value>
		/// <remarks>This property supports for SfNumericEntry only.</remarks>
		internal static readonly BindableProperty ShowClearButtonProperty =
			BindableProperty.Create(
				nameof(ShowClearButton),
				typeof(bool),
				typeof(SfTextInputLayout),
				false,
				BindingMode.Default,
				null,
				OnPropertyChanged);

		/// <summary>
		/// Identifies <see cref="UpButtonColor"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="UpButtonColor"/> bindable property.</value>
		internal static readonly BindableProperty UpButtonColorProperty =
			BindableProperty.Create(
				nameof(UpButtonColor),
				typeof(Color),
				typeof(SfTextInputLayout),
				Color.FromArgb("#49454F"),
				BindingMode.Default, null,
				OnUpDownButtonColorChanged);

		/// <summary>
		/// Identifies <see cref="DownButtonColor"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="DownButtonColor"/> bindable property.</value>
		internal static readonly BindableProperty DownButtonColorProperty =
			BindableProperty.Create(
				nameof(DownButtonColor),
				typeof(Color),
				typeof(SfTextInputLayout),
				Color.FromArgb("#49454F"),
				BindingMode.Default,
				null,
				OnUpDownButtonColorChanged);

		/// <summary>
		/// Returns the default label style for hint label.
		/// </summary>
		/// <returns>The LabelStyle</returns>
		static LabelStyle GetHintLabelStyleDefaultValue()
		{
			return new LabelStyle() { FontSize = DefaultHintFontSize };
		}

		/// <summary>
		/// Returns the default label style for helper label.
		/// </summary>
		/// <returns>The LabelStyle</returns>
		static LabelStyle GetHelperLabelStyleDefaultValue()
		{
			return new LabelStyle();
		}

		/// <summary>
		/// Returns the default label style for Error label.
		/// </summary>
		/// <returns>The LabelStyle</returns>
		static LabelStyle GetErrorLabelStyleDefaultValue()
		{
			return new LabelStyle();
		}

		/// <summary>
		/// Returns the default label style for counter label.
		/// </summary>
		/// <returns>The LabelStyle</returns>
		static LabelStyle GetCounterLabelStyleDefaultValue()
		{
			return new LabelStyle();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether the hint text should be displayed.
		/// </summary>
		/// <value><c>true</c> if the hint text should be displayed; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the ShowHint property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                ShowHint="True" 
		///                                Hint="Name">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.ShowHint = true;
		/// inputLayout.Hint = "Name";
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool ShowHint
		{
			get { return (bool)GetValue(ShowHintProperty); }
			set 
			{ 
				SetValue(ShowHintProperty, value);
				SetCustomDescription(this.Content);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to display the character count when the value is changed.
		/// </summary>
		/// <value><c>true</c> if enable counter; otherwise, <c>false</c>.</value>
		internal bool ShowCharCount
		{
			get { return (bool)GetValue(ShowCharCountProperty); }
			set { SetValue(ShowCharCountProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the helper text should be displayed.
		/// </summary>
		/// <value><c>true</c> if the helper text should be displayed; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the ShowHelperText property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                HelperText="Name">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.HelperText = "Name";
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool ShowHelperText
		{
			get => (bool)GetValue(ShowHelperTextProperty);
			set => SetValue(ShowHelperTextProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the input has validation errors.
		/// </summary>
		/// <value><c>true</c> if there are validation errors; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the HasError property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                ErrorText="Invalid"
		///                                HasError="True">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.HasError = true;
		/// inputLayout.ErrorText="Invalid";
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool HasError
		{
			get => (bool)GetValue(HasErrorProperty);
			set => SetValue(HasErrorProperty, value);
		}

		/// <summary>
		/// Gets or sets the color of the border or base line, depending on the container type.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the Stroke property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Stroke="Blue">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Stroke = Colors.Blue;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public Color Stroke
		{
			get => (Color)GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		/// <summary>
		/// Gets or sets the Hint text font size.
		/// </summary>
		internal float HintFontSize { get; set; }

		/// <remarks>
		/// This property gets or sets a Numeric "Down" button view.
		/// </remarks>
		internal View? DownIconTemplate
		{
			get { return (View)this.GetValue(DownIconTemplateProperty); }
			set { this.SetValue(DownIconTemplateProperty, value); }
		}

		/// <remarks>
		/// This property gets or sets a Numeric "Up" buttons view.
		/// </remarks>
		internal View? UpIconTemplate
		{
			get { return (View)this.GetValue(UpIconTemplateProperty); }
			set { this.SetValue(UpIconTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background of the container.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the ContainerBackground property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                ContainerType="Filled"
		///                                ContainerBackground="#E6EEF9">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.ContainerType = ContainerType.Filled;
		/// inputLayout.ContainerBackground = Color.FromHex("#E6EEF9");
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public Brush ContainerBackground
		{
			get => (Brush)GetValue(ContainerBackgroundProperty);
			set => SetValue(ContainerBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the corner radius of the outline border.
		/// </summary>
		/// <remarks>It is applicable only for the outlined container type.</remarks>
		/// <value>The default value is 4.</value>
		/// <example>
		/// The following code demonstrates how to use the OutlineCornerRadius property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                ContainerType="Outlined"
		///                                OutlineCornerRadius="8">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.ContainerType = ContainerType.Outlined;
		/// inputLayout.OutlineCornerRadius = 8;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public double OutlineCornerRadius
		{
			get => (double)GetValue(OutlineCornerRadiusProperty);
			set => SetValue(OutlineCornerRadiusProperty, value);
		}

		/// <summary>
		/// Gets or sets the stroke thickness of the bottom line or outline border when control is in a focused state.
		/// <remarks>This property is applicable for both filled and outlined container types.</remarks>
		/// </summary>
		/// <value>The default value is 2.</value>
		/// <example>
		/// The following code demonstrates how to use the FocusedStrokeThickness property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                ContainerType="Outlined"
		///                                FocusedStrokeThickness="10">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.ContainerType = ContainerType.Outlined;
		/// inputLayout.FocusedStrokeThickness = 10;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public double FocusedStrokeThickness
		{
			get => (double)GetValue(FocusedStrokeThicknessProperty);
			set => SetValue(FocusedStrokeThicknessProperty, value);
		}

		/// <summary>
		/// Gets or sets the stroke thickness for the bottom line or outline border when control is in an unfocused state
		/// <remarks>This property is applicable for filled and outlined container types.</remarks>
		/// </summary>
		/// <value>The default value is 1.</value>
		/// <example>
		/// The following code demonstrates how to use the UnfocusedStrokeThickness property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                ContainerType="Outlined"
		///                                UnfocusedStrokeThickness="10">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.ContainerType = ContainerType.Outlined;
		/// inputLayout.UnfocusedStrokeThickness = 10;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public double UnfocusedStrokeThickness
		{
			get => (double)GetValue(UnfocusedStrokeThicknessProperty);
			set => SetValue(UnfocusedStrokeThicknessProperty, value);
		}

		/// <summary>
		/// Gets or sets the maximum character length for the input. An error color is applied when this limit is exceeded.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the UnfocusedStrokeThickness property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                CharMaxLength="10">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.CharMaxLength = 10;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public int CharMaxLength
		{
			get => (int)GetValue(CharMaxLengthProperty);
			set => SetValue(CharMaxLengthProperty, value);
		}

		/// <summary>
		/// Gets or sets the view to be displayed before the input view.
		/// </summary>
		/// <remarks>This view is typically used to display icons or other visual elements.</remarks>
		/// <example>
		/// The following code demonstrates how to use the LeadingView property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput" 
		///                               Hint="Name">
		///    <Entry/>
		///    <inputLayout:SfTextInputLayout.LeadingView>
		///         <Label
		///              Text="&#x1F5D3;">
		///         </Label>
		///    </inputLayout:SfTextInputLayout.LeadingView>
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Birth date";
		/// inputLayout.LeadingView = new Label() { Text = "\U0001F5D3" };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public View LeadingView
		{
			get => (View)GetValue(LeadingViewProperty);
			set => SetValue(LeadingViewProperty, value);
		}

		/// <summary>
		/// Gets or sets the view to be displayed after the input view.
		/// </summary>
		/// <remarks>This view is typically used to display icons or other visual elements.</remarks>
		/// <example>
		/// The following code demonstrates how to use the TrailingView property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput" 
		///                               Hint="Name">
		///    <Entry/>
		///    <inputLayout:SfTextInputLayout.TrailingView>
		///         <Label
		///              Text="&#x1F5D3;">
		///         </Label>
		///    </inputLayout:SfTextInputLayout.TrailingView>
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Birth date";
		/// inputLayout.TrailingView = new Label() { Text = "\U0001F5D3" };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public View TrailingView
		{
			get => (View)GetValue(TrailingViewProperty);
			set => SetValue(TrailingViewProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the leading view should be displayed.
		/// The default value is <c>true</c>.
		/// </summary>
		/// <value><c>true</c> if the leading view should be displayed; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the ShowLeadingView property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput" 
		///                                Hint="Name"
		///                                ShowLeadingView="False">
		///    <Entry/>
		///    <inputLayout:SfTextInputLayout.LeadingView>
		///         <Label
		///              Text="&#x1F5D3;">
		///         </Label>
		///    </inputLayout:SfTextInputLayout.LeadingView>
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Birth date";
		/// inputLayout.LeadingView = new Label() { Text = "\U0001F5D3" };
		/// inputLayout.ShowLeadingView = false;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool ShowLeadingView
		{
			get => (bool)GetValue(ShowLeadingViewProperty);
			set => SetValue(ShowLeadingViewProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the trailing view should be displayed.
		/// The default value is <c>true</c>.
		/// </summary>
		/// <value><c>true</c> if the trailing view should be displayed; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the ShowTrailingView property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput" 
		///                                Hint="Name"
		///                                ShowTrailingView="False">
		///    <Entry/>
		///    <inputLayout:SfTextInputLayout.TrailingView>
		///         <Label
		///              Text="&#x1F5D3;">
		///         </Label>
		///    </inputLayout:SfTextInputLayout.TrailingView>
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Birth date";
		/// inputLayout.TrailingView = new Label() { Text = "\U0001F5D3" };
		/// inputLayout.ShowTrailingView = false;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool ShowTrailingView
		{
			get => (bool)GetValue(ShowTrailingViewProperty);
			set => SetValue(ShowTrailingViewProperty, value);
		}

		/// <summary>
		/// Gets or sets the position of the leading view relative to the input layout.
		/// The default value is <c>ViewPosition.Inside</c>.
		/// </summary>
		/// <example>
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput" 
		///                                Hint="Name"
		///                                LeadingViewPosition="Outside">
		///    <Entry/>
		///    <inputLayout:SfTextInputLayout.LeadingView>
		///         <Label
		///              Text="&#x1F5D3;">
		///         </Label>
		///    </inputLayout:SfTextInputLayout.LeadingView>
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Birth date";
		/// inputLayout.LeadingView = new Label() { Text = "\U0001F5D3" };
		/// inputLayout.LeadingViewPosition = ViewPosition.Outside;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public ViewPosition LeadingViewPosition
		{
			get => (ViewPosition)GetValue(LeadingViewPositionProperty);
			set => SetValue(LeadingViewPositionProperty, value);
		}

		/// <summary>
		/// Gets or sets the position of the trailing view relative to the input layout.
		/// The default value is <c>ViewPosition.Inside</c>.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the TrailingViewPosition property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput" 
		///                                Hint="Name"
		///                                TrailingViewPosition="Outside">
		///    <Entry/>
		///    <inputLayout:SfTextInputLayout.TrailingView>
		///         <Label
		///              Text="&#x1F5D3;">
		///         </Label>
		///    </inputLayout:SfTextInputLayout.TrailingView>
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Birth date";
		/// inputLayout.TrailingView = new Label() { Text = "\U0001F5D3" };
		/// inputLayout.TrailingViewPosition = ViewPosition.Outside;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public ViewPosition TrailingViewPosition
		{
			get => (ViewPosition)GetValue(TrailingViewPositionProperty);
			set => SetValue(TrailingViewPositionProperty, value);
		}

		/// <summary>
		/// Gets or sets custom padding for the input view, overriding the default padding.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the InputViewPadding property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                InputViewPadding="0,5,0,5">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.InputViewPadding= new Thickness(0,5,0,5);;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public Thickness InputViewPadding
		{
			get { return (Thickness)GetValue(InputViewPaddingProperty); }
			set { SetValue(InputViewPaddingProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this control is focused.
		/// </summary>
		bool IsLayoutFocused
		{
			get => (bool)GetValue(IsLayoutFocusedProperty);
			set => SetValue(IsLayoutFocusedProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the hint label should always be floated, even when the input text is empty.
		/// The default value is <c>false</c>.
		/// </summary>
		/// <value><c>true</c> if the hint label should always be floated; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the IsHintAlwaysFloated property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                IsHintAlwaysFloated="True"
		///                                Hint="Name">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.IsHintAlwaysFloated = true;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool IsHintAlwaysFloated
		{
			get => (bool)GetValue(IsHintAlwaysFloatedProperty);
			set => SetValue(IsHintAlwaysFloatedProperty, value);
		}

		/// <summary>
		/// Gets or sets the hint text displayed in the input view.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the Hint property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public string Hint
		{
			get { return (string)GetValue(HintProperty); }
			set
			{
				SetValue(HintProperty, value);
				SetCustomDescription(this.Content);
			}
		}

		/// <summary>
		/// Gets or sets the helper text that provides additional information about the input.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the HelperText property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name"
		///                                HelperText="Enter name">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HelperText = "Enter name";
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public string HelperText
		{
			get => (string)GetValue(HelperTextProperty);
			set => SetValue(HelperTextProperty, value);
		}

		/// <summary>
		/// Gets or sets the error text displayed when validation fails.
		/// </summary>
		/// <remarks>Error messages are displayed below the input line, replacing the helper text until the error is fixed.</remarks>
		/// <example>
		/// The following code demonstrates how to use the ErrorText property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Email"
		///                                ErrorText="Invalid email"
		///                                HasError="True">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Email";
		/// inputLayout.ErrorText = "Invalid email"
		/// inputLayout.HasError = true;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public string ErrorText
		{
			get => (string)GetValue(ErrorTextProperty);
			set => SetValue(ErrorTextProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether space is reserved for assistive labels such as 
		/// helper text, error text, and character counters.
		/// </summary>
		/// <value>
		/// <c>true</c> if space should be reserved for assistive labels; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		/// If set to <c>false</c>, space will only be allocated based on the presence of helper text, 
		/// error text, and character counter labels.
		/// </remarks>
		/// <example>
		/// The following code demonstrates how to use the ReserveSpaceForAssistiveLabels property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name"
		///                                ReserveSpaceForAssistiveLabels="False">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.ReserveSpaceForAssistiveLabels = false;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool ReserveSpaceForAssistiveLabels
		{
			get => (bool)GetValue(ReserveSpaceForAssistiveLabelsProperty);
			set => SetValue(ReserveSpaceForAssistiveLabelsProperty, value);
		}

		/// <summary>
		/// Gets or sets the type of container, which specifies the appearance of the background and its border.
		/// The default value is <c>ContainerType.Filled</c>.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the ContainerType property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name"
		///                                ContainerType="Outlined">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.ContainerType = ContainerType.Outlined;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public ContainerType ContainerType
		{
			get => (ContainerType)GetValue(ContainerTypeProperty);
			set => SetValue(ContainerTypeProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the password visibility toggle.
		/// </summary>
		/// <value><c>true</c> if the password visibility toggle is enabled; otherwise, <c>false</c>.</value>
		/// <remarks>This property is supported only for <see cref="Entry"/> control.</remarks>
		/// <example>
		/// The following code demonstrates how to use the EnablePasswordVisibilityToggle property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Password"
		///                                EnablePasswordVisibilityToggle="True">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Password";
		/// inputLayout.EnablePasswordVisibilityToggle = true;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool EnablePasswordVisibilityToggle
		{
			get => (bool)GetValue(EnablePasswordVisibilityToggleProperty);
			set => SetValue(EnablePasswordVisibilityToggleProperty, value);
		}

		/// <summary>
		/// Gets a value for current active color based on input view's focus state.  
		/// </summary>
		public Color CurrentActiveColor
		{
			get { return (Color)GetValue(CurrentActiveColorProperty); }
			internal set { SetValue(CurrentActiveColorKey, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is enabled and can interact with the user.
		/// The default value is <c>true</c>.
		/// </summary>
		/// <value><c>true</c> if the control is enabled; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the IsEnabled property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name"
		///                                IsEnabled="False">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.IsEnabled = false;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public new bool IsEnabled
		{
			get { return (bool)GetValue(IsEnabledProperty); }
			set { SetValue(IsEnabledProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the assistive label should float when the input is focused or unfocused.
		/// </summary>
		/// <value><c>true</c> if the label should float; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the EnableFloating property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name"
		///                                EnableFloating="False">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.EnableFloating = false;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool EnableFloating
		{
			get => (bool)GetValue(EnableFloatingProperty);
			set => SetValue(EnableFloatingProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to enable animation for the hint text
		/// when the input view is focused or unfocused.
		/// </summary>
		/// <value><c>true</c> if hint animation is enabled; otherwise, <c>false</c>.</value>
		/// <example>
		/// The following code demonstrates how to use the EnableHintAnimation property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name"
		///                                EnableHintAnimation="False">
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.EnableHintAnimation = false;
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool EnableHintAnimation
		{
			get => (bool)GetValue(EnableHintAnimationProperty);
			set => SetValue(EnableHintAnimationProperty, value);
		}

		/// <summary>
		/// Gets or sets the color of the clear button.
		/// </summary>
		/// <remarks>This property supports for SfNumericEntry only.</remarks>
		internal Color ClearButtonColor
		{
			get => (Color)GetValue(ClearButtonColorProperty);
			set => SetValue(ClearButtonColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the style applied to the hint label.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the HintLabelStyle property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name">
		///  <inputLayout:SfTextInputLayout.HintLabelStyle>
		///    <inputLayout:LabelStyle TextColor = "Green" />
		/// </inputLayout:SfTextInputLayout.HintLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HintLabelStyle = new LabelStyle() { TextColor = Color.Green };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public LabelStyle? HintLabelStyle
		{
			get { return (LabelStyle)GetValue(HintLabelStyleProperty); }
			set { SetValue(HintLabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the style applied to the helper label.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the HelperLabelStyle property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name"
		///                                HelperText="Enter your name">
		///  <inputLayout:SfTextInputLayout.HelperLabelStyle>
		///    <inputLayout:LabelStyle TextColor = "Green" />
		/// </inputLayout:SfTextInputLayout.HelperLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HelperText = "Enter your name";
		/// inputLayout.HelperLabelStyle = new LabelStyle() { TextColor = Color.Green };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public LabelStyle? HelperLabelStyle
		{
			get { return (LabelStyle)GetValue(HelperLabelStyleProperty); }
			set { SetValue(HelperLabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the style applied to the error label.
		/// </summary>
		/// <remarks>This style is used to customize the appearance of the error message displayed below the input field.</remarks>
		/// <example>
		/// The following code demonstrates how to use the ErrorLabelStyle property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Email"
		///                                ErrorText="Invalid email">
		///  <inputLayout:SfTextInputLayout.ErrorLabelStyle>
		///    <inputLayout:LabelStyle TextColor = "Red" />
		/// </inputLayout:SfTextInputLayout.ErrorLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Email";
		/// inputLayout.ErrorText = "Invalid email";
		/// inputLayout.ErrorLabelStyle = new LabelStyle() { TextColor = Color.Red };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public LabelStyle? ErrorLabelStyle
		{
			get { return (LabelStyle)GetValue(ErrorLabelStyleProperty); }
			set { SetValue(ErrorLabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the style for counter label.
		/// </summary>
		internal LabelStyle CounterLabelStyle
		{
			get { return (LabelStyle)GetValue(CounterLabelStyleProperty); }
			set { SetValue(CounterLabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color value of Hint Text
		/// </summary>
		internal Color HintTextColor
		{
			get { return (Color)GetValue(HintTextColorProperty); }
			set { SetValue(HintTextColorProperty, value); }
		}
		/// <summary>
		/// Gets or sets the color value of Helper Text
		/// </summary>
		internal Color HelperTextColor
		{
			get { return (Color)GetValue(HelperTextColorProperty); }
			set { SetValue(HelperTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the UpDown buttons.
		/// </summary>
		/// <value><c>true</c> if UpDown button is visible; otherwise, <c>false</c>.</value>
		/// <remarks>This property supports for NumericEntry control only.</remarks>
		internal bool ShowUpDownButton
		{
			get => (bool)this.GetValue(ShowUpDownButtonProperty);
			set => this.SetValue(ShowUpDownButtonProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the clear button.
		/// </summary>
		/// <value><c>false</c> if disable clear button; otherwise, <c>true</c>.</value>
		/// <remarks>This property supports for SfCombobox and SfAutoComplete only.</remarks>
		internal bool ShowClearButton
		{
			get => (bool)this.GetValue(ShowClearButtonProperty);
			set => this.SetValue(ShowClearButtonProperty, value);
		}

		/// <summary>
		/// Gets or sets the color of the up button.
		/// </summary>
		/// <value>Specifies the value for up button. The default value is Colors.Black.</value>
		/// <remarks>This property supports for SfNumericUpDown only.</remarks>
		internal Color UpButtonColor
		{
			get => (Color)this.GetValue(UpButtonColorProperty);
			set => this.SetValue(UpButtonColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the color of the down button.
		/// </summary>
		/// <value>Specifies the value for down button. The default value is Colors.Black.</value>
		/// <remarks>This property supports for SfNumericUpDown only.</remarks>
		internal Color DownButtonColor
		{
			get => (Color)this.GetValue(DownButtonColorProperty);
			set => this.SetValue(DownButtonColorProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the alignment of the up-down button is vertical or not.  
		/// </summary>
		internal bool IsUpDownVerticalAlignment
		{
			get { return _isUpDownVerticalAlignment; }
			set
			{
				if (value != _isUpDownVerticalAlignment)
				{
					_isUpDownVerticalAlignment = value;
					this.InvalidateMeasureOverride();
				}
			}
		}

		/// <summary>
		/// Gets the value of the input text of the <see cref="SfTextInputLayout"/>.
		/// </summary>
		public string Text
		{
			get => _text;
			internal set
			{
				if (value != _text)
				{
					_text = value;
					HandleSemanticsReset();
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the background mode is outline.
		/// </summary>
		internal bool IsOutlined
		{
			get { return ContainerType == ContainerType.Outlined; }
		}

		/// <summary>
		/// Gets a value indicating whether the background mode is none.
		/// </summary>
		internal bool IsNone
		{
			get { return ContainerType == ContainerType.None; }
		}

		/// <summary>
		/// Gets a value indicating whether the background mode is filled.
		/// </summary>
		internal bool IsFilled
		{
			get { return ContainerType == ContainerType.Filled; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the hint was floated.
		/// </summary>
		/// <remarks>This property is used to update the control UI based of hint state.</remarks>
		internal bool IsHintFloated
		{
			get { return (bool)GetValue(IsHintFloatedProperty); }
			set 
			{ 
				SetValue(IsHintFloatedProperty, value);
				SetCustomDescription(this.Content);
			}
		}

		/// <summary>
		/// Gets or sets the path to customize the appearance of the clear button.
		/// </summary>
		/// <value>Specifies the ClearButtonPath. The default value is null</value>
		internal Path ClearButtonPath
		{
			get { return (Path)GetValue(ClearButtonPathProperty); }
			set { SetValue(ClearButtonPathProperty, value); }
		}

		Color DisabledColor { get { return Color.FromUint(0x42000000); } }

		/// <summary>
		/// Gets a value indicating the size of hint.
		/// </summary>
		internal SizeF HintSize
		{
			get
			{
				if (string.IsNullOrEmpty(Hint) || HintLabelStyle == null)
				{
					return new Size(0);
				}

				_internalHintLabelStyle.FontSize = HintFontSize;
				var size = Hint.Measure(_internalHintLabelStyle);
				size.Height = GetHintLineCount(size.Width) * size.Height;


				return size;
			}
		}

		/// <summary>
		/// Gets a value indicating the size of floated hint.
		/// </summary>
		internal SizeF FloatedHintSize
		{
			get
			{
				if (string.IsNullOrEmpty(Hint) || HintLabelStyle == null || !ShowHint)
				{
					return new Size(0);
				}

				_internalHintLabelStyle.FontSize = FloatedHintFontSize;
				var size = Hint.Measure(_internalHintLabelStyle);
				size.Height = GetHintLineCount(size.Width) * size.Height;
				return size;
			}
		}

		/// <summary>
		/// Gets a value indicating the size of assistive text.
		/// </summary>
		internal SizeF CounterTextSize
		{
			get
			{
				if (string.IsNullOrEmpty(_counterText) || CounterLabelStyle == null)
				{
					return GetLabelSize(new Size(0, DefaultAssistiveTextHeight));
				}
				var size = Hint.Measure(_internalCounterLabelStyle);
				size.Width += DefaultAssistiveLabelPadding;
				return GetLabelSize(size);
			}
		}

		/// <summary>
		/// Gets a value indicating the size of helper text.
		/// </summary>
		internal SizeF HelperTextSize
		{
			get
			{
				if (string.IsNullOrEmpty(HelperText) || HelperLabelStyle == null)
				{
					return GetLabelSize(new Size(0, DefaultAssistiveTextHeight));
				}
				var size = HelperText.Measure(_internalHelperLabelStyle);
				size.Height = GetAssistiveTextLineCount(size.Width) * size.Height;
				return GetLabelSize(size);
			}
		}

		/// <summary>
		/// Gets a value indicating the size of Error text.
		/// </summary>
		internal SizeF ErrorTextSize
		{
			get
			{
				if (string.IsNullOrEmpty(ErrorText) || ErrorLabelStyle == null)
				{
					return GetLabelSize(new Size(0, DefaultAssistiveTextHeight));
				}
				var size = ErrorText.Measure(_internalErrorLabelStyle);
				size.Height = GetAssistiveTextLineCount(size.Width) * size.Height;
				return GetLabelSize(size);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the alignment of the up-down button alignment is left.  
		/// </summary>
		internal bool IsUpDownAlignmentLeft
		{
			get { return _isUpDownAlignmentLeft; }
			set
			{
				if (value != _isUpDownAlignmentLeft)
				{
					this._isUpDownAlignmentLeft = value;
					this.InvalidateMeasureOverride();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the alignment of the up-down button alignment is both.
		/// </summary>
		internal bool IsUpDownAlignmentBoth
		{
			get { return _isUpDownAlignmentBoth; }
			set
			{
				if (value != _isUpDownAlignmentBoth)
				{
					this._isUpDownAlignmentBoth = value;
					this.InvalidateMeasureOverride();
				}
			}
		}

		/// <summary>
		/// Gets the base line max height.
		/// </summary>
		internal double BaseLineMaxHeight => Math.Max(FocusedStrokeThickness, UnfocusedStrokeThickness);

		/// <summary>
		/// Gets a value indicating the top padding of the input view.
		/// </summary>
		double TopPadding
		{
			get
			{
				if (BaseLineMaxHeight <= 2)
				{
					return IsOutlined ? OutlinedPadding + (ShowHint ? (DefaultAssistiveTextHeight / 2) : (BaseLineMaxHeight * 2)) : IsFilled ? FilledTopPadding : NoneTopPadding;
				}
				return IsOutlined ? OutlinedPadding + (ShowHint ? (BaseLineMaxHeight > FloatedHintSize.Height / 2) ? BaseLineMaxHeight : FloatedHintSize.Height / 2 : (BaseLineMaxHeight)) : IsFilled ? FilledTopPadding : NoneTopPadding;
			}
		}

		/// <summary>
		/// Gets a value indicating the bottom padding of the input view.
		/// </summary>
		double BottomPadding
		{
			get
			{
				if (BaseLineMaxHeight <= 2)
				{
					return (IsFilled ? FilledBottomPadding
					: IsOutlined ? OutlinedPadding : NoneBottomPadding) + (ReserveSpaceForAssistiveLabels ? TotalAssistiveTextHeight() + DefaultAssistiveLabelPadding : 0);
				}
				return (IsFilled ? FilledBottomPadding
					: IsOutlined ? OutlinedPadding : NoneBottomPadding) + BaseLineMaxHeight + (ReserveSpaceForAssistiveLabels ? TotalAssistiveTextHeight() + DefaultAssistiveLabelPadding : 0);
			}
		}

		/// <summary>
		/// Gets a value indicating the edge padding of the input view.
		/// </summary>
		double LeftPadding
		{
			get
			{
				if (BaseLineMaxHeight <= 2)
				{
					return IsNone ? 0 : EdgePadding + (IsOutlined ? BaseLineMaxHeight : 0);
				}
				return IsNone ? 0 : EdgePadding + (IsOutlined ? BaseLineMaxHeight - DefaultAssistiveLabelPadding / 2 : 0);
			}
		}

		/// <summary>
		/// Gets the font size of the floated hint text.
		/// </summary>
		float FloatedHintFontSize
		{
			get
			{
				return (float)(HintFontSize * HintFontSizeScalingRatio);
			}
		}

		double AssistiveLabelPadding
		{
			get
			{
				return ReserveSpaceForAssistiveLabels ? DefaultAssistiveLabelPadding + BaseLineMaxHeight / 2 : BaseLineMaxHeight / 2;
			}
		}

		PathF ToggleIconPath
		{
			get
			{
				return _isPasswordTextVisible ? _pathBuilder.BuildPath(_toggleVisibleIconPath) : _pathBuilder.BuildPath(_toggleCollapsedIconPath);
			}
		}

		bool IsRTL
		{
			get { return ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft; }
		}

		bool IsPassowordToggleIconVisible
		{
			get { return (EnablePasswordVisibilityToggle && Content is Entry); }
		}

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Invoked whenever the <see cref="LeadingViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnLeadingViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.AddView(oldValue, newValue);
				inputLayout.UpdateLeadingViewPosition();
				inputLayout.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="TrailingViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnTrailingViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.AddView(oldValue, newValue);
				inputLayout.UpdateTrailingViewPosition();
				inputLayout.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="IsHintAlwaysFloatedProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnHintAlwaysFloatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout && newValue is bool value)
			{
				if (!value && !string.IsNullOrEmpty(inputLayout.Text))
				{
					inputLayout.IsHintFloated = true;
					inputLayout.IsHintDownToUp = !inputLayout.IsHintFloated;
					inputLayout.InvalidateDrawable();
					return;
				}

				inputLayout.IsHintFloated = value;
				inputLayout.IsHintDownToUp = !inputLayout.IsHintFloated;
				inputLayout.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ShowLeadingViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnShowLeadingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (newValue is bool value && bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.UpdateLeadingViewVisibility(value);
				if (inputLayout._initialLoaded)
				{
					inputLayout.UpdateViewBounds();
				}
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ShowTrailingViewProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnShowTrailingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (newValue is bool value && bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.UpdateTrailingViewVisibility(value);
				if (inputLayout._initialLoaded)
				{
					inputLayout.UpdateViewBounds();
				}
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="IsLayoutFocusedProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnIsLayoutFocusedChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.ChangeVisualState();
				inputLayout.StartAnimation();
				inputLayout.ResetSemantics();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="HasErrorProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnHasErrorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.ChangeVisualState();
				inputLayout.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="CharMaxLengthProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnCharMaxLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout && newValue is int value)
			{
				inputLayout._counterText = $"0/{value}";
				inputLayout.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="EnablePasswordVisibilityToggleProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnEnablePasswordTogglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout && inputLayout.Content is Entry entry && newValue is bool value)
			{
				entry.IsPassword = value;
				inputLayout._isPasswordTextVisible = false;
				if (inputLayout._initialLoaded)
				{
					inputLayout.UpdateViewBounds();
				}
			}
		}

        static void OnIsHintFloatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout && newValue is bool value)
            {
                double minOpacity = 0;
#if ANDROID || IOS
				minOpacity  = 0.00001;
#elif MACCATALYST
				minOpacity  = 0.011;
#endif

                double targetOpacity = value ? 1 : minOpacity;

                if (inputLayout.Content is InputView || inputLayout.Content is Microsoft.Maui.Controls.Picker)
                {
                    inputLayout.Content.Opacity = targetOpacity;
                }
                else if (inputLayout.Content is SfView numericEntry && numericEntry.Children.Count > 0)
                {
                    if (numericEntry.Children[0] is Entry numericInputView)
                    {
                        numericInputView.Opacity = targetOpacity;
                    }
                }
            }
        }

		static void OnInputViewPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				if (inputLayout.Content != null)
				{
					inputLayout.UpdateContentMargin(inputLayout.Content);
				}

				if (inputLayout._initialLoaded)
				{
					inputLayout.UpdateViewBounds();
				}
			}

		}

		/// <summary>
		/// Raised when the <see cref="IsEnabled"/> property was changed.
		/// </summary>
		/// <param name="bindable">object</param>
		/// <param name="oldValue">object</param>
		/// <param name="newValue">object</param>
		static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.OnEnabledPropertyChanged((bool)newValue);
			}
		}

		/// <summary>
		/// Raised when the <see cref="UpIconTemplate"/> or <see cref="DownIconTemplate"/> property changes.
		/// </summary>
		/// <param name="bindable">The object on which the property has changed.</param>
		/// <param name="oldValue">The previous value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		static void OnUpDownTemplateChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout sfTextInputLayout)
			{
				sfTextInputLayout.RemovedExistingView((View)oldValue);
				sfTextInputLayout.InvalidateMeasure();
			}
		}

		static void OnHintLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				if (oldValue is LabelStyle oldLabelStyle)
				{
					oldLabelStyle.PropertyChanged -= inputLayout.OnHintLabelStylePropertyChanged;
					SetInheritedBindingContext(oldLabelStyle, null);
					oldLabelStyle.Parent = null;
				}
				if (newValue is LabelStyle newLabelStyle)
				{
					newLabelStyle.Parent = inputLayout;
					SetInheritedBindingContext(newLabelStyle, inputLayout.BindingContext);
					inputLayout._internalHintLabelStyle.TextColor = newLabelStyle.TextColor;
					inputLayout._internalHintLabelStyle.FontFamily = newLabelStyle.FontFamily;
					inputLayout._internalHintLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
					inputLayout.HintFontSize = (float)(newLabelStyle.FontSize < 12d ? inputLayout.FloatedHintFontSize : newLabelStyle.FontSize);
					newLabelStyle.PropertyChanged += inputLayout.OnHintLabelStylePropertyChanged;
					if (inputLayout._initialLoaded)
					{
						inputLayout.UpdateViewBounds();
					}
				}
			}
		}

		static void OnHelperLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				if (oldValue is LabelStyle oldLabelStyle)
				{
					oldLabelStyle.PropertyChanged -= inputLayout.OnHelperLabelStylePropertyChanged;
					SetInheritedBindingContext(oldLabelStyle, inputLayout.BindingContext);
					oldLabelStyle.Parent = null;
				}
				if (newValue is LabelStyle newLabelStyle)
				{
					newLabelStyle.Parent = inputLayout;
					SetInheritedBindingContext(newLabelStyle, inputLayout.BindingContext);
					inputLayout._internalHelperLabelStyle.TextColor = newLabelStyle.TextColor;
					inputLayout._internalHelperLabelStyle.FontFamily = newLabelStyle.FontFamily;
					inputLayout._internalHelperLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
					inputLayout._internalHelperLabelStyle.FontSize = newLabelStyle.FontSize;
					newLabelStyle.PropertyChanged += inputLayout.OnHelperLabelStylePropertyChanged;
					if (inputLayout._initialLoaded)
					{
						inputLayout.UpdateViewBounds();
					}
				}
			}
		}

		static void OnErrorLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				if (oldValue is LabelStyle oldLabelStyle)
				{
					oldLabelStyle.PropertyChanged -= inputLayout.OnErrorLabelStylePropertyChanged;
					SetInheritedBindingContext(oldLabelStyle, inputLayout.BindingContext);
					oldLabelStyle.Parent = null;
				}
				if (newValue is LabelStyle newLabelStyle)
				{
					newLabelStyle.Parent = inputLayout;
					SetInheritedBindingContext(newLabelStyle, inputLayout.BindingContext);
					inputLayout._internalErrorLabelStyle.TextColor = newLabelStyle.TextColor;
					inputLayout._internalErrorLabelStyle.FontFamily = newLabelStyle.FontFamily;
					inputLayout._internalErrorLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
					inputLayout._internalErrorLabelStyle.FontSize = newLabelStyle.FontSize;
					newLabelStyle.PropertyChanged += inputLayout.OnErrorLabelStylePropertyChanged;
					if (inputLayout._initialLoaded)
					{
						inputLayout.UpdateViewBounds();
					}
				}
			}
		}

		static void OnCounterLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				if (oldValue is LabelStyle oldLabelStyle)
				{
					oldLabelStyle.PropertyChanged -= inputLayout.OnCounterLabelStylePropertyChanged;
				}
				if (newValue is LabelStyle newLabelStyle)
				{
					inputLayout._internalCounterLabelStyle.TextColor = newLabelStyle.TextColor;
					inputLayout._internalCounterLabelStyle.FontFamily = newLabelStyle.FontFamily;
					inputLayout._internalCounterLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
					inputLayout._internalCounterLabelStyle.FontSize = newLabelStyle.FontSize;
					newLabelStyle.PropertyChanged += inputLayout.OnCounterLabelStylePropertyChanged;
					if (inputLayout._initialLoaded)
					{
						inputLayout.UpdateViewBounds();
					}
				}
			}
		}

		static void OnReserveSpacePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout && inputLayout.Content != null)
			{
				inputLayout.UpdateContentMargin(inputLayout.Content);
			}
		}

		static void OnContainerTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.UpdateContentMargin(inputLayout.Content);
				if (inputLayout._initialLoaded && inputLayout.Content != null)
					inputLayout.UpdateViewBounds();
				inputLayout.ChangeVisualState();
			}
		}

		static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout && inputLayout._initialLoaded)
			{
				inputLayout.UpdateViewBounds();
				inputLayout.UpdateAssistiveLabels();
				inputLayout.ResetSemantics();
			}
		}

		static void OnContainerBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout._outlinedContainerBackground = (Brush)newValue;
				inputLayout.UpdateViewBounds();
			}
		}

		/// <summary>
		/// This method triggers when the up and down button colors change. 
		/// </summary>
		static void OnUpDownButtonColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Property changed method for  ClearButtonPath property
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnClearButtonPathChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTextInputLayout inputLayout)
			{
				inputLayout.InvalidateDrawable();
			}
		}
		#endregion

	}
}
