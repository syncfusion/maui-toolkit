
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	/// <summary>
	/// ViewModel class of NavigationDrawer.
	/// </summary>
	public partial class MailInfoViewModel : INotifyPropertyChanged
	{
		#region Fields

		ObservableCollection<MailInfo>? _mailInfos;
		ObservableCollection<MailInfo>? _archivedMessages;
		ObservableCollection<Notification>? notifications;
		Command? _undoCommand;
		Command? _deleteCommand;
		bool? _isDeleted;
		MailInfo? _collectionViewItem;
		Command? _archiveCommand;
		string? _popUpText;
		int _collectionViewItemIndex;
		readonly Random _random;
		readonly MailInfoRepository? _collectionViewRepository;

		#endregion

		#region Interface Member

		/// <summary>
		/// Represents the method that will handle the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"></see> event raised when a property is changed on a component
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Triggers when Items Collections Changed.
		/// </summary>
		/// <param name="name">string type parameter represent propertyName as name</param>
		public void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initiates new intance of CollectionViewInboxInfoViewModel class.
		/// </summary>
		public MailInfoViewModel()
		{
			_random = new Random();
			GenerateSource();
			_collectionViewRepository = new MailInfoRepository();

			notifications = new ObservableCollection<Notification>
		{
			new Notification { Name = "Clara", Subject = "Goto Meeting", Description = "Join meeting to discuss about daily status, workflow, pending work and improve process", ImageSource = "peoplecircle9.png",TimeStamp = "12:10 PM" },
			new Notification { Name = "Gabriella", Subject = "FW: Status Update", Description = "Hi, Please find the today's status", ImageSource = "peoplecircle16.png",TimeStamp = "12:50 PM" },
			new Notification { Name = "Jackson", Subject = "Greetings! Congrats", Description = "Hi, Congrats you have won the raffle", ImageSource = "peoplecircle18.png" ,TimeStamp = "05:45 PM"},
			new Notification { Name = "Liam", Subject = "Report Monitor", Description = "Do not reply, Please find the attachment. Attachment have the full details of monitor report with timing", ImageSource = "peoplecircle23.png" ,TimeStamp = "04:00 PM"},
			new Notification { Name = "Lita", Subject = "News Letter", Description = "Hi, Please find the attached news letter", ImageSource = "peoplecircle25.png" , TimeStamp = "07:10 AM"},
			new Notification { Name = "Briley", Subject = "Conference about Latest", Description = "Hi, We are scheduled a meeting", ImageSource = "peoplecircle26.png" ,TimeStamp = "12:10 PM"},
			new Notification { Name = "Maci", Subject = "RE: Status Update", Description = "Thanks for the update", ImageSource = "person.png" ,TimeStamp = "06:40 PM"},
			new Notification { Name = "Peter", Subject = "RE: Status Update", Description = "Thanks for the update", ImageSource = "peoplecircle5.png" ,TimeStamp = "10:25 AM"},
			new Notification { Name = "Sophia", Subject = "Project Kickoff", Description = "Reminder: Project kickoff meeting scheduled for tomorrow at 10 AM", ImageSource = "emp_08.png" ,TimeStamp = "2:10 PM"},
			new Notification { Name = "Ethan", Subject = "Weekly Report", Description = "Please review the attached weekly performance report", ImageSource = "emp_07.png" ,TimeStamp = "02:00 PM"},
			new Notification { Name = "Olivia", Subject = "System Maintenance", Description = "Scheduled maintenance will occur tonight from 12 AM to 3 AM", ImageSource = "emp_09.png" ,TimeStamp = "11:00 AM"},
			new Notification { Name = "Noah", Subject = "Policy Update", Description = "New company policy has been released. Please check your email for details", ImageSource = "emp_06.png" ,TimeStamp = "11:18 AM"},
			new Notification { Name = "Ava", Subject = "Training Session", Description = "Join the mandatory training session on Friday at 2 PM", ImageSource = "emp_03.png"  ,TimeStamp = "12:10 PM"},
			new Notification { Name = "Isabella", Subject = "Reminder: Submit Timesheet", Description = "Please submit your timesheet before 6 PM today to avoid delays in processing.", ImageSource = "emp_04.png"  ,TimeStamp = "12:10 PM"}
		};
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the MailInfo type of ObservableCollection and notifies user when collection value gets changed.
		/// </summary>
		public ObservableCollection<MailInfo>? MailInfos
		{
			get { return _mailInfos; }
			set { _mailInfos = value; OnPropertyChanged("MailInfos"); }
		}

		/// <summary>
		/// Gets or sets the MailInfo type of ObservableCollection and stores archived messages on right swipe.
		/// </summary>
		public ObservableCollection<MailInfo>? ArchivedMessages
		{
			get { return _archivedMessages; }
			set { _archivedMessages = value; OnPropertyChanged("ArchivedMessages"); }
		}

		/// <summary>
		/// Gets or sets the MailInfo type of ObservableCollection and notifies user when collection value gets changed.
		/// </summary>
		public ObservableCollection<Notification>? Notifications
		{
			get { return notifications; }
			set { notifications = value; OnPropertyChanged("Notifications"); }
		}

		/// <summary>
		/// Get or sets the command used to delete the messages.
		/// </summary>
		public Command? DeleteCommand
		{
			get { return _deleteCommand; }
			protected set { _deleteCommand = value; }
		}

		/// <summary>
		/// Get or sets the command used to undo the archive or delete actions.
		/// </summary>
		public Command? UndoCommand
		{
			get { return _undoCommand; }
			protected set { _undoCommand = value; }
		}

		/// <summary>
		/// Gets or sets the command used to archive the messages.
		/// </summary>
		public Command? ArchiveCommand
		{
			get { return _archiveCommand; }
			protected set { _archiveCommand = value; }
		}

		public bool? IsDeleted
		{
			get { return _isDeleted; }
			set { _isDeleted = value; OnPropertyChanged("IsDeleted"); }
		}

		public string? PopUpText
		{
			get { return _popUpText; }
			set { _popUpText = value; OnPropertyChanged("PopUpText"); }
		}


		#endregion
		/// <summary>
		/// Used to assign the Collection values to Model Properties while refreshing.
		/// </summary>
		/// <param name="count">Number of items to be added.</param>
		/// <returns>Added mailInfos items</returns>
		internal ObservableCollection<MailInfo> GenerateSource(int count)
		{
			var empInfo = new ObservableCollection<MailInfo>();
			int k = 0;
			for (int i = 0; i < count; i++)
			{

				if (k > 5)
				{
					k = 0;
				}

				if (_collectionViewRepository != null)
				{
					var record = new MailInfo()
					{
						ProfileName = _collectionViewRepository._profileList[i],
						Name = _collectionViewRepository._nameList[i],
						Subject = _collectionViewRepository._subject[i],
						Date = DateTime.Now.AddMinutes((i * -3)),
						Description = _collectionViewRepository._descriptions[i],
						Image = _collectionViewRepository._images[k],
						IsAttached = _collectionViewRepository._attachments[i],
						IsImportant = _collectionViewRepository._importants[i],
						IsOpened = _collectionViewRepository._opens[i],
					};
					empInfo.Add(record);
					k++;
				}
			}

			return empInfo;
		}

		#region Generate Source

		/// <summary>
		/// Initiates Commands, Repository and Collections. Also generates items for the collections.
		/// </summary>
		private void GenerateSource()
		{
			IsDeleted = false;
			MailInfoRepository mailinfo = new MailInfoRepository();
			_archivedMessages = [];
			_mailInfos = mailinfo.GetMailInfo();
			_deleteCommand = new Command(OnDelete);
			_undoCommand = new Command(OnUndo);
			_archiveCommand = new Command(OnArchive);
		}

		/// <summary>
		/// This method helps to delete an message on left swipe.
		/// </summary>
		/// <param name="item">Represents an swiping message.</param>
		private async void OnDelete(object item)
		{
			PopUpText = "Deleted";
			_collectionViewItem = (MailInfo)item;
			if (_mailInfos != null)
			{
				_collectionViewItemIndex = _mailInfos.IndexOf(_collectionViewItem);
				_mailInfos.Remove(_collectionViewItem);
			}

			IsDeleted = true;

			// Added Delay in order to maintain the Delete message popUp at screen bottom.
			await Task.Delay(3000);
			IsDeleted = false;
		}

		/// <summary>
		/// This method helps to archive an message on right swipe.
		/// </summary>
		/// <param name="item">Represent an swiping message.</param>
		private async void OnArchive(object item)
		{
			PopUpText = "Archived";
			_collectionViewItem = (MailInfo)item;
			if (_mailInfos != null && _archivedMessages != null)
			{
				_collectionViewItemIndex = _mailInfos.IndexOf(_collectionViewItem);
				_mailInfos.Remove(_collectionViewItem);
				_archivedMessages.Add(_collectionViewItem);
			}

			IsDeleted = true;

			// Added Delay in order to maintain the Archive message popUp at screen bottom.
			await Task.Delay(3000);
			IsDeleted = false;
		}

		/// <summary>
		/// This method helps to undo the archive or delete action.
		/// </summary>
		private void OnUndo()
		{
			IsDeleted = false;

			if (_collectionViewItem != null && _mailInfos != null && _archivedMessages != null)
			{
				_mailInfos.Insert(_collectionViewItemIndex, _collectionViewItem);

				var archivedItem = _archivedMessages.Where(x => x.Name != null && x.Name.Equals(_collectionViewItem.Name, StringComparison.Ordinal));

				if (archivedItem != null)
				{
					foreach (var item in archivedItem)
					{
						_archivedMessages.Remove(_collectionViewItem);
						break;
					}
				}
			}

			_collectionViewItemIndex = 0;
			_collectionViewItem = null;
		}

		/// <summary>
		/// This method helps to add messages while refreshing.
		/// </summary>
		/// <param name="count">Represent number of messages to be added.</param>
		public void AddItemsRefresh(int count)
		{
			MailInfoRepository inboxinfo = new MailInfoRepository();

			_mailInfos = inboxinfo.AddRefreshItems(count);
		}

		#endregion
	}

}