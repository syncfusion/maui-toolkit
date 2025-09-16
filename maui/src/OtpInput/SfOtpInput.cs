using System.Globalization;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Rect = Microsoft.Maui.Graphics.Rect;
using ResourceDictionary = Microsoft.Maui.Controls.ResourceDictionary;
using TextAlignment = Microsoft.Maui.TextAlignment;
#if WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
#elif MACCATALYST || IOS
using UIKit;
using Foundation;
#endif

namespace Syncfusion.Maui.Toolkit.OtpInput
{
    /// <summary>
    /// The <see cref="SfOtpInput"/> control is used to enter and verify one-time passwords (OTP) easily and securely.
    /// </summary>
    public class SfOtpInput : SfView, IKeyboardListener, IParentThemeElement
	{

        #region Fields

        /// <summary>
        /// Array of OTP entry controls, each representing an OTP input field.
        /// </summary>
        OTPEntry[]? _otpEntries;

        /// <summary>
        /// Array of bounds defining the position and size of each OTP input field.
        /// </summary>
        RectF[] _entryBounds = new RectF[4];

        /// <summary>
        /// Index of the currently focused OTP input field.
        /// </summary>
        int _focusedIndex = 0;

        /// <summary>
        /// Rectangle used to outline the bounds of each OTP input field for drawing purposes.
        /// </summary>
        RectF _outlineRectF = new();

        /// <summary>
        /// Starting point of a drawable element line within the OTP control.
        /// </summary>
        PointF _startPoint = new();

        /// <summary>
        /// Ending point of a drawable element line within the OTP control.
        /// </summary>
        PointF _endPoint = new();

        /// <summary>
        /// Width of each OTP input field.
        /// </summary>
        float _entryWidth;

        /// <summary>
        /// Height of each OTP input field.
        /// </summary>
        float _entryHeight;

        /// <summary>
        /// Corner radius for the rounded edges of OTP input fields.
        /// </summary>
        const float _cornerRadius = 4;

		/// <summary>
		/// This helps in adjusting the alignment or layout of the elements.
		/// </summary>
		float _extraSpacing = 10;

#if WINDOWS || MACCATALYST
		/// <summary>
		/// Specifies the additional spacing applied specifically for Windows and Mac platforms.
		/// </summary>
		float _platformSpecificPadding  = 6;
#endif

        /// <summary>
        /// Space between each OTP input field.
        /// </summary>
        float _spacing;

#if WINDOWS
        /// <summary>
        /// Determines whether the input should be displayed in uppercase letters.
        /// </summary>
        bool _isCapsOn = false;
#endif

        /// <summary>
        /// Array of separator labels placed between the OTP input fields.
        /// </summary>
        SfLabel[] _separators = new SfLabel[3];

        /// <summary>
        /// Width of the separator between OTP input fields.
        /// </summary>
        float _separatorWidth;

        /// <summary>
        /// Height of the separator between OTP input fields.
        /// </summary>
        float _separatorHeight;

        /// <summary>
        /// Font size of the separator text between OTP input fields.
        /// </summary>
        const double _separatorTextSize = 14;

        /// <summary>
        /// Represents the previous value of the OTP input.
        /// </summary>
        string? _oldValue;

#if IOS || MACCATALYST
		/// <summary>
		/// Represents the previous value of the OTP input.
		/// </summary>
		string? _oldText;

		/// <summary>
		/// A dictionary mapping OTPEntry objects to UITextField.
		/// </summary>
		Dictionary<OTPEntry, UIKit.UITextField> _platformViews = new Dictionary<OTPEntry, UIKit.UITextField>();

#endif

		/// <summary>
		/// Indicates whether the Shift key is currently pressed.
		/// </summary>
		bool _isShiftOn = false;
#if WINDOWS

		/// <summary>
		/// Indicates whether the Control key is currently pressed.
		/// </summary>
		bool _isCtrlPressed = false;

		/// <summary>
		/// Indicates the StrokeThickness of the entry.
		/// </summary>
		float _strokeThickness = 1 ;
#endif

#if ANDROID || WINDOWS
		bool _isPaste = false;

		/// <summary>
		/// Flag to indicate if the paste action is being handled in OnEntryTextChanged.
		/// </summary>
		bool _isPasteHandled = false;
#endif

		#endregion
		#region BindableProperties

