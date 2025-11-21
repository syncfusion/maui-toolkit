
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// ViewModel class of CollectionViewPullToRefresh.
	/// </summary>
	public partial class CollectionViewInboxInfoViewModel : INotifyPropertyChanged
	{
		#region Fields

		ObservableCollection<InboxInfo>? _inboxInfos;
		readonly Random _random;

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
		public CollectionViewInboxInfoViewModel()
		{
			_random = new Random();
			GenerateSource();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the InboxInfo type of ObservableCollection and notifies user when collection value gets changed.
		/// </summary>
		public ObservableCollection<InboxInfo>? InboxInfos
		{
			get { return _inboxInfos; }
			set { _inboxInfos = value; OnPropertyChanged("InboxInfos"); }
		}

		#endregion

		#region Generate Source

		/// <summary>
		/// Initiates Commands, Repository and Collections. Also generates items for the collections.
		/// </summary>
		private void GenerateSource()
		{
			CollectionViewInboxInfoRepository inboxinfo = new CollectionViewInboxInfoRepository();
			_inboxInfos = inboxinfo.GetInboxInfo();
		}

		/// <summary>
		/// This method helps to add messages while refreshing.
		/// </summary>
		/// <param name="count">Represent number of messages to be added.</param>
		public void AddItemsRefresh(int count)
		{
			CollectionViewInboxInfoRepository inboxinfo = new CollectionViewInboxInfoRepository();

			foreach (var i in inboxinfo.AddRefreshItems(count))
			{
				_inboxInfos?.Insert(0, i);
			}
		}

		#endregion
	}
}

