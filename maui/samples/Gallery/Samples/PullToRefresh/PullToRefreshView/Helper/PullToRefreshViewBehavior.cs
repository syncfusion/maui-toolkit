using Syncfusion.Maui.Toolkit.PullToRefresh;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// Base generic class for user-defined behaviors that can respond to conditions and events.
	/// </summary>
	public class PullToRefreshViewBehavior : Behavior<SampleView>
	{
		CollectionView? listView;
		Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh? pullToRefresh;
		Picker? transitionType;
		PullToRefreshViewModel? viewModel;

		/// <summary>
		/// Used to Update weather data value
		/// </summary>
		internal void UpdateData()
		{
			var weatherType = this.viewModel!.Data!.WeatherType;
			this.viewModel.Data.Temperature = this.viewModel.UpdateTemperature(weatherType!);
		}

		/// <summary>
		/// You can override this method while View was detached from window
		/// </summary>
		/// <param name="bindable">Sample View typed parameter named as bindable</param>
		protected override void OnAttachedTo(SampleView bindable)
		{
			this.viewModel = new PullToRefreshViewModel();
			bindable.BindingContext = this.viewModel;
			this.pullToRefresh = bindable.FindByName<Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh>("pullToRefresh");
			this.listView = bindable.FindByName<CollectionView>("listView");
			this.transitionType = bindable.FindByName<Picker>("comboBox");
			this.pullToRefresh.PullingThreshold = 100;
			this.listView.BindingContext = this.viewModel;
			this.listView.ItemsSource = this.viewModel.SelectedData;
			this.listView.SelectedItem = (this.listView.BindingContext as PullToRefreshViewModel)?.SelectedData![0];
			this.listView.SelectionChanged += ListView_SelectionChanged;
			this.pullToRefresh.Refreshing += this.PullToRefresh_Refreshing;
			this.transitionType.SelectedIndexChanged += this.OnSelectionChanged;
			base.OnAttachedTo(bindable);
		}

		private void ListView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection.Count > 0)
			{
				var selectedWeatherData = e.CurrentSelection[0] as WeatherData;
				if (selectedWeatherData != null && this.viewModel != null)
				{
					this.viewModel.Data = selectedWeatherData;
				}
			}
		}

		/// <summary>
		/// You can override this method while View was detached from window
		/// </summary>
		/// <param name="bindAble">SampleView typed parameter named as bindAble</param>
		protected override void OnDetachingFrom(SampleView bindAble)
		{
			if (this.pullToRefresh != null)
			{
				this.pullToRefresh.Refreshing -= this.PullToRefresh_Refreshing;
			}

			if (this.transitionType != null)
			{
				this.transitionType.SelectedIndexChanged -= this.OnSelectionChanged;
			}

			if (this.listView != null)
			{
				this.listView.SelectionChanged -= ListView_SelectionChanged;
			}

			this.pullToRefresh = null;
			this.viewModel = null;
			this.listView = null;
			this.transitionType = null;
			base.OnDetachingFrom(bindAble);
		}

		/// <summary>
		/// Fired when selected index gets changed
		/// </summary>
		/// <param name="sender">OnSelectionChanged event sender</param>
		/// <param name="e">OnSelectionChanged event args e</param>
		private void OnSelectionChanged(object? sender, EventArgs e)
		{
			if (this.pullToRefresh != null && this.transitionType != null)
			{
				this.pullToRefresh.TransitionMode = this.transitionType.SelectedIndex == 0 ? PullToRefreshTransitionType.Push : PullToRefreshTransitionType.SlideOnTop;
			}
		}

		/// <summary>
		/// Triggers while pulltorefresh view was refreshing
		/// </summary>
		/// <param name="sender">PullToRefresh_Refreshing sender</param>
		/// <param name="args">PullToRefresh_Refreshing event args e</param>
		private void PullToRefresh_Refreshing(object? sender, EventArgs args)
		{
			if (this.pullToRefresh != null)
			{
				this.pullToRefresh.IsRefreshing = true;
				Dispatcher.StartTimer(
				new TimeSpan(0, 0, 0, 1, 3000), () =>
				{
					this.UpdateData();
					this.pullToRefresh.IsRefreshing = false;
					return false;
				});
			}
		}
	}
}