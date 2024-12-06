namespace Syncfusion.Maui.ControlsGallery
{
	/// <summary>
	/// 
	/// </summary>
	public partial class SampleView : ContentView
	{

		/// <summary>
		/// 
		/// </summary>
		public View? OptionView
		{
			get { return (View)GetValue(OptionViewProperty); }
			set { SetValue(OptionViewProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		// Using a DependencyProperty as the backing store for OptionView.  This enables animation, styling, binding, etc...
		public static readonly BindableProperty OptionViewProperty =
			BindableProperty.Create(nameof(OptionView), typeof(View), typeof(SampleView), null);

		/// <summary>
		/// 
		/// </summary>
		public SampleView()
		{

		}

		View? _busyIndicatorView;

		/// <summary>
		/// 
		/// </summary>
		public bool IsCardView { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public void SetBusyIndicator(View view)
		{
			_busyIndicatorView = view;
		}

		/// <summary>
		/// Hooked when sample view disappears
		/// </summary>
		public virtual void OnDisappearing()
		{
		}

		/// <summary>
		/// Hooked when sample view appears
		/// </summary>
		public virtual void OnAppearing()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		protected override Size ArrangeOverride(Rect bounds)
		{
			if (_busyIndicatorView != null && _busyIndicatorView.IsVisible)
			{
				HideBusyIndicator();
			}

			return base.ArrangeOverride(bounds);

		}

		private async void HideBusyIndicator()
		{
			await Task.Delay(100);
			if (_busyIndicatorView != null)
			{
				_busyIndicatorView.IsVisible = false;
			}
		}
	}
}