		/// <summary>
		/// Identifies the <see cref="Value"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ValueProperty =
           BindableProperty.Create(nameof(Value), typeof(string), typeof(SfOtpInput), null, BindingMode.TwoWay, propertyChanged: OnValuePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Length"/> bindable property.
        /// </summary>
        public static readonly BindableProperty LengthProperty =
            BindableProperty.Create(nameof(Length), typeof(double), typeof(SfOtpInput), 4.0, BindingMode.TwoWay, propertyChanged: OnLengthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StylingMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StylingModeProperty =
            BindableProperty.Create(nameof(StylingMode), typeof(OtpInputStyle), typeof(SfOtpInput), OtpInputStyle.Outlined, BindingMode.TwoWay, propertyChanged: OnStylingModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Placeholder"/> bindable property.
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty =
          BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(SfOtpInput), null, BindingMode.TwoWay, propertyChanged: OnPlaceholderPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(nameof(Type), typeof(OtpInputType), typeof(SfOtpInput), OtpInputType.Number, BindingMode.TwoWay, propertyChanged: OnTypePropertyChanged);

        ///<summary>
        /// Identifies the <see cref="Separator"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SeparatorProperty =
          BindableProperty.Create(nameof(Separator), typeof(string), typeof(SfOtpInput), string.Empty, BindingMode.TwoWay, propertyChanged: OnSeparatorpropertyChanged);

        ///<summary>
        /// Identifies the <see cref="IsEnabled"/> bindable property.
        /// </summary>
        public static new readonly BindableProperty IsEnabledProperty =
          BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(SfOtpInput), true, BindingMode.TwoWay, propertyChanged: OnIsEnabledpropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AutoFocus"/> bindable property.
        /// </summary>
        public static readonly BindableProperty AutoFocusProperty =
            BindableProperty.Create(nameof(AutoFocus), typeof(bool), typeof(SfOtpInput), false, BindingMode.TwoWay, propertyChanged: OnAutoFocusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MaskCharacter"/> bindable property.
        /// </summary>
        public static readonly BindableProperty MaskCharacterProperty =
            BindableProperty.Create(nameof(MaskCharacter), typeof(char), typeof(SfOtpInput), '●', BindingMode.TwoWay, propertyChanged: OnMaskCharacterPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="InputState"/> bindable property.
        /// </summary>
        public static readonly BindableProperty InputStateProperty =
            BindableProperty.Create(nameof(InputState), typeof(OtpInputState), typeof(SfOtpInput), OtpInputState.Default, BindingMode.TwoWay, propertyChanged: OnInputStatePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#1C1B1F"), BindingMode.TwoWay,propertyChanged:OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#49454F"), BindingMode.TwoWay, propertyChanged: OnStrokePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PlaceholderColor"/> bindable property.
		/// </summary>
		public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#49454F"), BindingMode.TwoWay);

		/// <summary>
		/// Identifies the <see cref="InputBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty InputBackgroundProperty = BindableProperty.Create(nameof(InputBackground), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#E7E0EC"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

		/// <summary>
		/// Identifies the BoxWidth bindable property.
		/// </summary>
		public static readonly BindableProperty BoxWidthProperty =
			BindableProperty.Create(nameof(BoxWidth), typeof(double), typeof(SfOtpInput), 40.0, BindingMode.TwoWay, propertyChanged: OnBoxSizePropertyChanged);

		/// <summary>
		/// Identifies the BoxHeight bindable property.
		/// </summary>
		public static readonly BindableProperty BoxHeightProperty =
			BindableProperty.Create(nameof(BoxHeight), typeof(double), typeof(SfOtpInput), 40.0, BindingMode.TwoWay, propertyChanged: OnBoxSizePropertyChanged);

		#endregion

		#region Internal BindableProperties

		/// <summary>
		/// Identifies the <see cref="SeparatorColor"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#CAC4D0"), BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty FocusedStrokeProperty = BindableProperty.Create(nameof(FocusedStroke), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#6750A4"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

		///<summary>
		/// Identifies the <see cref="HoveredStroke"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty HoveredStrokeProperty = BindableProperty.Create(nameof(HoveredStroke), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#1C1B1F"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="FilledHoverBackground"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty FilledHoveredBackgroundProperty = BindableProperty.Create(nameof(FilledHoverBackground), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#D7D0DD"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="DisabledStroke"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty DisabledStrokeProperty = BindableProperty.Create(nameof(DisabledStroke), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#611c1b1f"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="FilledDisableBackground"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty FilledDisableBackgroundProperty = BindableProperty.Create(nameof(FilledDisableBackground), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#0a1c1b1f"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="SuccessStroke"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty SuccessStrokeProperty = BindableProperty.Create(nameof(SuccessStroke), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#205107"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="WarningStroke"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty WarningStrokeProperty = BindableProperty.Create(nameof(WarningStroke), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#914C00"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ErrorStroke"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ErrorStrokeProperty = BindableProperty.Create(nameof(ErrorStroke), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#B3261E"), BindingMode.TwoWay, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="DisabledTextColor"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty DisabledTextColorProperty = BindableProperty.Create(nameof(DisabledTextColor), typeof(Color), typeof(SfOtpInput), Color.FromArgb("#611c1b1f"), BindingMode.TwoWay);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfOtpInput"/> class.
        /// </summary>
        public SfOtpInput()
        {
            ThemeElement.InitializeThemeResources(this, "SfOtpInputTheme");
            DrawingOrder = DrawingOrder.BelowContent;
            _entryWidth = (float)BoxWidth;
            _entryHeight = (float)BoxHeight;
            InitializeFields();
#if IOS
            this.IgnoreSafeArea = true;
#endif
            this.AddKeyboardListener(this);
            HookEvents();
			this.SetDynamicResource(FilledHoveredBackgroundProperty, "SfOtpInputHoveredBackground");
			this.SetDynamicResource(FilledDisableBackgroundProperty, "SfOtpInputBackgroundDisabled");
			this.SetDynamicResource(FocusedStrokeProperty, "SfOtpInputBorderPressed");
			this.SetDynamicResource(HoveredStrokeProperty, "SfOtpInputBorderHovered");
			this.SetDynamicResource(DisabledStrokeProperty, "SfOtpInputBorderDisabled");
			this.SetDynamicResource(SuccessStrokeProperty, "SfOtpInputSuccessStroke");
			this.SetDynamicResource(WarningStrokeProperty, "SfOtpInputWarningStroke");
			this.SetDynamicResource(ErrorStrokeProperty, "SfOtpInputErrorStroke");
			this.SetDynamicResource(DisabledTextColorProperty, "SfOtpInputDisabledTextColor");
		}

		#endregion

		#region Properties

		/// <summary> 
		/// Gets or sets a value for the OTP input in SfOtpInput control. 
		/// </summary> 
		/// <value> 
		/// It accepts string values.
		/// </value> 
		/// <example>
		/// Below is an example of how to configure the <see cref="Value"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Value="1234"/>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Value = "1234";
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary> 
		/// Gets or sets a value that can be used to specify the length of the input fields in SfOtpInput control. 
		/// </summary> 
		/// <value> 
		/// It accepts double values and the default value is 4.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Length"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Length="5" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Length = 5;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public double Length
		{
			get { return (double)GetValue(LengthProperty); }
			set { SetValue(LengthProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a value that can be used to customize the OTP  input fields in SfOtpInput.
		/// </summary>  
		/// <value>  
		/// A <see cref="OtpInputStyle"/> value. The default is <see cref="OtpInputStyle.Outlined"/>.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="StylingMode"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" StylingMode="Filled" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.StylingMode = OtpInputStyle.Filled;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public OtpInputStyle StylingMode
		{
			get { return (OtpInputStyle)GetValue(StylingModeProperty); }
			set { SetValue(StylingModeProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a value that can be used to shown as a hint/placeholder until the user focuses on or enters a value in SfOtpInput.
		/// </summary>  
		/// <value>  
		/// It accepts string values.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Placeholder"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Placeholder="X" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Placeholder = "X";
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set { SetValue(PlaceholderProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a value that can be used to specify the input values for the OTP input fields in SfOtpInput.
		/// </summary>  
		/// <value>  
		/// It accepts the <see cref="OtpInputType"/> values and the default is <see cref="OtpInputType.Number"/>.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Type"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Type="Password" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Type = OtpInputType.Password;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public OtpInputType Type
		{
			get { return (OtpInputType)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}

		///<summary>
		/// Gets or sets a value that can be add a unique characters between the OTP input fields in SfOtpInput.
		/// </summary>  
		/// <value>  
		/// It accepts string values and the default value is an empty string.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Separator"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Separator="|" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Separator = "|";
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public string Separator
		{
			get { return (string)GetValue(SeparatorProperty); }
			set { SetValue(SeparatorProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a value indicating whether to enable or disable the SfOtpInput control.  
		/// </summary>  
		/// <value>  
		/// It accepts Boolean values and the default value is true.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="IsEnabled"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" IsEnabled="False" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.IsEnabled = false;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public new bool IsEnabled
		{
			get { return (bool)GetValue(IsEnabledProperty); }
			set { SetValue(IsEnabledProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a value indicating whether the SfOtpInput field should automatically receive focus when the component is rendered. 
		/// </summary>  
		/// <value>  
		/// It accepts the Boolean values and the default value is false.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="AutoFocus"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" AutoFocus="True" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.AutoFocus = true;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public bool AutoFocus
		{
			get { return (bool)GetValue(AutoFocusProperty); }
			set { SetValue(AutoFocusProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a character value that can be used to mask the OTP input values in password mode.
		/// </summary>  
		/// <value>  
		/// It accepts the character values, and the default value is a dot ('●').
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="MaskCharacter"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Type="Password" MaskCharacter="*" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Type = OtpInputType.Password;
		/// otpInput.MaskCharacter = '*';
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public char MaskCharacter
		{
			get { return (char)GetValue(MaskCharacterProperty); }
		    set { SetValue(MaskCharacterProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a value that can be used to customize the visual state of the SfOtpInput control.
		/// </summary>  
		/// <value>  
		/// It accepts the OtpInputState values, and the default value is Default.
		/// </value>
		/// <remarks>
		/// This property is useful for scenarios where sensitive information needs to be hidden from view, such as when entering a password or PIN. 
		/// </remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="InputState"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" InputState="Success" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.InputState = OtpInputState.Success;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public OtpInputState InputState
		{
			get { return (OtpInputState)GetValue(InputStateProperty); }
			set { SetValue(InputStateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color used to display the text within the OTP input fields.
		/// </summary>
		/// <value>
		/// A <see cref="Color"/> that represents the text color.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="TextColor"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" TextColor="Green" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.TextColor = Colors.Green;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Color TextColor
		{
			get { return (Color)this.GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the border stroke for the OTP input fields.
		/// </summary>
		/// <value>
		/// A <see cref="Color"/> that represents the stroke color.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Stroke"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Stroke="Blue" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Stroke = Colors.Blue;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Color Stroke
		{
			get { return (Color)this.GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the placeholder text displayed in the OTP input fields when they are empty.
		/// </summary>
		/// <value>
		/// A <see cref="Color"/> that represents the placeholder color.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="PlaceholderColor"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" Placeholder="X" PlaceholderColor="Orange" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.Placeholder = "X";
		/// otpInput.PlaceholderColor = Colors.Orange;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Color PlaceholderColor
		{
			get { return (Color)this.GetValue(PlaceholderColorProperty); }
			set { SetValue(PlaceholderColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background color for the OTP input fields.
		/// </summary>
		/// <value>
		/// <remarks>
		/// This property is applicable only when the StylingMode is set to the Filled state.
		/// </remarks>
		/// A <see cref="Color"/> that represents the background color.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="InputBackground"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" StylingMode="Filled" InputBackground="LightBlue" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.StylingMode = OtpInputStyle.Filled;
		/// otpInput.InputBackground = Colors.LightBlue;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Color InputBackground
		{
			get { return (Color)GetValue(InputBackgroundProperty); }
			set { SetValue(InputBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that defines the width of each input field in the OTP input.
		/// </summary>
		/// <remarks>
		/// A double value representing the width of each input field. The default value is 40.
		/// </remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="BoxWidth"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" BoxWidth="50" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.BoxWidth = 50;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public double BoxWidth
        {
            get => (double)GetValue(BoxWidthProperty);
            set => SetValue(BoxWidthProperty, value);
        }

		/// <summary>
		/// Gets or sets a value that defines the height of each input field in the OTP input.
		/// </summary>
		/// <remarks>
		/// A double value representing the height of each input field. The default value is 40.
		/// </remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="BoxHeight"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <otpInput:SfOtpInput x:Name="otpInput" BoxHeight="50" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfOtpInput otpInput = new SfOtpInput();
		/// otpInput.BoxHeight = 50;
		/// this.Content = otpInput;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public double BoxHeight
        {
            get => (double)GetValue(BoxHeightProperty);
            set => SetValue(BoxHeightProperty, value);
        }

        #endregion

        #region Event

        /// <summary>
        /// Invoke the event when the value of <see cref="SfOtpInput"/> is changed.
        /// </summary>
        public event EventHandler<OtpInputValueChangedEventArgs>? ValueChanged;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the color used for separators between OTP input fields.
        /// </summary>
        internal Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

		/// <summary>
		/// Gets or sets the hovered stroke color of the Input.
		/// </summary>
		internal Color HoveredStroke
		{
			get { return (Color)this.GetValue(HoveredStrokeProperty); }
			set { SetValue(HoveredStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the focused stroke color of the Input.
		/// </summary>
		internal Color FocusedStroke
		{
			get { return (Color)this.GetValue(FocusedStrokeProperty); }
			set { SetValue(FocusedStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the disabled stroke color of the Input.
		/// </summary>
		internal Color DisabledStroke
		{
			get { return (Color)this.GetValue(DisabledStrokeProperty); }
			set { SetValue(DisabledStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the success stroke color of the Input.
		/// </summary>
		internal Color SuccessStroke
		{
			get { return (Color)this.GetValue(SuccessStrokeProperty); }
			set { SetValue(SuccessStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the warning stroke color of the Input.
		/// </summary>
		internal Color WarningStroke
		{
			get { return (Color)GetValue(WarningStrokeProperty); }
			set { SetValue(WarningStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the error stroke color of the Input.
		/// </summary>
		internal Color ErrorStroke
		{
			get { return (Color)GetValue(ErrorStrokeProperty); }
			set { SetValue(ErrorStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the filled disabled background color of the Input.
		/// </summary>
		internal Color FilledDisableBackground
		{
			get { return (Color)GetValue(FilledDisableBackgroundProperty); }
			set { SetValue(FilledDisableBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the filled hovered background color of the Input.
		/// </summary>
		internal Color FilledHoverBackground
		{
			get { return (Color)this.GetValue(FilledHoveredBackgroundProperty); }
			set { SetValue(FilledHoveredBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the disabled text color of the Input.
		/// </summary>
		internal Color DisabledTextColor
		{
			get { return (Color)GetValue(DisabledTextColorProperty); }
			set { SetValue(DisabledTextColorProperty, value); }
		}

		#endregion

		#region OnPropertyChanged

		/// <summary>
		/// Property changed method for Value property.
		/// </summary>
		/// <param name="bindable">The bindable object should be SfOtpInput.</param>
		/// <param name="oldValue">The old value of the Value property.</param>
		/// <param name="newValue">The new value of the Value property.</param>
		static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
				if(oldValue is not null && newValue is not null && newValue.ToString()?.Length > (int)otpInput.Length)
				{
					otpInput.TrimValueToLength((int)otpInput.Length);
					return;
				}

				string newValueStr = newValue?.ToString() ?? string.Empty;
				string oldValueStr = oldValue?.ToString() ?? string.Empty;
				newValueStr = new string(newValueStr.Where(c => !char.IsControl(c)).ToArray());
				oldValueStr = new string(oldValueStr.Where(c => !char.IsControl(c)).ToArray());
				RaiseValueChangedEvent(otpInput, oldValueStr, newValueStr);
                otpInput.UpdateValue(bindable, newValue?? string.Empty);
            }
        }

        /// <summary>
        /// Property changed method for Length property.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the Length property.</param>
        /// <param name="newValue">The new value of the Length property.</param>
        static void OnLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput && newValue is double newLength && oldValue is double oldLength)
            {

				if (newLength <= 0)
				{
					newLength = oldLength;
					otpInput.Length = oldLength;
					return;
				}
				else if(oldLength <= 0)
				{
					oldLength = newLength;
				}

				otpInput.UpdateEntriesLength((int)oldLength, (int)newLength);
                otpInput.UpdateValue(bindable, otpInput.Value);
                otpInput.HookEvents();
                otpInput.UpdatePlaceholderText();
                OnIsEnabledpropertyChanged(bindable, otpInput.IsEnabled, otpInput.IsEnabled);
                otpInput.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for StylingMode property.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the StylingMode property.</param>
        /// <param name="newValue">The new value of the StylingMode property.</param>
        static void OnStylingModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
				if(newValue is OtpInputStyle.Filled)
				{
					if (otpInput._otpEntries is not null)
					{
						foreach (var entry in otpInput._otpEntries)
						{
							otpInput.ApplyEntrySize(entry);
						}
					}
				}
                otpInput.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for Placeholder property.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the Placeholder property.</param>
        /// <param name="newValue">The new value of the Placeholder property.</param>
        static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
                otpInput.UpdatePlaceholderText();
                otpInput.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for Separator property.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the Separator property.</param>
        /// <param name="newValue">The new value of the Separator property.</param>
        static void OnSeparatorpropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
                otpInput.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Property changed method for Type property.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the Type property.</param>
        /// <param name="newValue">The new value of the Type property.</param>
        static void OnTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
				if (otpInput.Type is OtpInputType.Number && otpInput.Value is not null && otpInput.Value.Any(char.IsLetter))
				{
					otpInput._oldValue = otpInput.Value;
				}
				if (otpInput.Type != OtpInputType.Number && !string.IsNullOrEmpty(otpInput._oldValue))
				{
					otpInput.Value = otpInput._oldValue;
				}
				otpInput.UpdateTypeProperty();
				otpInput.UpdateKeyboardType();
                otpInput.UpdateMaskCharacter();
				if (otpInput.Value is not null)
				{
					otpInput.UpdateValue(bindable, otpInput.Value);
				}
                otpInput.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Property changed method for IsEnabled property.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the IsEnabled property.</param>
        /// <param name="newValue">The new value of the IsEnabled property.</param>
        static void OnIsEnabledpropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
                if (otpInput._otpEntries is not null)
                {
                    foreach (var field in otpInput._otpEntries)
                    {
                        field.IsEnabled = (bool)newValue;
                    }

                    otpInput.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Property changed method for AutoFocus property.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the AutoFocus property.</param>
        /// <param name="newValue">The new value of the AutoFocus property.</param>
        static void OnAutoFocusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput && otpInput._otpEntries is not null)
            {
                if ((bool)newValue)
                {
#if !(MACCATALYST || IOS)
                    otpInput.Loaded += (s, e) =>
                    {
                        otpInput._otpEntries[0].Focus();
                    };
#else
                    Task.Run(() =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            otpInput._otpEntries[0].Focus();
                        });
                    });
#endif
                }
                else
                {
                    otpInput._otpEntries[0].Unfocus();
                }

                otpInput.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the MaskCharacter property is changed.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the MaskCharacter property.</param>
        /// <param name="newValue">The new value of the MaskCharacter property.</param>
        static void OnMaskCharacterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
				if(newValue is char maskChar && maskChar is ' ')
				{
					otpInput.MaskCharacter = '●';
				}
                otpInput.UpdateMaskCharacter();
            }
        }

        /// <summary>
        /// Invoked when the InputState property is changed.Invalidates and redraws the OTP input fields to reflect the updated state.
        /// </summary>
        /// <param name="bindable">The bindable object should be SfOtpInput.</param>
        /// <param name="oldValue">The old value of the InputState property.</param>
        /// <param name="newValue">The new value of the InputState property.</param>
        static void OnInputStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
                otpInput.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Raises the ValueChanged event for the SfOtpInput control.
        /// </summary>
        /// <param name="otpInput">The SfOtpInput control.</param>
        /// <param name="oldValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        static void RaiseValueChangedEvent(SfOtpInput otpInput, string? oldValue, string? newValue)
        {
            var valueChangedEventArgs = new OtpInputValueChangedEventArgs(newValue, oldValue);
            otpInput.ValueChanged?.Invoke(otpInput, valueChangedEventArgs);
        }

		/// <summary>
		/// Invoked when the background property of the control is changed.
		/// </summary>
		/// <param name="bindable">The bindable object should be OTPEntry.</param>
		/// <param name="oldValue">The old value of the BackgroundColor property.</param>
		/// <param name="newValue">The new value of the BackgroundColor property.</param>
		static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfOtpInput otpInput)
			{
				otpInput?.InvalidateDrawable();
			}
		}


		/// <summary>
		/// Invoked when the stroke property of the control is changed.
		/// </summary>
		/// <param name="bindable">The bindable object should be OTPEntry.</param>
		/// <param name="oldValue">The old value of the Stroke property.</param>
		/// <param name="newValue">The new value of the Stroke property.</param>
		static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfOtpInput otpInput)
			{
				otpInput?.InvalidateDrawable();
			}
		}

        /// <summary>
        /// Updates all OTP entry dimensions when BoxWidth or BoxHeight changes.
        /// </summary>
        static void OnBoxSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfOtpInput otpInput)
            {
				if (otpInput.BoxWidth > 0)
				{
					otpInput._entryWidth = (float)otpInput.BoxWidth;
				}

				if (otpInput.BoxHeight > 0)
				{
					otpInput._entryHeight = (float)otpInput.BoxHeight;
				}

				if (otpInput._otpEntries is not null)
				{
					foreach (var entry in otpInput._otpEntries)
					{
						otpInput.ApplyEntrySize(entry);
					}
				}
				otpInput.InvalidateMeasure();
				otpInput.InvalidateDrawable();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method used to draw the rectangles.
        /// </summary>
        /// <param name="canvas">The canvas on which the rectangles will be drawn.</param>
        /// <param name="rectF">The bounds defining the size and position of the rectangles.</param>
        public void DrawUI(ICanvas canvas, RectF rectF)
        {
            if (_otpEntries is not null)
            {
                for (int i = 0; i < Length; i++)
                {
					UpdateDrawingParameters(i);
					canvas.StrokeSize = GetStrokeThickness(i);
                    _otpEntries[i].UpdateParameters(StylingMode, _cornerRadius, _startPoint, _endPoint, this, IsEnabled, InputState, Stroke, InputBackground, TextColor, DisabledTextColor);
                    _otpEntries[i].Draw(canvas, _outlineRectF);

                }
            }
        }

		#endregion

		#region Private Methods

        /// <summary>
        /// This method handles navigation and focus changes based on keyboard input.
        /// </summary>
        internal void HandleKeyPress(string key)
        {
            if (_otpEntries is null)
            {
                return;
            }

            int newIndex;

            switch (key)
            {
                case "Left":
                    newIndex = _focusedIndex - 1;

                    if (newIndex >= 0)
                    {
                        FocusEntry(newIndex, true);
                    }

                    break;

                case "Right":
                    newIndex = _focusedIndex + 1;

                    if (newIndex < _otpEntries.Length)
                    {
                        FocusEntry(newIndex, true);
                    }
                    
                    break;

                case "Back":
                    _oldValue = string.Empty;
                    if ((!string.IsNullOrEmpty(_otpEntries[_focusedIndex].Text)) & _otpEntries[_focusedIndex].Text is not "\0")
                    {
                        _otpEntries[_focusedIndex].Text = string.Empty;
                    }
                    else if (_focusedIndex > 0)
                    {
                        newIndex = _focusedIndex - 1;
                        _otpEntries[newIndex].Text = string.Empty;
                        FocusEntry(newIndex, false);
                    }

                    break;

                case "Tab":
                    newIndex = _focusedIndex + (_isShiftOn ? -1 : 1);
                    if (newIndex >= 0 && newIndex < _otpEntries.Length)
                    {
                        FocusEntry(newIndex, true);
                    }
					else
					{
						MoveFocusToNextElement(_isShiftOn);
					}

                    break;

                default:
					_oldValue = string.Empty;
                    char input = key.Last();
                    bool isValidInput = ((Type == OtpInputType.Number && char.IsDigit(input)) || (Type != OtpInputType.Number) );
                    if (isValidInput)
                    {
                        UpdateEntryValue(input.ToString());
                    }

                    break;
            }
        }

        /// <summary>
		/// Moves the focus to the next or previous focusable element within the parent visual tree.
		/// </summary>
		/// <param name="moveBackward"></param>
		void MoveFocusToNextElement(bool moveBackward)
		{
			if (this.Parent is not VisualElement parent)
				return;

			var focusableElements = parent.GetVisualTreeDescendants()
										  .OfType<VisualElement>()
										  .Where(e => e.IsEnabled && e.IsVisible)
										  .ToList();

			int currentIndex = focusableElements.IndexOf(this);
			if (currentIndex == -1)
				return;

			if (moveBackward)
			{
				if (currentIndex - 1 >= 0)
				{
					var previousElement = focusableElements[currentIndex - 1];
					if (previousElement is SfOtpInput otpInput && otpInput._otpEntries is not null)
					{
						otpInput._otpEntries[^1]?.Focus(); 
					}
					else
					{
						previousElement.Focus();
					}
				}
			}
			else
			{
				if (_focusedIndex == Length - 1 && currentIndex + 1 < focusableElements.Count)
				{
					var nextElement = focusableElements[currentIndex + 1];
					if (nextElement is SfOtpInput otpInput && otpInput._otpEntries is not null)
					{
						otpInput._otpEntries[0]?.Focus(); 
					}
					else
					{
						nextElement.Focus();
					}
				}
			}
		}



		/// <summary>
        /// Initializes the OTP input fields, separators, and their layout.This method dynamically creates the required number of input fields and separators based on the specified length and positions them within the layout.
        /// </summary>
        void InitializeFields()
        {
            _otpEntries = new OTPEntry[(int)Length];
            _entryBounds = new RectF[(int)Length];
            _separators = new SfLabel[(int)Length - 1];

            var layout = new AbsoluteLayout();
#if IOS
            layout.IgnoreSafeArea = true;
#endif
			layout.BindingContext = this;
#if WINDOWS || ANDROID
			layout.SetBinding(AbsoluteLayout.FlowDirectionProperty, BindingHelper.CreateBinding(nameof(FlowDirection), getter: static (SfOtpInput otpInput) => otpInput.FlowDirection));

#elif MACCATALYST || IOS
			layout.SetBinding(AbsoluteLayout.FlowDirectionProperty,BindingHelper.CreateBinding(nameof(FlowDirection),getter: static (SfOtpInput otpInput) =>
			{
				VisualElement? parent = otpInput.Parent as VisualElement;

				while (parent != null && parent.FlowDirection == FlowDirection.MatchParent)
				{
					parent = parent.Parent as VisualElement;
				}

				return otpInput.FlowDirection == FlowDirection.MatchParent
					? (parent?.FlowDirection ?? FlowDirection.LeftToRight)
					: otpInput.FlowDirection;
				}
				)
			);
#endif

			for (int i = 0; i < Length; i++)
            {
                OTPEntry otpEntry = InitializeEntry();
                _otpEntries[i] = otpEntry;
                layout.Children.Add(otpEntry);

                // Add separator only if it's not the last entry.
                if (i < Length - 1)
                {
                    SfLabel separatorlabel = InitializeSeparator();
                    _separators[i] = separatorlabel;
                    layout.Children.Add(separatorlabel);
                }
            }

            Children.Clear();
            Children.Add(layout);
        }

        /// <summary>
        /// Initializes a separator label to be placed between OTP input fields.
        /// </summary>
        /// <returns>A new instance of the <see cref="SfLabel"/> configured as a separator.</returns>
        SfLabel InitializeSeparator()
        {
            return new SfLabel
            {
                Text = Separator,
                FontSize = _separatorTextSize,
                HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.NoWrap,
                TextColor = SeparatorColor
            };
        }

        /// <summary>
        /// Initializes an OTP input entry field at the specified index.
        /// </summary>
        /// <returns>A new instance of the <see cref="OTPEntry"/> configured for OTP input.</returns>
        OTPEntry InitializeEntry()
        {
            OTPEntry otpEntry = new OTPEntry
            {
            	FontSize = 16,
            	HorizontalTextAlignment = TextAlignment.Center,
            	VerticalTextAlignment = TextAlignment.Center,
            	Keyboard = Type is OtpInputType.Number ? Keyboard.Numeric : Keyboard.Text,
            };

            ApplyEntrySize(otpEntry);
            otpEntry.BindingContext = this;
            otpEntry.SetBinding(OTPEntry.PlaceholderColorProperty, BindingHelper.CreateBinding(nameof(PlaceholderColor), getter: static (SfOtpInput otpInput) => otpInput.PlaceholderColor));
            otpEntry.SetBinding(OTPEntry.TextColorProperty, BindingHelper.CreateBinding(nameof(TextColor), getter: static (SfOtpInput otpInput) => otpInput.TextColor));

            return otpEntry;
        }

        /// <summary>
        /// Applies the current entry size to the given OTPEntry.
        /// </summary>
        void ApplyEntrySize(OTPEntry entry)
		{
			entry.MinimumWidthRequest = _entryWidth;
			entry.WidthRequest = _entryWidth;

#if WINDOWS
			float height = (StylingMode == OtpInputStyle.Filled) ? _entryHeight - _strokeThickness : _entryHeight;
#else
			float height = _entryHeight;
#endif

			entry.MinimumHeightRequest = height;
			entry.HeightRequest = height;
		}

        /// <summary>
        /// Handles the event when an OTP input field receives focus.
        /// Updates the focused index and redraws the control.
        /// </summary>
        /// <param name="sender">The source of the focus event.</param>
        /// <param name="e">The event data for the focus event.</param>
        void FocusAsync(object? sender, FocusEventArgs e)
        {
            _focusedIndex = Array.IndexOf(_otpEntries!, sender);
			if (sender is Entry entry)
			{
				entry.CursorPosition = 0;
				entry.SelectionLength = entry.Text?.Length ?? 0;
#if WINDOWS
				if (_otpEntries is not null && _focusedIndex == Length - 1 && _isPaste)
				{
					_isPaste = false;
					char[] text = _otpEntries[_focusedIndex].Text.ToCharArray();
					if (_otpEntries[_focusedIndex].Text is not "" && text[0] is not '\0')
					{
						_otpEntries[_focusedIndex].Unfocus();
						
					}
				}
#endif
			}

			InvalidateDrawable();
        }

        /// <summary>
        /// Handles the event when an OTP input field loses focus.
        /// Redraws the control to update the visual state.
        /// </summary>
        /// <param name="sender">The source of the unfocus event.</param>
        /// <param name="e">The event data for the unfocus event.</param>
        void FocusOutAsync(object? sender, FocusEventArgs e)
        {
            InvalidateDrawable();
        }

        /// <summary>
        /// Focuses the OTP entry at the specified index and sets the cursor position.
        /// </summary>
        /// <param name="index">The index of the OTP entry to focus.</param>
        /// <param name="setCursorToStart">If true, the cursor is set to the start; otherwise, it is set to position 1.</param>
        void FocusEntry(int index, bool setCursorToStart)
        {
            if (_otpEntries is not null)
            {
                _focusedIndex = index;
                _otpEntries[_focusedIndex].Focus();
                _otpEntries[_focusedIndex].CursorPosition = setCursorToStart ? 0 : 1;
            }
        }

		/// <summary>
		/// Handles the text change event for the OTP entry fields.
		/// </summary>
		/// <param name="sender">The OTP entry where the text changed.</param>
		/// <param name="e">The event arguments containing text change details.</param>
		void OnEntryTextChanged(object? sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
		{
			if (sender is not OTPEntry otpEntry || _otpEntries is null || !this.IsLoaded)
			{
				return;
			}

			int index = Array.IndexOf(_otpEntries, otpEntry);
			if (index < 0 || index >= Length)
			{
				return;
			}
#if ANDROID || WINDOWS
			if (e.NewTextValue.Length <= 1)
			{
#endif
				string currentValue = Value?.PadRight((int)Length, '\0') ?? new string('\0', (int)Length);
				char[] valueArray = currentValue.ToCharArray();

				bool hasText = !string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue is not "\0";
				bool isValidText = (string.IsNullOrEmpty(e.NewTextValue) || e.NewTextValue is "\0" || char.IsLetterOrDigit(char.Parse(e.NewTextValue))) && (!string.IsNullOrEmpty(MaskCharacter.ToString()));
				valueArray[index] = hasText ? e.NewTextValue[0] : '\0';

#if (MACCATALYST || IOS)
			{
				Task.Run(() =>
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						if (Type is OtpInputType.Password && e.NewTextValue is not "")
						{
							_otpEntries[index].Text = MaskCharacter.ToString();
						}

						if (isValidText)
						{
							Value = new string(valueArray);
						}
						_oldText = e.OldTextValue;
						HandleFocus(index, hasText);
					});
				});
			}
#else
				{
					if (Type is OtpInputType.Password && e.NewTextValue is not "")
					{
						_otpEntries[index].Text = MaskCharacter.ToString();
					}

					if (isValidText)
					{
						Value = new string(valueArray);
					}

					HandleFocus(index, hasText);
				}
#endif

#if ANDROID || WINDOWS
			}
			else
			{
#if ANDROID
				if (index == 0 && _isPaste)
#elif WINDOWS
				if (index == 0)
#endif
				{
					_isPasteHandled = true;
					_isPaste = true;
					if (Type is OtpInputType.Password && e.NewTextValue is not "")
					{
						_otpEntries[index].Text = MaskCharacter.ToString();
					}
					else
					{
						_otpEntries[index].Text = e.OldTextValue;
					}
				}
#if ANDROID
				else if(e.NewTextValue.Length == 2 && !_isPaste)
				{
					if (Type == OtpInputType.Password && e.NewTextValue is not "")
					{
						_otpEntries[index].Text = MaskCharacter.ToString();
					}
					else
					{
						string newValue = e.NewTextValue.ToString();
						_otpEntries[index].Text = newValue[1].ToString();
					}
				}
#endif
				else
				{
						_otpEntries[index].Text = string.Empty;
				}
			}
#endif
		}

		/// <summary>
		/// Handles focus movement between OTP input fields based on the specified index and text presence.
		/// </summary>
		/// <param name="index">The current index of the OTP input field that triggered the focus change.</param>
		/// <param name="hasText">A boolean indicating whether the current field contains text.</param>
		void HandleFocus(int index, bool hasText)
        {
            if (_otpEntries is not null)
            {
                if (hasText)
                {
#if ANDROID || WINDOWS
					if(_isPasteHandled && _isPaste)
					{
						_isPasteHandled = false;
						_isPaste = false;
						_otpEntries[index].Unfocus();
#if WINDOWS
						if (_otpEntries is not null && Value.Length == Length)
						{
							char[] text = _otpEntries[(int)Length-1].Text.ToCharArray();
							if (_otpEntries[(int)Length - 1].Text is not "" && text[0] is not '\0')
							{
								_otpEntries[(int)Length - 1].Unfocus();
							}
						}
#endif
						return;
					}
#endif
					if (index < Length - 1)
                    {
                        _otpEntries[index + 1].Focus();
                    }
                    else if (index == Length - 1)
                    {
						_otpEntries[index].Unfocus();
                    }
                }
			}
		}

        /// <summary>
        /// Determines the stroke size of the OTP entry based on focus state.
        /// </summary>
        /// <param name="index">The index of the OTP entry.</param>
        /// <returns>Returns the stroke size for the OTP entry.</returns>
        float GetStrokeThickness(int index)
        {
            return _otpEntries is not null && _otpEntries[index].IsFocused ? 2 : 1;
        }

		/// <summary>
		/// Updates the drawing parameters such as line points and rectangle boundaries for an OTP entry.
		/// </summary>
		/// <param name="index">The index of the OTP entry to update parameters for.</param>
		void UpdateDrawingParameters(int index)
		{
			bool isRTL = (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft);

			// Base calculations
			float baseXPos = index * (_entryWidth + _spacing) + _extraSpacing;
			float xPos = 0;
			float yPadding = 0;

#if ANDROID
            yPadding = 2;
#elif WINDOWS || MACCATALYST
			baseXPos += _platformSpecificPadding;
			yPadding = _platformSpecificPadding;
#endif

            // Adjust for RTL
            xPos = isRTL
            	? (float)(this.Width - _entryWidth - baseXPos)
            	: baseXPos;

            _startPoint = new PointF(xPos, _entryHeight + _extraSpacing + yPadding);
            _endPoint = new PointF(xPos + _entryWidth, _entryHeight + _extraSpacing + yPadding);
			_outlineRectF = new RectF(xPos, _extraSpacing + yPadding, _entryWidth, _entryHeight);
		}

		/// <summary>
		/// Updates the OTP input fields values based on the bound value.
		/// </summary>
		/// <param name="bindable">The bindable object associated with this method.</param>
		/// <param name="value">The new value to update the OTP fields with.</param>
		void UpdateValue(BindableObject bindable, object value)
        {
            var otpInput = bindable as SfOtpInput;
            string? newValue = value as string;

            if (otpInput?._otpEntries is not null && newValue is not null)
            {
                newValue = !string.IsNullOrEmpty(_oldValue) ? _oldValue : newValue;

                for (int i = 0; i < otpInput._otpEntries.Length; i++)
                {
                    if (i < newValue.Length)
                    {
                        char enteredCharacter = newValue[i];
                        if (Type != OtpInputType.Password)
                        {
                            if (!char.IsLetterOrDigit(enteredCharacter) || (Type is OtpInputType.Number && !char.IsDigit(enteredCharacter)))
                            {
                                otpInput._otpEntries[i].Text = string.Empty;
                            }
                            else
                            {
                                otpInput._otpEntries[i].Text = enteredCharacter.ToString();
                            }
                        }
						else if(char.IsLetterOrDigit(enteredCharacter))
						{
							otpInput._otpEntries[i].Text = MaskCharacter.ToString();
						}
                    }
                    else
                    {
                        otpInput._otpEntries[i].Text = string.Empty;
                    }
                }
            }
		}

        /// <summary>
        /// Updates the length of OTP input fields when the length property changes.
        /// </summary>
        /// <param name="oldLength">The previous number of OTP input fields.</param>
        /// <param name="newLength">The new desired number of OTP input fields.</param>
        void UpdateEntriesLength(int oldLength, int newLength)
        {
            if (_otpEntries is null) 
            { 
                return;
            }

			var layout = Children[0] as AbsoluteLayout;
            if (newLength > oldLength)
            {
                AddEntry(oldLength, newLength, layout);
            }
            else if (newLength < oldLength)
            {
                RemoveEntry(oldLength, newLength, layout);
            }
        }

		/// <summary>
		/// Trims the value to the specified length if it exceeds the given length.
		/// </summary>
		/// <param name="length">Length of the OTPInput</param>
		void TrimValueToLength(int length)
		{
			if(!string.IsNullOrEmpty(Value) && Value.Length > length)
			{
				Value = Value.Substring(0, length);
			}
		}

        /// <summary>
        /// Removes OTP entry fields and separators when the length is reduced.
        /// </summary>
        /// <param name="oldLength">The previous number of OTP input fields.</param>
        /// <param name="newLength">The desired new number of OTP input fields.</param>
        /// <param name="layout">The AbsoluteLayout container holding the OTP input fields.</param>
        void RemoveEntry(int oldLength, int newLength, AbsoluteLayout? layout)
        {
            if (_otpEntries is not null)
            {
                for (int i = oldLength - 1; i >= newLength; i--)
                {
                    layout?.Children.Remove(_otpEntries[i]);
                    DetachEventsForEntry(i);
                    if (i >= newLength && i is not 0)
                    {
                        layout?.Children.Remove(_separators[i - 1]);
                    }
                }

                _otpEntries =_otpEntries.Take(newLength).ToArray();
                _entryBounds = _entryBounds.Take(newLength).ToArray();
                _separators = _separators.Take(newLength - 1).ToArray();
            }
        }

		/// <summary>
		/// Adds new OTP entry fields and separators when the length is increased.
		/// </summary>
		/// <param name="oldLength">The previous number of OTP input fields.</param>
		/// <param name="newLength">The new number of OTP input fields.</param>
		/// <param name="layout">The AbsoluteLayout container holding the OTP input fields.</param>
		void AddEntry(int oldLength, int newLength, AbsoluteLayout? layout)
		{
			if (_otpEntries is null)
			{
				return;
			}

			for (int i = oldLength; i < newLength; i++)
			{
				if (i < newLength && i is not 0)
				{
					SfLabel label = InitializeSeparator();
					SetSeparatorPosition(i, label);
					layout?.Children.Add(label);
				}

				OTPEntry otpEntry = InitializeEntry();
				AttachEvents(otpEntry);
                _otpEntries = _otpEntries.Concat(new[] { otpEntry }).ToArray();
				SetInputFieldPosition(i, otpEntry);
				layout?.Children.Add(otpEntry);
			}
		}

		/// <summary>
		/// Sets the position of an OTP input field within the SfOtpInput control.
		/// </summary>
		/// <param name="i">The instance of the SfOtpInput control which contains the OTP entries.</param>
		/// <param name="otpEntry">The OTPEntry instance being positioned.</param>
		void SetInputFieldPosition(int i, OTPEntry otpEntry)
		{
			if (otpEntry is null)
			{
				return;
			}

			RectF rect = new RectF(
				(_entryWidth + _spacing) * i,
				0,
				_entryWidth,
				_entryHeight);

			_entryBounds = _entryBounds.Concat(new[] { rect }).ToArray();
			AbsoluteLayout.SetLayoutBounds(otpEntry, new Rect(rect.X, rect.Y, rect.Width, rect.Height));

			float entryX = ((_entryWidth + _spacing) * i) + _extraSpacing;
			float entryY = _extraSpacing ;
			_entryBounds[i] = new RectF(entryX, entryY, _entryWidth, _entryHeight);
			AbsoluteLayout.SetLayoutBounds(otpEntry, new Rect(_entryBounds[i].X, _entryBounds[i].Y, _entryBounds[i].Width, _entryBounds[i].Height));
		}

		/// <summary>
		/// Sets the position of the seperator within the SfOtpInput control
		/// </summary>
		/// <param name="i">The index indicating where between input fields the separator should be positioned.</param>
		/// <param name="label">The SfLabel instance representing the separator to be positioned.</param>
		void SetSeparatorPosition(int i, SfLabel label)
		{
			float entryX = (_entryWidth + _spacing) * i;
			float separatorX = entryX + _entryWidth + _spacing / 2;
			RectF separatorRect = new RectF(separatorX, 0, _separatorWidth, _separatorHeight);

			_separators = _separators.Concat(new[] { label }).ToArray();
			AbsoluteLayout.SetLayoutBounds(label, new Rect(separatorRect.X, separatorRect.Y, separatorRect.Width, separatorRect.Height));
		}

		/// <summary>
		/// Retrieves the placeholder value for OTP input fields.
		/// </summary>
		/// <returns>
		/// A formatted string based on the placeholder value. If the placeholder length is less than the total OTP input field length, it appends empty characters.
		/// </returns>
		string GetPlaceHolder()
        {
            if (string.IsNullOrEmpty(Placeholder))
            {
                return string.Empty;
            }

            if (Placeholder.Length is 1)
            {
                return new string(Placeholder[0], _otpEntries?.Length ?? 0);
            }

            if (_otpEntries is not null)
            {
                if (Placeholder.Length < _otpEntries.Length)
                {
                    return Placeholder + new string('\0', _otpEntries.Length - Placeholder.Length);
                }
            }

            return Placeholder;
        }

        /// <summary>
        /// Updates the placeholder text for all OTP input fields based on the Placeholder property.
        /// </summary>
        void UpdatePlaceholderText()
        {
            if (_otpEntries is null || Placeholder is null)
            {
                return;
            }

            string actualPlaceholder = GetPlaceHolder();
            for (int i = 0; i < _otpEntries.Length; i++)
            {
                _otpEntries[i].Placeholder = actualPlaceholder.Length > i ? actualPlaceholder[i].ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Updates the text value of the currently focused OTP entry field based on the key pressed.
        /// </summary>
        /// <param name="key">The key input value to be assigned to the OTP entry.</param>
        void UpdateEntryValue(string key)
        {
            if (_otpEntries is not null)
            {
				char input = key[key.Length - 1];
#if WINDOWS
				if (_isCapsOn || _isShiftOn)
				{
					_otpEntries[_focusedIndex].Text = input.ToString().ToUpper(CultureInfo.CurrentCulture);
				}
				else
				{
					_otpEntries[_focusedIndex].Text = input.ToString().ToLower(CultureInfo.CurrentCulture);
				}
#else
				_otpEntries[_focusedIndex].Text = input.ToString();
#endif
			}
        }

        /// <summary>
        /// Updates the properties of OTP input fields when the Type property is changed.
        /// This includes enabling/disabling password masking and clearing invalid text.
        /// </summary>
        void UpdateTypeProperty()
        {
            if (_focusedIndex < 0 || _otpEntries is null || Length == 0)
            {
                return;
            }

            if (_otpEntries[_focusedIndex].Text is not "")
            {
                for (int i = 0; i < _otpEntries.Length; i++)
                {
                    if (Type == OtpInputType.Password && _otpEntries[i].Text is not "")
                    {
                        _otpEntries[i].Text = MaskCharacter.ToString();
                    }
                    else if (Type == OtpInputType.Number && _otpEntries[i].Text.Any(char.IsLetter))
                    {
                        _otpEntries[i].Text = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Helps to wire the event handlers to the OTP entry fields.
        /// </summary>
        void HookEvents()
        {
            UnHookEvents();
            if (_otpEntries is not null)
            {
                foreach (var otpEntry in _otpEntries)
				{
					AttachEvents(otpEntry);
				}
			}
        }

		/// <summary>
		/// Helps to Unwire the event handlers to the OTP entry fields.
		/// </summary>
		void UnHookEvents()
        {
            if (_otpEntries is not null)
            {
                foreach (var otpEntry in _otpEntries)
                {
                    DetachEvents(otpEntry);
                }
            }
        }

        /// <summary>
        /// Detaches event handlers from a specific OTP entry field when its length is changed.
        /// </summary>
        /// <param name="i">The index of the OTP input field to remove event handlers from.</param>
        void DetachEventsForEntry(int i)
        {
            if (_otpEntries is not null)
            {
                 DetachEvents(_otpEntries[i]);
            }
        }

		/// <summary>
		/// Attaches event handlers to a specified OTP entry field.
		/// </summary>
		/// <param name="otpEntry">The OTP entry field from which to attach event handlers.</param>
		void AttachEvents(OTPEntry otpEntry)
		{
			otpEntry.HandlerChanged += OnHandlerChanged;
			otpEntry.TextChanged += OnEntryTextChanged;
			otpEntry.Focused += FocusAsync;
			otpEntry.Unfocused += FocusOutAsync;
		}

		/// <summary>
		/// Detaches event handlers from a specified OTP entry field.
		/// </summary>
		/// <param name="otpEntry">The OTP entry field to which to attach event handlers.</param>
		void DetachEvents(OTPEntry otpEntry)
		{
			otpEntry.HandlerChanged -= OnHandlerChanged;
			otpEntry.TextChanged -= OnEntryTextChanged;
			otpEntry.Focused -= FocusAsync;
			otpEntry.Unfocused -= FocusOutAsync;
		}

#if WINDOWS
        /// <summary>
        /// Handles key down events for OTP input fields.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments containing key information.</param>
        void OnKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (_otpEntries is null)
            {
                return;
            }

            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                    HandleKeyPress("Left");
                    e.Handled = true;
                    break;

                case Windows.System.VirtualKey.Right:
                    HandleKeyPress("Right");
                    e.Handled = true;
                    break;

                case Windows.System.VirtualKey.Back:
                    HandleKeyPress("Back");
                    e.Handled = true;
                    break;

                case Windows.System.VirtualKey.CapitalLock:
                    _isCapsOn = GetVirtualKeyStates(Windows.System.VirtualKey.CapitalLock).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Locked);
                    break;

                case Windows.System.VirtualKey.Tab:
					_isShiftOn = GetVirtualKeyStates(Windows.System.VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
                    HandleKeyPress("Tab");
                    e.Handled = true;
                    break;

                case Windows.System.VirtualKey.Shift:
                    _isShiftOn = true;
                    e.Handled = true;
                    break;

				case Windows.System.VirtualKey.Control:
					_isCtrlPressed = true;
					e.Handled = true;
					break;

				case Windows.System.VirtualKey.V:
					if (_isCtrlPressed)
					{
						_isPaste = true;
						HandlePaste();
					}
					else
					{
						string text = e.Key.ToString();
						HandleKeyPress(text);
					}

					e.Handled = true;
					break;

				default:
                    if ((e.Key >= Windows.System.VirtualKey.A && e.Key <= Windows.System.VirtualKey.Z) ||
                        (e.Key >= Windows.System.VirtualKey.Number0 && e.Key <= Windows.System.VirtualKey.Number9))
                    {
                        string text = e.Key.ToString();
                        HandleKeyPress(text);
                    }
                    e.Handled = true;
                    break;
            }
        }

		/// <summary>
		/// Handles key up events for OTP input fields.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">Event arguments containing key information.</param>
		void OnKeyUp(object sender, KeyRoutedEventArgs e)
		{
			if(e.Key is Windows.System.VirtualKey.Shift)
			{
				_isShiftOn = false;
			}
		}

		/// <summary>
		/// Retrieves the current state of a specified virtual key for the current thread.
		/// </summary>
		/// <param name="virtualKey">The virtual key whose state needs to be retrieved.</param>
		/// <returns>
		/// A <see cref="Windows.UI.Core.CoreVirtualKeyStates"/> value that indicates whether the key is up, down, or locked.
		/// </returns>
		static Windows.UI.Core.CoreVirtualKeyStates GetVirtualKeyStates(Windows.System.VirtualKey virtualKey)
		{
			return Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(virtualKey);
		}

#endif


#if ANDROID
        /// <summary>
        /// Handles key press events for OTP input fields.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments containing key information.</param>
        void OnKeyPress(object? sender, Android.Views.View.KeyEventArgs e)
        {

			if (e.Event is null || e.Event.Action is not Android.Views.KeyEventActions.Down)
            {
				return;
            }
		    
			switch (e.KeyCode)
            {
                case Android.Views.Keycode.Del:
                    HandleKeyPress("Back");
                    e.Handled = true;
                    break;

                case Android.Views.Keycode.DpadLeft:
                    HandleKeyPress("Left");
                    e.Handled = true;
                    break;

                case Android.Views.Keycode.DpadRight:
                    HandleKeyPress("Right");
                    e.Handled = true;
                    break;

                case Android.Views.Keycode.Tab:
                    HandleKeyPress("Tab");
                    e.Handled = true;
                    break;

                default:
                    if ((e.KeyCode >= Android.Views.Keycode.A && e.KeyCode <= Android.Views.Keycode.Z) ||
                    (e.KeyCode >= Android.Views.Keycode.Num0 && e.KeyCode <= Android.Views.Keycode.Num9))
                    {
                        string text =e.KeyCode.ToString();
                        HandleKeyPress(text);
                    }
                    break;
            }
            
        }
#endif

		/// <summary>
		/// Updates the size of the separator text based on its content and font size.
		/// </summary>
		void UpdateSeparatorSize()
        {
            var size = TextMeasurer.CreateTextMeasurer().MeasureText(Separator, (float)_separatorTextSize);
            _separatorHeight = (float)size.Height;
            _separatorWidth = (float)size.Width + Separator.Length;
        }

#if MACCATALYST || IOS
        /// <summary>
        /// Validates the input text for the OTP entry field, handling backspace and alphanumeric input.
        /// </summary>
        /// <param name="textField">The UITextField where the input is occurring.</param>
        /// <param name="range">The range of the text to be replaced.</param>
        /// <param name="inputText">The new string being entered.</param>
        /// <returns>
        /// Returns <c>true</c> if the input should proceed; otherwise, <c>false</c> to prevent input.
        /// </returns>
        bool ValidateText(UITextField textField, NSRange range, string inputText)
        {
            if (_otpEntries is null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(inputText))
            {
                return true;
            }

			if (inputText.Length > 1)
			{
				HandlePaste();
				return false;
			}

			char enteredText = inputText[0];
            if (char.IsLetterOrDigit(enteredText))
            {
                HandleKeyPress(enteredText.ToString());
                return false;
            }

            return false;
        }
#endif

        /// <summary>
        /// Handles the key-down event for the OTP input field and invokes the <c>OnPreviewKeyDown</c> method.
        /// </summary>
        /// <param name="args">The key event arguments associated with the key-down event.</param>
        void IKeyboardListener.OnKeyDown(KeyEventArgs args)
        {
#if MACCATALYST || IOS
            OnPreviewKeyDown(args);
#endif
        }

        /// <summary>
        /// Handles the key-up event for the OTP input field.
        /// </summary>
        /// <param name="args">The key event arguments associated with the key-up event.</param>
        void IKeyboardListener.OnKeyUp(KeyEventArgs args)
        {
#if MACCATALYST || IOS
			if(!args.IsShiftKeyPressed)
			{
				_isShiftOn = false;
			}
#endif
		}

#if MACCATALYST || IOS
        /// <summary>
        /// Processes key-down events for OTP input fields.
        /// Handles navigation keys, backspace, delete, caps lock, and alphanumeric input.
        /// </summary>
        /// <param name="e">The key event arguments containing the key pressed.</param>
        void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (_otpEntries is null)
            {
                return;
            }

            string text = e.Key.ToString();
            switch (e.Key)
            {
                case KeyboardKey.Left:
                    HandleKeyPress("Left");
                    e.Handled = true;
                    break;

                case KeyboardKey.Right:
                    HandleKeyPress("Right");
                    e.Handled = true;
                    break;

                case KeyboardKey.Back:
                    HandleKeyPress("Back");
                    e.Handled = true;
                    break;

                case KeyboardKey.Delete:
                    HandleKeyPress("Back");
                    e.Handled = true;
                    break;

                case KeyboardKey.Tab:
					_isShiftOn = e.IsShiftKeyPressed;
                    HandleKeyPress("Tab");
                    e.Handled = true;
                    break;

                case KeyboardKey.Shift:
                    _isShiftOn = true;
                    break;

                default:
                    if ((e.Key >= KeyboardKey.A && e.Key <= KeyboardKey.Z) ||
                        (e.Key >= KeyboardKey.Num0 && e.Key <= KeyboardKey.Num9))
                    {
                        HandleKeyPress(text);
                    }
                    e.Handled = true;
                    break;
            }
        }
#endif

        /// <summary>
        /// Handles the handler change event for OTP entry fields.
        /// Sets up platform-specific event handlers for key input validation and handling.
        /// </summary>
        /// <param name="sender">The OTPEntry control whose handler has changed.</param>
        /// <param name="e">Event arguments containing details of the change.</param>
        void OnHandlerChanged(object? sender, EventArgs e)
        {
            if (sender is OTPEntry textBox)
            {
#if MACCATALYST || IOS
				// Unhook from previous handler specific to this OTPEntry
				if (_platformViews.TryGetValue(textBox, out var previousPlatformView))
				{
					previousPlatformView.ShouldChangeCharacters -= ValidateText;
					_platformViews.Remove(textBox);
				}
#endif

#if WINDOWS
                if ((sender as OTPEntry)?.Handler is not null && (sender as OTPEntry)?.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.TextBox platformView)
                {
					platformView.Paste += OnPaste;
                    platformView.PreviewKeyDown+=OnKeyDown;
					platformView.PreviewKeyUp += OnKeyUp;
                }
#elif ANDROID
                if ((sender as OTPEntry)?.Handler is not null && (sender as OTPEntry)?.Handler?.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText platformView)
                {
					platformView.KeyPress += OnKeyPress;
					platformView.BeforeTextChanged += OnBeforeTextChanged;
                }

#elif MACCATALYST || IOS
				if (textBox.Handler?.PlatformView is UIKit.UITextField platformView)
				{
					platformView.ShouldChangeCharacters += ValidateText;
					_platformViews[textBox] = platformView;
				}
#endif
			}
		}


#if ANDROID

		/// <summary>
		/// Event handler for the `OnBeforeTextChanged` event.
		/// </summary>
		/// <param name="sender">The source of the event (usually the text input field).</param>
		/// <param name="e">Event arguments containing information about the text change.</param>
		void OnBeforeTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
		{
			if (e.AfterCount-e.BeforeCount >1 && _focusedIndex==0)
			{
				_isPaste = true;
				HandlePaste();
			}
		}

#endif


#if WINDOWS

		/// <summary>
		/// Event handler for the paste event in a text control.
		/// </summary>
		/// <param name="sender">The source of the event (usually the text control where the paste action is happening).</param>
		/// <param name="e">Event arguments containing information about the paste operation.</param>
		void OnPaste(object sender, Microsoft.UI.Xaml.Controls.TextControlPasteEventArgs e)
		{
			_isPaste = true;
			HandlePaste();
		}
#endif

		/// <summary>
		/// Updates the text of all OTP input fields with the masked character if the input type is set to Password.
		/// </summary>
		void UpdateMaskCharacter()
        {
            if (_otpEntries is null || Value is null || Type is not OtpInputType.Password)
            {
                return;
            }
            
            foreach (var otpEntry in _otpEntries)
            {
				if (otpEntry.Text is not "")
				{
					otpEntry.Text = MaskCharacter.ToString();
				}
            }
        }

		/// <summary>
		/// Handles the paste operation when triggered. This method processes the clipboard content and performs necessary actions depending on the content type or context.
		/// </summary>
#if ANDROID || MACCATALYST || IOS || WINDOWS
		async void HandlePaste()
#else
		void HandlePaste()
#endif
		{
			string? copiedText = null;

#if MACCATALYST || IOS
			if (Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.HasText)
			{
				copiedText = await Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.GetTextAsync();
			}
#elif WINDOWS
			if (Clipboard.Default.HasText) 
			{
				copiedText = await Clipboard.Default.GetTextAsync();
				_isCtrlPressed = false;
			}
#elif ANDROID
			if (Clipboard.HasText)
			{
				copiedText = await Clipboard.Default.GetTextAsync();
			}
#endif
			if (!string.IsNullOrEmpty(copiedText))
			{
				Value = new string(copiedText);
			}
		}
		
        /// <summary>
        /// Updates the keyboard type for each OTP entry based on the input type of the control.
        /// </summary>
        void UpdateKeyboardType()
        {
            if (_otpEntries is not null)
            {
                for (int i = 0; i < _otpEntries.Length; i++)
                {
                    if (Type == OtpInputType.Password || Type == OtpInputType.Text)
                    {
                        _otpEntries[i].Keyboard = Keyboard.Text;
                    }
                    else
                    {
                        _otpEntries[i].Keyboard = Keyboard.Numeric;
                    }
                }
            }
        }
#endregion

		#region Override methods

		/// <summary>
		/// Arranges the layout and positions of OTP input fields and separators within the specified bounds, dynamically calculating positions to ensure proper alignment and spacing.
		/// </summary>
		/// <inheritdoc/>
		protected override Size ArrangeContent(Rect bounds)
		{
			if (_otpEntries == null || _separators == null)
			{
				return base.ArrangeContent(bounds);
			}

			TrimValueToLength((int)Length);
			float yPadding = 0;
			float platformSpecificOffset = _extraSpacing;

#if WINDOWS || MACCATALYST
			platformSpecificOffset += _platformSpecificPadding;
			yPadding = _platformSpecificPadding;
#elif ANDROID
            yPadding = 2;
#endif

			UpdateSeparatorSize();
			_spacing = _separatorWidth + _extraSpacing;

			for (int i = 0; i < Length; i++)
			{
				OTPEntry otpEntry = _otpEntries[i];

				float entryX = ((_entryWidth + _spacing) * i) + platformSpecificOffset;
				float entryY = _extraSpacing + yPadding;

				_entryBounds[i] = new RectF(entryX, entryY, _entryWidth, _entryHeight);
				AbsoluteLayout.SetLayoutBounds(otpEntry, new Rect(_entryBounds[i].X, _entryBounds[i].Y, _entryBounds[i].Width, _entryBounds[i].Height));

				if (i < Length - 1)
				{
					SfLabel separatorLabel = _separators[i];
					separatorLabel.Text = Separator;
					float separatorX = entryX + _entryWidth + ((_spacing - _separatorWidth) / 2);

					AbsoluteLayout.SetLayoutBounds(separatorLabel, new Rect(separatorX,entryY-(_extraSpacing/2), _separatorWidth, _entryHeight+_extraSpacing));
				}
			}

			this.MinimumWidthRequest = (Length * _entryWidth) + (_spacing * (Length - 1)) + (_extraSpacing * 2);
			InvalidateDrawable();

			return base.ArrangeContent(bounds);
		}

		/// <summary>
		/// Draws the OTP input UI on the canvas.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			base.OnDraw(canvas, dirtyRect);
			DrawUI(canvas, dirtyRect);
		}

#endregion

		#region Interface Implementation

		/// <inheritdoc/>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfOtpInputStyles();
		}

		/// <inheritdoc/>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{

		}

		/// <inheritdoc/>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		#endregion
	}
}
