using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Buttons
{
	public partial class SfButton
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="TextTransform"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="TextTransform"/> bindable property.
		/// </value>
		public static readonly BindableProperty TextTransformProperty = BindableProperty.Create(
			nameof(TextTransform),
			typeof(TextTransform),
			typeof(SfButton),
			TextTransform.Default,
			BindingMode.Default,
			null,
			OnTextTransformChangedProperty);


		/// <summary>
		/// Identifies the <see cref="CornerRadius"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="CornerRadius"/> bindable property.
		/// </value>
		public static new readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
			nameof(CornerRadius),
			typeof(CornerRadius),
			typeof(SfButton),
			new CornerRadius(20),
			BindingMode.Default,
			null,
			OnCornerRadiusPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LineBreakMode"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="LineBreakMode"/> bindable property.
		/// </value>
		public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(
			nameof(LineBreakMode),
			typeof(LineBreakMode),
			typeof(SfButton),
			LineBreakMode.NoWrap,
			BindingMode.Default,
			null,
			OnLineBreakModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeThickness"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="StrokeThickness"/> bindable property.
		/// </value>
		public static new readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(
			nameof(StrokeThickness),
			typeof(double),
			typeof(SfButton),
			0d,
			BindingMode.Default,
			null,
			OnStrokeThicknessPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="TextColor"/> bindable property.
		/// </value>
		public static new readonly BindableProperty TextColorProperty = BindableProperty.Create(
			nameof(TextColor),
			typeof(Color),
			typeof(SfButton),
			Colors.White,
			BindingMode.Default,
			null,
			OnTextColorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Background"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Background"/> bindable property.
		/// </value>
		public static new readonly BindableProperty BackgroundProperty = BindableProperty.Create(
			nameof(Background),
			typeof(Brush),
			typeof(SfButton),
			new SolidColorBrush(Color.FromArgb("#6750A4")),
			BindingMode.Default,
			null,
			OnBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="DashArray"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="DashArray"/> bindable property.
		/// </value>
		public static readonly BindableProperty DashArrayProperty = BindableProperty.Create(
			nameof(DashArray),
			typeof(float[]),
			typeof(SfButton),
			new float[] { 0, 0 },
			BindingMode.Default,
			null,
			OnDashArrayPropertyChanged);


		/// <summary>
		/// Identifies the <see cref="Content"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Content"/> bindable property.
		/// </value>
		public static readonly BindableProperty ContentProperty = BindableProperty.Create(
			nameof(Content),
			typeof(DataTemplate),
			typeof(SfButton),
			null,
			BindingMode.Default,
			null,
			OnContentPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsCheckable"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IsCheckable"/> bindable property.
		/// </value>
		public static new readonly BindableProperty IsCheckableProperty = BindableProperty.Create(
			nameof(IsCheckable),
			typeof(bool),
			typeof(SfButton),
			false,
			BindingMode.Default,
			null,
			OnCheckPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsChecked"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IsChecked"/> bindable property.
		/// </value>
		public static new readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
			nameof(IsChecked),
			typeof(bool),
			typeof(SfButton),
			false,
			BindingMode.TwoWay,
			null,
			OnCheckPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="BackgroundImageAspect"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="BackgroundImageAspect"/> bindable property.
		/// </value>
		public static readonly BindableProperty BackgroundImageAspectProperty = BindableProperty.Create(
			nameof(BackgroundImageAspect), 
			typeof(Aspect), 
			typeof(SfButton), 
			Aspect.AspectFill, 
			BindingMode.Default, 
			null, 
			OnBackgroundImageAspectPropertyChanged);

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value of text transform. This property is used to specify the text transformation options for text processing.
		/// </summary>
		/// <value>The default value is "Default", indicating that default transform is applied.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="TextTransform"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        TextTransform="Uppercase"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.TextTransform = TextTransform.Uppercase;
		/// ]]></code>
		/// ***
		/// </example>
		public TextTransform TextTransform
		{
			get { return (TextTransform)GetValue(TextTransformProperty); }
			set { SetValue(TextTransformProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of corner radius. This property can be used to customize the corners of button control.
		/// </summary>
		/// <value> The default value is 20.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="CornerRadius"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        CornerRadius="10"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.CornerRadius = 10;
		/// ]]></code>
		/// ***
		/// </example>
		public new CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of line breakmode. This property can be used to customize the line breakmode of the text.
		/// </summary>
		/// <value> The default value is NoWrap.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="LineBreakMode"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        LineBreakMode="TailTruncation"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.LineBreakMode = LineBreakMode.TailTruncation;
		/// ]]></code>
		/// ***
		/// </example>
		public LineBreakMode LineBreakMode
		{
			get { return (LineBreakMode)GetValue(LineBreakModeProperty); }
			set { SetValue(LineBreakModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of stroke thickness. This property can be used to give border thickness to button control.
		/// </summary>
		/// <value> The default value is 0d.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="StrokeThickness"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        StrokeThickness="4"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.StrokeThickness = 4;
		/// ]]></code>
		/// ***
		/// </example>
		public new double StrokeThickness
		{
			get { return (double)GetValue(StrokeThicknessProperty); }
			set { SetValue(StrokeThicknessProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of text color. This property can be used to give text color to the text in control.
		/// </summary>
		/// <value> The default value is Colors.White.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="TextColor"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        TextColor="White"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.TextColor = Colors.White;
		/// ]]></code>
		/// ***
		/// </example>
		public new Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of background. This property can be used to give background color to the control.
		/// </summary>
		/// <value> The default value is SolidColorBrush(Color.FromArgb("#6750A4")) .</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Background"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///         x:Name="button"
		///         Text="Button Text"
		///         Background="Green"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.Background = new SolidColorBrush(Colors.Green);
		/// ]]></code>
		/// ***
		/// </example>
		public new Brush? Background
		{
			get { return (Brush)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of dash array of the border. This property can be used to customize the dash of border. 
		/// </summary>
		/// <value>The default value is float[]{0,0}.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="DashArray"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///	       x:Name="button"
		///        Text="Button Text"
		///        StrokeThickness="4">
		///	       <buttons:SfButton.DashArray>
		///	           <x:Array Type = "{x:Type x:Single}" >
		///               < x:Single>2</x:Single>
		///               <x:Single>2</x:Single>
		///	           </x:Array>
		///        </buttons:SfButton.DashArray>
		/// </toolkit:SfButton>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.StrokeThickness = 4;
		/// button.DashArray = new float[] { 2, 2 };
		/// ]]></code>
		/// ***
		/// </example>
		public float[] DashArray
		{
			get { return (float[])GetValue(DashArrayProperty); }
			set { SetValue(DashArrayProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the content. This property can be used to set custom view to the button control.
		/// </summary>
		/// <value>The default value is null.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Content"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton Text = "Button Text"
		///        WidthRequest="120">
		///        <DataTemplate>
		///            <HorizontalStackLayout Spacing = "8" Padding="5">
		///               <ActivityIndicator Color = "White" IsRunning="True"/>
		///               <Label Text = "Loading..." VerticalOptions="Center"/>
		///            </HorizontalStackLayout>
		///        </DataTemplate>
		/// </toolkit:SfButton>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// var customContent = new DataTemplate(() =>
		/// {
		///	    var activityIndicator = new ActivityIndicator
		///     {
		///         Color = Colors.White,
		///         IsRunning = true,
		///     };
		///     var label = new Label
		///     {
		///         Text = "Loading...",
		///         VerticalOptions = LayoutOptions.Center
		///     };
		///     var stackLayout = new HorizontalStackLayout
		///     {
		///         Spacing = 8,
		///         Padding = new Thickness(5)
		///     };
		///     stackLayout.Children.Add(activityIndicator);
		///     stackLayout.Children.Add(label);
		///     return stackLayout
		/// });
		/// SfButton button = new SfButton
		/// {
		///    Text = "Button Text",
		///    WidthRequest = 120,
		///    Content = customContent
		/// };
		/// Content = button;
		/// ]]></code>
		/// ***
		/// </example>
		public DataTemplate Content
		{
			get { return (DataTemplate)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is in checkable state or not. This property can be used to change the state of the control.
		/// </summary>
		/// <value><c>true</c> if checkable is enabled; otherwise, <c>false</c>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="IsCheckable"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        IsCheckable ="True"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.IsCheckable = true;
		/// ]]></code>
		/// ***
		/// </example>
		public new bool IsCheckable
		{
			get { return (bool)GetValue(IsCheckableProperty); }
			set { SetValue(IsCheckableProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is checkable. It is used to check the state of the control.
		/// </summary>
		/// <value><c>true</c> if checked is enabled; otherwise, <c>false</c>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="IsChecked"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        IsCheckable ="True"
		///        IsChecked="True"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.IsCheckable = true;
		/// button.IsChecked = true;
		/// ]]></code>
		/// ***
		/// </example>
		public new bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the background image aspect. This property can be used to set an aspect for background image of Button.
		/// </summary>
		/// <value>The default value is Aspect.AspectFill.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="BackgroundImageAspect"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <toolkit:SfButton
		///        x:Name="button"
		///        Text="Button Text"
		///        BackgroundImageSource="dotnet_bot.png"
		///        BackgroundImageAspect="Fill"/>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfButton button = new SfButton();
		/// button.Text = "Button Text";
		/// button.BackgroundImageSource = "dotnet_bot.png";
		/// button.BackgroundImageAspect = Aspect.Fill;
		/// ]]></code>
		/// ***
		/// </example>
		public Aspect BackgroundImageAspect
		{
			get { return (Aspect)GetValue(BackgroundImageAspectProperty); }
			set { SetValue(BackgroundImageAspectProperty, value); }
		}
		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets the actual text size for the button control.
		/// </summary>
		internal override Size TextSize
		{
			get
			{
				return Text.Measure(this);
			}
		}

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Property changed method for text transform property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnTextTransformChangedProperty(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Property changed method for corner radius property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.UpdateCornerRadius();
			}
		}

		/// <summary>
		/// Property changed method for line breakmode property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnLineBreakModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.InvalidateMeasure();
				sfButton.InvalidateDrawable();
			}

		}

		/// <summary>
		/// Property changed method for stroke thickness property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.UpdateStrokeThickness();
			}
		}

		/// <summary>
		/// Property changed method for background color property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.UpdateBackground();
				sfButton.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Property changed method for text color property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Property changed method for dash array property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Property changed method for content property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.AddButtonContent(oldValue, newValue);
				sfButton.OnImageSourcePropertyChanged();
				sfButton.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Property changed method for checkable property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value </param>
		/// <param name="newValue">The new value </param>
		static void OnCheckPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.CheckPropertyChanged();
			}
		}

		/// <summary>
		/// Property changed method for background image aspect property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event</param>
		/// <param name="oldValue">The old value of background image property.></param>
		/// <param name="newValue">The new value of background image property. </param>
		private static void OnBackgroundImageAspectPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfButton sfButton)
			{
				sfButton.SetAspectForBackgroundImage((Aspect)newValue);
			}
		}
		#endregion
	}
}
