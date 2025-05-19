using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.ProgressBar
{
    /// <summary>
    /// Represents <see cref="ProgressGradientStop"/> class.
    /// </summary>
    /// <example>
    /// # [XAML](#tab/tabid-1)
    /// Snippet for <see cref="SfLinearProgressBar"/>
    /// <code><![CDATA[
    /// <progressBar:SfLinearProgressBar Progress="75">
    ///     <progressBar:SfLinearProgressBar.GradientStops>
    ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
    ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
    ///     </progressBar:SfLinearProgressBar.GradientStops>
    /// </progressBar:SfLinearProgressBar>
    /// ]]></code>
    /// Snippet for <see cref="SfCircularProgressBar"/>
    /// <code><![CDATA[
    /// <progressBar:SfCircularProgressBar Progress="75">
    ///     <progressBar:SfCircularProgressBar.GradientStops>
    ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
    ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
    ///     </progressBar:SfCircularProgressBar.GradientStops>                              
    /// </progressBar:SfCircularProgressBar>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// Snippet for <see cref="SfLinearProgressBar"/>
    /// <code><![CDATA[
    /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
    /// progressBar.Progress = 75;
    /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
    /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
    /// this.Content = progressBar;
    /// ]]></code>
    /// Snippet for <see cref="SfCircularProgressBar"/>
    /// <code><![CDATA[
    /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
    /// progressBar.Progress = 75;
    /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
    /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
    /// this.Content = progressBar;
    /// ]]></code>
    /// ***
    /// </example>
    public class ProgressGradientStop : Element, IThemeElement
    {
        #region Fields

        /// <summary>
        /// Backing field to store the actual value of gradient stop.
        /// </summary>
        double _actualValue;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Color"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Color"/> bindable property.
        /// </value>
        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(
                nameof(Color),
                typeof(Color),
                typeof(ProgressGradientStop),
                Colors.Transparent);

        /// <summary>
        /// Identifies the <see cref="Value"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Value"/> bindable property.
        /// </value>
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(double),
                typeof(ProgressGradientStop),
                0d,
                propertyChanged: OnValuePropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressGradientStop"/> class.
        /// </summary>
        public ProgressGradientStop()
        {
            ThemeElement.InitializeThemeResources(this, "SfProgressBarTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color that describes the gradient color value.
        /// </summary>
        /// <value>
        /// The default color is <c>transparent</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar Progress="75">
        ///     <progressBar:SfLinearProgressBar.GradientStops>
        ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
        ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
        ///     </progressBar:SfLinearProgressBar.GradientStops>
        /// </progressBar:SfLinearProgressBar>
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Progress="75">
        ///     <progressBar:SfCircularProgressBar.GradientStops>
        ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
        ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
        ///     </progressBar:SfCircularProgressBar.GradientStops>                              
        /// </progressBar:SfCircularProgressBar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.Progress = 75;
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Progress = 75;
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the gradient value.
        /// </summary>
        /// <value>
        /// The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfLinearProgressBar Progress="75">
        ///     <progressBar:SfLinearProgressBar.GradientStops>
        ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
        ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
        ///     </progressBar:SfLinearProgressBar.GradientStops>
        /// </progressBar:SfLinearProgressBar>
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// <progressBar:SfCircularProgressBar Progress="75">
        ///     <progressBar:SfCircularProgressBar.GradientStops>
        ///            <progressBar:ProgressGradientStop Color="Yellow" Value="30"/>
        ///            <progressBar:ProgressGradientStop Color="Green" Value="60"/>
        ///     </progressBar:SfCircularProgressBar.GradientStops>                              
        /// </progressBar:SfCircularProgressBar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// Snippet for <see cref="SfLinearProgressBar"/>
        /// <code><![CDATA[
        /// SfLinearProgressBar progressBar = new SfLinearProgressBar();
        /// progressBar.Progress = 75;
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
        /// this.Content = progressBar;
        /// ]]></code>
        /// Snippet for <see cref="SfCircularProgressBar"/>
        /// <code><![CDATA[
        /// SfCircularProgressBar progressBar = new SfCircularProgressBar();
        /// progressBar.Progress = 75;
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 });
        /// progressBar.GradientStops.Add(new ProgressGradientStop { Color = Colors.Green, Value = 60 });
        /// this.Content = progressBar;
        /// ]]></code>
        /// ***
        /// </example>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for actual value of gradient stop
        /// </summary>
        internal double ActualValue
        {
            get
            {
                return _actualValue;
            }

            set
            {
                if (_actualValue != value)
                {
                    _actualValue = value;
                }
            }
        }

        #endregion

        #region Property Changed

        /// <summary>
        /// Called when the value of gauge gradient stop is changed
        /// </summary>
        /// <param name="bindable">The bindable object</param>
        /// <param name="oldValue">Represents old value</param>
        /// <param name="newValue">Represents new value</param>
        private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProgressGradientStop gradientStop)
            {
                gradientStop.ActualValue = gradientStop.Value;
            }
        }

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme) { }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme) { }

        #endregion
    }
}
