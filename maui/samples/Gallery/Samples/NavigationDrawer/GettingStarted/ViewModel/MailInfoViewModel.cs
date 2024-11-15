
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	/// <summary>
	/// ViewModel class of NavigationDrawer.
	/// </summary>
	public class MailInfoViewModel : INotifyPropertyChanged
	{
		#region Fields

		ObservableCollection<MailInfo>? mailInfos;
		ObservableCollection<MailInfo>? archivedMessages;
		Command? undoCommand;
		Command? deleteCommand;
		bool? isDeleted;
		MailInfo? listViewItem;
		Command? archiveCommand;
		string? popUpText;
		int listViewItemIndex;
		Random random;
		MailInfoRepository? listViewRepository;

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
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initiates new intance of ListViewInboxInfoViewModel class.
		/// </summary>
		public MailInfoViewModel()
		{
			this.random = new Random();
			GenerateSource();
			this.listViewRepository = new MailInfoRepository();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the MailInfo type of ObservableCollection and notifies user when collection value gets changed.
		/// </summary>
		public ObservableCollection<MailInfo>? MailInfos
		{
			get { return mailInfos; }
			set { mailInfos = value; OnPropertyChanged("MailInfos"); }
		}

		/// <summary>
		/// Gets or sets the MailInfo type of ObservableCollection and stores archived messages on right swipe.
		/// </summary>
		public ObservableCollection<MailInfo>? ArchivedMessages
		{
			get { return archivedMessages; }
			set { archivedMessages = value; OnPropertyChanged("ArchivedMessages"); }
		}

		/// <summary>
		/// Get or sets the command used to delete the messages.
		/// </summary>
		public Command? DeleteCommand
		{
			get { return deleteCommand; }
			protected set { deleteCommand = value; }
		}

		/// <summary>
		/// Get or sets the command used to undo the archive or delete actions.
		/// </summary>
		public Command? UndoCommand
		{
			get { return undoCommand; }
			protected set { undoCommand = value; }
		}

		/// <summary>
		/// Gets or sets the command used to archive the messages.
		/// </summary>
		public Command? ArchiveCommand
		{
			get { return archiveCommand; }
			protected set { archiveCommand = value; }
		}

		public bool? IsDeleted
		{
			get { return isDeleted; }
			set { isDeleted = value; OnPropertyChanged("IsDeleted"); }
		}

		public string? PopUpText
		{
			get { return popUpText; }
			set { popUpText = value; OnPropertyChanged("PopUpText"); }
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

				if (this.listViewRepository != null)
				{
					var record = new MailInfo()
					{
						ProfileName = listViewRepository.ProfileList[i],
						Name = listViewRepository.NameList[i],
						Subject = listViewRepository.Subject[i],
						Date = DateTime.Now.AddMinutes((i * -3)),
						Description = listViewRepository.Descriptions[i],
						Image = listViewRepository.Images[k],
						IsAttached = listViewRepository.Attachments[i],
						IsImportant = listViewRepository.Importants[i],
						IsOpened = listViewRepository.Opens[i],
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
			archivedMessages = new ObservableCollection<MailInfo>();
			mailInfos = mailinfo.GetMailInfo();
			deleteCommand = new Command(OnDelete);
			undoCommand = new Command(OnUndo);
			archiveCommand = new Command(OnArchive);
		}

		/// <summary>
		/// This method helps to delete an message on left swipe.
		/// </summary>
		/// <param name="item">Represents an swiping message.</param>
		private async void OnDelete(object item)
		{
			PopUpText = "Deleted";
			listViewItem = (MailInfo)item;
			if (this.mailInfos != null)
			{
				listViewItemIndex = mailInfos.IndexOf(listViewItem);
				mailInfos.Remove(listViewItem);
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
			listViewItem = (MailInfo)item;
			if (this.mailInfos != null && this.archivedMessages != null)
			{
				listViewItemIndex = mailInfos.IndexOf(listViewItem);
				mailInfos.Remove(listViewItem);
				archivedMessages.Add(listViewItem);
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

			if (listViewItem != null && this.mailInfos != null && this.archivedMessages != null)
			{
				mailInfos.Insert(listViewItemIndex, listViewItem);

				var archivedItem = archivedMessages.Where(x => x.Name != null && x.Name.Equals(listViewItem.Name, StringComparison.Ordinal));

				if (archivedItem != null)
				{
					foreach (var item in archivedItem)
					{
						archivedMessages.Remove(listViewItem);
						break;
					}
				}
			}

			listViewItemIndex = 0;
			listViewItem = null;
		}

		/// <summary>
		/// This method helps to add messages while refreshing.
		/// </summary>
		/// <param name="count">Represent number of messages to be added.</param>
		public void AddItemsRefresh(int count)
		{
			MailInfoRepository inboxinfo = new MailInfoRepository();

			this.mailInfos = inboxinfo.AddRefreshItems(count);
		}

		#endregion
	}

}