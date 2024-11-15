using Microsoft.Maui.Platform;
using Syncfusion.Maui.Toolkit.PullToRefresh;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// Base generic class for user-defined behaviors that can respond to conditions and events.
	/// </summary>
	public class ListViewPullToRefreshBehavior : Behavior<SampleView>
	{
		Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh? pullToRefresh;
		ListView? ListView;
		ListViewInboxInfoViewModel? ViewModel;
		Picker? transitionType;

		/// <summary>
		/// You can override this method to subscribe to AssociatedObject events and initialize properties.
		/// </summary>
		/// <param name="bindable">SampleView type parameter named as bindable.</param>
		protected override void OnAttachedTo(SampleView bindable)
		{
			this.ViewModel = new ListViewInboxInfoViewModel();
			bindable.BindingContext = ViewModel;
			this.pullToRefresh = bindable.FindByName<Syncfusion.Maui.Toolkit.PullToRefresh.SfPullToRefresh>("pullToRefresh");
			this.ListView = bindable.FindByName<ListView>("listView");
			this.transitionType = bindable.FindByName<Picker>("comboBox");
			this.transitionType.SelectedIndexChanged += this.OnSelectionChanged;
			this.pullToRefresh.Refreshing += this.PullToRefresh_Refreshing;
			this.pullToRefresh.Refreshed += PullToRefresh_Refreshed;
			this.ListView.Loaded += ListView_Loaded;
			base.OnAttachedTo(bindable);
		}

		/// <summary>
		/// Fired when listView is loaded.
		/// </summary>
		/// <param name="sender">ListView_Loaded event sender</param>
		/// <param name="e">ListView_Loaded event args</param>
		private void ListView_Loaded(object? sender, EventArgs e)
		{
#if WINDOWS
        if (this.ListView != null && this.ListView.Handler != null)
        {
            var platformView = this.ListView.Handler.PlatformView as Microsoft.UI.Xaml.Controls.ListView;
            if (platformView != null)
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
			if (this.ListView != null && this.ListView.Handler != null)
			{
				var platformView = this.ListView.Handler.PlatformView as Android.Views.View;
				platformView?.InvalidateMeasure(this.ListView as IView);
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
			if (this.pullToRefresh != null && this.ViewModel != null)
			{
				this.pullToRefresh.IsRefreshing = true;
				await Task.Delay(2500);
				this.ViewModel.AddItemsRefresh(3);
				this.pullToRefresh.IsRefreshing = false;
			}
		}

		/// <summary>
		/// Fired when selected index is changed
		/// </summary>
		/// <param name="sender">OnSelectionChanged sender</param>
		/// <param name="e">EventArgs args e</param>
		private void OnSelectionChanged(object? sender, EventArgs e)
		{
			if (this.pullToRefresh != null && this.transitionType != null)
			{
				this.pullToRefresh.TransitionMode = this.transitionType.SelectedIndex == 0 ? PullToRefreshTransitionType.SlideOnTop : PullToRefreshTransitionType.Push;
			}
		}

		/// <summary>
		/// You can override this method while View was detached from window
		/// </summary>
		/// <param name="bindable">SampleView type parameter named as bindable</param>
		protected override void OnDetachingFrom(SampleView bindable)
		{
			if (this.pullToRefresh != null && this.transitionType != null && this.ListView != null)
			{
				this.pullToRefresh.Refreshing -= PullToRefresh_Refreshing;
				this.pullToRefresh.Refreshed -= PullToRefresh_Refreshed;
				this.transitionType.SelectedIndexChanged -= this.OnSelectionChanged;
				this.ListView.Loaded -= ListView_Loaded;
				this.pullToRefresh = null;
				this.ListView = null;
				this.ViewModel = null;
				this.transitionType = null;
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
			else if (IsLastWeek(groupName))
			{
				return GroupName.LastWeek;
			}
			else if (IsThisWeek(groupName))
			{
				return GroupName.ThisWeek;
			}
			else if (IsThisMonth(groupName))
			{
				return GroupName.ThisMonth;
			}
			else if (IsLastMonth(groupName))
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
		private bool IsThisWeek(DateTime groupName)
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
		private bool IsLastWeek(DateTime groupName)
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
		private bool IsThisMonth(DateTime groupName)
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
		private bool IsLastMonth(DateTime groupName)
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