using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.EffectsView
{
    /// <summary>
    /// <see cref="SfEffectsView"/> is a container control that provides the out-of-the-box effects
    /// such as highlight, ripple, selection, scaling, and rotation.
    /// </summary>
    /// <example>
    /// The following examples show how to initialize the badge view.
    /// # [XAML](#tab/tabid-1)
    /// <code><![CDATA[
    /// <effectsView:SfEffectsView>
    ///
    ///     <effectsView:SfEffectsView.Content>
    ///         <Button Text="Content"
    ///                 WidthRequest="120"
    ///                 HeightRequest="60"/>
    ///     </effectsView:SfEffectsView.Content>
    ///
    /// </effectsView:SfEffectsView>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfEffectsView effectsView = new SfEffectsView();
    ///
    /// Button button = new Button();
    /// button.Text = "Content";
    /// button.WidthRequest = 120;
    /// button.HeightRequest = 60;
    ///
    /// effectsView.Content = button;
    /// Content = effectsView;
    /// ]]></code>
    /// ***
    /// </example>
    [DesignTimeVisible(true)]
    [ContentProperty(nameof(Content))]
    public class SfEffectsView : SfContentView, ITouchListener, ITapGestureListener, ILongPressGestureListener, IParentThemeElement
    {
        #region Fields

        const float _anchorValue = 0.5005f;

        HighlightEffectLayer? _highlightEffectLayer;

        SelectionEffectLayer? _selectionEffectLayer;

        RippleEffectLayer? _rippleEffectLayer;

        bool _isSelect;

        bool _isSelectedCalled;

        bool _canRepeat;

        double _tempScaleFactor;

        readonly string _rotationAnimation = "Rotation";

        readonly string _scaleAnimation = "Scaling";

        readonly string _highlightAnimation = "Highlight";

        Point _touchDownPoint;

        RectF _elementBounds;

        #endregion

        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="RippleAnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RippleAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty RippleAnimationDurationProperty =
            BindableProperty.Create(
                nameof(RippleAnimationDuration),
                typeof(double),
                typeof(SfEffectsView),
                180d,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ScaleAnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScaleAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty ScaleAnimationDurationProperty =
            BindableProperty.Create(
                nameof(ScaleAnimationDuration),
                typeof(double),
                typeof(SfEffectsView),
                150d,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="RotationAnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RotationAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty RotationAnimationDurationProperty =
            BindableProperty.Create(
                nameof(RotationAnimationDuration),
                typeof(double),
                typeof(SfEffectsView),
                200d,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="InitialRippleFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="InitialRippleFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty InitialRippleFactorProperty =
            BindableProperty.Create(
                nameof(InitialRippleFactor),
                typeof(double),
                typeof(SfEffectsView),
                0.25d,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ScaleFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScaleFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty ScaleFactorProperty =
            BindableProperty.Create(
                nameof(ScaleFactor),
                typeof(double),
                typeof(SfEffectsView),
                1d,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="HighlightBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HighlightBackground"/> bindable property.
        /// </value>
        public static readonly BindableProperty HighlightBackgroundProperty =
            BindableProperty.Create(
                nameof(HighlightBackground),
                typeof(Brush),
                typeof(SfEffectsView),
                new SolidColorBrush(Color.FromArgb("#1C1B1F")),
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="RippleBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RippleBackground"/> bindable property.
        /// </value>
        public static readonly BindableProperty RippleBackgroundProperty =
            BindableProperty.Create(
                nameof(RippleBackground),
                typeof(Brush),
                typeof(SfEffectsView),
                new SolidColorBrush(Color.FromArgb("#1C1B1F")),
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="SelectionBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionBackground"/> bindable property.
        /// </value>
        public static readonly BindableProperty SelectionBackgroundProperty =
            BindableProperty.Create(
                nameof(SelectionBackground),
                typeof(Brush),
                typeof(SfEffectsView),
                new SolidColorBrush(Color.FromArgb("#1C1B1F")),
                BindingMode.Default,
                null,
                OnSelectionBackgroundPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Angle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Angle"/> bindable property.
        /// </value>
        public static readonly BindableProperty AngleProperty =
            BindableProperty.Create(
                nameof(Angle),
                typeof(int),
                typeof(SfEffectsView),
                0,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="FadeOutRipple"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FadeOutRipple"/> bindable property.
        /// </value>
        public static readonly BindableProperty FadeOutRippleProperty =
            BindableProperty.Create(
                nameof(FadeOutRipple),
                typeof(bool),
                typeof(SfEffectsView),
                false,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="AutoResetEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AutoResetEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty AutoResetEffectsProperty =
            BindableProperty.Create(
                nameof(AutoResetEffects),
                typeof(AutoResetEffects),
                typeof(SfEffectsView),
                AutoResetEffects.None,
                BindingMode.Default,
                null);

        /// <summary>
        /// Identifies the <see cref="TouchDownEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchDownEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchDownEffectsProperty =
            BindableProperty.Create(
                nameof(TouchDownEffects),
                typeof(SfEffects),
                typeof(SfEffectsView),
                SfEffects.Ripple,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchUpEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchUpEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchUpEffectsProperty =
            BindableProperty.Create(
                nameof(TouchUpEffects),
                typeof(SfEffects),
                typeof(SfEffectsView),
                SfEffects.None,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="LongPressEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LongPressEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty LongPressEffectsProperty =
            BindableProperty.Create(
                nameof(LongPressEffects),
                typeof(SfEffects),
                typeof(SfEffectsView),
                SfEffects.None,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="LongPressedCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LongPressedCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty LongPressedCommandProperty =
            BindableProperty.Create(
                nameof(LongPressedCommand),
                typeof(ICommand),
                typeof(SfEffectsView),
                null,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="LongPressedCommandParameter"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LongPressedCommandParameter"/> bindable property.
        /// </value>
        public static readonly BindableProperty LongPressedCommandParameterProperty =
            BindableProperty.Create(
                nameof(LongPressedCommandParameter),
                typeof(object),
                typeof(SfEffectsView),
                null,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsSelected"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(SfEffectsView),
                false,
                BindingMode.TwoWay,
                null,
                OnIsSelectedPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShouldIgnoreTouches"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShouldIgnoreTouches"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShouldIgnoreTouchesProperty =
            BindableProperty.Create(
                nameof(ShouldIgnoreTouches),
                typeof(bool),
                typeof(SfEffectsView),
                false,
                BindingMode.Default,
                null,
                OnShouldIgnorePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TouchDownCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchDownCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchDownCommandProperty =
            BindableProperty.Create(
                nameof(TouchDownCommand),
                typeof(ICommand),
                typeof(SfEffectsView),
                null,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchUpCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchUpCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchUpCommandProperty =
            BindableProperty.Create(
                nameof(TouchUpCommand),
                typeof(ICommand),
                typeof(SfEffectsView),
                null,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchDownCommandParameter"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchDownCommandParameter"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchDownCommandParameterProperty =
            BindableProperty.Create(
                nameof(TouchDownCommandParameter),
                typeof(object),
                typeof(SfEffectsView),
                null,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchUpCommandParameter"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchUpCommandParameter"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchUpCommandParameterProperty =
            BindableProperty.Create(
                nameof(TouchUpCommandParameter),
                typeof(object),
                typeof(SfEffectsView),
                null,
                BindingMode.Default);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfEffectsView"/> class.
        /// </summary>
        public SfEffectsView()
        {
            ThemeElement.InitializeThemeResources(this, "SfEffectsViewTheme");
            InitializeEffects();
            this.AddGestureListener(this);
            this.AddTouchListener(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the duration of the ripple animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the ripple animation. The default value is 180d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the ripple animation duration for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView RippleAnimationDuration="1200">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.RippleAnimationDuration = 1200;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public double RippleAnimationDuration
        {
            get { return (double)GetValue(RippleAnimationDurationProperty); }
            set { SetValue(RippleAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration of the scale animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the scale animation. The default value is 150d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the scale animation duration for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView ScaleAnimationDuration="1200">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.ScaleAnimationDuration = 1200;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public double ScaleAnimationDuration
        {
            get { return (double)GetValue(ScaleAnimationDurationProperty); }
            set { SetValue(ScaleAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration of the rotation animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the rotation animation. The default value is 200d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the rotation animation duration for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView RotationAnimationDuration="1200">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.RotationAnimationDuration = 1200;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public double RotationAnimationDuration
        {
            get { return (double)GetValue(RotationAnimationDurationProperty); }
            set { SetValue(RotationAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the initial radius factor of ripple effect.
        /// </summary>
        /// <value>
        /// Specifies the initial radius factor of ripple effect. The default value is 0.25d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the initial ripple factor for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView InitialRippleFactor="0.75">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.InitialRippleFactor = 0.75;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public double InitialRippleFactor
        {
            get { return (double)GetValue(InitialRippleFactorProperty); }
            set { SetValue(InitialRippleFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale factor used for scale effect.
        /// </summary>
        /// <value>
        /// Specifies the scale factor of the scale effect. The default value is 1d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the scale factor for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView ScaleFactor="0.5">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.ScaleFactor = 0.5;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to highlight the effects view.
        /// </summary>
        /// <value>
        /// Specifies the highlight color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        /// <example>
        /// The following example shows how to apply the highlight background for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView HighlightBackground="Red">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.HighlightBackground = Colors.Red;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush HighlightBackground
        {
            get { return (Brush)GetValue(HighlightBackgroundProperty); }
            set { SetValue(HighlightBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the ripple.
        /// </summary>
        /// <value>
        /// Specifies the color of the ripple effect. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        /// <example>
        /// The following example shows how to apply the ripple background for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView RippleBackground="Red">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.RippleBackground = Colors.Red;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush RippleBackground
        {
            get { return (Brush)GetValue(RippleBackgroundProperty); }
            set { SetValue(RippleBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color applied when the view is on selected state.
        /// </summary>
        /// <value>
        /// Specifies the selection color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        /// <example>
        /// The following example shows how to apply the selection background for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView SelectionBackground="Red">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.SelectionBackground = Colors.Red;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush SelectionBackground
        {
            get { return (Brush)GetValue(SelectionBackgroundProperty); }
            set { SetValue(SelectionBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rotation angle.
        /// </summary>
        /// <value>
        /// Specifies the rotation angle of the effects view. The default value is 0.
        /// </value>
        /// <example>
        /// The following example shows how to apply the angle for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView Angle="180">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.Angle = 180;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public int Angle
        {
            get { return (int)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the ripple color should fade out as it grows.
        /// </summary>
        /// <value>
        /// Specifies the value whether or not the ripple color should fade out as it grows. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the fade out ripple for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView FadeOutRipple="True">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.FadeOutRipple = true;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public bool FadeOutRipple
        {
            get { return (bool)GetValue(FadeOutRippleProperty); }
            set { SetValue(FadeOutRippleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the effect that was start rendering on touch down and start removing on touch up in Android and UWP platforms.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Toolkit.EffectsView.AutoResetEffects"/> enumeration that specifies the auto reset effect of the effects view. The default value is <see cref="Syncfusion.Maui.Toolkit.EffectsView.AutoResetEffects.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the auto reset effect for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView AutoResetEffects="Highlight">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.AutoResetEffects = AutoResetEffects.Highlight;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public AutoResetEffects AutoResetEffects
        {
            get { return (AutoResetEffects)GetValue(AutoResetEffectsProperty); }
            set { SetValue(AutoResetEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the effects for touch down.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Toolkit.EffectsView.SfEffects"/> enumeration that specifies the touch down effect of the effects view. The default value is <see cref="Syncfusion.Maui.Toolkit.EffectsView.SfEffects.Ripple"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch down effect for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchDownEffects="Highlight">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.TouchDownEffects = SfEffects.Highlight;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public SfEffects TouchDownEffects
        {
            get { return (SfEffects)GetValue(TouchDownEffectsProperty); }
            set { SetValue(TouchDownEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the effects for touch up.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Toolkit.EffectsView.SfEffects"/> enumeration that specifies the touch up effect of the effects view. The default value is <see cref="Syncfusion.Maui.Toolkit.EffectsView.SfEffects.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch up effect for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchUpEffects="Ripple">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.TouchUpEffects = SfEffects.Ripple;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public SfEffects TouchUpEffects
        {
            get { return (SfEffects)GetValue(TouchUpEffectsProperty); }
            set { SetValue(TouchUpEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the long-press effect.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Toolkit.EffectsView.SfEffects"/> enumeration that specifies the long press effect of the effects view. The default value is <see cref="Syncfusion.Maui.Toolkit.EffectsView.SfEffects.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the long press effect for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView LongPressEffects="Ripple">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.LongPressEffects = SfEffects.Ripple;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public SfEffects LongPressEffects
        {
            get { return (SfEffects)GetValue(LongPressEffectsProperty); }
            set { SetValue(LongPressEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to set the view state as selected.
        /// </summary>
        /// <value>
        /// Specifies the value that indicates whether the view state should be set to selected or not. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the IsSelected for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView IsSelected="True">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.IsSelected = true;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore the touches in EffectsView.
        /// </summary>
        /// <value>
        /// Specifies the value which indicates whether to ignore the touches in EffectsView. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the ShouldIgnoreTouches for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView ShouldIgnoreTouches="True">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.ShouldIgnoreTouches = true;
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public bool ShouldIgnoreTouches
        {
            get { return (bool)GetValue(ShouldIgnoreTouchesProperty); }
            set { SetValue(ShouldIgnoreTouchesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when handling long press.
        /// </summary>
        /// <value>
        /// Specifies the command to invoke when handling long press in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the long pressed command for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView LongPressedCommand="{Binding LongPressedCommand}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.SetBinding(SfEffectsView.LongPressedCommandProperty, "LongPressedCommand");
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public ICommand LongPressedCommand
        {
            get { return (ICommand)GetValue(LongPressedCommandProperty); }
            set { SetValue(LongPressedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when handling touch down.
        /// </summary>
        /// <value>
        /// Specifies the command to invoke when handling touch down in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch down command for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchDownCommand="{Binding TouchDownCommand}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.SetBinding(SfEffectsView.TouchDownCommandProperty, "TouchDownCommand");
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public ICommand TouchDownCommand
        {
            get { return (ICommand)GetValue(TouchDownCommandProperty); }
            set { SetValue(TouchDownCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when handling touch up.
        /// </summary>
        /// <value>
        /// Specifies the command to invoke when handling touch up in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch up command for the effects view.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchUpCommand="{Binding TouchUpCommand}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.SetBinding(SfEffectsView.TouchUpCommandProperty, "TouchUpCommand");
        ///
        /// Button button = new Button();
        /// button.Text = "Content";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// effectsView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public ICommand TouchUpCommand
        {
            get { return (ICommand)GetValue(TouchUpCommandProperty); }
            set { SetValue(TouchUpCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="TouchDownCommand"/>.
        /// </summary>
        /// <value>
        /// Specifies the parameter of the touch down command in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch down command parameter for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView x:Name="sfEffectsView"
        ///                            TouchDownCommand="{Binding TouchDownCommand}"
        ///                            TouchDownCommandParameter="{x:Reference sfEffectsView}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public object TouchDownCommandParameter
        {
            get { return (object)GetValue(TouchDownCommandParameterProperty); }
            set { SetValue(TouchDownCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="LongPressedCommand"/>.
        /// </summary>
        /// <value>
        /// Specifies the parameter of the long pressed command in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the long pressed command parameter for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView x:Name="sfEffectsView"
        ///                            LongPressedCommand="{Binding LongPressedCommand}"
        ///                            LongPressedCommandParameter="{x:Reference sfEffectsView}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public object LongPressedCommandParameter
        {
            get { return (object)GetValue(LongPressedCommandParameterProperty); }
            set { SetValue(LongPressedCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="TouchUpCommand"/>.
        /// </summary>
        /// <value>
        /// Specifies the parameter of the touch up command in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch up command parameter for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView x:Name="sfEffectsView"
        ///                            TouchUpCommand="{Binding TouchUpCommand}"
        ///                            TouchUpCommandParameter="{x:Reference sfEffectsView}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public object TouchUpCommandParameter
        {
            get { return (object)GetValue(TouchUpCommandParameterProperty); }
            set { SetValue(TouchUpCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to set the view state as selected.
        /// </summary>
        internal bool IsSelection
        {
            get
            {
                return _isSelect;
            }

            set
            {
                if (_isSelect != value)
                {
                    _isSelect = value;
                    if (value)
                    {
                        _selectionEffectLayer?.UpdateSelectionBounds(_selectionEffectLayer.Width, _selectionEffectLayer.Height, SelectionBackground);
                    }
                    else
                    {
                        RemoveSelection();
                    }

                    InvokeSelectionChangedEvent();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the long press handled or not.
        /// </summary>
        internal bool LongPressHandled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force reset the animation or not.
        /// </summary>
        internal bool ForceReset { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Removes the ripple and highlight effects.
        /// </summary>
        /// <remarks>
        /// This method stops any ongoing animations and resets the view to its original state.
        /// It is useful when you want to clear all effects and start fresh.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.Reset();
        /// ]]></code>
        /// </example>
        public void Reset()
        {
            _canRepeat = false;
            LongPressHandled = false;
            if (_rippleEffectLayer != null)
            {
                _rippleEffectLayer.CanRemoveRippleAnimation = this.AnimationIsRunning("RippleAnimator");
                if (!_rippleEffectLayer.CanRemoveRippleAnimation || ForceReset)
                {
                    _rippleEffectLayer.OnRippleAnimationFinished();
                }
            }

            _highlightEffectLayer?.UpdateHighlightBounds();

            if (_selectionEffectLayer != null && IsSelected)
            {
                IsSelected = false;
                InvokeSelectionChangedEvent();
            }

            if (TouchDownEffects == SfEffects.Scale || TouchUpEffects == SfEffects.Scale || LongPressEffects == SfEffects.Scale)
            {
                if (Content != null)
                {
                    Content.Scale = 1;
                }

                OnScaleAnimationEnd(0, true);
            }

            if (TouchUpEffects == SfEffects.Rotation || TouchDownEffects == SfEffects.Rotation || LongPressEffects == SfEffects.Rotation)
            {
                if (Content != null)
                {
                    Content.Rotation = 0;
                }

                OnRotationAnimationEnd(0, true);
            }
        }

        /// <summary>
        /// Used to trigger the invoke the effects.
        /// </summary>
        /// <param name="effects"><see cref="SfEffects"/> that need to apply.</param>
        /// <param name="rippleStartPosition">Position at where the ripple animation start growing.
        /// By default, value ripple start from center.</param>
        /// <param name="rippleStartPoint">Point at which the ripple animation start growing.
        /// By default, value is null.</param>
        /// <param name="repeat">To set whether to ripple animation need to repeat or not.</param>
        /// <remarks>
        /// The effects applied using this method will not be removed automatically.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// SfEffectsView effectsView = new SfEffectsView();
        /// effectsView.ApplyEffects(SfEffects.Ripple, RippleStartPosition.Top, new System.Drawing.Point(50, 50), true);
        /// ]]></code>
        /// </example>
        public void ApplyEffects(SfEffects effects = SfEffects.Ripple, RippleStartPosition rippleStartPosition = RippleStartPosition.Default, System.Drawing.Point? rippleStartPoint = null, bool repeat = false)
        {
            if (_rippleEffectLayer != null)
            {
                _rippleEffectLayer.CanRemoveRippleAnimation = false;
            }

            _canRepeat = repeat;
            float x = (float)(Width / 2), y = (float)(Height / 2);

            if (rippleStartPosition == RippleStartPosition.Left)
            {
                x = 0;
            }

            if (rippleStartPosition == RippleStartPosition.Top)
            {
                y = 0;
            }

            if (rippleStartPosition == RippleStartPosition.Right)
            {
                x = (float)Width;
            }

            if (rippleStartPosition == RippleStartPosition.Bottom)
            {
                y = (float)Height;
            }

            if (rippleStartPosition == RippleStartPosition.TopLeft)
            {
                x = 0;
                y = 0;
            }

            if (rippleStartPosition == RippleStartPosition.TopRight)
            {
                x = (float)Width;
                y = 0;
            }

            if (rippleStartPosition == RippleStartPosition.BottomLeft)
            {
                x = 0;
                y = (float)Height;
            }

            if (rippleStartPosition == RippleStartPosition.BottomRight)
            {
                x = (float)Width;
                y = (float)Height;
            }

            if (rippleStartPosition == RippleStartPosition.Default)
            {
                if (rippleStartPoint != null)
                {
                    x = rippleStartPoint.Value.X;
                    y = rippleStartPoint.Value.Y;
                }
            }

            AddEffects(effects, new Point(x, y));
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Invokes animation completed event.
        /// </summary>
        /// <param name="eventArgs">Animation completed events argument.</param>
        internal void RaiseAnimationCompletedEvent(EventArgs eventArgs)
        {
            AnimationCompleted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Invokes when this view selected.
        /// </summary>
        /// <param name="eventArgs">Selected events argument.</param>
        internal void RaiseSelectedEvent(EventArgs eventArgs)
        {
            SelectionChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Invokes <see cref="SelectionChanged"/> event.
        /// </summary>
        internal void InvokeSelectionChangedEvent()
        {
            RaiseSelectedEvent(EventArgs.Empty);
        }

        /// <summary>
        /// Invokes <see cref="LongPressed"/> when handling long press.
        /// </summary>
        internal void InvokeLongPressedEventAndCommand()
        {
            LongPressed?.Invoke(this, EventArgs.Empty);
            if (LongPressedCommand != null && LongPressedCommand.CanExecute(LongPressedCommandParameter))
            {
                LongPressedCommand.Execute(LongPressedCommandParameter);
            }
        }

        /// <summary>
        /// Invokes <see cref="TouchDown"/> when handling touch down.
        /// </summary>
        internal void InvokeTouchDownEventAndCommand()
        {
            TouchDown?.Invoke(this, EventArgs.Empty);
            if (TouchDownCommand != null && TouchDownCommand.CanExecute(TouchDownCommandParameter))
            {
                TouchDownCommand.Execute(TouchDownCommandParameter);
            }
        }

        /// <summary>
        /// Invokes <see cref="AnimationCompleted"/> event.
        /// </summary>
        internal void InvokeAnimationCompletedEvent()
        {
            RaiseAnimationCompletedEvent(EventArgs.Empty);
        }

        /// <summary>
        ///  Invokes <see cref="TouchUp"/> when handling touch up.
        /// </summary>
        internal void InvokeTouchUpEventAndCommand()
        {
            TouchUp?.Invoke(this, EventArgs.Empty);
            if (TouchUpCommand != null && TouchUpCommand.CanExecute(TouchUpCommandParameter))
            {
                TouchUpCommand.Execute(TouchUpCommandParameter);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updateselection method.
        /// </summary>
        /// <param name="selectionBackground">The selection background.</param>
        void UpdateSelectionBackground(Brush selectionBackground)
        {
            if (IsSelected)
            {
                _selectionEffectLayer?.UpdateSelectionBounds(Width, Height, selectionBackground);
            }
        }

        /// <summary>
        /// Remove the selection effect.
        /// </summary>
        void RemoveSelection()
        {
            _selectionEffectLayer?.UpdateSelectionBounds();
        }

        /// <summary>
        /// Remove the highlight effect.
        /// </summary>
        void RemoveHighlightEffect()
        {
            _highlightEffectLayer?.UpdateHighlightBounds();
        }

        /// <summary>
        /// Method used to add respective auto reset effects.
        /// </summary>
        /// <param name="effects">The effects value.</param>
        /// <param name="touchPoint">The touch point.</param>
        void AddResetEffects(AutoResetEffects effects, Point touchPoint)
        {
            foreach (AutoResetEffects effect in effects.GetAllItems())
            {
                if (effect == AutoResetEffects.None)
                {
                    continue;
                }

                if (effect == AutoResetEffects.Highlight)
                {
                    _highlightEffectLayer?.UpdateHighlightBounds(Width, Height, HighlightBackground);
                    AnimationExtensions.Animate(this, _highlightAnimation, OnHighlightAnimationUpdate, 16, 250, Easing.Linear, OnAnimationFinished, null);
                }

                if (effect == AutoResetEffects.Ripple)
                {
                    _rippleEffectLayer?.StartRippleAnimation(touchPoint, RippleBackground, RippleAnimationDuration, (float)InitialRippleFactor, FadeOutRipple);
                }

                if (effect == AutoResetEffects.Scale)
                {
                    StartScaleAnimation();
                }
            }
        }

        /// <summary>
        /// Method used to add respective effects.
        /// </summary>
        /// <param name="sfEffect">The Effects.</param>
        /// <param name="touchPoint">The touch point.</param>
        void AddEffects(SfEffects sfEffect, Point touchPoint)
        {
            foreach (SfEffects effect in sfEffect.GetAllItems())
            {
                if (effect == SfEffects.None)
                {
                    continue;
                }

                if (effect == SfEffects.Highlight)
                {
                    _highlightEffectLayer?.UpdateHighlightBounds(_highlightEffectLayer.Width, _highlightEffectLayer.Height, HighlightBackground);
                }

                if (effect == SfEffects.Ripple)
                {
                    _rippleEffectLayer?.StartRippleAnimation(touchPoint, RippleBackground, RippleAnimationDuration, (float)InitialRippleFactor, FadeOutRipple, _canRepeat);
                }

                if (effect == SfEffects.Selection)
                {
                    _selectionEffectLayer?.UpdateSelectionBounds(_selectionEffectLayer.Width, _selectionEffectLayer.Height, SelectionBackground);
                    if (!IsSelected)
                    {
                        IsSelected = true;
                    }
                }

                if (effect == SfEffects.Scale)
                {
                    StartScaleAnimation();
                }

                if (effect == SfEffects.Rotation)
                {
                    StartRotationAnimation();
                }
            }
        }

        /// <summary>
        /// Remove ripple effect.
        /// </summary>
        void RemoveRippleEffect()
        {
            _rippleEffectLayer?.OnRippleAnimationFinished();
        }

        /// <summary>
        /// Initialize method.
        /// </summary>
        void InitializeEffects()
        {

            _rippleEffectLayer ??= new RippleEffectLayer(RippleBackground, RippleAnimationDuration, this, this);
            if (_selectionEffectLayer == null)
                _selectionEffectLayer = new SelectionEffectLayer(SelectionBackground, this);
            if (_highlightEffectLayer == null)
                _highlightEffectLayer = new HighlightEffectLayer(HighlightBackground, this);
            DrawingOrder = DrawingOrder.AboveContent;
        }

        /// <summary>
        /// Scale animation method.
        /// </summary>
        void StartScaleAnimation()
        {
            if (Content != null && _tempScaleFactor != ScaleFactor)
            {
                Content.AnchorX = _anchorValue;
                Content.AnchorY = _anchorValue;
                _tempScaleFactor = ScaleFactor;
                AnimationExtensions.Animate(
                    Content,
                    _scaleAnimation,
                    OnScaleAnimationUpdate,
                    Content.Scale,
                    ScaleFactor,
                    16,
                    (uint)ScaleAnimationDuration,
                    Easing.Linear,
                    OnScaleAnimationEnd,
                    null);
            }
        }

        /// <summary>
        /// Rotation animation method.
        /// </summary>
        void StartRotationAnimation()
        {
            if (Content != null)
            {
                Content.AnchorX = _anchorValue;
                Content.AnchorY = _anchorValue;
                if (DeviceInfo.Platform == DevicePlatform.WinUI && ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
                {
                    Content.Rotation = -Content.Rotation;
                    Angle = -Angle;
                }

                AnimationExtensions.Animate(
                    Content,
                    _rotationAnimation,
                    OnAnimationUpdate,
                    Content.Rotation,
                    Angle,
                    16,
                    (uint)RotationAnimationDuration,
                    Easing.Linear,
                    OnRotationAnimationEnd,
                    null);
            }
        }

        /// <summary>
        /// Animation ended method.
        /// </summary>
        /// <param name="value">The animation value.</param>
        /// <param name="finished">The finished.</param>
        void OnRotationAnimationEnd(double value, bool finished)
        {
            AnimationExtensions.AbortAnimation(this, _rotationAnimation);

            if ((_rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (_rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Rotation)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Rotation))))
            {
                InvokeAnimationCompletedEvent();
            }
        }

        /// <summary>
        /// Animation update method.
        /// </summary>
        /// <param name="value">The animation value.</param>
        void OnAnimationUpdate(double value)
        {
            if (Content != null)
            {
                Content.Rotation = value;
            }
        }

        /// <summary>
        /// Scale animation ended method.
        /// </summary>
        /// <param name="value">The animation value.</param>
        /// <param name="finished">The finished value.</param>
        void OnScaleAnimationEnd(double value, bool finished)
        {
            AnimationExtensions.AbortAnimation(this, _scaleAnimation);
            if ((_rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (_rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Scale)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Scale))))
            {
                InvokeAnimationCompletedEvent();
            }
        }

        /// <summary>
        /// Scale animation update method.
        /// </summary>
        /// <param name="value">Animation update value.</param>
        void OnScaleAnimationUpdate(double value)
        {
            if (Content != null)
            {
                Content.Scale = value;
            }
        }

        /// <summary>
        /// Highlight animation update method.
        /// </summary>
        /// <param name="value">Animation update value.</param>
        void OnHighlightAnimationUpdate(double value)
        {
        }

        /// <summary>
        /// Animation ended method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="completed">Completed property.</param>
        void OnAnimationFinished(double value, bool completed)
        {
            _highlightEffectLayer?.UpdateHighlightBounds();
            if ((_rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (_rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Highlight))))
            {
                InvokeAnimationCompletedEvent();
            }
        }

		#endregion

		#region Override methods

		/// <summary>
		/// The draw method.
		/// </summary>
		/// <exclude/>
		/// <param name="canvas">The canvas.</param>
		/// <param name="dirtyRect">The rectangle.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            _highlightEffectLayer?.DrawHighlight(canvas);
            _rippleEffectLayer?.DrawRipple(canvas, dirtyRect);
            _selectionEffectLayer?.DrawSelection(canvas);
        }

		/// <summary>
		/// ArrangeContent method.
		/// </summary>
		/// <exclude/>
		/// <param name="bounds">The bounds.</param>
		/// <returns>The size.</returns>
		// TODO: To avoid argument width and height lesser than zero exception when not setting width and height to the control.
		protected override Size ArrangeContent(Rect bounds)
        {
            if (bounds.Width > 0 && bounds.Height > 0)
            {
                if (_highlightEffectLayer != null)
                {
                    _highlightEffectLayer.Width = bounds.Width;
                    _highlightEffectLayer.Height = bounds.Height;
                }

                if (_rippleEffectLayer != null)
                {
                    _rippleEffectLayer.Width = bounds.Width;
                    _rippleEffectLayer.Height = bounds.Height;
                }

                if (_selectionEffectLayer != null)
                {
                    _selectionEffectLayer.Width = bounds.Width;
                    _selectionEffectLayer.Height = bounds.Height;
                    if (IsSelected)
                    {
                        _selectionEffectLayer?.UpdateSelectionBounds(bounds.Width, bounds.Height, SelectionBackground);
                        if (!_isSelectedCalled)
                        {
                            InvokeSelectionChangedEvent();
                            _isSelectedCalled = true;
                        }
                    }
                }
            }

            return base.ArrangeContent(bounds);
        }

        #endregion

        #region Property changed methods

        /// <summary>
        /// Property changed for selection color.
        /// </summary>
        /// <param name="bindable">The bindable value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnSelectionBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null && (bindable as SfEffectsView) != null && newValue != null)
            {
                (bindable as SfEffectsView)?.UpdateSelectionBackground((Brush)newValue);
            }
        }

        /// <summary>
        /// Property changed for isselected.
        /// </summary>
        /// <param name="bindable">The bindable value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnIsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null && (bindable as SfEffectsView) != null && newValue != null)
            {
                if (bindable is SfEffectsView effectsView)
                {
                    effectsView.IsSelection = (bool)newValue;
                }
            }
        }

        /// <summary>
        /// Property changed for should ignore touch.
        /// </summary>
        /// <param name="bindable">The binable value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnShouldIgnorePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null && (bindable as SfEffectsView) != null && newValue != null)
            {
                if (bindable is SfEffectsView effectsView)
                {
                    effectsView.RemoveGestureListener(effectsView);
                    effectsView.RemoveTouchListener(effectsView);
                    if (!(bool)newValue)
                    {
                        effectsView.AddGestureListener(effectsView);
                        effectsView.AddTouchListener(effectsView);
                    }
                }
            }
        }

        #endregion

        #region Interface implementation

        /// <summary>
        /// LongPress method.
        /// </summary>
        /// <param name="e">The long press event arguments.</param>
        public void OnLongPress(LongPressEventArgs e)
        {
            if (!ShouldIgnoreTouches)
            {
                InvokeLongPressedEventAndCommand();
                LongPressHandled = true;

                if (AutoResetEffects == AutoResetEffects.None && e != null)
                {
                    AddEffects(LongPressEffects.ComplementsOf(TouchDownEffects), e.TouchPoint);
                }
            }
        }

        /// <summary>
        /// Tap method.
        /// </summary>
        /// <param name="e">The tap event arguments.</param>
        public void OnTap(TapEventArgs e)
        {
            if (!ShouldIgnoreTouches)
            {
                LongPressHandled = false;

                if (TouchUpEffects != SfEffects.None && e != null)
                {
                    AddEffects(TouchUpEffects, e.TapPoint);
                }

                if (TouchUpEffects.GetAllItems().Contains(SfEffects.Scale))
                {
                    StartScaleAnimation();
                }
            }
        }

        /// <summary>
        /// Touch Action method.
        /// </summary>
        /// <param name="e">The touch event arguments.</param>
        public void OnTouch(PointerEventArgs e)
        {
            if (!ShouldIgnoreTouches && e != null)
            {
                if (e.Action == PointerActions.Pressed)
                {
                    _touchDownPoint = e.TouchPoint;
                    LongPressHandled = false;

                    if (_rippleEffectLayer != null)
                    {
                        _rippleEffectLayer.CanRemoveRippleAnimation = false;
                    }

                    InvokeTouchDownEventAndCommand();

                    if (AutoResetEffects != AutoResetEffects.None)
                    {
                        AddResetEffects(AutoResetEffects, e.TouchPoint);
                    }
                    else
                    {
                        AddEffects(TouchDownEffects, e.TouchPoint);
                    }
                }

                if (e.Action == PointerActions.Released)
                {
                    _elementBounds.X = 0;
                    _elementBounds.Y = 0;
                    _elementBounds.Height = (float)Height;
                    _elementBounds.Width = (float)Width;
#if ANDROID || IOS
                    double diffY = Math.Abs(_touchDownPoint.Y - e.TouchPoint.Y);
                    if (diffY < 5)
#endif
                    {
                        if (_elementBounds.Contains(e.TouchPoint))
                        {
                            InvokeTouchUpEventAndCommand();
                        }
                    }
                    if (AutoResetEffects.GetAllItems().Contains(AutoResetEffects.Ripple))
                    {
                        if (_rippleEffectLayer != null)
                        {
                            _rippleEffectLayer.CanRemoveRippleAnimation = this.AnimationIsRunning("RippleAnimator");
                        }
                    }
                    else if (AutoResetEffects == AutoResetEffects.None)
                    {
                        if (TouchDownEffects == SfEffects.Highlight || TouchDownEffects.GetAllItems().Contains(SfEffects.Highlight))
                        {
                            _highlightEffectLayer?.UpdateHighlightBounds();
                            if ((_rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator")) || (_rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Highlight))))
                            {
                                InvokeAnimationCompletedEvent();
                            }
                        }

                        if (!IsSelected || (!IsSelected && (TouchDownEffects != SfEffects.Selection || !TouchDownEffects.GetAllItems().Contains(SfEffects.Selection))))
                        {
                            RemoveSelection();
                        }

                        if (TouchDownEffects.GetAllItems().Contains(SfEffects.Ripple) || TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || LongPressEffects.GetAllItems().Contains(SfEffects.Ripple))
                        {
                            if (_rippleEffectLayer != null)
                            {
                                _rippleEffectLayer.CanRemoveRippleAnimation = this.AnimationIsRunning("RippleAnimator");
                            }
                        }

                        if (TouchUpEffects != SfEffects.Highlight || !TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight))
                        {
                            RemoveHighlightEffect();
                        }
                        else
                        {
                            AnimationExtensions.Animate(this, _highlightAnimation, OnHighlightAnimationUpdate, 16, 250, Easing.Linear, OnAnimationFinished, null);
                        }

                        if ((TouchUpEffects != SfEffects.Ripple || !TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple)) && _rippleEffectLayer != null && !this.AnimationIsRunning("RippleAnimator"))
                        {
                            RemoveRippleEffect();
                        }
                    }
                }
                else if (e.Action == PointerActions.Cancelled)
                {
                    LongPressHandled = false;
                    RemoveRippleEffect();
                    RemoveHighlightEffect();
                    RemoveSelection();
                }
                else if (e.Action == PointerActions.Moved)
                {
                    double diffX = Math.Abs(_touchDownPoint.X - e.TouchPoint.X);
                    double diffY = Math.Abs(_touchDownPoint.Y - e.TouchPoint.Y);

                    if (diffX >= 20 || diffY >= 20)
                    {
                        RemoveRippleEffect();
                        RemoveHighlightEffect();
                    }
                }
            }
        }

        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfEffectsViewStyles();
        }

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {

        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {

        }

        #endregion

        #region Events

        /// <summary>
        /// The <see cref="AnimationCompleted"/> event occurs on direct interaction and when programmatically applied,
        /// it only occurs when touch-up is called on direct interaction and after the effects have been completed.
        /// It will not trigger the selection effect.
        /// </summary>
        /// <value>
        /// Occurs when animation is completed.
        /// </value>
        public event EventHandler? AnimationCompleted;

        /// <summary>
        /// The <see cref="SelectionChanged"/> event triggers both the rendering of <see cref="SfEffects.Selection"/> by direct interaction
        /// and the <see cref="IsSelected"/> property changed.
        /// </summary>
        /// <value>
        /// Occurs when changing IsSelected property and setting selection effect.
        /// </value>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when handling touch down.
        /// </summary>
        /// <value>
        /// Occurs when handling touch down.
        /// </value>
        public event EventHandler? TouchDown;

        /// <summary>
        /// Occurs when handling touch up.
        /// </summary>
        /// <value>
        /// Occurs when handling touch up.
        /// </value>
        public event EventHandler? TouchUp;

        /// <summary>
        /// Occurs when handling long press.
        /// </summary>
        /// <value>
        /// Occurs when handling long press.
        /// </value>
        public event EventHandler? LongPressed;

        #endregion
    }
}
