using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.PullToRefresh;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// Base generic class for user-defined behaviors that can respond to conditions and events.
	/// </summary>
	public partial class CollectionViewPullToRefreshBehavior : Behavior<SampleView>
	{
		Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh? _pullToRefresh;
		CollectionView? _collectionView;
		CollectionViewInboxInfoViewModel? _viewModel;
		Microsoft.Maui.Controls.Picker? _transitionType;

		/// <summary>
		/// You can override this method to subscribe to AssociatedObject events and initialize properties.
		/// </summary>
		/// <param name="bindable">SampleView type parameter named as bindable.</param>
		protected override void OnAttachedTo(SampleView bindable)
		{
			_viewModel = new CollectionViewInboxInfoViewModel();
			bindable.BindingContext = _viewModel;
			_pullToRefresh = bindable.FindByName<Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh>("pullToRefresh");
			_collectionView = bindable.FindByName<CollectionView>("listView");
			_transitionType = bindable.FindByName<Microsoft.Maui.Controls.Picker>("comboBox");
			_transitionType.SelectedIndexChanged += OnSelectionChanged;
			_pullToRefresh.Refreshing += PullToRefresh_Refreshing;
			_pullToRefresh.Refreshed += PullToRefresh_Refreshed;
			_collectionView.Loaded += CollectionView_Loaded;
			base.OnAttachedTo(bindable);
		}

		/// <summary>
		/// Fired when collectionview is loaded.
		/// </summary>
		/// <param name="sender">CollectionView_Loaded event sender</param>
		/// <param name="e">CollectionView_Loaded event args</param>
		private void CollectionView_Loaded(object? sender, EventArgs e)
		{
#if WINDOWS
			if (_collectionView != null && _collectionView.Handler != null)
			{
				if (_collectionView.Handler.PlatformView is Microsoft.UI.Xaml.Controls.ListViewBase platformView)
				{
					platformView.ItemContainerTransitions = null;
				}
			}
#endif
		}

		/// <summary>
		/// Fired when pulltorefresh view is refreshed.
		/// </summary>
		/// <param name="sender">PullToRefresh_Refreshed event sender</param>
		/// <param name="e">PullToRefresh_Refreshed event args</param>
		private void PullToRefresh_Refreshed(object? sender, EventArgs e)
		{
#if ANDROID
			if (_collectionView != null && _collectionView.Handler != null)
			{
				var platformView = _collectionView.Handler.PlatformView as Android.Views.View;
				platformView?.InvalidateMeasure(_collectionView as IView);
			}
#endif
		}

		/// <summary>
		/// Fired when pullToRefresh View is refreshing
		/// </summary>
		/// <param name="sender">PullToRefresh_Refreshing event sender</param>
		/// <param name="e">PullToRefresh_Refreshing event args</param>
		private async void PullToRefresh_Refreshing(object? sender, EventArgs e)
		{
			if (_pullToRefresh != null && _viewModel != null)
			{
				_pullToRefresh.IsRefreshing = true;
				await Task.Delay(2500);
				_viewModel.AddItemsRefresh(3);
				_pullToRefresh.IsRefreshing = false;
			}
		}

		/// <summary>
		/// Fired when selected index is changed
		/// </summary>
		/// <param name="sender">OnSelectionChanged sender</param>
		/// <param name="e">EventArgs args e</param>
		private void OnSelectionChanged(object? sender, EventArgs e)
		{
			if (_pullToRefresh != null && _transitionType != null)
			{
				_pullToRefresh.TransitionMode = _transitionType.SelectedIndex == 0 ? PullToRefreshTransitionType.SlideOnTop : PullToRefreshTransitionType.Push;
			}
		}

		/// <summary>
		/// You can override this method while View was detached from window
		/// </summary>
		/// <param name="bindable">SampleView type parameter named as bindable</param>
		protected override void OnDetachingFrom(SampleView bindable)
		{
			if (_pullToRefresh != null && _transitionType != null && _collectionView != null)
			{
				_pullToRefresh.Refreshing -= PullToRefresh_Refreshing;
				_pullToRefresh.Refreshed -= PullToRefresh_Refreshed;
				_transitionType.SelectedIndexChanged -= OnSelectionChanged;
				_collectionView.Loaded -= CollectionView_Loaded;
				_pullToRefresh = null;
				_collectionView = null;
				_viewModel = null;
				_transitionType = null;
				base.OnDetachingFrom(bindable);
			}
		}

		/// <summary>
		/// Helper method to get the key value for the GroupHeader name based on Data.
		/// </summary>
		/// <param name="groupName">Date of an item.</param>
		/// <returns>Returns specific group name.</returns>
		private GroupName GetKey(DateTime groupName)
		{
			int compare = groupName.Date.CompareTo(DateTime.Now.Date);

			if (compare == 0)
			{
				return GroupName.Today;
			}
			else if (groupName.Date.CompareTo(DateTime.Now.AddDays(-1).Date) == 0)
			{
				return GroupName.Yesterday;
			}
			else if (CollectionViewPullToRefreshBehavior.IsLastWeek(groupName))
			{
				return GroupName.LastWeek;
			}
			else if (CollectionViewPullToRefreshBehavior.IsThisWeek(groupName))
			{
				return GroupName.ThisWeek;
			}
			else if (CollectionViewPullToRefreshBehavior.IsThisMonth(groupName))
			{
				return GroupName.ThisMonth;
			}
			else if (CollectionViewPullToRefreshBehavior.IsLastMonth(groupName))
			{
				return GroupName.LastMonth;
			}
			else
			{
				return GroupName.Older;
			}
		}

		/// <summary>
		/// Helper method to check whether particular date is in this week or not.
		/// </summary>
		/// <param name="groupName">Date of an item.</param>
		/// <returns>Returns true if the mentioned date is in this week.</returns>
		private static bool IsThisWeek(DateTime groupName)
		{
			var groupWeekSunDay = groupName.AddDays(-(int)groupName.DayOfWeek).Day;
			var currentSunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).Day;

			var groupMonth = groupName.Month;
			var currentMonth = DateTime.Today.Month;

			var isCurrentYear = groupName.Year == DateTime.Today.Year;

			return currentSunday == groupWeekSunDay && (groupMonth == currentMonth || groupMonth == currentMonth - 1) && isCurrentYear;
		}

		/// <summary>
		/// Helper method to check whether particular date is in last week or not.
		/// </summary>
		/// <param name="groupName">Date of an item.</param>
		/// <returns>Returns true if the mentioned date is in last week.</returns>
		private static bool IsLastWeek(DateTime groupName)
		{
			var groupWeekSunDay = groupName.AddDays(-(int)groupName.DayOfWeek).Day;
			var lastSunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).Day - 7;

			var groupMonth = groupName.Month;
			var currentMonth = DateTime.Today.Month;

			var isCurrentYear = groupName.Year == DateTime.Today.Year;

			return lastSunday == groupWeekSunDay && (groupMonth == currentMonth || groupMonth == currentMonth - 1) && isCurrentYear;
		}

		/// <summary>
		/// Helper method to check whether particular date is in this month or not.
		/// </summary>
		/// <param name="groupName">Date of an item.</param>
		/// <returns>Returns true if the mentioned date is in this month.</returns>
		private static bool IsThisMonth(DateTime groupName)
		{
			var groupMonth = groupName.Month;
			var currentMonth = DateTime.Today.Month;

			var isCurrentYear = groupName.Year == DateTime.Today.Year;

			return groupMonth == currentMonth && isCurrentYear;
		}

		/// <summary>
		/// Helper method to check whether particular date is in last month or not.
		/// </summary>
		/// <param name="groupName">Date of an item.</param>
		/// <returns>Returns true if the mentioned date is in last month.</returns>
		private static bool IsLastMonth(DateTime groupName)
		{
			var groupMonth = groupName.Month;
			var currentMonth = DateTime.Today.AddMonths(-1).Month;

			var isCurrentYear = groupName.Year == DateTime.Today.Year;

			return groupMonth == currentMonth && isCurrentYear;
		}
	}

	public enum GroupName
	{
		Today = 0,
		Yesterday,
		ThisWeek,
		LastWeek,
		ThisMonth,
		LastMonth,
		Older
	}
}