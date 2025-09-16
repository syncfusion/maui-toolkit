using System.Runtime.CompilerServices;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents an individual item used in a segmented control.
	/// </summary>
	public partial class SfSegmentItem : Element
	{
		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="Background"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Background"/> dependency property.
		/// </value>
		public static readonly BindableProperty BackgroundProperty =
			BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SfSegmentItem), defaultValueCreator: bindable => null);

		/// <summary>
		/// Identifies the <see cref="Width"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Width"/> dependency property.
		/// </value>
		public static readonly BindableProperty WidthProperty =
			BindableProperty.Create(nameof(Width), typeof(double), typeof(SfSegmentItem), defaultValueCreator: bindable => double.NaN);

		/// <summary>
		/// Identifies the <see cref="ImageSource"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ImageSource"/> dependency property.
		/// </value>
		public static readonly BindableProperty ImageSourceProperty =
			BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(SfSegmentItem), defaultValueCreator: bindable => null);

		/// <summary>
		/// Identifies the <see cref="SelectedSegmentTextColor"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SelectedSegmentTextColor"/> dependency property.
		/// </value>
		public static readonly BindableProperty SelectedSegmentTextColorProperty =
			BindableProperty.Create(nameof(SelectedSegmentTextColor), typeof(Color), typeof(SfSegmentItem), defaultValueCreator: bindable => null);

		/// <summary>
		/// Identifies the <see cref="SelectedSegmentBackground"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SelectedSegmentBackground"/> dependency property.
		/// </value>
		public static readonly BindableProperty SelectedSegmentBackgroundProperty =
			BindableProperty.Create(nameof(SelectedSegmentBackground), typeof(Brush), typeof(SfSegmentItem), defaultValueCreator: bindable => null);

		/// <summary>
		/// Identifies the <see cref="TextStyle"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="TextStyle"/> dependency property.
		/// </value>
		public static readonly BindableProperty TextStyleProperty =
			BindableProperty.Create(nameof(TextStyle), typeof(SegmentTextStyle), typeof(SfSegmentItem), defaultValueCreator: bindable => null);

		/// <summary>
		/// Identifies the <see cref="ImageSize"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ImageSize"/> dependency property.
		/// </value>
		public static readonly BindableProperty ImageSizeProperty =
			BindableProperty.Create(nameof(ImageSize), typeof(double), typeof(SfSegmentItem), defaultValueCreator: bindable => 18d);

		/// <summary>
		/// Identifies the <see cref="IsEnabled"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IsEnabled"/> dependency property.
		/// </value>
		public static readonly BindableProperty IsEnabledProperty =
			BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(SfSegmentItem), defaultValueCreator: bindable => true);

		/// <summary>
		/// Identifies the <see cref="IsSelected"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IsSelected"/> dependency property.
		/// </value>
		public static readonly BindableProperty IsSelectedProperty =
			BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SfSegmentItem), false);

		/// <summary>
		/// Identifies the <see cref="Text"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Text"/> dependency property.
		/// </value>
		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(SfSegmentItem), defaultValueCreator: bindable => string.Empty);

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the background color of the segment item.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text="Jackson", Background = Colors.Red },
		///           new SfSegmentItem() { Text="Gabriella", Background = Colors.Blue},
		///           new SfSegmentItem() { Text="Liam", Background = Colors.Green},
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public Brush Background
		{
			get { return (Brush)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the width of the segment item.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text="Jackson", Width=100 },
		///           new SfSegmentItem() { Text="Gabriella", Width=100},
		///           new SfSegmentItem() { Text="Liam", Width=100},
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the image displayed in the segment item.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { ImageSource="jackson.png" },
		///           new SfSegmentItem() { ImageSource ="gabriella.png" },
		///           new SfSegmentItem() { ImageSource="liam.png" },
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public ImageSource ImageSource
		{
			get { return (ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text color of the segment item when it is selected.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text="Jackson", SelectedSegmentTextColor = Colors.LightBlue},
		///           new SfSegmentItem() { Text="Gabriella", SelectedSegmentTextColor = Colors.LightBlue},
		///           new SfSegmentItem() { Text="Liam", SelectedSegmentTextColor = Colors.LightBlue},
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public Color SelectedSegmentTextColor
		{
			get { return (Color)GetValue(SelectedSegmentTextColorProperty); }
			set { SetValue(SelectedSegmentTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background color of the segment item when it is selected.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text="Jackson", SelectedSegmentBackground = Colors.LightBlue},
		///           new SfSegmentItem() { Text="Gabriella", SelectedSegmentBackground = Colors.LightBlue},
		///           new SfSegmentItem() { Text="Liam", SelectedSegmentBackground = Colors.LightBlue},
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public Brush SelectedSegmentBackground
		{
			get { return (Brush)GetValue(SelectedSegmentBackgroundProperty); }
			set { SetValue(SelectedSegmentBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text style of the segment item.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text="Jackson", TextStyle = new SegmentTextStyle(){TextColor = Colors.Green}},
		///           new SfSegmentItem() { Text="Gabriella", TextStyle = new SegmentTextStyle(){TextColor = Colors.Red}},
		///           new SfSegmentItem() { Text="Liam", TextStyle = new SegmentTextStyle(){TextColor = Colors.Yellow}},
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public SegmentTextStyle TextStyle
		{
			get { return (SegmentTextStyle)GetValue(TextStyleProperty); }
			set { SetValue(TextStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the image size of the segment item.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() {  ImageSource="jackson.png", Text="Jackson", ImageSize = 40 },
		///           new SfSegmentItem() { ImageSource ="gabriella.png" ,Text="Gabriella", ImageSize = 30},
		///           new SfSegmentItem() { ImageSource="liam.png", Text="Liam", ImageSize = 40},
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public double ImageSize
		{
			get { return (double)GetValue(ImageSizeProperty); }
			set { SetValue(ImageSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the segment item is enabled.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text="Jackson", IsEnabled = false },
		///           new SfSegmentItem() { Text="Gabriella", IsEnabled = true },
		///           new SfSegmentItem() { Text="Liam", IsEnabled = true },
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public bool IsEnabled
		{
			get { return (bool)GetValue(IsEnabledProperty); }
			set { SetValue(IsEnabledProperty, value); }
		}

		/// <summary>
		/// Gets the value indicating whether the segment item is selected.
		/// </summary>
		public bool IsSelected
		{
			get { return (bool)this.GetValue(IsSelectedProperty); }
			internal set { this.SetValue(IsSelectedProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text of the segment item.
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Employees { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Employees = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text="Jackson" },
		///           new SfSegmentItem() { Text="Gabriella"},
		///           new SfSegmentItem() { Text="Liam"},
		///        };
		///    }
		/// }
		/// ]]></code>
		/// </example>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the corner radius for the segment item.
		/// </summary>
		/// <remarks>
		/// The corner radius determines the round rect of the corners of the segment item's border. By adjusting this property, control the appearance of rounded corners for the segment item.
		/// </remarks>
		internal CornerRadius CornerRadius { get; set; }

		#endregion

		#region Override Methods

		/// <summary>
		/// Invokes on the property changed.
		/// </summary>
		/// <exclude/>
		/// <param name="propertyName"></param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Invokes on the binding context of the view changed.
		/// </summary>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			TextStyle.BindingContext = BindingContext;
		}

		#endregion
	}
}