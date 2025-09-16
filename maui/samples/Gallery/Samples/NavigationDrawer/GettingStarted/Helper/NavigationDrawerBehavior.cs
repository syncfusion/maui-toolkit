using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.NavigationDrawer;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	/// <summary>
	/// Base generic class for user-defined behaviors that can respond to conditions and events.
	/// </summary>
	public partial class NavigationDrawerBehavior : Behavior<SampleView>
	{
		Microsoft.Maui.Controls.CollectionView? _collectionView;
		MailInfoViewModel? _viewModel;
		Microsoft.Maui.Controls.Picker? _positionType;
		SfEffectsView? _inboxeffectsView;
		SfEffectsView? _starredeffectsView;
		SfEffectsView? _senteffectsView;
		SfEffectsView? _draftseffectsView;
		SfEffectsView? _allmailseffectsView;
		SfEffectsView? _trasheffectsView;
		SfEffectsView? _spameffectsView;
		SfEffectsView? _primarynavigationEffectsView;
		SfNavigationDrawer? _navigationdrawer;
		Label? _title;

		/// <summary>
		/// You can override this method to subscribe to AssociatedObject events and initialize properties.
		/// </summary>
		/// <param name="bindable">SampleView type parameter named as bindable.</param>
		protected override void OnAttachedTo(SampleView bindable)
		{
			_viewModel = new MailInfoViewModel();
			bindable.BindingContext = _viewModel;
			_collectionView = bindable.FindByName<Microsoft.Maui.Controls.CollectionView>("collectionView");
			_collectionView.ItemsSource = _viewModel.MailInfos;
			_navigationdrawer = bindable.FindByName<SfNavigationDrawer>("navigationDrawer");
			_navigationdrawer.BindingContext = _viewModel;
			_positionType = bindable.FindByName<Microsoft.Maui.Controls.Picker>("positioncomboBox");
			_positionType.SelectedIndexChanged += OnPositionSelectionChanged;

			_inboxeffectsView = bindable.FindByName<SfEffectsView>("inboxEffects");
			TapGestureRecognizer inboxtapGestureRecognizer = new TapGestureRecognizer();
			inboxtapGestureRecognizer.Tapped += InboxTapGestureRecognizer_Tapped;
			_inboxeffectsView.GestureRecognizers.Add(inboxtapGestureRecognizer);

			_starredeffectsView = bindable.FindByName<SfEffectsView>("starredEffects");
			TapGestureRecognizer starredtapGestureRecognizer = new TapGestureRecognizer();
			starredtapGestureRecognizer.Tapped += StarredTapGestureRecognizer_Tapped;
			_starredeffectsView.GestureRecognizers.Add(starredtapGestureRecognizer);

			_senteffectsView = bindable.FindByName<SfEffectsView>("sentEffects");
			TapGestureRecognizer senttapGestureRecognizer = new TapGestureRecognizer();
			senttapGestureRecognizer.Tapped += SentTapGestureRecognizer_Tapped;
			_senteffectsView.GestureRecognizers.Add(senttapGestureRecognizer);

			_draftseffectsView = bindable.FindByName<SfEffectsView>("draftEffects");
			TapGestureRecognizer drafttapGestureRecognizer = new TapGestureRecognizer();
			drafttapGestureRecognizer.Tapped += DraftTapGestureRecognizer_Tapped;
			_draftseffectsView.GestureRecognizers.Add(drafttapGestureRecognizer);

			_allmailseffectsView = bindable.FindByName<SfEffectsView>("allMailEffects");
			TapGestureRecognizer allmailtapGestureRecognizer = new TapGestureRecognizer();
			allmailtapGestureRecognizer.Tapped += AllMailsTapGestureRecognizer_Tapped;
			_allmailseffectsView.GestureRecognizers.Add(allmailtapGestureRecognizer);

			_trasheffectsView = bindable.FindByName<SfEffectsView>("trashEffects");
			TapGestureRecognizer trashtapGestureRecognizer = new TapGestureRecognizer();
			trashtapGestureRecognizer.Tapped += TrashTapGestureRecognizer_Tapped;
			_trasheffectsView.GestureRecognizers.Add(trashtapGestureRecognizer);

			_spameffectsView = bindable.FindByName<SfEffectsView>("spamEffects");
			TapGestureRecognizer spamtapGestureRecognizer = new TapGestureRecognizer();
			spamtapGestureRecognizer.Tapped += SpamTapGestureRecognizer_Tapped;
			_spameffectsView.GestureRecognizers.Add(spamtapGestureRecognizer);

			_primarynavigationEffectsView = bindable.FindByName<SfEffectsView>("primaryNavigation");
			TapGestureRecognizer navigationtapGestureRecognizer = new TapGestureRecognizer();
			navigationtapGestureRecognizer.Tapped += NavigationTapGestureRecognizer_Tapped;
			_primarynavigationEffectsView.GestureRecognizers.Add(navigationtapGestureRecognizer);

			_title = bindable.FindByName<Label>("titleName");

			base.OnAttachedTo(bindable);
		}

		private void NavigationTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			_navigationdrawer?.ToggleDrawer();
		}

		private void InboxTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			ResetSelection();
			if (_navigationdrawer != null && _title != null && _viewModel != null && _collectionView != null && _inboxeffectsView != null)
			{
				_inboxeffectsView.IsSelected = true;
				_title.Text = "Inbox";
				_viewModel.AddItemsRefresh(1);
				_collectionView.ItemsSource = _viewModel.MailInfos;
				_navigationdrawer.ToggleDrawer();
			}
		}

		private void StarredTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			ResetSelection();
			if (_navigationdrawer != null && _title != null && _viewModel != null && _collectionView != null && _starredeffectsView != null)
			{
				_starredeffectsView.IsSelected = true;
				_title.Text = "Starred";
				_viewModel.AddItemsRefresh(2);
				_collectionView.ItemsSource = _viewModel.MailInfos;
				_navigationdrawer.ToggleDrawer();
			}
		}

		private void SentTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			ResetSelection();
			if (_navigationdrawer != null && _title != null && _viewModel != null && _collectionView != null && _senteffectsView != null)
			{
				_senteffectsView.IsSelected = true;
				_title.Text = "Sent";
				_viewModel.AddItemsRefresh(3);
				_collectionView.ItemsSource = _viewModel.MailInfos;
				_navigationdrawer.ToggleDrawer();
			}
		}

		private void DraftTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			ResetSelection();
			if (_navigationdrawer != null && _title != null && _viewModel != null && _collectionView != null && _draftseffectsView != null)
			{
				_draftseffectsView.IsSelected = true;
				_title.Text = "Draft";
				_viewModel.AddItemsRefresh(4);
				_collectionView.ItemsSource = _viewModel.MailInfos;
				_navigationdrawer.ToggleDrawer();
			}
		}

		private void AllMailsTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			ResetSelection();
			if (_navigationdrawer != null && _title != null && _viewModel != null && _collectionView != null && _allmailseffectsView != null)
			{
				_allmailseffectsView.IsSelected = true;
				_title.Text = "All Mails";
				_viewModel.AddItemsRefresh(1);
				_collectionView.ItemsSource = _viewModel.MailInfos;
				_navigationdrawer.ToggleDrawer();
			}
		}

		private void TrashTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			ResetSelection();
			if (_navigationdrawer != null && _title != null && _viewModel != null && _collectionView != null && _trasheffectsView != null)
			{
				_trasheffectsView.IsSelected = true;
				_title.Text = "Trash";
				_viewModel.AddItemsRefresh(2);
				_collectionView.ItemsSource = _viewModel.MailInfos;
				_navigationdrawer.ToggleDrawer();
			}
		}

		private void SpamTapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			ResetSelection();
			if (_navigationdrawer != null && _title != null && _viewModel != null && _collectionView != null && _spameffectsView != null)
			{
				_spameffectsView.IsSelected = true;
				_title.Text = "Spam";
				_viewModel.AddItemsRefresh(1);
				_collectionView.ItemsSource = _viewModel.MailInfos;
				_navigationdrawer.ToggleDrawer();
			}
		}

		private void ResetSelection()
		{
			if (_inboxeffectsView != null && _starredeffectsView != null && _senteffectsView != null && _draftseffectsView != null && _allmailseffectsView != null && _trasheffectsView != null && _spameffectsView != null)
			{
				_inboxeffectsView.IsSelected = false;
				_starredeffectsView.IsSelected = false;
				_senteffectsView.IsSelected = false;
				_draftseffectsView.IsSelected = false;
				_allmailseffectsView.IsSelected = false;
				_trasheffectsView.IsSelected = false;
				_spameffectsView.IsSelected = false;
			}
		}

		/// <summary>
		///  Fires whenever Position ComboBox's selection changed.
		/// </summary>
		/// <param name="sender">OnPositionSelectionChanged sender.</param>
		/// <param name="e">SelectionChangedEventArgs e.</param>
		private void OnPositionSelectionChanged(object? sender, EventArgs e)
		{
			if (_positionType != null && _navigationdrawer != null)
			{
				_navigationdrawer.DrawerSettings.Position = (_positionType.SelectedItem?.ToString()) switch
				{
					"Right" => Position.Right,
					"Top" => Position.Top,
					"Bottom" => Position.Bottom,
					_ => Position.Left,
				};
			}
		}


		/// <summary>
		/// You can override this method while View was detached from window
		/// </summary>
		/// <param name="bindable">SampleView type parameter named as bindable</param>
		protected override void OnDetachingFrom(SampleView bindable)
		{

			_collectionView = null;
			_viewModel = null;
			base.OnDetachingFrom(bindable);
		}

		/// <summary>
		/// Helper method to get the key value for the GroupHeader name based on Data.
		/// </summary>
		/// <param name="groupName">Date of an item.</param>
		/// <returns>Returns specific group name.</returns>
		private static GroupName GetKey(DateTime groupName)
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
			else if (NavigationDrawerBehavior.IsLastWeek(groupName))
			{
				return GroupName.LastWeek;
			}
			else if (NavigationDrawerBehavior.IsThisWeek(groupName))
			{
				return GroupName.ThisWeek;
			}
			else if (NavigationDrawerBehavior.IsThisMonth(groupName))
			{
				return GroupName.ThisMonth;
			}
			else if (NavigationDrawerBehavior.IsLastMonth(groupName))
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
