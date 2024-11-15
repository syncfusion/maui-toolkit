namespace Syncfusion.Maui.ControlsGallery.CustomView
{
	/// <summary>
	/// GettingStartedSampleView.
	/// </summary>
	public partial class GettingStartedSampleView : SampleView
	{
		/// <summary>
		/// GettingStartedSampleView's constructor 
		/// </summary>
		public GettingStartedSampleView()
		{
			InitializeComponent();
			this.HorizontalOptions = LayoutOptions.Fill;
			this.VerticalOptions = LayoutOptions.Fill;
			this.Margin = new Thickness(-6, -8, -6, -8);
			this.Padding = new Thickness(10, 20);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty GettingStartedContentProperty =
			BindableProperty.Create(nameof(GettingStartedContent), typeof(View), typeof(GettingStartedSampleView), null, propertyChanged: OnContentPropertyChanged);

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty FrameWidthProperty =
			BindableProperty.Create(nameof(FrameWidth), typeof(double), typeof(GettingStartedSampleView), null, propertyChanged: OnWidthPropertyChanged);

		/// <summary>
		/// 
		/// </summary>
		public double FrameWidth
		{
			get => (double)GetValue(FrameWidthProperty);
			set => SetValue(FrameWidthProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public View GettingStartedContent
		{
			get => (View)GetValue(GettingStartedContentProperty);
			set => SetValue(GettingStartedContentProperty, value);
		}

		private static void OnWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var bind = bindable as GettingStartedSampleView;
			bind?.WidthChanged((double)newValue);
		}

		private void WidthChanged(double width)
		{
			if (frame != null)
			{
				frame.WidthRequest = width > 0 ? width : 400;
			}
		}

		private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var bind = bindable as GettingStartedSampleView;
			bind?.ContentChanged((View)newValue);
		}

		private void ContentChanged(View newValue)
		{
			if (frame != null)
			{
				this.frame.Content = newValue;
			}
		}
	}
}