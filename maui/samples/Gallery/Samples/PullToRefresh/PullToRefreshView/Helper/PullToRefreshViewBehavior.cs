using Syncfusion.Maui.Toolkit.PullToRefresh;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// Base generic class for user-defined behaviors that can respond to conditions and events.
	/// </summary>
	public partial class PullToRefreshViewBehavior : Behavior<SampleView>
	{
		CollectionView? _listView;
		Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh? _pullToRefresh;
		Microsoft.Maui.Controls.Picker? _transitionType;
		PullToRefreshViewModel? _viewModel;

		/// <summary>
		/// Used to Update weather data value
		/// </summary>
		internal void UpdateData()
		{
			var weatherType = _viewModel!.Data!.WeatherType;
			_viewModel.Data.Temperature = _viewModel.UpdateTemperature(weatherType!);
		}

		/// <summary>
		/// You can override this method while View was detached from window
		/// </summary>
		/// <param name="bindable">Sample View typed parameter named as bindable</param>
		protected override void OnAttachedTo(SampleView bindable)
		{
			_viewModel = new PullToRefreshViewModel();
			bindable.BindingContext = _viewModel;
			_pullToRefresh = bindable.FindByName<Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh>("pullToRefresh");
			_listView = bindable.FindByName<CollectionView>("listView");
			_transitionType = bindable.FindByName<Microsoft.Maui.Controls.Picker>("comboBox");
			_pullToRefresh.PullingThreshold = 100;
			_listView.BindingContext = _viewModel;
			_listView.ItemsSource = _viewModel.SelectedData;
			_listView.SelectedItem = (_listView.BindingContext as PullToRefreshViewModel)?.SelectedData![0];
			_listView.SelectionChanged += CollectionView_SelectionChanged;
			_pullToRefresh.Refreshing += PullToRefresh_Refreshing;
			_transitionType.SelectedIndexChanged += OnSelectionChanged;
			base.OnAttachedTo(bindable);
		}

		private void CollectionView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection.Count > 0)
			{
				if (e.CurrentSelection[0] is WeatherData selectedWeatherData && _viewModel != null)
				{
					_viewModel.Data = selectedWeatherData;
				}
			}
		}

		/// <summary>
		/// You can override this method while View was detached from window
		/// </summary>
		/// <param name="bindAble">SampleView typed parameter named as bindAble</param>
		protected override void OnDetachingFrom(SampleView bindAble)
		{
			if (_pullToRefresh != null)
			{
				_pullToRefresh.Refreshing -= PullToRefresh_Refreshing;
			}

			if (_transitionType != null)
			{
				_transitionType.SelectedIndexChanged -= OnSelectionChanged;
			}

			if (_listView != null)
			{
				_listView.SelectionChanged -= CollectionView_SelectionChanged;
			}

			_pullToRefresh = null;
			_viewModel = null;
			_listView = null;
			_transitionType = null;
			base.OnDetachingFrom(bindAble);
		}

		/// <summary>
		/// Fired when selected index gets changed
		/// </summary>
		/// <param name="sender">OnSelectionChanged event sender</param>
		/// <param name="e">OnSelectionChanged event args e</param>
		private void OnSelectionChanged(object? sender, EventArgs e)
		{
			if (_pullToRefresh != null && _transitionType != null)
			{
				_pullToRefresh.TransitionMode = _transitionType.SelectedIndex == 0 ? PullToRefreshTransitionType.Push : PullToRefreshTransitionType.SlideOnTop;
			}
		}

		/// <summary>
		/// Triggers while pulltorefresh view was refreshing
		/// </summary>
		/// <param name="sender">PullToRefresh_Refreshing sender</param>
		/// <param name="args">PullToRefresh_Refreshing event args e</param>
		private void PullToRefresh_Refreshing(object? sender, EventArgs args)
		{
			if (_pullToRefresh != null)
			{
				_pullToRefresh.IsRefreshing = true;
				Dispatcher.StartTimer(
				new TimeSpan(0, 0, 0, 1, 3000), () =>
				{
					UpdateData();
					_pullToRefresh.IsRefreshing = false;
					return false;
				});
			}
		}
	}
}